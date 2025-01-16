--[[
*******************************************************************************

Filename:     awtxReqSQLiteMultiTable.lua
Version:      1.0.0.0
Date:         2016-02-01
Customer:     Avery Weigh-Tronix
Description:
This file provides a way to store and recall values in multiple Tables 
from a single SQLite Database in non-volatile memory.

********************************************************************************
DO NOT MAKE CHANGES TO THIS FILE UNLESS YOU FULLY UNDERSTAND WHAT YOU ARE DOING!
********************************************************************************
]]

----------------------------------------------------------------------------------------------------------
-- SQLITE error codes
------------------------------------------------------------------------------------
-- SQLITE_OK           0    Successful result
-- SQLITE_ERROR        1    SQL error or missing database
-- SQLITE_INTERNAL     2    Internal logic error in SQLite
-- SQLITE_PERM         3    Access permission denied
-- SQLITE_ABORT        4    Callback routine requested an abort
-- SQLITE_BUSY         5    The database file is locked
-- SQLITE_LOCKED       6    A table in the database is locked
-- SQLITE_NOMEM        7    A malloc() failed
-- SQLITE_READONLY     8    Attempt to write a readonly database
-- SQLITE_INTERRUPT    9    Operation terminated by sqlite3_interrupt()
-- SQLITE_IOERR       10    Some kind of disk I/O error occurred
-- SQLITE_CORRUPT     11    The database disk image is malformed
-- SQLITE_NOTFOUND    12    NOT USED. Table or record not found
-- SQLITE_FULL        13    Insertion failed because database is full
-- SQLITE_CANTOPEN    14    Unable to open the database file
-- SQLITE_PROTOCOL    15    NOT USED. Database lock protocol error
-- SQLITE_EMPTY       16    Database is empty
-- SQLITE_SCHEMA      17    The database schema changed
-- SQLITE_TOOBIG      18    String or BLOB exceeds size limit
-- SQLITE_CONSTRAINT  19    Abort due to constraint violation
-- SQLITE_MISMATCH    20    Data type mismatch
-- SQLITE_MISUSE      21    Library used incorrectly
-- SQLITE_NOLFS       22    Uses OS features not supported on host
-- SQLITE_AUTH        23    Authorization denied
-- SQLITE_FORMAT      24    Auxiliary database format error
-- SQLITE_RANGE       25    2nd parameter to sqlite3_bind out of range
-- SQLITE_NOTADB      26    File opened that is not a database file
-- SQLITE_ROW         100   sqlite3_step() has another row ready
-- SQLITE_DONE        101   sqlite3_step() has finished executing
--

awtxReq.database = {}

local databaseFileName = nil
local dbHandle = nil


