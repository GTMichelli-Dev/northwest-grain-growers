local MAX_SETPOINT_OUTPUT_INDEX=3
local MAX_SETPOINT_INPUT_INDEX=3
local minLatchTime=500
local DB_FileLocation_AppConfig
local DB_FileLocation_AppData
local DB_FileLocation_Reports
local DB_ReportName_Setpoint
setpoint={}Setpointtable={}SetpointOutputtable={}SetpointInputtable={}local newSetpoint=0
if config.calwtunitStr~=nil then
setpointCalUnit=config.calwtunitStr
end
setpoint.TargMin=-wt.curCapacity
setpoint.TargLo=0
setpoint.TargHi=0
setpoint.TargMax=wt.curCapacity
setpoint.TargBasis="grossWtTotal"local Output_Type_String={}local Output_Prompt_String
if system.modelStr=="ZM301"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above"}Output_Prompt_String="ACT-Abv,ACT-Bel"elseif system.modelStr=="ZM303"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above"}Output_Prompt_String="ACT-Abv,ACT-Bel"elseif system.modelStr=="ZQ375"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above"}Output_Prompt_String="ACT-Abv,ACT-Bel"elseif system.modelStr=="ZM305GTN"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside","Outside"}Output_Prompt_String="ACT-Abv,ACT-Bel,ACT- IN,ACT-OUT"elseif system.modelStr=="ZM305"then
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above","Inside","Outside"}Output_Prompt_String="ACT-Abv,ACT-Bel,ACT- IN,ACT-OUT"else
MAX_SETPOINT_OUTPUT_INDEX=3
Output_Type_String={[0]="Above_Below","Below_Above"}Output_Prompt_String="ACT-Abv,ACT-Bel"end
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
setpoint.basis="newWt"setpoint.setpt_above_below=1
setpoint.setpt_below_above=2
setpoint.setpt_inside=3
setpoint.setpt_outside=4
setpoint.setpt_checkweigh=5
InvertOutputsFlagDefault=0
Setpointtable.InvertOutputsFlag=InvertOutputsFlagDefault
OutputModeDefault=setpoint.setpt_above_below
OutputMode1Default=setpoint.setpt_above_below
OutputMode2Default=setpoint.setpt_above_below
OutputMode3Default=setpoint.setpt_above_below
Setpointtable.OutputMode=OutputModeDefault
Setpointtable.OutputMode1=OutputMode1Default
Setpointtable.OutputMode2=OutputMode2Default
Setpointtable.OutputMode3=OutputMode3Default
Input1ValueDefault=0
setpoint.Input1Value=Input1ValueDefault
Input2ValueDefault=0
setpoint.Input2Value=Input2ValueDefault
Input3ValueDefault=0
setpoint.Input3Value=Input3ValueDefault
OutputValueDefault=0
OutputValueLoDefault=0
OutputValueHiDefault=0
InputValueDefault=0
for index=1,MAX_SETPOINT_OUTPUT_INDEX do
SetpointOutputtable[index]={}SetpointOutputtable[index].outputIndex=index
SetpointOutputtable[index].OutputValue=OutputValueDefault
SetpointOutputtable[index].OutputValueLo=OutputValueLoDefault
SetpointOutputtable[index].OutputValueHi=OutputValueHiDefault
SetpointOutputtable[index].curOutputValue=OutputValueDefault
SetpointOutputtable[index].curOutputValueLo=OutputValueLoDefault
SetpointOutputtable[index].curOutputValueHi=OutputValueHiDefault
SetpointOutputtable[index].units=setpointCalUnit
end
for index=1,MAX_SETPOINT_INPUT_INDEX do
SetpointInputtable[index]={}SetpointInputtable[index].inputIndex=index
SetpointInputtable[index].InputValue=InputValueDefault
end
function setpoint.initSetpointPrintTokens()awtx.fmtPrint.varSet(4,SetpointOutputtable[1].curOutputValue,Input_Type_String[SetpointInputtable[1].InputValue],AWTX_LUA_FLOAT)printTokens[4].varName="SetpointOutputtable[1].curOutputValue"printTokens[4].varLabel="Input_Type_String[SetpointInputtable[1].InputValue]"printTokens[4].varType=AWTX_LUA_FLOAT
printTokens[4].varValue=SetpointOutputtable[1].curOutputValue
printTokens[4].varFunct=setpoint.setOut1
awtx.fmtPrint.varSet(5,SetpointOutputtable[2].curOutputValue,Input_Type_String[SetpointInputtable[2].InputValue],AWTX_LUA_FLOAT)printTokens[5].varName="SetpointOutputtable[2].curOutputValue"printTokens[5].varLabel="Input_Type_String[SetpointInputtable[2].InputValue]"printTokens[5].varType=AWTX_LUA_FLOAT
printTokens[5].varValue=SetpointOutputtable[2].curOutputValue
printTokens[5].varFunct=setpoint.setOut2
awtx.fmtPrint.varSet(6,SetpointOutputtable[3].curOutputValue,Input_Type_String[SetpointInputtable[3].InputValue],AWTX_LUA_FLOAT)printTokens[6].varName="SetpointOutputtable[3].curOutputValue"printTokens[6].varLabel="Input_Type_String[SetpointInputtable[3].InputValue]"printTokens[6].varType=AWTX_LUA_FLOAT
printTokens[6].varValue=SetpointOutputtable[3].curOutputValue
printTokens[6].varFunct=setpoint.setOut3
awtx.fmtPrint.varSet(7,SetpointOutputtable[1].curOutputValueLo,Input_Type_String[SetpointInputtable[1].InputValue],AWTX_LUA_FLOAT)printTokens[7].varName="SetpointOutputtable[1].curOutputValueLo"printTokens[7].varLabel="Input_Type_String[SetpointInputtable[1].InputValue]"printTokens[7].varType=AWTX_LUA_FLOAT
printTokens[7].varValue=SetpointOutputtable[1].curOutputValueLo
printTokens[7].varFunct=setpoint.setOut1Lo
awtx.fmtPrint.varSet(8,SetpointOutputtable[2].curOutputValueLo,Input_Type_String[SetpointInputtable[2].InputValue],AWTX_LUA_FLOAT)printTokens[8].varName="SetpointOutputtable[2].curOutputValueLo"printTokens[8].varLabel="Input_Type_String[SetpointInputtable[2].InputValue]"printTokens[8].varType=AWTX_LUA_FLOAT
printTokens[8].varValue=SetpointOutputtable[2].curOutputValueLo
printTokens[8].varFunct=setpoint.setOut2Lo
awtx.fmtPrint.varSet(9,SetpointOutputtable[3].curOutputValueLo,Input_Type_String[SetpointInputtable[3].InputValue],AWTX_LUA_FLOAT)printTokens[9].varName="SetpointOutputtable[3].curOutputValueLo"printTokens[9].varLabel="Input_Type_String[SetpointInputtable[3].InputValue]"printTokens[9].varType=AWTX_LUA_FLOAT
printTokens[9].varValue=SetpointOutputtable[3].curOutputValueLo
printTokens[9].varFunct=setpoint.setOut3Lo
awtx.fmtPrint.varSet(10,SetpointOutputtable[1].curOutputValueHi,Input_Type_String[SetpointInputtable[1].InputValue],AWTX_LUA_FLOAT)printTokens[10].varName="SetpointOutputtable[1].curOutputValueHi"printTokens[10].varLabel="Input_Type_String[SetpointInputtable[1].InputValue]"printTokens[10].varType=AWTX_LUA_FLOAT
printTokens[10].varValue=SetpointOutputtable[1].curOutputValueHi
printTokens[10].varFunct=setpoint.setOut1Hi
awtx.fmtPrint.varSet(11,SetpointOutputtable[2].curOutputValueHi,Input_Type_String[SetpointInputtable[2].InputValue],AWTX_LUA_FLOAT)printTokens[11].varName="SetpointOutputtable[2].curOutputValueHi"printTokens[11].varLabel="Input_Type_String[SetpointInputtable[2].InputValue]"printTokens[11].varType=AWTX_LUA_FLOAT
printTokens[11].varValue=SetpointOutputtable[2].curOutputValueHi
printTokens[11].varFunct=setpoint.setOut2Hi
awtx.fmtPrint.varSet(12,SetpointOutputtable[3].curOutputValueHi,Input_Type_String[SetpointInputtable[3].InputValue],AWTX_LUA_FLOAT)printTokens[12].varName="SetpointOutputtable[3].curOutputValueHi"printTokens[12].varLabel="Input_Type_String[SetpointInputtable[3].InputValue]"printTokens[12].varType=AWTX_LUA_FLOAT
printTokens[12].varValue=SetpointOutputtable[3].curOutputValueHi
printTokens[12].varFunct=setpoint.setOut3Hi
awtx.fmtPrint.varSet(13,setpoint.Input1Value,Input_Type_String[SetpointInputtable[1].InputValue],AWTX_LUA_FLOAT)printTokens[13].varName="setpoint.Input1Value"printTokens[13].varLabel="Input_Type_String[SetpointInputtable[1].InputValue]"printTokens[13].varType=AWTX_LUA_INTEGER
printTokens[13].varValue=setpoint.Input1Value
printTokens[13].varFunct=setpoint.setInput1Value
awtx.fmtPrint.varSet(14,setpoint.Input2Value,Input_Type_String[SetpointInputtable[2].InputValue],AWTX_LUA_FLOAT)printTokens[14].varName="setpoint.Input2Value"printTokens[14].varLabel="Input_Type_String[SetpointInputtable[2].InputValue]"printTokens[14].varType=AWTX_LUA_INTEGER
printTokens[14].varValue=setpoint.Input2Value
printTokens[14].varFunct=setpoint.setInput2Value
awtx.fmtPrint.varSet(15,setpoint.Input3Value,Input_Type_String[SetpointInputtable[3].InputValue],AWTX_LUA_FLOAT)printTokens[15].varName="setpoint.Input3Value"printTokens[15].varLabel="Input_Type_String[SetpointInputtable[3].InputValue]"printTokens[15].varType=AWTX_LUA_INTEGER
printTokens[15].varValue=setpoint.Input3Value
printTokens[15].varFunct=setpoint.setInput3Value
end
function setpoint.setSetpointPrintTokens()local tempUnitIndex=wt.units
if setpoint.basis=="count"then
SetpointOutputtable[1].curOutputValue=SetpointOutputtable[1].OutputValue
SetpointOutputtable[2].curOutputValue=SetpointOutputtable[2].OutputValue
SetpointOutputtable[3].curOutputValue=SetpointOutputtable[3].OutputValue
SetpointOutputtable[1].curOutputValueLo=SetpointOutputtable[1].OutputValueLo
SetpointOutputtable[2].curOutputValueLo=SetpointOutputtable[2].OutputValueLo
SetpointOutputtable[3].curOutputValueLo=SetpointOutputtable[3].OutputValueLo
SetpointOutputtable[1].curOutputValueHi=SetpointOutputtable[1].OutputValueHi
SetpointOutputtable[2].curOutputValueHi=SetpointOutputtable[2].OutputValueHi
SetpointOutputtable[3].curOutputValueHi=SetpointOutputtable[3].OutputValueHi
else
SetpointOutputtable[1].curOutputValue=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[1].OutputValue,tempUnitIndex,1)SetpointOutputtable[2].curOutputValue=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[2].OutputValue,tempUnitIndex,1)SetpointOutputtable[3].curOutputValue=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[3].OutputValue,tempUnitIndex,1)SetpointOutputtable[1].curOutputValueLo=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[1].OutputValueLo,tempUnitIndex,1)SetpointOutputtable[2].curOutputValueLo=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[2].OutputValueLo,tempUnitIndex,1)SetpointOutputtable[3].curOutputValueLo=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[3].OutputValueLo,tempUnitIndex,1)SetpointOutputtable[1].curOutputValueHi=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[1].OutputValueHi,tempUnitIndex,1)SetpointOutputtable[2].curOutputValueHi=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[2].OutputValueHi,tempUnitIndex,1)SetpointOutputtable[3].curOutputValueHi=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[3].OutputValueHi,tempUnitIndex,1)end
awtx.fmtPrint.varSet(4,SetpointOutputtable[1].curOutputValue,Input_Type_String[SetpointInputtable[1].InputValue],AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(5,SetpointOutputtable[2].curOutputValue,Input_Type_String[SetpointInputtable[2].InputValue],AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(6,SetpointOutputtable[3].curOutputValue,Input_Type_String[SetpointInputtable[3].InputValue],AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(7,SetpointOutputtable[1].curOutputValueLo,Input_Type_String[SetpointInputtable[1].InputValue],AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(8,SetpointOutputtable[2].curOutputValueLo,Input_Type_String[SetpointInputtable[2].InputValue],AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(9,SetpointOutputtable[3].curOutputValueLo,Input_Type_String[SetpointInputtable[3].InputValue],AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(10,SetpointOutputtable[1].curOutputValueHi,Input_Type_String[SetpointInputtable[1].InputValue],AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(11,SetpointOutputtable[2].curOutputValueHi,Input_Type_String[SetpointInputtable[2].InputValue],AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(12,SetpointOutputtable[3].curOutputValueHi,Input_Type_String[SetpointInputtable[3].InputValue],AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(13,setpoint.Input1Value,Input_Type_String[SetpointInputtable[1].InputValue],AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(14,setpoint.Input2Value,Input_Type_String[SetpointInputtable[2].InputValue],AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(15,setpoint.Input3Value,Input_Type_String[SetpointInputtable[3].InputValue],AWTX_LUA_FLOAT)end
function setpoint.setOutputValues()local tempUnitIndex
local tempOut1,tempOut1Lo,tempOut1Hi
local tempOut2,tempOut2Lo,tempOut2Hi
local tempOut3,tempOut3Lo,tempOut3Hi
if setpoint.basis=="count"then
tempOut1=SetpointOutputtable[1].OutputValue
tempOut1Lo=SetpointOutputtable[1].OutputValueLo
tempOut1Hi=SetpointOutputtable[1].OutputValueHi
tempOut2=SetpointOutputtable[2].OutputValue
tempOut2Lo=SetpointOutputtable[2].OutputValueLo
tempOut2Hi=SetpointOutputtable[2].OutputValueHi
tempOut3=SetpointOutputtable[3].OutputValue
tempOut3Lo=SetpointOutputtable[3].OutputValueLo
tempOut3Hi=SetpointOutputtable[3].OutputValueHi
else
tempUnitIndex=wt.units
tempOut1=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[1].OutputValue,tempUnitIndex,1)tempOut1Lo=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[1].OutputValueLo,tempUnitIndex,1)tempOut1Hi=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[1].OutputValueHi,tempUnitIndex,1)tempOut2=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[2].OutputValue,tempUnitIndex,1)tempOut2Lo=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[2].OutputValueLo,tempUnitIndex,1)tempOut2Hi=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[2].OutputValueHi,tempUnitIndex,1)tempOut3=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[3].OutputValue,tempUnitIndex,1)tempOut3Lo=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[3].OutputValueLo,tempUnitIndex,1)tempOut3Hi=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[3].OutputValueHi,tempUnitIndex,1)end
awtx.setPoint.varSet("UserOutput1",tempOut1)awtx.setPoint.varSet("UserOutput1Lo",tempOut1Lo)awtx.setPoint.varSet("UserOutput1Hi",tempOut1Hi)awtx.setPoint.varSet("UserOutput2",tempOut2)awtx.setPoint.varSet("UserOutput2Lo",tempOut2Lo)awtx.setPoint.varSet("UserOutput2Hi",tempOut2Hi)awtx.setPoint.varSet("UserOutput3",tempOut3)awtx.setPoint.varSet("UserOutput3Lo",tempOut3Lo)awtx.setPoint.varSet("UserOutput3Hi",tempOut3Hi)end
function setpoint.SetpointInit()for index=1,MAX_SETPOINT_OUTPUT_INDEX do
SetpointOutputtable[index].outputIndex=index
SetpointOutputtable[index].OutputValue=OutputValueDefault
SetpointOutputtable[index].OutputValueLo=OutputValueLoDefault
SetpointOutputtable[index].OutputValueHi=OutputValueHiDefault
SetpointOutputtable[index].units=setpointCalUnit
end
for index=1,MAX_SETPOINT_INPUT_INDEX do
SetpointInputtable[index].inputIndex=index
SetpointInputtable[index].InputValue=InputValueDefault
end
end
function setpoint.SetpointPrint()local widthprec
wt=awtx.weight.getCurrent(1)widthprec=wt.curDigitsTotal.."."..wt.curDigitsRight
for index=1,MAX_SETPOINT_OUTPUT_INDEX do
formatStr=string.gsub("\r\nOutputValue    %***f %s","***",widthprec)end
for index=1,MAX_SETPOINT_INPUT_INDEX do
end
end
function setpoint.SetpointDBInit()local dbFile,result,sqlStr,simAppPath
if awtx.os.amISimulator()then
simAppPath=awtx.os.applicationPath()DB_FileLocation_AppConfig=simAppPath..[[\AppConfig.db]]DB_FileLocation_AppData=simAppPath..[[\AppData.db]]DB_FileLocation_Reports=simAppPath..[[\Reports]]..SerialNumber
else
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]DB_FileLocation_AppData=[[C:\Database\AppData.db]]DB_FileLocation_Reports=[[G:\Reports]]..SerialNumber
end
DB_ReportName_Setpoint=[[\SetpointReport.csv]]dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointOutputs(outputIndex INTEGER, OutputValue, DOUBLE OutputValueLo DOUBLE, OutputValueHi DOUBLE, units VARCHAR)")dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointConfig(varID TEXT PRIMARY KEY, value TEXT)")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointInputs(inputIndex INTEGER, InputValue INTEGER)")dbFile:close()end
function setpoint.SetpointDBStore()local dbFile,result,sqlStr,searchIndex
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointOutputs(outputIndex INTEGER, OutputValue DOUBLE, OutputValueLo DOUBLE, OutputValueHi DOUBLE, units VARCHAR)")dbFile:execute("BEGIN TRANSACTION")for index=1,MAX_SETPOINT_OUTPUT_INDEX do
searchIndex=index
sqlStr=string.format("SELECT tblSetpointOutputs.outputIndex FROM tblSetpointOutputs WHERE tblSetpointOutputs.outputIndex = '%d'",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblSetpointOutputs(outputIndex, OutputValue, OutputValueLo, OutputValueHi, units) VALUES ('%d', '%f', '%f', '%f', '%s')",SetpointOutputtable[index].outputIndex,SetpointOutputtable[index].OutputValue,SetpointOutputtable[index].OutputValueLo,SetpointOutputtable[index].OutputValueHi,SetpointOutputtable[index].units)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblSetpointOutputs SET outputIndex = '%d', OutputValue = '%f', OutputValueLo = '%f', OutputValueHi = '%f', units = '%s' WHERE tblSetpointOutputs.outputIndex = '%d'",SetpointOutputtable[index].outputIndex,SetpointOutputtable[index].OutputValue,SetpointOutputtable[index].OutputValueLo,SetpointOutputtable[index].OutputValueHi,SetpointOutputtable[index].units,searchIndex)result=dbFile:exec(sqlStr)end
end
dbFile:execute("COMMIT")dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointInputs (inputIndex INTEGER, InputValue INTEGER)")dbFile:execute("BEGIN TRANSACTION")for index=1,MAX_SETPOINT_INPUT_INDEX do
searchIndex=index
sqlStr=string.format("SELECT tblSetpointInputs.inputIndex FROM tblSetpointInputs WHERE tblSetpointInputs.inputIndex = '%d'",searchIndex)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblSetpointInputs(inputIndex, InputValue) VALUES ('%d', '%d')",SetpointInputtable[index].inputIndex,SetpointInputtable[index].InputValue)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblSetpointInputs SET inputIndex = '%d', InputValue = '%d' WHERE inputIndex = '%d'",SetpointInputtable[index].inputIndex,SetpointInputtable[index].InputValue,searchIndex)result=dbFile:exec(sqlStr)end
end
dbFile:execute("COMMIT")dbFile:close()setpoint.setOutputValues()setpoint.setSetpointPrintTokens()end
function setpoint.extraStuffStore()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")found=false
for row in dbFile:rows("SELECT varID FROM tblSetpointConfig WHERE varID = 'InvertingOutputFlag'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblSetpointConfig (varID, value) VALUES ('InvertingOutputFlag', '%d')",Setpointtable.InvertOutputsFlag)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblSetpointConfig SET value = '%d' WHERE varID = 'InvertingOutputFlag'",Setpointtable.InvertOutputsFlag)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblSetpointConfig WHERE varID = 'OutputMode'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblSetpointConfig (varID, value) VALUES ('OutputMode', '%d')",Setpointtable.OutputMode)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblSetpointConfig SET value = '%d' WHERE varID = 'OutputMode'",Setpointtable.OutputMode)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblSetpointConfig WHERE varID = 'OutputMode1'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblSetpointConfig (varID, value) VALUES ('OutputMode1', '%d')",Setpointtable.OutputMode1)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblSetpointConfig SET value = '%d' WHERE varID = 'OutputMode1'",Setpointtable.OutputMode1)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblSetpointConfig WHERE varID = 'OutputMode2'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblSetpointConfig (varID, value) VALUES ('OutputMode2', '%d')",Setpointtable.OutputMode2)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblSetpointConfig SET value = '%d' WHERE varID = 'OutputMode2'",Setpointtable.OutputMode2)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblSetpointConfig WHERE varID = 'OutputMode3'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblSetpointConfig (varID, value) VALUES ('OutputMode3', '%d')",Setpointtable.OutputMode3)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblSetpointConfig SET value = '%d' WHERE varID = 'OutputMode3'",Setpointtable.OutputMode3)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()end
function setpoint.SetpointDBRecall()local dbFile,result,sqlStr,index
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointOutputs(outputIndex INTEGER, OutputValue DOUBLE, OutputValueLo DOUBLE, OutputValueHi DOUBLE, units VARCHAR)")for row in dbFile:rows("SELECT outputIndex, OutputValue, OutputValueLo, OutputValueHi, units FROM tblSetpointOutputs")do
index=row[1]if index<=MAX_SETPOINT_OUTPUT_INDEX then
SetpointOutputtable[index].outputIndex=row[1]SetpointOutputtable[index].OutputValue=row[2]SetpointOutputtable[index].OutputValueLo=row[3]SetpointOutputtable[index].OutputValueHi=row[4]SetpointOutputtable[index].units=row[5]end
end
dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointInputs(inputIndex INTEGER, InputValue INTEGER)")for row in dbFile:rows("SELECT inputIndex, InputValue FROM tblSetpointInputs")do
index=row[1]if index<=MAX_SETPOINT_INPUT_INDEX then
SetpointInputtable[index].inputIndex=row[1]SetpointInputtable[index].InputValue=row[2]end
end
dbFile:close()setpoint.setOutputValues()setpoint.setSetpointPrintTokens()end
function setpoint.extraStuffRecall()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointConfig (varID TEXT PRIMARY KEY, value TEXT)")found=false
for row in dbFile:rows("SELECT varID, value FROM tblSetpointConfig WHERE varID = 'InvertingOutputFlag'")do
found=true
Setpointtable.InvertOutputsFlag=tonumber(row[2])end
if found==false then
Setpointtable.InvertOutputsFlag=InvertOutputsFlagDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblSetpointConfig WHERE varID = 'OutputMode'")do
found=true
Setpointtable.OutputMode=tonumber(row[2])end
if found==false then
Setpointtable.OutputMode=OutputModeDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblSetpointConfig WHERE varID = 'OutputMode1'")do
found=true
Setpointtable.OutputMode1=tonumber(row[2])end
if found==false then
Setpointtable.OutputMode1=OutputMode1Default
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblSetpointConfig WHERE varID = 'OutputMode2'")do
found=true
Setpointtable.OutputMode2=tonumber(row[2])end
if found==false then
Setpointtable.OutputMode2=OutputMode2Default
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblSetpointConfig WHERE varID = 'OutputMode3'")do
found=true
Setpointtable.OutputMode3=tonumber(row[2])end
if found==false then
Setpointtable.OutputMode3=OutputMode3Default
end
dbFile:close()end
function setpoint.SetpointDBClear()setpoint.SetpointInit()setpoint.SetpointDBStore()setpoint.SetpointDBRecall()end
function setpoint.SetpointDBReport(label)if system.modelStr=="ZM301"then
setpoint.SetpointDBReport3xx(label)elseif system.modelStr=="ZM303"then
setpoint.SetpointDBReport3xx(label)elseif system.modelStr=="ZQ375"then
setpoint.SetpointDBReport3xx(label)elseif system.modelStr=="ZM305GTN"then
setpoint.SetpointDBReport305(label)elseif system.modelStr=="ZM305"then
setpoint.SetpointDBReport305(label)else
setpoint.SetpointDBReport3xx(label)end
end
function setpoint.SetpointDBReport3xx(label)local usermode,currentRPN,index,isEnterKey,dbFile,row,fho,err
local t={}currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)index,isEnterKey=awtx.keypad.selectList(" Port 1, Port 2, USB",0)awtx.display.writeLine(label)if isEnterKey then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(DB_FileLocation_AppData)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointOutputs(outputIndex INTEGER, OutputValue DOUBLE, OutputValueLo DOUBLE, OutputValueHi DOUBLE, units VARCHAR)")if index==0 or index==1 then
t[#t+1]=string.format("OutIndex                       OutputValue \r\n")elseif index==2 then
t[#t+1]=string.format("OutIndex, OutputValue, Units \r\n")end
for row in dbFile:rows("SELECT outputIndex, OutputValue, units FROM tblSetpointOutputs ORDER BY rowid DESC")do
if index==0 or index==1 then
t[#t+1]=string.format("%30d %10f %10s \r\n",row[1],row[2],row[3])elseif index==2 then
t[#t+1]=string.format("%d, %f, %s \r\n",row[1],row[2],row[3])else
end
end
dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointInputs(inputIndex INTEGER, InputValue INTEGER)")if index==0 or index==1 then
t[#t+1]=string.format("InputIndex                     InputType \r\n")elseif index==2 then
t[#t+1]=string.format("InputIndex, InputType \r\n")end
for row in dbFile:rows("SELECT inputIndex, InputValue FROM tblSetpointInputs ORDER BY rowid DESC")do
if index==0 or index==1 then
t[#t+1]=string.format("%30d %10s \r\n",row[1],Input_Type_String[row[2]])elseif index==2 then
t[#t+1]=string.format("%d, %s \r\n",row[1],Input_Type_String[row[2]])else
end
end
dbFile:close()if index==0 then
awtx.serial.send(1,table.concat(t))elseif index==1 then
awtx.serial.send(2,table.concat(t))elseif index==2 then
result=awtx.os.makeDirectory(DB_FileLocation_Reports)if result==0 then
fho,err=io.open(string.format("%s%s",DB_FileLocation_Reports,DB_ReportName_Setpoint),"w")if err then
awtx.display.writeLine("err "..err)awtx.os.sleep(canttime)awtx.os.sleep(canttime)else
fho:write(table.concat(t))fho:close()end
else
awtx.display.writeLine("err "..tostring(result))end
else
end
awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function setpoint.SetpointDBReport305(label)local usermode,currentRPN,index,isEnterKey,dbFile,row,fho,err
local t={}currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)index,isEnterKey=awtx.keypad.selectList(" Port 1, Port 2, USB",0)awtx.display.writeLine(label)if isEnterKey then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(DB_FileLocation_AppData)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointOutputs(outputIndex INTEGER, OutputValue DOUBLE, OutputValueLo DOUBLE, OutputValueHi DOUBLE, units VARCHAR)")if index==0 or index==1 then
t[#t+1]=string.format("OutIndex                       OutputValue OutputValueLo OutputValueHi Units\r\n")elseif index==2 then
t[#t+1]=string.format("OutIndex, OutputValue, OutputValueLo, OutputValueHi, Units \r\n")end
for row in dbFile:rows("SELECT outputIndex, OutputValue, OutputValueLo, OutputValueHi, units FROM tblSetpointOutputs ORDER BY rowid DESC")do
if index==0 or index==1 then
t[#t+1]=string.format("%30d %10f  %10f    %10f    %10s \r\n",row[1],row[2],row[3],row[4],row[5])elseif index==2 then
t[#t+1]=string.format("%d, %f, %f, %f, %s \r\n",row[1],row[2],row[3],row[4],row[5])else
end
end
dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointInputs(inputIndex INTEGER, InputValue INTEGER)")if index==0 or index==1 then
t[#t+1]=string.format("InputIndex                     InputType \r\n")elseif index==2 then
t[#t+1]=string.format("InputIndex, InputType \r\n")end
for row in dbFile:rows("SELECT inputIndex, InputValue FROM tblSetpointInputs ORDER BY rowid DESC")do
if index==0 or index==1 then
t[#t+1]=string.format("%30d %10s \r\n",row[1],Input_Type_String[row[2]])elseif index==2 then
t[#t+1]=string.format("%d, %s \r\n",row[1],Input_Type_String[row[2]])else
end
end
dbFile:close()if index==0 then
awtx.serial.send(1,table.concat(t))elseif index==1 then
awtx.serial.send(2,table.concat(t))elseif index==2 then
result=awtx.os.makeDirectory(DB_FileLocation_Reports)if result==0 then
fho,err=io.open(string.format("%s%s",DB_FileLocation_Reports,DB_ReportName_Setpoint),"w")if err then
awtx.display.writeLine("err "..err)awtx.os.sleep(canttime)awtx.os.sleep(canttime)else
fho:write(table.concat(t))fho:close()end
else
awtx.display.writeLine("err "..tostring(result))end
else
end
awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function setpoint.SetpointDBReportIn(label)local usermode,currentRPN,fho,err
local index,isEnterKey
local dbFile
local row
local t={}currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)index,isEnterKey=awtx.keypad.selectList(" Port 1, Port 2, USB",0)awtx.display.writeLine(label)if isEnterKey then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(DB_FileLocation_AppConfig)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointInputs(inputIndex INTEGER, InputValue INTEGER)")if index==0 or index==1 then
t[#t+1]=string.format("InputIndex                     InputType \r\n")elseif index==2 then
t[#t+1]=string.format("InputIndex, InputType \r\n")end
for row in dbFile:rows("SELECT inputIndex, InputValue FROM tblSetpointInputs ORDER BY rowid DESC")do
if index==0 or index==1 then
t[#t+1]=string.format("%30d %10s \r\n",row[1],Input_Type_String[row[2]])elseif index==2 then
t[#t+1]=string.format("%d, %s \r\n",row[1],Input_Type_String[row[2]])else
end
end
dbFile:close()if index==0 then
awtx.serial.send(1,table.concat(t))elseif index==1 then
awtx.serial.send(2,table.concat(t))elseif index==2 then
result=awtx.os.makeDirectory(DB_FileLocation_Reports)if result==0 then
fho,err=io.open(string.format("%s%s",DB_FileLocation_Reports,DB_ReportName_Setpoint),"w")if err then
awtx.display.writeLine("err "..err)awtx.os.sleep(canttime)awtx.os.sleep(canttime)else
fho:write(table.concat(t))fho:close()end
else
awtx.display.writeLine("err "..tostring(result))end
else
end
awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function setpoint.SetpointDBReportOut(label)local usermode,curretnRPN,index,isEnterKey,dbFile,row,fho,err
local t={}currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)index,isEnterKey=awtx.keypad.selectList(" Port 1, Port 2, USB",0)awtx.display.writeLine(label)if isEnterKey then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(DB_FileLocation_AppData)assert(dbFile~=nil)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblSetpointOutputs(outputIndex INTEGER, OutputValue DOUBLE, OutputValueLo DOUBLE, OutputValueHi DOUBLE, units VARCHAR)")if index==0 or index==1 then
t[#t+1]=string.format("OutIndex                       OutputValue                     OutputValueLo                     OutputValueHi \r\n")elseif index==2 then
t[#t+1]=string.format("OutIndex, OutputValue, OutputValueLo, OutputValueHi, Units \r\n")end
for row in dbFile:rows("SELECT outputIndex, OutputValue, OutputValueLo, OutputValueHi, units FROM tblSetpointOutputs ORDER BY rowid DESC")do
if index==0 or index==1 then
t[#t+1]=string.format("%30d %10f %10f %10f %10s \r\n",row[1],row[2],row[3],row[4],row[5])elseif index==2 then
t[#t+1]=string.format("%d, %f, %f, %f, %s \r\n",row[1],row[2],row[3],row[4],row[5])else
end
end
dbFile:close()if index==0 then
awtx.serial.send(1,table.concat(t))elseif index==1 then
awtx.serial.send(2,table.concat(t))elseif index==2 then
result=awtx.os.makeDirectory(DB_FileLocation_Reports)if result==0 then
fho,err=io.open(string.format("%s%s",DB_FileLocation_Reports,DB_ReportName_Setpoint),"w")if err then
awtx.display.writeLine("err "..err)awtx.os.sleep(canttime)awtx.os.sleep(canttime)else
fho:write(table.concat(t))fho:close()end
else
awtx.display.writeLine("err "..tostring(result))end
else
end
awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
awtx.keypad.set_RPN_mode(currentRPN)end
function setpoint.SetpointDBReset(label)local usermode,index,isEnterKey
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)setpoint.SetpointDBClear()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
end
end
function setpoint.editInvertingOutputFlag(label)local newFlag,isEnterKey
setpoint.extraStuffRecall()newFlag=Setpointtable.InvertOutputsFlag
newFlag,isEnterKey=awtx.keypad.selectList("Off, On",newFlag)awtx.display.writeLine(label)if isEnterKey then
Setpointtable.InvertOutputsFlag=newFlag
awtx.setPoint.setInvertingOutputFlag(newFlag)setpoint.extraStuffStore()else
end
end
function get_Mode_Model()if system.modelStr=="ZM301"then
return 0
elseif system.modelStr=="ZM303"then
return 0
elseif system.modelStr=="ZQ375"then
return 0
elseif system.modelStr=="ZM305GTN"then
return 1
elseif system.modelStr=="ZM305"then
return 1
else
return 1
end
end
function get_Out_Mode()if Setpointtable.OutputMode==setpoint.setpt_inside or Setpointtable.OutputMode==setpoint.setpt_outside then
return 1
else
return 0
end
end
function get_Out1_Mode()if Setpointtable.OutputMode1==setpoint.setpt_inside or Setpointtable.OutputMode1==setpoint.setpt_outside then
return 1
else
return 0
end
end
function get_Out2_Mode()if Setpointtable.OutputMode2==setpoint.setpt_inside or Setpointtable.OutputMode2==setpoint.setpt_outside then
return 1
else
return 0
end
end
function get_Out3_Mode()if Setpointtable.OutputMode3==setpoint.setpt_inside or Setpointtable.OutputMode3==setpoint.setpt_outside then
return 1
else
return 0
end
end
function setpoint.editOut1(label)local newOut1,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
if setpoint.basis=="count"then
newOut1,isEnterKey=awtx.keypad.enterInteger(SetpointOutputtable[1].OutputValue,0,100000)if isEnterKey then
setpoint.setOut1Cnt(newOut1)end
else
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
setpoint.SetpointDBRecall()newOut1=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[1].OutputValue,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
newOut1,isEnterKey=awtx.keypad.enterWeightWithUnits(newOut1,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
setpoint.setOut1(newOut1)else
end
end
return isEnterKey
end
function setpoint.setOut1(newOut1)SetpointOutputtable[1].OutputValue=awtx.weight.convertToInternalCalUnit(newOut1,1)setpoint.SetpointDBStore()end
function setpoint.setOut1Cnt(newOut1)SetpointOutputtable[1].OutputValue=newOut1
setpoint.SetpointDBStore()end
function setpoint.editOut1Lo(label)local newOut1,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
if setpoint.basis=="count"then
newOut1,isEnterKey=awtx.keypad.enterInteger(SetpointOutputtable[1].OutputValueLo,0,100000)if isEnterKey then
setpoint.setOut1LoCnt(newOut1)end
else
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
setpoint.SetpointDBRecall()newOut1=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[1].OutputValueLo,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
newOut1,isEnterKey=awtx.keypad.enterWeightWithUnits(newOut1,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
setpoint.setOut1Lo(newOut1)else
end
end
return isEnterKey
end
function setpoint.setOut1Lo(newOut1)SetpointOutputtable[1].OutputValueLo=awtx.weight.convertToInternalCalUnit(newOut1,1)setpoint.SetpointDBStore()end
function setpoint.setOut1LoCnt(newOut1)SetpointOutputtable[1].OutputValueLo=newOut1
setpoint.SetpointDBStore()end
function setpoint.editOut1Hi(label)local newOut1,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
if setpoint.basis=="count"then
newOut1,isEnterKey=awtx.keypad.enterInteger(SetpointOutputtable[1].OutputValueHi,0,100000)if isEnterKey then
setpoint.setOut1HiCnt(newOut1)end
else
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
setpoint.SetpointDBRecall()newOut1=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[1].OutputValueHi,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
newOut1,isEnterKey=awtx.keypad.enterWeightWithUnits(newOut1,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
setpoint.setOut1Hi(newOut1)else
end
end
return isEnterKey
end
function setpoint.setOut1Hi(newOut1)SetpointOutputtable[1].OutputValueHi=awtx.weight.convertToInternalCalUnit(newOut1,1)setpoint.SetpointDBStore()end
function setpoint.setOut1HiCnt(newOut1)SetpointOutputtable[1].OutputValueHi=newOut1
setpoint.SetpointDBStore()end
function setpoint.editOut2(label)local newOut2,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
if setpoint.basis=="count"then
newOut2,isEnterKey=awtx.keypad.enterInteger(SetpointOutputtable[2].OutputValue,0,100000)if isEnterKey then
setpoint.setOut2Cnt(newOut2)end
else
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
setpoint.SetpointDBRecall()newOut2=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[2].OutputValue,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
newOut2,isEnterKey=awtx.keypad.enterWeightWithUnits(newOut2,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
setpoint.setOut2(newOut2)else
end
end
return isEnterKey
end
function setpoint.setOut2(newOut2)SetpointOutputtable[2].OutputValue=awtx.weight.convertToInternalCalUnit(newOut2,1)setpoint.SetpointDBStore()end
function setpoint.setOut2Cnt(newOut2)SetpointOutputtable[2].OutputValue=newOut2
setpoint.SetpointDBStore()end
function setpoint.editOut2Lo(label)local newOut2,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
if setpoint.basis=="count"then
newOut2,isEnterKey=awtx.keypad.enterInteger(SetpointOutputtable[2].OutputValueLo,0,100000)if isEnterKey then
setpoint.setOut2LoCnt(newOut2)end
else
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
setpoint.SetpointDBRecall()newOut2=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[2].OutputValueLo,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
newOut2,isEnterKey=awtx.keypad.enterWeightWithUnits(newOut2,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
setpoint.setOut2Lo(newOut2)else
end
end
return isEnterKey
end
function setpoint.setOut2Lo(newOut2)SetpointOutputtable[2].OutputValueLo=awtx.weight.convertToInternalCalUnit(newOut2,1)setpoint.SetpointDBStore()end
function setpoint.setOut2LoCnt(newOut2)SetpointOutputtable[2].OutputValueLo=newOut2
setpoint.SetpointDBStore()end
function setpoint.editOut2Hi(label)local newOut2,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
if setpoint.basis=="count"then
newOut2,isEnterKey=awtx.keypad.enterInteger(SetpointOutputtable[2].OutputValueHi,0,100000)if isEnterKey then
setpoint.setOut2HiCnt(newOut2)end
else
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
setpoint.SetpointDBRecall()newOut2=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[2].OutputValueHi,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
newOut2,isEnterKey=awtx.keypad.enterWeightWithUnits(newOut2,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
setpoint.setOut2Hi(newOut2)else
end
end
return isEnterKey
end
function setpoint.setOut2Hi(newOut2)SetpointOutputtable[2].OutputValueHi=awtx.weight.convertToInternalCalUnit(newOut2,1)setpoint.SetpointDBStore()end
function setpoint.setOut2HiCnt(newOut2)SetpointOutputtable[2].OutputValueHi=newOut2
setpoint.SetpointDBStore()end
function setpoint.editOut3(label)local newOut3,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
if setpoint.basis=="count"then
newOut3,isEnterKey=awtx.keypad.enterInteger(SetpointOutputtable[3].OutputValue,0,100000)if isEnterKey then
setpoint.setOut3Cnt(newOut3)end
else
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
setpoint.SetpointDBRecall()newOut3=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[3].OutputValue,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
newOut3,isEnterKey=awtx.keypad.enterWeightWithUnits(newOut3,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
setpoint.setOut3(newOut3)else
end
end
return isEnterKey
end
function setpoint.setOut3(newOut3)SetpointOutputtable[3].OutputValue=awtx.weight.convertToInternalCalUnit(newOut3,1)setpoint.SetpointDBStore()end
function setpoint.setOut3Cnt(newOut3)SetpointOutputtable[3].OutputValue=newOut3
setpoint.SetpointDBStore()end
function setpoint.editOut3Lo(label)local newOut3,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
if setpoint.basis=="count"then
newOut3,isEnterKey=awtx.keypad.enterInteger(SetpointOutputtable[3].OutputValueLo,0,100000)if isEnterKey then
setpoint.setOut3LoCnt(newOut3)end
else
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
setpoint.SetpointDBRecall()newOut3=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[3].OutputValueLo,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
newOut3,isEnterKey=awtx.keypad.enterWeightWithUnits(newOut3,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
setpoint.setOut3Lo(newOut3)else
end
end
return isEnterKey
end
function setpoint.setOut3Lo(newOut3)SetpointOutputtable[3].OutputValueLo=awtx.weight.convertToInternalCalUnit(newOut3,1)setpoint.SetpointDBStore()end
function setpoint.setOut3LoCnt(newOut3)SetpointOutputtable[3].OutputValueLo=newOut3
setpoint.SetpointDBStore()end
function setpoint.editOut3Hi(label)local newOut3,Outmin,Outmax,tempUnitIndex,tempUnitStr,isEnterKey
if setpoint.basis=="count"then
newOut3,isEnterKey=awtx.keypad.enterInteger(SetpointOutputtable[3].OutputValueHi,0,100000)if isEnterKey then
setpoint.setOut3HiCnt(newOut3)end
else
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
setpoint.SetpointDBRecall()newOut3=awtx.weight.convertFromInternalCalUnit(SetpointOutputtable[3].OutputValueHi,tempUnitIndex,1)Outmin=-wt.curCapacity
Outmax=wt.curCapacity
newOut3,isEnterKey=awtx.keypad.enterWeightWithUnits(newOut3,Outmin,Outmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
setpoint.setOut3Hi(newOut3)else
end
end
return isEnterKey
end
function setpoint.setOut3Hi(newOut3)SetpointOutputtable[3].OutputValueHi=awtx.weight.convertToInternalCalUnit(newOut3,1)setpoint.SetpointDBStore()end
function setpoint.setOut3HiCnt(newOut3)SetpointOutputtable[3].OutputValueHi=newOut3
setpoint.SetpointDBStore()end
function setpoint.editIn1(label)local newIn1,isEnterKey
setpoint.SetpointDBRecall()newIn1=SetpointInputtable[1].InputValue
newIn1,isEnterKey=awtx.keypad.selectList(Input_Prompt_String,newIn1)awtx.display.writeLine(label)if isEnterKey then
setpoint.setIn1(newIn1)else
end
end
function setpoint.setIn1(newIn1)SetpointInputtable[1].InputValue=newIn1
setpoint.SetpointDBStore()end
function setpoint.editIn2(label)local newIn2,isEnterKey
setpoint.SetpointDBRecall()newIn2=SetpointInputtable[2].InputValue
newIn2,isEnterKey=awtx.keypad.selectList(Input_Prompt_String,newIn2)awtx.display.writeLine(label)if isEnterKey then
setpoint.setIn2(newIn2)else
end
end
function setpoint.setIn2(newIn2)SetpointInputtable[2].InputValue=newIn2
setpoint.SetpointDBStore()end
function setpoint.editIn3(label)local newIn3,isEnterKey
setpoint.SetpointDBRecall()newIn3=SetpointInputtable[3].InputValue
newIn3,isEnterKey=awtx.keypad.selectList(Input_Prompt_String,newIn3)awtx.display.writeLine(label)if isEnterKey then
setpoint.setIn3(newIn3)else
end
end
function setpoint.setIn3(newIn3)SetpointInputtable[3].InputValue=newIn3
setpoint.SetpointDBStore()end
function setpoint.editOutputMode(label)local newMode,isEnterKey
setpoint.extraStuffRecall()if Setpointtable.OutputMode==setpoint.setpt_above_below then
newMode=0
elseif Setpointtable.OutputMode==setpoint.setpt_below_above then
newMode=1
elseif Setpointtable.OutputMode==setpoint.setpt_inside then
newMode=2
elseif Setpointtable.OutputMode==setpoint.setpt_outside then
newMode=3
end
newMode,isEnterKey=awtx.keypad.selectList(Output_Prompt_String,newMode)awtx.display.writeLine(label)if isEnterKey then
if newMode==0 then
Setpointtable.OutputMode=setpoint.setpt_above_below
elseif newMode==1 then
Setpointtable.OutputMode=setpoint.setpt_below_above
elseif newMode==2 then
Setpointtable.OutputMode=setpoint.setpt_inside
elseif newMode==3 then
Setpointtable.OutputMode=setpoint.setpt_outside
end
Setpointtable.OutputMode1=Setpointtable.OutputMode
Setpointtable.OutputMode2=Setpointtable.OutputMode
Setpointtable.OutputMode3=Setpointtable.OutputMode
setpoint.extraStuffStore()else
end
end
function setpoint.editOutputMode1(label)local newMode,isEnterKey
setpoint.extraStuffRecall()if Setpointtable.OutputMode1==setpoint.setpt_above_below then
newMode=0
elseif Setpointtable.OutputMode1==setpoint.setpt_below_above then
newMode=1
elseif Setpointtable.OutputMode1==setpoint.setpt_inside then
newMode=2
elseif Setpointtable.OutputMode1==setpoint.setpt_outside then
newMode=3
end
newMode,isEnterKey=awtx.keypad.selectList(Output_Prompt_String,newMode)awtx.display.writeLine(label)if isEnterKey then
if newMode==0 then
Setpointtable.OutputMode1=setpoint.setpt_above_below
elseif newMode==1 then
Setpointtable.OutputMode1=setpoint.setpt_below_above
elseif newMode==2 then
Setpointtable.OutputMode1=setpoint.setpt_inside
elseif newMode==3 then
Setpointtable.OutputMode1=setpoint.setpt_outside
end
setpoint.extraStuffStore()else
end
end
function setpoint.editOutputMode2(label)local newMode,isEnterKey
setpoint.extraStuffRecall()if Setpointtable.OutputMode2==setpoint.setpt_above_below then
newMode=0
elseif Setpointtable.OutputMode2==setpoint.setpt_below_above then
newMode=1
elseif Setpointtable.OutputMode2==setpoint.setpt_inside then
newMode=2
elseif Setpointtable.OutputMode2==setpoint.setpt_outside then
newMode=3
end
newMode,isEnterKey=awtx.keypad.selectList(Output_Prompt_String,newMode)awtx.display.writeLine(label)if isEnterKey then
if newMode==0 then
Setpointtable.OutputMode2=setpoint.setpt_above_below
elseif newMode==1 then
Setpointtable.OutputMode2=setpoint.setpt_below_above
elseif newMode==2 then
Setpointtable.OutputMode2=setpoint.setpt_inside
elseif newMode==3 then
Setpointtable.OutputMode2=setpoint.setpt_outside
end
setpoint.extraStuffStore()else
end
end
function setpoint.editOutputMode3(label)local newMode,isEnterKey
setpoint.extraStuffRecall()if Setpointtable.OutputMode3==setpoint.setpt_above_below then
newMode=0
elseif Setpointtable.OutputMode3==setpoint.setpt_below_above then
newMode=1
elseif Setpointtable.OutputMode3==setpoint.setpt_inside then
newMode=2
elseif Setpointtable.OutputMode3==setpoint.setpt_outside then
newMode=3
end
newMode,isEnterKey=awtx.keypad.selectList(Output_Prompt_String,newMode)awtx.display.writeLine(label)if isEnterKey then
if newMode==0 then
Setpointtable.OutputMode3=setpoint.setpt_above_below
elseif newMode==1 then
Setpointtable.OutputMode3=setpoint.setpt_below_above
elseif newMode==2 then
Setpointtable.OutputMode3=setpoint.setpt_inside
elseif newMode==3 then
Setpointtable.OutputMode3=setpoint.setpt_outside
end
setpoint.extraStuffStore()else
end
end
setpoint.SetpointInit()setpoint.SetpointDBInit()setpoint.SetpointDBRecall()setpoint.extraStuffRecall()awtx.setPoint.setInvertingOutputFlag(Setpointtable.InvertOutputsFlag)setpoint.setOutputValues()setpoint.setSetpointPrintTokens()function setpoint.setpointOut1Handler(setpointNum,isActivate)if isActivate then
else
end
end
function setpoint.setpointOut2Handler(setpointNum,isActivate)if isActivate then
else
end
end
function setpoint.setpointOut3Handler(setpointNum,isActivate)if isActivate then
else
end
end
function setpoint.setpointOut4Handler(setpointNum,isActivate)if isActivate then
else
end
end
function setpoint.setpointOut5Handler(setpointNum,isActivate)if isActivate then
else
end
end
function setpoint.setpointOut6Handler(setpointNum,isActivate)if isActivate then
else
end
end
function setpoint.setpointOut7Handler(setpointNum,isActivate)if isActivate then
else
end
end
function setpoint.setpointOut8Handler(setpointNum,isActivate)if isActivate then
else
end
end
function setpoint.setpointOut9Handler(setpointNum,isActivate)if isActivate then
else
end
end
function setpoint.setpointOut10Handler(setpointNum,isActivate)if isActivate then
else
end
end
function setpoint.disableSetpointsOutputs()local setPointDisabled,setPointBattery
local retVal,resultMsg,result
setPointDisabled={mode="disabled"}setPointBattery={mode="output",bounceTime=minLatchTime,offInsideGZB=false,act="user",deact="user"}retVal,resultMsg=awtx.setPoint.set(1,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(2,setPointDisabled)if battery.BatteryEnable==1 then
retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)else
retVal,resultMsg=awtx.setPoint.set(3,setPointDisabled)end
result=awtx.setPoint.unregisterOutputEvent(1)result=awtx.setPoint.unregisterOutputEvent(2)result=awtx.setPoint.unregisterOutputEvent(3)result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(3)end
function setpoint.refreshSetpointsOutputs(cfg)local retVal,resultMsg,result
local setpt1AboveBelow,setpt1BelowAbove,setpt1InsideOutside,setpt1OutsideInside
local setpt2AboveBelow,setpt2BelowAbove,setpt2InsideOutside,setpt2OutsideInside
local setpt3AboveBelow,setpt3BelowAbove,setpt3InsideOutside,setpt3OutsideInside
local setPointBattery,setPointOutEx4,setPointOutEx5,setPointOutEx6,setPointOutEx7,setPointOutEx8,setPointOutEx9,setPointOutEx10
local setPointUnder,setPointTarget,setPointOver,setPointReject,setPointAccept
local offInsideGZBFlag=false
setpt1AboveBelow={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="above",actLowerVarName="UserOutput1",actBasis=setpoint.basis,actMotionInhibit=false,deact="below",deactLowerVarName="UserOutput1",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt1BelowAbove={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="below",actLowerVarName="UserOutput1",actBasis=setpoint.basis,actMotionInhibit=false,deact="above",deactLowerVarName="UserOutput1",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt1InsideOutside={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="UserOutput1Lo",actUpperVarName="UserOutput1Hi",actBasis=setpoint.basis,actMotionInhibit=false,deact="outside",deactLowerVarName="UserOutput1Lo",deactUpperVarName="UserOutput1Hi",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt1OutsideInside={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="outside",actLowerVarName="UserOutput1Lo",actUpperVarName="UserOutput1Hi",actBasis=setpoint.basis,actMotionInhibit=false,deact="inside",deactLowerVarName="UserOutput1Lo",deactUpperVarName="UserOutput1Hi",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt2AboveBelow={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="above",actLowerVarName="UserOutput2",actBasis=setpoint.basis,actMotionInhibit=false,deact="below",deactLowerVarName="UserOutput2",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt2BelowAbove={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="below",actLowerVarName="UserOutput2",actBasis=setpoint.basis,actMotionInhibit=false,deact="above",deactLowerVarName="UserOutput2",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt2InsideOutside={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="UserOutput2Lo",actUpperVarName="UserOutput2Hi",actBasis=setpoint.basis,actMotionInhibit=false,deact="outside",deactLowerVarName="UserOutput2Lo",deactUpperVarName="UserOutput2Hi",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt2OutsideInside={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="outside",actLowerVarName="UserOutput2Lo",actUpperVarName="UserOutput2Hi",actBasis=setpoint.basis,actMotionInhibit=false,deact="inside",deactLowerVarName="UserOutput2Lo",deactUpperVarName="UserOutput2Hi",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt3AboveBelow={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="above",actLowerVarName="UserOutput3",actBasis=setpoint.basis,actMotionInhibit=false,deact="below",deactLowerVarName="UserOutput3",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt3BelowAbove={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="below",actLowerVarName="UserOutput3",actBasis=setpoint.basis,actMotionInhibit=false,deact="above",deactLowerVarName="UserOutput3",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt3InsideOutside={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="UserOutput3Lo",actUpperVarName="UserOutput3Hi",actBasis=setpoint.basis,actMotionInhibit=false,deact="outside",deactLowerVarName="UserOutput3Lo",deactUpperVarName="UserOutput3Hi",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt3OutsideInside={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="outside",actLowerVarName="UserOutput3Lo",actUpperVarName="UserOutput3Hi",actBasis=setpoint.basis,actMotionInhibit=false,deact="inside",deactLowerVarName="UserOutput3Lo",deactUpperVarName="UserOutput3Hi",deactBasis=setpoint.basis,deactMotionInhibit=false}setPointBattery={mode="output",bounceTime=minLatchTime,offInsideGZB=false,act="user",deact="user"}setPointUnder={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="setpoint.TargMin",actUpperVarName="setpoint.TargLo",actBasis=setpoint.TargBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="setpoint.TargMin",deactUpperVarName="setpoint.TargLo",deactBasis=setpoint.TargBasis,deactMotionInhibit=false}setPointTarget={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="setpoint.TargLo",actUpperVarName="setpoint.TargHi",actBasis=setpoint.TargBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="setpoint.TargLo",deactUpperVarName="setpoint.TargHi",deactBasis=setpoint.TargBasis,deactMotionInhibit=false}setPointOver={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="setpoint.TargHi",actUpperVarName="setpoint.TargMax",actBasis=setpoint.TargBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="setpoint.TargHi",deactUpperVarName="setpoint.TargMax",deactBasis=setpoint.TargBasis,deactMotionInhibit=false}setPointReject={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="outside",actLowerVarName="setpoint.TargLo",actUpperVarName="setpoint.TargHi",actBasis=setpoint.TargBasis,actMotionInhibit=false,deact="inside",deactLowerVarName="setpoint.TargLo",deactUpperVarName="setpoint.TargHi",deactBasis=setpoint.TargBasis,deactMotionInhibit=false}setPointAccept={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="setpoint.TargLo",actUpperVarName="setpoint.TargHi",actBasis=setpoint.TargBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="setpoint.TargLo",deactUpperVarName="setpoint.TargHi",deactBasis=setpoint.TargBasis,deactMotionInhibit=false}setPointOutEx4={mode="disabled"}setPointOutEx5={mode="disabled"}setPointOutEx6={mode="disabled"}setPointOutEx7={mode="disabled"}setPointOutEx8={mode="disabled"}setPointOutEx9={mode="disabled"}setPointOutEx10={mode="disabled"}if(cfg==setpoint.setpt_above_below)then
if(Setpointtable.OutputMode1==setpoint.setpt_above_below)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1AboveBelow)elseif(Setpointtable.OutputMode1==setpoint.setpt_below_above)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1BelowAbove)elseif(Setpointtable.OutputMode1==setpoint.setpt_inside)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1InsideOutside)elseif(Setpointtable.OutputMode1==setpoint.setpt_outside)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1OutsideInside)else
retVal,resultMsg=awtx.setPoint.set(1,setpt1BelowAbove)end
if(Setpointtable.OutputMode2==setpoint.setpt_above_below)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2AboveBelow)elseif(Setpointtable.OutputMode2==setpoint.setpt_below_above)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2BelowAbove)elseif(Setpointtable.OutputMode2==setpoint.setpt_inside)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2InsideOutside)elseif(Setpointtable.OutputMode2==setpoint.setpt_outside)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2OutsideInside)else
retVal,resultMsg=awtx.setPoint.set(2,setpt2BelowAbove)end
if battery.BatteryEnable==1 then
retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)else
if(Setpointtable.OutputMode3==setpoint.setpt_above_below)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3AboveBelow)elseif(Setpointtable.OutputMode3==setpoint.setpt_below_above)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3BelowAbove)elseif(Setpointtable.OutputMode3==setpoint.setpt_inside)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3InsideOutside)elseif(Setpointtable.OutputMode3==setpoint.setpt_outside)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3OutsideInside)else
retVal,resultMsg=awtx.setPoint.set(3,setpt3BelowAbove)end
end
elseif(cfg==setpoint.setpt_below_above)then
if(Setpointtable.OutputMode1==setpoint.setpt_above_below)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1AboveBelow)elseif(Setpointtable.OutputMode1==setpoint.setpt_below_above)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1BelowAbove)elseif(Setpointtable.OutputMode1==setpoint.setpt_inside)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1InsideOutside)elseif(Setpointtable.OutputMode1==setpoint.setpt_outside)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1OutsideInside)else
retVal,resultMsg=awtx.setPoint.set(1,setpt1AboveBelow)end
if(Setpointtable.OutputMode2==setpoint.setpt_above_below)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2AboveBelow)elseif(Setpointtable.OutputMode2==setpoint.setpt_below_above)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2BelowAbove)elseif(Setpointtable.OutputMode2==setpoint.setpt_inside)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2InsideOutside)elseif(Setpointtable.OutputMode2==setpoint.setpt_outside)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2OutsideInside)else
retVal,resultMsg=awtx.setPoint.set(2,setpt2AboveBelow)end
if battery.BatteryEnable==1 then
retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)else
if(Setpointtable.OutputMode3==setpoint.setpt_above_below)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3AboveBelow)elseif(Setpointtable.OutputMode3==setpoint.setpt_below_above)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3BelowAbove)elseif(Setpointtable.OutputMode3==setpoint.setpt_inside)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3InsideOutside)elseif(Setpointtable.OutputMode3==setpoint.setpt_outside)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3OutsideInside)else
retVal,resultMsg=awtx.setPoint.set(3,setpt3AboveBelow)end
end
elseif(cfg==setpoint.setpt_inside)then
if(Setpointtable.OutputMode1==setpoint.setpt_above_below)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1AboveBelow)elseif(Setpointtable.OutputMode1==setpoint.setpt_below_above)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1BelowAbove)elseif(Setpointtable.OutputMode1==setpoint.setpt_inside)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1InsideOutside)elseif(Setpointtable.OutputMode1==setpoint.setpt_outside)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1OutsideInside)else
retVal,resultMsg=awtx.setPoint.set(1,setpt1InsideOutside)end
if(Setpointtable.OutputMode2==setpoint.setpt_above_below)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2AboveBelow)elseif(Setpointtable.OutputMode2==setpoint.setpt_below_above)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2BelowAbove)elseif(Setpointtable.OutputMode2==setpoint.setpt_inside)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2InsideOutside)elseif(Setpointtable.OutputMode2==setpoint.setpt_outside)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2OutsideInside)else
retVal,resultMsg=awtx.setPoint.set(2,setpt2InsideOutside)end
if battery.BatteryEnable==1 then
retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)else
if(Setpointtable.OutputMode3==setpoint.setpt_above_below)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3AboveBelow)elseif(Setpointtable.OutputMode3==setpoint.setpt_below_above)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3BelowAbove)elseif(Setpointtable.OutputMode3==setpoint.setpt_inside)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3InsideOutside)elseif(Setpointtable.OutputMode3==setpoint.setpt_outside)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3OutsideInside)else
retVal,resultMsg=awtx.setPoint.set(3,setpt_inside)end
end
elseif(cfg==setpoint.setpt_outside)then
if(Setpointtable.OutputMode1==setpoint.setpt_above_below)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1AboveBelow)elseif(Setpointtable.OutputMode1==setpoint.setpt_below_above)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1BelowAbove)elseif(Setpointtable.OutputMode1==setpoint.setpt_inside)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1InsideOutside)elseif(Setpointtable.OutputMode1==setpoint.setpt_outside)then
retVal,resultMsg=awtx.setPoint.set(1,setpt1OutsideInside)else
retVal,resultMsg=awtx.setPoint.set(1,setpt1OutsideInside)end
if(Setpointtable.OutputMode2==setpoint.setpt_above_below)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2AboveBelow)elseif(Setpointtable.OutputMode2==setpoint.setpt_below_above)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2BelowAbove)elseif(Setpointtable.OutputMode2==setpoint.setpt_inside)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2InsideOutside)elseif(Setpointtable.OutputMode2==setpoint.setpt_outside)then
retVal,resultMsg=awtx.setPoint.set(2,setpt2OutsideInside)else
retVal,resultMsg=awtx.setPoint.set(2,setpt2OutsideInside)end
if battery.BatteryEnable==1 then
retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)else
if(Setpointtable.OutputMode3==setpoint.setpt_above_below)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3AboveBelow)elseif(Setpointtable.OutputMode3==setpoint.setpt_below_above)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3BelowAbove)elseif(Setpointtable.OutputMode3==setpoint.setpt_inside)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3InsideOutside)elseif(Setpointtable.OutputMode3==setpoint.setpt_outside)then
retVal,resultMsg=awtx.setPoint.set(3,setpt3OutsideInside)else
retVal,resultMsg=awtx.setPoint.set(3,setpt3OutsideInside)end
end
elseif(cfg==setpoint.setpt_checkweigh)then
if battery.BatteryEnable==1 then
retVal,resultMsg=awtx.setPoint.set(1,setPointReject)retVal,resultMsg=awtx.setPoint.set(2,setPointAccept)retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)else
retVal,resultMsg=awtx.setPoint.set(1,setPointUnder)retVal,resultMsg=awtx.setPoint.set(2,setPointTarget)retVal,resultMsg=awtx.setPoint.set(3,setPointOver)end
end
retVal,resultMsg=awtx.setPoint.set(4,setPointOutEx4)retVal,resultMsg=awtx.setPoint.set(5,setPointOutEx5)retVal,resultMsg=awtx.setPoint.set(6,setPointOutEx6)retVal,resultMsg=awtx.setPoint.set(7,setPointOutEx7)retVal,resultMsg=awtx.setPoint.set(8,setPointOutEx8)retVal,resultMsg=awtx.setPoint.set(9,setPointOutEx9)retVal,resultMsg=awtx.setPoint.set(10,setPointOutEx10)result=awtx.setPoint.registerOutputEvent(1,setpoint.setpointOut1Handler)result=awtx.setPoint.registerOutputEvent(2,setpoint.setpointOut2Handler)result=awtx.setPoint.registerOutputEvent(3,setpoint.setpointOut3Handler)result=awtx.setPoint.registerOutputEvent(4,setpoint.setpointOut4Handler)result=awtx.setPoint.registerOutputEvent(5,setpoint.setpointOut5Handler)result=awtx.setPoint.registerOutputEvent(6,setpoint.setpointOut6Handler)result=awtx.setPoint.registerOutputEvent(7,setpoint.setpointOut7Handler)result=awtx.setPoint.registerOutputEvent(8,setpoint.setpointOut8Handler)result=awtx.setPoint.registerOutputEvent(9,setpoint.setpointOut9Handler)result=awtx.setPoint.registerOutputEvent(10,setpoint.setpointOut10Handler)setpoint.setOutputValues()setpoint.setSetpointPrintTokens()end
function setpoint.setpointInputAction(cfgInputType)if Input_Type_String[cfgInputType]=="None     "then
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
function setpoint.setpointIn1Handler(setpointNum,inputState)if inputState then
setpoint.setpointInputAction(SetpointInputtable[1].InputValue)else
end
end
function setpoint.setpointIn2Handler(setpointNum,inputState)if inputState then
setpoint.setpointInputAction(SetpointInputtable[2].InputValue)else
end
end
function setpoint.setpointIn3Handler(setpointNum,inputState)if inputState then
setpoint.setpointInputAction(SetpointInputtable[3].InputValue)else
end
end
function setpoint.setInput1Value(newIn1)setpoint.Input1Value=newIn1
setpoint.setpointIn1Handler(1,setpoint.Input1Value)setpoint.SetpointDBStore()end
function setpoint.setInput2Value(newIn2)setpoint.Input2Value=newIn2
setpoint.setpointIn2Handler(2,setpoint.Input2Value)setpoint.SetpointDBStore()end
function setpoint.setInput3Value(newIn3)setpoint.Input3Value=newIn3
setpoint.setpointIn3Handler(3,setpoint.Input3Value)setpoint.SetpointDBStore()end
function setpoint.refreshSetpointsInputs()local retVal,resultMsg,result
local setPointInEx1,setPointInEx2,setPointInEx3
setPointInEx1={mode="input",bounceTime=1}setPointInEx2={mode="input",bounceTime=1}setPointInEx3={mode="input",bounceTime=1}retVal,resultMsg=awtx.setPoint.set(1,setPointInEx1)retVal,resultMsg=awtx.setPoint.set(2,setPointInEx2)retVal,resultMsg=awtx.setPoint.set(3,setPointInEx3)result=awtx.setPoint.registerInputEvent(1,setpoint.setpointIn1Handler)result=awtx.setPoint.registerInputEvent(2,setpoint.setpointIn2Handler)result=awtx.setPoint.registerInputEvent(3,setpoint.setpointIn3Handler)setpoint.setOutputValues()setpoint.setSetpointPrintTokens()end