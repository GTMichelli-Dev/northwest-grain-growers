accum={}newAccumChannel=1
accum.PrintTotalFlagDefault=0
accum.TotalFmtDefault=10
accum.ClearTotalFlagDefault=0
accum.PrintTotalFlag=accum.PrintTotalFlagDefault
accum.TotalFmt=accum.TotalFmtDefault
accum.ClearTotalFlag=accum.ClearTotalFlagDefault
accumtable={}if config.calwtunitStr~=nil then
accumCalUnit=config.calwtunitStr
end
if system.modelStr=="ZM301"then
MIN_ACCUM_INDEX=0
MAX_ACCUM_INDEX=10
elseif system.modelStr=="ZM303"then
MIN_ACCUM_INDEX=0
MAX_ACCUM_INDEX=10
elseif system.modelStr=="ZQ375"then
MIN_ACCUM_INDEX=0
MAX_ACCUM_INDEX=10
elseif system.modelStr=="ZM305GTN"then
MIN_ACCUM_INDEX=0
MAX_ACCUM_INDEX=10
elseif system.modelStr=="ZM305"then
MIN_ACCUM_INDEX=0
MAX_ACCUM_INDEX=200
else
MIN_ACCUM_INDEX=0
MAX_ACCUM_INDEX=10
end
function accum.initAccumPrintTokens()awtx.fmtPrint.varSet(3,newAccumChannel,"Accum Channel",AWTX_LUA_INTEGER)printTokens[3].varName="newAccumChannel"printTokens[3].varLabel="Accum Channel"printTokens[3].varType=AWTX_LUA_INTEGER
printTokens[3].varValue=newAccumChannel
printTokens[3].varFunct=accum.newSetAccum
end
function accum.setAccumPrintTokens()awtx.fmtPrint.varSet(3,newAccumChannel,"Accum Channel",AWTX_LUA_INTEGER)end
function accum.accumInit()for index=1,MAX_ACCUM_INDEX do
accumtable[index]={}accumtable[index].accIndex=index
accumtable[index].accId=0
accumtable[index].grossTotal=0
accumtable[index].netTotal=0
accumtable[index].tareTotal=accumtable[index].grossTotal-accumtable[index].netTotal
accumtable[index].countTotal=0
accumtable[index].transactionCount=0
accumtable[index].units="lb"end
end
function accum.accumPrint()wt=awtx.weight.getCurrent(1)widthprec=wt.curDigitsTotal.."."..wt.curDigitsRight
for index=1,MAX_ACCUM_INDEX do
formatStr=string.gsub("\r\ngrossTotal       %***f %s","***",widthprec)formatStr=string.gsub("\r\nnetTotal         %***f %s","***",widthprec)formatStr=string.gsub("\r\ntareTotal        %***f %s","***",widthprec)end
end
local DB_FileLocation_AppConfig
local DB_FileLocation_AppData
local DB_FileLocation_Reports
local DB_ReportName_Accum
function accum.accumDBInit()local simAppPath,dbFile,result
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]DB_FileLocation_AppData=[[C:\Database\AppData.db]]DB_FileLocation_Reports=[[G:\Reports]]..SerialNumber
DB_ReportName_Accum=[[\AccumReport.csv]]dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblAccum (accIndex INTEGER, accId INTEGER, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, countTotal INTEGER, transactionCount INTEGER, units VARCHAR)")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblAccumLastChan (Channel INTEGER)")dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblAccumConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:close()end
function accum.accumDBStore()local dbFile,result
local found=false
local sqlStr,searchIndex,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblAccum (accIndex INTEGER, accId INTEGER, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, countTotal INTEGER, transactionCount INTEGER, units VARCHAR)")dbFile:execute("BEGIN TRANSACTION")for index=1,MAX_ACCUM_INDEX do
searchIndex=accumtable[index].accIndex
sqlStr=string.format("SELECT tblAccum.accIndex FROM tblAccum WHERE tblAccum.accIndex = '%d'",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblAccum (accIndex, accId, grossTotal, netTotal, tareTotal, countTotal, transactionCount, units) VALUES ('%d', '%d', '%f', '%f', '%f', '%d', '%d', '%s')",accumtable[index].accIndex,accumtable[index].accId,accumtable[index].grossTotal,accumtable[index].netTotal,accumtable[index].tareTotal,accumtable[index].countTotal,accumtable[index].transactionCount,accumtable[index].units)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblAccum SET accIndex = '%d', accId = '%d', grossTotal = '%f', netTotal = '%f', tareTotal = '%f', countTotal = '%d', transactionCount = '%d', units = '%s' WHERE tblAccum.accIndex = '%d'",accumtable[index].accIndex,accumtable[index].accId,accumtable[index].grossTotal,accumtable[index].netTotal,accumtable[index].tareTotal,accumtable[index].countTotal,accumtable[index].transactionCount,accumtable[index].units,searchIndex)result=dbFile:exec(sqlStr)end
end
dbFile:execute("COMMIT")dbFile:close()accum.lastChannStore()end
function accum.accumDBStoreSingle(channel)local dbFile,result
local found=false
local sqlStr,searchIndex,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblAccum (accIndex INTEGER, accId INTEGER, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, countTotal INTEGER, transactionCount INTEGER, units VARCHAR)")dbFile:execute("BEGIN TRANSACTION")index=channel
searchIndex=accumtable[index].accIndex
sqlStr=string.format("SELECT tblAccum.accIndex FROM tblAccum WHERE tblAccum.accIndex = '%d'",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblAccum (accIndex, accId, grossTotal, netTotal, tareTotal, countTotal, transactionCount, units) VALUES ('%d', '%d', '%f', '%f', '%f', '%d', '%d', '%s')",accumtable[index].accIndex,accumtable[index].accId,accumtable[index].grossTotal,accumtable[index].netTotal,accumtable[index].tareTotal,accumtable[index].countTotal,accumtable[index].transactionCount,accumtable[index].units)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblAccum SET accIndex = '%d', accId = '%d', grossTotal = '%f', netTotal = '%f', tareTotal = '%f', countTotal = '%d', transactionCount = '%d', units = '%s' WHERE tblAccum.accIndex = '%d'",accumtable[index].accIndex,accumtable[index].accId,accumtable[index].grossTotal,accumtable[index].netTotal,accumtable[index].tareTotal,accumtable[index].countTotal,accumtable[index].transactionCount,accumtable[index].units,searchIndex)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()accum.lastChannStore()end
function accum.lastChannStore()local dbFile,result
local found=false
local sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblAccumLastChan (Channel INTEGER)")dbFile:execute("BEGIN TRANSACTION")sqlStr=string.format("SELECT tblAccumLastChan.Channel FROM tblAccumLastChan")found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblAccumLastChan (Channel) VALUES ('%d')",newAccumChannel)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblAccumLastChan SET Channel = '%d'",newAccumChannel)result=dbFile:exec(sqlStr)end
accum.setAccumPrintTokens()dbFile:execute("COMMIT")dbFile:close()end
function accum.extraStuffStore()local dbFile,result
local found=false
local sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblAccumConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")found=false
for row in dbFile:rows("SELECT varID FROM tblAccumConfig WHERE varID = 'PrintTotalFlag'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblAccumConfig (varID, value) VALUES ('PrintTotalFlag', '%d')",accum.PrintTotalFlag)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblAccumConfig SET value = '%d' WHERE varID = 'PrintTotalFlag'",accum.PrintTotalFlag)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblAccumConfig WHERE varID = 'TotalFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblAccumConfig (varID, value) VALUES ('TotalFmt', '%d')",accum.TotalFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblAccumConfig SET value = '%d' WHERE varID = 'TotalFmt'",accum.TotalFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblAccumConfig WHERE varID = 'ClearTotalFlag'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblAccumConfig (varID, value) VALUES ('ClearTotalFlag', '%d')",accum.ClearTotalFlag)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblAccumConfig SET value = '%d' WHERE varID = 'ClearTotalFlag'",accum.ClearTotalFlag)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()end
function accum.accumDBRecall()local dbFile,result,sqlStr,index
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblAccum (accIndex INTEGER, accId INTEGER, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, countTotal INTEGER, transactionCount INTEGER, units VARCHAR)")sqlStr=string.format("SELECT accIndex, accId, grossTotal, netTotal, tareTotal, countTotal, transactionCount, units FROM tblAccum")for row in dbFile:rows(sqlStr)do
index=row[1]if index<=MAX_ACCUM_INDEX then
accumtable[index].accIndex=row[1]accumtable[index].accId=row[2]accumtable[index].grossTotal=row[3]accumtable[index].netTotal=row[4]accumtable[index].tareTotal=row[5]accumtable[index].countTotal=row[6]accumtable[index].transactionCount=row[7]accumtable[index].units=row[8]end
end
dbFile:close()end
function accum.lastChannRecall()local dbFile,result,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblAccumLastChan (Channel INTEGER)")sqlStr=string.format("SELECT Channel FROM tblAccumLastChan")for row in dbFile:rows(sqlStr)do
newAccumChannel=row[1]end
accum.setAccumPrintTokens()dbFile:close()end
function accum.extraStuffRecall()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblAccumConfig (varID TEXT PRIMARY KEY, value TEXT)")found=false
for row in dbFile:rows("SELECT varID, value FROM tblAccumConfig WHERE varID = 'PrintTotalFlag'")do
found=true
accum.PrintTotalFlag=tonumber(row[2])end
if found==false then
accum.PrintTotalFlag=accum.PrintTotalFlagDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblAccumConfig WHERE varID = 'TotalFmt'")do
found=true
accum.TotalFmt=tonumber(row[2])end
if found==false then
accum.TotalFmt=accum.TotalFmtDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblAccumConfig WHERE varID = 'ClearTotalFlag'")do
found=true
accum.ClearTotalFlag=tonumber(row[2])end
if found==false then
accum.ClearTotalFlag=accum.ClearTotalFlagDefault
end
dbFile:close()end
function accum.accumDBClear()accum.accumInit()accum.accumDBStore()accum.accumDBRecall()newAccumChannel=1
accum.lastChannStore()accum.newSetAccum(newAccumChannel)accum.setAccumPrintTokens()end
function accum.editPrintTotalFlag(label)local tempPrintTotalFlag,isEnterKey
accum.extraStuffRecall()tempPrintTotalFlag=accum.PrintTotalFlag
tempPrintTotalFlag,isEnterKey=awtx.keypad.selectList("OFF,ON",tempPrintTotalFlag)awtx.display.writeLine(label)if isEnterKey then
accum.PrintTotalFlag=tempPrintTotalFlag
else
end
accum.extraStuffStore()end
function accum.editTotalFmt(label)local tempTotalFmt,FMTmin,FMTmax,isEnterKey
accum.extraStuffRecall()tempTotalFmt=accum.TotalFmt
FMTmin=1
FMTmax=40
tempTotalFmt,isEnterKey=awtx.keypad.enterInteger(tempTotalFmt,FMTmin,FMTmax)awtx.display.writeLine(label)if isEnterKey then
accum.TotalFmt=tempTotalFmt
else
end
accum.extraStuffStore()end
function accum.editClearTotalFlag(label)local tempClearTotalFlag,isEnterKey
accum.extraStuffRecall()tempClearTotalFlag=accum.ClearTotalFlag
tempClearTotalFlag,isEnterKey=awtx.keypad.selectList("OFF,ON",tempClearTotalFlag)awtx.display.writeLine(label)if isEnterKey then
accum.ClearTotalFlag=tempClearTotalFlag
else
end
accum.extraStuffStore()end
function accum.AccumDBReport(label)local usermode,index,isEnterKey,dbFile,result,row,fho,err
local currentRPN,newGrossTotal,newNetTotal,newTareTotal
local t={}currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)index,isEnterKey=awtx.keypad.selectList(" Port 1, Port 2, USB",0)awtx.display.writeLine(label)if isEnterKey then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(DB_FileLocation_AppData)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblAccum (accIndex INTEGER, accId INTEGER, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, countTotal INTEGER, transactionCount INTEGER, units VARCHAR)")if index==0 or index==1 then
t[#t+1]=string.format("channel Id grossTotal  net Total tare Total cnt Total transactions units \r\n")elseif index==2 then
t[#t+1]=string.format("accIndex, accId, grossTotal, netTotal, tareTotal, countTotal, transactionCount, units \r\n")end
for row in dbFile:rows("SELECT accIndex, accId, grossTotal, netTotal, tareTotal, countTotal, transactionCount, units FROM tblAccum")do
newGrossTotal=awtx.weight.convertFromInternalCalUnit(row[3],wt.units,1)newNetTotal=awtx.weight.convertFromInternalCalUnit(row[4],wt.units,1)newTareTotal=awtx.weight.convertFromInternalCalUnit(row[5],wt.units,1)if index==0 or index==1 then
t[#t+1]=string.format("%7d %2d %10f %10f %10f %8d %12d %5s \r\n",row[1],row[2],newGrossTotal,newNetTotal,newTareTotal,row[6],row[7],wt.unitsStr)elseif index==2 then
t[#t+1]=string.format("%d, %d, %f, %f, %f, %d, %d, %s \r\n",row[1],row[2],newGrossTotal,newNetTotal,newTareTotal,row[6],row[7],wt.unitsStr)else
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
fho,err=io.open(string.format("%s%s",DB_FileLocation_Reports,DB_ReportName_Accum),"w")if err then
awtx.display.writeLine("err "..err)awtx.os.sleep(canttime)awtx.os.sleep(canttime)else
fho:write(table.concat(t))fho:close()end
else
awtx.display.writeLine("err "..tostring(result))end
else
end
awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function accum.AccumDBReset(label)local usermode,index,isEnterKey
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)accum.accumDBClear()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
end
end
function accum.newSetAccum(channel)if channel==0 then
elseif channel<=MAX_ACCUM_INDEX then
newAccumChannel=channel
accum.accumDBRecall()ID.id=accumtable[channel].accId
awtx.weight.setAccum(0,accumtable[channel].grossTotal,accumtable[channel].netTotal,accumtable[channel].countTotal,accumtable[channel].transactionCount,1)end
end
function accum.newGetAccum(channel)if channel==0 then
else
wt=awtx.weight.getLastPrint()accumtable[channel].grossTotal=wt.gtotcal
accumtable[channel].netTotal=wt.ntotcal
accumtable[channel].tareTotal=wt.gtotcal-wt.ntotcal
accumtable[channel].countTotal=wt.ctot
accumtable[channel].transactionCount=wt.tran
awtx.printer.PrintFmt(0)accum.accumDBStoreSingle(channel)end
end
function accum.newClrAccum()channel=newAccumChannel
if channel==0 then
else
accumtable[channel].grossTotal=0
accumtable[channel].netTotal=0
accumtable[channel].tareTotal=0
accumtable[channel].countTotal=0
accumtable[channel].transactionCount=0
accum.accumDBStoreSingle(channel)awtx.weight.setAccum(accumtable[channel].grossTotal,accumtable[channel].netTotal,accumtable[channel].countTotal,accumtable[channel].transactionCount)end
end
accum.accumInit()accum.accumDBInit()accum.accumDBRecall()accum.lastChannRecall()if newAccumChannel~=nil and newAccumChannel<=MAX_ACCUM_INDEX then
accum.newSetAccum(newAccumChannel)accum.setAccumPrintTokens()else
newAccumChannel=1
accum.lastChannStore()accum.newSetAccum(newAccumChannel)accum.setAccumPrintTokens()end
accum.extraStuffRecall()function accum.AccumChannelSelect()local usermode,isEnterKey
if MAX_ACCUM_INDEX==10 then
newAccumChannel=newAccumChannel-1
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)newAccumChannel,isEnterKey=awtx.keypad.selectList("CHAN1,CHAN2,CHAN3,CHAN4,CHAN5,CHAN6,CHAN7,CHAN8,CHAN9,CHAN10",newAccumChannel)awtx.display.setMode(usermode)newAccumChannel=newAccumChannel+1
else
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)newAccumChannel,isEnterKey=awtx.keypad.enterInteger(newAccumChannel,MIN_ACCUM_INDEX,MAX_ACCUM_INDEX)awtx.display.setMode(usermode)end
if isEnterKey then
accum.lastChannStore()accum.newSetAccum(newAccumChannel)else
end
accum.setAccumPrintTokens()end
function awtx.keypad.KEY_ACCUM_DOWN()end
function awtx.keypad.KEY_ACCUM_REPEAT()end
function awtx.keypad.KEY_ACCUM_UP()awtx.weight.requestAccum()end
function ACCUMCOMPLETEEVENT(eventResult,eventResultString)local usermode
if eventResult~=0 then
displayCANT()else
displayWORD("  ACC  ")accum.newGetAccum(newAccumChannel)end
end
awtx.weight.registerAccumCompleteEvent(ACCUMCOMPLETEEVENT)