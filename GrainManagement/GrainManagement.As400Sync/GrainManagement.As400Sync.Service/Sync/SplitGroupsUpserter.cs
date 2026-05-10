using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GrainManagement.As400Sync;

public sealed class SplitGroupsUpserter
{
    private readonly string _sqlConnStr;

    public SplitGroupsUpserter(IConfiguration cfg)
    {
        _sqlConnStr = cfg.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Missing ConnectionStrings:SqlServer");
    }

    /// <summary>
    /// Sync SplitGroups + SplitGroupPercents from AS400 (source of truth).
    ///
    /// Default behavior is "batch safe": it will only delete missing percents within groups present
    /// in the supplied AS400 rows.
    ///
    /// If <paramref name="isFullSnapshot"/> is true, the sync will HARD DELETE any SplitGroups/
    /// SplitGroupPercents not present in the supplied rows.
    /// Only set this to true if your AS400 query returns a complete snapshot.
    /// </summary>
    public async Task UpsertAsync(IReadOnlyList<As400LandlordSplitPercentRow> rows, CancellationToken ct, bool isFullSnapshot = true)
    {
        if (rows is null) throw new ArgumentNullException(nameof(rows));
        if (rows.Count == 0) return;

        await using var conn = new SqlConnection(_sqlConnStr);
        await conn.OpenAsync(ct);

        await using var tx = await conn.BeginTransactionAsync(ct);

        try
        {
            // Temp table mirrors the AS400 rows (LandlordSplitId == SplitGroupId in SQL)
            const string createTempSql = @"
SET NOCOUNT ON;

IF OBJECT_ID('tempdb..#Src') IS NOT NULL DROP TABLE #Src;

CREATE TABLE #Src
(
    SplitGroupId     int            NOT NULL,
    As400AccountId   bigint         NOT NULL,
    SplitGroupDescription nvarchar(200)  NOT NULL,
    IsPrimaryGrower  bit            NOT NULL,
    SplitPercent     decimal(10,7)  NOT NULL
);";

            await using (var cmd = new SqlCommand(createTempSql, conn, (SqlTransaction)tx))
            {
                cmd.CommandTimeout = 0;
                await cmd.ExecuteNonQueryAsync(ct);
            }

            // BulkCopy
            var table = new DataTable();
            table.Columns.Add("SplitGroupId", typeof(int));
            table.Columns.Add("As400AccountId", typeof(long));
            table.Columns.Add("SplitGroupDescription", typeof(string));
            table.Columns.Add("IsPrimaryGrower", typeof(bool));
            table.Columns.Add("SplitPercent", typeof(decimal));

            foreach (var r in rows)
            {
                if (r.SplitGroupId == 3133330)
                {
                    Console.WriteLine("Found It");
                }
                var desc = string.IsNullOrWhiteSpace(r.Description) ? "" : r.Description.Trim();
                table.Rows.Add(
                    r.SplitGroupId,   // AS400 LandlordSplitId becomes SQL SplitGroupId
                    r.As400AccountId,
                    desc,
                    r.IsPrimaryGrower,
                    r.SplitPercent
                );
            }

            using (var bulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, (SqlTransaction)tx))
            {
                bulk.DestinationTableName = "#Src";
                bulk.BatchSize = 5000;
                bulk.BulkCopyTimeout = 0;

                bulk.ColumnMappings.Add("SplitGroupId", "SplitGroupId");
                bulk.ColumnMappings.Add("As400AccountId", "As400AccountId");
                bulk.ColumnMappings.Add("SplitGroupDescription", "SplitGroupDescription");
                bulk.ColumnMappings.Add("IsPrimaryGrower", "IsPrimaryGrower");
                bulk.ColumnMappings.Add("SplitPercent", "SplitPercent");

                await bulk.WriteToServerAsync(table, ct);
            }

            // Sync: AS400 is source of truth
            const string syncSql = @"
SET NOCOUNT ON;

--------------------------------------------------------------------------------
-- Map AS400 AccountId -> SQL AccountId
--------------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#SrcMapped') IS NOT NULL DROP TABLE #SrcMapped;

SELECT
    s.SplitGroupId,
    a.AccountId        AS SqlAccountId,
    s.As400AccountId,
    s.SplitGroupDescription,
    s.IsPrimaryGrower,
    s.SplitPercent
INTO #SrcMapped
FROM #Src s
LEFT JOIN account.Accounts a
  ON a.As400AccountId = s.As400AccountId;

--------------------------------------------------------------------------------
-- Drop any unmapped rows (Account not yet in SQL). Do NOT delete whole groups.
--------------------------------------------------------------------------------
DELETE FROM #SrcMapped
WHERE SqlAccountId IS NULL;

--------------------------------------------------------------------------------
-- VALID rows only
--------------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#ValidRows') IS NOT NULL DROP TABLE #ValidRows;

SELECT *
INTO #ValidRows
FROM #SrcMapped
WHERE SqlAccountId IS NOT NULL;
--------------------------------------------------------------------------------
-- Reject bad groups (hard delete in SQL + remove from this batch)
-- Rules
            --1) Total SplitPercent per SplitGroupId must equal 100.0000000(within tolerance)
            --------------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#BadGroups') IS NOT NULL DROP TABLE #BadGroups;

SELECT
    v.SplitGroupId
INTO #BadGroups
FROM #ValidRows v
GROUP BY v.SplitGroupId
HAVING
    ABS(SUM(v.SplitPercent) -CAST(100.0 AS decimal(18, 7))) > CAST(0.00001 AS decimal(18, 7));

