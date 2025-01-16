--[[
*******************************************************************************

Filename:      Application.lua
Version:       1.0.0.0
Firmware:      2.2.0.0  or higher
Date:          2015-03-01
Customer:      Avery Weigh-Tronix
Description:
This lua applications file provides Truck IO application functionality.

*******************************************************************************

*******************************************************************************
]]

--create the awtxReq namespace
awtxReq = {}

require("awtxReqConstants")
require("awtxReqVariables")

--Global Memory Sentinel ... Define this in your app to a different value to clear
-- the Variable table out.
MEMORYSENTINEL = "A15_120001032016" 
MemorySentinel = awtxReq.variables.SavedVariable('MemorySentinel', "0", true)
-- if the memory sentinel has changed clear out the variable tables.
if MemorySentinel.value ~= MEMORYSENTINEL then    
    awtx.variables.clearTable()  -- Clears everything from the standard Variable table
    awtx.variables.clearTable("tblTruckConfig")  
    MemorySentinel.value = MEMORYSENTINEL
end


system = awtx.hardware.getSystem(1) -- Used to identify current hardware type.
config = awtx.weight.getConfig(1)   -- Used to get current system configuration information.
wt = awtx.weight.getCurrent(1)      -- Used to hold current scale snapshot information.

--create the truck namespace
truck = {}

promptTimeout = 20000
busytime = 2000
TRUCK_ID_LENGTH = 7
MinWeight = 2000

local printRTZflag = false;


require("awtxReqDisplayMessages")   -- Provides display message support
require("awtxReqAppMenu")         -- ReqTare is dependent on this
require("awtxReqScaleKeys")       
require("awtxReqScaleKeysEvents")
require("awtxReqRpnEntry")

require("ReqTruck")

-- AppName is displayed when escaping from password entry and entering a password of '0'
AppName = "TRUCKIO"

--------------------------------------------------------------------------------------------------
--  Application Functions
--------------------------------------------------------------------------------------------------

--[[
function create
Description:
  This function execute on start-up.
  It calls functions to be executed when the application starts.

Parameters:
  None

Returns:
  None
]]
local function create()
  --Display the App name on power up
  awtx.display.writeLine(AppName,1000)
  
  truck.curTruckInit()
  truck.DBInit()
  truck.CFGInit()          --initialize the configuration
  truck.configRecall()
  
-- recall the current channels values if there is one

  if TruckConfigTable.CurrentChannelType.value == truck.TRUCK_TYPE_INOUT then
    truck.recallIOId(TruckConfigTable.CurrentChannel.value)
  elseif TruckConfigTable.CurrentChannelType.value == truck.TRUCK_TYPE_FLEET then
    truck.recallFleetId(TruckConfigTable.CurrentChannel.value)      
  end

  truck.setPrintTokens()
  
  --Need to Register to overrided Print Complete Event
  awtx.weight.registerPrintCompleteEvent(onPrintCompleteEvent)
  awtx.setpoint.registerOutputEvent(6,lightStatus)     -- Allows this setpoint to be called from the operating system level
  awtx.setpoint.registerOutputEvent(15,printRTZ)       -- Allows this setpoint to be called from the operating system level
   
  --put setpoints in the correct state on powerup
  if TruckConfigTable.LiteEnableFlag.value == 0 then  --Off
     truck.setLiteOff()
  elseif TruckConfigTable.LiteEnableFlag.value == 1 then  --Manual
     truck.setLiteRed()
  else                                                     --Automatic=2  Both =3
     truck.setLiteGreen()
  end
end

--[[
function printRTZ
Description:
  This function is the callback function for Setpoint 15.  
  It handles the Print Return to Zero re-enable

Parameters:
  number - setpoint number
  newState - the state the setpoint is in
Returns:
  None
]]
function printRTZ (number, newState)
  if newState == true then
     printRTZflag = false
  end
end

--[[
function lightStatus
Description:
  This function is the callback function for Setpoint 15.  
  Handles the status of the Light (green/red) for auto/both mode
Parameters:
  number - setpoint number
  newState - the state the setpoint is in
Returns:
  None
]]
function lightStatus(number,state)
  if TruckConfigTable.LiteEnableFlag.value >= 2 then      --Automatic=2 Both=3
     if state then
        truck.setLiteGreen()
     else
        truck.setLiteRed()
     end
  end
end


