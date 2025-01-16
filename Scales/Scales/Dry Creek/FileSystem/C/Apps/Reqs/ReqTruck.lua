--[[
*******************************************************************************

Filename:     ReqTruck.lua
Version:      1.0.0.0
Date:         2016-02-01
Customer:     Avery Weigh-Tronix
Description:
This is a set of functions to do some truck IO operations.

*******************************************************************************

*******************************************************************************
]]

require("awtxReqSQLiteMultiTable")

TruckConfigTable = {} -- Holds current channel, total format, and flags

local reportListSelection = "RePRINT,IN Rpt,OUT Rpt,FLT Rpt"

local menuprintLabelList = {[0] = "IN Rpt", "OUT Rpt", "FLT Rpt"}
local menuprintListSelection = "IN Rpt,OUT Rpt,FLT Rpt"

local REPORT_TYPE_IN  = 0
local REPORT_TYPE_OUT = 1
local REPORT_TYPE_FLT = 2


truck.TRUCK_TYPE_NONE  = 0
truck.TRUCK_TYPE_INOUT = 1
truck.TRUCK_TYPE_FLEET = 2

truck.MAX_NUMBER_FLEET = 1000
truck.MAX_NUMBER_IO = 1000

local XR4500_RED = "&"
local XR4500_GRN = "*"
local XR4500_OFF = "%"

Threshold = 1000
negGZB = -wt.curDivision * config.grossZeroBand
posGZB = wt.curDivision * config.grossZeroBand

curTruck = {}       -- Holds current truck record values
reprintTruck = {}   -- Holds the last printed truck data for reprinting.

truck.reprintFmt = 0

-- these are the Unit of measure that the current truck is stored by
local curTruckUnitsStr = wt.unitsStr
local curTruckUnitsIndex = wt.units

local DB_FileLocation_TruckData = [[C:\Apps\Database\Truck.db]]

-- database table names
truck.FleetTableName = "tblFleet"
truck.IOTableName = "tblIO"

-- These are used for ticket printting.  
local newTruckGross  = 0
local newTruckNet    = 0
local newTruckTare   = 0
local newTicketDate  = " "
local newTicketTime  = " "

truck.XR4500 = XR4500_OFF


function truck.curTruckInit()
  wt = awtx.weight.getCurrent(1)
  
  curTruck.truckId     = ""
  curTruck.tare        = 0
  curTruck.weight1     = 0
  curTruck.weight1date = ""
  curTruck.weight1time = ""
  curTruck.grossTotal  = 0
  curTruck.netTotal    = 0
  curTruck.tareTotal   = 0
  curTruck.transCount  = 0
  curTruck.units       = wt.unitsStr
  
  reprintTruck.truckId     = ""
  reprintTruck.tare        = 0
  reprintTruck.weight1     = 0
  reprintTruck.weight1date = ""
  reprintTruck.weight1time = ""
  reprintTruck.grossTotal  = 0
  reprintTruck.netTotal    = 0
  reprintTruck.tareTotal   = 0
  reprintTruck.transCount  = 0
  reprintTruck.units       = wt.unitsStr

  reprintTruck.newTruckGross  = 0
  reprintTruck.newTruckNet    = 0
  reprintTruck.newTruckTare   = 0
  reprintTruck.newTicketDate  = ""
  reprintTruck.newTicketTime  = ""
end

function truck.storeReprintData()
  reprintTruck.truckId     = curTruck.truckId
  reprintTruck.tare        = curTruck.tare
  reprintTruck.weight1     = curTruck.weight1
  reprintTruck.weight1date = curTruck.weight1date
  reprintTruck.weight1time = curTruck.weight1time
  reprintTruck.grossTotal  = curTruck.grossTotal
  reprintTruck.netTotal    = curTruck.netTotal
  reprintTruck.tareTotal   = curTruck.tareTotal
  reprintTruck.transCount  = curTruck.transCount
  reprintTruck.units       = curTruck.units

  reprintTruck.newTruckGross  = newTruckGross
  reprintTruck.newTruckNet    = newTruckNet
  reprintTruck.newTruckTare   = newTruckTare
  reprintTruck.newTicketDate  = newTicketDate
  reprintTruck.newTicketTime  = newTicketTime
end

function truck.recallReprintData()
  curTruck.truckId     = reprintTruck.truckId
  curTruck.tare        = reprintTruck.tare
  curTruck.weight1     = reprintTruck.weight1
  curTruck.weight1date = reprintTruck.weight1date
  curTruck.weight1time = reprintTruck.weight1time
  curTruck.grossTotal  = reprintTruck.grossTotal
  curTruck.netTotal    = reprintTruck.netTotal
  curTruck.tareTotal   = reprintTruck.tareTotal
  curTruck.transCount  = reprintTruck.transCount
  curTruck.units       = reprintTruck.units

  newTruckGross  = reprintTruck.newTruckGross
  newTruckNet    = reprintTruck.newTruckNet
  newTruckTare   = reprintTruck.newTruckTare
  newTicketDate  = reprintTruck.newTicketDate
  newTicketTime  = reprintTruck.newTicketTime
end

function truck.setTruckPrintType(tmpType)
    -- save the new truck type out so that on power up we can recall
    TruckConfigTable.CurrentChannelType.value = tmpType
end


function truck.getTruckPrintType()
    return TruckConfigTable.CurrentChannelType.value
end

--[[
function truck.DBInit
Description:
  This function creates SQLite Database for truck channels
  
Parameters:
  None
  
Returns:
  None   
]]
function truck.DBInit()
    
  --Set up the fields for the Database tables
  local tableFleetFields = {"truckId VARCHAR PRIMARY KEY", "tare DOUBLE", "grossTotal DOUBLE", "netTotal DOUBLE", "tareTotal DOUBLE", "transCount INTEGER", "units VARCHAR"}
  local tableIOFields = {"truckId VARCHAR PRIMARY KEY", "weight1 DOUBLE", "weight1date VARCHAR", "weight1time VARCHAR", "grossTotal DOUBLE", "netTotal DOUBLE", "tareTotal DOUBLE", "transCount INTEGER", "units VARCHAR"}

  -- Create the Database and Tables if they doesn't exist.
  awtxReq.database.initializeDatabase(DB_FileLocation_TruckData, truck.FleetTableName,  tableFleetFields)
  awtxReq.database.initializeDatabase(DB_FileLocation_TruckData, truck.IOTableName,  tableIOFields)

end
  
--[[
function truck.findFleetId
Description:
  This function checks to see if a Fleet ID exists
  
Parameters:
  tmpId - Id value to look for
  
Returns:
  true if the tmpId is found    
]]
function truck.findFleetId(tmpId)
  local found = false
  local record = awtxReq.database.recallRecord("truckId", tmpId, truck.FleetTableName)
  
  if next(record) ~= nil then
    found = true
  end
  return found 
end

--[[
function truck.findIOId
Description:
  This function checks to see if an IO ID exists
  
Parameters:
  tmpId - Id value to look for
  
Returns:
  true if the tmpId is found    
]]
function truck.findIOId(tmpId)
  local found = false
  local record = awtxReq.database.recallRecord("truckId", tmpId, truck.IOTableName)
  
  if next(record) ~= nil then
    found = true
  end
  return found 
end

