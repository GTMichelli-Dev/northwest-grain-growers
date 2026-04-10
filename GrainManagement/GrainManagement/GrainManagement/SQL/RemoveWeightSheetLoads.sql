/*  ================================================================
    RemoveWeightSheetLoads.sql

    Eliminates the WeightSheetLoads intermediate table.
    InventoryTransactionDetails.RefId now points directly at
    WeightSheet.RowUid instead of WeightSheetLoad.Id.

    Run this ONCE. The migration is idempotent — safe to re-run.
    ================================================================ */

-- Step 1: Migrate existing RefId values
-- Change RefId from WeightSheetLoad.Id → WeightSheet.RowUid
-- Change RefType from 'WeightSheetLoad' → 'WeightSheet'
UPDATE itd
SET    itd.RefId   = wsl.WeightSheetUid,
       itd.RefType = 'WeightSheet'
FROM   [Inventory].[InventoryTransactionDetails] itd
INNER JOIN [warehouse].[WeightSheetLoads] wsl
    ON itd.RefId = wsl.Id
WHERE  itd.RefType = 'WeightSheetLoad';
GO

-- Step 2: Verify migration (should return 0 rows)
-- SELECT COUNT(*) AS RemainingOldRefs
-- FROM [Inventory].[InventoryTransactionDetails]
-- WHERE RefType = 'WeightSheetLoad';
-- GO

-- Step 3: Drop dependent table first
IF OBJECT_ID('[warehouse].[WeightSheetLoadLotAllocations]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [warehouse].[WeightSheetLoadLotAllocations];
    PRINT 'Dropped WeightSheetLoadLotAllocations';
END
GO

-- Step 4: Drop the WeightSheetLoads table
IF OBJECT_ID('[warehouse].[WeightSheetLoads]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [warehouse].[WeightSheetLoads];
    PRINT 'Dropped WeightSheetLoads';
END
GO
