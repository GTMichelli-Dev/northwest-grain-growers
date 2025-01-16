--[[
*******************************************************************************

Filename:     awtxReqScaleKeys.lua
Version:      1.0.1.0
Date:         2015-09-01
Customer:     Avery Weigh-Tronix
Description:
This file provides basic general key functionality for the ZM405 indicator.
The default handled event keys may or may not exist on other hardware.

*******************************************************************************

Functions:
-- These create the default functionality for the keys.
-- Redefine the function to change the default functionality.
  awtxReq.keypad.onXXXKeyDown()
  awtxReq.keypad.onXXXKeyHold()
  awtxReq.keypad.onXXXKeyUp()
  
Event Handlers:
-- Leave these as-is as they create your hold events.
-- Redefine them if you want to change or use the REPEAT event for each key
  awtx.keypad.KEY_XXX_DOWN()
  awtx.keypad.KEY_XXX_REPEAT()
  awtx.keypad.KEY_XXX_UP()


*******************************************************************************
Change History:
1.0.0.0 - Initial Release
1.0.1.0 - Added the zeroHoldFlag declaration that was missed.
*******************************************************************************
]]

-- create the keypad namespace
awtxReq.keypad = {}

require("awtxReqDisplayMessages")   -- Provides display message support

-- define how many repeat events create a HOLD event for the keys
local HowManyRepeatsMakeAHold = 3

-- Flags for counting the number of hold events for each key.
local tareHoldFlag = 0
local selectHoldFlag = 0
local printHoldFlag = 0
local unitsHoldFlag = 0
local sampleHoldFlag = 0
local startHoldFlag = 0
local stopHoldFlag = 0
local f1HoldFlag = 0
local scaleSelectHoldFlag = 0
local setupHoldFlag = 0
local targetHoldFlag = 0
local numericHoldFlag = 0
local clearHoldFlag = 0
local decimalHoldFlag = 0
local zeroHoldFlag = 0


---------------------------------- Tare Key Default Functions ----------------------------------

function awtxReq.keypad.onTareKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
  if config.pbTareFlag then
    awtx.weight.requestTare()
  else
    awtxReq.display.displayCant()
  end
end

function awtxReq.keypad.onTareKeyHold()
  -- Default Tare Key HOLD functionality.
  -- Redefine function to change functionality
    awtx.weight.requestTareClear()
    awtxReq.display.displayCleared()
end

function awtxReq.keypad.onTareKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
end


--------------------------------- Select Key Default Functions ---------------------------------
function awtxReq.keypad.onSelectKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onSelectKeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onSelectKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
  awtx.weight.cycleActiveValue()
end


--------------------------------- Print Key Default Functions ----------------------------------
function awtxReq.keypad.onPrintKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
  awtx.weight.requestPrint()
end

function awtxReq.keypad.onPrintKeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onPrintKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
end


--------------------------------- Units Key Default Function ----------------------------------
function awtxReq.keypad.onUnitsKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onUnitsKeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onUnitsKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
  awtx.weight.cycleUnits()
end


---------------------------------- Zero Key Default Function ----------------------------------
function awtxReq.keypad.onZeroKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
  awtx.weight.requestZero()  -- Tell the Indicator to requestZero on the active scale.
end

function awtxReq.keypad.onZeroKeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onZeroKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
end


---------------------------------- SAMPLE Key Default Function ---------------------------------
function awtxReq.keypad.onSampleKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onSampleKeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onSampleKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
end

---------------------------------- START Key Default Function ---------------------------------
function awtxReq.keypad.onStartKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onStartKeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onStartKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
end

---------------------------------- STOP Key Default Function ---------------------------------
function awtxReq.keypad.onStopKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onStopKeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onStopKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
end

---------------------------------- F1/ID Key Default Function ---------------------------------
function awtxReq.keypad.onF1KeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onF1KeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end


function awtxReq.keypad.onF1KeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
end

------------------------------ Scale Select Key Default Function ------------------------------
function  awtxReq.keypad.onScaleSelectKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
  -- Send request to firmware to cycle to the next available scale.
  awtx.weight.cycleActiveScale()
