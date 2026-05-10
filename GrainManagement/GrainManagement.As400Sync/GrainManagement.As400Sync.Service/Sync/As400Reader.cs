using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.Odbc;
using System.Runtime.CompilerServices;
using System.Threading;

namespace GrainManagement.As400Sync;

public sealed class As400Reader
{
    private readonly string _connStr;
    private readonly As400SyncOptions _opt;
    private readonly ILogger<As400Reader>? _log;

    public As400Reader(IConfiguration cfg, IOptions<As400SyncOptions> options, ILogger<As400Reader>? log = null)
    {
        _connStr = cfg.GetConnectionString("As400Odbc")
                  ?? throw new InvalidOperationException("Missing ConnectionStrings:As400Odbc");

        _opt = options.Value;
        _log = log;
    }

    private OdbcConnection OpenConn()
    {
        var conn = new OdbcConnection(_connStr);
        conn.Open();
        return conn;
    }

    /// <summary>
    /// Wraps any of the read SQL files in <c>SELECT COUNT(*) FROM (...) sub</c>
    /// so the runner can show a determinate progress bar. AS400 / DB2-for-i
    /// supports derived tables in the FROM clause. Returns <c>null</c> on
    /// any failure so the caller falls back to an indeterminate bar instead
    /// of refusing to start the job.
    ///
    /// Handles SQL files that begin with <c>WITH cte AS (...)</c> by lifting
    /// the CTE prefix out of the subquery — DB2 (and most engines) require
    /// <c>WITH</c> to be the very first token of a statement, so wrapping
    /// the whole thing in <c>SELECT COUNT(*) FROM (WITH ... )</c> would fail.
    /// </summary>
    private async Task<long?> CountFromSqlFileAsync(string sqlFile, CancellationToken ct)
    {
        string? sql = null;
        try
        {
            await using var conn = new OdbcConnection(_connStr);
            await conn.OpenAsync(ct);

            var inner = await File.ReadAllTextAsync(sqlFile, ct);
            sql = BuildCountSql(inner);

            await using var cmd = new OdbcCommand(sql, conn);
            var raw = await cmd.ExecuteScalarAsync(ct);

            if (raw is null || raw is DBNull) return null;
            if (raw is long l) return l;
            if (raw is int i) return i;
            if (raw is decimal d) return (long)d;
            if (long.TryParse(raw.ToString(), out var parsed)) return parsed;
            return null;
        }
        catch (Exception ex)
        {
            _log?.LogWarning(ex, "Count query failed for {File}; falling back to indeterminate progress. SQL: {Sql}",
                sqlFile, sql);
            return null;
        }
    }

    /// <summary>
    /// Wraps an inner SELECT in a COUNT(*), respecting any leading
    /// <c>WITH</c> CTE clause. Returns the rewritten SQL.
    /// </summary>
    internal static string BuildCountSql(string inner)
    {
        if (string.IsNullOrWhiteSpace(inner)) return "SELECT COUNT(*) FROM (SELECT 1 FROM SYSIBM.SYSDUMMY1) sub";

        // Strip trailing semicolons / whitespace — they'd be illegal inside
        // a derived-table subquery.
        var trimmed = inner.Trim().TrimEnd(';').TrimEnd();

        var splitIndex = FindMainSelectStart(trimmed);
        if (splitIndex <= 0)
        {
            // No leading CTE — just wrap the whole thing.
            return $"SELECT COUNT(*) FROM ({trimmed}) sub";
        }

        var ctePrefix = trimmed.Substring(0, splitIndex).TrimEnd();
        var mainSelect = trimmed.Substring(splitIndex);

        // Lift the CTE outside, count the main SELECT inside the subquery.
        return $"{ctePrefix}\nSELECT COUNT(*) FROM (\n{mainSelect}\n) sub";
    }

