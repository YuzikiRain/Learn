## 初始化

- 简单地跑一些lua代码

  ``` csharp
  public class Main : MonoBehaviour
  {
      LuaState lua = null;
  
      void Start()
      {   
          lua = new LuaState();
          lua.Start();
          string fullPath = Application.dataPath;
          lua.AddSearchPath(fullPath);
          // lua入口
          lua.Require("Main");
      }
  }
  ```

- 加载Wrap类型后再跑

``` csharp
public class TestGameObject: MonoBehaviour
{
    private string script =
        @"                                                
            local GameObject = UnityEngine.GameObject
            local ParticleSystem = UnityEngine.ParticleSystem                  
            
            local go = GameObject('go')
            go:AddComponent(typeof(ParticleSystem))                                        
        ";

    LuaState lua = null;

    void Start()
    {
        new LuaResLoader();
        lua = new LuaState();
        lua.LogGC = true;
        lua.Start();
        // LuaBinder是自动生成的，会将CustomSettings中指定生成的Wrap类在LuaBinder.Bind里绑定（成为lua全局变量）
        LuaBinder.Bind(lua);
        lua.DoString(script, "TestGameObject.cs");
    }
}
```

## 屏蔽某些类或字段不生成Wrap代码

- 字段成员：ToLuaExport.memberFilter下增加，比如"SkeletonRenderer.Start",
- 类：ToLuaMenu.dropType下增加，比如typeof(LayerMask),     
- 通用：添加标签`[NoToLuaAttribute]`

### 委托

- CustomSettings.cs中customDelegateList添加委托类型

  ``` csharp
  public static DelegateType[] customDelegateList = 
  {        
      ...
      // 自定义委托类型
      _DT(typeof(System.Action<GameObject>)),
      ...
  };
  ```

- 菜单栏 -> Gen Lua Delegates（或Generate All）

- 在LuaBinder.cs中查看生成的lua类型

  ``` csharp
  L.BeginModule("System");
  ...
  L.RegFunction("Action_UnityEngine_GameObject", System_Action_UnityEngine_GameObject);
  // System为module，方法名为Action_UnityEngine_GameObject
  ```

- 使用

  ``` lua
  local callback = System.Action_UnityEngine_GameObject(function(obj)
          print(obj)
      end)
  ```

### 模块

- 加载模块

  - lua ```require("moduleName")```

  - C#

    ``` csharp
    lua = new LuaState();
    lua.ReLoad("moduleName");
    ```

- 重载模块

  - lua

    ``` lua
    package.loaded = nil
    require("moduleName")
    ```

  - C#

    ``` csharp
    lua = new LuaState();
    lua.ReLoad("moduleName");
    ```

## C#与Lua类型交互

- lua调用C#：在CustomSetting中的customTypeList中添加要注册到lua的类型，并通过菜单项生成Wrap类型，以及在luastate初始化后会将对应类型和方法设置到_G里，因此就可以直接调用了

- C#调用lua

  ``` csharp
  private string luaScript =
          @"  function luaFunc(num)                        
                  return num + 1
              end
  
              test = {}
              test.luaFunc = luaFunc
          ";
  luaFunc = lua.GetFunction("test.luaFunc");
  int num = luaFunc.Invoke<int, int>(123456);
  ```

## 传递table给C#端

lua端

``` lua
t = { 
    dictionary = { first = "aaa", second = "bbb", }
    array = { "a", "b", "c", }, 
}
```

C#端

``` csharp
using LuaInterface;

public void Test(LuaTable luaTable)
{
    // 1.直接用索引器取得对应元素
    string first = (luaTable)["dictionary"]["first"];
    string second = (luaTable)["dictionary"]["second"];
    
    // 2.转换成数组再处理
    // 不能直接将object[]直接转换成string[]，需要迭代每个元素并构建string数组
    object[] array = ((luaTable)["array"]).ToArray();
    string[] stringArray = string[array.Length];
    for (int i = 0; i < array.Length; i++)
    {
        stringArray[i] = (string)array[i];
    }
    // 3.ToDictTable  ToArrayTable
}
```

## C#访问Lua

``` csharp
LuaState lua = new LuaState();
lua.Start();
// 访问全局变量
lua["Objs2Spawn"] = 5;

//cache成LuaTable进行访问
LuaTable table = lua.GetTable("varTable");
table["map.name"] = "new";  //table 字符串只能是key
Debug.Log(table["map"])

// 添加table
table.AddTable("newmap");
LuaTable table1 = (LuaTable)table["newmap"];
table1["name"] = "table1";
table1.Dispose();

// 调用lua的function
//Get the function object
luaFunc = lua.GetFunction("test.luaFunc");

if (luaFunc != null)
{
    int num = luaFunc.Invoke<int, int>(123456);
    Debugger.Log("generic call return: {0}", num);

    num = CallFunc();
    Debugger.Log("expansion call return: {0}", num);

    Func<int, int> Func = luaFunc.ToDelegate<Func<int, int>>();
    num = Func(123456);
    Debugger.Log("Delegate call return: {0}", num);

    num = lua.Invoke<int, int>("test.luaFunc", 123456, true);
    Debugger.Log("luastate call return: {0}", num);
}
```

