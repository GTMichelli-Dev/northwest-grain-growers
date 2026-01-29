SELECT  CSCNO AccountId,
CASE WHEN  CSACT='I' THEN 0 ELSE 1 END  Active,
CSCONM EntityName,
CSLKNM  LookupName,
CSFRNM  OwnerFirstName,
CSLSNM   OwnerLastName ,
CASE WHEN CSMBER='P' THEN 1 ELSE 0 END  IsProducer,
0  IsWholeSale,
0  IsAutoPriced,
0  PaysRoyalities,
CASE
  WHEN CSTEXD = 0 THEN NULL
  ELSE DATE(
    SUBSTR(DIGITS(CSTEXD),1,4) || '-' ||
    SUBSTR(DIGITS(CSTEXD),5,2) || '-' ||
    SUBSTR(DIGITS(CSTEXD),7,2)
  )
END AS TaxExemptDate,
CASE WHEN CSTXID='' THEN null ELSE CSTXID END TaxId,
CURRENT TIMESTAMP CreatedAt,
CSEADR  Email,
CASE WHEN CSESTMT='Y' THEN 1 ELSE 0 END  EmailWeightSheet,
CASE WHEN CSPSTA='Y'THEN 1 ELSE 0 END  PrintWeightSheet,
CASE WHEN CSESTMT='Y' THEN 1 ELSE 0 END  EmailStatement,
CASE WHEN CSPSTA='Y'THEN 1 ELSE 0 END  PrintStatement,
CSAD1  Address1 ,
CSAD2  Address2 ,
CSCITY  City ,
CSSTAT  State,  
CSZIP  Zip,
 CSCOUN Country,
CSHPHN  Phone1,
CSWPHN  Phone2,
CSMPHN  MobilePhone,
1  CustomerPaysRoyalties,
''  Contact,
Trim(
 COALESCE(trim(CSMEMO), '') || 
    COALESCE(trim(CSNOT1), '') || 
    COALESCE(trim(CSNOT2), '')) Notes
FROM COMDATA.U4CSTMR