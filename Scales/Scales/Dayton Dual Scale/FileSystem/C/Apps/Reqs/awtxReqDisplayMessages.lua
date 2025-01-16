--[[
*******************************************************************************

Filename:     awtxReqDisplayMessages.lua
Version:      1.0.0.0
Date:         2015-09-01
Customer:     Avery Weigh-Tronix
Description:
This file provides some basic message display functions used in various
  applications and require files.

*******************************************************************************


*******************************************************************************
]]

awtxReq.display = {}

require("awtxReqConstants")

local CANT_TIME = 500   -- Time to show 'Cant'

function awtxReq.display.displayWord(word, msg_time)
  msg_time = msg_time or CANT_TIME  -- defaults msg_time to CANT_TIME if it wasn't included.
  
  local curMode = awtx.display.setMode(awtx.display.MODE_USER)
  awtx.display.writeLine(word, msg_time)
  awtx.display.setMode(curMode)
end


function awtxReq.display.displayCant()
  awtxReq.display.displayWord("Cant", CANT_TIME)
end


function awtxReq.display.displayDone()
  awtxReq.display.displayWord("Done", CANT_TIME)
end


function awtxReq.display.displayAbort()
  awtxReq.display.displayWord("Abort", CANT_TIME)
end


function awtxReq.display.displayError()
  awtxReq.display.displayWord("Error", CANT_TIME)
end


function awtxReq.display.displayPrnTot()
  awtxReq.display.displayWord("Prn Tot", CANT_TIME)
end


function awtxReq.display.displayCleared()
  awtxReq.display.displayWord("Cleared", CANT_TIME)
end