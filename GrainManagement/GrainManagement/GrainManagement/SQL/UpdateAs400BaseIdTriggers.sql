-- ============================================================
-- Triggers touching As400BaseId. Run AddAs400BaseIdColumn.sql first.
--
-- All triggers declared NOT FOR REPLICATION so they don't fire on
-- replicated inserts/updates replayed by the replication agent.
--
-- Triggers in this file:
--   [Inventory].[trg_Lots_AutoGenerateIDs]            INSTEAD OF INSERT
--   [Inventory].[trg_Lots_LockAndSync]                AFTER UPDATE  (folded As400BaseId in)
--   [warehouse].[trg_WeightSheets_AutoGenerateIDs]    INSTEAD OF INSERT
--   [warehouse].[trg_WeightSheets_LockCoreColumns]    AFTER UPDATE  (folded As400BaseId in)
-- ============================================================

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- ============================================================
-- [Inventory].[trg_Lots_AutoGenerateIDs]
-- ============================================================
IF OBJECT_ID(N'[Inventory].[trg_Lots_AutoGenerateIDs]', 'TR') IS NOT NULL
    DROP TRIGGER [Inventory].[trg_Lots_AutoGenerateIDs];
GO

CREATE TRIGGER [Inventory].[trg_Lots_AutoGenerateIDs]
ON [Inventory].[Lots]
INSTEAD OF INSERT
NOT FOR REPLICATION
AS
BEGIN
    SET NOCOUNT ON;

    -- Resolve ServerId from @@SERVERNAME via [system].[Servers].
    DECLARE @ServerName NVARCHAR(128) = CAST(@@SERVERNAME AS NVARCHAR(128));
    DECLARE @ServerId   INT;
    SELECT @ServerId = ServerId
      FROM [system].[Servers]
     WHERE ServerName = @ServerName;

    IF @ServerId IS NULL
    BEGIN
        RAISERROR('No row in [system].[Servers] matches @@SERVERNAME (%s).', 16, 1, @ServerName);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Guard: every inserted row must have a LocationSequenceMapping for this server.
    -- Otherwise the INNER JOIN below would silently drop the row.
    IF EXISTS (
        SELECT 1 FROM inserted i
        WHERE NOT EXISTS (
            SELECT 1 FROM [system].[LocationSequenceMapping] lsm
            WHERE lsm.LocationId = i.LocationId AND lsm.ServerId = @ServerId
        )
    )
    BEGIN
        RAISERROR('No LocationSequenceMapping exists for ServerId=%d and one or more inserted LocationIds.', 16, 1, @ServerId);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- NOTE: As400Id is a computed column (fn_BuildAs400Id(SequenceId, LocationId, BaseId))
    -- so it's omitted from the INSERT column list — SQL Server fills it in automatically.
    INSERT INTO [Inventory].[Lots]
        (LotId, BaseId, As400BaseId, LocationId, ServerId, SequenceId,
         ItemId, ProductId, LotDescription, CreatedAt, IsOpen,
         UpdatedAt, SplitGroupId, LotLabel, Notes, RowUid,
         CreatedByUserName, LotType)
    SELECT
        -- LotId: SSS + LLL (padded 3) + BBBBBB (padded 6)
        CAST(
            RIGHT('000'    + CAST(@ServerId    AS VARCHAR(3)), 3) +
            RIGHT('000'    + CAST(x.LocationId AS VARCHAR(3)), 3) +
            RIGHT('000000' + CAST(x.NextBaseId AS VARCHAR(6)), 6)
        AS BIGINT),

        x.NextBaseId,
        x.NextAs400BaseId,
        x.LocationId,
        @ServerId,
        x.SequenceId,
        x.ItemId,
        x.ProductId,
        x.LotDescription,
        x.CreatedAt,
        x.IsOpen,
        x.UpdatedAt,
        x.SplitGroupId,
        x.LotLabel,
        x.Notes,
        x.RowUid,
        x.CreatedByUserName,
        x.LotType
    FROM (
        SELECT
            i.LocationId, i.ItemId, i.ProductId, i.LotDescription, i.CreatedAt,
            i.IsOpen, i.UpdatedAt, i.SplitGroupId, i.LotLabel, i.Notes, i.RowUid,
            i.CreatedByUserName, i.LotType,
            lsm.SequenceId,
            -- BaseId: per (ServerId, LocationId). Uses current max + this row's rank in the batch.
            mb.MaxBase + ROW_NUMBER() OVER (PARTITION BY i.LocationId ORDER BY (SELECT NULL)) AS NextBaseId,
            -- As400BaseId: per (LocationId, SequenceId), floor at LotSeed.
            CASE WHEN ma.MaxAs400Base < lsm.LotSeed THEN lsm.LotSeed ELSE ma.MaxAs400Base END
                + ROW_NUMBER() OVER (PARTITION BY i.LocationId, lsm.SequenceId ORDER BY (SELECT NULL))
                AS NextAs400BaseId
        FROM inserted i
        INNER JOIN [system].[LocationSequenceMapping] lsm
            ON lsm.LocationId = i.LocationId
           AND lsm.ServerId   = @ServerId
        CROSS APPLY (
            SELECT ISNULL(MAX(l.BaseId), 0) AS MaxBase
              FROM [Inventory].[Lots] l WITH (UPDLOCK, HOLDLOCK)
             WHERE l.ServerId   = @ServerId
               AND l.LocationId = i.LocationId
        ) mb
        CROSS APPLY (
            SELECT ISNULL(MAX(l.As400BaseId), 0) AS MaxAs400Base
              FROM [Inventory].[Lots] l WITH (UPDLOCK, HOLDLOCK)
             WHERE l.LocationId = i.LocationId
               AND l.SequenceId = lsm.SequenceId
        ) ma
    ) x;
