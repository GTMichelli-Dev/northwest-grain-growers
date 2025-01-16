MODE_UNDER=1
MODE_TARGET=2
MODE_OVER=3
VAL_GROSS=0
VAL_NET=1
VAL_TARE=2
VAL_MIN=3
VAL_MAX=4
VAL_ROC=5
VAL_GTOTAL=6
VAL_NTOTAL=7
VAL_TTOTAL=8
VAL_CTOTAL=9
VAL_TRANS=10
VAL_COUNT=11
VAL_VAR=12
VAL_PCWT=13
VAL_ADCCOUNTS=14
VAL_STD_DEV=15
VAL_TREND=16
VAL_X_BAR_R=17
VAL_PERCENT=18
VAL_NET_MINUS_ANNUN=19
VAL_BLANK=20
VAL_INMO=21
UNDER_STR="UNDER  "ACCEPT_STR="ACCEPT "OVER_STR="OVER   "REJECT_STR="REJECT "UNKNOWN_STR="UNKNOWN"XR4500_RED="&"XR4500_GRN="*"XR4500_OFF="%"XR4500_FLASH_ON="("XR4500_FLASH_OFF=")"XR4500_FLASH_3="!"XBAR_MSG0="      "XBAR_MSG1="1 of 1"XBAR_MSG2="2 of 3"XBAR_MSG3="4 of 5"XBAR_MSG4="8 of 8"CHECKWEIGH_TYPE_LIMITS=0
CHECKWEIGH_TYPE_SAMPLE=1
function doMyDateTime()local tmpOSDateTime,myDate,myTime,myDateTime
tmpOSDateTime=os.date("*t")if config.dateFormat==nil then
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)else
if config.dateFormat==0 then
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)elseif config.dateFormat==1 then
myDate=string.format("%02d-%02d-%04d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)elseif config.dateFormat==2 then
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.day,tmpOSDateTime.month,tmpOSDateTime.year)elseif config.dateFormat==3 then
myDate=string.format("%02d-%02d-%04d",tmpOSDateTime.day,tmpOSDateTime.month,tmpOSDateTime.year)else
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)end
end
if config.timeFormat==nil then
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)else
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)end
myDateTime=myDate.." "..myTime
return myDateTime
end
checkweigh={}checkweightable={}if system.modelStr=="ZM301"then
MAX_CHECKWEIGH_INDEX=1
elseif system.modelStr=="ZM303"then
MAX_CHECKWEIGH_INDEX=10
elseif system.modelStr=="ZQ375"then
MAX_CHECKWEIGH_INDEX=1
elseif system.modelStr=="ZM305GTN"then
MAX_CHECKWEIGH_INDEX=10
elseif system.modelStr=="ZM305"then
MAX_CHECKWEIGH_INDEX=10
else
MAX_CHECKWEIGH_INDEX=1
end
transaction={}transaction.TransactionCount=0
transaction.TransactionPLUNumber=0
transaction.TransactionTimeDate=doMyDateTime()transaction.TransactionSysCount=0
transaction.TransactionGrossWt=0
transaction.TransactionNetWt=0
transaction.TransactionUofM=wt.unitsStr
transaction.TransactionUAO="0"transaction.TransactionID=0
transaction.TransGrossWtTotal=0
transaction.TransNetWtTotal=0
transaction.TransCountTotal=0
Stat={}Stat.Netwt={}Stat.Underwt={}Stat.Targetwt={}Stat.Overwt={}Stat.meanNetwt=0
Stat.medianNetwt=0
Stat.sdNetwt=0
Stat.covNetwt=0
Stat.maxNetwt=0
Stat.minNetwt=0
Stat.cntNetwt=0
Stat.maxUnderwt=0
Stat.minUnderwt=0
Stat.cntUnderwt=0
Stat.maxTargetwt=0
Stat.minTargetwt=0
Stat.cntTargetwt=0
Stat.maxOverwt=0
Stat.minOverwt=0
Stat.cntOverwt=0
Stat.xbarLimit=0
Stat.xbarCnt=0
Stat.xbar=0
Stat.xbarR=0
Stat.xbarAverage={}Stat.xbarMsg=XBAR_MSG0
minLatchTime=0
checkweigh.StoreBeforePrint=false
checkweigh.UNITOFMEASUREDefault=wt.units
checkweigh.UNITOFMEASUREDefaultStr=wt.unitsStr
checkweigh.CheckWeighModeDefault=CHECK_NONE
checkweigh.CheckgraphEnableFlagDefault=0
checkweigh.CheckBasisDefault=VAL_NET
checkweigh.CheckMinDefault=wt.curDivision*config.grossZeroBand
checkweigh.CheckMaxDefault=wt.curCapacity
checkweigh.CheckTolLoDefault=wt.curDivision
checkweigh.CheckTolHiDefault=wt.curDivision
checkweigh.CheckUnderDivDefault=1
checkweigh.CheckOverDivDefault=1
checkweigh.CheckTargLoDefault=2.4
checkweigh.CheckTargDefault=2.5
checkweigh.CheckTargHiDefault=2.6
checkweigh.CheckUnderSegDivDefault=1
checkweigh.CheckOverSegDivDefault=1
checkweigh.CheckGraphTypeDefault=3
checkweigh.CheckWeighTypeDefault=1
checkweigh.CheckOutputTypeDefault=1
checkweigh.CheckOutputGZBDefault=0
checkweigh.CheckStatPackageDefault=0
checkweigh.CheckAutoTareAtTargetDefault=0
checkweigh.CheckAutoStoreAtTargetDefault=0
checkweigh.PrintTotalFlagDefault=0
if AppName=="MID375"then
checkweigh.TotalFmtDefault=9
else
checkweigh.TotalFmtDefault=8
end
checkweigh.ClearTotalFlagDefault=1
checkweigh.ClearPLUTotalFlagDefault=0
checkweigh.DigitsFlagDefault=1
checkweigh.GraphFlagDefault=1
checkweigh.PackRunCntDefault=0
checkweigh.UnitsOfMeasureStr=checkweigh.UNITOFMEASUREDefaultStr
checkweigh.UnitsOfMeasure=checkweigh.UNITOFMEASUREDefault
checkweigh.CheckWeighMode=checkweigh.CheckWeighModeDefault
checkweigh.CheckgraphEnableFlag=checkweigh.CheckgraphEnableFlagDefault
checkweigh.CheckBasis=checkweigh.CheckBasisDefault
checkweigh.CheckMin=checkweigh.CheckMinDefault
checkweigh.CheckMax=checkweigh.CheckMaxDefault
checkweigh.CheckTolLo=checkweigh.CheckTolLoDefault
checkweigh.CheckTolHi=checkweigh.CheckTolHiDefault
checkweigh.CheckUnderDiv=checkweigh.CheckUnderDivDefault
checkweigh.CheckOverDiv=checkweigh.CheckOverDivDefault
checkweigh.CheckTargLo=checkweigh.CheckTargLoDefault
checkweigh.CheckTarg=checkweigh.CheckTargDefault
checkweigh.CheckTargHi=checkweigh.CheckTargHiDefault
checkweigh.CheckUnderSegDiv=checkweigh.CheckUnderSegDivDefault
checkweigh.CheckOverSegDiv=checkweigh.CheckOverSegDivDefault
checkweigh.CheckGraphType=checkweigh.CheckGraphTypeDefault
checkweigh.CheckWeighType=checkweigh.CheckWeighTypeDefault
checkweigh.CheckOutputType=checkweigh.CheckOutputTypeDefault
checkweigh.CheckOutputGZB=checkweigh.CheckOutputGZBDefault
checkweigh.CheckStatPackage=checkweigh.CheckStatPackageDefault
checkweigh.CheckAutoTareAtTarget=checkweigh.CheckAutoTareAtTargetDefault
checkweigh.CheckAutoStoreAtTarget=checkweigh.CheckAutoStoreAtTargetDefault
checkweigh.PrintTotalFlag=checkweigh.PrintTotalFlagDefault
checkweigh.TotalFmt=checkweigh.TotalFmtDefault
checkweigh.ClearTotalFlag=checkweigh.ClearTotalFlagDefault
checkweigh.ClearPLUTotalFlag=checkweigh.ClearPLUTotalFlagDefault
checkweigh.DigitsFlag=checkweigh.DigitsFlagDefault
checkweigh.GraphFlag=checkweigh.GraphFlagDefault
checkweigh.PackRunCnt=checkweigh.PackRunCntDefault
checkweighBand=UNKNOWN_STR
checkweighBand1=UNKNOWN_STR
checkweigh.XR4500=XR4500_OFF
function checkweigh.initCheckPrintTokens()awtx.fmtPrint.varSet(51,checkweigh.CheckBasis,"Basis",AWTX_LUA_INTEGER)printTokens[51].varName="checkweigh.CheckBasis"printTokens[51].varLabel="Basis"printTokens[51].varType=AWTX_LUA_INTEGER
printTokens[51].varValue=checkweigh.CheckBasis
printTokens[51].varFunct=""awtx.fmtPrint.varSet(52,checkweigh.CheckMin,"Minimum",AWTX_LUA_FLOAT)printTokens[52].varName="checkweigh.CheckMin"printTokens[52].varLabel="Minimum"printTokens[52].varType=AWTX_LUA_FLOAT
printTokens[52].varValue=checkweigh.CheckMin
printTokens[52].varFunct=""awtx.fmtPrint.varSet(53,checkweigh.CheckTargLo,"Target Lo",AWTX_LUA_FLOAT)printTokens[53].varName="checkweigh.CheckTargLo"printTokens[53].varLabel="Target Lo"printTokens[53].varType=AWTX_LUA_FLOAT
printTokens[53].varValue=checkweigh.CheckTargLo
printTokens[53].varFunct=checkweigh.setTargLo
awtx.fmtPrint.varSet(54,checkweigh.CheckTargHi,"Target Hi",AWTX_LUA_FLOAT)printTokens[54].varName="checkweigh.CheckTargHi"printTokens[54].varLabel="Target Hi"printTokens[54].varType=AWTX_LUA_FLOAT
printTokens[54].varValue=checkweigh.CheckTargHi
printTokens[54].varFunct=checkweigh.setTargHi
awtx.fmtPrint.varSet(55,checkweigh.CheckTolLo,"Tolerance Lo",AWTX_LUA_FLOAT)printTokens[55].varName="checkweigh.CheckTolLo"printTokens[55].varLabel="Tolerance Lo"printTokens[55].varType=AWTX_LUA_FLOAT
printTokens[55].varValue=checkweigh.CheckTolLo
printTokens[55].varFunct=checkweigh.setTolLo
awtx.fmtPrint.varSet(56,checkweigh.CheckTolHi,"Tolerance Hi",AWTX_LUA_FLOAT)printTokens[56].varName="checkweigh.CheckTolHi"printTokens[56].varLabel="Tolerance Hi"printTokens[56].varType=AWTX_LUA_FLOAT
printTokens[56].varValue=checkweigh.CheckTolHi
printTokens[56].varFunct=checkweigh.setTolHi
awtx.fmtPrint.varSet(57,checkweigh.CheckTarg,"Target",AWTX_LUA_FLOAT)printTokens[57].varName="checkweigh.CheckTarg"printTokens[57].varLabel="Target"printTokens[57].varType=AWTX_LUA_FLOAT
printTokens[57].varValue=checkweigh.CheckTarg
printTokens[57].varFunct=checkweigh.setTarget
awtx.fmtPrint.varSet(58,checkweigh.CheckUnderDiv,"Under Divisions",AWTX_LUA_INTEGER)printTokens[58].varName="checkweigh.CheckUnderDiv"printTokens[58].varLabel="Under Divisions"printTokens[58].varType=AWTX_LUA_INTEGER
printTokens[58].varValue=checkweigh.CheckUnderDiv
printTokens[58].varFunct=checkweigh.setUnderDiv
awtx.fmtPrint.varSet(59,checkweigh.CheckOverDiv,"Over Divisions",AWTX_LUA_INTEGER)printTokens[59].varName="checkweigh.CheckOverDiv"printTokens[59].varLabel="Over Divisions"printTokens[59].varType=AWTX_LUA_INTEGER
printTokens[59].varValue=checkweigh.CheckOverDiv
printTokens[59].varFunct=checkweigh.setOverDiv
awtx.fmtPrint.varSet(60,checkweigh.CheckMax,"Maximum",AWTX_LUA_FLOAT)printTokens[60].varName="checkweigh.CheckMax"printTokens[60].varLabel="Maximum"printTokens[60].varType=AWTX_LUA_FLOAT
printTokens[60].varValue=checkweigh.CheckMax
printTokens[60].varFunct=""awtx.fmtPrint.varSet(61,checkweigh.CheckUnderSegDiv,"Under Divisions per Segment",AWTX_LUA_INTEGER)printTokens[61].varName="checkweigh.CheckUnderSegDiv"printTokens[61].varLabel="Under Divisions per Segment"printTokens[61].varType=AWTX_LUA_INTEGER
printTokens[61].varValue=checkweigh.CheckUnderSegDiv
printTokens[61].varFunct=checkweigh.setCheckUnderSegDiv
awtx.fmtPrint.varSet(62,checkweigh.CheckOverSegDiv,"Over Divisions per Segment",AWTX_LUA_INTEGER)printTokens[62].varName="checkweigh.CheckOverSegDiv"printTokens[62].varLabel="Over Divisions per Segment"printTokens[62].varType=AWTX_LUA_INTEGER
printTokens[62].varValue=checkweigh.CheckOverSegDiv
printTokens[62].varFunct=checkweigh.setCheckOverSegDiv
awtx.fmtPrint.varSet(63,wt.net,checkweighBand,AWTX_LUA_FLOAT)printTokens[63].varName="wt.net"printTokens[63].varLabel=checkweighBand
printTokens[63].varType=AWTX_LUA_FLOAT
printTokens[63].varValue=wt.net
printTokens[63].varFunct=""awtx.fmtPrint.varSet(64,wt.net,checkweighBand1,AWTX_LUA_FLOAT)printTokens[64].varName="wt.net"printTokens[64].varLabel=checkweighBand1
printTokens[64].varType=AWTX_LUA_FLOAT
printTokens[64].varValue=wt.net
printTokens[64].varFunct=""awtx.fmtPrint.varSet(65,checkweigh.XR4500,"XR4500 Light",AWTX_LUA_STRING)printTokens[65].varName="checkweigh.XR4500"printTokens[65].varLabel="XR4500 Light"printTokens[65].varType=AWTX_LUA_STRING
printTokens[65].varValue=checkweigh.XR4500
printTokens[65].varFunct=""awtx.fmtPrint.varSet(66,checkweigh.CheckWeighType,"Type",AWTX_LUA_INTEGER)printTokens[66].varName="checkweigh.CheckWeighType"printTokens[66].varLabel="Type"printTokens[66].varType=AWTX_LUA_INTEGER
printTokens[66].varValue=checkweigh.CheckWeighType
printTokens[66].varFunct=checkweigh.setCheckWeighType
awtx.fmtPrint.varSet(70,checkweigh.UnitsOfMeasureStr,"UofM",AWTX_LUA_STRING)printTokens[70].varName="checkweigh.UnitsOfMeasureStr"printTokens[70].varLabel="UofM"printTokens[70].varType=AWTX_LUA_STRING
printTokens[70].varValue=checkweigh.UnitsOfMeasureStr
printTokens[70].varFunct=checkweigh.setUnitsOfMeasure
end
function checkweigh.setCheckPrintTokens()awtx.fmtPrint.varSet(51,checkweigh.CheckBasis,"Basis",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(52,checkweigh.CheckMin,"Minimum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(53,checkweigh.CheckTargLo,"Target Lo",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(54,checkweigh.CheckTargHi,"Target Hi",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(55,checkweigh.CheckTolLo,"Tolerance Lo",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(56,checkweigh.CheckTolHi,"Tolerance Hi",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(57,checkweigh.CheckTarg,"Target",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(58,checkweigh.CheckUnderDiv,"Under Divisions",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(59,checkweigh.CheckOverDiv,"Over Divisions",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(60,checkweigh.CheckMax,"Maximum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(61,checkweigh.CheckUnderSegDiv,"Under Divisions per Segment",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(62,checkweigh.CheckOverSegDiv,"Over Divisions per Segment",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(63,wt.net,checkweighBand,AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(64,wt.net,checkweighBand1,AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(65,checkweigh.XR4500,"XR4500 Light",AWTX_LUA_STRING)awtx.fmtPrint.varSet(66,checkweigh.CheckWeighType,"Type",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(70,checkweigh.UnitsOfMeasureStr,"UofM",AWTX_LUA_STRING)end
function checkweigh.setXR4500PrintToken()awtx.fmtPrint.varSet(65,checkweigh.XR4500,"XR4500 Light",AWTX_LUA_STRING)end
function transaction.initTransPrintTokens()awtx.fmtPrint.varSet(91,transaction.TransactionCount,"Transaction Count",AWTX_LUA_INTEGER)printTokens[91].varName="transaction.TransactionCount"printTokens[91].varLabel="Transaction Count"printTokens[91].varType=AWTX_LUA_INTEGER
printTokens[91].varValue=transaction.TransactionCount
printTokens[91].varFunct=""awtx.fmtPrint.varSet(92,transaction.TransactionPLUNumber,"Transaction PLUNumber",AWTX_LUA_INTEGER)printTokens[92].varName="transaction.TransactionPLUNumber"printTokens[92].varLabel="Transaction PLUNumber"printTokens[92].varType=AWTX_LUA_INTEGER
printTokens[92].varValue=transaction.TransactionPLUNumber
printTokens[92].varFunct=""awtx.fmtPrint.varSet(93,transaction.TransactionTimeDate,"Transaction TimeDate",AWTX_LUA_STRING)printTokens[93].varName="transaction.TransactionTimeDate"printTokens[93].varLabel="Transaction TimeDate"printTokens[93].varType=AWTX_LUA_STRING
printTokens[93].varValue=transaction.TransactionTimeDate
printTokens[93].varFunct=""awtx.fmtPrint.varSet(94,transaction.TransactionSysCount,"Transaction SysCount",AWTX_LUA_INTEGER)printTokens[94].varName="transaction.TransactionSysCount"printTokens[94].varLabel="Transaction SysCount"printTokens[94].varType=AWTX_LUA_INTEGER
printTokens[94].varValue=transaction.TransactionSysCount
printTokens[94].varFunct=""awtx.fmtPrint.varSet(95,transaction.TransactionGrossWt,"Transaction GrossWt",AWTX_LUA_FLOAT)printTokens[95].varName="transaction.TransactionGrossWt"printTokens[95].varLabel="Transaction GrossWt"printTokens[95].varType=AWTX_LUA_FLOAT
printTokens[95].varValue=transaction.TransactionGrossWt
printTokens[95].varFunct=""awtx.fmtPrint.varSet(96,transaction.TransactionNetWt,"Transaction NetWt",AWTX_LUA_FLOAT)printTokens[96].varName="transaction.TransactionNetWt"printTokens[96].varLabel="Transaction NetWt"printTokens[96].varType=AWTX_LUA_FLOAT
printTokens[96].varValue=transaction.TransactionNetWt
printTokens[96].varFunct=""awtx.fmtPrint.varSet(97,transaction.TransactionUofM,"Transaction UofM",AWTX_LUA_STRING)printTokens[97].varName="transaction.TransactionUofM"printTokens[97].varLabel="Transaction UofM"printTokens[97].varType=AWTX_LUA_STRING
printTokens[97].varValue=transaction.TransactionUofM
printTokens[97].varFunct=""awtx.fmtPrint.varSet(98,transaction.TransactionUAO,"Transaction UAO",AWTX_LUA_STRING)printTokens[98].varName="transaction.TransactionUAO"printTokens[98].varLabel="Transaction UAO"printTokens[98].varType=AWTX_LUA_STRING
printTokens[98].varValue=transaction.TransactionUAO
printTokens[98].varFunct=""awtx.fmtPrint.varSet(99,transaction.TransactionID,"Transaction ID",AWTX_LUA_INTEGER)printTokens[99].varName="transaction.TransactionID"printTokens[99].varLabel="Transaction ID"printTokens[99].varType=AWTX_LUA_INTEGER
printTokens[99].varValue=transaction.TransactionID
printTokens[99].varFunct=""awtx.fmtPrint.varSet(24,transaction.TransactionCount,"Transaction Count",AWTX_LUA_INTEGER)printTokens[24].varName="transaction.TransactionCount"printTokens[24].varLabel="Transaction Count MID375"printTokens[24].varType=AWTX_LUA_INTEGER
printTokens[24].varValue=transaction.TransactionCount
printTokens[24].varFunct=""awtx.fmtPrint.varSet(25,transaction.TransactionUofM,"Transaction UofM",AWTX_LUA_STRING)printTokens[25].varName="transaction.TransactionUofM"printTokens[25].varLabel="Transaction UofM MID375"printTokens[25].varType=AWTX_LUA_STRING
printTokens[25].varValue=transaction.TransactionUofM
printTokens[25].varFunct=""awtx.fmtPrint.varSet(26,transaction.TransGrossWtTotal,"Transaction GrossWt Total",AWTX_LUA_FLOAT)printTokens[26].varName="transaction.TransGrossWtTotal"printTokens[26].varLabel="Transaction GrossWt Total MID375"printTokens[26].varType=AWTX_LUA_FLOAT
printTokens[26].varValue=transaction.TransGrossWtTotal
printTokens[26].varFunct=""awtx.fmtPrint.varSet(27,transaction.TransNetWtTotal,"Transaction NetWt Total",AWTX_LUA_FLOAT)printTokens[27].varName="transaction.TransNetWtTotal"printTokens[27].varLabel="Transaction NetWt Total MID375"printTokens[27].varType=AWTX_LUA_FLOAT
printTokens[27].varValue=transaction.TransNetWtTotal
printTokens[27].varFunct=""end
function transaction.setTransPrintTokens()awtx.fmtPrint.varSet(91,transaction.TransactionCount,"Transaction Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(92,transaction.TransactionPLUNumber,"Transaction PLUNumber",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(93,transaction.TransactionTimeDate,"Transaction TimeDate",AWTX_LUA_STRING)awtx.fmtPrint.varSet(94,transaction.TransactionSysCount,"Transaction SysCount",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(95,transaction.TransactionGrossWt,"Transaction GrossWt",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(96,transaction.TransactionNetWt,"Transaction NetWt",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(97,transaction.TransactionUofM,"Transaction UofM",AWTX_LUA_STRING)awtx.fmtPrint.varSet(98,transaction.TransactionUAO,"Transaction UAO",AWTX_LUA_STRING)awtx.fmtPrint.varSet(99,transaction.TransactionID,"Transaction ID",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(24,transaction.TransactionCount,"Transaction Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(25,transaction.TransactionUofM,"Transaction UofM",AWTX_LUA_STRING)awtx.fmtPrint.varSet(26,transaction.TransGrossWtTotal,"Transaction GrossWt Total MID375",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(27,transaction.TransNetWtTotal,"Transaction NetWt Total MID375",AWTX_LUA_FLOAT)end
function Stat.initStatsPrintTokens()awtx.fmtPrint.varSet(41,Stat.cntNetwt,"Cnt of Tot Wt",AWTX_LUA_INTEGER)printTokens[41].varName="Stat.cntNetwt"printTokens[41].varLabel="Cnt of Tot Wt"printTokens[41].varType=AWTX_LUA_INTEGER
printTokens[41].varValue=Stat.cntNetwt
printTokens[41].varFunct=""awtx.fmtPrint.varSet(42,Stat.cntUnderwt,"Cnt of Under Wt",AWTX_LUA_INTEGER)printTokens[42].varName="Stat.cntUnderwt"printTokens[42].varLabel="Cnt of Under Wt"printTokens[42].varType=AWTX_LUA_INTEGER
printTokens[42].varValue=Stat.cntUnderwt
printTokens[42].varFunct=""awtx.fmtPrint.varSet(43,Stat.cntTargetwt,"Cnt of Target Wt",AWTX_LUA_INTEGER)printTokens[43].varName="Stat.cntTargetwt"printTokens[43].varLabel="Cnt of Target Wt"printTokens[43].varType=AWTX_LUA_INTEGER
printTokens[43].varValue=Stat.cntTargetwt
printTokens[43].varFunct=""awtx.fmtPrint.varSet(44,Stat.cntOverwt,"Cnt of Over Wt",AWTX_LUA_INTEGER)printTokens[44].varName="Stat.cntOverwt"printTokens[44].varLabel="Cnt of Over Wt"printTokens[44].varType=AWTX_LUA_INTEGER
printTokens[44].varValue=Stat.cntOverwt
printTokens[44].varFunct=""awtx.fmtPrint.varSet(45,Stat.meanNetwt,"Mean Net Wt",AWTX_LUA_FLOAT)printTokens[45].varName="Stat.meanNetwt"printTokens[45].varLabel="Mean Net Wt"printTokens[45].varType=AWTX_LUA_FLOAT
printTokens[45].varValue=Stat.meanNetwt
printTokens[45].varFunct=""awtx.fmtPrint.varSet(46,Stat.medianNetwt,"Median Net Wt",AWTX_LUA_FLOAT)printTokens[46].varName="Stat.medianNetwt"printTokens[46].varLabel="Median Net Wt"printTokens[46].varType=AWTX_LUA_FLOAT
printTokens[46].varValue=Stat.medianNetwt
printTokens[46].varFunct=""awtx.fmtPrint.varSet(47,Stat.sdNetwt,"SD Net Wt",AWTX_LUA_FLOAT)printTokens[47].varName="Stat.sdNetwt"printTokens[47].varLabel="SD Net Wt"printTokens[47].varType=AWTX_LUA_FLOAT
printTokens[47].varValue=Stat.sdNetwt
printTokens[47].varFunct=""awtx.fmtPrint.varSet(48,Stat.covNetwt,"CV Net Wt",AWTX_LUA_FLOAT)printTokens[48].varName="Stat.covNetwt"printTokens[48].varLabel="CV Net Wt"printTokens[48].varType=AWTX_LUA_FLOAT
printTokens[48].varValue=Stat.covNetwt
printTokens[48].varFunct=""awtx.fmtPrint.varSet(49,Stat.maxNetwt,"Max Net Wt",AWTX_LUA_FLOAT)printTokens[49].varName="Stat.maxNetwt"printTokens[49].varLabel="Max Net Wt"printTokens[49].varType=AWTX_LUA_FLOAT
printTokens[49].varValue=Stat.maxNetwt
printTokens[49].varFunct=""awtx.fmtPrint.varSet(50,Stat.minNetwt,"Min Net Wt",AWTX_LUA_FLOAT)printTokens[50].varName="Stat.minNetwt"printTokens[50].varLabel="Min Net Wt"printTokens[50].varType=AWTX_LUA_FLOAT
printTokens[50].varValue=Stat.minNetwt
printTokens[50].varFunct=""awtx.fmtPrint.varSet(67,Stat.xbar,"XBar",AWTX_LUA_FLOAT)printTokens[67].varName="Stat.xbar"printTokens[67].varLabel="XBar"printTokens[67].varType=AWTX_LUA_FLOAT
printTokens[67].varValue=Stat.xbar
printTokens[67].varFunct=""awtx.fmtPrint.varSet(68,Stat.xbarR,"XBar/R",AWTX_LUA_FLOAT)printTokens[68].varName="Stat.xbarR"printTokens[68].varLabel="XBar/R"printTokens[68].varType=AWTX_LUA_FLOAT
printTokens[68].varValue=Stat.xbarR
printTokens[68].varFunct=""awtx.fmtPrint.varSet(69,Stat.xbarMsg,"XBar Message",AWTX_LUA_STRING)printTokens[69].varName="Stat.xbarMsg"printTokens[69].varLabel="XBar/R Message"printTokens[69].varType=AWTX_LUA_STRING
printTokens[69].varValue=Stat.xbarMsg
printTokens[69].varFunct=""end
function Stat.setStatsPrintTokens()awtx.fmtPrint.varSet(41,Stat.cntNetwt,"Cnt of Tot Wt",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(42,Stat.cntUnderwt,"Cnt of Under Wt",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(43,Stat.cntTargetwt,"Cnt of Target Wt",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(44,Stat.cntOverwt,"Cnt of Over Wt",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(45,Stat.meanNetwt,"Mean Net Wt",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(46,Stat.medianNetwt,"Median Net Wt",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(47,Stat.sdNetwt,"SD Net Wt",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(48,Stat.covNetwt,"CV Net Wt",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(49,Stat.maxNetwt,"Max Net Wt",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(50,Stat.minNetwt,"Min Net Wt",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(67,Stat.xbar,"XBar",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(68,Stat.xbarR,"XBar/R",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(69,Stat.xbarMsg,"XBar Message",AWTX_LUA_STRING)end
local DB_FileLocation_AppConfig
local DB_FileLocation_AppData
local DB_FileLocation_Reports
local DB_ReportName_Checkweigh
local DB_ReportName_Transactions
function checkweigh.updateWeightBasedSettingsAfterUnitsChange()local newUnits,oldUnits
newUnits=wt.units
oldUnits=checkweigh.UnitsOfMeasure
if checkweigh.CheckWeighMode==SIM375 then
checkweigh.CheckMinDefault=wt.curCapacity*-1
checkweigh.CheckMaxDefault=wt.curCapacity
checkweigh.CheckTolLoDefault=wt.curDivision
checkweigh.CheckTolHiDefault=wt.curDivision
elseif checkweigh.CheckWeighMode==PER375 then
checkweigh.CheckMinDefault=0.0
checkweigh.CheckMaxDefault=100.0
checkweigh.CheckTolLoDefault=0.1
checkweigh.CheckTolHiDefault=0.1
else
checkweigh.CheckMinDefault=wt.curDivision*config.grossZeroBand
checkweigh.CheckMaxDefault=wt.curCapacity
checkweigh.CheckTolLoDefault=wt.curDivision
checkweigh.CheckTolHiDefault=wt.curDivision
end
if checkweigh.CheckWeighMode~=PER375 then
checkweigh.CheckMin=awtx.weight.convertWeight(oldUnits,checkweigh.CheckMin,newUnits,1)checkweigh.CheckMax=awtx.weight.convertWeight(oldUnits,checkweigh.CheckMax,newUnits,1)checkweigh.CheckTolLo=awtx.weight.convertWeight(oldUnits,checkweigh.CheckTolLo,newUnits,1)checkweigh.CheckTolHi=awtx.weight.convertWeight(oldUnits,checkweigh.CheckTolHi,newUnits,1)checkweigh.CheckTargLo=awtx.weight.convertWeight(oldUnits,checkweigh.CheckTargLo,newUnits,1)checkweigh.CheckTarg=awtx.weight.convertWeight(oldUnits,checkweigh.CheckTarg,newUnits,1)checkweigh.CheckTargHi=awtx.weight.convertWeight(oldUnits,checkweigh.CheckTargHi,newUnits,1)end
transaction.TransGrossWtTotal=awtx.weight.convertWeight(oldUnits,transaction.TransGrossWtTotal,newUnits,1)transaction.TransNetWtTotal=awtx.weight.convertWeight(oldUnits,transaction.TransNetWtTotal,newUnits,1)transaction.TransactionUofM=wt.unitsStr
checkweigh.clrStatsPackage()checkweigh.clrXBarRPackage()checkweigh.refreshCheckGraph()checkweigh.UnitsOfMeasureStr=wt.unitsStr
checkweigh.UnitsOfMeasure=wt.units
end
function checkweigh.checkweighDBInit()local simAppPath,dbFile,result
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]DB_FileLocation_AppData=[[C:\Database\AppData.db]]DB_FileLocation_Reports=[[G:\Reports]]..SerialNumber
DB_ReportName_Checkweigh=[[\CheckweighReport.csv]]DB_ReportName_Transactions=[[\TransactionReport.csv]]dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighTransactions (TransactionCount INTEGER PRIMARY KEY, TransactionPLUNumber INTEGER, TransactionTimeDate TEXT, TransactionSysCount INTEGER, TransactionGrossWt DOUBLE, TransactionNetWt DOUBLE, TransactionUofM TEXT, TransactionUAO TEXT, TransactionID INTEGER)")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblLastTransactionCount (TransactionCount INTEGER)")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblLastTransactionSysCount (TransactionSysCount INTEGER)")dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:close()end
function checkweigh.checkweighDBStore()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighData (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")dbFile:execute("COMMIT")dbFile:close()end
function checkweigh.storeCheckAll()local found=false
local dbFile,result,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'CheckgraphEnableFlag'",checkweigh.CheckgraphEnableFlag)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'CheckBasis'",checkweigh.CheckBasis)result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%s' WHERE varID = 'UnitsOfMeasureStr'",checkweigh.UnitsOfMeasureStr)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%s' WHERE varID = 'UnitsOfMeasureStr'",checkweigh.UnitsOfMeasureStr)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'UnitsOfMeasure'",checkweigh.UnitsOfMeasure)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'UnitsOfMeasure'",checkweigh.UnitsOfMeasure)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckMin'",checkweigh.CheckMin)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckMin'",checkweigh.CheckMin)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTolLo'",checkweigh.CheckTolLo)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTolLo'",checkweigh.CheckTolLo)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTolHi'",checkweigh.CheckTolHi)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTolHi'",checkweigh.CheckTolHi)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckUnderDiv'",checkweigh.CheckUnderDiv)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckUnderDiv'",checkweigh.CheckUnderDiv)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckOverDiv'",checkweigh.CheckOverDiv)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckOverDiv'",checkweigh.CheckOverDiv)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTargLo'",checkweigh.CheckTargLo)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTargLo'",checkweigh.CheckTargLo)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTarg'",checkweigh.CheckTarg)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTarg'",checkweigh.CheckTarg)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTargHi'",checkweigh.CheckTargHi)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTargHi'",checkweigh.CheckTargHi)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckMax'",checkweigh.CheckMax)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckMax'",checkweigh.CheckMax)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckOverSegDiv=checkweigh.CheckOverSegDivDefault
end
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'CheckOverSegDiv'",checkweigh.CheckOverSegDiv)result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckUnderSegDiv=checkweigh.CheckUnderSegDivDefault
end
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'CheckUnderSegDiv'",checkweigh.CheckUnderSegDiv)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'CheckGraphType'",checkweigh.CheckGraphType)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'CheckOutputType'",checkweigh.CheckOutputType)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'CheckOutputGZB'",checkweigh.CheckOutputGZB)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'CheckWeighType'",checkweigh.CheckWeighType)result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==SIM375 then
checkweigh.CheckAutoTareAtTarget=checkweigh.CheckAutoTareAtTargetDefault
end
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'CheckAutoTareAtTarget'",checkweigh.CheckAutoTareAtTarget)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'CheckAutoStoreAtTarget'",checkweigh.CheckAutoStoreAtTarget)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'CheckStatPackage'",checkweigh.CheckStatPackage)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'PrintTotalFlag'",checkweigh.PrintTotalFlag)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'TotalFmt'",checkweigh.TotalFmt)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'ClearTotalFlag'",checkweigh.ClearTotalFlag)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'ClearPLUTotalFlag'",checkweigh.ClearPLUTotalFlag)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'DigitsFlag'",checkweigh.DigitsFlag)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'GraphFlag'",checkweigh.GraphFlag)result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'PackRunCnt'",checkweigh.PackRunCnt)result=dbFile:exec(sqlStr)dbFile:execute("COMMIT")dbFile:close()end
function checkweigh.storeCheckTargAll()local found=false
local dbFile,result,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%s' WHERE varID = 'UnitsOfMeasureStr'",checkweigh.UnitsOfMeasureStr)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%s' WHERE varID = 'UnitsOfMeasureStr'",checkweigh.UnitsOfMeasureStr)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'UnitsOfMeasure'",checkweigh.UnitsOfMeasure)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'UnitsOfMeasure'",checkweigh.UnitsOfMeasure)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTolLo'",checkweigh.CheckTolLo)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTolLo'",checkweigh.CheckTolLo)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTolHi'",checkweigh.CheckTolHi)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTolHi'",checkweigh.CheckTolHi)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckUnderDiv'",checkweigh.CheckUnderDiv)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckUnderDiv'",checkweigh.CheckUnderDiv)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckOverDiv'",checkweigh.CheckOverDiv)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckOverDiv'",checkweigh.CheckOverDiv)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTargLo'",checkweigh.CheckTargLo)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTargLo'",checkweigh.CheckTargLo)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTarg'",checkweigh.CheckTarg)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTarg'",checkweigh.CheckTarg)end
result=dbFile:exec(sqlStr)if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTargHi'",checkweigh.CheckTargHi)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%f' WHERE varID = 'CheckTargHi'",checkweigh.CheckTargHi)end
result=dbFile:exec(sqlStr)sqlStr=string.format("UPDATE OR REPLACE tblCheckweighConfig SET value = '%d' WHERE varID = 'CheckWeighType'",checkweigh.CheckWeighType)result=dbFile:exec(sqlStr)dbFile:execute("COMMIT")dbFile:close()end
function checkweigh.storeFloatValueInTblCheckweighDB(keyName,floatValue)local dbFile,result,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighConfig (varID TEXT PRIMARY KEY, value TEXT)")if checkweigh.CheckWeighMode==PER375 then
sqlStr=string.format("INSERT OR REPLACE INTO tblCheckweighConfig (varID, value) VALUES ('%s', '%f')",keyName,floatValue)else
sqlStr=string.format("INSERT OR REPLACE INTO tblCheckweighConfig (varID, value) VALUES ('%s', '%f')",keyName,floatValue)end
result=dbFile:exec(sqlStr)dbFile:close()end
function checkweigh.storeIntegerValueInTblCheckweighDB(keyName,integerValue)local dbFile,result,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighConfig (varID TEXT PRIMARY KEY, value TEXT)")sqlStr=string.format("INSERT OR REPLACE INTO tblCheckweighConfig (varID, value) VALUES ('%s', '%d')",keyName,integerValue)result=dbFile:exec(sqlStr)dbFile:close()end
function checkweigh.storeStringValueInTblCheckweighDB(keyName,stringValue)local dbFile,result,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighConfig (varID TEXT PRIMARY KEY, value TEXT)")sqlStr=string.format("INSERT OR REPLACE INTO tblCheckweighConfig (varID, value) VALUES ('%s', '%s')",keyName,stringValue)result=dbFile:exec(sqlStr)dbFile:close()end
function checkweigh.storeCheckgraphEnableFlag()checkweigh.storeIntegerValueInTblCheckweighDB("CheckgraphEnableFlag",checkweigh.CheckgraphEnableFlag)end
function checkweigh.storeCheckBasis()checkweigh.storeIntegerValueInTblCheckweighDB("CheckBasis",checkweigh.CheckBasis)end
function checkweigh.storeUnitsOfMeasureStr()checkweigh.storeStringValueInTblCheckweighDB("UnitsOfMeasureStr",checkweigh.UnitsOfMeasureStr)end
function checkweigh.storeUnitsOfMeasure()checkweigh.storeIntegerValueInTblCheckweighDB("UnitsOfMeasure",checkweigh.UnitsOfMeasure)end
function checkweigh.storeCheckMin()checkweigh.storeFloatValueInTblCheckweighDB("CheckMin",checkweigh.CheckMin)end
function checkweigh.storeCheckTolLo()checkweigh.storeFloatValueInTblCheckweighDB("CheckTolLo",checkweigh.CheckTolLo)end
function checkweigh.storeCheckTolHi()checkweigh.storeFloatValueInTblCheckweighDB("CheckTolHi",checkweigh.CheckTolHi)end
function checkweigh.storeCheckUnderDiv()checkweigh.storeFloatValueInTblCheckweighDB("CheckUnderDiv",checkweigh.CheckUnderDiv)end
function checkweigh.storeCheckOverDiv()checkweigh.storeFloatValueInTblCheckweighDB("CheckOverDiv",checkweigh.CheckOverDiv)end
function checkweigh.storeCheckTargLo()checkweigh.storeFloatValueInTblCheckweighDB("CheckTargLo",checkweigh.CheckTargLo)end
function checkweigh.storeCheckTarg()checkweigh.storeFloatValueInTblCheckweighDB("CheckTarg",checkweigh.CheckTarg)end
function checkweigh.storeCheckTargHi()checkweigh.storeFloatValueInTblCheckweighDB("CheckTargHi",checkweigh.CheckTargHi)end
function checkweigh.storeCheckMax()checkweigh.storeFloatValueInTblCheckweighDB("CheckMax",checkweigh.CheckMax)end
function checkweigh.storeCheckUnderSegDiv()if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckUnderSegDiv=checkweigh.CheckUnderSegDivDefault
end
checkweigh.storeIntegerValueInTblCheckweighDB("CheckUnderSegDiv",checkweigh.CheckUnderSegDiv)end
function checkweigh.storeCheckOverSegDiv()if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckOverSegDiv=checkweigh.CheckOverSegDivDefault
end
checkweigh.storeIntegerValueInTblCheckweighDB("CheckOverSegDiv",checkweigh.CheckOverSegDiv)end
function checkweigh.storeCheckGraphType()checkweigh.storeIntegerValueInTblCheckweighDB("CheckGraphType",checkweigh.CheckGraphType)end
function checkweigh.storeCheckOutputType()checkweigh.storeIntegerValueInTblCheckweighDB("CheckOutputType",checkweigh.CheckOutputType)end
function checkweigh.storeCheckOutputGZB()checkweigh.storeIntegerValueInTblCheckweighDB("CheckOutputGZB",checkweigh.CheckOutputGZB)end
function checkweigh.storeCheckWeighMode()end
function checkweigh.storeCheckWeighType()checkweigh.storeIntegerValueInTblCheckweighDB("CheckWeighType",checkweigh.CheckWeighType)end
function checkweigh.storeCheckAutoTareAtTarget()if checkweigh.CheckWeighMode==SIM375 then
checkweigh.CheckAutoTareAtTarget=checkweigh.CheckAutoTareAtTargetDefault
end
checkweigh.storeIntegerValueInTblCheckweighDB("CheckAutoTareAtTarget",checkweigh.CheckAutoTareAtTarget)end
function checkweigh.storeCheckAutoStoreAtTarget()checkweigh.storeIntegerValueInTblCheckweighDB("CheckAutoStoreAtTarget",checkweigh.CheckAutoStoreAtTarget)end
function checkweigh.storeCheckStatPackage()checkweigh.storeIntegerValueInTblCheckweighDB("CheckStatPackage",checkweigh.CheckStatPackage)end
function checkweigh.storePrintTotalFlag()checkweigh.storeIntegerValueInTblCheckweighDB("PrintTotalFlag",checkweigh.PrintTotalFlag)end
function checkweigh.storeTotalFmt()checkweigh.storeIntegerValueInTblCheckweighDB("TotalFmt",checkweigh.TotalFmt)end
function checkweigh.storeClearTotalFlag()checkweigh.storeIntegerValueInTblCheckweighDB("ClearTotalFlag",checkweigh.ClearTotalFlag)end
function checkweigh.storeClearPLUTotalFlag()checkweigh.storeIntegerValueInTblCheckweighDB("ClearPLUTotalFlag",checkweigh.ClearPLUTotalFlag)end
function checkweigh.storeDigitsFlag()checkweigh.storeIntegerValueInTblCheckweighDB("DigitsFlag",checkweigh.DigitsFlag)end
function checkweigh.storeGraphFlag()checkweigh.storeIntegerValueInTblCheckweighDB("GraphFlag",checkweigh.GraphFlag)end
function checkweigh.storePackRunCnt()checkweigh.storeIntegerValueInTblCheckweighDB("PackRunCnt",checkweigh.PackRunCnt)end
function checkweigh.checkweighDBRecall()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighData (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")dbFile:execute("COMMIT")dbFile:close()end
function checkweigh.recallCheckAll()checkweigh.recallCheckgraphEnableFlag()checkweigh.recallCheckBasis()checkweigh.recallUnitsOfMeasureStr()checkweigh.recallUnitsOfMeasure()checkweigh.recallCheckMin()checkweigh.recallCheckTolLo()checkweigh.recallCheckTolHi()checkweigh.recallCheckUnderDiv()checkweigh.recallCheckOverDiv()checkweigh.recallCheckTargLo()checkweigh.recallCheckTarg()checkweigh.recallCheckTargHi()checkweigh.recallCheckMax()checkweigh.recallCheckUnderSegDiv()if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckUnderSegDiv=checkweigh.CheckUnderSegDivDefault
end
checkweigh.recallCheckOverSegDiv()if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckOverSegDiv=checkweigh.CheckOverSegDivDefault
end
checkweigh.recallCheckGraphType()checkweigh.recallCheckOutputType()checkweigh.recallCheckOutputGZB()checkweigh.recallCheckWeighType()checkweigh.recallCheckAutoTareAtTarget()checkweigh.recallCheckAutoStoreAtTarget()checkweigh.recallCheckStatPackage()checkweigh.recallPrintTotalFlag()checkweigh.recallTotalFmt()checkweigh.recallClearTotalFlag()checkweigh.recallClearPLUTotalFlag()checkweigh.recallDigitsFlag()checkweigh.recallGraphFlag()checkweigh.recallPackRunCnt()if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckTolLo=checkweigh.CheckUnderDiv*wt.curDivision
checkweigh.CheckTolHi=checkweigh.CheckOverDiv*wt.curDivision
if checkweigh.CheckWeighType==CHECKWEIGH_TYPE_LIMITS then
checkweigh.CheckTarg=(checkweigh.CheckTargHi+checkweigh.CheckTargLo)/2
else
if checkweigh.CheckTarg<0 then
checkweigh.CheckTargLo=checkweigh.CheckTarg+(checkweigh.CheckTolLo)checkweigh.CheckTargHi=checkweigh.CheckTarg-(checkweigh.CheckTolHi)else
checkweigh.CheckTargLo=checkweigh.CheckTarg-(checkweigh.CheckTolLo)checkweigh.CheckTargHi=checkweigh.CheckTarg+(checkweigh.CheckTolHi)end
end
end
end
function checkweigh.recallCheckTargAll()local found=false
found,checkweigh.CheckTolLo=checkweigh.recallFloatFromCheckDB("CheckTolLo")if found==false then
checkweigh.CheckTolLo=checkweigh.CheckTolLoDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckTolLo",checkweigh.CheckTolLo)end
found=false
found,checkweigh.CheckTolHi=checkweigh.recallFloatFromCheckDB("CheckTolHi")if found==false then
checkweigh.CheckTolHi=checkweigh.CheckTolHiDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckTolHi",checkweigh.CheckTolHi)end
found=false
found,checkweigh.CheckUnderDiv=checkweigh.recallFloatFromCheckDB("CheckUnderDiv")if found==false then
checkweigh.CheckUnderDiv=checkweigh.CheckUnderDivDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckUnderDiv",checkweigh.CheckUnderDiv)end
found=false
found,checkweigh.CheckOverDiv=checkweigh.recallFloatFromCheckDB("CheckOverDiv")if found==false then
checkweigh.CheckOverDiv=checkweigh.CheckOverDivDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckOverDiv",checkweigh.CheckOverDiv)end
found=false
found,checkweigh.CheckTargLo=checkweigh.recallFloatFromCheckDB("CheckTargLo")if found==false then
checkweigh.CheckTargLo=checkweigh.CheckTargLoDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckTargLo",checkweigh.CheckTargLo)end
found=false
found,checkweigh.CheckTarg=checkweigh.recallFloatFromCheckDB("CheckTarg")if found==false then
checkweigh.CheckTarg=checkweigh.CheckTargDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckTarg",checkweigh.CheckTarg)end
found=false
found,checkweigh.CheckTargHi=checkweigh.recallFloatFromCheckDB("CheckTargHi")if found==false then
checkweigh.CheckTargHi=checkweigh.CheckTargHiDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckTargHi",checkweigh.CheckTargHi)end
found=false
found,checkweigh.CheckWeighType=checkweigh.recallIntegerFromCheckDB("CheckWeighType")if found==false then
checkweigh.CheckWeighType=checkweigh.CheckWeighTypeDefault
checkweigh.storeIntegerValueInTblCheckweighDB("CheckWeighType",checkweigh.CheckWeighType)end
if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckTolLo=checkweigh.CheckUnderDiv*wt.curDivision
checkweigh.CheckTolHi=checkweigh.CheckOverDiv*wt.curDivision
if checkweigh.CheckWeighType==CHECKWEIGH_TYPE_LIMITS then
checkweigh.CheckTarg=(checkweigh.CheckTargHi+checkweigh.CheckTargLo)/2
else
if checkweigh.CheckTarg<0 then
checkweigh.CheckTargLo=checkweigh.CheckTarg+(checkweigh.CheckTolLo)checkweigh.CheckTargHi=checkweigh.CheckTarg-(checkweigh.CheckTolHi)else
checkweigh.CheckTargLo=checkweigh.CheckTarg-(checkweigh.CheckTolLo)checkweigh.CheckTargHi=checkweigh.CheckTarg+(checkweigh.CheckTolHi)end
end
end
end
function checkweigh.recallIntegerFromCheckDB(keyName)local found=false
local result=0
local sqlResult,dbFile,strQuery
dbFile=sqlite3.open(DB_FileLocation_AppConfig)sqlResult=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighConfig (varID TEXT PRIMARY KEY, value TEXT)")if sqlResult==0 then
strQuery=string.format("SELECT varID, value FROM tblCheckweighConfig WHERE varID = '%s'",keyName)for row in dbFile:rows(strQuery)do
found=true
result=tonumber(row[2])if found==true then break end
end
end
dbFile:close()return found,result
end
function checkweigh.recallFloatFromCheckDB(keyValName)local found=false
local result=0
local newUnits,oldUnits
newUnits=wt.units
checkweigh.recallUnitsOfMeasure()oldUnits=checkweigh.UnitsOfMeasure
found,result=checkweigh.recallIntegerFromCheckDB(keyValName)if found==true then
if checkweigh.CheckWeighMode==PER375 then
else
result=awtx.weight.convertWeight(oldUnits,result,newUnits,1)end
end
return found,result
end
function checkweigh.recallStringFromCheckDB(keyName)local found=false
local result=0
local sqlResult,dbFile,strQuery
dbFile=sqlite3.open(DB_FileLocation_AppConfig)sqlResult=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighConfig (varID TEXT PRIMARY KEY, value TEXT)")if sqlResult==0 then
strQuery=string.format("SELECT varID, value FROM tblCheckweighConfig WHERE varID = '%s'",keyName)for row in dbFile:rows(strQuery)do
found=true
result=tostring(row[2])if found==true then break end
end
end
dbFile:close()return found,result
end
function checkweigh.recallCheckgraphEnableFlag()local found=false
found,checkweigh.CheckgraphEnableFlag=checkweigh.recallIntegerFromCheckDB("CheckgraphEnableFlag")if found==false then
checkweigh.storeIntegerValueInTblCheckweighDB("CheckgraphEnableFlag",checkweigh.CheckgraphEnableFlagDefault)checkweigh.CheckgraphEnableFlag=checkweigh.CheckgraphEnableFlagDefault
end
end
function checkweigh.recallCheckBasis()local found=false
found,checkweigh.CheckBasis=checkweigh.recallIntegerFromCheckDB("CheckBasis")if found==false then
checkweigh.storeIntegerValueInTblCheckweighDB("CheckBasis",checkweigh.CheckBasisDefault)checkweigh.CheckBasis=checkweigh.CheckBasisDefault
end
end
function checkweigh.recallUnitsOfMeasureStr()local found=false
found,checkweigh.UnitsOfMeasureStr=checkweigh.recallStringFromCheckDB("UnitsOfMeasureStr")if found==false then
checkweigh.UnitsOfMeasureStr=checkweigh.UNITOFMEASUREDefaultStr
checkweigh.storeStringValueInTblCheckweighDB("UnitsOfMeasureStr",checkweigh.UnitsOfMeasureStr)end
end
function checkweigh.recallUnitsOfMeasure()local found=false
found,checkweigh.UnitsOfMeasure=checkweigh.recallIntegerFromCheckDB("UnitsOfMeasure")if found==false then
checkweigh.UnitsOfMeasure=checkweigh.UNITOFMEASUREDefault
checkweigh.storeStringValueInTblCheckweighDB("UnitsOfMeasure",checkweigh.UnitsOfMeasure)end
end
function checkweigh.recallCheckMin()local found=false
found,checkweigh.CheckMin=checkweigh.recallFloatFromCheckDB("CheckMin")if found==false then
checkweigh.CheckMin=checkweigh.CheckMinDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckMin",checkweigh.CheckMin)end
end
function checkweigh.recallCheckTolLo()local found=false
found,checkweigh.CheckTolLo=checkweigh.recallFloatFromCheckDB("CheckTolLo")if found==false then
checkweigh.CheckTolLo=checkweigh.CheckTolLoDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckTolLo",checkweigh.CheckTolLo)end
end
function checkweigh.recallCheckTolHi()local found=false
found,checkweigh.CheckTolHi=checkweigh.recallFloatFromCheckDB("CheckTolHi")if found==false then
checkweigh.CheckTolHi=checkweigh.CheckTolHiDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckTolHi",checkweigh.CheckTolHi)end
end
function checkweigh.recallCheckUnderDiv()local found=false
found,checkweigh.CheckUnderDiv=checkweigh.recallFloatFromCheckDB("CheckUnderDiv")if found==false then
checkweigh.CheckUnderDiv=checkweigh.CheckUnderDivDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckUnderDiv",checkweigh.CheckUnderDiv)end
if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckTolLo=checkweigh.CheckUnderDiv*wt.curDivision
if checkweigh.CheckWeighType==CHECKWEIGH_TYPE_LIMITS then
else
if checkweigh.CheckTarg<0 then
checkweigh.CheckTargLo=checkweigh.CheckTarg+(checkweigh.CheckTolLo)else
checkweigh.CheckTargLo=checkweigh.CheckTarg-(checkweigh.CheckTolLo)end
end
end
end
function checkweigh.recallCheckOverDiv()local found=false
found,checkweigh.CheckOverDiv=checkweigh.recallFloatFromCheckDB("CheckOverDiv")if found==false then
checkweigh.CheckOverDiv=checkweigh.CheckOverDivDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckOverDiv",checkweigh.CheckOverDiv)end
if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckTolHi=checkweigh.CheckOverDiv*wt.curDivision
if checkweigh.CheckWeighType==CHECKWEIGH_TYPE_LIMITS then
else
if checkweigh.CheckTarg<0 then
checkweigh.CheckTargHi=checkweigh.CheckTarg-(checkweigh.CheckTolHi)else
checkweigh.CheckTargHi=checkweigh.CheckTarg+(checkweigh.CheckTolHi)end
end
end
end
function checkweigh.recallCheckTargLo()local found=false
found,checkweigh.CheckTargLo=checkweigh.recallFloatFromCheckDB("CheckTargLo")if found==false then
checkweigh.CheckTargLo=checkweigh.CheckTargLoDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckTargLo",checkweigh.CheckTargLo)end
end
function checkweigh.recallCheckTarg()local found=false
found,checkweigh.CheckTarg=checkweigh.recallFloatFromCheckDB("CheckTarg")if found==false then
checkweigh.CheckTarg=checkweigh.CheckTargDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckTarg",checkweigh.CheckTarg)end
end
function checkweigh.recallCheckTargHi()local found=false
found,checkweigh.CheckTargHi=checkweigh.recallFloatFromCheckDB("CheckTargHi")if found==false then
checkweigh.CheckTargHi=checkweigh.CheckTargHiDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckTargHi",checkweigh.CheckTargHi)end
end
function checkweigh.recallCheckMax()local found=false
found,checkweigh.CheckMax=checkweigh.recallFloatFromCheckDB("CheckMax")if found==false then
checkweigh.CheckMax=checkweigh.CheckMaxDefault
checkweigh.storeFloatValueInTblCheckweighDB("CheckMax",checkweigh.CheckMax)end
end
function checkweigh.recallCheckUnderSegDiv()local found=false
found,checkweigh.CheckUnderSegDiv=checkweigh.recallIntegerFromCheckDB("CheckUnderSegDiv")if found==false then
checkweigh.CheckUnderSegDiv=checkweigh.CheckUnderSegDivDefault
checkweigh.storeIntegerValueInTblCheckweighDB("CheckUnderSegDiv",checkweigh.CheckUnderSegDiv)end
if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckUnderSegDiv=checkweigh.CheckUnderSegDivDefault
end
end
function checkweigh.recallCheckOverSegDiv()local found=false
found,checkweigh.CheckOverSegDiv=checkweigh.recallIntegerFromCheckDB("CheckOverSegDiv")if found==false then
checkweigh.CheckOverSegDiv=checkweigh.CheckOverSegDivDefault
checkweigh.storeIntegerValueInTblCheckweighDB("CheckOverSegDiv",checkweigh.CheckOverSegDiv)end
if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckOverSegDiv=checkweigh.CheckOverSegDivDefault
end
end
function checkweigh.recallCheckGraphType()local found=false
found,checkweigh.CheckGraphType=checkweigh.recallIntegerFromCheckDB("CheckGraphType")if found==false then
checkweigh.CheckGraphType=checkweigh.CheckGraphTypeDefault
checkweigh.storeIntegerValueInTblCheckweighDB("CheckGraphType",checkweigh.CheckGraphType)end
end
function checkweigh.recallCheckOutputType()local found=false
found,checkweigh.CheckOutputType=checkweigh.recallIntegerFromCheckDB("CheckOutputType")if found==false then
checkweigh.CheckOutputType=checkweigh.CheckOutputTypeDefault
checkweigh.storeIntegerValueInTblCheckweighDB("CheckOutputType",checkweigh.CheckOutputType)end
end
function checkweigh.recallCheckOutputGZB()local found=false
found,checkweigh.CheckOutputGZB=checkweigh.recallIntegerFromCheckDB("CheckOutputGZB")if found==false then
checkweigh.CheckOutputGZB=checkweigh.CheckOutputGZBDefault
checkweigh.storeIntegerValueInTblCheckweighDB("CheckOutputGZB",checkweigh.CheckOutputGZB)end
end
function checkweigh.recallCheckWeighType()local found=false
found,checkweigh.CheckWeighType=checkweigh.recallIntegerFromCheckDB("CheckWeighType")if found==false then
checkweigh.CheckWeighType=checkweigh.CheckWeighTypeDefault
checkweigh.storeIntegerValueInTblCheckweighDB("CheckWeighType",checkweigh.CheckWeighType)end
end
function checkweigh.recallCheckAutoTareAtTarget()local found=false
found,checkweigh.CheckAutoTareAtTarget=checkweigh.recallIntegerFromCheckDB("CheckAutoTareAtTarget")if found==false then
checkweigh.CheckAutoTareAtTarget=checkweigh.CheckAutoTareAtTargetDefault
checkweigh.storeIntegerValueInTblCheckweighDB("CheckAutoTareAtTarget",checkweigh.CheckAutoTareAtTarget)end
if checkweigh.CheckWeighMode==SIM375 then
checkweigh.CheckAutoTareAtTarget=checkweigh.CheckAutoTareAtTargetDefault
end
end
function checkweigh.recallCheckAutoStoreAtTarget()local found=false
found,checkweigh.CheckAutoStoreAtTarget=checkweigh.recallIntegerFromCheckDB("CheckAutoStoreAtTarget")if found==false then
checkweigh.CheckAutoStoreAtTarget=checkweigh.CheckAutoStoreAtTargetDefault
checkweigh.storeIntegerValueInTblCheckweighDB("CheckAutoStoreAtTarget",checkweigh.CheckAutoStoreAtTarget)end
end
function checkweigh.recallCheckStatPackage()local found=false
found,checkweigh.CheckStatPackage=checkweigh.recallIntegerFromCheckDB("CheckStatPackage")if found==false then
checkweigh.CheckStatPackage=checkweigh.CheckStatPackageDefault
checkweigh.storeIntegerValueInTblCheckweighDB("CheckStatPackage",checkweigh.CheckStatPackage)end
end
function checkweigh.recallPrintTotalFlag()local found=false
found,checkweigh.PrintTotalFlag=checkweigh.recallIntegerFromCheckDB("PrintTotalFlag")if found==false then
checkweigh.PrintTotalFlag=checkweigh.PrintTotalFlagDefault
checkweigh.storeIntegerValueInTblCheckweighDB("PrintTotalFlag",checkweigh.PrintTotalFlag)end
end
function checkweigh.recallTotalFmt()local found=false
found,checkweigh.TotalFmt=checkweigh.recallIntegerFromCheckDB("TotalFmt")if found==false then
checkweigh.TotalFmt=checkweigh.TotalFmtDefault
checkweigh.storeIntegerValueInTblCheckweighDB("TotalFmt",checkweigh.TotalFmt)end
end
function checkweigh.recallClearTotalFlag()local found=false
found,checkweigh.ClearTotalFlag=checkweigh.recallIntegerFromCheckDB("ClearTotalFlag")if found==false then
checkweigh.ClearTotalFlag=checkweigh.ClearTotalFlagDefault
checkweigh.storeIntegerValueInTblCheckweighDB("ClearTotalFlag",checkweigh.ClearTotalFlag)end
end
function checkweigh.recallClearPLUTotalFlag()local found=false
found,checkweigh.ClearPLUTotalFlag=checkweigh.recallIntegerFromCheckDB("ClearPLUTotalFlag")if found==false then
checkweigh.ClearPLUTotalFlag=checkweigh.ClearPLUTotalFlagDefault
checkweigh.storeIntegerValueInTblCheckweighDB("ClearPLUTotalFlag",checkweigh.ClearPLUTotalFlag)end
end
function checkweigh.recallDigitsFlag()local found=false
found,checkweigh.DigitsFlag=checkweigh.recallIntegerFromCheckDB("DigitsFlag")if found==false then
checkweigh.DigitsFlag=checkweigh.DigitsFlagDefault
checkweigh.storeIntegerValueInTblCheckweighDB("DigitsFlag",checkweigh.DigitsFlag)end
end
function checkweigh.recallGraphFlag()local found=false
found,checkweigh.GraphFlag=checkweigh.recallIntegerFromCheckDB("GraphFlag")if found==false then
checkweigh.GraphFlag=checkweigh.GraphFlagDefault
checkweigh.storeIntegerValueInTblCheckweighDB("GraphFlag",checkweigh.GraphFlag)end
end
function checkweigh.recallPackRunCnt()local found=false
found,checkweigh.PackRunCnt=checkweigh.recallIntegerFromCheckDB("PackRunCnt")if found==false then
checkweigh.PackRunCnt=checkweigh.PackRunCntDefault
checkweigh.storeIntegerValueInTblCheckweighDB("PackRunCnt",checkweigh.PackRunCnt)end
end
function checkweigh.checkweighDBClear()checkweigh.checkweighDBInit()checkweigh.checkweighDBStore()checkweigh.checkweighDBRecall()end
function checkweigh.transactionStore()local dbFile,result,sqlStr
local found=false
transaction.TransactionID=0
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighTransactions (TransactionCount INTEGER PRIMARY KEY, TransactionPLUNumber INTEGER, TransactionTimeDate TEXT, TransactionSysCount INTEGER, TransactionGrossWt DOUBLE, TransactionNetWt DOUBLE, TransactionUofM TEXT, TransactionUAO TEXT, TransactionID INTEGER)")dbFile:execute("BEGIN TRANSACTION")sqlStr=string.format("SELECT * FROM tblCheckweighTransactions WHERE (TransactionID = 0)")found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblCheckweighTransactions (TransactionCount, TransactionPLUNumber, TransactionTimeDate, TransactionSysCount, TransactionGrossWt, TransactionNetWt, TransactionUofM, TransactionUAO, TransactionID) VALUES ('%d', '%d', '%s', '%d', '%f', '%f', '%s', '%s', '%d')",transaction.TransactionCount,transaction.TransactionPLUNumber,transaction.TransactionTimeDate,transaction.TransactionSysCount,transaction.TransGrossWtTotal,transaction.TransNetWtTotal,transaction.TransactionUofM,transaction.TransactionUAO,transaction.TransactionID)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE OR REPLACE tblCheckweighTransactions SET TransactionCount = '%d', TransactionPLUNumber = '%d', TransactionTimeDate = '%s', TransactionSysCount = '%d', TransactionGrossWt = '%f', TransactionNetWt = '%f', TransactionUofM = '%s', TransactionUAO = '%s', TransactionID = '%d' WHERE TransactionID = 0",transaction.TransactionCount,transaction.TransactionPLUNumber,transaction.TransactionTimeDate,transaction.TransactionSysCount,transaction.TransGrossWtTotal,transaction.TransNetWtTotal,transaction.TransactionUofM,transaction.TransactionUAO,transaction.TransactionID)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()checkweigh.lastTransactionCountStore()checkweigh.lastTransactionSysCountStore()end
function checkweigh.transactionRecall()local dbFile,result,sqlStr
transaction.TransactionID=0
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighTransactions (TransactionCount INTEGER PRIMARY KEY, TransactionPLUNumber INTEGER, TransactionTimeDate TEXT, TransactionSysCount INTEGER, TransactionGrossWt DOUBLE, TransactionNetWt DOUBLE, TransactionUofM TEXT, TransactionUAO TEXT, TransactionID INTEGER)")sqlStr=string.format("SELECT * FROM tblCheckweighTransactions WHERE (TransactionID = 0)")found=false
for row in dbFile:rows(sqlStr)do
found=true
transaction.TransactionCount=(row[1])transaction.TransactionPLUNumber=(row[2])transaction.TransactionTimeDate=(row[3])transaction.TransactionSysCount=(row[4])transaction.TransGrossWtTotal=(row[5])transaction.TransNetWtTotal=(row[6])transaction.TransactionUofM=(row[7])transaction.TransactionUAO=(row[8])transaction.TransactionID=(row[9])end
dbFile:close()if found==false then
checkweigh.newClrAccum()checkweigh.transactionStore()end
checkweigh.lastTransactionCountRecall()checkweigh.lastTransactionSysCountRecall()end
function checkweigh.lastTransactionCountStore()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblLastTransactionCount (TransactionCount INTEGER)")dbFile:execute("BEGIN TRANSACTION")sqlStr=string.format("SELECT TransactionCount FROM tblLastTransactionCount")found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblLastTransactionCount (TransactionCount) VALUES ('%d')",transaction.TransactionCount)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblLastTransactionCount SET TransactionCount = '%d'",transaction.TransactionCount)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()end
function checkweigh.lastTransactionCountRecall()local dbFile,result,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblLastTransactionCount (TransactionCount INTEGER)")sqlStr=string.format("SELECT TransactionCount FROM tblLastTransactionCount")for row in dbFile:rows(sqlStr)do
transaction.TransactionCount=row[1]end
dbFile:close()end
function checkweigh.lastTransactionSysCountStore()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblLastTransactionSysCount (TransactionSysCount INTEGER)")dbFile:execute("BEGIN TRANSACTION")sqlStr=string.format("SELECT tblLastTransactionSysCount.TransactionSysCount FROM tblLastTransactionSysCount")found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblLastTransactionSysCount (TransactionSysCount) VALUES ('%d')",transaction.TransactionSysCount)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblLastTransactionSysCount SET TransactionSysCount = '%d'",transaction.TransactionSysCount)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()end
function checkweigh.lastTransactionSysCountRecall()local dbFile,result,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblLastTransactionSysCount (TransactionSysCount INTEGER)")sqlStr=string.format("SELECT TransactionSysCount FROM tblLastTransactionSysCount")for row in dbFile:rows(sqlStr)do
transaction.TransactionSysCount=row[1]end
dbFile:close()end
function checkweigh.doPrintPopulate()if checkweigh.CheckTarg<0 then
if wt.net>checkweigh.CheckTargLo then
checkweighBand=UNDER_STR
checkweighBand1=REJECT_STR
checkweigh.XR4500=XR4500_RED
elseif wt.net<=checkweigh.CheckTargLo and wt.net>=checkweigh.CheckTargHi then
checkweighBand=ACCEPT_STR
checkweighBand1=ACCEPT_STR
checkweigh.XR4500=XR4500_GRN
else
checkweighBand=OVER_STR
checkweighBand1=REJECT_STR
checkweigh.XR4500=XR4500_RED
end
else
if wt.net<checkweigh.CheckTargLo then
checkweighBand=UNDER_STR
checkweighBand1=REJECT_STR
checkweigh.XR4500=XR4500_RED
elseif wt.net>=checkweigh.CheckTargLo and wt.net<=checkweigh.CheckTargHi then
checkweighBand=ACCEPT_STR
checkweighBand1=ACCEPT_STR
checkweigh.XR4500=XR4500_GRN
else
checkweighBand=OVER_STR
checkweighBand1=REJECT_STR
checkweigh.XR4500=XR4500_RED
end
end
checkweigh.setCheckPrintTokens()transaction.setTransPrintTokens()checkweigh.setXR4500PrintToken()end
function checkweigh.doPrintPopulatePercent()if checkweigh.CheckTarg<0 then
if wt.percent>checkweigh.CheckTargLo then
checkweighBand=UNDER_STR
checkweighBand1=REJECT_STR
checkweigh.XR4500=XR4500_RED
elseif wt.percent<=checkweigh.CheckTargLo and wt.percent>=checkweigh.CheckTargHi then
checkweighBand=ACCEPT_STR
checkweighBand1=ACCEPT_STR
checkweigh.XR4500=XR4500_GRN
else
checkweighBand=OVER_STR
checkweighBand1=REJECT_STR
checkweigh.XR4500=XR4500_RED
end
else
if wt.percent<checkweigh.CheckTargLo then
checkweighBand=UNDER_STR
checkweighBand1=REJECT_STR
checkweigh.XR4500=XR4500_RED
elseif wt.percent>=checkweigh.CheckTargLo and wt.percent<=checkweigh.CheckTargHi then
checkweighBand=ACCEPT_STR
checkweighBand1=ACCEPT_STR
checkweigh.XR4500=XR4500_GRN
else
checkweighBand=OVER_STR
checkweighBand1=REJECT_STR
checkweigh.XR4500=XR4500_RED
end
end
checkweigh.setCheckPrintTokens()transaction.setTransPrintTokens()checkweigh.setXR4500PrintToken()end
function checkweigh.doStandardDeviation()Stat.meanNetwt=stats.mean(Stat.Netwt)Stat.medianNetwt=stats.median(Stat.Netwt)Stat.sdNetwt=stats.standardDeviation(Stat.Netwt)Stat.covNetwt=Stat.sdNetwt/Stat.meanNetwt
Stat.maxNetwt,Stat.minNetwt,Stat.cntNetwt=stats.maxmincount(Stat.Netwt)Stat.maxUnderwt,Stat.minUnderwt,Stat.cntUnderwt=stats.maxmincount(Stat.Underwt)Stat.maxTargetwt,Stat.minTargetwt,Stat.cntTargetwt=stats.maxmincount(Stat.Targetwt)Stat.maxOverwt,Stat.minOverwt,Stat.cntOverwt=stats.maxmincount(Stat.Overwt)Stat.xbarMsg=XBAR_MSG0
Stat.setStatsPrintTokens()end
function checkweigh.doXBar()Stat.meanNetwt=stats.mean(Stat.Netwt)Stat.medianNetwt=stats.median(Stat.Netwt)Stat.sdNetwt=stats.standardDeviation(Stat.Netwt)Stat.covNetwt=Stat.sdNetwt/Stat.meanNetwt
Stat.maxNetwt,Stat.minNetwt,Stat.cntNetwt=stats.maxmincount(Stat.Netwt)Stat.maxUnderwt,Stat.minUnderwt,Stat.cntUnderwt=stats.maxmincount(Stat.Underwt)Stat.maxTargetwt,Stat.minTargetwt,Stat.cntTargetwt=stats.maxmincount(Stat.Targetwt)Stat.maxOverwt,Stat.minOverwt,Stat.cntOverwt=stats.maxmincount(Stat.Overwt)Stat.xbarLimit=checkweigh.CheckTolHi
Stat.xbarCnt=Stat.xbarCnt+1
Stat.xbar=Stat.meanNetwt
Stat.xbarR=Stat.maxNetwt-Stat.minNetwt
Stat.xbarAverage[math.mod(Stat.xbarCnt,8)]=Stat.xbar
Stat.xbarMsg=XBAR_MSG0
if math.abs(Stat.xbar-checkweigh.CheckTarg)>(3*Stat.xbarLimit)then
Stat.xbarMsg=XBAR_MSG1
elseif Stat.xbarCnt>=3 then
tmpAverageCount=0
if Stat.xbarCnt>=8 then
tmpLoopCount=8
else
tmpLoopCount=Stat.xbarCnt
end
for i=0,(tmpLoopCount-1)do
tmpIndex=math.mod(Stat.xbarCnt-i,8)if math.abs(Stat.xbarAverage[tmpIndex]-checkweigh.CheckTarg)>Stat.xbarLimit then
tmpAverageCount=tmpAverageCount+1
end
if(i+1==3)and(tmpAverageCount>=2)then
Stat.xbarMsg=XBAR_MSG2
break
end
if(i+1==5)and(tmpAverageCount>=4)then
Stat.xbarMsg=XBAR_MSG3
break
end
if(i+1==8)and(tmpAverageCount>=8)then
Stat.xbarMsg=XBAR_MSG4
break
end
end
end
Stat.setStatsPrintTokens()end
function checkweigh.doStatOther()Stat.meanNetwt=stats.mean(Stat.Netwt)Stat.medianNetwt=stats.median(Stat.Netwt)Stat.sdNetwt=stats.standardDeviation(Stat.Netwt)Stat.covNetwt=Stat.sdNetwt/Stat.meanNetwt
Stat.maxNetwt,Stat.minNetwt,Stat.cntNetwt=stats.maxmincount(Stat.Netwt)Stat.maxUnderwt,Stat.minUnderwt,Stat.cntUnderwt=stats.maxmincount(Stat.Underwt)Stat.maxTargetwt,Stat.minTargetwt,Stat.cntTargetwt=stats.maxmincount(Stat.Targetwt)Stat.maxOverwt,Stat.minOverwt,Stat.cntOverwt=stats.maxmincount(Stat.Overwt)Stat.xbarMsg=XBAR_MSG0
Stat.setStatsPrintTokens()end
function checkweigh.clrStatsPackage()Stat.Netwt={}Stat.Underwt={}Stat.Targetwt={}Stat.Overwt={}for k in pairs(Stat.Netwt)do
Stat.Netwt[k]=nil
end
for k in pairs(Stat.Underwt)do
Stat.Netwt[k]=nil
end
for k in pairs(Stat.Targetwt)do
Stat.Netwt[k]=nil
end
for k in pairs(Stat.Overwt)do
Stat.Netwt[k]=nil
end
Stat.meanNetwt=0
Stat.medianNetwt=0
Stat.sdNetwt=0
Stat.covNetwt=0
Stat.maxNetwt=0
Stat.minNetwt=0
Stat.cntNetwt=0
Stat.maxUnderwt=0
Stat.minUnderwt=0
Stat.cntUnderwt=0
Stat.maxTargetwt=0
Stat.minTargetwt=0
Stat.cntTargetwt=0
Stat.maxOverwt=0
Stat.minOverwt=0
Stat.cntOverwt=0
end
function checkweigh.clrXBarRPackage()Stat.xbarLimit=checkweigh.CheckTolHi
Stat.xbarCnt=0
Stat.xbar=0
Stat.xbarR=0
Stat.xbarAverage={}Stat.xbarMsg=XBAR_MSG0
end
function checkweigh.doStatsPackage()local usermode,currentRPN
if checkweigh.CheckStatPackage==0 then
elseif checkweigh.CheckStatPackage==1 then
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("STD DEV")checkweigh.doStandardDeviation()awtx.os.sleep(displaytime)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)elseif checkweigh.CheckStatPackage==2 then
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("X-BAR")checkweigh.doXBar()awtx.os.sleep(displaytime)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)else
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("OTHER")checkweigh.doStatOther()awtx.os.sleep(displaytime)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
end
function checkweigh.doTransaction()local usermode,currentRPN
transaction.TransactionPLUNumber=0
transaction.TransactionTimeDate=doMyDateTime()transaction.TransactionCount=transaction.TransactionCount+1
transaction.TransactionSysCount=transaction.TransactionSysCount+1
transaction.TransactionGrossWt=wt.gross
transaction.TransactionNetWt=wt.net
transaction.TransGrossWtTotal=transaction.TransGrossWtTotal+wt.gross
transaction.TransNetWtTotal=transaction.TransNetWtTotal+wt.net
transaction.TransCountTotal=transaction.TransCountTotal+wt.count
transaction.TransactionUofM=wt.unitsStr
awtx.weight.setAccum(transaction.TransGrossWtTotal,transaction.TransNetWtTotal,transaction.TransCountTotal,transaction.TransactionCount)Stat.Netwt[tmpPackRunCnt]=transaction.TransactionNetWt
if wt.net<checkweigh.CheckTargLo then
transaction.TransactionUAO="0"Stat.Underwt[tmpPackRunCnt]=transaction.TransactionNetWt
elseif wt.net>=checkweigh.CheckTargLo and wt.net<=checkweigh.CheckTargHi then
transaction.TransactionUAO="1"Stat.Targetwt[tmpPackRunCnt]=transaction.TransactionNetWt
else
transaction.TransactionUAO="2"Stat.Overwt[tmpPackRunCnt]=transaction.TransactionNetWt
end
if ID.id==nil then
transaction.TransactionID=0
else
transaction.TransactionID=ID.id
end
if checkweigh.StoreBeforePrint==true then
checkweigh.transactionStore()end
end
function checkweigh.doTransactionWithPLU()local usermode,currentRPN
if plu.PLUNumber==nil then
transaction.TransactionPLUNumber=0
else
transaction.TransactionPLUNumber=plu.PLUNumber
end
transaction.TransactionTimeDate=doMyDateTime()transaction.TransactionCount=transaction.TransactionCount+1
transaction.TransactionSysCount=transaction.TransactionSysCount+1
transaction.TransactionGrossWt=wt.gross
transaction.TransactionNetWt=wt.net
transaction.TransGrossWtTotal=transaction.TransGrossWtTotal+wt.gross
transaction.TransNetWtTotal=transaction.TransNetWtTotal+wt.net
transaction.TransCountTotal=transaction.TransCountTotal+wt.count
transaction.TransactionUofM=wt.unitsStr
awtx.weight.setAccum(transaction.TransGrossWtTotal,transaction.TransNetWtTotal,transaction.TransCountTotal,transaction.TransactionCount)Stat.Netwt[tmpPackRunCnt]=transaction.TransactionNetWt
if wt.net<checkweigh.CheckTargLo then
transaction.TransactionUAO="0"Stat.Underwt[tmpPackRunCnt]=transaction.TransactionNetWt
elseif wt.net>=checkweigh.CheckTargLo and wt.net<=checkweigh.CheckTargHi then
transaction.TransactionUAO="1"Stat.Targetwt[tmpPackRunCnt]=transaction.TransactionNetWt
else
transaction.TransactionUAO="2"Stat.Overwt[tmpPackRunCnt]=transaction.TransactionNetWt
end
if ID.id==nil then
transaction.TransactionID=0
else
transaction.TransactionID=ID.id
end
if checkweigh.StoreBeforePrint==true then
checkweigh.transactionStore()end
end
function checkweigh.doPercentTransaction()local usermode,currentRPN
transaction.TransactionPLUNumber=0
transaction.TransactionTimeDate=doMyDateTime()transaction.TransactionCount=transaction.TransactionCount+1
transaction.TransactionSysCount=transaction.TransactionSysCount+1
transaction.TransactionGrossWt=wt.gross
transaction.TransactionNetWt=wt.net
transaction.TransGrossWtTotal=transaction.TransGrossWtTotal+wt.gross
transaction.TransNetWtTotal=transaction.TransNetWtTotal+wt.net
transaction.TransCountTotal=transaction.TransCountTotal+wt.count
transaction.TransactionUofM=wt.unitsStr
awtx.weight.setAccum(transaction.TransGrossWtTotal,transaction.TransNetWtTotal,transaction.TransCountTotal,transaction.TransactionCount)Stat.Netwt[tmpPackRunCnt]=transaction.TransactionNetWt
if wt.percent<checkweigh.CheckTargLo then
transaction.TransactionUAO="0"Stat.Underwt[tmpPackRunCnt]=transaction.TransactionNetWt
elseif wt.percent>=checkweigh.CheckTargLo and wt.percent<=checkweigh.CheckTargHi then
transaction.TransactionUAO="1"Stat.Targetwt[tmpPackRunCnt]=transaction.TransactionNetWt
else
transaction.TransactionUAO="2"Stat.Overwt[tmpPackRunCnt]=transaction.TransactionNetWt
end
if ID.id==nil then
transaction.TransactionID=0
else
transaction.TransactionID=ID.id
end
if checkweigh.StoreBeforePrint==true then
checkweigh.transactionStore()end
end
function checkweigh.doPercentTransactionWithPLU()local usermode,currentRPN
if plu.PLUNumber==nil then
transaction.TransactionPLUNumber=0
else
transaction.TransactionPLUNumber=plu.PLUNumber
end
transaction.TransactionTimeDate=doMyDateTime()transaction.TransactionCount=transaction.TransactionCount+1
transaction.TransactionSysCount=transaction.TransactionSysCount+1
transaction.TransactionGrossWt=wt.gross
transaction.TransactionNetWt=wt.net
transaction.TransGrossWtTotal=transaction.TransGrossWtTotal+wt.gross
transaction.TransNetWtTotal=transaction.TransNetWtTotal+wt.net
transaction.TransCountTotal=transaction.TransCountTotal+wt.count
transaction.TransactionUofM=wt.unitsStr
awtx.weight.setAccum(transaction.TransGrossWtTotal,transaction.TransNetWtTotal,transaction.TransCountTotal,transaction.TransactionCount)Stat.Netwt[tmpPackRunCnt]=transaction.TransactionNetWt
if wt.percent<checkweigh.CheckTargLo then
transaction.TransactionUAO="0"Stat.Underwt[tmpPackRunCnt]=transaction.TransactionNetWt
elseif wt.percent>=checkweigh.CheckTargLo and wt.percent<=checkweigh.CheckTargHi then
transaction.TransactionUAO="1"Stat.Targetwt[tmpPackRunCnt]=transaction.TransactionNetWt
else
transaction.TransactionUAO="2"Stat.Overwt[tmpPackRunCnt]=transaction.TransactionNetWt
end
if ID.id==nil then
transaction.TransactionID=0
else
transaction.TransactionID=ID.id
end
if checkweigh.StoreBeforePrint==true then
checkweigh.transactionStore()end
end
function checkweigh.newClrAccum()transaction.TransactionCount=0
transaction.TransactionPLUNumber=0
transaction.TransactionTimeDate=doMyDateTime()transaction.TransactionSysCount=0
transaction.TransactionGrossWt=0
transaction.TransactionNetWt=0
transaction.TransGrossWtTotal=0
transaction.TransNetWtTotal=0
transaction.TransCountTotal=0
transaction.TransactionUofM=wt.unitsStr
transaction.TransactionUAO=0
transaction.TransactionID=0
awtx.weight.setAccum(transaction.TransGrossWtTotal,transaction.TransNetWtTotal,transaction.TransCountTotal,transaction.TransactionCount)end
function checkweigh.setUnitsOfMeasure(newUnits)awtx.weight.setCurrentUnits(newUnits)wt=awtx.weight.getCurrent(1)checkweigh.UnitsOfMeasureStr=wt.unitsStr
checkweigh.UnitsOfMeasure=wt.units
checkweigh.storeUnitsOfMeasureStr()checkweigh.storeUnitsOfMeasure()end
function checkweigh.editCheckBasis(label)local newBasis,isEnterKey
checkweigh.recallCheckBasis()newBasis=checkweigh.CheckBasis
if newBasis==VAL_COUNT then
newBasis=2
end
newBasis,isEnterKey=awtx.keypad.selectList("GROSS,NET,COUNT",newBasis)awtx.display.writeLine(label)if isEnterKey then
if newBasis==2 then
newBasis=VAL_COUNT
end
checkweigh.CheckBasis=newBasis
checkweigh.storeCheckBasis()checkweigh.refreshCheckGraph()else
end
end
function checkweigh.editCheckMin(label)local newMin,Minmin,Minmax,strUnits,isEnterKey
checkweigh.recallCheckMin()newMin=checkweigh.CheckMin
if checkweigh.CheckWeighMode==PER375 then
Minmin=-100
Minmax=100
strUnits="pc"else
wt=awtx.weight.getCurrent(1)Minmin=wt.curCapacity*-1
Minmax=wt.curCapacity
strUnits=wt.unitsStr
end
newMin,isEnterKey=awtx.keypad.enterWeightWithUnits(newMin,Minmin,Minmax,strUnits,separatorChar)awtx.display.writeLine(label)if isEnterKey then
checkweigh.CheckMin=newMin
checkweigh.storeCheckMin()checkweigh.refreshCheckGraph()else
end
end
function checkweigh.editCheckMax(label)local newMax,Maxmin,Maxmax,strUnits,isEnterKey
checkweigh.recallCheckMax()newMax=checkweigh.CheckMax
if checkweigh.CheckWeighMode==PER375 then
Maxmin=-100
Maxmax=100
strUnits="pc"else
wt=awtx.weight.getCurrent(1)Maxmin=wt.curCapacity*-1
Maxmax=wt.curCapacity
strUnits=wt.unitsStr
end
newMax,isEnterKey=awtx.keypad.enterWeightWithUnits(newMax,Maxmin,Maxmax,strUnits,separatorChar)awtx.display.writeLine(label)if isEnterKey then
checkweigh.CheckMax=newMax
checkweigh.storeCheckMax()checkweigh.refreshCheckGraph()else
end
end
function checkweigh.editCheckUnderSegDiv(label)local newUnderSegDiv,UnderDivSegmin,UnderDivSegmax,isEnterKey
checkweigh.recallCheckUnderSegDiv()newUnderSegDiv=checkweigh.CheckUnderSegDiv
UnderDivSegmin=1
UnderDivSegmax=1000
newUnderSegDiv,isEnterKey=awtx.keypad.enterInteger(newUnderSegDiv,UnderDivSegmin,UnderDivSegmax)awtx.display.writeLine(label)if isEnterKey then
checkweigh.setCheckUnderSegDiv(newUnderSegDiv)else
end
end
function checkweigh.setCheckUnderSegDiv(newUnderSegDiv)checkweigh.CheckUnderSegDiv=newUnderSegDiv
checkweigh.storeCheckUnderSegDiv()checkweigh.refreshCheckGraph()end
function checkweigh.editCheckOverSegDiv(label)local newOverSegDiv,OverDivSegmin,OverDivSegmax,isEnterKey
checkweigh.recallCheckOverSegDiv()newOverSegDiv=checkweigh.CheckOverSegDiv
OverDivSegmin=1
OverDivSegmax=1000
newOverSegDiv,isEnterKey=awtx.keypad.enterInteger(newOverSegDiv,OverDivSegmin,OverDivSegmax)awtx.display.writeLine(label)if isEnterKey then
checkweigh.setCheckOverSegDiv(newOverSegDiv)else
end
end
function checkweigh.setCheckOverSegDiv(newOverSegDiv)checkweigh.CheckOverSegDiv=newOverSegDiv
checkweigh.storeCheckOverSegDiv()checkweigh.refreshCheckGraph()end
function checkweigh.editCheckGraphType(label)local newGraphType,isEnterKey
checkweigh.recallCheckGraphType()newGraphType=checkweigh.CheckGraphType
newGraphType,isEnterKey=awtx.keypad.selectList("DISABLE,CHECK, BAR",newGraphType)awtx.display.writeLine(label)if isEnterKey then
checkweigh.CheckGraphType=newGraphType
checkweigh.storeCheckGraphType()checkweigh.refreshCheckGraph()else
end
end
function checkweigh.editCheckOutputType(label)local newOutputType,isEnterKey
checkweigh.recallCheckOutputType()newOutputType=checkweigh.CheckOutputType
newOutputType,isEnterKey=awtx.keypad.selectList("LATCH,UNLATCH",newOutputType)awtx.display.writeLine(label)if isEnterKey then
checkweigh.CheckOutputType=newOutputType
checkweigh.storeCheckOutputType()checkweigh.refreshCheckGraph()else
end
end
function checkweigh.editCheckOutputGZB(label)local newOutputGZB,isEnterKey
checkweigh.recallCheckOutputGZB()newOutputGZB=checkweigh.CheckOutputGZB
newOutputGZB,isEnterKey=awtx.keypad.selectList("ON,OFF",newOutputGZB)awtx.display.writeLine(label)if isEnterKey then
checkweigh.CheckOutputGZB=newOutputGZB
checkweigh.storeCheckOutputGZB()checkweigh.refreshCheckGraph()else
end
end
function checkweigh.editCheckWeighType(label)local newType,isEnterKey
checkweigh.recallCheckWeighType()newType=checkweigh.CheckWeighType
newType,isEnterKey=awtx.keypad.selectList("LIMITS,SAMPLE",newType)awtx.display.writeLine(label)if isEnterKey then
checkweigh.setCheckWeighType(newType)else
end
end
function checkweigh.setCheckWeighType(newType)if newType>=0 and newType<=1 then
checkweigh.CheckWeighType=newType
checkweigh.storeCheckWeighType()else
end
end
function checkweigh.editCheckAutoTareAtTarget(label)local newAutoTareAtTarget,isEnterKey
checkweigh.recallCheckAutoTareAtTarget()newAutoTareAtTarget=checkweigh.CheckAutoTareAtTarget
newAutoTareAtTarget,isEnterKey=awtx.keypad.selectList("OFF,ON",newAutoTareAtTarget)awtx.display.writeLine(label)if isEnterKey then
checkweigh.CheckAutoTareAtTarget=newAutoTareAtTarget
checkweigh.storeCheckAutoTareAtTarget()else
end
end
function checkweigh.editAutoStoreAtTarget(label)local newAutoStoreAtTarget,isEnterKey
checkweigh.recallCheckAutoStoreAtTarget()newAutoStoreAtTarget=checkweigh.CheckAutoStoreAtTarget
newAutoStoreAtTarget,isEnterKey=awtx.keypad.selectList("OFF,ON",newAutoStoreAtTarget)awtx.display.writeLine(label)if isEnterKey then
checkweigh.CheckAutoStoreAtTarget=newAutoStoreAtTarget
checkweigh.storeCheckAutoStoreAtTarget()else
end
end
function checkweigh.editCheckStatPackage(label)local newStatPackage,isEnterKey
checkweigh.recallCheckStatPackage()newStatPackage=checkweigh.CheckStatPackage
newStatPackage,isEnterKey=awtx.keypad.selectList("OFF,STD DEV,X-BAR",newStatPackage)awtx.display.writeLine(label)if isEnterKey then
checkweigh.CheckStatPackage=newStatPackage
checkweigh.storeCheckStatPackage()else
end
end
function checkweigh.editPrintTotalFlag(label)local newPrintTotalFlag,isEnterKey
checkweigh.recallPrintTotalFlag()newPrintTotalFlag=checkweigh.PrintTotalFlag
newPrintTotalFlag,isEnterKey=awtx.keypad.selectList("OFF,ON",newPrintTotalFlag)awtx.display.writeLine(label)if isEnterKey then
checkweigh.PrintTotalFlag=newPrintTotalFlag
checkweigh.storePrintTotalFlag()else
end
end
function checkweigh.editTotalFmt(label)local newTotalFmt,FMTmin,FMTmax,isEnterKey
checkweigh.recallTotalFmt()newTotalFmt=checkweigh.TotalFmt
FMTmin=1
FMTmax=40
newTotalFmt,isEnterKey=awtx.keypad.enterInteger(newTotalFmt,FMTmin,FMTmax)awtx.display.writeLine(label)if isEnterKey then
checkweigh.TotalFmt=newTotalFmt
checkweigh.storeTotalFmt()else
end
end
function checkweigh.editClearTotalFlag(label)local newClearTotalFlag,isEnterKey
checkweigh.recallClearTotalFlag()newClearTotalFlag=checkweigh.ClearTotalFlag
newClearTotalFlag,isEnterKey=awtx.keypad.selectList("OFF,ON",newClearTotalFlag)awtx.display.writeLine(label)if isEnterKey then
checkweigh.ClearTotalFlag=newClearTotalFlag
checkweigh.storeClearTotalFlag()else
end
end
function checkweigh.editClearPLUTotalFlag(label)local newClearPLUTotalFlag,isEnterKey
checkweigh.recallClearPLUTotalFlag()newClearPLUTotalFlag=checkweigh.ClearPLUTotalFlag
newClearPLUTotalFlag,isEnterKey=awtx.keypad.selectList("OFF,ON",newClearPLUTotalFlag)awtx.display.writeLine(label)if isEnterKey then
checkweigh.ClearPLUTotalFlag=newClearPLUTotalFlag
checkweigh.storeClearPLUTotalFlag()else
end
end
function checkweigh.editDigitsFlag(label)local newDigitsFlag,isEnterKey
checkweigh.recallDigitsFlag()newDigitsFlag=checkweigh.DigitsFlag
newDigitsFlag,isEnterKey=awtx.keypad.selectList("OFF,ON",newDigitsFlag)awtx.display.writeLine(label)if isEnterKey then
checkweigh.DigitsFlag=newDigitsFlag
checkweigh.storeDigitsFlag()else
end
end
function checkweigh.editGraphFlag(label)local newGraphFlag,isEnterKey
checkweigh.recallGraphFlag()newGraphFlag=checkweigh.GraphFlag
newGraphFlag,isEnterKey=awtx.keypad.selectList("OFF,ON",newGraphFlag)awtx.display.writeLine(label)if isEnterKey then
checkweigh.GraphFlag=newGraphFlag
checkweigh.storeGraphFlag()else
end
end
function checkweigh.editPackRunCnt(label)local newPackRunCnt,MinPackRunCnt,MaxPackRunCnt,isEnterKey
checkweigh.recallPackRunCnt()newPackRunCnt=checkweigh.PackRunCnt
MinPackRunCnt=0
MaxPackRunCnt=9999999
newPackRunCnt,isEnterKey=awtx.keypad.enterInteger(newPackRunCnt,MinPackRunCnt,MaxPackRunCnt)awtx.display.writeLine(label)if isEnterKey then
checkweigh.PackRunCnt=newPackRunCnt
checkweigh.storePackRunCnt()else
end
end
function checkweigh.usePLUValues()local PLUWtUnit,errorState,tmpGacc,tmpNacc
wt=awtx.weight.getCurrent(1)errorState,PLUWtUnit=awtx.weight.unitStrToUnitIndex(plu.PLUunits)checkweigh.UnitsOfMeasureStr=wt.unitsStr
if plu.PLUTargLo==0 or plu.PLUTargHi==0 then
if plu.PLUTarg<0 then
plu.PLUTargLo=plu.PLUTarg+plu.PLUTolLo
plu.PLUTargHi=plu.PLUTarg-plu.PLUTolHi
else
plu.PLUTargLo=plu.PLUTarg-plu.PLUTolLo
plu.PLUTargHi=plu.PLUTarg+plu.PLUTolHi
end
end
if errorState==0 then
if checkweigh.CheckWeighMode==PER375 then
checkweigh.CheckTargLo=plu.PLUTargLo
checkweigh.CheckTargHi=plu.PLUTargHi
checkweigh.CheckTolLo=plu.PLUTolLo
checkweigh.CheckTolHi=plu.PLUTolHi
checkweigh.CheckTarg=plu.PLUTarg
checkweigh.CheckMin=checkweigh.CheckMinDefault
checkweigh.CheckMax=checkweigh.CheckMaxDefault
else
checkweigh.CheckTargLo=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTargLo,wt.units,1)checkweigh.CheckTargHi=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTargHi,wt.units,1)checkweigh.CheckTolLo=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTolLo,wt.units,1)checkweigh.CheckTolHi=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTolHi,wt.units,1)checkweigh.CheckTarg=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTarg,wt.units,1)checkweigh.CheckMin=checkweigh.CheckMinDefault
checkweigh.CheckMax=checkweigh.CheckMaxDefault
end
if checkweigh.PackRunCnt==0 then
tmpGacc=awtx.weight.convertWeight(PLUWtUnit,plu.PLUGrossAccum,wt.units,1)tmpNacc=awtx.weight.convertWeight(PLUWtUnit,plu.PLUNetAccum,wt.units,1)awtx.weight.setAccum(tmpGacc,tmpNacc,0,plu.PLUTransCount)end
else
checkweigh.CheckTargLo=plu.PLUTargLo
checkweigh.CheckTargHi=plu.PLUTargHi
checkweigh.CheckTolLo=plu.PLUTolLo
checkweigh.CheckTolHi=plu.PLUTolHi
checkweigh.CheckTarg=plu.PLUTarg
checkweigh.CheckMin=checkweigh.CheckMinDefault
checkweigh.CheckMax=checkweigh.CheckMaxDefault
if checkweigh.PackRunCnt==0 then
awtx.weight.setAccum(plu.PLUGrossAccum,plu.PLUNetAccum,0,plu.PLUTransCount)end
end
checkweigh.clrXBarRPackage()end
function checkweigh.calcCheckweighByType()if checkweigh.CheckWeighType==0 then
checkweigh.CheckTarg=(checkweigh.CheckTargHi+checkweigh.CheckTargLo)/2
else
if checkweigh.CheckTarg<0 then
checkweigh.CheckTargLo=checkweigh.CheckTarg+(checkweigh.CheckTolLo)checkweigh.CheckTargHi=checkweigh.CheckTarg-(checkweigh.CheckTolHi)else
checkweigh.CheckTargLo=checkweigh.CheckTarg-(checkweigh.CheckTolLo)checkweigh.CheckTargHi=checkweigh.CheckTarg+(checkweigh.CheckTolHi)end
end
checkweigh.storeCheckTargAll()checkweigh.refreshCheckGraph()end
function checkweigh.refreshCheckGraph()local tmpGraphEnable=0
newscaleNumber=1
awtx.weight.graphEnable(newscaleNumber,0)if battery.BatteryEnable==1 then
checkweigh.refreshSetpointsDisabledBattery()else
checkweigh.refreshSetpointsDisabled()end
if checkweigh.CheckTarg<0 then
checkweigh.CheckMin=checkweigh.CheckMinDefault*-1
checkweigh.CheckMax=checkweigh.CheckMaxDefault*-1
else
checkweigh.CheckMin=checkweigh.CheckMinDefault
checkweigh.CheckMax=checkweigh.CheckMaxDefault
end
if checkweigh.CheckWeighMode==CHECK_NONE then
checkweigh.CheckgraphEnableFlag=0
checkweigh.CheckGraphType=0
tmpGraphEnable=checkweigh.CheckGraphType
if wt.tare==0 then
awtx.weight.setActiveValue(VAL_GROSS)else
awtx.weight.setActiveValue(VAL_NET)end
elseif checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckGraphType=3
checkweigh.CheckBasis=VAL_NET
checkweigh.CheckUnderSegDiv=checkweigh.CheckUnderSegDivDefault
checkweigh.CheckOverSegDiv=checkweigh.CheckOverSegDivDefault
awtx.weight.setCheck(newscaleNumber,checkweigh.CheckBasis,checkweigh.CheckMin,checkweigh.CheckTargLo,checkweigh.CheckTargHi,checkweigh.CheckMax,checkweigh.CheckUnderSegDiv,checkweigh.CheckOverSegDiv)checkweigh.CheckgraphEnableFlag=1
tmpGraphEnable=checkweigh.CheckGraphType
if wt.tare==0 then
awtx.weight.setActiveValue(VAL_GROSS)else
awtx.weight.setActiveValue(VAL_NET)end
elseif checkweigh.CheckWeighMode==SIM375 then
if checkweigh.CheckOutputGZB==1 then
checkweigh.CheckGraphType=1
else
checkweigh.CheckGraphType=3
end
checkweigh.CheckBasis=VAL_NET
awtx.weight.setCheck(newscaleNumber,checkweigh.CheckBasis,checkweigh.CheckMin,checkweigh.CheckTargLo,checkweigh.CheckTargHi,checkweigh.CheckMax,checkweigh.CheckUnderSegDiv,checkweigh.CheckOverSegDiv)if checkweigh.CheckgraphEnableFlag==0 then
awtx.weight.setActiveValue(VAL_GROSS)tmpGraphEnable=0
else
awtx.weight.setActiveValue(VAL_NET_MINUS_ANNUN)tmpGraphEnable=checkweigh.CheckGraphType
end
elseif checkweigh.CheckWeighMode==MID375 or checkweigh.CheckWeighMode==ADV375 then
if checkweigh.CheckOutputGZB==1 then
checkweigh.CheckGraphType=1
else
checkweigh.CheckGraphType=3
end
checkweigh.CheckBasis=VAL_NET
awtx.weight.setCheck(newscaleNumber,checkweigh.CheckBasis,checkweigh.CheckMin,checkweigh.CheckTargLo,checkweigh.CheckTargHi,checkweigh.CheckMax,checkweigh.CheckUnderSegDiv,checkweigh.CheckOverSegDiv)checkweigh.CheckgraphEnableFlag=1
if checkweigh.GraphFlag==0 then
tmpGraphEnable=0
else
tmpGraphEnable=checkweigh.CheckGraphType
end
if checkweigh.DigitsFlag==0 then
awtx.weight.setActiveValue(VAL_BLANK)else
if wt.tare==0 then
awtx.weight.setActiveValue(VAL_GROSS)else
awtx.weight.setActiveValue(VAL_NET)end
end
elseif checkweigh.CheckWeighMode==PER375 then
if checkweigh.CheckOutputGZB==1 then
checkweigh.CheckGraphType=1
else
checkweigh.CheckGraphType=3
end
checkweigh.CheckBasis=VAL_PERCENT
awtx.weight.setCheck(newscaleNumber,checkweigh.CheckBasis,checkweigh.CheckMin,checkweigh.CheckTargLo,checkweigh.CheckTargHi,checkweigh.CheckMax,checkweigh.CheckUnderSegDiv,checkweigh.CheckOverSegDiv)checkweigh.CheckgraphEnableFlag=1
if checkweigh.GraphFlag==0 then
tmpGraphEnable=0
else
tmpGraphEnable=checkweigh.CheckGraphType
end
if checkweigh.DigitsFlag==0 then
awtx.weight.setActiveValue(VAL_BLANK)else
awtx.weight.setActiveValue(VAL_PERCENT)end
elseif checkweigh.CheckWeighMode==GRAD375 then
checkweigh.CheckGraphType=0
checkweigh.CheckBasis=VAL_NET
awtx.weight.setCheck(newscaleNumber,checkweigh.CheckBasis,checkweigh.CheckMin,checkweigh.CheckTargLo,checkweigh.CheckTargHi,checkweigh.CheckMax,checkweigh.CheckUnderSegDiv,checkweigh.CheckOverSegDiv)checkweigh.CheckgraphEnableFlag=0
if checkweigh.GraphFlag==0 then
tmpGraphEnable=0
else
tmpGraphEnable=checkweigh.CheckGraphType
end
if checkweigh.DigitsFlag==0 then
awtx.weight.setActiveValue(VAL_BLANK)else
if wt.tare==0 then
awtx.weight.setActiveValue(VAL_GROSS)else
awtx.weight.setActiveValue(VAL_NET)end
end
else
if checkweigh.CheckOutputGZB==1 then
checkweigh.CheckGraphType=1
else
checkweigh.CheckGraphType=3
end
checkweigh.CheckBasis=VAL_NET
awtx.weight.setCheck(newscaleNumber,checkweigh.CheckBasis,checkweigh.CheckMin,checkweigh.CheckTargLo,checkweigh.CheckTargHi,checkweigh.CheckMax,checkweigh.CheckUnderSegDiv,checkweigh.CheckOverSegDiv)checkweigh.CheckgraphEnableFlag=1
tmpGraphEnable=checkweigh.CheckGraphType
if wt.tare==0 then
awtx.weight.setActiveValue(VAL_GROSS)else
awtx.weight.setActiveValue(VAL_NET)end
end
checkweigh.setCheckPrintTokens()if battery.BatteryEnable==1 then
if checkweigh.CheckTarg<0 then
checkweigh.refreshSetpointsRejectAcceptBatteryNeg()else
checkweigh.refreshSetpointsRejectAcceptBattery()end
else
if checkweigh.CheckTarg<0 then
checkweigh.refreshSetpointsUnderTargetOverNeg()else
checkweigh.refreshSetpointsUnderTargetOver()end
end
awtx.weight.graphEnable(newscaleNumber,tmpGraphEnable)end
function checkweigh.showTarget()local usermode,currentRPN
local newTarg,Targmin,Targmax,tempUnitStr,isEnterKey
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)if checkweigh.CheckWeighMode==ADV375 then
checkweigh.usePLUValues()else
checkweigh.recallCheckTarg()end
newTarg=checkweigh.CheckTarg
if checkweigh.CheckWeighMode==PER375 then
Targmin=0.0
Targmax=100.0
tempUnitStr="pc"else
wt=awtx.weight.getCurrent(1)Targmin=wt.curCapacity*-1
Targmax=wt.curCapacity
tempUnitStr=wt.unitsStr
end
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("TARGET")awtx.os.sleep(showtime1)newTarg,isEnterKey=awtx.keypad.enterCheckWtWithUnits(newTarg,Targmin,Targmax,tempUnitStr,MODE_TARGET,separatorChar,showtime2,0)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function checkweigh.sampleTarget()local newTarg
if checkweigh.CheckWeighMode==SIM375 then
newTarg=0.00
elseif checkweigh.CheckWeighMode==SIMPLECHECK then
wt=awtx.weight.getCurrent(1)newTarg=wt.net
checkweigh.CheckWeighType=CHECKWEIGH_TYPE_SAMPLE
else
wt=awtx.weight.getCurrent(1)newTarg=wt.net
end
checkweigh.setTarget(newTarg)end
function checkweigh.enterTarget()local usermode,newTarg,Targmin,Targmax,tempUnitStr,isEnterKey
checkweigh.recallCheckTarg()newTarg=checkweigh.CheckTarg
if checkweigh.CheckWeighMode==PER375 then
Targmin=0.0
Targmax=100.0
tempUnitStr="pc"else
wt=awtx.weight.getCurrent(1)Targmin=wt.curCapacity*-1
Targmax=wt.curCapacity
tempUnitStr=wt.unitsStr
end
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)newTarg,isEnterKey=awtx.keypad.enterWeightWithUnits(newTarg,Targmin,Targmax,tempUnitStr,separatorChar,entertime)awtx.display.setMode(usermode)if isEnterKey then
checkweigh.setTarget(newTarg)else
end
return isEnterKey
end
function checkweigh.setTarget(newTarg)checkweigh.UnitsOfMeasureStr=wt.unitsStr
checkweigh.UnitsOfMeasure=wt.units
local myVarLo,myVarHi
myVarLo=-wt.curDivision*config.grossZeroBand
myVarHi=wt.curDivision*config.grossZeroBand
if checkweigh.CheckWeighMode==SIM375 then
elseif checkweigh.CheckWeighMode==PER375 then
elseif newTarg<=myVarHi and newTarg>=myVarLo then
displayCANT()newTarg=0
end
if newTarg<0 then
checkweigh.CheckTarg=newTarg
checkweigh.CheckTargLo=checkweigh.CheckTarg+(checkweigh.CheckTolLo)checkweigh.CheckTargHi=checkweigh.CheckTarg-(checkweigh.CheckTolHi)else
checkweigh.CheckTarg=newTarg
checkweigh.CheckTargLo=checkweigh.CheckTarg-(checkweigh.CheckTolLo)checkweigh.CheckTargHi=checkweigh.CheckTarg+(checkweigh.CheckTolHi)end
checkweigh.refreshCheckGraph()checkweigh.storeCheckTargAll()checkweigh.clrXBarRPackage()end
function checkweigh.enterTargetWithIncrement()local usermode,newTarg,Targmin,Targmax,tempUnitStr,isEnterKey
checkweigh.recallCheckTarg()newTarg=checkweigh.CheckTarg
if checkweigh.CheckWeighMode==PER375 then
Targmin=0.0
Targmax=100.0
tempUnitStr="pc"else
wt=awtx.weight.getCurrent(1)Targmin=wt.curCapacity*-1
Targmax=wt.curCapacity
tempUnitStr=wt.unitsStr
end
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)newTarg,isEnterKey=awtx.keypad.enterCheckWtWithUnits(newTarg,Targmin,Targmax,tempUnitStr,MODE_TARGET,separatorChar,entertime)awtx.display.setMode(usermode)if isEnterKey then
checkweigh.setTarget(newTarg)else
end
end
function checkweigh.showTargLo()local usermode,currentRPN,newTargLo,TargLomin,TargLomax,tempUnitStr,isEnterKey
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)if checkweigh.CheckWeighMode==ADV375 then
checkweigh.usePLUValues()else
checkweigh.recallCheckTargLo()end
newTargLo=checkweigh.CheckTargLo
if checkweigh.CheckWeighMode==PER375 then
TargLomin=0.0
TargLomax=100.0
tempUnitStr="pc"else
wt=awtx.weight.getCurrent(1)TargLomin=wt.curCapacity*-1
TargLomax=wt.curCapacity
tempUnitStr=wt.unitsStr
end
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("TargLo")awtx.os.sleep(showtime1)newTargLo,isEnterKey=awtx.keypad.enterWeightWithUnits(newTargLo,TargLomin,TargLomax,tempUnitStr,separatorChar,showtime2,0)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function checkweigh.enterTargLo()local usermode,currentRPN,newTargLo,TargLomin,TargLomax,tempUnitStr,isEnterKey
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)checkweigh.recallCheckTargLo()newTargLo=checkweigh.CheckTargLo
if checkweigh.CheckWeighMode==PER375 then
TargLomin=0.0
TargLomax=100.0
tempUnitStr="pc"else
wt=awtx.weight.getCurrent(1)TargLomin=wt.curCapacity*-1
TargLomax=wt.curCapacity
tempUnitStr=wt.unitsStr
end
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.writeLine("Lo")awtx.os.sleep(showtime1)newTargLo,isEnterKey=awtx.keypad.enterWeightWithUnits(newTargLo,TargLomin,TargLomax,tempUnitStr,separatorChar,entertime)awtx.display.setMode(usermode)if isEnterKey then
if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckWeighType=CHECKWEIGH_TYPE_LIMITS
end
checkweigh.setTargLo(newTargLo)else
end
awtx.keypad.set_RPN_mode(currentRPN)return isEnterKey
end
function checkweigh.setTargLo(newTargLo)checkweigh.UnitsOfMeasureStr=wt.unitsStr
checkweigh.UnitsOfMeasure=wt.units
checkweigh.CheckTargLo=newTargLo
checkweigh.CheckTarg=(checkweigh.CheckTargHi+checkweigh.CheckTargLo)/2
checkweigh.refreshCheckGraph()checkweigh.storeCheckTargAll()checkweigh.clrXBarRPackage()end
function checkweigh.showTolLo()local usermode,currentRPN,newTolLo,Tolmin,Tolmax,tempUnitStr,isEnterKey
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)if checkweigh.CheckWeighMode==ADV375 then
checkweigh.usePLUValues()else
checkweigh.recallCheckTolLo()end
newTolLo=checkweigh.CheckTolLo
if checkweigh.CheckWeighMode==PER375 then
Tolmin=0.0
Tolmax=100.0
tempUnitStr="pc"else
wt=awtx.weight.getCurrent(1)Tolmin=wt.curDivision
Tolmax=wt.curCapacity
if newTolLo<Tolmin then
newTolLo=Tolmin
elseif newTolLo>Tolmax then
newTolLo=Tolmin
end
tempUnitStr=wt.unitsStr
end
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("Tol-Lo")awtx.os.sleep(showtime1)newTolLo,isEnterKey=awtx.keypad.enterCheckWtWithUnits(newTolLo,Tolmin,Tolmax,tempUnitStr,MODE_UNDER,separatorChar,showtime2,0)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function checkweigh.enterTolLo()local usermode,newTolLo,Tolmin,Tolmax,tempUnitStr,isEnterKey
checkweigh.recallCheckTolLo()newTolLo=checkweigh.CheckTolLo
if checkweigh.CheckWeighMode==PER375 then
Tolmin=0.0
Tolmax=100.0
tempUnitStr="pc"else
wt=awtx.weight.getCurrent(1)Tolmin=wt.curDivision
Tolmax=wt.curCapacity
if newTolLo<Tolmin then
newTolLo=Tolmin
elseif newTolLo>Tolmax then
newTolLo=Tolmin
end
tempUnitStr=wt.unitsStr
end
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)newTolLo,isEnterKey=awtx.keypad.enterCheckWtWithUnits(newTolLo,Tolmin,Tolmax,tempUnitStr,MODE_UNDER,separatorChar,entertime)awtx.display.setMode(usermode)if isEnterKey then
checkweigh.setTolLo(newTolLo)else
end
end
function checkweigh.setTolLo(newTolLo)checkweigh.UnitsOfMeasureStr=wt.unitsStr
checkweigh.UnitsOfMeasure=wt.units
checkweigh.CheckTolLo=newTolLo
if checkweigh.CheckTarg<0 then
checkweigh.CheckTargLo=checkweigh.CheckTarg+(checkweigh.CheckTolLo)checkweigh.CheckTargHi=checkweigh.CheckTarg-(checkweigh.CheckTolHi)else
checkweigh.CheckTargLo=checkweigh.CheckTarg-(checkweigh.CheckTolLo)checkweigh.CheckTargHi=checkweigh.CheckTarg+(checkweigh.CheckTolHi)end
checkweigh.refreshCheckGraph()checkweigh.storeCheckTargAll()checkweigh.clrXBarRPackage()end
function checkweigh.showTargHi()local usermode,currentRPN,newTargHi,TargHimin,TargHimax,tempUnitStr,isEnterKey
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)if checkweigh.CheckWeighMode==ADV375 then
checkweigh.usePLUValues()else
checkweigh.recallCheckTargHi()end
newTargHi=checkweigh.CheckTargHi
if checkweigh.CheckWeighMode==PER375 then
TargHimin=0.0
TargHimax=100.0
tempUnitStr="pc"else
wt=awtx.weight.getCurrent(1)TargHimin=wt.curCapacity*-1
TargHimax=wt.curCapacity
wt=awtx.weight.getCurrent(1)tempUnitStr=wt.unitsStr
end
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("TargHi")awtx.os.sleep(showtime1)newTargHi,isEnterKey=awtx.keypad.enterWeightWithUnits(newTargHi,TargHimin,TargHimax,tempUnitStr,separatorChar,showtime2,0)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function checkweigh.enterTargHi()local usermode,currentRPN,newTargHi,TargHimin,TargHimax,tempUnitStr,isEnterKey
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)checkweigh.recallCheckTargHi()newTargHi=checkweigh.CheckTargHi
if checkweigh.CheckWeighMode==PER375 then
TargHimin=0.0
TargHimax=100.0
tempUnitStr="pc"else
wt=awtx.weight.getCurrent(1)TargHimin=wt.curCapacity*-1
TargHimax=wt.curCapacity
wt=awtx.weight.getCurrent(1)tempUnitStr=wt.unitsStr
end
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.writeLine("Hi")awtx.os.sleep(showtime1)newTargHi,isEnterKey=awtx.keypad.enterWeightWithUnits(newTargHi,TargHimin,TargHimax,tempUnitStr,separatorChar,entertime)awtx.display.setMode(usermode)if isEnterKey then
if checkweigh.CheckWeighMode==SIMPLECHECK then
checkweigh.CheckWeighType=CHECKWEIGH_TYPE_LIMITS
end
checkweigh.setTargHi(newTargHi)else
end
awtx.keypad.set_RPN_mode(currentRPN)return isEnterKey
end
function checkweigh.setTargHi(newTargHi)checkweigh.UnitsOfMeasureStr=wt.unitsStr
checkweigh.UnitsOfMeasure=wt.units
checkweigh.CheckTargHi=newTargHi
checkweigh.CheckTarg=(checkweigh.CheckTargHi+checkweigh.CheckTargLo)/2
checkweigh.refreshCheckGraph()checkweigh.storeCheckTargAll()checkweigh.clrXBarRPackage()end
function checkweigh.showTolHi()local usermode,currentRPN,newTolHi,Tolmin,Tolmax,tempUnitStr,isEnterKey
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)if checkweigh.CheckWeighMode==ADV375 then
checkweigh.usePLUValues()else
checkweigh.recallCheckTolHi()end
newTolHi=checkweigh.CheckTolHi
if checkweigh.CheckWeighMode==PER375 then
Tolmin=0.0
Tolmax=100.0
tempUnitStr="pc"else
wt=awtx.weight.getCurrent(1)Tolmin=wt.curDivision
Tolmax=wt.curCapacity
if newTolHi<Tolmin then
newTolHi=Tolmin
elseif newTolHi>Tolmax then
newTolHi=Tolmin
end
tempUnitStr=wt.unitsStr
end
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("Tol-Hi")awtx.os.sleep(showtime1)newTolHi,isEnterKey=awtx.keypad.enterCheckWtWithUnits(newTolHi,Tolmin,Tolmax,tempUnitStr,MODE_OVER,separatorChar,showtime2,0)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function checkweigh.enterTolHi()local usermode,newTolHi,Tolmin,Tolmax,tempUnitStr,isEnterKey
checkweigh.recallCheckTolHi()newTolHi=checkweigh.CheckTolHi
if checkweigh.CheckWeighMode==PER375 then
Tolmin=0.0
Tolmax=100.0
tempUnitStr="pc"else
wt=awtx.weight.getCurrent(1)Tolmin=wt.curDivision
Tolmax=wt.curCapacity
if newTolHi<Tolmin then
newTolHi=Tolmin
elseif newTolHi>Tolmax then
newTolHi=Tolmin
end
tempUnitStr=wt.unitsStr
end
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)newTolHi,isEnterKey=awtx.keypad.enterCheckWtWithUnits(newTolHi,Tolmin,Tolmax,tempUnitStr,MODE_OVER,separatorChar,entertime)awtx.display.setMode(usermode)if isEnterKey then
checkweigh.setTolHi(newTolHi)else
end
end
function checkweigh.setTolHi(newTolHi)checkweigh.UnitsOfMeasureStr=wt.unitsStr
checkweigh.UnitsOfMeasure=wt.units
checkweigh.CheckTolHi=newTolHi
if checkweigh.CheckTarg<0 then
checkweigh.CheckTargLo=checkweigh.CheckTarg+(checkweigh.CheckTolLo)checkweigh.CheckTargHi=checkweigh.CheckTarg-(checkweigh.CheckTolHi)else
checkweigh.CheckTargLo=checkweigh.CheckTarg-(checkweigh.CheckTolLo)checkweigh.CheckTargHi=checkweigh.CheckTarg+(checkweigh.CheckTolHi)end
checkweigh.refreshCheckGraph()checkweigh.storeCheckTargAll()checkweigh.clrXBarRPackage()end
function checkweigh.enterUnderDiv(label)local newUnderDiv,UnderDivmin,UnderDivmax,isEnterKey
checkweigh.recallCheckUnderDiv()newUnderDiv=checkweigh.CheckUnderDiv
UnderDivmin=1
UnderDivmax=100
newUnderDiv,isEnterKey=awtx.keypad.enterInteger(newUnderDiv,UnderDivmin,UnderDivmax)awtx.display.writeLine(label)if isEnterKey then
checkweigh.setUnderDiv(newUnderDiv)else
end
end
function checkweigh.setUnderDiv(newUnderDiv)checkweigh.UnitsOfMeasureStr=wt.unitsStr
checkweigh.UnitsOfMeasure=wt.units
checkweigh.CheckUnderDiv=newUnderDiv
checkweigh.CheckTolLo=checkweigh.CheckUnderDiv*wt.curDivision
checkweigh.CheckTargLo=checkweigh.CheckTarg-(checkweigh.CheckTolLo)checkweigh.refreshCheckGraph()checkweigh.storeCheckTargAll()end
function checkweigh.enterOverDiv(label)local newOverDiv,OverDivmin,OverDivmax,isEnterKey
checkweigh.recallCheckOverDiv()newOverDiv=checkweigh.CheckOverDiv
OverDivmin=1
OverDivmax=100
newOverDiv,isEnterKey=awtx.keypad.enterInteger(newOverDiv,OverDivmin,OverDivmax)awtx.display.writeLine(label)if isEnterKey then
checkweigh.setOverDiv(newOverDiv)else
end
end
function checkweigh.setOverDiv(newOverDiv)checkweigh.UnitsOfMeasureStr=wt.unitsStr
checkweigh.UnitsOfMeasure=wt.units
checkweigh.CheckOverDiv=newOverDiv
checkweigh.CheckTolHi=checkweigh.CheckOverDiv*wt.curDivision
checkweigh.CheckTargHi=checkweigh.CheckTarg+(checkweigh.CheckTolHi)checkweigh.refreshCheckGraph()checkweigh.storeCheckTargAll()end
local Output_Type_String={}local Output_Prompt_String
if system.modelStr=="ZM301"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside     ","Outside    ","Checkweigh ","User     "}Output_Prompt_String="ACT-Abv,ACT-Bel"elseif system.modelStr=="ZM303"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside     ","Outside    ","Checkweigh ","User     "}Output_Prompt_String="ACT-Abv,ACT-Bel"elseif system.modelStr=="ZQ375"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside     ","Outside    ","Checkweigh ","User     "}Output_Prompt_String="ACT-Abv,ACT-Bel"elseif system.modelStr=="ZM305GTN"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside     ","Outside    ","Checkweigh ","User     "}Output_Prompt_String="ACT-Abv,ACT-Bel"elseif system.modelStr=="ZM305"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside     ","Outside    ","Checkweigh ","User     "}Output_Prompt_String="ACT-Abv,ACT-Bel"else
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside     ","Outside    ","Checkweigh ","User     "}Output_Prompt_String="ACT-Abv,ACT-Bel"end
local Input_Type_String={}local Input_Prompt_String
if system.modelStr=="ZM301"then
MAX_SETPOINT_INPUT_INDEX=3
Input_Type_String={[0]="None     ","PrintKey ","UnitsKey ","SelectKey","TareKey  ","ZeroKey  ","SampleKey","F1Key    ","AccumKey ","PrintHold","User     "}Input_Prompt_String="None,Print,Units,Select,Tare,Zero,Sample,F1,Accu,PrntHld,User"elseif system.modelStr=="ZM303"then
MAX_SETPOINT_INPUT_INDEX=3
Input_Type_String={[0]="None     ","PrintKey ","UnitsKey ","SelectKey","TareKey  ","ZeroKey  ","SampleKey","F1Key    ","TargetKey","StartKey ","StopKey  ","IDKey    ","SetupKey ","AccumKey ","PrintHold","User     "}Input_Prompt_String="None,Print,Units,Select,Tare,Zero,Sample,F1,Target,Start,Stop,ID,Setup,Under,Over,Accu,Base,PrntHld,User"elseif system.modelStr=="ZQ375"then
MAX_SETPOINT_INPUT_INDEX=3
Input_Type_String={[0]="None     ","PrintKey ","UnitsKey ","SelectKey","TareKey  ","ZeroKey  ","F1Key    ","TargetKey","IDKey    ","UnderKey ","OverKey  ","AccumKey ","PrintHold","User     "}Input_Prompt_String="None,Print,Units,Select,Tare,Zero,F1,Target,ID,Under,Over,Accu,PrntHld,User"elseif system.modelStr=="ZM305GTN"then
MAX_SETPOINT_INPUT_INDEX=3
Input_Type_String={[0]="None     ","PrintKey ","UnitsKey ","SelectKey","TareKey  ","ZeroKey  ","SampleKey","F1Key    ","TargetKey","StartKey ","StopKey  ","IDKey    ","SetupKey ","AccumKey ","PrintHold","User     "}Input_Prompt_String="None,Print,Units,Select,Tare,Zero,Fleet,In-Out,Report,Start,Stop,ID,Setup,Under,Over,Accu,Base,PrntHld,User"elseif system.modelStr=="ZM305"then
MAX_SETPOINT_INPUT_INDEX=3
Input_Type_String={[0]="None     ","PrintKey ","UnitsKey ","SelectKey","TareKey  ","ZeroKey  ","SampleKey","F1Key    ","TargetKey","StartKey ","StopKey  ","IDKey    ","SetupKey ","AccumKey ","PrintHold","User     "}Input_Prompt_String="None,Print,Units,Select,Tare,Zero,Sample,F1,Target,Start,Stop,ID,Setup,Under,Over,Accu,Base,PrntHld,User"else
MAX_SETPOINT_INPUT_INDEX=3
Input_Type_String={[0]="None     ","PrintKey ","UnitsKey ","SelectKey","TareKey  ","ZeroKey  ","SampleKey","F1Key    ","TargetKey","StartKey ","StopKey  ","IDKey    ","SetupKey ","UnderKey ","OverKey  ","AccumKey ","BaseKey  ","PrintHold","User     "}Input_Prompt_String="None,Print,Units,Select,Tare,Zero,Sample,F1,Target,Start,Stop,ID,Setup,Under,Over,Accu,Base,PrntHld,User"end
function checkweigh.doUnderEventStuff()if checkweigh.CheckOutputType==0 then
result=awtx.setPoint.outputClr(7)else
result=awtx.setPoint.outputSet(7)end
result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(3)result=awtx.setPoint.outputClr(5)result=awtx.setPoint.outputClr(6)checkweigh.XR4500=XR4500_RED
checkweigh.setXR4500PrintToken()end
function checkweigh.doTargetEventStuff()local usermode,currentRPN
if checkweigh.CheckOutputType==0 then
result=awtx.setPoint.outputClr(7)else
result=awtx.setPoint.outputSet(7)end
result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(3)result=awtx.setPoint.outputClr(4)result=awtx.setPoint.outputClr(6)if checkweigh.CheckAutoTareAtTarget==0 then
elseif checkweigh.CheckAutoTareAtTarget==1 and checkweigh.CheckTarg~=0 then
awtx.weight.requestTare()else
end
checkweigh.XR4500=XR4500_GRN
checkweigh.setXR4500PrintToken()end
function checkweigh.doOverEventStuff()if checkweigh.CheckOutputType==0 then
result=awtx.setPoint.outputClr(7)else
result=awtx.setPoint.outputSet(7)end
result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(4)result=awtx.setPoint.outputClr(5)checkweigh.XR4500=XR4500_RED
checkweigh.setXR4500PrintToken()end
function checkweigh.doRejectEventStuff()if checkweigh.CheckOutputType==0 then
result=awtx.setPoint.outputClr(7)else
result=awtx.setPoint.outputSet(7)end
result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(5)checkweigh.XR4500=XR4500_RED
checkweigh.setXR4500PrintToken()end
function checkweigh.doAcceptEventStuff()local usermode,currentRPN
if checkweigh.CheckOutputType==0 then
result=awtx.setPoint.outputClr(7)else
result=awtx.setPoint.outputSet(7)end
result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(4)if checkweigh.CheckAutoTareAtTarget==0 then
elseif checkweigh.CheckAutoTareAtTarget==1 and checkweigh.CheckTarg~=0 then
awtx.weight.requestTare()else
end
checkweigh.XR4500=XR4500_GRN
checkweigh.setXR4500PrintToken()end
function checkweigh.doNearZero()if checkweigh.CheckOutputType==0 then
result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(3)result=awtx.setPoint.outputClr(4)result=awtx.setPoint.outputClr(5)result=awtx.setPoint.outputClr(6)result=awtx.setPoint.outputSet(7)else
result=awtx.setPoint.outputSet(7)end
checkweigh.XR4500=XR4500_OFF
checkweigh.setXR4500PrintToken()end
function checkweigh.CheckweighDBReport(label)local usermode,currentRPN,fho,err
local index,isEnterKey,dbFile,row
local t={}currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)index,isEnterKey=awtx.keypad.selectList(" Port 1, Port 2, USB",0)awtx.display.writeLine(label)if isEnterKey then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(DB_FileLocation_AppConfig)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighConfig (varID TEXT PRIMARY KEY, value TEXT)")if index==0 or index==1 then
t[#t+1]=string.format("varID                       value \r\n")elseif index==2 then
t[#t+1]=string.format("varID, value \r\n")end
for row in dbFile:rows("SELECT varID, value, FROM tblCheckweighConfig")do
if index==0 or index==1 then
t[#t+1]=string.format("%30s %10s \r\n",row[1],row[2])elseif index==2 then
t[#t+1]=string.format("%s, %s \r\n",row[1],row[2])else
end
end
dbFile:close()if separatorChar==0 then
elseif separatorChar==1 then
for i,j in pairs(t)do
t[i]=string.gsub(t[i],"%,","%;")end
for i,j in pairs(t)do
t[i]=string.gsub(t[i],"%.","%,")end
end
if index==0 then
awtx.serial.send(1,table.concat(t))elseif index==1 then
awtx.serial.send(2,table.concat(t))elseif index==2 then
result=awtx.os.makeDirectory(DB_FileLocation_Reports)if result==0 then
fho,err=io.open(string.format("%s%s",DB_FileLocation_Reports,DB_ReportName_Checkweigh),"w")if err then
awtx.display.writeLine("err "..err)awtx.os.sleep(canttime)awtx.os.sleep(canttime)else
fho:write(table.concat(t))fho:close()end
else
awtx.display.writeLine("err "..tostring(result))end
else
end
awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function checkweigh.TransactionDBReport(label)local usermode,currentRPN,fho,err
local index,isEnterKey,dbFile,row
local t={}currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)index,isEnterKey=awtx.keypad.selectList(" Port 1, Port 2, USB",0)awtx.display.writeLine(label)if isEnterKey then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(DB_FileLocation_AppData)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCheckweighTransactions (TransactionCount INTEGER PRIMARY KEY, TransactionPLUNumber INTEGER, TransactionTimeDate TEXT, TransactionSysCount INTEGER, TransactionGrossWt DOUBLE, TransactionNetWt DOUBLE, TransactionUofM TEXT, TransactionUAO TEXT, TransactionID INTEGER)")if index==0 or index==1 then
t[#t+1]=string.format("TransactionCount, TransactionPLUNumber, TransactionTimeDate, TransactionSysCount, TransactionGrossWt, TransactionNetWt, TransactionUofM, TransactionUAO, TransactionID \r\n")elseif index==2 then
t[#t+1]=string.format("TransactionCount, TransactionPLUNumber, TransactionTimeDate, TransactionSysCount, TransactionGrossWt, TransactionNetWt, TransactionUofM, TransactionUAO, TransactionID \r\n")end
for row in dbFile:rows("SELECT TransactionCount, TransactionPLUNumber, TransactionTimeDate, TransactionSysCount, TransactionGrossWt, TransactionNetWt, TransactionUofM, TransactionUAO, TransactionID FROM tblCheckweighTransactions")do
if index==0 or index==1 then
t[#t+1]=string.format("%d, %d, %s, %d, %f, %f, %s, %s, %d \r\n",row[1],row[2],row[3],row[4],row[5],row[6],row[7],row[8],row[9])elseif index==2 then
t[#t+1]=string.format("%d, %d, %s, %d, %f, %f, %s, %s, %d \r\n",row[1],row[2],row[3],row[4],row[5],row[6],row[7],row[8],row[9])else
end
end
dbFile:close()if separatorChar==0 then
elseif separatorChar==1 then
for i,j in pairs(t)do
t[i]=string.gsub(t[i],"%,","%;")end
for i,j in pairs(t)do
t[i]=string.gsub(t[i],"%.","%,")end
end
if index==0 then
awtx.serial.send(1,table.concat(t))elseif index==1 then
awtx.serial.send(2,table.concat(t))elseif index==2 then
result=awtx.os.makeDirectory(DB_FileLocation_Reports)if result==0 then
fho,err=io.open(string.format("%s%s",DB_FileLocation_Reports,DB_ReportName_Transactions),"w")if err then
awtx.display.writeLine("err "..err)awtx.os.sleep(canttime)awtx.os.sleep(canttime)else
fho:write(table.concat(t))fho:close()end
else
awtx.display.writeLine("err "..tostring(result))end
else
end
awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function checkweigh.CheckweighDBReset(label)local index,isEnterKey
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)checkweigh.newClrAccum()checkweigh.transactionStore()newscaleNumber=1
checkweigh.UnitsOfMeasureStr=wt.unitsStr
checkweigh.UnitsOfMeasure=wt.units
checkweigh.CheckgraphEnableFlag=checkweigh.CheckgraphEnableFlagDefault
checkweigh.CheckBasis=checkweigh.CheckBasisDefault
checkweigh.CheckMin=checkweigh.CheckMinDefault
checkweigh.CheckMax=checkweigh.CheckMaxDefault
checkweigh.CheckTolLo=checkweigh.CheckTolLoDefault
checkweigh.CheckTolHi=checkweigh.CheckTolHiDefault
checkweigh.CheckUnderDiv=checkweigh.CheckUnderDivDefault
checkweigh.CheckOverDiv=checkweigh.CheckOverDivDefault
checkweigh.CheckTargLo=checkweigh.CheckTargLoDefault
checkweigh.CheckTarg=checkweigh.CheckTargDefault
checkweigh.CheckTargHi=checkweigh.CheckTargHiDefault
checkweigh.CheckUnderSegDiv=checkweigh.CheckUnderSegDivDefault
checkweigh.CheckOverSegDiv=checkweigh.CheckOverSegDivDefault
checkweigh.CheckGraphType=checkweigh.CheckGraphTypeDefault
checkweigh.CheckWeighType=checkweigh.CheckWeighTypeDefault
checkweigh.CheckOutputType=checkweigh.CheckOutputTypeDefault
checkweigh.CheckOutputGZB=checkweigh.CheckOutputGZBDefault
checkweigh.CheckStatPackage=checkweigh.CheckStatPackageDefault
checkweigh.CheckAutoTareAtTarget=checkweigh.CheckAutoTareAtTargetDefault
checkweigh.CheckAutoStoreAtTarget=checkweigh.CheckAutoStoreAtTargetDefault
checkweigh.PrintTotalFlag=checkweigh.PrintTotalFlagDefault
checkweigh.TotalFmt=checkweigh.TotalFmtDefault
checkweigh.ClearTotalFlag=checkweigh.ClearTotalFlagDefault
checkweigh.ClearPLUTotalFlag=checkweigh.ClearPLUTotalFlagDefault
checkweigh.DigitsFlag=checkweigh.DigitsFlagDefault
checkweigh.GraphFlag=checkweigh.GraphFlagDefault
checkweigh.PackRunCnt=checkweigh.PackRunCntDefault
checkweigh.storeCheckAll()checkweigh.initAll()checkweigh.clrStatsPackage()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
end
end
function checkweigh.refreshSetpointsValues()local myVarLo,myVarHi,netGZBLo,netGZBHi
myVarLo=-wt.curDivision*config.grossZeroBand
myVarHi=wt.curDivision*config.grossZeroBand
awtx.setPoint.varSet("GZBLo",myVarLo)awtx.setPoint.varSet("GZBHi",myVarHi)netGZBLo=myVarLo
netGZBHi=myVarHi
if checkweigh.CheckTarg<0 then
netGZBLo=wt.curCapacity-myVarLo
netGZBHi=wt.curCapacity-myVarHi
else
netGZBLo=myVarLo-wt.tare
netGZBHi=myVarHi-wt.tare
end
awtx.setPoint.varSet("netGZBLo",netGZBLo)awtx.setPoint.varSet("netGZBHi",netGZBHi)awtx.setPoint.varSet("checkweigh.CheckMin",checkweigh.CheckMin)if checkweigh.CheckTarg<0 then
awtx.setPoint.varSet("underTargLo",checkweigh.CheckTargLo+wt.curDivision)else
awtx.setPoint.varSet("underTargLo",checkweigh.CheckTargLo-wt.curDivision)end
awtx.setPoint.varSet("checkweigh.CheckTargLo",checkweigh.CheckTargLo)awtx.setPoint.varSet("checkweigh.CheckTargHi",checkweigh.CheckTargHi)if checkweigh.CheckTarg<0 then
awtx.setPoint.varSet("overTargHi",checkweigh.CheckTargHi-wt.curDivision)else
awtx.setPoint.varSet("overTargHi",checkweigh.CheckTargHi+wt.curDivision)end
awtx.setPoint.varSet("checkweigh.CheckMax",checkweigh.CheckMax)if checkweigh.CheckOutputType==0 then
result=awtx.setPoint.outputSet(7)else
result=awtx.setPoint.outputSet(7)end
end
function checkweigh.refreshSetpointsDisabled()local setPointDisabled,result,retVal,resultMsg
setPointDisabled={mode="disabled"}retVal,resultMsg=awtx.setPoint.set(1,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(2,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(3,setPointDisabled)result=awtx.setPoint.unregisterOutputEvent(1)result=awtx.setPoint.unregisterOutputEvent(2)result=awtx.setPoint.unregisterOutputEvent(3)result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(3)checkweigh.refreshSetpointsValues()end
function checkweigh.refreshSetpointsDisabledBattery()local setPointDisabled,setPointBattery,result,retVal,resultMsg
setPointDisabled={mode="disabled"}setPointBattery={mode="output",bounceTime=minLatchTime,offInsideGZB=false,act="user",deact="user"}retVal,resultMsg=awtx.setPoint.set(1,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(2,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)result=awtx.setPoint.unregisterOutputEvent(1)result=awtx.setPoint.unregisterOutputEvent(1)result=awtx.setPoint.unregisterOutputEvent(2)result=awtx.setPoint.unregisterOutputEvent(3)result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(3)checkweigh.refreshSetpointsValues()end
function checkweigh.refreshSetpointsUnderTargetOver()local result,newsetptBasis,offInsideGZBFlag,retVal,resultMsg
local setPointUnderNet,setPointUnderWMotionNet,setPointUnderGross,setPointUnderWMotionGross
local setPointTarget,setPointTargetWMotion
local setPointOver,setPointOverWMotion
local setPoint4And7,setPoint5And7,setPoint6And7,setPoint7User,setPointOutEx8,setPointDisabled
if checkweigh.CheckBasis==VAL_GROSS then
newsetptBasis="grossWt"elseif checkweigh.CheckBasis==VAL_NET then
newsetptBasis="netwt"elseif checkweigh.CheckBasis==VAL_NET_MINUS_ANNUN then
newsetptBasis="netwt"elseif checkweigh.CheckBasis==VAL_COUNT then
newsetptBasis="count"elseif checkweigh.CheckBasis==VAL_PERCENT then
newsetptBasis="percent"else
newsetptBasis="grossWt"end
if checkweigh.CheckOutputGZB==1 then
offInsideGZBFlag=true
else
offInsideGZBFlag=false
end
setPointUnderNet={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="checkweigh.CheckMin",actUpperVarName="underTargLo",actBasis=newsetptBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="checkweigh.CheckMin",deactUpperVarName="underTargLo",deactBasis=newsetptBasis,deactMotionInhibit=false}setPointUnderWMotionNet={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="checkweigh.CheckMin",actUpperVarName="underTargLo",actBasis=newsetptBasis,actMotionInhibit=true,deact="outside",deactLowerVarName="checkweigh.CheckMin",deactUpperVarName="underTargLo",deactBasis=newsetptBasis,deactMotionInhibit=true}setPointUnderGross={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="netGZBHi",actUpperVarName="underTargLo",actBasis=newsetptBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="netGZBHi",deactUpperVarName="underTargLo",deactBasis=newsetptBasis,deactMotionInhibit=false}setPointUnderWMotionGross={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="netGZBHi",actUpperVarName="underTargLo",actBasis=newsetptBasis,actMotionInhibit=true,deact="outside",deactLowerVarName="netGZBHi",deactUpperVarName="underTargLo",deactBasis=newsetptBasis,deactMotionInhibit=true}setPointTarget={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="checkweigh.CheckTargLo",actUpperVarName="checkweigh.CheckTargHi",actBasis=newsetptBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="checkweigh.CheckTargLo",deactUpperVarName="checkweigh.CheckTargHi",deactBasis=newsetptBasis,deactMotionInhibit=false}setPointTargetWMotion={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="checkweigh.CheckTargLo",actUpperVarName="checkweigh.CheckTargHi",actBasis=newsetptBasis,actMotionInhibit=true,deact="outside",deactLowerVarName="checkweigh.CheckTargLo",deactUpperVarName="checkweigh.CheckTargHi",deactBasis=newsetptBasis,deactMotionInhibit=true}setPointOver={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="overTargHi",actUpperVarName="checkweigh.CheckMax",actBasis=newsetptBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="overTargHi",deactUpperVarName="checkweigh.CheckMax",deactBasis=newsetptBasis,deactMotionInhibit=false}setPointOverWMotion={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="overTargHi",actUpperVarName="checkweigh.CheckMax",actBasis=newsetptBasis,actMotionInhibit=true,deact="outside",deactLowerVarName="overTargHi",deactUpperVarName="checkweigh.CheckMax",deactBasis=newsetptBasis,deactMotionInhibit=true}setPoint4And7={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=4,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPoint4And7Net={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=4,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=true}setPoint5And7={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=5,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPoint5And7Net={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=5,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=true}setPoint6And7={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=6,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPoint6And7Net={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=6,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=true}setPoint7User={mode="output",bounceTime=0,offInsideGZB=offInsideGZBFlag,act="user",deact="user"}setPointOutEx8={mode="output",bounceTime=0,offInsideGZB=false,act="inside",actLowerVarName="GZBLo",actUpperVarName="GZBHi",actBasis="grossWt",actMotionInhibit=false,deact="outside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPointOutEx8Net={mode="output",bounceTime=0,offInsideGZB=false,act="inside",actLowerVarName="GZBLo",actUpperVarName="GZBHi",actBasis="netwt",actMotionInhibit=true,deact="outside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=false}setPointDisabled={mode="disabled"}if checkweigh.CheckOutputType==0 then
if checkweigh.CheckAutoTareAtTarget==0 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7)retVal,resultMsg=awtx.setPoint.set(3,setPoint6And7)if checkweigh.CheckOutputGZB==1 then
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderWMotionGross)else
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderWMotionGross)end
retVal,resultMsg=awtx.setPoint.set(5,setPointTargetWMotion)retVal,resultMsg=awtx.setPoint.set(6,setPointOverWMotion)elseif checkweigh.CheckAutoTareAtTarget==1 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7Net)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7Net)retVal,resultMsg=awtx.setPoint.set(3,setPoint6And7Net)if checkweigh.CheckOutputGZB==1 then
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderWMotionGross)else
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderWMotionGross)end
retVal,resultMsg=awtx.setPoint.set(5,setPointTargetWMotion)retVal,resultMsg=awtx.setPoint.set(6,setPointOverWMotion)end
else
if checkweigh.CheckAutoTareAtTarget==0 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7)retVal,resultMsg=awtx.setPoint.set(3,setPoint6And7)if checkweigh.CheckOutputGZB==1 then
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderGross)else
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderGross)end
retVal,resultMsg=awtx.setPoint.set(5,setPointTarget)retVal,resultMsg=awtx.setPoint.set(6,setPointOver)elseif checkweigh.CheckAutoTareAtTarget==1 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7Net)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7Net)retVal,resultMsg=awtx.setPoint.set(3,setPoint6And7Net)if checkweigh.CheckOutputGZB==1 then
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderWMotionGross)else
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderWMotionGross)end
retVal,resultMsg=awtx.setPoint.set(5,setPointTargetWMotion)retVal,resultMsg=awtx.setPoint.set(6,setPointOverWMotion)end
end
if checkweigh.CheckOutputType==0 then
retVal,resultMsg=awtx.setPoint.set(7,setPoint7User)else
retVal,resultMsg=awtx.setPoint.set(7,setPoint7User)end
if checkweigh.CheckAutoTareAtTarget==0 then
retVal,resultMsg=awtx.setPoint.set(8,setPointOutEx8)elseif checkweigh.CheckAutoTareAtTarget==1 then
retVal,resultMsg=awtx.setPoint.set(8,setPointOutEx8Net)end
retVal,resultMsg=awtx.setPoint.set(9,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(10,setPointDisabled)result=awtx.setPoint.registerOutputEvent(1,checkweigh.setpointUnderEvent)result=awtx.setPoint.registerOutputEvent(2,checkweigh.setpointTargetEvent)result=awtx.setPoint.registerOutputEvent(3,checkweigh.setpointOverEvent)result=awtx.setPoint.registerOutputEvent(4,checkweigh.setpoint4Event)result=awtx.setPoint.registerOutputEvent(5,checkweigh.setpoint5Event)result=awtx.setPoint.registerOutputEvent(6,checkweigh.setpoint6Event)result=awtx.setPoint.registerOutputEvent(7,checkweigh.setpoint7Event)result=awtx.setPoint.registerOutputEvent(8,checkweigh.setpoint8Event)checkweigh.refreshSetpointsValues()end
function checkweigh.refreshSetpointsUnderTargetOverNeg()local result,newsetptBasis,offInsideGZBFlag,retVal,resultMsg
local setPointUnderNet,setPointUnderWMotionNet,setPointUnderGross,setPointUnderWMotionGross
local setPointTarget,setPointTargetWMotion
local setPointOver,setPointOverWMotion
local setPoint4And7,setPoint5And7,setPoint6And7,setPoint7User,setPointOutEx8,setPointDisabled
if checkweigh.CheckBasis==VAL_GROSS then
newsetptBasis="grossWt"elseif checkweigh.CheckBasis==VAL_NET then
newsetptBasis="netwt"elseif checkweigh.CheckBasis==VAL_NET_MINUS_ANNUN then
newsetptBasis="netwt"elseif checkweigh.CheckBasis==VAL_COUNT then
newsetptBasis="count"elseif checkweigh.CheckBasis==VAL_PERCENT then
newsetptBasis="percent"else
newsetptBasis="grossWt"end
if checkweigh.CheckOutputGZB==1 then
offInsideGZBFlag=true
else
offInsideGZBFlag=false
end
setPointUnderNet={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="underTargLo",actUpperVarName="checkweigh.CheckMin",actBasis=newsetptBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="underTargLo",deactUpperVarName="checkweigh.CheckMin",deactBasis=newsetptBasis,deactMotionInhibit=false}setPointUnderWMotionNet={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="underTargLo",actUpperVarName="checkweigh.CheckMin",actBasis=newsetptBasis,actMotionInhibit=true,deact="outside",deactLowerVarName="underTargLo",deactUpperVarName="checkweigh.CheckMin",deactBasis=newsetptBasis,deactMotionInhibit=true}setPointUnderGross={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="underTargLo",actUpperVarName="netGZBHi",actBasis=newsetptBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="underTargLo",deactUpperVarName="netGZBHi",deactBasis=newsetptBasis,deactMotionInhibit=false}setPointUnderWMotionGross={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="underTargLo",actUpperVarName="netGZBHi",actBasis=newsetptBasis,actMotionInhibit=true,deact="outside",deactLowerVarName="underTargLo",deactUpperVarName="netGZBHi",deactBasis=newsetptBasis,deactMotionInhibit=true}setPointTarget={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="checkweigh.CheckTargHi",actUpperVarName="checkweigh.CheckTargLo",actBasis=newsetptBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="checkweigh.CheckTargHi",deactUpperVarName="checkweigh.CheckTargLo",deactBasis=newsetptBasis,deactMotionInhibit=false}setPointTargetWMotion={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="checkweigh.CheckTargHi",actUpperVarName="checkweigh.CheckTargLo",actBasis=newsetptBasis,actMotionInhibit=true,deact="outside",deactLowerVarName="checkweigh.CheckTargHi",deactUpperVarName="checkweigh.CheckTargLo",deactBasis=newsetptBasis,deactMotionInhibit=true}setPointOver={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="checkweigh.CheckMax",actUpperVarName="overTargHi",actBasis=newsetptBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="checkweigh.CheckMax",deactUpperVarName="overTargHi",deactBasis=newsetptBasis,deactMotionInhibit=false}setPointOverWMotion={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="checkweigh.CheckMax",actUpperVarName="overTargHi",actBasis=newsetptBasis,actMotionInhibit=true,deact="outside",deactLowerVarName="checkweigh.CheckMax",deactUpperVarName="overTargHi",deactBasis=newsetptBasis,deactMotionInhibit=true}setPoint4And7={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=4,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPoint4And7Net={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=4,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=true}setPoint5And7={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=5,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPoint5And7Net={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=5,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=true}setPoint6And7={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=6,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPoint6And7Net={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=6,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=true}setPoint7User={mode="output",bounceTime=0,offInsideGZB=offInsideGZBFlag,act="user",deact="user"}setPointOutEx8={mode="output",bounceTime=0,offInsideGZB=false,act="inside",actLowerVarName="GZBLo",actUpperVarName="GZBHi",actBasis="grossWt",actMotionInhibit=false,deact="outside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPointOutEx8Net={mode="output",bounceTime=0,offInsideGZB=false,act="inside",actLowerVarName="GZBLo",actUpperVarName="GZBHi",actBasis="netwt",actMotionInhibit=true,deact="outside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=false}setPointDisabled={mode="disabled"}if checkweigh.CheckOutputType==0 then
if checkweigh.CheckAutoTareAtTarget==0 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7)retVal,resultMsg=awtx.setPoint.set(3,setPoint6And7)if checkweigh.CheckOutputGZB==1 then
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderWMotionGross)else
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderWMotionGross)end
retVal,resultMsg=awtx.setPoint.set(5,setPointTargetWMotion)retVal,resultMsg=awtx.setPoint.set(6,setPointOverWMotion)elseif checkweigh.CheckAutoTareAtTarget==1 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7Net)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7Net)retVal,resultMsg=awtx.setPoint.set(3,setPoint6And7Net)if checkweigh.CheckOutputGZB==1 then
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderWMotionGross)else
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderWMotionGross)end
retVal,resultMsg=awtx.setPoint.set(5,setPointTargetWMotion)retVal,resultMsg=awtx.setPoint.set(6,setPointOverWMotion)end
else
if checkweigh.CheckAutoTareAtTarget==0 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7)retVal,resultMsg=awtx.setPoint.set(3,setPoint6And7)if checkweigh.CheckOutputGZB==1 then
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderGross)else
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderGross)end
retVal,resultMsg=awtx.setPoint.set(5,setPointTarget)retVal,resultMsg=awtx.setPoint.set(6,setPointOver)elseif checkweigh.CheckAutoTareAtTarget==1 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7Net)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7Net)retVal,resultMsg=awtx.setPoint.set(3,setPoint6And7Net)if checkweigh.CheckOutputGZB==1 then
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderWMotionGross)else
retVal,resultMsg=awtx.setPoint.set(4,setPointUnderWMotionGross)end
retVal,resultMsg=awtx.setPoint.set(5,setPointTargetWMotion)retVal,resultMsg=awtx.setPoint.set(6,setPointOverWMotion)end
end
if checkweigh.CheckOutputType==0 then
retVal,resultMsg=awtx.setPoint.set(7,setPoint7User)else
retVal,resultMsg=awtx.setPoint.set(7,setPoint7User)end
if checkweigh.CheckAutoTareAtTarget==0 then
retVal,resultMsg=awtx.setPoint.set(8,setPointOutEx8)elseif checkweigh.CheckAutoTareAtTarget==1 then
retVal,resultMsg=awtx.setPoint.set(8,setPointOutEx8Net)end
retVal,resultMsg=awtx.setPoint.set(9,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(10,setPointDisabled)result=awtx.setPoint.registerOutputEvent(1,checkweigh.setpointUnderEvent)result=awtx.setPoint.registerOutputEvent(2,checkweigh.setpointTargetEvent)result=awtx.setPoint.registerOutputEvent(3,checkweigh.setpointOverEvent)result=awtx.setPoint.registerOutputEvent(4,checkweigh.setpoint4Event)result=awtx.setPoint.registerOutputEvent(5,checkweigh.setpoint5Event)result=awtx.setPoint.registerOutputEvent(6,checkweigh.setpoint6Event)result=awtx.setPoint.registerOutputEvent(7,checkweigh.setpoint7Event)result=awtx.setPoint.registerOutputEvent(8,checkweigh.setpoint8Event)checkweigh.refreshSetpointsValues()end
function checkweigh.setpointUnderEvent(setpointNum,isActivate)if isActivate then
checkweigh.doUnderEventStuff()else
end
end
function checkweigh.setpointTargetEvent(setpointNum,isActivate)if isActivate then
checkweigh.doTargetEventStuff()else
end
end
function checkweigh.setpointOverEvent(setpointNum,isActivate)if isActivate then
checkweigh.doOverEventStuff()else
end
end
function checkweigh.setpoint4Event(setpointNum,isActivate)if isActivate then
else
end
end
function checkweigh.setpoint5Event(setpointNum,isActivate)if isActivate then
else
end
end
function checkweigh.setpoint6Event(setpointNum,isActivate)if isActivate then
else
end
end
function checkweigh.setpoint7Event(setpointNum,isActivate)if isActivate then
else
end
end
function checkweigh.setpoint8Event(setpointNum,isActivate)if isActivate then
checkweigh.doNearZero()else
result=awtx.setPoint.outputSet(7)end
end
function checkweigh.setpoint9Event(setpointNum,isActivate)if isActivate then
else
end
end
function checkweigh.refreshSetpointsRejectAcceptBattery()local newsetptBasis,offInsideGZBFlag,retVal,resultMsg,result
local setPointReject,setPointRejectWMotion,setPointAccept,setPointAcceptWMotion,setPointBattery
local setPoint4And7,setPoint5And7,setPoint6And7,setPoint7User,setPointOutEx8,setPointDisabled
if checkweigh.CheckBasis==VAL_GROSS then
newsetptBasis="grossWt"elseif checkweigh.CheckBasis==VAL_NET then
newsetptBasis="netwt"elseif checkweigh.CheckBasis==VAL_NET_MINUS_ANNUN then
newsetptBasis="netwt"elseif checkweigh.CheckBasis==VAL_COUNT then
newsetptBasis="count"elseif checkweigh.CheckBasis==VAL_PERCENT then
newsetptBasis="percent"else
newsetptBasis="grossWt"end
if checkweigh.CheckOutputGZB==1 then
offInsideGZBFlag=true
else
offInsideGZBFlag=false
end
setPointReject={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="outside",actLowerVarName="checkweigh.CheckTargLo",actUpperVarName="checkweigh.CheckTargHi",actBasis=newsetptBasis,actMotionInhibit=false,deact="inside",deactLowerVarName="checkweigh.CheckTargLo",deactUpperVarName="checkweigh.CheckTargHi",deactBasis=newsetptBasis,deactMotionInhibit=false}setPointRejectWMotion={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="outside",actLowerVarName="checkweigh.CheckTargLo",actUpperVarName="checkweigh.CheckTargHi",actBasis=newsetptBasis,actMotionInhibit=true,deact="inside",deactLowerVarName="checkweigh.CheckTargLo",deactUpperVarName="checkweigh.CheckTargHi",deactBasis=newsetptBasis,deactMotionInhibit=true}setPointAccept={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="checkweigh.CheckTargLo",actUpperVarName="checkweigh.CheckTargHi",actBasis=newsetptBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="checkweigh.CheckTargLo",deactUpperVarName="checkweigh.CheckTargHi",deactBasis=newsetptBasis,deactMotionInhibit=false}setPointAcceptWMotion={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="checkweigh.CheckTargLo",actUpperVarName="checkweigh.CheckTargHi",actBasis=newsetptBasis,actMotionInhibit=true,deact="outside",deactLowerVarName="checkweigh.CheckTargLo",deactUpperVarName="checkweigh.CheckTargHi",deactBasis=newsetptBasis,deactMotionInhibit=true}setPointBattery={mode="output",bounceTime=minLatchTime,offInsideGZB=false,act="user",deact="user"}setPoint4And7={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=4,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPoint4And7Net={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=4,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=false}setPoint5And7={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=5,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPoint5And7Net={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=5,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=false}setPoint6And7={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=6,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPoint6And7Net={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=6,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=false}setPoint7User={mode="output",bounceTime=0,offInsideGZB=offInsideGZBFlag,act="user",deact="user"}setPointOutEx8={mode="output",bounceTime=0,offInsideGZB=false,act="inside",actLowerVarName="GZBLo",actUpperVarName="GZBHi",actBasis="grossWt",actMotionInhibit=false,deact="outside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPointOutEx8Net={mode="output",bounceTime=0,offInsideGZB=false,act="inside",actLowerVarName="GZBLo",actUpperVarName="GZBHi",actBasis="netwt",actMotionInhibit=true,deact="outside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=false}setPointDisabled={mode="disabled"}if checkweigh.CheckOutputType==0 then
if checkweigh.CheckAutoTareAtTarget==0 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7)retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)retVal,resultMsg=awtx.setPoint.set(4,setPointRejectWMotion)retVal,resultMsg=awtx.setPoint.set(5,setPointAcceptWMotion)retVal,resultMsg=awtx.setPoint.set(6,setPointDisabled)elseif checkweigh.CheckAutoTareAtTarget==1 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7Net)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7Net)retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)retVal,resultMsg=awtx.setPoint.set(4,setPointRejectWMotion)retVal,resultMsg=awtx.setPoint.set(5,setPointAcceptWMotion)retVal,resultMsg=awtx.setPoint.set(6,setPointDisabled)end
else
if checkweigh.CheckAutoTareAtTarget==0 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7)retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)retVal,resultMsg=awtx.setPoint.set(4,setPointReject)retVal,resultMsg=awtx.setPoint.set(5,setPointAccept)retVal,resultMsg=awtx.setPoint.set(6,setPointDisabled)elseif checkweigh.CheckAutoTareAtTarget==1 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7Net)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7Net)retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)retVal,resultMsg=awtx.setPoint.set(4,setPointRejectWMotion)retVal,resultMsg=awtx.setPoint.set(5,setPointAcceptWMotion)retVal,resultMsg=awtx.setPoint.set(6,setPointDisabled)end
end
if checkweigh.CheckOutputType==0 then
retVal,resultMsg=awtx.setPoint.set(7,setPoint7User)else
retVal,resultMsg=awtx.setPoint.set(7,setPoint7User)end
if checkweigh.CheckAutoTareAtTarget==0 then
retVal,resultMsg=awtx.setPoint.set(8,setPointOutEx8)elseif checkweigh.CheckAutoTareAtTarget==1 then
retVal,resultMsg=awtx.setPoint.set(8,setPointOutEx8Net)end
retVal,resultMsg=awtx.setPoint.set(9,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(10,setPointDisabled)result=awtx.setPoint.registerOutputEvent(1,checkweigh.setpointRejectEvent)result=awtx.setPoint.registerOutputEvent(2,checkweigh.setpointAcceptEvent)result=awtx.setPoint.registerOutputEvent(3,checkweigh.setpointBatteryEvent)result=awtx.setPoint.registerOutputEvent(4,checkweigh.setpoint4Event)result=awtx.setPoint.registerOutputEvent(5,checkweigh.setpoint5Event)result=awtx.setPoint.registerOutputEvent(7,checkweigh.setpoint7Event)result=awtx.setPoint.registerOutputEvent(8,checkweigh.setpoint8Event)checkweigh.refreshSetpointsValues()end
function checkweigh.refreshSetpointsRejectAcceptBatteryNeg()local newsetptBasis,offInsideGZBFlag,retVal,resultMsg,result
local setPointReject,setPointRejectWMotion,setPointAccept,setPointAcceptWMotion,setPointBattery
local setPoint4And7,setPoint5And7,setPoint6And7,setPoint7User,setPointOutEx8,setPointDisabled
if checkweigh.CheckBasis==VAL_GROSS then
newsetptBasis="grossWt"elseif checkweigh.CheckBasis==VAL_NET then
newsetptBasis="netwt"elseif checkweigh.CheckBasis==VAL_NET_MINUS_ANNUN then
newsetptBasis="netwt"elseif checkweigh.CheckBasis==VAL_COUNT then
newsetptBasis="count"elseif checkweigh.CheckBasis==VAL_PERCENT then
newsetptBasis="percent"else
newsetptBasis="grossWt"end
if checkweigh.CheckOutputGZB==1 then
offInsideGZBFlag=true
else
offInsideGZBFlag=false
end
setPointReject={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="outside",actLowerVarName="checkweigh.CheckTargHi",actUpperVarName="checkweigh.CheckTargLo",actBasis=newsetptBasis,actMotionInhibit=false,deact="inside",deactLowerVarName="checkweigh.CheckTargHi",deactUpperVarName="checkweigh.CheckTargLo",deactBasis=newsetptBasis,deactMotionInhibit=false}setPointRejectWMotion={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="outside",actLowerVarName="checkweigh.CheckTargHi",actUpperVarName="checkweigh.CheckTargLo",actBasis=newsetptBasis,actMotionInhibit=true,deact="inside",deactLowerVarName="checkweigh.CheckTargHi",deactUpperVarName="checkweigh.CheckTargLo",deactBasis=newsetptBasis,deactMotionInhibit=true}setPointAccept={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="checkweigh.CheckTargHi",actUpperVarName="checkweigh.CheckTargLo",actBasis=newsetptBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="checkweigh.CheckTargHi",deactUpperVarName="checkweigh.CheckTargLo",deactBasis=newsetptBasis,deactMotionInhibit=false}setPointAcceptWMotion={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="checkweigh.CheckTargHi",actUpperVarName="checkweigh.CheckTargLo",actBasis=newsetptBasis,actMotionInhibit=true,deact="outside",deactLowerVarName="checkweigh.CheckTargHi",deactUpperVarName="checkweigh.CheckTargLo",deactBasis=newsetptBasis,deactMotionInhibit=true}setPointBattery={mode="output",bounceTime=minLatchTime,offInsideGZB=false,act="user",deact="user"}setPoint4And7={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=4,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPoint4And7Net={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=4,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=false}setPoint5And7={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=5,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPoint5And7Net={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=5,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=false}setPoint6And7={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=6,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPoint6And7Net={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="AND",actLower=6,actUpper=7,deact="inside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=false}setPoint7User={mode="output",bounceTime=0,offInsideGZB=offInsideGZBFlag,act="user",deact="user"}setPointOutEx8={mode="output",bounceTime=0,offInsideGZB=false,act="inside",actLowerVarName="GZBLo",actUpperVarName="GZBHi",actBasis="grossWt",actMotionInhibit=false,deact="outside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPointOutEx8Net={mode="output",bounceTime=0,offInsideGZB=false,act="inside",actLowerVarName="GZBLo",actUpperVarName="GZBHi",actBasis="netwt",actMotionInhibit=true,deact="outside",deactLowerVarName="GZBLo",deactUpperVarName="GZBHi",deactBasis="netwt",deactMotionInhibit=false}setPointDisabled={mode="disabled"}if checkweigh.CheckOutputType==0 then
if checkweigh.CheckAutoTareAtTarget==0 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7)retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)retVal,resultMsg=awtx.setPoint.set(4,setPointRejectWMotion)retVal,resultMsg=awtx.setPoint.set(5,setPointAcceptWMotion)retVal,resultMsg=awtx.setPoint.set(6,setPointDisabled)elseif checkweigh.CheckAutoTareAtTarget==1 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7Net)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7Net)retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)retVal,resultMsg=awtx.setPoint.set(4,setPointRejectWMotion)retVal,resultMsg=awtx.setPoint.set(5,setPointAcceptWMotion)retVal,resultMsg=awtx.setPoint.set(6,setPointDisabled)end
else
if checkweigh.CheckAutoTareAtTarget==0 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7)retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)retVal,resultMsg=awtx.setPoint.set(4,setPointReject)retVal,resultMsg=awtx.setPoint.set(5,setPointAccept)retVal,resultMsg=awtx.setPoint.set(6,setPointDisabled)elseif checkweigh.CheckAutoTareAtTarget==1 then
retVal,resultMsg=awtx.setPoint.set(1,setPoint4And7Net)retVal,resultMsg=awtx.setPoint.set(2,setPoint5And7Net)retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)retVal,resultMsg=awtx.setPoint.set(4,setPointRejectWMotion)retVal,resultMsg=awtx.setPoint.set(5,setPointAcceptWMotion)retVal,resultMsg=awtx.setPoint.set(6,setPointDisabled)end
end
if checkweigh.CheckOutputType==0 then
retVal,resultMsg=awtx.setPoint.set(7,setPoint7User)else
retVal,resultMsg=awtx.setPoint.set(7,setPoint7User)end
if checkweigh.CheckAutoTareAtTarget==0 then
retVal,resultMsg=awtx.setPoint.set(8,setPointOutEx8)elseif checkweigh.CheckAutoTareAtTarget==1 then
retVal,resultMsg=awtx.setPoint.set(8,setPointOutEx8Net)end
retVal,resultMsg=awtx.setPoint.set(9,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(10,setPointDisabled)result=awtx.setPoint.registerOutputEvent(1,checkweigh.setpointRejectEvent)result=awtx.setPoint.registerOutputEvent(2,checkweigh.setpointAcceptEvent)result=awtx.setPoint.registerOutputEvent(3,checkweigh.setpointBatteryEvent)result=awtx.setPoint.registerOutputEvent(4,checkweigh.setpoint4Event)result=awtx.setPoint.registerOutputEvent(5,checkweigh.setpoint5Event)result=awtx.setPoint.registerOutputEvent(7,checkweigh.setpoint7Event)result=awtx.setPoint.registerOutputEvent(8,checkweigh.setpoint8Event)checkweigh.refreshSetpointsValues()end
function checkweigh.setpointRejectEvent(setpointNum,isActivate)if isActivate then
checkweigh.doRejectEventStuff()else
end
end
function checkweigh.setpointAcceptEvent(setpointNum,isActivate)if isActivate then
checkweigh.doAcceptEventStuff()else
end
end
function checkweigh.setpointBatteryEvent(setpointNum,isActivate)if isActivate then
else
end
end
function checkweigh.setpointInputAction(cfgInputType)if Input_Type_String[cfgInputType]=="None     "then
elseif Input_Type_String[cfgInputType]=="PrintKey "then
awtx.keypad.KEY_PRINT_DOWN()elseif Input_Type_String[cfgInputType]=="UnitsKey "then
awtx.keypad.KEY_UNITS_UP()elseif Input_Type_String[cfgInputType]=="SelectKey"then
awtx.keypad.KEY_SELECT_UP()elseif Input_Type_String[cfgInputType]=="TareKey  "then
awtx.keypad.KEY_TARE_DOWN()elseif Input_Type_String[cfgInputType]=="ZeroKey  "then
awtx.keypad.KEY_ZERO_DOWN()elseif Input_Type_String[cfgInputType]=="SampleKey"then
awtx.keypad.KEY_SAMPLE_UP()elseif Input_Type_String[cfgInputType]=="F1Key    "then
awtx.keypad.KEY_F1_UP()elseif Input_Type_String[cfgInputType]=="TargetKey"then
awtx.keypad.KEY_TARGET_UP()elseif Input_Type_String[cfgInputType]=="StartKey "then
awtx.keypad.KEY_START_UP()elseif Input_Type_String[cfgInputType]=="StopKey  "then
awtx.keypad.KEY_STOP_UP()elseif Input_Type_String[cfgInputType]=="IDKey    "then
awtx.keypad.KEY_ID_UP()elseif Input_Type_String[cfgInputType]=="SetupKey "then
awtx.keypad.KEY_SETUP_UP()elseif Input_Type_String[cfgInputType]=="UnderKey "then
awtx.keypad.KEY_UNDER_UP()elseif Input_Type_String[cfgInputType]=="OverKey  "then
awtx.keypad.KEY_OVER_UP()elseif Input_Type_String[cfgInputType]=="AccumKey "then
awtx.keypad.KEY_ACCUM_UP()elseif Input_Type_String[cfgInputType]=="BaseKey  "then
awtx.keypad.KEY_BASE_UP()elseif Input_Type_String[cfgInputType]=="PrintHold"then
printHoldFlag=HowManyRepeatsMakeAHold-1
awtx.keypad.KEY_PRINT_REPEAT()elseif Input_Type_String[cfgInputType]=="User     "then
else
end
end
function checkweigh.setpointIn1Handler(setpointNum,inputState)if inputState then
checkweigh.setpointInputAction(SetpointInputtable[1].InputValue)else
end
end
function checkweigh.setpointIn2Handler(setpointNum,inputState)if inputState then
checkweigh.setpointInputAction(SetpointInputtable[2].InputValue)else
end
end
function checkweigh.setpointIn3Handler(setpointNum,inputState)if inputState then
checkweigh.setpointInputAction(SetpointInputtable[3].InputValue)else
end
end
function checkweigh.refreshSetpointsInputs()local retVal,resultMsg,result
local setPointInEx1,setPointInEx2,setPointInEx3
setPointInEx1={mode="input",bounceTime=1}setPointInEx2={mode="input",bounceTime=1}setPointInEx3={mode="input",bounceTime=1}retVal,resultMsg=awtx.setPoint.set(1,setPointInEx1)retVal,resultMsg=awtx.setPoint.set(2,setPointInEx2)retVal,resultMsg=awtx.setPoint.set(3,setPointInEx3)result=awtx.setPoint.registerInputEvent(1,checkweigh.setpointIn1Handler)result=awtx.setPoint.registerInputEvent(2,checkweigh.setpointIn2Handler)result=awtx.setPoint.registerInputEvent(3,checkweigh.setpointIn3Handler)setpoint.setSetpointPrintTokens()end
function checkweigh.initAll()if checkweigh.CheckWeighMode==SIM375 then
checkweigh.CheckMinDefault=wt.curCapacity*-1
checkweigh.CheckMaxDefault=wt.curCapacity
checkweigh.CheckTolLoDefault=wt.curDivision
checkweigh.CheckTolHiDefault=wt.curDivision
elseif checkweigh.CheckWeighMode==PER375 then
checkweigh.CheckMinDefault=0.0
checkweigh.CheckMaxDefault=100.0
checkweigh.CheckTolLoDefault=0.1
checkweigh.CheckTolHiDefault=0.1
else
checkweigh.CheckMinDefault=wt.curDivision*config.grossZeroBand
checkweigh.CheckMaxDefault=wt.curCapacity
checkweigh.CheckTolLoDefault=wt.curDivision
checkweigh.CheckTolHiDefault=wt.curDivision
end
transaction.TransactionCount=0
transaction.TransactionSysCount=0
checkweigh.transactionRecall()checkweigh.recallCheckAll()checkweigh.refreshCheckGraph()awtx.weight.setAccum(transaction.TransGrossWtTotal,transaction.TransNetWtTotal,transaction.TransCountTotal,transaction.TransactionCount)end
Stat.setStatsPrintTokens()checkweigh.setCheckPrintTokens()transaction.setTransPrintTokens()checkweigh.setXR4500PrintToken()checkweigh.checkweighDBInit()checkweigh.checkweighDBRecall()tempMyDateTime=doMyDateTime()