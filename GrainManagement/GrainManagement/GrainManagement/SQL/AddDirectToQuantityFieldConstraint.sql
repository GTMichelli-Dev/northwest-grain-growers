/*  ================================================================
    AddDirectToQuantityFieldConstraint.sql
    Allow 'DIRECT' as a valid QuantityField value for direct
    quantity (non-truck) loads.
    ================================================================ */

ALTER TABLE [Inventory].[TransactionQuantitySources]
    DROP CONSTRAINT [CK_TxnQtySources_QuantityField];
GO

ALTER TABLE [Inventory].[TransactionQuantitySources]
    ADD CONSTRAINT [CK_TxnQtySources_QuantityField] CHECK (
        [QuantityField] IN ('START', 'END', 'DIRECT')
    );
GO
