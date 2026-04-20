-- SplitGroups_AllowNullPrimaryAccountId.sql
--
-- Allow account.SplitGroups.PrimaryAccountId to be NULL.
--
-- A SplitGroup may legitimately have no primary grower when no detail row
-- in COMDATA.U5SPLTS (AS400) is flagged SPDEL = 'G'. The AS400 sync
-- (GrainManagement.As400Sync / LandLordSplitPercentages.sql /
--  SplitGroupsUpserter) will write NULL in that case instead of inventing
-- a fallback account. The FK to account.Accounts is kept; rows with a
-- NULL PrimaryAccountId simply do not participate in the FK.
--
-- Safe to re-run: every step is guarded.

SET NOCOUNT ON;
BEGIN TRAN;

-- 1) Drop the existing FK so we can alter the column nullability.
IF EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_SplitGroups_Accounts'
      AND parent_object_id = OBJECT_ID('account.SplitGroups')
)
BEGIN
    ALTER TABLE account.SplitGroups
        DROP CONSTRAINT FK_SplitGroups_Accounts;
END;

-- 2) Make PrimaryAccountId NULLable.
IF EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID('account.SplitGroups')
      AND name = 'PrimaryAccountId'
      AND is_nullable = 0
)
BEGIN
    ALTER TABLE account.SplitGroups
        ALTER COLUMN PrimaryAccountId bigint NULL;
END;

-- 3) Re-create the FK (nullable FKs do not enforce the relationship on NULL rows).
IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_SplitGroups_Accounts'
      AND parent_object_id = OBJECT_ID('account.SplitGroups')
)
BEGIN
    ALTER TABLE account.SplitGroups
        ADD CONSTRAINT FK_SplitGroups_Accounts
        FOREIGN KEY (PrimaryAccountId)
        REFERENCES account.Accounts (AccountId);
END;

COMMIT TRAN;
