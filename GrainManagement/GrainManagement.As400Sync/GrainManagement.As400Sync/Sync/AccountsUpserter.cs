using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GrainManagement.As400Sync;

public sealed class AccountsUpserter
{
    private readonly string _sqlConnStr;

    public AccountsUpserter(IConfiguration cfg)
    {
        _sqlConnStr = cfg.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Missing ConnectionStrings:SqlServer");
    }

    public async Task UpsertAsync(As400AccountRow row, Guid syncRunId, DateTime utcNow, CancellationToken ct)
    {
        const string mergeSql = @"
MERGE [account].[Accounts] AS tgt
USING (SELECT
    @As400AccountId        AS As400AccountId,
    @Active                AS Active,
    @EntityName            AS EntityName,
    @LookupName            AS LookupName,
    @OwnerFirstName        AS OwnerFirstName,
    @OwnerLastName         AS OwnerLastName,
    @IsProducer            AS IsProducer,
    @IsWholesale           AS IsWholesale,
    @IsAutoPriced          AS IsAutoPriced,
    @PaysRoyalties         AS PaysRoyalties,
    @TaxExemptDate         AS TaxExemptDate,
    @TaxId                 AS TaxId,
    @CreatedAt             AS CreatedAt,
    @Email                 AS Email,
    @EmailWeightSheet      AS EmailWeightSheet,
    @PrintWeightSheet      AS PrintWeightSheet,
    @EmailStatement        AS EmailStatement,
    @PrintStatement        AS PrintStatement,
    @Address1              AS Address1,
    @Address2              AS Address2,
    @City                  AS City,
    @State                 AS State,
    @Zip                   AS Zip,
    @Country               AS Country,
    @Phone1                AS Phone1,
    @Phone2                AS Phone2,
    @MobilePhone           AS MobilePhone,
    @Contact               AS Contact,
    @Notes                 AS Notes,
    @LastSeenSyncRunId     AS LastSeenSyncRunId
) AS src
ON  tgt.As400AccountId = src.As400AccountId
WHEN MATCHED THEN
  UPDATE SET
    tgt.Active            = src.Active,
    tgt.EntityName        = src.EntityName,
    tgt.LookupName        = src.LookupName,
    tgt.OwnerFirstName    = NULLIF(src.OwnerFirstName, ''),
    tgt.OwnerLastName     = NULLIF(src.OwnerLastName, ''),
    tgt.IsProducer        = src.IsProducer,
    tgt.IsWholesale       = src.IsWholesale,
    tgt.IsAutoPriced      = src.IsAutoPriced,
    tgt.PaysRoyalties     = src.PaysRoyalties,
    tgt.TaxExemptDate     = src.TaxExemptDate,
    tgt.TaxId             = src.TaxId,
    tgt.Email             = src.Email,
    tgt.EmailWeightSheet  = src.EmailWeightSheet,
    tgt.PrintWeightSheet  = src.PrintWeightSheet,
    tgt.EmailStatement    = src.EmailStatement,
    tgt.PrintStatement    = src.PrintStatement,
    tgt.Address1          = src.Address1,
    tgt.Address2          = src.Address2,
    tgt.City              = src.City,
    tgt.State             = src.State,
    tgt.Zip               = src.Zip,
    tgt.Country           = src.Country,
    tgt.Phone1            = src.Phone1,
    tgt.Phone2            = src.Phone2,
    tgt.MobilePhone       = src.MobilePhone,
    tgt.Contact           = src.Contact,
    tgt.Notes             = src.Notes,
    tgt.LastSeenSyncRunId = src.LastSeenSyncRunId
WHEN NOT MATCHED THEN
  INSERT (
    As400AccountId,
    Active,
    EntityName,
    LookupName,
    OwnerFirstName,
    OwnerLastName,
    IsProducer,
    IsWholesale,
    IsAutoPriced,
    PaysRoyalties,
    TaxExemptDate,
    TaxId,
    CreatedAt,
    Email,
    EmailWeightSheet,
    PrintWeightSheet,
    EmailStatement,
    PrintStatement,
    Address1,
    Address2,
    City,
    State,
    Zip,
    Country,
    Phone1,
    Phone2,
    MobilePhone,
    Contact,
    Notes,
    LastSeenSyncRunId
  )
  VALUES (
    src.As400AccountId,
    src.Active,
    src.EntityName,
    src.LookupName,
    NULLIF(src.OwnerFirstName, ''),
    NULLIF(src.OwnerLastName, ''),
    src.IsProducer,
    src.IsWholesale,
    src.IsAutoPriced,
    src.PaysRoyalties,
    src.TaxExemptDate,
    src.TaxId,
    COALESCE(src.CreatedAt, SYSUTCDATETIME()),
    src.Email,
    src.EmailWeightSheet,
    src.PrintWeightSheet,
    src.EmailStatement,
    src.PrintStatement,
    src.Address1,
    src.Address2,
    src.City,
    src.State,
    src.Zip,
    src.Country,
    src.Phone1,
    src.Phone2,
    src.MobilePhone,
    src.Contact,
    src.Notes,
    src.LastSeenSyncRunId
  );
";

        await using var conn = new SqlConnection(_sqlConnStr);
        await conn.OpenAsync(ct);

        await using var cmd = new SqlCommand(mergeSql, conn);

        cmd.Parameters.AddWithValue("@As400AccountId", row.As400AccountId);
        cmd.Parameters.AddWithValue("@Active", row.Active);
        cmd.Parameters.AddWithValue("@EntityName", row.EntityName);
        cmd.Parameters.AddWithValue("@LookupName", (object?)row.LookupName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@OwnerFirstName", (object?)row.OwnerFirstName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@OwnerLastName", (object?)row.OwnerLastName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@IsProducer", row.IsProducer);
        cmd.Parameters.AddWithValue("@IsWholesale", row.IsWholeSale);
        cmd.Parameters.AddWithValue("@IsAutoPriced", row.IsAutoPriced);
        cmd.Parameters.AddWithValue("@PaysRoyalties", row.PaysRoyalities);
        cmd.Parameters.AddWithValue("@TaxExemptDate", (object?)row.TaxExemptDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@TaxId", (object?)row.TaxId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@CreatedAt", row.CreatedAt);
        cmd.Parameters.AddWithValue("@Email", (object?)row.Email ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@EmailWeightSheet", row.EmailWeightSheet);
        cmd.Parameters.AddWithValue("@PrintWeightSheet", row.PrintWeightSheet);
        cmd.Parameters.AddWithValue("@EmailStatement", row.EmailStatement);
        cmd.Parameters.AddWithValue("@PrintStatement", row.PrintStatement);
        cmd.Parameters.AddWithValue("@Address1", (object?)row.Address1 ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Address2", (object?)row.Address2 ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@City", (object?)row.City ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@State", (object?)row.State ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Zip", (object?)row.Zip ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Country", (object?)row.Country ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Phone1", (object?)row.Phone1 ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Phone2", (object?)row.Phone2 ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@MobilePhone", (object?)row.MobilePhone ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Contact", (object?)row.Contact ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Notes", (object?)row.Notes ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@LastSeenSyncRunId", syncRunId);

        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task MarkMissingAsInactiveAsync(Guid syncRunId, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_sqlConnStr);
        await conn.OpenAsync(ct);

        const string sql = @"
UPDATE a
SET a.Active = 0
FROM [account].[Accounts] a
WHERE a.As400AccountId IS NOT NULL
  AND (a.LastSeenSyncRunId IS NULL OR a.LastSeenSyncRunId <> @SyncRunId);
";

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@SyncRunId", syncRunId);
        await cmd.ExecuteNonQueryAsync(ct);
    }
}
