enableDebugPrint=false
printCnt=0
function printAppDebug(...)local printResult=""if printCnt==0 then
if awtx.os.amISimulator()==1 then
awtx.simulator.clearscreen()end
printCnt=1
end
if enableDebugPrint then
for i,v in ipairs(arg)do
printResult=printResult..tostring(v)end
print(printResult)end
end
awtx.os.sleep(1000)function cycleWeightDisplay()end