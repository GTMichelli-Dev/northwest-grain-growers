inmo={}inmo.EnabledDefault=1
inmo.StartIODefault=1
inmo.StopIODefault=2
inmo.DelayTimeDefault=0
inmo.WeightDefault=0
inmo.SclNumDefault=0
inmo.MultipyDefault=1.0
inmo.Multipy1Default=1000000
inmo.Multipy2Default=000000
inmo.Switch2GrossDefault=0
inmo.delayFlagDefault=false
inmo.timetickDefault=0
inmo.delaytickDefault=0
inmo.Enabled=inmo.EnabledDefault
inmo.StartIO=inmo.StartIODefault
inmo.StopIO=inmo.StopIODefault
inmo.DelayTime=inmo.DelayTimeDefault
inmo.Weight=inmo.WeightDefault
inmo.SclNum=inmo.SclNumDefault
inmo.Multipy=inmo.MultipyDefault
inmo.Multipy1=inmo.Multipy1Default
inmo.Multipy2=inmo.Multipy2Default
inmo.Switch2Gross=inmo.Switch2GrossDefault
inmo.delayFlag=inmo.delayFlagDefault
inmo.timetick=inmo.timetickDefault
inmo.delaytick=inmo.delaytickDefault
if system.modelStr=="ZM301"then
MAX_INMO_INDEX=10
elseif system.modelStr=="ZM303"then
MAX_INMO_INDEX=10
elseif system.modelStr=="ZQ375"then
MAX_INMO_INDEX=10
elseif system.modelStr=="ZM305GTN"then
MAX_INMO_INDEX=10
elseif system.modelStr=="ZM305"then
MAX_INMO_INDEX=10
else
MAX_INMO_INDEX=10
end
function inmo.initInMoPrintTokens()end
function inmo.setInMoPrintTokens()end
XR4500_RED="&"XR4500_GRN="*"XR4500_OFF="%"XR4500_FLASH_ON="("XR4500_FLASH_OFF=")"XR4500_FLASH_3="!"inmo.XR4500=XR4500_OFF
function inmo.initXR4500PrintToken()awtx.fmtPrint.varSet(65,inmo.XR4500,"XR4500 Light",AWTX_LUA_STRING)printTokens[65].varName="inmo.XR4500"printTokens[65].varLabel="XR4500 Light"printTokens[65].varType=AWTX_LUA_STRING
printTokens[65].varValue=inmo.XR4500
printTokens[65].varFunct=""end
function inmo.setXR4500PrintToken()awtx.fmtPrint.varSet(65,inmo.XR4500,"XR4500 Light",AWTX_LUA_STRING)end
function inmo.clrNewVars()end
function inmo.disable()inmo.Enabled=0
awtx.weight.inmotion(inmo.Enabled,inmo.StartIO,inmo.StopIO,inmo.SclNum,inmo.Multipy)end
function inmo.enable()inmo.Enabled=1
awtx.weight.inmotion(inmo.Enabled,inmo.StartIO,inmo.StopIO,inmo.SclNum,inmo.Multipy)end
function inmo.init()awtx.weight.inmotion(inmo.Enabled,inmo.StartIO,inmo.StopIO,inmo.SclNum,inmo.Multipy)end
local DB_FileLocation_AppConfig
local DB_FileLocation_AppData
local DB_FileLocation_Reports
local DB_ReportName_InMo
function inmo.InMoDBInit()local simAppPath,dbFile,result
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]DB_FileLocation_AppData=[[C:\Database\AppData.db]]DB_FileLocation_Reports=[[G:\Reports]]..SerialNumber
DB_ReportName_InMo=[[\InMoReport.csv]]dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblInMoConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:close()end
function inmo.extraStuffStore()local dbFile,result
local found=false
local sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblInMoConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")found=false
for row in dbFile:rows("SELECT varID FROM tblInMoConfig WHERE varID = 'Enabled'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblInMoConfig (varID, value) VALUES ('Enabled', '%d')",inmo.Enabled)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblInMoConfig SET value = '%d' WHERE varID = 'Enabled'",inmo.Enabled)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblInMoConfig WHERE varID = 'StartIO'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblInMoConfig (varID, value) VALUES ('StartIO', '%d')",inmo.StartIO)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblInMoConfig SET value = '%d' WHERE varID = 'StartIO'",inmo.StartIO)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblInMoConfig WHERE varID = 'StopIO'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblInMoConfig (varID, value) VALUES ('StopIO', '%d')",inmo.StopIO)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblInMoConfig SET value = '%d' WHERE varID = 'StopIO'",inmo.StopIO)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblInMoConfig WHERE varID = 'DelayTime'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblInMoConfig (varID, value) VALUES ('DelayTime', '%d')",inmo.DelayTime)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblInMoConfig SET value = '%d' WHERE varID = 'DelayTime'",inmo.DelayTime)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblInMoConfig WHERE varID = 'SclNum'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblInMoConfig (varID, value) VALUES ('SclNum', '%d')",inmo.SclNum)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblInMoConfig SET value = '%d' WHERE varID = 'SclNum'",inmo.SclNum)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblInMoConfig WHERE varID = 'Multipy'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblInMoConfig (varID, value) VALUES ('Multipy', '%f')",inmo.Multipy)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblInMoConfig SET value = '%f' WHERE varID = 'Multipy'",inmo.Multipy)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblInMoConfig WHERE varID = 'Multipy1'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblInMoConfig (varID, value) VALUES ('Multipy1', '%f')",inmo.Multipy1)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblInMoConfig SET value = '%f' WHERE varID = 'Multipy1'",inmo.Multipy1)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblInMoConfig WHERE varID = 'Multipy2'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblInMoConfig (varID, value) VALUES ('Multipy2', '%f')",inmo.Multipy2)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblInMoConfig SET value = '%f' WHERE varID = 'Multipy2'",inmo.Multipy2)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblInMoConfig WHERE varID = 'Switch2Gross'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblInMoConfig (varID, value) VALUES ('Switch2Gross', '%d')",inmo.Switch2Gross)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblInMoConfig SET value = '%d' WHERE varID = 'Switch2Gross'",inmo.Switch2Gross)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()end
function inmo.extraStuffRecall()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblInMoConfig (varID TEXT PRIMARY KEY, value TEXT)")found=false
for row in dbFile:rows("SELECT varID, value FROM tblInMoConfig WHERE varID = 'Enabled'")do
found=true
inmo.Enabled=tonumber(row[2])end
if found==false then
inmo.Enabled=inmo.EnabledDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblInMoConfig WHERE varID = 'StartIO'")do
found=true
inmo.StartIO=tonumber(row[2])end
if found==false then
inmo.StartIO=inmo.StartIODefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblInMoConfig WHERE varID = 'StopIO'")do
found=true
inmo.StopIO=tonumber(row[2])end
if found==false then
inmo.StopIO=inmo.StopIODefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblInMoConfig WHERE varID = 'DelayTime'")do
found=true
inmo.DelayTime=tonumber(row[2])end
if found==false then
inmo.DelayTime=inmo.DelayTimeDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblInMoConfig WHERE varID = 'SclNum'")do
found=true
inmo.SclNum=tonumber(row[2])end
if found==false then
inmo.SclNum=inmo.SclNumDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblInMoConfig WHERE varID = 'Multipy'")do
found=true
inmo.Multipy=tonumber(row[2])end
if found==false then
inmo.Multipy=inmo.MultipyDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblInMoConfig WHERE varID = 'Multipy1'")do
found=true
inmo.Multipy1=tonumber(row[2])end
if found==false then
inmo.Multipy1=inmo.Multipy1Default
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblInMoConfig WHERE varID = 'Multipy2'")do
found=true
inmo.Multipy2=tonumber(row[2])end
if found==false then
inmo.Multipy2=inmo.Multipy2Default
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblInMoConfig WHERE varID = 'Switch2Gross'")do
found=true
inmo.Switch2Gross=tonumber(row[2])end
if found==false then
inmo.Switch2Gross=inmo.Switch2GrossDefault
end
dbFile:close()inmo.Multipy=(inmo.Multipy1/1000000)+(inmo.Multipy2/10000000000000)end
function inmo.editInMoEnable(label)local tempEnabled,isEnterKey
inmo.extraStuffRecall()tempEnabled=inmo.Enabled
tempEnabled,isEnterKey=awtx.keypad.selectList("OFF,ON",tempEnabled)awtx.display.writeLine(label)if isEnterKey then
inmo.setInMoEnable(tempEnabled)else
end
end
function inmo.setInMoEnable(tempEnabled)inmo.Enabled=tempEnabled
inmo.extraStuffStore()inmo.init()end
function inmo.editInMoStartIO(label)local tempStartIO,IOmin,IOmax,isEnterKey
inmo.extraStuffRecall()tempStartIO=inmo.StartIO
IOmin=-3
IOmax=3
tempStartIO,isEnterKey=awtx.keypad.enterInteger(tempStartIO,IOmin,IOmax)awtx.display.writeLine(label)if isEnterKey then
inmo.setInMoStartIO(tempStartIO)else
end
end
function inmo.setInMoStartIO(tempStartIO)inmo.StartIO=tempStartIO
inmo.extraStuffStore()inmo.init()end
function inmo.editInMoStopIO(label)local tempStopIO,IOmin,IOmax,isEnterKey
inmo.extraStuffRecall()tempStopIO=inmo.StopIO
IOmin=-3
IOmax=3
tempStopIO,isEnterKey=awtx.keypad.enterInteger(tempStopIO,IOmin,IOmax)awtx.display.writeLine(label)if isEnterKey then
inmo.setInMoStopIO(tempStopIO)else
end
end
function inmo.setInMoStopIO(tempStopIO)inmo.StopIO=tempStopIO
inmo.extraStuffStore()inmo.init()end
function inmo.editInMoDelayTime(label)local tempDelayTime,Delaymin,Delaymax,isEnterKey
inmo.extraStuffRecall()tempDelayTime=inmo.DelayTime
Delaymin=0
Delaymax=60
tempDelayTime,isEnterKey=awtx.keypad.enterInteger(tempDelayTime,Delaymin,Delaymax)awtx.display.writeLine(label)if isEnterKey then
inmo.setInMoDelayTime(tempDelayTime)else
end
end
function inmo.setInMoDelayTime(tempDelayTime)inmo.DelayTime=tempDelayTime
inmo.extraStuffStore()inmo.init()end
function inmo.editInMoSclNum(label)local tempSclNum,Delaymin,Delaymax,isEnterKey
inmo.extraStuffRecall()tempSclNum=inmo.SclNum
SclNummin=0
SclNummax=8
tempSclNum,isEnterKey=awtx.keypad.enterInteger(tempSclNum,SclNummin,SclNummax)awtx.display.writeLine(label)if isEnterKey then
inmo.setInMoSclNum(tempSclNum)else
end
end
function inmo.setInMoSclNum(tempSclNum)inmo.SclNum=tempSclNum
inmo.extraStuffStore()inmo.init()end
function inmo.editInMoMultipy(label)local tempMultipy,tempMultipy1,tempMultipy2,Delaymin,Delaymax,isEnterKey
inmo.extraStuffRecall()tempMultipy=inmo.Multipy
tempMultipy1=inmo.Multipy1
tempMultipy2=inmo.Multipy2
Multipymin=-99999999
Multipymax=99999999
tempMultipy1,isEnterKey=awtx.keypad.enterFloat(tempMultipy1,Multipymin,Multipymax,separatorChar,entertime)tempMultipy2,isEnterKey=awtx.keypad.enterFloat(tempMultipy2,Multipymin,Multipymax,separatorChar,entertime)tempMultipy=(tempMultipy1/1000000)+(tempMultipy2/10000000000000)awtx.display.writeLine(label)if isEnterKey then
inmo.setInMoMultipy(tempMultipy,tempMultipy1,tempMultipy2)else
end
end
function inmo.setInMoMultipy(tempMultipy,tempMultipy1,tempMultipy2)inmo.Multipy=tempMultipy
inmo.Multipy1=tempMultipy1
inmo.Multipy2=tempMultipy2
inmo.extraStuffStore()inmo.init()end
function inmo.editInMoSwitch2Gross(label)local tempSwitch2Gross,isEnterKey
inmo.extraStuffRecall()tempSwitch2Gross=inmo.Switch2Gross
tempSwitch2Gross,isEnterKey=awtx.keypad.selectList("OFF,ON",tempSwitch2Gross)awtx.display.writeLine(label)if isEnterKey then
inmo.setInMoSwitch2Gross(tempSwitch2Gross)else
end
end
function inmo.setInMoSwitch2Gross(tempSwitch2Gross)inmo.Switch2Gross=tempSwitch2Gross
inmo.extraStuffStore()inmo.init()end
inmo.clrNewVars()inmo.InMoDBInit()inmo.extraStuffRecall()inmo.init()function InmotionStartEvent()inmo.delayFlag=false
inmo.timetick=0
inmo.delaytick=inmo.DelayTime*10
if inmo.Switch2Gross==1 then
awtx.weight.setActiveValue(VAL_INMO)end
end
awtx.weight.registerInmotionStartEvent(InmotionStartEvent)function InmotionCompleteEvent()wt=awtx.weight.getRefreshLastPrint()inmo.Weight=awtx.weight.getInmotion()awtx.printer.PrintFmt(0)if inmo.DelayTime~=0 then
inmo.delayFlag=true
inmo.timetick=0
inmo.delaytick=inmo.DelayTime*10
end
end
awtx.weight.registerInmotionCompleteEvent(InmotionCompleteEvent)