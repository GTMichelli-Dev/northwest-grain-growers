WITH
/* ============================================================
   1) U5CROPS -> lookup: GROUPCODEMAP => CROPID
      (Seed wins: if any (SEED) exists for a GROUPCODEMAP,
       only keep the (SEED) rows for that GROUPCODEMAP)
   ============================================================ */
crop_base AS
(
    SELECT
        CRCROP AS CROPID,

        /* base code (no seed suffix) */
        (CASE
            WHEN CRCROP IN (90,38)     THEN 'GPEA'
            WHEN CRCROP IN (39,46)     THEN 'PEA'
            WHEN CRCROP IN (85,42)     THEN 'YPEA'
            WHEN CRCROP IN (7)         THEN 'MUS'
            WHEN CRCROP IN (54)        THEN 'GARBS'
            WHEN CRCROP IN (34)        THEN 'MIXGRN'
            WHEN CRCROP IN (50)        THEN 'LENT'
            WHEN CRCROP IN (10,11,12)  THEN 'HRW'
            WHEN CRCROP IN (89,26)     THEN 'OAT'
            WHEN CRCROP IN (30,31)     THEN 'CORN'
            WHEN LOCATE(' ', CRLCDE) = 0 THEN TRIM(CRLCDE)
            ELSE SUBSTR(CRLCDE, 1, LOCATE(' ', CRLCDE) - 1)
        END) AS GROUPCODEMAP,

        /* full productcode (may include seed suffix) */
        ((CASE
            WHEN CRCROP IN (90,38)     THEN 'GPEA'
            WHEN CRCROP IN (39,46)     THEN 'PEA'
            WHEN CRCROP IN (85,42)     THEN 'YPEA'
            WHEN CRCROP IN (7)         THEN 'MUS'
            WHEN CRCROP IN (54)        THEN 'GARBS'
            WHEN CRCROP IN (34)        THEN 'MIXGRN'
            WHEN CRCROP IN (50)        THEN 'LENT'
            WHEN CRCROP IN (10,11,12)  THEN 'HRW'
            WHEN CRCROP IN (89,26)     THEN 'OAT'
            WHEN CRCROP IN (30,31)     THEN 'CORN'
            WHEN LOCATE(' ', CRLCDE) = 0 THEN TRIM(CRLCDE)
            ELSE SUBSTR(CRLCDE, 1, LOCATE(' ', CRLCDE) - 1)
        END)
        ||
        CASE
            WHEN CRCROP IN (82,84,86,87,88,85,89,90,80,70) THEN ' (SEED)'
            ELSE ''
        END) AS PRODUCTCODE
    FROM COMDATA.U5CROPS
),
crop_flagged AS
(
    SELECT
        b.*,
        MAX(CASE WHEN b.PRODUCTCODE LIKE '%(SEED)%' THEN 1 ELSE 0 END)
            OVER (PARTITION BY b.GROUPCODEMAP) AS HasSeedInGroup
    FROM crop_base b
),
crop_filtered AS
(
    SELECT *
    FROM crop_flagged
    WHERE HasSeedInGroup = 0
       OR PRODUCTCODE LIKE '%(SEED)%'
),
crop_lookup AS
(
    /* one row per GROUPCODEMAP -> choose deterministic CROPID */
    SELECT
        GROUPCODEMAP,
        MIN(CROPID) AS CROPID
    FROM crop_filtered
    GROUP BY GROUPCODEMAP
),

/* ============================================================
   2) U4ITMMR -> your existing item cleanup pipeline
   ============================================================ */