END;
GO

ALTER TABLE [Inventory].[Lots] ENABLE TRIGGER [trg_Lots_AutoGenerateIDs];
GO

-- ============================================================
-- [Inventory].[trg_Lots_LockAndSync]
--   As400BaseId added to the immutability check.
-- ============================================================
IF OBJECT_ID(N'[Inventory].[trg_Lots_LockAndSync]', 'TR') IS NOT NULL
    DROP TRIGGER [Inventory].[trg_Lots_LockAndSync];
GO

CREATE TRIGGER [Inventory].[trg_Lots_LockAndSync]
ON [Inventory].[Lots]
AFTER UPDATE
NOT FOR REPLICATION
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
          FROM inserted i
          JOIN deleted  d ON i.LotId = d.LotId
         WHERE i.LocationId  <> d.LocationId
            OR i.ServerId    <> d.ServerId
            OR i.BaseId      <> d.BaseId
            OR i.As400BaseId <> d.As400BaseId
            OR i.LotId       <> d.LotId
            OR i.As400Id     <> d.As400Id
            OR i.SequenceId  <> d.SequenceId
    )
    BEGIN
        RAISERROR('Security Violation: Lot key components (LotId, As400Id, Location, Server, Base, As400Base, Sequence) are immutable.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    IF UPDATE(LotDescription)
    BEGIN
        UPDATE l
           SET l.LotLabel = CAST(l.LotId AS NVARCHAR(20)) + ' - ' + l.LotDescription
          FROM [Inventory].[Lots] l
          INNER JOIN inserted i ON l.LotId = i.LotId;
    END;
END;
GO

ALTER TABLE [Inventory].[Lots] ENABLE TRIGGER [trg_Lots_LockAndSync];
GO

-- ============================================================
-- [warehouse].[trg_WeightSheets_AutoGenerateIDs]
-- ============================================================
IF OBJECT_ID(N'[warehouse].[trg_WeightSheets_AutoGenerateIDs]', 'TR') IS NOT NULL
    DROP TRIGGER [warehouse].[trg_WeightSheets_AutoGenerateIDs];
GO

CREATE TRIGGER [warehouse].[trg_WeightSheets_AutoGenerateIDs]
ON [warehouse].[WeightSheets]
INSTEAD OF INSERT
NOT FOR REPLICATION
AS
BEGIN
    SET NOCOUNT ON;

    -- Resolve ServerId from @@SERVERNAME via [system].[Servers].
    DECLARE @ServerName NVARCHAR(128) = CAST(@@SERVERNAME AS NVARCHAR(128));
    DECLARE @ServerId   INT;
    SELECT @ServerId = ServerId
      FROM [system].[Servers]
     WHERE ServerName = @ServerName;

    IF @ServerId IS NULL
    BEGIN
        RAISERROR('No row in [system].[Servers] matches @@SERVERNAME (%s).', 16, 1, @ServerName);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Guard: every inserted row must have a LocationSequenceMapping for this server.
    IF EXISTS (
        SELECT 1 FROM inserted i
        WHERE NOT EXISTS (
            SELECT 1 FROM [system].[LocationSequenceMapping] lsm
            WHERE lsm.LocationId = i.LocationId AND lsm.ServerId = @ServerId
        )
    )
    BEGIN
        RAISERROR('No LocationSequenceMapping exists for ServerId=%d and one or more inserted LocationIds.', 16, 1, @ServerId);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- NOTE: As400Id is a computed column (fn_BuildAs400Id(SequenceId, LocationId, BaseId))
    -- so it's omitted from the INSERT column list — SQL Server fills it in automatically.
    INSERT INTO [warehouse].[WeightSheets]
        (WeightSheetId, BaseId, As400BaseId, LocationId, ServerId, SequenceId,
         WeightSheetType, CreationDate, CreatedAt, ClosedAt,
         HaulerId, Miles, CustomRateDescription, RateType, Rate,
         LotId, Notes, UpdatedAt, RowUid, WeightmasterName, StatusId)
    SELECT
        -- WeightSheetId: SSS + LLL (padded 3) + BBBBBB (padded 6)
        CAST(
            RIGHT('000'    + CAST(@ServerId    AS VARCHAR(3)), 3) +
            RIGHT('000'    + CAST(x.LocationId AS VARCHAR(3)), 3) +
            RIGHT('000000' + CAST(x.NextBaseId AS VARCHAR(6)), 6)
        AS BIGINT),

        x.NextBaseId,
        x.NextAs400BaseId,
        x.LocationId,
        @ServerId,
        x.SequenceId,
        x.WeightSheetType,
        x.CreationDate,
        x.CreatedAt,
        x.ClosedAt,
        x.HaulerId,
        x.Miles,
        x.CustomRateDescription,
        x.RateType,
        x.Rate,
        x.LotId,
        x.Notes,
        x.UpdatedAt,
        x.RowUid,
        x.WeightmasterName,
        x.StatusId
    FROM (
        SELECT
            i.LocationId, i.WeightSheetType, i.CreationDate, i.CreatedAt, i.ClosedAt,
            i.HaulerId, i.Miles, i.CustomRateDescription, i.RateType, i.Rate,
            i.LotId, i.Notes, i.UpdatedAt, i.RowUid, i.WeightmasterName, i.StatusId,
            lsm.SequenceId,
            -- BaseId: per (ServerId, LocationId). Uses current max + this row's rank in the batch.
            mb.MaxBase + ROW_NUMBER() OVER (PARTITION BY i.LocationId ORDER BY (SELECT NULL)) AS NextBaseId,
            -- As400BaseId: per (LocationId, SequenceId), floor at WeightSheetSeed.
            CASE WHEN ma.MaxAs400Base < lsm.WeightSheetSeed THEN lsm.WeightSheetSeed ELSE ma.MaxAs400Base END
                + ROW_NUMBER() OVER (PARTITION BY i.LocationId, lsm.SequenceId ORDER BY (SELECT NULL))
                AS NextAs400BaseId
        FROM inserted i
        INNER JOIN [system].[LocationSequenceMapping] lsm
            ON lsm.LocationId = i.LocationId
           AND lsm.ServerId   = @ServerId
        CROSS APPLY (
            SELECT ISNULL(MAX(ws.BaseId), 0) AS MaxBase
              FROM [warehouse].[WeightSheets] ws WITH (UPDLOCK, HOLDLOCK)
             WHERE ws.ServerId   = @ServerId
               AND ws.LocationId = i.LocationId
        ) mb
        CROSS APPLY (
            SELECT ISNULL(MAX(ws.As400BaseId), 0) AS MaxAs400Base
              FROM [warehouse].[WeightSheets] ws WITH (UPDLOCK, HOLDLOCK)
             WHERE ws.LocationId = i.LocationId
               AND ws.SequenceId = lsm.SequenceId
        ) ma
    ) x;
END;
GO

ALTER TABLE [warehouse].[WeightSheets] ENABLE TRIGGER [trg_WeightSheets_AutoGenerateIDs];
GO

-- ============================================================
-- [warehouse].[trg_WeightSheets_LockCoreColumns]
--   As400BaseId added to the immutability check.
-- ============================================================
IF OBJECT_ID(N'[warehouse].[trg_WeightSheets_LockCoreColumns]', 'TR') IS NOT NULL
    DROP TRIGGER [warehouse].[trg_WeightSheets_LockCoreColumns];
GO

CREATE TRIGGER [warehouse].[trg_WeightSheets_LockCoreColumns]
ON [warehouse].[WeightSheets]
AFTER UPDATE
NOT FOR REPLICATION
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
          FROM inserted i
          JOIN deleted  d ON i.WeightSheetId = d.WeightSheetId
         WHERE i.LocationId    <> d.LocationId
            OR i.ServerId      <> d.ServerId
            OR i.BaseId        <> d.BaseId
            OR i.As400BaseId   <> d.As400BaseId
            OR i.WeightSheetId <> d.WeightSheetId
            OR i.As400Id       <> d.As400Id
            OR i.SequenceId    <> d.SequenceId
    )
    BEGIN
        RAISERROR('Security Violation: WeightSheet key components (WeightSheetId, As400Id, Location, Server, Base, As400Base, Sequence) are immutable.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;
END;
GO

ALTER TABLE [warehouse].[WeightSheets] ENABLE TRIGGER [trg_WeightSheets_LockCoreColumns];
GO

-- Remove the standalone interim trigger if an earlier version installed it.
IF OBJECT_ID(N'[warehouse].[trg_WeightSheets_LockAs400BaseId]', 'TR') IS NOT NULL
    DROP TRIGGER [warehouse].[trg_WeightSheets_LockAs400BaseId];
GO
