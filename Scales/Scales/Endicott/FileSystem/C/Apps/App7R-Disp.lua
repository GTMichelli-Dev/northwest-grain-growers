--[[
--## VERSION=0.1.0
--## DATE=2012-02-17  12:00
--## DESC=This is the Remote Display App (App7R-Disp.lua)

]]
require("ReqDebug")AppName="R-DISP"SerialNumber=awtx.setupMenu.getSerialNumber()AWTX_LUA_UNDEFINED=0
AWTX_LUA_WEIGHT=1
AWTX_LUA_STRING=2
AWTX_LUA_DATE=3
AWTX_LUA_TIME=4
AWTX_LUA_FLOAT=5
AWTX_LUA_INTEGER=6
awtx.keypad.setEditorOptions(-1)canttime=500
wordtime=1000
entertime=5000
showtime1=1000
showtime2=1500
busytime=500
displaytime=500
gradtime=250
batchtime=500
newscaleNumber=1
HowManyRepeatsMakeAHold=3
DISPLAY_MODE_SCALE_OBJECT=0
DISPLAY_MODE_USER_SCRIPT=1
DISPLAY_MODE_USER_MENU=2
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
SITE_US=0
SITE_GB=1
SITE_CAN=2
SITE_EU=3
SITE_CHINA=4
SITE_INDIA=5
function calcAfterUnitsChange()system=awtx.hardware.getSystem(1)config=awtx.weight.getConfig(1)wt=awtx.weight.getCurrent(1)setpoint.setOutputValues()end
wt={}system={}config={}printTokens={}system=awtx.hardware.getSystem(1)config=awtx.weight.getConfig(1)wt=awtx.weight.getCurrent(1)separatorChar=config.displaySeparator
tareHoldFlag=0
selectHoldFlag=0
zeroHoldFlag=0
printHoldFlag=0
unitsHoldFlag=0
idHoldFlag=0
f1HoldFlag=0
setupHoldFlag=0
targetHoldFlag=0
sampleHoldFlag=0
startHoldFlag=0
stopHoldFlag=0
underHoldFlag=0
overHoldFlag=0
numericHoldFlag=0
clearHoldFlag=0
rpnidflag=false
rpnprintflag=false
rpntareflag=false
rpnotherflag=false
SendRDKey={[0]={tarekey=string.format("%c",244),selectkey=string.format("%c",243),zerokey=string.format("%c",250),printkey=string.format("%c",240),unitskey=string.format("%c",245),f1key=string.format("%c",229),targetkey=string.format("%c",129),samplekey="",startkey=string.format("%c",133),stopkey=string.format("%c",134),clearkey=string.format("%c",227),idkey="",setupkey="",enterkey=string.format("%c",229),overkey="",underkey="",menukey=string.format("%c%c%c%c%c%c%c%c%c%c",49,48,48,243,50,51,54,52,48,229)},{tarekey=string.format("%c%c",84,10),selectkey=string.format("%c%c",83,10),zerokey=string.format("%c%c",90,10),printkey=string.format("%c%c",80,10),unitskey=string.format("%c%c",85,10),f1key=string.format("%c%c",70,10),targetkey=string.format("%c%c",76,10),samplekey=string.format("%c%c",74,10),startkey=string.format("%c%c",71,10),stopkey=string.format("%c%c",72,10),clearkey=string.format("%c%c",67,10),idkey=string.format("%c%c",73,10),setupkey=string.format("%c%c",75,10),enterkey="",overkey=string.format("%c%c",79,10),underkey=string.format("%c%c",85,10),menukey=""},{tarekey=string.format("%c%c",84,10),selectkey=string.format("%c%c",83,10),zerokey=string.format("%c%c",90,10),printkey=string.format("%c%c",80,10),unitskey=string.format("%c%c",85,10),f1key=string.format("%c%c",70,10),targetkey="",samplekey="",startkey="",stopkey="",clearkey="",idkey="",setupkey="",enterkey="",overkey="",underkey="",menukey=""},{tarekey=string.format("%c%c",84,10),selectkey=string.format("%c%c",83,10),zerokey=string.format("%c%c",90,10),printkey=string.format("%c%c",80,10),unitskey=string.format("%c%c",85,10),f1key=string.format("%c%c",70,10),targetkey="",samplekey="",startkey="",stopkey="",clearkey="",idkey="",setupkey="",enterkey="",overkey="",underkey="",menukey=""},{tarekey=string.format("%c%c",84,10),selectkey=string.format("%c%c",83,10),zerokey=string.format("%c%c",90,10),printkey=string.format("%c%c",80,10),unitskey=string.format("%c%c",85,10),f1key=string.format("%c%c",70,10),targetkey="",samplekey="",startkey="",stopkey="",clearkey="",idkey="",setupkey="",enterkey="",overkey="",underkey="",menukey=""},{tarekey=string.format("%c%c",84,10),selectkey=string.format("%c%c",83,10),zerokey=string.format("%c%c",90,10),printkey=string.format("%c%c",80,10),unitskey=string.format("%c%c",85,10),f1key=string.format("%c%c",70,10),targetkey="",samplekey="",startkey="",stopkey="",clearkey="",idkey="",setupkey="",enterkey="",overkey="",underkey="",menukey=""},{tarekey=string.format("%c%c",84,10),selectkey=string.format("%c%c",83,10),zerokey=string.format("%c%c",90,10),printkey=string.format("%c%c",80,10),unitskey=string.format("%c%c",85,10),f1key=string.format("%c%c",70,10),targetkey=string.format("%c%c",76,10),samplekey=string.format("%c%c",74,10),startkey=string.format("%c%c",71,10),stopkey=string.format("%c%c",72,10),clearkey=string.format("%c%c",67,10),idkey=string.format("%c%c",73,10),setupkey=string.format("%c%c",75,10),enterkey="",overkey=string.format("%c%c",79,10),underkey=string.format("%c%c",85,10),menukey=""}}if config.keyTareFlag then
rpntareflag=false
end
require("ReqScaleKeys")require("ReqTare")require("ReqSetpoint")require("ReqR-Disp")require("ReqBattery")require("ReqAppMenu")function awtx.keypad.KEY_TARE_DOWN()if rdisp.RDispEnable==0 then
local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)tareHoldFlag=0
if config.presetTareFlag then
elseif config.keyTareFlag then
elseif config.pbTareFlag then
checkTareEnable()end
end
end
function awtx.keypad.KEY_TARE_REPEAT()if rdisp.RDispEnable==0 then
tareHoldFlag=tareHoldFlag+1
if tareHoldFlag==HowManyRepeatsMakeAHold then
if config.pbTareFlag then
awtx.weight.requestTareClear()displayCLEARED()elseif config.keyTareFlag then
awtx.weight.requestKeyboardTare(0)displayCLEARED()elseif config.presetTareFlag then
awtx.weight.requestPresetTare(0)displayCLEARED()else
displayCANT()end
end
end
end
function awtx.keypad.KEY_TARE_UP()if rdisp.RDispEnable==0 then
if tareHoldFlag<HowManyRepeatsMakeAHold then
if config.presetTareFlag then
checkTareEnable()elseif config.keyTareFlag then
checkTareEnable()elseif config.pbTareFlag then
end
end
tareHoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].tarekey)end
end
function TARECOMPLETEEVENT(eventResult,eventResultString)local usermode
if rdisp.RDispEnable==0 then
if eventResult~=0 then
displayCANT()else
end
else
end
end
awtx.weight.registerTareCompleteEvent(TARECOMPLETEEVENT)function awtx.keypad.KEY_SELECT_DOWN()if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)selectHoldFlag=0
else
selectHoldFlag=0
end
end
function awtx.keypad.KEY_SELECT_REPEAT()if rdisp.RDispEnable==0 then
selectHoldFlag=selectHoldFlag+1
if selectHoldFlag==HowManyRepeatsMakeAHold then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()supervisor.dynamicMenu(SetpointEdit,resolveCircular2)appEnableSetpoints()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)end
else
selectHoldFlag=selectHoldFlag+1
if selectHoldFlag==HowManyRepeatsMakeAHold then
end
end
end
function awtx.keypad.KEY_SELECT_UP()if rdisp.RDispEnable==0 then
if selectHoldFlag<HowManyRepeatsMakeAHold then
awtx.weight.cycleActiveValue()end
selectHoldFlag=0
else
if selectHoldFlag<HowManyRepeatsMakeAHold then
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].selectkey)end
selectHoldFlag=0
end
end
function awtx.keypad.KEY_ZERO_DOWN()if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)zeroHoldFlag=0
awtx.weight.requestZero()else
end
end
function awtx.keypad.KEY_ZERO_REPEAT()if rdisp.RDispEnable==0 then
zeroHoldFlag=zeroHoldFlag+1
if zeroHoldFlag==HowManyRepeatsMakeAHold then
end
else
end
end
function awtx.keypad.KEY_ZERO_UP()if rdisp.RDispEnable==0 then
if zeroHoldFlag<HowManyRepeatsMakeAHold then
end
zeroHoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].zerokey)end
end
function ZEROCOMPLETEEVENT(eventResult,eventResultString)if rdisp.RDispEnable==0 then
if eventResult~=0 then
displayCANT()else
end
else
end
end
awtx.weight.registerZeroCompleteEvent(ZEROCOMPLETEEVENT)function awtx.keypad.KEY_PRINT_DOWN()local usermode
if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)printHoldFlag=0
wt=awtx.weight.getCurrent(1)if config.printRTZ then
if not wt.inGrossBand then
awtx.weight.requestPrint()end
else
awtx.weight.requestPrint()end
else
end
end
function awtx.keypad.KEY_PRINT_REPEAT()if rdisp.RDispEnable==0 then
printHoldFlag=printHoldFlag+1
if printHoldFlag==HowManyRepeatsMakeAHold then
end
else
end
end
function awtx.keypad.KEY_PRINT_UP()if rdisp.RDispEnable==0 then
if printHoldFlag<HowManyRepeatsMakeAHold then
end
printHoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].printkey)end
end
function PRINTCOMPLETEEVENT(eventResult,eventResultString)if rdisp.RDispEnable==0 then
if eventResult~=0 then
displayCANT()else
wt=awtx.weight.getLastPrint(1)awtx.printer.PrintFmt(0)end
else
end
end
awtx.weight.registerPrintCompleteEvent(PRINTCOMPLETEEVENT)function PRINTERSTATUSEVENT(prnnum,param1,param2,param3,param4,param5)if rdisp.RDispEnable==0 then
if prnnum>=0 then
if param1~=0 then
printerstatusmsg(param1)if param2~=0 then
printerstatusmsg(param2)if param3~=0 then
printerstatusmsg(param3)if param4~=0 then
printerstatusmsg(param4)if param5~=0 then
printerstatusmsg(param5)end
end
end
end
end
end
else
end
end
awtx.printer.registerPrinterStatusEvent(PRINTERSTATUSEVENT)function PRINTERJOBSTATUSEVENT(prnnum,param1,param2,param3,param4,param5)if rdisp.RDispEnable==0 then
if prnnum>=0 then
if param1~=0 then
printerjobstatusmsg(param1)if param2~=0 then
printerjobstatusmsg(param2)if param3~=0 then
printerjobstatusmsg(param3)if param4~=0 then
printerjobstatusmsg(param4)if param5~=0 then
printerjobstatusmsg(param5)end
end
end
end
end
end
else
end
end
awtx.printer.registerPrinterJobStatusEvent(PRINTERJOBSTATUSEVENT)function printerstatusmsg(msgid)msg=""time=1000
if msgid==1 then
msg="Offline"elseif msgid==2 then
msg="Online "elseif msgid==5 then
msg="PaperLo"time=600
end
if msg~=""then
local usermode
local currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine(msg)awtx.os.sleep(time)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
end
function printerjobstatusmsg(msgid)msg=""time=1000
if msgid==1 then
msg="PrnDone"elseif msgid==2 then
msg="PrnAbrt"elseif msgid==3 then
msg="PrnFail"end
if msg~=""then
local usermode
local currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine(msg)awtx.os.sleep(time)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
end
function awtx.keypad.KEY_UNITS_DOWN()if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)unitsHoldFlag=0
else
end
end
function awtx.keypad.KEY_UNITS_REPEAT()if rdisp.RDispEnable==0 then
unitsHoldFlag=unitsHoldFlag+1
if unitsHoldFlag==HowManyRepeatsMakeAHold then
end
else
end
end
function awtx.keypad.KEY_UNITS_UP()if rdisp.RDispEnable==0 then
if unitsHoldFlag<HowManyRepeatsMakeAHold then
awtx.weight.cycleUnits()awtx.os.sleep(100)calcAfterUnitsChange()end
unitsHoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].unitskey)end
end
function awtx.keypad.KEY_ID_DOWN()if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)idHoldFlag=0
else
end
end
function awtx.keypad.KEY_ID_REPEAT()if rdisp.RDispEnable==0 then
idHoldFlag=idHoldFlag+1
if idHoldFlag==HowManyRepeatsMakeAHold then
enterId()end
else
end
end
function awtx.keypad.KEY_ID_UP()if rdisp.RDispEnable==0 then
if idHoldFlag<HowManyRepeatsMakeAHold then
showId()end
idHoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].idkey)end
end
function awtx.keypad.KEY_F1_DOWN()if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)f1HoldFlag=0
else
end
end
function awtx.keypad.KEY_F1_REPEAT()if rdisp.RDispEnable==0 then
f1HoldFlag=f1HoldFlag+1
if f1HoldFlag==HowManyRepeatsMakeAHold then
end
else
end
end
function awtx.keypad.KEY_F1_UP()if rdisp.RDispEnable==0 then
if f1HoldFlag<HowManyRepeatsMakeAHold then
end
f1HoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].f1key)end
end
function awtx.keypad.KEY_SETUP_DOWN()if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)setupHoldFlag=0
else
end
end
function awtx.keypad.KEY_SETUP_REPEAT()if rdisp.RDispEnable==0 then
setupHoldFlag=setupHoldFlag+1
if setupHoldFlag==HowManyRepeatsMakeAHold then
end
else
end
end
function awtx.keypad.KEY_SETUP_UP()if rdisp.RDispEnable==0 then
if setupHoldFlag<HowManyRepeatsMakeAHold then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()supervisor.dynamicMenu(SetpointEdit,resolveCircular2)appEnableSetpoints()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)end
setupHoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].setupkey)end
end
function awtx.keypad.KEY_TARGET_DOWN()if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)targetHoldFlag=0
else
end
end
function awtx.keypad.KEY_TARGET_REPEAT()if rdisp.RDispEnable==0 then
targetHoldFlag=targetHoldFlag+1
if targetHoldFlag==HowManyRepeatsMakeAHold then
end
else
end
end
function awtx.keypad.KEY_TARGET_UP()if rdisp.RDispEnable==0 then
if targetHoldFlag<HowManyRepeatsMakeAHold then
end
targetHoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].targetkey)end
end
function awtx.keypad.KEY_SAMPLE_DOWN()if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)sampleHoldFlag=0
else
end
end
function awtx.keypad.KEY_SAMPLE_REPEAT()if rdisp.RDispEnable==0 then
sampleHoldFlag=sampleHoldFlag+1
if sampleHoldFlag==HowManyRepeatsMakeAHold then
end
else
end
end
function awtx.keypad.KEY_SAMPLE_UP()if rdisp.RDispEnable==0 then
if sampleHoldFlag<HowManyRepeatsMakeAHold then
end
sampleHoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].samplekey)end
end
function awtx.keypad.KEY_START_DOWN()if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)startHoldFlag=0
else
end
end
function awtx.keypad.KEY_START_REPEAT()if rdisp.RDispEnable==0 then
startHoldFlag=startHoldFlag+1
if startHoldFlag==HowManyRepeatsMakeAHold then
end
else
end
end
function awtx.keypad.KEY_START_UP()if rdisp.RDispEnable==0 then
if startHoldFlag<HowManyRepeatsMakeAHold then
end
startHoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].startkey)end
end
function awtx.keypad.KEY_STOP_DOWN()if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)stopHoldFlag=0
else
end
end
function awtx.keypad.KEY_STOP_REPEAT()if rdisp.RDispEnable==0 then
stopHoldFlag=stopHoldFlag+1
if stopHoldFlag==HowManyRepeatsMakeAHold then
end
else
end
end
function awtx.keypad.KEY_STOP_UP()if rdisp.RDispEnable==0 then
if stopHoldFlag<HowManyRepeatsMakeAHold then
end
stopHoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].stopkey)end
end
function awtx.keypad.KEY_UNDER_DOWN()if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)underHoldFlag=0
else
end
end
function awtx.keypad.KEY_UNDER_REPEAT()if rdisp.RDispEnable==0 then
underHoldFlag=underHoldFlag+1
if underHoldFlag==HowManyRepeatsMakeAHold then
end
else
end
end
function awtx.keypad.KEY_UNDER_UP()if rdisp.RDispEnable==0 then
if underHoldFlag<HowManyRepeatsMakeAHold then
end
underHoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].underkey)end
end
function awtx.keypad.KEY_OVER_DOWN()if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)overHoldFlag=0
else
end
end
function awtx.keypad.KEY_OVER_REPEAT()if rdisp.RDispEnable==0 then
overHoldFlag=overHoldFlag+1
if overHoldFlag==HowManyRepeatsMakeAHold then
end
else
end
end
function awtx.keypad.KEY_OVER_UP()if rdisp.RDispEnable==0 then
if overHoldFlag<HowManyRepeatsMakeAHold then
end
overHoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].overkey)end
end
numericHoldFlag=0
function awtx.keypad.KEY_NUMERIC_DOWN(keyval)if rdisp.RDispEnable==0 then
numericHoldFlag=0
else
end
end
function awtx.keypad.KEY_NUMERIC_REPEAT(keyval)if rdisp.RDispEnable==0 then
numericHoldFlag=numericHoldFlag+1
if numericHoldFlag==HowManyRepeatsMakeAHold then
end
else
end
end
function awtx.keypad.KEY_NUMERIC_UP(keyval)if rdisp.RDispEnable==0 then
if numericHoldFlag<HowManyRepeatsMakeAHold then
end
numericHoldFlag=0
else
if rdisp.RDispDataFormat==0 then
if keyval>=0 and keyval<=9 then
keyval=keyval+0x30
end
result=awtx.serial.send(rdisp.RDispPort,string.format("%c",keyval))else
if keyval>=0 and keyval<=9 then
keyval=keyval+0x30
end
result=awtx.serial.send(rdisp.RDispPort,string.format("%c%c",keyval,10))end
end
end
function awtx.keypad.KEY_CLEAR_DOWN()if rdisp.RDispEnable==0 then
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)clearHoldFlag=0
else
end
end
function awtx.keypad.KEY_CLEAR_REPEAT()if rdisp.RDispEnable==0 then
clearHoldFlag=clearHoldFlag+1
if clearHoldFlag==HowManyRepeatsMakeAHold then
end
else
end
end
function awtx.keypad.KEY_CLEAR_UP()if rdisp.RDispEnable==0 then
if clearHoldFlag<HowManyRepeatsMakeAHold then
end
clearHoldFlag=0
else
result=awtx.serial.send(rdisp.RDispPort,SendRDKey[rdisp.RDispDataFormat].clearkey)end
end
local rpnstartflag=false
local rpnstopflag=false
local rpnf1flag=false
local rpnsetupflag=false
local rpntargetflag=false
local rpnzeroflag=false
local rpnunitflag=false
local rpnsampleflag=false
local rpnselectflag=true
local rpnoverflag=false
local rpnunderflag=false
function RPNCOMPLETEEVENT(lastKey,enteredValue)local usermode,result
local newID=0
local newFmt=0
local newTare=0
local newSelect=0
if rdisp.RDispEnable==0 then
if lastKey=="ID"then
if rpnidflag==true then
newID=tonumber(enteredValue)setId(newID)else
displayCANT()end
elseif lastKey=="PRINT"then
if rpnprintflag==true then
newFmt=tonumber(enteredValue)wt=awtx.weight.getRefreshLastPrint(1)awtx.printer.PrintFmt(newFmt)else
displayCANT()end
elseif lastKey=="TARE"then
if rpntareflag==true then
if config.keyTareFlag then
newTare=tonumber(enteredValue)awtx.weight.requestKeyboardTare(newTare)elseif config.presetTareFlag then
newTare=tonumber(enteredValue)tare.setTareChannel(newTare)end
else
displayCANT()end
elseif lastKey=="SELECT"then
if rpnselectflag==true then
newSelect=tonumber(enteredValue)if newSelect==11 then
result=awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.setupMenu.diagCurrentTime()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)result=awtx.keypad.set_RPN_mode(1)elseif newSelect==100 then
awtx.setupMenu.getPasswordFromUser()awtx.setupMenu.runPasswordMenu()else
displayCANT()end
else
displayCANT()end
else
if rpnotherflag==true then
else
displayCANT()end
end
else
end
end
awtx.keypad.registerNumberEntryRPN(RPNCOMPLETEEVENT)function checkRPNMode()if rdisp.RDispEnable==0 then
if system.modelStr=="ZM301"then
result=awtx.keypad.set_RPN_mode(0)elseif system.modelStr=="ZM303"then
result=awtx.keypad.set_RPN_mode(1)elseif system.modelStr=="ZQ375"then
result=awtx.keypad.set_RPN_mode(0)elseif system.modelStr=="ZM305"then
result=awtx.keypad.set_RPN_mode(1)elseif system.modelStr=="ZM305"then
result=awtx.keypad.set_RPN_mode(1)else
result=awtx.keypad.set_RPN_mode(0)end
else
result=awtx.keypad.set_RPN_mode(0)end
end
checkRPNMode()channelNum1=1
eomChar1=13
awtx.serial.setEomChar(channelNum1,eomChar1)channelNum2=2
eomChar2=13
awtx.serial.setEomChar(channelNum2,eomChar2)function SERIAL_EOM_RX_EVENT1(channelNumber,notUsed1,notUsed2)local usermode
if channelNumber==1 then
local currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("Port 1")awtx.os.sleep(canttime)bufcount1=awtx.serial.getRxCount(channelNum1)rxDataStr1,readCount1=awtx.serial.getRx(channelNum1)bufcount1=awtx.serial.getRxCount(channelNum1)result=awtx.serial.send(channelNum1,"sendString")awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)else
end
end
function SERIAL_EOM_RX_EVENT2(channelNumber,notUsed1,notUsed2)local usermode
if channelNumber==2 then
local currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("Port 2")awtx.os.sleep(canttime)bufcount2=awtx.serial.getRxCount(channelNum2)rxDataStr2,readCount2=awtx.serial.getRx(channelNum2)bufcount2=awtx.serial.getRxCount(channelNum2)result=awtx.serial.send(channelNum2,"sendString")awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)else
end
end
T1={text="Super",key=1,action="MENU",variable="SuperMenu"}T2={text="EXIT",key=2,action="FUNC",callThis=supervisor.exitFunction}topMenu={T1,T2}SuperMenu1={text=" SETPNT",key=1,action="MENU",variable="SetpointSetupMenu"}SuperMenu2={text=" R-DISP",key=2,action="MENU",variable="RdispSetupMenu"}SuperMenu3={text="BATTERY",key=3,action="MENU",variable="BatterySetupMenu"}SuperMenu4={text=" BACK  ",key=4,action="MENU",variable="topMenu",subMenu=1}SuperMenu={SuperMenu1,SuperMenu2,SuperMenu3,SuperMenu4}SetpointSetupMenu1={text=" Edit  ",key=1,action="MENU",variable="SetpointSetupEdit"}SetpointSetupMenu2={text=" Print ",key=2,action="FUNC",callThis=setpoint.SetpointDBReport}SetpointSetupMenu3={text=" Reset ",key=3,action="FUNC",callThis=setpoint.SetpointDBReset}SetpointSetupMenu4={text=" BACK  ",key=4,action="MENU",variable="SuperMenu",subMenu=1}SetpointSetupMenu={SetpointSetupMenu1,SetpointSetupMenu2,SetpointSetupMenu3,SetpointSetupMenu4}SetpointSetupEdit1={text=" ANNUN ",key=1,action="FUNC",callThis=setpoint.editInvertingOutputFlag}SetpointSetupEdit2={text=" OUT1  ",key=2,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Mode_Model,val1=0}}SetpointSetupEdit3={text=" OUT1  ",key=3,action="MENU",variable="SetpointOut1Edit",show={callThis=get_Mode_Model,val1=1}}SetpointSetupEdit4={text=" OUT2  ",key=4,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Mode_Model,val1=0}}SetpointSetupEdit5={text=" OUT2  ",key=5,action="MENU",variable="SetpointOut2Edit",show={callThis=get_Mode_Model,val1=1}}SetpointSetupEdit6={text=" OUT3  ",key=6,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Mode_Model,val1=0}}SetpointSetupEdit7={text=" OUT3  ",key=7,action="MENU",variable="SetpointOut3Edit",show={callThis=get_Mode_Model,val1=1}}SetpointSetupEdit8={text="  IN1  ",key=8,action="FUNC",callThis=setpoint.editIn1}SetpointSetupEdit9={text="  IN2  ",key=9,action="FUNC",callThis=setpoint.editIn2}SetpointSetupEdit10={text="  IN3  ",key=10,action="FUNC",callThis=setpoint.editIn3}SetpointSetupEdit11={text=" MODE  ",key=11,action="FUNC",callThis=setpoint.editOutputMode,show={callThis=get_Mode_Model,val1=0}}SetpointSetupEdit12={text=" BACK  ",key=12,action="MENU",variable="SetpointSetupMenu",subMenu=1}SetpointSetupEdit={SetpointSetupEdit1,SetpointSetupEdit2,SetpointSetupEdit3,SetpointSetupEdit4,SetpointSetupEdit5,SetpointSetupEdit6,SetpointSetupEdit7,SetpointSetupEdit8,SetpointSetupEdit9,SetpointSetupEdit10,SetpointSetupEdit11,SetpointSetupEdit12}SetpointOut1Edit1={text=" EDIT1 ",key=1,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Out1_Mode,val1=0}}SetpointOut1Edit2={text=" EDIT1 ",key=2,action="MENU",variable="SetpointOut1LoHiEdit",show={callThis=get_Out1_Mode,val1=1}}SetpointOut1Edit3={text=" MODE1 ",key=3,action="FUNC",callThis=setpoint.editOutputMode1}SetpointOut1Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointSetupEdit",subMenu=3}SetpointOut1Edit={SetpointOut1Edit1,SetpointOut1Edit2,SetpointOut1Edit3,SetpointOut1Edit4}SetpointOut1LoHiEdit1={text="OUT1 LO",key=1,action="FUNC",callThis=setpoint.editOut1Lo}SetpointOut1LoHiEdit2={text="OUT1 HI",key=2,action="FUNC",callThis=setpoint.editOut1Hi}SetpointOut1LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointOut1Edit",subMenu=2}SetpointOut1LoHiEdit={SetpointOut1LoHiEdit1,SetpointOut1LoHiEdit2,SetpointOut1LoHiEdit3}SetpointOut2Edit1={text=" EDIT2 ",key=1,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Out2_Mode,val1=0}}SetpointOut2Edit2={text=" EDIT2 ",key=2,action="MENU",variable="SetpointOut2LoHiEdit",show={callThis=get_Out2_Mode,val1=1}}SetpointOut2Edit3={text=" MODE2 ",key=3,action="FUNC",callThis=setpoint.editOutputMode2}SetpointOut2Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointSetupEdit",subMenu=5}SetpointOut2Edit={SetpointOut2Edit1,SetpointOut2Edit2,SetpointOut2Edit3,SetpointOut2Edit4}SetpointOut2LoHiEdit1={text="OUT2 LO",key=1,action="FUNC",callThis=setpoint.editOut2Lo}SetpointOut2LoHiEdit2={text="OUT2 HI",key=2,action="FUNC",callThis=setpoint.editOut2Hi}SetpointOut2LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointOut2Edit",subMenu=2}SetpointOut2LoHiEdit={SetpointOut2LoHiEdit1,SetpointOut2LoHiEdit2,SetpointOut2LoHiEdit3}SetpointOut3Edit1={text=" EDIT3 ",key=1,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Out3_Mode,val1=0}}SetpointOut3Edit2={text=" EDIT3 ",key=2,action="MENU",variable="SetpointOut3LoHiEdit",show={callThis=get_Out3_Mode,val1=1}}SetpointOut3Edit3={text=" MODE3 ",key=3,action="FUNC",callThis=setpoint.editOutputMode3}SetpointOut3Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointSetupEdit",subMenu=7}SetpointOut3Edit={SetpointOut3Edit1,SetpointOut3Edit2,SetpointOut3Edit3,SetpointOut3Edit4}SetpointOut3LoHiEdit1={text="OUT3 LO",key=1,action="FUNC",callThis=setpoint.editOut3Lo}SetpointOut3LoHiEdit2={text="OUT3 HI",key=2,action="FUNC",callThis=setpoint.editOut3Hi}SetpointOut3LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointOut3Edit",subMenu=2}SetpointOut3LoHiEdit={SetpointOut3LoHiEdit1,SetpointOut3LoHiEdit2,SetpointOut3LoHiEdit3}RdispSetupMenu1={text=" Enable",key=1,action="FUNC",callThis=rdisp.editRDispEnable}RdispSetupMenu2={text=" Mode  ",key=2,action="FUNC",callThis=rdisp.editRDispDataFormat}RdispSetupMenu3={text="BindFwd",key=3,action="FUNC",callThis=rdisp.editRDispBindFwd}RdispSetupMenu4={text=" BACK  ",key=4,action="MENU",variable="SuperMenu",subMenu=2}RdispSetupMenu={RdispSetupMenu1,RdispSetupMenu2,RdispSetupMenu3,RdispSetupMenu4}BatterySetupMenu1={text=" Enable",key=1,action="FUNC",callThis=battery.editBatteryEnable}BatterySetupMenu2={text=" Tmout ",key=2,action="FUNC",callThis=battery.editBatterySetting}BatterySetupMenu3={text=" BACK  ",key=3,action="MENU",variable="SuperMenu",subMenu=3}BatterySetupMenu={BatterySetupMenu1,BatterySetupMenu2,BatterySetupMenu3}resolveCircular1={topMenu=topMenu,SuperMenu=SuperMenu,SetpointSetupMenu=SetpointSetupMenu,SetpointSetupEdit=SetpointSetupEdit,SetpointOut1Edit=SetpointOut1Edit,SetpointOut1LoHiEdit=SetpointOut1LoHiEdit,SetpointOut2Edit=SetpointOut2Edit,SetpointOut2LoHiEdit=SetpointOut2LoHiEdit,SetpointOut3Edit=SetpointOut3Edit,SetpointOut3LoHiEdit=SetpointOut3LoHiEdit,RdispSetupMenu=RdispSetupMenu,BatterySetupMenu=BatterySetupMenu}SetpointEdit1={text=" Setpnt",key=1,action="MENU",variable="SetpointEditor"}SetpointEdit2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}SetpointEdit={SetpointEdit1,SetpointEdit2}SetpointEditor1={text=" OUT1  ",key=1,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Mode_Model,val1=0}}SetpointEditor2={text=" OUT1  ",key=2,action="MENU",variable="SetpointEditorOut1Edit",show={callThis=get_Mode_Model,val1=1}}SetpointEditor3={text=" OUT2  ",key=3,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Mode_Model,val1=0}}SetpointEditor4={text=" OUT2  ",key=4,action="MENU",variable="SetpointEditorOut2Edit",show={callThis=get_Mode_Model,val1=1}}SetpointEditor5={text=" OUT3  ",key=5,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Mode_Model,val1=0}}SetpointEditor6={text=" OUT3  ",key=6,action="MENU",variable="SetpointEditorOut3Edit",show={callThis=get_Mode_Model,val1=1}}SetpointEditor7={text=" BACK  ",key=7,action="MENU",variable="SetpointEdit",subMenu=1}SetpointEditor={SetpointEditor1,SetpointEditor2,SetpointEditor3,SetpointEditor4,SetpointEditor5,SetpointEditor6,SetpointEditor7}SetpointEditorOut1Edit1={text=" EDIT1 ",key=1,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Out1_Mode,val1=0}}SetpointEditorOut1Edit2={text=" EDIT1 ",key=2,action="MENU",variable="SetpointEditorOut1LoHiEdit",show={callThis=get_Out1_Mode,val1=1}}SetpointEditorOut1Edit3={text=" MODE1 ",key=3,action="FUNC",callThis=setpoint.editOutputMode1}SetpointEditorOut1Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointEditor",subMenu=2}SetpointEditorOut1Edit={SetpointEditorOut1Edit1,SetpointEditorOut1Edit2,SetpointEditorOut1Edit3,SetpointEditorOut1Edit4}SetpointEditorOut1LoHiEdit1={text="OUT1 LO",key=1,action="FUNC",callThis=setpoint.editOut1Lo}SetpointEditorOut1LoHiEdit2={text="OUT1 HI",key=2,action="FUNC",callThis=setpoint.editOut1Hi}SetpointEditorOut1LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointEditorOut1Edit",subMenu=2}SetpointEditorOut1LoHiEdit={SetpointEditorOut1LoHiEdit1,SetpointEditorOut1LoHiEdit2,SetpointEditorOut1LoHiEdit3}SetpointEditorOut2Edit1={text=" EDIT2 ",key=1,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Out2_Mode,val1=0}}SetpointEditorOut2Edit2={text=" EDIT2 ",key=2,action="MENU",variable="SetpointEditorOut2LoHiEdit",show={callThis=get_Out2_Mode,val1=1}}SetpointEditorOut2Edit3={text=" MODE2 ",key=3,action="FUNC",callThis=setpoint.editOutputMode2}SetpointEditorOut2Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointEditor",subMenu=4}SetpointEditorOut2Edit={SetpointEditorOut2Edit1,SetpointEditorOut2Edit2,SetpointEditorOut2Edit3,SetpointEditorOut2Edit4}SetpointEditorOut2LoHiEdit1={text="OUT2 LO",key=1,action="FUNC",callThis=setpoint.editOut2Lo}SetpointEditorOut2LoHiEdit2={text="OUT2 HI",key=2,action="FUNC",callThis=setpoint.editOut2Hi}SetpointEditorOut2LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointEditorOut2Edit",subMenu=2}SetpointEditorOut2LoHiEdit={SetpointEditorOut2LoHiEdit1,SetpointEditorOut2LoHiEdit2,SetpointEditorOut2LoHiEdit3}SetpointEditorOut3Edit1={text=" EDIT3 ",key=1,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Out3_Mode,val1=0}}SetpointEditorOut3Edit2={text=" EDIT3 ",key=2,action="MENU",variable="SetpointEditorOut3LoHiEdit",show={callThis=get_Out3_Mode,val1=1}}SetpointEditorOut3Edit3={text=" MODE3 ",key=3,action="FUNC",callThis=setpoint.editOutputMode3}SetpointEditorOut3Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointEditor",subMenu=6}SetpointEditorOut3Edit={SetpointEditorOut3Edit1,SetpointEditorOut3Edit2,SetpointEditorOut3Edit3,SetpointEditorOut3Edit4}SetpointEditorOut3LoHiEdit1={text="OUT3 LO",key=1,action="FUNC",callThis=setpoint.editOut3Lo}SetpointEditorOut3LoHiEdit2={text="OUT3 HI",key=2,action="FUNC",callThis=setpoint.editOut3Hi}SetpointEditorOut3LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointEditorOut3Edit",subMenu=2}SetpointEditorOut3LoHiEdit={SetpointEditorOut3LoHiEdit1,SetpointEditorOut3LoHiEdit2,SetpointEditorOut3LoHiEdit3}resolveCircular2={SetpointEdit=SetpointEdit,SetpointEditor=SetpointEditor,SetpointEditorOut1Edit=SetpointEditorOut1Edit,SetpointEditorOut1LoHiEdit=SetpointEditorOut1LoHiEdit,SetpointEditorOut2Edit=SetpointEditorOut2Edit,SetpointEditorOut2LoHiEdit=SetpointEditorOut2LoHiEdit,SetpointEditorOut3Edit=SetpointEditorOut3Edit,SetpointEditorOut3LoHiEdit=SetpointEditorOut3LoHiEdit}ParamTopMenu1={text=" Param ",key=1,action="MENU",variable="OutputModeEdit"}ParamTopMenu2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}ParamTopMenu={ParamTopMenu1,ParamTopMenu2}OutputModeEdit1={text="OutMode",key=1,action="MENU",variable="OutputModeEditor"}OutputModeEdit2={text=" BACK  ",key=2,action="MENU",variable="ParamTopMenu",subMenu=1}OutputModeEdit={OutputModeEdit1,OutputModeEdit2}OutputModeEditor1={text=" OUT1  ",key=1,action="FUNC",callThis=setpoint.editOutputMode1}OutputModeEditor2={text=" OUT2  ",key=2,action="FUNC",callThis=setpoint.editOutputMode2}OutputModeEditor3={text=" OUT3  ",key=3,action="FUNC",callThis=setpoint.editOutputMode3}OutputModeEditor4={text=" BACK  ",key=4,action="MENU",variable="OutputModeEdit",subMenu=1}OutputModeEditor={OutputModeEditor1,OutputModeEditor2,OutputModeEditor3,OutputModeEditor4}resolveCircular7={ParamTopMenu=ParamTopMenu,OutputModeEdit=OutputModeEdit,OutputModeEditor=OutputModeEditor}function appEnterSuperMenu()rdisp.disableSettings()end
function appExitSuperMenu()local usermode
checkRPNMode()rdisp.updateSettings()end
setpoint.basis="netWt"function appDisableSetpoints()setpoint.disableSetpointsOutputs()end
function appEnableSetpoints()setpoint.refreshSetpointsOutputs(Setpointtable.OutputMode)end
function appRefreshBatterySettings()setpoint.refreshSetpointsOutputs(Setpointtable.OutputMode)end
appRefreshBatterySettings()setpoint.refreshSetpointsInputs()function appInput(setpointNum,inputState)if setpointNum==1 then
setpoint.setpointIn1Handler(setpointNum,inputState)end
if setpointNum==2 then
setpoint.setpointIn2Handler(setpointNum,inputState)end
if setpointNum==3 then
setpoint.setpointIn3Handler(setpointNum,inputState)end
end
ID.initIDPrintTokens()tare.initTarePrintTokens()setpoint.initSetpointPrintTokens()SMAPC.initPrintTokens()appExitSuperMenu()printAppDebug("\nStartup App7R-Disp.lua non-event ")local analogcount=0
while true do
analogcount=analogcount+1
if analogcount>100 then
analogcount=0
rdisp.getRDispAnalog()end
awtx.os.systemEvents(100)end