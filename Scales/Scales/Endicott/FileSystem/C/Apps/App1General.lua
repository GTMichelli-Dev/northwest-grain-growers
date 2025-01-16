--[[
--## VERSION=0.1.0
--## DATE=2012-02-17  12:00
--## DESC=This is the General Weighing App (App1General.lua)

]]
require("ReqDebug")AppName="GENERAL"SerialNumber=awtx.setupMenu.getSerialNumber()AWTX_LUA_UNDEFINED=0
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
if config.keyTareFlag then
rpntareflag=true
end
if system.modelStr=="2060"then
require("ReqScaleKeys2060")else
require("ReqScaleKeys")end
require("ReqTare")require("ReqSetpoint")require("ReqBattery")require("ReqAppMenu")lblF1="  ID"lblF2=""lblF3="UNITS"lblF4="SCALE"lblF5=" DISP"function awtx.keypad.KEY_F1_UP()if f1HoldFlag<HowManyRepeatsMakeAHold then
enterId()end
f1HoldFlag=0
end
function awtx.keypad.KEY_F2_UP()if f2HoldFlag<HowManyRepeatsMakeAHold then
end
f2HoldFlag=0
end
function awtx.keypad.KEY_F3_UP()if f3HoldFlag<HowManyRepeatsMakeAHold then
awtx.weight.cycleUnits()end
f3HoldFlag=0
end
function awtx.keypad.KEY_F4_UP()if f4HoldFlag<HowManyRepeatsMakeAHold then
awtx.weight.cycleActiveScale()end
f4HoldFlag=0
end
function awtx.keypad.KEY_F5_UP()if f5HoldFlag<HowManyRepeatsMakeAHold then
cycleWeightDisplay()end
f5HoldFlag=0
end
T1={text="Super",key=1,action="MENU",variable="SuperMenu"}T2={text="EXIT",key=2,action="FUNC",callThis=supervisor.exitFunction}topMenu={T1,T2}SuperMenu1={text=" SETPNT",key=1,action="MENU",variable="SetpointSetupMenu"}SuperMenu2={text=" TARE  ",key=2,action="MENU",variable="TareSetupMenu",show={callThis=get_PresetTareEnabled,val1=1}}SuperMenu3={text="BATTERY",key=3,action="MENU",variable="BatterySetupMenu"}SuperMenu4={text=" BACK  ",key=4,action="MENU",variable="topMenu",subMenu=1}SuperMenu={SuperMenu1,SuperMenu2,SuperMenu3,SuperMenu4}SetpointSetupMenu1={text=" Edit  ",key=1,action="MENU",variable="SetpointSetupEdit"}SetpointSetupMenu2={text=" Print ",key=2,action="FUNC",callThis=setpoint.SetpointDBReport}SetpointSetupMenu3={text=" Reset ",key=3,action="FUNC",callThis=setpoint.SetpointDBReset}SetpointSetupMenu4={text=" BACK  ",key=4,action="MENU",variable="SuperMenu",subMenu=1}SetpointSetupMenu={SetpointSetupMenu1,SetpointSetupMenu2,SetpointSetupMenu3,SetpointSetupMenu4}SetpointSetupEdit1={text=" ANNUN ",key=1,action="FUNC",callThis=setpoint.editInvertingOutputFlag}SetpointSetupEdit2={text=" OUT1  ",key=2,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Mode_Model,val1=0}}SetpointSetupEdit3={text=" OUT1  ",key=3,action="MENU",variable="SetpointOut1Edit",show={callThis=get_Mode_Model,val1=1}}SetpointSetupEdit4={text=" OUT1  ",key=4,action="MENU",variable="MatrixSetpointOut1Edit",show={callThis=get_Mode_Model,val1=2}}SetpointSetupEdit5={text=" OUT2  ",key=5,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Mode_Model,val1=0}}SetpointSetupEdit6={text=" OUT2  ",key=6,action="MENU",variable="SetpointOut2Edit",show={callThis=get_Mode_Model,val1=1}}SetpointSetupEdit7={text=" OUT2  ",key=7,action="MENU",variable="MatrixSetpointOut2Edit",show={callThis=get_Mode_Model,val1=2}}SetpointSetupEdit8={text=" OUT3  ",key=8,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Mode_Model,val1=0}}SetpointSetupEdit9={text=" OUT3  ",key=9,action="MENU",variable="SetpointOut3Edit",show={callThis=get_Mode_Model,val1=1}}SetpointSetupEdit10={text=" OUT3  ",key=10,action="MENU",variable="MatrixSetpointOut3Edit",show={callThis=get_Mode_Model,val1=2}}SetpointSetupEdit11={text="  IN1  ",key=11,action="FUNC",callThis=setpoint.editIn1}SetpointSetupEdit12={text="  IN2  ",key=12,action="FUNC",callThis=setpoint.editIn2}SetpointSetupEdit13={text="  IN3  ",key=13,action="FUNC",callThis=setpoint.editIn3}SetpointSetupEdit14={text=" MODE  ",key=14,action="FUNC",callThis=setpoint.editOutputMode,show={callThis=get_Mode_Model,val1=0}}SetpointSetupEdit15={text=" BACK  ",key=15,action="MENU",variable="SetpointSetupMenu",subMenu=1}SetpointSetupEdit={SetpointSetupEdit1,SetpointSetupEdit2,SetpointSetupEdit3,SetpointSetupEdit4,SetpointSetupEdit5,SetpointSetupEdit6,SetpointSetupEdit7,SetpointSetupEdit8,SetpointSetupEdit9,SetpointSetupEdit10,SetpointSetupEdit11,SetpointSetupEdit12,SetpointSetupEdit13,SetpointSetupEdit14,SetpointSetupEdit15}SetpointOut1Edit1={text=" EDIT1 ",key=1,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Out1_Mode,val1=0}}SetpointOut1Edit2={text=" EDIT1 ",key=2,action="MENU",variable="SetpointOut1LoHiEdit",show={callThis=get_Out1_Mode,val1=1}}SetpointOut1Edit3={text=" MODE1 ",key=3,action="FUNC",callThis=setpoint.editOutputMode1}SetpointOut1Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointSetupEdit",subMenu=3}SetpointOut1Edit={SetpointOut1Edit1,SetpointOut1Edit2,SetpointOut1Edit3,SetpointOut1Edit4}SetpointOut1LoHiEdit1={text="OUT1 LO",key=1,action="FUNC",callThis=setpoint.editOut1Lo}SetpointOut1LoHiEdit2={text="OUT1 HI",key=2,action="FUNC",callThis=setpoint.editOut1Hi}SetpointOut1LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointOut1Edit",subMenu=2}SetpointOut1LoHiEdit={SetpointOut1LoHiEdit1,SetpointOut1LoHiEdit2,SetpointOut1LoHiEdit3}MatrixSetpointOut1Edit1={text=" EDIT1 ",key=1,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Out1_Mode,val1=0}}MatrixSetpointOut1Edit2={text=" EDIT1 ",key=2,action="MENU",variable="MatrixSetpointOut1LoHiEdit",show={callThis=get_Out1_Mode,val1=1}}MatrixSetpointOut1Edit3={text=" BACK  ",key=3,action="MENU",variable="SetpointSetupEdit",subMenu=4}MatrixSetpointOut1Edit={MatrixSetpointOut1Edit1,MatrixSetpointOut1Edit2,MatrixSetpointOut1Edit3}MatrixSetpointOut1LoHiEdit1={text="OUT1 LO",key=1,action="FUNC",callThis=setpoint.editOut1Lo}MatrixSetpointOut1LoHiEdit2={text="OUT1 HI",key=2,action="FUNC",callThis=setpoint.editOut1Hi}MatrixSetpointOut1LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="MatrixSetpointOut1Edit",subMenu=2}MatrixSetpointOut1LoHiEdit={MatrixSetpointOut1LoHiEdit1,MatrixSetpointOut1LoHiEdit2,MatrixSetpointOut1LoHiEdit3}SetpointOut2Edit1={text=" EDIT2 ",key=1,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Out2_Mode,val1=0}}SetpointOut2Edit2={text=" EDIT2 ",key=2,action="MENU",variable="SetpointOut2LoHiEdit",show={callThis=get_Out2_Mode,val1=1}}SetpointOut2Edit3={text=" MODE2 ",key=3,action="FUNC",callThis=setpoint.editOutputMode2}SetpointOut2Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointSetupEdit",subMenu=6}SetpointOut2Edit={SetpointOut2Edit1,SetpointOut2Edit2,SetpointOut2Edit3,SetpointOut2Edit4}SetpointOut2LoHiEdit1={text="OUT2 LO",key=1,action="FUNC",callThis=setpoint.editOut2Lo}SetpointOut2LoHiEdit2={text="OUT2 HI",key=2,action="FUNC",callThis=setpoint.editOut2Hi}SetpointOut2LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointOut2Edit",subMenu=2}SetpointOut2LoHiEdit={SetpointOut2LoHiEdit1,SetpointOut2LoHiEdit2,SetpointOut2LoHiEdit3}MatrixSetpointOut2Edit1={text=" EDIT2 ",key=1,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Out2_Mode,val1=0}}MatrixSetpointOut2Edit2={text=" EDIT2 ",key=2,action="MENU",variable="MatrixSetpointOut2LoHiEdit",show={callThis=get_Out2_Mode,val1=1}}MatrixSetpointOut2Edit3={text=" BACK  ",key=3,action="MENU",variable="SetpointSetupEdit",subMenu=7}MatrixSetpointOut2Edit={MatrixSetpointOut2Edit1,MatrixSetpointOut2Edit2,MatrixSetpointOut2Edit3}MatrixSetpointOut2LoHiEdit1={text="OUT2 LO",key=1,action="FUNC",callThis=setpoint.editOut2Lo}MatrixSetpointOut2LoHiEdit2={text="OUT2 HI",key=2,action="FUNC",callThis=setpoint.editOut2Hi}MatrixSetpointOut2LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="MatrixSetpointOut2Edit",subMenu=2}MatrixSetpointOut2LoHiEdit={MatrixSetpointOut2LoHiEdit1,MatrixSetpointOut2LoHiEdit2,MatrixSetpointOut2LoHiEdit3}SetpointOut3Edit1={text=" EDIT3 ",key=1,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Out3_Mode,val1=0}}SetpointOut3Edit2={text=" EDIT3 ",key=2,action="MENU",variable="SetpointOut3LoHiEdit",show={callThis=get_Out3_Mode,val1=1}}SetpointOut3Edit3={text=" MODE3 ",key=3,action="FUNC",callThis=setpoint.editOutputMode3}SetpointOut3Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointSetupEdit",subMenu=9}SetpointOut3Edit={SetpointOut3Edit1,SetpointOut3Edit2,SetpointOut3Edit3,SetpointOut3Edit4}SetpointOut3LoHiEdit1={text="OUT3 LO",key=1,action="FUNC",callThis=setpoint.editOut3Lo}SetpointOut3LoHiEdit2={text="OUT3 HI",key=2,action="FUNC",callThis=setpoint.editOut3Hi}SetpointOut3LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointOut3Edit",subMenu=2}SetpointOut3LoHiEdit={SetpointOut3LoHiEdit1,SetpointOut3LoHiEdit2,SetpointOut3LoHiEdit3}MatrixSetpointOut3Edit1={text=" EDIT3 ",key=1,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Out3_Mode,val1=0}}MatrixSetpointOut3Edit2={text=" EDIT3 ",key=2,action="MENU",variable="MatrixSetpointOut3LoHiEdit",show={callThis=get_Out3_Mode,val1=1}}MatrixSetpointOut3Edit3={text=" BACK  ",key=3,action="MENU",variable="SetpointSetupEdit",subMenu=10}MatrixSetpointOut3Edit={MatrixSetpointOut3Edit1,MatrixSetpointOut3Edit2,MatrixSetpointOut3Edit3}MatrixSetpointOut3LoHiEdit1={text="OUT3 LO",key=1,action="FUNC",callThis=setpoint.editOut3Lo}MatrixSetpointOut3LoHiEdit2={text="OUT3 HI",key=2,action="FUNC",callThis=setpoint.editOut3Hi}MatrixSetpointOut3LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="MatrixSetpointOut3Edit",subMenu=2}MatrixSetpointOut3LoHiEdit={MatrixSetpointOut3LoHiEdit1,MatrixSetpointOut3LoHiEdit2,MatrixSetpointOut3LoHiEdit3}TareSetupMenu1={text=" Edit  ",key=1,action="MENU",variable="TareSetupEdit"}TareSetupMenu2={text=" Print ",key=2,action="FUNC",callThis=tare.TareDBReport}TareSetupMenu3={text=" Reset ",key=3,action="FUNC",callThis=tare.TareDBReset}TareSetupMenu4={text=" BACK  ",key=4,action="MENU",variable="SuperMenu",subMenu=2}TareSetupMenu={TareSetupMenu1,TareSetupMenu2,TareSetupMenu3,TareSetupMenu4}TareSetupEdit1={text="Tare 1 ",key=1,action="FUNC",callThis=tare.editPresetTare1}TareSetupEdit2={text="Tare 2 ",key=2,action="FUNC",callThis=tare.editPresetTare2}TareSetupEdit3={text="Tare 3 ",key=3,action="FUNC",callThis=tare.editPresetTare3}TareSetupEdit4={text="Tare 4 ",key=4,action="FUNC",callThis=tare.editPresetTare4}TareSetupEdit5={text="Tare 5 ",key=5,action="FUNC",callThis=tare.editPresetTare5}TareSetupEdit6={text="Tare 6 ",key=6,action="FUNC",callThis=tare.editPresetTare6}TareSetupEdit7={text="Tare 7 ",key=7,action="FUNC",callThis=tare.editPresetTare7}TareSetupEdit8={text="Tare 8 ",key=8,action="FUNC",callThis=tare.editPresetTare8}TareSetupEdit9={text="Tare 9 ",key=9,action="FUNC",callThis=tare.editPresetTare9}TareSetupEdit10={text="Tare 10",key=10,action="FUNC",callThis=tare.editPresetTare10}TareSetupEdit11={text=" BACK  ",key=11,action="MENU",variable="TareSetupMenu",subMenu=1}TareSetupEdit={TareSetupEdit1,TareSetupEdit2,TareSetupEdit3,TareSetupEdit4,TareSetupEdit5,TareSetupEdit6,TareSetupEdit7,TareSetupEdit8,TareSetupEdit9,TareSetupEdit10,TareSetupEdit11}BatterySetupMenu1={text=" Enable ",key=1,action="FUNC",callThis=battery.editBatteryEnable}BatterySetupMenu2={text=" Tmout  ",key=2,action="FUNC",callThis=battery.editBatterySetting}BatterySetupMenu3={text=" BACK   ",key=3,action="MENU",variable="SuperMenu",subMenu=3}BatterySetupMenu={BatterySetupMenu1,BatterySetupMenu2,BatterySetupMenu3}resolveCircular1={topMenu=topMenu,SuperMenu=SuperMenu,SetpointSetupMenu=SetpointSetupMenu,SetpointSetupEdit=SetpointSetupEdit,SetpointOut1Edit=SetpointOut1Edit,SetpointOut1LoHiEdit=SetpointOut1LoHiEdit,MatrixSetpointOut1Edit=MatrixSetpointOut1Edit,MatrixSetpointOut1LoHiEdit=MatrixSetpointOut1LoHiEdit,SetpointOut2Edit=SetpointOut2Edit,SetpointOut2LoHiEdit=SetpointOut2LoHiEdit,MatrixSetpointOut2Edit=MatrixSetpointOut2Edit,MatrixSetpointOut2LoHiEdit=MatrixSetpointOut2LoHiEdit,SetpointOut3Edit=SetpointOut3Edit,SetpointOut3LoHiEdit=SetpointOut3LoHiEdit,MatrixSetpointOut3Edit=MatrixSetpointOut3Edit,MatrixSetpointOut3LoHiEdit=MatrixSetpointOut3LoHiEdit,TareSetupMenu=TareSetupMenu,TareSetupEdit=TareSetupEdit,BatterySetupMenu=BatterySetupMenu}SetpointEdit1={text=" Setpnt",key=1,action="MENU",variable="SetpointEditor"}SetpointEdit2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}SetpointEdit={SetpointEdit1,SetpointEdit2}SetpointEditor1={text=" OUT1  ",key=1,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Mode_Model,val1=0}}SetpointEditor2={text=" OUT1  ",key=2,action="MENU",variable="SetpointEditorOut1Edit",show={callThis=get_Mode_Model,val1=1}}SetpointEditor3={text=" OUT1  ",key=3,action="MENU",variable="MatrixSetpointEditorOut1Edit",show={callThis=get_Mode_Model,val1=2}}SetpointEditor4={text=" OUT2  ",key=4,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Mode_Model,val1=0}}SetpointEditor5={text=" OUT2  ",key=5,action="MENU",variable="SetpointEditorOut2Edit",show={callThis=get_Mode_Model,val1=1}}SetpointEditor6={text=" OUT2  ",key=6,action="MENU",variable="MatrixSetpointEditorOut2Edit",show={callThis=get_Mode_Model,val1=2}}SetpointEditor7={text=" OUT3  ",key=7,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Mode_Model,val1=0}}SetpointEditor8={text=" OUT3  ",key=8,action="MENU",variable="SetpointEditorOut3Edit",show={callThis=get_Mode_Model,val1=1}}SetpointEditor9={text=" OUT3  ",key=9,action="MENU",variable="MatrixSetpointEditorOut3Edit",show={callThis=get_Mode_Model,val1=2}}SetpointEditor10={text=" BACK  ",key=10,action="MENU",variable="SetpointEdit",subMenu=1}SetpointEditor={SetpointEditor1,SetpointEditor2,SetpointEditor3,SetpointEditor4,SetpointEditor5,SetpointEditor6,SetpointEditor7,SetpointEditor8,SetpointEditor9,SetpointEditor10}SetpointEditorOut1Edit1={text=" EDIT1 ",key=1,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Out1_Mode,val1=0}}SetpointEditorOut1Edit2={text=" EDIT1 ",key=2,action="MENU",variable="SetpointEditorOut1LoHiEdit",show={callThis=get_Out1_Mode,val1=1}}SetpointEditorOut1Edit3={text=" MODE1 ",key=3,action="FUNC",callThis=setpoint.editOutputMode1}SetpointEditorOut1Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointEditor",subMenu=2}SetpointEditorOut1Edit={SetpointEditorOut1Edit1,SetpointEditorOut1Edit2,SetpointEditorOut1Edit3,SetpointEditorOut1Edit4}SetpointEditorOut1LoHiEdit1={text="OUT1 LO",key=1,action="FUNC",callThis=setpoint.editOut1Lo}SetpointEditorOut1LoHiEdit2={text="OUT1 HI",key=2,action="FUNC",callThis=setpoint.editOut1Hi}SetpointEditorOut1LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointEditorOut1Edit",subMenu=2}SetpointEditorOut1LoHiEdit={SetpointEditorOut1LoHiEdit1,SetpointEditorOut1LoHiEdit2,SetpointEditorOut1LoHiEdit3}MatrixSetpointEditorOut1Edit1={text=" EDIT1 ",key=1,action="FUNC",callThis=setpoint.editOut1,show={callThis=get_Out1_Mode,val1=0}}MatrixSetpointEditorOut1Edit2={text=" EDIT1 ",key=2,action="MENU",variable="MatrixSetpointEditorOut1LoHiEdit",show={callThis=get_Out1_Mode,val1=1}}MatrixSetpointEditorOut1Edit3={text=" BACK  ",key=3,action="MENU",variable="SetpointEditor",subMenu=3}MatrixSetpointEditorOut1Edit={MatrixSetpointEditorOut1Edit1,MatrixSetpointEditorOut1Edit2,MatrixSetpointEditorOut1Edit3}MatrixSetpointEditorOut1LoHiEdit1={text="OUT1 LO",key=1,action="FUNC",callThis=setpoint.editOut1Lo}MatrixSetpointEditorOut1LoHiEdit2={text="OUT1 HI",key=2,action="FUNC",callThis=setpoint.editOut1Hi}MatrixSetpointEditorOut1LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="MatrixSetpointEditorOut1Edit",subMenu=2}MatrixSetpointEditorOut1LoHiEdit={MatrixSetpointEditorOut1LoHiEdit1,MatrixSetpointEditorOut1LoHiEdit2,MatrixSetpointEditorOut1LoHiEdit3}SetpointEditorOut2Edit1={text=" EDIT2 ",key=1,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Out2_Mode,val1=0}}SetpointEditorOut2Edit2={text=" EDIT2 ",key=2,action="MENU",variable="SetpointEditorOut2LoHiEdit",show={callThis=get_Out2_Mode,val1=1}}SetpointEditorOut2Edit3={text=" MODE2 ",key=3,action="FUNC",callThis=setpoint.editOutputMode2}SetpointEditorOut2Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointEditor",subMenu=5}SetpointEditorOut2Edit={SetpointEditorOut2Edit1,SetpointEditorOut2Edit2,SetpointEditorOut2Edit3,SetpointEditorOut2Edit4}SetpointEditorOut2LoHiEdit1={text="OUT2 LO",key=1,action="FUNC",callThis=setpoint.editOut2Lo}SetpointEditorOut2LoHiEdit2={text="OUT2 HI",key=2,action="FUNC",callThis=setpoint.editOut2Hi}SetpointEditorOut2LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointEditorOut2Edit",subMenu=2}SetpointEditorOut2LoHiEdit={SetpointEditorOut2LoHiEdit1,SetpointEditorOut2LoHiEdit2,SetpointEditorOut2LoHiEdit3}MatrixSetpointEditorOut2Edit1={text=" EDIT2 ",key=1,action="FUNC",callThis=setpoint.editOut2,show={callThis=get_Out2_Mode,val1=0}}MatrixSetpointEditorOut2Edit2={text=" EDIT2 ",key=2,action="MENU",variable="MatrixSetpointEditorOut2LoHiEdit",show={callThis=get_Out2_Mode,val1=1}}MatrixSetpointEditorOut2Edit3={text=" BACK  ",key=3,action="MENU",variable="SetpointEditor",subMenu=6}MatrixSetpointEditorOut2Edit={MatrixSetpointEditorOut2Edit1,MatrixSetpointEditorOut2Edit2,MatrixSetpointEditorOut2Edit3}MatrixSetpointEditorOut2LoHiEdit1={text="OUT2 LO",key=1,action="FUNC",callThis=setpoint.editOut2Lo}MatrixSetpointEditorOut2LoHiEdit2={text="OUT2 HI",key=2,action="FUNC",callThis=setpoint.editOut2Hi}MatrixSetpointEditorOut2LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="MatrixSetpointEditorOut2Edit",subMenu=2}MatrixSetpointEditorOut2LoHiEdit={MatrixSetpointEditorOut2LoHiEdit1,MatrixSetpointEditorOut2LoHiEdit2,MatrixSetpointEditorOut2LoHiEdit3}SetpointEditorOut3Edit1={text=" EDIT3 ",key=1,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Out3_Mode,val1=0}}SetpointEditorOut3Edit2={text=" EDIT3 ",key=2,action="MENU",variable="SetpointEditorOut3LoHiEdit",show={callThis=get_Out3_Mode,val1=1}}SetpointEditorOut3Edit3={text=" MODE3 ",key=3,action="FUNC",callThis=setpoint.editOutputMode3}SetpointEditorOut3Edit4={text=" BACK  ",key=4,action="MENU",variable="SetpointEditor",subMenu=8}SetpointEditorOut3Edit={SetpointEditorOut3Edit1,SetpointEditorOut3Edit2,SetpointEditorOut3Edit3,SetpointEditorOut3Edit4}SetpointEditorOut3LoHiEdit1={text="OUT3 LO",key=1,action="FUNC",callThis=setpoint.editOut3Lo}SetpointEditorOut3LoHiEdit2={text="OUT3 HI",key=2,action="FUNC",callThis=setpoint.editOut3Hi}SetpointEditorOut3LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="SetpointEditorOut3Edit",subMenu=2}SetpointEditorOut3LoHiEdit={SetpointEditorOut3LoHiEdit1,SetpointEditorOut3LoHiEdit2,SetpointEditorOut3LoHiEdit3}MatrixSetpointEditorOut3Edit1={text=" EDIT3 ",key=1,action="FUNC",callThis=setpoint.editOut3,show={callThis=get_Out3_Mode,val1=0}}MatrixSetpointEditorOut3Edit2={text=" EDIT3 ",key=2,action="MENU",variable="MatrixSetpointEditorOut3LoHiEdit",show={callThis=get_Out3_Mode,val1=1}}MatrixSetpointEditorOut3Edit3={text=" BACK  ",key=3,action="MENU",variable="SetpointEditor",subMenu=9}MatrixSetpointEditorOut3Edit={MatrixSetpointEditorOut3Edit1,MatrixSetpointEditorOut3Edit2,MatrixSetpointEditorOut3Edit3}MatrixSetpointEditorOut3LoHiEdit1={text="OUT3 LO",key=1,action="FUNC",callThis=setpoint.editOut3Lo}MatrixSetpointEditorOut3LoHiEdit2={text="OUT3 HI",key=2,action="FUNC",callThis=setpoint.editOut3Hi}MatrixSetpointEditorOut3LoHiEdit3={text=" BACK  ",key=3,action="MENU",variable="MatrixSetpointEditorOut3Edit",subMenu=2}MatrixSetpointEditorOut3LoHiEdit={MatrixSetpointEditorOut3LoHiEdit1,MatrixSetpointEditorOut3LoHiEdit2,MatrixSetpointEditorOut3LoHiEdit3}resolveCircular2={SetpointEdit=SetpointEdit,SetpointEditor=SetpointEditor,SetpointEditorOut1Edit=SetpointEditorOut1Edit,SetpointEditorOut1LoHiEdit=SetpointEditorOut1LoHiEdit,MatrixSetpointEditorOut1Edit=MatrixSetpointEditorOut1Edit,MatrixSetpointEditorOut1LoHiEdit=MatrixSetpointEditorOut1LoHiEdit,SetpointEditorOut2Edit=SetpointEditorOut2Edit,SetpointEditorOut2LoHiEdit=SetpointEditorOut2LoHiEdit,MatrixSetpointEditorOut2Edit=MatrixSetpointEditorOut2Edit,MatrixSetpointEditorOut2LoHiEdit=MatrixSetpointEditorOut2LoHiEdit,SetpointEditorOut3Edit=SetpointEditorOut3Edit,SetpointEditorOut3LoHiEdit=SetpointEditorOut3LoHiEdit,MatrixSetpointEditorOut3Edit=MatrixSetpointEditorOut3Edit,MatrixSetpointEditorOut3LoHiEdit=MatrixSetpointEditorOut3LoHiEdit}ParamTopMenu1={text=" Param ",key=1,action="MENU",variable="OutputModeEdit"}ParamTopMenu2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}ParamTopMenu={ParamTopMenu1,ParamTopMenu2}OutputModeEdit1={text="OutMode",key=1,action="MENU",variable="OutputModeEditor"}OutputModeEdit2={text=" BACK  ",key=2,action="MENU",variable="ParamTopMenu",subMenu=1}OutputModeEdit={OutputModeEdit1,OutputModeEdit2}OutputModeEditor1={text=" OUT1  ",key=1,action="FUNC",callThis=setpoint.editOutputMode1}OutputModeEditor2={text=" OUT2  ",key=2,action="FUNC",callThis=setpoint.editOutputMode2}OutputModeEditor3={text=" OUT3  ",key=3,action="FUNC",callThis=setpoint.editOutputMode3}OutputModeEditor4={text=" BACK  ",key=4,action="MENU",variable="OutputModeEdit",subMenu=1}OutputModeEditor={OutputModeEditor1,OutputModeEditor2,OutputModeEditor3,OutputModeEditor4}resolveCircular7={ParamTopMenu=ParamTopMenu,OutputModeEdit=OutputModeEdit,OutputModeEditor=OutputModeEditor}function appEnterSuperMenu()end
function appExitSuperMenu()end
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
ID.initIDPrintTokens()tare.initTarePrintTokens()setpoint.initSetpointPrintTokens()SMAPC.initPrintTokens()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)printAppDebug("\n Startup App1General.lua non-event ")while true do
awtx.os.systemEvents(100)end