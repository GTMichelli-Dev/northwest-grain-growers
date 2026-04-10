/*  ================================================================
    AddRowUidToInventoryTransactions.sql
    Add RowUid column to InventoryTransactions so we can retrieve
    the trigger-generated TransactionId after insert (same pattern
    as Lots and WeightSheets).
    ================================================================ */

IF NOT EXISTS (
    SELECT 1
    FROM   sys.columns
    WHERE  object_id = OBJECT_ID(N'[Inventory].[InventoryTransactions]')
      AND  name = N'RowUid'
)
BEGIN
    ALTER TABLE [Inventory].[InventoryTransactions]
        ADD [RowUid] UNIQUEIDENTIFIER NULL;
END
GO
