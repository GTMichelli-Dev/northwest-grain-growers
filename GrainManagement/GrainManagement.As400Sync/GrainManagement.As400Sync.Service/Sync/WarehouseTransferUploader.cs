using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Odbc;

namespace GrainManagement.As400Sync;

/// <summary>
/// Reads closed weight sheets from SQL Server, builds U5SILOAD rows per the
/// spec (1 row per intake, 2 rows per transfer), and writes them to the
/// Agvantage AS400 via ODBC. Existing rows for the same SILDNO are deleted
/// first so re-uploads are safe.
///
/// Per-WS errors are accumulated and returned alongside progress so the
/// caller (As400SyncHubClient) can stream status / error events to the
/// admin UI without aborting the whole batch on one bad row.
/// </summary>
public sealed class WarehouseTransferUploader
{
    private readonly string _sqlConnStr;
    private readonly string _as400ConnStr;
    private readonly ILogger<WarehouseTransferUploader>? _log;

    private static readonly TimeZoneInfo PacificTz = ResolvePacific();

    public WarehouseTransferUploader(IConfiguration cfg, ILogger<WarehouseTransferUploader>? log = null)
    {
        _sqlConnStr = cfg.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Missing ConnectionStrings:SqlServer");

        // The configured AS400 connection uses CommitMode=2 (cursor
        // stability) which requires the target table to be under journal
        // control. U5SILOAD typically is not journaled, so writes were
        // silently being dropped by the driver. Override CommitMode to 0
        // (no commit control / per-statement auto-commit) for this
        // uploader only — reads from the same DSN are unaffected.
        var raw = cfg.GetConnectionString("As400Odbc")
            ?? throw new InvalidOperationException("Missing ConnectionStrings:As400Odbc");
        _as400ConnStr = WithCommitModeNone(raw);

        _log = log;
    }

    private static string WithCommitModeNone(string connStr)
    {
        var parts = connStr.Split(';', StringSplitOptions.RemoveEmptyEntries);
        var keep = parts
            .Where(p => !p.TrimStart().StartsWith("CommitMode=", StringComparison.OrdinalIgnoreCase))
            .ToArray();
        return string.Join(";", keep) + ";CommitMode=0;";
    }

    private static TimeZoneInfo ResolvePacific()
    {
        try { return TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles"); }
        catch
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"); }
            catch { return TimeZoneInfo.Local; }
        }
    }

    public sealed class UploadResult
    {
        public int Total { get; set; }
        public int Succeeded { get; set; }
        public List<RowError> Errors { get; } = new();
    }

    public sealed class RowError
    {
        public long WeightSheetId { get; set; }
        public long? As400Id { get; set; }
        public string Message { get; set; } = "";
    }

