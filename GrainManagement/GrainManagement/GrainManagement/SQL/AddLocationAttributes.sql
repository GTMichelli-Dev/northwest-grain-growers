/* =====================================================================
   AddLocationAttributes.sql

   Adds the LocationAttributes / LocationAttributeTypes pair (mirrors the
   existing TransactionAttributes / TransactionAttributeTypes pair) under
   the [system] schema, plus seeds two attribute-type rows:

     • LocationAttributeTypes : REQUIRE_DUMP_TYPE (bool)
     • TransactionAttributeTypes : IS_END_DUMP    (bool)

   Idempotent — safe to re-run. Each step checks for the object's
   existence before creating it.
   ===================================================================== */

SET NOCOUNT ON;
GO

-- LocationAttributeTypes ------------------------------------------------
IF NOT EXISTS (
    SELECT 1 FROM sys.tables t
    JOIN   sys.schemas s ON s.schema_id = t.schema_id
    WHERE  s.name = 'system' AND t.name = 'LocationAttributeTypes'
)
BEGIN
    CREATE TABLE [system].[LocationAttributeTypes] (
        [Id]          INT           IDENTITY(1,1) NOT NULL,
        [Code]        VARCHAR(30)   NOT NULL,
        [Description] NVARCHAR(100) NOT NULL,
        [DataType]    VARCHAR(20)   NOT NULL,
        [IsActive]    BIT           NOT NULL CONSTRAINT [DF_LocAttrTypes_IsActive] DEFAULT (1),
        [CreatedAt]   DATETIME2(0)  NOT NULL CONSTRAINT [DF_LocAttrTypes_CreatedAt] DEFAULT (sysutcdatetime()),
        [UpdatedAt]   DATETIME2(0)  NULL,
        CONSTRAINT [PK_LocationAttributeTypes] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [UX_LocationAttributeTypes_Code] UNIQUE ([Code])
    );
    PRINT '  Created [system].[LocationAttributeTypes].';
END
GO

-- LocationAttributes ----------------------------------------------------
IF NOT EXISTS (
    SELECT 1 FROM sys.tables t
    JOIN   sys.schemas s ON s.schema_id = t.schema_id
    WHERE  s.name = 'system' AND t.name = 'LocationAttributes'
)
BEGIN
    CREATE TABLE [system].[LocationAttributes] (
        [LocationAttributesUid] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_LocationAttributes_LocationAttributesUid] DEFAULT (newsequentialid()),
        [LocationId]            INT              NOT NULL,
        [AttributeTypeId]       INT              NOT NULL,
        [DecimalValue]          DECIMAL(18, 6)   NULL,
        [StringValue]           NVARCHAR(200)    NULL,
        [BoolValue]             BIT              NULL,
        [IntValue]              INT              NULL,
        [CreatedAt]             DATETIME2(0)     NOT NULL CONSTRAINT [DF_LocAttr_CreatedAt] DEFAULT (sysutcdatetime()),
        [UpdatedAt]             DATETIME2(0)     NULL,
        CONSTRAINT [PK_LocationAttributes] PRIMARY KEY CLUSTERED ([LocationAttributesUid]),
        CONSTRAINT [FK_LocationAttributes_Locations]
            FOREIGN KEY ([LocationId]) REFERENCES [system].[Locations] ([LocationId]),
        CONSTRAINT [FK_LocAttr_AttributeType]
            FOREIGN KEY ([AttributeTypeId]) REFERENCES [system].[LocationAttributeTypes] ([Id])
    );

    -- Same-row uniqueness so a Location only gets one row per type.
    CREATE UNIQUE INDEX [UX_LocationAttributes_LocationAndType]
        ON [system].[LocationAttributes] ([LocationId], [AttributeTypeId]);

    -- Cover the lookup that fires every page render of the Locations
    -- admin grid.
    CREATE INDEX [IX_LocationAttributes_LocationId]
        ON [system].[LocationAttributes] ([LocationId]);

    PRINT '  Created [system].[LocationAttributes].';
END
GO

-- Seed LocationAttributeType: REQUIRE_DUMP_TYPE ------------------------
IF NOT EXISTS (
    SELECT 1 FROM [system].[LocationAttributeTypes] WHERE [Code] = 'REQUIRE_DUMP_TYPE'
)
BEGIN
    INSERT INTO [system].[LocationAttributeTypes] ([Code], [Description], [DataType], [IsActive])
    VALUES ('REQUIRE_DUMP_TYPE', 'Require Dump Type prompt on each load', 'bool', 1);
    PRINT '  Seeded LocationAttributeTypes row REQUIRE_DUMP_TYPE.';
END
GO

-- Seed TransactionAttributeType: IS_END_DUMP ---------------------------
IF NOT EXISTS (
    SELECT 1 FROM [Inventory].[TransactionAttributeTypes] WHERE [Code] = 'IS_END_DUMP'
)
BEGIN
    INSERT INTO [Inventory].[TransactionAttributeTypes] ([Code], [Description], [DataType], [IsActive])
    VALUES ('IS_END_DUMP', 'Operator marked the load as an end dump', 'bool', 1);
    PRINT '  Seeded TransactionAttributeTypes row IS_END_DUMP.';
END
GO

PRINT 'AddLocationAttributes.sql applied successfully.';
GO
