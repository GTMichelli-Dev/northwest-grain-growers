plu={}if config.calwtunitStr~=nil then
pluCalUnit=config.calwtunitStr
end
if system.modelStr=="ZM301"then
MIN_PLUNUMBER=0
MAX_PLUNUMBER=1
elseif system.modelStr=="ZM303"then
MIN_PLUNUMBER=0
MAX_PLUNUMBER=10
elseif system.modelStr=="ZQ375"then
MIN_PLUNUMBER=0
MAX_PLUNUMBER=500
elseif system.modelStr=="ZM305GTN"then
MIN_PLUNUMBER=0
MAX_PLUNUMBER=10
elseif system.modelStr=="ZM305"then
MIN_PLUNUMBER=0
MAX_PLUNUMBER=10
else
MIN_PLUNUMBER=0
MAX_PLUNUMBER=1
end
plu.AutoUpdateFlagDefault=0
plu.AutoUpdateFlag=plu.AutoUpdateFlagDefault
plu.PLUNumberDefault=0
plu.PLUTargLoDefault=wt.net-wt.curDivision
plu.PLUTargHiDefault=wt.net+wt.curDivision
plu.PLUTolLoDefault=wt.curDivision
plu.PLUTolHiDefault=wt.curDivision
plu.PLUTargDefault=wt.net
plu.PLUTareDefault=0
plu.PLUSubTransCountDefault=0
plu.PLUSubGrossAccumDefault=0
plu.PLUSubNetAccumDefault=0
plu.PLUSubUnderAccumDefault=0
plu.PLUSubTargAccumDefault=0
plu.PLUSubOverAccumDefault=0
plu.PLUSubUnderCountDefault=0
plu.PLUSubTargCountDefault=0
plu.PLUSubOverCountDefault=0
plu.PLUTransCountDefault=0
plu.PLUGrossAccumDefault=0
plu.PLUNetAccumDefault=0
plu.PLUUnderAccumDefault=0
plu.PLUTargAccumDefault=0
plu.PLUOverAccumDefault=0
plu.PLUUnderCountDefault=0
plu.PLUTargCountDefault=0
plu.PLUOverCountDefault=0
plu.PLUPcwtDefault=0
plu.PLUunitsDefault=pluCalUnit
plu.PLUNumber=plu.PLUNumberDefault
plu.PLUTargLo=plu.PLUTargLoDefault
plu.PLUTargHi=plu.PLUTargHiDefault
plu.PLUTolLo=plu.PLUTolLoDefault
plu.PLUTolHi=plu.PLUTolHiDefault
plu.PLUTarg=plu.PLUTargDefault
plu.PLUTare=plu.PLUTareDefault
plu.PLUSubTransCount=plu.PLUSubTransCountDefault
plu.PLUSubGrossAccum=plu.PLUSubGrossAccumDefault
plu.PLUSubNetAccum=plu.PLUSubNetAccumDefault
plu.PLUSubUnderAccum=plu.PLUSubUnderAccumDefault
plu.PLUSubTargAccum=plu.PLUSubTargAccumDefault
plu.PLUSubOverAccum=plu.PLUSubOverAccumDefault
plu.PLUSubUnderCount=plu.PLUSubUnderCountDefault
plu.PLUSubTargCount=plu.PLUSubTargCountDefault
plu.PLUSubOverCount=plu.PLUSubOverCountDefault
plu.PLUTransCount=plu.PLUTransCountDefault
plu.PLUGrossAccum=plu.PLUGrossAccumDefault
plu.PLUNetAccum=plu.PLUNetAccumDefault
plu.PLUUnderAccum=plu.PLUUnderAccumDefault
plu.PLUTargAccum=plu.PLUTargAccumDefault
plu.PLUOverAccum=plu.PLUOverAccumDefault
plu.PLUUnderCount=plu.PLUUnderCountDefault
plu.PLUTargCount=plu.PLUTargCountDefault
plu.PLUOverCount=plu.PLUOverCountDefault
plu.PLUPcwt=plu.PLUPcwtDefault
plu.PLUunits=plu.PLUunitsDefault
newPLUChannelDefault=0
newPLUChannel=newPLUChannelDefault
function plu.initPLUPrintTokens()awtx.fmtPrint.varSet(19,plu.PLUTare,"Tare",AWTX_LUA_FLOAT)printTokens[19].varName="plu.PLUTare"printTokens[19].varLabel="Tare"printTokens[19].varType=AWTX_LUA_FLOAT
printTokens[19].varValue=plu.PLUTare
printTokens[19].varFunct=""awtx.fmtPrint.varSet(20,plu.PLUSubTransCount,"Transaction SubTotal Count",AWTX_LUA_INTEGER)printTokens[20].varName="plu.PLUSubTransCount"printTokens[20].varLabel="Transaction SubTotal Count"printTokens[20].varType=AWTX_LUA_INTEGER
printTokens[20].varValue=plu.PLUSubTransCount
printTokens[20].varFunct=""awtx.fmtPrint.varSet(21,plu.PLUSubGrossAccum,"Gross SubTotal Accum",AWTX_LUA_FLOAT)printTokens[21].varName="plu.PLUSubGrossAccum"printTokens[21].varLabel="Gross SubTotal Accum"printTokens[21].varType=AWTX_LUA_FLOAT
printTokens[21].varValue=plu.PLUSubGrossAccum
printTokens[21].varFunct=""awtx.fmtPrint.varSet(22,plu.PLUSubNetAccum,"Net SubTotal Accum",AWTX_LUA_FLOAT)printTokens[22].varName="plu.PLUSubNetAccum"printTokens[22].varLabel="Net SubTotal Accum"printTokens[22].varType=AWTX_LUA_FLOAT
printTokens[22].varValue=plu.PLUSubNetAccum
printTokens[22].varFunct=""awtx.fmtPrint.varSet(23,plu.PLUSubUnderAccum,"Under SubTotal Accum",AWTX_LUA_FLOAT)printTokens[23].varName="plu.PLUSubUnderAccum"printTokens[23].varLabel="Under SubTotal Accum"printTokens[23].varType=AWTX_LUA_FLOAT
printTokens[23].varValue=plu.PLUSubUnderAccum
printTokens[23].varFunct=""awtx.fmtPrint.varSet(24,plu.PLUSubTargAccum,"Target SubTotal Accum",AWTX_LUA_FLOAT)printTokens[24].varName="plu.PLUSubTargAccum"printTokens[24].varLabel="Target SubTotal Accum"printTokens[24].varType=AWTX_LUA_FLOAT
printTokens[24].varValue=plu.PLUSubTargAccum
printTokens[24].varFunct=""awtx.fmtPrint.varSet(25,plu.PLUSubOverAccum,"Over SubTotal Accum",AWTX_LUA_FLOAT)printTokens[25].varName="plu.PLUSubOverAccum"printTokens[25].varLabel="Over SubTotal Accum"printTokens[25].varType=AWTX_LUA_FLOAT
printTokens[25].varValue=plu.PLUSubOverAccum
printTokens[25].varFunct=""awtx.fmtPrint.varSet(26,plu.PLUSubUnderCount,"Under SubTotal Count",AWTX_LUA_INTEGER)printTokens[26].varName="plu.PLUSubUnderCount"printTokens[26].varLabel="Under SubTotal Count"printTokens[26].varType=AWTX_LUA_INTEGER
printTokens[26].varValue=plu.PLUSubUnderCount
printTokens[26].varFunct=""awtx.fmtPrint.varSet(27,plu.PLUSubTargCount,"Target SubTotal Count",AWTX_LUA_INTEGER)printTokens[27].varName="plu.PLUSubTargCount"printTokens[27].varLabel="Target SubTotal Count"printTokens[27].varType=AWTX_LUA_INTEGER
printTokens[27].varValue=plu.PLUSubTargCount
printTokens[27].varFunct=""awtx.fmtPrint.varSet(28,plu.PLUSubOverCount,"Over SubTotal Count",AWTX_LUA_INTEGER)printTokens[28].varName="plu.PLUSubOverCount"printTokens[28].varLabel="Over SubTotal Count"printTokens[28].varType=AWTX_LUA_INTEGER
printTokens[28].varValue=plu.PLUSubOverCount
printTokens[28].varFunct=""awtx.fmtPrint.varSet(71,newPLUChannel,"PLU Channel",AWTX_LUA_INTEGER)printTokens[71].varName="newPLUChannel"printTokens[71].varLabel="PLU Channel"printTokens[71].varType=AWTX_LUA_INTEGER
printTokens[71].varValue=newPLUChannel
printTokens[71].varFunct=plu.setPLUChannel
awtx.fmtPrint.varSet(72,plu.PLUNumber,"PLU Number",AWTX_LUA_INTEGER)printTokens[72].varName="plu.PLUNumber"printTokens[72].varLabel="PLU Number"printTokens[72].varType=AWTX_LUA_INTEGER
printTokens[72].varValue=plu.PLUNumber
printTokens[72].varFunct=""awtx.fmtPrint.varSet(73,plu.PLUTargLo,"Target Lo",AWTX_LUA_FLOAT)printTokens[73].varName="plu.PLUTargLo"printTokens[73].varLabel="PLU Channel"printTokens[73].varType=AWTX_LUA_INTEGER
printTokens[73].varValue=plu.PLUTargLo
printTokens[73].varFunct=""awtx.fmtPrint.varSet(74,plu.PLUTargHi,"Target Hi",AWTX_LUA_FLOAT)printTokens[74].varName="plu.PLUTargHi"printTokens[74].varLabel="Target Hi"printTokens[74].varType=AWTX_LUA_FLOAT
printTokens[74].varValue=plu.PLUTargHi
printTokens[74].varFunct=""awtx.fmtPrint.varSet(75,plu.PLUTolLo,"Tolerance Lo",AWTX_LUA_FLOAT)printTokens[75].varName="plu.PLUTolLo"printTokens[75].varLabel="Tolerance Lo"printTokens[75].varType=AWTX_LUA_FLOAT
printTokens[75].varValue=plu.PLUTolLo
printTokens[75].varFunct=""awtx.fmtPrint.varSet(76,plu.PLUTolHi,"Tolerance Hi",AWTX_LUA_FLOAT)printTokens[76].varName="plu.PLUTolHi"printTokens[76].varLabel="Tolerance Hi"printTokens[76].varType=AWTX_LUA_FLOAT
printTokens[76].varValue=plu.PLUTolHi
printTokens[76].varFunct=""awtx.fmtPrint.varSet(77,plu.PLUTarg,"Target",AWTX_LUA_FLOAT)printTokens[77].varName="plu.PLUTarg"printTokens[77].varLabel="Target"printTokens[77].varType=AWTX_LUA_FLOAT
printTokens[77].varValue=plu.PLUTarg
printTokens[77].varFunct=""awtx.fmtPrint.varSet(78,plu.PLUTransCount,"Transaction Count",AWTX_LUA_INTEGER)printTokens[78].varName="plu.PLUTransCount"printTokens[78].varLabel="Transaction Count"printTokens[78].varType=AWTX_LUA_INTEGER
printTokens[78].varValue=plu.PLUTransCount
printTokens[78].varFunct=""awtx.fmtPrint.varSet(79,plu.PLUGrossAccum,"Gross Accum",AWTX_LUA_FLOAT)printTokens[79].varName="plu.PLUGrossAccum"printTokens[79].varLabel="Gross Accum"printTokens[79].varType=AWTX_LUA_FLOAT
printTokens[79].varValue=plu.PLUGrossAccum
printTokens[79].varFunct=""awtx.fmtPrint.varSet(80,plu.PLUNetAccum,"Net Accum",AWTX_LUA_FLOAT)printTokens[80].varName="plu.PLUNetAccum"printTokens[80].varLabel="Net Accum"printTokens[80].varType=AWTX_LUA_FLOAT
printTokens[80].varValue=plu.PLUNetAccum
printTokens[80].varFunct=""awtx.fmtPrint.varSet(81,plu.PLUUnderAccum,"Under Accum",AWTX_LUA_FLOAT)printTokens[81].varName="plu.PLUUnderAccum"printTokens[81].varLabel="Under Accum"printTokens[81].varType=AWTX_LUA_FLOAT
printTokens[81].varValue=plu.PLUUnderAccum
printTokens[81].varFunct=""awtx.fmtPrint.varSet(82,plu.PLUTargAccum,"Target Accum",AWTX_LUA_FLOAT)printTokens[82].varName="plu.PLUTargAccum"printTokens[82].varLabel="Target Accum"printTokens[82].varType=AWTX_LUA_FLOAT
printTokens[82].varValue=plu.PLUTargAccum
printTokens[82].varFunct=""awtx.fmtPrint.varSet(83,plu.PLUOverAccum,"Over Accum",AWTX_LUA_FLOAT)printTokens[83].varName="plu.PLUOverAccum"printTokens[83].varLabel="Over Accum"printTokens[83].varType=AWTX_LUA_FLOAT
printTokens[83].varValue=plu.PLUOverAccum
printTokens[83].varFunct=""awtx.fmtPrint.varSet(84,plu.PLUUnderCount,"Under Count",AWTX_LUA_INTEGER)printTokens[84].varName="plu.PLUUnderCount"printTokens[84].varLabel="Under Count"printTokens[84].varType=AWTX_LUA_INTEGER
printTokens[84].varValue=plu.PLUUnderCount
printTokens[84].varFunct=""awtx.fmtPrint.varSet(85,plu.PLUTargCount,"Target Count",AWTX_LUA_INTEGER)printTokens[85].varName="plu.PLUTargCount"printTokens[85].varLabel="Target Count"printTokens[85].varType=AWTX_LUA_INTEGER
printTokens[85].varValue=plu.PLUTargCount
printTokens[85].varFunct=""awtx.fmtPrint.varSet(86,plu.PLUOverCount,"Over Count",AWTX_LUA_INTEGER)printTokens[86].varName="plu.PLUOverCount"printTokens[86].varLabel="Over Count"printTokens[86].varType=AWTX_LUA_INTEGER
printTokens[86].varValue=plu.PLUOverCount
printTokens[86].varFunct=""awtx.fmtPrint.varSet(87,plu.PLUPcwt,"Pieceweight",AWTX_LUA_FLOAT)printTokens[87].varName="plu.PLUPcwt"printTokens[87].varLabel="Pieceweight"printTokens[87].varType=AWTX_LUA_FLOAT
printTokens[87].varValue=plu.PLUPcwt
printTokens[87].varFunct=""awtx.fmtPrint.varSet(88,plu.PLUunits,"units",AWTX_LUA_STRING)printTokens[88].varName="plu.PLUunits"printTokens[88].varLabel="units"printTokens[88].varType=AWTX_LUA_STRING
printTokens[88].varValue=plu.PLUunits
printTokens[88].varFunct=""end
function plu.setPLUPrintTokens()awtx.fmtPrint.varSet(19,plu.PLUTare,"Tare",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(20,plu.PLUSubTransCount,"Transaction SubTotal Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(21,plu.PLUSubGrossAccum,"Gross SubTotal Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(22,plu.PLUSubNetAccum,"Net SubTotal Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(23,plu.PLUSubUnderAccum,"Under SubTotal Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(24,plu.PLUSubTargAccum,"Target SubTotal Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(25,plu.PLUSubOverAccum,"Over SubTotal Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(26,plu.PLUSubUnderCount,"Under SubTotal Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(27,plu.PLUSubTargCount,"Target SubTotal Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(28,plu.PLUSubOverCount,"Over SubTotal Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(71,newPLUChannel,"PLU Channel",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(72,plu.PLUNumber,"PLU Number",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(73,plu.PLUTargLo,"Target Lo",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(74,plu.PLUTargHi,"Target Hi",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(75,plu.PLUTolLo,"Tolerance Lo",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(76,plu.PLUTolHi,"Tolerance Hi",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(77,plu.PLUTarg,"Target",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(78,plu.PLUTransCount,"Transaction Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(79,plu.PLUGrossAccum,"Gross Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(80,plu.PLUNetAccum,"Net Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(81,plu.PLUUnderAccum,"Under Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(82,plu.PLUTargAccum,"Target Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(83,plu.PLUOverAccum,"Over Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(84,plu.PLUUnderCount,"Under Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(85,plu.PLUTargCount,"Target Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(86,plu.PLUOverCount,"Over Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(87,plu.PLUPcwt,"Pieceweight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(88,plu.PLUunits,"units",AWTX_LUA_STRING)end
function plu.updateWeightBasedSettingsAfterUnitsChange()if checkweigh.CheckWeighMode==PER375 then
plu.PLUTolLoDefault=0.1
plu.PLUTolHiDefault=0.1
else
plu.PLUTolLoDefault=wt.curDivision
plu.PLUTolHiDefault=wt.curDivision
end
end
function plu.PLUClear()config=awtx.weight.getConfig(1)wt=awtx.weight.getCurrent(1)plu.PLUNumber=0
plu.PLUTargLo=0
plu.PLUTargHi=0
plu.PLUTolLo=0
plu.PLUTolHi=0
plu.PLUTarg=0
plu.PLUTare=0
plu.PLUSubTransCount=0
plu.PLUSubGrossAccum=0
plu.PLUSubNetAccum=0
plu.PLUSubUnderAccum=0
plu.PLUSubTargAccum=0
plu.PLUSubOverAccum=0
plu.PLUSubUnderCount=0
plu.PLUSubTargCount=0
plu.PLUSubOverCount=0
plu.PLUTransCount=0
plu.PLUGrossAccum=0
plu.PLUNetAccum=0
plu.PLUUnderAccum=0
plu.PLUTargAccum=0
plu.PLUOverAccum=0
plu.PLUUnderCount=0
plu.PLUTargCount=0
plu.PLUOverCount=0
plu.PLUPcwt=0
plu.PLUunits=pluCalUnit
end
function plu.PLUInit()config=awtx.weight.getConfig(1)wt=awtx.weight.getCurrent(1)plu.PLUTargLoDefault=wt.net-wt.curDivision
plu.PLUTargHiDefault=wt.net+wt.curDivision
plu.PLUTolLoDefault=wt.curDivision
plu.PLUTolHiDefault=wt.curDivision
plu.PLUTargDefault=wt.net
plu.PLUTargLo=plu.PLUTargLoDefault
plu.PLUTargHi=plu.PLUTargHiDefault
plu.PLUTolLo=plu.PLUTolLoDefault
plu.PLUTolHi=plu.PLUTolHiDefault
plu.PLUTarg=plu.PLUTargDefault
plu.PLUTare=plu.PLUTareDefault
plu.PLUSubTransCount=plu.PLUSubTransCountDefault
plu.PLUSubGrossAccum=plu.PLUSubGrossAccumDefault
plu.PLUSubNetAccum=plu.PLUSubNetAccumDefault
plu.PLUSubUnderAccum=plu.PLUSubUnderAccumDefault
plu.PLUSubTargAccum=plu.PLUSubTargAccumDefault
plu.PLUSubOverAccum=plu.PLUSubOverAccumDefault
plu.PLUSubUnderCount=plu.PLUSubUnderCountDefault
plu.PLUSubTargCount=plu.PLUSubTargCountDefault
plu.PLUSubOverCount=plu.PLUSubOverCountDefault
plu.PLUTransCount=plu.PLUTransCountDefault
plu.PLUGrossAccum=plu.PLUGrossAccumDefault
plu.PLUNetAccum=plu.PLUNetAccumDefault
plu.PLUUnderAccum=plu.PLUUnderAccumDefault
plu.PLUTargAccum=plu.PLUTargAccumDefault
plu.PLUOverAccum=plu.PLUOverAccumDefault
plu.PLUUnderCount=plu.PLUUnderCountDefault
plu.PLUTargCount=plu.PLUTargCountDefault
plu.PLUOverCount=plu.PLUOverCountDefault
plu.PLUPcwt=plu.PLUPcwtDefault
plu.PLUunits=plu.PLUunitsDefault
end
local DB_FileLocation_AppConfig
local DB_FileLocation_AppData
local DB_FileLocation_Reports
local DB_ReportName_PLU
function plu.PLUDBInit()local simAppPath,dbFile,result,sqlStr
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]DB_FileLocation_AppData=[[C:\Database\AppData.db]]DB_FileLocation_Reports=[[G:\Reports]]..SerialNumber
DB_ReportName_PLU=[[\PLUReport.csv]]dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU2 (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTare DOUBLE, PLUSubTransCount INTEGER, PLUSubGrossAccum DOUBLE, PLUSubNetAccum DOUBLE, PLUSubUnderAccum DOUBLE, PLUSubTargAccum DOUBLE, PLUSubOverAccum DOUBLE, PLUSubUnderCount INTEGER, PLUSubTargCount INTEGER, PLUSubOverCount INTEGER, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLULastChan (Channel INTEGER)")dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLUConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:close()end
function plu.pluDBStore()local dbFile,result,searchIndex,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)dbFile:execute("BEGIN TRANSACTION")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU2 (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTare DOUBLE, PLUSubTransCount INTEGER, PLUSubGrossAccum DOUBLE, PLUSubNetAccum DOUBLE, PLUSubUnderAccum DOUBLE, PLUSubTargAccum DOUBLE, PLUSubOverAccum DOUBLE, PLUSubUnderCount INTEGER, PLUSubTargCount INTEGER, PLUSubOverCount INTEGER, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")searchIndex=plu.PLUNumber
sqlStr=string.format("SELECT tblPLU2.PLUNumber FROM tblPLU2 WHERE tblPLU2.PLUNumber = %d",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblPLU2 (PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTare, PLUSubTransCount, PLUSubGrossAccum, PLUSubNetAccum, PLUSubUnderAccum, PLUSubTargAccum, PLUSubOverAccum, PLUSubUnderCount, PLUSubTargCount, PLUSubOverCount, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits) VALUES ( '%d', '%f', '%f', '%f', '%f', '%f', '%f', '%d', '%f', '%f', '%f', '%f', '%f', '%d', '%d', '%d', '%d', '%f', '%f', '%f', '%f', '%f', '%d', '%d', '%d', '%f', '%s')",plu.PLUNumber,plu.PLUTargLo,plu.PLUTargHi,plu.PLUTolLo,plu.PLUTolHi,plu.PLUTarg,plu.PLUTare,plu.PLUSubTransCount,plu.PLUSubGrossAccum,plu.PLUSubNetAccum,plu.PLUSubUnderAccum,plu.PLUSubTargAccum,plu.PLUSubOverAccum,plu.PLUSubUnderCount,plu.PLUSubTargCount,plu.PLUSubOverCount,plu.PLUTransCount,plu.PLUGrossAccum,plu.PLUNetAccum,plu.PLUUnderAccum,plu.PLUTargAccum,plu.PLUOverAccum,plu.PLUUnderCount,plu.PLUTargCount,plu.PLUOverCount,plu.PLUPcwt,plu.PLUunits)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblPLU2 SET PLUNumber= '%d', PLUTargLo= '%f', PLUTargHi= '%f', PLUTolLo= '%f', PLUTolHi= '%f', PLUTarg= '%f', PLUTare= '%f', PLUSubTransCount= '%d', PLUSubGrossAccum= '%f', PLUSubNetAccum= '%f', PLUSubUnderAccum= '%f', PLUSubTargAccum= '%f', PLUSubOverAccum= '%f', PLUSubUnderCount= '%d', PLUSubTargCount= '%d', PLUSubOverCount= '%d', PLUTransCount= '%d', PLUGrossAccum= '%f', PLUNetAccum= '%f', PLUUnderAccum= '%f', PLUTargAccum= '%f', PLUOverAccum= '%f', PLUUnderCount= '%d', PLUTargCount= '%d', PLUOverCount= '%d', PLUPcwt= '%f', PLUunits= '%s' WHERE tblPLU2.PLUNumber = '%d'",plu.PLUNumber,plu.PLUTargLo,plu.PLUTargHi,plu.PLUTolLo,plu.PLUTolHi,plu.PLUTarg,plu.PLUTare,plu.PLUSubTransCount,plu.PLUSubGrossAccum,plu.PLUSubNetAccum,plu.PLUSubUnderAccum,plu.PLUSubTargAccum,plu.PLUSubOverAccum,plu.PLUSubUnderCount,plu.PLUSubTargCount,plu.PLUSubOverCount,plu.PLUTransCount,plu.PLUGrossAccum,plu.PLUNetAccum,plu.PLUUnderAccum,plu.PLUTargAccum,plu.PLUOverAccum,plu.PLUUnderCount,plu.PLUTargCount,plu.PLUOverCount,plu.PLUPcwt,plu.PLUunits,searchIndex)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()newPLUChannel=plu.PLUNumber
plu.lastPLUChannStore()end
function plu.pluDBStoreSingle(newPLUNumber)local dbFile,result,searchIndex,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)dbFile:execute("BEGIN TRANSACTION")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU2 (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTare DOUBLE, PLUSubTransCount INTEGER, PLUSubGrossAccum DOUBLE, PLUSubNetAccum DOUBLE, PLUSubUnderAccum DOUBLE, PLUSubTargAccum DOUBLE, PLUSubOverAccum DOUBLE, PLUSubUnderCount INTEGER, PLUSubTargCount INTEGER, PLUSubOverCount INTEGER, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")searchIndex=newPLUNumber
sqlStr=string.format("SELECT tblPLU2.PLUNumber FROM tblPLU2 WHERE tblPLU2.PLUNumber = %d",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblPLU2 (PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTare, PLUSubTransCount, PLUSubGrossAccum, PLUSubNetAccum, PLUSubUnderAccum, PLUSubTargAccum, PLUSubOverAccum, PLUSubUnderCount, PLUSubTargCount, PLUSubOverCount, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits) VALUES ( '%d', '%f', '%f', '%f', '%f', '%f', '%f', '%d', '%f', '%f', '%f', '%f', '%f', '%d', '%d', '%d', '%d', '%f', '%f', '%f', '%f', '%f', '%d', '%d', '%d', '%f', '%s')",plu.PLUNumber,plu.PLUTargLo,plu.PLUTargHi,plu.PLUTolLo,plu.PLUTolHi,plu.PLUTarg,plu.PLUTare,plu.PLUSubTransCount,plu.PLUSubGrossAccum,plu.PLUSubNetAccum,plu.PLUSubUnderAccum,plu.PLUSubTargAccum,plu.PLUSubOverAccum,plu.PLUSubUnderCount,plu.PLUSubTargCount,plu.PLUSubOverCount,plu.PLUTransCount,plu.PLUGrossAccum,plu.PLUNetAccum,plu.PLUUnderAccum,plu.PLUTargAccum,plu.PLUOverAccum,plu.PLUUnderCount,plu.PLUTargCount,plu.PLUOverCount,plu.PLUPcwt,plu.PLUunits)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblPLU2 SET PLUNumber= '%d', PLUTargLo= '%f', PLUTargHi= '%f', PLUTolLo= '%f', PLUTolHi= '%f', PLUTarg= '%f', PLUTare= '%f', PLUSubTransCount= '%d', PLUSubGrossAccum= '%f', PLUSubNetAccum= '%f', PLUSubUnderAccum= '%f', PLUSubTargAccum= '%f', PLUSubOverAccum= '%f', PLUSubUnderCount= '%d', PLUSubTargCount= '%d', PLUSubOverCount= '%d', PLUTransCount= '%d', PLUGrossAccum= '%f', PLUNetAccum= '%f', PLUUnderAccum= '%f', PLUTargAccum= '%f', PLUOverAccum= '%f', PLUUnderCount= '%d', PLUTargCount= '%d', PLUOverCount= '%d', PLUPcwt= '%f', PLUunits= '%s' WHERE tblPLU2.PLUNumber = '%d'",plu.PLUNumber,plu.PLUTargLo,plu.PLUTargHi,plu.PLUTolLo,plu.PLUTolHi,plu.PLUTarg,plu.PLUTare,plu.PLUSubTransCount,plu.PLUSubGrossAccum,plu.PLUSubNetAccum,plu.PLUSubUnderAccum,plu.PLUSubTargAccum,plu.PLUSubOverAccum,plu.PLUSubUnderCount,plu.PLUSubTargCount,plu.PLUSubOverCount,plu.PLUTransCount,plu.PLUGrossAccum,plu.PLUNetAccum,plu.PLUUnderAccum,plu.PLUTargAccum,plu.PLUOverAccum,plu.PLUUnderCount,plu.PLUTargCount,plu.PLUOverCount,plu.PLUPcwt,plu.PLUunits,searchIndex)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()newPLUChannel=plu.PLUNumber
plu.lastPLUChannStore()end
function plu.lastPLUChannStore()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLULastChan (Channel INTEGER)")dbFile:execute("BEGIN TRANSACTION")sqlStr=string.format("SELECT tblPLULastChan.Channel FROM tblPLULastChan")found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblPLULastChan (Channel) VALUES ('%d')",newPLUChannel)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblPLULastChan SET Channel = '%d'",newPLUChannel)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()end
function plu.extraStuffStore()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLUConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")dbFile:execute("COMMIT")dbFile:close()end
function plu.PLUDBRecall()local dbFile,result,tmpPLUNumber,searchIndex,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU2 (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTare DOUBLE, PLUSubTransCount INTEGER, PLUSubGrossAccum DOUBLE, PLUSubNetAccum DOUBLE, PLUSubUnderAccum DOUBLE, PLUSubTargAccum DOUBLE, PLUSubOverAccum DOUBLE, PLUSubUnderCount INTEGER, PLUSubTargCount INTEGER, PLUSubOverCount INTEGER, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")searchIndex=plu.PLUNumber
sqlStr=string.format("SELECT PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTare, PLUSubTransCount, PLUSubGrossAccum, PLUSubNetAccum, PLUSubUnderAccum, PLUSubTargAccum, PLUSubOverAccum, PLUSubUnderCount, PLUSubTargCount, PLUSubOverCount, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits FROM tblPLU2 WHERE tblPLU2.PLUNumber = '%d'",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
plu.PLUNumber=tonumber(row[1])plu.PLUTargLo=tonumber(row[2])plu.PLUTargHi=tonumber(row[3])plu.PLUTolLo=tonumber(row[4])plu.PLUTolHi=tonumber(row[5])plu.PLUTarg=tonumber(row[6])plu.PLUTare=tonumber(row[7])plu.PLUSubTransCount=tonumber(row[8])plu.PLUSubGrossAccum=tonumber(row[9])plu.PLUSubNetAccum=tonumber(row[10])plu.PLUSubUnderAccum=tonumber(row[11])plu.PLUSubTargAccum=tonumber(row[12])plu.PLUSubOverAccum=tonumber(row[13])plu.PLUSubUnderCount=tonumber(row[14])plu.PLUSubTargCount=tonumber(row[15])plu.PLUSubOverCount=tonumber(row[16])plu.PLUTransCount=tonumber(row[17])plu.PLUGrossAccum=tonumber(row[18])plu.PLUNetAccum=tonumber(row[19])plu.PLUUnderAccum=tonumber(row[20])plu.PLUTargAccum=tonumber(row[21])plu.PLUOverAccum=tonumber(row[22])plu.PLUUnderCount=tonumber(row[23])plu.PLUTargCount=tonumber(row[24])plu.PLUOverCount=tonumber(row[25])plu.PLUPcwt=tonumber(row[26])plu.PLUunits=string.format("%s",row[27])end
if found==false then
tmpPLUNumber=plu.PLUNumber
plu.PLUInit()plu.PLUNumber=tmpPLUNumber
plu.PLUunits=plu.PLUunitsDefault
end
dbFile:close()if checkweigh.PackRunCnt==0 then
awtx.weight.setAccum(plu.PLUSubGrossAccum,plu.PLUSubNetAccum,0,plu.PLUSubTransCount)end
if found==true then
return true
else
return false
end
end
function plu.lastPLUChannRecall()local dbFile,result,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLULastChan (Channel INTEGER)")sqlStr=string.format("SELECT Channel FROM tblPLULastChan")for row in dbFile:rows(sqlStr)do
newPLUChannel=row[1]end
dbFile:close()plu.PLUNumber=newPLUChannel
end
function plu.extraStuffRecall()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLUConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:close()end
function plu.PLUDBClearPLU0()plu.PLUClear()plu.pluDBStore()end
function plu.newClrAccum()plu.PLUSubTransCount=plu.PLUSubTransCountDefault
plu.PLUSubGrossAccum=plu.PLUSubGrossAccumDefault
plu.PLUSubNetAccum=plu.PLUSubNetAccumDefault
plu.PLUSubUnderAccum=plu.PLUSubUnderAccumDefault
plu.PLUSubTargAccum=plu.PLUSubTargAccumDefault
plu.PLUSubOverAccum=plu.PLUSubOverAccumDefault
plu.PLUSubUnderCount=plu.PLUSubUnderCountDefault
plu.PLUSubTargCount=plu.PLUSubTargCountDefault
plu.PLUSubOverCount=plu.PLUSubOverCountDefault
plu.PLUTransCount=plu.PLUTransCountDefault
plu.PLUGrossAccum=plu.PLUGrossAccumDefault
plu.PLUNetAccum=plu.PLUNetAccumDefault
plu.PLUUnderAccum=plu.PLUUnderAccumDefault
plu.PLUTargAccum=plu.PLUTargAccumDefault
plu.PLUOverAccum=plu.PLUOverAccumDefault
plu.PLUUnderCount=plu.PLUUnderCountDefault
plu.PLUTargCount=plu.PLUTargCountDefault
plu.PLUOverCount=plu.PLUOverCountDefault
plu.pluDBStoreSingle(plu.PLUNumber)end
function plu.PLUDBClear()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU2 (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTare DOUBLE, PLUSubTransCount INTEGER, PLUSubGrossAccum DOUBLE, PLUSubNetAccum DOUBLE, PLUSubUnderAccum DOUBLE, PLUSubTargAccum DOUBLE, PLUSubOverAccum DOUBLE, PLUSubUnderCount INTEGER, PLUSubTargCount INTEGER, PLUSubOverCount INTEGER, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")dbFile:execute("BEGIN TRANSACTION")for row in dbFile:rows("SELECT PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTare, PLUPcwt, PLUunits FROM tblPLU2")do
plu.PLUNumber=tonumber(row[1])plu.PLUTargLo=tonumber(row[2])plu.PLUTargHi=tonumber(row[3])plu.PLUTolLo=tonumber(row[4])plu.PLUTolHi=tonumber(row[5])plu.PLUTarg=tonumber(row[6])plu.PLUTare=tonumber(row[7])plu.PLUPcwt=tonumber(row[8])plu.PLUunits=string.format("%s",row[9])sqlStr=string.format("UPDATE tblPLU2 SET PLUNumber= '%d', PLUTargLo= '%f', PLUTargHi= '%f', PLUTolLo= '%f', PLUTolHi= '%f', PLUTarg= '%f', PLUTare= '%f', PLUPcwt= '%f', PLUunits= '%s' WHERE tblPLU2.PLUNumber = '%d'",plu.PLUNumber,plu.PLUTargLo,plu.PLUTargHi,plu.PLUTolLo,plu.PLUTolHi,plu.PLUTarg,plu.PLUTare,plu.PLUPcwt,plu.PLUunits,plu.PLUNumber)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()end
function plu.editPLU(label)local newPLUNumber,isEnterKey
supervisor.menuing=false
config=awtx.weight.getConfig(1)wt=awtx.weight.getCurrent(1)newPLUNumber=0
newPLUNumber,isEnterKey=awtx.keypad.enterInteger(newPLUNumber,MIN_PLUNUMBER,MAX_PLUNUMBER)awtx.display.writeLine(label)if isEnterKey then
plu.PLUNumber=newPLUNumber
result=plu.PLUDBRecall()if result==false then
plu.pluDBStore()end
supervisor.menuLevel=PLUSetupEdit
else
supervisor.menuLevel=PLUSetupMenu
end
supervisor.menuing=true
end
function plu.editPLUTARGLO(label)local newPLUTargLo,PLUTargLoMin,PLUTargLoMax,PLUWtUnit,errorState,tmpunitsStr,isEnterKey
if checkweigh.CheckWeighMode==PER375 then
PLUTargLoMin=0.0
PLUTargLoMax=100.0
tmpunitsStr="pc"newPLUTargLo=plu.PLUTargLo
newPLUTargLo,isEnterKey=awtx.keypad.enterWeightWithUnits(newPLUTargLo,PLUTargLoMin,PLUTargLoMax,tmpunitsStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
plu.PLUTargLo=newPLUTargLo
plu.PLUTolLo=plu.PLUTarg-plu.PLUTargLo
plu.pluDBStore()else
end
else
wt=awtx.weight.getCurrent(1)PLUTargLoMin=wt.curCapacity*-1
PLUTargLoMax=wt.curCapacity
errorState,PLUWtUnit=awtx.weight.unitStrToUnitIndex(plu.PLUunits)if errorState==0 then
newPLUTargLo=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTargLo,wt.units,1)else
newPLUTargLo=plu.PLUTargLo
end
newPLUTargLo,isEnterKey=awtx.keypad.enterWeightWithUnits(newPLUTargLo,PLUTargLoMin,PLUTargLoMax,wt.unitsStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
if errorState==0 then
plu.PLUTargLo=awtx.weight.convertWeight(wt.units,newPLUTargLo,PLUWtUnit,1)else
plu.PLUTargLo=newPLUTargLo
end
plu.PLUTolLo=plu.PLUTarg-plu.PLUTargLo
plu.pluDBStore()else
end
end
end
function plu.editPLUTARGHI(label)local newPLUTargHi,PLUTargHiMin,PLUTargHiMax,PLUWtUnit,errorState,tmpunitsStr,isEnterKey
if checkweigh.CheckWeighMode==PER375 then
PLUTargHiMin=0.0
PLUTargHiMax=100.0
tmpunitsStr="pc"newPLUTargHi=plu.PLUTargHi
newPLUTargHi,isEnterKey=awtx.keypad.enterWeightWithUnits(newPLUTargHi,PLUTargHiMin,PLUTargHiMax,tmpunitsStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
plu.PLUTargHi=newPLUTargHi
plu.PLUTolHi=plu.PLUTargHi-plu.PLUTarg
plu.pluDBStore()else
end
else
wt=awtx.weight.getCurrent(1)PLUTargHiMin=wt.curCapacity*-1
PLUTargHiMax=wt.curCapacity
errorState,PLUWtUnit=awtx.weight.unitStrToUnitIndex(plu.PLUunits)if errorState==0 then
newPLUTargHi=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTargHi,wt.units,1)else
newPLUTargHi=plu.PLUTargHi
end
newPLUTargHi,isEnterKey=awtx.keypad.enterWeightWithUnits(newPLUTargHi,PLUTargHiMin,PLUTargHiMax,wt.unitsStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
if errorState==0 then
plu.PLUTargHi=awtx.weight.convertWeight(wt.units,newPLUTargHi,PLUWtUnit,1)else
plu.PLUTargHi=newPLUTargHi
end
plu.PLUTolHi=plu.PLUTargHi-plu.PLUTarg
plu.pluDBStore()else
end
end
end
function plu.editPLUTOLLO(label)local newPLUTolLo,PLUTolLoMin,PLUTolLoMax,PLUWtUnit,errorState,tmpunitsStr,isEnterKey
if checkweigh.CheckWeighMode==PER375 then
PLUTolLoMin=0.0
PLUTolLoMax=100.0
tmpunitsStr="pc"newPLUTolLo=plu.PLUTolLo
newPLUTolLo,isEnterKey=awtx.keypad.enterWeightWithUnits(newPLUTolLo,PLUTolLoMin,PLUTolLoMax,tmpunitsStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
plu.PLUTolLo=newPLUTolLo
plu.PLUTargLo=plu.PLUTarg-plu.PLUTolLo
plu.pluDBStore()else
end
else
wt=awtx.weight.getCurrent(1)PLUTolLoMin=wt.curDivision
PLUTolLoMax=wt.curCapacity
errorState,PLUWtUnit=awtx.weight.unitStrToUnitIndex(plu.PLUunits)if errorState==0 then
newPLUTolLo=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTolLo,wt.units,1)else
newPLUTolLo=plu.PLUTolLo
end
newPLUTolLo,isEnterKey=awtx.keypad.enterWeightWithUnits(newPLUTolLo,PLUTolLoMin,PLUTolLoMax,wt.unitsStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
if errorState==0 then
plu.PLUTolLo=awtx.weight.convertWeight(wt.units,newPLUTolLo,PLUWtUnit,1)else
plu.PLUTolLo=newPLUTolLo
end
plu.PLUTargLo=plu.PLUTarg-plu.PLUTolLo
plu.pluDBStore()else
end
end
end
function plu.editPLUTOLHI(label)local newPLUTolHi,PLUTolHiMin,PLUTolHiMax,PLUWtUnit,errorState,tmpunitsStr,isEnterKey
if checkweigh.CheckWeighMode==PER375 then
PLUTolHiMin=0.0
PLUTolHiMax=100.0
tmpunitsStr="pc"newPLUTolHi=plu.PLUTolHi
newPLUTolHi,isEnterKey=awtx.keypad.enterWeightWithUnits(newPLUTolHi,PLUTolHiMin,PLUTolHiMax,tmpunitsStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
plu.PLUTolHi=newPLUTolHi
plu.PLUTargHi=plu.PLUTarg+plu.PLUTolHi
plu.pluDBStore()else
end
else
wt=awtx.weight.getCurrent(1)PLUTolHiMin=wt.curDivision
PLUTolHiMax=wt.curCapacity
errorState,PLUWtUnit=awtx.weight.unitStrToUnitIndex(plu.PLUunits)if errorState==0 then
newPLUTolHi=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTolHi,wt.units,1)else
newPLUTolHi=plu.PLUTolHi
end
newPLUTolHi,isEnterKey=awtx.keypad.enterWeightWithUnits(newPLUTolHi,PLUTolHiMin,PLUTolHiMax,wt.unitsStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
if errorState==0 then
plu.PLUTolHi=awtx.weight.convertWeight(wt.units,newPLUTolHi,PLUWtUnit,1)else
plu.PLUTolHi=newPLUTolHi
end
plu.PLUTargHi=plu.PLUTarg+plu.PLUTolHi
plu.pluDBStore()else
end
end
end
function plu.editPLUTARG(label)local newPLUTarg,PLUTargMin,PLUTargMax,PLUWtUnit,errorState,tmpunitsStr,isEnterKey
if checkweigh.CheckWeighMode==PER375 then
PLUTargMin=0.0
PLUTargMax=100.0
tmpunitsStr="pc"newPLUTarg=plu.PLUTarg
newPLUTarg,isEnterKey=awtx.keypad.enterWeightWithUnits(newPLUTarg,PLUTargMin,PLUTargMax,tmpunitsStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
plu.PLUTarg=newPLUTarg
plu.pluDBStore()else
end
else
wt=awtx.weight.getCurrent(1)PLUTargMin=wt.curCapacity*-1
PLUTargMax=wt.curCapacity
errorState,PLUWtUnit=awtx.weight.unitStrToUnitIndex(plu.PLUunits)if errorState==0 then
newPLUTarg=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTarg,wt.units,1)else
newPLUTarg=plu.PLUTarg
end
newPLUTarg,isEnterKey=awtx.keypad.enterWeightWithUnits(newPLUTarg,PLUTargMin,PLUTargMax,wt.unitsStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
if errorState==0 then
plu.PLUTarg=awtx.weight.convertWeight(wt.units,newPLUTarg,PLUWtUnit,1)else
plu.PLUTarg=newPLUTarg
end
plu.pluDBStore()else
end
end
end
MAX_LOOP=100
function plu.waitForMotion()local loop=0
local tmpMotion=false
wt=awtx.weight.getCurrent(1)tmpMotion=wt.motion
loop=0
while wt.motion do
wt=awtx.weight.getCurrent(1)tmpMotion=wt.motion
awtx.os.sleep(50)loop=loop+1
if loop>MAX_LOOP then
tmpMotion=true
break
end
end
return tmpMotion,wt.gross
end
function plu.editPLUTARE(label)local newPLUTare,PLUTareMin,PLUTareMax,PLUWtUnit,errorState,tmpunitsStr,isEnterKey
local tmpMotion,tmpWeight
wt=awtx.weight.getCurrent(1)PLUTareMin=0
PLUTareMax=wt.curCapacity
errorState,PLUWtUnit=awtx.weight.unitStrToUnitIndex(plu.PLUunits)if errorState==0 then
if wt.inGrossBand then
newPLUTare=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTare,wt.units,1)else
tmpMotion,tmpWeight=plu.waitForMotion()if not tmpMotion then
newPLUTare=tmpWeight
else
newPLUTare=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTare,wt.units,1)end
end
else
newPLUTare=plu.PLUTare
end
newPLUTare,isEnterKey=awtx.keypad.enterWeightWithUnits(newPLUTare,PLUTareMin,PLUTareMax,wt.unitsStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
if errorState==0 then
plu.PLUTare=awtx.weight.convertWeight(wt.units,newPLUTare,PLUWtUnit,1)else
plu.PLUTare=newPLUTare
end
plu.pluDBStore()else
end
end
function plu.PLUChannelSelect()local usermode,currentRPN,newPLUNumber,isEnterKey
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.writeLine("PLU")awtx.os.sleep(showtime1)newPLUNumber=plu.PLUNumber
newPLUNumber,isEnterKey=awtx.keypad.enterInteger(newPLUNumber,MIN_PLUNUMBER,MAX_PLUNUMBER,entertime)awtx.display.writeLine("PLU")awtx.display.setMode(usermode)if isEnterKey then
plu.PLUNumber=newPLUNumber
result=plu.PLUDBRecall()if result==false then
displayCANT()plu.PLUInit()newPLUNumber=0
plu.PLUNumber=newPLUNumber
end
newPLUChannel=newPLUNumber
plu.lastPLUChannStore()tmpPackRunCnt=0
checkweigh.usePLUValues()checkweigh.calcCheckweighByType()plu.setPLUPrintTokens()else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function plu.setPLUChannel(newPLUNumber)plu.PLUNumber=newPLUNumber
result=plu.PLUDBRecall()if result==false then
plu.PLUInit()newPLUNumber=0
plu.PLUNumber=newPLUNumber
end
newPLUChannel=newPLUNumber
plu.lastPLUChannStore()tmpPackRunCnt=0
checkweigh.usePLUValues()checkweigh.calcCheckweighByType()plu.setPLUPrintTokens()end
function plu.PLUDBReport(label)local usermode,currentRPN,fho,err
local index,isEnterKey
local dbFile,result
local row
local t={}currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)index,isEnterKey=awtx.keypad.selectList(" Port 1, Port 2, USB",0)awtx.display.writeLine(label)if isEnterKey then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(DB_FileLocation_AppData)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU2 (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTare DOUBLE, PLUSubTransCount INTEGER, PLUSubGrossAccum DOUBLE, PLUSubNetAccum DOUBLE, PLUSubUnderAccum DOUBLE, PLUSubTargAccum DOUBLE, PLUSubOverAccum DOUBLE, PLUSubUnderCount INTEGER, PLUSubTargCount INTEGER, PLUSubOverCount INTEGER, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")if index==0 or index==1 then
t[#t+1]=string.format("PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTare, PLUSubTransCount, PLUSubGrossAccum, PLUSubNetAccum, PLUSubUnderAccum, PLUSubTargAccum, PLUSubOverAccum, PLUSubUnderCount, PLUSubTargCount, PLUSubOverCount, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits \r\n")elseif index==2 then
t[#t+1]=string.format("PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTare, PLUSubTransCount, PLUSubGrossAccum, PLUSubNetAccum, PLUSubUnderAccum, PLUSubTargAccum, PLUSubOverAccum, PLUSubUnderCount, PLUSubTargCount, PLUSubOverCount, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits \r\n")end
for row in dbFile:rows("SELECT PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTare, PLUSubTransCount, PLUSubGrossAccum, PLUSubNetAccum, PLUSubUnderAccum, PLUSubTargAccum, PLUSubOverAccum, PLUSubUnderCount, PLUSubTargCount, PLUSubOverCount, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits FROM tblPLU2")do
if index==0 or index==1 then
t[#t+1]=string.format("%d, %f, %f, %f, %f, %f, %f, %d, %f, %f, %f, %f, %f, %d, %d, %d, %d, %f, %f, %f, %f, %f, %d, %d, %d, %f, %s \r\n",row[1],row[2],row[3],row[4],row[5],row[6],row[7],row[8],row[9],row[10],row[11],row[12],row[13],row[14],row[15],row[16],row[17],row[18],row[19],row[20],row[21],row[22],row[23],row[24],row[25],row[26])elseif index==2 then
t[#t+1]=string.format("%d, %f, %f, %f, %f, %f, %f, %d, %f, %f, %f, %f, %f, %d, %d, %d, %d, %f, %f, %f, %f, %f, %d, %d, %d, %f, %s \r\n",row[1],row[2],row[3],row[4],row[5],row[6],row[7],row[8],row[9],row[10],row[11],row[12],row[13],row[14],row[15],row[16],row[17],row[18],row[19],row[20],row[21],row[22],row[23],row[24],row[25],row[26])else
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
fho,err=io.open(string.format("%s%s",DB_FileLocation_Reports,DB_ReportName_PLU),"w")if err then
awtx.display.writeLine("err "..err)awtx.os.sleep(canttime)awtx.os.sleep(canttime)else
fho:write(table.concat(t))fho:close()end
else
awtx.display.writeLine("err "..tostring(result))end
else
end
awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function plu.PLUDBResetPLU0(label)local usermode,isEnterKey
local index
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)plu.PLUDBClearPLU0()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
end
end
function plu.PLUDBReset(label)local usermode,isEnterKey
local index
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)plu.PLUDBClear()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
end
end
plu.PLUClear()plu.PLUDBInit()plu.lastPLUChannRecall()if newPLUChannel~=nil then
plu.PLUNumber=newPLUChannel
plu.PLUDBRecall()checkweigh.usePLUValues()checkweigh.calcCheckweighByType()plu.setPLUPrintTokens()else
newPLUChannel=MIN_PLUNUMBER
plu.lastPLUChannStore()plu.PLUNumber=newPLUChannel
plu.PLUClear()checkweigh.usePLUValues()checkweigh.calcCheckweighByType()plu.setPLUPrintTokens()end
function plu.doPLUAccum()local tmpGacc,tmpNacc,PLUWtUnit,errorState
wt=awtx.weight.getCurrent(1)if plu.PLUunits==nil then
plu.PLUunits=plu.PLUunitsDefault
end
errorState,PLUWtUnit=awtx.weight.unitStrToUnitIndex(plu.PLUunits)if errorState==0 then
plu.PLUSubTransCount=plu.PLUSubTransCount+1
plu.PLUSubGrossAccum=plu.PLUSubGrossAccum+awtx.weight.convertWeight(wt.units,wt.gross,PLUWtUnit,1)plu.PLUSubNetAccum=plu.PLUSubNetAccum+awtx.weight.convertWeight(wt.units,wt.net,PLUWtUnit,1)if wt.net<awtx.weight.convertWeight(PLUWtUnit,plu.PLUTargLo,wt.units,1)then
plu.PLUSubUnderAccum=plu.PLUSubUnderAccum+awtx.weight.convertWeight(wt.units,wt.net,PLUWtUnit,1)plu.PLUSubUnderCount=plu.PLUSubUnderCount+1
elseif wt.net>=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTargLo,wt.units,1)and wt.net<=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTargHi,wt.units,1)then
plu.PLUSubTargAccum=plu.PLUSubTargAccum+awtx.weight.convertWeight(wt.units,wt.net,PLUWtUnit,1)plu.PLUSubTargCount=plu.PLUSubTargCount+1
else
plu.PLUSubOverAccum=plu.PLUSubOverAccum+awtx.weight.convertWeight(wt.units,wt.net,PLUWtUnit,1)plu.PLUSubOverCount=plu.PLUSubOverCount+1
end
plu.PLUTransCount=plu.PLUTransCount+1
plu.PLUGrossAccum=plu.PLUGrossAccum+awtx.weight.convertWeight(wt.units,wt.gross,PLUWtUnit,1)plu.PLUNetAccum=plu.PLUNetAccum+awtx.weight.convertWeight(wt.units,wt.net,PLUWtUnit,1)if wt.net<awtx.weight.convertWeight(PLUWtUnit,plu.PLUTargLo,wt.units,1)then
plu.PLUUnderAccum=plu.PLUUnderAccum+awtx.weight.convertWeight(wt.units,wt.net,PLUWtUnit,1)plu.PLUUnderCount=plu.PLUUnderCount+1
elseif wt.net>=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTargLo,wt.units,1)and wt.net<=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTargHi,wt.units,1)then
plu.PLUTargAccum=plu.PLUTargAccum+awtx.weight.convertWeight(wt.units,wt.net,PLUWtUnit,1)plu.PLUTargCount=plu.PLUTargCount+1
else
plu.PLUOverAccum=plu.PLUOverAccum+awtx.weight.convertWeight(wt.units,wt.net,PLUWtUnit,1)plu.PLUOverCount=plu.PLUOverCount+1
end
if checkweigh.PackRunCnt==0 then
tmpGacc=awtx.weight.convertWeight(PLUWtUnit,plu.PLUSubGrossAccum,wt.units,1)tmpNacc=awtx.weight.convertWeight(PLUWtUnit,plu.PLUSubNetAccum,wt.units,1)awtx.weight.setAccum(tmpGacc,tmpNacc,0,plu.PLUSubTransCount)end
else
plu.PLUSubTransCount=plu.PLUSubTransCount+1
plu.PLUSubGrossAccum=plu.PLUSubGrossAccum+wt.gross
plu.PLUSubNetAccum=plu.PLUSubNetAccum+wt.net
if wt.net<plu.PLUTargLo then
plu.PLUSubUnderAccum=plu.PLUSubUnderAccum+wt.net
plu.PLUSubUnderCount=plu.PLUSubUnderCount+1
elseif wt.net>=plu.PLUTargLo and wt.net<=plu.PLUTargHi then
plu.PLUSubTargAccum=plu.PLUSubTargAccum+wt.net
plu.PLUSubTargCount=plu.PLUSubTargCount+1
else
plu.PLUSubOverAccum=plu.PLUSubOverAccum+wt.net
plu.PLUSubOverCount=plu.PLUSubOverCount+1
end
plu.PLUTransCount=plu.PLUTransCount+1
plu.PLUGrossAccum=plu.PLUGrossAccum+wt.gross
plu.PLUNetAccum=plu.PLUNetAccum+wt.net
if wt.net<plu.PLUTargLo then
plu.PLUUnderAccum=plu.PLUUnderAccum+wt.net
plu.PLUUnderCount=plu.PLUUnderCount+1
elseif wt.net>=plu.PLUTargLo and wt.net<=plu.PLUTargHi then
plu.PLUTargAccum=plu.PLUTargAccum+wt.net
plu.PLUTargCount=plu.PLUTargCount+1
else
plu.PLUOverAccum=plu.PLUOverAccum+wt.net
plu.PLUOverCount=plu.PLUOverCount+1
end
if checkweigh.PackRunCnt==0 then
awtx.weight.setAccum(plu.PLUSubGrossAccum,plu.PLUSubNetAccum,0,plu.PLUSubTransCount)end
end
plu.pluDBStore()plu.setPLUPrintTokens()end
function plu.importPLU(label)local errorCode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.writeLine("Busy")errorCode=awtx.os.convertCsvFileToTable_LUA(DB_FileLocation_AppData,"tblPLU2",[[G:\PLU_CSV\plu2.csv]])if errorCode==0 or errorCode==nil then
awtx.display.writeLine("Done")else
awtx.display.writeLine(string.format("Err %2d",errorCode))end
awtx.os.sleep(1000)awtx.keypad.set_RPN_mode(currentRPN)end
function plu.importPLULUA(label)local errorCode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.writeLine("Busy")errorCode=plu.importSQL(DB_FileLocation_AppData,"tblPLU2",[[G:\PLU_CSV\plu2.csv]])if errorCode==0 or errorCode==nil then
awtx.display.writeLine("Done")else
awtx.display.writeLine(string.format("Err %2d",errorCode))end
awtx.os.sleep(1000)awtx.keypad.set_RPN_mode(currentRPN)end
function plu.exportPLU(label)local errorCode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.writeLine("Busy")errorCode=awtx.os.convertTableToCsvFile_LUA(DB_FileLocation_AppData,"tblPLU2","plu2.csv",[[G:\PLU_CSV]])if errorCode==0 or errorCode==nil then
awtx.display.writeLine("Done")else
awtx.display.writeLine(string.format("Err %2d",errorCode))end
awtx.os.sleep(1000)awtx.keypad.set_RPN_mode(currentRPN)end
function plu.exportPLULUA(label)local errorCode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.writeLine("Busy")errorCode=plu.exportSQL(DB_FileLocation_AppData,"tblPLU2","plu2.csv",[[G:\PLU_CSV]])if errorCode==0 or errorCode==nil then
awtx.display.writeLine("Done")else
awtx.display.writeLine(string.format("Err %2d",errorCode))end
awtx.os.sleep(1000)awtx.keypad.set_RPN_mode(currentRPN)end
function plu.initAll()if checkweigh.CheckWeighMode==PER375 then
plu.PLUTolLoDefault=0.1
plu.PLUTolHiDefault=0.1
else
plu.PLUTolLoDefault=wt.curDivision
plu.PLUTolHiDefault=wt.curDivision
end
end
impPLUNumber=plu.PLUNumberDefault
impPLUTargLo=plu.PLUTargLoDefault
impPLUTargHi=plu.PLUTargHiDefault
impPLUTolLo=plu.PLUTolLoDefault
impPLUTolHi=plu.PLUTolHiDefault
impPLUTarg=plu.PLUTargDefault
impPLUTare=plu.PLUTareDefault
impPLUSubTransCount=plu.PLUSubTransCountDefault
impPLUSubGrossAccum=plu.PLUSubGrossAccumDefault
impPLUSubNetAccum=plu.PLUSubNetAccumDefault
impPLUSubUnderAccum=plu.PLUSubUnderAccumDefault
impPLUSubTargAccum=plu.PLUSubTargAccumDefault
impPLUSubOverAccum=plu.PLUSubOverAccumDefault
impPLUSubUnderCount=plu.PLUSubUnderCountDefault
impPLUSubTargCount=plu.PLUSubTargCountDefault
impPLUSubOverCount=plu.PLUSubOverCountDefault
impPLUTransCount=plu.PLUTransCountDefault
impPLUGrossAccum=plu.PLUGrossAccumDefault
impPLUNetAccum=plu.PLUNetAccumDefault
impPLUUnderAccum=plu.PLUUnderAccumDefault
impPLUTargAccum=plu.PLUTargAccumDefault
impPLUOverAccum=plu.PLUOverAccumDefault
impPLUUnderCount=plu.PLUUnderCountDefault
impPLUTargCount=plu.PLUTargCountDefault
impPLUOverCount=plu.PLUOverCountDefault
impPLUPcwt=plu.PLUPcwtDefault
impPLUunits=plu.PLUunitsDefault
function plu.saveImportPLU(newPLUNumber)local dbFile,result,searchIndex,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)dbFile:execute("BEGIN TRANSACTION")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU2 (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTare DOUBLE, PLUSubTransCount INTEGER, PLUSubGrossAccum DOUBLE, PLUSubNetAccum DOUBLE, PLUSubUnderAccum DOUBLE, PLUSubTargAccum DOUBLE, PLUSubOverAccum DOUBLE, PLUSubUnderCount INTEGER, PLUSubTargCount INTEGER, PLUSubOverCount INTEGER, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")searchIndex=newPLUNumber
sqlStr=string.format("SELECT tblPLU2.PLUNumber FROM tblPLU2 WHERE tblPLU2.PLUNumber = %d",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblPLU2 (PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTare, PLUSubTransCount, PLUSubGrossAccum, PLUSubNetAccum, PLUSubUnderAccum, PLUSubTargAccum, PLUSubOverAccum, PLUSubUnderCount, PLUSubTargCount, PLUSubOverCount, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits) VALUES ( '%d', '%f', '%f', '%f', '%f', '%f', '%f', '%d', '%f', '%f', '%f', '%f', '%f', '%d', '%d', '%d', '%d', '%f', '%f', '%f', '%f', '%f', '%d', '%d', '%d', '%f', '%s')",impPLUNumber,impPLUTargLo,impPLUTargHi,impPLUTolLo,impPLUTolHi,impPLUTarg,impPLUTare,impPLUSubTransCount,impPLUSubGrossAccum,impPLUSubNetAccum,impPLUSubUnderAccum,impPLUSubTargAccum,impPLUSubOverAccum,impPLUSubUnderCount,impPLUSubTargCount,impPLUSubOverCount,impPLUTransCount,impPLUGrossAccum,impPLUNetAccum,impPLUUnderAccum,impPLUTargAccum,impPLUOverAccum,impPLUUnderCount,impPLUTargCount,impPLUOverCount,impPLUPcwt,impPLUunits)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblPLU2 SET PLUNumber= '%d', PLUTargLo= '%f', PLUTargHi= '%f', PLUTolLo= '%f', PLUTolHi= '%f', PLUTarg= '%f', PLUTare= '%f', PLUSubTransCount= '%d', PLUSubGrossAccum= '%f', PLUSubNetAccum= '%f', PLUSubUnderAccum= '%f', PLUSubTargAccum= '%f', PLUSubOverAccum= '%f', PLUSubUnderCount= '%d', PLUSubTargCount= '%d', PLUSubOverCount= '%d', PLUTransCount= '%d', PLUGrossAccum= '%f', PLUNetAccum= '%f', PLUUnderAccum= '%f', PLUTargAccum= '%f', PLUOverAccum= '%f', PLUUnderCount= '%d', PLUTargCount= '%d', PLUOverCount= '%d', PLUPcwt= '%f', PLUunits= '%s' WHERE tblPLU2.PLUNumber = '%d'",impPLUNumber,impPLUTargLo,impPLUTargHi,impPLUTolLo,impPLUTolHi,impPLUTarg,impPLUTare,impPLUSubTransCount,impPLUSubGrossAccum,impPLUSubNetAccum,impPLUSubUnderAccum,impPLUSubTargAccum,impPLUSubOverAccum,impPLUSubUnderCount,impPLUSubTargCount,impPLUSubOverCount,impPLUTransCount,impPLUGrossAccum,impPLUNetAccum,impPLUUnderAccum,impPLUTargAccum,impPLUOverAccum,impPLUUnderCount,impPLUTargCount,impPLUOverCount,impPLUPcwt,impPLUunits,searchIndex)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()end
function plu.importSQL(tmpAppData,tmpTable,tmpDestFile)local errorCode
local file
local numRecords=0
local curRecord={}local index
local isEnterKey
file,errorCode=io.open(tmpDestFile,"r")if errorCode==nil then
local nextCommaIndex=-1
plu.PLUDBClear()for line in file:lines()do
curRecord={}if numRecords>=MAX_PLUNUMBER then
numRecords=numRecords+1
break
end
numRecords=numRecords+1
if separatorChar==0 then
elseif separatorChar==1 then
line=string.gsub(line,"%,","%.")line=string.gsub(line,"%;","%,")end
line=string.gsub(line,'\n',',')local startingIndex=0
nextCommaIndex=string.find(line,',',startingIndex)if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUNumber=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUTargLo=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUTargHi=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUTolLo=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUTolHi=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUTarg=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUTare=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUSubTransCount=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUSubGrossAccum=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUSubNetAccum=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUSubUnderAccum=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUSubTargAccum=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUSubOverAccum=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUSubUnderCount=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUSubTargCount=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUSubOverCount=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUTransCount=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUGrossAccum=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUNetAccum=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUUnderAccum=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUTargAccum=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUOverAccum=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUUnderCount=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUTargCount=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUOverCount=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUPcwt=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impPLUunits=tostring(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=0
end
if nextCommaIndex~=0 then
break
end
if numRecords==1 then
else
plu.saveImportPLU(impPLUNumber)end
end
file:close()errorCode=0
if nextCommaIndex~=0 then
awtx.display.writeLine(" SQL",2000)awtx.display.writeLine("ParseFail",2000)errorCode=-1
elseif numRecords>MAX_PLUNUMBER then
awtx.display.writeLine("SQL FUL",500)errorCode=-2
end
else
awtx.display.writeLine(" SQL",2000)awtx.display.writeLine("ReadFail",2000)errorCode=-3
end
return errorCode
end
expPLUNumber=plu.PLUNumberDefault
expPLUTargLo=plu.PLUTargLoDefault
expPLUTargHi=plu.PLUTargHiDefault
expPLUTolLo=plu.PLUTolLoDefault
expPLUTolHi=plu.PLUTolHiDefault
expPLUTarg=plu.PLUTargDefault
expPLUTare=plu.PLUTareDefault
expPLUSubTransCount=plu.PLUSubTransCountDefault
expPLUSubGrossAccum=plu.PLUSubGrossAccumDefault
expPLUSubNetAccum=plu.PLUSubNetAccumDefault
expPLUSubUnderAccum=plu.PLUSubUnderAccumDefault
expPLUSubTargAccum=plu.PLUSubTargAccumDefault
expPLUSubOverAccum=plu.PLUSubOverAccumDefault
expPLUSubUnderCount=plu.PLUSubUnderCountDefault
expPLUSubTargCount=plu.PLUSubTargCountDefault
expPLUSubOverCount=plu.PLUSubOverCountDefault
expPLUTransCount=plu.PLUTransCountDefault
expPLUGrossAccum=plu.PLUGrossAccumDefault
expPLUNetAccum=plu.PLUNetAccumDefault
expPLUUnderAccum=plu.PLUUnderAccumDefault
expPLUTargAccum=plu.PLUTargAccumDefault
expPLUOverAccum=plu.PLUOverAccumDefault
expPLUUnderCount=plu.PLUUnderCountDefault
expPLUTargCount=plu.PLUTargCountDefault
expPLUOverCount=plu.PLUOverCountDefault
expPLUPcwt=plu.PLUPcwtDefault
expPLUunits=plu.PLUunitsDefault
function plu.exportSQL(tmpAppData,tmpTable,tmpDestFile,tmpDestFolder)local errorCode,errorState1
local errorCode1,usbfile
local errorCode2,dbFile
local result=0
result=awtx.os.makeDirectory(tmpDestFolder)if result~=0 then
return result
end
usbfile,errorCode1=io.open(tmpDestFolder..[[\]]..tmpDestFile,"w")dbFile,errorCode2=sqlite3.open(tmpAppData)if errorCode1==nil and errorCode2==nil then
local outputString='"PLUNumber","PLUTargLo","PLUTargHi","PLUTolLo","PLUTolHi","PLUTarg","PLUTare","PLUSubTransCount","PLUSubGrossAccum","PLUSubNetAccum","PLUSubUnderAccum","PLUSubTargAccum","PLUSubOverAccum","PLUSubUnderCount","PLUSubTargCount","PLUSubOverCount","PLUTransCount","PLUGrossAccum","PLUNetAccum","PLUUnderAccum","PLUTargAccum","PLUOverAccum","PLUUnderCount","PLUTargCount","PLUOverCount","PLUPcwt","PLUunits"\n'if separatorChar==0 then
elseif separatorChar==1 then
outputString=string.gsub(outputString,"%,","%;")outputString=string.gsub(outputString,"%.","%,")end
usbfile:write(outputString)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU2 (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTare DOUBLE, PLUSubTransCount INTEGER, PLUSubGrossAccum DOUBLE, PLUSubNetAccum DOUBLE, PLUSubUnderAccum DOUBLE, PLUSubTargAccum DOUBLE, PLUSubOverAccum DOUBLE, PLUSubUnderCount INTEGER, PLUSubTargCount INTEGER, PLUSubOverCount INTEGER, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")sqlStr=string.format("SELECT PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTare, PLUSubTransCount, PLUSubGrossAccum, PLUSubNetAccum, PLUSubUnderAccum, PLUSubTargAccum, PLUSubOverAccum, PLUSubUnderCount, PLUSubTargCount, PLUSubOverCount, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits FROM tblPLU2")for row in dbFile:rows(sqlStr)do
expPLUNumber=row[1]expPLUTargLo=row[2]expPLUTargHi=row[3]expPLUTolLo=row[4]expPLUTolHi=row[5]expPLUTarg=row[6]expPLUTare=row[7]expPLUSubTransCount=row[8]expPLUSubGrossAccum=row[9]expPLUSubNetAccum=row[10]expPLUSubUnderAccum=row[11]expPLUSubTargAccum=row[12]expPLUSubOverAccum=row[13]expPLUSubUnderCount=row[14]expPLUSubTargCount=row[15]expPLUSubOverCount=row[16]expPLUTransCount=row[17]expPLUGrossAccum=row[18]expPLUNetAccum=row[19]expPLUUnderAccum=row[20]expPLUTargAccum=row[21]expPLUOverAccum=row[22]expPLUUnderCount=row[23]expPLUTargCount=row[24]expPLUOverCount=row[25]expPLUPcwt=row[26]expPLUunits=row[27]widthprec=wt.curDigitsTotal.."."..wt.curDigitsRight
formatStr=string.gsub("%s,%***f","***",widthprec)outputString=""outputString=string.format("%d",expPLUNumber)outputString=string.format(formatStr,outputString,expPLUTargLo)outputString=string.format(formatStr,outputString,expPLUTargHi)outputString=string.format(formatStr,outputString,expPLUTolLo)outputString=string.format(formatStr,outputString,expPLUTolHi)outputString=string.format(formatStr,outputString,expPLUTarg)outputString=string.format(formatStr,outputString,expPLUTare)outputString=string.format("%s,%d",outputString,expPLUSubTransCount)outputString=string.format(formatStr,outputString,expPLUSubGrossAccum)outputString=string.format(formatStr,outputString,expPLUSubNetAccum)outputString=string.format(formatStr,outputString,expPLUSubUnderAccum)outputString=string.format(formatStr,outputString,expPLUSubTargAccum)outputString=string.format(formatStr,outputString,expPLUSubOverAccum)outputString=string.format("%s,%d",outputString,expPLUSubUnderCount)outputString=string.format("%s,%d",outputString,expPLUSubTargCount)outputString=string.format("%s,%d",outputString,expPLUSubOverCount)outputString=string.format("%s,%d",outputString,expPLUTransCount)outputString=string.format(formatStr,outputString,expPLUGrossAccum)outputString=string.format(formatStr,outputString,expPLUNetAccum)outputString=string.format(formatStr,outputString,expPLUUnderAccum)outputString=string.format(formatStr,outputString,expPLUTargAccum)outputString=string.format(formatStr,outputString,expPLUOverAccum)outputString=string.format("%s,%d",outputString,expPLUUnderCount)outputString=string.format("%s,%d",outputString,expPLUTargCount)outputString=string.format("%s,%d",outputString,expPLUOverCount)outputString=string.format("%s,%f",outputString,expPLUPcwt)outputString=string.format("%s,%s\n",outputString,expPLUunits)if separatorChar==0 then
elseif separatorChar==1 then
outputString=string.gsub(outputString,"%,","%;")outputString=string.gsub(outputString,"%.","%,")end
usbfile:write(outputString)end
dbFile:close()usbfile:close()errorCode=0
else
if errorCode2==nil then
errorCode=errorCode1
elseif errorCode2==nil then
errorCode=errorCode2
else
errorCode=errorCode1+errorCode2
end
end
return errorCode
end