count={}counttable={}if config.calwtunitStr~=nil then
countCalUnit=config.calwtunitStr
end
if system.modelStr=="ZM301"then
MAX_COUNT_INDEX=1
elseif system.modelStr=="ZM303"then
MAX_COUNT_INDEX=1
elseif system.modelStr=="ZQ375"then
MAX_COUNT_INDEX=1
elseif system.modelStr=="ZM305GTN"then
MAX_COUNT_INDEX=1
elseif system.modelStr=="ZM305"then
MAX_COUNT_INDEX=1
else
MAX_COUNT_INDEX=1
end
count.SampleStepFlag=0
count.globalRPN=awtx.keypad.get_RPN_mode()count.newCountChannel=1
count.BulkDribbleFlagDefault=0
count.PrintTotalFlagDefault=0
count.TotalFmtDefault=9
count.ClearTotalFlagDefault=0
count.BulkDribbleFlag=count.BulkDribbleFlagDefault
count.PrintTotalFlag=count.PrintTotalFlagDefault
count.TotalFmt=count.TotalFmtDefault
count.ClearTotalFlag=count.ClearTotalFlagDefault
function count.initCountPrintTokens()awtx.fmtPrint.varSet(3,count.newCountChannel,"Accum Channel",AWTX_LUA_INTEGER)printTokens[3].varName="count.newCountChannel"printTokens[3].varLabel="Accum Channel"printTokens[3].varType=AWTX_LUA_INTEGER
printTokens[3].varValue=count.newCountChannel
printTokens[3].varFunct=count.newSetAccum
end
function count.setCountPrintTokens()awtx.fmtPrint.varSet(3,count.newCountChannel,"Accum Channel",AWTX_LUA_INTEGER)end
local DB_FileLocation_AppConfig
local DB_FileLocation_AppData
local DB_FileLocation_Reports
local DB_ReportName_Count
function count.enterPcwt()local usermode,newPcWt,PcWtmin,PcWtmax,isEnterKey
PcWtmin=0.00001
PcWtmax=100000
newPcWt=awtx.weight.getPieceWeight()newPcWt=awtx.weight.convertFromInternalCalUnit(newPcWt,wt.units,0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)newPcWt,isEnterKey=awtx.keypad.enterFloat(newPcWt,PcWtmin,PcWtmax,separatorChar,entertime)awtx.display.setMode(usermode)if isEnterKey then
newPcWt=awtx.weight.convertToInternalCalUnit(newPcWt,0)awtx.weight.setPieceWeight(newPcWt)awtx.weight.setActiveValue(VAL_COUNT)else
end
end
function count.enterSmpsz()local usermode,newSMPSZ,SMPSZmin,SMPSZmax,isEnterKey
SMPSZmin=1
SMPSZmax=100000
newSMPSZ=awtx.weight.getSampleSize()usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)newSMPSZ,isEnterKey=awtx.keypad.enterInteger(newSMPSZ,SMPSZmin,SMPSZmax,entertime)awtx.display.setMode(usermode)if newSMPSZ==-1 then
displayERROR()isEnterKey=false
else
end
if isEnterKey then
awtx.weight.setSampleSize(newSMPSZ)else
end
return isEnterKey
end
function count.editSampleMode(label)local tempBulkDribbleFlag,isEnterKey
count.extraStuffRecall()tempBulkDribbleFlag=count.BulkDribbleFlag
tempBulkDribbleFlag,isEnterKey=awtx.keypad.selectList("bulk,dribble",tempBulkDribbleFlag)awtx.display.writeLine(label)if isEnterKey then
count.BulkDribbleFlag=tempBulkDribbleFlag
count.extraStuffStore()else
end
end
function count.checkSample()local usermode,isEnterKey,exitkeyCode1,exitkeyCode2,lastkeyCode
count.SampleStepFlag=count.SampleStepFlag+1
if count.SampleStepFlag==1 then
count.globalRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)wt=awtx.weight.getCurrent(1)if wt.tare==0 then
awtx.display.writeLine("ZEROING")awtx.weight.requestZero()else
awtx.display.writeLine("ZEROING")awtx.weight.requestTare()end
elseif count.SampleStepFlag==2 then
if count.BulkDribbleFlag==0 then
isEnterKey=count.enterSmpsz()if isEnterKey then
awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("ADD")awtx.weight.requestSample(2)else
displayABORT()count.SampleStepFlag=0
awtx.keypad.set_RPN_mode(count.globalRPN)end
elseif count.BulkDribbleFlag==1 then
isEnterKey=count.enterSmpsz()if isEnterKey then
awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("ADD")exitkeyCode1=21
exitkeyCode2=43
while true do
lastkeyCode=awtx.keypad.getKeyWithoutEvents()if(lastkeyCode==exitkeyCode1)or(lastkeyCode==exitkeyCode2)then
break
end
end
awtx.weight.requestSample(1)awtx.keypad.set_RPN_mode(count.globalRPN)else
displayABORT()count.SampleStepFlag=0
awtx.keypad.set_RPN_mode(count.globalRPN)end
end
else
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)count.SampleStepFlag=0
awtx.keypad.set_RPN_mode(count.globalRPN)end
end
function SAMPLECOMPLETEEVENT(eventResult,eventResultString)local usermode
if eventResult~=0 then
displayCANT()else
awtx.weight.setActiveValue(VAL_COUNT)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)end
count.SampleStepFlag=0
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(count.globalRPN)end
awtx.weight.registerSampleCompleteEvent(SAMPLECOMPLETEEVENT)function count.countInit()for index=1,MAX_COUNT_INDEX do
counttable[index]={}counttable[index].accIndex=index
counttable[index].accId=0
counttable[index].grossTotal=0
counttable[index].netTotal=0
counttable[index].tareTotal=counttable[index].grossTotal-counttable[index].netTotal
counttable[index].countTotal=0
counttable[index].transactionCount=0
counttable[index].units="lb"end
end
function count.countPrint()wt=awtx.weight.getCurrent(1)widthprec=wt.curDigitsTotal.."."..wt.curDigitsRight
for index=1,MAX_COUNT_INDEX do
formatStr=string.gsub("\r\n grossTotal       %***f %s","***",widthprec)formatStr=string.gsub("\r\n netTotal         %***f %s","***",widthprec)formatStr=string.gsub("\r\n tareTotal        %***f %s","***",widthprec)end
end
function count.countDBInit()local simAppPath,dbFile,result
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]DB_FileLocation_AppData=[[C:\Database\AppData.db]]DB_FileLocation_Reports=[[G:\Reports]]..SerialNumber
DB_ReportName_Count=[[\CountReport.csv]]dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCountAccum (accIndex INTEGER, accId INTEGER, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, countTotal INTEGER, transactionCount INTEGER, units VARCHAR)")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCountLastChan (Channel INTEGER)")dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCountConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:close()end
function count.countDBStore()local dbFile,result,searchIndex,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCountAccum (accIndex INTEGER, accId INTEGER, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, countTotal INTEGER, transactionCount INTEGER, units VARCHAR)")dbFile:execute("BEGIN TRANSACTION")for index=1,MAX_COUNT_INDEX do
searchIndex=counttable[index].accIndex
sqlStr=string.format("SELECT tblCountAccum.accIndex FROM tblCountAccum WHERE tblCountAccum.accIndex = '%d'",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblCountAccum (accIndex, accId, grossTotal, netTotal, tareTotal, countTotal, transactionCount, units) VALUES ('%d', '%d', '%f', '%f', '%f', '%d', '%d', '%s')",counttable[index].accIndex,counttable[index].accId,counttable[index].grossTotal,counttable[index].netTotal,counttable[index].tareTotal,counttable[index].countTotal,counttable[index].transactionCount,counttable[index].units)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblCountAccum SET accIndex = '%d', accId = '%d', grossTotal = '%f', netTotal = '%f', tareTotal = '%f', countTotal = '%d', transactionCount = '%d', units = '%s' WHERE tblCountAccum.accIndex = '%d'",counttable[index].accIndex,counttable[index].accId,counttable[index].grossTotal,counttable[index].netTotal,counttable[index].tareTotal,counttable[index].countTotal,counttable[index].transactionCount,counttable[index].units,searchIndex)result=dbFile:exec(sqlStr)end
end
dbFile:execute("COMMIT")dbFile:close()count.lastChannStore()end
function count.lastChannStore()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCountLastChan (Channel INTEGER)")dbFile:execute("BEGIN TRANSACTION")sqlStr=string.format("SELECT tblCountLastChan.Channel FROM tblCountLastChan")found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblCountLastChan (Channel) VALUES ('%d')",count.newCountChannel)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblCountLastChan SET Channel = '%d'",count.newCountChannel)result=dbFile:exec(sqlStr)end
count.setCountPrintTokens()dbFile:execute("COMMIT")dbFile:close()end
function count.extraStuffStore()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCountConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")found=false
for row in dbFile:rows("SELECT varID FROM tblCountConfig WHERE varID = 'BulkDribbleFlag'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblCountConfig (varID, value) VALUES ('BulkDribbleFlag', '%d')",count.BulkDribbleFlag)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblCountConfig SET value = '%d' WHERE varID = 'BulkDribbleFlag'",count.BulkDribbleFlag)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblCountConfig WHERE varID = 'PrintTotalFlag'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblCountConfig (varID, value) VALUES ('PrintTotalFlag', '%d')",count.PrintTotalFlag)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblCountConfig SET value = '%d' WHERE varID = 'PrintTotalFlag'",count.PrintTotalFlag)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblCountConfig WHERE varID = 'TotalFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblCountConfig (varID, value) VALUES ('TotalFmt', '%d')",count.TotalFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblCountConfig SET value = '%d' WHERE varID = 'TotalFmt'",count.TotalFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblCountConfig WHERE varID = 'ClearTotalFlag'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblCountConfig (varID, value) VALUES ('ClearTotalFlag', '%d')",count.ClearTotalFlag)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblCountConfig SET value = '%d' WHERE varID = 'ClearTotalFlag'",count.ClearTotalFlag)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()end
function count.countDBRecall()local dbFile,result,index,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCountAccum (accIndex INTEGER, accId INTEGER, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, countTotal INTEGER, transactionCount INTEGER, units VARCHAR)")for row in dbFile:rows("SELECT accIndex, accId, grossTotal, netTotal, tareTotal, countTotal, transactionCount, units FROM tblCountAccum")do
index=row[1]if index<=MAX_COUNT_INDEX then
counttable[index].accIndex=row[1]counttable[index].accId=row[2]counttable[index].grossTotal=row[3]counttable[index].netTotal=row[4]counttable[index].tareTotal=row[5]counttable[index].countTotal=row[6]counttable[index].transactionCount=row[7]counttable[index].units=row[8]end
end
dbFile:close()end
function count.lastChannRecall()local dbFile,result,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCountLastChan (Channel INTEGER)")sqlStr=string.format("SELECT Channel FROM tblCountLastChan")for row in dbFile:rows(sqlStr)do
count.newCountChannel=row[1]end
count.setCountPrintTokens()dbFile:close()end
function count.extraStuffRecall()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCountConfig (varID TEXT PRIMARY KEY, value TEXT)")found=false
for row in dbFile:rows("SELECT varID, value FROM tblCountConfig WHERE varID = 'BulkDribbleFlag'")do
found=true
count.BulkDribbleFlag=tonumber(row[2])end
if found==false then
count.BulkDribbleFlag=count.BulkDribbleFlagDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblCountConfig WHERE varID = 'PrintTotalFlag'")do
found=true
count.PrintTotalFlag=tonumber(row[2])end
if found==false then
count.PrintTotalFlag=count.PrintTotalFlagDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblCountConfig WHERE varID = 'TotalFmt'")do
found=true
count.TotalFmt=tonumber(row[2])end
if found==false then
count.TotalFmt=count.TotalFmtDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblCountConfig WHERE varID = 'ClearTotalFlag'")do
found=true
count.ClearTotalFlag=tonumber(row[2])end
if found==false then
count.ClearTotalFlag=count.ClearTotalFlagDefault
end
dbFile:close()end
function count.countDBClear()count.countInit()count.countDBStore()count.countDBRecall()count.newCountChannel=1
count.lastChannStore()count.newSetAccum(count.newCountChannel)count.setCountPrintTokens()end
function count.editPrintTotalFlag(label)local tempPrintTotalFlag,isEnterKey
count.extraStuffRecall()tempPrintTotalFlag=count.PrintTotalFlag
tempPrintTotalFlag,isEnterKey=awtx.keypad.selectList("OFF,ON",tempPrintTotalFlag)awtx.display.writeLine(label)if isEnterKey then
count.PrintTotalFlag=tempPrintTotalFlag
count.extraStuffStore()else
end
end
function count.editTotalFmt(label)local tempTotalFmt,FMTmin,FMTmax,isEnterKey
count.extraStuffRecall()tempTotalFmt=count.TotalFmt
FMTmin=1
FMTmax=40
tempTotalFmt,isEnterKey=awtx.keypad.enterInteger(tempTotalFmt,FMTmin,FMTmax)awtx.display.writeLine(label)if isEnterKey then
count.TotalFmt=tempTotalFmt
count.extraStuffStore()else
end
end
function count.editClearTotalFlag(label)local tempClearTotalFlag,isEnterKey
count.extraStuffRecall()tempClearTotalFlag=count.ClearTotalFlag
tempClearTotalFlag,isEnterKey=awtx.keypad.selectList("OFF,ON",tempClearTotalFlag)awtx.display.writeLine(label)if isEnterKey then
count.ClearTotalFlag=tempClearTotalFlag
count.extraStuffStore()else
end
end
function count.CountDBReport(label)local usermode,currentRPN,index,isEnterKey,dbFile,row,fho,err
local newGrossTotal,newNetTotal,newTareTotal
local t={}currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)index,isEnterKey=awtx.keypad.selectList(" Port 1, Port 2, USB",0)awtx.display.writeLine(label)if isEnterKey then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(DB_FileLocation_AppData)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblCountAccum (accIndex INTEGER, accId INTEGER, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, countTotal INTEGER, transactionCount INTEGER, units VARCHAR)")if index==0 or index==1 then
t[#t+1]=string.format("accIndex, accId, grossTotal, netTotal, tareTotal, countTotal, transactionCount, units \r\n")elseif index==2 then
t[#t+1]=string.format("accIndex, accId, grossTotal, netTotal, tareTotal, countTotal, transactionCount, units \r\n")end
for row in dbFile:rows("SELECT accIndex, accId, grossTotal, netTotal, tareTotal, countTotal, transactionCount, units FROM tblCountAccum")do
newGrossTotal=awtx.weight.convertFromInternalCalUnit(row[3],wt.units,1)newNetTotal=awtx.weight.convertFromInternalCalUnit(row[4],wt.units,1)newTareTotal=awtx.weight.convertFromInternalCalUnit(row[5],wt.units,1)if index==0 or index==1 then
t[#t+1]=string.format("%d, %d, %f, %f, %f, %d, %d, %s \r\n",row[1],row[2],newGrossTotal,newNetTotal,newTareTotal,row[6],row[7],wt.unitsStr)elseif index==2 then
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
fho,err=io.open(string.format("%s%s",DB_FileLocation_Reports,DB_ReportName_Count),"w")if err then
awtx.display.writeLine("err "..err)awtx.os.sleep(canttime)awtx.os.sleep(canttime)else
fho:write(table.concat(t))fho:close()end
else
awtx.display.writeLine("err "..tostring(result))end
else
end
awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function count.CountDBReset(label)local usermode,index,isEnterKey
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)count.countDBClear()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
end
end
function count.newSetAccum(channel)if channel==0 then
else
count.countDBRecall()ID.id=counttable[channel].accId
awtx.weight.setAccum(0,counttable[channel].grossTotal,counttable[channel].netTotal,counttable[channel].countTotal,counttable[channel].transactionCount,1)end
end
function count.newGetAccum(channel)if channel==0 then
else
wt=awtx.weight.getLastPrint()counttable[channel].grossTotal=wt.gtotcal
counttable[channel].netTotal=wt.ntotcal
counttable[channel].tareTotal=wt.gtotcal-wt.ntotcal
counttable[channel].countTotal=wt.ctot
counttable[channel].transactionCount=wt.tran
awtx.printer.PrintFmt(0)count.countDBStore()end
end
function count.newClrAccum()channel=count.newCountChannel
if channel==0 then
else
counttable[channel].grossTotal=0
counttable[channel].netTotal=0
counttable[channel].tareTotal=0
counttable[channel].countTotal=0
counttable[channel].transactionCount=0
count.countDBStore()awtx.weight.setAccum(counttable[channel].grossTotal,counttable[channel].netTotal,counttable[channel].countTotal,counttable[channel].transactionCount)end
end
count.countInit()count.countDBInit()count.countDBRecall()count.lastChannRecall()if count.newCountChannel~=nil and count.newCountChannel<=MAX_COUNT_INDEX then
count.newSetAccum(count.newCountChannel)count.setCountPrintTokens()else
count.newCountChannel=1
count.lastChannStore()count.newSetAccum(count.newCountChannel)count.setCountPrintTokens()end
count.extraStuffRecall()function count.AccumChannelSelect()local usermode,isEnterKey
count.newCountChannel=count.newCountChannel-1
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)count.newCountChannel,isEnterKey=awtx.keypad.selectList("CHAN1,CHAN2,CHAN3,CHAN4,CHAN5,CHAN6,CHAN7,CHAN8,CHAN9,CHAN10",count.newCountChannel)awtx.display.setMode(usermode)count.newCountChannel=count.newCountChannel+1
if isEnterKey then
count.lastChannStore()count.newSetAccum(count.newCountChannel)else
end
count.setCountPrintTokens()end
function awtx.keypad.KEY_ACCUM_DOWN()end
function awtx.keypad.KEY_ACCUM_REPEAT()end
function awtx.keypad.KEY_ACCUM_UP()awtx.weight.requestAccum()end
function ACCUMCOMPLETEEVENT(eventResult,eventResultString)if eventResult~=0 then
displayCANT()else
displayWORD("  ACC  ")count.newGetAccum(count.newCountChannel)end
end
awtx.weight.registerAccumCompleteEvent(ACCUMCOMPLETEEVENT)