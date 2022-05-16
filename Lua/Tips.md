## loadstring(str)

可以访问全局变量，但是无法访问局部变量，因为`str`内的局部变量仅限于该环境内，可通过包装成function来引入外部的局部变量

``` lua
local f1 = loadstring("return function(left, right) return left == right end")
local f2 = f1()
for i = 1, 2 do
    print(f3(1, i))
end

print("------------------")

local t1 = {
    "return function(left, right) return left == right end",
    "return function(left, right) return left == right+1 end",
}
for i = 1, 2 do
    local f1 = loadstring(t1[i])
    local f2 = f1()
    print(f2(101, 101))
end
```

打印如下，外部的`i`是有效的

``` lua
true
false
------------------
true
false
```

