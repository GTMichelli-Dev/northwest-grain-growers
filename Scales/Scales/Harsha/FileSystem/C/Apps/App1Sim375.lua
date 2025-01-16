--[[
--## VERSION=0.1.0
--## DATE=2012-02-17 12:00
--## DESC=This is the SIM375 Checkmate App (App1Sim375.lua)

]]
require("ReqDebug")AppName="SIM375"SerialNumber=awtx.setupMenu.getSerialNumber()AWTX_LUA_UNDEFINED=0
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
function calcAfterUnitsChange()awtx.weight.graphEnable(1,0)system=awtx.hardware.getSystem(1)config=awtx.weight.getConfig(1)wt=awtx.weight.getCurrent(1)setpoint.setOutputValues()checkweigh.updateWeightBasedSettingsAfterUnitsChange()if checkweigh.CheckgraphEnableFlag==1 then
awtx.weight.graphEnable(1,checkweigh.CheckGraphType)end
end
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
CHECK_NONE=0
SIMPLECHECK=1
SIM375=2
MID375=3
ADV375=4
PER375=5
GRAD375=6
tmpPackRunCnt=0
if config.keyTareFlag then
rpntareflag=false
end
require("ReqScaleKeys")require("ReqStats")require("ReqTare")require("ReqSetpoint")require("ReqCheckWeigh")require("ReqBattery")require("ReqAppMenu")function awtx.keypad.KEY_TARE_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)tareHoldFlag=0
displayCANT()end
function awtx.keypad.KEY_TARE_UP()local usermode
if tareHoldFlag<HowManyRepeatsMakeAHold then
end
tareHoldFlag=0
end
function TARECOMPLETEEVENT(eventResult,eventResultString)local usermode
if eventResult~=0 then
displayCANT()else
if checkweigh.CheckgraphEnableFlag==0 then
else
checkweigh.sampleTarget()end
end
end
awtx.weight.registerTareCompleteEvent(TARECOMPLETEEVENT)function awtx.keypad.KEY_SELECT_UP()if selectHoldFlag<HowManyRepeatsMakeAHold then
if checkweigh.CheckgraphEnableFlag==0 then
checkweigh.CheckgraphEnableFlag=1
checkweigh.storeCheckgraphEnableFlag()checkweigh.refreshCheckGraph()else
checkweigh.CheckgraphEnableFlag=0
checkweigh.storeCheckgraphEnableFlag()checkweigh.refreshCheckGraph()end
end
selectHoldFlag=0
end
function awtx.keypad.KEY_ZERO_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)zeroHoldFlag=0
awtx.weight.requestZeroCheckWeigh()end
function awtx.keypad.KEY_PRINT_DOWN()local usermode
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
wt=awtx.weight.getLastPrint(1)tmpPackRunCnt=tmpPackRunCnt+1
checkweigh.doTransaction()checkweigh.doPrintPopulate()awtx.printer.PrintFmt(0)if checkweigh.StoreBeforePrint==false then
checkweigh.transactionStore()end
end
end
awtx.weight.registerPrintCompleteEvent(PRINTCOMPLETEEVENT)function awtx.keypad.KEY_UNDER_DOWN()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)underHoldFlag=0
end
function awtx.keypad.KEY_UNDER_REPEAT()underHoldFlag=underHoldFlag+1
if underHoldFlag==HowManyRepeatsMakeAHold then
checkweigh.showTolLo()end
end
function awtx.keypad.KEY_UNDER_UP()if underHoldFlag<HowManyRepeatsMakeAHold then
wt=awtx.weight.getCurrent(1)checkweigh.enterTolLo()end
underHoldFlag=0
end
function awtx.keypad.KEY_TARGET_DOWN()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)targetHoldFlag=0
end
function awtx.keypad.KEY_TARGET_REPEAT()local usermode
targetHoldFlag=targetHoldFlag+1
if targetHoldFlag==HowManyRepeatsMakeAHold then
displayCANT()end
end
function awtx.keypad.KEY_TARGET_UP()if targetHoldFlag<HowManyRepeatsMakeAHold then
wt=awtx.weight.getCurrent(1)if not wt.inGrossBand then
checkweigh.CheckgraphEnableFlag=1
checkweigh.storeCheckgraphEnableFlag()getPbTare()else
checkweigh.setTarget(0)checkweigh.CheckgraphEnableFlag=0
checkweigh.storeCheckgraphEnableFlag()checkweigh.refreshCheckGraph()end
end
targetHoldFlag=0
end
function awtx.keypad.KEY_OVER_DOWN()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)overHoldFlag=0
end
function awtx.keypad.KEY_OVER_REPEAT()overHoldFlag=overHoldFlag+1
if overHoldFlag==HowManyRepeatsMakeAHold then
checkweigh.showTolHi()end
end
function awtx.keypad.KEY_OVER_UP()if overHoldFlag<HowManyRepeatsMakeAHold then
wt=awtx.weight.getCurrent(1)checkweigh.enterTolHi()end
overHoldFlag=0
end
T1={text="Super",key=1,action="MENU",variable="SuperMenu"}T2={text="EXIT",key=2,action="FUNC",callThis=supervisor.exitFunction}topMenu={T1,T2}SuperMenu1={text=" SETPNT",key=1,action="MENU",variable="SetpointSetupMenu"}SuperMenu2={text=" CHECK ",key=2,action="MENU",variable="CheckSetupMenu"}SuperMenu3={text="BATTERY",key=3,action="MENU",variable="BatterySetupMenu"}SuperMenu4={text=" BACK  ",key=4,action="MENU",variable="topMenu",subMenu=1}SuperMenu={SuperMenu1,SuperMenu2,SuperMenu3,SuperMenu4}SetpointSetupMenu1={text=" Edit  ",key=1,action="MENU",variable="SetpointSetupEdit"}SetpointSetupMenu2={text=" Print ",key=2,action="FUNC",callThis=setpoint.SetpointDBReportIn}SetpointSetupMenu3={text=" Reset ",key=3,action="FUNC",callThis=setpoint.SetpointDBReset}SetpointSetupMenu4={text=" BACK  ",key=4,action="MENU",variable="SuperMenu",subMenu=1}SetpointSetupMenu={SetpointSetupMenu1,SetpointSetupMenu2,SetpointSetupMenu3,SetpointSetupMenu4}SetpointSetupEdit1={text=" ANNUN ",key=1,action="FUNC",callThis=setpoint.editInvertingOutputFlag}SetpointSetupEdit2={text="  IN1  ",key=2,action="FUNC",callThis=setpoint.editIn1}SetpointSetupEdit3={text="  IN2  ",key=3,action="FUNC",callThis=setpoint.editIn2}SetpointSetupEdit4={text="  IN3  ",key=4,action="FUNC",callThis=setpoint.editIn3}SetpointSetupEdit5={text=" BACK  ",key=5,action="MENU",variable="SetpointSetupMenu",subMenu=1}SetpointSetupEdit={SetpointSetupEdit1,SetpointSetupEdit2,SetpointSetupEdit3,SetpointSetupEdit4,SetpointSetupEdit5}CheckSetupMenu1={text="OUTPUTS",key=1,action="FUNC",callThis=checkweigh.editCheckOutputType}CheckSetupMenu2={text="OUT-GZB",key=2,action="FUNC",callThis=checkweigh.editCheckOutputGZB}CheckSetupMenu3={text="USEGDIV",key=3,action="FUNC",callThis=checkweigh.editCheckUnderSegDiv}CheckSetupMenu4={text="OSEGDIV",key=4,action="FUNC",callThis=checkweigh.editCheckOverSegDiv}CheckSetupMenu5={text="Reset  ",key=5,action="FUNC",callThis=checkweigh.CheckweighDBReset}CheckSetupMenu6={text=" BACK  ",key=6,action="MENU",variable="SuperMenu",subMenu=2}CheckSetupMenu={CheckSetupMenu1,CheckSetupMenu2,CheckSetupMenu3,CheckSetupMenu4,CheckSetupMenu5,CheckSetupMenu6}BatterySetupMenu1={text=" Enable",key=1,action="FUNC",callThis=battery.editBatteryEnable}BatterySetupMenu2={text=" Tmout ",key=2,action="FUNC",callThis=battery.editBatterySetting}BatterySetupMenu3={text=" BACK  ",key=3,action="MENU",variable="SuperMenu",subMenu=3}BatterySetupMenu={BatterySetupMenu1,BatterySetupMenu2,BatterySetupMenu3}resolveCircular1={topMenu=topMenu,SuperMenu=SuperMenu,SetpointSetupMenu=SetpointSetupMenu,SetpointSetupEdit=SetpointSetupEdit,CheckSetupMenu=CheckSetupMenu,BatterySetupMenu=BatterySetupMenu}ParamTopMenu1={text=" Param ",key=1,action="MENU",variable="OutputModeEdit"}ParamTopMenu2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}ParamTopMenu={ParamTopMenu1,ParamTopMenu2}OutputModeEdit1={text="OutMode",key=1,action="MENU",variable="OutputModeEditor"}OutputModeEdit2={text=" BACK  ",key=2,action="MENU",variable="ParamTopMenu",subMenu=1}OutputModeEdit={OutputModeEdit1,OutputModeEdit2}OutputModeEditor1={text=" OUT1  ",key=1,action="FUNC",callThis=setpoint.editOutputMode1}OutputModeEditor2={text=" OUT2  ",key=2,action="FUNC",callThis=setpoint.editOutputMode2}OutputModeEditor3={text=" OUT3  ",key=3,action="FUNC",callThis=setpoint.editOutputMode3}OutputModeEditor4={text=" BACK  ",key=4,action="MENU",variable="OutputModeEdit",subMenu=1}OutputModeEditor={OutputModeEditor1,OutputModeEditor2,OutputModeEditor3,OutputModeEditor4}resolveCircular7={ParamTopMenu=ParamTopMenu,OutputModeEdit=OutputModeEdit,OutputModeEditor=OutputModeEditor}checkweigh.CheckWeighMode=SIM375
function appEnterSuperMenu()end
function appExitSuperMenu()checkweigh.refreshCheckGraph()end
function appDisableSetpoints()if battery.BatteryEnable==1 then
checkweigh.refreshSetpointsDisabledBattery()else
checkweigh.refreshSetpointsDisabled()end
end
function appEnableSetpoints()if battery.BatteryEnable==1 then
checkweigh.refreshSetpointsRejectAcceptBattery()else
checkweigh.refreshSetpointsUnderTargetOver()end
end
function appRefreshBatterySettings()if battery.BatteryEnable==1 then
checkweigh.refreshSetpointsRejectAcceptBattery()else
checkweigh.refreshSetpointsUnderTargetOver()end
end
appRefreshBatterySettings()checkweigh.refreshSetpointsInputs()function appInput(setpointNum,inputState)if setpointNum==1 then
checkweigh.setpointIn1Handler(setpointNum,inputState)end
if setpointNum==2 then
checkweigh.setpointIn2Handler(setpointNum,inputState)end
if setpointNum==3 then
checkweigh.setpointIn3Handler(setpointNum,inputState)end
end
checkweigh.initAll()checkweigh.DigitsFlag=1
checkweigh.GraphFlag=1
ID.initIDPrintTokens()tare.initTarePrintTokens()setpoint.initSetpointPrintTokens()checkweigh.initCheckPrintTokens()transaction.initTransPrintTokens()Stat.initStatsPrintTokens()SMAPC.initPrintTokens()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)printAppDebug("\n Startup App1Sim375.lua non-event ")while true do
awtx.os.systemEvents(100)end