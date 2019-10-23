local function Dump(array, str)
	print("----------Dump " .. str .. " Begin----------")
	for key, value in pairs(array) do
		print("-- " .. key .. ":" .. value)
	end
	print("----------Dump " .. str .. " End----------")
end

local function iDump(array, str)
	print("----------iDump " .. str .. " Begin----------")
	for index, value in ipairs(array) do
		print("-- " .. index .. ":" .. value)
	end
	print("----------iDump " .. str .. " End----------")
end

local array ={1,2,3,4,5,6,7}
print("-- full:" .. #array)
Dump(array, "full")
iDump(array, "full")

print("\n")
array[7] = nil
print("-- 7 nil:" .. #array)
Dump(array, "7 nil")
iDump(array, "7 nil")

print("\n")
array[3] = nil
print("-- 3 nil:" .. #array)
Dump(array, "3 nil")
iDump(array, "3 nil")

-- 这个取到的值不对
print("\n")
local arr4 = {1,[3]=2}
arr4[4] = 4
print("-- arr4:" .. #arr4)
Dump(arr4, "arr4")
iDump(arr4, "arr4")

-- 输出结果
-- full:7
----------Dump full Begin----------
-- 1:1
-- 2:2
-- 3:3
-- 4:4
-- 5:5
-- 6:6
-- 7:7
----------Dump full End----------
----------iDump full Begin----------
-- 1:1
-- 2:2
-- 3:3
-- 4:4
-- 5:5
-- 6:6
-- 7:7
----------iDump full End----------


-- 7 nil:6
----------Dump 7 nil Begin----------
-- 1:1
-- 2:2
-- 3:3
-- 4:4
-- 5:5
-- 6:6
----------Dump 7 nil End----------
----------iDump 7 nil Begin----------
-- 1:1
-- 2:2
-- 3:3
-- 4:4
-- 5:5
-- 6:6
----------iDump 7 nil End----------


-- 3 nil:2
----------Dump 3 nil Begin----------
-- 1:1
-- 2:2
-- 4:4
-- 5:5
-- 6:6
----------Dump 3 nil End----------
----------iDump 3 nil Begin----------
-- 1:1
-- 2:2
----------iDump 3 nil End----------


-- arr4:4
----------Dump arr4 Begin----------
-- 1:1
-- 3:2
-- 4:4
----------Dump arr4 End----------
----------iDump arr4 Begin----------
-- 1:1
----------iDump arr4 End----------

-- https://www.kancloud.cn/kancloud/openresty-best-practices/50391
-- 数组以nil为结束，跟'\0'一样

-- https://blog.csdn.net/twwk120120/article/details/79749232
-- lua table 数组为什么#array时得出的长度不一定是对的
-- 数组在每一个2次方位置,其容纳的元素数量都超过了该范围的50%,能达到这个目标的话,那么Lua认为这个数组范围就发挥了最大的效率。
-- 1到n 之间至少一半的空间会被利用（避免像稀疏数组一样浪费空间）；
-- 并且n/2+1到n 之间的空间至少有一个空间被利用（避免n/2 个空间就能容纳所有数据时申请n 个空间而造成浪费）。
