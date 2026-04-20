-- LandLordSplitPercentages.sql
--
-- Returns one row per account within each landlord split group from COMDATA.U5SPLTS.
--
-- PrimaryGrower flag:
--   A split group's "primary grower" is the detail row with SPDEL = 'G'.
--   A split group may legitimately have NO row flagged 'G' — in that case every
--   row for the group is returned with PrimaryGrower = 0 and the downstream
--   upsert (SplitGroupsUpserter) will write a NULL account.SplitGroups.PrimaryAccountId
--   for that group. Groups without a 'G' row are still synced (description and
--   percents), only the PrimaryAccountId is left null.
--
-- The header row (SPCNO = 0) is joined only to provide the group description
-- (SPCNM) and is not returned as a percent row.
SELECT
    d.spspgp        AS SplitGroupId,
    d.spcno         AS As400AccountId,
    h.SPCNM         AS SplitGroupDescription,
    CASE
        WHEN d.spdel = 'G' THEN 1
        ELSE 0
    END             AS PrimaryGrower,
    d.SPSPPC        AS SplitPercentage
FROM COMDATA.U5SPLTS d
JOIN COMDATA.U5SPLTS h
    ON h.spspgp = d.spspgp
   AND h.spcno  = 0
WHERE
    d.spcno > 0
    AND TRIM(d.SPCNM) <> ''
    AND TRIM(h.SPCNM) <> ''
ORDER BY
    h.SPCNM, PrimaryGrower DESC