--[[
function truck.recallFleetId
Description:
  This function recall the values for an Fleet channel from the SQLite Database
  
Parameters:
  None
  
Returns:
  None   
]]
function truck.recallFleetId(channel)
  local tempError
  local wt = awtx.weight.getCurrent()

  --local record = {curTruck.truckId, curTruck.tare, curTruck.weight1, curTruck.weight1date, curTruck.weight1time, curTruck.grossTotal, curTruck.netTotal, curTruck.tareTotal, curTruck.transCount, curTruck.units}
  local record = nil
  tmpChannel = tostring(channel)
  record = awtxReq.database.recallRecord("truckId", tmpChannel, truck.FleetTableName)
  --make sure we got a record back
  if next(record) ~= nil then
    -- TODO : convert the weights to the current unit of measure...
    
    curTruck.truckId     = record[1].truckId
    curTruck.units       = record[1].units      
  
        
    --saved the "stored" units
    curTruckUnitsStr = curTruck.units
    tempError, curTruckUnitsIndex = awtx.weight.unitStrToUnitIndex(curTruckUnitsStr)
    if (tempError ~= 0) then
      curTruck.units = wt.unitsStr
      curTruckUnitsStr = wt.unitsStr
      curTruckUnitsIndex = wt.units
    end
    
    -- convert the stored weights to the current unit of measure
    curTruck.tare        = awtx.weight.convertWeight(curTruckUnitsIndex, record[1].tare, wt.units, 1)
    curTruck.grossTotal  = awtx.weight.convertWeight(curTruckUnitsIndex, record[1].grossTotal, wt.units, 1)
    curTruck.netTotal    = awtx.weight.convertWeight(curTruckUnitsIndex, record[1].netTotal, wt.units, 1)
    curTruck.tareTotal   = awtx.weight.convertWeight(curTruckUnitsIndex, record[1].tareTotal, wt.units, 1)
    curTruck.transCount  = record[1].transCount
  
  else
      curTruck.truckId     = tmpChannel
      curTruck.tare        = 0
      curTruck.grossTotal  = 0
      curTruck.netTotal    = 0
      curTruck.tareTotal   = 0
      curTruck.transCount  = 0
      curTruck.units       = wt.unitsStr 
    --saved the "stored" units
    curTruckUnitsStr = wt.unitsStr
    curTruckUnitsIndex = wt.units
  end
  
  TruckConfigTable.CurrentChannel.value = channel   -- set the curent channel
  truck.setPrintTokens()
end

--[[
function truck.recallIOId
Description:
  This function recall the values for an In Out channel from the SQLite Database
  
Parameters:
  None
  
Returns:
  None   
]]
function truck.recallIOId(channel)
  local tempError
  local wt = awtx.weight.getCurrent()
  
  --local record = {curTruck.truckId, curTruck.tare, curTruck.weight1, curTruck.weight1date, curTruck.weight1time, curTruck.grossTotal, curTruck.netTotal, curTruck.tareTotal, curTruck.transCount, curTruck.units}
  local record = nil
  tmpChannel = tostring(channel)
  record = awtxReq.database.recallRecord("truckId", tmpChannel, truck.IOTableName)
  --make sure we got a record back
  if next(record) ~= nil then
    curTruck.truckId     = record[1].truckId
    curTruck.units       = record[1].units
    
    --saved the "stored" units
    curTruckUnitsStr = curTruck.units
    tempError, curTruckUnitsIndex = awtx.weight.unitStrToUnitIndex(curTruckUnitsStr)
    if (tempError ~= 0) then
      curTruck.units = wt.unitsStr
      curTruckUnitsStr = wt.unitsStr
      curTruckUnitsIndex = wt.units
    end
    -- convert the stored weights to the current unit of measure
    curTruck.weight1      = awtx.weight.convertWeight(curTruckUnitsIndex, record[1].weight1, wt.units, 1)
    curTruck.weight1time  = record[1].weight1time
    curTruck.weight1date  = record[1].weight1date
    curTruck.grossTotal  = awtx.weight.convertWeight(curTruckUnitsIndex, record[1].grossTotal, wt.units, 1)
    curTruck.netTotal    = awtx.weight.convertWeight(curTruckUnitsIndex, record[1].netTotal, wt.units, 1)
    curTruck.tareTotal   = awtx.weight.convertWeight(curTruckUnitsIndex, record[1].tareTotal, wt.units, 1)
    curTruck.transCount  = record[1].transCount
  else
    curTruck.truckId     = tmpChannel
    curTruck.weight1      = 0
    curTruck.weight1time  = " "
    curTruck.weight1date  = " "
    curTruck.grossTotal  = 0
    curTruck.netTotal    = 0
    curTruck.tareTotal   = 0
    curTruck.transCount  = 0
    curTruck.units       = wt.unitsStr 
    --saved the "stored" units
    curTruckUnitsStr = wt.unitsStr
    curTruckUnitsIndex = wt.units
  end
  
  
  TruckConfigTable.CurrentChannel.value = channel   -- set the curent channel
  truck.setPrintTokens()

end

function truck.saveFleetId(tmpId)
  local tempTare
  local tempGrossTotal
  local tempNetTotal
  local tempTareTotal
  local curRecord = {}
  
  wt = awtx.weight.getCurrent(1)

  --Convert all the weights to the database unit
  tempTare = awtx.weight.convertWeight(wt.units, curTruck.tare, curTruckUnitsIndex, 1)
  tempGrossTotal = awtx.weight.convertWeight(wt.units, curTruck.grossTotal, curTruckUnitsIndex, 1)
  tempNetTotal = awtx.weight.convertWeight(wt.units, curTruck.netTotal, curTruckUnitsIndex, 1)
  tempTareTotal = awtx.weight.convertWeight(wt.units, curTruck.tareTotal, curTruckUnitsIndex, 1)

  curRecord = {
      curTruck.truckId,
      tempTare,
      tempGrossTotal,
      tempNetTotal,
      tempTareTotal,
      curTruck.transCount,
      curTruckUnitsStr }

  awtxReq.database.addUpdateRecord(curRecord, truck.FleetTableName)  

end
function truck.saveIOId(tmpId)
  local tempWeight1
  local tempGrossTotal
  local tempNetTotal
  local tempTareTotal
  local curRecord = {}
  
  wt = awtx.weight.getCurrent(1)
  
  --Convert all the weights to the database unit
  tempWeight1 = awtx.weight.convertWeight(wt.units, curTruck.weight1, curTruckUnitsIndex, 1)
  tempGrossTotal = awtx.weight.convertWeight(wt.units, curTruck.grossTotal, curTruckUnitsIndex, 1)
  tempNetTotal = awtx.weight.convertWeight(wt.units, curTruck.netTotal, curTruckUnitsIndex, 1)
  tempTareTotal = awtx.weight.convertWeight(wt.units, curTruck.tareTotal, curTruckUnitsIndex, 1)

  curRecord = {
      curTruck.truckId,
      tempWeight1,
      curTruck.weight1date,
      curTruck.weight1time,
      tempGrossTotal,
      tempNetTotal,
      tempTareTotal,
      curTruck.transCount,
      curTruckUnitsStr }

  awtxReq.database.addUpdateRecord(curRecord, truck.IOTableName)  
  
  -- Added by Matt Burkett 05/26/2016 to remove records as they are printed.
  if (curTruck.weight1 == 0) then
    
    truck.deleteIOId(curTruck.truckId)
    
  end
    
end

function truck.getIdType(tempTruckId)
  if truck.findIOId(tempTruckId) then 
    return truck.TRUCK_TYPE_INOUT
  end
  if truck.findFleetId(tempTruckId) then
    return truck.TRUCK_TYPE_FLEET
  end
  return truck.TRUCK_TYPE_NONE
end