base AS
(
    SELECT
        t.IMPROD ,
        t.IMDESC ,
        t.IMFLCD ,
        t.IMDEPT,
        t.IMDEL
    FROM COMDATA.U4ITMMR t
    WHERE
        t.IMDEPT NOT IN
        (
            1, 10, 12, 13, 14, 15, 17, 19,
            20, 27, 33,
            50, 60, 61, 62, 63, 64, 65, 66,
            69, 70,
            91, 92, 93, 95, 97, 99
        )
        AND t.IMPROD IS NOT NULL
        AND LENGTH(TRIM(t.IMPROD)) > 0
        AND t.IMPROD <> 290

        /* digits-only IMPROD (portable DB2/i) */
        AND LENGTH(
            REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
            REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
                TRIM(t.IMPROD),
                '0',''),
                '1',''),
                '2',''),
                '3',''),
                '4',''),
                '5',''),
                '6',''),
                '7',''),
                '8',''),
                '9','')
        ) = 0

        /* filter out non-item / accounting rows */
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE 'SALES TAX%'
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE 'TAX DISCOUNT%'
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE 'CASH DISCOUNT%'
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE 'FINANCE CHARGE%'
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE 'TRANSACTION FEE%'
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE 'BALANCE FWD%'
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE 'DON''T USE%'
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE '*INACTIVE*%'
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE '%INACTIVE%'
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE '%PURCHASE%'
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE '%SALE%'
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE 'DO NOT USE%'
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE 'INACTIVE%'
        AND UPPER(TRIM(t.IMDESC)) NOT LIKE '%***DO NOT USE%'
        AND UPPER(TRIM(t.IMPROD)) NOT LIKE 777
        AND LENGTH(TRIM(t.IMDESC)) > 0
),
norm AS
(
    SELECT
        IMPROD,
        IMDESC,
        IMFLCD,
        IMDEPT,
        IMDEL,
        REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
            ' ' || UPPER(IMDESC) || ' ',
            ',', ' '),
            '.', ' '),
            '/', ' '),
            '-', ' '),
            '(', ' '),
            ')', ' ') AS D0
    FROM base
),
removed AS
(
    SELECT
        IMPROD,
        IMDESC,
        IMFLCD,
        IMDEPT,
        IMDEL,
        REPLACE(
          REPLACE(
            REPLACE(
              REPLACE(
                REPLACE(
                  REPLACE(
                  REPLACE(
                   REPLACE(
                    REPLACE(
                    REPLACE(
                     REPLACE(
                     REPLACE(
                     REPLACE(
                     REPLACE(
                     REPLACE(
                     REPLACE(
                     REPLACE(
                    REPLACE(D0,
                    ' DNS ', ' '),
                     ' GRB ', ' '),
                    ' HRW ', ' '),
                    ' SWH ', ' '),
                    ' SWW ', ' '),
                      ' BLY ', ' '),
                      ' WHC ', ' '),
                      ' REG ', ' '),
                      ' HWW ', ' '),
                      ' REG ', ' '),
                      ' CERT ', ' '),
                      ' FOUNDATION ', ' '),
                      ' REGISTERED ', ' '),
                      ' CERTIFIED ', ' '),
                      ' COMMON ', ' '),
                      ' "COMMON" ', ' '),
                      ' "REGISTARD" ', ' '),
                      ' SEED ', ' ') AS D1
    FROM norm
),
clean AS
(
    SELECT
        IMPROD,
        IMDESC,
        IMFLCD,
        IMDEPT,
        IMDEL,
        TRIM(
            REPLACE(
              REPLACE(
                REPLACE(
                  REPLACE(D1, '  ', ' '),
                             '  ', ' '),
                             '  ', ' '),
                             '  ', ' ')
        ) AS IMDESC_CLEAN
    FROM removed
),
items AS
(
    SELECT
        IMPROD,
        IMDESC,

        'SEED' AS SYSTEM_USAGE,

        CASE
            WHEN  UPPER(TRIM(IMDESC)) LIKE '%COAX%' THEN 'COAXIMUM'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '% AX %' THEN 'COAXIMUM'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '% CL+ %' THEN 'CLEARFIELD'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '% CLP %' THEN 'CLEARFIELD'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '% CL %' THEN 'CLEARFIELD'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '% CL2 %' THEN 'CLEARFIELD'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '% CF %' THEN 'CLEARFIELD'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '% CLEARFIELD %' THEN 'CLEARFIELD'
        END AS HERBICIDE_SYSTEM,

        CASE
            WHEN  UPPER(TRIM(IMDESC)) LIKE '% FALL %' THEN 'FALL'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '% FALL%' THEN 'FALL'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '%FALL %' THEN 'FALL'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '% WINTER %' THEN 'WINTER'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '% WINTER%' THEN 'WINTER'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '%WINTER %' THEN 'WINTER'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '% SPRING %' THEN 'SPRING'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '% SPRING%' THEN 'SPRING'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '%SPRING %' THEN 'SPRING'
        END AS SEASON,

        CASE
            WHEN ' ' || UPPER(TRIM(IMDESC)) || ' ' LIKE '% CRP %' THEN 'CRP'
            WHEN ' ' || UPPER(TRIM(IMDESC)) || ' ' LIKE '% CPR %' THEN 'CRP'
        END AS LAND_PROGRAM,

        CASE
            WHEN  UPPER(TRIM(IMDESC)) LIKE '%COMMON%' THEN 'COMMON'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '%REGISTARD%' THEN 'REGISTERED'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '%REGISTERED%' THEN 'REGISTERED'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '%CERTIFIED%' THEN 'CERTIFIED'
            WHEN  UPPER(TRIM(IMDESC)) LIKE '%FOUNDATION%' THEN 'FOUNDATION'
        END AS CERT_CLASS,

        CASE
            WHEN UPPER(TRIM(IMDESC)) LIKE '%CLEAN %' THEN 'CLEAN'
            WHEN UPPER(TRIM(IMDESC)) LIKE '% CLEAN%' THEN 'CLEAN'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%INDIRT%' THEN 'IN-DIRT'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%IN DIRT%' THEN 'IN-DIRT'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%IN_DIRT%' THEN 'IN-DIRT'
        END AS CONDITION,

        CASE WHEN IMDEL='I' THEN 0 ELSE 1 END AS ISACTIVE,

        CASE
            WHEN UPPER(TRIM(IMDESC)) LIKE '%CORN%' THEN 'CORN'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%ALFALFA%' THEN 'ALF'
            WHEN IMPROD IN (21081) THEN UPPER(TRIM(IMDESC))
            WHEN UPPER(TRIM(IMDESC)) LIKE '%SCREENINGS%' THEN 'SCRFEENINGS'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%CLOVER%' THEN 'CLOVER'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%BROME%' THEN 'BROME'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%WHEATGRASS%' THEN 'WHEATGRASS'
            
            WHEN UPPER(TRIM(IMDESC)) LIKE '%GREEN PEA%' THEN 'GPEA'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%RYEGRASS%' THEN 'RYEGRASS'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%SUNFLOWER%' THEN 'SUN'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%YELLOW PEA%' THEN 'YPEA'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%ORCHARD%' THEN 'ORCHARD'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%FESCUE%' THEN 'FESCUE'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%TRITICALE%' THEN 'TRIT'
            WHEN IMPROD IN (21080,21170) THEN 'CAN'
            WHEN IMPROD IN (22350) THEN 'BUCK'
            WHEN IMPROD IN (27375) THEN 'SORGHSUD'
            WHEN IMPROD IN (27418) THEN 'RADISH'
            WHEN IMPROD IN (346646) THEN 'MILLT'
            WHEN IMPROD IN (27340) THEN 'FORAGE'
            WHEN IMPROD IN (27380) THEN 'TIMO'
            WHEN IMPROD IN (29250) THEN 'INOC'
            WHEN IMPROD IN (27368) THEN 'RYE'
            WHEN IMPROD IN (50220) THEN 'BAG'
            WHEN IMPROD IN (4646) THEN 'PEA'
            WHEN IMPROD IN (50222) THEN 'PALLET'
            WHEN IMPROD IN (50221) THEN 'TOTE'
            WHEN IMPROD IN (384025) THEN 'LENT'
            WHEN IMPROD IN (366) THEN 'MUS'
            WHEN IMPROD IN (27370) THEN 'SUDAN'
            WHEN IMPROD IN (27377) THEN 'TURNIP'
            WHEN IMDEPT = 16 THEN 'PEA'
            WHEN IMDEPT = 18 THEN 'GARBS'
            WHEN IMDEPT IN (21,88,89) THEN 'BLY'
            WHEN IMDEPT = 22 THEN 'OAT'
            WHEN IMPROD IN (29300) THEN 'BIOST'
            WHEN IMPROD IN (823035) THEN 'COVER'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%CRUISER%' THEN 'CRUISER'
            WHEN IMDEPT = 90 THEN 'SPECIAL'
            WHEN IMDEPT IN (85,86) THEN 'WHC'
            WHEN IMDEPT IN (31,32) THEN 'GRASS'
            WHEN IMDEPT IN (71,72,73,77,82,83) THEN 'SWH'
            WHEN IMDEPT IN (74,76,84) THEN 'DNS'
            WHEN IMDEPT IN (75,80,81,87) THEN 'HRW'
            ELSE UPPER(TRIM(IMDESC))
        END AS PRODUCTCODE,

        CASE
            WHEN UPPER(TRIM(IMDESC)) LIKE '%SCREENINGS%' THEN 'SPECIAL'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%ALFALFA%' THEN 'GRASS'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%CLOVER%' THEN 'GRASS'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%BROME%' THEN 'GRASS'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%CORN%' THEN 'GRAIN'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%WHEATGRASS%' THEN 'GRASS'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%GREEN PEA%' THEN 'PULSES'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%RYEGRASS%' THEN 'GRASS'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%SUNFLOWER%' THEN 'OILSEED'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%YELLOW PEA%' THEN 'PULSES'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%ORCHARD%' THEN 'GRASS'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%FESCUE%' THEN 'GRASS'
            WHEN UPPER(TRIM(IMDESC)) LIKE '%TRITICALE%' THEN 'GRASS'
            
            WHEN IMPROD IN (29300) THEN 'CHEM'
            WHEN IMPROD IN (21081) THEN 'COVER'
            WHEN IMPROD IN (21080,21170) THEN 'OILSEED'
            WHEN IMPROD IN (22350) THEN 'COVER'
            WHEN IMPROD IN (20075) THEN 'GRASS'
            WHEN IMPROD IN (27375) THEN 'GRASS'
            WHEN IMPROD IN (27418) THEN 'COVER'
            WHEN IMPROD IN (346646) THEN 'GRASS'
            WHEN IMPROD IN (27340) THEN 'GRASS'
            WHEN IMPROD IN (27380) THEN 'GRASS'
            WHEN IMPROD IN (29100,29120) THEN 'FERT'
            WHEN IMPROD IN (29250) THEN 'FERT'
            WHEN IMPROD IN (27368) THEN 'GRAIN'
            WHEN IMPROD IN (50220) THEN 'PACK'
            WHEN IMPROD IN (4646) THEN 'GRAIN'
            WHEN IMPROD IN (50222) THEN 'PACK'
            WHEN IMPROD IN (50221) THEN 'PACK'
            WHEN IMPROD IN (384025) THEN 'PULSES'
            WHEN IMPROD IN (366) THEN 'OILSEED'
            WHEN IMPROD IN (27370) THEN 'GRASS'
            WHEN IMPROD IN (27377) THEN 'COVER'
            WHEN IMDEPT = 16 THEN 'PULSES'
            WHEN IMDEPT = 18 THEN 'PULSES'
            WHEN IMDEPT IN (21,88,89) THEN 'GRAIN'
            WHEN IMDEPT = 22 THEN 'GRAIN'
            WHEN IMPROD IN (823035) THEN 'COVER'
--             WHEN IMDEPT = 90 THEN 'MISC'
            WHEN IMDEPT IN (85,86) THEN 'GRAIN'
            WHEN IMDEPT IN (28,29) THEN 'CHEM'
            WHEN IMDEPT IN (31,32) THEN 'GRASS'
            WHEN IMDEPT IN (71,72,73,77,82,83) THEN 'GRAIN'
            WHEN IMDEPT IN (74,76,84) THEN 'GRAIN'
            WHEN IMDEPT IN (75,80,81,87) THEN 'GRAIN'
        END AS CATAGORY
        
        
  
    FROM clean
)

