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

## 委托

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

## 模块

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

## Lua与C#交互的原理

这位大佬写的比较全面 [lua(tolua)与C#交互以及泄漏的整理与总结_c# lua_脱发怪的博客-CSDN博客](https://blog.csdn.net/qq_29261149/article/details/122876092)

### C#调用Lua

#### 初始化虚拟机

``` c#
state = new LuaState();
state.Start();

//使用文件调用Lua
//手动添加一个lua文件搜索地址
string sceneFile = Application.dataPath + "/LuaStudy";
state.AddSearchPath(sceneFile);

state.Require(luaFile);//载入文件
```

#### 调用Lua方法

**简要来说，就是将函数名推到栈顶，然后将需要的参数（按参数列表顺序）也推到栈顶，然后调用Call或者Invoke来调用lua方法，最后将结果也返回到栈顶。**

``` c#
luaFunc = lua.GetFunction("test.luaFunc");
int num = luaFunc.Invoke<int, int>(123456);
```

##### GetFunction

如果是调用`public LuaFunction GetFunction(string name, bool beLogMiss = true)`，则会以函数名为key，WeakReference为value存放到字典funcMap中。

（虽然是名称是GetFunction）PushLuaFunction将函数名通过`LuaDLL.lua_pushstring(L, funcName)`放到交互栈顶。

计算出函数引用句柄reference，以reference为key，WeakReference为value存放到字典funcRefMap中。

如果是调用`public LuaFunction GetFunction(int reference)`则同理，只是跳过funcMap这一步。

##### Call或Invoke

调用`LuaTable.GetLuaFunction(funcName)`将函数推到栈上，然后调用LuaFunction的`public R1 Invoke<T1, R1>(T1 arg1)`方法或`Call`方法，其中包括将参数推到栈上，以及调用 `LuaDLL.lua_call`来进行C#对C的方法的调用。

##### C#调用C

C#调用C的代码是通过P/invoke, 即平台调用，.net 提供了一种托管代码调用非托管代码的机制。通过DllImport特性实现，把c的相关函数声明成 static， extern的形式，还可以为方法的参数和返回值指定自定义封送处理信息。具体可以参考[MSDN的描述](https://link.zhihu.com/?target=https%3A//docs.microsoft.com/en-us/dotnet/standard/native-interop/pinvoke)

``` c#
// LuaDLL.cs
const string LUADLL = "tolua";
[DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
public static extern void lua_call(IntPtr luaState, int nArgs, int nResults);
```

``` c
// tolua.c源代码文件，编译生成tolua.cs
#include "lua.h"
```

``` c
// luajit-2.1/src/lua.h
LUA_API void  (lua_call) (lua_State *L, int nargs, int nresults);
```

##### C调用Lua

如何在C中执行等同于Lua执行`a = f("how", t.x, 14)`的操作。

``` c
lua_getfield(L, LUA_GLOBALSINDEX, "f"); // 从全局table取得字段（类型为函数）f，并推到栈顶。等同于lua_getglobal(L, "f");
lua_pushstring(L, "how");  				// 将参数推到栈顶
lua_getfield(L, LUA_GLOBALSINDEX, "t"); // 从全局table取得字段t，并推到栈顶。
lua_getfield(L, -1, "x"); 				// 从栈顶元素t（类型为table）取得字段x，并推到栈顶。
lua_remove(L, -2); 						// 要取的是x，因此将从栈顶倒数第二个的元素t移除
lua_pushinteger(L, 14); 				// 将参数推到栈顶
lua_call(L, 3, 1); 						// 调用方法，带有3个参数，1个返回值
lua_setfield(L, LUA_GLOBALSINDEX, "a"); // 将栈顶元素弹出，并设置为全局table的字段a的值。等同于lua_setglobal(L, a)
```

参考：[ToLua框架下C#与Lua代码的互调_达也酱的博客-CSDN博客_tolua luastate](https://blog.csdn.net/fjjaylz/article/details/86578489)

### Lua调用C#

具体参考这位大佬[Unity中C#与Lua的交互 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/395361399)，讲的很清楚。

#### 将c函数注册到lua中

[Learn/与其他语言的交互.md at master · YuzikiRain/Learn · GitHub](https://github.com/YuzikiRain/Learn/blob/master/Lua/与其他语言的交互.md)

#### 生成Wrap文件

CustomSettings中添加要生成wrap文件的类

GenerateClassWraps生成对应类的wrap文件

``` c#
public class UnityEngine_GameObjectWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.GameObject), typeof(UnityEngine.Object));
```

#### 运行时

##### 注册

启动lua虚拟机（LuaState）时，LuaBinder调用所有已绑定类对应的wrap文件的Register方法，其中进行了注册类型、注册方法、注册变量。

``` c#
L.BeginClass(typeof(UnityEngine.GameObject), typeof(UnityEngine.Object));
L.RegFunction("GetComponent", GetComponent);
L.RegVar("transform", get_transform, null);
```

注册类型

``` c#
// LuaState.BeginClass()
...
    
if (metaMap.TryGetValue(t, out reference))
{
    LuaDLL.tolua_beginclass(L, name, baseMetaRef, reference);
    RegFunction("__gc", Collect);
}
else
{
    reference = LuaDLL.tolua_beginclass(L, name, baseMetaRef);
    RegFunction("__gc", Collect);                
    BindTypeRef(reference, t);
}
```

注册方法：将一个方法委托

``` c#
public void RegFunction(string name, LuaCSFunction func)
{
    IntPtr fn = Marshal.GetFunctionPointerForDelegate(func);
    LuaDLL.tolua_function(L, name, fn);            
}
```

经过注册，为lua虚拟机生成所有已经绑定的类的同名table：`LuaBinder.Bind(lua);`

比如lua中执行`local tempGameObject = UnityEngine.GameObject("temp")`，实际上注册时已经生成了名为UnityEngine全局table，其GameObject键

##### 执行

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

参考：

- [【Unity游戏开发】tolua之wrap文件的原理与使用 - 马三小伙儿 - 博客园 (cnblogs.com)](https://www.cnblogs.com/msxh/p/9813147.html)
- [【ToLua】C#和Lua的交互细节 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/109198841)

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
