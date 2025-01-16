--[[
*******************************************************************************
Filename:     ReqPresetTare.lua
Version:      1.0.0.1
Date:         2015-09-01
Customer:     Avery Weigh-Tronix
Description:    
    This file creates a 50 channel Preset Tare table with an alphanumeric ID 
    that is associated with each Tare channel
*******************************************************************************

*******************************************************************************
]]--

require('awtxReqVariables')
require('awtxReqConstants')

-- Due to how the ZM4XX file system works, creating nonvolatile data structures
--  greater than 100 elements are strongly discouraged.   
-- For data sets this large, utilize the SQL database system for these products.
minTareNum = 1 -- Minumum number of Tare Channels that can be stored
maxTareNum = 50 -- Maximum number of Tare Channels that can be stored

blankRecord = { }
blankRecord.index = 0
blankRecord.tare = 0
blankRecord.tareNumber = "0"
  
 tareList = {}

  
  --[[
Description:
  This Function is called to initialize things for this Require file 
Parameters:
  None
  
Returns:
  None
]]--
local function create()
  for i = minTareNum, maxTareNum do
    tareList[i] = awtxReq.variables.SavedVariable('tareList_' .. i, blankRecord, true)
    tareList[i].index = i
  end

end

  --[[
Description:
  This function finds exsisting Tare channel IDs or looks for the next available place
  to store a new recipe.
  
Parameters:
  param[in]   tmpRecNum - This is the Tare channel ID that the function is trying to find
  
Returns:
  foundIndex - The index of the found Tare channel ID
  or 0 if Tare channel not found
]]--
function find(tmpTareNum)
  for foundIndex = minTareNum , maxTareNum do
    if(tareList[foundIndex].tareNumber == tmpTareNum) then
      return foundIndex
    end
  end
  return 0
end

--[[
Description:
  This function will ask the user for a Tare channel. If it exists it will ask them 
  if they want to edit the tare. If it doesn't exist it will ask them if they
  want to enter a new tare. If it doesn't exist and they all ready have 50 tares
  it will tell them the data base is full.
  
Parameters:
  None
  
Returns:
  None
]]--
function findTare()
  local curScaleConfig = awtx.weight.getCurrent()
  local choice
  local tmpTareNum
  local isEnterKey
  local curDispMode
  
  curDispMode = awtx.display.setMode(awtx.display.MODE_USER)
  tmpTareNum,isEnterKey = awtx.keypad.enterString("",4,-1,"Tare #","Enter")
 
  if(not isEnterKey) then
    --Esc key was pressed
    awtx.display.setMode(curDispMode)
    return
  end
  
  tmpIndex = find(tmpTareNum)
  
  if(tmpIndex > 0) then
    awtx.display.writeLine("Found",500)
    awtx.display.writeLine("")  -- clear the display 
    choice,isEnterKey = awtx.keypad.selectList("No,Yes",0,-1, "Edit", "Tare?")
    
    if(not isEnterKey) then
      -- Excape Key was pressed
      awtx.display.writeLine("Abort",500)
      awtx.display.setMode(curDispMode)
      return
    end
    
    if (choice == 1) then
      -- Yes edit the Tare
      tmpTare, isEnterKey = awtx.keypad.enterWeightWithUnits(tareList[tmpIndex].tare,0,curScaleConfig.curCapacity,curScaleConfig.unitsStr , config.displaySeparator,-1,1,"Enter","Tare")
      if (isEnterKey) then
        tareList[tmpIndex].tare = tmpTare         
      else
        awtx.display.writeLine("Abort",500)
        awtx.display.setMode(curDispMode)
        return
      end
    else
      -- No don't edit the Tare
      awtx.display.writeLine("Abort",500)
      awtx.display.setMode(curDispMode)
      return
    end      
    
  else
    tmpIndex = find("0")
    
    if (tmpIndex > 0) then
      awtx.display.setMode(awtx.display.MODE_USER)
      awtx.display.writeLine("Not",500)
      awtx.display.writeLine("Found",500)
      awtx.display.writeLine("")  -- clear the display 

      choice,isEnterKey = awtx.keypad.selectList("No,Yes",0,-1,"Enter", "New?")
      
      if(not isEnterKey) then
        -- Escape Key was pressed
        awtx.display.writeLine("Abort",500)
        awtx.display.setMode(curDispMode)
        return
      end
      if (choice == 1) then
        tareList[tmpIndex].tareNumber = tmpTareNum
        tmpTare, isEnterKey = awtx.keypad.enterWeightWithUnits(0,0,curScaleConfig.curCapacity,curScaleConfig.unitsStr , config.displaySeparator,-1,1,"Enter","Tare")
        if (isEnterKey) then
          tareList[tmpIndex].tare = tmpTare
        else
          awtx.display.writeLine("Abort",500)
          awtx.display.setMode(curDispMode)
          return
        end
      end
        
    else
      awtx.display.writeLine("Not",500)
      awtx.display.writeLine("Found",500)
      awtx.display.writeLine("Data",500)
      awtx.display.writeLine("Base",500)
      awtx.display.writeLine("Full",500)
      awtx.display.writeLine("")  -- clear the display 
    end
  end
  awtx.display.setMode(curDispMode)