--------------------------------------------------------------------------------------------------
--  Application Event Handlers
--------------------------------------------------------------------------------------------------
-- Set F1 Key Hold functionality to add fleet clear.
function awtxReq.keypad.onF1KeyHold()
  
  -- Commented Out by Matt Burkett on 05/26/2016
  --if truck.getTruckPrintType() == truck.TRUCK_TYPE_INOUT then
  --  truck.setTruckPrintType(truck.TRUCK_TYPE_NONE)
  --  awtxReq.display.displayWord(" IO CLR")
  --else
  --  awtxReq.display.displayCant()
  --end
end


-- Set F1 Key functionality.
function awtxReq.keypad.onF1KeyUp()
  
  wt = awtx.weight.getCurrent(1)
  if (wt.gross > MinWeight) then
      
    local curMode = awtx.display.setMode(awtx.display.MODE_MENU) 
    -- Select the truck IO ID
    truck.selectTruckIOID()
    awtx.display.setMode(curMode)
    -- New code by Matt
    --if printRTZflag == true then
    --  awtxReq.display.displayCant()
    --else
    
    if truck.getTruckPrintType() == truck.TRUCK_TYPE_INOUT then
       curActVal = awtx.weight.getActiveValue()
       --if curActVal ~= awtx.weight.VAL_GROSS then
       --   awtxReq.display.displayCant()
       --else
        awtx.weight.requestPrint()
       --end
    elseif truck.getTruckPrintType() == truck.TRUCK_TYPE_FLEET then
       awtx.weight.requestPrint()
         
    --else
    --   awtx.weight.requestPrint()
    end
  
  else
    awtxReq.display.displayWord("NoTruck")
  end
end


-- Override the Tare Key Down functionality.
function awtxReq.keypad.onTareKeyDown()
  if truck.getTruckPrintType() == truck.TRUCK_TYPE_NONE then
    if not config.presetTareFlag then
      if config.pbTareFlag then
        awtx.weight.requestTare()
      else        
        awtxReq.display.displayCant()
      end
    end
  else
    awtxReq.display.displayCant()
  end
end


-- Override the Tare Key Hold functionality.
function awtxReq.keypad.onTareKeyHold()
  if truck.getTruckPrintType() == truck.TRUCK_TYPE_NONE then
    awtx.weight.requestTareClear()
    awtxReq.display.displayCleared()
  else
    awtxReq.display.displayCant()
  end
  
end

-- Override the Tare Key Up functionality.
function awtxReq.keypad.onTareKeyUp()
  if truck.getTruckPrintType() == truck.TRUCK_TYPE_NONE then
    --Push button tare has to happen on the down but keypad and preset can happen on the up
  else
    awtxReq.display.displayCant()
  end
end


-- Override the Select Key Up functionality.
function awtxReq.keypad.onSelectKeyUp()
  if truck.getTruckPrintType() == truck.TRUCK_TYPE_NONE then
    awtx.weight.cycleActiveValue()
  else
    awtxReq.display.displayCant()
  end
end


-- Override the Print Key Down functionality.
function awtxReq.keypad.onPrintKeyDown()
  if printRTZflag == true then
    awtxReq.display.displayCant()
  else
    wt = awtx.weight.getCurrent(1)
    if not wt.inGrossBand then
      if truck.getTruckPrintType() == truck.TRUCK_TYPE_INOUT then
         curActVal = awtx.weight.getActiveValue()
         if curActVal ~= awtx.weight.VAL_GROSS then
            awtxReq.display.displayCant()
         else
            awtx.weight.requestPrint()
         end
      elseif truck.getTruckPrintType() == truck.TRUCK_TYPE_FLEET then
         awtx.weight.requestPrint()
      else
         awtx.weight.requestPrint()
      end
    end
  end
end


-- Override the Start Key Up functionality to add light operation.
function awtxReq.keypad.onStartKeyUp()
  if TruckConfigTable.LiteEnableFlag.value == 0 then  --Manual
     truck.setLiteOff()
  elseif TruckConfigTable.LiteEnableFlag.value ~= 2 then  --Auto
     truck.setLiteGreen()    
  end
end


-- Override the Stop Key Up functionality to add light operation.
function awtxReq.keypad.onStopKeyUp()
  if TruckConfigTable.LiteEnableFlag.value == 0 then  --Manual
     truck.setLiteOff()
  elseif TruckConfigTable.LiteEnableFlag.value ~= 2 then  --Auto
     truck.setLiteRed()    
  end