--[[
function truck.setPrintTokens
Description:
  This function sets Application Variable 3 as the truck channel in-use.
  
Parameters:
  None
  
Returns:
  None   
]]
function truck.setPrintTokens()
  awtx.fmtPrint.varSet(3, TruckConfigTable.CurrentChannel.value, "Truck Channel", awtx.fmtPrint.TYPE_INTEGER)
    awtx.fmtPrint.varSet(21, curTruck.truckId, "Truck ID", awtx.fmtPrint.TYPE_INTEGER)
    awtx.fmtPrint.varSet(22, curTruck.tare, "Fleet Tare", awtx.fmtPrint.TYPE_FLOAT)
    awtx.fmtPrint.varSet(23, curTruck.weight1, "InWeight", awtx.fmtPrint.TYPE_FLOAT)
    awtx.fmtPrint.varSet(24, curTruck.weight1date, "InDate", awtx.fmtPrint.TYPE_STRING)
    awtx.fmtPrint.varSet(25, curTruck.weight1time, "InTime", awtx.fmtPrint.TYPE_STRING)
    awtx.fmtPrint.varSet(26, curTruck.grossTotal, "Gross Total", awtx.fmtPrint.TYPE_FLOAT)
    awtx.fmtPrint.varSet(27, curTruck.netTotal, "Net Total", awtx.fmtPrint.TYPE_FLOAT)
    awtx.fmtPrint.varSet(28, curTruck.tareTotal, "Tare Total", awtx.fmtPrint.TYPE_FLOAT)
    awtx.fmtPrint.varSet(29, curTruck.transCount, "Transaction Count", awtx.fmtPrint.TYPE_INTEGER)
    awtx.fmtPrint.varSet(30, curTruck.units, "Unit of Measure", awtx.fmtPrint.TYPE_STRING)

    awtx.fmtPrint.varSet(31, newTruckGross, "Gross Weight", awtx.fmtPrint.TYPE_FLOAT)
    awtx.fmtPrint.varSet(32, newTruckTare, "Tare Weight", awtx.fmtPrint.TYPE_FLOAT)
    awtx.fmtPrint.varSet(33, newTruckNet, "Net Weight", awtx.fmtPrint.TYPE_FLOAT)
    awtx.fmtPrint.varSet(34, newTicketDate, "Ticket Date", awtx.fmtPrint.TYPE_STRING)
    awtx.fmtPrint.varSet(35, newTicketTime, "Ticket Time", awtx.fmtPrint.TYPE_STRING)
end

function truck.setLiteGreen()
  --Set XR code to green
  truck.XR4500 = XR4500_GRN
  truck.setXR4500PrintToken()
  -- out 2 OFF
  result = awtx.setPoint.outputClr(2) 
  -- out 1 ON
  result = awtx.setPoint.outputSet(1)
end


function truck.setLiteRed()
  --Set XR code to red
  truck.XR4500 = XR4500_RED
  truck.setXR4500PrintToken()
  -- out 1 OFF
  result = awtx.setPoint.outputClr(1) 
  -- out 2 ON
  result = awtx.setPoint.outputSet(2)
end

function truck.setLiteOff()
  truck.XR4500 = XR4500_OFF
  truck.setXR4500PrintToken()
  if TruckConfigTable.LiteEnableFlag.value == 0 then  --Off
    -- out 1 OFF
    result = awtx.setPoint.outputClr(1)
    result = awtx.setPoint.outputClr(6)
    -- out 2 OFF
    result = awtx.setPoint.outputClr(2)      
    result = awtx.setPoint.outputClr(7)
  elseif TruckConfigTable.LiteEnableFlag.value == 1 then  --Manual
    -- out 1 OFF
    result = awtx.setPoint.outputClr(1)
    -- out 2 OFF
    result = awtx.setPoint.outputClr(2)      
  elseif TruckConfigTable.LiteEnableFlag.value >= 2 then        --Automatic=2 Both=3
    -- out 1 OFF
    result = awtx.setPoint.outputClr(6)
    result = awtx.setPoint.outputClr(1)      
    -- out 2 OFF
    result = awtx.setPoint.outputClr(7)
    result = awtx.setPoint.outputClr(2)      
  end
end

function truck.setXR4500PrintToken()
    awtx.fmtPrint.varSet(65, truck.XR4500, "XR4500 Light" , AWTX_LUA_STRING)
end

function truck.setFleetId(tmpTruckId)
  local idType = truck.getIdType(tmpTruckId)
  if idType == truck.TRUCK_TYPE_NONE then
    awtxReq.display.displayWord("NOT FND")
  else
    if idType == truck.TRUCK_TYPE_INOUT then
      awtxReq.display.displayCant()
      awtxReq.display.displayWord(" IO ID ")
    else
      truck.recallFleetId(tmpTruckId)
      truck.setTruckPrintType(truck.TRUCK_TYPE_FLEET)
      awtx.weight.requestPresetTare(curTruck.tare)
      truck.setPrintTokens()
      awtxReq.display.displayWord(" FLEET")
    end
  end
end

function truck.setIOId(tmpTruckId)
  local idType = truck.getIdType(tmpTruckId)
  
  -- Added by Matt Burkett 05/27/2016
  -- Check weight before printing anything to screen.  If not above Min Wt 
  -- then Print No Trk to the screen
  wt = awtx.weight.getCurrent(1)
  if (wt.gross > MinWeight) then
    
    if idType == truck.TRUCK_TYPE_FLEET then
      awtxReq.display.displayCant()
      awtxReq.display.displayWord("FLEETID")
    elseif idType == truck.TRUCK_TYPE_INOUT then
      truck.recallIOId(tmpTruckId)
      truck.setTruckPrintType(truck.TRUCK_TYPE_INOUT)
      awtx.weight.requestTareClear()
      truck.setPrintTokens()
      if curTruck.weight1 == 0 then
        awtxReq.display.displayWord(" INBND")
      else
        awtxReq.display.displayWord("OUTBND")
      end
    else
      if (truck.MAX_NUMBER_IO > awtxReq.database.recordCount(truck.IOTableName)) then
        awtxReq.display.displayWord(" INBND")
        -- this will create the blank record if it doesnt 
        truck.recallIOId(tmpTruckId)
        truck.setTruckPrintType(truck.TRUCK_TYPE_INOUT)
        awtx.weight.requestTareClear()
        truck.setPrintTokens()
      else
         --if it is then display error
        awtxReq.display.displayCant()
        awtxReq.display.displayWord("DB Full")
      end    
    end
  else
    if (truck.MAX_NUMBER_IO > awtxReq.database.recordCount(truck.IOTableName)) then
      awtxReq.display.displayWord("NoTruck")
    end
  end
end

function truck.printTruckFleet()
  local tmpWeight
  local tmpTare
  local tmpNet
  local tmpGross
  
  tmpWeight = wt.gross
  if tmpWeight > 0 then
    if not wt.inGrossBand then
      if curTruck.tare == 0 then
          awtxReq.display.displayWord("No Tare")
      else
        if tmpWeight > (curTruck.tare + (config.grossZeroBand * wt.curDivision)) then
          awtxReq.display.displayWord("Printng")
          tmpTare = curTruck.tare
          tmpGross = tmpWeight
          tmpNet = tmpGross - tmpTare
          curTruck.grossTotal = curTruck.grossTotal + tmpGross
          curTruck.netTotal   = curTruck.netTotal   + tmpNet
          curTruck.tareTotal  = curTruck.tareTotal  + tmpTare
          curTruck.transCount = curTruck.transCount + 1
          newTruckGross = tmpGross
          newTruckTare  = tmpTare
          newTruckNet   = tmpNet
          curTruck.units      = wt.unitsStr
          newTicketDate = truck.formatDate(wt.timeStamp)
          newTicketTime = truck.formatTime(wt.timeStamp)

          truck.setPrintTokens()
          awtx.printer.PrintFmt(TruckConfigTable.FleetFmt.value)
          truck.reprintFmt = TruckConfigTable.FleetFmt.value
          truck.storeReprintData()
          truck.saveFleetId(curTruck.truckId)
        else
          awtxReq.display.displayWord("LOW Wt ")
        end
      end
      truck.setTruckPrintType(truck.TRUCK_TYPE_NONE)
      awtx.weight.requestTareClear()
    else
      awtxReq.display.displayWord(" No Wt ")
    end
  else
    awtxReq.display.displayWord("Neg Wt ")
  end
