function doMyDateTime()local tmpOSDateTime,myDate,myTime,myDateTime,APstr
tmpOSDateTime=os.date("*t")if config.dateFormat==nil then
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)else
if config.dateFormat==0 then
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)elseif config.dateFormat==1 then
myDate=string.format("%02d-%02d-%04d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)elseif config.dateFormat==2 then
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.day,tmpOSDateTime.month,tmpOSDateTime.year)elseif config.dateFormat==3 then
myDate=string.format("%02d-%02d-%04d",tmpOSDateTime.day,tmpOSDateTime.month,tmpOSDateTime.year)else
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)end
end
if config.timeFormat==nil then
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)else
if config.timeFormat==0 then
if tmpOSDateTime.hour>12 then
tmpOSDateTime.hour=tmpOSDateTime.hour-12
end
if tmpOSDateTime.hour==0 then
tmpOSDateTime.hour=12
end
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)elseif config.timeFormat==1 then
APstr="A"if tmpOSDateTime.hour>11 then
APstr="P"end
if tmpOSDateTime.hour>12 then
tmpOSDateTime.hour=tmpOSDateTime.hour-12
end
if tmpOSDateTime.hour==0 then
tmpOSDateTime.hour=12
end
myTime=string.format("%02d:%02d:%02d %s",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec,APstr)elseif config.timeFormat==2 then
if tmpOSDateTime.hour==0 then
tmpOSDateTime.hour=24
end
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)else
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)end
end
myDateTime=myDate.." "..myTime
return myDateTime
end
function doMyDate()local tmpOSDateTime,myDate,myTime,myDateTime
tmpOSDateTime=os.date("*t")if config.dateFormat==nil then
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)else
if config.dateFormat==0 then
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)elseif config.dateFormat==1 then
myDate=string.format("%02d-%02d-%04d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)elseif config.dateFormat==2 then
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.day,tmpOSDateTime.month,tmpOSDateTime.year)elseif config.dateFormat==3 then
myDate=string.format("%02d-%02d-%04d",tmpOSDateTime.day,tmpOSDateTime.month,tmpOSDateTime.year)else
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)end
end
return myDate
end
function doMyTime()local tmpOSDateTime,myDate,myTime,myDateTime,APstr
tmpOSDateTime=os.date("*t")if config.timeFormat==nil then
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)else
if config.timeFormat==0 then
if tmpOSDateTime.hour>12 then
tmpOSDateTime.hour=tmpOSDateTime.hour-12
end
if tmpOSDateTime.hour==0 then
tmpOSDateTime.hour=12
end
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)elseif config.timeFormat==1 then
APstr="A"if tmpOSDateTime.hour>11 then
APstr="P"end
if tmpOSDateTime.hour>12 then
tmpOSDateTime.hour=tmpOSDateTime.hour-12
end
if tmpOSDateTime.hour==0 then
tmpOSDateTime.hour=12
end
myTime=string.format("%02d:%02d:%02d %s",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec,APstr)elseif config.timeFormat==2 then
if tmpOSDateTime.hour==0 then
tmpOSDateTime.hour=24
end
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)else
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)end
end
return myTime
end
function doTimestampDate(tmpTimestamp)local tmpOSDateTime,myDate,myTime,myDateTime
tmpOSDateTime=os.date("*t",tmpTimestamp)if config.dateFormat==nil then
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)else
if config.dateFormat==0 then
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)elseif config.dateFormat==1 then
myDate=string.format("%02d-%02d-%04d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)elseif config.dateFormat==2 then
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.day,tmpOSDateTime.month,tmpOSDateTime.year)elseif config.dateFormat==3 then
myDate=string.format("%02d-%02d-%04d",tmpOSDateTime.day,tmpOSDateTime.month,tmpOSDateTime.year)else
myDate=string.format("%02d-%02d-%02d",tmpOSDateTime.month,tmpOSDateTime.day,tmpOSDateTime.year)end
end
return myDate
end
function doTimestampTime(tmpTimestamp)local tmpOSDateTime,myDate,myTime,myDateTime,APstr
tmpOSDateTime=os.date("*t",tmpTimestamp)if config.timeFormat==nil then
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)else
if config.timeFormat==0 then
if tmpOSDateTime.hour>12 then
tmpOSDateTime.hour=tmpOSDateTime.hour-12
end
if tmpOSDateTime.hour==0 then
tmpOSDateTime.hour=12
end
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)elseif config.timeFormat==1 then
APstr="A"if tmpOSDateTime.hour>11 then
APstr="P"end
if tmpOSDateTime.hour>12 then
tmpOSDateTime.hour=tmpOSDateTime.hour-12
end
if tmpOSDateTime.hour==0 then
tmpOSDateTime.hour=12
end
myTime=string.format("%02d:%02d:%02d %s",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec,APstr)elseif config.timeFormat==2 then
if tmpOSDateTime.hour==0 then
tmpOSDateTime.hour=24
end
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)else
myTime=string.format("%02d:%02d:%02d",tmpOSDateTime.hour,tmpOSDateTime.min,tmpOSDateTime.sec)end
end
return myTime
end
truck={}reprintFmt=0
local reportLabelList={}local reportLabel
reportLabelList={[0]="RePRINT","IN Rpt","OUT Rpt","FLT Rpt"}reportLabel="RePRINT,IN Rpt,OUT Rpt,FLT Rpt"local menuprintLabelList={}local menuprintLabel
menuprintLabelList={[0]="IN Rpt","OUT Rpt","FLT Rpt"}menuprintLabel="IN Rpt,OUT Rpt,FLT Rpt"dbunitsDefault="lb"if config.calwtunitStr~=nil then
dbunitsDefault=config.calwtunitStr
end
newtruckId=0
newtareFleet=0
newweight1=0
newweight1date=" "newweight1time=" "newgrossTotal=0
newnetTotal=0
newtareTotal=0
newtransCount=0
newunits=wt.unitsStr
dbunits=dbunitsDefault
newTruckGross=0
newTruckNet=0
newTruckTare=0
newTicketDate=" "newTicketTime=" "truck.InFmtDefault=2
truck.InHeaderFmtDefault=3
truck.InBodyFmtDefault=4
truck.InFooterFmtDefault=5
truck.OutFmtDefault=6
truck.OutHeaderFmtDefault=7
truck.OutBodyFmtDefault=8
truck.OutFooterFmtDefault=9
truck.FleetFmtDefault=10
truck.FleetHeaderFmtDefault=11
truck.FleetBodyFmtDefault=12
truck.FleetFooterFmtDefault=13
truck.LiteEnableFlagDefault=0
truck.LiteTHoldDefault=1000
truck.InFmt=truck.InFmtDefault
truck.InHeaderFmt=truck.InHeaderFmtDefault
truck.InBodyFmt=truck.InBodyFmtDefault
truck.InFooterFmt=truck.InFooterFmtDefault
truck.OutFmt=truck.OutFmtDefault
truck.OutHeaderFmt=truck.OutHeaderFmtDefault
truck.OutBodyFmt=truck.OutBodyFmtDefault
truck.OutFooterFmt=truck.OutFooterFmtDefault
truck.FleetFmt=truck.FleetFmtDefault
truck.FleetHeaderFmt=truck.FleetHeaderFmtDefault
truck.FleetBodyFmt=truck.FleetBodyFmtDefault
truck.FleetFooterFmt=truck.FleetFooterFmtDefault
truck.LiteEnableFlag=truck.LiteEnableFlagDefault
truck.LiteTHold=truck.LiteTHoldDefault
if system.modelStr=="ZM301"then
MAX_TRUCK_INDEX=10
elseif system.modelStr=="ZM303"then
MAX_TRUCK_INDEX=10
elseif system.modelStr=="ZQ375"then
MAX_TRUCK_INDEX=10
elseif system.modelStr=="ZM305GTN"then
MAX_TRUCK_INDEX=1000
elseif system.modelStr=="ZM305"then
MAX_TRUCK_INDEX=10
else
MAX_TRUCK_INDEX=10
end
function truck.initTruckPrintTokens()awtx.fmtPrint.varSet(21,newtruckId,"Truck ID",AWTX_LUA_INTEGER)printTokens[21].varName="newtruckId"printTokens[21].varLabel="Truck ID"printTokens[21].varType=AWTX_LUA_INTEGER
printTokens[21].varValue=newtruckId
printTokens[21].varFunct=""awtx.fmtPrint.varSet(22,newtareFleet,"Fleet Tare",AWTX_LUA_FLOAT)printTokens[22].varName="newtareFleet"printTokens[22].varLabel="Fleet Tare"printTokens[22].varType=AWTX_LUA_FLOAT
printTokens[22].varValue=newtareFleet
printTokens[22].varFunct=""awtx.fmtPrint.varSet(23,newweight1,"InWeight",AWTX_LUA_FLOAT)printTokens[23].varName="newweight1"printTokens[23].varLabel="InWeight"printTokens[23].varType=AWTX_LUA_FLOAT
printTokens[23].varValue=newweight1
printTokens[23].varFunct=""awtx.fmtPrint.varSet(24,newweight1date,"In Date",AWTX_LUA_STRING)printTokens[24].varName="newweight1date"printTokens[24].varLabel="InDate"printTokens[24].varType=AWTX_LUA_STRING
printTokens[24].varValue=newweight1date
printTokens[24].varFunct=""awtx.fmtPrint.varSet(25,newweight1time,"In Time",AWTX_LUA_STRING)printTokens[25].varName="newweight1time"printTokens[25].varLabel="InTime"printTokens[25].varType=AWTX_LUA_STRING
printTokens[25].varValue=newweight1time
printTokens[25].varFunct=""awtx.fmtPrint.varSet(26,newgrossTotal,"Gross Total",AWTX_LUA_FLOAT)printTokens[26].varName="newgrossTotal"printTokens[26].varLabel="Gross Total"printTokens[26].varType=AWTX_LUA_FLOAT
printTokens[26].varValue=newgrossTotal
printTokens[26].varFunct=""awtx.fmtPrint.varSet(27,newnetTotal,"Net Total",AWTX_LUA_FLOAT)printTokens[27].varName="newnetTotal"printTokens[27].varLabel="Net Total"printTokens[27].varType=AWTX_LUA_FLOAT
printTokens[27].varValue=newnetTotal
printTokens[27].varFunct=""awtx.fmtPrint.varSet(28,newtareTotal,"Tare Total",AWTX_LUA_FLOAT)printTokens[28].varName="newtareTotal"printTokens[28].varLabel="Tare Total"printTokens[28].varType=AWTX_LUA_FLOAT
printTokens[28].varValue=newtareTotal
printTokens[28].varFunct=""awtx.fmtPrint.varSet(29,newtransCount,"Transaction Count",AWTX_LUA_INTEGER)printTokens[29].varName="newtransCount"printTokens[29].varLabel="Transaction Count"printTokens[29].varType=AWTX_LUA_INTEGER
printTokens[29].varValue=newtransCount
printTokens[29].varFunct=""awtx.fmtPrint.varSet(30,newunits,"Unit of Measure",AWTX_LUA_STRING)printTokens[30].varName="newunits"printTokens[30].varLabel="Unit of Measure"printTokens[30].varType=AWTX_LUA_STRING
printTokens[30].varValue=newunits
printTokens[30].varFunct=""awtx.fmtPrint.varSet(31,newTruckGross,"Gross Weight",AWTX_LUA_FLOAT)printTokens[31].varName="newTruckGross"printTokens[31].varLabel="Gross"printTokens[31].varType=AWTX_LUA_FLOAT
printTokens[31].varValue=newTruckGross
printTokens[31].varFunct=""awtx.fmtPrint.varSet(32,newTruckTare,"Tare Weight",AWTX_LUA_FLOAT)printTokens[32].varName="newTruckTare"printTokens[32].varLabel="Tare"printTokens[32].varType=AWTX_LUA_FLOAT
printTokens[32].varValue=newTruckTare
printTokens[32].varFunct=""awtx.fmtPrint.varSet(33,newTruckNet,"Net Weight",AWTX_LUA_FLOAT)printTokens[33].varName="newTruckNet"printTokens[33].varLabel="Net"printTokens[33].varType=AWTX_LUA_FLOAT
printTokens[33].varValue=newTruckNet
printTokens[33].varFunct=""awtx.fmtPrint.varSet(34,newTicketDate,"Ticket Date",AWTX_LUA_STRING)printTokens[34].varName="newweight1date"printTokens[34].varLabel="InDate"printTokens[34].varType=AWTX_LUA_STRING
printTokens[34].varValue=newTicketDate
printTokens[34].varFunct=""awtx.fmtPrint.varSet(35,newTicketTime,"Ticket Time",AWTX_LUA_STRING)printTokens[35].varName="newweight1time"printTokens[35].varLabel="InTime"printTokens[35].varType=AWTX_LUA_STRING
printTokens[35].varValue=newTicketTime
printTokens[35].varFunct=""end
function truck.setTruckPrintTokens()if HiResDB then
tmpunitindex=wt.units
tmptareFleet=awtx.weight.convertWeight(tmpunitindex,newtareFleet,tmpunitindex,1)tmpweight1=awtx.weight.convertWeight(tmpunitindex,newweight1,tmpunitindex,1)tmpgrossTotal=awtx.weight.convertWeight(tmpunitindex,newgrossTotal,tmpunitindex,1)tmpnetTotal=awtx.weight.convertWeight(tmpunitindex,newnetTotal,tmpunitindex,1)tmptareTotal=awtx.weight.convertWeight(tmpunitindex,newtareTotal,tmpunitindex,1)awtx.fmtPrint.varSet(21,newtruckId,"Truck ID",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(22,tmptareFleet,"Fleet Tare",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(23,tmpweight1,"InWeight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(24,newweight1date,"InDate",AWTX_LUA_STRING)awtx.fmtPrint.varSet(25,newweight1time,"InTime",AWTX_LUA_STRING)awtx.fmtPrint.varSet(26,tmpgrossTotal,"Gross Total",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(27,tmpnetTotal,"Net Total",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(28,tmptareTotal,"Tare Total",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(29,newtransCount,"Transaction Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(30,newunits,"Unit of Measure",AWTX_LUA_STRING)awtx.fmtPrint.varSet(31,newTruckGross,"Gross Weight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(32,newTruckTare,"Tare Weight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(33,newTruckNet,"Net Weight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(34,newTicketDate,"Ticket Date",AWTX_LUA_STRING)awtx.fmtPrint.varSet(35,newTicketTime,"Ticket Time",AWTX_LUA_STRING)else
awtx.fmtPrint.varSet(21,newtruckId,"Truck ID",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(22,newtareFleet,"Fleet Tare",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(23,newweight1,"InWeight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(24,newweight1date,"InDate",AWTX_LUA_STRING)awtx.fmtPrint.varSet(25,newweight1time,"InTime",AWTX_LUA_STRING)awtx.fmtPrint.varSet(26,newgrossTotal,"Gross Total",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(27,newnetTotal,"Net Total",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(28,newtareTotal,"Tare Total",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(29,newtransCount,"Transaction Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(30,newunits,"Unit of Measure",AWTX_LUA_STRING)awtx.fmtPrint.varSet(31,newTruckGross,"Gross Weight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(32,newTruckTare,"Tare Weight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(33,newTruckNet,"Net Weight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(34,newTicketDate,"Ticket Date",AWTX_LUA_STRING)awtx.fmtPrint.varSet(35,newTicketTime,"Ticket Time",AWTX_LUA_STRING)end
end
function truck.setTruckReportTokens()if HiResDB then
tmpunitindex=wt.units
tmptareFleet=awtx.weight.convertWeight(tmpunitindex,newtareFleet,tmpunitindex,1)tmpweight1=awtx.weight.convertWeight(tmpunitindex,newweight1,tmpunitindex,1)tmpgrossTotal=awtx.weight.convertWeight(tmpunitindex,newgrossTotal,tmpunitindex,1)tmpnetTotal=awtx.weight.convertWeight(tmpunitindex,newnetTotal,tmpunitindex,1)tmptareTotal=awtx.weight.convertWeight(tmpunitindex,newtareTotal,tmpunitindex,1)awtx.fmtPrint.varSet(21,newtruckId,"Truck ID",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(22,tmptareFleet,"Fleet Tare",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(23,tmpweight1,"InWeight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(24,newweight1date,"InDate",AWTX_LUA_STRING)awtx.fmtPrint.varSet(25,newweight1time,"InTime",AWTX_LUA_STRING)awtx.fmtPrint.varSet(26,tmpgrossTotal,"Gross Total",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(27,tmpnetTotal,"Net Total",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(28,tmptareTotal,"Tare Total",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(29,newtransCount,"Transaction Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(30,newunits,"Unit of Measure",AWTX_LUA_STRING)awtx.fmtPrint.varSet(34,newTicketDate,"Ticket Date",AWTX_LUA_STRING)awtx.fmtPrint.varSet(35,newTicketTime,"Ticket Time",AWTX_LUA_STRING)else
awtx.fmtPrint.varSet(21,newtruckId,"Truck ID",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(22,newtareFleet,"Fleet Tare",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(23,newweight1,"InWeight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(24,newweight1date,"InDate",AWTX_LUA_STRING)awtx.fmtPrint.varSet(25,newweight1time,"InTime",AWTX_LUA_STRING)awtx.fmtPrint.varSet(26,newgrossTotal,"Gross Total",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(27,newnetTotal,"Net Total",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(28,newtareTotal,"Tare Total",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(29,newtransCount,"Transaction Count",AWTX_LUA_INTEGER)awtx.fmtPrint.varSet(30,newunits,"Unit of Measure",AWTX_LUA_STRING)awtx.fmtPrint.varSet(34,newTicketDate,"Ticket Date",AWTX_LUA_STRING)awtx.fmtPrint.varSet(35,newTicketTime,"Ticket Time",AWTX_LUA_STRING)end
end
function truck.setTruckTicketTokens()awtx.fmtPrint.varSet(30,newunits,"Unit of Measure",AWTX_LUA_STRING)awtx.fmtPrint.varSet(31,newTruckGross,"Gross Weight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(32,newTruckTare,"Tare Weight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(33,newTruckNet,"Net Weight",AWTX_LUA_FLOAT)awtx.fmtPrint.varSet(34,newTicketDate,"Ticket Date",AWTX_LUA_STRING)awtx.fmtPrint.varSet(35,newTicketTime,"Ticket Time",AWTX_LUA_STRING)end
XR4500_RED="&"XR4500_GRN="*"XR4500_OFF="%"XR4500_FLASH_ON="("XR4500_FLASH_OFF=")"XR4500_FLASH_3="!"truck.XR4500=XR4500_OFF
function truck.initXR4500PrintToken()awtx.fmtPrint.varSet(65,truck.XR4500,"XR4500 Light",AWTX_LUA_STRING)printTokens[65].varName="truck.XR4500"printTokens[65].varLabel="XR4500 Light"printTokens[65].varType=AWTX_LUA_STRING
printTokens[65].varValue=truck.XR4500
printTokens[65].varFunct=""end
function truck.setXR4500PrintToken()awtx.fmtPrint.varSet(65,truck.XR4500,"XR4500 Light",AWTX_LUA_STRING)end
truck.truckPrintType=TRUCK_TYPE_NONE
function truck.setTruckPrintType(tmpType)truck.truckPrintType=tmpType
end
function truck.getTruckPrintType()return truck.truckPrintType
end
function truck.clrNewVars()newtruckId=0
newtareFleet=0
newweight1=0
newweight1date=" "newweight1time=" "newgrossTotal=0
newnetTotal=0
newtareTotal=0
newtransCount=0
newunits=wt.unitsStr
dbunits=dbunitsDefault
newTruckGross=0
newTruckNet=0
newTruckTare=0
newTicketDate=" "newTicketTime=" "truck.setTruckPrintType(TRUCK_TYPE_NONE)awtx.weight.requestTareClear()end
local DB_FileLocation_AppConfig
local DB_FileLocation_AppData
local DB_FileLocation_Reports
local DB_ReportName_Truck
function truck.truckDBInit()local simAppPath,dbFile,result
DB_FileLocation_AppConfig=[[C:\Database\AppConfig.db]]DB_FileLocation_AppData=[[C:\Database\AppData.db]]DB_FileLocation_Reports=[[G:\Reports]]..SerialNumber
DB_ReportName_Truck=[[\TruckReport.csv]]dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")dbFile:close()dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTNConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:close()end
function truck.truckDBInClear()local dbFile,result
local found=false
local sqlStr,sqlStr1
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")sqlStr=string.format("SELECT weight1, weight1date, weight1time  FROM tblGTN WHERE weight1 != 0")found=false
newweight1=0
newweight1date=" "newweight1time=" "for row in dbFile:rows(sqlStr)do
sqlStr1=string.format("UPDATE tblGTN SET weight1 = '%f', weight1date = '%s', weight1time = '%s' WHERE weight1 != 0",newweight1,newweight1date,newweight1time)result=dbFile:exec(sqlStr1)end
dbFile:close()return found
end
function truck.truckDBIODelete()local dbFile,result
local found=false
local sqlStr,sqlStr1
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")sqlStr=string.format("DELETE FROM tblGTN WHERE tareFleet = 0")dbFile:exec(sqlStr)dbFile:close()return found
end
function truck.truckDBFleetDelete()local dbFile,result
local found=false
local sqlStr,sqlStr1
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")sqlStr=string.format("DELETE FROM tblGTN WHERE tareFleet != 0")dbFile:exec(sqlStr)dbFile:close()return found
end
function truck.truckDBAllDelete()local dbFile,result
local found=false
local sqlStr,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")sqlStr=string.format("DELETE FROM tblGTN")dbFile:exec(sqlStr)dbFile:close()return found
end
function truck.saveTruckId(tmpId)local dbFile,result
local found=false
local sqlStr,searchIndex,sqlStr
local dbWtUnit,newWtUnit,errorState1,errorState2
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")dbFile:execute("BEGIN TRANSACTION")searchId=tmpId
if searchId~=0 then
sqlStr=string.format("SELECT truckId FROM tblGTN WHERE tblGTN.truckId = %d",searchId)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
local recNo=dbFile:exec("SELECT Count(*) FROM tblGTN")if recNo>=MAX_TRUCK_INDEX then
displayWORD("DB FULL")end
if dbunits==nil then
dbunits=dbunitsDefault
end
errorState1,dbWtUnit=awtx.weight.unitStrToUnitIndex(dbunits)errorState2,newWtUnit=awtx.weight.unitStrToUnitIndex(newunits)if errorState1==0 and errorState2==0 then
if HiResDB then
sqlStr=string.format("INSERT INTO tblGTN (truckId, tareFleet, weight1, weight1date, weight1time, grossTotal, netTotal, tareTotal, transCount, units) VALUES ('%d', '%f', '%f', '%s', '%s', '%f', '%f', '%f', '%d', '%s')",newtruckId,awtx.weight.convertWeight(newWtUnit,newtareFleet,dbWtUnit,0),awtx.weight.convertWeight(newWtUnit,newweight1,dbWtUnit,0),newweight1date,newweight1time,awtx.weight.convertWeight(newWtUnit,newgrossTotal,dbWtUnit,0),awtx.weight.convertWeight(newWtUnit,newnetTotal,dbWtUnit,0),awtx.weight.convertWeight(newWtUnit,newtareTotal,dbWtUnit,0),newtransCount,dbunits)else
sqlStr=string.format("INSERT INTO tblGTN (truckId, tareFleet, weight1, weight1date, weight1time, grossTotal, netTotal, tareTotal, transCount, units) VALUES ('%d', '%f', '%f', '%s', '%s', '%f', '%f', '%f', '%d', '%s')",newtruckId,awtx.weight.convertWeight(newWtUnit,newtareFleet,dbWtUnit,1),awtx.weight.convertWeight(newWtUnit,newweight1,dbWtUnit,1),newweight1date,newweight1time,awtx.weight.convertWeight(newWtUnit,newgrossTotal,dbWtUnit,1),awtx.weight.convertWeight(newWtUnit,newnetTotal,dbWtUnit,1),awtx.weight.convertWeight(newWtUnit,newtareTotal,dbWtUnit,1),newtransCount,dbunits)end
else
sqlStr=string.format("INSERT INTO tblGTN (truckId, tareFleet, weight1, weight1date, weight1time, grossTotal, netTotal, tareTotal, transCount, units) VALUES ('%d', '%f', '%f', '%s', '%s', '%f', '%f', '%f', '%d', '%s')",newtruckId,newtareFleet,newweight1,newweight1date,newweight1time,newgrossTotal,newnetTotal,newtareTotal,newtransCount,dbunits)end
result=dbFile:exec(sqlStr)else
if dbunits==nil then
dbunits=dbunitsDefault
end
errorState1,dbWtUnit=awtx.weight.unitStrToUnitIndex(dbunits)errorState2,newWtUnit=awtx.weight.unitStrToUnitIndex(newunits)if errorState1==0 and errorState2==0 then
if HiResDB then
sqlStr=string.format("UPDATE tblGTN SET truckId = '%d', tareFleet = '%f', weight1 = '%f', weight1date = '%s', weight1time = '%s', grossTotal = '%f', netTotal = '%f', tareTotal = '%f', transCount = '%d', units = '%s' WHERE truckId = %d",newtruckId,awtx.weight.convertWeight(newWtUnit,newtareFleet,dbWtUnit,0),awtx.weight.convertWeight(newWtUnit,newweight1,dbWtUnit,0),newweight1date,newweight1time,awtx.weight.convertWeight(newWtUnit,newgrossTotal,dbWtUnit,0),awtx.weight.convertWeight(newWtUnit,newnetTotal,dbWtUnit,0),awtx.weight.convertWeight(newWtUnit,newtareTotal,dbWtUnit,0),newtransCount,dbunits,searchId)else
sqlStr=string.format("UPDATE tblGTN SET truckId = '%d', tareFleet = '%f', weight1 = '%f', weight1date = '%s', weight1time = '%s', grossTotal = '%f', netTotal = '%f', tareTotal = '%f', transCount = '%d', units = '%s' WHERE truckId = %d",newtruckId,awtx.weight.convertWeight(newWtUnit,newtareFleet,dbWtUnit,1),awtx.weight.convertWeight(newWtUnit,newweight1,dbWtUnit,1),newweight1date,newweight1time,awtx.weight.convertWeight(newWtUnit,newgrossTotal,dbWtUnit,1),awtx.weight.convertWeight(newWtUnit,newnetTotal,dbWtUnit,1),awtx.weight.convertWeight(newWtUnit,newtareTotal,dbWtUnit,1),newtransCount,dbunits,searchId)end
else
sqlStr=string.format("UPDATE tblGTN SET truckId = '%d', tareFleet = '%f', weight1 = '%f', weight1date = '%s', weight1time = '%s', grossTotal = '%f', netTotal = '%f', tareTotal = '%f', transCount = '%d', units = '%s' WHERE truckId = %d",newtruckId,newtareFleet,newweight1,newweight1date,newweight1time,newgrossTotal,newnetTotal,newtareTotal,newtransCount,dbunits,searchId)end
result=dbFile:exec(sqlStr)end
end
dbFile:execute("COMMIT")dbFile:close()end
function truck.findTruckId(tmpId)local dbFile,result
local found=false
local sqlStr,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")sqlStr=string.format("SELECT truckId FROM tblGTN WHERE truckId = %d",tmpId)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
dbFile:close()return found
end
function truck.clearInTruckId(tmpId)local dbFile,result
local found=false
local sqlStr,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")sqlStr=string.format("SELECT truckId FROM tblGTN WHERE truckId = %d",tmpId)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
dbFile:close()if found then
newweight1=0
newweight1date=" "newweight1time=" "truck.saveTruckId(tmpId)end
return found
end
function truck.deleteIOTruckId(tmpId)local dbFile,result
local found=false
local sqlStr,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")sqlStr=string.format("SELECT truckId FROM tblGTN WHERE truckId = %d",tmpId)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found then
sqlStr=string.format("DELETE FROM tblGTN WHERE truckId = %d",tmpId)for row in dbFile:rows(sqlStr)do
end
end
dbFile:close()return found
end
function truck.deleteFleetTruckId(tmpId)local dbFile,result
local found=false
local sqlStr,sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")sqlStr=string.format("SELECT truckId FROM tblGTN WHERE truckId = %d",tmpId)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found then
sqlStr=string.format("DELETE FROM tblGTN WHERE truckId = %d",tmpId)for row in dbFile:rows(sqlStr)do
end
end
dbFile:close()return found
end
function truck.extraStuffStore()local dbFile,result
local found=false
local sqlStr
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTNConfig (varID TEXT PRIMARY KEY, value TEXT)")dbFile:execute("BEGIN TRANSACTION")found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'InFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('InFmt', '%d')",truck.InFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%d' WHERE varID = 'InFmt'",truck.InFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'InHeaderFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('InHeaderFmt', '%d')",truck.InHeaderFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%d' WHERE varID = 'InHeaderFmt'",truck.InHeaderFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'InBodyFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('InBodyFmt', '%d')",truck.InBodyFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%d' WHERE varID = 'InBodyFmt'",truck.InBodyFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'InFooterFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('InFooterFmt', '%d')",truck.InFooterFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%d' WHERE varID = 'InFooterFmt'",truck.InFooterFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'OutFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('OutFmt', '%d')",truck.OutFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%d' WHERE varID = 'OutFmt'",truck.OutFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'OutHeaderFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('OutHeaderFmt', '%d')",truck.OutHeaderFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%d' WHERE varID = 'OutHeaderFmt'",truck.OutHeaderFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'OutBodyFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('OutBodyFmt', '%d')",truck.OutBodyFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%d' WHERE varID = 'OutBodyFmt'",truck.OutBodyFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'OutFooterFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('OutFooterFmt', '%d')",truck.OutFooterFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%d' WHERE varID = 'OutFooterFmt'",truck.OutFooterFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'FleetFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('FleetFmt', '%d')",truck.FleetFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%d' WHERE varID = 'FleetFmt'",truck.FleetFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'FleetHeaderFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('FleetHeaderFmt', '%d')",truck.FleetHeaderFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%d' WHERE varID = 'FleetHeaderFmt'",truck.FleetHeaderFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'FleetBodyFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('FleetBodyFmt', '%d')",truck.FleetBodyFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%d' WHERE varID = 'FleetBodyFmt'",truck.FleetBodyFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'FleetFooterFmt'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('FleetFooterFmt', '%d')",truck.FleetFooterFmt)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%d' WHERE varID = 'FleetFooterFmt'",truck.FleetFooterFmt)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'LiteEnableFlag'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('LiteEnableFlag', '%d')",truck.LiteEnableFlag)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%d' WHERE varID = 'LiteEnableFlag'",truck.LiteEnableFlag)result=dbFile:exec(sqlStr)end
found=false
for row in dbFile:rows("SELECT varID FROM tblGTNConfig WHERE varID = 'LiteTHold'")do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTNConfig (varID, value) VALUES ('LiteTHold', '%f')",truck.LiteTHold)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTNConfig SET value = '%f' WHERE varID = 'LiteTHold'",truck.LiteTHold)result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()end
function truck.recallTruckId(tmptruckId)local dbFile,result,sqlStr
local dbWtUnit,newWtUnit,errorState1,errorState2
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")sqlStr=string.format("SELECT truckId, tareFleet, weight1, weight1date, weight1time, grossTotal, netTotal, tareTotal, transCount, units FROM tblGTN WHERE truckId = %d",tmptruckId)for row in dbFile:rows(sqlStr)do
dbunits=row[10]newunits=wt.unitsStr
if dbunits==nil then
dbunits=dbunitsDefault
end
errorState1,dbWtUnit=awtx.weight.unitStrToUnitIndex(dbunits)errorState2,newWtUnit=awtx.weight.unitStrToUnitIndex(newunits)if errorState1==0 and errorState2==0 then
if HiResDB then
newtruckId=row[1]newtareFleet=awtx.weight.convertWeight(dbWtUnit,row[2],newWtUnit,0)newweight1=awtx.weight.convertWeight(dbWtUnit,row[3],newWtUnit,0)newweight1date=row[4]newweight1time=row[5]newgrossTotal=awtx.weight.convertWeight(dbWtUnit,row[6],newWtUnit,0)newnetTotal=awtx.weight.convertWeight(dbWtUnit,row[7],newWtUnit,0)newtareTotal=awtx.weight.convertWeight(dbWtUnit,row[8],newWtUnit,0)newtransCount=row[9]else
newtruckId=row[1]newtareFleet=awtx.weight.convertWeight(dbWtUnit,row[2],newWtUnit,1)newweight1=awtx.weight.convertWeight(dbWtUnit,row[3],newWtUnit,1)newweight1date=row[4]newweight1time=row[5]newgrossTotal=awtx.weight.convertWeight(dbWtUnit,row[6],newWtUnit,1)newnetTotal=awtx.weight.convertWeight(dbWtUnit,row[7],newWtUnit,1)newtareTotal=awtx.weight.convertWeight(dbWtUnit,row[8],newWtUnit,1)newtransCount=row[9]end
else
newtruckId=row[1]newtareFleet=row[2]newweight1=row[3]newweight1date=row[4]newweight1time=row[5]newgrossTotal=row[6]newnetTotal=row[7]newtareTotal=row[8]newtransCount=row[9]end
end
dbFile:close()end
function truck.extraStuffRecall()local dbFile,result,sqlStr
local found=false
dbFile=sqlite3.open(DB_FileLocation_AppConfig)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTNConfig (varID TEXT PRIMARY KEY, value TEXT)")found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'InFmt'")do
found=true
truck.InFmt=tonumber(row[2])end
if found==false then
truck.InFmt=truck.InFmtDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'InHeaderFmt'")do
found=true
truck.InHeaderFmt=tonumber(row[2])end
if found==false then
truck.InHeaderFmt=truck.InHeaderFmtDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'InBodyFmt'")do
found=true
truck.InBodyFmt=tonumber(row[2])end
if found==false then
truck.InBodyFmt=truck.InBodyFmtDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'InFooterFmt'")do
found=true
truck.InFooterFmt=tonumber(row[2])end
if found==false then
truck.InFooterFmt=truck.InFooterFmtDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'OutFmt'")do
found=true
truck.OutFmt=tonumber(row[2])end
if found==false then
truck.OutFmt=truck.OutFmtDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'OutHeaderFmt'")do
found=true
truck.OutHeaderFmt=tonumber(row[2])end
if found==false then
truck.OutHeaderFmt=truck.OutHeaderFmtDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'OutBodyFmt'")do
found=true
truck.OutBodyFmt=tonumber(row[2])end
if found==false then
truck.OutBodyFmt=truck.OutBodyFmtDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'OutFooterFmt'")do
found=true
truck.OutFooterFmt=tonumber(row[2])end
if found==false then
truck.OutFooterFmt=truck.OutFooterFmtDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'FleetFmt'")do
found=true
truck.FleetFmt=tonumber(row[2])end
if found==false then
truck.FleetFmt=truck.FleetFmtDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'FleetHeaderFmt'")do
found=true
truck.FleetHeaderFmt=tonumber(row[2])end
if found==false then
truck.FleetHeaderFmt=truck.FleetHeaderFmtDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'FleetBodyFmt'")do
found=true
truck.FleetBodyFmt=tonumber(row[2])end
if found==false then
truck.FleetBodyFmt=truck.FleetBodyFmtDefault
end
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'FleetFooterFmt'")do
found=true
truck.FleetFooterFmt=tonumber(row[2])end
if found==false then
truck.FleetFooterFmt=truck.FleetFooterFmtDefault
end
truck.LiteEnableFlag=index
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'LiteEnableFlag'")do
found=true
truck.LiteEnableFlag=tonumber(row[2])end
if found==false then
truck.LiteEnableFlag=truck.LiteEnableFlagDefault
end
truck.LiteTHold=tmpLiteTHold
found=false
for row in dbFile:rows("SELECT varID, value FROM tblGTNConfig WHERE varID = 'LiteTHold'")do
found=true
truck.LiteTHold=tonumber(row[2])end
if found==false then
truck.LiteTHold=truck.LiteTHoldDefault
end
dbFile:close()end
function truck.editInFmt(label)local tempInFmt,FMTmin,FMTmax,isEnterKey
truck.extraStuffRecall()tempInFmt=truck.InFmt
FMTmin=1
FMTmax=40
tempInFmt,isEnterKey=awtx.keypad.enterInteger(tempInFmt,FMTmin,FMTmax,timeout,"In Ticket Fmt")awtx.display.writeLine(label)if isEnterKey then
truck.InFmt=tempInFmt
else
end
truck.extraStuffStore()end
function truck.editInHeaderFmt(label)local tempInHeaderFmt,FMTmin,FMTmax,isEnterKey
truck.extraStuffRecall()tempInHeaderFmt=truck.InHeaderFmt
FMTmin=1
FMTmax=40
tempInHeaderFmt,isEnterKey=awtx.keypad.enterInteger(tempInHeaderFmt,FMTmin,FMTmax,timeout,"In Header Fmt")awtx.display.writeLine(label)if isEnterKey then
truck.InHeaderFmt=tempInHeaderFmt
else
end
truck.extraStuffStore()end
function truck.editInBodyFmt(label)local tempInBodyFmt,FMTmin,FMTmax,isEnterKey
truck.extraStuffRecall()tempInBodyFmt=truck.InBodyFmt
FMTmin=1
FMTmax=40
tempInBodyFmt,isEnterKey=awtx.keypad.enterInteger(tempInBodyFmt,FMTmin,FMTmax,timeout,"In Body Fmt")awtx.display.writeLine(label)if isEnterKey then
truck.InBodyFmt=tempInBodyFmt
else
end
truck.extraStuffStore()end
function truck.editInFooterFmt(label)local tempInFooterFmt,FMTmin,FMTmax,isEnterKey
truck.extraStuffRecall()tempInFooterFmt=truck.InFooterFmt
FMTmin=1
FMTmax=40
tempInFooterFmt,isEnterKey=awtx.keypad.enterInteger(tempInFooterFmt,FMTmin,FMTmax,timeout,"In Footer Fmt")awtx.display.writeLine(label)if isEnterKey then
truck.InFooterFmt=tempInFooterFmt
else
end
truck.extraStuffStore()end
function truck.editOutFmt(label)local tempOutFmt,FMTmin,FMTmax,isEnterKey
truck.extraStuffRecall()tempOutFmt=truck.OutFmt
FMTmin=1
FMTmax=40
tempOutFmt,isEnterKey=awtx.keypad.enterInteger(tempOutFmt,FMTmin,FMTmax,timeout,"Out Ticket Fmt")awtx.display.writeLine(label)if isEnterKey then
truck.OutFmt=tempOutFmt
else
end
truck.extraStuffStore()end
function truck.editOutHeaderFmt(label)local tempOutHeaderFmt,FMTmin,FMTmax,isEnterKey
truck.extraStuffRecall()tempOutHeaderFmt=truck.OutHeaderFmt
FMTmin=1
FMTmax=40
tempOutHeaderFmt,isEnterKey=awtx.keypad.enterInteger(tempOutHeaderFmt,FMTmin,FMTmax,timeout,"Out Header Fmt")awtx.display.writeLine(label)if isEnterKey then
truck.OutHeaderFmt=tempOutHeaderFmt
else
end
truck.extraStuffStore()end
function truck.editOutBodyFmt(label)local tempOutBodyFmt,FMTmin,FMTmax,isEnterKey
truck.extraStuffRecall()tempOutBodyFmt=truck.OutBodyFmt
FMTmin=1
FMTmax=40
tempOutBodyFmt,isEnterKey=awtx.keypad.enterInteger(tempOutBodyFmt,FMTmin,FMTmax,timeout,"Out Body Fmt")awtx.display.writeLine(label)if isEnterKey then
truck.OutBodyFmt=tempOutBodyFmt
else
end
truck.extraStuffStore()end
function truck.editOutFooterFmt(label)local tempOutFooterFmt,FMTmin,FMTmax,isEnterKey
truck.extraStuffRecall()tempOutFooterFmt=truck.OutFooterFmt
FMTmin=1
FMTmax=40
tempOutFooterFmt,isEnterKey=awtx.keypad.enterInteger(tempOutFooterFmt,FMTmin,FMTmax,timeout,"Out Footer Fmt")awtx.display.writeLine(label)if isEnterKey then
truck.OutFooterFmt=tempOutFooterFmt
else
end
truck.extraStuffStore()end
function truck.editFleetFmt(label)local tempFleetFmt,FMTmin,FMTmax,isEnterKey
truck.extraStuffRecall()tempFleetFmt=truck.FleetFmt
FMTmin=1
FMTmax=40
tempFleetFmt,isEnterKey=awtx.keypad.enterInteger(tempFleetFmt,FMTmin,FMTmax,timeout,"Fleet Ticket Fmt")awtx.display.writeLine(label)if isEnterKey then
truck.FleetFmt=tempFleetFmt
else
end
truck.extraStuffStore()end
function truck.editFleetHeaderFmt(label)local tempFleetHeaderFmt,FMTmin,FMTmax,isEnterKey
truck.extraStuffRecall()tempFleetHeaderFmt=truck.FleetHeaderFmt
FMTmin=1
FMTmax=40
tempFleetHeaderFmt,isEnterKey=awtx.keypad.enterInteger(tempFleetHeaderFmt,FMTmin,FMTmax,timeout,"Fleet Header Fmt")awtx.display.writeLine(label)if isEnterKey then
truck.FleetHeaderFmt=tempFleetHeaderFmt
else
end
truck.extraStuffStore()end
function truck.editFleetBodyFmt(label)local tempFleetBodyFmt,FMTmin,FMTmax,isEnterKey
truck.extraStuffRecall()tempFleetBodyFmt=truck.FleetBodyFmt
FMTmin=1
FMTmax=40
tempFleetBodyFmt,isEnterKey=awtx.keypad.enterInteger(tempFleetBodyFmt,FMTmin,FMTmax,timeout,"Fleet Body Fmt")awtx.display.writeLine(label)if isEnterKey then
truck.FleetBodyFmt=tempFleetBodyFmt
else
end
truck.extraStuffStore()end
function truck.editFleetFooterFmt(label)local tempFleetFooterFmt,FMTmin,FMTmax,isEnterKey
truck.extraStuffRecall()tempFleetFooterFmt=truck.FleetFooterFmt
FMTmin=1
FMTmax=40
tempFleetFooterFmt,isEnterKey=awtx.keypad.enterInteger(tempFleetFooterFmt,FMTmin,FMTmax,timeout,"Fleet Footer Fmt")awtx.display.writeLine(label)if isEnterKey then
truck.FleetFooterFmt=tempFleetFooterFmt
else
end
truck.extraStuffStore()end
IN_REPORT_TYPE=0
OUT_REPORT_TYPE=1
FLT_REPORT_TYPE=2
function truck.TruckDBReport(label,reportType)local usermode,index,isEnterKey,dbFile,result,row,fho,err
local currentRPN
local t={}currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.writeLine(label)usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)newTicketDate=doMyDate()newTicketTime=doMyTime()truck.setTruckReportTokens()if reportType==IN_REPORT_TYPE then
awtx.printer.ReportFmt(truck.InHeaderFmt,1)elseif reportType==OUT_REPORT_TYPE then
awtx.printer.ReportFmt(truck.OutHeaderFmt,1)elseif reportType==FLT_REPORT_TYPE then
awtx.printer.ReportFmt(truck.FleetHeaderFmt,1)end
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")sqlStr=string.format("SELECT truckId, tareFleet, weight1, weight1date, weight1time, grossTotal, netTotal, tareTotal, transCount, units FROM tblGTN")for row in dbFile:rows(sqlStr)do
newtruckId=row[1]newtareFleet=row[2]newweight1=row[3]newweight1date=row[4]newweight1time=row[5]newgrossTotal=row[6]newnetTotal=row[7]newtareTotal=row[8]newtransCount=row[9]newunits=row[10]if reportType==IN_REPORT_TYPE then
if newweight1~=0 and newtareFleet==0 then
truck.setTruckReportTokens()awtx.printer.ReportFmt(truck.InBodyFmt,2)end
elseif reportType==OUT_REPORT_TYPE then
if newweight1==0 and newtareFleet==0 then
truck.setTruckReportTokens()awtx.printer.ReportFmt(truck.OutBodyFmt,2)end
elseif reportType==FLT_REPORT_TYPE then
if newtareFleet~=0 then
truck.setTruckReportTokens()awtx.printer.ReportFmt(truck.FleetBodyFmt,2)end
end
end
dbFile:close()if reportType==IN_REPORT_TYPE then
awtx.printer.ReportFmt(truck.InFooterFmt,3)elseif reportType==OUT_REPORT_TYPE then
awtx.printer.ReportFmt(truck.OutFooterFmt,3)elseif reportType==FLT_REPORT_TYPE then
awtx.printer.ReportFmt(truck.FleetFooterFmt,3)end
awtx.os.sleep(busytime)awtx.display.clrDisplayBusy()awtx.keypad.set_RPN_mode(currentRPN)awtx.display.setMode(usermode)truck.clrNewVars()truck.setTruckPrintTokens()end
function truck.truckInClear()truck.truckDBInClear()truck.clrNewVars()end
function truck.truckIODelete()truck.truckDBIODelete()truck.clrNewVars()end
function truck.truckFleetDelete()truck.truckDBFleetDelete()truck.clrNewVars()end
function truck.truckAllDelete()truck.truckDBAllDelete()truck.clrNewVars()end
function truck.TruckInReset(label)local usermode,index,isEnterKey
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)truck.truckInClear()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
end
end
function truck.TruckIOReset(label)local usermode,index,isEnterKey
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)truck.truckIODelete()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
end
end
function truck.TruckFleetReset(label)local usermode,index,isEnterKey
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)truck.truckFleetDelete()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
end
end
function truck.TruckAllReset(label)local usermode,index,isEnterKey
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)truck.truckAllDelete()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)else
end
end
end
function truck.newSetTruckId(tmpTruckId)local found
if tmpTruckId==0 then
truck.clrNewVars()truck.newSetTruckIdWithoutFind(tmpTruckId)else
found=truck.findTruckId(tmpTruckId)if found==true then
truck.newSetTruckIdWithoutFind(tmpTruckId)else
displayWORD("NOT FND")end
end
end
function truck.editLiteEnable(label)local usermode,index,isEnterKey
truck.extraStuffRecall()index=truck.LiteEnableFlag
index,isEnterKey=awtx.keypad.selectList("Off,Manual,Auto",index)awtx.display.writeLine(label)if isEnterKey then
truck.LiteEnableFlag=index
else
end
truck.extraStuffStore()end
function truck.editLiteTHold(label)local tmpLiteTHold,minVal,maxVal,tempUnitIndex,tempUnitStr,isEnterKey
truck.extraStuffRecall()wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
minVal=0
maxVal=wt.curCapacity
tmpLiteTHold=truck.LiteTHold
tmpLiteTHold,isEnterKey=awtx.keypad.enterWeightWithUnits(tmpLiteTHold,minVal,maxVal,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
truck.LiteTHold=tmpLiteTHold
else
end
truck.extraStuffStore()end
truck.clrNewVars()truck.truckDBInit()truck.extraStuffRecall()IDMIN=0
IDMAX=9999999
function truck.selectTruckIOID()local usermode,currentRPN,tmpTruckId,isEnterKey
local found,curActVal
curActVal=awtx.weight.getActiveValue()if curActVal~=VAL_GROSS then
displayCANT()else
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.writeLine("TruckId")awtx.os.sleep(showtime1)tmpTruckId=newtruckId
tmpTruckId,isEnterKey=awtx.keypad.enterInteger(tmpTruckId,IDMIN,IDMAX,timeout,"TruckId")if isEnterKey then
truck.setIOTruckId(tmpTruckId)else
end
awtx.keypad.set_RPN_mode(currentRPN)awtx.display.setMode(usermode)end
end
function truck.selectTruckFleetID()local usermode,currentRPN,tmpTruckId,isEnterKey
local found,curActVal
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.writeLine("FleetId")awtx.os.sleep(showtime1)tmpTruckId=newtruckId
tmpTruckId,isEnterKey=awtx.keypad.enterInteger(tmpTruckId,IDMIN,IDMAX,timeout,"FleetId")if isEnterKey then
truck.setFleetTruckId(tmpTruckId)else
end
awtx.keypad.set_RPN_mode(currentRPN)awtx.display.setMode(usermode)end
function truck.selectTruckFleetIDNext()local usermode,currentRPN,tmpTruckIndex,tmpTruckId,isEnterKey
local tmpTruckIdList
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)tmpTruckId=newtruckId
return isEnterKey
end
function truck.newSetTruckIdWithoutFind(tmpTruckId)if tmpTruckId==0 then
truck.clrNewVars()else
newtruckId=tmpTruckId
truck.recallTruckId(newtruckId)end
truck.setTruckPrintTokens()end
function truck.setTruckId(tmpTruckId)local found
found=truck.findTruckId(tmpTruckId)if found==true then
truck.newSetTruckIdWithoutFind(tmpTruckId)else
displayCANT()end
end
function truck.setIOTruckId(tmpTruckId)local found,curActVal
curActVal=awtx.weight.getActiveValue()if curActVal~=VAL_GROSS then
displayCANT()else
found=truck.findTruckId(tmpTruckId)if found==false then
menuWORD(" INBND")truck.clrNewVars()newtruckId=tmpTruckId
found=truck.saveTruckId(tmpTruckId)newtruckId=tmpTruckId
truck.recallTruckId(newtruckId)truck.setTruckPrintType(TRUCK_TYPE_INOUT)awtx.weight.requestTareClear()truck.setTruckPrintTokens()else
truck.recallTruckId(tmpTruckId)if newtareFleet~=0 then
menuCANT()menuWORD("FLEETID")truck.clrNewVars()else
newtruckId=tmpTruckId
truck.recallTruckId(newtruckId)truck.setTruckPrintType(TRUCK_TYPE_INOUT)awtx.weight.requestTareClear()truck.setTruckPrintTokens()if newweight1==0 then
menuWORD(" INBND")else
menuWORD("OUTBND")end
end
end
end
end
function truck.setFleetTruckId(tmpTruckId)local found,curActVal
found=truck.findTruckId(tmpTruckId)if found==false then
displayWORD("NOT FND")else
truck.recallTruckId(tmpTruckId)if newtareFleet==0 then
menuCANT()menuWORD(" IO ID ")truck.clrNewVars()else
newtruckId=tmpTruckId
truck.recallTruckId(newtruckId)truck.setTruckPrintType(TRUCK_TYPE_FLEET)awtx.weight.requestPresetTare(newtareFleet)truck.setTruckPrintTokens()menuWORD(" FLEET")end
end
end
MAX_LOOP=100
function truck.waitForMotion()local loop=0
local tmpMotion=false
wt=awtx.weight.getCurrent(1)tmpMotion=wt.motion
loop=0
while wt.motion do
wt=awtx.weight.getCurrent(1)tmpMotion=wt.motion
awtx.os.sleep(50)loop=loop+1
if loop>MAX_LOOP then
tmpMotion=true
break
end
end
return tmpMotion,wt.gross
end
function truck.printTruckIO()local tmpMotion,tmpWeight
local tmpTare,tmpNet,tmpGross,tmpTimestamp
tmpMotion=wt.motion
tmpWeight=wt.gross
if tmpWeight>0 then
if not wt.inGrossBand then
displayWORD("Printng")if newweight1==0 then
newTruckGross=tmpWeight
newTruckNet=0
newTruckTare=0
newunits=wt.unitsStr
tmpTimestamp=wt.timeStamp
newTicketDate=doTimestampDate(tmpTimestamp)newTicketTime=doTimestampTime(tmpTimestamp)truck.setTruckPrintTokens()awtx.printer.PrintFmt(truck.InFmt)reprintFmt=truck.InFmt
newweight1=tmpWeight
newweight1date=newTicketDate
newweight1time=newTicketTime
truck.saveTruckId(newtruckId)else
if tmpWeight<newweight1 then
tmpTare=tmpWeight
tmpGross=newweight1
else
tmpTare=newweight1
tmpGross=tmpWeight
end
tmpNet=tmpGross-tmpTare
newgrossTotal=newgrossTotal+tmpGross
newnetTotal=newnetTotal+tmpNet
newtareTotal=newtareTotal+tmpTare
newtransCount=newtransCount+1
newTruckGross=tmpGross
newTruckTare=tmpTare
newTruckNet=tmpNet
newunits=wt.unitsStr
tmpTimestamp=wt.timeStamp
newTicketDate=doTimestampDate(tmpTimestamp)newTicketTime=doTimestampTime(tmpTimestamp)truck.setTruckPrintTokens()awtx.printer.PrintFmt(truck.OutFmt)reprintFmt=truck.OutFmt
newweight1=0
newweight1date=" "newweight1time=" "truck.saveTruckId(newtruckId)end
truck.setTruckPrintType(TRUCK_TYPE_NONE)awtx.weight.requestTareClear()else
displayWORD(" No Wt ")end
else
displayWORD("Neg Wt ")end
end
function truck.printTruckFleet()local tmpMotion,tmpWeight
local tmpTare,tmpNet,tmpGross
tmpMotion=wt.motion
tmpWeight=wt.gross
if tmpWeight>0 then
if not wt.inGrossBand then
if newtareFleet==0 then
displayWORD("No Tare")else
if tmpWeight>(newtareFleet+(config.grossZeroBand*wt.curDivision))then
displayWORD("Printng")tmpTare=newtareFleet
tmpGross=tmpWeight
tmpNet=tmpGross-tmpTare
newgrossTotal=newgrossTotal+tmpGross
newnetTotal=newnetTotal+tmpNet
newtareTotal=newtareTotal+tmpTare
newtransCount=newtransCount+1
newTruckGross=tmpGross
newTruckTare=tmpTare
newTruckNet=tmpNet
newunits=wt.unitsStr
tmpTimestamp=wt.timeStamp
newTicketDate=doTimestampDate(tmpTimestamp)newTicketTime=doTimestampTime(tmpTimestamp)truck.setTruckPrintTokens()awtx.printer.PrintFmt(truck.FleetFmt)reprintFmt=truck.FleetFmt
truck.saveTruckId(newtruckId)else
displayWORD("LOW Wt ")end
end
truck.setTruckPrintType(TRUCK_TYPE_NONE)awtx.weight.requestTareClear()else
displayWORD(" No Wt ")end
else
displayWORD("Neg Wt ")end
end
function truck.TruckReport(label)local usermode,currentRPN,tmpreport,tmpreport1,isEnterKey
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)tmpreport,isEnterKey=awtx.keypad.selectList(reportLabel,0)if isEnterKey then
if tmpreport==0 then
displayWORD("Printng")awtx.printer.PrintFmt(reprintFmt)awtx.printer.PrintFmt(40)else
tmpreport1=tmpreport-1
truck.TruckDBReport(reportLabelList[tmpreport],tmpreport1)end
end
awtx.keypad.set_RPN_mode(currentRPN)awtx.display.setMode(usermode)end
function truck.TruckMenuReport(label)local usermode,currentRPN,tmpreport,isEnterKey
if HiResDB then
tmpCurunit=awtx.weight.getCurrentUnits()awtx.weight.setCurrentUnits(awtx.weight.unitStrToUnitIndex(dbunits))wt=awtx.weight.getCurrent(1)end
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.writeLine(label)awtx.os.sleep(showtime1)tmpreport,isEnterKey=awtx.keypad.selectList(menuprintLabel,0)awtx.display.writeLine(menuprintLabelList[tmpreport])awtx.os.sleep(showtime1)if isEnterKey then
truck.TruckDBReport(menuprintLabelList[tmpreport],tmpreport)end
awtx.keypad.set_RPN_mode(currentRPN)awtx.display.setMode(usermode)if HiResDB then
awtx.weight.setCurrentUnits(tmpCurunit)wt=awtx.weight.getCurrent(1)end
end
function truck.editTruckInTruckId(label)local tmpTruckId,isEnterKey
local found
supervisor.menuing=false
tmpTruckId=0
tmpTruckId,isEnterKey=awtx.keypad.enterInteger(tmpTruckId,IDMIN,IDMAX,timeout,"In TruckId")if isEnterKey then
found=truck.findTruckId(tmpTruckId)if found==false then
menuWORD("NOT FND")supervisor.menuLevel=TruckSetupEditMenu
supervisor.menuCurrent=1
else
newtruckId=tmpTruckId
truck.recallTruckId(newtruckId)if newtareFleet~=0 then
menuCANT()menuWORD("FLEETID")supervisor.menuLevel=TruckSetupEditMenu
supervisor.menuCurrent=1
else
supervisor.menuLevel=TruckEditInMenu
supervisor.menuCurrent=1
end
end
else
supervisor.menuLevel=TruckSetupEditMenu
supervisor.menuCurrent=1
end
supervisor.menuing=true
end
function truck.editTruckIOTruckId(label)local tmpTruckId,isEnterKey
local found
supervisor.menuing=false
tmpTruckId=0
tmpTruckId,isEnterKey=awtx.keypad.enterInteger(tmpTruckId,IDMIN,IDMAX,timeout,"IO TruckId")if isEnterKey then
found=truck.findTruckId(tmpTruckId)if found==false then
menuWORD("NOT FND")truck.clrNewVars()supervisor.menuLevel=TruckSetupEditMenu
supervisor.menuCurrent=2
else
newtruckId=tmpTruckId
truck.recallTruckId(newtruckId)if newtareFleet~=0 then
menuCANT()menuWORD("FLEETID")supervisor.menuLevel=TruckSetupEditMenu
supervisor.menuCurrent=2
else
supervisor.menuLevel=TruckEditIOMenu
supervisor.menuCurrent=1
end
end
else
supervisor.menuLevel=TruckSetupEditMenu
supervisor.menuCurrent=2
end
supervisor.menuing=true
end
function truck.editTruckFleetTruckId(label)local tmpTruckId,isEnterKey
local found
supervisor.menuing=false
tmpTruckId=0
tmpTruckId,isEnterKey=awtx.keypad.enterInteger(tmpTruckId,IDMIN,IDMAX,timeout,"Fleet TruckId")if isEnterKey then
found=truck.findTruckId(tmpTruckId)if found==false then
menuWORD("NOT FND")menuWORD("ADDING")truck.clrNewVars()newtruckId=tmpTruckId
supervisor.menuLevel=TruckEditFleetMenu
supervisor.menuCurrent=1
else
newtruckId=tmpTruckId
truck.recallTruckId(newtruckId)if newtareFleet==0 then
menuCANT()menuWORD(" IO ID ")supervisor.menuLevel=TruckSetupEditMenu
supervisor.menuCurrent=3
else
supervisor.menuLevel=TruckEditFleetMenu
supervisor.menuCurrent=1
end
end
else
supervisor.menuLevel=TruckSetupEditMenu
supervisor.menuCurrent=3
end
supervisor.menuing=true
end
function truck.editTruckTareFleet(label)local tmptareFleet,minVal,maxVal,tempUnitIndex,tempUnitStr,isEnterKey
local tmpMotion,tmpWeight
wt=awtx.weight.getCurrent(1)tempUnitIndex=wt.units
tempUnitStr=wt.unitsStr
if wt.inGrossBand then
tmptareFleet=newtareFleet
else
tmpMotion,tmpWeight=truck.waitForMotion()if not tmpMotion then
tmptareFleet=tmpWeight
else
tmptareFleet=newtareFleet
end
end
minVal=0
maxVal=wt.curCapacity
tmptareFleet,isEnterKey=awtx.keypad.enterWeightWithUnits(tmptareFleet,minVal,maxVal,tempUnitStr,separatorChar)awtx.display.writeLine(label)if isEnterKey then
newtareFleet=tmptareFleet
found=truck.saveTruckId(newtruckId)else
end
return isEnterKey
end
function truck.clrTruckInWeight(label)local tmpTruckId,isEnterKey
local found
supervisor.menuing=false
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)truck.clearInTruckId(newtruckId)truck.clrNewVars()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)supervisor.menuLevel=TruckSetupEditMenu
supervisor.menuCurrent=1
else
supervisor.menuLevel=TruckEditInMenu
supervisor.menuCurrent=1
end
else
supervisor.menuLevel=TruckEditInMenu
supervisor.menuCurrent=1
end
supervisor.menuing=true
end
function truck.delTruckIdIO(label)supervisor.menuing=false
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)truck.deleteIOTruckId(newtruckId)truck.clrNewVars()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)supervisor.menuLevel=TruckSetupEditMenu
supervisor.menuCurrent=2
else
supervisor.menuLevel=TruckEditIOMenu
supervisor.menuCurrent=1
end
else
supervisor.menuLevel=TruckEditIOMenu
supervisor.menuCurrent=1
end
supervisor.menuing=true
end
function truck.delTruckIdFleet(label)supervisor.menuing=false
index,isEnterKey=awtx.keypad.selectList("No,Yes",0)awtx.display.writeLine(label)if isEnterKey then
if index==1 then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)truck.deleteFleetTruckId(newtruckId)truck.clrNewVars()awtx.display.clrDisplayBusy()awtx.display.setMode(usermode)supervisor.menuLevel=TruckSetupEditMenu
supervisor.menuCurrent=3
else
supervisor.menuLevel=TruckEditFleetMenu
supervisor.menuCurrent=2
end
else
supervisor.menuLevel=TruckEditFleetMenu
supervisor.menuCurrent=2
end
supervisor.menuing=true
end
function truck.setValuesAfterUnitChange()newtruckId=0
newtareFleet=0
newweight1=0
newweight1date=" "newweight1time=" "newgrossTotal=0
newnetTotal=0
newtareTotal=0
newtransCount=0
newunits=wt.unitsStr
dbunits=dbunitsDefault
newTruckGross=0
newTruckNet=0
newTruckTare=0
newTicketDate=" "newTicketTime=" "truck.setTruckPrintType(TRUCK_TYPE_NONE)awtx.weight.requestTareClear()end
function truck.convertUnitofMeasure()local dbWtUnit,newWtUnit,errorState1,errorState2
newunits=wt.unitsStr
if dbunits==nil then
dbunits=dbunitsDefault
end
errorState1,dbWtUnit=awtx.weight.unitStrToUnitIndex(dbunits)errorState2,newWtUnit=awtx.weight.unitStrToUnitIndex(newunits)if errorState1==0 and errorState2==0 then
if HiResDB then
newtareFleet=awtx.weight.convertWeight(newWtUnit,newtareFleet,dbWtUnit,0)else
newtareFleet=awtx.weight.convertWeight(newWtUnit,newtareFleet,dbWtUnit,1)end
else
newtareFleet=newtareFleet
end
end
function truck.importGTN(label)local errorCode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.writeLine("Busy")errorCode=awtx.os.convertCsvFileToTable_LUA(DB_FileLocation_AppData,"tblGTN",[[G:\GTN_CSV\gtn.csv]])if errorCode==0 or errorCode==nil then
awtx.display.writeLine("Done")else
awtx.display.writeLine(string.format("Err %2d",errorCode))end
awtx.os.sleep(1000)awtx.keypad.set_RPN_mode(currentRPN)end
function truck.importGTNLUA(label)local errorCode,currentRPN
tmpCurunit=awtx.weight.getCurrentUnits()awtx.weight.setCurrentUnits(awtx.weight.unitStrToUnitIndex(dbunits))wt=awtx.weight.getCurrent(1)currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.writeLine("Busy")errorCode=truck.importSQL(DB_FileLocation_AppData,"tblGTN",[[G:\GTN_CSV\gtn.csv]])if errorCode==0 or errorCode==nil then
awtx.display.writeLine("Done")else
end
awtx.os.sleep(1000)awtx.keypad.set_RPN_mode(currentRPN)awtx.weight.setCurrentUnits(tmpCurunit)wt=awtx.weight.getCurrent(1)end
function truck.exportGTN(label)local errorCode,currentRPN
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.writeLine("Busy")errorCode=awtx.os.convertTableToCsvFile_LUA(DB_FileLocation_AppData,"tblGTN","gtn.csv",[[G:\GTN_CSV]])if errorCode==0 or errorCode==nil then
awtx.display.writeLine("Done")else
awtx.display.writeLine(string.format("Err %2d",errorCode))end
awtx.os.sleep(1000)awtx.keypad.set_RPN_mode(currentRPN)end
function truck.exportGTNLUA(label)local errorCode,currentRPN
tmpCurunit=awtx.weight.getCurrentUnits()awtx.weight.setCurrentUnits(awtx.weight.unitStrToUnitIndex(dbunits))wt=awtx.weight.getCurrent(1)currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.writeLine("Busy")errorCode=truck.exportSQL(DB_FileLocation_AppData,"tblGTN","gtn.csv",[[G:\GTN_CSV]])if errorCode==0 or errorCode==nil then
awtx.display.writeLine("Done")else
awtx.display.writeLine(string.format("Err %2d",errorCode))end
awtx.os.sleep(1000)awtx.keypad.set_RPN_mode(currentRPN)awtx.weight.setCurrentUnits(tmpCurunit)wt=awtx.weight.getCurrent(1)end
imptruckId=0
imptareFleet=0
impweight1=0
impweight1date=" "impweight1time=" "impgrossTotal=0
impnetTotal=0
imptareTotal=0
imptransCount=0
impunits=dbunitsDefault
impWtUnit=0
function truck.saveImportTruckId(tmpId)local dbFile,result
local found=false
local sqlStr,searchIndex,sqlStr
local dbWtUnit,newWtUnit,errorState1,errorState2
dbFile=sqlite3.open(DB_FileLocation_AppData)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")dbFile:execute("BEGIN TRANSACTION")searchId=tmpId
if searchId~=0 then
sqlStr=string.format("SELECT truckId FROM tblGTN WHERE tblGTN.truckId = %d",searchId)found=false
for row in dbFile:rows(sqlStr)do
found=true
end
if found==false then
sqlStr=string.format("INSERT INTO tblGTN (truckId, tareFleet, weight1, weight1date, weight1time, grossTotal, netTotal, tareTotal, transCount, units) VALUES ('%d', '%f', '%f', '%s', '%s', '%f', '%f', '%f', '%d', '%s')",imptruckId,imptareFleet,impweight1,impweight1date,impweight1time,impgrossTotal,impnetTotal,imptareTotal,imptransCount,impunits)result=dbFile:exec(sqlStr)else
sqlStr=string.format("UPDATE tblGTN SET truckId = '%d', tareFleet = '%f', weight1 = '%f', weight1date = '%s', weight1time = '%s', grossTotal = '%f', netTotal = '%f', tareTotal = '%f', transCount = '%d', units = '%s' WHERE truckId = %d",imptruckId,imptareFleet,impweight1,impweight1date,impweight1time,impgrossTotal,impnetTotal,imptareTotal,imptransCount,impunits,searchId)result=dbFile:exec(sqlStr)end
end
dbFile:execute("COMMIT")dbFile:close()end
function truck.importSQL(tmpAppData,tmpTable,tmpDestFile)local errorCode
local file
local numRecords=0
local curRecord={}local index
local isEnterKey
file,errorCode=io.open(tmpDestFile,"r")if errorCode==nil then
local nextCommaIndex=-1
truck.truckAllDelete()for line in file:lines()do
curRecord={}if numRecords>=MAX_TRUCK_INDEX then
numRecords=numRecords+1
break
end
numRecords=numRecords+1
if separatorChar==0 then
elseif separatorChar==1 then
line=string.gsub(line,"%,","%.")line=string.gsub(line,"%;","%,")end
line=string.gsub(line,'\n',',')local startingIndex=0
nextCommaIndex=string.find(line,',',startingIndex)if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)imptruckId=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)imptareFleet=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impweight1=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impweight1date=tostring(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impweight1time=tostring(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impgrossTotal=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impnetTotal=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)imptareTotal=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)imptransCount=tonumber(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=string.find(line,',',startingIndex)end
if nextCommaIndex then
local currentInfo=string.sub(line,startingIndex,nextCommaIndex-1)impunits=tostring(currentInfo)startingIndex=nextCommaIndex+1
nextCommaIndex=0
end
if nextCommaIndex~=0 then
break
end
if numRecords==1 then
else
truck.saveImportTruckId(imptruckId)end
end
file:close()errorCode=0
if nextCommaIndex~=0 then
awtx.display.writeLine(" SQL",2000)awtx.display.writeLine("ParseFail",2000)errorCode=-1
elseif numRecords>MAX_TRUCK_INDEX then
awtx.display.writeLine("SQL FUL",500)errorCode=-2
end
else
awtx.display.writeLine(" SQL",2000)awtx.display.writeLine("ReadFail",2000)errorCode=-3
end
return errorCode
end
exptruckId=0
exptareFleet=0
expweight1=0
expweight1date=" "expweight1time=" "expgrossTotal=0
expnetTotal=0
exptareTotal=0
exptransCount=0
expunits=dbunitsDefault
expWtUnit=0
function truck.exportSQL(tmpAppData,tmpTable,tmpDestFile,tmpDestFolder)local errorCode,errorState1
local errorCode1,usbfile
local errorCode2,dbFile
local result=0
result=awtx.os.makeDirectory(tmpDestFolder)if result~=0 then
return result
end
usbfile,errorCode1=io.open(tmpDestFolder..[[\]]..tmpDestFile,"w")dbFile,errorCode2=sqlite3.open(tmpAppData)if errorCode1==nil and errorCode2==nil then
local outputString='"truckId","tareFleet","weight1","weight1date","weight1time","grossTotal","netTotal","tareTotal","transCount","units"\n'if separatorChar==0 then
elseif separatorChar==1 then
outputString=string.gsub(outputString,"%,","%;")outputString=string.gsub(outputString,"%.","%,")end
usbfile:write(outputString)result=dbFile:exec("CREATE TABLE IF NOT EXISTS tblGTN (truckId INTEGER, tareFleet DOUBLE, weight1 DOUBLE, weight1date VARCHAR, weight1time VARCHAR, grossTotal DOUBLE, netTotal DOUBLE, tareTotal DOUBLE, transCount INTEGER, units VARCHAR)")sqlStr=string.format("SELECT truckId, tareFleet, weight1, weight1date, weight1time, grossTotal, netTotal, tareTotal, transCount, units FROM tblGTN")for row in dbFile:rows(sqlStr)do
expunits=row[10]errorState1,expWtUnit=awtx.weight.unitStrToUnitIndex(expunits)if errorState1==0 then
exptruckId=row[1]exptareFleet=awtx.weight.convertWeight(expWtUnit,row[2],expWtUnit,1)expweight1=awtx.weight.convertWeight(expWtUnit,row[3],expWtUnit,1)expweight1date=row[4]expweight1time=row[5]expgrossTotal=awtx.weight.convertWeight(expWtUnit,row[6],expWtUnit,1)expnetTotal=awtx.weight.convertWeight(expWtUnit,row[7],expWtUnit,1)exptareTotal=awtx.weight.convertWeight(expWtUnit,row[8],expWtUnit,1)exptransCount=row[9]widthprec=wt.curDigitsTotal.."."..wt.curDigitsRight
formatStr=string.gsub("%s,%***f","***",widthprec)outputString=""outputString=string.format("%d",exptruckId)outputString=string.format(formatStr,outputString,exptareFleet)outputString=string.format(formatStr,outputString,expweight1)outputString=string.format("%s,%s",outputString,expweight1date)outputString=string.format("%s,%s",outputString,expweight1time)outputString=string.format(formatStr,outputString,expgrossTotal)outputString=string.format(formatStr,outputString,expnetTotal)outputString=string.format(formatStr,outputString,exptareTotal)outputString=string.format("%s,%d",outputString,exptransCount)outputString=string.format("%s,%s\n",outputString,expunits)if separatorChar==0 then
elseif separatorChar==1 then
outputString=string.gsub(outputString,"%,","%;")outputString=string.gsub(outputString,"%.","%,")end
usbfile:write(outputString)end
end
dbFile:close()usbfile:close()errorCode=0
else
if errorCode2==nil then
errorCode=errorCode1
elseif errorCode2==nil then
errorCode=errorCode2
else
errorCode=errorCode1+errorCode2
end
end
return errorCode
end
local rpnstartflag=false
local rpnstopflag=false
local rpnsetupflag=false
local rpnzeroflag=false
local rpnunitflag=false
local rpnselectflag=true
local rpnoverflag=false
local rpnunderflag=false
local newID=0
local newFmt=0
local newTare=0
local newSelect=0
function RPNCOMPLETEEVENT(lastKey,enteredValue)local usermode
local found,curActVal
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
elseif lastKey=="START"then
if rpnstartflag==true then
else
displayCANT()end
elseif lastKey=="STOP"then
if rpnstopflag==true then
else
displayCANT()end
elseif lastKey=="F1"then
if rpnf1flag==true then
tmpTruckId=enteredValue
truck.setIOTruckId(tmpTruckId)else
displayCANT()end
elseif lastKey=="SETUP"then
if rpnsetupflag==true then
else
displayCANT()end
elseif lastKey=="TARGET"then
if rpntargetflag==true then
else
displayCANT()end
elseif lastKey=="ZERO"then
if rpnzeroflag==true then
else
displayCANT()end
elseif lastKey=="UNIT"then
if rpnunitflag==true then
else
displayCANT()end
elseif lastKey=="SAMPLE"then
if rpnsampleflag==true then
tmpTruckId=enteredValue
truck.setFleetTruckId(tmpTruckId)else
displayCANT()end
elseif lastKey=="OVER"then
if rpnoverflag==true then
else
displayCANT()end
elseif lastKey=="UNDER"then
if rpnunderflag==true then
else
displayCANT()end
else
if rpnotherflag==true then
else
displayCANT()end
end
end
awtx.keypad.registerNumberEntryRPN(RPNCOMPLETEEVENT)function truck.setLiteOff()truck.XR4500=XR4500_OFF
truck.setXR4500PrintToken()if truck.LiteEnableFlag==1 then
result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(2)end
end
function truck.setLiteGreen()truck.XR4500=XR4500_GRN
truck.setXR4500PrintToken()if truck.LiteEnableFlag==1 then
result=awtx.setPoint.outputSet(1)result=awtx.setPoint.outputClr(2)elseif truck.LiteEnableFlag==2 then
result=awtx.setPoint.outputSet(5)end
end
function truck.setLiteRed()truck.XR4500=XR4500_RED
truck.setXR4500PrintToken()if truck.LiteEnableFlag==1 then
result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputSet(2)elseif truck.LiteEnableFlag==2 then
result=awtx.setPoint.outputClr(5)end
end
function truck.setpointOut1Handler(setpointNum,isActivate)if isActivate then
else
end
end
function truck.setpointOut2Handler(setpointNum,isActivate)if isActivate then
else
end
end
function truck.LiteAutomaticOut1Handler(setpointNum,isActivate)if isActivate then
truck.setLiteGreen()else
end
end
function truck.LiteAutomaticOut2Handler(setpointNum,isActivate)if isActivate then
truck.setLiteRed()else
end
end
function truck.LiteAutomaticOut3Handler(setpointNum,isActivate)if isActivate then
else
end
end
function truck.LiteAutomaticOut4Handler(setpointNum,isActivate)if isActivate then
else
end
end
function truck.LiteAutomaticOut5Handler(setpointNum,isActivate)if isActivate then
else
end
end
function truck.LiteAutomaticOut6Handler(setpointNum,isActivate)if isActivate then
else
end
end
function truck.LiteAutomaticOut7Handler(setpointNum,isActivate)if isActivate then
else
end
end
function truck.LiteAutomaticOut8Handler(setpointNum,isActivate)if isActivate then
else
end
end
function truck.LiteAutomaticOut9Handler(setpointNum,isActivate)if isActivate then
else
end
end
function truck.LiteAutomaticOut10Handler(setpointNum,isActivate)if isActivate then
else
end
end
local minLatchTime=500
function truck.disableLiteOutputs()local setPointDisabled,setPointBattery
local retVal,resultMsg,result
setPointDisabled={mode="disabled"}setPointBattery={mode="output",bounceTime=minLatchTime,offInsideGZB=false,act="user",deact="user"}retVal,resultMsg=awtx.setPoint.set(1,setPointDisabled)retVal,resultMsg=awtx.setPoint.set(2,setPointDisabled)if battery.BatteryEnable==1 then
retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)else
retVal,resultMsg=awtx.setPoint.set(3,setPointDisabled)end
result=awtx.setPoint.unregisterOutputEvent(1)result=awtx.setPoint.unregisterOutputEvent(2)result=awtx.setPoint.unregisterOutputEvent(3)result=awtx.setPoint.outputClr(1)result=awtx.setPoint.outputClr(2)result=awtx.setPoint.outputClr(3)end
function truck.refreshLiteOutputs(cfg)local retVal,resultMsg,result
local setpt1AboveBelow,setpt1BelowAbove,setpt1InsideOutside,setpt1OutsideInside
local setpt2AboveBelow,setpt2BelowAbove,setpt2InsideOutside,setpt2OutsideInside
local setpt3AboveBelow,setpt3BelowAbove,setpt3InsideOutside,setpt3OutsideInside
local setPointBattery,setPointOutEx4,setPointOutEx5,setPointOutEx6,setPointOutEx7,setPointOutEx8,setPointOutEx9,setPointOutEx10
local setPointUnder,setPointTarget,setPointOver,setPointReject,setPointAccept
local LiteAutomaticOutEx1,LiteAutomaticOutEx2,LiteAutomaticOutEx3,LiteAutomaticOutEx4,LiteAutomaticOutEx5,LiteAutomaticOutEx6,LiteAutomaticOutEx7,LiteAutomaticOutEx8,LiteAutomaticOutEx9,LiteAutomaticOutEx10
local offInsideGZBFlag=false
setpt1AboveBelow={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="above",actLowerVarName="UserOutput1",actBasis=setpoint.basis,actMotionInhibit=false,deact="below",deactLowerVarName="UserOutput1",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt1BelowAbove={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="below",actLowerVarName="UserOutput1",actBasis=setpoint.basis,actMotionInhibit=false,deact="above",deactLowerVarName="UserOutput1",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt1InsideOutside={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="UserOutput1Lo",actUpperVarName="UserOutput1Hi",actBasis=setpoint.basis,actMotionInhibit=false,deact="outside",deactLowerVarName="UserOutput1Lo",deactUpperVarName="UserOutput1Hi",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt1OutsideInside={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="outside",actLowerVarName="UserOutput1Lo",actUpperVarName="UserOutput1Hi",actBasis=setpoint.basis,actMotionInhibit=false,deact="inside",deactLowerVarName="UserOutput1Lo",deactUpperVarName="UserOutput1Hi",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt1user={mode="output",bounceTime=minLatchTime,offInsideGZB=false,act="user",deact="user"}setpt2AboveBelow={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="above",actLowerVarName="UserOutput2",actBasis=setpoint.basis,actMotionInhibit=false,deact="below",deactLowerVarName="UserOutput2",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt2BelowAbove={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="below",actLowerVarName="UserOutput2",actBasis=setpoint.basis,actMotionInhibit=false,deact="above",deactLowerVarName="UserOutput2",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt2InsideOutside={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="UserOutput2Lo",actUpperVarName="UserOutput2Hi",actBasis=setpoint.basis,actMotionInhibit=false,deact="outside",deactLowerVarName="UserOutput2Lo",deactUpperVarName="UserOutput2Hi",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt2OutsideInside={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="outside",actLowerVarName="UserOutput2Lo",actUpperVarName="UserOutput2Hi",actBasis=setpoint.basis,actMotionInhibit=false,deact="inside",deactLowerVarName="UserOutput2Lo",deactUpperVarName="UserOutput2Hi",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt2user={mode="output",bounceTime=minLatchTime,offInsideGZB=false,act="user",deact="user"}setpt3AboveBelow={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="above",actLowerVarName="UserOutput3",actBasis=setpoint.basis,actMotionInhibit=false,deact="below",deactLowerVarName="UserOutput3",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt3BelowAbove={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="below",actLowerVarName="UserOutput3",actBasis=setpoint.basis,actMotionInhibit=false,deact="above",deactLowerVarName="UserOutput3",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt3InsideOutside={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="UserOutput3Lo",actUpperVarName="UserOutput3Hi",actBasis=setpoint.basis,actMotionInhibit=false,deact="outside",deactLowerVarName="UserOutput3Lo",deactUpperVarName="UserOutput3Hi",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt3OutsideInside={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="outside",actLowerVarName="UserOutput3Lo",actUpperVarName="UserOutput3Hi",actBasis=setpoint.basis,actMotionInhibit=false,deact="inside",deactLowerVarName="UserOutput3Lo",deactUpperVarName="UserOutput3Hi",deactBasis=setpoint.basis,deactMotionInhibit=false}setpt3user={mode="output",bounceTime=minLatchTime,offInsideGZB=false,act="user",deact="user"}setPointBattery={mode="output",bounceTime=minLatchTime,offInsideGZB=false,act="user",deact="user"}setPointUnder={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="setpoint.TargMin",actUpperVarName="setpoint.TargLo",actBasis=setpoint.TargBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="setpoint.TargMin",deactUpperVarName="setpoint.TargLo",deactBasis=setpoint.TargBasis,deactMotionInhibit=false}setPointTarget={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="setpoint.TargLo",actUpperVarName="setpoint.TargHi",actBasis=setpoint.TargBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="setpoint.TargLo",deactUpperVarName="setpoint.TargHi",deactBasis=setpoint.TargBasis,deactMotionInhibit=false}setPointOver={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="setpoint.TargHi",actUpperVarName="setpoint.TargMax",actBasis=setpoint.TargBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="setpoint.TargHi",deactUpperVarName="setpoint.TargMax",deactBasis=setpoint.TargBasis,deactMotionInhibit=false}setPointReject={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="outside",actLowerVarName="setpoint.TargLo",actUpperVarName="setpoint.TargHi",actBasis=setpoint.TargBasis,actMotionInhibit=false,deact="inside",deactLowerVarName="setpoint.TargLo",deactUpperVarName="setpoint.TargHi",deactBasis=setpoint.TargBasis,deactMotionInhibit=false}setPointAccept={mode="output",bounceTime=minLatchTime,offInsideGZB=offInsideGZBFlag,act="inside",actLowerVarName="setpoint.TargLo",actUpperVarName="setpoint.TargHi",actBasis=setpoint.TargBasis,actMotionInhibit=false,deact="outside",deactLowerVarName="setpoint.TargLo",deactUpperVarName="setpoint.TargHi",deactBasis=setpoint.TargBasis,deactMotionInhibit=false}setPointOutEx4={mode="disabled"}setPointOutEx5={mode="disabled"}setPointOutEx6={mode="disabled"}setPointOutEx7={mode="disabled"}setPointOutEx8={mode="disabled"}setPointOutEx9={mode="disabled"}setPointOutEx10={mode="disabled"}LiteAutomaticOutEx1={mode="output",bounceTime=0,offInsideGZB=false,act="OR",actLower=-4,actUpper=5,deact="AND",deactLower=4,deactUpper=-5}LiteAutomaticOutEx2={mode="output",bounceTime=0,offInsideGZB=false,act="AND",actLower=-1,actUpper=-1,deact="AND",deactLower=1,deactUpper=1}LiteAutomaticOutEx3={mode="disabled"}LiteAutomaticOutEx4={mode="output",bounceTime=0,offInsideGZB=false,act="above",actLowerVarName="THold",actBasis="grossWt",actMotionInhibit=false,deact="below",deactLowerVarName="THold",deactBasis="grossWt",deactMotionInhibit=false}LiteAutomaticOutEx5={mode="output",bounceTime=0,offInsideGZB=false,act="user",deact="inside",deactLowerVarName="negGZB",deactUpperVarName="posGZB",deactBasis="grossWt",deactMotionInhibit=true}LiteAutomaticOutEx6={mode="disabled"}LiteAutomaticOutEx7={mode="disabled"}LiteAutomaticOutEx8={mode="disabled"}LiteAutomaticOutEx9={mode="disabled"}LiteAutomaticOutEx10={mode="disabled"}if truck.LiteEnableFlag==0 then
elseif truck.LiteEnableFlag==1 then
retVal,resultMsg=awtx.setPoint.set(1,setpt1user)retVal,resultMsg=awtx.setPoint.set(2,setpt2user)if(cfg==setpoint.setpt_above_below)then
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
retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)else
retVal,resultMsg=awtx.setPoint.set(3,setPointOver)end
end
retVal,resultMsg=awtx.setPoint.set(4,setPointOutEx4)retVal,resultMsg=awtx.setPoint.set(5,setPointOutEx5)retVal,resultMsg=awtx.setPoint.set(6,setPointOutEx6)retVal,resultMsg=awtx.setPoint.set(7,setPointOutEx7)retVal,resultMsg=awtx.setPoint.set(8,setPointOutEx8)retVal,resultMsg=awtx.setPoint.set(9,setPointOutEx9)retVal,resultMsg=awtx.setPoint.set(10,setPointOutEx10)result=awtx.setPoint.registerOutputEvent(1,truck.setpointOut1Handler)result=awtx.setPoint.registerOutputEvent(2,truck.setpointOut2Handler)result=awtx.setPoint.registerOutputEvent(3,setpoint.setpointOut3Handler)result=awtx.setPoint.registerOutputEvent(4,setpoint.setpointOut4Handler)result=awtx.setPoint.registerOutputEvent(5,setpoint.setpointOut5Handler)result=awtx.setPoint.registerOutputEvent(6,setpoint.setpointOut6Handler)result=awtx.setPoint.registerOutputEvent(7,setpoint.setpointOut7Handler)result=awtx.setPoint.registerOutputEvent(8,setpoint.setpointOut8Handler)result=awtx.setPoint.registerOutputEvent(9,setpoint.setpointOut9Handler)result=awtx.setPoint.registerOutputEvent(10,setpoint.setpointOut10Handler)setpoint.setOutputValues()setpoint.setSetpointPrintTokens()elseif truck.LiteEnableFlag==2 then
retVal,resultMsg=awtx.setPoint.set(1,LiteAutomaticOutEx1)retVal,resultMsg=awtx.setPoint.set(2,LiteAutomaticOutEx2)if(cfg==setpoint.setpt_above_below)then
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
retVal,resultMsg=awtx.setPoint.set(3,setPointBattery)else
retVal,resultMsg=awtx.setPoint.set(3,setPointOver)end
end
retVal,resultMsg=awtx.setPoint.set(4,LiteAutomaticOutEx4)retVal,resultMsg=awtx.setPoint.set(5,LiteAutomaticOutEx5)retVal,resultMsg=awtx.setPoint.set(6,LiteAutomaticOutEx6)retVal,resultMsg=awtx.setPoint.set(7,LiteAutomaticOutEx7)retVal,resultMsg=awtx.setPoint.set(8,LiteAutomaticOutEx8)retVal,resultMsg=awtx.setPoint.set(9,LiteAutomaticOutEx9)retVal,resultMsg=awtx.setPoint.set(10,LiteAutomaticOutEx10)result=awtx.setPoint.registerOutputEvent(1,truck.LiteAutomaticOut1Handler)result=awtx.setPoint.registerOutputEvent(2,truck.LiteAutomaticOut2Handler)result=awtx.setPoint.registerOutputEvent(3,truck.LiteAutomaticOut3Handler)result=awtx.setPoint.registerOutputEvent(4,truck.LiteAutomaticOut4Handler)result=awtx.setPoint.registerOutputEvent(5,truck.LiteAutomaticOut5Handler)result=awtx.setPoint.registerOutputEvent(6,truck.LiteAutomaticOut6Handler)result=awtx.setPoint.registerOutputEvent(7,truck.LiteAutomaticOut7Handler)result=awtx.setPoint.registerOutputEvent(8,truck.LiteAutomaticOut8Handler)result=awtx.setPoint.registerOutputEvent(9,truck.LiteAutomaticOut9Handler)result=awtx.setPoint.registerOutputEvent(10,truck.LiteAutomaticOut10Handler)setpoint.setOutputValues()setpoint.setSetpointPrintTokens()end
THold=truck.LiteTHold
negGZB=-wt.curDivision*config.grossZeroBand
posGZB=wt.curDivision*config.grossZeroBand
awtx.setPoint.varSet("THold",THold)awtx.setPoint.varSet("negGZB",negGZB)awtx.setPoint.varSet("posGZB",posGZB)end