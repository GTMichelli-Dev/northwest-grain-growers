using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Odbc;

namespace GrainManagement.As400Sync;

public sealed class As400Reader
{
    private readonly string _connStr;

    public As400Reader(IConfiguration cfg)
    {
        _connStr = cfg.GetConnectionString("As400Odbc")
                  ?? throw new InvalidOperationException("Missing ConnectionStrings:As400Odbc");
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

        // This query is expected to match the columns in /mnt/data/Accounts.sql
        // (AccountId, Active, EntityName, LookupName, OwnerFirstName, OwnerLastName, ...)
        // If you later move the query to config, keep the column aliases the same.
        var sql = @"
SELECT  CSCNO AccountId,
CASE WHEN  CSACT='I' THEN 0 ELSE 1 END  Active,
CSCONM EntityName,
CSLKNM  LookupName,
CSFRNM  OwnerFirstName,
CSLSNM   OwnerLastName ,
CASE WHEN CSMBER='P' THEN 1 ELSE 0 END  IsProducer,
0  IsWholeSale,
0  IsAutoPriced,
0  PaysRoyalities,
CASE
  WHEN CSTEXD = 0 THEN NULL
  ELSE DATE(
    SUBSTR(DIGITS(CSTEXD),1,4) || '-' ||
    SUBSTR(DIGITS(CSTEXD),5,2) || '-' ||
    SUBSTR(DIGITS(CSTEXD),7,2)
  )
END AS TaxExemptDate,
CASE WHEN CSTXID='' THEN null ELSE CSTXID END TaxId,
CURRENT TIMESTAMP CreatedAt,
CSEADR  Email,
CASE WHEN CSESTMT='Y' THEN 1 ELSE 0 END  EmailWeightSheet,
CASE WHEN CSPSTA='Y'THEN 1 ELSE 0 END  PrintWeightSheet,
CASE WHEN CSESTMT='Y' THEN 1 ELSE 0 END  EmailStatement,
CASE WHEN CSPSTA='Y'THEN 1 ELSE 0 END  PrintStatement,
CSAD1  Address1 ,
CSAD2  Address2 ,
CSCITY  City ,
CSSTAT  State,  
CSZIP  Zip,
 CSCOUN Country,
CSHPHN  Phone1,
CSWPHN  Phone2,
CSMPHN  MobilePhone,
1  CustomerPaysRoyalties,
''  Contact,
Trim(
 COALESCE(trim(CSMEMO), '') || 
    COALESCE(trim(CSNOT1), '') || 
    COALESCE(trim(CSNOT2), '')) Notes


   FROM COMDATA.U4CSTMR
";

        await using var cmd = new OdbcCommand(sql, conn);
        // Important with ODBC: ExecuteReaderAsync exists, but some drivers behave better with sync ExecuteReader.
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
                Active: GetBool01(reader, "Active"),
                EntityName: GetTrim(reader, "EntityName"),
                LookupName: GetTrim(reader, "LookupName"),
                OwnerFirstName: GetTrim(reader, "OwnerFirstName"),
                OwnerLastName: GetTrim(reader, "OwnerLastName"),
                IsProducer: GetBool01(reader, "IsProducer"),
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
}