end


function truck.printTruckIO()
  local tmpWeight
  local tmpTare
  local tmpNet
  local tmpGross
  local tmpTimestamp
  
  tmpWeight = wt.gross
  if tmpWeight > 0 then
    if not wt.inGrossBand then
      awtxReq.display.displayWord("Printng")
      if curTruck.weight1 == 0 then
        newTruckGross   = tmpWeight
        newTruckNet     = 0
        newTruckTare    = 0
        curTruck.units        = wt.unitsStr

        tmpTimestamp    = wt.timeStamp
        newTicketDate   = truck.formatDate(tmpTimestamp)
        newTicketTime   = truck.formatTime(tmpTimestamp)

        truck.storeReprintData()
        truck.setPrintTokens()
        awtx.printer.PrintFmt(TruckConfigTable.InFmt.value)
        truck.reprintFmt = TruckConfigTable.InFmt.value

        curTruck.weight1     = tmpWeight
        curTruck.weight1date = newTicketDate
        curTruck.weight1time = newTicketTime
        truck.saveIOId(curTruck.truckId)
      else
        if tmpWeight < curTruck.weight1 then
          tmpTare = tmpWeight
          tmpGross = curTruck.weight1
        else
          tmpTare = curTruck.weight1
          tmpGross = tmpWeight
        end
        tmpNet = tmpGross - tmpTare
        curTruck.grossTotal = curTruck.grossTotal + tmpGross
        curTruck.netTotal   = curTruck.netTotal   + tmpNet
        curTruck.tareTotal  = curTruck.tareTotal  + tmpTare
        curTruck.transCount = curTruck.transCount + 1
        newTruckGross = tmpGross
        newTruckTare  = tmpTare
        newTruckNet   = tmpNet
        curTruck.units       = wt.unitsStr

        tmpTimestamp  = wt.timeStamp
        newTicketDate = truck.formatDate(tmpTimestamp)
        newTicketTime = truck.formatTime(tmpTimestamp)
        truck.storeReprintData()
        truck.setPrintTokens()
        awtx.printer.PrintFmt(TruckConfigTable.OutFmt.value)
        truck.reprintFmt = TruckConfigTable.OutFmt.value

        curTruck.weight1 = 0
        curTruck.weight1date = " "
        curTruck.weight1time = " "
        truck.saveIOId(curTruck.truckId)
      end
      truck.setTruckPrintType(truck.TRUCK_TYPE_NONE)
      awtx.weight.requestTareClear()
    else
      awtxReq.display.displayWord(" No Wt ")
    end
  else
    awtxReq.display.displayWord("Neg Wt ")
  end
end

function truck.deleteFleetId(tempId)
  awtxReq.database.deleteRecord("truckId", tempId, truck.FleetTableName)
end

function truck.deleteIOId(tempId)
  awtxReq.database.deleteRecord("truckId", tempId, truck.IOTableName)
end


function truck.inClearAll()
  local index
  local reportTable = {}
  local curRecord = {}
  
    reportTable = awtxReq.database.recallAllRecords(truck.IOTableName)
  
  -- Print Body
  for index = 1, #reportTable do
    if (reportTable[index].weight1 > 0) then
      curRecord = {
        reportTable[index].truckId,
        0,
        " ",
        " ",
        reportTable[index].grossTotal,
        reportTable[index].netTotal,
        reportTable[index].tareTotal,
        reportTable[index].transCount,
        reportTable[index].units 
      }

      awtxReq.database.addUpdateRecord(curRecord, truck.IOTableName)  

    end
  end
end


function truck.ioDeleteAll()
  awtxReq.database.deleteAllRecords(truck.IOTableName)
end


function truck.fleetDeleteAll()
  awtxReq.database.deleteAllRecords(truck.FleetTableName)
end


function truck.allDelete()
  awtxReq.database.deleteAllRecords(truck.IOTableName)
  awtxReq.database.deleteAllRecords(truck.FleetTableName)
end



--[[
function truck.CFGInit
Description:
  This function creates the config table
  
Parameters:
  None
  
Returns:
  None   
]]
function truck.CFGInit()
	  awtx.variables.createTable("tblTruckConfig")      -- create table for application parameters
end

--[[
function truck.configRecall
Description:
  This function retrieves application parameters from permanent storage
  
Parameters:
  None
  
Returns:
  None   
]]
function truck.configRecall()
  TruckConfigTable.CurrentChannel = awtxReq.variables.SavedVariable("Channel", 1, true, "tblTruckConfig")
  TruckConfigTable.CurrentChannelType = awtxReq.variables.SavedVariable("ChannelType", 0, true, "tblTruckConfig")
  TruckConfigTable.PrintTotalFlag = awtxReq.variables.SavedVariable("PrintTotalFlag", 0, true, "tblTruckConfig")
  TruckConfigTable.TotalFmt = awtxReq.variables.SavedVariable("TotalFmt", 8, true, "tblTruckConfig")
  TruckConfigTable.ClearTotalFlag = awtxReq.variables.SavedVariable("ClearTotalFlag", 0, true, "tblTruckConfig")
  TruckConfigTable.InFmt = awtxReq.variables.SavedVariable("InFmt", 2, true, "tblTruckConfig")
  TruckConfigTable.InHeaderFmt = awtxReq.variables.SavedVariable("InHeaderFmt", 3, true, "tblTruckConfig")
  TruckConfigTable.InBodyFmt = awtxReq.variables.SavedVariable("InBodyFmt", 4, true, "tblTruckConfig")
  TruckConfigTable.InFooterFmt = awtxReq.variables.SavedVariable("InFooterFmt", 5, true, "tblTruckConfig")
  TruckConfigTable.OutFmt = awtxReq.variables.SavedVariable("OutFmt", 6, true, "tblTruckConfig")
  TruckConfigTable.OutHeaderFmt = awtxReq.variables.SavedVariable("OutHeaderFmt", 7, true, "tblTruckConfig")
  TruckConfigTable.OutBodyFmt = awtxReq.variables.SavedVariable("OutBodyFmt", 8, true, "tblTruckConfig")
  TruckConfigTable.OutFooterFmt = awtxReq.variables.SavedVariable("OutFooterFmt", 9, true, "tblTruckConfig")
  TruckConfigTable.FleetFmt = awtxReq.variables.SavedVariable("FleetFmt", 10, true, "tblTruckConfig")
  TruckConfigTable.FleetHeaderFmt = awtxReq.variables.SavedVariable("FleetHeaderFmt", 11, true, "tblTruckConfig")
  TruckConfigTable.FleetBodyFmt = awtxReq.variables.SavedVariable("FleetBodyFmt", 12, true, "tblTruckConfig")
  TruckConfigTable.FleetFooterFmt = awtxReq.variables.SavedVariable("FleetFooterFmt", 13, true, "tblTruckConfig")
  TruckConfigTable.LiteEnableFlag = awtxReq.variables.SavedVariable("LiteEnableFlag", 3, true, "tblTruckConfig")
  TruckConfigTable.LiteTHold = awtxReq.variables.SavedVariable("LiteTHold", 1000, true, "tblTruckConfig")
  --also calculate setpoint variables
  Threshold = TruckConfigTable.LiteTHold.value
end



