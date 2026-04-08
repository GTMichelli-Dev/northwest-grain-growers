-- Creates the LocationQuantityMethods join table to configure which
-- QuantityMethods (TRUCK_SCALE, BULKLOAD, RAIL, MANUAL) are available per location.
-- NOT FOR REPLICATION — this table is managed locally at each deployment.

IF NOT EXISTS (SELECT 1 FROM sys.tables t
               JOIN sys.schemas s ON t.schema_id = s.schema_id
               WHERE s.name = 'Inventory' AND t.name = 'LocationQuantityMethods')
BEGIN
    CREATE TABLE [Inventory].[LocationQuantityMethods] (
        [LocationId]        INT NOT NULL,
        [QuantityMethodId]  INT NOT NULL,
        CONSTRAINT [PK_LocationQuantityMethods]
            PRIMARY KEY CLUSTERED ([LocationId], [QuantityMethodId]),
        CONSTRAINT [FK_LQM_Location]
            FOREIGN KEY ([LocationId]) REFERENCES [system].[Locations] ([LocationId])
            NOT FOR REPLICATION,
        CONSTRAINT [FK_LQM_QuantityMethod]
            FOREIGN KEY ([QuantityMethodId]) REFERENCES [Inventory].[QuantityMethods] ([QuantityMethodId])
            NOT FOR REPLICATION
    );
END
GO

-- Seed all existing locations with MANUAL and TRUCK_SCALE
INSERT INTO [Inventory].[LocationQuantityMethods] ([LocationId], [QuantityMethodId])
SELECT L.LocationId, QM.QuantityMethodId
FROM [system].[Locations] L
CROSS JOIN [Inventory].[QuantityMethods] QM
WHERE QM.Code IN ('MANUAL', 'TRUCK_SCALE')
  AND QM.IsActive = 1
  AND NOT EXISTS (
      SELECT 1 FROM [Inventory].[LocationQuantityMethods] LQM
      WHERE LQM.LocationId = L.LocationId AND LQM.QuantityMethodId = QM.QuantityMethodId
  );
GO
