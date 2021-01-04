### 异步加载
资源地址就是在Addressables Groups里看到的具体资源的地址，只要给定地址就能找到需要的资源
#### 使用地址加载
```csharp
// 参数为资源地址，包含后缀.prefab，handle.Result就是实例化的物体，而不是prefab
Addressables.InstantiateAsync("address of prefab");
// 参数为资源地址，包含资源后缀名
Addressables.LoadAssetAsync("address of asset");
```
#### 使用直接资源引用AssetReference加载

```csharp
AssetReference asset;
// 参数为资源地址，包含后缀.prefab
Addressables.InstantiateAsync(asset);
// 参数为资源地址，包含资源后缀名
Addressables.LoadAssetAsync(asset);
```

### 异步回调
#### 匿名委托
```csharp
Addressables.InstantiateAsync($"address of prefab").Completed += (handle) =>
{
    if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
    {
        var obj = handle.Result;
    }
}
```
#### 回调函数
```csharp
Addressables.InstantiateAsync($"address of prefab").Completed += OnCompleted;
private void OnCompleted(AsyncOperationHandle<SceneInstance> handle)
{
    if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
    {
        var obj = handle.Result;
    }
}
```
#### async
```csharp
public static async void PlaySoundEffect(AssetReference audioClipAsset)
{
    var handle = audioClipAsset.LoadAssetAsync<AudioClip>();
    await handle.Task;
    if (handle.Status == AsyncOperationStatus.Succeeded)
    {
        var audioClip = handle.Result;
    }
}
```
#### async Task<T>
```csharp
private async static Task<T> LoadAssetAsync<T>() where T : Object
{
    var path = "address of prefab";
    var handle = Addressables.LoadAssetAsync<T>(path);
    await handle.Task;
    if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
    {
        return handle.Result;
    }
    return null;
}

public static async Task<T> InstantiateAssetAsync<T>() where T : Object
{
    var obj = await LoadAssetAsync<T>();
}
```
#### 协程
```csharp
public IEnumerator Start() {
    AsyncOperationHandle<Texture2D> handle = Addressables.LoadAssetAsync<Texture2D>("mytexture");
    yield return handle;
    if (handle.Status == AsyncOperationStatus.Succeeded) {
        Texture2D texture = handle.Result;
        // The texture is ready for use.
        // ...
    // Release the asset after its use:
        Addressables.Release(handle);
    }
}
```