            --Hard delete invalid groups in SQL(FK - safe)
DELETE sp
FROM account.SplitGroupPercents sp
JOIN #BadGroups bg ON bg.SplitGroupId = sp.SplitGroupId;

DELETE sg
FROM account.SplitGroups sg
JOIN #BadGroups bg ON bg.SplitGroupId = sg.SplitGroupId;

-- Remove bad groups from this batch so we don't re-insert them
DELETE v
FROM #ValidRows v
JOIN #BadGroups bg ON bg.SplitGroupId = v.SplitGroupId;


--------------------------------------------------------------------------------
-- Build one row per SplitGroupId for SplitGroups upsert
-- PrimaryAccountId = IsPrimaryGrower ('G') row if present, otherwise NULL.
-- A SplitGroup is allowed to have a NULL PrimaryAccountId when no detail row
-- in U5SPLTS is flagged with SPDEL = 'G'.
--------------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#SrcGroups') IS NOT NULL DROP TABLE #SrcGroups;

; WITH PrimaryPick AS
(
    SELECT
        SplitGroupId,
        MAX(CASE WHEN IsPrimaryGrower = 1 THEN SqlAccountId END) AS PrimaryAccountId
    FROM #ValidRows
    GROUP BY SplitGroupId
)
SELECT
    v.SplitGroupId,
    MAX(COALESCE(NULLIF(LTRIM(RTRIM(v.SplitGroupDescription)), ''), '')) AS SplitGroupDescription,
    p.PrimaryAccountId AS PrimaryAccountId,
    CAST(CASE
        WHEN UPPER(MAX(COALESCE(NULLIF(LTRIM(RTRIM(v.SplitGroupDescription)), ''), ''))) LIKE '%DO NOT USE%'
        THEN 0 ELSE 1
    END AS bit) AS IsActive
INTO #SrcGroups
FROM #ValidRows v
JOIN PrimaryPick p ON p.SplitGroupId = v.SplitGroupId
GROUP BY v.SplitGroupId, p.PrimaryAccountId;

            --------------------------------------------------------------------------------
            --Upsert SplitGroups(keyed by SplitGroupId; there is NO Id column)
--------------------------------------------------------------------------------
MERGE account.SplitGroups WITH(HOLDLOCK) AS tgt
USING #SrcGroups AS src
   ON tgt.SplitGroupId = src.SplitGroupId
WHEN MATCHED AND
(
       (tgt.PrimaryAccountId IS NULL AND src.PrimaryAccountId IS NOT NULL)
    OR (tgt.PrimaryAccountId IS NOT NULL AND src.PrimaryAccountId IS NULL)
    OR (tgt.PrimaryAccountId <> src.PrimaryAccountId)
    OR tgt.SplitGroupDescription <> src.SplitGroupDescription
    OR tgt.IsActive <> src.IsActive
)
THEN UPDATE SET
    tgt.PrimaryAccountId = src.PrimaryAccountId,
    tgt.SplitGroupDescription = src.SplitGroupDescription,
    tgt.IsActive = src.IsActive
WHEN NOT MATCHED BY TARGET THEN
    INSERT(SplitGroupId, PrimaryAccountId, SplitGroupDescription, IsActive, UseForSales, UseForReceive)
    VALUES(src.SplitGroupId, src.PrimaryAccountId, src.SplitGroupDescription, src.IsActive, 1, 1);

            --------------------------------------------------------------------------------
            --Sync percents(mirror AS400): update / insert / delete missing(scoped)
            --------------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#SrcPerc') IS NOT NULL DROP TABLE #SrcPerc;

SELECT
    v.SplitGroupId,
    v.SqlAccountId AS AccountId,
    v.SplitPercent
INTO #SrcPerc
FROM #ValidRows v;


MERGE account.SplitGroupPercents WITH(HOLDLOCK) AS tgt
USING #SrcPerc AS src
   ON tgt.SplitGroupId = src.SplitGroupId
  AND tgt.AccountId = src.AccountId
WHEN MATCHED AND tgt.SplitPercent<> src.SplitPercent THEN
    UPDATE SET tgt.SplitPercent = src.SplitPercent
WHEN NOT MATCHED BY TARGET THEN
    INSERT(SplitGroupId, AccountId, SplitPercent)
    VALUES(src.SplitGroupId, src.AccountId, src.SplitPercent)
WHEN NOT MATCHED BY SOURCE
     AND tgt.SplitGroupId IN(SELECT SplitGroupId FROM #SrcGroups) THEN
    DELETE;

            --------------------------------------------------------------------------------
            --Optional FULL SNAPSHOT purge
            -- Only enable when the supplied AS400 rows are a complete snapshot.
            --------------------------------------------------------------------------------
IF @IsFullSnapshot = 1
BEGIN
    -- delete children first(FK - safe)
    DELETE sp
    FROM account.SplitGroupPercents sp
    WHERE NOT EXISTS(
        SELECT 1
        FROM #SrcGroups s
        WHERE s.SplitGroupId = sp.SplitGroupId
    );

            DELETE sg
    FROM account.SplitGroups sg
    WHERE NOT EXISTS(
        SELECT 1
        FROM #SrcGroups s
        WHERE s.SplitGroupId = sg.SplitGroupId
    );
            END
";

            await using (var cmd = new SqlCommand(syncSql, conn, (SqlTransaction)tx))
            {
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@IsFullSnapshot", SqlDbType.Bit).Value = isFullSnapshot;
                await cmd.ExecuteNonQueryAsync(ct);
            }

            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }
}