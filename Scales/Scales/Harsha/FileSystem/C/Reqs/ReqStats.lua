stats={}function stats.mean(t)local sum=0
local count=0
for k,v in pairs(t)do
if type(v)=='number'then
sum=sum+v
count=count+1
end
end
return(sum/count)end
function stats.mode(t)local counts={}for k,v in pairs(t)do
if counts[v]==nil then
counts[v]=1
else
counts[v]=counts[v]+1
end
end
local biggestCount=0
for k,v in pairs(counts)do
if v>biggestCount then
biggestCount=v
end
end
local temp={}for k,v in pairs(counts)do
if v==biggestCount then
table.insert(temp,k)end
end
return temp
end
function stats.median(t)local temp={}for k,v in pairs(t)do
if type(v)=='number'then
table.insert(temp,v)end
end
table.sort(temp)if math.fmod(#temp,2)==0 then
return(temp[#temp/2]+temp[(#temp/2)+1])/2
else
return temp[math.ceil(#temp/2)]end
end
function stats.standardDeviation(t)local m
local vm
local sum=0
local count=0
local result
m=stats.mean(t)for k,v in pairs(t)do
if type(v)=='number'then
vm=v-m
sum=sum+(vm*vm)count=count+1
end
end
result=math.sqrt(sum/(count-1))return result
end
function stats.maxmincount(t)local max=-math.huge
local min=math.huge
local count=0
for k,v in pairs(t)do
if type(v)=='number'then
max=math.max(max,v)min=math.min(min,v)count=count+1
end
end
return max,min,count
end