end


-- Override the Sample Key Hold functionality to add fleet clear.
function awtxReq.keypad.onSampleKeyHold()
  if truck.getTruckPrintType() == truck.TRUCK_TYPE_FLEET then
    truck.setTruckPrintType(truck.TRUCK_TYPE_NONE)
    awtxReq.display.displayWord("FLT CLR")
    --truck.setPrintTokens()
    awtx.weight.requestPresetTare(0);
  else
    awtxReq.display.displayCant()
  end
end


-- Override the Target Key Hold functionality to add target display.
function awtxReq.keypad.onSampleKeyUp()
  -- Select the truck fleet ID
  local curMode = awtx.display.setMode(awtx.display.MODE_MENU) 
  truck.selectTruckFleetID()
  curMode = awtx.display.setMode(curMode)
end


-- Override the Target Key Up functionality to print truck report.
function awtxReq.keypad.onTargetKeyUp()
  -- Print the truck report
  truck.truckReport("Report")
end

-- Override the Print Complete event.
function onPrintCompleteEvent(eventResult, eventResultString)
  if ((eventResult ~= 0)) then
    awtxReq.display.displayCant()
  else
    printRTZflag = true;
    wt = awtx.weight.getLastPrint()
    if truck.getTruckPrintType() == truck.TRUCK_TYPE_NONE then
       awtx.printer.printFmt(0)
    elseif truck.getTruckPrintType() == truck.TRUCK_TYPE_INOUT then
       truck.printTruckIO()
    elseif truck.getTruckPrintType() == truck.TRUCK_TYPE_FLEET then
       truck.printTruckFleet()
    end
    if TruckConfigTable.LiteEnableFlag.value >= 2 then  --Automatic
       truck.setLiteGreen()
    end
  end
end

  
--------------------------------------------------------------------------------------------------
--  Super Menu
--------------------------------------------------------------------------------------------------
-- Top level Menu
TopMenu1 = {text = "Super", key = 1, action = "MENU", variable = "SuperMenu"}
TopMenu2 = {text = "EXIT",  key = 2, action = "FUNC", callThis = supervisor.SupervisorMenuExit}
TopMenu = {TopMenu1, TopMenu2}

-- These lines are needed to construct the top layer of the Supervisor Menu
-- As more menus are added through require files, this layer will grow to include them.
SuperMenu  = { }

-- Need this to turn the table string names into the table addresses
generalMenu =
{
  TopMenu = TopMenu,
    SuperMenu = SuperMenu,
}

--------------------------------------------------------------------------------------------------
--  Application Menu
--------------------------------------------------------------------------------------------------

-- Truck Configuration Menu Structure
TruckSetupMenu1 = {text = " Edit  ", key = 1, action = "MENU", variable = "TruckEditMenu"}
TruckSetupMenu2 = {text = " Lite  ", key = 2, action = "MENU", variable = "TruckLiteMenu"}
TruckSetupMenu3 = {text = " Print ", key = 3, action = "FUNC", callThis = truck.menuReport}
TruckSetupMenu4 = {text = " Import", key = 4, action = "FUNC", callThis = truck.importGTN}
TruckSetupMenu5 = {text = " Export", key = 5, action = "FUNC", callThis = truck.exportGTN}
TruckSetupMenu6 = {text = " Reset ", key = 6, action = "MENU", variable = "TruckResetMenu"}
TruckSetupMenu7 = {text = " BACK  ", key = 7, action = "MENU", variable = "SuperMenu", subMenu = 3}
TruckSetupMenu  = {TruckSetupMenu1, TruckSetupMenu2, TruckSetupMenu3, TruckSetupMenu4, TruckSetupMenu5, TruckSetupMenu6, TruckSetupMenu7}

TruckEditMenu1 = {text = "  In   ", key = 1, action = "FUNC", callThis = truck.editTruckInTruckId}
TruckEditMenu2 = {text = "  IO   ", key = 2, action = "FUNC", callThis = truck.editTruckIOTruckId}
TruckEditMenu3 = {text = " Fleet ", key = 3, action = "FUNC", callThis = truck.editTruckFleetTruckId}
TruckEditMenu4 = {text = " Report", key = 4, action = "MENU", variable = "TruckEditReportMenu"}
TruckEditMenu5 = {text = " BACK  ", key = 5, action = "MENU", variable = "TruckSetupMenu", subMenu = 1}
TruckEditMenu  = {TruckEditMenu1, TruckEditMenu2, TruckEditMenu3, TruckEditMenu4, TruckEditMenu5}