end

--[[
Description:
  This function will ask the user for a Tare channel. If it exists it will 
  load the associated Tare value into the scale.
  
Parameters:
  None
  
Returns:
  None
]]--
function getTare()
  local tmpTareNum
  local isEnterKey
  
  tmpTareNum,isEnterKey = awtx.keypad.enterString("",4,-1,"Tare #","Enter")
  if (isEnterKey) then
    tmpIndex = find(tmpTareNum)

    if(tmpIndex > 0) then
      awtx.weight.requestPresetTare(1,tareList[tmpIndex].tare)
    else
      awtx.display.setMode(awtx.display.MODE_USER)
      awtx.display.writeLine("Cant",500)
      awtx.display.setMode(awtx.display.MODE_SCALE)
    end
  end
end

--[[
Description:
  This function will reset the Tare channels IDs and Tares back to 0.
  
Parameters:
  None
  
Returns:
  None
]]--
function tareReset()
  awtx.display.setDisplayBusy()
  for i = minTareNum,maxTareNum do
    tareList[i].tare = 0
    tareList[i].tareNumber = "0"
  end
  awtx.weight.requestTareClear(awtx.weight.getActiveScale())
  awtx.display.clrDisplayBusy()
end

--[[
Description:
  This function will Print all the Tare Channels' IDs and Values out Com1  
Parameters:
  None
  
Returns:
  None
]]--
function printTareList()
  awtx.display.setDisplayBusy()
  awtx.printer.PrintFmt(2)  -- Print the Header
  for i = minTareNum,maxTareNum do
    -- Load the App variables with the current Tare Index values
    awtx.fmtPrint.varSet(3,i)
    awtx.fmtPrint.varSet(1,tareList[i].tareNumber,"ID",awtx.fmtPrint.TYPE_STRING)
    awtx.fmtPrint.varSet(2,tareList[i].tare,"Tare Value",awtx.fmtPrint.TYPE_INTEGER)
    awtx.printer.PrintFmt(4)  -- Print the current Tare Values
    awtx.os.sleep(100)
  end
  awtx.printer.PrintFmt(3)  -- Print the Footer
  awtx.display.clrDisplayBusy()
end

-- if the model is a Zm405 allow the Preset Tares to function
if system.modelStr == "ZM405" then

  --[[
  Description:
    Overides the Tare Key Up Functionality to prompt for a Preset Tare.
    
  Parameters:
    None
    
  Returns:
    None
  ]]--
  function awtxReq.keypad.onTareKeyUp()
    if config.presetTareFlag then
      getTare()
    end
  end

  --[[
  Description:
    Overides the Tare Key Down Functionality to check if Preset Tare is enabled 
    before doing a Pushbutton Tare.
    
  Parameters:
    None
    
  Returns:
    None
  ]]--
  function awtxReq.keypad.onTareKeyDown()
    -- Default functionality.
    -- Redefine function to change functionality
    if not config.presetTareFlag then
      if config.pbTareFlag then
        awtx.weight.requestTare()
      else
        awtxReq.display.displayCant()
      end
    end
  end
end

create()
