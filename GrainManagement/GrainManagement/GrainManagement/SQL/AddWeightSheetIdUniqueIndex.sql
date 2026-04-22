-- ============================================================
-- Make WeightSheetId unique and indexed on warehouse.WeightSheets.
-- (PK is on RowUid; without this, lookups by WeightSheetId do a
-- full table scan and time out under load.)
-- ============================================================

SET XACT_ABORT ON;
SET NOCOUNT ON;

BEGIN TRANSACTION;

-- Guard: halt if duplicates exist. The trigger generates WeightSheetId
-- from WsIdSeed + ROW_NUMBER() so duplicates shouldn't exist, but check
-- before we add a UNIQUE index that would fail mid-migration.
IF EXISTS (
    SELECT 1
      FROM [warehouse].[WeightSheets]
     GROUP BY [WeightSheetId]
    HAVING COUNT(*) > 1
)
BEGIN
    RAISERROR(
        'warehouse.WeightSheets has duplicate WeightSheetId values. '
      + 'Resolve the duplicates before creating a UNIQUE index.',
        16, 1);
    ROLLBACK TRANSACTION;
    RETURN;
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'[warehouse].[WeightSheets]')
      AND name = N'UQ_WeightSheets_WeightSheetId'
)
    EXEC sp_executesql N'
        CREATE UNIQUE INDEX [UQ_WeightSheets_WeightSheetId]
            ON [warehouse].[WeightSheets] ([WeightSheetId]);';

COMMIT TRANSACTION;
