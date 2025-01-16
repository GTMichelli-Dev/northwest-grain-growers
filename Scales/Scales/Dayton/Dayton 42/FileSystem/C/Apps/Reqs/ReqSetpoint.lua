--[[
*******************************************************************************

Filename:      ReqSetpoint.lua
Version:       1.0.0.2
Date:          2015-09-01
Customer:      Avery Weigh-Tronix
Description:
This is the Setpoint Database library.

*******************************************************************************

*******************************************************************************
]]

-- create the setpoint Namespace
setpoint = {}

require("awtxReqConstants")
require("awtxReqDisplayMessages")   -- Provides display message support
require("awtxReqVariables")


--- Define variables
Setpoint1Value = 0.0  -- Initialize setpoint variables to be accessed by the firmware.
Setpoint2Value = 0.0
Setpoint3Value = 0.0

--[[
Description:
  This Function is called to initialize things for this Require file 
Parameters:
  None
  
Returns:
  None
]]--
local function create()
  -- Copy stored setpoint values from database and store to lua variables.
  Setpoint1Variable = awtxReq.variables.SavedVariable("Setpoint1Variable", 0.0, true)
  Setpoint2Variable = awtxReq.variables.SavedVariable("Setpoint2Variable", 0.0, true)
  Setpoint3Variable = awtxReq.variables.SavedVariable("Setpoint3Variable", 0.0, true)
  
  -- Copy current stored values into setpoint config-mapped parameters.
  Setpoint1Value = Setpoint1Variable.value
  Setpoint2Value = Setpoint2Variable.value
  Setpoint3Value = Setpoint3Variable.value
  
  -- Register input event handlers.  Config is set as input setpoints on SP11, SP12, and SP13.
  awtx.setpoint.registerInputEvent(11, setpoint.onInput1)
  awtx.setpoint.registerInputEvent(12, setpoint.onInput2)
  awtx.setpoint.registerInputEvent(13, setpoint.onInput3)
end


-- Default Input 1 functionality.
-- Can be overridden by redefining setpoint.onInput1() function call after ReqSetpoint.lua is included in main application.
function setpoint.onInput1(spNum, state)
  if state == true then
    awtx.keypad.KEY_ZERO_DOWN()
  end
end


-- Default Input 2 functionality.
-- Can be overridden by redefining setpoint.onInput2() function call after ReqSetpoint.lua is included in main application.
function setpoint.onInput2(spNum, state)
  if state == true then
    awtx.weight.requestTare()    
  end
end


-- Default Input 3 functionality.
-- Can be overridden by redefining setpoint.onInput3() function call after ReqSetpoint.lua is included in main application.
function setpoint.onInput3(spNum, state)
  if state == true then
    awtx.keypad.KEY_PRINT_DOWN()
  end
end


-- Global function to enable outputs.
function setpoint.enableOutputSetpoints()
  awtx.setpoint.unlock(1)
  awtx.setpoint.unlock(2)
  awtx.setpoint.unlock(3)
end


-- Global function to disable outputs.
function setpoint.disableOutputSetpoints()
  awtx.setpoint.lockOff(1)
  awtx.setpoint.lockOff(2)
  awtx.setpoint.lockOff(3)
end


-- This function cycles through three prompts for the user to enter the three output setpoint values.
function setpoint.configureSetpoints()
  local newVal1 = 0
  local newVal2 = 0
  local newVal3 = 0
  local isEnterKey1 = 0
  local isEnterKey2 = 0
  local isEnterKey3 = 0
  local spMinVal = -999999
  local spMaxVal = 9999999
  local entertime = 10000
  local curMode = awtx.display.setMode(awtx.display.MODE_USER)
  
  setpoint.disableOutputSetpoints()
  
  awtxReq.display.displayWord('Out1', 1000)
  awtx.display.writeLine("")  -- clear the display 
  newVal1, isEnterKey1 = awtx.keypad.enterFloat(Setpoint1Variable.value, spMinVal, spMaxVal, config.displaySeparator, entertime, 'Enter', 'Out1')
  
  if isEnterKey1 then
    awtx.display.writeLine('Out2', 1000)
    awtx.display.writeLine("")  -- clear the display 
    newVal2, isEnterKey2 = awtx.keypad.enterFloat(Setpoint2Variable.value, spMinVal, spMaxVal, config.displaySeparator, entertime, 'Enter', 'Out2')
    
    if isEnterKey2 then
      awtx.display.writeLine('Out3', 1000)
      awtx.display.writeLine("")  -- clear the display 
      newVal3, isEnterKey3 = awtx.keypad.enterFloat(Setpoint3Variable.value, spMinVal, spMaxVal, config.displaySeparator, entertime, 'Enter', 'Out3')
    end
  end
  awtx.display.setMode(curMode)

  if isEnterKey1 then
    Setpoint1Variable.value = newVal1         -- Assign to table variable for storage
    Setpoint1Value = Setpoint1Variable.value  -- Assign to current setpoint config-mapped parameter
  end

  if isEnterKey2 then
    Setpoint2Variable.value = newVal2         -- Assign to table variable for storage
    Setpoint2Value = Setpoint2Variable.value  -- Assign to current setpoint config-mapped parameter
  end

  if isEnterKey3 then
    Setpoint3Variable.value = newVal3         -- Assign to table variable for storage
    Setpoint3Value = Setpoint3Variable.value  -- Assign to current setpoint config-mapped parameter
  end
  
  setpoint.enableOutputSetpoints()
end

-- Overide the default functionality of the Select Hold Function
function awtxReq.keypad.onSelectKeyHold()
  -- When the Select Key is held we want to configure the Setpoint Values.
  setpoint.configureSetpoints()
end

create()
