enableDebugPrint=false
printCnt=0
function printAppDebug(...)local printResult=""if printCnt==0 then
printCnt=1
end
if enableDebugPrint then
for i,v in ipairs(arg)do
printResult=printResult..tostring(v)end
print(printResult)end
end
FirmwareVersionStr=""FirmwareVersionNum=0
local versionTable=awtx.os.getVersionInfo("*all")FirmwareStr=versionTable[10]FirmwareVersionStr=versionTable[10].version
FirmwareDescStr=versionTable[10].desc
function changeFirmwareVersionToANumber(versionStr)local tmpSep,tmpversion,tmpdecimal
local tmpversion1,tmpversion2,tmpversion3,tmpversion4
local tmpversionnum
tmpSep="."tmpversion=versionStr..tmpSep
tmpdecimal=string.find(tmpversion,tmpSep,1,true)tmpversion1=string.sub(tmpversion,1,tmpdecimal-1)tmpversion=string.sub(tmpversion,tmpdecimal+1,string.len(tmpversion))tmpdecimal=string.find(tmpversion,tmpSep,1,true)tmpversion2=string.sub(tmpversion,1,tmpdecimal-1)tmpversion=string.sub(tmpversion,tmpdecimal+1,string.len(tmpversion))tmpdecimal=string.find(tmpversion,tmpSep,1,true)tmpversion3=string.sub(tmpversion,1,tmpdecimal-1)tmpversion=string.sub(tmpversion,tmpdecimal+1,string.len(tmpversion))tmpdecimal=string.find(tmpversion,tmpSep,1,true)tmpversion4=string.sub(tmpversion,1,tmpdecimal-1)tmpversionnum=(tmpversion1*1000000000)+(tmpversion2*1000000)+(tmpversion3*1000)+(tmpversion4*1)return tmpversionnum
end
FirmwareVersionNum=changeFirmwareVersionToANumber(FirmwareVersionStr)awtx.os.sleep(1000)function cycleWeightDisplay()end