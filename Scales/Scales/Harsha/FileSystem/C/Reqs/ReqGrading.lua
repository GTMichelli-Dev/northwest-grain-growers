grading={}Grad0StrDefault="UNDER  "Grad1StrDefault="GRAD 1 "Grad2StrDefault="GRAD 2 "Grad3StrDefault="GRAD 3 "Grad4StrDefault="GRAD 4 "Grad5StrDefault="GRAD 5 "Grad6StrDefault="GRAD 6 "Grad7StrDefault="GRAD 7 "Grad8StrDefault="GRAD 8 "Grad9StrDefault="GRAD 9 "Grad10StrDefault="GRAD 10"Grad11StrDefault="OVER   "Grad12StrDefault="UNKNOWN"Grading_Names={[0]=Grad0StrDefault,Grad1StrDefault,Grad2StrDefault,Grad3StrDefault,Grad4StrDefault,Grad5StrDefault,Grad6StrDefault,Grad7StrDefault,Grad8StrDefault,Grad9StrDefault,Grad10StrDefault,Grad11StrDefault,Grad12StrDefault}Grad1Default=0.8
Grad2Default=1.2
Grad3Default=0
Grad4Default=0
Grad5Default=0
Grad6Default=0
Grad7Default=0
Grad8Default=0
Grad9Default=0
Grad10Default=0
Grad11Default=0
GradMinDefault=Grad1Default
GradMaxDefault=Grad2Default
GradStrDefault=Grading_Names[12]grading.Grad1=Grad1Default
grading.Grad2=Grad2Default
grading.Grad3=Grad3Default
grading.Grad4=Grad4Default
grading.Grad5=Grad5Default
grading.Grad6=Grad6Default
grading.Grad7=Grad7Default
grading.Grad8=Grad8Default
grading.Grad9=Grad9Default
grading.Grad10=Grad10Default
grading.Grad11=Grad11Default
grading.GradMin=GradMinDefault
grading.GradMax=GradMaxDefault
grading.GradStr=GradStrDefault
grading.temp=0.0
Grad1NegDefault=-0.8
Grad2NegDefault=-1.2
Grad3NegDefault=0
Grad4NegDefault=0
Grad5NegDefault=0
Grad6NegDefault=0
Grad7NegDefault=0
Grad8NegDefault=0
Grad9NegDefault=0
Grad10NegDefault=0
Grad11NegDefault=0
GradMinNegDefault=Grad1NegDefault
GradMaxNegDefault=Grad2NegDefault
grading.Grad1Neg=Grad1NegDefault
grading.Grad2Neg=Grad2NegDefault
grading.Grad3Neg=Grad3NegDefault
grading.Grad4Neg=Grad4NegDefault
grading.Grad5Neg=Grad5NegDefault
grading.Grad6Neg=Grad6NegDefault
grading.Grad7Neg=Grad7NegDefault
grading.Grad8Neg=Grad8NegDefault
grading.Grad9Neg=Grad9NegDefault
grading.Grad10Neg=Grad10NegDefault
grading.Grad11Neg=Grad11NegDefault
grading.GradMinNeg=GradMinNegDefault
grading.GradMaxNeg=GradMaxNegDefault
grading.UnitsOfMeasure=wt.units
POS_GRADING=0
NEG_GRADING=1
grading.GradType=POS_GRADING
TARE_MANUAL=0
TARE_AUTO=1
grading.TareType=TARE_AUTO
PRINT_MANUAL=0
PRINT_AUTO=1
grading.PrintType=PRINT_AUTO
function grading.initGradPrintTokens(temp)awtx.fmtPrint.varSet(29,grading.temp,grading.GradStr,AWTX_LUA_FLOAT)printTokens[29].varName="grading.temp"printTokens[29].varLabel=grading.GradStr
printTokens[29].varType=AWTX_LUA_FLOAT
printTokens[29].varValue=grading.temp
printTokens[29].varFunct=""awtx.fmtPrint.varSet(30,grading.Grad1,"Grad1",AWTX_LUA_FLOAT)printTokens[30].varName="grading.Grad1"printTokens[30].varLabel="Grad1"printTokens[30].varType=AWTX_LUA_FLOAT
printTokens[30].varValue=grading.Grad1
printTokens[30].varFunct=grading.setGrad1
awtx.fmtPrint.varSet(31,grading.Grad2,"Grad2",AWTX_LUA_FLOAT)printTokens[30].varName="grading.Grad2"printTokens[31].varLabel="Grad2"printTokens[31].varType=AWTX_LUA_FLOAT
printTokens[31].varValue=grading.Grad2
printTokens[31].varFunct=grading.setGrad2
awtx.fmtPrint.varSet(32,grading.Grad3,"Grad3",AWTX_LUA_FLOAT)printTokens[32].varName="grading.Grad3"printTokens[32].varLabel="Grad3"printTokens[32].varType=AWTX_LUA_FLOAT
printTokens[32].varValue=grading.Grad3
printTokens[32].varFunct=grading.setGrad3
awtx.fmtPrint.varSet(33,grading.Grad4,"Grad4",AWTX_LUA_FLOAT)printTokens[33].varName="grading.Grad4"printTokens[33].varLabel="Grad4"printTokens[33].varType=AWTX_LUA_FLOAT
printTokens[33].varValue=grading.Grad4
printTokens[33].varFunct=grading.setGrad4
awtx.fmtPrint.varSet(34,grading.Grad5,"Grad5",AWTX_LUA_FLOAT)printTokens[34].varName="grading.Grad5"printTokens[34].varLabel="Grad5"printTokens[34].varType=AWTX_LUA_FLOAT
printTokens[34].varValue=grading.Grad5
printTokens[34].varFunct=grading.setGrad5
awtx.fmtPrint.varSet(35,grading.Grad6,"Grad6",AWTX_LUA_FLOAT)printTokens[35].varName="grading.Grad6"printTokens[35].varLabel="Grad6"printTokens[35].varType=AWTX_LUA_FLOAT
printTokens[35].varValue=grading.Grad6
printTokens[35].varFunct=grading.setGrad6
awtx.fmtPrint.varSet(36,grading.Grad7,"Grad7",AWTX_LUA_FLOAT)printTokens[36].varName="grading.Grad7"printTokens[36].varLabel="Grad7"printTokens[36].varType=AWTX_LUA_FLOAT
printTokens[36].varValue=grading.Grad7
printTokens[36].varFunct=grading.setGrad7
awtx.fmtPrint.varSet(37,grading.Grad8,"Grad8",AWTX_LUA_FLOAT)printTokens[37].varName="grading.Grad8"printTokens[37].varLabel="Grad8"printTokens[37].varType=AWTX_LUA_FLOAT
printTokens[37].varValue=grading.Grad8
printTokens[37].varFunct=grading.setGrad8
awtx.fmtPrint.varSet(38,grading.Grad9,"Grad9",AWTX_LUA_FLOAT)printTokens[38].varName="grading.Grad9"printTokens[38].varLabel="Grad9"printTokens[38].varType=AWTX_LUA_FLOAT
printTokens[38].varValue=grading.Grad9
printTokens[38].varFunct=grading.setGrad9
awtx.fmtPrint.varSet(39,grading.Grad10,"Grad10",AWTX_LUA_FLOAT)printTokens[39].varName="grading.Grad10"printTokens[39].varLabel="Grad10"printTokens[39].varType=AWTX_LUA_FLOAT
printTokens[39].varValue=grading.Grad10
printTokens[39].varFunct=grading.setGrad10
awtx.fmtPrint.varSet(40,grading.Grad11,"Grad11",AWTX_LUA_FLOAT)printTokens[40].varName="grading.Grad11"printTokens[40].varLabel="Grad11"printTokens[40].varType=AWTX_LUA_FLOAT
printTokens[40].varValue=grading.Grad11
printTokens[40].varFunct=grading.setGrad11
end
function grading.setGradPrintTokens(temp)awtx.fmtPrint.varSet(29,grading.temp,grading.GradStr,AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(30,grading.Grad1,"Grad1",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(31,grading.Grad2,"Grad2",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(32,grading.Grad3,"Grad3",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(33,grading.Grad4,"Grad4",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(34,grading.Grad5,"Grad5",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(35,grading.Grad6,"Grad6",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(36,grading.Grad7,"Grad7",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(37,grading.Grad8,"Grad8",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(38,grading.Grad9,"Grad9",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(39,grading.Grad10,"Grad10",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(40,grading.Grad11,"Grad11",AWTX_LUA_FLOAT)end
local DB_FileLocation_AppConfig
local DB_FileLocation_AppData
local DB_FileLocation_Reports
local DB_ReportName_Grading
function grading.updateWeightBasedSettingsAfterUnitsChange()local newUnits,oldUnits
newUnits=wt.units
oldUnits=grading.UnitsOfMeasure
grading.Grad1=awtx.weight.convertWeight(oldUnits,grading.Grad1,newUnits,1)grading.Grad1Neg=grading.Grad1*-1
grading.Grad2=awtx.weight.convertWeight(oldUnits,grading.Grad2,newUnits,1)grading.Grad2Neg=grading.Grad2*-1
grading.Grad3=awtx.weight.convertWeight(oldUnits,grading.Grad3,newUnits,1)grading.Grad3Neg=grading.Grad3*-1
grading.Grad4=awtx.weight.convertWeight(oldUnits,grading.Grad4,newUnits,1)grading.Grad4Neg=grading.Grad4*-1
grading.Grad5=awtx.weight.convertWeight(oldUnits,grading.Grad5,newUnits,1)grading.Grad5Neg=grading.Grad5*-1
grading.Grad6=awtx.weight.convertWeight(oldUnits,grading.Grad6,newUnits,1)grading.Grad6Neg=grading.Grad6*-1
grading.Grad7=awtx.weight.convertWeight(oldUnits,grading.Grad7,newUnits,1)grading.Grad7Neg=grading.Grad7*-1
grading.Grad8=awtx.weight.convertWeight(oldUnits,grading.Grad8,newUnits,1)grading.Grad8Neg=grading.Grad8*-1
grading.Grad9=awtx.weight.convertWeight(oldUnits,grading.Grad9,newUnits,1)grading.Grad9Neg=grading.Grad9*-1
grading.Grad10=awtx.weight.convertWeight(oldUnits,grading.Grad10,newUnits,1)grading.Grad10Neg=grading.Grad10*-1
grading.Grad11=awtx.weight.convertWeight(oldUnits,grading.Grad11,newUnits,1)grading.Grad11Neg=grading.Grad11*-1
grading.temp=0.0
grading.setGradPrintTokens(grading.temp)grading.refreshSetpointGrading()grading.UnitsOfMeasure=wt.units
end
function grading.gradDBInit()local simAppPath,dbFile,result,sqlStr
if awtx.os.amISimulator()then
simAppPath=awtx.os.applicationPath()DB_FileLocation_AppConfig=simAppPath..[[\AppConfig.db]]DB_FileLocation_AppData=simAppPath..[[\AppData.db]]DB_FileLocation_Reports=simAppPath..[[\Reports]]..SerialNumber
else
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]DB_FileLocation_AppData=[[C:\Database\AppData.db]]DB_FileLocation_Reports=[[G:\Reports]]..SerialNumber
end
DB_ReportName_Grading=[[\GradingReport.csv]]dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGradingData (GradIndex INTEGER, Grad1  DOUBLE, Grad2  DOUBLE, Grad3  DOUBLE, Grad4  DOUBLE, Grad5  DOUBLE, Grad6  DOUBLE, Grad7  DOUBLE,Grad8  DOUBLE, Grad9  DOUBLE, Grad10 DOUBLE, Grad11 DOUBLE)")dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGradingConfig (varID TEXT PRIMARY KEY, value TEXT)")result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGradingLabels (GradIndex INTEGER, Under  TEXT, Grad1  TEXT, Grad2  TEXT, Grad3  TEXT, Grad4  TEXT, Grad5  TEXT, Grad6  TEXT, Grad7  TEXT,Grad8  TEXT, Grad9  TEXT, Grad10 TEXT, Over TEXT)")dbFile:close()end
function grading.gradDBStore()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGradingData (GradIndex INTEGER, Grad1  DOUBLE, Grad2  DOUBLE, Grad3  DOUBLE, Grad4  DOUBLE, Grad5  DOUBLE, Grad6  DOUBLE, Grad7  DOUBLE,Grad8  DOUBLE, Grad9  DOUBLE, Grad10 DOUBLE, Grad11 DOUBLE)")dbFile:execute("BEGIN TRANSACTION")sqlStr=string.format("SELECT GradIndex, Grad1, Grad2, Grad3, Grad4, Grad5, Grad6, Grad7, Grad8, Grad9, Grad10, Grad11 FROM tblGradingData WHERE GradIndex = '1'")found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGradingData (GradIndex, Grad1, Grad2, Grad3, Grad4, Grad5, Grad6, Grad7, Grad8, Grad9, Grad10, Grad11) VALUES ('1', '%f', '%f', '%f', '%f', '%f', '%f', '%f', '%f', '%f', '%f', '%f')",grading.Grad1,grading.Grad2,grading.Grad3,grading.Grad4,grading.Grad5,grading.Grad6,grading.Grad7,grading.Grad8,grading.Grad9,grading.Grad10,grading.Grad11)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGradingData SET GradIndex = '1', Grad1 = '%f', Grad2 = '%f', Grad3 = '%f', Grad4 = '%f', Grad5 = '%f', Grad6 = '%f', Grad7 = '%f', Grad8 = '%f', Grad9 = '%f', Grad10 = '%f', Grad11 = '%f'",grading.Grad1,grading.Grad2,grading.Grad3,grading.Grad4,grading.Grad5,grading.Grad6,grading.Grad7,grading.Grad8,grading.Grad9,grading.Grad10,grading.Grad11)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGradingConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")found=false
for row in dbFile:rows("SELECT varID FROM tblGradingConfig WHERE varID = 'GradType'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGradingConfig (varID, value) VALUES ('GradType', '%d')",grading.GradType)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGradingConfig SET value = '%d' WHERE varID = 'GradType'",grading.GradType)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGradingConfig WHERE varID = 'TareType'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGradingConfig (varID, value) VALUES ('TareType', '%d')",grading.TareType)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGradingConfig SET value = '%d' WHERE varID = 'TareType'",grading.TareType)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGradingConfig WHERE varID = 'PrintType'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGradingConfig (varID, value) VALUES ('PrintType', '%d')",grading.PrintType)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGradingConfig SET value = '%d' WHERE varID = 'PrintType'",grading.PrintType)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()grading.temp=0.0
grading.setGradPrintTokens(grading.temp)end
function grading.gradDBRecall()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGradingData (GradIndex INTEGER, Grad1  DOUBLE, Grad2  DOUBLE, Grad3  DOUBLE, Grad4  DOUBLE, Grad5  DOUBLE, Grad6  DOUBLE, Grad7  DOUBLE,Grad8  DOUBLE, Grad9  DOUBLE, Grad10 DOUBLE, Grad11 DOUBLE)")sqlStr=string.format("SELECT GradIndex, Grad1, Grad2, Grad3, Grad4, Grad5, Grad6, Grad7, Grad8, Grad9, Grad10, Grad11 FROM tblGradingData WHERE GradIndex = '1'")found=false
for row in dbFile:rows(sqlStr)do
found=true
grading.Grad1=row[2]grading.Grad2=row[3]grading.Grad3=row[4]grading.Grad4=row[5]grading.Grad5=row[6]grading.Grad6=row[7]grading.Grad7=row[8]grading.Grad8=row[9]grading.Grad9=row[10]grading.Grad10=row[11]grading.Grad11=row[12]grading.GradStr=GradStrDefault
end
if found==false then
grading.Grad1=Grad1Default
grading.Grad2=Grad2Default
grading.Grad3=Grad3Default
grading.Grad4=Grad4Default
grading.Grad5=Grad5Default
grading.Grad6=Grad6Default
grading.Grad7=Grad7Default
grading.Grad8=Grad8Default
grading.Grad9=Grad9Default
grading.Grad10=Grad10Default
grading.Grad11=Grad11Default
grading.GradMin=GradMinDefault
grading.GradMax=GradMaxDefault
grading.GradStr=GradStrDefault
sqlStr=string.format("INSERT INTO tblGradingData (GradIndex, Grad1, Grad2, Grad3, Grad4, Grad5, Grad6, Grad7, Grad8, Grad9, Grad10, Grad11) VALUES ('1', '%f', '%f', '%f', '%f', '%f', '%f', '%f', '%f', '%f', '%f', '%f')",grading.Grad1,grading.Grad2,grading.Grad3,grading.Grad4,grading.Grad5,grading.Grad6,grading.Grad7,grading.Grad8,grading.Grad9,grading.Grad10,grading.Grad11)result=dbFile:exec(sqlStr)dbFile:execute("COMMIT")end
dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGradingConfig (varID TEXT PRIMARY KEY, value TEXT)")found=false
for row in dbFile:rows("SELECT varID, value FROM tblGradingConfig WHERE varID = 'GradType'")do
found=true
grading.GradType=tonumber(row[2])end
if found==false then
grading.GradType=POS_GRADING
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGradingConfig WHERE varID = 'TareType'")do
found=true
grading.TareType=tonumber(row[2])end
if found==false then
grading.TareType=TARE_AUTO
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGradingConfig WHERE varID = 'PrintType'")do
found=true
grading.PrintType=tonumber(row[2])end
if found==false then
grading.PrintType=PRINT_AUTO
end
dbFile:close()grading.Grad1Neg=grading.Grad1*-1
grading.Grad2Neg=grading.Grad2*-1
grading.Grad3Neg=grading.Grad3*-1
grading.Grad4Neg=grading.Grad4*-1
grading.Grad5Neg=grading.Grad5*-1
grading.Grad6Neg=grading.Grad6*-1
grading.Grad7Neg=grading.Grad7*-1
grading.Grad8Neg=grading.Grad8*-1
grading.Grad9Neg=grading.Grad9*-1
grading.Grad10Neg=grading.Grad10*-1
grading.Grad11Neg=grading.Grad11*-1
grading.temp=0.0
grading.setGradPrintTokens(grading.temp)end
function grading.editGrad1(label)local newGrad1,isEnterKey,newGradmin,newGradmax,tempUnitStr
tempUnitStr=wt.unitsStr
newGrad1=awtx.setPoint.varGet("Grad1")newGrad1=grading.Grad1
newGradmin=wt.curDivision
newGradmax=wt.curCapacity
newGrad1,isEnterKey=awtx.keypad.enterWeightWithUnits(newGrad1,newGradmin,newGradmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
grading.setGrad1(newGrad1)else
end
return isEnterKey
end
function grading.setGrad1(newGrad1)grading.Grad1=newGrad1
grading.Grad1Neg=grading.Grad1*-1
awtx.setPoint.varSet("Grad1",grading.Grad1)awtx.setPoint.varSet("Grad1Neg",grading.Grad1Neg)grading.gradDBStore()grading.refreshSetpointGrading()end
function grading.editGrad2(label)local newGrad2,isEnterKey,newGradmin,newGradmax,tempUnitStr
tempUnitStr=wt.unitsStr
newGrad2=awtx.setPoint.varGet("Grad2")newGrad2=grading.Grad2
newGradmin=wt.curDivision
newGradmax=wt.curCapacity
newGrad2,isEnterKey=awtx.keypad.enterWeightWithUnits(newGrad2,newGradmin,newGradmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
grading.setGrad2(newGrad2)else
end
return isEnterKey
end
function grading.setGrad2(newGrad2)grading.Grad2=newGrad2
grading.Grad2Neg=grading.Grad2*-1
awtx.setPoint.varSet("Grad2",grading.Grad2)awtx.setPoint.varSet("Grad2Neg",grading.Grad2Neg)grading.gradDBStore()grading.refreshSetpointGrading()end
function grading.editGrad3(label)local newGrad3,isEnterKey,newGradmin,newGradmax,tempUnitStr
tempUnitStr=wt.unitsStr
newGrad3=awtx.setPoint.varGet("Grad3")newGrad3=grading.Grad3
newGradmin=0
newGradmax=wt.curCapacity
newGrad3,isEnterKey=awtx.keypad.enterWeightWithUnits(newGrad3,newGradmin,newGradmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
grading.setGrad3(newGrad3)else
end
return isEnterKey
end
function grading.setGrad3(newGrad3)grading.Grad3=newGrad3
grading.Grad3Neg=grading.Grad3*-1
awtx.setPoint.varSet("Grad3",grading.Grad3)awtx.setPoint.varSet("Grad3Neg",grading.Grad3Neg)grading.gradDBStore()grading.refreshSetpointGrading()end
function grading.editGrad4(label)local newGrad4,isEnterKey,newGradmin,newGradmax,tempUnitStr
tempUnitStr=wt.unitsStr
newGrad4=awtx.setPoint.varGet("Grad4")newGrad4=grading.Grad4
newGradmin=0
newGradmax=wt.curCapacity
newGrad4,isEnterKey=awtx.keypad.enterWeightWithUnits(newGrad4,newGradmin,newGradmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
grading.setGrad4(newGrad4)else
end
return isEnterKey
end
function grading.setGrad4(newGrad4)grading.Grad4=newGrad4
grading.Grad4Neg=grading.Grad4*-1
awtx.setPoint.varSet("Grad4",grading.Grad4)awtx.setPoint.varSet("Grad4Neg",grading.Grad4Neg)grading.gradDBStore()grading.refreshSetpointGrading()end
function grading.editGrad5(label)local newGrad5,isEnterKey,newGradmin,newGradmax,tempUnitStr
tempUnitStr=wt.unitsStr
newGrad5=awtx.setPoint.varGet("Grad5")newGrad5=grading.Grad5
newGradmin=0
newGradmax=wt.curCapacity
newGrad5,isEnterKey=awtx.keypad.enterWeightWithUnits(newGrad5,newGradmin,newGradmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
grading.setGrad5(newGrad5)else
end
return isEnterKey
end
function grading.setGrad5(newGrad5)grading.Grad5=newGrad5
grading.Grad5Neg=grading.Grad5*-1
awtx.setPoint.varSet("Grad5",grading.Grad5)awtx.setPoint.varSet("Grad5Neg",grading.Grad5Neg)grading.gradDBStore()grading.refreshSetpointGrading()end
function grading.editGrad6(label)local newGrad6,isEnterKey,newGradmin,newGradmax,tempUnitStr
tempUnitStr=wt.unitsStr
newGrad6=awtx.setPoint.varGet("Grad6")newGrad6=grading.Grad6
newGradmin=0
newGradmax=wt.curCapacity
newGrad6,isEnterKey=awtx.keypad.enterWeightWithUnits(newGrad6,newGradmin,newGradmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
grading.setGrad6(newGrad6)else
end
return isEnterKey
end
function grading.setGrad6(newGrad6)grading.Grad6=newGrad6
grading.Grad6Neg=grading.Grad6*-1
awtx.setPoint.varSet("Grad6",grading.Grad6)awtx.setPoint.varSet("Grad6Neg",grading.Grad6Neg)grading.gradDBStore()grading.refreshSetpointGrading()end
function grading.editGrad7(label)local newGrad7,isEnterKey,newGradmin,newGradmax,tempUnitStr
tempUnitStr=wt.unitsStr
newGrad7=awtx.setPoint.varGet("Grad7")newGrad7=grading.Grad7
newGradmin=0
newGradmax=wt.curCapacity
newGrad7,isEnterKey=awtx.keypad.enterWeightWithUnits(newGrad7,newGradmin,newGradmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
grading.setGrad7(newGrad7)else
end
return isEnterKey
end
function grading.setGrad7(newGrad7)grading.Grad7=newGrad7
grading.Grad7Neg=grading.Grad7*-1
awtx.setPoint.varSet("Grad7",grading.Grad7)awtx.setPoint.varSet("Grad7Neg",grading.Grad7Neg)grading.gradDBStore()grading.refreshSetpointGrading()end
function grading.editGrad8(label)local newGrad8,isEnterKey,newGradmin,newGradmax,tempUnitStr
tempUnitStr=wt.unitsStr
newGrad8=awtx.setPoint.varGet("Grad8")newGrad8=grading.Grad8
newGradmin=0
newGradmax=wt.curCapacity
newGrad8,isEnterKey=awtx.keypad.enterWeightWithUnits(newGrad8,newGradmin,newGradmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
grading.setGrad8(newGrad8)else
end
return isEnterKey
end
function grading.setGrad8(newGrad8)grading.Grad8=newGrad8
grading.Grad8Neg=grading.Grad8*-1
awtx.setPoint.varSet("Grad8",grading.Grad8)awtx.setPoint.varSet("Grad8Neg",grading.Grad8Neg)grading.gradDBStore()grading.refreshSetpointGrading()end
function grading.editGrad9(label)local newGrad9,isEnterKey,newGradmin,newGradmax,tempUnitStr
tempUnitStr=wt.unitsStr
newGrad9=awtx.setPoint.varGet("Grad9")newGrad9=grading.Grad9
newGradmin=0
newGradmax=wt.curCapacity
newGrad9,isEnterKey=awtx.keypad.enterWeightWithUnits(newGrad9,newGradmin,newGradmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
grading.setGrad9(newGrad9)else
end
return isEnterKey
end
function grading.setGrad9(newGrad9)grading.Grad9=newGrad9
grading.Grad9Neg=grading.Grad9*-1
awtx.setPoint.varSet("Grad9",grading.Grad9)awtx.setPoint.varSet("Grad9Neg",grading.Grad9Neg)grading.gradDBStore()grading.refreshSetpointGrading()end
function grading.editGrad10(label)local newGrad10,isEnterKey,newGradmin,newGradmax,tempUnitStr
tempUnitStr=wt.unitsStr
newGrad10=awtx.setPoint.varGet("Grad10")newGrad10=grading.Grad10
newGradmin=0
newGradmax=wt.curCapacity
newGrad10,isEnterKey=awtx.keypad.enterWeightWithUnits(newGrad10,newGradmin,newGradmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
grading.setGrad10(newGrad10)else
end
return isEnterKey
end
function grading.setGrad10(newGrad10)grading.Grad10=newGrad10
grading.Grad10Neg=grading.Grad10*-1
awtx.setPoint.varSet("Grad10",grading.Grad10)awtx.setPoint.varSet("Grad10Neg",grading.Grad10Neg)grading.gradDBStore()grading.refreshSetpointGrading()end
function grading.editGrad11(label)local newGrad11,isEnterKey,newGradmin,newGradmax,tempUnitStr
tempUnitStr=wt.unitsStr
newGrad11=awtx.setPoint.varGet("Grad11")newGrad11=grading.Grad11
newGradmin=0
newGradmax=wt.curCapacity
newGrad11,isEnterKey=awtx.keypad.enterWeightWithUnits(newGrad11,newGradmin,newGradmax,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
grading.setGrad11(newGrad11)else
end
return isEnterKey
end
function grading.setGrad11(newGrad11)grading.Grad11=newGrad11
grading.Grad11Neg=grading.Grad11*-1
awtx.setPoint.varSet("Grad11",grading.Grad11)awtx.setPoint.varSet("Grad11Neg",grading.Grad11Neg)grading.gradDBStore()grading.refreshSetpointGrading()end
function grading.editGradType(label)local newGradType,isEnterKey
newGradType=grading.GradType
newGradType,isEnterKey=awtx.keypad.selectList(" POS, NEG",newGradType)awtx.display.writeLine(label)if isEnterKey then
grading.setGradType(newGradType)else
end
return isEnterKey
end
function grading.setGradType(newGradType)grading.GradType=newGradType
grading.gradDBStore()grading.refreshSetpointGrading()end
function get_NegGradingEnabled()if grading.GradType==NEG_GRADING then
return 1
else
return 0
end
end
function grading.editTareType(label)local newTareType,isEnterKey
newTareType=grading.TareType
newTareType,isEnterKey=awtx.keypad.selectList("MANUAL, AUTO",newTareType)awtx.display.writeLine(label)if isEnterKey then
grading.setTareType(newTareType)else
end
return isEnterKey
end
function grading.setTareType(newTareType)grading.TareType=newTareType
grading.gradDBStore()grading.refreshSetpointGrading()end
function grading.editPrintType(label)local newPrintType,isEnterKey
newPrintType=grading.PrintType
newPrintType,isEnterKey=awtx.keypad.selectList("MANUAL, AUTO",newPrintType)awtx.display.writeLine(label)if isEnterKey then
grading.setPrintType(newPrintType)else
end
return isEnterKey
end
function grading.setPrintType(newPrintType)grading.PrintType=newPrintType
grading.gradDBStore()grading.refreshSetpointGrading()end
function grading.gradDBStoreLabels()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGradingLabels (GradIndex INTEGER, Under  TEXT, Grad1  TEXT, Grad2  TEXT, Grad3  TEXT, Grad4  TEXT, Grad5  TEXT, Grad6  TEXT, Grad7  TEXT,Grad8  TEXT, Grad9  TEXT, Grad10 TEXT, Over TEXT)")dbFile:execute("BEGIN TRANSACTION")sqlStr=string.format("SELECT GradIndex, Under, Grad1, Grad2, Grad3, Grad4, Grad5, Grad6, Grad7, Grad8, Grad9, Grad10, Over FROM tblGradingLabels WHERE GradIndex = '1'")found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGradingLabels (GradIndex, Under, Grad1, Grad2, Grad3, Grad4, Grad5, Grad6, Grad7, Grad8, Grad9, Grad10, Under) VALUES ('1', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s')",Grading_Names[0],Grading_Names[1],Grading_Names[2],Grading_Names[3],Grading_Names[4],Grading_Names[5],Grading_Names[6],Grading_Names[7],Grading_Names[8],Grading_Names[9],Grading_Names[10],Grading_Names[11])result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGradingLabels SET GradIndex = '1', Under = '%s', Grad1 = '%s', Grad2 = '%s', Grad3 = '%s', Grad4 = '%s', Grad5 = '%s', Grad6 = '%s', Grad7 = '%s', Grad8 = '%s', Grad9 = '%s', Grad10 = '%s', Over = '%s'",Grading_Names[0],Grading_Names[1],Grading_Names[2],Grading_Names[3],Grading_Names[4],Grading_Names[5],Grading_Names[6],Grading_Names[7],Grading_Names[8],Grading_Names[9],Grading_Names[10],Grading_Names[11])result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()end
function grading.gradDBRecallLabels()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGradingLabels (GradIndex INTEGER, Under  TEXT, Grad1  TEXT, Grad2  TEXT, Grad3  TEXT, Grad4  TEXT, Grad5  TEXT, Grad6  TEXT, Grad7  TEXT,Grad8  TEXT, Grad9  TEXT, Grad10 TEXT, Over TEXT)")sqlStr=string.format("SELECT GradIndex, Under, Grad1, Grad2, Grad3, Grad4, Grad5, Grad6, Grad7, Grad8, Grad9, Grad10, Over FROM tblGradingLabels WHERE GradIndex = '1'")found=false
for row in dbFile:rows(sqlStr)do
found=true
Grading_Names[0]=row[2]Grading_Names[1]=row[3]Grading_Names[2]=row[4]Grading_Names[3]=row[5]Grading_Names[4]=row[6]Grading_Names[5]=row[7]Grading_Names[6]=row[8]Grading_Names[7]=row[9]Grading_Names[8]=row[10]Grading_Names[9]=row[11]Grading_Names[10]=row[12]Grading_Names[11]=row[13]grading.GradStr=GradStrDefault
end
if found==false then
Grading_Names[0]=Grad0StrDefault
Grading_Names[1]=Grad1StrDefault
Grading_Names[2]=Grad2StrDefault
Grading_Names[3]=Grad3StrDefault
Grading_Names[4]=Grad4StrDefault
Grading_Names[5]=Grad5StrDefault
Grading_Names[6]=Grad6StrDefault
Grading_Names[7]=Grad7StrDefault
Grading_Names[8]=Grad8StrDefault
Grading_Names[9]=Grad9StrDefault
Grading_Names[10]=Grad10StrDefault
Grading_Names[11]=Grad11StrDefault
grading.GradStr=GradStrDefault
sqlStr=string.format("INSERT INTO tblGradingLabels (GradIndex, Under, Grad1, Grad2, Grad3, Grad4, Grad5, Grad6, Grad7, Grad8, Grad9, Grad10, Under) VALUES ('1', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s')",Grading_Names[0],Grading_Names[1],Grading_Names[2],Grading_Names[3],Grading_Names[4],Grading_Names[5],Grading_Names[6],Grading_Names[7],Grading_Names[8],Grading_Names[9],Grading_Names[10],Grading_Names[11])result=dbFile:exec(sqlStr)dbFile:execute("COMMIT")end
dbFile:close()grading.setGradPrintTokens(grading.temp)end
function grading.editLabel0(label)local newGrad,isEnterKey,newGradmin,newGradmax,tempUnitStr
newGrad=Grading_Names[0]newGradmax=7
newGrad,isEnterKey=awtx.keypad.enterString(newGrad,newGradmax)awtx.display.writeLine(label)if isEnterKey then
Grading_Names[0]=newGrad
grading.gradDBStoreLabels()else
end
return isEnterKey
end
function grading.editLabel1(label)local newGrad,isEnterKey,newGradmin,newGradmax,tempUnitStr
newGrad=Grading_Names[1]newGradmax=7
newGrad,isEnterKey=awtx.keypad.enterString(newGrad,newGradmax)awtx.display.writeLine(label)if isEnterKey then
Grading_Names[1]=newGrad
grading.gradDBStoreLabels()else
end
return isEnterKey
end
function grading.editLabel2(label)local newGrad,isEnterKey,newGradmin,newGradmax,tempUnitStr
newGrad=Grading_Names[2]newGradmax=7
newGrad,isEnterKey=awtx.keypad.enterString(newGrad,newGradmax)awtx.display.writeLine(label)if isEnterKey then
Grading_Names[2]=newGrad
grading.gradDBStoreLabels()else
end
return isEnterKey
end
function grading.editLabel3(label)local newGrad,isEnterKey,newGradmin,newGradmax,tempUnitStr
newGrad=Grading_Names[3]newGradmax=7
newGrad,isEnterKey=awtx.keypad.enterString(newGrad,newGradmax)awtx.display.writeLine(label)if isEnterKey then
Grading_Names[3]=newGrad
grading.gradDBStoreLabels()else
end
return isEnterKey
end
function grading.editLabel4(label)local newGrad,isEnterKey,newGradmin,newGradmax,tempUnitStr
newGrad=Grading_Names[4]newGradmax=7
newGrad,isEnterKey=awtx.keypad.enterString(newGrad,newGradmax)awtx.display.writeLine(label)if isEnterKey then
Grading_Names[4]=newGrad
grading.gradDBStoreLabels()else
end
return isEnterKey
end
function grading.editLabel5(label)local newGrad,isEnterKey,newGradmin,newGradmax,tempUnitStr
newGrad=Grading_Names[5]newGradmax=7
newGrad,isEnterKey=awtx.keypad.enterString(newGrad,newGradmax)awtx.display.writeLine(label)if isEnterKey then
Grading_Names[5]=newGrad
grading.gradDBStoreLabels()else
end
return isEnterKey
end
function grading.editLabel6(label)local newGrad,isEnterKey,newGradmin,newGradmax,tempUnitStr
newGrad=Grading_Names[6]newGradmax=7
newGrad,isEnterKey=awtx.keypad.enterString(newGrad,newGradmax)awtx.display.writeLine(label)if isEnterKey then
Grading_Names[6]=newGrad
grading.gradDBStoreLabels()else
end
return isEnterKey
end
function grading.editLabel7(label)local newGrad,isEnterKey,newGradmin,newGradmax,tempUnitStr
newGrad=Grading_Names[7]newGradmax=7
newGrad,isEnterKey=awtx.keypad.enterString(newGrad,newGradmax)awtx.display.writeLine(label)if isEnterKey then
Grading_Names[7]=newGrad
grading.gradDBStoreLabels()else
end
return isEnterKey
end
function grading.editLabel8(label)local newGrad,isEnterKey,newGradmin,newGradmax,tempUnitStr
newGrad=Grading_Names[8]newGradmax=7
newGrad,isEnterKey=awtx.keypad.enterString(newGrad,newGradmax)awtx.display.writeLine(label)if isEnterKey then
Grading_Names[8]=newGrad
grading.gradDBStoreLabels()else
end
return isEnterKey
end
function grading.editLabel9(label)local newGrad,isEnterKey,newGradmin,newGradmax,tempUnitStr
newGrad=Grading_Names[9]newGradmax=7
newGrad,isEnterKey=awtx.keypad.enterString(newGrad,newGradmax)awtx.display.writeLine(label)if isEnterKey then
Grading_Names[9]=newGrad
grading.gradDBStoreLabels()else
end
return isEnterKey
end
function grading.editLabel10(label)local newGrad,isEnterKey,newGradmin,newGradmax,tempUnitStr
newGrad=Grading_Names[10]newGradmax=7
newGrad,isEnterKey=awtx.keypad.enterString(newGrad,newGradmax)awtx.display.writeLine(label)if isEnterKey then
Grading_Names[10]=newGrad
grading.gradDBStoreLabels()else
end
return isEnterKey
end
function grading.editLabel11(label)local newGrad,isEnterKey,newGradmin,newGradmax,tempUnitStr
newGrad=Grading_Names[11]newGradmax=7
newGrad,isEnterKey=awtx.keypad.enterString(newGrad,newGradmax)awtx.display.writeLine(label)if isEnterKey then
Grading_Names[11]=newGrad
grading.gradDBStoreLabels()else
end
return isEnterKey
end
function grading.aboveGZB()local usermode
awtx.os.sleep(500)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)wt=awtx.weight.getCurrent(1)if wt.net<=grading.Grad1 then
grading.GradStr=Grading_Names[0]usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif wt.net<=grading.Grad2 then
awtx.display.writeLine(Grading_Names[1])grading.GradStr=Grading_Names[1]awtx.os.sleep(gradtime)elseif wt.net<=grading.Grad3 or(grading.Grad3==0 and wt.net<grading.GradMax)then
awtx.display.writeLine(Grading_Names[2])grading.GradStr=Grading_Names[2]awtx.os.sleep(gradtime)elseif wt.net<=grading.Grad4 or(grading.Grad4==0 and wt.net<grading.GradMax)then
awtx.display.writeLine(Grading_Names[3])grading.GradStr=Grading_Names[3]awtx.os.sleep(gradtime)elseif wt.net<=grading.Grad5 or(grading.Grad5==0 and wt.net<grading.GradMax)then
awtx.display.writeLine(Grading_Names[4])grading.GradStr=Grading_Names[4]awtx.os.sleep(gradtime)elseif wt.net<=grading.Grad6 or(grading.Grad6==0 and wt.net<grading.GradMax)then
awtx.display.writeLine(Grading_Names[5])grading.GradStr=Grading_Names[5]awtx.os.sleep(gradtime)elseif wt.net<=grading.Grad7 or(grading.Grad7==0 and wt.net<grading.GradMax)then
awtx.display.writeLine(Grading_Names[6])grading.GradStr=Grading_Names[6]awtx.os.sleep(gradtime)elseif wt.net<=grading.Grad8 or(grading.Grad8==0 and wt.net<grading.GradMax)then
awtx.display.writeLine(Grading_Names[7])grading.GradStr=Grading_Names[7]awtx.os.sleep(gradtime)elseif wt.net<=grading.Grad9 or(grading.Grad9==0 and wt.net<grading.GradMax)then
awtx.display.writeLine(Grading_Names[8])grading.GradStr=Grading_Names[8]awtx.os.sleep(gradtime)elseif wt.net<=grading.Grad10 or(grading.Grad10==0 and wt.net<grading.GradMax)then
awtx.display.writeLine(Grading_Names[9])grading.GradStr=Grading_Names[9]awtx.os.sleep(gradtime)elseif wt.net<=grading.Grad11 or(grading.Grad11==0 and wt.net<grading.GradMax)then
awtx.display.writeLine(Grading_Names[10])grading.GradStr=Grading_Names[10]awtx.os.sleep(gradtime)elseif wt.net>grading.GradMax then
grading.GradStr=Grading_Names[11]usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)else
grading.GradStr=Grading_Names[12]usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)end
grading.temp=wt.net
grading.setGradPrintTokens(grading.temp)end
function grading.aboveGZBNeg()local usermode
awtx.os.sleep(500)usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)wt=awtx.weight.getCurrent(1)if wt.net>=grading.Grad1Neg then
grading.GradStr=Grading_Names[0]usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif wt.net>=grading.Grad2Neg then
awtx.display.writeLine(Grading_Names[1])grading.GradStr=Grading_Names[1]awtx.os.sleep(gradtime)elseif wt.net>=grading.Grad3Neg or(grading.Grad3Neg==0 and wt.net>grading.GradMaxNeg)then
awtx.display.writeLine(Grading_Names[2])grading.GradStr=Grading_Names[2]awtx.os.sleep(gradtime)elseif wt.net>=grading.Grad4Neg or(grading.Grad4Neg==0 and wt.net>grading.GradMaxNeg)then
awtx.display.writeLine(Grading_Names[3])grading.GradStr=Grading_Names[3]awtx.os.sleep(gradtime)elseif wt.net>=grading.Grad5Neg or(grading.Grad5Neg==0 and wt.net>grading.GradMaxNeg)then
awtx.display.writeLine(Grading_Names[4])grading.GradStr=Grading_Names[4]awtx.os.sleep(gradtime)elseif wt.net>=grading.Grad6Neg or(grading.Grad6Neg==0 and wt.net>grading.GradMaxNeg)then
awtx.display.writeLine(Grading_Names[5])grading.GradStr=Grading_Names[5]awtx.os.sleep(gradtime)elseif wt.net>=grading.Grad7Neg or(grading.Grad7Neg==0 and wt.net>grading.GradMaxNeg)then
awtx.display.writeLine(Grading_Names[6])grading.GradStr=Grading_Names[6]awtx.os.sleep(gradtime)elseif wt.net>=grading.Grad8Neg or(grading.Grad8Neg==0 and wt.net>grading.GradMaxNeg)then
awtx.display.writeLine(Grading_Names[7])grading.GradStr=Grading_Names[7]awtx.os.sleep(gradtime)elseif wt.net>=grading.Grad9Neg or(grading.Grad9Neg==0 and wt.net>grading.GradMaxNeg)then
awtx.display.writeLine(Grading_Names[8])grading.GradStr=Grading_Names[8]awtx.os.sleep(gradtime)elseif wt.net>=grading.Grad10Neg or(grading.Grad10Neg==0 and wt.net>grading.GradMaxNeg)then
awtx.display.writeLine(Grading_Names[9])grading.GradStr=Grading_Names[9]awtx.os.sleep(gradtime)elseif wt.net>=grading.Grad11Neg or(grading.Grad11Neg==0 and wt.net>grading.GradMaxNeg)then
awtx.display.writeLine(Grading_Names[10])grading.GradStr=Grading_Names[10]awtx.os.sleep(gradtime)elseif wt.net<grading.GradMaxNeg then
grading.GradStr=Grading_Names[11]usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)else
grading.GradStr=Grading_Names[12]usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)end
grading.temp=wt.net*-1
grading.setGradPrintTokens(grading.temp)if grading.PrintType==PRINT_AUTO then
wt=awtx.weight.getRefreshLastPrint()awtx.printer.PrintFmt(0)end
if grading.TareType==TARE_AUTO then
awtx.weight.requestTare()end
end
function grading.belowGZB()local usermode
awtx.os.sleep(500)wt=awtx.weight.getCurrent(1)if wt.net<=grading.Grad1 then
grading.GradStr=Grading_Names[0]usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif wt.net>grading.GradMax then
grading.GradStr=Grading_Names[11]usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)end
grading.temp=wt.net
grading.setGradPrintTokens(grading.temp)end
function grading.belowGZBNeg()local usermode
awtx.os.sleep(500)wt=awtx.weight.getCurrent(1)if wt.net>=grading.Grad1Neg then
grading.GradStr=Grading_Names[0]usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif wt.net<grading.GradMaxNeg then
grading.GradStr=Grading_Names[11]usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)end
grading.temp=wt.net*-1
grading.setGradPrintTokens(grading.temp)end
function grading.setpointOut9Handler(setpointNum,isActivate)if isActivate then
else
end
end
function grading.setpointOut9HandlerNeg(setpointNum,isActivate)if isActivate then
if grading.TareType==TARE_AUTO then
awtx.weight.requestTare()end
else
end
end
function grading.setpointOut10Handler(setpointNum,isActivate)if isActivate then
grading.aboveGZB()else
grading.belowGZB()end
end
function grading.setpointOut10HandlerNeg(setpointNum,isActivate)if isActivate then
grading.aboveGZBNeg()else
grading.belowGZBNeg()end
end
function grading.sortEntry()grading.GradMin=grading.Grad1
grading.GradMinNeg=grading.Grad1Neg
grading.GradMax=grading.Grad11
grading.GradMaxNeg=grading.Grad11Neg
if grading.Grad11==0 and grading.GradMax==0 then
grading.GradMax=grading.Grad10
grading.GradMaxNeg=grading.Grad10Neg
end
if grading.Grad10==0 and grading.GradMax==0 then
grading.GradMax=grading.Grad9
grading.GradMaxNeg=grading.Grad9Neg
end
if grading.Grad9==0 and grading.GradMax==0 then
grading.GradMax=grading.Grad8
grading.GradMaxNeg=grading.Grad8Neg
end
if grading.Grad8==0 and grading.GradMax==0 then
grading.GradMax=grading.Grad7
grading.GradMaxNeg=grading.Grad7Neg
end
if grading.Grad7==0 and grading.GradMax==0 then
grading.GradMax=grading.Grad6
grading.GradMaxNeg=grading.Grad6Neg
end
if grading.Grad6==0 and grading.GradMax==0 then
grading.GradMax=grading.Grad5
grading.GradMaxNeg=grading.Grad5Neg
end
if grading.Grad5==0 and grading.GradMax==0 then
grading.GradMax=grading.Grad4
grading.GradMaxNeg=grading.Grad4Neg
end
if grading.Grad4==0 and grading.GradMax==0 then
grading.GradMax=grading.Grad3
grading.GradMaxNeg=grading.Grad3Neg
end
if grading.Grad3==0 and grading.GradMax==0 then
grading.GradMax=grading.Grad2
grading.GradMaxNeg=grading.Grad2Neg
end
end
function grading.refreshSetpointGrading()local setPointDisabled,setPointOutEx9,setPointOutEx10,setPointOutEx10Neg,retVal,resultMsg,result,myVarLo,myVarHi
grading.sortEntry()setPointDisabled={mode="disabled"}setPointOutEx9={mode="output",bounceTime=0,offInsideGZB=false,act="above",actLowerVarName="GZBHi",actBasis="grossWt",actMotionInhibit=true,deact="below",deactLowerVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=true}setPointOutEx10={mode="output",bounceTime=0,offInsideGZB=false,act="above",actLowerVarName="GZBHi",actBasis="grossWt",actMotionInhibit=true,deact="below",deactLowerVarName="GZBHi",deactBasis="grossWt",deactMotionInhibit=false}setPointOutEx10Neg={mode="output",bounceTime=0,offInsideGZB=false,act="below",actLowerVarName="GZBLo",actBasis="netWt",actMotionInhibit=true,deact="above",deactLowerVarName="GZBLo",deactBasis="netWt",deactMotionInhibit=false}retVal,resultMsg=awtx.setPoint.set(4,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(5,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(6,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(7,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(8,setPointDisabled)if grading.GradType==NEG_GRADING then
retVal,resultMsg=awtx.setPoint.set(9,setPointOutEx9)result=awtx.setPoint.registerOutputEvent(9,grading.setpointOut9HandlerNeg)retVal,resultMsg=awtx.setPoint.set(10,setPointOutEx10Neg)result=awtx.setPoint.registerOutputEvent(10,grading.setpointOut10HandlerNeg)else
retVal,resultMsg=awtx.setPoint.set(9,setPointDisabled)result=awtx.setPoint.registerOutputEvent(9,grading.setpointOut9Handler)retVal,resultMsg=awtx.setPoint.set(10,setPointOutEx10)result=awtx.setPoint.registerOutputEvent(10,grading.setpointOut10Handler)end
myVarLo=-wt.curDivision*config.grossZeroBand
myVarHi=wt.curDivision*config.grossZeroBand
awtx.setPoint.varSet("GZBLo",myVarLo)awtx.setPoint.varSet("GZBHi",myVarHi)awtx.setPoint.varSet("GradMin",grading.GradMin)awtx.setPoint.varSet("GradMinNeg",grading.GradMinNeg)awtx.setPoint.varSet("GradMax",grading.GradMax)awtx.setPoint.varSet("GradMaxNeg",grading.GradMaxNeg)awtx.setPoint.varSet("Grad1",grading.Grad1)awtx.setPoint.varSet("Grad1Neg",grading.Grad1Neg)awtx.setPoint.varSet("Grad2",grading.Grad2)awtx.setPoint.varSet("Grad2Neg",grading.Grad2Neg)awtx.setPoint.varSet("Grad3",grading.Grad3)awtx.setPoint.varSet("Grad3Neg",grading.Grad3Neg)awtx.setPoint.varSet("Grad4",grading.Grad4)awtx.setPoint.varSet("Grad4Neg",grading.Grad4Neg)awtx.setPoint.varSet("Grad5",grading.Grad5)awtx.setPoint.varSet("Grad5Neg",grading.Grad5Neg)awtx.setPoint.varSet("Grad6",grading.Grad6)awtx.setPoint.varSet("Grad6Neg",grading.Grad6Neg)awtx.setPoint.varSet("Grad7",grading.Grad7)awtx.setPoint.varSet("Grad7Neg",grading.Grad7Neg)awtx.setPoint.varSet("Grad8",grading.Grad8)awtx.setPoint.varSet("Grad8Neg",grading.Grad8Neg)awtx.setPoint.varSet("Grad9",grading.Grad9)awtx.setPoint.varSet("Grad9Neg",grading.Grad9Neg)awtx.setPoint.varSet("Grad10",grading.Grad10)awtx.setPoint.varSet("Grad10Neg",grading.Grad10Neg)awtx.setPoint.varSet("Grad11",grading.Grad11)awtx.setPoint.varSet("Grad11Neg",grading.Grad11Neg)grading.temp=0.0
grading.GradStr=GradStrDefault
grading.setGradPrintTokens(grading.temp)end
function grading.refreshSetpointsDisabled()local setPointDisabled,retVal,resultMsg,result
setPointDisabled={mode="disabled"}retVal,resultMsg=awtx.setPoint.set(1,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(2,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(3,setPointDisabled)result=awtx.setPoint.unregisterOutputEvent(1)result=awtx.setPoint.unregisterOutputEvent(2)result=awtx.setPoint.unregisterOutputEvent(3)result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(3)end
function grading.refreshSetpointsDisabledBattery()local setPointDisabled,setPointBattery,retVal,resultMsg,result
setPointDisabled={mode="disabled"}setPointBattery={mode="output",bounceTime=minLatchTime,offInsideGZB=false,act="user",deact="user"}retVal,resultMsg=awtx.setPoint.set(1,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(2,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)result=awtx.setPoint.unregisterOutputEvent(1)result=awtx.setPoint.unregisterOutputEvent(2)result=awtx.setPoint.unregisterOutputEvent(3)result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(3)end
function grading.setpointInputAction(cfgInputType)if Input_Type_String[cfgInputType]=="None     "then
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
function grading.setpointIn1Handler(setpointNum,inputState)if inputState then
grading.setpointInputAction(SetpointInputtable[1].InputValue)else
end
end
function grading.setpointIn2Handler(setpointNum,inputState)if inputState then
grading.setpointInputAction(SetpointInputtable[2].InputValue)else
end
end
function grading.setpointIn3Handler(setpointNum,inputState)if inputState then
grading.setpointInputAction(SetpointInputtable[3].InputValue)else
end
end
function grading.refreshSetpointsInputs()local setPointInEx1,setPointInEx2,setPointInEx3,retVal,resultMsg,result
setPointInEx1={mode="input",bounceTime=1}setPointInEx2={mode="input",bounceTime=1}setPointInEx3={mode="input",bounceTime=1}retVal,resultMsg=awtx.setPoint.set(1,setPointInEx1)retVal,resultMsg=awtx.setPoint.set(2,setPointInEx2)retVal,resultMsg=awtx.setPoint.set(3,setPointInEx3)result=awtx.setPoint.registerInputEvent(1,grading.setpointIn1Handler)result=awtx.setPoint.registerInputEvent(2,grading.setpointIn2Handler)result=awtx.setPoint.registerInputEvent(3,grading.setpointIn3Handler)setpoint.setSetpointPrintTokens()end
grading.gradDBInit()grading.gradDBRecall()grading.gradDBRecallLabels()grading.refreshSetpointGrading()