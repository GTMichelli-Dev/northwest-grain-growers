/*
    Purpose
    -------
    One-shot (idempotent) merge to populate product master data from AgVantage item master snapshot.

    Source
    ------
      stg.As400ItemMaster (populated by the GrainManagement.As400Sync worker)

    Targets
    -------
      product.Categories      (fallback insert only for dept codes seen in staging)
      product.Conditions
      product.Classes
      product.Products        (optionally sets CommodityId if category matches CommodityCode or stg.AgCropToBaseCommodity)
      product.Types
      product.Items

    Notes
    -----
    - product.Items has an audit trigger that requires SESSION_CONTEXT('AuditUserName') to be set.
      This script sets it to 'As400Sync'.

    - stg.AgDeptCategoryMap and stg.AgCropToBaseCommodity are OPTIONAL mapping tables that
      (if populated/active) will be used to pick CategoryId and CommodityId.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRAN;

    EXEC sys.sp_set_session_context @key = N'AuditUserName', @value = N'As400Sync';

    DECLARE @UNSPEC NVARCHAR(20) = N'UNSPEC';
    DECLARE @UNSPEC_NAME NVARCHAR(100) = N'Unspecified';

    /* Ensure Categories exist for any DeptNumbers we see in staging (fallback only).
       If you use stg.AgDeptCategoryMap, your internal categories may already exist and be used instead. */
    ;WITH src AS (
        SELECT DISTINCT
            CategoryCode = COALESCE(NULLIF(LTRIM(RTRIM(DeptNumber)), N''), @UNSPEC),
            CategoryName = COALESCE(NULLIF(LTRIM(RTRIM(DeptNumber)), N''), @UNSPEC_NAME)
        FROM stg.As400ItemMaster
    )
    INSERT INTO product.Categories (CategoryCode, CategoryName, Active)
    SELECT s.CategoryCode, s.CategoryName, 1
    FROM src s
    WHERE NOT EXISTS (
        SELECT 1
        FROM product.Categories c
        WHERE c.CategoryCode = s.CategoryCode
    );

    /* Conditions: distinct PackageUom */
    MERGE product.Conditions AS tgt
    USING (
        SELECT DISTINCT
            ConditionCode = COALESCE(NULLIF(LTRIM(RTRIM(PackageUom)), N''), @UNSPEC),
            ConditionName = COALESCE(NULLIF(LTRIM(RTRIM(PackageUom)), N''), @UNSPEC_NAME)
        FROM stg.As400ItemMaster
    ) AS src
    ON tgt.ConditionCode = src.ConditionCode
    WHEN MATCHED AND (tgt.ConditionName <> src.ConditionName OR tgt.Active <> 1) THEN
        UPDATE SET ConditionName = src.ConditionName, Active = 1
    WHEN NOT MATCHED THEN
        INSERT (ConditionCode, ConditionName, Active)
        VALUES (src.ConditionCode, src.ConditionName, 1)
    ;

    /* Classes: distinct (CategoryId, FineLineCode) */
    MERGE product.Classes AS tgt
    USING (
        SELECT DISTINCT
            CategoryId = COALESCE(map.CategoryId, c.CategoryId),
            ClassCode  = COALESCE(NULLIF(LTRIM(RTRIM(m.FineLineCode)), N''), @UNSPEC),
            ClassName  = COALESCE(NULLIF(LTRIM(RTRIM(m.FineLineCode)), N''), @UNSPEC_NAME)
        FROM stg.As400ItemMaster m
        LEFT JOIN stg.AgDeptCategoryMap map
            ON map.IsActive = 1
           AND map.AgDept = TRY_CONVERT(int, NULLIF(LTRIM(RTRIM(m.DeptNumber)), N''))
        LEFT JOIN product.Categories c
            ON c.CategoryCode = COALESCE(NULLIF(LTRIM(RTRIM(m.DeptNumber)), N''), @UNSPEC)
        WHERE COALESCE(map.CategoryId, c.CategoryId) IS NOT NULL
    ) AS src
    ON tgt.CategoryId = src.CategoryId
   AND tgt.ClassCode  = src.ClassCode
    WHEN MATCHED AND (tgt.ClassName <> src.ClassName OR tgt.Active <> 1) THEN
        UPDATE SET ClassName = src.ClassName, Active = 1
    WHEN NOT MATCHED THEN
        INSERT (CategoryId, ClassCode, ClassName, Active)
        VALUES (src.CategoryId, src.ClassCode, src.ClassName, 1)
    ;

    /* Products: distinct (CategoryId, Category)
       CommodityId is filled when:
         1) stg.AgCropToBaseCommodity maps AgCropCode=Category, or
         2) product.Commodities.CommodityCode == Category
    */
    MERGE product.Products AS tgt
    USING (
        SELECT DISTINCT
            CategoryId = COALESCE(map.CategoryId, c.CategoryId),
            ProductCode = COALESCE(NULLIF(LTRIM(RTRIM(m.Category)), N''), @UNSPEC),
            CommodityId = COALESCE(cropMap.CommodityId, comByCode.CommodityId),
            ProductName = COALESCE(
                comByMap.CommodityName,
                comByCode.CommodityName,
                COALESCE(NULLIF(LTRIM(RTRIM(m.Category)), N''), @UNSPEC_NAME)
            )
        FROM stg.As400ItemMaster m
        LEFT JOIN stg.AgDeptCategoryMap map
            ON map.IsActive = 1
           AND map.AgDept = TRY_CONVERT(int, NULLIF(LTRIM(RTRIM(m.DeptNumber)), N''))
        LEFT JOIN product.Categories c
            ON c.CategoryCode = COALESCE(NULLIF(LTRIM(RTRIM(m.DeptNumber)), N''), @UNSPEC)
        LEFT JOIN stg.AgCropToBaseCommodity cropMap
            ON cropMap.IsActive = 1
           AND cropMap.AgCropCode = COALESCE(NULLIF(LTRIM(RTRIM(m.Category)), N''), @UNSPEC)
           AND (cropMap.AgDept IS NULL OR cropMap.AgDept = TRY_CONVERT(int, NULLIF(LTRIM(RTRIM(m.DeptNumber)), N'')))
        LEFT JOIN product.Commodities comByMap
            ON comByMap.CommodityId = cropMap.CommodityId
        LEFT JOIN product.Commodities comByCode
            ON comByCode.CommodityCode = COALESCE(NULLIF(LTRIM(RTRIM(m.Category)), N''), @UNSPEC)
        WHERE COALESCE(map.CategoryId, c.CategoryId) IS NOT NULL
    ) AS src
    ON tgt.CategoryId  = src.CategoryId
   AND tgt.ProductCode = src.ProductCode
    WHEN MATCHED THEN
        UPDATE SET
            ProductName = src.ProductName,
            CommodityId = COALESCE(tgt.CommodityId, src.CommodityId),
            Active = 1
    WHEN NOT MATCHED THEN
        INSERT (CategoryId, CommodityId, ProductCode, ProductName, Active)
        VALUES (src.CategoryId, src.CommodityId, src.ProductCode, src.ProductName, 1)
    ;

    /* Types: distinct (ProductId, ItemType) */
    MERGE product.Types AS tgt
    USING (
        SELECT DISTINCT
            ProductId = p.ProductId,
            TypeCode = COALESCE(NULLIF(LTRIM(RTRIM(m.ItemType)), N''), @UNSPEC),
            TypeName = COALESCE(NULLIF(LTRIM(RTRIM(m.ItemType)), N''), @UNSPEC_NAME)
        FROM stg.As400ItemMaster m
        LEFT JOIN stg.AgDeptCategoryMap map
            ON map.IsActive = 1
           AND map.AgDept = TRY_CONVERT(int, NULLIF(LTRIM(RTRIM(m.DeptNumber)), N''))
        LEFT JOIN product.Categories c
            ON c.CategoryCode = COALESCE(NULLIF(LTRIM(RTRIM(m.DeptNumber)), N''), @UNSPEC)
        JOIN product.Products p
            ON p.CategoryId = COALESCE(map.CategoryId, c.CategoryId)
           AND p.ProductCode = COALESCE(NULLIF(LTRIM(RTRIM(m.Category)), N''), @UNSPEC)
        WHERE COALESCE(map.CategoryId, c.CategoryId) IS NOT NULL
    ) AS src
    ON tgt.ProductId = src.ProductId
   AND tgt.TypeCode  = src.TypeCode
    WHEN MATCHED AND (tgt.TypeName <> src.TypeName OR tgt.Active <> 1) THEN
        UPDATE SET TypeName = src.TypeName, Active = 1
    WHEN NOT MATCHED THEN
        INSERT (ProductId, TypeCode, TypeName, Active)
        VALUES (src.ProductId, src.TypeCode, src.TypeName, 1)
    ;

    /* Items: one row per ItemNumber.
       NOTE: product.Items does NOT store Item description; the app can fetch it from stg.As400ItemMaster.
    */
    MERGE product.Items AS tgt
    USING (
        SELECT DISTINCT
            ItemId = TRY_CONVERT(bigint, NULLIF(LTRIM(RTRIM(m.ItemNumber)), N'')),
            TypeId = t.TypeId,
            ClassId = cls.ClassId,
            ConditionId = cond.ConditionId,
            Active = CASE
                        WHEN UPPER(COALESCE(NULLIF(LTRIM(RTRIM(m.InactiveCode)), N''), N'N')) IN (N'Y', N'1', N'I', N'D') THEN 0
                        ELSE 1
                     END
        FROM stg.As400ItemMaster m
        LEFT JOIN stg.AgDeptCategoryMap map
            ON map.IsActive = 1
           AND map.AgDept = TRY_CONVERT(int, NULLIF(LTRIM(RTRIM(m.DeptNumber)), N''))
        LEFT JOIN product.Categories c
            ON c.CategoryCode = COALESCE(NULLIF(LTRIM(RTRIM(m.DeptNumber)), N''), @UNSPEC)
        JOIN product.Products p
            ON p.CategoryId = COALESCE(map.CategoryId, c.CategoryId)
           AND p.ProductCode = COALESCE(NULLIF(LTRIM(RTRIM(m.Category)), N''), @UNSPEC)
        JOIN product.Types t
            ON t.ProductId = p.ProductId
           AND t.TypeCode = COALESCE(NULLIF(LTRIM(RTRIM(m.ItemType)), N''), @UNSPEC)
        JOIN product.Classes cls
            ON cls.CategoryId = COALESCE(map.CategoryId, c.CategoryId)
           AND cls.ClassCode = COALESCE(NULLIF(LTRIM(RTRIM(m.FineLineCode)), N''), @UNSPEC)
        JOIN product.Conditions cond
            ON cond.ConditionCode = COALESCE(NULLIF(LTRIM(RTRIM(m.PackageUom)), N''), @UNSPEC)
        WHERE TRY_CONVERT(bigint, NULLIF(LTRIM(RTRIM(m.ItemNumber)), N'')) IS NOT NULL
          AND COALESCE(map.CategoryId, c.CategoryId) IS NOT NULL
    ) AS src
    ON tgt.ItemId = src.ItemId
    WHEN MATCHED AND (
         tgt.TypeId <> src.TypeId
      OR tgt.ClassId <> src.ClassId
      OR tgt.ConditionId <> src.ConditionId
      OR tgt.Active <> src.Active
    ) THEN
        UPDATE SET
            TypeId = src.TypeId,
            ClassId = src.ClassId,
            ConditionId = src.ConditionId,
            Active = src.Active
    WHEN NOT MATCHED THEN
        INSERT (ItemId, TypeId, ClassId, ConditionId, Active)
        VALUES (src.ItemId, src.TypeId, src.ClassId, src.ConditionId, src.Active)
    ;

    COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    DECLARE @Err NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@Err, 16, 1);
END CATCH;
