- 函数的多返回值（如**unpack**）可作为table的元素 tableA = {unpack(tableB)}

- 用元方法**add**实现的加法重载一般不满足加法交换律，因为两个操作数一般有不同的元方法，而最终执行的方法取决于使用的是哪个操作数的元方法

- 语言提供了 **a.name** 这样的语法糖来替代 **a["name"]**

- **???rawget(this, "function")** 可用于调用任意拼接名称的函数

- **__newindex**键 可用于禁止使用全局变量：setmetatable(_G, {__newindex = function(){print("禁止使用全局变量") throw exception} })

- **__index = function**的用法 像C#属性 罗马数字转换

- **continue**实现：在循环内部最外层添加循环和退出条件

- 语法糖：**classA.function(classA)** **classA:function()** 

- 语法糖：**tableA = {"a", "b", "c"}** 等同于 **tableA = {[1] = "a", [2] = "b", [3] = "c"}**

- **require**在整个程序中只会执行一次，再次调用时返回当时return的对象（可以理解为首次调用时，_G.returnValue = 返回值，下次调用时直接返回它，像是全局单例）

- 元表中可以有特殊的key（前缀为两个下划线__），对应一个事件

- 协程中的错误处理

    ```lua
    local succeed, errorInfo = coroutine.resume(co)
    if not succeed then
        print(errorInfo)
    end
    ```

    