    /// <summary>
    /// Walks an SQL string skipping comments, string literals, and parenthesized
    /// regions, and returns the offset of the first depth-0 <c>SELECT</c>
    /// keyword — i.e. the start of the main SELECT after any CTE definitions.
    /// Returns -1 if none is found.
    /// </summary>
    private static int FindMainSelectStart(string sql)
    {
        int i = 0;
        int depth = 0;
        while (i < sql.Length)
        {
            char c = sql[i];

            // -- line comment
            if (c == '-' && i + 1 < sql.Length && sql[i + 1] == '-')
            {
                while (i < sql.Length && sql[i] != '\n') i++;
                continue;
            }
            // /* block comment */
            if (c == '/' && i + 1 < sql.Length && sql[i + 1] == '*')
            {
                i += 2;
                while (i + 1 < sql.Length && !(sql[i] == '*' && sql[i + 1] == '/')) i++;
                if (i + 1 < sql.Length) i += 2;
                continue;
            }
            // 'string literal' (DB2 doubled-single-quote escape)
            if (c == '\'')
            {
                i++;
                while (i < sql.Length)
                {
                    if (sql[i] == '\'')
                    {
                        if (i + 1 < sql.Length && sql[i + 1] == '\'') { i += 2; continue; }
                        i++;
                        break;
                    }
                    i++;
                }
                continue;
            }

            if (c == '(') { depth++; i++; continue; }
            if (c == ')') { depth--; i++; continue; }

            if (depth == 0 && (c == 'S' || c == 's') && IsKeywordAt(sql, i, "SELECT"))
                return i;

            i++;
        }
        return -1;
    }

    private static bool IsKeywordAt(string s, int pos, string keyword)
    {
        if (pos + keyword.Length > s.Length) return false;
        for (int k = 0; k < keyword.Length; k++)
        {
            if (char.ToUpperInvariant(s[pos + k]) != char.ToUpperInvariant(keyword[k]))
                return false;
        }
        if (pos > 0)
        {
            char prev = s[pos - 1];
            if (char.IsLetterOrDigit(prev) || prev == '_') return false;
        }
        int after = pos + keyword.Length;
        if (after < s.Length)
        {
            char next = s[after];
            if (char.IsLetterOrDigit(next) || next == '_') return false;
        }
        return true;
    }

    public Task<long?> CountAccountsAsync(CancellationToken ct)
        => CountFromSqlFileAsync("Accounts.sql", ct);

    public Task<long?> CountAllProductItemsAsync(CancellationToken ct)
        => CountFromSqlFileAsync("AllProductItems.sql", ct);

    public Task<long?> CountLandlordSplitPercentsAsync(CancellationToken ct)
        => CountFromSqlFileAsync("LandLordSplitPercentages.sql", ct);


    static long GetInt64(IDataRecord r, string col)
    {
        var o = r[col];
        if (o is long l) return l;
        if (o is int i) return i;
        if (o is short s) return s;
        if (o is decimal d) return (long)d;
        if (long.TryParse(o?.ToString()?.Trim(), out var parsed)) return parsed;
        throw new InvalidOperationException($"Column {col} is not a valid BIGINT: '{o}'");
    }

    private static long? GetNullableInt64(IDataRecord r, string col)
    {
        var i = r.GetOrdinal(col);
        if (r.IsDBNull(i)) return null;

        var v = r.GetValue(i);

        if (v is long l) return l;
        if (v is int n) return n;
        if (v is decimal d) return (long)d;

        var s = Convert.ToString(v)?.Trim();
        if (string.IsNullOrEmpty(s)) return null;        // <-- THIS fixes ''
        if (long.TryParse(s, out var parsed)) return parsed;

        throw new InvalidOperationException($"Column {col} is not a valid BIGINT: '{s}'");
    }



    private static string GetTrim(IDataRecord r, string col)
        => r[col]?.ToString()?.Trim() ?? string.Empty;

    private static string? GetTrimNullIfBlank(IDataRecord r, string col)
    {
        var s = r[col]?.ToString()?.Trim();
        return string.IsNullOrWhiteSpace(s) ? null : s;
    }

