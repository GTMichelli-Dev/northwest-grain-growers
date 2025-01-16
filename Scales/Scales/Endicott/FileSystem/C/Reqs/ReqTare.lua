MAX_TARE_INDEX=10
tare={}taretable={}newTareChannelDefault=1
newTareChannel=newTareChannelDefault
DefaultKeyTare301Enable=0
DefaultPresetTare301Enable=0
DefaultKeyTare303Enable=0
DefaultPresetTare303Enable=0
DefaultKeyTare375Enable=0
DefaultPresetTare375Enable=0
DefaultKeyTare305Enable=0
DefaultPresetTare305Enable=0
DefaultKeyTare305GTNEnable=0
DefaultPresetTare305GTNEnable=0
KeyTare301Enable=DefaultKeyTare301Enable
PresetTare301Enable=DefaultPresetTare301Enable
KeyTare303Enable=DefaultKeyTare303Enable
PresetTare303Enable=DefaultPresetTare303Enable
KeyTare375Enable=DefaultKeyTare375Enable
PresetTare375Enable=DefaultPresetTare375Enable
KeyTare305Enable=DefaultKeyTare305Enable
PresetTare305Enable=DefaultPresetTare305Enable
KeyTare305GTNEnable=DefaultKeyTare305GTNEnable
PresetTare305GTNEnable=DefaultPresetTare305GTNEnable
PBTarePressed=0
function tare.initTarePrintTokens()awtx.fmtPrint.varSet(2,newTareChannel,"Tare Channel",AWTX_LUA_INTEGER)printTokens[2].varName="newTareChannel"printTokens[2].varLabel="Tare Channel"printTokens[2].varType=AWTX_LUA_INTEGER
printTokens[2].varValue=newTareChannel
printTokens[2].varFunct=tare.setTareChannel
end
function tare.setTarePrintTokens()awtx.fmtPrint.varSet(2,newTareChannel,"Tare Channel",AWTX_LUA_INTEGER)end
local DB_FileLocation_AppConfig
local DB_FileLocation_AppData
local DB_FileLocation_Reports
local DB_ReportName_Tare
if config.calwtunitStr~=nil then
tareCalUnit=config.calwtunitStr
end
if system.modelStr=="ZM301"then
MAX_TARE_INDEX=10
elseif system.modelStr=="ZM303"then
MAX_TARE_INDEX=10
elseif system.modelStr=="ZQ375"then
MAX_TARE_INDEX=10
elseif system.modelStr=="ZM305GTN"then
MAX_TARE_INDEX=10
elseif system.modelStr=="ZM305"then
MAX_TARE_INDEX=10
else
MAX_TARE_INDEX=10
end
function tare.tareInit()local index
for index=1,MAX_TARE_INDEX do
taretable[index]={}taretable[index].tareIndex=index
taretable[index].presetTare=0
taretable[index].units=tareCalUnit
end
end
function tare.tarePrint()local widthprec,index,formatStr
wt=awtx.weight.getCurrent(1)widthprec=wt.curDigitsTotal.."."..wt.curDigitsRight
for index=1,MAX_TARE_INDEX do
formatStr=string.gsub("\r\nPresetTare    %***f %s","***",widthprec)end
end
function tare.tareDBInit()local simAppPath,dbFile,result
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]DB_FileLocation_AppData=[[C:\Database\AppData.db]]DB_FileLocation_Reports=[[G:\Reports]]..SerialNumber
DB_ReportName_Tare=[[\TareReport.csv]]dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblTare(tareIndex INTEGER, presetTare DOUBLE, units VARCHAR)")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblTareLastChan (Channel INTEGER)")dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblTareConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:close()end
function tare.tareDBStore()local dbFile,result,index,searchIndex,sqlStr,row
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblTare(tareIndex INTEGER, presetTare DOUBLE, units VARCHAR)")dbFile:execute("BEGIN TRANSACTION")for index=1,MAX_TARE_INDEX do
searchIndex=taretable[index].tareIndex
sqlStr=string.format("SELECT tblTare.tareIndex FROM tblTare WHERE tblTare.tareIndex = '%d'",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblTare(tareIndex, presetTare, units) VALUES ('%d', '%f', '%s')",taretable[index].tareIndex,taretable[index].presetTare,taretable[index].units)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblTare SET tareIndex = '%d', presetTare = '%f', units = '%s' WHERE tblTare.tareIndex = '%d'",taretable[index].tareIndex,taretable[index].presetTare,taretable[index].units,searchIndex)result=dbFile:exec(sqlStr)end
end
dbFile:execute("COMMIT")dbFile:close()tare.lastTareChannStore()end
function tare.lastTareChannStore()local dbFile,result,sqlStr,row
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblTareLastChan (Channel INTEGER)")dbFile:execute("BEGIN TRANSACTION")sqlStr=string.format("SELECT tblTareLastChan.Channel FROM tblTareLastChan")found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblTareLastChan (Channel) VALUES ('%d')",newTareChannel)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblTareLastChan SET Channel = '%d'",newTareChannel)result=dbFile:exec(sqlStr)end
tare.setTarePrintTokens()dbFile:execute("COMMIT")dbFile:close()end
function tare.extraStuffStore()local dbFile,result,sqlStr,row
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblTareConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")found=false
for row in dbFile:rows("SELECT varID FROM tblTareConfig WHERE varID = 'KeyTare301Enable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblTareConfig (varID, value) VALUES ('KeyTare301Enable', '%d')",KeyTare301Enable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblTareConfig SET value = '%d' WHERE varID = 'KeyTare301Enable'",KeyTare301Enable)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblTareConfig WHERE varID = 'PresetTare301Enable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblTareConfig (varID, value) VALUES ('PresetTare301Enable', '%d')",PresetTare301Enable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblTareConfig SET value = '%d' WHERE varID = 'PresetTare301Enable'",PresetTare301Enable)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblTareConfig WHERE varID = 'KeyTare303Enable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblTareConfig (varID, value) VALUES ('KeyTare303Enable', '%d')",KeyTare303Enable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblTareConfig SET value = '%d' WHERE varID = 'KeyTare303Enable'",KeyTare303Enable)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblTareConfig WHERE varID = 'PresetTare303Enable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblTareConfig (varID, value) VALUES ('PresetTare303Enable', '%d')",PresetTare303Enable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblTareConfig SET value = '%d' WHERE varID = 'PresetTare303Enable'",PresetTare303Enable)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblTareConfig WHERE varID = 'KeyTare375Enable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblTareConfig (varID, value) VALUES ('KeyTare375Enable', '%d')",KeyTare375Enable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblTareConfig SET value = '%d' WHERE varID = 'KeyTare375Enable'",KeyTare375Enable)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblTareConfig WHERE varID = 'PresetTare375Enable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblTareConfig (varID, value) VALUES ('PresetTare375Enable', '%d')",PresetTare375Enable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblTareConfig SET value = '%d' WHERE varID = 'PresetTare375Enable'",PresetTare375Enable)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblTareConfig WHERE varID = 'KeyTare305Enable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblTareConfig (varID, value) VALUES ('KeyTare305Enable', '%d')",KeyTare305Enable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblTareConfig SET value = '%d' WHERE varID = 'KeyTare305Enable'",KeyTare305Enable)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblTareConfig WHERE varID = 'PresetTare305Enable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblTareConfig (varID, value) VALUES ('PresetTare305Enable', '%d')",PresetTare305Enable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblTareConfig SET value = '%d' WHERE varID = 'PresetTare305Enable'",PresetTare305Enable)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblTareConfig WHERE varID = 'KeyTare305GTNEnable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblTareConfig (varID, value) VALUES ('KeyTare305GTNEnable', '%d')",KeyTare305GTNEnable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblTareConfig SET value = '%d' WHERE varID = 'KeyTare305GTNEnable'",KeyTare305GTNEnable)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblTareConfig WHERE varID = 'PresetTare305GTNEnable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblTareConfig (varID, value) VALUES ('PresetTare305GTNEnable', '%d')",PresetTare305GTNEnable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblTareConfig SET value = '%d' WHERE varID = 'PresetTare305GTNEnable'",PresetTare305GTNEnable)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()end
function tare.tareDBRecall()local dbFile,result,index,row
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblTare (tareIndex INTEGER, presetTare DOUBLE, units VARCHAR)")for row in dbFile:rows("SELECT tareIndex, presetTare, units FROM tblTare")do
index=row[1]if index<=MAX_TARE_INDEX then
taretable[index].tareIndex=row[1]taretable[index].presetTare=row[2]taretable[index].units=row[3]end
end
dbFile:close()end
function tare.lastTareChannRecall()local dbFile,result,sqlStr,row
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblTareLastChan (Channel INTEGER)")sqlStr=string.format("SELECT Channel FROM tblTareLastChan")for row in dbFile:rows(sqlStr)do
newTareChannel=row[1]end
tare.setTarePrintTokens()dbFile:close()end
function tare.extraStuffRecall()local dbFile,result,sqlStr,row
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblTareConfig (varID TEXT PRIMARY KEY, value TEXT)")found=false
for row in dbFile:rows("SELECT varID, value FROM tblTareConfig WHERE varID = 'KeyTare301Enable'")do
found=true
KeyTare301Enable=tonumber(row[2])end
if found==false then
KeyTare301Enable=DefaultKeyTare301Enable
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblTareConfig WHERE varID = 'PresetTare301Enable'")do
found=true
PresetTare301Enable=tonumber(row[2])end
if found==false then
PresetTare301Enable=DefaultPresetTare301Enable
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblTareConfig WHERE varID = 'KeyTare303Enable'")do
found=true
KeyTare303Enable=tonumber(row[2])end
if found==false then
KeyTare303Enable=DefaultKeyTare303Enable
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblTareConfig WHERE varID = 'PresetTare303Enable'")do
found=true
PresetTare303Enable=tonumber(row[2])end
if found==false then
PresetTare303Enable=DefaultPresetTare303Enable
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblTareConfig WHERE varID = 'KeyTare375Enable'")do
found=true
KeyTare375Enable=tonumber(row[2])end
if found==false then
KeyTare375Enable=DefaultKeyTare375Enable
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblTareConfig WHERE varID = 'PresetTare375Enable'")do
found=true
PresetTare375Enable=tonumber(row[2])end
if found==false then
PresetTare375Enable=DefaultPresetTare375Enable
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblTareConfig WHERE varID = 'KeyTare305Enable'")do
found=true
KeyTare305Enable=tonumber(row[2])end
if found==false then
KeyTare305Enable=DefaultKeyTare305Enable
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblTareConfig WHERE varID = 'PresetTare305Enable'")do
found=true
PresetTare305Enable=tonumber(row[2])end
if found==false then
PresetTare305Enable=DefaultPresetTare305Enable
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblTareConfig WHERE varID = 'KeyTare305GTNEnable'")do
found=true
KeyTare305GTNEnable=tonumber(row[2])end
if found==false then
KeyTare305GTNEnable=DefaultKeyTare305GTNEnable
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblTareConfig WHERE varID = 'PresetTare305GTNEnable'")do
found=true
PresetTare305GTNEnable=tonumber(row[2])end
if found==false then
PresetTare305GTNEnable=DefaultPresetTare305GTNEnable
end
dbFile:close()end
function tare.tareDBClear()tare.tareInit()tare.tareDBStore()tare.tareDBRecall()end
function tare.editPresetTareOriginal(index,label)local newTare,isEnterKey
if index<=MAX_TARE_INDEX then
local minTare,maxTare
tare.tareDBRecall()wt=awtx.weight.getCurrent(1)minTare=0
maxTare=wt.curCapacity
newTare=taretable[index].presetTare
newTare,isEnterKey=awtx.keypad.enterFloat(newTare,minTare,maxTare,separatorChar)awtx.display.writeLine(label)if isEnterKey then
taretable[index].presetTare=newTare
tare.tareDBStore()else
end
else
end
end
function tare.editPresetTare(index,label)local newTare,isEnterKey
if index<=MAX_TARE_INDEX then
local minTare,maxTare,tareWtUnit,errorState
tare.tareDBRecall()wt=awtx.weight.getCurrent(1)minTare=0
maxTare=wt.curCapacity
errorState,tareWtUnit=awtx.weight.unitStrToUnitIndex(taretable[index].units)if errorState==0 then
newTare=awtx.weight.convertWeight(tareWtUnit,taretable[index].presetTare,wt.units,1)else
newTare=taretable[index].presetTare
end
newTare,isEnterKey=awtx.keypad.enterWeightWithUnits(newTare,minTare,maxTare,wt.unitsStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
taretable[index].presetTare=newTare
taretable[index].units=wt.unitsStr
tare.tareDBStore()else
end
else
end
end
function tare.editPresetTare1(label)supervisor.menuing=false
tare.editPresetTare(1,label)supervisor.menuing=true
end
function tare.editPresetTare2(label)supervisor.menuing=false
tare.editPresetTare(2,label)supervisor.menuing=true
end
function tare.editPresetTare3(label)supervisor.menuing=false
tare.editPresetTare(3,label)supervisor.menuing=true
end
function tare.editPresetTare4(label)supervisor.menuing=false
tare.editPresetTare(4,label)supervisor.menuing=true
end
function tare.editPresetTare5(label)supervisor.menuing=false
tare.editPresetTare(5,label)supervisor.menuing=true
end
function tare.editPresetTare6(label)supervisor.menuing=false
tare.editPresetTare(6,label)supervisor.menuing=true
end
function tare.editPresetTare7(label)supervisor.menuing=false
tare.editPresetTare(7,label)supervisor.menuing=true
end
function tare.editPresetTare8(label)supervisor.menuing=false
tare.editPresetTare(8,label)supervisor.menuing=true
end
function tare.editPresetTare9(label)supervisor.menuing=false
tare.editPresetTare(9,label)supervisor.menuing=true
end
function tare.editPresetTare10(label)supervisor.menuing=false
tare.editPresetTare(10,label)supervisor.menuing=true
end
function tare.TareDBReport(label)local usermode,currentRPN,fho,err
local index,isEnterKey
local dbFile
local row
local t={}currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)index,isEnterKey=awtx.keypad.selectList(" Port 1, Port 2, USB",0)awtx.display.writeLine(label)if isEnterKey then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(DB_FileLocation_AppData)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblTare (tareIndex INTEGER, presetTare DOUBLE, units VARCHAR)")if index==0 or index==1 then
t[#t+1]=string.format("tareIndex                       presetTare \r\n")elseif index==2 then
t[#t+1]=string.format("tareIndex, presetTare, units \r\n")end
for row in dbFile:rows("SELECT tareIndex, presetTare, units FROM tblTare ORDER BY rowid DESC")do
if index==0 or index==1 then
t[#t+1]=string.format("%30d %10f %10s \r\n",row[1],row[2],row[3])elseif index==2 then
t[#t+1]=string.format("%d, %f, %s \r\n",row[1],row[2],row[3])else
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
fho,err=io.open(string.format("%s%s",DB_FileLocation_Reports,DB_ReportName_Tare),"w")if err then
awtx.display.writeLine("err "..err)awtx.os.sleep(canttime)awtx.os.sleep(canttime)else
fho:write(table.concat(t))fho:close()end
else
awtx.display.writeLine("err "..tostring(result))end
else
end
awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function tare.TareDBReset(label)local usermode,isEnterKey
local index
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)tare.tareDBClear()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
end
end
function tare.setTareChannelOriginal(channel)newTareChannel=channel
tare.lastTareChannStore()tare.tareDBRecall()wt=awtx.weight.getCurrent(1)if wt.curUnitsFactor==0 then
wt.curUnitsFactor=1.0
end
newtare=taretable[newTareChannel].presetTare*wt.curUnitsFactor
awtx.weight.requestPresetTare(newtare)end
function tare.setTareChannel(channel)local errorState,tareWtUnit,newtare
newTareChannel=channel
tare.lastTareChannStore()tare.tareDBRecall()wt=awtx.weight.getCurrent(1)newtare=taretable[newTareChannel].presetTare
if newtare~=nil then
errorState,tareWtUnit=awtx.weight.unitStrToUnitIndex(taretable[newTareChannel].units)if errorState==0 then
if tareWtUnit~=wt.units then
newtare=awtx.weight.convertWeight(tareWtUnit,newtare,wt.units,1)end
end
awtx.weight.requestPresetTare(newtare)end
end
function tare.editKeyTare301Enable(label)local newKeyTare301Enable,isEnterKey
tare.extraStuffRecall()newKeyTare301Enable=KeyTare301Enable
newKeyTare301Enable,isEnterKey=awtx.keypad.selectList("No,Yes",newKeyTare301Enable)awtx.display.writeLine(label)if isEnterKey then
KeyTare301Enable=newKeyTare301Enable
tare.extraStuffStore()else
end
end
function tare.editPresetTare301Enable(label)local newPresetTare301Enable,isEnterKey
tare.extraStuffRecall()newPresetTare301Enable=PresetTare301Enable
newPresetTare301Enable,isEnterKey=awtx.keypad.selectList("No,Yes",newPresetTare301Enable)awtx.display.writeLine(label)if isEnterKey then
PresetTare301Enable=newPresetTare301Enable
tare.extraStuffStore()else
end
end
function tare.editKeyTare303Enable(label)local newKeyTare303Enable,isEnterKey
tare.extraStuffRecall()newKeyTare303Enable=KeyTare303Enable
newKeyTare303Enable,isEnterKey=awtx.keypad.selectList("No,Yes",newKeyTare303Enable)awtx.display.writeLine(label)if isEnterKey then
KeyTare303Enable=newKeyTare303Enable
tare.extraStuffStore()else
end
end
function tare.editPresetTare303Enable(label)local newPresetTare303Enable,isEnterKey
tare.extraStuffRecall()newPresetTare303Enable=PresetTare303Enable
newPresetTare303Enable,isEnterKey=awtx.keypad.selectList("No,Yes",newPresetTare303Enable)awtx.display.writeLine(label)if isEnterKey then
PresetTare303Enable=newPresetTare303Enable
tare.extraStuffStore()else
end
end
function tare.editKeyTare375Enable(label)local newKeyTare375Enable,isEnterKey
tare.extraStuffRecall()newKeyTare375Enable=KeyTare375Enable
newKeyTare375Enable,isEnterKey=awtx.keypad.selectList("No,Yes",newKeyTare375Enable)awtx.display.writeLine(label)if isEnterKey then
KeyTare375Enable=newKeyTare375Enable
tare.extraStuffStore()else
end
end
function tare.editPresetTare375Enable(label)local newPresetTare375Enable,isEnterKey
tare.extraStuffRecall()newPresetTare375Enable=PresetTare375Enable
newPresetTare375Enable,isEnterKey=awtx.keypad.selectList("No,Yes",newPresetTare375Enable)awtx.display.writeLine(label)if isEnterKey then
PresetTare375Enable=newPresetTare375Enable
tare.extraStuffStore()else
end
end
function tare.editKeyTare305Enable(label)local newKeyTare305Enable,isEnterKey
tare.extraStuffRecall()newKeyTare305Enable=KeyTare305Enable
newKeyTare305Enable,isEnterKey=awtx.keypad.selectList("No,Yes",newKeyTare305Enable)awtx.display.writeLine(label)if isEnterKey then
KeyTare305Enable=newKeyTare305Enable
tare.extraStuffStore()else
end
end
function tare.editPresetTare305Enable(label)local newPresetTare305Enable,isEnterKey
tare.extraStuffRecall()newPresetTare305Enable=PresetTare305Enable
newPresetTare305Enable,isEnterKey=awtx.keypad.selectList("No,Yes",newPresetTare305Enable)awtx.display.writeLine(label)if isEnterKey then
PresetTare305Enable=newPresetTare305Enable
tare.extraStuffStore()else
end
end
function tare.editKeyTare305GTNEnable(label)local newKeyTare305GTNEnable,isEnterKey
tare.extraStuffRecall()newKeyTare305GTNEnable=KeyTare305GTNEnable
newKeyTare305GTNEnable,isEnterKey=awtx.keypad.selectList("No,Yes",newKeyTare305GTNEnable)awtx.display.writeLine(label)if isEnterKey then
KeyTare305GTNEnable=newKeyTare305GTNEnable
tare.extraStuffStore()else
end
end
function tare.editPresetTare305GTNEnable(label)local newPresetTare305GTNEnable,isEnterKey
tare.extraStuffRecall()newPresetTare305GTNEnable=PresetTare305GTNEnable
newPresetTare305GTNEnable,isEnterKey=awtx.keypad.selectList("No,Yes",newPresetTare305GTNEnable)awtx.display.writeLine(label)if isEnterKey then
PresetTare305GTNEnable=newPresetTare305GTNEnable
tare.extraStuffStore()else
end
end
tare.tareInit()tare.tareDBInit()tare.tareDBRecall()tare.extraStuffRecall()tare.lastTareChannRecall()if config.presetTareFlag then
if newTareChannel~=nil and newTareChannel<=MAX_TARE_INDEX then
tare.setTareChannel(newTareChannel)end
tare.setTarePrintTokens()else
newTareChannel=newTareChannelDefault
tare.setTarePrintTokens()end
function getPresetTare()local usermode,isEnterKey
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)newTareChannel,isEnterKey=awtx.keypad.enterInteger(newTareChannel,1,MAX_TARE_INDEX)awtx.display.setMode(usermode)if isEnterKey then
tare.setTareChannel(newTareChannel)else
end
end
function get_PresetTareEnabled()if config.presetTareFlag then
return 1
else
return 0
end
end
function getTareClear()awtx.weight.requestTareClear()end
function getPbTare()awtx.weight.requestTare()end
function getKeyboardTare()local usermode,newTare,isEnterKey
local minTare,maxTare
wt=awtx.weight.getCurrent(1)minTare=0
maxTare=wt.curCapacity
wt=awtx.weight.getCurrent(1)newTare=wt.tare
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)newTare,isEnterKey=awtx.keypad.enterFloat(newTare,minTare,maxTare,separatorChar)awtx.display.setMode(usermode)if isEnterKey then
awtx.weight.requestKeyboardTare(newTare)else
end
end
function checkTareEnable()local usermode,curActVal
system=awtx.hardware.getSystem(1)config=awtx.weight.getConfig(1)wt=awtx.weight.getCurrent(1)curActVal=awtx.weight.getActiveValue()if system.modelStr=="ZM301"then
if(config.presetTareFlag or PresetTare301Enable~=0)then
getPresetTare()elseif config.pbTareFlag then
getPbTare()else
displayCANT()end
elseif system.modelStr=="ZM303"then
if(config.presetTareFlag or PresetTare303Enable~=0)then
getPresetTare()elseif config.pbTareFlag then
getPbTare()else
displayCANT()end
elseif system.modelStr=="ZQ375"then
if(config.presetTareFlag or PresetTare375Enable~=0)then
getPresetTare()elseif config.pbTareFlag then
getPbTare()else
displayCANT()end
elseif system.modelStr=="ZM305"then
if(config.presetTareFlag or PresetTare305Enable~=0)then
getPresetTare()elseif config.pbTareFlag then
getPbTare()else
displayCANT()end
elseif system.modelStr=="ZM305GTN"then
if(config.presetTareFlag or PresetTare305GTNEnable~=0)then
getPresetTare()elseif config.pbTareFlag then
getPbTare()else
displayCANT()end
else
if config.presetTareFlag then
getPresetTare()elseif config.pbTareFlag then
getPbTare()else
displayCANT()end
end
end