function truck.editFormat(label, editorTitle, currentVal)
  local newVal
  local isEnterKey
  
  newVal, isEnterKey = awtx.keypad.enterInteger(currentVal, 1, 40, promptTimeout, editorTitle)
  awtx.display.writeLine(label)

  return newVal, isEnterKey
end


function truck.editInFmt(label)
  local newVal
  local isEnterKey
  
  newVal, isEnterKey = truck.editFormat(label, "In Ticket Fmt", TruckConfigTable.InFmt.value)
  if isEnterKey then
    TruckConfigTable.InFmt.value = newVal
  end
end


function truck.editInHeaderFmt(label)
  local newVal
  local isEnterKey
  
  newVal, isEnterKey = truck.editFormat(label, "In Header Fmt", TruckConfigTable.InHeaderFmt.value)
  if isEnterKey then
    TruckConfigTable.InHeaderFmt.value = newVal
  end
end


function truck.editInBodyFmt(label)
  local newVal
  local isEnterKey
  
  newVal, isEnterKey = truck.editFormat(label, "In Body Fmt", TruckConfigTable.InBodyFmt.value)
  if isEnterKey then
    TruckConfigTable.InBodyFmt.value = newVal
  end
end


function truck.editInFooterFmt(label)
  local newVal
  local isEnterKey
  
  newVal, isEnterKey = truck.editFormat(label, "In Footer Fmt", TruckConfigTable.InFooterFmt.value)
  if isEnterKey then
    TruckConfigTable.InFooterFmt.value = newVal
  end
end


function truck.editOutFmt(label)
  local newVal
  local isEnterKey
  
  newVal, isEnterKey = truck.editFormat(label, "Out Ticket Fmt", TruckConfigTable.OutFmt.value)
  if isEnterKey then
    TruckConfigTable.OutFmt.value = newVal
  end
end


function truck.editOutHeaderFmt(label)
  local newVal
  local isEnterKey
  
  newVal, isEnterKey = truck.editFormat(label, "Out Header Fmt", TruckConfigTable.OutHeaderFmt.value)
  if isEnterKey then
    TruckConfigTable.OutHeaderFmt.value = newVal
  end
end


function truck.editOutBodyFmt(label)
  local newVal
  local isEnterKey
  
  newVal, isEnterKey = truck.editFormat(label, "Out Body Fmt", TruckConfigTable.OutBodyFmt.value)
  if isEnterKey then
    TruckConfigTable.OutBodyFmt.value = newVal
  end
end


function truck.editOutFooterFmt(label)
  local newVal
  local isEnterKey
  
  newVal, isEnterKey = truck.editFormat(label, "Out Footer Fmt", TruckConfigTable.OutFooterFmt.value)
  if isEnterKey then
    TruckConfigTable.OutFooterFmt.value = newVal
  end
end


function truck.editFleetFmt(label)
  local newVal
  local isEnterKey
  
  newVal, isEnterKey = truck.editFormat(label, "Fleet Ticket Fmt", TruckConfigTable.FleetFmt.value)
  if isEnterKey then
    TruckConfigTable.FleetFmt.value = newVal
  end
end


function truck.editFleetHeaderFmt(label)
  local newVal
  local isEnterKey
  
  newVal, isEnterKey = truck.editFormat(label, "Fleet Header Fmt", TruckConfigTable.FleetHeaderFmt.value)
  if isEnterKey then
    TruckConfigTable.FleetHeaderFmt.value = newVal
  end
end


function truck.editFleetBodyFmt(label)
  local newVal
  local isEnterKey
  
  newVal, isEnterKey = truck.editFormat(label, "Fleet Body Fmt", TruckConfigTable.FleetBodyFmt.value)
  if isEnterKey then
    TruckConfigTable.FleetBodyFmt.value = newVal
  end
end


function truck.editFleetFooterFmt(label)
  local newVal
  local isEnterKey
  
  newVal, isEnterKey = truck.editFormat(label, "Fleet Footer Fmt", TruckConfigTable.FleetFooterFmt.value)
  if isEnterKey then
    TruckConfigTable.FleetFooterFmt.value = newVal
  end
end


function truck.editLiteEnable(label)
  local usermode
  local index
  local isEnterKey
  
  index = TruckConfigTable.LiteEnableFlag.value
  index, isEnterKey = awtx.keypad.selectList("Off,Manual,Auto,boTH", index)
  awtx.display.writeLine(label)
  if isEnterKey then
    TruckConfigTable.LiteEnableFlag.value = index
  end
end


function truck.editLiteTHold(label)
  local tmpLiteTHold
  local minVal
  local maxVal
  local tempUnitStr
  local isEnterKey
  
  wt = awtx.weight.getCurrent(1)
  tempUnitStr = wt.unitsStr
  minVal = 0
  maxVal = wt.curCapacity
  tmpLiteTHold = TruckConfigTable.LiteTHold.value
  tmpLiteTHold, isEnterKey = awtx.keypad.enterWeightWithUnits(tmpLiteTHold, minVal, maxVal, tempUnitStr, config.displaySeparator)
  awtx.display.writeLine(label)
  if isEnterKey then
    TruckConfigTable.LiteTHold.value = tmpLiteTHold
    Threshold = TruckConfigTable.LiteTHold.value
  end
end

function truck.editTruckFleetTruckId(label)
  local tmpTruckId = 0
  local isEnterKey = 0
  local found 
  
  tmpTruckId, isEnterKey = awtx.keypad.enterString("", TRUCK_ID_LENGTH, promptTimeout, "Id:", "Fleet")

  if isEnterKey then
    -- check to see if it exists as a Fleet truck
    found = truck.findFleetId(tmpTruckId)
    if found == false then
      -- check to see if it exists as an IO truck
      found = truck.findIOId(tmpTruckId)
      if found == false then
        -- check to see if the fleet table is full 
        if (truck.MAX_NUMBER_FLEET <= awtxReq.database.recordCount(truck.FleetTableName)) then
          --if it is then display error
          awtxReq.display.displayCant()
          awtxReq.display.displayWord("DB Full")
          --switch to the Truck Setup menu
          supervisor.menuLevel = TruckEditMenu
          supervisor.menuCurrent = 3          
        
        else
          -- create new Fleet ID
          awtxReq.display.displayWord("NOT FND")
          awtxReq.display.displayWord("ADDING")
          --Wait until they enter a Tare before Creating so ..
    
          -- this will recall a blank record when it doesn't exits
          truck.recallFleetId(tmpTruckId)

          --switch to the Truck Edit Fleet menu
          supervisor.menuLevel = TruckEditFleetMenu
          supervisor.menuCurrent = 1
        end
      else
        -- tell them its already an IO truck
          awtxReq.display.displayCant()
          awtxReq.display.displayWord(" IO ID ")
          --switch to the Truck Setup menu
          supervisor.menuLevel = TruckEditMenu
          supervisor.menuCurrent = 3          
      end
    else
      -- recall the Fleet ID's Record to edit
      truck.recallFleetId(tmpTruckId)
      --switch to the Truck Edit Fleet menu
      supervisor.menuLevel = TruckEditFleetMenu
      supervisor.menuCurrent = 1
    end
  end
end

function truck.editTruckInTruckId(label)
  local tmpTruckId = 0
  local isEnterKey = 0
  local found
  
  tmpTruckId, isEnterKey = awtx.keypad.enterString("", TRUCK_ID_LENGTH, promptTimeout, "Id:", "  IN")

  if isEnterKey then
      found = truck.findIOId(tmpTruckId)
      if found == false then
          awtxReq.display.displayWord("NOT FND")
          --switch to the Truck Edit In menu
          supervisor.menuLevel = TruckEditMenu
          supervisor.menuCurrent = 1
      else
          if curTruck.weight1 > 0 then
            --switch to the Truck Edit In menu
            supervisor.menuLevel = TruckEditInMenu
            supervisor.menuCurrent = 1
          else
            awtxReq.display.displayWord(" NO IN")
            --switch to the Truck Setup menu
            supervisor.menuLevel = TruckEditMenu
            supervisor.menuCurrent = 1          
          end
      end
  end