TruckEditInMenu1 = {text = " Clear ", key = 1, action = "FUNC", callThis = truck.clrTruckInWeight}
TruckEditInMenu2 = {text = " BACK  ", key = 2, action = "MENU", variable = "TruckEditMenu", subMenu = 1}
TruckEditInMenu  = {TruckEditInMenu1, TruckEditInMenu2}

TruckEditIOMenu1 = {text = "Delete ", key = 1, action = "FUNC", callThis = truck.delTruckIdIO}
TruckEditIOMenu2 = {text = " BACK  ", key = 2, action = "MENU", variable = "TruckEditMenu", subMenu = 2}
TruckEditIOMenu  = {TruckEditIOMenu1, TruckEditIOMenu2}

TruckEditFleetMenu1 = {text = " Tare  ", key = 1, action = "FUNC", callThis = truck.editTareFleet}
TruckEditFleetMenu2 = {text = "Delete ", key = 2, action = "FUNC", callThis = truck.delIdFleet}
TruckEditFleetMenu3 = {text = " BACK  ", key = 3, action = "MENU", variable = "TruckEditMenu", subMenu = 3}
TruckEditFleetMenu  = {TruckEditFleetMenu1, TruckEditFleetMenu2, TruckEditFleetMenu3}

TruckEditReportMenu1 = {text = " InFmt ", key = 1, action = "MENU", variable = "TruckInFmtMenu"}
TruckEditReportMenu2 = {text = " OutFmt", key = 2, action = "MENU", variable = "TruckOutFmtMenu"}
TruckEditReportMenu3 = {text = " FltFmt", key = 3, action = "MENU", variable = "TruckFltFmtMenu"}
TruckEditReportMenu4 = {text = " BACK  ", key = 4, action = "MENU", variable = "TruckEditMenu", subMenu = 4}
TruckEditReportMenu  = {TruckEditReportMenu1, TruckEditReportMenu2, TruckEditReportMenu3, TruckEditReportMenu4}

TruckInFmtMenu1 = {text = "In Tick", key = 1, action = "FUNC", callThis = truck.editInFmt}
TruckInFmtMenu2 = {text = "In Head", key = 2, action = "FUNC", callThis = truck.editInHeaderFmt}
TruckInFmtMenu3 = {text = "In Body", key = 3, action = "FUNC", callThis = truck.editInBodyFmt}
TruckInFmtMenu4 = {text = "In Foot", key = 4, action = "FUNC", callThis = truck.editInFooterFmt}
TruckInFmtMenu5 = {text = " BACK  ", key = 5, action = "MENU", variable = "TruckEditReportMenu", subMenu = 1}
TruckInFmtMenu  = {TruckInFmtMenu1, TruckInFmtMenu2, TruckInFmtMenu3, TruckInFmtMenu4, TruckInFmtMenu5}

TruckOutFmtMenu1 = {text = "OutTick", key = 1, action = "FUNC", callThis = truck.editOutFmt}
TruckOutFmtMenu2 = {text = "OutHead", key = 2, action = "FUNC", callThis = truck.editOutHeaderFmt}
TruckOutFmtMenu3 = {text = "OutBody", key = 3, action = "FUNC", callThis = truck.editOutBodyFmt}
TruckOutFmtMenu4 = {text = "OutFoot", key = 4, action = "FUNC", callThis = truck.editOutFooterFmt}
TruckOutFmtMenu5 = {text = " BACK  ", key = 5, action = "MENU", variable = "TruckEditReportMenu", subMenu = 2}
TruckOutFmtMenu  = {TruckOutFmtMenu1, TruckOutFmtMenu2, TruckOutFmtMenu3, TruckOutFmtMenu4, TruckOutFmtMenu5}

TruckFltFmtMenu1 = {text = "FltTick", key = 1, action = "FUNC", callThis = truck.editFleetFmt}
TruckFltFmtMenu2 = {text = "FltHead", key = 2, action = "FUNC", callThis = truck.editFleetHeaderFmt}
TruckFltFmtMenu3 = {text = "FltBody", key = 3, action = "FUNC", callThis = truck.editFleetBodyFmt}
TruckFltFmtMenu4 = {text = "FltFoot", key = 4, action = "FUNC", callThis = truck.editFleetFooterFmt}
TruckFltFmtMenu5 = {text = " BACK  ", key = 5, action = "MENU", variable = "TruckEditReportMenu", subMenu = 3}
TruckFltFmtMenu  = {TruckFltFmtMenu1, TruckFltFmtMenu2, TruckFltFmtMenu3, TruckFltFmtMenu4, TruckFltFmtMenu5}