参考：

- https://github.com/topameng/tolua/blob/master/Assets/ToLua/Examples/04_AccessingLuaVariables/AccessingLuaVariables.cs
- [tolua/CallLuaFunction.cs at master · topameng/tolua · GitHub](https://github.com/topameng/tolua/blob/master/Assets/ToLua/Examples/03_CallLuaFunction/CallLuaFunction.cs)

#### 数组长度

``` lua
list = {[0] = 0, [1] = 1, [2] = 2, }
```

``` csharp
public void Show(LuaTable luaTable)
{
	object[] objectArray = luaTable.ToArray();
    int[] intArray = new int[objectArray.Length];
    for (int i = 0; i < intArray.Length; i++)
    {
        intArray[i] = (int)objectArray[i];
    }
    // intArray为 {1, 2}，忽略了索引为0的元素（如果有负数索引的元素应该也会被忽略吧）
}
```

### Vector3 Vector2

与实数相乘时，Vector变量要放在最左边

``` lua
--- 正确
local vector = Vector3.one * 1.5
--- 错误
local vector = 1.5 * Vector3.one
```

这是因为ToLua对于Vector的乘法用元方法__mul实现的，且vector变量放在左边（不支持函数重载）

``` lua
Vector3.__mul = function(va, d)
	if type(d) == "number" then
		return _new(va.x * d, va.y * d, va.z * d)
	else
		local vec = va:Clone()
		vec:MulQuat(d)
		return vec
	end	
end
```

## Lua访问C#

### 数组

``` lua
--- 创建数组
local length = 10
--- ExampleClass在CustomSetting中已注册
local array = System.Array.CreateInstance(typeof(UnityEngine.XXXClass), length)
--- 访问数组
for i = 0, array.Length - 1 do
	print(array[i])
end
```

### List

#### lua中创建C#的list

CustomSettings添加`_GT(typeof(List<string>)),`

``` lua
local list = System.Collections.Generic.List_string()
list:Add("123")
print(list[0])
```

### Dictionary

#### lua中创建C#的dictionary

CustomSettings添加`_GT(typeof(Dictionary<int, string>)),`

``` lua
local dict = System.Collections.Generic.Dictionary_int_string()
dict:Add(123,"123")
```

### 泛型

ToLua并没有直接支持泛型，而是只能使用传入Type类型的方法，比如`GetComponent(typeof(UnityEngine.GameObject))`，因此泛型方法并不会生成到Wrap文件里，也无法被调用

推荐做法是在一个自定义的扩展方法类里定义一个同名的参数为Type的泛型方法，可以被生成到Wrap文件里并被调用

## 与其他语言交互的原理

### 事前准备

-   CustomSettings中添加要生成wrap文件的类

-   GenerateClassWraps生成对应类的wrap文件

-   启动lua虚拟机（LuaState）时，函数GenLuaBinder生成绑定类（wrap文件）


### 运行时

先为lua虚拟机生成所有已经绑定的类的同名table：`LuaBinder.Bind(lua);`

 以如下代码为例

``` lua
local tempGameObject = UnityEngine.GameObject("temp")
local transform = tempGameObject.GetComponent("Transform")
```

`UnityEngine.GameObject`就是`_G.UnityEngine.GameObject`，其指向一个构造函数

``` c#
public class UnityEngine_GameObjectWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.GameObject), typeof(UnityEngine.Object));
```



​    

参考：[【Unity游戏开发】tolua之wrap文件的原理与使用 - 马三小伙儿 - 博客园 (cnblogs.com)](https://www.cnblogs.com/msxh/p/9813147.html)

## Update

### FrameTimer

``` lua
local updateFunction = function()
   ---每帧调用一次 
end
--- 新建timer，参数分别为：update函数，多少帧调用一次，总共调用多少次（-1表示无限次）
local updateFrameTimer = FrameTimer.New(updateFunction, 1, -1)
--- 启动timer
updateFrameTimer:Start()
--- 停止timer
updateFrameTimer:Stop()
```

### UpdateBeat

``` lua
local updateFunction = function()
   ---每帧调用一次 
end
--- 创建
if self.updater == nil then
	self.updater = UpdateBeat:CreateListener(updateFunction, self)    
end
--- 开始
CoUpdateBeat:AddListener(self.updater)
--- 结束
CoUpdateBeat:RemoveListener(self.updater)
```

## 可嵌套协程

[LuaUtil/CustomCoroutine at master · YuzikiRain/LuaUtil (github.com)](https://github.com/YuzikiRain/LuaUtil/tree/master/CustomCoroutine)