end


function truck.editTruckIOTruckId(label)
  local tmpTruckId = 0
  local isEnterKey = 0
  local found
  
  tmpTruckId, isEnterKey = awtx.keypad.enterString("", TRUCK_ID_LENGTH, promptTimeout, "Id:", "  IO")
  
  if isEnterKey then
    found = truck.findIOId(tmpTruckId)
    if found == false then
      awtxReq.display.displayWord("NOT FND")
      
      --switch to the Truck Setup menu
      supervisor.menuLevel = TruckEditMenu
      supervisor.menuCurrent = 2
    else
      truck.recallIOId(tmpTruckId)
      --switch to the Truck Edit In menu
      supervisor.menuLevel = TruckEditIOMenu
      supervisor.menuCurrent = 1
    end
  end
end


function truck.clrTruckInWeight(label)
  local index = 0
  local isEnterKey = 0
  
  index, isEnterKey = awtx.keypad.selectList("No,Yes", 0)
  awtx.display.writeLine(label)
  if isEnterKey then
    if index == 1 then
      usermode = awtx.display.setMode(awtx.display.MODE_MENU)
      awtx.display.setDisplayBusy()
      awtx.os.sleep(busytime)
      
      truck.clearInId(curTruck.truckId)

      awtx.display.clrDisplayBusy()
      awtx.display.setMode(usermode)
      --switch to the Truck Setup menu
      supervisor.menuLevel = TruckEditMenu
      supervisor.menuCurrent = 1
    end
  end
end


function truck.delTruckIdIO(label)
  local index = 0
  local isEnterKey = 0

  index, isEnterKey = awtx.keypad.selectList("No,Yes", 0)
  awtx.display.writeLine(label)
  if isEnterKey and index == 1 then
    usermode = awtx.display.setMode(awtx.display.MODE_MENU)
    awtx.display.setDisplayBusy()
    awtx.os.sleep(busytime)

    truck.deleteIOId(curTruck.truckId)

    awtx.display.clrDisplayBusy()
    awtx.display.setMode(usermode)
    --switch to the Truck Setup menu
    supervisor.menuLevel = TruckEditMenu
    supervisor.menuCurrent = 2
  end
end

function truck.clearInId(tempId)
    curTruck.weight1     = 0
    curTruck.weight1date = " "
    curTruck.weight1time = " "
    
    -- ID was already found in the database, so clear the values and save it.
    truck.saveIOId(tmpId)
end

function truck.editTareFleet(label)
  local tmptare
  local minVal
  local maxVal
  local tempUnitStr
  local isEnterKey
  local tmpMotion
  local tmpWeight
  
  wt = awtx.weight.getCurrent(1)

  if wt.inGrossBand then
    tmptare = curTruck.tare
  else
    tmpMotion, tmpWeight = truck.waitForMotion()
    if not tmpMotion then
      tmptare = tmpWeight
    else
      tmptare = curTruck.tare
    end
  end
  minVal = 0
  maxVal = wt.curCapacity
  tmptare, isEnterKey = awtx.keypad.enterWeightWithUnits(tmptare, minVal, maxVal, wt.unitsStr, config.displaySeparator)
  awtx.display.writeLine(label)
  if isEnterKey then
    curTruck.tare = tmptare
    truck.saveFleetId(curTruck.truckId)
  end
  return isEnterKey
end


function truck.delIdFleet(label)
  local index
  local isEnterKey
  local curMode

    index, isEnterKey = awtx.keypad.selectList("No,Yes", 0)
    awtx.display.writeLine(label)
    if isEnterKey then
        if index == 1 then
            curMode = awtx.display.setMode(awtx.display.MODE_MENU)
            awtx.display.setDisplayBusy()
            awtx.os.sleep(busytime)

            truck.deleteFleetId(curTruck.truckId)

            awtx.display.clrDisplayBusy()
            awtx.display.setMode(curMode)
            --switch to the Truck Setup menu
            supervisor.menuLevel = TruckEditMenu
            supervisor.menuCurrent = 3
        end
    end
end

function truck.selectTruckFleetID()
  local tmpTruckId
  local isEnterKey
  awtxReq.display.displayWord("Fleet")

  tmpTruckId, isEnterKey = awtx.keypad.enterString(curTruck.truckId, TRUCK_ID_LENGTH, promptTimeout,"ID:", "Enter")
  if isEnterKey then
    truck.setFleetId(tmpTruckId)
  end
end


function truck.selectTruckIOID()
  local tmpTruckId
  local isEnterKey
  local curActVal = awtx.weight.getActiveValue()
  
  if curActVal ~= awtx.weight.VAL_GROSS then
    awtxReq.display.displayCant()
  else
    -- Removed by Matt Burkett on 05/27/2016 because it was annoying and served
    -- no purpose.
    --awtx.display.writeLine("TruckId")
    --awtx.os.sleep(busytime)
    --awtx.display.writeLine("")
    curTruck.truckId = ""
    tmpTruckId, isEnterKey = awtx.keypad.enterString(curTruck.truckId, TRUCK_ID_LENGTH, promptTimeout, "ID:", "Enter")
    if isEnterKey then
      truck.setIOId(tmpTruckId)
    end
  end
end

function truck.formatDate(dateTimeArg)
  local tmpDateTime
  local formattedDate
  if dateTimeArg ~= nil then
    tmpDateTime = os.date("*t", dateTimeArg)
  else
    tmpDateTime = os.date("*t")
  end
  
  if config.dateFormat == nil then
    formattedDate = string.format("%02d-%02d-%02d", tmpDateTime.month, tmpDateTime.day, tmpDateTime.year)
  else
    if config.dateFormat == 0 then  --"MMDD2Y"
      formattedDate = string.format("%02d-%02d-%02d", tmpDateTime.month, tmpDateTime.day, tmpDateTime.year)
    elseif config.dateFormat == 1 then  --"MMDD4Y"
      formattedDate = string.format("%02d-%02d-%04d", tmpDateTime.month, tmpDateTime.day, tmpDateTime.year)
    elseif config.dateFormat == 2 then  --"DDMM2Y"
      formattedDate = string.format("%02d-%02d-%02d", tmpDateTime.day, tmpDateTime.month, tmpDateTime.year)
    elseif config.dateFormat == 3 then  --"DDMM4Y"
      formattedDate = string.format("%02d-%02d-%04d", tmpDateTime.day, tmpDateTime.month, tmpDateTime.year)
    else
      formattedDate = string.format("%02d-%02d-%02d", tmpDateTime.month, tmpDateTime.day, tmpDateTime.year)
    end
  end

  return formattedDate
end


function truck.formatTime(dateTimeArg)
  local tmpDateTime
  local formattedTime
  if dateTimeArg ~= nil then
    tmpDateTime = os.date("*t", dateTimeArg)
  else
    tmpDateTime = os.date("*t")
  end
  
  if config.timeFormat == nil then
    formattedTime = string.format("%02d:%02d:%02d", tmpDateTime.hour, tmpDateTime.min, tmpDateTime.sec)
  else
    if config.timeFormat == 0 then  --"12HR"
      if tmpDateTime.hour > 12 then
        tmpDateTime.hour = tmpDateTime.hour - 12
      end
      formattedTime = string.format("%02d:%02d:%02d", tmpDateTime.hour, tmpDateTime.min, tmpDateTime.sec)
    elseif config.timeFormat == 1 then  --"12HR-AP"
      APstr = "A"
      if tmpDateTime.hour > 12 then
        tmpDateTime.hour = tmpDateTime.hour - 12
        APstr = "P"
      end
      formattedTime = string.format("%02d:%02d:%02d %s", tmpDateTime.hour, tmpDateTime.min, tmpDateTime.sec, APstr)
    elseif config.timeFormat == 2 then  --"24HR"
      formattedTime = string.format("%02d:%02d:%02d", tmpDateTime.hour, tmpDateTime.min, tmpDateTime.sec)
    else
      formattedTime = string.format("%02d:%02d:%02d", tmpDateTime.hour, tmpDateTime.min, tmpDateTime.sec)
    end
  end

  return formattedTime