--[[
Function: initializeDatabase
Description:  Creates and/or Opens a Database with the specified table             
Parameters: 
  filename- Name of the database file to use.
  newtableName- Name of the table in the database to create.
  fields- table containing the Columns in the table to create, needs to include the column names 
        and types in the form of {"Name1 Type1", "Name2 Type2", ... "NameN TypeN"}
Returns:
  an SQLite error code.  0 = successful
]]
function awtxReq.database.initializeDatabase(fileName, newTableName, fields)
  local errorCode = 0
  local sqlCommandString = ""
  
  -- Create and Store the full path filename for future reference
  awtx.os.makeDirectory("C:\\Apps\\Database")
  databaseFileName = fileName
  
  -- if the Database is not already open then Open it
  if dbHandle == nil then
    dbHandle, errorCode = sqlite3.open(databaseFileName)
  end
  
  -- Create the Command String from the Fields Parameter
  sqlCommandString = [[CREATE TABLE IF NOT EXISTS ]] .. newTableName .. [[ (]] .. table.concat(fields, ",") .. ")"
  
  --if the database was able to be opened create the table if it doesn't exist already
  if dbHandle ~= nil then
    local error1 = dbHandle:exec [[BEGIN TRANSACTION]]
    errorCode = dbHandle:exec(sqlCommandString)
    local error9 = dbHandle:exec [[COMMIT]]       
  end
  
  -- return the error code
  return errorCode
end

--[[
Function: recordCount
Description:  Returns the number of records in the database table            
Parameters: 
  tableName- Name of the table in the database to use.
Returns:
  the number of records in the database table
]]
function awtxReq.database.recordCount(tableName)
local numberOfRecords

  -- Get the number of records in the table.
  for x in dbHandle:urows("select count(*) from "  .. tableName) 
  do
    numberOfRecords = x
  end  

  return numberOfRecords
end


--[[
Function: addUpdateRecord
Description:  adds (or updates a record if already exists) to the table.  
    ### NOTE : This function doesn't check for a full table. ###
Parameters: 
  curRecord- a table of the record to be added or updated 
            in the form of {column1, column2, ... columnN}
            Values for columns must be listed in the same order as 
            they are defined in the dbObj.CreateDatabase function.
  tableName- Name of the table in the database to use.
Returns:
  An Error code which includes:
  the list of SQLite error codes along with the following
  0 = Successfull
  -1 = the database is not open
  -2 = the record to be added was blank
]]  
function awtxReq.database.addUpdateRecord(curRecord, tableName)
  
  local stmt = nil
  local result = -1  -- database was not open 
  local sqlCommandString = nil 
  
  -- make sure there is a record to add
  if (#curRecord == 0) then
    -- the record to be added was blank
    return -2
  end
  
  
  --Create the Command by figuring out how may fields are in the table
  sqlCommandString = [[ REPLACE INTO ]] .. tableName .. [[ VALUES (?]]
  for i = 2, #curRecord do
    sqlCommandString = sqlCommandString .. ", ?"
  end
  sqlCommandString = sqlCommandString .. ")"
  
  -- make sure the database is open
  if dbHandle:isopen() == true then
  
    stmt = dbHandle:prepare(sqlCommandString)
    
    --unpacking the table was how we got the strings with quotes to come over to SQLite
    result = stmt:bind_values(unpack(curRecord))

    stmt:step()
    stmt:reset()
  end
  return result
end

--[[
Function: deleteRecord
Description:  Deletes record(s) from the table.             
Parameters: 
  fieldName- field to match the records to
  fieldValue- records that are to be deleted.
  tableName- Name of the table in the database to use.
Returns:
  An Error code which includes:
  the list of SQLite error codes along with the following
  0 = Successfull
  -1 = the database is not open
]]
function awtxReq.database.deleteRecord(fieldName, fieldValue, tableName)
  local stmt = ""
  local result = -1
  
  -- make sure the database is open
  if dbHandle:isopen() == true then
    stmt = dbHandle:prepare([[DELETE FROM ]] .. tableName .. [[ WHERE ]] .. fieldName .. [[ = ? ]])
    result = stmt:bind_values(fieldValue) 
    stmt:step()
    stmt:reset()
    
  end
  return result
end
  
--[[
Function: deleteAllRecord
Description:  Deletes record(s) from the table.             
Parameters: 
  tableName- Name of the table in the database to use.
Returns:
  An Error code which includes:
  the list of SQLite error codes along with the following
  0 = Successfull
  -1 = the database is not open
]]
function awtxReq.database.deleteAllRecords(tableName)
  local stmt = ""
  local result = -1
  
  -- make sure the database is open
  if dbHandle:isopen() == true then
    local error1 = dbHandle:exec [[BEGIN TRANSACTION]]
    result = dbHandle:exec([[DELETE FROM ]] .. tableName)
    local error9 = dbHandle:exec [[COMMIT]]       
    
  end
  return result
end
  
--[[
Function: recallRecords
Description:  selects a record(s) from the table.             
Parameters: 
  fieldName- field to match the records to
  fieldValue- records that are to be recalled.
  tableName- Name of the table in the database to use.
Returns:
  table containing the matching records with field names.
]]
function awtxReq.database.recallRecord(fieldName, fieldValue, tableName)
  local stmt = nil
  local errorCode = 0
  local curRecord = {}
  
  -- make sure the database is open
  if dbHandle:isopen() == true then
    stmt = dbHandle:prepare ([[SELECT * FROM ]] .. tableName .. [[ WHERE ]] .. fieldName .. [[ = ?]])
    errorCode = stmt:bind_values(fieldValue)   
  end
    
  for row in stmt:nrows() do
    curRecord[#curRecord+1] = row
  end  
  stmt:reset()
      
  return curRecord
end

--[[
Function: recallAllRecords
Description:  recalls all the records from the table.
            Not to be used with Large Tables.
Parameters: 
  tableName- Name of the table in the database to use.
Returns:
  table containing all the records from the database table.
]]
function awtxReq.database.recallAllRecords(tableName)
  local stmt = nil
  local errorCode = 0
  local curRecord = {}
  
  -- make sure the database is open
  if dbHandle:isopen() == true then
    stmt = dbHandle:prepare([[SELECT * FROM ]] .. tableName )
    errorCode = stmt:bind_values()   
  end
    
  for row in stmt:nrows() do
    curRecord[#curRecord+1] = row
  end  
  stmt:reset()
      
  return curRecord
end 

