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
        RAISERROR('No row in [system].[Servers] matches @@SERVERNAME.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    INSERT INTO [Inventory].[Lots]
        (LotId, As400Id, BaseId, As400BaseId, LocationId, ServerId, SequenceId,
         ItemId, ProductId, LotDescription, CreatedAt, IsOpen,
         UpdatedAt, SplitGroupId, LotLabel, Notes, RowUid,
         CreatedByUserName, LotType)
    SELECT
        s.LotIdSeed + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)),

        -- As400Id is built from As400BaseId
        [system].[fn_BuildAs400Id](@ServerId, sub.NextAs400BaseId),

        sub.NextBaseId,
        sub.NextAs400BaseId,

        i.LocationId,
        @ServerId,
        lsm.SequenceId,
        i.ItemId,
        i.ProductId,
        i.LotDescription,
        i.CreatedAt,
        i.IsOpen,
        i.UpdatedAt,
        i.SplitGroupId,
        i.LotLabel,
        i.Notes,
        i.RowUid,
        i.CreatedByUserName,
        i.LotType
    FROM inserted i
    INNER JOIN [system].[LocationSequenceMapping] lsm
        ON lsm.LocationId = i.LocationId
       AND lsm.ServerId   = @ServerId
    CROSS JOIN [EFOptions].[SiteSetup] s
    CROSS APPLY (
        SELECT
            -- BaseId: per (LocationId, ServerId), bounded by LotSeed
            CASE
                WHEN ISNULL(
                    (SELECT MAX(l2.BaseId) FROM [Inventory].[Lots] l2
                     WHERE l2.LocationId = i.LocationId
                       AND l2.ServerId   = @ServerId),
                    0
                ) < lsm.LotSeed
                THEN lsm.LotSeed + 1
                ELSE ISNULL(
                    (SELECT MAX(l2.BaseId) FROM [Inventory].[Lots] l2
                     WHERE l2.LocationId = i.LocationId
                       AND l2.ServerId   = @ServerId),
                    0
                ) + 1
            END AS NextBaseId,

            -- As400BaseId: per (LocationId, SequenceId), floor at LotSeed.
            CASE
                WHEN ISNULL(
                    (SELECT MAX(l3.As400BaseId) FROM [Inventory].[Lots] l3
                     WHERE l3.LocationId = i.LocationId
                       AND l3.SequenceId = lsm.SequenceId),
                    0
                ) < lsm.LotSeed
                THEN lsm.LotSeed + 1
                ELSE ISNULL(
                    (SELECT MAX(l3.As400BaseId) FROM [Inventory].[Lots] l3
                     WHERE l3.LocationId = i.LocationId
                       AND l3.SequenceId = lsm.SequenceId),
                    0
                ) + 1
            END AS NextAs400BaseId
    ) sub;

    IF EXISTS (
        SELECT 1 FROM [Inventory].[Lots] l
        INNER JOIN inserted i ON l.RowUid = i.RowUid
        WHERE l.BaseId < 1 OR l.BaseId > 99999
    )
    BEGIN
        RAISERROR('BaseId must be between 1 and 99999 for Lots.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    UPDATE s
    SET s.LotIdSeed = s.LotIdSeed + (SELECT COUNT(*) FROM inserted)
    FROM [EFOptions].[SiteSetup] s;
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
        RAISERROR('No row in [system].[Servers] matches @@SERVERNAME.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    INSERT INTO [warehouse].[WeightSheets]
        (WeightSheetId, As400Id, BaseId, As400BaseId, LocationId, ServerId, SequenceId,
         WeightSheetType, CreationDate, CreatedAt, ClosedAt,
         HaulerId, Miles, CustomRateDescription, RateType, Rate,
         LotId, Notes, UpdatedAt, RowUid, WeightmasterName, StatusId)
    SELECT
        s.WsIdSeed + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)),

        -- As400Id is built from As400BaseId
        [system].[fn_BuildAs400Id](@ServerId, sub.NextAs400BaseId),

        sub.NextBaseId,
        sub.NextAs400BaseId,

        i.LocationId,
        @ServerId,
        lsm.SequenceId,
        i.WeightSheetType,
        i.CreationDate,
        i.CreatedAt,
        i.ClosedAt,
        i.HaulerId,
        i.Miles,
        i.CustomRateDescription,
        i.RateType,
        i.Rate,
        i.LotId,
        i.Notes,
        i.UpdatedAt,
        i.RowUid,
        i.WeightmasterName,
        i.StatusId
    FROM inserted i
    INNER JOIN [system].[LocationSequenceMapping] lsm
        ON lsm.LocationId = i.LocationId
       AND lsm.ServerId   = @ServerId
    CROSS JOIN [EFOptions].[SiteSetup] s
    CROSS APPLY (
        SELECT
            -- BaseId: per (LocationId, ServerId), bounded by WeightSheetSeed
            CASE
                WHEN ISNULL(
                    (SELECT MAX(ws2.BaseId) FROM [warehouse].[WeightSheets] ws2
                     WHERE ws2.LocationId = i.LocationId
                       AND ws2.ServerId   = @ServerId),
                    0
                ) < lsm.WeightSheetSeed
                THEN lsm.WeightSheetSeed + 1
                ELSE ISNULL(
                    (SELECT MAX(ws2.BaseId) FROM [warehouse].[WeightSheets] ws2
                     WHERE ws2.LocationId = i.LocationId
                       AND ws2.ServerId   = @ServerId),
                    0
                ) + 1
            END AS NextBaseId,

            -- As400BaseId: per (LocationId, SequenceId), floor at WeightSheetSeed.
            CASE
                WHEN ISNULL(
                    (SELECT MAX(ws3.As400BaseId) FROM [warehouse].[WeightSheets] ws3
                     WHERE ws3.LocationId = i.LocationId
                       AND ws3.SequenceId = lsm.SequenceId),
                    0
                ) < lsm.WeightSheetSeed
                THEN lsm.WeightSheetSeed + 1
                ELSE ISNULL(
                    (SELECT MAX(ws3.As400BaseId) FROM [warehouse].[WeightSheets] ws3
                     WHERE ws3.LocationId = i.LocationId
                       AND ws3.SequenceId = lsm.SequenceId),
                    0
                ) + 1
            END AS NextAs400BaseId
    ) sub;

    IF EXISTS (
        SELECT 1 FROM [warehouse].[WeightSheets] ws
        INNER JOIN inserted i ON ws.RowUid = i.RowUid
        WHERE ws.BaseId < 1 OR ws.BaseId > 99999
    )
    BEGIN
        RAISERROR('BaseId must be between 1 and 99999 for WeightSheets.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    UPDATE s
    SET s.WsIdSeed = s.WsIdSeed + (SELECT COUNT(*) FROM inserted)
    FROM [EFOptions].[SiteSetup] s;
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