end
 
function truck.truckReport(label)
  local usermode
  local currentRPN
  local tmpreport
  local tmpreport1
  local isEnterKey
  local usermode
  
usermode = awtx.display.setMode(awtx.display.MODE_MENU)

  awtx.display.setMode(awtx.display.MODE_MENU)
  
  --select which report
  tmpreport, isEnterKey = awtx.keypad.selectList(reportListSelection, 0)

  if isEnterKey then
    if tmpreport == 0 then
      -- reprint
      awtxReq.display.displayWord("Printng")
      truck.recallReprintData()
      truck.setPrintTokens()
      awtx.printer.printFmt(truck.reprintFmt)
      awtx.printer.printFmt(40)
      awtx.display.setMode(awtx.display.MODE_SCALE)
    else
      -- print the correct report
      tmpreport1 = tmpreport - 1    -- because reprint is first we have to remove one
      truck.DBReport("menu", tmpreport1)
    end
  end
  awtx.display.setMode(usermode)
end


function truck.menuReport(label)
  local usermode
  local tmpreport
  local isEnterKey
  
  usermode = awtx.display.setMode(awtx.display.MODE_MENU)

  --select which report
  tmpreport, isEnterKey = awtx.keypad.selectList(menuprintListSelection, 0)
  if isEnterKey then
    -- print the correct report
    truck.DBReport(menuprintLabelList[tmpreport], tmpreport)
  end
  
  awtx.display.setMode(usermode)
end


function truck.DBReport(label, reportType)
  local index
  local reportTable = {}
  
  awtx.display.writeLine(label)

  awtx.display.setDisplayBusy()
  awtx.os.sleep(busytime)

  newTicketDate = truck.formatDate()
  newTicketTime = truck.formatTime()
  truck.setPrintTokens()

  -- Print Header
  if reportType == REPORT_TYPE_IN then
    awtx.printer.reportFmt(TruckConfigTable.InHeaderFmt.value, 1)
    reportTable = awtxReq.database.recallAllRecords(truck.IOTableName)
  elseif reportType == REPORT_TYPE_OUT then
    awtx.printer.reportFmt(TruckConfigTable.OutHeaderFmt.value, 1)
    reportTable = awtxReq.database.recallAllRecords(truck.IOTableName)
  elseif reportType == REPORT_TYPE_FLT then
    awtx.printer.reportFmt(TruckConfigTable.FleetHeaderFmt.value, 1)
    reportTable = awtxReq.database.recallAllRecords(truck.FleetTableName)
  end

  
  -- Print Body
  for index = 1, #reportTable do
    if (not((reportType == REPORT_TYPE_IN) and (reportTable[index].weight1 == 0))) then
      if ((reportType == REPORT_TYPE_FLT)) then
        curTruck.truckId     = reportTable[index].truckId
        curTruck.tare        = reportTable[index].tare
        curTruck.weight1     = 0
        curTruck.weight1date = " "
        curTruck.weight1time = " "
        curTruck.grossTotal  = reportTable[index].grossTotal
        curTruck.netTotal    = reportTable[index].netTotal
        curTruck.tareTotal   = reportTable[index].tareTotal
        curTruck.transCount  = reportTable[index].transCount
        curTruck.units       = reportTable[index].units
      else
        curTruck.truckId     = reportTable[index].truckId
        curTruck.weight1     = reportTable[index].weight1
        curTruck.weight1date = reportTable[index].weight1date
        curTruck.weight1time = reportTable[index].weight1time
        curTruck.grossTotal  = reportTable[index].grossTotal
        curTruck.netTotal    = reportTable[index].netTotal
        curTruck.tareTotal   = reportTable[index].tareTotal
        curTruck.transCount  = reportTable[index].transCount
        curTruck.units       = reportTable[index].units
      end
    
      if reportType == REPORT_TYPE_IN then
          truck.setPrintTokens()
          awtx.printer.reportFmt(TruckConfigTable.InBodyFmt.value, 2)
      elseif reportType == REPORT_TYPE_OUT then
          truck.setPrintTokens()
          awtx.printer.reportFmt(TruckConfigTable.OutBodyFmt.value, 2)
      elseif reportType == REPORT_TYPE_FLT then
          truck.setPrintTokens()
          awtx.printer.reportFmt(TruckConfigTable.FleetBodyFmt.value, 2)
      end
    end
  end

  -- Print Footer
  if reportType == REPORT_TYPE_IN then
    awtx.printer.reportFmt(TruckConfigTable.InFooterFmt.value, 3)
  elseif reportType == REPORT_TYPE_OUT then
    awtx.printer.reportFmt(TruckConfigTable.OutFooterFmt.value, 3)
  elseif reportType == REPORT_TYPE_FLT then
    awtx.printer.reportFmt(TruckConfigTable.FleetFooterFmt.value, 3)
  end

  awtx.os.sleep(busytime)

  truck.setPrintTokens()
  awtx.display.clrDisplayBusy()
  if label == "menu" then
    awtx.display.setMode(awtx.display.MODE_SCALE)
  end

end


function truck.inReset(label)
  local usermode
  local index
  local isEnterKey
  
  index, isEnterKey = awtx.keypad.selectList("No,Yes", 0)
  if isEnterKey and index == 1 then
    usermode = awtx.display.setMode(awtx.display.MODE_MENU)
    awtx.display.setDisplayBusy()
    awtx.os.sleep(busytime)  -- leave Busy there for a second so they see it
    truck.inClearAll()
    awtx.display.clrDisplayBusy()
    awtx.display.setMode(usermode)
  end
end


function truck.ioReset(label)
  local usermode
  local index
  local isEnterKey
  
  index, isEnterKey = awtx.keypad.selectList("No,Yes", 0)
  if isEnterKey and index == 1 then
    usermode = awtx.display.setMode(awtx.display.MODE_MENU)
    awtx.display.setDisplayBusy()
    awtx.os.sleep(busytime)  -- leave Busy there for a second so they see it
    truck.ioDeleteAll()
    awtx.display.clrDisplayBusy()
    awtx.display.setMode(usermode)
  end
end


function truck.fleetReset(label)
  local usermode
  local index
  local isEnterKey
  
  index, isEnterKey = awtx.keypad.selectList("No,Yes", 0)
  if isEnterKey and index == 1 then
    usermode = awtx.display.setMode(awtx.display.MODE_MENU)
    awtx.display.setDisplayBusy()
    awtx.os.sleep(busytime)  -- leave Busy there for a second so they see it
    truck.fleetDeleteAll()
    awtx.display.clrDisplayBusy()
    awtx.display.setMode(usermode)
  end
end

function truck.allReset(label)
  local usermode
  local index
  local isEnterKey
  
  index, isEnterKey = awtx.keypad.selectList("No,Yes", 0)
  if isEnterKey and index == 1 then
    usermode = awtx.display.setMode(awtx.display.MODE_MENU)
    awtx.display.setDisplayBusy()
    awtx.os.sleep(busytime)  -- leave Busy there for a second so they see it
    truck.allDelete()
    awtx.display.clrDisplayBusy()
    awtx.display.setMode(usermode)
  end