end

function awtxReq.keypad.onScaleSelectKeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onScaleSelectKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
end

---------------------------------- SETUP Key Default Function ---------------------------------
function awtxReq.keypad.onSetupKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onSetupKeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onSetupKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
end

---------------------------------- TARGET Key Default Function ---------------------------------
function awtxReq.keypad.onTargetKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onTargetKeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onTargetKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
end

---------------------------------- NUMERIC Key Default Function ---------------------------------
function awtxReq.keypad.onNumericKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onNumericKeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onNumericKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
end

---------------------------------- CLEAR Key Default Function ---------------------------------
function awtxReq.keypad.onClearKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onClearKeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onClearKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
end


---------------------------------- DECIMAL Key Default Function ---------------------------------
function awtxReq.keypad.onDecimalKeyDown()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onDecimalKeyHold()
  -- Default functionality.
  -- Redefine function to change functionality
end

function awtxReq.keypad.onDecimalKeyUp()
  -- Default functionality.
  -- Redefine function to change functionality
end


-----------------------------------------------------------------------------------------------
-- Tare Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_TARE_DOWN()
  tareHoldFlag = 0
  awtxReq.keypad.onTareKeyDown()
end

function awtx.keypad.KEY_TARE_REPEAT()
  tareHoldFlag = tareHoldFlag + 1
  if tareHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onTareKeyHold()
  end
end

function awtx.keypad.KEY_TARE_UP()
  if tareHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onTareKeyUp()
  end
  tareHoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- Select Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_SELECT_DOWN()
  selectHoldFlag = 0
  awtxReq.keypad.onSelectKeyDown()
 end

function awtx.keypad.KEY_SELECT_REPEAT()
  selectHoldFlag = selectHoldFlag + 1
  if selectHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onSelectKeyHold()
  end
end

function awtx.keypad.KEY_SELECT_UP()
  if selectHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onSelectKeyUp()
  end
  selectHoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- Print Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_PRINT_DOWN()
  printHoldFlag = 0
  awtxReq.keypad.onPrintKeyDown()
end

function awtx.keypad.KEY_PRINT_REPEAT()
  printHoldFlag = printHoldFlag + 1
  if printHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onPrintKeyHold()
  end
end

function awtx.keypad.KEY_PRINT_UP()
  if printHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onPrintKeyUp()
  end
  printHoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- Units Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_UNITS_DOWN()
  unitsHoldFlag = 0
  awtxReq.keypad.onUnitsKeyDown()
end

function awtx.keypad.KEY_UNITS_REPEAT()
  unitsHoldFlag = unitsHoldFlag + 1
  if unitsHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onUnitsKeyHold()
  end
end

function awtx.keypad.KEY_UNITS_UP()
  if unitsHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onUnitsKeyUp()
  end
  unitsHoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- Zero Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_ZERO_DOWN()
  zeroHoldFlag = 0
  awtxReq.keypad.onZeroKeyDown()
end

function awtx.keypad.KEY_ZERO_REPEAT()
  zeroHoldFlag = zeroHoldFlag + 1
  if zeroHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onZeroKeyHold()
  end
end

function awtx.keypad.KEY_ZERO_UP()
  if zeroHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onZeroKeyUp()
  end
  zeroHoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- Sample Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_SAMPLE_DOWN()
  sampleHoldFlag = 0
  awtxReq.keypad.onSampleKeyDown()
end

function awtx.keypad.KEY_SAMPLE_REPEAT()
  sampleHoldFlag = sampleHoldFlag + 1
  if sampleHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onSampleKeyHold()
  end
end

function awtx.keypad.KEY_SAMPLE_UP()
  if sampleHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onSampleKeyUp()
  end
  sampleHoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- Start Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_START_DOWN()
  startHoldFlag = 0
  awtxReq.keypad.onStartKeyDown()
end

function awtx.keypad.KEY_START_REPEAT()
  startHoldFlag = startHoldFlag + 1
  if startHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onStartKeyHold()
  end
end

