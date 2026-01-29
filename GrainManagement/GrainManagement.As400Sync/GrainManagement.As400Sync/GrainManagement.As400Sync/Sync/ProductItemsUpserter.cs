using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GrainManagement.As400Sync;

/// <summary>
/// Upserts Categories, ProductGroups, Products, Items, TraitTypes, Traits, ItemTraits, and ProductTraits
/// from a single AS400 product/item row. No EF. Single parameterized SQL command per row.
/// AS400 is source-of-truth for IsActive (via MarkMissingAsInactiveAsync).
///
/// IMPORTANT: product.Items.LastSeenSyncRunId and product.Products.LastSeenSyncRunId are datetime2 in your schema,
/// so we use a run timestamp (syncRunAtUtc) as the marker.
/// </summary>
public sealed class ProductItemsUpserter
{
    private readonly string _sqlConnStr;

    public ProductItemsUpserter(IConfiguration cfg)
    {
        _sqlConnStr = cfg.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Missing ConnectionStrings:SqlServer");
    }

    /// <summary>
    /// Upsert one AS400 product/item row. Sets LastSeenSyncRunId to syncRunAtUtc (datetime marker).
    /// </summary>
    public async Task UpsertAsync(As400ProductItemRow row, Guid syncRunAtUtc, DateTime utcNow, CancellationToken ct)
    {
        if (row.ItemId <= 0) return;
        if (string.IsNullOrWhiteSpace(row.ProductCode)) return;
        if (string.IsNullOrWhiteSpace(row.Category)) return;

        const string sql = @"
SET NOCOUNT ON;

DECLARE @Now datetime2(0) = @UtcNow;

/* ----------------------------
   1) Category
---------------------------- */
MERGE product.Categories AS tgt
USING (SELECT
    NULLIF(LTRIM(RTRIM(@CategoryCode)), '') AS CategoryCode
) AS src
ON tgt.CategoryCode = src.CategoryCode
WHEN MATCHED THEN
  UPDATE SET
    tgt.IsActive = 1,
    tgt.UpdatedAt = @Now
WHEN NOT MATCHED THEN
  INSERT (CategoryCode, Description, IsActive, CreatedAt, UpdatedAt)
  VALUES (src.CategoryCode, src.CategoryCode, 1, @Now, @Now);

DECLARE @CategoryId int =
(
  SELECT CategoryId
  FROM product.Categories
  WHERE CategoryCode = NULLIF(LTRIM(RTRIM(@CategoryCode)), '')
);

/* ----------------------------
   2) ProductGroup (nullable)
---------------------------- */
DECLARE @ProductGroupId int = NULL;

IF NULLIF(LTRIM(RTRIM(@ProductGroupCode)), '') IS NOT NULL
BEGIN
  MERGE product.ProductGroups AS tgt
  USING (SELECT
      @CategoryId AS CategoryId,
      NULLIF(LTRIM(RTRIM(@ProductGroupCode)), '') AS GroupCode
  ) AS src
  ON tgt.GroupCode = src.GroupCode
  WHEN MATCHED THEN
    UPDATE SET
      tgt.CategoryId = src.CategoryId,
      tgt.IsActive = 1,
      tgt.UpdatedAt = @Now
  WHEN NOT MATCHED THEN
    INSERT (CategoryId, GroupCode, Description, IsActive, CreatedAt, UpdatedAt)
    VALUES (src.CategoryId, src.GroupCode, src.GroupCode, 1, @Now, @Now);

  SELECT @ProductGroupId = ProductGroupId
  FROM product.ProductGroups
  WHERE GroupCode = NULLIF(LTRIM(RTRIM(@ProductGroupCode)), '');
END

/* ----------------------------
   3) Product (touch LastSeenSyncRunId)
---------------------------- */
MERGE product.Products AS tgt
USING (SELECT
    @CategoryId AS CategoryId,
    NULLIF(LTRIM(RTRIM(@ProductCode)), '') AS ProductCode,
    NULLIF(@CropId, 0) AS CropId,
    @ProductGroupId AS ProductGroupId,
    @SyncRunAtUtc AS SyncRunAtUtc
) AS src
ON tgt.ProductCode = src.ProductCode
WHEN MATCHED THEN
  UPDATE SET
    tgt.CategoryId = src.CategoryId,
    tgt.CropId = src.CropId,
    tgt.ProductGroupId = src.ProductGroupId,
    tgt.IsActive = 1,
    tgt.LastSeenSyncRunId = src.SyncRunAtUtc,
    tgt.UpdatedAt = @Now
WHEN NOT MATCHED THEN
  INSERT (CategoryId, ProductCode, CropId, Description, IsActive, IsHidden, ProductGroupId, LastSeenSyncRunId, CreatedAt, UpdatedAt)
  VALUES (src.CategoryId, src.ProductCode, src.CropId, src.ProductCode, 1, 0, src.ProductGroupId, src.SyncRunAtUtc, @Now, @Now);

DECLARE @ProductId int =
(
  SELECT ProductId
  FROM product.Products
  WHERE ProductCode = NULLIF(LTRIM(RTRIM(@ProductCode)), '')
);

/* ----------------------------
   4) Item (touch LastSeenSyncRunId)
---------------------------- */
MERGE product.Items AS tgt
USING (SELECT
    @ItemId AS ItemId,
    @ProductId AS ProductId,
    LEFT(NULLIF(LTRIM(RTRIM(@ItemDescription)), ''), 100) AS ItemDescription,
    @ItemIsActive AS IsActive,
    @SyncRunAtUtc AS SyncRunAtUtc
) AS src
ON tgt.ItemId = src.ItemId
WHEN MATCHED THEN
  UPDATE SET
    tgt.ProductId = src.ProductId,
    tgt.Description = COALESCE(src.ItemDescription, tgt.Description),
    tgt.IsActive = src.IsActive,
    tgt.LastSeenSyncRunId = src.SyncRunAtUtc,
    tgt.UpdatedAt = @Now
WHEN NOT MATCHED THEN
  INSERT (ItemId, ProductId, Description, IsActive, LastSeenSyncRunId, CreatedAt, UpdatedAt)
  VALUES (src.ItemId, src.ProductId, COALESCE(src.ItemDescription, CAST(src.ItemId AS nvarchar(100))), src.IsActive, src.SyncRunAtUtc, @Now, @Now);

/* ----------------------------
   5) Ensure TraitTypes (only types we sync)
---------------------------- */
DECLARE @TraitTypes TABLE (TypeCode nvarchar(30) NOT NULL, IsExclusive bit NOT NULL);

INSERT INTO @TraitTypes(TypeCode, IsExclusive)
VALUES
('SYSTEM_USAGE', 1),
('HERBICIDE_SYSTEM', 0),
('SEASON', 1),
('LAND_PROGRAM', 0),
('CERT_CLASS', 1),
('CONDITION', 1),
('CROPID', 1);

MERGE product.TraitTypes AS tgt
USING (SELECT TypeCode, IsExclusive FROM @TraitTypes) AS src
ON tgt.TypeCode = src.TypeCode
WHEN MATCHED THEN
  UPDATE SET
    tgt.IsActive = 1,
    tgt.IsExclusive = src.IsExclusive,
    tgt.UpdatedAt = @Now
WHEN NOT MATCHED THEN
  INSERT (TypeCode, Description, IsActive, CreatedAt, UpdatedAt, IsExclusive)
  VALUES (src.TypeCode, src.TypeCode, 1, @Now, @Now, src.IsExclusive);

/* ----------------------------
   6) Ensure Traits for this row’s values
---------------------------- */
DECLARE @TraitValues TABLE (TypeCode nvarchar(30), TraitCode nvarchar(30), [Description] nvarchar(100));

INSERT INTO @TraitValues(TypeCode, TraitCode, [Description])
SELECT 'SYSTEM_USAGE', LEFT(LTRIM(RTRIM(@SystemUsage)),30), LEFT(LTRIM(RTRIM(@SystemUsage)),100)
WHERE NULLIF(LTRIM(RTRIM(@SystemUsage)), '') IS NOT NULL
UNION ALL
SELECT 'HERBICIDE_SYSTEM', LEFT(LTRIM(RTRIM(@HerbicideSystem)),30), LEFT(LTRIM(RTRIM(@HerbicideSystem)),100)
WHERE NULLIF(LTRIM(RTRIM(@HerbicideSystem)), '') IS NOT NULL
UNION ALL
SELECT 'SEASON', LEFT(LTRIM(RTRIM(@Season)),30), LEFT(LTRIM(RTRIM(@Season)),100)
WHERE NULLIF(LTRIM(RTRIM(@Season)), '') IS NOT NULL
UNION ALL
SELECT 'LAND_PROGRAM', LEFT(LTRIM(RTRIM(@LandProgram)),30), LEFT(LTRIM(RTRIM(@LandProgram)),100)
WHERE NULLIF(LTRIM(RTRIM(@LandProgram)), '') IS NOT NULL
UNION ALL
SELECT 'CERT_CLASS', LEFT(LTRIM(RTRIM(@CertClass)),30), LEFT(LTRIM(RTRIM(@CertClass)),100)
WHERE NULLIF(LTRIM(RTRIM(@CertClass)), '') IS NOT NULL
UNION ALL
SELECT 'CONDITION', LEFT(LTRIM(RTRIM(@Condition)),30), LEFT(LTRIM(RTRIM(@Condition)),100)
WHERE NULLIF(LTRIM(RTRIM(@Condition)), '') IS NOT NULL
UNION ALL
SELECT 'CROPID', LEFT(CAST(NULLIF(@CropId,0) AS nvarchar(30)),30), LEFT(CAST(NULLIF(@CropId,0) AS nvarchar(100)),100)
WHERE NULLIF(@CropId,0) IS NOT NULL;

;WITH tv AS
(
  SELECT
    tt.TraitTypeId,
    v.TraitCode,
    v.[Description]
  FROM @TraitValues v
  JOIN product.TraitTypes tt ON tt.TypeCode = v.TypeCode
)
MERGE product.Traits AS tgt
USING tv AS src
ON tgt.TraitTypeId = src.TraitTypeId AND tgt.TraitCode = src.TraitCode
WHEN MATCHED THEN
  UPDATE SET
    tgt.IsActive = 1,
    tgt.Description = COALESCE(NULLIF(src.[Description],''), tgt.Description),
    tgt.UpdatedAt = @Now
WHEN NOT MATCHED THEN
  INSERT (TraitTypeId, TraitCode, Description, IsActive, CreatedAt, UpdatedAt)
  VALUES (src.TraitTypeId, src.TraitCode, COALESCE(NULLIF(src.[Description],''), src.TraitCode), 1, @Now, @Now);

/* ----------------------------
   7) ItemTraits for managed item-level types (AS400 source-of-truth for these types)
---------------------------- */
DECLARE @ManagedItemTypes TABLE(TypeCode nvarchar(30) PRIMARY KEY);
INSERT INTO @ManagedItemTypes(TypeCode)
VALUES ('SYSTEM_USAGE'),('HERBICIDE_SYSTEM'),('SEASON'),('LAND_PROGRAM'),('CERT_CLASS'),('CONDITION');

DECLARE @DesiredItemTraits TABLE
(
  ItemId bigint NOT NULL,
  TraitId int NOT NULL,
  TraitTypeId int NOT NULL,
  IsExclusiveApplied bit NOT NULL,
  PRIMARY KEY(ItemId, TraitId)
);

INSERT INTO @DesiredItemTraits(ItemId, TraitId, TraitTypeId, IsExclusiveApplied)
SELECT
  @ItemId,
  t.TraitId,
  t.TraitTypeId,
  CASE WHEN tt.IsExclusive = 1 THEN 1 ELSE 0 END
FROM @TraitValues v
JOIN @ManagedItemTypes mit ON mit.TypeCode = v.TypeCode
JOIN product.TraitTypes tt ON tt.TypeCode = v.TypeCode
JOIN product.Traits t ON t.TraitTypeId = tt.TraitTypeId AND t.TraitCode = v.TraitCode;

MERGE product.ItemTraits AS tgt
USING @DesiredItemTraits AS src
ON tgt.ItemId = src.ItemId AND tgt.TraitId = src.TraitId
WHEN MATCHED THEN
  UPDATE SET
    tgt.TraitTypeId = src.TraitTypeId,
    tgt.IsExclusiveApplied = src.IsExclusiveApplied,
    tgt.UpdatedAt = @Now
WHEN NOT MATCHED THEN
  INSERT (ItemId, TraitId, TraitTypeId, IsExclusiveApplied, CreatedAt, UpdatedAt)
  VALUES (src.ItemId, src.TraitId, src.TraitTypeId, src.IsExclusiveApplied, @Now, @Now)
WHEN NOT MATCHED BY SOURCE
 AND tgt.ItemId = @ItemId
 AND tgt.TraitTypeId IN (
     SELECT tt.TraitTypeId
     FROM product.TraitTypes tt
     JOIN @ManagedItemTypes m ON m.TypeCode = tt.TypeCode
 )
THEN DELETE;

/* ----------------------------
   8) ProductTraits for CropId (AS400 source-of-truth for crop binding)
---------------------------- */
DECLARE @CropTraitTypeId int = (SELECT TraitTypeId FROM product.TraitTypes WHERE TypeCode='CROPID');

DECLARE @DesiredProductTraits TABLE
(
  ProductId int NOT NULL,
  TraitId int NOT NULL,
  TraitTypeId int NOT NULL,
  IsExclusiveApplied bit NOT NULL,
  PRIMARY KEY(ProductId, TraitId)
);

IF NULLIF(@CropId,0) IS NOT NULL
BEGIN
  INSERT INTO @DesiredProductTraits(ProductId, TraitId, TraitTypeId, IsExclusiveApplied)
  SELECT
    @ProductId,
    t.TraitId,
    t.TraitTypeId,
    1
  FROM product.Traits t
  WHERE t.TraitTypeId = @CropTraitTypeId
    AND t.TraitCode = LEFT(CAST(@CropId AS nvarchar(30)),30);
END

MERGE product.ProductTraits AS tgt
USING @DesiredProductTraits AS src
ON tgt.ProductId = src.ProductId AND tgt.TraitId = src.TraitId
WHEN MATCHED THEN
  UPDATE SET
    tgt.TraitTypeId = src.TraitTypeId,
    tgt.IsExclusiveApplied = src.IsExclusiveApplied,
    tgt.UpdatedAt = @Now
WHEN NOT MATCHED THEN
  INSERT (ProductId, TraitId, TraitTypeId, IsExclusiveApplied, CreatedAt, UpdatedAt)
  VALUES (src.ProductId, src.TraitId, src.TraitTypeId, src.IsExclusiveApplied, @Now, @Now)
WHEN NOT MATCHED BY SOURCE
 AND tgt.ProductId = @ProductId
 AND tgt.TraitTypeId = @CropTraitTypeId
THEN DELETE;
";

        await using var conn = new SqlConnection(_sqlConnStr);
        await conn.OpenAsync(ct);

        await using var cmd = new SqlCommand(sql, conn);

        // Markers
        cmd.Parameters.AddWithValue("@UtcNow", utcNow);
        cmd.Parameters.AddWithValue("@SyncRunAtUtc", syncRunAtUtc);

        // Core
        cmd.Parameters.AddWithValue("@ItemId", row.ItemId);
        cmd.Parameters.AddWithValue("@ItemDescription", row.ItemDescription ?? string.Empty);
        cmd.Parameters.AddWithValue("@ItemIsActive", row.IsActive);

        cmd.Parameters.AddWithValue("@ProductCode", row.ProductCode);
        cmd.Parameters.AddWithValue("@CategoryCode", row.Category);
        cmd.Parameters.AddWithValue("@ProductGroupCode", (object?)row.ProductGroup ?? DBNull.Value);

        // CropId
        cmd.Parameters.AddWithValue("@CropId", (object?)row.CropId ?? DBNull.Value);

        // Trait values
        cmd.Parameters.AddWithValue("@SystemUsage", (object?)row.SystemUsage ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@HerbicideSystem", (object?)row.HerbicideSystem ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Season", (object?)row.Season ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@LandProgram", (object?)row.LandProgram ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@CertClass", (object?)row.CertClass ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Condition", (object?)row.Condition ?? DBNull.Value);

        await cmd.ExecuteNonQueryAsync(ct);
    }

    /// <summary>
    /// Marks any previously-synced Items not seen in this run as inactive.
    /// Products are also inactivated if not seen AND they have no active items remaining.
    /// </summary>
    public async Task MarkMissingAsInactiveAsync(Guid syncRunAtId, DateTime utcNow, CancellationToken ct)
    {
        const string sql = @"
SET NOCOUNT ON;

-- 1) Items: not seen this run => inactive
UPDATE i
SET
    i.IsActive = 0,
    i.UpdatedAt = @UtcNow
FROM product.Items i
WHERE
    i.LastSeenSyncRunId IS NOT NULL
    AND i.LastSeenSyncRunId <> @syncRunAtId
    AND i.IsActive = 1;

-- 2) Products: not seen this run AND no active items => inactive
UPDATE p
SET
    p.IsActive = 0,
    p.UpdatedAt = @UtcNow
FROM product.Products p
WHERE
    p.LastSeenSyncRunId IS NOT NULL
    AND p.LastSeenSyncRunId <> @syncRunAtId
    AND p.IsActive = 1
    AND NOT EXISTS
    (
        SELECT 1
        FROM product.Items i
        WHERE i.ProductId = p.ProductId
          AND i.IsActive = 1
    );
";

        await using var conn = new SqlConnection(_sqlConnStr);
        await conn.OpenAsync(ct);

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@syncRunAtId", syncRunAtId);
        cmd.Parameters.AddWithValue("@UtcNow", utcNow);

        await cmd.ExecuteNonQueryAsync(ct);
    }
}
