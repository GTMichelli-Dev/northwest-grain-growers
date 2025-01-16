battery={}battery.BatteryEnableDefault=0
battery.BatteryTimeOutDefault=60
awtx.hardware.setShutdownEnable(0)battery.BatteryEnable=battery.BatteryEnableDefault
battery.BatteryTimeOut=battery.BatteryTimeOutDefault
local DB_FileLocation_AppConfig
local DB_FileLocation_AppData
local DB_FileLocation_Reports
local DB_ReportName_Battery
function battery.batteryDBInit()local simAppPath,dbFile,result
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]DB_FileLocation_AppData=[[C:\Database\AppData.db]]DB_FileLocation_Reports=[[G:\Reports]]..SerialNumber
DB_ReportName_Battery=[[\BatteryReport.csv]]dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatteryConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:close()end
function battery.extraStuffStore()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatteryConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")found=false
for row in dbFile:rows("SELECT varID FROM tblBatteryConfig WHERE varID = 'BatteryEnable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblBatteryConfig (varID, value) VALUES ('BatteryEnable', '%d')",battery.BatteryEnable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblBatteryConfig SET value = '%d' WHERE varID = 'BatteryEnable'",battery.BatteryEnable)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblBatteryConfig WHERE varID = 'BatteryTimeOut'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblBatteryConfig (varID, value) VALUES ('BatteryTimeOut', '%d')",battery.BatteryTimeOut)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblBatteryConfig SET value = '%d' WHERE varID = 'BatteryTimeOut'",battery.BatteryTimeOut)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()awtx.hardware.setShutdownEnable(battery.BatteryEnable)awtx.hardware.setInactivityShutdownTime(battery.BatteryTimeOut)end
function battery.extraStuffRecall()local dbFile,result
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatteryConfig (varID TEXT PRIMARY KEY, value TEXT)")found=false
for row in dbFile:rows("SELECT varID, value FROM tblBatteryConfig WHERE varID = 'BatteryEnable'")do
found=true
battery.BatteryEnable=tonumber(row[2])end
if found==false then
battery.BatteryEnable=battery.BatteryEnableDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblBatteryConfig WHERE varID = 'BatteryTimeOut'")do
found=true
battery.BatteryTimeOut=tonumber(row[2])end
if found==false then
battery.BatteryTimeOut=battery.BatteryTimeOutDefault
end
dbFile:close()awtx.hardware.setShutdownEnable(battery.BatteryEnable)awtx.hardware.setInactivityShutdownTime(battery.BatteryTimeOut)end
function battery.editBatteryEnable(label)local newBatteryEnable,isEnterKey
battery.BatteryEnable=awtx.hardware.getShutdownEnable()newBatteryEnable=battery.BatteryEnable
newBatteryEnable,isEnterKey=awtx.keypad.selectList("Off, On",newBatteryEnable)awtx.display.writeLine(label)if isEnterKey then
battery.BatteryEnable=newBatteryEnable
battery.extraStuffStore()else
end
appRefreshBatterySettings()end
function battery.editBatterySetting(label)local newBatteryTimeOut,newBatteryTimeOutMin,newBatteryTimeOutMax,isEnterKey
newBatteryTimeOutMin=1
newBatteryTimeOutMax=1440
battery.BatteryTimeOut=awtx.hardware.getInactivityShutdownTime()newBatteryTimeOut=battery.BatteryTimeOut
newBatteryTimeOut,isEnterKey=awtx.keypad.enterInteger(newBatteryTimeOut,newBatteryTimeOutMin,newBatteryTimeOutMax)awtx.display.writeLine(label)if isEnterKey then
battery.BatteryTimeOut=newBatteryTimeOut
battery.extraStuffStore()else
end
end
battery.batteryDBInit()battery.extraStuffRecall()function battery.viewBatteryVoltage()local newBatteryVolts
newBatteryVolts=awtx.hardware.getSupplyVolts()awtx.display.writeLine(string.format("%5.2f V",newBatteryVolts))end
function battery.printBattery()local newBatteryVolts,noBarVolts,oneBarVolts,twoBarVolts,threeBarVolts,mainsVolts
noBarVolts,oneBarVolts,twoBarVolts,threeBarVolts,mainsVolts=awtx.hardware.getBatteryLevels()newBatteryVolts=awtx.hardware.getSupplyVolts()awtx.display.writeLine(string.format("%5.2f V",newBatteryVolts))end
function battery.showBatteryVoltage()local usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("BAT VOLT")awtx.os.sleep(showtime1)battery.printBattery()awtx.os.sleep(showtime2)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function battery.ShutHerDown()currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("--OFF--")awtx.os.sleep(displaytime*2)result=awtx.setPoint.outputSet(3)awtx.os.sleep(1000)result=awtx.setPoint.outputClr(3)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function SHUTDOWNEVENT()local usermode,currentRPN
local newBatteryVolts,noBarVolts,oneBarVolts,twoBarVolts,threeBarVolts,mainsVolts
noBarVolts,oneBarVolts,twoBarVolts,threeBarVolts,mainsVolts=awtx.hardware.getBatteryLevels()newBatteryVolts=awtx.hardware.getSupplyVolts()if system.modelStr=="ZQ375"then
if(newBatteryVolts<mainsVolts)then
battery.ShutHerDown()end
else
battery.ShutHerDown()end
end
awtx.hardware.registerShutdownEvent(SHUTDOWNEVENT)