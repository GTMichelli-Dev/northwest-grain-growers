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
function plu.initPLUPrintTokens()awtx.fmtPrint.varSet(71,newPLUChannel,"PLU Channel",AWTX_LUA_INTEGER)printTokens[71].varName="newPLUChannel"printTokens[71].varLabel="PLU Channel"printTokens[71].varType=AWTX_LUA_INTEGER
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
function plu.setPLUPrintTokens()awtx.fmtPrint.varSet(71,newPLUChannel,"PLU Channel",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(72,plu.PLUNumber,"PLU Number",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(73,plu.PLUTargLo,"Target Lo",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(74,plu.PLUTargHi,"Target Hi",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(75,plu.PLUTolLo,"Tolerance Lo",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(76,plu.PLUTolHi,"Tolerance Hi",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(77,plu.PLUTarg,"Target",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(78,plu.PLUTransCount,"Transaction Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(79,plu.PLUGrossAccum,"Gross Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(80,plu.PLUNetAccum,"Net Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(81,plu.PLUUnderAccum,"Under Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(82,plu.PLUTargAccum,"Target Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(83,plu.PLUOverAccum,"Over Accum",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(84,plu.PLUUnderCount,"Under Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(85,plu.PLUTargCount,"Target Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(86,plu.PLUOverCount,"Over Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(87,plu.PLUPcwt,"Pieceweight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(88,plu.PLUunits,"units",AWTX_LUA_STRING)end
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
if awtx.os.amISimulator()then
simAppPath=awtx.os.applicationPath()DB_FileLocation_AppConfig=simAppPath..[[\AppConfig.db]]DB_FileLocation_AppData=simAppPath..[[\AppData.db]]DB_FileLocation_Reports=simAppPath..[[\Reports]]..SerialNumber
else
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]DB_FileLocation_AppData=[[C:\Database\AppData.db]]DB_FileLocation_Reports=[[G:\Reports]]..SerialNumber
end
DB_ReportName_PLU=[[\PLUReport.csv]]dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLULastChan (Channel INTEGER)")dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLUConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:close()end
function plu.pluDBStore()local dbFile,result,searchIndex,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)dbFile:execute("BEGIN TRANSACTION")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")searchIndex=plu.PLUNumber
sqlStr=string.format("SELECT tblPLU.PLUNumber FROM tblPLU WHERE tblPLU.PLUNumber = %d",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblPLU (PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits) VALUES ( '%d', '%f', '%f', '%f', '%f', '%f', '%d', '%f', '%f', '%f', '%f', '%f', '%d', '%d', '%d', '%f', '%s')",plu.PLUNumber,plu.PLUTargLo,plu.PLUTargHi,plu.PLUTolLo,plu.PLUTolHi,plu.PLUTarg,plu.PLUTransCount,plu.PLUGrossAccum,plu.PLUNetAccum,plu.PLUUnderAccum,plu.PLUTargAccum,plu.PLUOverAccum,plu.PLUUnderCount,plu.PLUTargCount,plu.PLUOverCount,plu.PLUPcwt,plu.PLUunits)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblPLU SET PLUNumber= '%d', PLUTargLo= '%f', PLUTargHi= '%f', PLUTolLo= '%f', PLUTolHi= '%f', PLUTarg= '%f', PLUTransCount= '%d', PLUGrossAccum= '%f', PLUNetAccum= '%f', PLUUnderAccum= '%f', PLUTargAccum= '%f', PLUOverAccum= '%f', PLUUnderCount= '%d', PLUTargCount= '%d', PLUOverCount= '%d', PLUPcwt= '%f', PLUunits= '%s' WHERE tblPLU.PLUNumber = '%d'",plu.PLUNumber,plu.PLUTargLo,plu.PLUTargHi,plu.PLUTolLo,plu.PLUTolHi,plu.PLUTarg,plu.PLUTransCount,plu.PLUGrossAccum,plu.PLUNetAccum,plu.PLUUnderAccum,plu.PLUTargAccum,plu.PLUOverAccum,plu.PLUUnderCount,plu.PLUTargCount,plu.PLUOverCount,plu.PLUPcwt,plu.PLUunits,searchIndex)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()newPLUChannel=plu.PLUNumber
plu.lastPLUChannStore()end
function plu.pluDBStoreSingle(newPLUNumber)local dbFile,result,searchIndex,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)dbFile:execute("BEGIN TRANSACTION")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")searchIndex=newPLUNumber
sqlStr=string.format("SELECT tblPLU.PLUNumber FROM tblPLU WHERE tblPLU.PLUNumber = %d",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblPLU (PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits) VALUES ( '%d', '%f', '%f', '%f', '%f', '%f', '%d', '%f', '%f', '%f', '%f', '%f', '%d', '%d', '%d', '%f', '%s')",plu.PLUNumber,plu.PLUTargLo,plu.PLUTargHi,plu.PLUTolLo,plu.PLUTolHi,plu.PLUTarg,plu.PLUTransCount,plu.PLUGrossAccum,plu.PLUNetAccum,plu.PLUUnderAccum,plu.PLUTargAccum,plu.PLUOverAccum,plu.PLUUnderCount,plu.PLUTargCount,plu.PLUOverCount,plu.PLUPcwt,plu.PLUunits)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblPLU SET PLUNumber= '%d', PLUTargLo= '%f', PLUTargHi= '%f', PLUTolLo= '%f', PLUTolHi= '%f', PLUTarg= '%f', PLUTransCount= '%d', PLUGrossAccum= '%f', PLUNetAccum= '%f', PLUUnderAccum= '%f', PLUTargAccum= '%f', PLUOverAccum= '%f', PLUUnderCount= '%d', PLUTargCount= '%d', PLUOverCount= '%d', PLUPcwt= '%f', PLUunits= '%s' WHERE tblPLU.PLUNumber = '%d'",plu.PLUNumber,plu.PLUTargLo,plu.PLUTargHi,plu.PLUTolLo,plu.PLUTolHi,plu.PLUTarg,plu.PLUTransCount,plu.PLUGrossAccum,plu.PLUNetAccum,plu.PLUUnderAccum,plu.PLUTargAccum,plu.PLUOverAccum,plu.PLUUnderCount,plu.PLUTargCount,plu.PLUOverCount,plu.PLUPcwt,plu.PLUunits,searchIndex)result=dbFile:exec(sqlStr)end
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
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")searchIndex=plu.PLUNumber
sqlStr=string.format("SELECT PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits FROM tblPLU WHERE tblPLU.PLUNumber = '%d'",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
plu.PLUNumber=tonumber(row[1])plu.PLUTargLo=tonumber(row[2])plu.PLUTargHi=tonumber(row[3])plu.PLUTolLo=tonumber(row[4])plu.PLUTolHi=tonumber(row[5])plu.PLUTarg=tonumber(row[6])plu.PLUTransCount=tonumber(row[7])plu.PLUGrossAccum=tonumber(row[8])plu.PLUNetAccum=tonumber(row[9])plu.PLUUnderAccum=tonumber(row[10])plu.PLUTargAccum=tonumber(row[11])plu.PLUOverAccum=tonumber(row[12])plu.PLUUnderCount=tonumber(row[13])plu.PLUTargCount=tonumber(row[14])plu.PLUOverCount=tonumber(row[15])plu.PLUPcwt=tonumber(row[16])plu.PLUunits=string.format("%s",row[17])end
if found==false then
tmpPLUNumber=plu.PLUNumber
plu.PLUInit()plu.PLUNumber=tmpPLUNumber
plu.PLUunits=plu.PLUunitsDefault
end
dbFile:close()if checkweigh.PackRunCnt==0 then
awtx.weight.setAccum(plu.PLUGrossAccum,plu.PLUNetAccum,0,plu.PLUTransCount)end
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
function plu.newClrAccum()plu.PLUTransCount=plu.PLUTransCountDefault
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
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")dbFile:execute("BEGIN TRANSACTION")for row in dbFile:rows("SELECT PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits FROM tblPLU")do
plu.PLUNumber=tonumber(row[1])plu.PLUTargLo=tonumber(row[2])plu.PLUTargHi=tonumber(row[3])plu.PLUTolLo=tonumber(row[4])plu.PLUTolHi=tonumber(row[5])plu.PLUTarg=tonumber(row[6])plu.PLUPcwt=tonumber(row[16])plu.PLUunits=string.format("%s",row[17])sqlStr=string.format("UPDATE tblPLU SET PLUNumber= '%d', PLUTargLo= '%f', PLUTargHi= '%f', PLUTolLo= '%f', PLUTolHi= '%f', PLUTarg= '%f', PLUTransCount= '0', PLUGrossAccum= '0', PLUNetAccum= '0', PLUUnderAccum= '0', PLUTargAccum= '0', PLUOverAccum= '0', PLUUnderCount= '0', PLUTargCount= '0', PLUOverCount= '0', PLUPcwt= '%f', PLUunits= '%s' WHERE tblPLU.PLUNumber = '%d'",plu.PLUNumber,plu.PLUTargLo,plu.PLUTargHi,plu.PLUTolLo,plu.PLUTolHi,plu.PLUTarg,plu.PLUPcwt,plu.PLUunits,plu.PLUNumber)result=dbFile:exec(sqlStr)end
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
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(DB_FileLocation_AppData)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblPLU (PLUNumber INTEGER, PLUTargLo DOUBLE, PLUTargHi DOUBLE, PLUTolLo DOUBLE, PLUTolHi DOUBLE, PLUTarg DOUBLE, PLUTransCount INTEGER, PLUGrossAccum DOUBLE, PLUNetAccum DOUBLE, PLUUnderAccum DOUBLE, PLUTargAccum DOUBLE, PLUOverAccum DOUBLE, PLUUnderCount INTEGER, PLUTargCount INTEGER, PLUOverCount INTEGER, PLUPcwt DOUBLE, PLUunits TEXT)")if index==0 or index==1 then
t[#t+1]=string.format("PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits \r\n")elseif index==2 then
t[#t+1]=string.format("PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits \r\n")end
for row in dbFile:rows("SELECT PLUNumber, PLUTargLo, PLUTargHi, PLUTolLo, PLUTolHi, PLUTarg, PLUTransCount, PLUGrossAccum, PLUNetAccum, PLUUnderAccum, PLUTargAccum, PLUOverAccum, PLUUnderCount, PLUTargCount, PLUOverCount, PLUPcwt, PLUunits FROM tblPLU")do
if index==0 or index==1 then
t[#t+1]=string.format("%d, %f, %f, %f, %f, %f, %d, %f, %f, %f, %f, %f, %d, %d, %d, %f, %s \r\n",row[1],row[2],row[3],row[4],row[5],row[6],row[7],row[8],row[9],row[10],row[11],row[12],row[13],row[14],row[15],row[16],row[17])elseif index==2 then
t[#t+1]=string.format("%d, %f, %f, %f, %f, %f, %d, %f, %f, %f, %f, %f, %d, %d, %d, %f, %s \r\n",row[1],row[2],row[3],row[4],row[5],row[6],row[7],row[8],row[9],row[10],row[11],row[12],row[13],row[14],row[15],row[16],row[17])else
end
end
dbFile:close()if index==0 then
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
plu.PLUTransCount=plu.PLUTransCount+1
plu.PLUGrossAccum=plu.PLUGrossAccum+awtx.weight.convertWeight(wt.units,wt.gross,PLUWtUnit,1)plu.PLUNetAccum=plu.PLUNetAccum+awtx.weight.convertWeight(wt.units,wt.net,PLUWtUnit,1)if wt.net<awtx.weight.convertWeight(PLUWtUnit,plu.PLUTargLo,wt.units,1)then
plu.PLUUnderAccum=plu.PLUUnderAccum+awtx.weight.convertWeight(wt.units,wt.net,PLUWtUnit,1)plu.PLUUnderCount=plu.PLUUnderCount+1
elseif wt.net>=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTargLo,wt.units,1)and wt.net<=awtx.weight.convertWeight(PLUWtUnit,plu.PLUTargHi,wt.units,1)then
plu.PLUTargAccum=plu.PLUTargAccum+awtx.weight.convertWeight(wt.units,wt.net,PLUWtUnit,1)plu.PLUTargCount=plu.PLUTargCount+1
else
plu.PLUOverAccum=plu.PLUOverAccum+awtx.weight.convertWeight(wt.units,wt.net,PLUWtUnit,1)plu.PLUOverCount=plu.PLUOverCount+1
end
if checkweigh.PackRunCnt==0 then
tmpGacc=awtx.weight.convertWeight(PLUWtUnit,plu.PLUGrossAccum,wt.units,1)tmpNacc=awtx.weight.convertWeight(PLUWtUnit,plu.PLUNetAccum,wt.units,1)awtx.weight.setAccum(tmpGacc,tmpNacc,0,plu.PLUTransCount)end
else
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
awtx.weight.setAccum(plu.PLUGrossAccum,plu.PLUNetAccum,0,plu.PLUTransCount)end
end
plu.pluDBStore()plu.setPLUPrintTokens()end
function plu.importPLU(label)local errorCode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.writeLine("Busy")errorCode=awtx.os.convertCsvFileToTable_LUA(DB_FileLocation_AppData,"tblPLU",[[G:\PLU_CSV\plu.csv]])if errorCode==0 then
awtx.display.writeLine("Done")else
awtx.display.writeLine(string.format("Fail=%d",errorCode))end
awtx.os.sleep(1000)awtx.keypad.set_RPN_mode(currentRPN)end
function plu.exportPLU(label)local errorCode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.writeLine("Busy")errorCode=awtx.os.convertTableToCsvFile_LUA(DB_FileLocation_AppData,"tblPLU","plu.csv",[[G:\PLU_CSV]])if errorCode==0 then
awtx.display.writeLine("Done")else
awtx.display.writeLine(string.format("Fail=%d",errorCode))end
awtx.os.sleep(1000)awtx.keypad.set_RPN_mode(currentRPN)end
function plu.initAll()if checkweigh.CheckWeighMode==PER375 then
plu.PLUTolLoDefault=0.1
plu.PLUTolHiDefault=0.1
else
plu.PLUTolLoDefault=wt.curDivision
plu.PLUTolHiDefault=wt.curDivision
end
end