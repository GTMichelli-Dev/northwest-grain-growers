/* =====================================================================
   AddCameraAssignments.sql

   Creates [system].[CameraAssignments] — the web-side source of truth for
   which physical camera (hosted on a CameraService) plays which role at
   which location. The CameraService's local SQLite owns the hardware
   connection details (IP, USB device, credentials); this table owns the
   role/assignment layer so central admins can reassign without touching
   field machines.

   A camera is uniquely identified across the system by (ServiceId,
   CameraId). One physical camera may be assigned to multiple roles
   (e.g. inbound view + BOL view) by inserting multiple rows.

   Idempotent — safe to re-run.
   ===================================================================== */

SET NOCOUNT ON;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.tables t
    JOIN   sys.schemas s ON s.schema_id = t.schema_id
    WHERE  s.name = 'system' AND t.name = 'CameraAssignments'
)
BEGIN
    CREATE TABLE [system].[CameraAssignments] (
        [CameraAssignmentId] INT            IDENTITY(1,1) NOT NULL,
        [ServiceId]          VARCHAR(64)    NOT NULL,
        [CameraId]           VARCHAR(64)    NOT NULL,
        [DisplayName]        NVARCHAR(200)  NOT NULL,
        [LocationId]         INT            NULL,
        [ScaleId]            INT            NULL,
        [Role]               VARCHAR(20)    NOT NULL,   -- Inbound | Outbound | BOL | View
        [IsPrimary]          BIT            NOT NULL CONSTRAINT [DF_CamAssign_IsPrimary] DEFAULT (0),
        [IsActive]           BIT            NOT NULL CONSTRAINT [DF_CamAssign_IsActive]  DEFAULT (1),
        [CreatedAt]          DATETIME2(0)   NOT NULL CONSTRAINT [DF_CamAssign_CreatedAt] DEFAULT (sysutcdatetime()),
        [UpdatedAt]          DATETIME2(0)   NULL,
        CONSTRAINT [PK_CameraAssignments] PRIMARY KEY CLUSTERED ([CameraAssignmentId]),
        CONSTRAINT [CK_CamAssign_Role]
            CHECK ([Role] IN ('Inbound','Outbound','BOL','View')),
        CONSTRAINT [FK_CamAssign_Location]
            FOREIGN KEY ([LocationId]) REFERENCES [system].[Locations]([LocationId])
    );

    CREATE INDEX [IX_CamAssign_Lookup]
        ON [system].[CameraAssignments] ([LocationId], [ScaleId], [Role], [IsActive])
        INCLUDE ([ServiceId], [CameraId], [IsPrimary]);

    -- Only one primary per (LocationId, ScaleId, Role). Allows nulls for
    -- View-role cameras that aren't scale-bound.
    CREATE UNIQUE INDEX [UX_CamAssign_OnePrimary]
        ON [system].[CameraAssignments] ([LocationId], [ScaleId], [Role])
        WHERE [IsPrimary] = 1 AND [IsActive] = 1;

    PRINT '  Created [system].[CameraAssignments].';
END
ELSE
BEGIN
    PRINT '  [system].[CameraAssignments] already exists; skipping.';
END
GO
