SELECT
    d.spspgp        AS SplitGroupId,
    d.spcno         AS As400AccountId,
    h.SPCNM         AS SplitGroupDescription,
--     d.SPCNM         AS AccountDescription,
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
--     AND d.spdel = 'G'
    AND TRIM(d.SPCNM) <> ''
    AND TRIM(h.SPCNM) <> ''
ORDER BY
    h.SPCNM,PrimaryGrower desc
