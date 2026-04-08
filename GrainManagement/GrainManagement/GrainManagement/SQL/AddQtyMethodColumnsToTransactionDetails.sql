/*  ================================================================
    AddQtyMethodColumnsToTransactionDetails.sql
    Add LocationQuantityMethodId and description columns for each
    quantity field (Start, End, Direct) on InventoryTransactionDetails.
    ================================================================ */

ALTER TABLE [Inventory].[InventoryTransactionDetails]
    ADD [StartQtyLocationQuantityMethodId]          INT           NULL,
        [StartQtyLocationQuantityMethodDescription] NVARCHAR(200) NULL,
        [EndQtyLocationQuantityMethodId]            INT           NULL,
        [EndQtyLocationQuantityMethodDescription]   NVARCHAR(200) NULL,
        [DirectQtyLocationQuantityMethodId]         INT           NULL,
        [DirectQtyLocationQuantityMethodDescription] NVARCHAR(200) NULL;
GO

ALTER TABLE [Inventory].[InventoryTransactionDetails]
    ADD CONSTRAINT [FK_InventoryTxn_StartQtyMethod]
        FOREIGN KEY ([StartQtyLocationQuantityMethodId])
        REFERENCES [Inventory].[QuantityMethods] ([QuantityMethodId]);

ALTER TABLE [Inventory].[InventoryTransactionDetails]
    ADD CONSTRAINT [FK_InventoryTxn_EndQtyMethod]
        FOREIGN KEY ([EndQtyLocationQuantityMethodId])
        REFERENCES [Inventory].[QuantityMethods] ([QuantityMethodId]);

ALTER TABLE [Inventory].[InventoryTransactionDetails]
    ADD CONSTRAINT [FK_InventoryTxn_DirectQtyMethod]
        FOREIGN KEY ([DirectQtyLocationQuantityMethodId])
        REFERENCES [Inventory].[QuantityMethods] ([QuantityMethodId]);
GO
