-- AddWeightSheetStatus.sql
--
-- Creates warehouse.WeightSheetStatuses (reference table for the WeightSheet
-- lifecycle), seeds the four allowed values, then blocks any further changes
-- to that table via INSTEAD OF triggers so nobody can add/rename/delete a
-- status.
--
--   0 = Open                     — Not done; still accepting new loads.
--   1 = Pending Not Finished     — 25 loads reached but outbound weight /
--                                   protein / bin are not yet set on every
--                                   load. Edits allowed, no new loads.
--   2 = Pending Finished         — 25 loads with all three fields set. Edits
--                                   allowed, no new loads.
--   3 = Closed                   — End-of-day finalized. No edits.
--
-- Also adds warehouse.WeightSheets.StatusId (TINYINT NOT NULL, DEFAULT 0 =
-- Open) and a FK to the new reference table. Existing rows are backfilled
-- from the current ClosedAt column so already-closed sheets end up with
-- StatusId = 3 (Closed).
--
-- Idempotent: safe to run multiple times. Upgrades cleanly from an earlier
-- 3-row version of this script (where 2 meant Closed) by renumbering
-- existing WeightSheets.StatusId = 2 to 3 before re-seeding.

SET NOCOUNT ON;
GO

-- 1) Reference table --------------------------------------------------------

IF NOT EXISTS (
    SELECT 1 FROM sys.tables t
    JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE s.name = 'warehouse' AND t.name = 'WeightSheetStatuses'
)
BEGIN
    CREATE TABLE [warehouse].[WeightSheetStatuses]
    (
        [StatusId]    TINYINT       NOT NULL CONSTRAINT PK_WeightSheetStatuses PRIMARY KEY,
        [StatusCode]  VARCHAR(20)   NOT NULL CONSTRAINT UX_WeightSheetStatuses_StatusCode UNIQUE,
        [Description] VARCHAR(100)  NULL
    );
END
GO

-- 2) Seed the four allowed values -------------------------------------------
--
-- Drop the lock triggers (if present) so we can safely upgrade from an
-- earlier 3-row seed to the 4-row scheme. We put them back in section 3.
-- We also drop the FK on warehouse.WeightSheets before mutating the
-- reference rows, so that a renumber of existing WS.StatusId values can't
-- trip the constraint mid-way; the FK is re-added in section 5.

IF OBJECT_ID('warehouse.trg_WeightSheetStatuses_BlockInsert', 'TR') IS NOT NULL
    DROP TRIGGER [warehouse].[trg_WeightSheetStatuses_BlockInsert];
IF OBJECT_ID('warehouse.trg_WeightSheetStatuses_BlockUpdate', 'TR') IS NOT NULL
    DROP TRIGGER [warehouse].[trg_WeightSheetStatuses_BlockUpdate];
IF OBJECT_ID('warehouse.trg_WeightSheetStatuses_BlockDelete', 'TR') IS NOT NULL
    DROP TRIGGER [warehouse].[trg_WeightSheetStatuses_BlockDelete];
GO

IF EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = 'FK_WeightSheets_WeightSheetStatuses'
      AND parent_object_id = OBJECT_ID('warehouse.WeightSheets')
)
BEGIN
    ALTER TABLE [warehouse].[WeightSheets]
        DROP CONSTRAINT FK_WeightSheets_WeightSheetStatuses;
END
GO

-- If we are upgrading from the old 3-row scheme (where 2 meant Closed),
-- renumber any WeightSheets.StatusId = 2 → 3 before we overwrite the
-- reference table. Only touch rows if the old 'Closed' row is still at id 2.
IF OBJECT_ID('warehouse.WeightSheetStatuses', 'U') IS NOT NULL
   AND EXISTS (
       SELECT 1 FROM [warehouse].[WeightSheetStatuses]
       WHERE StatusId = 2 AND StatusCode = 'Closed'
   )
   AND COL_LENGTH('warehouse.WeightSheets', 'StatusId') IS NOT NULL
