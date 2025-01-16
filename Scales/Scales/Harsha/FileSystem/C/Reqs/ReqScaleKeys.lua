tmp=0
for index=1,100 do
printTokens[index]={}printTokens[index].varName=""printTokens[index].varLabel="Invalid"printTokens[index].varValue=tmp
printTokens[index].varFunct=""awtx.fmtPrint.varSet(index,tmp,"Invalid",AWTX_LUA_INTEGER)end
ID={}ID.id=0
if system.modelStr=="ZM301"then
result=awtx.keypad.set_RPN_mode(0)elseif system.modelStr=="ZM303"then
result=awtx.keypad.set_RPN_mode(1)elseif system.modelStr=="ZQ375"then
result=awtx.keypad.set_RPN_mode(0)elseif system.modelStr=="ZM305GTN"then
result=awtx.keypad.set_RPN_mode(1)elseif system.modelStr=="ZM305"then
result=awtx.keypad.set_RPN_mode(1)else
result=awtx.keypad.set_RPN_mode(0)end
function ID.initIDPrintTokens()awtx.fmtPrint.varSet(1,ID.id,"ID",AWTX_LUA_INTEGER)printTokens[1].varName="ID.id"printTokens[1].varLabel="ID"printTokens[1].varType=AWTX_LUA_INTEGER
printTokens[1].varValue=ID.id
printTokens[1].varFunct=setId
end
function ID.setIDPrintTokens()awtx.fmtPrint.varSet(1,ID.id,"ID",AWTX_LUA_INTEGER)end
SMAPC={}SMAWeighMode=0
SMACheckMode=1
SMALogonMode=2
SMAFindMeMode=3
SMAOfflineMode=4
SMAMinMode=0
SMAMaxMode=4
SMAPC.userIDSTR="USER123"SMAPC.userIDNUM=12345678
SMAPC.message="Test"SMAPC.msgtime=5
SMAPC.timeout=awtx.serial.getSMATimeout()SMAPC.SMAmode=SMAWeighMode
function SMAPC.initPrintTokens()awtx.fmtPrint.varSet(16,SMAPC.userIDSTR,"User ID",AWTX_LUA_STRING)printTokens[16].varName="SMAPC.userIDSTR"printTokens[16].varLabel="User ID"printTokens[16].varType=AWTX_LUA_STRING
printTokens[16].varValue=SMAPC.userIDSTR
printTokens[16].varFunct=SMAPC.setuserIDSTR
awtx.fmtPrint.varSet(17,SMAPC.userIDNUM,"User ID",AWTX_LUA_INTEGER)printTokens[17].varName="SMAPC.userIDNUM"printTokens[17].varLabel="User ID"printTokens[17].varType=AWTX_LUA_INTEGER
printTokens[17].varValue=SMAPC.userIDNUM
printTokens[17].varFunct=SMAPC.setuserIDNUM
awtx.fmtPrint.varSet(18,SMAPC.message,"Message",AWTX_LUA_STRING)printTokens[18].varName="SMAPC.message"printTokens[18].varLabel="Message"printTokens[18].varType=AWTX_LUA_STRING
printTokens[18].varValue=SMAPC.message
printTokens[18].varFunct=SMAPC.setMessage
awtx.fmtPrint.varSet(19,SMAPC.msgtime,"Message Time",AWTX_LUA_INTEGER)printTokens[19].varName="SMAPC.msgtime"printTokens[19].varLabel="Message Time"printTokens[19].varType=AWTX_LUA_INTEGER
printTokens[19].varValue=SMAPC.msgtime
printTokens[19].varFunct=SMAPC.setMsgTime
awtx.fmtPrint.varSet(20,SMAPC.timeout,"Timeout",AWTX_LUA_INTEGER)printTokens[20].varName="SMAPC.timeout"printTokens[20].varLabel="Timeout"printTokens[20].varType=AWTX_LUA_INTEGER
printTokens[20].varValue=SMAPC.timeout
printTokens[20].varFunct=SMAPC.setTimeout
awtx.fmtPrint.varSet(70,SMAPC.SMAmode,"Mode",AWTX_LUA_INTEGER)printTokens[70].varName="SMAPC.SMAmode"printTokens[70].varLabel="Mode"printTokens[70].varType=AWTX_LUA_INTEGER
printTokens[70].varValue=SMAPC.SMAmode
printTokens[70].varFunct=SMAPC.setSMAMode
end
function SMAPC.setPrintTokens()awtx.fmtPrint.varSet(16,SMAPC.userIDSTR,"User ID",AWTX_LUA_STRING)awtx.fmtPrint.varSet(17,SMAPC.userIDNUM,"User ID",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(18,SMAPC.message,"Message",AWTX_LUA_STRING)awtx.fmtPrint.varSet(19,SMAPC.msgtime,"Message Time",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(20,SMAPC.timeout,"Timeout",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(70,SMAPC.SMAmode,"Mode",AWTX_LUA_INTEGER)end
function SMAPC.setuserIDSTR(newuserIDSTR)local usermode
local idmaxVal,isEnterKey
SMAPC.userIDSTR=newuserIDSTR
SMAPC.setPrintTokens()local currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.writeLine("LOGON")SMAPC.SMAmode=SMALogonMode
SMAPC.setPrintTokens()awtx.os.sleep(showtime1)while true do
lastkeyCode=awtx.keypad.getKeyWithoutEvents()if(lastkeyCode~=0)then
break
end
end
idmaxVal=8
newuserIDSTR,isEnterKey=awtx.keypad.enterString(newuserIDSTR,idmaxVal,entertime)awtx.display.setMode(usermode)if isEnterKey then
SMAPC.userIDSTR=newuserIDSTR
SMAPC.setPrintTokens()else
end
SMAPC.SMAmode=SMAWeighMode
SMAPC.setPrintTokens()awtx.keypad.set_RPN_mode(currentRPN)end
function SMAPC.setuserIDNUM(newuserIDNUM)local usermode
local idminVal,idmaxVal,isEnterKey
SMAPC.userIDNUM=newuserIDNUM
SMAPC.setPrintTokens()local currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.writeLine("LOGON")SMAPC.SMAmode=SMALogonMode
SMAPC.setPrintTokens()awtx.os.sleep(showtime1)while true do
lastkeyCode=awtx.keypad.getKeyWithoutEvents()if(lastkeyCode~=0)then
break
end
end
idminVal=0
idmaxVal=9999999
newuserIDNUM,isEnterKey=awtx.keypad.enterInteger(newuserIDNUM,idminVal,idmaxVal,entertime)awtx.display.setMode(usermode)if isEnterKey then
SMAPC.userIDNUM=newuserIDNUM
SMAPC.setPrintTokens()else
end
SMAPC.SMAmode=SMAWeighMode
SMAPC.setPrintTokens()awtx.keypad.set_RPN_mode(currentRPN)end
function SMAPC.setMessage(newMessage)SMAPC.message=newMessage
SMAPC.setPrintTokens()displayMessage(SMAPC.message,SMAPC.msgtime)end
function SMAPC.setMsgTime(newTime)SMAPC.msgtime=newTime
SMAPC.setPrintTokens()end
function SMAPC.setTimeout(newTimeout)SMAPC.timeout=newTimeout
SMAPC.setPrintTokens()awtx.serial.setSMATimeout(SMAPC.timeout)end
function SMAPC.setSMAMode(newMode)local usermode,currentRPN
local modeminVal,modemaxVal
local Segment1,Segment2
modeminVal=SMAMinMode
modemaxVal=SMAMaxMode
if newMode>=modeminVal and newMode<=modemaxVal then
SMAPC.SMAmode=newMode
SMAPC.setPrintTokens()if SMAPC.SMAmode==SMAWeighMode then
elseif SMAPC.SMAmode==SMACheckMode then
elseif SMAPC.SMAmode==SMALogonMode then
SMAPC.setuserIDNUM(0)elseif SMAPC.SMAmode==SMAFindMeMode then
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayFlash()awtx.os.sleep(5000)awtx.display.clrDisplayFlash()awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)elseif SMAPC.SMAmode==SMAOfflineMode then
SMAOnlineEvent(0)end
end
end
function displayMessage(message,time)local usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine(message)if time==0 then
while true do
lastkeyCode=awtx.keypad.getKeyWithoutEvents()if(lastkeyCode~=0)then
break
end
end
else
awtx.os.sleep(time*1000)end
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(currentRPN)end
SMAPC.SMAOnlineRPN=awtx.keypad.get_RPN_mode()SMAPC.SMAOnlineUsermode=awtx.display.getMode()function SMAOnlineEvent(flag)if(flag==1)then
SMAPC.SMAmode=SMAWeighMode
SMAPC.setPrintTokens()awtx.keypad.set_RPN_mode(0)awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine(" ONLINE")awtx.os.sleep(1000)awtx.display.setMode(SMAPC.SMAOnlineUsermode)awtx.keypad.set_RPN_mode(SMAPC.SMAOnlineRPN)else
SMAPC.SMAmode=SMAOfflineMode
SMAPC.setPrintTokens()SMAPC.SMAOnlineRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("OFFLINE")awtx.os.sleep(1000)end
end
awtx.serial.registerSMAOnlineEvent(SMAOnlineEvent)function displayWORD(word)local usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine(word)awtx.os.sleep(wordtime)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(currentRPN)end
function menuWORD(word)local curmode,usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)curmode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine(word)awtx.os.sleep(wordtime)usermode=awtx.display.setMode(curmode)awtx.keypad.set_RPN_mode(currentRPN)end
function displayCANT()local usermode,currentRPN
usermode=awtx.display.getMode()if usermode==DISPLAY_MODE_SCALE_OBJECT then
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("  CANT ")awtx.os.sleep(canttime)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(currentRPN)end
end
function menuCANT()local curmode,usermode,currentRPN
curmode=awtx.display.getMode()currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)curmode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("  CANT ")awtx.os.sleep(canttime)usermode=awtx.display.setMode(curmode)awtx.keypad.set_RPN_mode(currentRPN)end
function displayDONE()local usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("  DONE ")awtx.os.sleep(canttime)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(currentRPN)end
function displayABORT()local usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine(" ABORT ")awtx.os.sleep(canttime)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(currentRPN)end
function displayERROR()local usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine(" ERROR ")awtx.os.sleep(canttime)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(currentRPN)end
function displayPRNTOT()local usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("PRN-TOT")awtx.os.sleep(canttime)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(currentRPN)end
function displayCLEARED()local usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("CLEARED")awtx.os.sleep(canttime)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(currentRPN)end
function awtx.keypad.KEY_TARE_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)tareHoldFlag=0
if config.presetTareFlag then
elseif config.pbTareFlag then
checkTareEnable()end
end
function awtx.keypad.KEY_TARE_REPEAT()tareHoldFlag=tareHoldFlag+1
if tareHoldFlag==HowManyRepeatsMakeAHold then
if config.pbTareFlag then
awtx.weight.requestTareClear()displayCLEARED()elseif config.keyTareFlag then
awtx.weight.requestKeyboardTare(0)displayCLEARED()elseif config.presetTareFlag then
awtx.weight.requestPresetTare(0)displayCLEARED()else
displayCANT()end
end
end
function awtx.keypad.KEY_TARE_UP()if tareHoldFlag<HowManyRepeatsMakeAHold then
if config.presetTareFlag then
checkTareEnable()elseif config.pbTareFlag then
end
end
tareHoldFlag=0
end
function TARECOMPLETEEVENT(eventResult,eventResultString)if eventResult~=0 then
displayCANT()else
end
end
awtx.weight.registerTareCompleteEvent(TARECOMPLETEEVENT)function awtx.keypad.KEY_SELECT_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)selectHoldFlag=0
end
function awtx.keypad.KEY_SELECT_REPEAT()local usermode
selectHoldFlag=selectHoldFlag+1
if selectHoldFlag==HowManyRepeatsMakeAHold then
if system.modelStr=="ZM301"then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()supervisor.dynamicMenu(SetpointEdit,resolveCircular2)appEnableSetpoints()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif system.modelStr=="ZM303"then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()supervisor.dynamicMenu(SetpointEdit,resolveCircular2)appEnableSetpoints()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif system.modelStr=="ZQ375"then
elseif system.modelStr=="ZM305GTN"then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()supervisor.dynamicMenu(SetpointEdit,resolveCircular2)appEnableSetpoints()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif system.modelStr=="ZM305"then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()supervisor.dynamicMenu(SetpointEdit,resolveCircular2)appEnableSetpoints()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)else
end
end
end
function awtx.keypad.KEY_SELECT_UP()if selectHoldFlag<HowManyRepeatsMakeAHold then
awtx.weight.cycleActiveValue()end
selectHoldFlag=0
end
function SELECT_COMPLETE_EVENT(eventResult,eventResultString)end
function awtx.keypad.KEY_ZERO_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)zeroHoldFlag=0
if config.autoTareClear then
awtx.weight.requestTareClear()end
awtx.weight.requestZero()end
function awtx.keypad.KEY_ZERO_REPEAT()zeroHoldFlag=zeroHoldFlag+1
if zeroHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_ZERO_UP()if zeroHoldFlag<HowManyRepeatsMakeAHold then
end
zeroHoldFlag=0
end
function ZEROCOMPLETEEVENT(eventResult,eventResultString)if eventResult~=0 then
displayCANT()else
end
end
awtx.weight.registerZeroCompleteEvent(ZEROCOMPLETEEVENT)function awtx.keypad.KEY_PRINT_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)printHoldFlag=0
wt=awtx.weight.getCurrent(1)if config.printRTZ then
if not wt.inGrossBand then
awtx.weight.requestPrint()end
else
awtx.weight.requestPrint()end
end
function awtx.keypad.KEY_PRINT_REPEAT()printHoldFlag=printHoldFlag+1
if printHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_PRINT_UP()if printHoldFlag<HowManyRepeatsMakeAHold then
end
printHoldFlag=0
end
function PRINTCOMPLETEEVENT(eventResult,eventResultString)if eventResult~=0 then
displayCANT()else
wt=awtx.weight.getLastPrint()local tmpNetWt=wt.net
awtx.printer.PrintFmt(0)end
end
awtx.weight.registerPrintCompleteEvent(PRINTCOMPLETEEVENT)function PRINTERSTATUSEVENT(prnnum,param1,param2,param3,param4,param5)if prnnum>=0 then
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
end
awtx.printer.registerPrinterStatusEvent(PRINTERSTATUSEVENT)function PRINTERJOBSTATUSEVENT(prnnum,param1,param2,param3,param4,param5)if prnnum>=0 then
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
end
awtx.printer.registerPrinterJobStatusEvent(PRINTERJOBSTATUSEVENT)function printerstatusmsg(msgid)msg=""time=1000
if msgid==1 then
msg="Offline"elseif msgid==2 then
msg="Online "elseif msgid==5 then
msg="PaperLo"time=600
elseif msgid==9 then
msg="Ptr Err"time=600
end
if msg~=""then
local currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)local usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine(msg)awtx.os.sleep(time)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
end
function printerjobstatusmsg(msgid)msg=""time=1000
if msgid==1 then
msg="PrnDone"elseif msgid==2 then
msg="PrnAbrt"elseif msgid==3 then
msg="PrnFail"end
if msg~=""then
local currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)local usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine(msg)awtx.os.sleep(time)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
end
function awtx.keypad.KEY_UNITS_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)unitsHoldFlag=0
end
function awtx.keypad.KEY_UNITS_REPEAT()unitsHoldFlag=unitsHoldFlag+1
if unitsHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_UNITS_UP()if unitsHoldFlag<HowManyRepeatsMakeAHold then
awtx.weight.cycleUnits()end
unitsHoldFlag=0
end
function UNITS_COMPLETE_EVENT(eventResult,eventResultString)if eventResult~=0 then
displayCANT()else
calcAfterUnitsChange()end
end
awtx.weight.registerUnitsCompleteEvent(UNITS_COMPLETE_EVENT)function showSiteId()local usermode
local currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("SITE ID")awtx.os.sleep(showtime1)awtx.display.writeLine(string.format("%d",config.siteID))awtx.os.sleep(showtime2)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function showId()local usermode
local currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("ID")awtx.os.sleep(showtime1)awtx.display.writeLine(string.format("%d",ID.id))awtx.os.sleep(showtime2)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function enterId()local usermode
local idminVal,idmaxVal,newID,isEnterKey
local currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.writeLine("ID")awtx.os.sleep(showtime1)newID=ID.id
idminVal=0
idmaxVal=9999999
newID,isEnterKey=awtx.keypad.enterInteger(newID,idminVal,idmaxVal,entertime)awtx.display.setMode(usermode)if isEnterKey then
setId(newID)else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function setId(newID)local idminVal,idmaxVal
idminVal=0
idmaxVal=9999999
if newID>=idminVal and newID<=idmaxVal then
ID.id=tonumber(string.format("%d",newID))ID.setIDPrintTokens()end
end
function awtx.keypad.KEY_ID_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)idHoldFlag=0
end
function awtx.keypad.KEY_ID_REPEAT()idHoldFlag=idHoldFlag+1
if idHoldFlag==HowManyRepeatsMakeAHold then
enterId()end
end
function awtx.keypad.KEY_ID_UP()if idHoldFlag<HowManyRepeatsMakeAHold then
showId()end
idHoldFlag=0
end
function awtx.keypad.KEY_SETUP_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)setupHoldFlag=0
end
function awtx.keypad.KEY_SETUP_REPEAT()setupHoldFlag=setupHoldFlag+1
if setupHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_SETUP_UP()local usermode
if setupHoldFlag<HowManyRepeatsMakeAHold then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()supervisor.dynamicMenu(SetpointEdit,resolveCircular2)appEnableSetpoints()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)end
setupHoldFlag=0
end
function awtx.keypad.KEY_TARGET_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)targetHoldFlag=0
end
function awtx.keypad.KEY_TARGET_REPEAT()targetHoldFlag=targetHoldFlag+1
if targetHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_TARGET_UP()if targetHoldFlag<HowManyRepeatsMakeAHold then
end
targetHoldFlag=0
end
function awtx.keypad.KEY_SAMPLE_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)sampleHoldFlag=0
end
function awtx.keypad.KEY_SAMPLE_REPEAT()sampleHoldFlag=sampleHoldFlag+1
if sampleHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_SAMPLE_UP()if sampleHoldFlag<HowManyRepeatsMakeAHold then
end
sampleHoldFlag=0
end
function awtx.keypad.KEY_START_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)startHoldFlag=0
end
function awtx.keypad.KEY_START_REPEAT()startHoldFlag=startHoldFlag+1
if startHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_START_UP()if startHoldFlag<HowManyRepeatsMakeAHold then
end
startHoldFlag=0
end
function awtx.keypad.KEY_STOP_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)stopHoldFlag=0
end
function awtx.keypad.KEY_STOP_REPEAT()stopHoldFlag=stopHoldFlag+1
if stopHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_STOP_UP()if stopHoldFlag<HowManyRepeatsMakeAHold then
end
stopHoldFlag=0
end
function awtx.keypad.KEY_UNDER_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)underHoldFlag=0
end
function awtx.keypad.KEY_UNDER_REPEAT()underHoldFlag=underHoldFlag+1
if underHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_UNDER_UP()if underHoldFlag<HowManyRepeatsMakeAHold then
end
underHoldFlag=0
end
function awtx.keypad.KEY_OVER_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)overHoldFlag=0
end
function awtx.keypad.KEY_OVER_REPEAT()overHoldFlag=overHoldFlag+1
if overHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_OVER_UP()if overHoldFlag<HowManyRepeatsMakeAHold then
end
overHoldFlag=0
end
numericHoldFlag=0
function awtx.keypad.KEY_NUMERIC_DOWN(keyval)numericHoldFlag=0
end
function awtx.keypad.KEY_NUMERIC_REPEAT(keyval)numericHoldFlag=numericHoldFlag+1
if numericHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_NUMERIC_UP(keyval)if numericHoldFlag<HowManyRepeatsMakeAHold then
end
numericHoldFlag=0
end
function awtx.keypad.KEY_CLEAR_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)clearHoldFlag=0
end
function awtx.keypad.KEY_CLEAR_REPEAT()clearHoldFlag=clearHoldFlag+1
if clearHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_CLEAR_UP()if clearHoldFlag<HowManyRepeatsMakeAHold then
end
clearHoldFlag=0
end
function awtx.keypad.KEY_F1_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)f1HoldFlag=0
end
function awtx.keypad.KEY_F1_REPEAT()f1HoldFlag=f1HoldFlag+1
if f1HoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_F1_UP()if f1HoldFlag<HowManyRepeatsMakeAHold then
end
f1HoldFlag=0
end
function awtx.keypad.KEY_F2_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)f2HoldFlag=0
end
function awtx.keypad.KEY_F2_REPEAT()f2HoldFlag=f2HoldFlag+1
if f2HoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_F2_UP()if f2HoldFlag<HowManyRepeatsMakeAHold then
end
f2HoldFlag=0
end
function awtx.keypad.KEY_F3_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)f3HoldFlag=0
end
function awtx.keypad.KEY_F3_REPEAT()f3HoldFlag=f3HoldFlag+1
if f3HoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_F3_UP()if f3HoldFlag<HowManyRepeatsMakeAHold then
end
f3HoldFlag=0
end
function awtx.keypad.KEY_F4_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)f4HoldFlag=0
end
function awtx.keypad.KEY_F4_REPEAT()f4HoldFlag=f4HoldFlag+1
if f4HoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_F2_UP()if f4HoldFlag<HowManyRepeatsMakeAHold then
end
f4HoldFlag=0
end
function awtx.keypad.KEY_F5_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)f5HoldFlag=0
end
function awtx.keypad.KEY_F5_REPEAT()f5HoldFlag=f5HoldFlag+1
if f5HoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_F5_UP()if f5HoldFlag<HowManyRepeatsMakeAHold then
end
f5HoldFlag=0
end
function enterSclNum()local usermode
local sclminVal,sclmaxVal,newSclNum,isEnterKey
local currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.writeLine("Scl Num")awtx.os.sleep(showtime1)newSclNum=awtx.weight.getActiveScale()sclminVal=1
sclmaxVal=awtx.weight.getNumActiveScales()newSclNum,isEnterKey=awtx.keypad.enterInteger(newSclNum,sclminVal,sclmaxVal,entertime)awtx.display.setMode(usermode)if isEnterKey then
setSclNum(newSclNum)else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function setSclNum(newSclNum)local sclminVal,sclmaxVal
sclminVal=1
sclmaxVal=awtx.weight.getNumActiveScales()if newSclNum>=sclminVal and newSclNum<=sclmaxVal then
awtx.weight.setActiveScale(newSclNum)else
displayCANT()end
end
function awtx.keypad.KEY_SCALE_SELECT_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)scaleselectHoldFlag=0
end
function awtx.keypad.KEY_SCALE_SELECT_REPEAT()scaleselectHoldFlag=scaleselectHoldFlag+1
if scaleselectHoldFlag==HowManyRepeatsMakeAHold then
enterSclNum()end
end
function awtx.keypad.KEY_SCALE_SELECT_UP()if scaleselectHoldFlag<HowManyRepeatsMakeAHold then
awtx.weight.cycleActiveScale()end
scaleselectHoldFlag=0
end
function SCALE_SELECT_COMPLETE_EVENT(eventResult,eventResultString)end
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
local newID=0
local newFmt=0
local newTare=0
local newSelect=0
function RPNCOMPLETEEVENT(lastKey,enteredValue)local usermode
if lastKey=="ID"then
if rpnidflag==true then
newID=tonumber(enteredValue)setId(newID)else
displayCANT()end
elseif lastKey=="PRINT"then
if rpnprintflag==true then
newFmt=tonumber(enteredValue)wt=awtx.weight.getRefreshLastPrint()awtx.printer.PrintFmt(newFmt)else
displayCANT()end
elseif lastKey=="TARE"then
if rpntareflag==true then
if config.keyTareFlag then
newTare=tonumber(enteredValue)awtx.weight.requestKeyboardTare(newTare)elseif config.presetTareFlag then
end
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
end
awtx.keypad.registerNumberEntryRPN(RPNCOMPLETEEVENT)local mailServer="123.456.789.012"local loginUser="user@domain.com"local loginPassword="Password"local loginDomain="domain.com"local serverPort=25
local periodInMins=10
local authentType="none"function initEmail()displayWORD("initEmail")awtx.mail.setMailServerIP(mailServer)local ipAddrStr=awtx.mail.getMailServerIP()if mailServer==ipAddrStr then
displayWORD("IPServ")else
displayWORD("ERR 1")end
local result=awtx.mail.setAntiSpamPeriod(periodInMins)local timeMins=awtx.mail.getAntiSpamPeriod()if periodInMins==timeMins then
displayWORD("AntiSPM")else
displayWORD("ERR 2")end
awtx.mail.setAuthenticationType(authentType)local authentStr=awtx.mail.getAuthenticationType()if authentType==authentStr then
displayWORD("AUTHENT")else
displayWORD("ERR 3")end
awtx.mail.setAuthenticationDetails(loginUser,loginPassword,loginDomain)local userStr,passwordStr,domainStr=awtx.mail.getAuthenticationDetails()if loginUser==userStr then
displayWORD("USER")else
displayWORD("ERR 4")end
if loginPassword==passwordStr then
displayWORD("PASSWRD")else
displayWORD("ERR 5")end
if loginDomain==domainStr then
displayWORD("DOMAIN")else
displayWORD("ERR 6")end
local result=awtx.mail.setMailServerPort(serverPort)local port=awtx.mail.getMailServerPort()if serverPort==port then
displayWORD("Port")else
displayWORD("ERR 7")end
end
local mailTo="SendTo@domain.com"local mailFrom=loginUser
local copyTo="CopyTo@domain.com"local bodyTxt="This is the body of the email, if this had been a real email more stuff would have been here"local subjectCnt=0
local subjectTxt="Test subject ("..tostring(subjectCnt)..")"function sendEmail()subjectCnt=subjectCnt+1
subjectTxt="Test subject ("..tostring(subjectCnt)..")"local result=awtx.mail.send(bodyTxt,mailTo,mailFrom,copyTo,subjectTxt)if result==0 then
displayWORD("Sent "..tostring(subjectCnt))else
displayWORD("ERR "..tostring(result))end
end
plc={}function plc.luaVarChangeFunc(scaleNum,slotNum,varValue)if printTokens[slotNum].varLabel~="Invalid"then
if printTokens[slotNum].varFunct~=""then
printTokens[slotNum].varFunct(varValue)end
end
end
result=awtx.fmtPrint.registerVarChangeReqEvent(plc.luaVarChangeFunc)scanner={}scanner.msg1=""scanner.msg2=""function scanner.initPrintTokens()awtx.fmtPrint.varSet(89,scanner.msg1,"Message1",AWTX_LUA_STRING)printTokens[89].varName="scanner.msg1"printTokens[89].varLabel="Message1"printTokens[89].varType=AWTX_LUA_STRING
printTokens[89].varValue=scanner.msg1
printTokens[89].varFunct=""awtx.fmtPrint.varSet(90,scanner.msg2,"Message2",AWTX_LUA_STRING)printTokens[90].varName="scanner.msg2"printTokens[90].varLabel="Message2"printTokens[90].varType=AWTX_LUA_STRING
printTokens[90].varValue=scanner.msg2
printTokens[90].varFunct=""end
function scanner.setPrintTokens()awtx.fmtPrint.varSet(89,scanner.msg1,"Message1",AWTX_LUA_STRING)awtx.fmtPrint.varSet(90,scanner.msg2,"Message2",AWTX_LUA_STRING)end
local DB_FileLocation_AppConfig
function scanner.scannerDBInit()local simAppPath,dbFile,result
if awtx.os.amISimulator()then
simAppPath=awtx.os.applicationPath()DB_FileLocation_AppConfig=simAppPath..[[\AppConfig.db]]else
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]end
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblScannerConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:close()end
function scanner.extraStuffStore()local dbFile,result
local found=false
local sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblScannerConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")found=false
for row in dbFile:rows("SELECT varID FROM tblScannerConfig WHERE varID = 'channelNum1enable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblScannerConfig (varID, value) VALUES ('channelNum1enable', '%d')",scanner.channelNum1enable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblScannerConfig SET value = '%d' WHERE varID = 'channelNum1enable'",scanner.channelNum1enable)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblScannerConfig WHERE varID = 'channelNum2enable'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblScannerConfig (varID, value) VALUES ('channelNum2enable', '%d')",scanner.channelNum2enable)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblScannerConfig SET value = '%d' WHERE varID = 'channelNum2enable'",scanner.channelNum2enable)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblScannerConfig WHERE varID = 'autoPrintAfterScan'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblScannerConfig (varID, value) VALUES ('autoPrintAfterScan', '%d')",scanner.autoPrintAfterScan)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblScannerConfig SET value = '%d' WHERE varID = 'autoPrintAfterScan'",scanner.autoPrintAfterScan)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()end
function scanner.extraStuffRecall()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblScannerConfig (varID TEXT PRIMARY KEY, value TEXT)")found=false
for row in dbFile:rows("SELECT varID, value FROM tblScannerConfig WHERE varID = 'channelNum1enable'")do
found=true
scanner.channelNum1enable=tonumber(row[2])end
if found==false then
scanner.channelNum1enable=scanner.channelNum1enableDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblScannerConfig WHERE varID = 'channelNum2enable'")do
found=true
scanner.channelNum2enable=tonumber(row[2])end
if found==false then
scanner.channelNum2enable=scanner.channelNum2enableDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblScannerConfig WHERE varID = 'autoPrintAfterScan'")do
found=true
scanner.autoPrintAfterScan=tonumber(row[2])end
if found==false then
scanner.autoPrintAfterScan=scanner.autoPrintAfterScanDefault
end
dbFile:close()end
function scanner.editchannelNum1enable(label)local tempchannelNum1enable,isEnterKey
scanner.extraStuffRecall()tempchannelNum1enable=scanner.channelNum1enable
tempchannelNum1enable,isEnterKey=awtx.keypad.selectList("OFF,ON",tempchannelNum1enable)awtx.display.writeLine(label)if isEnterKey then
scanner.channelNum1enable=tempchannelNum1enable
scanner.extraStuffStore()scanner.enable()else
end
end
function scanner.editchannelNum2enable(label)local tempchannelNum2enable,isEnterKey
scanner.extraStuffRecall()tempchannelNum2enable=scanner.channelNum2enable
tempchannelNum2enable,isEnterKey=awtx.keypad.selectList("OFF,ON",tempchannelNum2enable)awtx.display.writeLine(label)if isEnterKey then
scanner.channelNum2enable=tempchannelNum2enable
scanner.extraStuffStore()scanner.enable()else
end
end
function scanner.editautoPrintAfterScan(label)local tempautoPrintAfterScan,isEnterKey
scanner.extraStuffRecall()tempautoPrintAfterScan=scanner.autoPrintAfterScan
tempautoPrintAfterScan,isEnterKey=awtx.keypad.selectList("OFF,ON",tempautoPrintAfterScan)awtx.display.writeLine(label)if isEnterKey then
scanner.autoPrintAfterScan=tempautoPrintAfterScan
scanner.extraStuffStore()else
end
end
scanner.channelNum1=1
scanner.channelNum1enableDefault=0
scanner.channelNum1enable=scanner.channelNum1enableDefault
scanner.bufcount1=0
scanner.rxDataStr1=""scanner.readCount1=0
scanner.channelNum2=2
scanner.channelNum2enableDefault=0
scanner.channelNum2enable=scanner.channelNum2enableDefault
scanner.bufcount2=0
scanner.rxDataStr2=""scanner.readCount2=0
scanner.autoPrintAfterScanDefault=0
scanner.autoPrintAfterScan=scanner.autoPrintAfterScanDefault
function SERIAL_EOM_RX_EVENT(channelNumber)if channelNumber==scanner.channelNum1 then
if scanner.channelNum1enable==1 then
scanner.bufcount1=awtx.serial.getRxCount(channelNumber)scanner.rxDataStr1,scanner.readCount1=awtx.serial.getRx(channelNumber)scanner.bufcount1=awtx.serial.getRxCount(channelNumber)scanner.msg1=string.sub(scanner.rxDataStr1,1,-2)scanner.setPrintTokens()if scanner.autoPrintAfterScan==1 then
awtx.weight.requestPrint()end
end
elseif channelNumber==scanner.channelNum2 then
if scanner.channelNum2enable==1 then
scanner.bufcount2=awtx.serial.getRxCount(channelNumber)scanner.rxDataStr2,scanner.readCount2=awtx.serial.getRx(channelNumber)scanner.bufcount2=awtx.serial.getRxCount(channelNumber)scanner.msg2=string.sub(scanner.rxDataStr2,1,-2)scanner.setPrintTokens()if scanner.autoPrintAfterScan==1 then
awtx.weight.requestPrint()end
end
else
end
end
function scanner.enable()if scanner.channelNum1enable==1 then
awtx.serial.setEomChar(scanner.channelNum1,13)awtx.serial.registerEomEvent(scanner.channelNum1,SERIAL_EOM_RX_EVENT)else
awtx.serial.unregisterEomEvent(scanner.channelNum1)end
if scanner.channelNum2enable==1 then
awtx.serial.setEomChar(scanner.channelNum2,13)awtx.serial.registerEomEvent(scanner.channelNum2,SERIAL_EOM_RX_EVENT)else
awtx.serial.unregisterEomEvent(scanner.channelNum2)end
end
scanner.initPrintTokens()scanner.scannerDBInit()scanner.extraStuffRecall()scanner.enable()