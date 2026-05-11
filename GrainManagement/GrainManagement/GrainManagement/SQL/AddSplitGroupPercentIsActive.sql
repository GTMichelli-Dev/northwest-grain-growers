/* =====================================================================
   AddSplitGroupPercentIsActive.sql

   Adds an IsActive bit to [account].[SplitGroupPercents] so the AS400
   sync can soft-delete percent rows instead of hard-deleting them.
   Hard deletes used to fight FK constraints elsewhere; soft deletes
   keep the row in place while signalling "no longer a member".

   Existing rows backfill to IsActive = 1 (still a member). The matching
   filter index speeds up the typical `WHERE SplitGroupId = X AND IsActive = 1`
   lookup that the admin grids, account screen, grower-delivery split,
   and producer-email recipients all now share.

   Idempotent — safe to re-run.
   ===================================================================== */

SET NOCOUNT ON;
GO

IF NOT EXISTS (
    SELECT 1
    FROM   sys.columns c
    JOIN   sys.tables  t ON t.object_id = c.object_id
    JOIN   sys.schemas s ON s.schema_id = t.schema_id
    WHERE  s.name = 'account'
      AND  t.name = 'SplitGroupPercents'
      AND  c.name = 'IsActive'
)
BEGIN
    ALTER TABLE [account].[SplitGroupPercents]
        ADD [IsActive] BIT NOT NULL
            CONSTRAINT [DF_SplitGroupPercents_IsActive] DEFAULT (1);

    PRINT '  Added column [account].[SplitGroupPercents].[IsActive] (default 1).';
END
ELSE
BEGIN
    PRINT '  [account].[SplitGroupPercents].[IsActive] already exists - skipping ALTER.';
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM   sys.indexes i
    JOIN   sys.tables  t ON t.object_id = i.object_id
    JOIN   sys.schemas s ON s.schema_id = t.schema_id
    WHERE  s.name = 'account'
      AND  t.name = 'SplitGroupPercents'
      AND  i.name = 'IX_SplitGroupPercents_SplitGroupId_IsActive'
)
BEGIN
    CREATE INDEX [IX_SplitGroupPercents_SplitGroupId_IsActive]
        ON [account].[SplitGroupPercents] ([SplitGroupId], [IsActive])
        INCLUDE ([AccountId], [SplitPercent]);

    PRINT '  Created index IX_SplitGroupPercents_SplitGroupId_IsActive.';
END
ELSE
BEGIN
    PRINT '  Index IX_SplitGroupPercents_SplitGroupId_IsActive already exists - skipping CREATE.';
END
GO

PRINT 'AddSplitGroupPercentIsActive.sql applied successfully.';
GO
