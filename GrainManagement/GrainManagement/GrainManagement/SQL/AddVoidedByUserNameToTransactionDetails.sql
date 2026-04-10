/*  ================================================================
    AddVoidedByUserNameToTransactionDetails.sql
    Add VoidedByUserName column to InventoryTransactionDetails
    to track who voided a delivery ticket.
    ================================================================ */

IF NOT EXISTS (
    SELECT 1
    FROM   sys.columns
    WHERE  object_id = OBJECT_ID(N'[Inventory].[InventoryTransactionDetails]')
      AND  name = N'VoidedByUserName'
)
BEGIN
    ALTER TABLE [Inventory].[InventoryTransactionDetails]
        ADD [VoidedByUserName] NVARCHAR(256) NULL;
END
GO