    private static bool GetBool01(IDataRecord r, string col)
    {
        var o = r[col];
        if (o is bool b) return b;
        if (o is short s) return s != 0;
        if (o is int i) return i != 0;
        if (o is long l) return l != 0;
        if (o is decimal d) return d != 0m;
        var t = o?.ToString()?.Trim();
        return t == "1" || string.Equals(t, "true", StringComparison.OrdinalIgnoreCase) || string.Equals(t, "y", StringComparison.OrdinalIgnoreCase);
    }

    private static DateTime? ParseAs400Date(object? o)
    {
        if (o is null || o is DBNull) return null;

        if (o is DateTime dt) return dt.Date;

        // Many IBM i apps store dates as numeric: YYYYMMDD or YYYYDDD (Julian)
        var s = o.ToString()?.Trim();
        if (string.IsNullOrWhiteSpace(s) || s == "0") return null;

        // If it's already in an ISO-ish string, let DateTime handle it.
        if (DateTime.TryParse(s, out var parsed))
            return parsed.Date;

        // Numeric formats
        if (!long.TryParse(s, out var n))
            return null;

        // YYYYMMDD
        if (s.Length == 8)
        {
            var year = (int)(n / 10000);
            var month = (int)((n / 100) % 100);
            var day = (int)(n % 100);
            try { return new DateTime(year, month, day); }
            catch { return null; }
        }

        // YYYYDDD (Julian day-of-year)
        if (s.Length == 7)
        {
            var year = (int)(n / 1000);
            var dayOfYear = (int)(n % 1000);
            try { return new DateTime(year, 1, 1).AddDays(dayOfYear - 1); }
            catch { return null; }
        }

        return null;
    }


    public async IAsyncEnumerable<As400AccountRow> ReadAccountsAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        await using var conn = new OdbcConnection(_connStr);
        await conn.OpenAsync(ct);

        // Read the query from Accounts.sql
        var sql = await File.ReadAllTextAsync("Accounts.sql", ct);

        await using var cmd = new OdbcCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            ct.ThrowIfCancellationRequested();

            // IMPORTANT: read by the aliases in the SELECT.
            var as400Id = GetInt64(reader, "AccountId");
            if (as400Id <= 0)
                continue;

            // CreatedAt can be driven by SQL Server; keep this as "best effort".
            DateTime createdAt;
            var createdObj = reader["CreatedAt"];
            if (createdObj is DateTime cd) createdAt = cd;
            else if (DateTime.TryParse(createdObj?.ToString(), out var cd2)) createdAt = cd2;
            else createdAt = DateTime.UtcNow;

