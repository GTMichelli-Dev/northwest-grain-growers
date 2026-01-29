using Microsoft.Extensions.Configuration;
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

    public As400Reader(IConfiguration cfg, IOptions<As400SyncOptions> options)
    {
        _connStr = cfg.GetConnectionString("As400Odbc")
                  ?? throw new InvalidOperationException("Missing ConnectionStrings:As400Odbc");

        _opt = options.Value;
    }

    private OdbcConnection OpenConn()
    {
        var conn = new OdbcConnection(_connStr);
        conn.Open();
        return conn;
    }


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
