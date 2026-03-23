WITH tsl AS
(
    SELECT
        CCCNO AS AccountId,
        MAX(CASE WHEN CCSLCD BETWEEN '09' AND '13' AND CCSLYN = 'Y' THEN 1 ELSE 0 END) AS IsProducer,
        MAX(CASE WHEN CCSLCD = 7 AND CCSLYN = 'Y' THEN 1 ELSE 0 END) AS IsSeedProducer
    FROM COMDATA.U4CStsl
    GROUP BY CCCNO
)
SELECT
    m.CSCNO AS AccountId,
    COALESCE(tsl.IsProducer, 0) AS IsProducer,
    COALESCE(tsl.IsSeedProducer, 0) AS IsSeedProducer,

    /* All columns from U4CSTMR query */
    CASE WHEN m.CSACT = 'I' THEN 0 ELSE 1 END  AS IsActive,
    m.CSCONM AS EntityName,
    m.CSLKNM AS LookupName,
    m.CSFRNM AS OwnerFirstName,
    m.CSLSNM AS OwnerLastName,
    0 AS IsWholeSale,
    0 AS IsAutoPriced,
    0 AS PaysRoyalities,
    CASE
      WHEN m.CSTEXD = 0 THEN NULL
      ELSE DATE(
        SUBSTR(DIGITS(m.CSTEXD),1,4) || '-' ||
        SUBSTR(DIGITS(m.CSTEXD),5,2) || '-' ||
        SUBSTR(DIGITS(m.CSTEXD),7,2)
      )
    END AS TaxExemptDate,
    CASE WHEN m.CSTXID = '' THEN NULL ELSE m.CSTXID END AS TaxId,
    CURRENT TIMESTAMP AS CreatedAt,
    m.CSEADR AS Email,
    CASE WHEN m.CSESTMT='Y' THEN 1 ELSE 0 END  AS EmailWeightSheet,
    CASE WHEN m.CSPSTA='Y'  THEN 1 ELSE 0 END  AS PrintWeightSheet,
    CASE WHEN m.CSESTMT='Y' THEN 1 ELSE 0 END  AS EmailStatement,
    CASE WHEN m.CSPSTA='Y'  THEN 1 ELSE 0 END  AS PrintStatement,
    m.CSAD1  AS Address1,
    m.CSAD2  AS Address2,
    m.CSCITY AS City,
    m.CSSTAT AS State,
    m.CSZIP  AS Zip,
    m.CSCOUN AS Country,
    m.CSHPHN AS Phone1,
    m.CSWPHN AS Phone2,
    m.CSMPHN AS MobilePhone,
    1 AS CustomerPaysRoyalties,
    '' AS Contact,
    TRIM(
      COALESCE(TRIM(m.CSMEMO), '') ||
      COALESCE(TRIM(m.CSNOT1), '') ||
      COALESCE(TRIM(m.CSNOT2), '')
    ) AS Notes
FROM COMDATA.U4CSTMR m
LEFT JOIN tsl
  ON tsl.AccountId = m.CSCNO
