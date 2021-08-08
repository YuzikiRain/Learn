### 调试

- 安装IDEA社区版2021
- File->Setting->Plugins->EmmyLua
- DebugConfigurations里添加配置，选择Emmy Debugger(NEW)，Connection选择```TCP(Debugger connect IDE)```，将下方的编辑器通过TCP连接IDE的lua启动代码复制并粘贴到lua入口处
- 点击debug（虫子）按钮，然后（从lua入口处）运行lua脚本

### 注解

``` lua
-- class注解用于标记哪个是类
---@class ClassA
---@field field1 number
local ClassA = class()
ClassA.field1 = 1

-- type注解用于标记局部/全局变量类型，因为有些变量的类型无法通过函数返回值获得
---@type ClassA
local ClassA = require("Lua.ClassA")

-- generic注解用于表示类型参数的类型（这里的prototype相当于类型参数）
-- param注解用于表示函数各个参数的类型
-- return注解用于表示返回值类型
---@generic TCharacter : ClassA
---@param prototype TCharacter
---@return TCharacter
function Character.Create(prototype)
    return prototype()
end
```

