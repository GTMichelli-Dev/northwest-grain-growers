/*  ================================================================
    MigrateContainerSplits.sql
    Replace single FromContainerId / ToContainerId on
    [Inventory].[InventoryTransactionDetails] with child split tables.
    ================================================================ */

SET XACT_ABORT ON;
BEGIN TRANSACTION;

/* ── Phase A: Create new tables ───────────────────────────────── */

CREATE TABLE [Inventory].[InventoryTransactionDetailFromContainers]
(
    TransactionId   BIGINT          NOT NULL,
    ContainerId     BIGINT          NOT NULL,
    [Percent]       DECIMAL(10, 7)  NOT NULL,

    CONSTRAINT PK_InventoryTransactionDetailFromContainers
        PRIMARY KEY CLUSTERED (TransactionId, ContainerId),

    CONSTRAINT FK_ITDFromContainers_TransactionDetail
        FOREIGN KEY (TransactionId)
        REFERENCES [Inventory].[InventoryTransactionDetails] (TransactionId),

    CONSTRAINT FK_ITDFromContainers_Container
        FOREIGN KEY (ContainerId)
        REFERENCES [container].[Containers] (ContainerId),

    CONSTRAINT CK_ITDFromContainers_Percent
        CHECK ([Percent] > 0 AND [Percent] <= 100)
);

CREATE NONCLUSTERED INDEX IX_ITDFromContainers_ContainerId
    ON [Inventory].[InventoryTransactionDetailFromContainers] (ContainerId);

CREATE TABLE [Inventory].[InventoryTransactionDetailToContainers]
(
    TransactionId   BIGINT          NOT NULL,
    ContainerId     BIGINT          NOT NULL,
    [Percent]       DECIMAL(10, 7)  NOT NULL,

    CONSTRAINT PK_InventoryTransactionDetailToContainers
        PRIMARY KEY CLUSTERED (TransactionId, ContainerId),

    CONSTRAINT FK_ITDToContainers_TransactionDetail
        FOREIGN KEY (TransactionId)
        REFERENCES [Inventory].[InventoryTransactionDetails] (TransactionId),

    CONSTRAINT FK_ITDToContainers_Container
        FOREIGN KEY (ContainerId)
        REFERENCES [container].[Containers] (ContainerId),

    CONSTRAINT CK_ITDToContainers_Percent
        CHECK ([Percent] > 0 AND [Percent] <= 100)
);

CREATE NONCLUSTERED INDEX IX_ITDToContainers_ContainerId
    ON [Inventory].[InventoryTransactionDetailToContainers] (ContainerId);


/* ── Phase B: Migrate existing data ───────────────────────────── */

INSERT INTO [Inventory].[InventoryTransactionDetailFromContainers]
       (TransactionId, ContainerId, [Percent])
SELECT  TransactionId, FromContainerId, 100.0000000
FROM    [Inventory].[InventoryTransactionDetails]
WHERE   FromContainerId IS NOT NULL;

INSERT INTO [Inventory].[InventoryTransactionDetailToContainers]
       (TransactionId, ContainerId, [Percent])
SELECT  TransactionId, ToContainerId, 100.0000000
FROM    [Inventory].[InventoryTransactionDetails]
WHERE   ToContainerId IS NOT NULL;


/* ── Phase C: Drop old columns and constraints ────────────────── */

ALTER TABLE [Inventory].[InventoryTransactionDetails]
    DROP CONSTRAINT FK_InventoryTransactionDetails_Containers1;   -- FromContainerId

ALTER TABLE [Inventory].[InventoryTransactionDetails]
    DROP CONSTRAINT FK_InventoryTransactionDetails_Containers;    -- ToContainerId

ALTER TABLE [Inventory].[InventoryTransactionDetails]
    DROP COLUMN FromContainerId;

ALTER TABLE [Inventory].[InventoryTransactionDetails]
    DROP COLUMN ToContainerId;


COMMIT TRANSACTION;
GO
