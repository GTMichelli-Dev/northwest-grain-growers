--[[
--## VERSION=0.1.0
--## DATE=2012-02-17  12:00
--## DESC=This is the Batching App (App5Batch.lua)

]]
require("ReqDebug")AppName="Batch"SerialNumber=awtx.setupMenu.getSerialNumber()AWTX_LUA_UNDEFINED=0
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
function calcAfterUnitsChange()system=awtx.hardware.getSystem(1)config=awtx.weight.getConfig(1)wt=awtx.weight.getCurrent(1)batching.setOutputValues()end
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
calcPreactBefore=0
calcPreactAfter=1
calcPreactWhen=calcPreactAfter
rpnidflag=false
rpnprintflag=true
rpntareflag=false
rpnotherflag=false
if config.keyTareFlag then
rpntareflag=true
end
require("ReqScaleKeys")require("ReqTare")require("ReqBatching")require("ReqBattery")require("ReqAppMenu")f1Press=0
function awtx.keypad.KEY_F1_UP()if f1HoldFlag<HowManyRepeatsMakeAHold then
if(bit.band(awtx.ports.getOutputs(),1)==1)or(bit.band(awtx.ports.getOutputs(),2)==2)or(bit.band(awtx.ports.getOutputs(),4)==4)then
f1Press=0
batching.stopBatch()else
f1Press=1
batching.startBatch()end
end
f1HoldFlag=0
end
function TARECOMPLETEEVENT(eventResult,eventResultString)if eventResult~=0 then
displayCANT()else
wt=awtx.weight.getCurrent(1)batching.TareSuccessful()end
end
awtx.weight.registerTareCompleteEvent(TARECOMPLETEEVENT)function awtx.keypad.KEY_START_DOWN()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)startHoldFlag=0
end
function awtx.keypad.KEY_START_REPEAT()startHoldFlag=startHoldFlag+1
if startHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_START_UP()if startHoldFlag<HowManyRepeatsMakeAHold then
batching.startBatch()end
startHoldFlag=0
end
function awtx.keypad.KEY_STOP_DOWN()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)stopHoldFlag=0
end
function awtx.keypad.KEY_STOP_REPEAT()stopHoldFlag=stopHoldFlag+1
if stopHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_STOP_UP()if stopHoldFlag<HowManyRepeatsMakeAHold then
batching.stopBatch()end
stopHoldFlag=0
end
T1={text="Super",key=1,action="MENU",variable="SuperMenu"}T2={text="EXIT",key=2,action="FUNC",callThis=supervisor.exitFunction}topMenu={T1,T2}SuperMenu1={text=" SETPNT",key=1,action="MENU",variable="SetpointSetupMenu"}SuperMenu2={text=" TARE  ",key=2,action="MENU",variable="TareSetupMenu",show={callThis=get_PresetTareEnabled,val1=1}}SuperMenu3={text=" BATCH ",key=3,action="MENU",variable="BatchSetupMenu"}SuperMenu4={text="BATTERY",key=4,action="MENU",variable="BatterySetupMenu"}SuperMenu5={text=" BACK  ",key=5,action="MENU",variable="topMenu",subMenu=1}SuperMenu={SuperMenu1,SuperMenu2,SuperMenu3,SuperMenu4,SuperMenu5}SetpointSetupMenu1={text=" Edit  ",key=1,action="MENU",variable="SetpointSetupEdit"}SetpointSetupMenu2={text=" Print ",key=2,action="FUNC",callThis=batching.BatchingDBReport}SetpointSetupMenu3={text=" Reset ",key=3,action="FUNC",callThis=batching.BatchingDBReset}SetpointSetupMenu4={text=" BACK  ",key=4,action="MENU",variable="SuperMenu",subMenu=1}SetpointSetupMenu={SetpointSetupMenu1,SetpointSetupMenu2,SetpointSetupMenu3,SetpointSetupMenu4}SetpointSetupEdit1={text=" OUT1  ",key=1,action="FUNC",callThis=batching.enterBatchOutValue1}SetpointSetupEdit2={text=" OUT2  ",key=2,action="FUNC",callThis=batching.enterBatchOutValue2}SetpointSetupEdit3={text=" OUT3  ",key=3,action="FUNC",callThis=batching.enterBatchOutValue3}SetpointSetupEdit4={text="PREACT1",key=4,action="FUNC",callThis=batching.enterBatchPreactValue1}SetpointSetupEdit5={text="PREACT2",key=5,action="FUNC",callThis=batching.enterBatchPreactValue2}SetpointSetupEdit6={text="PREACT3",key=6,action="FUNC",callThis=batching.enterBatchPreactValue3}SetpointSetupEdit7={text="  IN1  ",key=7,action="FUNC",callThis=batching.enterBatchInputValue1}SetpointSetupEdit8={text="  IN2  ",key=8,action="FUNC",callThis=batching.enterBatchInputValue2}SetpointSetupEdit9={text="  IN3  ",key=9,action="FUNC",callThis=batching.enterBatchInputValue3}SetpointSetupEdit10={text=" BACK  ",key=10,action="MENU",variable="SetpointSetupMenu",subMenu=1}SetpointSetupEdit={SetpointSetupEdit1,SetpointSetupEdit2,SetpointSetupEdit3,SetpointSetupEdit4,SetpointSetupEdit5,SetpointSetupEdit6,SetpointSetupEdit7,SetpointSetupEdit8,SetpointSetupEdit9,SetpointSetupEdit10}TareSetupMenu1={text=" Edit  ",key=1,action="MENU",variable="TareSetupEdit"}TareSetupMenu2={text=" Print ",key=2,action="FUNC",callThis=tare.TareDBReport}TareSetupMenu3={text=" Reset ",key=3,action="FUNC",callThis=tare.TareDBReset}TareSetupMenu4={text=" BACK  ",key=4,action="MENU",variable="SuperMenu",subMenu=2}TareSetupMenu={TareSetupMenu1,TareSetupMenu2,TareSetupMenu3,TareSetupMenu4}TareSetupEdit1={text="Tare 1 ",key=1,action="FUNC",callThis=tare.editPresetTare1}TareSetupEdit2={text="Tare 2 ",key=2,action="FUNC",callThis=tare.editPresetTare2}TareSetupEdit3={text="Tare 3 ",key=3,action="FUNC",callThis=tare.editPresetTare3}TareSetupEdit4={text="Tare 4 ",key=4,action="FUNC",callThis=tare.editPresetTare4}TareSetupEdit5={text="Tare 5 ",key=5,action="FUNC",callThis=tare.editPresetTare5}TareSetupEdit6={text="Tare 6 ",key=6,action="FUNC",callThis=tare.editPresetTare6}TareSetupEdit7={text="Tare 7 ",key=7,action="FUNC",callThis=tare.editPresetTare7}TareSetupEdit8={text="Tare 8 ",key=8,action="FUNC",callThis=tare.editPresetTare8}TareSetupEdit9={text="Tare 9 ",key=9,action="FUNC",callThis=tare.editPresetTare9}TareSetupEdit10={text="Tare 10",key=10,action="FUNC",callThis=tare.editPresetTare10}TareSetupEdit11={text=" BACK  ",key=11,action="MENU",variable="TareSetupMenu",subMenu=1}TareSetupEdit={TareSetupEdit1,TareSetupEdit2,TareSetupEdit3,TareSetupEdit4,TareSetupEdit5,TareSetupEdit6,TareSetupEdit7,TareSetupEdit8,TareSetupEdit9,TareSetupEdit10,TareSetupEdit11}BatchSetupMenu1={text="TYPE   ",key=1,action="FUNC",callThis=batching.editBatchingType}BatchSetupMenu2={text="MODE   ",key=2,action="FUNC",callThis=batching.editBatchingMode}BatchSetupMenu3={text="DISP   ",key=3,action="FUNC",callThis=batching.editBatchingDisp}BatchSetupMenu4={text="PREACT ",key=4,action="FUNC",callThis=batching.editBatchingPreact}BatchSetupMenu5={text="NEG FIL",key=5,action="FUNC",callThis=batching.editBatchingNegFil}BatchSetupMenu6={text="BACK   ",key=6,action="MENU",variable="SuperMenu",subMenu=3}BatchSetupMenu={BatchSetupMenu1,BatchSetupMenu2,BatchSetupMenu3,BatchSetupMenu4,BatchSetupMenu5,BatchSetupMenu6}BatterySetupMenu1={text=" Enable",key=1,action="FUNC",callThis=battery.editBatteryEnable}BatterySetupMenu2={text=" Tmout ",key=2,action="FUNC",callThis=battery.editBatterySetting}BatterySetupMenu3={text=" BACK  ",key=3,action="MENU",variable="SuperMenu",subMenu=4}BatterySetupMenu={BatterySetupMenu1,BatterySetupMenu2,BatterySetupMenu3}resolveCircular1={topMenu=topMenu,SuperMenu=SuperMenu,SetpointSetupMenu=SetpointSetupMenu,SetpointSetupEdit=SetpointSetupEdit,TareSetupMenu=TareSetupMenu,TareSetupEdit=TareSetupEdit,BatchSetupMenu=BatchSetupMenu,BatterySetupMenu=BatterySetupMenu}SetpointEdit1={text=" Batch ",key=1,action="MENU",variable="SetpointEditor"}SetpointEdit2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}SetpointEdit={SetpointEdit1,SetpointEdit2}SetpointEditor1={text=" OUT1  ",key=1,action="FUNC",callThis=batching.enterBatchOutValue1}SetpointEditor2={text=" OUT2  ",key=2,action="FUNC",callThis=batching.enterBatchOutValue2}SetpointEditor3={text=" OUT3  ",key=3,action="FUNC",callThis=batching.enterBatchOutValue3}SetpointEditor4={text="PREACT1",key=4,action="FUNC",callThis=batching.enterBatchPreactValue1}SetpointEditor5={text="PREACT2",key=5,action="FUNC",callThis=batching.enterBatchPreactValue2}SetpointEditor6={text="PREACT3",key=6,action="FUNC",callThis=batching.enterBatchPreactValue3}SetpointEditor7={text=" BACK  ",key=7,action="MENU",variable="SetpointEdit",subMenu=1}SetpointEditor={SetpointEditor1,SetpointEditor2,SetpointEditor3,SetpointEditor4,SetpointEditor5,SetpointEditor6,SetpointEditor7}resolveCircular2={SetpointEdit=SetpointEdit,SetpointEditor=SetpointEditor}ParamTopMenu1={text=" Param ",key=1,action="MENU",variable="OutputModeEdit"}ParamTopMenu2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}ParamTopMenu={ParamTopMenu1,ParamTopMenu2}OutputModeEdit1={text="OutMode",key=1,action="MENU",variable="OutputModeEditor"}OutputModeEdit2={text=" BACK  ",key=2,action="MENU",variable="ParamTopMenu",subMenu=1}OutputModeEdit={OutputModeEdit1,OutputModeEdit2}OutputModeEditor1={text=" OUT1  ",key=1,action="FUNC",callThis=menuCANT}OutputModeEditor2={text=" OUT2  ",key=2,action="FUNC",callThis=menuCANT}OutputModeEditor3={text=" OUT3  ",key=3,action="FUNC",callThis=menuCANT}OutputModeEditor4={text=" BACK  ",key=4,action="MENU",variable="OutputModeEdit",subMenu=1}OutputModeEditor={OutputModeEditor1,OutputModeEditor2,OutputModeEditor3,OutputModeEditor4}resolveCircular7={ParamTopMenu=ParamTopMenu,OutputModeEdit=OutputModeEdit,OutputModeEditor=OutputModeEditor}function appEnterSuperMenu()end
function appExitSuperMenu()end
batching.basedoncount=false
function appDisableSetpoints()batching.disableSetpointsOutputs()end
function appEnableSetpoints()if batching.BatchingNegFil==0 then
batching.refreshSetpointsOutputs(batching.setpt_batching_up)else
batching.refreshSetpointsOutputs(batching.setpt_batching_down)end
end
function appRefreshBatterySettings()if batching.BatchingNegFil==0 then
batching.refreshSetpointsOutputs(batching.setpt_batching_up)else
batching.refreshSetpointsOutputs(batching.setpt_batching_down)end
end
appRefreshBatterySettings()batching.refreshSetpointsInputs()function appInput(setpointNum,inputState)if setpointNum==1 then
batching.setpointIn1Handler(setpointNum,inputState)end
if setpointNum==2 then
batching.setpointIn2Handler(setpointNum,inputState)end
if setpointNum==3 then
batching.setpointIn3Handler(setpointNum,inputState)end
end
ID.initIDPrintTokens()tare.initTarePrintTokens()batching.initBatchPrintTokens()SMAPC.initPrintTokens()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)printAppDebug("\n Startup App5Batch.lua non-event ")while true do
awtx.os.systemEvents(100)end