    public async Task<UploadResult> UploadAsync(
        IReadOnlyList<long> weightSheetIds,
        IProgress<(int current, int total, string? message)>? progress,
        CancellationToken ct)
    {
        var result = new UploadResult { Total = weightSheetIds.Count };

        await using var sql = new SqlConnection(_sqlConnStr);
        await sql.OpenAsync(ct);

        // The AS400 ODBC connection can land in a sticky bad state after a
        // SQL0205 / driver error — subsequent commands then fail with the
        // same message until the connection is reset. Keep the connection
        // here as a mutable local so we can dispose + reopen mid-loop.
        var as400 = new OdbcConnection(_as400ConnStr);
        await as400.OpenAsync(ct);
        try
        {
            for (int i = 0; i < weightSheetIds.Count; i++)
            {
                ct.ThrowIfCancellationRequested();
                var wsId = weightSheetIds[i];

                try
                {
                    var meta = await LoadWsMetaAsync(sql, wsId, ct);
                    if (meta is null)
                    {
                        result.Errors.Add(new RowError { WeightSheetId = wsId, Message = "Weight sheet not found." });
                        continue;
                    }

                    if (meta.As400Id <= 0)
                    {
                        result.Errors.Add(new RowError { WeightSheetId = wsId, Message = "Weight sheet has no AS400 id (As400Id = 0)." });
                        continue;
                    }

                    // Make sure the AS400 connection is still healthy from
                    // any earlier failure before we issue more statements.
                    as400 = await EnsureOpenAsync(as400, ct);

                    // SILDNO key — deletion target and primary key in u5siload.
                    await DeleteBySildnoAsync(as400, meta.As400Id, ct);

                    int expectedRows;
                    if (string.Equals(meta.WeightSheetType, "Delivery", StringComparison.OrdinalIgnoreCase))
                    {
                        await UploadIntakeAsync(sql, as400, meta, ct);
                        expectedRows = 1;
                    }
                    else if (string.Equals(meta.WeightSheetType, "Transfer", StringComparison.OrdinalIgnoreCase))
                    {
                        await UploadTransferAsync(sql, as400, meta, ct);
                        expectedRows = 2;
                    }
                    else
                    {
                        result.Errors.Add(new RowError
                        {
                            WeightSheetId = wsId,
                            As400Id = meta.As400Id,
                            Message = $"Unsupported weight sheet type: {meta.WeightSheetType}"
                        });
                        continue;
                    }

                    // Verify the rows actually landed. The IBM i ODBC driver
                    // can silently drop INSERTs (e.g. when commit-mode +
                    // journaling are mismatched). If the row count after
                    // commit is zero, surface that explicitly instead of
                    // claiming success.
                    var actual = await CountBySildnoAsync(as400, meta.As400Id, ct);
                    if (actual < expectedRows)
                    {
                        result.Errors.Add(new RowError
                        {
                            WeightSheetId = wsId,
                            As400Id = meta.As400Id,
                            Message = $"Insert reported success but {actual} of {expectedRows} expected rows are present (silent drop — check commit mode / journaling).",
                        });
                        continue;
                    }

                    result.Succeeded++;
                }
                catch (OperationCanceledException) { throw; }
                catch (OdbcException odbcEx)
                {
                    _log?.LogWarning(odbcEx, "ODBC failure for WS {WsId}; recycling AS400 connection.", wsId);
                    result.Errors.Add(new RowError { WeightSheetId = wsId, Message = odbcEx.Message });
                    // Force-recycle the AS400 connection — driver state
                    // after a failed INSERT can poison subsequent commands.
                    as400 = await RecycleAs400Async(as400, ct);
                }
                catch (Exception ex)
                {
                    _log?.LogWarning(ex, "Upload failed for WS {WsId}", wsId);
                    result.Errors.Add(new RowError { WeightSheetId = wsId, Message = ex.Message });
                }

                progress?.Report((i + 1, weightSheetIds.Count, $"Uploaded {i + 1:N0} of {weightSheetIds.Count:N0}."));
            }
        }
        finally
        {
            try { await as400.DisposeAsync(); } catch { /* ignore */ }
        }

        return result;
    }

    private async Task<OdbcConnection> EnsureOpenAsync(OdbcConnection as400, CancellationToken ct)
    {
        if (as400.State == System.Data.ConnectionState.Open) return as400;
        return await RecycleAs400Async(as400, ct);
    }

    private async Task<OdbcConnection> RecycleAs400Async(OdbcConnection as400, CancellationToken ct)
    {
        try { await as400.DisposeAsync(); } catch { /* ignore */ }
        var fresh = new OdbcConnection(_as400ConnStr);
        await fresh.OpenAsync(ct);
        return fresh;
    }

    // ── SQL Server ──────────────────────────────────────────────────────