### 同步加载
[Unity Github Addressables](https://github.com/Unity-Technologies/Addressables-Sample/tree/master/Advanced/Sync%20Addressables)
if can not find, go to Unity github repository and find the repository named addresables-sample

### 本地打包流程
- 在AddressableAssetsData/AssetGroups下找到对应Group的配置，这里可以查看到Build Path的本地路径。
- 根据需要修改Advanced Options -> Bundle Mode
    - Pack Together：每个Group中，除了场景之外的资源，打成一个包，场景另外打一个包
    - Pack Separately：每个Group中的Entry分别打一个包，Entry即Group下的各个目录
    - Pack Together By Label：相同Label打一个包
- 手动打包：Addressables Group界面下，Build -> New Build -> Default Build Script 
- 主菜单File -> Build Settings -> Build

### AssetReference filter 过滤器
如果在Addressables Group界面里以文件夹为单位创建Entry，那么Inspector下的AssetReference打开的过滤器中会看到很多可选的
#### 筛选指定类型的asset
```csharp
[Serializable]
public class AssetReferenceSceneAsset : AssetReferenceT<SceneAsset>
{
    public AssetReferenceSceneAsset(string guid) : base(guid) { }
}
```
SceneAsset可换成其他资源类型，具体可通过在Project窗口右键Create，弹出的类型都是资源类型
#### 筛选Component
ComponentReference.cs
```csharp
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ComponentReference<TComponent> : AssetReference where TComponent : Component
{
    public ComponentReference(string guid) : base(guid)
    {
    }
    
    public new AsyncOperationHandle<TComponent> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        return Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.InstantiateAsync(position, Quaternion.identity, parent), GameObjectReady);
    }
   
    public new AsyncOperationHandle<TComponent> InstantiateAsync(Transform parent = null, bool instantiateInWorldSpace = false)
    {
        return Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.InstantiateAsync(parent, instantiateInWorldSpace), GameObjectReady);
    }
    public AsyncOperationHandle<TComponent> LoadAssetAsync()
    {
        return Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.LoadAssetAsync<GameObject>(), GameObjectReady);
    }

    AsyncOperationHandle<TComponent> GameObjectReady(AsyncOperationHandle<GameObject> arg)
    {
        var comp = arg.Result.GetComponent<TComponent>();
        return Addressables.ResourceManager.CreateCompletedOperation<TComponent>(comp, string.Empty);
    }

    public override bool ValidateAsset(Object obj)
    {
        var go = obj as GameObject;
        return go != null && go.GetComponent<TComponent>() != null;
    }
    
    public override bool ValidateAsset(string path)
    {
#if UNITY_EDITOR
        //this load can be expensive...
        var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        return go != null && go.GetComponent<TComponent>() != null;
#else
            return false;
#endif
    }

    public void ReleaseInstance(AsyncOperationHandle<TComponent> op)
    {
        // Release the instance
        var component = op.Result as Component;
        if (component != null)
        {
            Addressables.ReleaseInstance(component.gameObject);
        }

        // Release the handle
        Addressables.Release(op);
    }

}
```
```csharp
[Serializable]
public class ComponentReferenceYourComponent : ComponentReference<YourComponent>
{
    public ComponentReferenceYourComponent(string guid)
        : base(guid) { }
}
```
YourComponent为你的继承自Component的脚本组件
### BUG

#### 找不到Sprite资源

```csharp
Addressables.LoadAssetAsync("folder/sprite.png");
```
假设你的sprite在Addressables中的地址为```folder/sprite.png```
当使用版本大于1.3.8的Addressables时，在使用非Use Exsisting Build的模式时，无法找到资源。
但当你的sprite的地址为```sprite.png```，不带任何父目录时，则不会出现以上问题。

#### 正确的使用方法

- 使用版本1.3.8（或者更低版本，目前1.3.8 < 版本 <= 1.7.4的均存在该BUG）
-   ```csharp
    Addressables.LoadAssetAsync("folder/sprite.png[sprite]");
    // 对于sprite atlas中只有一个sprite的情形，最后的[sprite]是可以省略的，以下代码也有效
    Addressables.LoadAssetAsync("folder/sprite.png");
    // 对于sprite atlas中有多个sprite的情形，最后的[sprite_0]不能省略，[sprite_0]为sprite atlas中的目标sprite名称
    Addressables.LoadAssetAsync("folder/sprite.png[sprite_0]");
    ```

#### 初始场景载入之前就使用异步加载，一直加载不完成

比如使用了标签```[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]```的游戏启动函数中，在游戏加载初始场景之前使用Addressables进行异步加载，则一直不会加载完成

解决方法就是在初始空场景中用一个Mono脚本的Start来作为游戏启动函数

### Address中使用文件夹，文件夹名称和内部的资源名称有重复的部分，引发InvalidKeyException

``` csharp
string GetInternalIdFromFolderEntry(string keyStr, AddressableAssetEntry entry)
{
    var entryPath = entry.AssetPath;
    if (keyStr.StartsWith(entry.address + "/"))
        return keyStr.Replace(entry.address, entryPath);
    foreach (var l in entry.labels)
        if (keyStr.StartsWith(l + "/"))
            return keyStr.Replace(l, entryPath);
    return string.Empty;
}
```

``` csharp
keyStr = "Config/DebugConfigData.asset";
entry.address = "Config";
entryPath = "Assets/AddressablesResources/Config";
Debug.Log(keyStr.Replace(entry.address, entryPath) == "Assets/AddressablesResources/Config/DebugAssets/AddressablesResources/ConfigData.asset");
```

-   文件夹名称为aaa，路径为"Assets/aaa"
-   资源名称为aaabbb.asset
-   勾选aaa文件夹的Addressable选项，那么资源的地址就是"aaa/aaabbb.asset"
-   仅当有文件夹时才会进入到这个函数，而Replace会将aaa替换为全路径，即"aaa" -> "Assets/aaa"，所以最终结果是"Assets/aaa/Assets/aaabbb.asset"。绝了！~~你特么不会用IndexOf和SubString来分割字符串吗？~~

https://forum.unity.com/threads/addressable-entry-contains-two-more-same-word-leads-error.1032619/

#### 重命名资产的Address后，报错InvalidKeyException

##### Addressables 1.8.4版本

-   Project Settings -> Editor -> Enter Play Mode Options 勾选
-   Window -> Asset Management -> Addressables -> Groups -> Play Mode Script 选择 Use Asset Database
-   修改某个资源为错误的Address，在加载时会报错InvalidKeyException
-   将资源的Address修正，再次加载，仍会报错InvalidKeyException
-   将Project Settings -> Editor -> Enter Play Mode Options 不勾选（或者勾选Reload Domain），则加载正常
-   不执行上一步，而是改为重启Unity编辑器，也会加载正常

猜测是游戏启动后会先执行重建资源对应key的动作，再开始游戏。可能是某个静态构造函数内执行的，因为 Enter Play Mode Options 勾选 会导致静态类创建后即使下一次启动游戏也不会被销毁（静态类生命周期和Unity编辑器一致），静态构造函数就不会再次执行，但也可能是其他机制实现的

##### Addressables 1.16.10或更高版本

-   Project Settings -> Editor -> Enter Play Mode Options 勾选
-   Window -> Asset Management -> Addressables -> Groups -> Play Mode Script 选择 Use Asset Database
-   修改某个资源为错误的Address，在加载时会报错InvalidKeyException
-   将资源的Address修正，再次加载，仍会报错InvalidKeyException
-   将Project Settings -> Editor -> Enter Play Mode Options 不勾选（或者勾选Reload Domain），加载仍报错

懒得从源码找原因了，如果遇到这个问题，注意把Addressables版本回退到1.8.4或者其他有效版本

## 参考

[addressables sample unity official github](https://github.com/Unity-Technologies/Addressables-Sample)
