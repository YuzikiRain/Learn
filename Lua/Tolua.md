### 初始化

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

### C#与Lua类型交互

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


### 传递table给C#端

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

