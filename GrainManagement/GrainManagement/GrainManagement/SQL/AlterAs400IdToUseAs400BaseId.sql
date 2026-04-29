-- ============================================================
-- Re-point the computed [As400Id] column on [Inventory].[Lots] and
-- [warehouse].[WeightSheets] so it's computed from [As400BaseId]
-- (per-sequence counter with seed floor) instead of [BaseId]
-- (per-server+location counter).
--
-- Safe on existing data: all current rows have BaseId = As400BaseId,
-- so the computed values don't change.
--
-- Must be run AFTER UpdateAs400BaseIdTriggers.sql so the triggers
-- know how to populate As400BaseId.
-- ============================================================

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- ============================================================
-- [Inventory].[Lots]
-- ============================================================
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Lots_As400Id' AND object_id = OBJECT_ID('Inventory.Lots'))
    DROP INDEX [IX_Lots_As400Id] ON [Inventory].[Lots];
GO

ALTER TABLE [Inventory].[Lots] DROP COLUMN [As400Id];
GO

ALTER TABLE [Inventory].[Lots]
    ADD [As400Id] AS ([system].[fn_BuildAs400Id]([SequenceId], [LocationId], [As400BaseId])) PERSISTED;
GO

CREATE UNIQUE INDEX [IX_Lots_As400Id] ON [Inventory].[Lots]([As400Id]);
GO

-- ============================================================
-- [warehouse].[WeightSheets]
-- ============================================================
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_WeightSheets_As400Id' AND object_id = OBJECT_ID('warehouse.WeightSheets'))
    DROP INDEX [IX_WeightSheets_As400Id] ON [warehouse].[WeightSheets];
GO

ALTER TABLE [warehouse].[WeightSheets] DROP COLUMN [As400Id];
GO

ALTER TABLE [warehouse].[WeightSheets]
    ADD [As400Id] AS ([system].[fn_BuildAs400Id]([SequenceId], [LocationId], [As400BaseId])) PERSISTED;
GO

CREATE UNIQUE INDEX [IX_WeightSheets_As400Id] ON [warehouse].[WeightSheets]([As400Id]);
GO
