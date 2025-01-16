batching={}BatchingOutputtable={}BatchingInputtable={}if config.calwtunitStr~=nil then
BatchingCalUnit=config.calwtunitStr
end
local Output_Type_String={}local Output_Prompt_String
if system.modelStr=="ZM301"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside     ","Outside    ","Checkweigh ","User     "}Output_Prompt_String="ACT-Abv,ACT-Bel"elseif system.modelStr=="ZM303"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside     ","Outside    ","Checkweigh ","User     "}Output_Prompt_String="ACT-Abv,ACT-Bel"elseif system.modelStr=="ZQ375"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside     ","Outside    ","Checkweigh ","User     "}Output_Prompt_String="ACT-Abv,ACT-Bel"elseif system.modelStr=="ZM305GTN"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside     ","Outside    ","Checkweigh ","User     "}Output_Prompt_String="ACT-Abv,ACT-Bel"elseif system.modelStr=="ZM305"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside     ","Outside    ","Checkweigh ","User     "}Output_Prompt_String="ACT-Abv,ACT-Bel"else
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside     ","Outside    ","Checkweigh ","User     "}Output_Prompt_String="ACT-Abv,ACT-Bel"end
local Input_Type_String={}local Input_Prompt_String
if system.modelStr=="ZM301"then
MAX_SETPOINT_INPUT_INDEX=3
Input_Type_String={[0]="None     ","PrintKey ","UnitsKey ","SelectKey","TareKey  ","ZeroKey  ","SampleKey","F1Key    ","AccumKey ","PrintHold","User     "}Input_Prompt_String="None,Print,Units,Select,Tare,Zero,Sample,F1,Accu,PrntHld,User"elseif system.modelStr=="ZM303"then
MAX_SETPOINT_INPUT_INDEX=3
Input_Type_String={[0]="None     ","PrintKey ","UnitsKey ","SelectKey","TareKey  ","ZeroKey  ","SampleKey","F1Key    ","TargetKey","StartKey ","StopKey  ","IDKey    ","SetupKey ","AccumKey ","PrintHold","User     "}Input_Prompt_String="None,Print,Units,Select,Tare,Zero,Sample,F1,Target,Start,Stop,ID,Setup,Accu,PrntHld,User"elseif system.modelStr=="ZQ375"then
MAX_SETPOINT_INPUT_INDEX=3
Input_Type_String={[0]="None     ","PrintKey ","UnitsKey ","SelectKey","TareKey  ","ZeroKey  ","F1Key    ","TargetKey","IDKey    ","UnderKey ","OverKey  ","AccumKey ","PrintHold","User     "}Input_Prompt_String="None,Print,Units,Select,Tare,Zero,F1,Target,ID,Under,Over,Accu,PrntHld,User"elseif system.modelStr=="ZM305GTN"then
MAX_SETPOINT_INPUT_INDEX=3
Input_Type_String={[0]="None     ","PrintKey ","UnitsKey ","SelectKey","TareKey  ","ZeroKey  ","SampleKey","F1Key    ","TargetKey","StartKey ","StopKey  ","IDKey    ","SetupKey ","AccumKey ","PrintHold","User     "}Input_Prompt_String="None,Print,Units,Select,Tare,Zero,Fleet,In-Out,Report,Start,Stop,ID,Setup,Accu,PrntHld,User"elseif system.modelStr=="ZM305"then
MAX_SETPOINT_INPUT_INDEX=3
Input_Type_String={[0]="None     ","PrintKey ","UnitsKey ","SelectKey","TareKey  ","ZeroKey  ","SampleKey","F1Key    ","TargetKey","StartKey ","StopKey  ","IDKey    ","SetupKey ","AccumKey ","PrintHold","User     "}Input_Prompt_String="None,Print,Units,Select,Tare,Zero,Sample,F1,Target,Start,Stop,ID,Setup,Accu,PrntHld,User"else
MAX_SETPOINT_INPUT_INDEX=3
Input_Type_String={[0]="None     ","PrintKey ","UnitsKey ","SelectKey","TareKey  ","ZeroKey  ","SampleKey","F1Key    ","TargetKey","StartKey ","StopKey  ","IDKey    ","SetupKey ","UnderKey ","OverKey  ","AccumKey ","BaseKey  ","PrintHold","User     "}Input_Prompt_String="None,Print,Units,Select,Tare,Zero,Sample,F1,Target,Start,Stop,ID,Setup,Under,Over,Accu,Base,PrntHld,User"end
batching.BatchingTypeDefault=0
batching.BatchingModeDefault=1
batching.BatchingDispDefault=1
batching.BatchingPreactDefault=0
batching.BatchingNegFilDefault=0
OutputValueDefault=0
PreactValueDefault=0
ActualValueDefault=0
StoppedDefault=0
InputValueDefault=0
batching.BatchingType=batching.BatchingTypeDefault
batching.BatchingMode=batching.BatchingModeDefault
batching.BatchingDisp=batching.BatchingDispDefault
batching.BatchingPreact=batching.BatchingPreactDefault
batching.BatchingNegFil=batching.BatchingNegFilDefault
for index=1,MAX_SETPOINT_OUTPUT_INDEX do
BatchingOutputtable[index]={}BatchingOutputtable[index].outputIndex=index
BatchingOutputtable[index].OutputValue=OutputValueDefault
BatchingOutputtable[index].curOutputValue=OutputValueDefault
BatchingOutputtable[index].PreactValue=PreactValueDefault
BatchingOutputtable[index].curPreactValue=PreactValueDefault
BatchingOutputtable[index].ActualValue=ActualValueDefault
BatchingOutputtable[index].curActualValue=ActualValueDefault
BatchingOutputtable[index].Stopped=StoppedDefault
BatchingOutputtable[index].units=BatchingCalUnit
end
for index=1,MAX_SETPOINT_INPUT_INDEX do
BatchingInputtable[index]={}BatchingInputtable[index].inputIndex=index
BatchingInputtable[index].InputValue=InputValueDefault
end
InvertOutputsFlagDefault=0
batching.InvertOutputsFlag=InvertOutputsFlagDefault
Input1ValueDefault=0
batching.Input1Value=Input1ValueDefault
Input2ValueDefault=0
batching.Input2Value=Input2ValueDefault
Input3ValueDefault=0
batching.Input3Value=Input3ValueDefault
batchOut1=0
batchOut2=0
batchOut3=0
batching.setpt_batching_up=4
batching.setpt_batching_down=5
minLatchTime=500
batchingFlag=false
batchingOut=0
tareBeforeStartFlag=false
stoppedFlag=false
newGraphType=0
function batching.initBatchPrintTokens()awtx.fmtPrint.varSet(4,BatchingOutputtable[1].curOutputValue,"Target1",AWTX_LUA_FLOAT)printTokens[4].varName="BatchingOutputtable[1].curOutputValue"printTokens[4].varLabel="Target1"printTokens[4].varType=AWTX_LUA_FLOAT
printTokens[4].varValue=BatchingOutputtable[1].curOutputValue
printTokens[4].varFunct=batching.setBatchOutValue1
awtx.fmtPrint.varSet(5,BatchingOutputtable[2].curOutputValue,"Target2",AWTX_LUA_FLOAT)printTokens[5].varName="BatchingOutputtable[2].curOutputValue"printTokens[5].varLabel="Target2"printTokens[5].varType=AWTX_LUA_FLOAT
printTokens[5].varValue=BatchingOutputtable[2].curOutputValue
printTokens[5].varFunct=batching.setBatchOutValue2
awtx.fmtPrint.varSet(6,BatchingOutputtable[3].curOutputValue,"Target3",AWTX_LUA_FLOAT)printTokens[6].varName="BatchingOutputtable[3].curOutputValue"printTokens[6].varLabel="Target3"printTokens[6].varType=AWTX_LUA_FLOAT
printTokens[6].varValue=BatchingOutputtable[3].curOutputValue
printTokens[6].varFunct=batching.setBatchOutValue3
awtx.fmtPrint.varSet(7,BatchingOutputtable[1].curPreactValue,"Preact1",AWTX_LUA_FLOAT)printTokens[7].varName="BatchingOutputtable[1].curPreactValue"printTokens[7].varLabel="Preact1"printTokens[7].varType=AWTX_LUA_FLOAT
printTokens[7].varValue=BatchingOutputtable[1].curPreactValue
printTokens[7].varFunct=batching.setBatchPreactValue1
awtx.fmtPrint.varSet(8,BatchingOutputtable[2].curPreactValue,"Preact2",AWTX_LUA_FLOAT)printTokens[8].varName="BatchingOutputtable[2].curPreactValue"printTokens[8].varLabel="Preact2"printTokens[8].varType=AWTX_LUA_FLOAT
printTokens[8].varValue=BatchingOutputtable[2].curPreactValue
printTokens[8].varFunct=batching.setBatchPreactValue2
awtx.fmtPrint.varSet(9,BatchingOutputtable[3].curPreactValue,"Preact3",AWTX_LUA_FLOAT)printTokens[9].varName="BatchingOutputtable[3].curPreactValue"printTokens[9].varLabel="Preact3"printTokens[9].varType=AWTX_LUA_FLOAT
printTokens[9].varValue=BatchingOutputtable[3].curPreactValue
printTokens[9].varFunct=batching.setBatchPreactValue3
awtx.fmtPrint.varSet(10,BatchingOutputtable[1].curActualValue,"Actual1",AWTX_LUA_FLOAT)printTokens[10].varName="BatchingOutputtable[1].curActualValue"printTokens[10].varLabel="Actual1"printTokens[10].varType=AWTX_LUA_FLOAT
printTokens[10].varValue=BatchingOutputtable[1].curActualValue
printTokens[10].varFunct=""awtx.fmtPrint.varSet(11,BatchingOutputtable[2].curActualValue,"Actual2",AWTX_LUA_FLOAT)printTokens[11].varName="BatchingOutputtable[2].curActualValue"printTokens[11].varLabel="Actual2"printTokens[11].varType=AWTX_LUA_FLOAT
printTokens[11].varValue=BatchingOutputtable[2].curActualValue
printTokens[11].varFunct=""awtx.fmtPrint.varSet(12,BatchingOutputtable[3].curActualValue,"Actual3",AWTX_LUA_FLOAT)printTokens[12].varName="BatchingOutputtable[3].curActualValue"printTokens[12].varLabel="Actual3"printTokens[12].varType=AWTX_LUA_FLOAT
printTokens[12].varValue=BatchingOutputtable[3].curActualValue
printTokens[12].varFunct=""awtx.fmtPrint.varSet(13,batching.Input1Value,Input_Type_String[BatchingInputtable[1].InputValue],AWTX_LUA_FLOAT)printTokens[13].varName="batching.Input1Value"printTokens[13].varLabel="Input_Type_String[BatchingInputtable[1].InputValue]"printTokens[13].varType=AWTX_LUA_INTEGER
printTokens[13].varValue=batching.Input1Value
printTokens[13].varFunct=batching.setInput1Value
awtx.fmtPrint.varSet(14,batching.Input2Value,Input_Type_String[BatchingInputtable[2].InputValue],AWTX_LUA_FLOAT)printTokens[14].varName="batching.Input2Value"printTokens[14].varLabel="Input_Type_String[BatchingInputtable[2].InputValue]"printTokens[14].varType=AWTX_LUA_INTEGER
printTokens[14].varValue=batching.Input2Value
printTokens[14].varFunct=batching.setInput2Value
awtx.fmtPrint.varSet(15,batching.Input3Value,Input_Type_String[BatchingInputtable[3].InputValue],AWTX_LUA_FLOAT)printTokens[15].varName="batching.Input3Value"printTokens[15].varLabel="Input_Type_String[BatchingInputtable[3].InputValue]"printTokens[15].varType=AWTX_LUA_INTEGER
printTokens[15].varValue=batching.Input3Value
printTokens[15].varFunct=batching.setInput3Value
end
function batching.setBatchPrintTokens()local tempUnitIndex=wt.units
BatchingOutputtable[1].curOutputValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].OutputValue,tempUnitIndex,1)BatchingOutputtable[2].curOutputValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].OutputValue,tempUnitIndex,1)BatchingOutputtable[3].curOutputValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[3].OutputValue,tempUnitIndex,1)BatchingOutputtable[1].curPreactValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].PreactValue,tempUnitIndex,1)BatchingOutputtable[2].curPreactValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].PreactValue,tempUnitIndex,1)BatchingOutputtable[3].curPreactValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[3].PreactValue,tempUnitIndex,1)BatchingOutputtable[1].curActualValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].ActualValue,tempUnitIndex,1)BatchingOutputtable[2].curActualValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].ActualValue,tempUnitIndex,1)BatchingOutputtable[3].curActualValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[3].ActualValue,tempUnitIndex,1)awtx.fmtPrint.varSet(4,BatchingOutputtable[1].curOutputValue,"Target1",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(5,BatchingOutputtable[2].curOutputValue,"Target2",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(6,BatchingOutputtable[3].curOutputValue,"Target3",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(7,BatchingOutputtable[1].curPreactValue,"Preact1",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(8,BatchingOutputtable[2].curPreactValue,"Preact2",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(9,BatchingOutputtable[3].curPreactValue,"Preact3",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(10,BatchingOutputtable[1].curActualValue,"Actual1",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(11,BatchingOutputtable[2].curActualValue,"Actual2",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(12,BatchingOutputtable[3].curActualValue,"Actual3",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(13,batching.Input1Value,Input_Type_String[BatchingInputtable[1].InputValue],AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(14,batching.Input2Value,Input_Type_String[BatchingInputtable[2].InputValue],AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(15,batching.Input3Value,Input_Type_String[BatchingInputtable[3].InputValue],AWTX_LUA_FLOAT)end
function batching.setOutputValues()awtx.setPoint.varSet("UserOutput1",0)awtx.setPoint.varSet("UserOutput2",0)awtx.setPoint.varSet("UserOutput3",0)batching.doneBatch()end
local DB_FileLocation_AppConfig
local DB_FileLocation_AppData
local DB_FileLocation_Reports
local DB_ReportName_Batching
function batching.setpointOut1Handler(setpointNum,isActivate)if isActivate then
if batching.BatchingType==0 then
elseif batching.BatchingType==1 then
elseif batching.BatchingType==2 then
elseif batching.BatchingType==3 then
end
else
if batching.BatchingType==0 then
elseif batching.BatchingType==1 then
batching.stopOut1()elseif batching.BatchingType==2 then
batching.stopOut123()elseif batching.BatchingType==3 then
batching.stopFill1()end
end
end
function batching.setpointOut2Handler(setpointNum,isActivate)if isActivate then
if batching.BatchingType==0 then
elseif batching.BatchingType==1 then
elseif batching.BatchingType==2 then
elseif batching.BatchingType==3 then
end
else
if batching.BatchingType==0 then
batching.stop2SpeedOut12()elseif batching.BatchingType==1 then
batching.stopOut2()elseif batching.BatchingType==2 then
batching.stopOut123()elseif batching.BatchingType==3 then
batching.stopDischarge2()end
end
end
function batching.setpointOut3Handler(setpointNum,isActivate)if isActivate then
if batching.BatchingType==0 then
elseif batching.BatchingType==1 then
elseif batching.BatchingType==2 then
elseif batching.BatchingType==3 then
end
else
if batching.BatchingType==0 then
elseif batching.BatchingType==1 then
batching.stopOut3()elseif batching.BatchingType==2 then
batching.stopOut123()elseif batching.BatchingType==3 then
end
end
end
function batching.setpointOut4Handler(setpointNum,isActivate)if isActivate then
else
end
end
function batching.setpointOut5Handler(setpointNum,isActivate)if isActivate then
else
end
end
function batching.setpointOut6Handler(setpointNum,isActivate)if isActivate then
else
end
end
function batching.setpointOut7Handler(setpointNum,isActivate)if isActivate then
else
end
end
function batching.setpointOut8Handler(setpointNum,isActivate)if isActivate then
else
end
end
function batching.setpointOut9Handler(setpointNum,isActivate)if isActivate then
else
end
end
function batching.setpointOut10Handler(setpointNum,isActivate)if isActivate then
else
end
end
function batching.disableSetpointsOutputs()local setPointDisabled,setPointBattery,retVal,resultMsg,result
setPointDisabled={mode="disabled"}setPointBattery={mode="output",bounceTime=minLatchTime,offInsideGZB=false,act="user",deact="user"}retVal,resultMsg=awtx.setPoint.set(1,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(2,setPointDisabled)if battery.BatteryEnable==1 then
retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)else
retVal,resultMsg=awtx.setPoint.set(3,setPointDisabled)end
result=awtx.setPoint.unregisterOutputEvent(1)result=awtx.setPoint.unregisterOutputEvent(2)result=awtx.setPoint.unregisterOutputEvent(3)result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(3)end
function batching.refreshSetpointsOutputs(cfg)local setPointOutEx1e,setPointOutEx1f,setPointOutEx1g
local setPointOutEx2e,setPointOutEx2f,setPointOutEx2g
local setPointOutEx3e,setPointOutEx3f
local setPointDisabled,setPointUser,retVal,resultMsg,result,newbasis
local offInsideGZBFlag=false
if batching.BatchingDisp==0 then
newbasis="grossWt"else
newbasis="netWt"end
setPointOutEx1e={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="user",deact="above",deactLowerVarName="UserOutput1",deactBasis=newbasis,deactMotionInhibit=false}setPointOutEx1f={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="user",deact="below",deactLowerVarName="UserOutput1",deactBasis=newbasis,deactMotionInhibit=false}setPointOutEx1g={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="user",deact="above",deactLowerVarName="UserOutput1",deactBasis="grossWt",deactMotionInhibit=false}setPointOutEx2e={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="user",deact="above",deactLowerVarName="UserOutput2",deactBasis=newbasis,deactMotionInhibit=false}setPointOutEx2f={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="user",deact="below",deactLowerVarName="UserOutput2",deactBasis=newbasis,deactMotionInhibit=false}setPointOutEx2g={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="user",deact="below",deactLowerVarName="UserOutput2",deactBasis="netWt",deactMotionInhibit=false}setPointOutEx3e={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="user",deact="above",deactLowerVarName="UserOutput3",deactBasis=newbasis,deactMotionInhibit=false}setPointOutEx3f={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="user",deact="below",deactLowerVarName="UserOutput3",deactBasis=newbasis,deactMotionInhibit=false}setPointDisabled={mode="disabled"}setPointUser={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="user",deact="user"}if(cfg==batching.setpt_batching_up)then
if batching.BatchingType==0 then
retVal,resultMsg=awtx.setPoint.set(1,setPointOutEx1e)retVal,resultMsg=awtx.setPoint.set(2,setPointOutEx2e)retVal,resultMsg=awtx.setPoint.set(3,setPointUser)elseif batching.BatchingType==1 then
retVal,resultMsg=awtx.setPoint.set(1,setPointOutEx1e)retVal,resultMsg=awtx.setPoint.set(2,setPointOutEx2e)retVal,resultMsg=awtx.setPoint.set(3,setPointOutEx3e)elseif batching.BatchingType==2 then
retVal,resultMsg=awtx.setPoint.set(1,setPointOutEx1e)retVal,resultMsg=awtx.setPoint.set(2,setPointOutEx2e)retVal,resultMsg=awtx.setPoint.set(3,setPointOutEx3e)elseif batching.BatchingType==3 then
retVal,resultMsg=awtx.setPoint.set(1,setPointOutEx1g)retVal,resultMsg=awtx.setPoint.set(2,setPointOutEx2g)retVal,resultMsg=awtx.setPoint.set(3,setPointUser)end
elseif(cfg==batching.setpt_batching_down)then
if batching.BatchingType==0 then
retVal,resultMsg=awtx.setPoint.set(1,setPointOutEx1f)retVal,resultMsg=awtx.setPoint.set(2,setPointOutEx2f)retVal,resultMsg=awtx.setPoint.set(3,setPointUser)elseif batching.BatchingType==1 then
retVal,resultMsg=awtx.setPoint.set(1,setPointOutEx1f)retVal,resultMsg=awtx.setPoint.set(2,setPointOutEx2f)retVal,resultMsg=awtx.setPoint.set(3,setPointOutEx3f)elseif batching.BatchingType==2 then
retVal,resultMsg=awtx.setPoint.set(1,setPointOutEx1f)retVal,resultMsg=awtx.setPoint.set(2,setPointOutEx2f)retVal,resultMsg=awtx.setPoint.set(3,setPointOutEx3f)elseif batching.BatchingType==3 then
retVal,resultMsg=awtx.setPoint.set(1,setPointOutEx1g)retVal,resultMsg=awtx.setPoint.set(2,setPointOutEx2g)retVal,resultMsg=awtx.setPoint.set(3,setPointUser)end
end
retVal,resultMsg=awtx.setPoint.set(4,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(5,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(6,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(7,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(8,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(9,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(10,setPointDisabled)result=awtx.setPoint.registerOutputEvent(1,batching.setpointOut1Handler)result=awtx.setPoint.registerOutputEvent(2,batching.setpointOut2Handler)result=awtx.setPoint.registerOutputEvent(3,batching.setpointOut3Handler)result=awtx.setPoint.registerOutputEvent(4,batching.setpointOut4Handler)result=awtx.setPoint.registerOutputEvent(5,batching.setpointOut5Handler)result=awtx.setPoint.registerOutputEvent(6,batching.setpointOut6Handler)result=awtx.setPoint.registerOutputEvent(7,batching.setpointOut7Handler)result=awtx.setPoint.registerOutputEvent(8,batching.setpointOut8Handler)result=awtx.setPoint.registerOutputEvent(9,batching.setpointOut9Handler)result=awtx.setPoint.registerOutputEvent(10,batching.setpointOut10Handler)batching.setBatchPrintTokens()end
function batching.setpointInputAction(cfgInputType)if Input_Type_String[cfgInputType]=="None     "then
elseif Input_Type_String[cfgInputType]=="PrintKey "then
awtx.keypad.KEY_PRINT_DOWN()elseif Input_Type_String[cfgInputType]=="UnitsKey "then
awtx.keypad.KEY_UNITS_UP()elseif Input_Type_String[cfgInputType]=="SelectKey"then
awtx.keypad.KEY_SELECT_UP()elseif Input_Type_String[cfgInputType]=="TareKey  "then
awtx.keypad.KEY_TARE_DOWN()elseif Input_Type_String[cfgInputType]=="ZeroKey  "then
awtx.keypad.KEY_ZERO_DOWN()elseif Input_Type_String[cfgInputType]=="SampleKey"then
awtx.keypad.KEY_SAMPLE_UP()elseif Input_Type_String[cfgInputType]=="F1Key    "then
awtx.keypad.KEY_F1_UP()elseif Input_Type_String[cfgInputType]=="TargetKey"then
awtx.keypad.KEY_TARGET_UP()elseif Input_Type_String[cfgInputType]=="StartKey "then
awtx.keypad.KEY_START_UP()elseif Input_Type_String[cfgInputType]=="StopKey  "then
awtx.keypad.KEY_STOP_UP()elseif Input_Type_String[cfgInputType]=="IDKey    "then
awtx.keypad.KEY_ID_UP()elseif Input_Type_String[cfgInputType]=="SetupKey "then
awtx.keypad.KEY_SETUP_UP()elseif Input_Type_String[cfgInputType]=="UnderKey "then
awtx.keypad.KEY_UNDER_UP()elseif Input_Type_String[cfgInputType]=="OverKey  "then
awtx.keypad.KEY_OVER_UP()elseif Input_Type_String[cfgInputType]=="AccumKey "then
awtx.keypad.KEY_ACCUM_UP()elseif Input_Type_String[cfgInputType]=="BaseKey  "then
awtx.keypad.KEY_BASE_UP()elseif Input_Type_String[cfgInputType]=="PrintHold"then
printHoldFlag=HowManyRepeatsMakeAHold-1
awtx.keypad.KEY_PRINT_REPEAT()elseif Input_Type_String[cfgInputType]=="User     "then
else
end
end
function batching.setpointIn1Handler(setpointNum,inputState)if inputState then
batching.setpointInputAction(BatchingInputtable[1].InputValue)else
end
end
function batching.setpointIn2Handler(setpointNum,inputState)if inputState then
batching.setpointInputAction(BatchingInputtable[2].InputValue)else
end
end
function batching.setpointIn3Handler(setpointNum,inputState)if inputState then
batching.setpointInputAction(BatchingInputtable[3].InputValue)else
end
end
function batching.setInput1Value(newIn1)batching.Input1Value=newIn1
batching.setpointIn1Handler(1,batching.Input1Value)batching.BatchingDBStoreInputs()end
function batching.setInput2Value(newIn2)batching.Input2Value=newIn2
batching.setpointIn2Handler(2,batching.Input2Value)batching.BatchingDBStoreInputs()end
function batching.setInput3Value(newIn3)batching.Input3Value=newIn3
batching.setpointIn3Handler(3,batching.Input3Value)batching.BatchingDBStoreInputs()end
function batching.refreshSetpointsInputs()local setPointInEx1,setPointInEx2,setPointInEx3
local retVal,resultMsg,result
setPointInEx1={mode="input",bounceTime=1}setPointInEx2={mode="input",bounceTime=1}setPointInEx3={mode="input",bounceTime=1}retVal,resultMsg=awtx.setPoint.set(1,setPointInEx1)retVal,resultMsg=awtx.setPoint.set(2,setPointInEx2)retVal,resultMsg=awtx.setPoint.set(3,setPointInEx3)result=awtx.setPoint.registerInputEvent(1,batching.setpointIn1Handler)result=awtx.setPoint.registerInputEvent(2,batching.setpointIn2Handler)result=awtx.setPoint.registerInputEvent(3,batching.setpointIn3Handler)batching.setBatchPrintTokens()end
function batching.BatchingInit()for index=1,MAX_SETPOINT_OUTPUT_INDEX do
BatchingOutputtable[index]={}BatchingOutputtable[index].outputIndex=index
BatchingOutputtable[index].OutputValue=OutputValueDefault
BatchingOutputtable[index].PreactValue=PreactValueDefault
BatchingOutputtable[index].ActualValue=ActualValueDefault
BatchingOutputtable[index].Stopped=StoppedDefault
BatchingOutputtable[index].units=BatchingCalUnit
end
for index=1,MAX_SETPOINT_INPUT_INDEX do
BatchingInputtable[index]={}BatchingInputtable[index].inputIndex=index
BatchingInputtable[index].InputValue=InputValueDefault
end
batching.InvertOutputsFlag=InvertOutputsFlagDefault
end
function batching.BatchingDBInit()local simAppPath,dbFile,result
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]DB_FileLocation_AppData=[[C:\Database\AppData.db]]DB_FileLocation_Reports=[[G:\Reports]]..SerialNumber
DB_ReportName_Batching=[[\BatchingReport.csv]]dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingOutputs(outputIndex INTEGER, OutputValue DOUBLE, PreactValue DOUBLE, ActualValue DOUBLE, Stopped DOUBLE, units VARCHAR)")dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingConfig(varID TEXT PRIMARY KEY, value TEXT)")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingInputs(inputIndex INTEGER, InputValue INTEGER)")dbFile:close()end
function batching.BatchingDBStore()local dbFile,result
local sqlStr,searchIndex,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingOutputs (outputIndex INTEGER, OutputValue DOUBLE, PreactValue DOUBLE, ActualValue DOUBLE, Stopped DOUBLE, units VARCHAR)")dbFile:execute("BEGIN TRANSACTION")for index=1,MAX_SETPOINT_OUTPUT_INDEX do
searchIndex=index
sqlStr=string.format("SELECT tblBatchingOutputs.outputIndex FROM tblBatchingOutputs WHERE tblBatchingOutputs.outputIndex = '%d'",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblBatchingOutputs(outputIndex, OutputValue, PreactValue, ActualValue, Stopped, units) VALUES ('%d', '%f', '%f', '%f', '%f', '%s')",BatchingOutputtable[index].outputIndex,BatchingOutputtable[index].OutputValue,BatchingOutputtable[index].PreactValue,BatchingOutputtable[index].ActualValue,BatchingOutputtable[index].Stopped,BatchingOutputtable[index].units)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblBatchingOutputs SET outputIndex = '%d', OutputValue = '%f', PreactValue = '%f', ActualValue = '%f', Stopped = '%f', units = '%s' WHERE tblBatchingOutputs.outputIndex = '%d'",BatchingOutputtable[index].outputIndex,BatchingOutputtable[index].OutputValue,BatchingOutputtable[index].PreactValue,BatchingOutputtable[index].ActualValue,BatchingOutputtable[index].Stopped,BatchingOutputtable[index].units,searchIndex)result=dbFile:exec(sqlStr)end
end
dbFile:execute("COMMIT")dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingInputs (inputIndex INTEGER, InputValue INTEGER)")dbFile:execute("BEGIN TRANSACTION")for index=1,MAX_SETPOINT_INPUT_INDEX do
searchIndex=index
sqlStr=string.format("SELECT tblBatchingInputs.inputIndex FROM tblBatchingInputs WHERE tblBatchingInputs.inputIndex = '%d'",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblBatchingInputs(inputIndex, InputValue) VALUES ('%d', '%d')",BatchingInputtable[index].inputIndex,BatchingInputtable[index].InputValue)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblBatchingInputs SET inputIndex = '%d', InputValue = '%d' WHERE tblBatchingInputs.inputIndex = '%d'",BatchingInputtable[index].inputIndex,BatchingInputtable[index].InputValue,searchIndex)result=dbFile:exec(sqlStr)end
end
dbFile:execute("COMMIT")dbFile:close()batching.setBatchPrintTokens()end
function batching.BatchingDBStoreOutputs()local dbFile,result
local sqlStr,searchIndex,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingOutputs (outputIndex INTEGER, OutputValue DOUBLE, PreactValue DOUBLE, ActualValue DOUBLE, Stopped DOUBLE, units VARCHAR)")dbFile:execute("BEGIN TRANSACTION")for index=1,MAX_SETPOINT_OUTPUT_INDEX do
searchIndex=index
sqlStr=string.format("SELECT tblBatchingOutputs.outputIndex FROM tblBatchingOutputs WHERE tblBatchingOutputs.outputIndex = '%d'",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblBatchingOutputs(outputIndex, OutputValue, PreactValue, ActualValue, Stopped, units) VALUES ('%d', '%f', '%f', '%f', '%f', '%s')",BatchingOutputtable[index].outputIndex,BatchingOutputtable[index].OutputValue,BatchingOutputtable[index].PreactValue,BatchingOutputtable[index].ActualValue,BatchingOutputtable[index].Stopped,BatchingOutputtable[index].units)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblBatchingOutputs SET outputIndex = '%d', OutputValue = '%f', PreactValue = '%f', ActualValue = '%f', Stopped = '%f', units = '%s' WHERE tblBatchingOutputs.outputIndex = '%d'",BatchingOutputtable[index].outputIndex,BatchingOutputtable[index].OutputValue,BatchingOutputtable[index].PreactValue,BatchingOutputtable[index].ActualValue,BatchingOutputtable[index].Stopped,BatchingOutputtable[index].units,searchIndex)result=dbFile:exec(sqlStr)end
end
dbFile:execute("COMMIT")dbFile:close()batching.setBatchPrintTokens()end
function batching.BatchingDBStoreInputs()local dbFile,result
local sqlStr,searchIndex,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingInputs (inputIndex INTEGER, InputValue INTEGER)")dbFile:execute("BEGIN TRANSACTION")for index=1,MAX_SETPOINT_INPUT_INDEX do
searchIndex=index
sqlStr=string.format("SELECT tblBatchingInputs.inputIndex FROM tblBatchingInputs WHERE tblBatchingInputs.inputIndex = '%d'",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblBatchingInputs(inputIndex, InputValue) VALUES ('%d', '%d')",BatchingInputtable[index].inputIndex,BatchingInputtable[index].InputValue)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblBatchingInputs SET inputIndex = '%d', InputValue = '%d' WHERE tblBatchingInputs.inputIndex = '%d'",BatchingInputtable[index].inputIndex,BatchingInputtable[index].InputValue,searchIndex)result=dbFile:exec(sqlStr)end
end
dbFile:execute("COMMIT")dbFile:close()batching.setBatchPrintTokens()end
function batching.BatchingDBRecall()local dbFile,result,index
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingOutputs(outputIndex INTEGER, OutputValue DOUBLE, PreactValue DOUBLE, ActualValue DOUBLE, Stopped DOUBLE, units VARCHAR)")for row in dbFile:rows("SELECT outputIndex, OutputValue, PreactValue, ActualValue, Stopped, units FROM tblBatchingOutputs")do
index=row[1]if index<=MAX_SETPOINT_OUTPUT_INDEX then
BatchingOutputtable[index].outputIndex=row[1]BatchingOutputtable[index].OutputValue=row[2]BatchingOutputtable[index].PreactValue=row[3]BatchingOutputtable[index].ActualValue=row[4]BatchingOutputtable[index].Stopped=row[5]BatchingOutputtable[index].units=row[6]end
end
dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingInputs(inputIndex INTEGER, InputValue INTEGER)")for row in dbFile:rows("SELECT inputIndex, InputValue FROM tblBatchingInputs")do
index=row[1]if index<=MAX_SETPOINT_INPUT_INDEX then
BatchingInputtable[index].inputIndex=row[1]BatchingInputtable[index].InputValue=row[2]end
end
dbFile:close()batching.setBatchPrintTokens()end
function batching.BatchingDBRecallOutputs()local dbFile,result,index
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingOutputs(outputIndex INTEGER, OutputValue DOUBLE, PreactValue DOUBLE, ActualValue DOUBLE, Stopped DOUBLE, units VARCHAR)")for row in dbFile:rows("SELECT outputIndex, OutputValue, PreactValue, ActualValue, Stopped, units FROM tblBatchingOutputs")do
index=row[1]if index<=MAX_SETPOINT_OUTPUT_INDEX then
BatchingOutputtable[index].outputIndex=row[1]BatchingOutputtable[index].OutputValue=row[2]BatchingOutputtable[index].PreactValue=row[3]BatchingOutputtable[index].ActualValue=row[4]BatchingOutputtable[index].Stopped=row[5]BatchingOutputtable[index].units=row[6]end
end
dbFile:close()batching.setBatchPrintTokens()end
function batching.BatchingDBRecallInputs()local dbFile,result,index
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingInputs(inputIndex INTEGER, InputValue INTEGER)")for row in dbFile:rows("SELECT inputIndex, InputValue FROM tblBatchingInputs")do
index=row[1]if index<=MAX_SETPOINT_INPUT_INDEX then
BatchingInputtable[index].inputIndex=row[1]BatchingInputtable[index].InputValue=row[2]end
end
dbFile:close()batching.setBatchPrintTokens()end
function batching.extraStuffStore()local dbFile,result,sqlStr,cfg
local BatchingType,BatchingMode,BatchingDisp,BatchingPreact,BatchingNegFil
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")BatchingType=batching.BatchingType
found=false
for row in dbFile:rows("SELECT varID FROM tblBatchingConfig WHERE varID = 'BatchingType'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblBatchingConfig (varID, value) VALUES ('BatchingType', '%d')",BatchingType)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblBatchingConfig SET value = '%d' WHERE varID = 'BatchingType'",BatchingType)result=dbFile:exec(sqlStr)end
BatchingMode=batching.BatchingMode
found=false
for row in dbFile:rows("SELECT varID FROM tblBatchingConfig WHERE varID = 'BatchingMode'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblBatchingConfig (varID, value) VALUES ('BatchingMode', '%d')",BatchingMode)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblBatchingConfig SET value = '%d' WHERE varID = 'BatchingMode'",BatchingMode)result=dbFile:exec(sqlStr)end
BatchingDisp=batching.BatchingDisp
found=false
for row in dbFile:rows("SELECT varID FROM tblBatchingConfig WHERE varID = 'BatchingDisp'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblBatchingConfig (varID, value) VALUES ('BatchingDisp', '%d')",BatchingDisp)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblBatchingConfig SET value = '%d' WHERE varID = 'BatchingDisp'",BatchingDisp)result=dbFile:exec(sqlStr)end
BatchingPreact=batching.BatchingPreact
found=false
for row in dbFile:rows("SELECT varID FROM tblBatchingConfig WHERE varID = 'BatchingPreact'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblBatchingConfig (varID, value) VALUES ('BatchingPreact', '%d')",BatchingPreact)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblBatchingConfig SET value = '%d' WHERE varID = 'BatchingPreact'",BatchingPreact)result=dbFile:exec(sqlStr)end
BatchingNegFil=batching.BatchingNegFil
found=false
for row in dbFile:rows("SELECT varID FROM tblBatchingConfig WHERE varID = 'BatchingNegFil'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblBatchingConfig (varID, value) VALUES ('BatchingNegFil', '%d')",BatchingNegFil)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblBatchingConfig SET value = '%d' WHERE varID = 'BatchingNegFil'",BatchingNegFil)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()if batching.BatchingNegFil==0 then
cfg=batching.setpt_batching_up
batching.refreshSetpointsOutputs(cfg)else
cfg=batching.setpt_batching_down
batching.refreshSetpointsOutputs(cfg)end
end
function batching.extraStuffRecall()local dbFile,result
local found=false
local BatchingType,BatchingDisp,BatchingMode,BatchingPreact,BatchingNegFil,cfg
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingConfig (varID TEXT PRIMARY KEY, value TEXT)")found=false
for row in dbFile:rows("SELECT varID, value FROM tblBatchingConfig WHERE varID = 'BatchingType'")do
found=true
BatchingType=tonumber(row[2])end
if found==false then
BatchingType=0
end
batching.BatchingType=BatchingType
found=false
for row in dbFile:rows("SELECT varID, value FROM tblBatchingConfig WHERE varID = 'BatchingMode'")do
found=true
BatchingMode=tonumber(row[2])end
if found==false then
BatchingMode=1
end
batching.BatchingMode=BatchingMode
found=false
for row in dbFile:rows("SELECT varID, value FROM tblBatchingConfig WHERE varID = 'BatchingDisp'")do
found=true
BatchingDisp=tonumber(row[2])end
if found==false then
BatchingDisp=1
end
batching.BatchingDisp=BatchingDisp
found=false
for row in dbFile:rows("SELECT varID, value FROM tblBatchingConfig WHERE varID = 'BatchingPreact'")do
found=true
BatchingPreact=tonumber(row[2])end
if found==false then
BatchingPreact=0
end
batching.BatchingPreact=BatchingPreact
found=false
for row in dbFile:rows("SELECT varID, value FROM tblBatchingConfig WHERE varID = 'BatchingNegFil'")do
found=true
BatchingNegFil=tonumber(row[2])end
if found==false then
BatchingNegFil=0
end
batching.BatchingNegFil=BatchingNegFil
dbFile:close()if batching.BatchingNegFil==0 then
cfg=batching.setpt_batching_up
batching.refreshSetpointsOutputs(cfg)else
cfg=batching.setpt_batching_down
batching.refreshSetpointsOutputs(cfg)end
end
function batching.enterBatchOutValue1(label)local newOut1,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()newOut1=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].OutputValue,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
newOut1,isEnterKey=awtx.keypad.enterWeightWithUnits(newOut1,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
batching.setBatchOutValue1(newOut1)end
return isEnterKey
end
function batching.setBatchOutValue1(newOut1)BatchingOutputtable[1].OutputValue=awtx.weight.convertToInternalCalUnit(newOut1,1)BatchingOutputtable[1].ActualValue=BatchingOutputtable[1].OutputValue
batching.BatchingDBStoreOutputs()end
function batching.enterBatchOutValue2(label)local newOut2,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()newOut2=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].OutputValue,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
newOut2,isEnterKey=awtx.keypad.enterWeightWithUnits(newOut2,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
batching.setBatchOutValue2(newOut2)end
return isEnterKey
end
function batching.setBatchOutValue2(newOut2)BatchingOutputtable[2].OutputValue=awtx.weight.convertToInternalCalUnit(newOut2,1)if batching.BatchingType==3 then
if BatchingOutputtable[2].OutputValue>0 then
BatchingOutputtable[2].OutputValue=-BatchingOutputtable[2].OutputValue
end
end
BatchingOutputtable[2].ActualValue=BatchingOutputtable[2].OutputValue
batching.BatchingDBStoreOutputs()end
function batching.enterBatchOutValue3(label)local newOut3,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()newOut3=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[3].OutputValue,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
newOut3,isEnterKey=awtx.keypad.enterWeightWithUnits(newOut3,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
batching.setBatchOutValue3(newOut3)end
return isEnterKey
end
function batching.setBatchOutValue3(newOut3)BatchingOutputtable[3].OutputValue=awtx.weight.convertToInternalCalUnit(newOut3,1)BatchingOutputtable[3].ActualValue=BatchingOutputtable[3].OutputValue
batching.BatchingDBStoreOutputs()end
function batching.enterBatchPreactValue1(label)local preact1,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()preact1=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].PreactValue,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
preact1,isEnterKey=awtx.keypad.enterWeightWithUnits(preact1,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
batching.setBatchPreactValue1(preact1)end
return isEnterKey
end
function batching.setBatchPreactValue1(preact1)BatchingOutputtable[1].PreactValue=awtx.weight.convertToInternalCalUnit(preact1,1)batching.BatchingDBStoreOutputs()end
function batching.enterBatchPreactValue2(label)local preact2,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()preact2=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].PreactValue,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
preact2,isEnterKey=awtx.keypad.enterWeightWithUnits(preact2,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
batching.setBatchPreactValue2(preact2)end
return isEnterKey
end
function batching.setBatchPreactValue2(preact2)BatchingOutputtable[2].PreactValue=awtx.weight.convertToInternalCalUnit(preact2,1)batching.BatchingDBStoreOutputs()end
function batching.enterBatchPreactValue3(label)local preact3,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()preact3=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[3].PreactValue,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
preact3,isEnterKey=awtx.keypad.enterWeightWithUnits(preact3,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
batching.setBatchPreactValue3(preact3)end
return isEnterKey
end
function batching.setBatchPreactValue3(preact3)BatchingOutputtable[3].PreactValue=awtx.weight.convertToInternalCalUnit(preact3,1)batching.BatchingDBStoreOutputs()end
function batching.enterBatchInputValue1(label)local newIn1,isEnterKey
batching.BatchingDBRecallInputs()newIn1=BatchingInputtable[1].InputValue
newIn1,isEnterKey=awtx.keypad.selectList(Input_Prompt_String,newIn1)awtx.display.writeLine(label)if isEnterKey then
batching.setBatchInputValue1(newIn1)end
end
function batching.setBatchInputValue1(newIn1)BatchingInputtable[1].InputValue=tonumber(newIn1)batching.BatchingDBStoreInputs()end
function batching.enterBatchInputValue2(label)local newIn2,isEnterKey
batching.BatchingDBRecallInputs()newIn2=BatchingInputtable[2].InputValue
newIn2,isEnterKey=awtx.keypad.selectList(Input_Prompt_String,newIn2)awtx.display.writeLine(label)if isEnterKey then
batching.setBatchInputValue2(newIn2)end
end
function batching.setBatchInputValue2(newIn2)BatchingInputtable[2].InputValue=tonumber(newIn2)batching.BatchingDBStoreInputs()end
function batching.enterBatchInputValue3(label)local newIn3,isEnterKey
batching.BatchingDBRecallInputs()newIn3=BatchingInputtable[3].InputValue
newIn3,isEnterKey=awtx.keypad.selectList(Input_Prompt_String,newIn3)awtx.display.writeLine(label)if isEnterKey then
batching.setBatchInputValue3(newIn3)end
end
function batching.setBatchInputValue3(newIn3)BatchingInputtable[3].InputValue=tonumber(newIn3)batching.BatchingDBStoreInputs()end
function batching.editBatchingType(label)local newBatchingType,isEnterKey
batching.extraStuffRecall()newBatchingType=batching.BatchingType
newBatchingType,isEnterKey=awtx.keypad.selectList("2 SPEED,INGRED,INDEP,FIL-DSC",newBatchingType)awtx.display.writeLine(label)if isEnterKey then
batching.BatchingType=newBatchingType
batching.extraStuffStore()else
end
end
function batching.editBatchingMode(label)local newBatchingMode,isEnterKey
batching.extraStuffRecall()newBatchingMode=batching.BatchingMode
newBatchingMode,isEnterKey=awtx.keypad.selectList("MANUAL,AUTO",newBatchingMode)awtx.display.writeLine(label)if isEnterKey then
batching.BatchingMode=newBatchingMode
batching.extraStuffStore()else
end
end
function batching.editBatchingDisp(label)local newBatchingDisp,isEnterKey
batching.extraStuffRecall()newBatchingDisp=batching.BatchingDisp
newBatchingDisp,isEnterKey=awtx.keypad.selectList("GROSS,NET,NET-A",newBatchingDisp)awtx.display.writeLine(label)if isEnterKey then
batching.BatchingDisp=newBatchingDisp
batching.extraStuffStore()else
end
end
function batching.editBatchingPreact(label)local newBatchingPreact,isEnterKey
batching.extraStuffRecall()newBatchingPreact=batching.BatchingPreact
newBatchingPreact,isEnterKey=awtx.keypad.selectList("MANUAL,AUTO,CLEAR",newBatchingPreact)awtx.display.writeLine(label)if isEnterKey then
if newBatchingPreact==2 then
BatchingOutputtable[1].PreactValue=0
BatchingOutputtable[1].ActualValue=BatchingOutputtable[1].OutputValue
BatchingOutputtable[1].Stopped=0
BatchingOutputtable[2].PreactValue=0
BatchingOutputtable[2].ActualValue=BatchingOutputtable[2].OutputValue
BatchingOutputtable[2].Stopped=0
BatchingOutputtable[3].PreactValue=0
BatchingOutputtable[3].ActualValue=BatchingOutputtable[3].OutputValue
BatchingOutputtable[3].Stopped=0
batching.BatchingDBStoreOutputs()else
batching.BatchingPreact=newBatchingPreact
end
batching.extraStuffStore()else
end
end
function batching.editBatchingNegFil(label)local newBatchingNegFil,isEnterKey
batching.extraStuffRecall()newBatchingNegFil=batching.BatchingNegFil
newBatchingNegFil,isEnterKey=awtx.keypad.selectList("OFF,ON",newBatchingNegFil)awtx.display.writeLine(label)if isEnterKey then
batching.BatchingNegFil=newBatchingNegFil
batching.extraStuffStore()else
end
end
function batching.BatchingDBReport(label)local usermode,index,isEnterKey,dbFile,result,row
local currentRPN,fho,err
local t={}currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)index,isEnterKey=awtx.keypad.selectList(" Port 1, Port 2, USB",0)awtx.display.writeLine(label)if isEnterKey then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(DB_FileLocation_AppData)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingOutputs(outputIndex INTEGER, OutputValue DOUBLE, PreactValue DOUBLE, ActualValue DOUBLE, Stopped DOUBLE, units VARCHAR)")if index==0 or index==1 then
t[#t+1]=string.format("OutIndex  OutputValue PreactValue ActualValue \r\n")elseif index==2 then
t[#t+1]=string.format("OutIndex, OutputValue, PreactValue, ActualValue, Units \r\n")end
for row in dbFile:rows("SELECT outputIndex, OutputValue, PreactValue, ActualValue, units FROM tblBatchingOutputs")do
if index==0 or index==1 then
t[#t+1]=string.format("%10d %11f %11f %10s \r\n",row[1],row[2],row[3],row[4],row[6])elseif index==2 then
t[#t+1]=string.format("%d, %f, %f, %s \r\n",row[1],row[2],row[3],row[4],row[6])else
end
end
dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblBatchingInputs(inputIndex INTEGER, InputValue INTEGER)")if index==0 or index==1 then
t[#t+1]=string.format("InputIndex InputType \r\n")elseif index==2 then
t[#t+1]=string.format("InputIndex, InputType \r\n")end
for row in dbFile:rows("SELECT inputIndex, InputValue FROM tblBatchingInputs ORDER BY rowid DESC")do
if index==0 or index==1 then
t[#t+1]=string.format("%10d %10s \r\n",row[1],Input_Type_String[row[2]])elseif index==2 then
t[#t+1]=string.format("%d, %s \r\n",row[1],Input_Type_String[row[2]])else
end
end
dbFile:close()if separatorChar==0 then
elseif separatorChar==1 then
for i,j in pairs(t)do
t[i]=string.gsub(t[i],"%,","%;")end
for i,j in pairs(t)do
t[i]=string.gsub(t[i],"%.","%,")end
end
if index==0 then
awtx.serial.send(1,table.concat(t))elseif index==1 then
awtx.serial.send(2,table.concat(t))elseif index==2 then
result=awtx.os.makeDirectory(DB_FileLocation_Reports)if result==0 then
fho,err=io.open(string.format("%s%s",DB_FileLocation_Reports,DB_ReportName_Batching),"w")if err then
awtx.display.writeLine("err "..err)awtx.os.sleep(canttime)awtx.os.sleep(canttime)else
fho:write(table.concat(t))fho:close()end
else
awtx.display.writeLine("err "..tostring(result))end
else
end
awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function batching.BatchingDBClear()batching.BatchingInit()batching.BatchingDBStore()batching.BatchingDBRecall()end
function batching.BatchingDBReset(label)local usermode,index,isEnterKey
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)batching.BatchingDBClear()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
end
end
batching.BatchingInit()batching.BatchingDBInit()batching.BatchingDBRecall()batching.extraStuffRecall()awtx.setPoint.setInvertingOutputFlag(batching.InvertOutputsFlag)function batching.calcPreact1()local tempOut,tempPreact,tempAct,tempUnitIndex,tempUnitStr,tempStopped
tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()tempOut=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].OutputValue,tempUnitIndex,1)tempPreact=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].PreactValue,tempUnitIndex,1)tempAct=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].ActualValue,tempUnitIndex,1)tempStopped=BatchingOutputtable[1].Stopped
if batching.BatchingPreact==0 then
else
if tempStopped==0 then
if tempAct~=0 then
tempPreact=tempPreact+((tempAct-tempOut)*0.7)else
tempPreact=0
end
end
end
BatchingOutputtable[1].OutputValue=awtx.weight.convertToInternalCalUnit(tempOut,1)BatchingOutputtable[1].PreactValue=awtx.weight.convertToInternalCalUnit(tempPreact,1)BatchingOutputtable[1].ActualValue=awtx.weight.convertToInternalCalUnit(tempAct,1)BatchingOutputtable[1].Stopped=0
batching.BatchingDBStoreOutputs()batchOut1=tempOut-tempPreact
end
function batching.calcOutput1()local tempOut,tempPreact,tempAct,tempUnitIndex,tempUnitStr,tempStopped
tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()tempOut=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].OutputValue,tempUnitIndex,1)tempPreact=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].PreactValue,tempUnitIndex,1)tempAct=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].ActualValue,tempUnitIndex,1)tempStopped=BatchingOutputtable[1].Stopped
batchOut1=tempOut-tempPreact
end
function batching.calcPreactFill1()local tempOut,tempPreact,tempAct,tempUnitIndex,tempUnitStr,tempStopped
tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()tempOut=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].OutputValue,tempUnitIndex,1)tempPreact=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].PreactValue,tempUnitIndex,1)tempAct=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].ActualValue,tempUnitIndex,1)tempStopped=BatchingOutputtable[1].Stopped
if batching.BatchingPreact==0 then
else
if tempStopped==0 then
end
end
BatchingOutputtable[1].OutputValue=awtx.weight.convertToInternalCalUnit(tempOut,1)BatchingOutputtable[1].PreactValue=awtx.weight.convertToInternalCalUnit(tempPreact,1)BatchingOutputtable[1].ActualValue=awtx.weight.convertToInternalCalUnit(tempAct,1)BatchingOutputtable[1].Stopped=0
batching.BatchingDBStoreOutputs()batchOut1=tempOut-tempPreact
end
function batching.calcOutputFill1()local tempOut,tempPreact,tempAct,tempUnitIndex,tempUnitStr,tempStopped
tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()tempOut=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].OutputValue,tempUnitIndex,1)tempPreact=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].PreactValue,tempUnitIndex,1)tempAct=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].ActualValue,tempUnitIndex,1)tempStopped=BatchingOutputtable[1].Stopped
batchOut1=tempOut-tempPreact
end
function batching.calcPreact2()local tempOut,tempPreact,tempAct,tempUnitIndex,tempUnitStr,tempStopped
tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()tempOut=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].OutputValue,tempUnitIndex,1)tempPreact=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].PreactValue,tempUnitIndex,1)tempAct=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].ActualValue,tempUnitIndex,1)tempStopped=BatchingOutputtable[2].Stopped
if batching.BatchingPreact==0 then
else
if tempStopped==0 then
if tempAct~=0 then
tempPreact=tempPreact+((tempAct-tempOut)*0.7)else
tempPreact=0
end
end
end
BatchingOutputtable[2].OutputValue=awtx.weight.convertToInternalCalUnit(tempOut,1)BatchingOutputtable[2].PreactValue=awtx.weight.convertToInternalCalUnit(tempPreact,1)BatchingOutputtable[2].ActualValue=awtx.weight.convertToInternalCalUnit(tempAct,1)BatchingOutputtable[2].Stopped=0
batching.BatchingDBStoreOutputs()batchOut2=tempOut-tempPreact
end
function batching.calcOutput2()local tempOut,tempPreact,tempAct,tempUnitIndex,tempUnitStr,tempStopped
tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()tempOut=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].OutputValue,tempUnitIndex,1)tempPreact=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].PreactValue,tempUnitIndex,1)tempAct=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].ActualValue,tempUnitIndex,1)tempStopped=BatchingOutputtable[2].Stopped
batchOut2=tempOut-tempPreact
end
function batching.calcPreact12()local tempOut,tempPreact,tempAct,tempUnitIndex,tempUnitStr,tempStopped
tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()tempOut=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].OutputValue,tempUnitIndex,1)tempPreact=0
tempAct=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].ActualValue,tempUnitIndex,1)tempStopped=BatchingOutputtable[1].Stopped
BatchingOutputtable[1].OutputValue=awtx.weight.convertToInternalCalUnit(tempOut,1)BatchingOutputtable[1].PreactValue=0
BatchingOutputtable[1].ActualValue=awtx.weight.convertToInternalCalUnit(tempAct,1)BatchingOutputtable[1].Stopped=0
batchOut1=tempOut-tempPreact
tempOut=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].OutputValue,tempUnitIndex,1)tempPreact=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].PreactValue,tempUnitIndex,1)tempAct=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].ActualValue,tempUnitIndex,1)tempStopped=BatchingOutputtable[2].Stopped
if batching.BatchingPreact==0 then
else
if tempStopped==0 then
if tempAct~=0 then
tempPreact=tempPreact+((tempAct-tempOut)*0.7)else
tempPreact=0
end
end
end
BatchingOutputtable[2].OutputValue=awtx.weight.convertToInternalCalUnit(tempOut,1)BatchingOutputtable[2].PreactValue=awtx.weight.convertToInternalCalUnit(tempPreact,1)BatchingOutputtable[2].ActualValue=awtx.weight.convertToInternalCalUnit(tempAct,1)BatchingOutputtable[2].Stopped=0
batching.BatchingDBStoreOutputs()batchOut2=tempOut-tempPreact
end
function batching.calcOutput12()local tempOut,tempPreact,tempAct,tempUnitIndex,tempUnitStr,tempStopped
tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()tempOut=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].OutputValue,tempUnitIndex,1)tempPreact=0
tempAct=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].ActualValue,tempUnitIndex,1)tempStopped=BatchingOutputtable[1].Stopped
batchOut1=tempOut-tempPreact
tempOut=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].OutputValue,tempUnitIndex,1)tempPreact=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].PreactValue,tempUnitIndex,1)tempAct=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].ActualValue,tempUnitIndex,1)tempStopped=BatchingOutputtable[2].Stopped
batchOut2=tempOut-tempPreact
end
function batching.calcPreact3()local tempOut,tempPreact,tempAct,tempUnitIndex,tempUnitStr,tempStopped
tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()tempOut=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[3].OutputValue,tempUnitIndex,1)tempPreact=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[3].PreactValue,tempUnitIndex,1)tempAct=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[3].ActualValue,tempUnitIndex,1)tempStopped=BatchingOutputtable[3].Stopped
if batching.BatchingPreact==0 then
else
if tempStopped==0 then
if tempAct~=0 then
tempPreact=tempPreact+((tempAct-tempOut)*0.7)else
tempPreact=0
end
end
end
BatchingOutputtable[3].OutputValue=awtx.weight.convertToInternalCalUnit(tempOut,1)BatchingOutputtable[3].PreactValue=awtx.weight.convertToInternalCalUnit(tempPreact,1)BatchingOutputtable[3].ActualValue=awtx.weight.convertToInternalCalUnit(tempAct,1)BatchingOutputtable[3].Stopped=0
batching.BatchingDBStoreOutputs()batchOut3=tempOut-tempPreact
end
function batching.calcOutput3()local tempOut,tempPreact,tempAct,tempUnitIndex,tempUnitStr,tempStopped
tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
batching.BatchingDBRecallOutputs()tempOut=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[3].OutputValue,tempUnitIndex,1)tempPreact=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[3].PreactValue,tempUnitIndex,1)tempAct=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[3].ActualValue,tempUnitIndex,1)tempStopped=BatchingOutputtable[3].Stopped
batchOut3=tempOut-tempPreact
end
function batching.setOutputs()awtx.setPoint.varSet("UserOutput1",batchOut1)awtx.setPoint.varSet("UserOutput2",batchOut2)awtx.setPoint.varSet("UserOutput3",batchOut3)batching.setBatchPrintTokens()end
MAX_LOOP=100
function batching.waitForMotion()local loop=0
local tmpMotion=false
wt=awtx.weight.getCurrent(1)loop=0
while wt.motion do
wt=awtx.weight.getCurrent(1)awtx.os.sleep(50)loop=loop+1
if loop>MAX_LOOP then
tmpMotion=true
break
end
end
return tmpMotion,wt.gross,wt.net
end
function batching.doneBatch()local usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("  Done ")result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(3)if batchingFlag==true then
end
awtx.setPoint.varSet("UserOutput1",0)awtx.setPoint.varSet("UserOutput2",0)awtx.setPoint.varSet("UserOutput3",0)batching.setBatchPrintTokens()if calcPreactWhen==calcPreactAfter then
if batching.BatchingType==0 then
batching.calcPreact12()elseif batching.BatchingType==1 then
batching.calcPreact1()batching.calcPreact2()batching.calcPreact3()elseif batching.BatchingType==2 then
batching.calcPreact1()batching.calcPreact2()batching.calcPreact3()elseif batching.BatchingType==3 then
batching.calcPreactFill1()end
end
batchingFlag=false
batchingOut=0
tareBeforeStartFlag=false
stoppedFlag=false
f1Press=0
if batching.BatchingType==0 then
elseif batching.BatchingType==1 then
elseif batching.BatchingType==2 then
elseif batching.BatchingType==3 then
end
newGraphType=0
awtx.weight.graphEnable(newscaleNumber,newGraphType)batching.BatchingDBStoreOutputs()awtx.os.sleep(250)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(currentRPN)end
function batching.startBatch()local usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine(" Start ")wt=awtx.weight.getCurrent(1)stoppedFlag=false
if batching.BatchingType==0 then
if batching.BatchingDisp==0 then
awtx.weight.setActiveValue(VAL_GROSS)batchingFlag=true
batchingOut=0
if BatchingOutputtable[2].OutputValue~=0 then
batchingOut=1
batching.start2SpeedOut12()end
if batchingOut==0 then
batching.doneBatch()end
else
if batchingFlag==true then
tareBeforeStartFlag=true
batching.TareSuccessful()else
tareBeforeStartFlag=true
awtx.weight.requestTare()end
end
elseif batching.BatchingType==1 then
if batching.BatchingDisp==0 then
awtx.weight.setActiveValue(VAL_GROSS)if batchingFlag==false then
batchingFlag=true
batchingOut=0
if BatchingOutputtable[1].OutputValue~=0 and wt.gross<BatchingOutputtable[1].OutputValue then
batchingOut=1
batching.startOut1()elseif BatchingOutputtable[2].OutputValue~=0 and wt.gross<BatchingOutputtable[2].OutputValue then
batchingOut=2
batching.startOut2()elseif BatchingOutputtable[3].OutputValue~=0 and wt.gross<BatchingOutputtable[3].OutputValue then
batchingOut=3
batching.startOut3()end
if batchingOut==0 then
batching.doneBatch()end
else
if batchingOut==0 then
if BatchingOutputtable[1].OutputValue~=0 and wt.gross<BatchingOutputtable[1].OutputValue then
batchingOut=1
batching.startOut1()elseif BatchingOutputtable[2].OutputValue~=0 and wt.gross<BatchingOutputtable[2].OutputValue then
batchingOut=2
batching.startOut2()elseif BatchingOutputtable[3].OutputValue~=0 and wt.gross<BatchingOutputtable[3].OutputValue then
batchingOut=3
batching.startOut3()end
if batchingOut==0 then
batching.doneBatch()end
elseif batchingOut==1 then
if(bit.band(awtx.ports.getOutputs(),1)==1)then
result=awtx.setPoint.outputClr(1)BatchingOutputtable[1].Stopped=1
batching.stopOut1()elseif BatchingOutputtable[2].OutputValue~=0 and wt.gross<BatchingOutputtable[2].OutputValue then
batchingOut=2
BatchingOutputtable[2].Stopped=1
batching.startOut2()elseif BatchingOutputtable[3].OutputValue~=0 and wt.gross<BatchingOutputtable[3].OutputValue then
batchingOut=3
batching.startOut3()BatchingOutputtable[3].Stopped=1
else
batching.doneBatch()end
elseif batchingOut==2 then
if(bit.band(awtx.ports.getOutputs(),2)==2)then
result=awtx.setPoint.outputClr(2)BatchingOutputtable[2].Stopped=1
batching.stopOut2()elseif BatchingOutputtable[3].OutputValue~=0 and wt.gross<BatchingOutputtable[3].OutputValue then
batchingOut=3
BatchingOutputtable[3].Stopped=1
batching.startOut3()else
batching.doneBatch()end
elseif batchingOut==3 then
if(bit.band(awtx.ports.getOutputs(),4)==4)then
result=awtx.setPoint.outputClr(3)BatchingOutputtable[3].Stopped=1
batching.stopOut3()else
batching.doneBatch()end
end
end
else
if batchingFlag==true then
if batching.BatchingDisp==2 then
tareBeforeStartFlag=true
awtx.weight.requestTare()else
tareBeforeStartFlag=true
batching.TareSuccessful()end
else
if batchingOut==0 then
if BatchingOutputtable[1].OutputValue~=0 then
tareBeforeStartFlag=true
if batching.BatchingMode==1 or batching.BatchingDisp==2 then
awtx.weight.requestTare()else
batching.TareSuccessful()end
elseif BatchingOutputtable[2].OutputValue~=0 then
tareBeforeStartFlag=true
if batching.BatchingMode==1 or batching.BatchingDisp==2 then
awtx.weight.requestTare()else
batching.TareSuccessful()end
elseif BatchingOutputtable[3].OutputValue~=0 then
tareBeforeStartFlag=true
if batching.BatchingMode==1 or batching.BatchingDisp==2 then
awtx.weight.requestTare()else
batching.TareSuccessful()end
else
batching.doneBatch()end
elseif batchingOut==1 then
if BatchingOutputtable[2].OutputValue~=0 then
tareBeforeStartFlag=true
if batching.BatchingMode==1 or batching.BatchingDisp==2 then
awtx.weight.requestTare()else
batching.TareSuccessful()end
elseif BatchingOutputtable[3].OutputValue~=0 then
tareBeforeStartFlag=true
if batching.BatchingMode==1 or batching.BatchingDisp==2 then
awtx.weight.requestTare()else
batching.TareSuccessful()end
else
batching.doneBatch()end
elseif batchingOut==2 then
if BatchingOutputtable[3].OutputValue~=0 then
tareBeforeStartFlag=true
if batching.BatchingMode==1 or batching.BatchingDisp==2 then
awtx.weight.requestTare()else
batching.TareSuccessful()end
else
batching.doneBatch()end
elseif batchingOut==3 then
end
end
end
elseif batching.BatchingType==2 then
batching.BatchingPreact=0
if batching.BatchingDisp==0 then
awtx.weight.setActiveValue(VAL_GROSS)batchingFlag=true
batchingOut=0
if(BatchingOutputtable[1].OutputValue~=0)or(BatchingOutputtable[2].OutputValue~=0)or(BatchingOutputtable[3].OutputValue~=0)then
batchingOut=1
batching.startOut123()end
if batchingOut==0 then
batching.doneBatch()end
else
if batchingFlag==true then
tareBeforeStartFlag=true
batching.TareSuccessful()else
tareBeforeStartFlag=true
awtx.weight.requestTare()end
end
elseif batching.BatchingType==3 then
awtx.weight.setActiveValue(VAL_GROSS)batchingFlag=true
batchingOut=0
if BatchingOutputtable[1].OutputValue~=0 then
batchingOut=1
batching.startFill1()end
if batchingOut==0 then
batching.doneBatch()end
end
awtx.os.sleep(250)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(currentRPN)end
function batching.stopBatch()if stoppedFlag==false then
local usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine(" PAUSE ")stoppedFlag=true
if batching.BatchingType==0 then
if batchingOut==0 then
elseif batchingOut==1 then
BatchingOutputtable[1].Stopped=1
elseif batchingOut==2 then
BatchingOutputtable[2].Stopped=1
elseif batchingOut==3 then
BatchingOutputtable[3].Stopped=1
end
elseif batching.BatchingType==1 then
if batchingOut==0 then
elseif batchingOut==1 and(bit.band(awtx.ports.getOutputs(),1)==1)then
BatchingOutputtable[1].Stopped=1
batchingOut=0
elseif batchingOut==2 and(bit.band(awtx.ports.getOutputs(),2)==2)then
BatchingOutputtable[2].Stopped=1
batchingOut=1
elseif batchingOut==3 and(bit.band(awtx.ports.getOutputs(),4)==4)then
BatchingOutputtable[3].Stopped=1
batchingOut=2
end
elseif batching.BatchingType==2 then
BatchingOutputtable[1].Stopped=1
BatchingOutputtable[2].Stopped=1
BatchingOutputtable[3].Stopped=1
elseif batching.BatchingType==3 then
BatchingOutputtable[1].Stopped=1
BatchingOutputtable[2].Stopped=1
end
result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(3)awtx.os.sleep(250)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(currentRPN)else
result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(3)stoppedFlag=false
if batching.BatchingType==0 then
batching.doneBatch()elseif batching.BatchingType==1 then
batching.doneBatch()elseif batching.BatchingType==2 then
batching.doneBatch()elseif batching.BatchingType==3 then
batching.doneBatch()end
BatchingOutputtable[1].Stopped=1
BatchingOutputtable[2].Stopped=1
BatchingOutputtable[3].Stopped=1
end
batching.BatchingDBStoreOutputs()end
function batching.start2SpeedOut12()local tempOutputValue,tempUnitIndex,result
if calcPreactWhen==calcPreactBefore then
batching.calcPreact12()else
batching.calcOutput12()end
batching.setOutputs()result=awtx.setPoint.outputSet(3)result=awtx.setPoint.outputSet(1)result=awtx.setPoint.outputSet(2)newGraphType=2
tempUnitIndex=wt.units
tempOutputValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].OutputValue,tempUnitIndex,1)if batching.BatchingNegFil==0 then
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,0,tempOutputValue)else
awtx.weight.setBar(newscaleNumber,VAL_NET,0,tempOutputValue)end
else
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,tempOutputValue,0)else
awtx.weight.setBar(newscaleNumber,VAL_NET,tempOutputValue,0)end
end
awtx.weight.graphEnable(newscaleNumber,newGraphType)end
function batching.stop2SpeedOut12()local tmpmotion,tmpgross,tmpnet
tmpmotion,tmpgross,tmpnet=batching.waitForMotion()newGraphType=0
awtx.weight.graphEnable(newscaleNumber,newGraphType)result=awtx.setPoint.outputClr(3)if batching.BatchingDisp==0 then
awtx.weight.setActiveValue(VAL_GROSS)BatchingOutputtable[1].ActualValue=BatchingOutputtable[1].OutputValue
BatchingOutputtable[2].ActualValue=awtx.weight.convertToInternalCalUnit(tmpgross,1)else
awtx.weight.setActiveValue(VAL_NET)BatchingOutputtable[1].ActualValue=BatchingOutputtable[1].OutputValue
BatchingOutputtable[2].ActualValue=awtx.weight.convertToInternalCalUnit(tmpnet,1)end
batching.BatchingDBStoreOutputs()if batching.BatchingMode==1 then
else
end
batching.doneBatch()end
function batching.startOut1()local tempOutputValue,tempUnitIndex,result
if calcPreactWhen==calcPreactBefore then
batching.calcPreact1()else
batching.calcOutput1()end
batching.setOutputs()result=awtx.setPoint.outputSet(1)newGraphType=2
tempUnitIndex=wt.units
tempOutputValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].OutputValue,tempUnitIndex,1)if batching.BatchingNegFil==0 then
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,0,tempOutputValue)else
awtx.weight.setBar(newscaleNumber,VAL_NET,0,tempOutputValue)end
else
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,tempOutputValue,0)else
awtx.weight.setBar(newscaleNumber,VAL_NET,tempOutputValue,0)end
end
awtx.weight.graphEnable(newscaleNumber,newGraphType)end
function batching.stopOut1()local tmpmotion,tmpgross,tmpnet
tmpmotion,tmpgross,tmpnet=batching.waitForMotion()newGraphType=0
awtx.weight.graphEnable(newscaleNumber,newGraphType)if batching.BatchingDisp==0 then
awtx.weight.setActiveValue(VAL_GROSS)BatchingOutputtable[1].ActualValue=awtx.weight.convertToInternalCalUnit(tmpgross,1)else
awtx.weight.setActiveValue(VAL_NET)BatchingOutputtable[1].ActualValue=awtx.weight.convertToInternalCalUnit(tmpnet,1)end
batching.BatchingDBStoreOutputs()if batching.BatchingType==0 then
elseif batching.BatchingType==1 then
awtx.os.sleep(batchtime)if batching.BatchingMode==1 then
if batching.BatchingDisp==0 then
if BatchingOutputtable[2].OutputValue~=0 and BatchingOutputtable[1].ActualValue<BatchingOutputtable[2].OutputValue then
batchingOut=2
batching.startOut2()elseif BatchingOutputtable[3].OutputValue~=0 and BatchingOutputtable[1].ActualValue<BatchingOutputtable[3].OutputValue then
batchingOut=3
batching.startOut3()else
batching.doneBatch()end
else
if BatchingOutputtable[2].OutputValue~=0 then
tareBeforeStartFlag=true
awtx.weight.requestTare()elseif BatchingOutputtable[3].OutputValue~=0 then
tareBeforeStartFlag=true
awtx.weight.requestTare()else
batching.doneBatch()end
end
end
elseif batching.BatchingType==2 then
elseif batching.BatchingType==3 then
batching.doneBatch()end
end
function batching.startOut2()local tempOutputValue,tempUnitIndex,result
if calcPreactWhen==calcPreactBefore then
batching.calcPreact2()else
batching.calcOutput2()end
batching.setOutputs()result=awtx.setPoint.outputSet(2)newGraphType=2
tempUnitIndex=wt.units
tempOutputValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].OutputValue,tempUnitIndex,1)if batching.BatchingNegFil==0 then
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,0,tempOutputValue)else
awtx.weight.setBar(newscaleNumber,VAL_NET,0,tempOutputValue)end
else
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,tempOutputValue,0)else
awtx.weight.setBar(newscaleNumber,VAL_NET,tempOutputValue,0)end
end
awtx.weight.graphEnable(newscaleNumber,newGraphType)end
function batching.stopOut2()local tmpmotion,tmpgross,tmpnet
tmpmotion,tmpgross,tmpnet=batching.waitForMotion()newGraphType=0
awtx.weight.graphEnable(newscaleNumber,newGraphType)if batching.BatchingDisp==0 then
awtx.weight.setActiveValue(VAL_GROSS)BatchingOutputtable[2].ActualValue=awtx.weight.convertToInternalCalUnit(tmpgross,1)else
awtx.weight.setActiveValue(VAL_NET)BatchingOutputtable[2].ActualValue=awtx.weight.convertToInternalCalUnit(tmpnet,1)end
batching.BatchingDBStoreOutputs()if batching.BatchingType==0 then
elseif batching.BatchingType==1 then
awtx.os.sleep(batchtime)if batching.BatchingMode==1 then
if batching.BatchingDisp==0 then
if BatchingOutputtable[3].OutputValue~=0 and BatchingOutputtable[2].ActualValue<BatchingOutputtable[3].OutputValue then
batchingOut=3
batching.startOut3()else
batching.doneBatch()end
else
if BatchingOutputtable[3].OutputValue~=0 then
tareBeforeStartFlag=true
awtx.weight.requestTare()else
batching.doneBatch()end
end
end
elseif batching.BatchingType==2 then
elseif batching.BatchingType==3 then
end
end
function batching.startOut3()local tempOutputValue,tempUnitIndex,result
if calcPreactWhen==calcPreactBefore then
batching.calcPreact3()else
batching.calcOutput3()end
batching.setOutputs()result=awtx.setPoint.outputSet(3)newGraphType=2
tempUnitIndex=wt.units
tempOutputValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[3].OutputValue,tempUnitIndex,1)if batching.BatchingNegFil==0 then
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,0,tempOutputValue)else
awtx.weight.setBar(newscaleNumber,VAL_NET,0,tempOutputValue)end
else
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,tempOutputValue,0)else
awtx.weight.setBar(newscaleNumber,VAL_NET,tempOutputValue,0)end
end
awtx.weight.graphEnable(newscaleNumber,newGraphType)end
function batching.stopOut3()local tmpmotion,tmpgross,tmpnet
tmpmotion,tmpgross,tmpnet=batching.waitForMotion()newGraphType=0
awtx.weight.graphEnable(newscaleNumber,newGraphType)if batching.BatchingDisp==0 then
awtx.weight.setActiveValue(VAL_GROSS)BatchingOutputtable[3].ActualValue=awtx.weight.convertToInternalCalUnit(tmpgross,1)else
awtx.weight.setActiveValue(VAL_NET)BatchingOutputtable[3].ActualValue=awtx.weight.convertToInternalCalUnit(tmpnet,1)end
batching.BatchingDBStoreOutputs()if batching.BatchingType==0 then
elseif batching.BatchingType==1 then
if batching.BatchingMode==1 then
awtx.os.sleep(batchtime)batching.doneBatch()else
awtx.os.sleep(batchtime)batching.doneBatch()end
elseif batching.BatchingType==2 then
if batching.BatchingMode==1 then
awtx.os.sleep(batchtime)batching.doneBatch()else
awtx.os.sleep(batchtime)batching.doneBatch()end
elseif batching.BatchingType==3 then
end
end
function batching.startOut123()local tempOutputValue,tempUnitIndex,result
newGraphType=0
tempUnitIndex=wt.units
if(BatchingOutputtable[1].OutputValue~=0)then
if calcPreactWhen==calcPreactBefore then
batching.calcPreact1()else
batching.calcOutput1()end
batching.setOutputs()tempOutputValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].OutputValue,tempUnitIndex,1)if batching.BatchingNegFil==0 then
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,0,tempOutputValue)else
awtx.weight.setBar(newscaleNumber,VAL_NET,0,tempOutputValue)end
else
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,tempOutputValue,0)else
awtx.weight.setBar(newscaleNumber,VAL_NET,tempOutputValue,0)end
end
end
if(BatchingOutputtable[2].OutputValue~=0)then
if calcPreactWhen==calcPreactBefore then
batching.calcPreact2()else
batching.calcOutput2()end
batching.setOutputs()tempOutputValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].OutputValue,tempUnitIndex,1)if batching.BatchingNegFil==0 then
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,0,tempOutputValue)else
awtx.weight.setBar(newscaleNumber,VAL_NET,0,tempOutputValue)end
else
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,tempOutputValue,0)else
awtx.weight.setBar(newscaleNumber,VAL_NET,tempOutputValue,0)end
end
end
if(BatchingOutputtable[3].OutputValue~=0)then
if calcPreactWhen==calcPreactBefore then
batching.calcPreact3()else
batching.calcOutput3()end
batching.setOutputs()tempOutputValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[3].OutputValue,tempUnitIndex,1)if batching.BatchingNegFil==0 then
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,0,tempOutputValue)else
awtx.weight.setBar(newscaleNumber,VAL_NET,0,tempOutputValue)end
else
if batching.BatchingDisp==0 then
awtx.weight.setBar(newscaleNumber,VAL_GROSS,tempOutputValue,0)else
awtx.weight.setBar(newscaleNumber,VAL_NET,tempOutputValue,0)end
end
end
if(BatchingOutputtable[1].OutputValue~=0)or(BatchingOutputtable[2].OutputValue~=0)or(BatchingOutputtable[3].OutputValue~=0)then
result=awtx.setPoint.outputSet(1)end
if(BatchingOutputtable[1].OutputValue~=0)or(BatchingOutputtable[2].OutputValue~=0)or(BatchingOutputtable[3].OutputValue~=0)then
result=awtx.setPoint.outputSet(2)end
if(BatchingOutputtable[1].OutputValue~=0)or(BatchingOutputtable[2].OutputValue~=0)or(BatchingOutputtable[3].OutputValue~=0)then
result=awtx.setPoint.outputSet(3)end
awtx.weight.graphEnable(newscaleNumber,newGraphType)end
function batching.stopOut123()local tmpmotion,tmpgross,tmpnet
if(awtx.ports.getOutputs()==0)then
tmpmotion,tmpgross,tmpnet=batching.waitForMotion()newGraphType=0
awtx.weight.graphEnable(newscaleNumber,newGraphType)if batching.BatchingDisp==0 then
awtx.weight.setActiveValue(VAL_GROSS)BatchingOutputtable[1].ActualValue=awtx.weight.convertToInternalCalUnit(tmpgross,1)BatchingOutputtable[2].ActualValue=awtx.weight.convertToInternalCalUnit(tmpgross,1)BatchingOutputtable[3].ActualValue=awtx.weight.convertToInternalCalUnit(tmpgross,1)else
awtx.weight.setActiveValue(VAL_NET)BatchingOutputtable[1].ActualValue=awtx.weight.convertToInternalCalUnit(tmpnet,1)BatchingOutputtable[2].ActualValue=awtx.weight.convertToInternalCalUnit(tmpnet,1)BatchingOutputtable[3].ActualValue=awtx.weight.convertToInternalCalUnit(tmpnet,1)end
batching.BatchingDBStoreOutputs()if batching.BatchingType==2 then
if batching.BatchingMode==1 then
awtx.os.sleep(batchtime)batching.doneBatch()else
awtx.os.sleep(batchtime)batching.doneBatch()end
end
end
end
function batching.doneFill()local usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("  Done ")if batchingFlag==true then
end
awtx.setPoint.varSet("UserOutput1",0)awtx.setPoint.varSet("UserOutput2",0)awtx.setPoint.varSet("UserOutput3",0)batching.setBatchPrintTokens()if calcPreactWhen==calcPreactAfter then
if batching.BatchingType==0 then
batching.calcPreact12()elseif batching.BatchingType==1 then
batching.calcPreact1()batching.calcPreact2()batching.calcPreact3()elseif batching.BatchingType==2 then
batching.calcPreact1()batching.calcPreact2()batching.calcPreact3()elseif batching.BatchingType==3 then
batching.calcPreactFill1()end
end
batchingFlag=false
batchingOut=0
tareBeforeStartFlag=false
stoppedFlag=false
awtx.os.sleep(250)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(currentRPN)end
function batching.startFill1()local tempOutputValue,tempUnitIndex,result
if calcPreactWhen==calcPreactBefore then
batching.calcPreactFill1()else
batching.calcOutputFill1()end
batching.setOutputs()result=awtx.setPoint.outputSet(1)newGraphType=2
tempUnitIndex=wt.units
tempOutputValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[1].OutputValue,tempUnitIndex,1)awtx.weight.setBar(newscaleNumber,VAL_GROSS,0,tempOutputValue)awtx.weight.graphEnable(newscaleNumber,newGraphType)end
function batching.stopFill1()local tmpmotion,tmpgross,tmpnet
tmpmotion,tmpgross,tmpnet=batching.waitForMotion()newGraphType=0
awtx.weight.graphEnable(newscaleNumber,newGraphType)if batching.BatchingDisp==0 then
awtx.weight.setActiveValue(VAL_GROSS)BatchingOutputtable[1].ActualValue=awtx.weight.convertToInternalCalUnit(tmpgross,1)else
awtx.weight.setActiveValue(VAL_NET)BatchingOutputtable[1].ActualValue=awtx.weight.convertToInternalCalUnit(tmpnet,1)end
batching.BatchingDBStoreOutputs()batching.doneFill()end
function batching.doneDischarge()local usermode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)awtx.display.writeLine("  Done ")if batchingFlag==true then
end
awtx.setPoint.varSet("UserOutput1",0)awtx.setPoint.varSet("UserOutput2",0)awtx.setPoint.varSet("UserOutput3",0)batching.setBatchPrintTokens()if calcPreactWhen==calcPreactAfter then
if batching.BatchingType==0 then
batching.calcPreact12()elseif batching.BatchingType==1 then
batching.calcPreact1()batching.calcPreact2()batching.calcPreact3()elseif batching.BatchingType==2 then
batching.calcPreact1()batching.calcPreact2()batching.calcPreact3()elseif batching.BatchingType==3 then
batching.calcPreactFill1()end
end
batchingFlag=false
batchingOut=0
tareBeforeStartFlag=false
stoppedFlag=false
awtx.os.sleep(250)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)awtx.keypad.set_RPN_mode(currentRPN)end
function batching.startDischarge2()local tempOutputValue,tempUnitIndex,result
if calcPreactWhen==calcPreactBefore then
batching.calcPreact2()else
batching.calcOutput2()end
batching.setOutputs()result=awtx.setPoint.outputSet(2)newGraphType=2
tempUnitIndex=wt.units
tempOutputValue=awtx.weight.convertFromInternalCalUnit(BatchingOutputtable[2].OutputValue,tempUnitIndex,1)awtx.weight.setBar(newscaleNumber,VAL_NET,tempOutputValue,0)awtx.weight.graphEnable(newscaleNumber,newGraphType)end
function batching.stopDischarge2()local tmpmotion,tmpgross,tmpnet
tmpmotion,tmpgross,tmpnet=batching.waitForMotion()newGraphType=0
awtx.weight.graphEnable(newscaleNumber,newGraphType)if batching.BatchingDisp==0 then
awtx.weight.setActiveValue(VAL_GROSS)BatchingOutputtable[2].ActualValue=awtx.weight.convertToInternalCalUnit(tmpgross,1)else
awtx.weight.setActiveValue(VAL_NET)BatchingOutputtable[2].ActualValue=awtx.weight.convertToInternalCalUnit(tmpnet,1)end
batching.BatchingDBStoreOutputs()batching.doneDischarge()end
function batching.TareSuccessful()if batching.BatchingDisp==0 then
awtx.weight.setActiveValue(VAL_GROSS)else
awtx.weight.setActiveValue(VAL_NET)end
if tareBeforeStartFlag==true then
tareBeforeStartFlag=false
if batching.BatchingType==0 then
batchingFlag=true
batchingOut=0
if BatchingOutputtable[2].OutputValue~=0 then
batchingOut=1
batching.start2SpeedOut12()end
if batchingOut==0 then
batching.doneBatch()end
elseif batching.BatchingType==1 then
if batchingFlag==false then
batchingFlag=true
batchingOut=0
if BatchingOutputtable[1].OutputValue~=0 then
batchingOut=1
batching.startOut1()elseif BatchingOutputtable[2].OutputValue~=0 then
batchingOut=2
batching.startOut2()elseif BatchingOutputtable[3].OutputValue~=0 then
batchingOut=3
batching.startOut3()end
if batchingOut==0 then
batching.doneBatch()end
else
if batchingOut==0 then
if BatchingOutputtable[1].OutputValue~=0 then
batchingOut=1
batching.startOut1()elseif BatchingOutputtable[2].OutputValue~=0 then
batchingOut=2
batching.startOut2()elseif BatchingOutputtable[3].OutputValue~=0 then
batchingOut=3
batching.startOut3()else
batching.doneBatch()end
elseif batchingOut==1 then
if(bit.band(awtx.ports.getOutputs(),1)==1)then
result=awtx.setPoint.outputClr(1)batching.stopOut1()elseif BatchingOutputtable[2].OutputValue~=0 then
batchingOut=2
batching.startOut2()elseif BatchingOutputtable[3].OutputValue~=0 then
batchingOut=3
batching.startOut3()else
batching.doneBatch()end
elseif batchingOut==2 then
if(bit.band(awtx.ports.getOutputs(),2)==2)then
result=awtx.setPoint.outputClr(2)batching.stopOut2()elseif BatchingOutputtable[3].OutputValue~=0 then
batchingOut=3
batching.startOut3()else
batching.doneBatch()end
elseif batchingOut==3 then
if(bit.band(awtx.ports.getOutputs(),4)==4)then
result=awtx.setPoint.outputClr(3)batching.stopOut3()else
batching.doneBatch()end
end
end
elseif batching.BatchingType==2 then
batching.BatchingPreact=0
batchingFlag=true
batchingOut=0
if(BatchingOutputtable[1].OutputValue~=0)or(BatchingOutputtable[2].OutputValue~=0)or(BatchingOutputtable[3].OutputValue~=0)then
batchingOut=1
batching.startOut123()end
if batchingOut==0 then
batching.doneBatch()end
end
end
if batching.BatchingType==3 then
if BatchingOutputtable[2].OutputValue~=0 then
batching.startDischarge2()end
end
end