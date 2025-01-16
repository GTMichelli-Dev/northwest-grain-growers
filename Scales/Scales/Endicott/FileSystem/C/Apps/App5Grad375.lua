--[[
--## VERSION=0.1.0
--## DATE=2012-02-17  12:00
--## DESC=This is the GRAD375 Checkmate App (App5Grad375.lua)

]]
require("ReqDebug")AppName="GRAD375"SerialNumber=awtx.setupMenu.getSerialNumber()AWTX_LUA_UNDEFINED=0
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
function calcAfterUnitsChange()system=awtx.hardware.getSystem(1)config=awtx.weight.getConfig(1)wt=awtx.weight.getCurrent(1)grading.updateWeightBasedSettingsAfterUnitsChange()setpoint.setOutputValues()end
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
if config.keyTareFlag then
rpntareflag=false
end
require("ReqScaleKeys")require("ReqTare")require("ReqSetpoint")require("ReqGrading")require("ReqBattery")require("ReqAppMenu")function TARECOMPLETEEVENT(eventResult,eventResultString)if eventResult~=0 then
displayCANT()else
if grading.GradType==NEG_GRADING then
else
end
end
end
awtx.weight.registerTareCompleteEvent(TARECOMPLETEEVENT)function ZEROCOMPLETEEVENT(eventResult,eventResultString)local result
if eventResult~=0 then
displayCANT()else
if grading.GradType==NEG_GRADING then
result=awtx.setPoint.outputClr(9)else
end
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
if grading.GradType==NEG_GRADING then
else
grading.aboveGZB()end
wt=awtx.weight.getLastPrint(1)awtx.printer.PrintFmt(0)end
end
awtx.weight.registerPrintCompleteEvent(PRINTCOMPLETEEVENT)function awtx.keypad.KEY_UNDER_DOWN()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)underHoldFlag=0
end
function awtx.keypad.KEY_UNDER_REPEAT()underHoldFlag=underHoldFlag+1
if underHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_UNDER_UP()if underHoldFlag<HowManyRepeatsMakeAHold then
end
underHoldFlag=0
end
function awtx.keypad.KEY_TARGET_DOWN()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)targetHoldFlag=0
end
function awtx.keypad.KEY_TARGET_REPEAT()targetHoldFlag=targetHoldFlag+1
if targetHoldFlag==HowManyRepeatsMakeAHold then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()supervisor.dynamicMenu(GradingEdit,resolveCircular2)appEnableSetpoints()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)end
end
function awtx.keypad.KEY_TARGET_UP()if targetHoldFlag<HowManyRepeatsMakeAHold then
end
targetHoldFlag=0
end
function awtx.keypad.KEY_OVER_DOWN()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)overHoldFlag=0
end
function awtx.keypad.KEY_OVER_REPEAT()overHoldFlag=overHoldFlag+1
if overHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_OVER_UP()if overHoldFlag<HowManyRepeatsMakeAHold then
end
overHoldFlag=0
end
T1={text="Super",key=1,action="MENU",variable="SuperMenu"}T2={text="EXIT",key=2,action="FUNC",callThis=supervisor.exitFunction}topMenu={T1,T2}SuperMenu1={text=" SETPNT",key=1,action="MENU",variable="SetpointSetupMenu"}SuperMenu2={text=" TARE  ",key=2,action="MENU",variable="TareSetupMenu",show={callThis=get_PresetTareEnabled,val1=1}}SuperMenu3={text="GRADING",key=3,action="MENU",variable="GradingEditing"}SuperMenu4={text="BATTERY",key=4,action="MENU",variable="BatterySetupMenu"}SuperMenu5={text=" BACK  ",key=5,action="MENU",variable="topMenu",subMenu=1}SuperMenu={SuperMenu1,SuperMenu2,SuperMenu3,SuperMenu4,SuperMenu5}SetpointSetupMenu1={text=" Edit  ",key=1,action="MENU",variable="SetpointSetupEdit"}SetpointSetupMenu2={text=" Print ",key=2,action="FUNC",callThis=setpoint.SetpointDBReportIn}SetpointSetupMenu3={text=" Reset ",key=3,action="FUNC",callThis=setpoint.SetpointDBReset}SetpointSetupMenu4={text=" BACK  ",key=4,action="MENU",variable="SuperMenu",subMenu=1}SetpointSetupMenu={SetpointSetupMenu1,SetpointSetupMenu2,SetpointSetupMenu3,SetpointSetupMenu4}SetpointSetupEdit1={text=" ANNUN ",key=1,action="FUNC",callThis=setpoint.editInvertingOutputFlag}SetpointSetupEdit2={text="  IN1  ",key=2,action="FUNC",callThis=setpoint.editIn1}SetpointSetupEdit3={text="  IN2  ",key=3,action="FUNC",callThis=setpoint.editIn2}SetpointSetupEdit4={text="  IN3  ",key=4,action="FUNC",callThis=setpoint.editIn3}SetpointSetupEdit5={text=" BACK  ",key=5,action="MENU",variable="SetpointSetupMenu",subMenu=1}SetpointSetupEdit={SetpointSetupEdit1,SetpointSetupEdit2,SetpointSetupEdit3,SetpointSetupEdit4,SetpointSetupEdit5}TareSetupMenu1={text=" Edit  ",key=1,action="MENU",variable="TareSetupEdit"}TareSetupMenu2={text=" Print ",key=2,action="FUNC",callThis=tare.TareDBReport}TareSetupMenu3={text=" Reset ",key=3,action="FUNC",callThis=tare.TareDBReset}TareSetupMenu4={text=" BACK  ",key=4,action="MENU",variable="SuperMenu",subMenu=2}TareSetupMenu={TareSetupMenu1,TareSetupMenu2,TareSetupMenu3,TareSetupMenu4}TareSetupEdit1={text="Tare 1 ",key=1,action="FUNC",callThis=tare.editPresetTare1}TareSetupEdit2={text="Tare 2 ",key=2,action="FUNC",callThis=tare.editPresetTare2}TareSetupEdit3={text="Tare 3 ",key=3,action="FUNC",callThis=tare.editPresetTare3}TareSetupEdit4={text="Tare 4 ",key=4,action="FUNC",callThis=tare.editPresetTare4}TareSetupEdit5={text="Tare 5 ",key=5,action="FUNC",callThis=tare.editPresetTare5}TareSetupEdit6={text="Tare 6 ",key=6,action="FUNC",callThis=tare.editPresetTare6}TareSetupEdit7={text="Tare 7 ",key=7,action="FUNC",callThis=tare.editPresetTare7}TareSetupEdit8={text="Tare 8 ",key=8,action="FUNC",callThis=tare.editPresetTare8}TareSetupEdit9={text="Tare 9 ",key=9,action="FUNC",callThis=tare.editPresetTare9}TareSetupEdit10={text="Tare 10",key=10,action="FUNC",callThis=tare.editPresetTare10}TareSetupEdit11={text=" BACK  ",key=11,action="MENU",variable="TareSetupMenu",subMenu=1}TareSetupEdit={TareSetupEdit1,TareSetupEdit2,TareSetupEdit3,TareSetupEdit4,TareSetupEdit5,TareSetupEdit6,TareSetupEdit7,TareSetupEdit8,TareSetupEdit9,TareSetupEdit10,TareSetupEdit11}GradingEditing1={text="GRAD 1 ",key=1,action="FUNC",callThis=grading.editGrad1}GradingEditing2={text="GRAD 2 ",key=2,action="FUNC",callThis=grading.editGrad2}GradingEditing3={text="GRAD 3 ",key=3,action="FUNC",callThis=grading.editGrad3}GradingEditing4={text="GRAD 4 ",key=4,action="FUNC",callThis=grading.editGrad4}GradingEditing5={text="GRAD 5 ",key=5,action="FUNC",callThis=grading.editGrad5}GradingEditing6={text="GRAD 6 ",key=6,action="FUNC",callThis=grading.editGrad6}GradingEditing7={text="GRAD 7 ",key=7,action="FUNC",callThis=grading.editGrad7}GradingEditing8={text="GRAD 8 ",key=8,action="FUNC",callThis=grading.editGrad8}GradingEditing9={text="GRAD 9 ",key=9,action="FUNC",callThis=grading.editGrad9}GradingEditing10={text="GRAD 10",key=10,action="FUNC",callThis=grading.editGrad10}GradingEditing11={text="GRAD 11",key=11,action="FUNC",callThis=grading.editGrad11}GradingEditing12={text="TYPE   ",key=12,action="FUNC",callThis=grading.editGradType}GradingEditing13={text="TARE   ",key=13,action="FUNC",callThis=grading.editTareType,show={callThis=get_NegGradingEnabled,val1=1}}GradingEditing14={text="PRINT  ",key=14,action="FUNC",callThis=grading.editPrintType,show={callThis=get_NegGradingEnabled,val1=1}}GradingEditing15={text="LABELS ",key=15,action="MENU",variable="GradingLabel"}GradingEditing16={text=" BACK  ",key=16,action="MENU",variable="SuperMenu",subMenu=3}GradingEditing={GradingEditing1,GradingEditing2,GradingEditing3,GradingEditing4,GradingEditing5,GradingEditing6,GradingEditing7,GradingEditing8,GradingEditing9,GradingEditing10,GradingEditing11,GradingEditing12,GradingEditing13,GradingEditing14,GradingEditing15,GradingEditing16}GradingLabel0={text="UNDER  ",key=1,action="FUNC",callThis=grading.editLabel0}GradingLabel1={text="LABEL1 ",key=2,action="FUNC",callThis=grading.editLabel1}GradingLabel2={text="LABEL2 ",key=3,action="FUNC",callThis=grading.editLabel2}GradingLabel3={text="LABEL3 ",key=4,action="FUNC",callThis=grading.editLabel3}GradingLabel4={text="LABEL4 ",key=5,action="FUNC",callThis=grading.editLabel4}GradingLabel5={text="LABEL5 ",key=6,action="FUNC",callThis=grading.editLabel5}GradingLabel6={text="LABEL6 ",key=7,action="FUNC",callThis=grading.editLabel6}GradingLabel7={text="LABEL7 ",key=8,action="FUNC",callThis=grading.editLabel7}GradingLabel8={text="LABEL8 ",key=9,action="FUNC",callThis=grading.editLabel8}GradingLabel9={text="LABEL9 ",key=10,action="FUNC",callThis=grading.editLabel9}GradingLabel10={text="LABEL10",key=11,action="FUNC",callThis=grading.editLabel10}GradingLabel11={text="OVER   ",key=12,action="FUNC",callThis=grading.editLabel11}GradingLabel12={text=" BACK  ",key=13,action="MENU",variable="GradingEditing",subMenu=15}GradingLabel={GradingLabel0,GradingLabel1,GradingLabel2,GradingLabel3,GradingLabel4,GradingLabel5,GradingLabel6,GradingLabel7,GradingLabel8,GradingLabel9,GradingLabel10,GradingLabel11,GradingLabel12}BatterySetupMenu1={text=" Enable",key=1,action="FUNC",callThis=battery.editBatteryEnable}BatterySetupMenu2={text=" Tmout ",key=2,action="FUNC",callThis=battery.editBatterySetting}BatterySetupMenu3={text=" BACK  ",key=3,action="MENU",variable="SuperMenu",subMenu=4}BatterySetupMenu={BatterySetupMenu1,BatterySetupMenu2,BatterySetupMenu3}resolveCircular1={topMenu=topMenu,SuperMenu=SuperMenu,SetpointSetupMenu=SetpointSetupMenu,SetpointSetupEdit=SetpointSetupEdit,TareSetupMenu=TareSetupMenu,TareSetupEdit=TareSetupEdit,GradingEditing=GradingEditing,GradingLabel=GradingLabel,BatterySetupMenu=BatterySetupMenu}GradingEdit1={text="GRADING",key=1,action="MENU",variable="GradingEditor"}GradingEdit2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}GradingEdit={GradingEdit1,GradingEdit2}GradingEditor1={text="GRAD 1 ",key=1,action="FUNC",callThis=grading.editGrad1}GradingEditor2={text="GRAD 2 ",key=2,action="FUNC",callThis=grading.editGrad2}GradingEditor3={text="GRAD 3 ",key=3,action="FUNC",callThis=grading.editGrad3}GradingEditor4={text="GRAD 4 ",key=4,action="FUNC",callThis=grading.editGrad4}GradingEditor5={text="GRAD 5 ",key=5,action="FUNC",callThis=grading.editGrad5}GradingEditor6={text="GRAD 6 ",key=6,action="FUNC",callThis=grading.editGrad6}GradingEditor7={text="GRAD 7 ",key=7,action="FUNC",callThis=grading.editGrad7}GradingEditor8={text="GRAD 8 ",key=8,action="FUNC",callThis=grading.editGrad8}GradingEditor9={text="GRAD 9 ",key=9,action="FUNC",callThis=grading.editGrad9}GradingEditor10={text="GRAD 10",key=10,action="FUNC",callThis=grading.editGrad10}GradingEditor11={text="GRAD 11",key=11,action="FUNC",callThis=grading.editGrad11}GradingEditor12={text=" BACK  ",key=12,action="MENU",variable="GradingEdit",subMenu=1}GradingEditor={GradingEditor1,GradingEditor2,GradingEditor3,GradingEditor4,GradingEditor5,GradingEditor6,GradingEditor7,GradingEditor8,GradingEditor9,GradingEditor10,GradingEditor11,GradingEditor12}resolveCircular2={GradingEdit=GradingEdit,GradingEditor=GradingEditor}ParamTopMenu1={text=" Param ",key=1,action="MENU",variable="OutputModeEdit"}ParamTopMenu2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}ParamTopMenu={ParamTopMenu1,ParamTopMenu2}OutputModeEdit1={text="OutMode",key=1,action="MENU",variable="OutputModeEditor"}OutputModeEdit2={text=" BACK  ",key=2,action="MENU",variable="ParamTopMenu",subMenu=1}OutputModeEdit={OutputModeEdit1,OutputModeEdit2}OutputModeEditor1={text=" OUT1  ",key=1,action="FUNC",callThis=setpoint.editOutputMode1}OutputModeEditor2={text=" OUT2  ",key=2,action="FUNC",callThis=setpoint.editOutputMode2}OutputModeEditor3={text=" OUT3  ",key=3,action="FUNC",callThis=setpoint.editOutputMode3}OutputModeEditor4={text=" BACK  ",key=4,action="MENU",variable="OutputModeEdit",subMenu=1}OutputModeEditor={OutputModeEditor1,OutputModeEditor2,OutputModeEditor3,OutputModeEditor4}resolveCircular7={ParamTopMenu=ParamTopMenu,OutputModeEdit=OutputModeEdit,OutputModeEditor=OutputModeEditor}function appEnterSuperMenu()end
function appExitSuperMenu()end
function appDisableSetpoints()if battery.BatteryEnable==1 then
grading.refreshSetpointsDisabledBattery()else
grading.refreshSetpointsDisabled()end
end
function appEnableSetpoints()if battery.BatteryEnable==1 then
grading.refreshSetpointsDisabledBattery()else
grading.refreshSetpointsDisabled()end
end
function appRefreshBatterySettings()if battery.BatteryEnable==1 then
grading.refreshSetpointsDisabledBattery()else
grading.refreshSetpointsDisabled()end
end
appRefreshBatterySettings()grading.refreshSetpointsInputs()function appInput(setpointNum,inputState)if setpointNum==1 then
grading.setpointIn1Handler(setpointNum,inputState)end
if setpointNum==2 then
grading.setpointIn2Handler(setpointNum,inputState)end
if setpointNum==3 then
grading.setpointIn3Handler(setpointNum,inputState)end
end
ID.initIDPrintTokens()tare.initTarePrintTokens()setpoint.initSetpointPrintTokens()grading.initGradPrintTokens(0.0)SMAPC.initPrintTokens()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)printAppDebug("\n Startup App5Grad375.lua non-event ")while true do
awtx.os.systemEvents(100)end