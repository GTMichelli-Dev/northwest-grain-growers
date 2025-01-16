--[[
*******************************************************************************

Filename:      awtxReqScaleKeysEvents.lua
Version:       1.0.0.0
Date:          2015-09-01
Customer:      Avery Weigh-Tronix
Description:
This file provides event handler functions required to perform general
   application operations.

*******************************************************************************

*******************************************************************************
]]

require("awtxReqConstants")
require("awtxReqDisplayMessages")  -- Needed for display messages

  --[[
Description:
  This Function is called to initialize things for this Require file 
Parameters:
  None
  
Returns:
  None
]]--
local function create()
  awtx.weight.registerTareCompleteEvent(onTareCompleteEvent)
  awtx.weight.registerPrintCompleteEvent(onPrintCompleteEvent)
  awtx.weight.registerUnitsCompleteEvent(onUnitsCompleteEvent)
  awtx.weight.registerZeroCompleteEvent(onZeroCompleteEvent)

  awtx.printer.registerPrinterStatusEvent(onPrinterStatusEvent)
  awtx.printer.registerPrinterJobStatusEvent(onPrinterJobStatusEvent)
end


---------------------------------------------------------------------------------------------------
-- Tare Request Callback Function
---------------------------------------------------------------------------------------------------
function onTareCompleteEvent(eventResult, eventResultString)
  if eventResult ~= 0 then
    awtxReq.display.displayCant()
  end
end


---------------------------------------------------------------------------------------------------
-- Print Request Callback Function
---------------------------------------------------------------------------------------------------
function onPrintCompleteEvent(eventResult, eventResultString)
  if eventResult ~= 0 then
    awtxReq.display.displayCant()
  else
    wt = awtx.weight.getLastPrint()
    awtx.printer.PrintFmt(0)
  end
end


function onPrinterStatusEvent(prnnum, param1, param2, param3, param4, param5)
  if prnnum >= 0 then
    if param1 ~= 0 then
      displayPrinterStatusMsg(param1)
      if param2 ~= 0 then
        displayPrinterStatusMsg(param2)
        if param3 ~= 0 then
          displayPrinterStatusMsg(param3)
          if param4 ~= 0 then
            displayPrinterStatusMsg(param4)
            if param5 ~= 0 then
              displayPrinterStatusMsg(param5)
            end
          end
        end
      end
    end
  end
end


function onPrinterJobStatusEvent(prnnum, param1, param2, param3, param4, param5)
  if prnnum >= 0 then
    if param1 ~= 0 then
      displayPrinterJobStatusMsg(param1)
      if param2 ~= 0 then
        displayPrinterJobStatusMsg(param2)
        if param3 ~= 0 then
          displayPrinterJobStatusMsg(param3)
          if param4 ~= 0 then
            displayPrinterJobStatusMsg(param4)
            if param5 ~= 0 then
              displayPrinterJobStatusMsg(param5)
            end
          end
        end
      end
    end
  end
end


function displayPrinterStatusMsg(msgid)
  local msg = ""
  local msg_time = 1000

  if msgid == awtx.printer.STATUS_OFFLINE then
    msg = "Offline"
  elseif msgid == awtx.printer.STATUS_ONLINE then
    msg = "Online "
  elseif msgid == awtx.printer.STATUS_PAPER_LOW then
    msg = "PaperLo"
    msg_time = 600
  elseif msgid == awtx.printer.STATUS_GENERAL_ERROR then
    msg = "Ptr Err"
    msg_time = 600
  end

  if msg ~= "" then
    awtxReq.display.displayWord(msg, msg_time)
  end
end


function displayPrinterJobStatusMsg(msgid)
  local msg = ""
  local msg_time = 1000

  if msgid == awtx.printer.JOB_STATUS_DONE then
    msg = "PrnDone"
  elseif msgid == awtx.printer.JOB_STATUS_ABORTED then
    msg = "PrnAbrt"
  elseif msgid == awtx.printer.JOB_STATUS_CREATE_FAILED then
    msg = "PrnFail"
  end

  if msg ~= "" then
    awtxReq.display.displayWord(msg, msg_time)
  end
end


---------------------------------------------------------------------------------------------------
-- Units Request Callback Function
---------------------------------------------------------------------------------------------------
function onUnitsCompleteEvent(eventResult, eventResultString)
  if eventResult ~= 0 then
    awtxReq.display.displayCant()
  else
    -- If the units changed successfully, we need to re-grab
    --   the width and precision for printing application variables.
    wt = awtx.weight.getCurrent(1)
  end
end

---------------------------------------------------------------------------------------------------
-- Zero Request Callback Function
---------------------------------------------------------------------------------------------------
function onZeroCompleteEvent(eventResult, eventResultString)
  if eventResult ~= 0 then
    awtxReq.display.displayCant()
  end
end

-- call the Create function to initialize the functionality for the Require File
create()