BEGIN
    UPDATE [warehouse].[WeightSheets]
    SET StatusId = 3
    WHERE StatusId = 2;
END
GO

-- Replace the reference rows with the canonical 4-row set.
DELETE FROM [warehouse].[WeightSheetStatuses];

INSERT INTO [warehouse].[WeightSheetStatuses] ([StatusId], [StatusCode], [Description])
VALUES
    (0, 'Open',               'Open and still accepting loads.'),
    (1, 'PendingNotFinished', 'Reached 25 loads; outbound weight, protein, or bin missing. Edits allowed.'),
    (2, 'PendingFinished',    '25 loads with outbound weight, protein, and bin set on every load. Edits allowed.'),
    (3, 'Closed',             'Closed at end of day and immutable.');
GO

-- 3) Lock the reference table with INSTEAD OF triggers ----------------------
--    No row can be inserted, updated, or deleted after seeding. Errors use
--    severity 16 so they surface to the client as a normal exception.

IF OBJECT_ID('warehouse.trg_WeightSheetStatuses_BlockInsert', 'TR') IS NOT NULL
    DROP TRIGGER [warehouse].[trg_WeightSheetStatuses_BlockInsert];
GO

CREATE TRIGGER [warehouse].[trg_WeightSheetStatuses_BlockInsert]
ON [warehouse].[WeightSheetStatuses]
INSTEAD OF INSERT
NOT FOR REPLICATION
AS
BEGIN
    RAISERROR('warehouse.WeightSheetStatuses is a fixed reference table; new rows cannot be added.', 16, 1);
END
GO

IF OBJECT_ID('warehouse.trg_WeightSheetStatuses_BlockUpdate', 'TR') IS NOT NULL
    DROP TRIGGER [warehouse].[trg_WeightSheetStatuses_BlockUpdate];
GO

CREATE TRIGGER [warehouse].[trg_WeightSheetStatuses_BlockUpdate]
ON [warehouse].[WeightSheetStatuses]
INSTEAD OF UPDATE
NOT FOR REPLICATION
AS
BEGIN
    RAISERROR('warehouse.WeightSheetStatuses rows are immutable; updates are not allowed.', 16, 1);
END
GO

IF OBJECT_ID('warehouse.trg_WeightSheetStatuses_BlockDelete', 'TR') IS NOT NULL
    DROP TRIGGER [warehouse].[trg_WeightSheetStatuses_BlockDelete];
GO

CREATE TRIGGER [warehouse].[trg_WeightSheetStatuses_BlockDelete]
ON [warehouse].[WeightSheetStatuses]
INSTEAD OF DELETE
NOT FOR REPLICATION
AS
BEGIN
    RAISERROR('warehouse.WeightSheetStatuses rows cannot be deleted.', 16, 1);
END
GO

-- 4) WeightSheets.StatusId column -------------------------------------------

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('warehouse.WeightSheets')
      AND name = 'StatusId'
)
BEGIN
    ALTER TABLE [warehouse].[WeightSheets]
        ADD [StatusId] TINYINT NOT NULL
            CONSTRAINT DF_WeightSheets_StatusId DEFAULT (0);
END
GO

-- Backfill existing rows: ClosedAt set → Closed (3), otherwise Open (0).
-- Only touch rows that still hold the default value so repeat runs are no-ops.
UPDATE ws
SET ws.StatusId = 3
FROM [warehouse].[WeightSheets] ws
WHERE ws.ClosedAt IS NOT NULL
  AND ws.StatusId = 0;
GO

-- 5) Foreign key ------------------------------------------------------------

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = 'FK_WeightSheets_WeightSheetStatuses'
      AND parent_object_id = OBJECT_ID('warehouse.WeightSheets')
)
BEGIN
    ALTER TABLE [warehouse].[WeightSheets]
        WITH CHECK
        ADD CONSTRAINT FK_WeightSheets_WeightSheetStatuses
            FOREIGN KEY ([StatusId])
            REFERENCES [warehouse].[WeightSheetStatuses] ([StatusId])
            NOT FOR REPLICATION;
END
GO
