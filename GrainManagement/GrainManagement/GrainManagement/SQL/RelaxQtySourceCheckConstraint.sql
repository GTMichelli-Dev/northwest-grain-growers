/*  ================================================================
    RelaxQtySourceCheckConstraint.sql
    Allow truck loads to be saved with only StartQty (inbound)
    before EndQty (outbound) has been captured.
    ================================================================ */

ALTER TABLE [Inventory].[InventoryTransactionDetails]
    DROP CONSTRAINT [CK_InventoryTxn_QtySource];
GO

ALTER TABLE [Inventory].[InventoryTransactionDetails]
    ADD CONSTRAINT [CK_InventoryTxn_QtySource] CHECK (
        ([StartQty] IS NOT NULL AND [DirectQty] IS NULL)
        OR
        ([DirectQty] IS NOT NULL AND [StartQty] IS NULL AND [EndQty] IS NULL)
    );
GO
