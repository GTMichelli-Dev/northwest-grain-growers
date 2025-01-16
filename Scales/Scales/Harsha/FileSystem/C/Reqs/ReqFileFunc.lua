file={}local runChecksumAfterCopy=true
appFolder="Apps"appFolderC="C:\\Apps"appFolderG="G:\\Apps"appFiles={{name="App1General.lua",csum=nil},{name="App2Accum.lua",csum=nil},{name="App3Count.lua",csum=nil},{name="App4Check.lua",csum=nil},{name="App5Batch.lua",csum=nil},{name="App6P-Hold.lua",csum=nil},{name="App7R-Disp.lua",csum=nil},{name="App8User.lua",csum=nil},{name="App9User.lua",csum=nil},{name="App1Sim375.lua",csum=nil},{name="App2Mid375.lua",csum=nil},{name="App3Adv375.lua",csum=nil},{name="App4Per375.lua",csum=nil},{name="App5Grad375.lua",csum=nil},{name="App6User375.lua",csum=nil},{name="App7User375.lua",csum=nil}}appExtraFiles={{name="App1GeneralWeighing.lua",csum=nil},{name="App2Accumulator.lua",csum=nil},{name="App3PartsCounting.lua",csum=nil},{name="App4SimpleCheckweighing.lua",csum=nil},{name="App5Batching.lua",csum=nil},{name="App6PeakHold.lua",csum=nil},{name="App7CheckMate.lua",csum=nil},{name="App7RemoteDisplay.lua",csum=nil},{name="App8User.lua",csum=nil},{name="App9User.lua",csum=nil},{name="App10CheckMate.lua",csum=nil},{name="App11CheckMate.lua",csum=nil},{name="App11Sim375.lua",csum=nil},{name="App12CheckMate.lua",csum=nil},{name="App12Mid375.lua",csum=nil},{name="App13Adv375.lua",csum=nil},{name="App13CheckMate.lua",csum=nil},{name="App14CheckMate.lua",csum=nil},{name="App14Per375.lua",csum=nil},{name="App15CheckMate.lua",csum=nil},{name="App15Grad375.lua",csum=nil},{name="App16CheckMate.lua",csum=nil},{name="App16User375.lua",csum=nil},{name="App17CheckMate.lua",csum=nil},{name="App17User375.lua",csum=nil},{name="App18CheckMate.lua",csum=nil},{name="App19CheckMate.lua",csum=nil},{name="App.db",csum=nil}}reqFolder="Reqs"reqFolderC="C:\\Reqs"reqFolderG="G:\\Reqs"reqFiles={{name="ReqAppMenu.lua",csum=nil},{name="ReqStats.lua",csum=nil},{name="ReqFileFunc.lua",csum=nil},{name="ReqAccum.lua",csum=nil},{name="ReqTare.lua",csum=nil},{name="ReqPLU.lua",csum=nil},{name="ReqCheckWeigh.lua",csum=nil},{name="ReqSetpoint.lua",csum=nil},{name="ReqBattery.lua",csum=nil},{name="ReqCount.lua",csum=nil},{name="ReqBatching.lua",csum=nil},{name="ReqGrading.lua",csum=nil},{name="ReqScaleKeys.lua",csum=nil},{name="ReqR-Disp.lua",csum=nil},{name="ReqDebug.lua",csum=nil}}reqExtraFiles={{name="DebugPrint.lua",csum=nil},{name="Req0AppMenu.lua",csum=nil},{name="Req0Menu.lua",csum=nil},{name="Req1SimpleStats.lua",csum=nil},{name="Req2FileFunc.lua",csum=nil},{name="Req3AccumDatabase.lua",csum=nil},{name="Req4TareDatabase.lua",csum=nil},{name="Req5PLUDatabase.lua",csum=nil},{name="Req6Checkweigh.lua",csum=nil},{name="Req7SetpointDatabase.lua",csum=nil},{name="Req8Battery.lua",csum=nil},{name="Req9Sample.lua",csum=nil},{name="Req10Batching.lua",csum=nil},{name="Req11Grading.lua",csum=nil},{name="Req12ScaleKeys.lua",csum=nil},{name="Req13RDisp.lua",csum=nil}}dbFolder="AWTX"dbFolderC="C:\\AWTX"dbFolderG="G:\\AWTX"dbFiles={{name="ScaleConfig.db",csum=nil},{name="CalibConfig.db",csum=nil},{name="TransactionLog.db",csum=nil},{name="ErrorLog.db",csum=nil},{name="AuditLog.db",csum=nil}}dbExtraFiles={{name="ScaleExtra.db",csum=nil}}dbappFolder="Database"dbappFolderC="C:\\Database"dbappFolderG="G:\\Database"dbappFiles={{name="AppData.db",csum=nil},{name="AppConfig.db",csum=nil}}dbappExtraFiles={{name="Accum.db",csum=nil},{name="Checkweigh.db",csum=nil},{name="PLU.db",csum=nil},{name="ScaleExtra.db",csum=nil},{name="Setpoint.db",csum=nil},{name="Tare.db",csum=nil},{name="App.db",csum=nil}}serviceMenuFolder="AWTX"serviceMenuFolderC="C:\\AWTX"serviceMenuFolderG="G:\\AWTX"serviceMenuFiles={{name="App0Menu.lua",csum=nil}}serviceMenuExtraFiles={{name="App0Extra.lua",csum=nil}}rootFolderC="C:\\"rootExtraFiles={{name="ftp_file_test_beta_0999.txt",csum=nil},{name="dayPass.dat",csum=nil}}function file.CopyFiles(passedFromPath,passedToPath,passedFiles,passedChecksumAfterCopy)local usermode,currentRPN,fromPath,toPath,result
local errorStatus=0
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)result=awtx.os.makeDirectory(passedFromPath)if result~=0 then
errorStatus=-1
end
result=awtx.os.makeDirectory(passedToPath)if result~=0 then
errorStatus=-2
end
for i=1,#passedFiles do
fromPath=passedFromPath.."\\"..passedFiles[i].name
toPath=passedToPath.."\\"..passedFiles[i].name
awtx.display.writeLine(string.format("File-%02d",i))result=awtx.os.copyFile(fromPath,toPath)if result~=0 then
errorStatus=-3
end
if passedChecksumAfterCopy and errorStatus==0 then
result,csum=awtx.os.checksumFile(toPath)if result~=0 then
end
if passedFiles[i].csum~=nil then
if csum==passedFiles[i].csum then
else
errorStatus=-4
end
else
end
end
end
if errorStatus==0 then
awtx.display.writeLine(" DONE ")else
awtx.display.writeLine(string.format("Err %2d",errorStatus))end
awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.DeleteFiles(passedPath,passedFiles)local usermode,currentRPN,tempPath,fromPath,toPath,result
local errorStatus=0
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)for i=1,#passedFiles do
tempPath=passedPath.."\\"..passedFiles[i].name
awtx.display.writeLine(string.format("File-%02d",i))result=awtx.os.deleteFile(tempPath)if result~=0 then
errorStatus=-3
end
end
if errorStatus==0 then
awtx.display.writeLine(" DONE ")else
awtx.display.writeLine(string.format("Err %2d",errorStatus))end
awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.ListDirs()local result
print("This is ListDirs")print("\n ")print("This is Internal ---------------------------------------------------------------------------------------")result=awtx.os.listDirectory("C:\\*")result=awtx.os.listDirectory("C:\\AWTX\\*")result=awtx.os.listDirectory("C:\\Apps\\*")result=awtx.os.listDirectory("C:\\Reqs\\*")result=awtx.os.listDirectory("C:\\Database\\*")result=awtx.os.listDirectory("C:\\Update\\*")result=awtx.os.listDirectory("C:\\scaleUpdate\\*")print("This is Internal ---------------------------------------------------------------------------------------")print("\n ")print("This is External ---------------------------------------------------------------------------------------")result=awtx.os.listDirectory("G:\\*")result=awtx.os.listDirectory("G:\\AWTX\\*")result=awtx.os.listDirectory("G:\\Apps\\*")result=awtx.os.listDirectory("G:\\Reqs\\*")result=awtx.os.listDirectory("G:\\Database\\*")result=awtx.os.listDirectory("G:\\Update\\*")result=awtx.os.listDirectory("G:\\scaleUpdate\\*")print("This is External ---------------------------------------------------------------------------------------")print("\n ")print("ListDirs Done")print("\n ")end
function file.CopyFromCtoG()local usermode,currentRPN
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.writeLine("Phase 1")tempFromPath=dbFolderC
tempToPath=dbFolderG
file.CopyFiles(tempFromPath,tempToPath,dbFiles,runChecksumAfterCopy)awtx.display.writeLine("Phase 2")tempFromPath=dbappFolderC
tempToPath=dbappFolderG
file.CopyFiles(tempFromPath,tempToPath,dbappFiles,runChecksumAfterCopy)awtx.display.writeLine("Phase 3")tempFromPath=serviceMenuFolderC
tempToPath=serviceMenuFolderG
file.CopyFiles(tempFromPath,tempToPath,serviceMenuFiles,runChecksumAfterCopy)awtx.display.writeLine("Phase 4")tempFromPath=appFolderC
tempToPath=appFolderG
file.CopyFiles(tempFromPath,tempToPath,appFiles,runChecksumAfterCopy)awtx.display.writeLine("Phase 5")tempFromPath=reqFolderC
tempToPath=reqFolderG
file.CopyFiles(tempFromPath,tempToPath,reqFiles,runChecksumAfterCopy)file.ListDirs()awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.CopyFromGtoC()local usermode,curretnRPN
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.writeLine("Phase 1")tempFromPath=dbFolderG
tempToPath=dbFolderC
file.CopyFiles(tempFromPath,tempToPath,dbFiles,runChecksumAfterCopy)awtx.display.writeLine("Phase 2")tempFromPath=dbappFolderG
tempToPath=dbappFolderC
file.CopyFiles(tempFromPath,tempToPath,dbappFiles,runChecksumAfterCopy)awtx.display.writeLine("Phase 3")tempFromPath=serviceMenuFolderG
tempToPath=serviceMenuFolderC
file.CopyFiles(tempFromPath,tempToPath,serviceMenuFiles,runChecksumAfterCopy)awtx.display.writeLine("Phase 4")tempFromPath=appFolderG
tempToPath=appFolderC
file.CopyFiles(tempFromPath,tempToPath,appFiles,runChecksumAfterCopy)awtx.display.writeLine("Phase 5")tempFromPath=reqFolderG
tempToPath=reqFolderC
file.CopyFiles(tempFromPath,tempToPath,reqFiles,runChecksumAfterCopy)file.ListDirs()awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.remAWTX()local usermode,curretnRPN,result
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)result=0
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)file.DeleteFiles(dbFolderC,dbExtraFiles)file.DeleteFiles(serviceMenuFolderC,serviceMenuExtraFiles)awtx.display.clrDisplayBusy()if result==0 then
awtx.display.writeLine(" DONE ")else
awtx.display.writeLine(string.format("Err1 %2d",result))awtx.os.sleep(2000)end
awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.remAPPS()local usermode,curretnRPN,result
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)result=0
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)file.DeleteFiles(appFolderC,appExtraFiles)awtx.display.clrDisplayBusy()if result==0 then
awtx.display.writeLine(" DONE ")else
awtx.display.writeLine(string.format("Err %2d",result))awtx.os.sleep(2000)end
awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.remDTBS()local usermode,currentRPN,result
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)result=0
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)file.DeleteFiles(dbappFolderC,dbappExtraFiles)awtx.display.clrDisplayBusy()if result==0 then
awtx.display.writeLine(" DONE ")else
awtx.display.writeLine(string.format("Err %2d",result))awtx.os.sleep(2000)end
awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.remREQS()local usermode,curretnRPN,result
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)result=0
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)file.DeleteFiles(appFolderC,reqFiles)file.DeleteFiles(reqFolderC,reqExtraFiles)awtx.display.clrDisplayBusy()if result==0 then
awtx.display.writeLine(" DONE ")else
awtx.display.writeLine(string.format("Err %2d",result))awtx.os.sleep(2000)end
awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.remRoot()local usermode,curretnRPN,result
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)result=0
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)file.DeleteFiles(rootFolderC,rootExtraFiles)awtx.display.clrDisplayBusy()if result==0 then
awtx.display.writeLine(" DONE ")else
awtx.display.writeLine(string.format("Err %2d",result))awtx.os.sleep(2000)end
awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.delAWTX()local usermode,curretnRPN,result1,result2
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)result1=0
result2=0
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)file.DeleteFiles(dbFolderC,dbFiles)file.DeleteFiles(serviceMenuFolderC,serviceMenuFiles)result1=awtx.os.deleteDirectory(dbFolderC)result2=awtx.os.deleteDirectory(serviceMenuFolderC)awtx.display.clrDisplayBusy()if result1==0 and result2==0 then
awtx.display.writeLine(" DONE ")else
awtx.display.writeLine(string.format("Err1 %2d",result1))awtx.os.sleep(2000)awtx.display.writeLine(string.format("Err2 %2d",result2))awtx.os.sleep(2000)end
awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.delAPPS()local usermode,currentRPN,result
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)result=0
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)file.DeleteFiles(appFolderC,appFiles)result=awtx.os.deleteDirectory(appFolderC)awtx.display.clrDisplayBusy()if result==0 then
awtx.display.writeLine(" DONE ")else
awtx.display.writeLine(string.format("Err %2d",result))awtx.os.sleep(2000)end
awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.delDTBS()local usermode,currentRPN,result
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)result=0
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)file.DeleteFiles(dbappFolderC,dbappFiles)result=awtx.os.deleteDirectory(dbappFolderC)awtx.display.clrDisplayBusy()if result==0 then
awtx.display.writeLine(" DONE ")else
awtx.display.writeLine(string.format("Err %2d",result))awtx.os.sleep(2000)end
awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.delREQS()local usermode,currentRPN,result
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)result=0
currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)awtx.display.setDisplayBusy()awtx.os.sleep(busytime)file.DeleteFiles(appFolderC,reqFiles)file.DeleteFiles(reqFolderC,reqFiles)result=awtx.os.deleteDirectory(reqFolderC)awtx.display.clrDisplayBusy()if result==0 then
awtx.display.writeLine(" DONE ")else
awtx.display.writeLine(string.format("Err %2d",result))awtx.os.sleep(2000)end
awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
local DB_FileLocation_Dummy
function file.createDummyDatabase()local simAppPath,usermode,currentRPN
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)if awtx.os.amISimulator()then
simAppPath=awtx.os.applicationPath()DB_FileLocation_Dummy=simAppPath..[[\Dummy.db]]else
DB_FileLocation_Dummy=[[C:\Database\Dummy.db]]end
dbFile=sqlite3.open(DB_FileLocation_Dummy)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.deleteDummyDatabase()local simAppPath,usermode,currentRPN
usermode=awtx.display.setMode(DISPLAY_MODE_USER_SCRIPT)currentRPN=awtx.keypad.get_RPN_mode()awtx.keypad.set_RPN_mode(0)if awtx.os.amISimulator()then
simAppPath=awtx.os.applicationPath()DB_FileLocation_Dummy=simAppPath..[[\Dummy.db]]else
DB_FileLocation_Dummy=[[C:\Database\Dummy.db]]end
result=awtx.os.deleteFile(DB_FileLocation_Dummy)awtx.display.setMode(usermode)awtx.keypad.set_RPN_mode(currentRPN)end
function file.copyConfigIn()fromPath="G:\\AWTX\\ScaleConfig.db"toPath="C:\\AWTX\\ScaleConfig.db"result=awtx.os.copyFile(fromPath,toPath)end
function file.copyConfigOut()fromPath="C:\\AWTX\\ScaleConfig.db"toPath="G:\\AWTX\\ScaleConfig.db"result=awtx.os.copyFile(fromPath,toPath)end
function file.setPrintFmts()local Format_01,Format_02,Format_03,Format_04,Format_05,Format_06,Format_07,Format_08,Format_09,Format_10
local Format_11,Format_12,Format_13,Format_14,Format_15,Format_16,Format_17,Format_18,Format_19,Format_20
local Format_21,Format_22,Format_23,Format_24,Format_25,Format_26,Format_27,Format_28,Format_29,Format_30
local Format_31,Format_32,Format_33,Format_34,Format_35,Format_36,Format_37,Format_38,Format_39,Format_40
local allFormats,myStr
Format_01=[[{T.GWT.2[W6]} {T.GWT.1} {T.UNIT.1}#CR#LF{T.SAT.2[W6]} {T.SAT.1} {T.UNIT.1}#CR#LF{T.NWT.2[W6]} {T.NWT.1} {T.UNIT.1}#CR#LF]]Format_02=[[{T.RTN.2}: {T.RTN.1}#CR#LF{T.GWT.2[W6]} {T.GWT.1} {T.UNIT.1}#CR#LF{T.SAT.2[W6]} {T.SAT.1} {T.UNIT.1}#CR#LF{T.NWT.2[W6]} {T.NWT.1} {T.UNIT.1}#CR#LF]]Format_03=[[{T.CNT.2} {T.CNT.1}#CR#LF]]Format_04=[[{T.NWT.2} {T.NWT.1} {T.UNIT.1}#CR#LF]]Format_05=[[{T.ACT.2[W1]} {T.ACT.1} {T.UNIT.1}#CR#LF]]Format_06=[[{T.PGW.2} {T.PGW.1} {T.UNIT.1}#CR#LF]]Format_07=[[{T.ACT.1} {T.UNIT.1} {T.ACT.2[W1]}#CR#LF]]Format_08=[[{T.RTN.2}: {T.RTN.1}#CR#LF{T.GAT.2} {T.GAT.1} {T.UNIT.1}#CR#LF {T.TAT.2} {T.TAT.1} {T.UNIT.1}#CR#LF  {T.NAT.2} {T.NAT.1} {T.UNIT.1}#CR#LF]]Format_09=[[#STX{T.GWT.1[W8]}{T.LK.1}{T.ACT.2[W1]}{T.STAT.1}#CR#LF]]Format_10=[[{A.2.1[W1]} {T.ACT.2[W1]} {T.ACT.1} {T.UNIT.1}#CR#LF]]Format_11=[[{T.ACT.2[W1]} {T.ACT.1} {T.UNIT.1} {T.DIS.1[W3]}#CR#LF]]Format_12=[[{T.ACT.2[W1]} {T.ACT.1} {T.UNIT.1} {T.WSTAT.1[W1]}#CR#LF]]Format_13=[[{T.TIM.1}#TAB{T.DAT.1}#CR#LF{A.1.2[W6]} {A.1.1[W6]}#CR#LF{T.GWT.2[W6]} {T.GWT.1} {T.UNIT.1}#CR#LF{T.SAT.2[W6]} {T.SAT.1} {T.UNIT.1}#CR#LF{T.NWT.2[W6]} {T.NWT.1} {T.UNIT.1}#CR#LF]]Format_14=[[{T.TIM.1} {T.DAT.1}#CR#LF  {T.GWT.2[W1]} {T.GWT.1} {T.UNIT.1}#CR#LF{A.2.1[W1]} {T.SAT.2[W1]} {T.SAT.1} {T.UNIT.1}#CR#LF  {T.NWT.2[W1]} {T.NWT.1} {T.UNIT.1}#CR#LF]]Format_15=[[{T.GAT.2} {T.GAT.1} {T.UNIT.1}#CR#LF]]Format_16=[[{T.NAT.2} {T.NAT.1} {T.UNIT.1}#CR#LF]]Format_17=[[{T.GAT.2} {T.GAT.1} {T.UNIT.1}#CR#LF {T.TAT.2} {T.TAT.1} {T.UNIT.1}#CR#LF  {T.NAT.2} {T.NAT.1} {T.UNIT.1}#CR#LF]]Format_18=[[{T.PNW.2} {T.PNW.1} {T.UNIT.1}#CR#LF]]Format_19=[[OD#CR#LFN#CR#LFq464#CR#LFQ812,20+0#CR#LFS2#CR#LFD8#CR#LFZT#CR#LFA55,650,3,4,1,1,N,"{T.TIM.1} {T.DAT.1}"#CR#LFB100,690,3,3,2,4,40,B,"ID   {A.1.1[W2]}"#CR#LFB180,690,3,3,2,4,40,B,"G  {T.GWT.1} {T.UNIT.1}"#CR#LFB265,690,3,3,2,4,40,B,"T  {T.SAT.1} {T.UNIT.1}"#CR#LFB370,690,3,3,2,4,40,B,"N  {T.NWT.1} {T.UNIT.1}"#CR#LFP1#CR#LF#FF]]Format_20=[[#CR#LFOD#CR#LFN#CR#LFq248#CR#LFQ173,24+0#CR#LFS2#CR#LFD8#CR#LFZT#CR#LFA18,8,0,3,1,1,N,"{T.TIM.1}"#CR#LFA134,8,0,3,1,1,N,"{T.DAT.1}"#CR#LFA30,47,0,4,1,1,N,"G {T.GWT.1} {T.UNIT.1}"#CR#LFA30,81,0,4,1,1,N,"T {T.SAT.1} {T.UNIT.1}"#CR#LFA30,116,0,4,1,1,N,"N {T.NWT.1} {T.UNIT.1}"#CR#LFP1#CR#LF#FF]]Format_21=[[#CR#LFOD#CR#LFN#CR#LFq464#CR#LFQ812,24+0#CR#LFS2#CR#LFD8#CR#LFZT#CR#LFA40,120,0,1,2,2,N,"{T.TIM.1} {T.DAT.1}"#CR#LFA60,225,0,1,3,4,N,"ID   {A.1.1[W2]}"#CR#LFA30,360,0,1,3,5,N,"G {T.GWT.1} {T.UNIT.1}"#CR#LFA30,490,0,1,3,5,N,"T {T.SAT.1} {T.UNIT.1}"#CR#LFA30,620,0,1,3,5,N,"N {T.NWT.1} {T.UNIT.1}"#CR#LFP1#CR#LF#FF]]Format_22=[[#CR#LFOD#CR#LFN#CR#LFq816#CR#LFQ1218,20+0#CR#LFS2#CR#LFD8#CR#LFZT#CR#LFA190,135,0,2,2,2,N,"{T.TIM.1} {T.DAT.1}"#CR#LFA190,275,0,2,3,3,N,"ID   {A.1.1[W2]}"#CR#LFA90,545,0,2,4,4,N,"G {T.GWT.1} {T.UNIT.1}"#CR#LFA90,685,0,2,4,4,N,"T {T.SAT.1} {T.UNIT.1}"#CR#LFA90,825,0,2,4,4,N,"N {T.NWT.1} {T.UNIT.1}"#CR#LFP1#CR#LF#FF]]Format_23=[[{T.TIM.1[F2]}#TAB{T.DAT.1[F2]}#CR#LF{T.GWT.2[W6]}: {T.GWT.1} {T.UNIT.1}#CR#LF{T.NWT.2[W6]}: {T.NWT.1} {T.UNIT.1}#CR#LF{T.SAT.2[W6]}: {T.SAT.1} {T.UNIT.1}#CR#LF]]Format_24=[[{A.63.2}: {A.63.1} {T.UNIT.1}#CR#LF]]Format_25=[[{A.64.2}: {A.64.1} {T.UNIT.1}#CR#LF]]Format_26=[[ {T.ACT.2[W1]}  {T.ACT.1[W6]} {T.UNIT.1[W2]} #CR#LF]]Format_27=[[{T.NWT.1} {T.UNIT.1} {A.63.2}#CR#LF]]Format_28=[[{A.56.2} = {A.56.1} {T.UNIT.1}#CR#LF{A.55.2} = {A.55.1} {T.UNIT.1}#CR#LF#CR#LF{A.44.2} = {A.44.1}#CR#LF{A.42.2} = {A.42.1}#CR#LF{A.43.2} = {A.43.1}#CR#LF{A.45.2} = {A.45.1} {T.UNIT.1}#CR#LF{A.49.2} = {A.49.1} {T.UNIT.1}#CR#LF{A.50.2} = {A.50.1} {T.UNIT.1}#CR#LF{A.47.2} = {A.47.1}#CR#LF{A.48.2} = {A.48.1} PCT#CR#LF{A.41.2} = {A.41.1}#CR#LF]]Format_29=[[{A.54.2} = {A.54.1} {T.UNIT.1}#CR#LF{A.53.2} = {A.53.1} {T.UNIT.1}#CR#LF#CR#LF{A.45.2} = {A.45.1} {T.UNIT.1}#CR#LF{A.46.2} = {A.46.1} {T.UNIT.1}#CR#LF]]Format_30=[[{A.29.2} {A.29.1} {T.UNIT.1}#CR#LF]]Format_31=[[{T.CAT.2} {T.CAT.1}#CR#LF]]Format_32=[[{T.ACT.1}]]Format_33=[[@{A.65.1}#CR{T.ACT.2[W1]} {T.ACT.1[W6]} {T.UNIT.1}#CR]]Format_34=[[{T.GWT.2[W6]} {T.GWT.1} {T.UNIT.1}#CR#LF{T.SAT.2[W6]} {T.SAT.1} {T.UNIT.1}#CR#LF{T.NWT.2[W6]} {T.NWT.1} {T.UNIT.1}#CR#LF]]Format_35=[[{T.GWT.2[W6]} {T.GWT.1} {T.UNIT.1}#CR#LF{T.SAT.2[W6]} {T.SAT.1} {T.UNIT.1}#CR#LF{T.NWT.2[W6]} {T.NWT.1} {T.UNIT.1}#CR#LF]]Format_36=[[{T.GWT.2[W6]} {T.GWT.1} {T.UNIT.1}#CR#LF{T.SAT.2[W6]} {T.SAT.1} {T.UNIT.1}#CR#LF{T.NWT.2[W6]} {T.NWT.1} {T.UNIT.1}#CR#LF]]Format_37=[[{T.GWT.2[W6]} {T.GWT.1} {T.UNIT.1}#CR#LF{T.SAT.2[W6]} {T.SAT.1} {T.UNIT.1}#CR#LF{T.NWT.2[W6]} {T.NWT.1} {T.UNIT.1}#CR#LF]]Format_38=[[{T.GWT.2[W6]} {T.GWT.1} {T.UNIT.1}#CR#LF{T.SAT.2[W6]} {T.SAT.1} {T.UNIT.1}#CR#LF{T.NWT.2[W6]} {T.NWT.1} {T.UNIT.1}#CR#LF]]Format_39=[[{T.GWT.2[W6]} {T.GWT.1} {T.UNIT.1}#CR#LF{T.SAT.2[W6]} {T.SAT.1} {T.UNIT.1}#CR#LF{T.NWT.2[W6]} {T.NWT.1} {T.UNIT.1}#CR#LF]]Format_40=[[{T.GWT.2[W6]} {T.GWT.1} {T.UNIT.1}#CR#LF{T.SAT.2[W6]} {T.SAT.1} {T.UNIT.1}#CR#LF{T.NWT.2[W6]} {T.NWT.1} {T.UNIT.1}#CR#LF]]allFormats={Format_01,Format_02,Format_03,Format_04,Format_05,Format_06,Format_07,Format_08,Format_09,Format_10,Format_11,Format_12,Format_13,Format_14,Format_15,Format_16,Format_17,Format_18,Format_19,Format_20,Format_21,Format_22,Format_23,Format_24,Format_25,Format_26,Format_27,Format_28,Format_29,Format_30,Format_31,Format_32,Format_33,Format_34,Format_35,Format_36,Format_37,Format_38,Format_39,Format_40}for i=1,#allFormats do
result=awtx.fmtPrint.set(i,allFormats[i])if(result==0)then
print(string.format("Print format number %d set correctly",i))else
myStr=awtx.fmtPrint.get(i)print(string.format("Error format mumber %d:\r\n%s",i,myStr))end
end
end
function file.getPrintFmts()local Format_01,Format_02,Format_03,Format_04,Format_05,Format_06,Format_07,Format_08
local allFormats,myStr
Format_01=[[{T.GWT.2} {T.GWT.1} {T.UNIT.1}{13}{10}{T.SAT.2} {T.SAT.1} {T.UNIT.1}{13}{10}{T.NWT.2} {T.NWT.1} {T.UNIT.1}{13}{10}]]Format_02=[[{T.RTN.2} {T.RTN.1}\r\n{T.GWT.2} {T.GWT.1} {T.UNIT.1}\r\n{T.SAT.2} {T.SAT.1} {T.UNIT.1}\r\n{T.NWT.2} {T.NWT.1} {T.UNIT.1}\r\n]]Format_03=[[{T.CNT.2} {T.CNT.1}]]Format_04=[[{T.NWT.2} {T.NWT.1} {T.UNIT.1}]]Format_05=[[{T.ACT.2} {T.ACT.1} {T.UNIT.1}]]Format_06=[[{T.PGW.2} {T.PGW.1} {T.UNIT.1}]]Format_07=[[{T.ACT.1} {T.UNIT.1} {T.ACT.2[W1]}]]Format_08=[[{T.RTN.2} {T.RTN.1}\r\n{T.GAT.2} {T.GAT.1} {T.UNIT.1}\r\n\{T.TAT.2} {T.TAT.1} {T.UNIT.1}\r\n{T.NAT.2} {T.NAT.1} {T.UNIT.1}\r\n]]allFormats={Format_01,Format_02,Format_03,Format_04,Format_05,Format_06,Format_07,Format_08}for i=1,#allFormats do
allFormats[i]=awtx.fmtPrint.get(i)print(string.format("%d: %s",i,allFormats[i]))end
end