    private sealed class WsMeta
    {
        public long WeightSheetId { get; set; }
        public long As400Id { get; set; }
        public string WeightSheetType { get; set; } = "";
        public int LocationId { get; set; }
        public int? SourceLocationId { get; set; }
        public int? DestinationLocationId { get; set; }
        public long? LotId { get; set; }
        public long? ItemId { get; set; }
        public Guid RowUid { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    private static async Task<WsMeta?> LoadWsMetaAsync(SqlConnection sql, long wsId, CancellationToken ct)
    {
        const string q = @"
SELECT TOP 1 WeightSheetId, As400Id, WeightSheetType, LocationId,
             SourceLocationId, DestinationLocationId, LotId, ItemId,
             RowUid, CreatedAt
  FROM warehouse.WeightSheets
 WHERE WeightSheetId = @id;";
        await using var cmd = new SqlCommand(q, sql);
        cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = wsId;
        await using var r = await cmd.ExecuteReaderAsync(ct);
        if (!await r.ReadAsync(ct)) return null;
        return new WsMeta
        {
            WeightSheetId = r.GetInt64(0),
            As400Id = r.GetInt64(1),
            WeightSheetType = r.IsDBNull(2) ? "" : r.GetString(2),
            LocationId = r.GetInt32(3),
            SourceLocationId = r.IsDBNull(4) ? null : r.GetInt32(4),
            DestinationLocationId = r.IsDBNull(5) ? null : r.GetInt32(5),
            LotId = r.IsDBNull(6) ? null : r.GetInt64(6),
            ItemId = r.IsDBNull(7) ? null : r.GetInt64(7),
            RowUid = r.GetGuid(8),
            CreatedAt = r.GetDateTime(9),
        };
    }

    private static async Task<decimal> GetNetLbsAsync(SqlConnection sql, Guid wsRowUid, CancellationToken ct)
    {
        const string q = @"
SELECT ISNULL(SUM(ISNULL(itd.NetQty, ISNULL(itd.DirectQty, 0))), 0)
  FROM Inventory.InventoryTransactionDetails itd
 WHERE itd.RefId = @uid AND itd.RefType = 'WeightSheet' AND itd.IsVoided = 0;";
        await using var cmd = new SqlCommand(q, sql);
        cmd.Parameters.Add("@uid", SqlDbType.UniqueIdentifier).Value = wsRowUid;
        var raw = await cmd.ExecuteScalarAsync(ct);
        return raw is decimal d ? d : 0m;
    }

    // ── Intake (Delivery) ──────────────────────────────────────────────

    private sealed class IntakeAux
    {
        public long? LotAs400Id { get; set; }
        public int? LotSplitGroupId { get; set; }
        public long? CropId { get; set; }
        public string? Commodity { get; set; }
        public string? FsaNumber { get; set; }
        public List<(long As400AccountId, decimal SplitPercent)> Splits { get; } = new();
    }

    private static async Task<IntakeAux> LoadIntakeAuxAsync(SqlConnection sql, WsMeta ws, CancellationToken ct)
    {
        var aux = new IntakeAux();

        if (ws.LotId.HasValue)
        {
            // Lot + product crop + item description
            const string q = @"
SELECT TOP 1
       lot.As400Id              AS LotAs400Id,
       lot.SplitGroupId         AS SplitGroupId,
       p.CropId                 AS CropId,
       i.Description            AS Commodity
  FROM Inventory.Lots lot
  LEFT JOIN product.Items    i ON i.ItemId    = lot.ItemId
  LEFT JOIN product.Products p ON p.ProductId = lot.ProductId
 WHERE lot.LotId = @lotId;";
            await using var cmd = new SqlCommand(q, sql);
            cmd.Parameters.Add("@lotId", SqlDbType.BigInt).Value = ws.LotId.Value;
            await using var r = await cmd.ExecuteReaderAsync(ct);
            if (await r.ReadAsync(ct))
            {
                aux.LotAs400Id = r.IsDBNull(0) ? null : r.GetInt64(0);
                aux.LotSplitGroupId = r.IsDBNull(1) ? null : r.GetInt32(1);
                aux.CropId = r.IsDBNull(2) ? null : r.GetInt64(2);
                aux.Commodity = r.IsDBNull(3) ? null : r.GetString(3);
            }
        }

        if (ws.LotId.HasValue)
        {
            // Splits: primary first, then by split percent desc (stable enough).
            const string qs = @"
SELECT a.As400AccountId, lsg.SplitPercent, lsg.PrimaryAccount
  FROM Inventory.LotSplitGroups lsg
  JOIN account.Accounts a ON a.AccountId = lsg.AccountId
 WHERE lsg.LotId = @lotId
 ORDER BY lsg.PrimaryAccount DESC, lsg.SplitPercent DESC;";
            await using var cmd = new SqlCommand(qs, sql);
            cmd.Parameters.Add("@lotId", SqlDbType.BigInt).Value = ws.LotId.Value;
            await using var r = await cmd.ExecuteReaderAsync(ct);
            while (await r.ReadAsync(ct))
            {
                var as400AcctId = r.IsDBNull(0) ? 0L : r.GetInt64(0);
                var pct = r.IsDBNull(1) ? 0m : r.GetDecimal(1);
                aux.Splits.Add((as400AcctId, pct));
            }
        }

        if (ws.LotId.HasValue)
        {
            // FSA number = first 11 chars of trait description where TraitTypeId = 19.
            const string qf = @"
SELECT TOP 1 LEFT(t.Description, 11) AS Fsa
  FROM Inventory.LotTraits lt
  JOIN product.Traits t ON t.TraitId = lt.TraitId
 WHERE lt.LotId = @lotId AND lt.TraitTypeId = 19;";
            await using var cmd = new SqlCommand(qf, sql);
            cmd.Parameters.Add("@lotId", SqlDbType.BigInt).Value = ws.LotId.Value;
            var raw = await cmd.ExecuteScalarAsync(ct);
            aux.FsaNumber = raw as string;
        }

        return aux;
    }

    private async Task UploadIntakeAsync(SqlConnection sql, OdbcConnection as400, WsMeta ws, CancellationToken ct)
    {
        var net = await GetNetLbsAsync(sql, ws.RowUid, ct);
        var aux = await LoadIntakeAuxAsync(sql, ws, ct);
        var ldDate = ToPacificYyyyMmDd(ws.CreatedAt);

        // Pad splits to exactly 10 slots so SICNO1..10 / SIPCT1..10 always
        // bind to a parameter — DB2 doesn't like mid-INSERT defaulting.
        var slots = new (long acct, decimal pct)[10];
        for (int i = 0; i < aux.Splits.Count && i < 10; i++)
            slots[i] = aux.Splits[i];

        // U5SILOAD column-name conventions confirmed against the AgVantage
        // V8.1 file structures PDF:
        //   - SICNO1..SICNO9 + SICN10  (the 10th slot drops the trailing
        //     'O' so the name stays at 6 chars total).
        //   - SIPCT1..SIPCT9 + SIPC10  (same pattern — drops the 'T').
        //   - SILOT#  needs delimited quoting because the IBM i ODBC
        //     driver strips trailing # from unquoted identifiers.
        const string sqlInsert = @"
INSERT INTO U5SILOAD
    (SILDNO,
     SICNO1, SIPCT1, SICNO2, SIPCT2, SICNO3, SIPCT3, SICNO4, SIPCT4,
     SICNO5, SIPCT5, SICNO6, SIPCT6, SICNO7, SIPCT7, SICNO8, SIPCT8,
     SICNO9, SIPCT9, SICN10, SIPC10,
     SICROP, SILOC, SILDDT, SILDIO,
     SIGROS, SITARE, SINET,
     ""SILOT#"", SISPGP, SIFMNO, SITSCM, SICOMM)
VALUES
    (?,
     ?, ?, ?, ?, ?, ?, ?, ?,
     ?, ?, ?, ?, ?, ?, ?, ?,
     ?, ?, ?, ?,
     ?, ?, ?, ?,
     ?, ?, ?,
     ?, ?, ?, ?, ?)";

        await using var cmd = new OdbcCommand(sqlInsert, as400);
        cmd.Parameters.AddWithValue("p_sildno", ws.As400Id);
        for (int i = 0; i < 10; i++)
        {
            cmd.Parameters.AddWithValue($"p_sicno{i + 1}", slots[i].acct);
            cmd.Parameters.AddWithValue($"p_sipct{i + 1}", slots[i].pct);
        }
        cmd.Parameters.AddWithValue("p_sicrop", aux.CropId ?? 0L);
        cmd.Parameters.AddWithValue("p_siloc", ws.LocationId);
        cmd.Parameters.AddWithValue("p_silddt", ldDate);
        cmd.Parameters.AddWithValue("p_sildio", "I");
        cmd.Parameters.AddWithValue("p_sigros", net);
        cmd.Parameters.AddWithValue("p_sitare", 0m);
        cmd.Parameters.AddWithValue("p_sinet", net);
        cmd.Parameters.AddWithValue("p_silot", aux.LotAs400Id ?? 0L);
        cmd.Parameters.AddWithValue("p_sispgp", aux.LotSplitGroupId ?? 0);
        cmd.Parameters.AddWithValue("p_sifmno", aux.FsaNumber ?? "");
        cmd.Parameters.AddWithValue("p_sitscm", "N");
        cmd.Parameters.AddWithValue("p_sicomm", aux.Commodity ?? "");

        await cmd.ExecuteNonQueryAsync(ct);
    }

    // ── Transfer ───────────────────────────────────────────────────────

    private static async Task<long?> GetTransferCropIdAsync(SqlConnection sql, long? itemId, CancellationToken ct)
    {
        if (!itemId.HasValue) return null;
        const string q = @"
SELECT TOP 1 p.CropId
  FROM product.Items    i
  JOIN product.Products p ON p.ProductId = i.ProductId
 WHERE i.ItemId = @itemId;";
        await using var cmd = new SqlCommand(q, sql);
        cmd.Parameters.Add("@itemId", SqlDbType.BigInt).Value = itemId.Value;
        var raw = await cmd.ExecuteScalarAsync(ct);
        if (raw is null || raw is DBNull) return null;
        if (raw is long l) return l;
        if (raw is int n) return n;
        return Convert.ToInt64(raw);
    }

    private async Task UploadTransferAsync(SqlConnection sql, OdbcConnection as400, WsMeta ws, CancellationToken ct)
    {
        var net = await GetNetLbsAsync(sql, ws.RowUid, ct);
        var crop = await GetTransferCropIdAsync(sql, ws.ItemId, ct) ?? 0L;
        var ldDate = ToPacificYyyyMmDd(ws.CreatedAt);

        // Two rows: O at SourceLocationId, I at DestinationLocationId.
        var srcLoc = ws.SourceLocationId      ?? ws.LocationId;
        var dstLoc = ws.DestinationLocationId ?? ws.LocationId;

        const string sqlInsert = @"
INSERT INTO U5SILOAD
    (SILDNO, SICROP, SILOC, SILDDT, SILDIO,
     SIGROS, SITARE, SINET,
     SIAMTH, SITLOC, SITTCK, SITSCM)
VALUES
    (?, ?, ?, ?, ?,
     ?, ?, ?,
     ?, ?, ?, ?)";

        // Row 1: outbound at source.
        await using (var c1 = new OdbcCommand(sqlInsert, as400))
        {
            c1.Parameters.AddWithValue("p_sildno", ws.As400Id);
            c1.Parameters.AddWithValue("p_sicrop", crop);
            c1.Parameters.AddWithValue("p_siloc",  srcLoc);
            c1.Parameters.AddWithValue("p_silddt", ldDate);
            c1.Parameters.AddWithValue("p_sildio", "O");
            c1.Parameters.AddWithValue("p_sigros", net);
            c1.Parameters.AddWithValue("p_sitare", 0m);
            c1.Parameters.AddWithValue("p_sinet",  net);
            c1.Parameters.AddWithValue("p_siamth", "TR");
            c1.Parameters.AddWithValue("p_sitloc", dstLoc); // counterpart location
            c1.Parameters.AddWithValue("p_sittck", ws.As400Id);
            c1.Parameters.AddWithValue("p_sitscm", "N");
            await c1.ExecuteNonQueryAsync(ct);
        }

        // Row 2: inbound at destination.
        await using (var c2 = new OdbcCommand(sqlInsert, as400))
        {
            c2.Parameters.AddWithValue("p_sildno", ws.As400Id);
            c2.Parameters.AddWithValue("p_sicrop", crop);
            c2.Parameters.AddWithValue("p_siloc",  dstLoc);
            c2.Parameters.AddWithValue("p_silddt", ldDate);
            c2.Parameters.AddWithValue("p_sildio", "I");
            c2.Parameters.AddWithValue("p_sigros", net);
            c2.Parameters.AddWithValue("p_sitare", 0m);
            c2.Parameters.AddWithValue("p_sinet",  net);
            c2.Parameters.AddWithValue("p_siamth", "TR");
            c2.Parameters.AddWithValue("p_sitloc", srcLoc);
            c2.Parameters.AddWithValue("p_sittck", ws.As400Id);
            c2.Parameters.AddWithValue("p_sitscm", "N");
            await c2.ExecuteNonQueryAsync(ct);
        }
    }

    // ── Helpers ────────────────────────────────────────────────────────

    private static async Task DeleteBySildnoAsync(OdbcConnection as400, long sildno, CancellationToken ct)
    {
        await using var cmd = new OdbcCommand("DELETE FROM U5SILOAD WHERE SILDNO = ?", as400);
        cmd.Parameters.AddWithValue("p_sildno", sildno);
        await cmd.ExecuteNonQueryAsync(ct);
    }

    /// <summary>
    /// Wipes every row from U5SILOAD on AS400. Returns the row count that
    /// was deleted (or -1 if the driver doesn't report it). Uses the same
    /// CommitMode=0 connection as the upload path so non-journaled tables
    /// don't silently swallow the DELETE.
    /// </summary>
    public async Task<int> ClearAllAsync(CancellationToken ct)
    {
        await using var as400 = new OdbcConnection(_as400ConnStr);
        await as400.OpenAsync(ct);

        await using var cmd = new OdbcCommand("DELETE FROM U5SILOAD", as400);
        var affected = await cmd.ExecuteNonQueryAsync(ct);
        return affected;
    }

    private static async Task<int> CountBySildnoAsync(OdbcConnection as400, long sildno, CancellationToken ct)
    {
        await using var cmd = new OdbcCommand("SELECT COUNT(*) FROM U5SILOAD WHERE SILDNO = ?", as400);
        cmd.Parameters.AddWithValue("p_sildno", sildno);
        var raw = await cmd.ExecuteScalarAsync(ct);
        if (raw is null || raw is DBNull) return 0;
        if (raw is int i) return i;
        if (raw is long l) return (int)l;
        if (raw is decimal d) return (int)d;
        return int.TryParse(raw.ToString(), out var p) ? p : 0;
    }

    /// <summary>Numeric YYYYMMDD in Pacific time — matches the AS400 convention.</summary>
    private static long ToPacificYyyyMmDd(DateTime utcOrLocal)
    {
        var asUtc = utcOrLocal.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(utcOrLocal, DateTimeKind.Utc)
            : utcOrLocal.ToUniversalTime();
        var pt = TimeZoneInfo.ConvertTimeFromUtc(asUtc, PacificTz);
        return pt.Year * 10000L + pt.Month * 100L + pt.Day;
    }
}