function awtx.keypad.KEY_START_UP()
  if startHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onStartKeyUp()
  end
  startHoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- Stop Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_STOP_DOWN()
  stopHoldFlag = 0
  awtxReq.keypad.onStopKeyDown()
end

function awtx.keypad.KEY_STOP_REPEAT()
  stopHoldFlag = stopHoldFlag + 1
  if stopHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onStopKeyHold()
  end
end

function awtx.keypad.KEY_STOP_UP()
  if stopHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onStopKeyUp()
  end
  stopHoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- F1 Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_F1_DOWN()
  f1HoldFlag = 0
  awtxReq.keypad.onF1KeyDown()
end

function awtx.keypad.KEY_F1_REPEAT()
  f1HoldFlag = f1HoldFlag + 1
  if f1HoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onF1KeyHold()
  end
end

function awtx.keypad.KEY_F1_UP()
  if f1HoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onF1KeyUp()
  end
  f1HoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- Scale Select Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_SCALE_SELECT_DOWN()
  scaleSelectHoldFlag = 0
   awtxReq.keypad.onScaleSelectKeyDown()
end

function awtx.keypad.KEY_SCALE_SELECT_REPEAT()
  scaleSelectHoldFlag = scaleSelectHoldFlag + 1
  if scaleSelectHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onScaleSelectKeyHold()
  end
end

function awtx.keypad.KEY_SCALE_SELECT_UP()
  if scaleSelectHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onScaleSelectKeyUp()
  end
  scaleSelectHoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- Setup Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_SETUP_DOWN()
  setupHoldFlag = 0
  awtxReq.keypad.onSetupKeyDown()
end

function awtx.keypad.KEY_SETUP_REPEAT()
  setupHoldFlag = setupHoldFlag + 1
  if setupHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onSetupKeyHold()
  end
end

function awtx.keypad.KEY_SETUP_UP()
  if setupHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onSetupKeyUp()
  end
  setupHoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- Target Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_TARGET_DOWN()
  targetHoldFlag = 0
  awtxReq.keypad.onTargetKeyDown()
end

function awtx.keypad.KEY_TARGET_REPEAT()
  targetHoldFlag = targetHoldFlag + 1
  if targetHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onTargetKeyHold()
  end
end

function awtx.keypad.KEY_TARGET_UP()
  if targetHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onTargetKeyUp()
  end
  targetHoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- Numeric Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_NUMERIC_DOWN(numChar)
  numericHoldFlag = 0
  awtxReq.keypad.onNumericKeyDown(numChar)
end

function awtx.keypad.KEY_NUMERIC_REPEAT(numChar)
  numericHoldFlag = numericHoldFlag + 1
  if numericHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onNumericKeyHold(numChar)
  end
end

function awtx.keypad.KEY_NUMERIC_UP(numChar)
  if numericHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onNumericKeyUp(numChar)
  end
  numericHoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- Clear Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_CLEAR_DOWN()
  clearHoldFlag = 0
  awtxReq.keypad.onClearKeyDown()
end

function awtx.keypad.KEY_CLEAR_REPEAT()
  clearHoldFlag = clearHoldFlag + 1
  if clearHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onClearKeyHold()
  end
end

function awtx.keypad.KEY_CLEAR_UP()
  if clearHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onClearKeyUp()
  end
  clearHoldFlag = 0
end


-----------------------------------------------------------------------------------------------
-- Decimal Key Event Handlers
-----------------------------------------------------------------------------------------------
function awtx.keypad.KEY_DECIMAL_DOWN()
  decimalHoldFlag = 0
  awtxReq.keypad.onDecimalKeyDown()
end

function awtx.keypad.KEY_DECIMAL_REPEAT()
  decimalHoldFlag = decimalHoldFlag + 1
  if decimalHoldFlag == HowManyRepeatsMakeAHold then
    awtxReq.keypad.onDecimalKeyHold()
  end
end

function awtx.keypad.KEY_DECIMAL_UP()
  if decimalHoldFlag < HowManyRepeatsMakeAHold then
    awtxReq.keypad.onDecimalKeyUp()
  end
  decimalHoldFlag = 0
end