--[[
--## VERSION=0.1.0
--## DATE=2012-02-17  12:00
--## DESC=This is the PER375 Checkmate App (App4Per375.lua)

]]
require("ReqDebug")AppName="PER375"SerialNumber=awtx.setupMenu.getSerialNumber()AWTX_LUA_UNDEFINED=0
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
function calcAfterUnitsChange()awtx.weight.graphEnable(1,0)system=awtx.hardware.getSystem(1)config=awtx.weight.getConfig(1)wt=awtx.weight.getCurrent(1)setpoint.setOutputValues()checkweigh.updateWeightBasedSettingsAfterUnitsChange()plu.updateWeightBasedSettingsAfterUnitsChange()awtx.weight.graphEnable(1,checkweigh.CheckGraphType)end
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
require("ReqScaleKeys")require("ReqTare")require("ReqSetpoint")require("ReqStats")require("ReqCheckWeigh")require("ReqBattery")require("ReqPLU")require("ReqAppMenu")function awtx.keypad.KEY_TARE_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)tareHoldFlag=0
displayCANT()end
function awtx.keypad.KEY_TARE_UP()local usermode
if tareHoldFlag<HowManyRepeatsMakeAHold then
end
tareHoldFlag=0
end
function awtx.keypad.KEY_ZERO_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)zeroHoldFlag=0
awtx.weight.requestZeroCheckWeigh()end
function awtx.keypad.KEY_SELECT_UP()if selectHoldFlag<HowManyRepeatsMakeAHold then
if checkweigh.DigitsFlag==1 then
awtx.weight.cycleActiveValue()end
end
selectHoldFlag=0
end
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
tmpPackRunCnt=0
if checkweigh.CheckStatPackage~=2 then
checkweigh.doStatsPackage()end
if checkweigh.PrintTotalFlag==1 then
displayPRNTOT()wt=awtx.weight.getCurrent(1)awtx.printer.PrintFmt(checkweigh.TotalFmt)end
if checkweigh.ClearTotalFlag==1 then
checkweigh.newClrAccum()if checkweigh.ClearPLUTotalFlag==1 then
plu.newClrAccum()end
end
if checkweigh.CheckStatPackage~=2 then
checkweigh.clrStatsPackage()end
end
end
function awtx.keypad.KEY_PRINT_UP()if printHoldFlag<HowManyRepeatsMakeAHold then
end
printHoldFlag=0
end
function PRINTCOMPLETEEVENT(eventResult,eventResultString)if eventResult~=0 then
displayCANT()else
wt=awtx.weight.getLastPrint(1)tmpPackRunCnt=tmpPackRunCnt+1
checkweigh.doPercentTransactionWithPLU()plu.doPLUAccum()checkweigh.doPrintPopulatePercent()if checkweigh.CheckStatPackage==2 then
checkweigh.doStatsPackage()end
awtx.printer.PrintFmt(0)if checkweigh.StoreBeforePrint==false then
checkweigh.transactionStore()end
if tmpPackRunCnt>=checkweigh.PackRunCnt and checkweigh.PackRunCnt~=0 then
tmpPackRunCnt=0
if checkweigh.CheckStatPackage~=2 then
checkweigh.doStatsPackage()end
if checkweigh.PrintTotalFlag==1 then
displayPRNTOT()wt=awtx.weight.getCurrent(1)awtx.printer.PrintFmt(checkweigh.TotalFmt)end
if checkweigh.ClearTotalFlag==1 then
checkweigh.newClrAccum()if checkweigh.ClearPLUTotalFlag==1 then
plu.newClrAccum()end
end
if checkweigh.CheckStatPackage~=2 then
checkweigh.clrStatsPackage()end
end
end
end
awtx.weight.registerPrintCompleteEvent(PRINTCOMPLETEEVENT)function awtx.keypad.KEY_F1_UP()if f1HoldFlag<HowManyRepeatsMakeAHold then
plu.PLUChannelSelect()end
f1HoldFlag=0
end
function awtx.keypad.KEY_UNDER_DOWN()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)underHoldFlag=0
end
function awtx.keypad.KEY_UNDER_REPEAT()underHoldFlag=underHoldFlag+1
if underHoldFlag==HowManyRepeatsMakeAHold then
if checkweigh.CheckWeighType==0 then
checkweigh.showTargLo()else
checkweigh.showTolLo()end
end
end
function awtx.keypad.KEY_UNDER_UP()if underHoldFlag<HowManyRepeatsMakeAHold then
if plu.PLUNumber==0 then
wt=awtx.weight.getCurrent(1)checkweigh.enterTolLo()plu.pluDBStore()end
end
underHoldFlag=0
end
function awtx.keypad.KEY_TARGET_DOWN()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)targetHoldFlag=0
end
function awtx.keypad.KEY_TARGET_REPEAT()targetHoldFlag=targetHoldFlag+1
if targetHoldFlag==HowManyRepeatsMakeAHold then
checkweigh.showTarget()end
end
function awtx.keypad.KEY_TARGET_UP()if targetHoldFlag<HowManyRepeatsMakeAHold then
wt=awtx.weight.getCurrent(1)if not wt.inGrossBand then
checkTareEnable()else
if plu.PLUNumber==0 then
checkweigh.enterTargetWithIncrement()plu.pluDBStore()end
end
end
targetHoldFlag=0
end
function awtx.keypad.KEY_OVER_DOWN()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)overHoldFlag=0
end
function awtx.keypad.KEY_OVER_REPEAT()overHoldFlag=overHoldFlag+1
if overHoldFlag==HowManyRepeatsMakeAHold then
if checkweigh.CheckWeighType==0 then
checkweigh.showTargHi()else
checkweigh.showTolHi()end
end
end
function awtx.keypad.KEY_OVER_UP()if overHoldFlag<HowManyRepeatsMakeAHold then
if plu.PLUNumber==0 then
wt=awtx.weight.getCurrent(1)checkweigh.enterTolHi()plu.pluDBStore()end
end
overHoldFlag=0
end
T1={text="Super",key=1,action="MENU",variable="SuperMenu"}T2={text="EXIT",key=2,action="FUNC",callThis=supervisor.exitFunction}topMenu={T1,T2}SuperMenu1={text=" SETPNT",key=1,action="MENU",variable="SetpointSetupMenu"}SuperMenu2={text=" TARE  ",key=2,action="MENU",variable="TareSetupMenu",show={callThis=get_PresetTareEnabled,val1=1}}SuperMenu3={text=" CHECK ",key=3,action="MENU",variable="CheckSetupMenu"}SuperMenu4={text=" PLU   ",key=4,action="MENU",variable="PLUSetupMenu"}SuperMenu5={text="BATTERY",key=5,action="MENU",variable="BatterySetupMenu"}SuperMenu6={text=" BACK  ",key=6,action="MENU",variable="topMenu",subMenu=1}SuperMenu={SuperMenu1,SuperMenu2,SuperMenu3,SuperMenu4,SuperMenu5,SuperMenu6}SetpointSetupMenu1={text=" Edit  ",key=1,action="MENU",variable="SetpointSetupEdit"}SetpointSetupMenu2={text=" Print ",key=2,action="FUNC",callThis=setpoint.SetpointDBReportIn}SetpointSetupMenu3={text=" Reset ",key=3,action="FUNC",callThis=setpoint.SetpointDBReset}SetpointSetupMenu4={text=" BACK  ",key=4,action="MENU",variable="SuperMenu",subMenu=1}SetpointSetupMenu={SetpointSetupMenu1,SetpointSetupMenu2,SetpointSetupMenu3,SetpointSetupMenu4}SetpointSetupEdit1={text=" ANNUN ",key=1,action="FUNC",callThis=setpoint.editInvertingOutputFlag}SetpointSetupEdit2={text="  IN1  ",key=2,action="FUNC",callThis=setpoint.editIn1}SetpointSetupEdit3={text="  IN2  ",key=3,action="FUNC",callThis=setpoint.editIn2}SetpointSetupEdit4={text="  IN3  ",key=4,action="FUNC",callThis=setpoint.editIn3}SetpointSetupEdit5={text=" BACK  ",key=5,action="MENU",variable="SetpointSetupMenu",subMenu=1}SetpointSetupEdit={SetpointSetupEdit1,SetpointSetupEdit2,SetpointSetupEdit3,SetpointSetupEdit4,SetpointSetupEdit5}TareSetupMenu1={text=" Edit  ",key=1,action="MENU",variable="TareSetupEdit"}TareSetupMenu2={text=" Print ",key=2,action="FUNC",callThis=tare.TareDBReport}TareSetupMenu3={text=" Reset ",key=3,action="FUNC",callThis=tare.TareDBReset}TareSetupMenu4={text=" BACK  ",key=4,action="MENU",variable="SuperMenu",subMenu=2}TareSetupMenu={TareSetupMenu1,TareSetupMenu2,TareSetupMenu3,TareSetupMenu4}TareSetupEdit1={text="Tare 1 ",key=1,action="FUNC",callThis=tare.editPresetTare1}TareSetupEdit2={text="Tare 2 ",key=2,action="FUNC",callThis=tare.editPresetTare2}TareSetupEdit3={text="Tare 3 ",key=3,action="FUNC",callThis=tare.editPresetTare3}TareSetupEdit4={text="Tare 4 ",key=4,action="FUNC",callThis=tare.editPresetTare4}TareSetupEdit5={text="Tare 5 ",key=5,action="FUNC",callThis=tare.editPresetTare5}TareSetupEdit6={text="Tare 6 ",key=6,action="FUNC",callThis=tare.editPresetTare6}TareSetupEdit7={text="Tare 7 ",key=7,action="FUNC",callThis=tare.editPresetTare7}TareSetupEdit8={text="Tare 8 ",key=8,action="FUNC",callThis=tare.editPresetTare8}TareSetupEdit9={text="Tare 9 ",key=9,action="FUNC",callThis=tare.editPresetTare9}TareSetupEdit10={text="Tare 10",key=10,action="FUNC",callThis=tare.editPresetTare10}TareSetupEdit11={text=" BACK  ",key=11,action="MENU",variable="TareSetupMenu",subMenu=1}TareSetupEdit={TareSetupEdit1,TareSetupEdit2,TareSetupEdit3,TareSetupEdit4,TareSetupEdit5,TareSetupEdit6,TareSetupEdit7,TareSetupEdit8,TareSetupEdit9,TareSetupEdit10,TareSetupEdit11}CheckSetupMenu1={text="OUTPUTS",key=1,action="FUNC",callThis=checkweigh.editCheckOutputType}CheckSetupMenu2={text="OUT-GZB",key=2,action="FUNC",callThis=checkweigh.editCheckOutputGZB}CheckSetupMenu3={text="USEGDIV",key=3,action="FUNC",callThis=checkweigh.editCheckUnderSegDiv}CheckSetupMenu4={text="OSEGDIV",key=4,action="FUNC",callThis=checkweigh.editCheckOverSegDiv}CheckSetupMenu5={text="Prt TOT",key=5,action="FUNC",callThis=checkweigh.editPrintTotalFlag}CheckSetupMenu6={text="ToT Fmt",key=6,action="FUNC",callThis=checkweigh.editTotalFmt}CheckSetupMenu7={text="CLR TOT",key=7,action="FUNC",callThis=checkweigh.editClearTotalFlag}CheckSetupMenu8={text="Digits ",key=8,action="FUNC",callThis=checkweigh.editDigitsFlag}CheckSetupMenu9={text="Graph  ",key=9,action="FUNC",callThis=checkweigh.editGraphFlag}CheckSetupMenu10={text="STATS  ",key=10,action="FUNC",callThis=checkweigh.editCheckStatPackage}CheckSetupMenu11={text="PACKRUN",key=11,action="FUNC",callThis=checkweigh.editPackRunCnt}CheckSetupMenu12={text="TYPE   ",key=12,action="FUNC",callThis=checkweigh.editCheckWeighType}CheckSetupMenu13={text="Reset  ",key=13,action="FUNC",callThis=checkweigh.CheckweighDBReset}CheckSetupMenu14={text=" BACK  ",key=14,action="MENU",variable="SuperMenu",subMenu=3}CheckSetupMenu={CheckSetupMenu1,CheckSetupMenu2,CheckSetupMenu3,CheckSetupMenu4,CheckSetupMenu5,CheckSetupMenu6,CheckSetupMenu7,CheckSetupMenu8,CheckSetupMenu9,CheckSetupMenu10,CheckSetupMenu11,CheckSetupMenu12,CheckSetupMenu13,CheckSetupMenu14}PLUSetupMenu1={text=" Edit  ",key=1,action="FUNC",callThis=plu.editPLU}PLUSetupMenu2={text=" Print ",key=2,action="FUNC",callThis=plu.PLUDBReport}PLUSetupMenu3={text=" Import",key=3,action="FUNC",callThis=plu.importPLU}PLUSetupMenu4={text=" Export",key=4,action="FUNC",callThis=plu.exportPLU}PLUSetupMenu5={text=" Reset ",key=5,action="FUNC",callThis=plu.PLUDBReset}PLUSetupMenu6={text=" BACK  ",key=6,action="MENU",variable="SuperMenu",subMenu=4}PLUSetupMenu={PLUSetupMenu1,PLUSetupMenu2,PLUSetupMenu3,PLUSetupMenu4,PLUSetupMenu5,PLUSetupMenu6}PLUSetupEdit1={text=" TARGET",key=1,action="FUNC",callThis=plu.editPLUTARG}PLUSetupEdit2={text=" TARGLO",key=2,action="FUNC",callThis=plu.editPLUTARGLO,show={callThis=supervisor.CheckWeighType,val1=0}}PLUSetupEdit3={text=" TARGHI",key=3,action="FUNC",callThis=plu.editPLUTARGHI,show={callThis=supervisor.CheckWeighType,val1=0}}PLUSetupEdit4={text=" TOL-LO",key=4,action="FUNC",callThis=plu.editPLUTOLLO,show={callThis=supervisor.CheckWeighType,val1=1}}PLUSetupEdit5={text=" TOL-HI",key=5,action="FUNC",callThis=plu.editPLUTOLHI,show={callThis=supervisor.CheckWeighType,val1=1}}PLUSetupEdit6={text=" BACK  ",key=6,action="MENU",variable="PLUSetupMenu",subMenu=1}PLUSetupEdit={PLUSetupEdit1,PLUSetupEdit2,PLUSetupEdit3,PLUSetupEdit4,PLUSetupEdit5,PLUSetupEdit6}BatterySetupMenu1={text=" Enable",key=1,action="FUNC",callThis=battery.editBatteryEnable}BatterySetupMenu2={text=" Tmout ",key=2,action="FUNC",callThis=battery.editBatterySetting}BatterySetupMenu3={text=" BACK  ",key=3,action="MENU",variable="SuperMenu",subMenu=5}BatterySetupMenu={BatterySetupMenu1,BatterySetupMenu2,BatterySetupMenu3}resolveCircular1={topMenu=topMenu,SuperMenu=SuperMenu,SetpointSetupMenu=SetpointSetupMenu,SetpointSetupEdit=SetpointSetupEdit,TareSetupMenu=TareSetupMenu,TareSetupEdit=TareSetupEdit,CheckSetupMenu=CheckSetupMenu,PLUSetupMenu=PLUSetupMenu,PLUSetupEdit=PLUSetupEdit,BatterySetupMenu=BatterySetupMenu}ParamTopMenu1={text=" Param ",key=1,action="MENU",variable="OutputModeEdit"}ParamTopMenu2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}ParamTopMenu={ParamTopMenu1,ParamTopMenu2}OutputModeEdit1={text="OutMode",key=1,action="MENU",variable="OutputModeEditor"}OutputModeEdit2={text=" BACK  ",key=2,action="MENU",variable="ParamTopMenu",subMenu=1}OutputModeEdit={OutputModeEdit1,OutputModeEdit2}OutputModeEditor1={text=" OUT1  ",key=1,action="FUNC",callThis=setpoint.editOutputMode1}OutputModeEditor2={text=" OUT2  ",key=2,action="FUNC",callThis=setpoint.editOutputMode2}OutputModeEditor3={text=" OUT3  ",key=3,action="FUNC",callThis=setpoint.editOutputMode3}OutputModeEditor4={text=" BACK  ",key=4,action="MENU",variable="OutputModeEdit",subMenu=1}OutputModeEditor={OutputModeEditor1,OutputModeEditor2,OutputModeEditor3,OutputModeEditor4}resolveCircular7={ParamTopMenu=ParamTopMenu,OutputModeEdit=OutputModeEdit,OutputModeEditor=OutputModeEditor}checkweigh.CheckWeighMode=PER375
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
plu.initAll()checkweigh.initAll()ID.initIDPrintTokens()tare.initTarePrintTokens()setpoint.initSetpointPrintTokens()checkweigh.initCheckPrintTokens()transaction.initTransPrintTokens()Stat.initStatsPrintTokens()plu.initPLUPrintTokens()SMAPC.initPrintTokens()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)printAppDebug("\n Startup App4Per375.lua non-event ")while true do
awtx.os.systemEvents(100)end