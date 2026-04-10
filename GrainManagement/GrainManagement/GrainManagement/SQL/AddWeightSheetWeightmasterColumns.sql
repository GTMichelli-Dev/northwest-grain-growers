/*  ================================================================
    AddWeightSheetWeightmasterColumns.sql
    Add WeightmasterName and WeightmasterUserId to WeightSheets
    to track who created the weight sheet.
    ================================================================ */

IF NOT EXISTS (
    SELECT 1
    FROM   sys.columns
    WHERE  object_id = OBJECT_ID(N'[warehouse].[WeightSheets]')
      AND  name = N'WeightmasterName'
)
BEGIN
    ALTER TABLE [warehouse].[WeightSheets]
        ADD [WeightmasterName] NVARCHAR(200) NULL,
            [WeightmasterUserId] INT NULL;
END
GO
