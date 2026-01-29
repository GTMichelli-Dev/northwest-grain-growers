/*
   Staging table used by the updated GrainManagement.As400Sync worker.

   The worker loads the AS400 A/R Item Master (U4ITMMR) into this table as a snapshot.
*/

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'stg')
BEGIN
    EXEC('CREATE SCHEMA stg');
END
GO

IF OBJECT_ID('stg.As400ItemMaster', 'U') IS NULL
BEGIN
    CREATE TABLE stg.As400ItemMaster
    (
        ItemNumber      bigint          NOT NULL,
        ItemDescription nvarchar(200)   NULL,
        DeptNumber      nvarchar(10)    NULL,
        FineLineCode    nvarchar(10)    NULL,
        PackageUom      nvarchar(50)    NULL,
        Category        nvarchar(50)    NULL,
        ItemType        nvarchar(50)    NULL,
        InactiveCode    nvarchar(10)    NULL,
        QtyPerUnit      decimal(18,6)   NULL,
        SourceSystem    nvarchar(50)    NOT NULL,
        ExtractedAt     datetime2(7)    NOT NULL,

        CONSTRAINT PK_stg_As400ItemMaster PRIMARY KEY (ItemNumber)
    );
END
GO
