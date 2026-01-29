SELECT
   
 
    CASE 

        WHEN DPDEPT=16
            THEN 'PEA'
        WHEN DPDEPT=18
            THEN 'GRB'
       WHEN DPDEPT=21 OR  DPDEPT=88 OR  DPDEPT=89
            THEN 'BLY'   
       WHEN DPDEPT=22
            THEN 'OAT'      
       WHEN DPDEPT=90
            THEN 'MISC'                  
       WHEN DPDEPT=85 OR DPDEPT=86
            THEN 'WHC'                
       WHEN DPDEPT=28 OR DPDEPT=29
            THEN 'CHEM'                 
       WHEN DPDEPT=31 OR DPDEPT=32
            THEN 'GRASS'
       WHEN DPDEPT=71  OR DPDEPT=77  OR DPDEPT=72  OR DPDEPT=73 OR DPDEPT=82 OR DPDEPT=83
            THEN 'SWH'  
       WHEN DPDEPT=76 OR DPDEPT=74 OR DPDEPT=84
            THEN 'DNS'     
            
       WHEN DPDEPT=75 OR DPDEPT=80 OR DPDEPT=81 OR DPDEPT=87
            THEN 'HRW'          
            
        END AS Dept,
            DPDEPT
   
FROM COMDATA.U4DEPT
 WHERE 
    DPDEPT<>1
      AND
    DPDEPT<>10
      AND
    DPDEPT<>12
      AND
    DPDEPT<>13
      AND
    DPDEPT<>14
      AND      
    DPDEPT<>15
      AND     
     DPDEPT<>17
      AND   
    DPDEPT<>19
      AND                          
    DPDEPT<>27
      AND
    DPDEPT<>20
       AND 
    DPDEPT<>50

      AND
    DPDEPT<>60
     AND 
    DPDEPT<>61
     AND 
    DPDEPT<>50
        AND 
    DPDEPT<>64
       AND 
    DPDEPT<>33
       AND 
    DPDEPT<>62
       AND        
    DPDEPT<>65
       AND 
    DPDEPT<>66
       AND 
    DPDEPT<>69
       AND 
    DPDEPT<>70
       AND 
    DPDEPT<>99
       AND 
    DPDEPT<>63
       AND 
    DPDEPT<>91
       AND 
    DPDEPT<>93
       AND        
    DPDEPT<>92
       AND 
    DPDEPT<>95
       AND 
    DPDEPT<>97

ORDER BY DPDEPT
