local PASSWORD_SUPERVISOR="1793"local PASSWORD_SECRET="4862"local PASSWORD_SSID="5510"local PASSWORD_KEYTARE="8273"local PASSWORD_PARAM="365"local PASSWORD_SPECIAL="7732"local PASSWORD_OTHER="666"local PASSWORD_MULTIPLY="6858"local PASSWORD_APPVIEW="0"SCALE_DB_FileLocation=awtx.os.dbScaleConfigFile()function setSSID()supervisor.menuing=false
awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(SCALE_DB_FileLocation)assert(dbFile~=nil)if dbFile~=nil then
dbFile:exec("CREATE TABLE IF NOT EXISTS tblSettings (varID TEXT PRIMARY KEY, value TEXT, valueType TEXT)")sqlStr=string.format("UPDATE tblSettings SET varID= '802.11_OPTION_SSID', value= 'AWTX000', valueType= 'STRING' WHERE tblSettings.varID = '802.11_OPTION_SSID'")result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()awtx.display.clrDisplayBusy()supervisor.menuing=true
end
function setSSID1()supervisor.menuing=false
awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(SCALE_DB_FileLocation)assert(dbFile~=nil)if dbFile~=nil then
dbFile:exec("CREATE TABLE IF NOT EXISTS tblSettings (varID TEXT PRIMARY KEY, value TEXT, valueType TEXT)")sqlStr=string.format("UPDATE tblSettings SET varID= '802.11_OPTION_SSID', value= 'AWTX001', valueType= 'STRING' WHERE tblSettings.varID = '802.11_OPTION_SSID'")result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()awtx.display.clrDisplayBusy()supervisor.menuing=true
end
function setSSID2()supervisor.menuing=false
awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(SCALE_DB_FileLocation)assert(dbFile~=nil)if dbFile~=nil then
dbFile:exec("CREATE TABLE IF NOT EXISTS tblSettings (varID TEXT PRIMARY KEY, value TEXT, valueType TEXT)")sqlStr=string.format("UPDATE tblSettings SET varID= '802.11_OPTION_SSID', value= 'AWTX002', valueType= 'STRING' WHERE tblSettings.varID = '802.11_OPTION_SSID'")result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()awtx.display.clrDisplayBusy()supervisor.menuing=true
end
function setSSID3()supervisor.menuing=false
awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(SCALE_DB_FileLocation)assert(dbFile~=nil)if dbFile~=nil then
dbFile:exec("CREATE TABLE IF NOT EXISTS tblSettings (varID TEXT PRIMARY KEY, value TEXT, valueType TEXT)")sqlStr=string.format("UPDATE tblSettings SET varID= '802.11_OPTION_SSID', value= 'AWTX003', valueType= 'STRING' WHERE tblSettings.varID = '802.11_OPTION_SSID'")result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()awtx.display.clrDisplayBusy()supervisor.menuing=true
end
function setSSID4()supervisor.menuing=false
awtx.display.setDisplayBusy()awtx.os.sleep(busytime)dbFile=sqlite3.open(SCALE_DB_FileLocation)assert(dbFile~=nil)if dbFile~=nil then
dbFile:exec("CREATE TABLE IF NOT EXISTS tblSettings (varID TEXT PRIMARY KEY, value TEXT, valueType TEXT)")sqlStr=string.format("UPDATE tblSettings SET varID= '802.11_OPTION_SSID', value= 'AWTX004', valueType= 'STRING' WHERE tblSettings.varID = '802.11_OPTION_SSID'")result=dbFile:exec(sqlStr)end
dbFile:execute("COMMIT")dbFile:close()awtx.display.clrDisplayBusy()supervisor.menuing=true
end
supervisor={}menuingDefault=false
menuCurrentDefault=1
T1Default={text="Super",key=1,action="MENU",variable="SuperMenu"}T2Default={text="EXIT",key=2,action="FUNC",callThis=supervisor.exitFunction}topMenuDefault={T1Default,T2Default}resolveCircular1Default={topMenuDefault=topMenuDefault}supervisor.menuing=menuingDefault
supervisor.menuCurrent=menuCurrentDefault
supervisor.menuLevel=topMenuDefault
supervisor.menuCircular=resolveCircular1Default
assert(type(awtx.setupMenu)=='table',"Don't appear to have loaded the example module")function supervisor.exitFunction(param)supervisor.menuing=false
appExitSuperMenu()end
function supervisor.CheckWeighType()return checkweigh.CheckWeighType
end
function supervisor.menuVisible(menuItem)local retVal=false
local found=false
local variable,Compare,val
if menuItem.show==nil then
retVal=true
elseif type(menuItem.show)=="boolean"and menuItem.show==true then
retVal=true
elseif type(menuItem.show)=="string"then
retVal=true
elseif type(menuItem.show)=="table"then
Compare=menuItem.show
if Compare.callThis==nil or Compare.val1==nil then
retVal=true
elseif type(Compare.callThis)=="function"then
val=tonumber(Compare.callThis())if val==Compare.val1 then
retVal=true
else
retVal=false
end
end
else
retVal=false
end
return retVal
end
function supervisor.handleMenuActionFunction(menuItem)if type(menuItem.callThis)=="function"then
if menuItem.text==nil then
menuItem.callThis()else
menuItem.callThis(menuItem.text)end
end
end
function supervisor.showMenus()local menuStr=""local menuSelection
local menuActive=1
local menuKey={}local a,b,i
local numItems=0
for i=1,#supervisor.menuLevel do
if supervisor.menuVisible(supervisor.menuLevel[i])then
menuStr=menuStr..string.format("%s}",supervisor.menuLevel[i].text)table.insert(menuKey,supervisor.menuLevel[i].key)if supervisor.menuLevel[i].key==supervisor.menuCurrent then
menuActive=#menuKey
end
numItems=numItems+1
end
end
if numItems==1 then
a=menuKey[1]b=nil
else
menuSelection=awtx.setupMenu.selectMenu(menuStr,menuActive)a=menuKey[tonumber(menuSelection)]b=nil
end
collectgarbage()return a,b
end
function doMenuLoop()local result
local aKey,bKey,j
local menuIndex,myTable
result=awtx.keypad.set_RPN_mode(0)awtx.keypad.disableLuaKeyboardEvents()while(supervisor.menuing==true)do
aKey,bKey=supervisor.showMenus()if bKey==nil then
menuIndex=-1
for j=1,#supervisor.menuLevel do
if supervisor.menuLevel[j].key==aKey then
menuIndex=j
break
end
end
else
menuIndex=-1
end
if(menuIndex==-1)then
else
supervisor.menuCurrent=supervisor.menuLevel[menuIndex].key
if supervisor.menuLevel[menuIndex].action==nil then
elseif supervisor.menuLevel[menuIndex].action=="MENU"then
if type(supervisor.menuLevel[menuIndex].variable)=="string"then
myTable=supervisor.menuCircular[supervisor.menuLevel[menuIndex].variable]if supervisor.menuLevel[menuIndex].subMenu~=nil then
supervisor.menuCurrent=supervisor.menuLevel[menuIndex].subMenu
else
supervisor.menuCurrent=1
end
supervisor.menuLevel=myTable
end
elseif supervisor.menuLevel[menuIndex].action=="FUNC"then
supervisor.handleMenuActionFunction(supervisor.menuLevel[menuIndex])else
end
end
end
result=awtx.keypad.set_RPN_mode(1)awtx.os.sleep(500)awtx.keypad.clearKeyBuffer()awtx.keypad.enableLuaKeyboardEvents()end
SSIDTopMenu1={text=" SSID  ",key=1,action="MENU",variable="SSIDMenu"}SSIDTopMenu2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}SSIDTopMenu={SSIDTopMenu1,SSIDTopMenu2}SSIDMenu1={text="AWTX000",key=1,action="FUNC",callThis=setSSID}SSIDMenu2={text="AWTX001",key=2,action="FUNC",callThis=setSSID1}SSIDMenu3={text="AWTX002",key=3,action="FUNC",callThis=setSSID2}SSIDMenu4={text="AWTX003",key=4,action="FUNC",callThis=setSSID3}SSIDMenu5={text="AWTX004",key=5,action="FUNC",callThis=setSSID4}SSIDMenu6={text=" BACK  ",key=6,action="MENU",variable="SSIDTopMenu",subMenu=1}SSIDMenu={SSIDMenu1,SSIDMenu2,SSIDMenu3,SSIDMenu4,SSIDMenu5,SSIDMenu6}resolveCircular5={SSIDTopMenu=SSIDTopMenu,SSIDMenu=SSIDMenu}KeyTareTopMenu1={text="KEYTARE",key=1,action="MENU",variable="KeyTareMenu"}KeyTareTopMenu2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}KeyTareTopMenu={KeyTareTopMenu1,KeyTareTopMenu2}KeyTareMenu1={text="SCR-301 ",key=1,action="FUNC",callThis=tare.editKeyTare301Enable,show=true}KeyTareMenu2={text="PST-301 ",key=2,action="FUNC",callThis=tare.editPresetTare301Enable,show=true}KeyTareMenu3={text="SCR-303 ",key=3,action="FUNC",callThis=tare.editKeyTare303Enable,show=true}KeyTareMenu4={text="PST-303 ",key=4,action="FUNC",callThis=tare.editPresetTare303Enable,show=false}KeyTareMenu5={text="SCR-375 ",key=5,action="FUNC",callThis=tare.editKeyTare375Enable,show=true}KeyTareMenu6={text="PST-375 ",key=6,action="FUNC",callThis=tare.editPresetTare375Enable,show=false}KeyTareMenu7={text="SCR-305 ",key=7,action="FUNC",callThis=tare.editKeyTare305Enable,show=false}KeyTareMenu8={text="PST-305 ",key=8,action="FUNC",callThis=tare.editPresetTare305Enable,show=false}KeyTareMenu9={text="SCR-305G",key=9,action="FUNC",callThis=tare.editKeyTare305GTNEnable,show=false}KeyTareMenu10={text="PST-305G",key=10,action="FUNC",callThis=tare.editPresetTare305GTNEnable,show=false}KeyTareMenu11={text=" BACK   ",key=11,action="MENU",variable="KeyTareTopMenu",subMenu=1}KeyTareMenu={KeyTareMenu1,KeyTareMenu2,KeyTareMenu3,KeyTareMenu4,KeyTareMenu5,KeyTareMenu6,KeyTareMenu7,KeyTareMenu8,KeyTareMenu9,KeyTareMenu10,KeyTareMenu11}resolveCircular6={KeyTareTopMenu=KeyTareTopMenu,KeyTareMenu=KeyTareMenu}SpecialTopMenu1={text="SPECIAL",key=1,action="MENU",variable="SpecialEdit"}SpecialTopMenu2={text=" EXIT  ",key=2,action="FUNC",callThis=supervisor.exitFunction}SpecialTopMenu={SpecialTopMenu1,SpecialTopMenu2}SpecialEdit1={text="SCANNER",key=1,action="MENU",variable="ScannerEditor"}SpecialEdit2={text=" BACK  ",key=2,action="MENU",variable="SpecialTopMenu",subMenu=1}SpecialEdit={SpecialEdit1,SpecialEdit2}ScannerEditor1={text=" Port1 ",key=1,action="FUNC",callThis=scanner.editchannelNum1enable}ScannerEditor2={text=" Port2 ",key=2,action="FUNC",callThis=scanner.editchannelNum2enable}ScannerEditor3={text="A-Print",key=3,action="FUNC",callThis=scanner.editautoPrintAfterScan}ScannerEditor4={text=" BACK  ",key=4,action="MENU",variable="SpecialEdit",subMenu=1}ScannerEditor={ScannerEditor1,ScannerEditor2,ScannerEditor3,ScannerEditor4}resolveCircular8={SpecialTopMenu=SpecialTopMenu,SpecialEdit=SpecialEdit,ScannerEditor=ScannerEditor}function supervisor.dynamicMenu(topMenuTable,resolveCircularTable)local usermode
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)supervisor.menuing=true
supervisor.menuCurrent=1
supervisor.menuLevel=topMenuTable
supervisor.menuCircular=resolveCircularTable
doMenuLoop()awtx.display.setMode(usermode)end
function supervisor.SupervisorMenuEntry()local usermode
appEnterSuperMenu()usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)supervisor.menuing=true
supervisor.menuCurrent=1
supervisor.menuLevel=topMenu
supervisor.menuCircular=resolveCircular1
doMenuLoop()awtx.display.setMode(usermode)end
function supervisor.SSIDMenuEntry()local usermode
appEnterSuperMenu()usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)supervisor.menuing=true
supervisor.menuCurrent=1
supervisor.menuLevel=SSIDTopMenu
supervisor.menuCircular=resolveCircular5
doMenuLoop()awtx.display.setMode(usermode)end
function supervisor.SecretMenuEntry()local usermode
appEnterSuperMenu()usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)supervisor.menuing=true
supervisor.menuCurrent=1
supervisor.menuLevel=SecretTopMenu
supervisor.menuCircular=resolveCircular3
doMenuLoop()awtx.display.setMode(usermode)end
function supervisor.KeyTareMenuEntry()local usermode
appEnterSuperMenu()usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)supervisor.menuing=true
supervisor.menuCurrent=1
supervisor.menuLevel=KeyTareTopMenu
supervisor.menuCircular=resolveCircular6
doMenuLoop()awtx.display.setMode(usermode)end
function supervisor.ParamMenuEntry()local usermode
appEnterSuperMenu()usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)local tmpOutput_Type_String={}local tmpOutput_Prompt_String=""tmpOutput_Type_String=Output_Type_String
tmpOutput_Prompt_String=Output_Prompt_String
Output_Type_String=Matrix_Output_Type_String
Output_Prompt_String=Matrix_Output_Prompt_String
supervisor.menuing=true
supervisor.menuCurrent=1
supervisor.menuLevel=ParamTopMenu
supervisor.menuCircular=resolveCircular7
doMenuLoop()Output_Type_String=tmpOutput_Type_String
Output_Prompt_String=tmpOutput_Prompt_String
awtx.display.setMode(usermode)end
function supervisor.SpecialMenuEntry()local usermode
appEnterSuperMenu()usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)supervisor.menuing=true
supervisor.menuCurrent=1
supervisor.menuLevel=SpecialTopMenu
supervisor.menuCircular=resolveCircular8
doMenuLoop()awtx.display.setMode(usermode)end
function supervisor.OtherMenuEntry()local usermode
appEnterSuperMenu()usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)supervisor.menuing=true
supervisor.menuCurrent=1
supervisor.menuLevel=OtherTopMenu
supervisor.menuCircular=resolveCircular4
doMenuLoop()awtx.display.setMode(usermode)end
function supervisor.MultiplyMenuEntry()local usermode
appEnterSuperMenu()usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)supervisor.menuing=true
supervisor.menuCurrent=1
supervisor.menuLevel=MultiplyTopMenu
supervisor.menuCircular=resolveCircular9
doMenuLoop()awtx.display.setMode(usermode)end
function supervisor.PASSWORDENTEREDEVENT(passwordString)if passwordString==PASSWORD_SUPERVISOR then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()supervisor.SupervisorMenuEntry()appEnableSetpoints()usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif passwordString==PASSWORD_SSID then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()awtx.display.writeLine("SSID")supervisor.SSIDMenuEntry()appEnableSetpoints()awtx.os.sleep(displaytime*2)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif passwordString==PASSWORD_SECRET then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()awtx.display.writeLine("Secret")supervisor.SecretMenuEntry()appEnableSetpoints()awtx.os.sleep(displaytime*2)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif passwordString==PASSWORD_KEYTARE then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()awtx.display.writeLine("KEYTARE")supervisor.KeyTareMenuEntry()appEnableSetpoints()awtx.os.sleep(displaytime*2)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif passwordString==PASSWORD_PARAM then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()awtx.display.writeLine(" Param ")supervisor.ParamMenuEntry()appEnableSetpoints()awtx.os.sleep(displaytime*2)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif passwordString==PASSWORD_SPECIAL then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()awtx.display.writeLine("SPECIAL")supervisor.SpecialMenuEntry()appEnableSetpoints()awtx.os.sleep(displaytime*2)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif passwordString==PASSWORD_OTHER then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()awtx.display.writeLine("OTHER")appEnableSetpoints()awtx.os.sleep(displaytime*2)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif passwordString==PASSWORD_MULTIPLY then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)appDisableSetpoints()awtx.display.writeLine("MULTIPY")supervisor.MultiplyMenuEntry()appEnableSetpoints()awtx.os.sleep(displaytime*2)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)elseif passwordString==PASSWORD_APPVIEW then
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.writeLine(AppName)awtx.os.sleep(displaytime*2)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)else
usermode=awtx.display.setMode(DISPLAY_MODE_USER_MENU)awtx.display.writeLine("invalid")awtx.os.sleep(displaytime*2)usermode=awtx.display.setMode(DISPLAY_MODE_SCALE_OBJECT)end
appExitSuperMenu()awtx.keypad.clearKeyBuffer()end
awtx.os.registerPasswordEnteredEvent(supervisor.PASSWORDENTEREDEVENT)