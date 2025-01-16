--[[
*******************************************************************************

Filename:     awtxReqVariables.lua
Version:      1.0.0.1
Date:         2015-09-01
Customer:     Avery Weigh-Tronix
Description:
This file provides a way to store and recall variables in non-volatile memory.

-- Due to how the ZM4XX file system works, creating nonvolatile data structures
--  greater than 100 elements are strongly discouraged.   
-- For data sets this large, utilize the SQL database system for these products.


********************************************************************************
DO NOT MAKE CHANGES TO THIS FILE UNLESS YOU FULLY UNDERSTAND WHAT YOU ARE DOING!
********************************************************************************
]]


awtxReq.variables = {}
local mt = {}

function class(base, init)
   local c = {}    -- a new class instance
   if not init and type(base) == 'function' then
      init = base
      base = nil
   elseif type(base) == 'table' then
    -- our new class is a shallow copy of the base class!
      for i,v in pairs(base) do
        c[i] = v
      end
      c._base = base
   end
   -- the class will be the metatable for all its objects,
   -- and they will look up their methods in it.
   c.__index = c

   -- expose a constructor which can be called by <classname>(<args>)
   local mt = {}
   mt.__call = function(class_tbl, ...)
   local obj = {}
   setmetatable(obj,c)
   if init then
      init(obj,...)
   else 
      -- make sure that any stuff from the base class is initialized!
      if base and base.init then
        base.init(obj, ...)
      end
   end
   return obj
   end
   c.init = init
   c.is_a = function(self, klass)
      local m = getmetatable(self)
      while m do 
        if m == klass then 
          return true 
        end
        m = m._base
      end
      return false
   end
   setmetatable(c, mt)
   return c
end

mt.__index = function(self, key)
  local value = rawget(self, '_props')[key]
  if (value == nil) then
    value = rawget(self, '_methods')[key]
  end
  if (value == nil) then
    value = rawget(self, '_props').value[key]
  end
  return value
end


mt.__newindex = function(self, key, value)
  local _props = rawget(self, '_props')
  if (_props[key] ~= nil) then
    _props[key] = value
    if (key == 'value') then
      _props.dirty = true
    end
  else
    _props.value[key] = value
    _props.dirty = true
  end
  if (_props.autoSave and _props.dirty) then
    rawget(self, '_methods')['store'](self)
  end
end


local methods = {}

function methods:store()
  if (self.dirty) then
    self.dirty = false
    local tblName = self._props.tblName
    if (tblName == nil) then
      awtx.variables.store(self.varName, self.value)
    else
      awtx.variables.store(tblName, self.varName, self.value)
    end
  end
end


function methods:recall()
  local value = nil
  local tblName = self._props.tblName
  if (tblName == nil) then
    value = awtx.variables.recall(self.varName)
  else
    value = awtx.variables.recall(tblName, self.varName)
  end
  if (value == nil) then
    if (tblName == nil) then
      awtx.variables.store(self.varName, self.value)
    else
      awtx.variables.store(tblName, self.varName, self.value)
    end
  else
    self.value = value
  end
  self.dirty = false
end


awtxReq.variables.SavedVariable = class(function(var, varName, value, autoSave, tblName)
    assert(varName)
    if (value == nil) then 
      assert(value) 
    end
    autoSave = autoSave or false
    var._props = {}
    var._methods = methods
    var._props.varName = varName
    var._props.value = value
    var._props.dirty = false
    var._props.autoSave = false
    var._props.tblName = tblName
    setmetatable(var, mt)
    var:recall()
    var._props.autoSave = autoSave
    return var
end)
