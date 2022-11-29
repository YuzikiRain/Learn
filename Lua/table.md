## 语法糖 ```.```

这几种表达式作用相同 ```table["x"] = "abc" --> table.x = "abc" --> table = { x = "abc"}```
`table.x`的写法暗示了table具有固定的、预定义的字符串key
`table["x"]`的写法暗示了table可能用任何字符串作为key

## 长度

- `#table`： `#`是长度操作符，返回table的最后一个索引值（将`nil`作为界定数组结尾的标识，因此如果一个table中有value为`nil`的元素将无法返回实际长度）
- `table.getn`：等同于`#table`
- `table.maxn`：返回table的最大正索引（因此如果有负索引或者非数值类型作为key的元素，该函数无法正常工作）

如果要保证取得集合的长度，需要自己手动用table实现相关的集合类型

## 索引

- 构造式： `t[1] = 123 --> t = { [1] = 123 }`
- **默认索引从1开始（而不是0）**，用table构造表达式来初始化table时，如果没有指定key，如```a = {10,20}```，则自动使用数字进行索引，即第一个元素是`table[1]`而不是`table[0]`
- 如果要让索引从0开始，且让之后的索引递增，可以这么写 ``` days = {[0] = "Monday", "Tuesday"}```
- **不推荐以0作为起始索引，因为大部分内建函数都假设以索引1起始**
- 最后一个元素后边可以加逗号
- 可以在构造式中使用分号代替逗号，来显示地分隔构造式中不同的成分，如 ```table = {x = 10, y = 45; "one", "two"}``

## unpack

接收一个table，从下标1开始返回table的所有元素（多返回值）

``` lua
local t = {1, 2, 3}
--- lua5.3
local a,b,c = table.unpack(t)  --- 返回 1, 2, 3，是多返回值
print(a,b,c)
```

## 排序

要求：

- key连续
- table中间不能有nil的元素

``` lua
local a = {id = 1}
local b = {id = 2}
--- 注意这里开始key并不是连续的了
local c = {id = 5}
local t1 = 
{
    [1] = a,
    [2] = b,
    [5] = c,
}
table.sort(t1, function(left, right)
   return left.id < right.id
end)
```

结果并不是排序了的，需要改为以下代码

``` lua
local a = {id = 1}
local b = {id = 2}
local c = {id = 5}
local t2 = { a, b, c, }
table.sort(t2, function(left, right)
   return left.id < right.id
end)
```

