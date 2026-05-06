/* =====================================================================
   WidenAuditTrailAction.sql

   The system.AuditTrail.Action column is varchar(10) on legacy installs;
   newer audit codes (SET_PENDING, MOVE_LOAD, etc.) are longer than that
   and get truncated, throwing 2628 errors on insert. Widens the column
   to varchar(40) to give plenty of headroom for future codes.

   Idempotent — only alters when the current size is < 40.
   ===================================================================== */

SET NOCOUNT ON;
GO

DECLARE @len INT = (
    SELECT max_length
    FROM   sys.columns
    WHERE  object_id = OBJECT_ID('system.AuditTrail')
      AND  name = 'Action'
);

IF @len IS NOT NULL AND @len < 40
BEGIN
    DECLARE @nullable BIT = (
        SELECT is_nullable
        FROM   sys.columns
        WHERE  object_id = OBJECT_ID('system.AuditTrail')
          AND  name = 'Action'
    );

    IF @nullable = 0
        ALTER TABLE [system].[AuditTrail] ALTER COLUMN [Action] VARCHAR(40) NOT NULL;
    ELSE
        ALTER TABLE [system].[AuditTrail] ALTER COLUMN [Action] VARCHAR(40) NULL;
END
GO

PRINT 'WidenAuditTrailAction.sql applied successfully.';
GO