TruckLiteMenu1 = {text = " Enable",  key = 1, action = "FUNC", callThis = truck.editLiteEnable}
TruckLiteMenu2 = {text = " T-Hold",  key = 2, action = "FUNC", callThis = truck.editLiteTHold}
TruckLiteMenu3 = {text = " BACK  ",  key = 3, action = "MENU", variable = "TruckSetupMenu", subMenu = 2}
TruckLiteMenu  = {TruckLiteMenu1, TruckLiteMenu2, TruckLiteMenu3}

TruckResetMenu1 = {text = "  In   ", key = 1, action = "FUNC", callThis = truck.inReset}
TruckResetMenu2 = {text = "  IO   ", key = 2, action = "FUNC", callThis = truck.ioReset}
TruckResetMenu3 = {text = " Fleet ", key = 3, action = "FUNC", callThis = truck.fleetReset}
TruckResetMenu4 = {text = "  All  ", key = 4, action = "FUNC", callThis = truck.allReset}
TruckResetMenu5 = {text = " BACK  ", key = 5, action = "MENU", variable = "TruckSetupMenu", subMenu = 6}
TruckResetMenu  = {TruckResetMenu1, TruckResetMenu2, TruckResetMenu3, TruckResetMenu4, TruckResetMenu5}

truckMenu =
{
  TruckSetupMenu = TruckSetupMenu,
  TruckEditMenu = TruckEditMenu,
  TruckEditInMenu = TruckEditInMenu,
  TruckEditIOMenu = TruckEditIOMenu,
  TruckEditFleetMenu = TruckEditFleetMenu,
  TruckEditReportMenu = TruckEditReportMenu,
  TruckInFmtMenu = TruckInFmtMenu,
  TruckOutFmtMenu = TruckOutFmtMenu,
  TruckFltFmtMenu = TruckFltFmtMenu,
  TruckLiteMenu = TruckLiteMenu,
  TruckResetMenu = TruckResetMenu
}


if truckMenu ~= nil then
  local curIndex = #SuperMenu + 1
  SuperMenu[curIndex] = {text = " TRUCK ",  key = curIndex, action = "MENU", variable = "TruckSetupMenu" }
  TruckSetupMenu[#TruckSetupMenu].subMenu = curIndex  -- Set the return index to Super Menu Truck selection
  for k, v in pairs(truckMenu) do generalMenu[k] = v end
end



--------------------------------------------------------------------------------------------------
--  Menu Builder
--------------------------------------------------------------------------------------------------
-- This line closes out the Super Menu level and enables UP functionality from lower level menus.
local curIndex = #SuperMenu + 1
SuperMenu[curIndex] = {text = " BACK  ",  key = curIndex, action = "MENU", variable = "TopMenu", subMenu = 1}


-- Function override from ReqAppMenu.lua
-- This function is called when the Supervisor menu is entered.
function appEnterSuperMenu()
  -- abort any transaction that was started ...
  truck.setTruckPrintType(truck.TRUCK_TYPE_NONE)
  awtx.weight.requestPresetTare(0);

  
  truck.setLiteOff()
  supervisor.menuLevel    = TopMenu         -- Set current menu level
  supervisor.menuCircular = generalMenu     -- Set menu address table
end


-- Function override from ReqAppMenu.lua
-- This function is called when the Supervisor menu is exited.
function appExitSuperMenu()
  -- abort any transaction that was started ...
  truck.setTruckPrintType(truck.TRUCK_TYPE_NONE)
  awtx.weight.requestPresetTare(0);

  --put setpoints in the correct state after exit menu
  if TruckConfigTable.LiteEnableFlag.value == 0 then  --Off
     truck.setLiteOff()
  elseif TruckConfigTable.LiteEnableFlag.value == 1 then  --Manual
     truck.setLiteRed()
  elseif TruckConfigTable.LiteEnableFlag.value >= 2 then  --Automatic=2 Both=3
     truck.setLiteGreen()
  end
end


-- Final function call executes the application specific create function to perform application initialization.
create()