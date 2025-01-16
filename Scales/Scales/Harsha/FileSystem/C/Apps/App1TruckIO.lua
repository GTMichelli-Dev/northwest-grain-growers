--[[
--## VERSION=0.1.0
--## DATE=2012-02-17  12:00
--## DESC=This is the Truck In/Out App (App1TruckIO.lua)

]]
require("ReqDebug")AppName="TRUCKIO"SerialNumber=awtx.setupMenu.getSerialNumber()AWTX_LUA_UNDEFINED=0
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
function calcAfterUnitsChange()system=awtx.hardware.getSystem(1)config=awtx.weight.getConfig(1)wt=awtx.weight.getCurrent(1)truck.clrNewVars()setpoint.setOutputValues()end
wt={}system={}config={}printTokens={}system=awtx.hardware.getSystem(1)config=awtx.weight.getConfig(1)wt=awtx.weight.getCurrent(1)if system.modelStr=="ZM401"then
elseif system.modelStr=="ZM405"then
elseif system.modelStr=="ZM505SD3"then
elseif system.modelStr=="ZM505SD4"then
require("ReqDisplay")elseif system.modelStr=="ZM510"then
require("ReqDisplay")elseif system.modelStr=="ZM605SD3"then
elseif system.modelStr=="ZM605SD4"then
require("ReqDisplay")elseif system.modelStr=="ZM615"then
require("ReqDisplay")elseif system.modelStr=="2060"then
require("ReqDisplay")else
end
separatorChar=config.displaySeparator
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
f2HoldFlag=0
f3HoldFlag=0
f4HoldFlag=0
f5HoldFlag=0
scaleselectHoldFlag=0
rpnidflag=false
rpnprintflag=true
rpntareflag=false
rpnotherflag=false
rpnf1flag=true
rpntargetflag=true
rpnsampleflag=true
TRUCK_TYPE_NONE=0
TRUCK_TYPE_INOUT=1
TRUCK_TYPE_FLEET=2
if config.keyTareFlag then
rpntareflag=true
end
if system.modelStr=="2060"then
require("ReqScaleKeys2060")else
require("ReqScaleKeys")end
require("ReqTare")require("ReqSetpoint")require("ReqTruckIO")require("ReqBattery")require("ReqAppMenu")result=awtx.keypad.set_RPN_mode(1)function awtx.keypad.KEY_TARE_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)tareHoldFlag=0
if truck.truckPrintType==TRUCK_TYPE_INOUT then
displayCANT()elseif truck.truckPrintType==TRUCK_TYPE_FLEET then
displayCANT()else
if config.presetTareFlag then
elseif config.keyTareFlag then
elseif config.pbTareFlag then
checkTareEnable()end
end
end
function awtx.keypad.KEY_TARE_REPEAT()tareHoldFlag=tareHoldFlag+1
if tareHoldFlag==HowManyRepeatsMakeAHold then
if config.pbTareFlag then
awtx.weight.requestTareClear()displayCLEARED()elseif config.keyTareFlag then
awtx.weight.requestKeyboardTare(0)displayCLEARED()elseif config.presetTareFlag then
awtx.weight.requestPresetTare(0)displayCLEARED()else
displayCANT()end
truck.setTruckPrintType(TRUCK_TYPE_NONE)truck.clrNewVars()truck.setTruckPrintTokens()end
end
function awtx.keypad.KEY_TARE_UP()if tareHoldFlag<HowManyRepeatsMakeAHold then
if truck.truckPrintType==TRUCK_TYPE_INOUT then
displayCANT()elseif truck.truckPrintType==TRUCK_TYPE_FLEET then
displayCANT()else
if config.presetTareFlag then
checkTareEnable()elseif config.keyTareFlag then
displayCANT()elseif config.pbTareFlag then
end
end
end
tareHoldFlag=0
end
function TARECOMPLETEEVENT(eventResult,eventResultString)if eventResult~=0 then
displayCANT()else
end
end
awtx.weight.registerTareCompleteEvent(TARECOMPLETEEVENT)function awtx.keypad.KEY_SELECT_UP()if selectHoldFlag<HowManyRepeatsMakeAHold then
if truck.truckPrintType==TRUCK_TYPE_INOUT then
displayCANT()elseif truck.truckPrintType==TRUCK_TYPE_FLEET then
displayCANT()else
awtx.weight.cycleActiveValue()end
end
selectHoldFlag=0
end
function SELECT_COMPLETE_EVENT(eventResult,eventResultString)end
function awtx.keypad.KEY_PRINT_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)printHoldFlag=0
wt=awtx.weight.getCurrent(1)if not wt.inGrossBand then
if truck.truckPrintType==TRUCK_TYPE_INOUT then
curActVal=awtx.weight.getActiveValue()if curActVal~=VAL_GROSS then
displayCANT()else
awtx.weight.requestPrint()end
elseif truck.truckPrintType==TRUCK_TYPE_FLEET then
awtx.weight.requestPrint()else
awtx.weight.requestPrint()end
end
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
wt=awtx.weight.getLastPrint()if truck.truckPrintType==TRUCK_TYPE_NONE then
awtx.printer.PrintFmt(0)if truck.LiteEnableFlag==2 then
truck.setLiteGreen()end
elseif truck.truckPrintType==TRUCK_TYPE_INOUT then
truck.printTruckIO()if truck.LiteEnableFlag==2 then
truck.setLiteGreen()end
elseif truck.truckPrintType==TRUCK_TYPE_FLEET then
truck.printTruckFleet()if truck.LiteEnableFlag==2 then
truck.setLiteGreen()end
end
end
end
awtx.weight.registerPrintCompleteEvent(PRINTCOMPLETEEVENT)function awtx.keypad.KEY_ID_REPEAT()idHoldFlag=idHoldFlag+1
if idHoldFlag==HowManyRepeatsMakeAHold then
enterId()end
end
lblF1="IN/OUT"lblF2="FLEET"lblF3="UNITS"lblF4="REPORT"lblF5=" DISP"function awtx.keypad.KEY_F1_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)f1HoldFlag=0
end
function awtx.keypad.KEY_F1_REPEAT()f1HoldFlag=f1HoldFlag+1
if f1HoldFlag==HowManyRepeatsMakeAHold then
truck.setTruckPrintType(TRUCK_TYPE_NONE)displayWORD(" IO CLR")truck.clrNewVars()truck.setTruckPrintTokens()end
end
function awtx.keypad.KEY_F1_UP()if f1HoldFlag<HowManyRepeatsMakeAHold then
truck.selectTruckIOID()end
f1HoldFlag=0
end
function awtx.keypad.KEY_F2_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)f2HoldFlag=0
end
function awtx.keypad.KEY_F2_REPEAT()f2HoldFlag=f2HoldFlag+1
if f2HoldFlag==HowManyRepeatsMakeAHold then
truck.setTruckPrintType(TRUCK_TYPE_NONE)displayWORD("FLT CLR")truck.clrNewVars()truck.setTruckPrintTokens()end
end
function awtx.keypad.KEY_F2_UP()if f2HoldFlag<HowManyRepeatsMakeAHold then
truck.selectTruckFleetID()end
f2HoldFlag=0
end
function awtx.keypad.KEY_F3_UP()if f3HoldFlag<HowManyRepeatsMakeAHold then
awtx.weight.cycleUnits()end
f3HoldFlag=0
end
function awtx.keypad.KEY_F4_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)f4HoldFlag=0
end
function awtx.keypad.KEY_F4_REPEAT()f4HoldFlag=f4HoldFlag+1
if f4HoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_F4_UP()if f4HoldFlag<HowManyRepeatsMakeAHold then
truck.TruckReport("Report")end
f4HoldFlag=0
end
function awtx.keypad.KEY_F5_UP()if f5HoldFlag<HowManyRepeatsMakeAHold then
cycleWeightDisplay()end
f5HoldFlag=0
end
function awtx.keypad.KEY_SAMPLE_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)sampleHoldFlag=0
end
function awtx.keypad.KEY_SAMPLE_REPEAT()sampleHoldFlag=sampleHoldFlag+1
if sampleHoldFlag==HowManyRepeatsMakeAHold then
truck.setTruckPrintType(TRUCK_TYPE_NONE)displayWORD("FLT CLR")truck.clrNewVars()truck.setTruckPrintTokens()end
end
function awtx.keypad.KEY_SAMPLE_UP()if sampleHoldFlag<HowManyRepeatsMakeAHold then
truck.selectTruckFleetID()end
sampleHoldFlag=0
end
function awtx.keypad.KEY_TARGET_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)targetHoldFlag=0
end
function awtx.keypad.KEY_TARGET_REPEAT()targetHoldFlag=targetHoldFlag+1
if targetHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_TARGET_UP()if targetHoldFlag<HowManyRepeatsMakeAHold then
truck.TruckReport("Report")end
targetHoldFlag=0
end
function awtx.keypad.KEY_START_DOWN()local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)startHoldFlag=0
end
function awtx.keypad.KEY_START_REPEAT()startHoldFlag=startHoldFlag+1
if startHoldFlag==HowManyRepeatsMakeAHold then
end
end
function awtx.keypad.KEY_START_UP()if startHoldFlag<HowManyRepeatsMakeAHold then
truck.setLiteGreen()end
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
truck.setLiteRed()end
stopHoldFlag=0
end
T1={text="Super",key=1,action="MENU",variable="SuperMenu"}T2={text="EXIT",key=2,action="FUNC",callThis=supervisor.exitFunction}topMenu={T1,T2}SuperMenu1={text=" SETPNT",key=1,action="MENU",variable="SetpointSetupMenu"}SuperMenu2={text=" TARE  ",key=2,action="MENU",variable="TareSetupMenu",show={callThis=get_PresetTareEnabled,val1=1}}SuperMenu3={text=" TRUCK ",key=3,action="MENU",variable="TruckSetupMenu"}SuperMenu4={text="BATTERY",key=4,action="MENU",variable="BatterySetupMenu"}SuperMenu5={text=" BACK  ",key=5,action="MENU",variable="topMenu",subMenu=1}SuperMenu={SuperMenu1,SuperMenu2,SuperMenu3,SuperMenu4,SuperMenu5}SetpointSetupMenu1={text=" Edit  ",key=1,action="MENU",variable="SetpointSetupEdit"}SetpointSetupMenu2={text=" Print ",key=2,action="FUNC",callThis=setpoint.SetpointDBReport}SetpointSetupMenu3={text=" Reset ",key=3,action="FUNC",callThis=setpoint.SetpointDBReset}SetpointSetupMenu4={text=" BACK  ",key=4,action="MENU",variable="SuperMenu",subMenu=1}SetpointSetupMenu={SetpointSetupMenu1,SetpointSetupMenu2,SetpointSetupMenu3,SetpointSetupMenu4}SetpointSetupEdit1={text=" ANNUN ",key=1,action="FUNC",callThis=setpoint.editInvertingOutputFlag}SetpointSetupEdit2={text=" OUT1  ",key=2,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Mode_Model,val1=0}}SetpointSetupEdit3={text=" OUT1  ",key=3,action="MENU",variable="SetpointOut1Edit",show={callThis=get_Mode_Model,val1=1}}SetpointSetupEdit4={text=" OUT2  ",key=4,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Mode_Model,val1=0}}SetpointSetupEdit5={text=" OUT2  ",key=5,action="MENU",variable="SetpointOut2Edit",show={callThis=get_Mode_Model,val1=1}}SetpointSetupEdit6={text=" OUT3  ",key=6,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Mode_Model,val1=0}}SetpointSetupEdit7={text=" OUT3  ",key=7,action="MENU",variable="SetpointOut3Edit",show={callThis=get_Mode_Model,val1=1}}SetpointSetupEdit8={text="  IN1  ",key=8,action="FUNC",callThis=setpoint.editIn1}SetpointSetupEdit9={text="  IN2  ",key=9,action="FUNC",callThis=setpoint.editIn2}SetpointSetupEdit10={text="  IN3  ",key=10,action="FUNC",callThis=setpoint.editIn3}SetpointSetupEdit11={text=" MODE  ",key=11,action="FUNC",callThis=setpoint.editOutputMode,show={callThis=get_Mode_Model,val1=0}}SetpointSetupEdit12={text=" BACK  ",key=12,action="MENU",variable="SetpointSetupMenu",subMenu=1}SetpointSetupEdit={SetpointSetupEdit1,SetpointSetupEdit2,SetpointSetupEdit3,SetpointSetupEdit4,SetpointSetupEdit5,SetpointSetupEdit6,SetpointSetupEdit7,SetpointSetupEdit8,SetpointSetupEdit9,SetpointSetupEdit10,SetpointSetupEdit11,SetpointSetupEdit12}SetpointOut1Edit1={text=" EDIT1 ",key=1,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Out1_Mode,val1=0}}SetpointOut1Edit2={text=" EDIT1 ",key=2,action="MENU",variable="SetpointOut1LoHiEdit",show={callThis=get_Out1_Mode,val1=1}}SetpointOut1Edit3={text=" MODE1 ",key=3,action="FUNC",callThis=setpoint.editOutputMode1}SetpointOut1Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointSetupEdit",subMenu=3}SetpointOut1Edit={SetpointOut1Edit1,SetpointOut1Edit2,SetpointOut1Edit3,SetpointOut1Edit4}SetpointOut1LoHiEdit1={text="OUT1 LO",key=1,action="FUNC",callThis=setpoint.editOut1Lo}SetpointOut1LoHiEdit2={text="OUT1 HI",key=2,action="FUNC",callThis=setpoint.editOut1Hi}SetpointOut1LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointOut1Edit",subMenu=2}SetpointOut1LoHiEdit={SetpointOut1LoHiEdit1,SetpointOut1LoHiEdit2,SetpointOut1LoHiEdit3}SetpointOut2Edit1={text=" EDIT2 ",key=1,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Out2_Mode,val1=0}}SetpointOut2Edit2={text=" EDIT2 ",key=2,action="MENU",variable="SetpointOut2LoHiEdit",show={callThis=get_Out2_Mode,val1=1}}SetpointOut2Edit3={text=" MODE2 ",key=3,action="FUNC",callThis=setpoint.editOutputMode2}SetpointOut2Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointSetupEdit",subMenu=5}SetpointOut2Edit={SetpointOut2Edit1,SetpointOut2Edit2,SetpointOut2Edit3,SetpointOut2Edit4}SetpointOut2LoHiEdit1={text="OUT2 LO",key=1,action="FUNC",callThis=setpoint.editOut2Lo}SetpointOut2LoHiEdit2={text="OUT2 HI",key=2,action="FUNC",callThis=setpoint.editOut2Hi}SetpointOut2LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointOut2Edit",subMenu=2}SetpointOut2LoHiEdit={SetpointOut2LoHiEdit1,SetpointOut2LoHiEdit2,SetpointOut2LoHiEdit3}SetpointOut3Edit1={text=" EDIT3 ",key=1,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Out3_Mode,val1=0}}SetpointOut3Edit2={text=" EDIT3 ",key=2,action="MENU",variable="SetpointOut3LoHiEdit",show={callThis=get_Out3_Mode,val1=1}}SetpointOut3Edit3={text=" MODE3 ",key=3,action="FUNC",callThis=setpoint.editOutputMode3}SetpointOut3Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointSetupEdit",subMenu=7}SetpointOut3Edit={SetpointOut3Edit1,SetpointOut3Edit2,SetpointOut3Edit3,SetpointOut3Edit4}SetpointOut3LoHiEdit1={text="OUT3 LO",key=1,action="FUNC",callThis=setpoint.editOut3Lo}SetpointOut3LoHiEdit2={text="OUT3 HI",key=2,action="FUNC",callThis=setpoint.editOut3Hi}SetpointOut3LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointOut3Edit",subMenu=2}SetpointOut3LoHiEdit={SetpointOut3LoHiEdit1,SetpointOut3LoHiEdit2,SetpointOut3LoHiEdit3}TareSetupMenu1={text=" Edit  ",key=1,action="MENU",variable="TareSetupEdit"}TareSetupMenu2={text=" Print ",key=2,action="FUNC",callThis=tare.TareDBReport}TareSetupMenu3={text=" Reset ",key=3,action="FUNC",callThis=tare.TareDBReset}TareSetupMenu4={text=" BACK  ",key=4,action="MENU",variable="SuperMenu",subMenu=2}TareSetupMenu={TareSetupMenu1,TareSetupMenu2,TareSetupMenu3,TareSetupMenu4}TareSetupEdit1={text="Tare 1 ",key=1,action="FUNC",callThis=tare.editPresetTare1}TareSetupEdit2={text="Tare 2 ",key=2,action="FUNC",callThis=tare.editPresetTare2}TareSetupEdit3={text="Tare 3 ",key=3,action="FUNC",callThis=tare.editPresetTare3}TareSetupEdit4={text="Tare 4 ",key=4,action="FUNC",callThis=tare.editPresetTare4}TareSetupEdit5={text="Tare 5 ",key=5,action="FUNC",callThis=tare.editPresetTare5}TareSetupEdit6={text="Tare 6 ",key=6,action="FUNC",callThis=tare.editPresetTare6}TareSetupEdit7={text="Tare 7 ",key=7,action="FUNC",callThis=tare.editPresetTare7}TareSetupEdit8={text="Tare 8 ",key=8,action="FUNC",callThis=tare.editPresetTare8}TareSetupEdit9={text="Tare 9 ",key=9,action="FUNC",callThis=tare.editPresetTare9}TareSetupEdit10={text="Tare 10",key=10,action="FUNC",callThis=tare.editPresetTare10}TareSetupEdit11={text=" BACK  ",key=11,action="MENU",variable="TareSetupMenu",subMenu=1}TareSetupEdit={TareSetupEdit1,TareSetupEdit2,TareSetupEdit3,TareSetupEdit4,TareSetupEdit5,TareSetupEdit6,TareSetupEdit7,TareSetupEdit8,TareSetupEdit9,TareSetupEdit10,TareSetupEdit11}TruckSetupMenu1={text=" Edit  ",key=1,action="MENU",variable="TruckSetupEditMenu"}TruckSetupMenu2={text=" Lite  ",key=2,action="MENU",variable="TruckLiteMenu"}TruckSetupMenu3={text=" Print ",key=3,action="FUNC",callThis=truck.TruckMenuReport}TruckSetupMenu4={text=" Import",key=4,action="FUNC",callThis=truck.importGTN}TruckSetupMenu5={text=" Export",key=5,action="FUNC",callThis=truck.exportGTN}TruckSetupMenu6={text=" Reset ",key=6,action="MENU",variable="TruckSetupResetMenu"}TruckSetupMenu7={text=" BACK  ",key=7,action="MENU",variable="SuperMenu",subMenu=3}TruckSetupMenu={TruckSetupMenu1,TruckSetupMenu2,TruckSetupMenu3,TruckSetupMenu4,TruckSetupMenu5,TruckSetupMenu6,TruckSetupMenu7}TruckSetupEditMenu1={text="  In   ",key=1,action="FUNC",callThis=truck.editTruckInTruckId}TruckSetupEditMenu2={text="  IO   ",key=2,action="FUNC",callThis=truck.editTruckIOTruckId}TruckSetupEditMenu3={text=" Fleet ",key=3,action="FUNC",callThis=truck.editTruckFleetTruckId}TruckSetupEditMenu4={text=" Report",key=4,action="MENU",variable="TruckReportMenu"}TruckSetupEditMenu5={text=" BACK  ",key=5,action="MENU",variable="TruckSetupMenu",subMenu=1}TruckSetupEditMenu={TruckSetupEditMenu1,TruckSetupEditMenu2,TruckSetupEditMenu3,TruckSetupEditMenu4,TruckSetupEditMenu5}TruckEditInMenu1={text=" CLEAR ",key=1,action="FUNC",callThis=truck.clrTruckInWeight}TruckEditInMenu2={text=" BACK  ",key=2,action="MENU",variable="TruckSetupEditMenu",subMenu=1}TruckEditInMenu={TruckEditInMenu1,TruckEditInMenu2}TruckEditIOMenu1={text="Delete ",key=1,action="FUNC",callThis=truck.delTruckIdIO}TruckEditIOMenu2={text=" BACK  ",key=2,action="MENU",variable="TruckSetupEditMenu",subMenu=2}TruckEditIOMenu={TruckEditIOMenu1,TruckEditIOMenu2}TruckEditFleetMenu1={text=" Tare  ",key=1,action="FUNC",callThis=truck.editTruckTareFleet}TruckEditFleetMenu2={text="Delete ",key=2,action="FUNC",callThis=truck.delTruckIdFleet}TruckEditFleetMenu3={text=" BACK  ",key=3,action="MENU",variable="TruckSetupEditMenu",subMenu=3}TruckEditFleetMenu={TruckEditFleetMenu1,TruckEditFleetMenu2,TruckEditFleetMenu3}TruckReportMenu1={text=" InFmt ",key=1,action="MENU",variable="TruckInFmtSetupMenu"}TruckReportMenu2={text=" OutFmt",key=2,action="MENU",variable="TruckOutFmtSetupMenu"}TruckReportMenu3={text=" FltFmt",key=3,action="MENU",variable="TruckFltFmtSetupMenu"}TruckReportMenu4={text=" BACK  ",key=4,action="MENU",variable="TruckSetupEditMenu",subMenu=4}TruckReportMenu={TruckReportMenu1,TruckReportMenu2,TruckReportMenu3,TruckReportMenu4}TruckInFmtSetupMenu1={text="In Tick",key=1,action="FUNC",callThis=truck.editInFmt}TruckInFmtSetupMenu2={text="In Head",key=2,action="FUNC",callThis=truck.editInHeaderFmt}TruckInFmtSetupMenu3={text="In Body",key=3,action="FUNC",callThis=truck.editInBodyFmt}TruckInFmtSetupMenu4={text="In Foot",key=4,action="FUNC",callThis=truck.editInFooterFmt}TruckInFmtSetupMenu5={text=" BACK  ",key=5,action="MENU",variable="TruckReportMenu",subMenu=1}TruckInFmtSetupMenu={TruckInFmtSetupMenu1,TruckInFmtSetupMenu2,TruckInFmtSetupMenu3,TruckInFmtSetupMenu4,TruckInFmtSetupMenu5}TruckOutFmtSetupMenu1={text="OutTick",key=1,action="FUNC",callThis=truck.editOutFmt}TruckOutFmtSetupMenu2={text="OutHead",key=2,action="FUNC",callThis=truck.editOutHeaderFmt}TruckOutFmtSetupMenu3={text="OutBody",key=3,action="FUNC",callThis=truck.editOutBodyFmt}TruckOutFmtSetupMenu4={text="OutFoot",key=4,action="FUNC",callThis=truck.editOutFooterFmt}TruckOutFmtSetupMenu5={text=" BACK  ",key=5,action="MENU",variable="TruckReportMenu",subMenu=2}TruckOutFmtSetupMenu={TruckOutFmtSetupMenu1,TruckOutFmtSetupMenu2,TruckOutFmtSetupMenu3,TruckOutFmtSetupMenu4,TruckOutFmtSetupMenu5}TruckFltFmtSetupMenu1={text="FltTick",key=1,action="FUNC",callThis=truck.editFleetFmt}TruckFltFmtSetupMenu2={text="FltHead",key=2,action="FUNC",callThis=truck.editFleetHeaderFmt}TruckFltFmtSetupMenu3={text="FltBody",key=3,action="FUNC",callThis=truck.editFleetBodyFmt}TruckFltFmtSetupMenu4={text="FltFoot",key=4,action="FUNC",callThis=truck.editFleetFooterFmt}TruckFltFmtSetupMenu5={text=" BACK  ",key=5,action="MENU",variable="TruckReportMenu",subMenu=3}TruckFltFmtSetupMenu={TruckFltFmtSetupMenu1,TruckFltFmtSetupMenu2,TruckFltFmtSetupMenu3,TruckFltFmtSetupMenu4,TruckFltFmtSetupMenu5}TruckSetupResetMenu1={text="  IN   ",key=1,action="FUNC",callThis=truck.TruckInReset}TruckSetupResetMenu2={text="  IO   ",key=2,action="FUNC",callThis=truck.TruckIOReset}TruckSetupResetMenu3={text=" FLEET ",key=3,action="FUNC",callThis=truck.TruckFleetReset}TruckSetupResetMenu4={text="  ALL  ",key=4,action="FUNC",callThis=truck.TruckAllReset}TruckSetupResetMenu5={text=" BACK  ",key=5,action="MENU",variable="TruckSetupMenu",subMenu=6}TruckSetupResetMenu={TruckSetupResetMenu1,TruckSetupResetMenu2,TruckSetupResetMenu3,TruckSetupResetMenu4,TruckSetupResetMenu5}TruckLiteMenu1={text=" Enable",key=1,action="FUNC",callThis=truck.editLiteEnable}TruckLiteMenu2={text=" T-Hold",key=2,action="FUNC",callThis=truck.editLiteTHold}TruckLiteMenu3={text=" BACK  ",key=3,action="MENU",variable="TruckSetupMenu",subMenu=2}TruckLiteMenu={TruckLiteMenu1,TruckLiteMenu2,TruckLiteMenu3}BatterySetupMenu1={text=" Enable",key=1,action="FUNC",callThis=battery.editBatteryEnable}BatterySetupMenu2={text=" Tmout ",key=2,action="FUNC",callThis=battery.editBatterySetting}BatterySetupMenu3={text=" BACK  ",key=3,action="MENU",variable="SuperMenu",subMenu=4}BatterySetupMenu={BatterySetupMenu1,BatterySetupMenu2,BatterySetupMenu3}resolveCircular1={topMenu=topMenu,SuperMenu=SuperMenu,SetpointSetupMenu=SetpointSetupMenu,SetpointSetupEdit=SetpointSetupEdit,SetpointOut1Edit=SetpointOut1Edit,SetpointOut1LoHiEdit=SetpointOut1LoHiEdit,SetpointOut2Edit=SetpointOut2Edit,SetpointOut2LoHiEdit=SetpointOut2LoHiEdit,SetpointOut3Edit=SetpointOut3Edit,SetpointOut3LoHiEdit=SetpointOut3LoHiEdit,TareSetupMenu=TareSetupMenu,TareSetupEdit=TareSetupEdit,TruckSetupMenu=TruckSetupMenu,TruckSetupEditMenu=TruckSetupEditMenu,TruckEditInMenu=TruckEditInMenu,TruckEditIOMenu=TruckEditIOMenu,TruckEditFleetMenu=TruckEditFleetMenu,TruckReportMenu=TruckReportMenu,TruckLiteMenu=TruckLiteMenu,TruckInFmtSetupMenu=TruckInFmtSetupMenu,TruckOutFmtSetupMenu=TruckOutFmtSetupMenu,TruckFltFmtSetupMenu=TruckFltFmtSetupMenu,TruckSetupResetMenu=TruckSetupResetMenu,BatterySetupMenu=BatterySetupMenu}SetpointEdit1={text=" Setpnt",key=1,action="MENU",variable="SetpointEditor"}SetpointEdit2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}SetpointEdit={SetpointEdit1,SetpointEdit2}SetpointEditor1={text=" OUT1  ",key=1,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Mode_Model,val1=0}}SetpointEditor2={text=" OUT1  ",key=2,action="MENU",variable="SetpointEditorOut1Edit",show={callThis=get_Mode_Model,val1=1}}SetpointEditor3={text=" OUT2  ",key=3,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Mode_Model,val1=0}}SetpointEditor4={text=" OUT2  ",key=4,action="MENU",variable="SetpointEditorOut2Edit",show={callThis=get_Mode_Model,val1=1}}SetpointEditor5={text=" OUT3  ",key=5,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Mode_Model,val1=0}}SetpointEditor6={text=" OUT3  ",key=6,action="MENU",variable="SetpointEditorOut3Edit",show={callThis=get_Mode_Model,val1=1}}SetpointEditor7={text=" BACK  ",key=7,action="MENU",variable="SetpointEdit",subMenu=1}SetpointEditor={SetpointEditor1,SetpointEditor2,SetpointEditor3,SetpointEditor4,SetpointEditor5,SetpointEditor6,SetpointEditor7}SetpointEditorOut1Edit1={text=" EDIT1 ",key=1,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Out1_Mode,val1=0}}SetpointEditorOut1Edit2={text=" EDIT1 ",key=2,action="MENU",variable="SetpointEditorOut1LoHiEdit",show={callThis=get_Out1_Mode,val1=1}}SetpointEditorOut1Edit3={text=" MODE1 ",key=3,action="FUNC",callThis=setpoint.editOutputMode1}SetpointEditorOut1Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointEditor",subMenu=2}SetpointEditorOut1Edit={SetpointEditorOut1Edit1,SetpointEditorOut1Edit2,SetpointEditorOut1Edit3,SetpointEditorOut1Edit4}SetpointEditorOut1LoHiEdit1={text="OUT1 LO",key=1,action="FUNC",callThis=setpoint.editOut1Lo}SetpointEditorOut1LoHiEdit2={text="OUT1 HI",key=2,action="FUNC",callThis=setpoint.editOut1Hi}SetpointEditorOut1LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointEditorOut1Edit",subMenu=2}SetpointEditorOut1LoHiEdit={SetpointEditorOut1LoHiEdit1,SetpointEditorOut1LoHiEdit2,SetpointEditorOut1LoHiEdit3}SetpointEditorOut2Edit1={text=" EDIT2 ",key=1,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Out2_Mode,val1=0}}SetpointEditorOut2Edit2={text=" EDIT2 ",key=2,action="MENU",variable="SetpointEditorOut2LoHiEdit",show={callThis=get_Out2_Mode,val1=1}}SetpointEditorOut2Edit3={text=" MODE2 ",key=3,action="FUNC",callThis=setpoint.editOutputMode2}SetpointEditorOut2Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointEditor",subMenu=4}SetpointEditorOut2Edit={SetpointEditorOut2Edit1,SetpointEditorOut2Edit2,SetpointEditorOut2Edit3,SetpointEditorOut2Edit4}SetpointEditorOut2LoHiEdit1={text="OUT2 LO",key=1,action="FUNC",callThis=setpoint.editOut2Lo}SetpointEditorOut2LoHiEdit2={text="OUT2 HI",key=2,action="FUNC",callThis=setpoint.editOut2Hi}SetpointEditorOut2LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointEditorOut2Edit",subMenu=2}SetpointEditorOut2LoHiEdit={SetpointEditorOut2LoHiEdit1,SetpointEditorOut2LoHiEdit2,SetpointEditorOut2LoHiEdit3}SetpointEditorOut3Edit1={text=" EDIT3 ",key=1,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Out3_Mode,val1=0}}SetpointEditorOut3Edit2={text=" EDIT3 ",key=2,action="MENU",variable="SetpointEditorOut3LoHiEdit",show={callThis=get_Out3_Mode,val1=1}}SetpointEditorOut3Edit3={text=" MODE3 ",key=3,action="FUNC",callThis=setpoint.editOutputMode3}SetpointEditorOut3Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointEditor",subMenu=6}SetpointEditorOut3Edit={SetpointEditorOut3Edit1,SetpointEditorOut3Edit2,SetpointEditorOut3Edit3,SetpointEditorOut3Edit4}SetpointEditorOut3LoHiEdit1={text="OUT3 LO",key=1,action="FUNC",callThis=setpoint.editOut3Lo}SetpointEditorOut3LoHiEdit2={text="OUT3 HI",key=2,action="FUNC",callThis=setpoint.editOut3Hi}SetpointEditorOut3LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointEditorOut3Edit",subMenu=2}SetpointEditorOut3LoHiEdit={SetpointEditorOut3LoHiEdit1,SetpointEditorOut3LoHiEdit2,SetpointEditorOut3LoHiEdit3}resolveCircular2={SetpointEdit=SetpointEdit,SetpointEditor=SetpointEditor,SetpointEditorOut1Edit=SetpointEditorOut1Edit,SetpointEditorOut1LoHiEdit=SetpointEditorOut1LoHiEdit,SetpointEditorOut2Edit=SetpointEditorOut2Edit,SetpointEditorOut2LoHiEdit=SetpointEditorOut2LoHiEdit,SetpointEditorOut3Edit=SetpointEditorOut3Edit,SetpointEditorOut3LoHiEdit=SetpointEditorOut3LoHiEdit}ParamTopMenu1={text=" Param ",key=1,action="MENU",variable="OutputModeEdit"}ParamTopMenu2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}ParamTopMenu={ParamTopMenu1,ParamTopMenu2}OutputModeEdit1={text="OutMode",key=1,action="MENU",variable="OutputModeEditor"}OutputModeEdit2={text=" BACK  ",key=2,action="MENU",variable="ParamTopMenu",subMenu=1}OutputModeEdit={OutputModeEdit1,OutputModeEdit2}OutputModeEditor1={text=" OUT1  ",key=1,action="FUNC",callThis=setpoint.editOutputMode1}OutputModeEditor2={text=" OUT2  ",key=2,action="FUNC",callThis=setpoint.editOutputMode2}OutputModeEditor3={text=" OUT3  ",key=3,action="FUNC",callThis=setpoint.editOutputMode3}OutputModeEditor4={text=" BACK  ",key=4,action="MENU",variable="OutputModeEdit",subMenu=1}OutputModeEditor={OutputModeEditor1,OutputModeEditor2,OutputModeEditor3,OutputModeEditor4}resolveCircular7={ParamTopMenu=ParamTopMenu,OutputModeEdit=OutputModeEdit,OutputModeEditor=OutputModeEditor}function appEnterSuperMenu()end
function appExitSuperMenu()truck.clrNewVars()end
setpoint.basis="netWt"function appDisableSetpoints()if truck.LiteEnableFlag==0 then
setpoint.disableSetpointsOutputs()elseif truck.LiteEnableFlag==1 then
truck.disableLiteOutputs()truck.setLiteOff()elseif truck.LiteEnableFlag==2 then
truck.disableLiteOutputs()truck.setLiteOff()end
end
function appEnableSetpoints()if truck.LiteEnableFlag==0 then
setpoint.refreshSetpointsOutputs(Setpointtable.OutputMode)elseif truck.LiteEnableFlag==1 then
truck.refreshLiteOutputs(Setpointtable.OutputMode)truck.setLiteOff()elseif truck.LiteEnableFlag==2 then
truck.refreshLiteOutputs(Setpointtable.OutputMode)end
end
function appRefreshBatterySettings()if truck.LiteEnableFlag==0 then
setpoint.refreshSetpointsOutputs(Setpointtable.OutputMode)elseif truck.LiteEnableFlag==1 then
truck.refreshLiteOutputs(Setpointtable.OutputMode)truck.setLiteOff()elseif truck.LiteEnableFlag==2 then
truck.refreshLiteOutputs(Setpointtable.OutputMode)end
end
appRefreshBatterySettings()setpoint.refreshSetpointsInputs()function appInput(setpointNum,inputState)if setpointNum==1 then
setpoint.setpointIn1Handler(setpointNum,inputState)end
if setpointNum==2 then
setpoint.setpointIn2Handler(setpointNum,inputState)end
if setpointNum==3 then
setpoint.setpointIn3Handler(setpointNum,inputState)end
end
ID.initIDPrintTokens()tare.initTarePrintTokens()setpoint.initSetpointPrintTokens()truck.initTruckPrintTokens()truck.initXR4500PrintToken()SMAPC.initPrintTokens()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)printAppDebug("\n Startup App1TruckIO.lua non-event ")while true do
awtx.os.systemEvents(100)end