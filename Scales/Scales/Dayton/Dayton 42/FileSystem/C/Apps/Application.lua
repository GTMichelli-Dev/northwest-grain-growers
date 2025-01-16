--[[
*******************************************************************************

Filename:      Application.lua
Version:       1.0.0.2
Date:          2015-09-01
Customer:      Avery Weigh-Tronix
Description:
This lua application file provides basic general weighing functionality.

*******************************************************************************
]]
--create the awtxReq namespace
awtxReq = {}

require("awtxReqConstants")
require("awtxReqVariables")

--Global Memory Sentinel ... Define this in your app to a different value to clear
-- the Variable table out.
MEMORYSENTINEL = "A5A520150800" 
MemorySentinel = awtxReq.variables.SavedVariable('MemorySentinel', "0", true)
-- if the memory sentinel has changed clear out the variable tables.
if MemorySentinel.value ~= MEMORYSENTINEL then
    -- Clears everything
    awtx.variables.clearTable()
    MemorySentinel.value = MEMORYSENTINEL
end

system = awtx.hardware.getSystem(1) -- Used to identify current hardware type.
config = awtx.weight.getConfig(1)   -- Used to get current system configuration information.
wt = awtx.weight.getCurrent(1)      -- Used to hold current scale snapshot information.
Scale1 = 0
Scale2 = 0
TotalWt = 0

printTokens = {}

-- Initialize print tokens to access various require file variables
for index = 1, 100 do
  printTokens[index] = {}
  printTokens[index].varName  = ""                  -- Holds a string of the variable name of the indexed token.
  printTokens[index].varLabel = "Invalid"           -- Long form name of the token variable.
  printTokens[index].varType  = awtx.fmtPrint.TYPE_UNDEFINED  -- Identifies type of variable for formatting during print operations.
  printTokens[index].varValue = tmp                 -- Holds the current value of the variable.
  printTokens[index].varFunct = ""                  -- Pointer to function used to set the current variable value.

  awtx.fmtPrint.varSet(index, 0, "Invalid", awtx.fmtPrint.TYPE_INTEGER)
end

require("awtxReqAppMenu")         
require("awtxReqScaleKeys")       
require("awtxReqScaleKeysEvents")
require("awtxReqRpnEntry")
require("ReqSetpoint")
require("ReqPresetTare")

-- AppName is displayed when escaping from password entry and entering a password of '0'
AppName = "GENERAL"
  
--------------------------------------------------------------------------------------------------
  --  Super Menu
  --------------------------------------------------------------------------------------------------
  -- Top level Menu
  TopMenu1 = {text = "Super", key = 1, action = "MENU", variable = "SuperMenu"}
  TopMenu2 = {text = "EXIT",  key = 2, action = "FUNC", callThis = supervisor.SupervisorMenuExit} 
  TopMenu = {TopMenu1, TopMenu2}

  -- These lines are needed to construct the top layer of the Supervisor Menu
  -- As more menus are added through require files, this layer will grow to include them.
  --SuperMenu  = { }

  SuperMenu1 = {text = " TARE  ",  key = 1, action = "MENU", variable = "TareSetupMenu", show = (system.modelStr == "ZM405")}
  SuperMenu2 = {text = " BACK  ",  key = 2, action = "MENU", variable = "TopMenu", subMenu = 1} 
  SuperMenu  = {SuperMenu1, SuperMenu2}

  TareSetupMenu1 = {text = " Edit  ",  key = 1, action = "FUNC", callThis = findTare}
  TareSetupMenu2 = {text = " Print ",  key = 2, action = "FUNC", callThis = printTareList, show = true}
  TareSetupMenu3 = {text = " Reset ",  key = 3, action = "FUNC", callThis = tareReset, show = true}
  TareSetupMenu4 = {text = " BACK  ",  key = 4, action = "MENU", variable = "SuperMenu", subMenu = 1} 
  TareSetupMenu  = {TareSetupMenu1, TareSetupMenu2, TareSetupMenu3, TareSetupMenu4}

  -- Need this to turn the table string names into the table addresses 
  generalMenu = {
    TopMenu = TopMenu,
      SuperMenu = SuperMenu,
        TareSetupMenu = TareSetupMenu
  }

--[[
Description:
  Function override from ReqAppMenu.lua
  This function is called when the Supervisor menu is entered.
Parameters:
  None
  
Returns:
  None
]]--
function appEnterSuperMenu()
  setpoint.disableOutputSetpoints()  -- Disable setpoints before entering supervisor menu.
  supervisor.menuLevel    = TopMenu         -- Set current menu level
  supervisor.menuCircular = generalMenu     -- Set menu address table
end

function ScaleTimer(TimerID)
  
  MotionLabel = " "
  wt1 = awtx.weight.getCurrent(1) 
  Scale1 = wt1.gross
  Motion1 = wt1.motion
  wt2 = awtx.weight.getCurrent(2) 
  Scale2 = wt2.gross
  Motion2 = wt2.motion
  TotalWt = Scale1 + Scale2
  
  awtx.fmtPrint.varSet(21, TotalWt, "Total Scale Wt", awtx.fmtPrint.TYPE_FLOAT)
  
  if Motion1 or Motion2 then
    
    MotionLabel = "M"
    
  end
  
  awtx.fmtPrint.varSet(22, MotionLabel, "Scale Motion", awtx.fmtPrint.TYPE_STRING)
  
end

--[[
Description:
  Function override from ReqAppMenu.lua
  This function is called when the Supervisor menu is exited.
Parameters:
  None
  
Returns:
  None
]]--
function appExitSuperMenu()
    -- This function retrieves the updated values from the setpoint configuration table
    --  and updates the current setpoints with the latest information.
  setpoint.enableOutputSetpoints()
end

function OnStart()
    
    -- Start .2 second timer function
timerID1 = awtx.os.createTimer(ScaleTimer, 200)

end


OnStart()