/* ============================================================
   3) Final: join items -> crop_lookup and output cl.CROPID AS CROPID
   ============================================================ */
SELECT 
    i.IMPROD,
    i.IMDESC,
    
    i.SYSTEM_USAGE,
    i.HERBICIDE_SYSTEM,
    i.SEASON,
    i.LAND_PROGRAM,
    i.CERT_CLASS,
    i.CONDITION,
    i.ISACTIVE,
    cl.CROPID AS CROPID,
    i.PRODUCTCODE,
    i.CATAGORY,
          
   CASE
        WHEN UPPER(TRIM(i.IMDESC)) LIKE '%PEA%' THEN 'PEA'
        WHEN UPPER(TRIM(i.IMDESC)) LIKE '%OAT%' THEN 'OAT'
        WHEN UPPER(TRIM(i.IMDESC)) LIKE '%CORN%' THEN 'CORN'
        WHEN UPPER(TRIM(i.IMDESC)) LIKE '%BARLEY%' THEN 'BARLEY'
        WHEN UPPER(TRIM(i.PRODUCTCODE)) LIKE '%BLY%' THEN 'BARLEY'
        WHEN UPPER(TRIM(i.PRODUCTCODE)) IN ('WHC','SWH','HRW','DNS') THEN 'WHEAT' 
        
    
--         WHEN IMDEPT IN (5,10,11,12,14,15,2,3,58,59,6,62,80,82,84,86,87) THEN 'WHEAT'
 
    END AS PRODUCTGROUP
FROM items i
LEFT JOIN crop_lookup cl
    ON cl.GROUPCODEMAP = i.PRODUCTCODE

ORDER BY i.PRODUCTCODE