end

function truck.waitForMotion()
  local loop = 0
  local curScale = awtx.weight.getCurrent(1)
  
  while curScale.motion do
    awtx.os.sleep(50)
    loop = loop + 1
    if loop > 100 then
      break
    end
    curScale = awtx.weight.getCurrent(1)
  end
  return curScale.motion, curScale.gross
end



function truck.importGTN(label)
  local errorCode
  local file
  local numRecords = 0
  local curRecord = {}
  local index
  local isEnterKey
  
  index, isEnterKey = awtx.keypad.selectList("No,Yes", 0, "","import?")
  if (not isEnterKey) or (index == 0) then
    return
  end
  
  awtx.display.writeLine("Busy")

  file, errorCode = io.open("G:\\FLEET.csv", "r")
  
  if errorCode == nil then
    local nextCommaIndex = -1
    
    -- Delete the old Trucks
    truck.fleetDeleteAll()
    
    
    for line in file:lines() do
      curRecord = {}
      
      -- only allow the first MAX number to import
      if numRecords >= truck.MAX_NUMBER_FLEET then
        numRecords = numRecords + 1
        break
      end
      numRecords = numRecords + 1
      
      
      line = string.gsub(line, '\n', ',')
      local startingIndex = 0
      nextCommaIndex = string.find(line, ',', startingIndex)
      
      --ID
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        table.insert(curRecord, currentInfo)
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      --Tare
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        table.insert(curRecord, tonumber(currentInfo))
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      -- GrossTotal
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        table.insert(curRecord, tonumber(currentInfo))
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      --NetTotal
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        table.insert(curRecord, tonumber(currentInfo))
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      --TareTotal
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        table.insert(curRecord, tonumber(currentInfo))
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      --transaction counter
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        --TruckTable[tableIndex].grossTotal.value = tonumber(currentInfo)
        table.insert(curRecord, tonumber(currentInfo))
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      --units
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        --TruckTable[tableIndex].netTotal.value = tonumber(currentInfo)
        table.insert(curRecord, currentInfo)
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = 0
      end
      
      if nextCommaIndex ~= 0 then
        break
      end
      -- add to the Fleet table
      awtxReq.database.addUpdateRecord(curRecord, truck.FleetTableName)  
    end
    
    file:close()
    
    if nextCommaIndex ~= 0 then
      awtxReq.display.displayWord(" FLEET", 2000)
      awtxReq.display.displayWord("ParseFail", 2000)
      return
    elseif numRecords > truck.MAX_NUMBER_FLEET then
      awtxReq.display.displayWord("FLT FUL")
      return
    end
  else
    awtxReq.display.displayWord(" FLEET", 2000)
    awtxReq.display.displayWord("ReadFail", 2000)
    return
  end
  
  
  file, errorCode = io.open("G:\\INOUT.csv", "r")
  
  if errorCode == nil then
    local nextCommaIndex = -1
    
    -- Delete the old Trucks
    truck.ioDeleteAll()
    
    numRecords = 0
    for line in file:lines() do
      curRecord = {}
      -- only allow the first MAX number to import
      if numRecords >= truck.MAX_NUMBER_IO then
        numRecords = numRecords + 1
        break
      end
      numRecords = numRecords + 1
      
      
      line = string.gsub(line, '\n', ',')
      local startingIndex = 0
      nextCommaIndex = string.find(line, ',', startingIndex)
      
      --ID
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        table.insert(curRecord, currentInfo)
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      --Weight1
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        table.insert(curRecord, tonumber(currentInfo))
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      --Weight1date
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        if currentInfo == nil or currentInfo == "" then
          currentInfo = " "
        end        
        table.insert(curRecord, currentInfo)
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      --Weight1time
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        if currentInfo == nil or currentInfo == "" then
          currentInfo = " "
        end        
        table.insert(curRecord, currentInfo)
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      -- GrossTotal
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        table.insert(curRecord, tonumber(currentInfo))
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      --NetTotal
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        table.insert(curRecord, tonumber(currentInfo))
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      --TareTotal
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        table.insert(curRecord, tonumber(currentInfo))
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      --transaction counter
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        --TruckTable[tableIndex].grossTotal.value = tonumber(currentInfo)
        table.insert(curRecord, tonumber(currentInfo))
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = string.find(line, ',', startingIndex)
      end
      
      --units
      if nextCommaIndex then
        local currentInfo = string.sub(line, startingIndex, nextCommaIndex - 1)
        --TruckTable[tableIndex].netTotal.value = tonumber(currentInfo)
        table.insert(curRecord, currentInfo)
        startingIndex = nextCommaIndex + 1
        nextCommaIndex = 0
      end
      
      if nextCommaIndex ~= 0 then
        break
      end
      -- add to the Fleet table
      awtxReq.database.addUpdateRecord(curRecord, truck.IOTableName)  
    end
    
    file:close()
    
    if nextCommaIndex ~= 0 then
      awtxReq.display.displayWord("  IO ", 2000)
      awtxReq.display.displayWord("ParseFail", 2000)
      return
    elseif numRecords > truck.MAX_NUMBER_IO then
      awtxReq.display.displayWord("IO FULL", 2000)
      return
    end
  else
    awtx.display.writeLine(" IO ", 2000)
    awtxReq.display.displayWord("ReadFail", 2000)
    return
  end

  awtxReq.display.displayWord(" DONE", 2000)

end


function truck.exportGTN(label)
  local errorCode, file
  local result = 0
  local reportTable = {}
  awtx.display.writeLine("Busy")
    
  file, errorCode = io.open("G:\\FLEET.csv", "w+")
  
    reportTable = awtxReq.database.recallAllRecords(truck.FleetTableName)
  
  if errorCode == nil then
    for index = 1, #reportTable do
        local outputString = ""
        
        outputString = string.format("%s", reportTable[index].truckId)
        outputString = string.format("%s,%f", outputString, reportTable[index].tare)
        outputString = string.format("%s,%f", outputString, reportTable[index].grossTotal)
        outputString = string.format("%s,%f", outputString, reportTable[index].netTotal)
        outputString = string.format("%s,%f", outputString, reportTable[index].tareTotal)
        outputString = string.format("%s,%d", outputString, reportTable[index].transCount)
        outputString = string.format("%s,%s\n", outputString, reportTable[index].units)
    
        file:write(outputString)
    end

    file:close()
    file, errorCode = io.open("G:\\INOUT.csv", "w+")
    
      reportTable = awtxReq.database.recallAllRecords(truck.IOTableName)
    
    if errorCode == nil then
      for index = 1, #reportTable do
          local outputString = ""
          
          outputString = string.format("%s", reportTable[index].truckId)
          outputString = string.format("%s,%f", outputString, reportTable[index].weight1)
          outputString = string.format("%s,%s", outputString, reportTable[index].weight1date)
          outputString = string.format("%s,%s", outputString, reportTable[index].weight1time)
          outputString = string.format("%s,%f", outputString, reportTable[index].grossTotal)
          outputString = string.format("%s,%f", outputString, reportTable[index].netTotal)
          outputString = string.format("%s,%f", outputString, reportTable[index].tareTotal)
          outputString = string.format("%s,%d", outputString, reportTable[index].transCount)
          outputString = string.format("%s,%s\n", outputString, reportTable[index].units)
      
          file:write(outputString)
      end
      file:close()
      awtxReq.display.displayWord(" DONE", 2000)
    else
      awtxReq.display.displayWord(errorCode, 2000)
    end
  else
    awtxReq.display.displayWord(errorCode, 2000)
  end
end

