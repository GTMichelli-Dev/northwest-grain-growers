--[[
*******************************************************************************

Filename:      awtxReqAppMenu.lua
Version:       1.0.0.0
Date:          2015-09-01
Customer:      Avery Weigh-Tronix
Description:
This file provides menu functionality for the ZM405 indicator.  With this file
included, other require files may add additional sections to the super menu or
create their own.

*******************************************************************************


*******************************************************************************

Global Data:
  supervisor

*******************************************************************************
]]

require("awtxReqConstants")


supervisor = {} --Global Table
supervisor.AvailableMenus = {}  -- This table is filled with available menu passwords
                                --   and entry functions for each specific menu.


supervisor.menuing      = false
supervisor.menuCurrent  = 1
supervisor.menuLevel    = topMenuDefault
supervisor.menuCircular = resolveCircular1Default


  --[[
Description:
  This Function is called to initialize things for this Require file 
Parameters:
  None
  
Returns:
  None
]]--
local function create()
  awtx.os.registerPasswordEnteredEvent(supervisor.onPasswordEntered)
end


-- Function stub to be overridden by current application if any actions need to be
--  performed before entering the menus.
function appEnterSuperMenu()
end


-- Function stub to be overridden by current application if any actions need to be
--  performed after exiting the menus.
function appExitSuperMenu()
end


-- decide whether menu level should be shown or not
function supervisor.menuVisible(menuItem)
  local retVal = false
  local found = false
  local variable, Compare, val
  if menuItem.show == nil then
    retVal = true
  elseif type(menuItem.show) == "boolean" and menuItem.show == true then
    retVal = true
  elseif type(menuItem.show) == "string" then
    retVal = true
  elseif type(menuItem.show) == "table" then
    Compare = menuItem.show

    if Compare.callThis == nil or Compare.val1 == nil then
      retVal = true
    elseif type(Compare.callThis) == "function" then
      val = tonumber(Compare.callThis())
      if val == Compare.val1 then
        retVal = true
      else
        retVal = false
      end
    end
  else
    retVal = false
  end
  return retVal
end


-- handle the menu when its action type is function
function supervisor.handleMenuActionFunction(menuItem)
  if type(menuItem.callThis) == "function" then
    if menuItem.text == nil then
      menuItem.callThis()
    else
      menuItem.callThis(menuItem.text)
    end
  end
end


-- show available menus on the display
function supervisor.showMenus()
  local menuStr = ""
  local menuSelection
  local menuActive = 1 -- this is the current menu(visible) the list of menus is longer
  local menuKey={}
  local a, b, i
  local numItems = 0
  -- format a string representing this menu struct
  for i = 1, #supervisor.menuLevel do
    if supervisor.menuVisible(supervisor.menuLevel[i]) then
      menuStr = menuStr .. string.format("%s}", supervisor.menuLevel[i].text)
      table.insert(menuKey, supervisor.menuLevel[i].key)
      if supervisor.menuLevel[i].key == supervisor.menuCurrent then
        menuActive = #menuKey
      end
      numItems = numItems + 1
    end
  end
  --display via "C" driver
  if numItems == 1 then   -- the one is the back menu!!
    -- no items to display so empty menu
    a = menuKey [1]
    b = nil
  else
    menuSelection = awtx.setupMenu.selectMenu(menuStr, menuActive)
    a = menuKey [tonumber(menuSelection)]
    b = nil
  end
  collectgarbage()
  return a, b
end


function doMenuLoop()
  local aKey
  local bKey
  local j
  local menuIndex
  local myTable
  
  awtx.keypad.disableLuaKeyboardEvents()

  while supervisor.menuing == true do
    -- Show menus
    aKey, bKey = supervisor.showMenus()
    if bKey == nil then
      -- Find the menu the user selected
      menuIndex = -1
      for j=1, #supervisor.menuLevel do
        if supervisor.menuLevel[j].key == aKey then
          menuIndex = j
          break
        end
      end
    else
      menuIndex = -1
    end
    if(menuIndex == -1) then
    else
      supervisor.menuCurrent = supervisor.menuLevel[menuIndex].key
      if supervisor.menuLevel[menuIndex].action == nil then
        -- Nothing to do
      elseif supervisor.menuLevel[menuIndex].action == "MENU" then
        -- Ok the action is menu so select menu and if defined submenu
        if type(supervisor.menuLevel[menuIndex].variable) == "string" then
          myTable = supervisor.menuCircular[supervisor.menuLevel[menuIndex].variable]
          if supervisor.menuLevel[menuIndex].subMenu ~= nil then
            supervisor.menuCurrent = supervisor.menuLevel[menuIndex].subMenu
          else
            supervisor.menuCurrent = 1
          end
          supervisor.menuLevel = myTable
        end
      elseif supervisor.menuLevel[menuIndex].action == "FUNC" then
        supervisor.handleMenuActionFunction(supervisor.menuLevel[menuIndex])
      else
      end
    end
  end
  -- avoid double tap of tare key
  awtx.os.sleep (500)
  awtx.keypad.clearKeyBuffer()
  awtx.keypad.enableLuaKeyboardEvents()
end


-- Event handler to occur after the firmware has tested the entered password against
--  all internal menu passwords
function supervisor.onPasswordEntered(passwordString)
  local validPass = false
  for entry in pairs (supervisor.AvailableMenus) do
    if passwordString == supervisor.AvailableMenus[entry].entryPassword then
      validPass = true
      supervisor.AvailableMenus[entry].entryFunction()
    end
  end

  if validPass == false then
    -- Invalid Password
    usermode = awtx.display.setMode(awtx.display.MODE_MENU)
    awtx.display.writeLine("Invalid")
    awtx.os.sleep(1000)
    usermode = awtx.display.setMode(awtx.display.MODE_SCALE)
  end

  awtx.keypad.clearKeyBuffer()
end


-- exit from menu system
function supervisor.SupervisorMenuExit(param)
  -- kill the menu system
  supervisor.menuing = false
  -- run this when exiting the Menu
  appExitSuperMenu()
end


-----------------------------------------------------------------------------------
--  Default Menus
-----------------------------------------------------------------------------------

-- Default Supervisor Menu if specific app doesn't declare it's own.
-- Top level Menu
T1Default = {text = "Super",   key = 1, action = "FUNC", callThis = nil}
T2Default = {text = "EXIT",    key = 2, action = "FUNC", callThis = supervisor.SupervisorMenuExit}
topMenuDefault = {T1Default, T2Default}

resolveCircular1Default =
{
  topMenuDefault = topMenuDefault
}


local PASSWORD_SUPER = "1793"
function supervisor.SupervisorMenuEntry()
  local usermode
  -- do this before entering Menu
  appEnterSuperMenu()
  usermode = awtx.display.setMode(awtx.display.MODE_MENU)
  supervisor.menuing = true
  supervisor.menuCurrent = 1
  doMenuLoop()
  awtx.display.setMode(usermode)
end
supervisor.AvailableMenus["Super"] = {entryPassword = PASSWORD_SUPER,
                                      entryFunction = supervisor.SupervisorMenuEntry}


local PASSWORD_APPVIEW = "0"
function supervisor.AppViewMenuEntry()
  -- do this before entering Menu
  if AppName ~= nil then
    awtx.display.setMode(awtx.display.MODE_MENU)
    awtx.display.writeLine(AppName)
    awtx.os.sleep(1000)
    awtx.display.setMode(awtx.display.MODE_SCALE)
  end
end

supervisor.AvailableMenus["AppView"] = {entryPassword = PASSWORD_APPVIEW,
                                        entryFunction = supervisor.AppViewMenuEntry}


create()