            yield return new As400AccountRow(
                As400AccountId: as400Id,
                IsActive: GetBool01(reader, "IsActive"),
                EntityName: GetTrim(reader, "EntityName"),
                LookupName: GetTrim(reader, "LookupName"),
                OwnerFirstName: GetTrim(reader, "OwnerFirstName"),
                OwnerLastName: GetTrim(reader, "OwnerLastName"),
                IsProducer: GetBool01(reader, "IsProducer"),
                IsSeedProducer: GetBool01(reader, "IsSeedProducer"),
                IsWholeSale: GetBool01(reader, "IsWholeSale"),
                IsAutoPriced: GetBool01(reader, "IsAutoPriced"),
                PaysRoyalities: GetBool01(reader, "PaysRoyalities"),
                TaxExemptDate: ParseAs400Date(reader["TaxExemptDate"]),
                TaxId: GetTrimNullIfBlank(reader, "TaxId"),
                CreatedAt: createdAt,
                Email: GetTrimNullIfBlank(reader, "Email"),
                EmailWeightSheet: GetBool01(reader, "EmailWeightSheet"),
                PrintWeightSheet: GetBool01(reader, "PrintWeightSheet"),
                EmailStatement: GetBool01(reader, "EmailStatement"),
                PrintStatement: GetBool01(reader, "PrintStatement"),
                Address1: GetTrimNullIfBlank(reader, "Address1"),
                Address2: GetTrimNullIfBlank(reader, "Address2"),
                City: GetTrimNullIfBlank(reader, "City"),
                State: GetTrimNullIfBlank(reader, "State"),
                Zip: GetTrimNullIfBlank(reader, "Zip"),
                Country: GetTrimNullIfBlank(reader, "Country"),
                Phone1: GetTrimNullIfBlank(reader, "Phone1"),
                Phone2: GetTrimNullIfBlank(reader, "Phone2"),
                MobilePhone: GetTrimNullIfBlank(reader, "MobilePhone"),
                CustomerPaysRoyalties: GetBool01(reader, "CustomerPaysRoyalties"),
                Contact: GetTrim(reader, "Contact"),
                Notes: GetTrim(reader, "Notes")
            );
        }
    }

    private static decimal? GetDecimalNullable(IDataRecord r, string col)
    {
        var o = r[col];
        if (o is null || o is DBNull) return null;
        if (o is decimal d) return d;
        if (o is double db) return (decimal)db;
        if (o is float f) return (decimal)f;
        if (o is int i) return i;
        if (o is long l) return l;
        if (decimal.TryParse(o.ToString()?.Trim(), out var parsed)) return parsed;
        return null;
    }

    public async IAsyncEnumerable<As400ProductItemRow> ReadAllProductItemsAsync(
    [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        await using var conn = new OdbcConnection(_connStr);
        await conn.OpenAsync(ct);

        // Prefer override, else read from file (same pattern as Accounts.sql)
        string sql;
      
            sql = await File.ReadAllTextAsync("AllProductItems.sql", ct);

        await using var cmd = new OdbcCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            ct.ThrowIfCancellationRequested();

            var itemId = GetInt64(reader, "IMPROD");
            if (itemId <= 0) continue;

            yield return new As400ProductItemRow(
                ItemId: itemId,
                ItemDescription: GetTrim(reader, "IMDESC"),
                IsActive: GetBool01(reader, "ISACTIVE"),
                ProductCode: GetTrim(reader, "PRODUCTCODE"),
                Category: GetTrim(reader, "CATEGORY"),
                ProductGroup: GetTrimNullIfBlank(reader, "PRODUCTGROUP"),
                CropId: GetNullableInt64(reader, "CROPID"),

                SystemUsage: GetTrimNullIfBlank(reader, "SYSTEM_USAGE"),
                HerbicideSystem: GetTrimNullIfBlank(reader, "HERBICIDE_SYSTEM"),
                Season: GetTrimNullIfBlank(reader, "SEASON"),
                LandProgram: GetTrimNullIfBlank(reader, "LAND_PROGRAM"),
                CertClass: GetTrimNullIfBlank(reader, "CERT_CLASS"),
                Condition: GetTrimNullIfBlank(reader, "CONDITION")
            );
        }
    }





    public async IAsyncEnumerable<As400LandlordSplitPercentRow> ReadLandlordSplitPercentsAsync(
    [EnumeratorCancellation] CancellationToken ct)
    {
        await using var conn = new OdbcConnection(_connStr);
        await conn.OpenAsync(ct);

        var sql = await File.ReadAllTextAsync("LandLordSplitPercentages.sql", ct);

        await using var cmd = new OdbcCommand(sql, conn);
        using var r = cmd.ExecuteReader();

        while (r.Read())
        {
            ct.ThrowIfCancellationRequested();

            var splitGroupId = Convert.ToInt32(r["SplitGroupId"]);
            var as400AccountId = Convert.ToInt64(r["As400AccountId"]);
            var desc = (r["SplitGroupDescription"]?.ToString() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(desc))
                continue;

            var isPrimary = Convert.ToInt32(r["PrimaryGrower"]) == 1;
            var pct = Convert.ToDecimal(r["SplitPercentage"]);

            yield return new As400LandlordSplitPercentRow(
                SplitGroupId: splitGroupId,
                As400AccountId: as400AccountId,
                Description: desc,
                IsPrimaryGrower: isPrimary,
                SplitPercent: pct
            );
        }
    }


}
