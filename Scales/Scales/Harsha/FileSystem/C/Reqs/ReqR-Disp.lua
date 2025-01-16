rdisp={}rdisp.RDispEnableDefault=0
rdisp.RDispDataFormatDefault=0
rdisp.RDispBindFwdDefault=0
rdisp.RDispP176Default=2
rdisp.RDispP177Default=0
rdisp.RDispP61200Default=0
rdisp.RDispP61201Default=65535
rdisp.RDispP61202Default=0
rdisp.RDispP61203Default=65535
rdisp.RDispP61204Default=0
rdisp.RDispP61205Default=65535
rdisp.RDispEnable=rdisp.RDispEnableDefault
rdisp.RDispDataFormat=rdisp.RDispDataFormatDefault
rdisp.RDispBindFwd=rdisp.RDispBindFwdDefault
rdisp.RDispP176=rdisp.RDispP176Default
rdisp.RDispP177=rdisp.RDispP177Default
rdisp.RDispP61200=rdisp.RDispP61200Default
rdisp.RDispP61201=rdisp.RDispP61201Default
rdisp.RDispP61202=rdisp.RDispP61202Default
rdisp.RDispP61203=rdisp.RDispP61203Default
rdisp.RDispP61204=rdisp.RDispP61204Default
rdisp.RDispP61205=rdisp.RDispP61205Default
rdisp.RDispPort=awtx.keypad.getRDport()function rdisp.disableSettings()awtx.keypad.setRDstate(0)end
function rdisp.updateSettings()local usermode
rdisp.RDispPort=awtx.keypad.getRDport()if rdisp.RDispEnable==1 then
appDisableSetpoints()awtx.keypad.setRDanalog(rdisp.RDispP176,rdisp.RDispP177,rdisp.RDispP61200,rdisp.RDispP61201,rdisp.RDispP61202,rdisp.RDispP61203,rdisp.RDispP61204,rdisp.RDispP61205)awtx.keypad.setRDstate(rdisp.RDispEnable)awtx.keypad.setRDformat(rdisp.RDispDataFormat)awtx.keypad.setRDbindFwd(rdisp.RDispBindFwd)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("       ")awtx.display.setSegment(0,0)else
appEnableSetpoints()awtx.keypad.setRDstate(rdisp.RDispEnable)awtx.keypad.setRDformat(rdisp.RDispDataFormat)awtx.keypad.setRDbindFwd(rdisp.RDispBindFwd)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.display.writeLine("       ")awtx.display.setSegment(0,0)end
end
local DB_FileLocation_AppConfig
local DB_FileLocation_AppData
local DB_FileLocation_Reports
local DB_ReportName_RDisp
function rdisp.batteryDBInit()local simAppPath,dbFile,result
if awtx.os.amISimulator()then
simAppPath=awtx.os.applicationPath()DB_FileLocation_AppConfig=simAppPath..[[\AppConfig.db]]DB_FileLocation_AppData=simAppPath..[[\AppData.db]]DB_FileLocation_Reports=simAppPath..[[\Reports]]..SerialNumber
else
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]DB_FileLocation_AppData=[[C:\Database\AppData.db]]DB_FileLocation_Reports=[[G:\Reports]]..SerialNumber
end
DB_ReportName_RDisp=[[\RDispReport.csv]]dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblRDispConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:close()end
function rdisp.extraStuffStore()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblRDispConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")found=false
for row in dbFile:rows("SELECT varID FROM tblRDispConfig WHERE varID = 'RDispEnable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblRDispConfig (varID, value) VALUES ('RDispEnable', '%d')",rdisp.RDispEnable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblRDispConfig SET value = '%d' WHERE varID = 'RDispEnable'",rdisp.RDispEnable)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblRDispConfig WHERE varID = 'RDispDataFormat'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblRDispConfig (varID, value) VALUES ('RDispDataFormat', '%d')",rdisp.RDispDataFormat)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblRDispConfig SET value = '%d' WHERE varID = 'RDispDataFormat'",rdisp.RDispDataFormat)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblRDispConfig WHERE varID = 'RDispBindFwd'")do
found=true
end
if found==false then
local strInsert=string.format("INSERT INTO tblRDispConfig (varID, value) VALUES ('RDispBindFwd', '%d')",rdisp.RDispBindFwd)result=dbFile:exec(strInsert)else
local strUpdate=string.format("UPDATE tblRDispConfig SET value = '%d' WHERE varID = 'RDispBindFwd'",rdisp.RDispBindFwd)result=dbFile:exec(strUpdate)end
found=false
for row in dbFile:rows("SELECT varID FROM tblRDispConfig WHERE varID = 'RDispP176'")do
found=true
end
if found==false then
local strInsert=string.format("INSERT INTO tblRDispConfig (varID, value) VALUES ('RDispP176', '%d')",rdisp.RDispP176)result=dbFile:exec(strInsert)else
local strUpdate=string.format("UPDATE tblRDispConfig SET value = '%d' WHERE varID = 'RDispP176'",rdisp.RDispP176)result=dbFile:exec(strUpdate)end
found=false
for row in dbFile:rows("SELECT varID FROM tblRDispConfig WHERE varID = 'RDispP177'")do
found=true
end
if found==false then
local strInsert=string.format("INSERT INTO tblRDispConfig (varID, value) VALUES ('RDispP177', '%d')",rdisp.RDispP177)result=dbFile:exec(strInsert)else
local strUpdate=string.format("UPDATE tblRDispConfig SET value = '%d' WHERE varID = 'RDispP177'",rdisp.RDispP177)result=dbFile:exec(strUpdate)end
found=false
for row in dbFile:rows("SELECT varID FROM tblRDispConfig WHERE varID = 'RDispP61200'")do
found=true
end
if found==false then
local strInsert=string.format("INSERT INTO tblRDispConfig (varID, value) VALUES ('RDispP61200', '%d')",rdisp.RDispP61200)result=dbFile:exec(strInsert)else
local strUpdate=string.format("UPDATE tblRDispConfig SET value = '%d' WHERE varID = 'RDispP61200'",rdisp.RDispP61200)result=dbFile:exec(strUpdate)end
found=false
for row in dbFile:rows("SELECT varID FROM tblRDispConfig WHERE varID = 'RDispP61201'")do
found=true
end
if found==false then
local strInsert=string.format("INSERT INTO tblRDispConfig (varID, value) VALUES ('RDispP61201', '%d')",rdisp.RDispP61201)result=dbFile:exec(strInsert)else
local strUpdate=string.format("UPDATE tblRDispConfig SET value = '%d' WHERE varID = 'RDispP61201'",rdisp.RDispP61201)result=dbFile:exec(strUpdate)end
found=false
for row in dbFile:rows("SELECT varID FROM tblRDispConfig WHERE varID = 'RDispP61202'")do
found=true
end
if found==false then
local strInsert=string.format("INSERT INTO tblRDispConfig (varID, value) VALUES ('RDispP61202', '%d')",rdisp.RDispP61202)result=dbFile:exec(strInsert)else
local strUpdate=string.format("UPDATE tblRDispConfig SET value = '%d' WHERE varID = 'RDispP61202'",rdisp.RDispP61202)result=dbFile:exec(strUpdate)end
found=false
for row in dbFile:rows("SELECT varID FROM tblRDispConfig WHERE varID = 'RDispP61203'")do
found=true
end
if found==false then
local strInsert=string.format("INSERT INTO tblRDispConfig (varID, value) VALUES ('RDispP61203', '%d')",rdisp.RDispP61203)result=dbFile:exec(strInsert)else
local strUpdate=string.format("UPDATE tblRDispConfig SET value = '%d' WHERE varID = 'RDispP61203'",rdisp.RDispP61203)result=dbFile:exec(strUpdate)end
found=false
for row in dbFile:rows("SELECT varID FROM tblRDispConfig WHERE varID = 'RDispP61204'")do
found=true
end
if found==false then
local strInsert=string.format("INSERT INTO tblRDispConfig (varID, value) VALUES ('RDispP61204', '%d')",rdisp.RDispP61204)result=dbFile:exec(strInsert)else
local strUpdate=string.format("UPDATE tblRDispConfig SET value = '%d' WHERE varID = 'RDispP61204'",rdisp.RDispP61204)result=dbFile:exec(strUpdate)end
found=false
for row in dbFile:rows("SELECT varID FROM tblRDispConfig WHERE varID = 'RDispP61205'")do
found=true
end
if found==false then
local strInsert=string.format("INSERT INTO tblRDispConfig (varID, value) VALUES ('RDispP61205', '%d')",rdisp.RDispP61205)result=dbFile:exec(strInsert)else
local strUpdate=string.format("UPDATE tblRDispConfig SET value = '%d' WHERE varID = 'RDispP61205'",rdisp.RDispP61205)result=dbFile:exec(strUpdate)end
dbFile:execute("COMMIT")dbFile:close()end
function rdisp.extraStuffRecall()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblRDispConfig (varID TEXT PRIMARY KEY, value TEXT)")found=false
for row in dbFile:rows("SELECT varID, value FROM tblRDispConfig WHERE varID = 'RDispEnable'")do
found=true
rdisp.RDispEnable=tonumber(row[2])end
if found==false then
rdisp.RDispEnable=rdisp.RDispEnableDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblRDispConfig WHERE varID = 'RDispDataFormat'")do
found=true
rdisp.RDispDataFormat=tonumber(row[2])end
if found==false then
rdisp.RDispDataFormat=rdisp.RDispDataFormatDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblRDispConfig WHERE varID = 'RDispBindFwd'")do
found=true
rdisp.RDispBindFwd=tonumber(row[2])end
if found==false then
rdisp.RDispBindFwd=rdisp.RDispBindFwdDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblRDispConfig WHERE varID = 'RDispP176'")do
found=true
rdisp.RDispP176=tonumber(row[2])end
if found==false then
rdisp.RDispP176=rdisp.RDispP176Default
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblRDispConfig WHERE varID = 'RDispP177'")do
found=true
rdisp.RDispP177=tonumber(row[2])end
if found==false then
rdisp.RDispP177=rdisp.RDispP177Default
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblRDispConfig WHERE varID = 'RDispP61200'")do
found=true
rdisp.RDispP61200=tonumber(row[2])end
if found==false then
rdisp.RDispP61200=rdisp.RDispP61200Default
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblRDispConfig WHERE varID = 'RDispP61201'")do
found=true
rdisp.RDispP61201=tonumber(row[2])end
if found==false then
rdisp.RDispP61201=rdisp.RDispP61201Default
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblRDispConfig WHERE varID = 'RDispP61202'")do
found=true
rdisp.RDispP61202=tonumber(row[2])end
if found==false then
rdisp.RDispP61202=rdisp.RDispP61202Default
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblRDispConfig WHERE varID = 'RDispP61203'")do
found=true
rdisp.RDispP61203=tonumber(row[2])end
if found==false then
rdisp.RDispP61203=rdisp.RDispP61203Default
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblRDispConfig WHERE varID = 'RDispP61204'")do
found=true
rdisp.RDispP61204=tonumber(row[2])end
if found==false then
rdisp.RDispP61204=rdisp.RDispP61204Default
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblRDispConfig WHERE varID = 'RDispP61205'")do
found=true
rdisp.RDispP61205=tonumber(row[2])end
if found==false then
rdisp.RDispP61205=rdisp.RDispP61205Default
end
dbFile:close()end
function rdisp.getRDispAnalog()local IsUpdated
if rdisp.RDispEnable==1 and rdisp.RDispDataFormat==0 then
IsUpdated=awtx.keypad.getRDanalog_isupdated()if IsUpdated==1 then
rdisp.RDispP176=awtx.keypad.getRDanalog_p176()rdisp.RDispP177=awtx.keypad.getRDanalog_p177()rdisp.RDispP61200=awtx.keypad.getRDanalog_p61200()rdisp.RDispP61201=awtx.keypad.getRDanalog_p61201()rdisp.RDispP61202=awtx.keypad.getRDanalog_p61202()rdisp.RDispP61203=awtx.keypad.getRDanalog_p61203()rdisp.RDispP61204=awtx.keypad.getRDanalog_p61204()rdisp.RDispP61205=awtx.keypad.getRDanalog_p61205()rdisp.extraStuffStore()end
end
end
function rdisp.editRDispEnable(label)local newRDispEnable,isEnterKey
rdisp.extraStuffRecall()newRDispEnable=rdisp.RDispEnable
newRDispEnable,isEnterKey=awtx.keypad.selectList("Off, On",newRDispEnable)awtx.display.writeLine(label)if isEnterKey then
rdisp.RDispEnable=newRDispEnable
rdisp.extraStuffStore()end
end
function rdisp.editRDispDataFormat(label)local newRDispDataFormat,isEnterKey
rdisp.extraStuffRecall()newRDispDataFormat=rdisp.RDispDataFormat
newRDispDataFormat,isEnterKey=awtx.keypad.selectList("350iS,RD4000",newRDispDataFormat)awtx.display.writeLine(label)if isEnterKey then
rdisp.RDispDataFormat=newRDispDataFormat
rdisp.extraStuffStore()else
end
end
function rdisp.editRDispBindFwd(label)local newRDispBindFwd,isEnterKey
rdisp.extraStuffRecall()newRDispBindFwd=rdisp.RDispBindFwd
newRDispBindFwd,isEnterKey=awtx.keypad.selectList("Off,Port 1,Port 2,Enet 1,Enet 2,Enet 3,Enet 4,Enet 5",newRDispBindFwd)awtx.display.writeLine(label)if isEnterKey then
rdisp.RDispBindFwd=newRDispBindFwd
rdisp.extraStuffStore()else
end
end
rdisp.batteryDBInit()rdisp.extraStuffRecall()