Select IMPROD,IMDESC,IMFLCD,IMDEPT,IMDEL,
case when IMFLCD=1  then 'FOUNDATION'
 when IMFLCD=2  then 'REGISTERED'
 when IMFLCD=3  then 'CERTIFIED'
 ELSE  ''
 END AS CLASS,
case when IMFLCD=1  then 1
 when IMFLCD=2  then 2
 when IMFLCD=3  then 3
 ELSE 0
 END AS CLASSID,




CASE
    WHEN IMDEPT=16
            THEN 'PEA'
        WHEN IMDEPT=18
            THEN 'GRB'
       WHEN IMDEPT=21 OR  IMDEPT=88 OR  IMDEPT=89
            THEN 'BLY'   
       WHEN IMDEPT=22
            THEN 'OAT'      
       WHEN IMDEPT=90
            THEN 'MISC'                  
       WHEN IMDEPT=85 OR IMDEPT=86
            THEN 'WHC'                
       WHEN IMDEPT=28 OR IMDEPT=29
            THEN 'CHEM'                 
       WHEN IMDEPT=31 OR IMDEPT=32
            THEN 'GRASS'
       WHEN IMDEPT=71  OR IMDEPT=77  OR IMDEPT=72  OR IMDEPT=73 OR IMDEPT=82 OR IMDEPT=83
            THEN 'SWH'  
       WHEN IMDEPT=76 OR IMDEPT=74 OR IMDEPT=84
            THEN 'DNS'     
            
       WHEN IMDEPT=75 OR IMDEPT=80 OR IMDEPT=81 OR IMDEPT=87
            THEN 'HRW'          
            
        END AS Dept,
            IMDEPT,
   CASE WHEN IMDEPT=16
            THEN 'PULSES'
        WHEN IMDEPT=18
            THEN 'PULSES'
       WHEN IMDEPT=21 OR  IMDEPT=88 OR  IMDEPT=89
            THEN 'BARLEY'   
       WHEN IMDEPT=22
            THEN 'OATS'      
       WHEN IMDEPT=90
            THEN 'SPECIAL'                  
       WHEN IMDEPT=85 OR IMDEPT=86
            THEN 'WHEAT'                
       WHEN IMDEPT=28 OR IMDEPT=29
            THEN 'CHEM'                 
       WHEN IMDEPT=31 OR IMDEPT=32
            THEN 'GRASS'
       WHEN IMDEPT=71  OR IMDEPT=77  OR IMDEPT=72  OR IMDEPT=73 OR IMDEPT=82 OR IMDEPT=83
            THEN 'WHEAT'  
       WHEN IMDEPT=76 OR IMDEPT=74 OR IMDEPT=84
            THEN 'WHEAT'     
            
       WHEN IMDEPT=75 OR IMDEPT=80 OR IMDEPT=81 OR IMDEPT=87
            THEN 'WHEAT'          
            
        END AS CAEAGORY
from
COMDATA.U4ITMMR

WHERE IMDEPT NOT IN
(
    1, 10, 12, 13, 14, 15, 17, 19,
    20, 27, 33,
    50, 60, 61, 62, 63, 64, 65, 66,
    69, 70,
    91, 92, 93, 95, 97, 99
)
order by IMDEPT desc
