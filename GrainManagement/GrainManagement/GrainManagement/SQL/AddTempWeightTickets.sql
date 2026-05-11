/* =====================================================================
   AddTempWeightTickets.sql

   Phase 1 of the temp-weight-ticket feature.

   Creates:
     • [Inventory].[TempWeightTickets]    -- one row per kiosk button-press
     • Extends the CameraAssignments CHECK constraint with 'TempTicket'
       so a camera can be assigned to capture temp-ticket images
       alongside its existing Inbound/Outbound/BOL/View roles.

   The temp-ticket row holds the captured weight + which kiosk pressed
   the button + which server stored it. The web app picks them up on
   the load-create "Retrieve Temp Weight" picker, scoped by (ServerId,
   ScaleId, CreatedAt within today).

   Idempotent — safe to re-run.
   ===================================================================== */

SET NOCOUNT ON;
GO

-- ── 1. Inventory.TempWeightTickets ────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM sys.tables t
    JOIN   sys.schemas s ON s.schema_id = t.schema_id
    WHERE  s.name = 'Inventory' AND t.name = 'TempWeightTickets'
)
BEGIN
    CREATE TABLE [Inventory].[TempWeightTickets] (
        [TempTicketId]    BIGINT          IDENTITY(1,1) NOT NULL,
        [ServerId]        INT             NOT NULL,
        [ScaleId]         INT             NOT NULL,
        [KioskId]         VARCHAR(64)     NOT NULL,
        [Gross]           DECIMAL(18, 3)  NOT NULL,
        [Tare]            DECIMAL(18, 3)  NULL,
        [Net]             DECIMAL(18, 3)  NULL,
        [Units]           VARCHAR(8)      NOT NULL CONSTRAINT [DF_TempTicket_Units] DEFAULT ('lbs'),
        [ImagePath]       NVARCHAR(500)   NULL,
        [CreatedAt]       DATETIME2(0)    NOT NULL CONSTRAINT [DF_TempTicket_CreatedAt] DEFAULT (sysutcdatetime()),
        [CompletedAt]     DATETIME2(0)    NULL,
        [ConsumedByLotId] BIGINT          NULL,
        CONSTRAINT [PK_TempWeightTickets] PRIMARY KEY CLUSTERED ([TempTicketId]),
        CONSTRAINT [FK_TempTicket_Server]
            FOREIGN KEY ([ServerId]) REFERENCES [system].[Servers] ([ServerId])
    );

    -- Picker query: "what temp tickets are available for this server +
    -- scale today?" hits this index directly.
    CREATE INDEX [IX_TempTicket_Picker]
        ON [Inventory].[TempWeightTickets] ([ServerId], [ScaleId], [CreatedAt], [CompletedAt])
        INCLUDE ([Gross], [Tare], [Net], [Units], [KioskId], [ImagePath]);

    -- Purge sweep: "delete rows older than X" hits this index.
    CREATE INDEX [IX_TempTicket_CreatedAt]
        ON [Inventory].[TempWeightTickets] ([CreatedAt]);

    PRINT '  Created [Inventory].[TempWeightTickets] + indexes.';
END
ELSE
BEGIN
    PRINT '  [Inventory].[TempWeightTickets] already exists - skipping CREATE.';
END
GO

-- ── 2. Extend CameraAssignments.Role check to include 'TempTicket' ────
-- A new check constraint replaces the old one. Idempotent: only drops if
-- the old version is in place, only adds if a TempTicket-aware version
-- isn't already there.
IF EXISTS (
    SELECT 1
    FROM   sys.check_constraints
    WHERE  name = 'CK_CamAssign_Role'
      AND  parent_object_id = OBJECT_ID('[system].[CameraAssignments]')
      AND  definition NOT LIKE '%TempTicket%'
)
BEGIN
    ALTER TABLE [system].[CameraAssignments]
        DROP CONSTRAINT [CK_CamAssign_Role];

    ALTER TABLE [system].[CameraAssignments]
        ADD CONSTRAINT [CK_CamAssign_Role]
            CHECK ([Role] IN ('Inbound','Outbound','BOL','View','TempTicket'));

    PRINT '  Extended CK_CamAssign_Role to allow ''TempTicket''.';
END
ELSE IF NOT EXISTS (
    SELECT 1
    FROM   sys.check_constraints
    WHERE  name = 'CK_CamAssign_Role'
      AND  parent_object_id = OBJECT_ID('[system].[CameraAssignments]')
)
BEGIN
    -- Original CameraAssignments table missing the check entirely
    -- (older deployment) — add the full one.
    ALTER TABLE [system].[CameraAssignments]
        ADD CONSTRAINT [CK_CamAssign_Role]
            CHECK ([Role] IN ('Inbound','Outbound','BOL','View','TempTicket'));

    PRINT '  Created CK_CamAssign_Role with TempTicket support.';
END
ELSE
BEGIN
    PRINT '  CK_CamAssign_Role already supports TempTicket - skipping.';
END
GO

PRINT 'AddTempWeightTickets.sql applied successfully.';
GO
