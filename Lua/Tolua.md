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
        LuaBinder.Bind(lua);
        lua.DoString(script, "TestGameObject.cs");
    }
}
```

### 调用

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

  

