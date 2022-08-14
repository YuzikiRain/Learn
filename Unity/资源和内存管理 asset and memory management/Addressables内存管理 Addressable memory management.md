### 加载和引用计数

使用 ```Addressables.LoadAssetAsync``` 或 ```Addressables.InstantiateAsync``` 加载资源时，会自动加载资源所在AB包，以及加载所有依赖的AB包，并保持它们一直在内存中，直到您调用```Addressables.Release```

相当于自动完成了加载AB包和从AB包中加载资源

### 内存

-   加载AB包：如果是LZ4压缩，则只会加载包头信息，而不会加载数据段。否则会将整个AB包所有数据都加载到内存中
-   从AB包中加载资源：如果是LZ4压缩，仅从清单中取得数据索引，并从磁盘中加载对应资源到内存中。否则此前已经加载整个AB包了，可以立即取得对应资源

### 释放资源

调用 ```Addressables.Release``` 或 ```Addressables.ReleaseInstance``` 会减少加载资源的 ```AsyncOperation``` 的引用计数，如果为0，调用对应Provider的释放方法。

AssetBundleProvider调用的释放方法里会执行 ```bundleInstance.Unload(true)``` 来释放资源并销毁资源的实例化对象

不再引用资产（在探查器中蓝色部分的末尾表示）并不一定意味着资产已卸载。常见的适用场景涉及[AssetBundle中的](https://docs.unity3d.com/Manual/AssetBundlesIntro.html)多个Assets 。例如：

-   一个AB包中有资源C和D
-   加载C，此时C和AB包都有一个引用计数
-   再加载D，此时C和D的引用计数都为1，AB包则为2
-   释放C，此时C引用计数为0，D和AB包则都为1
-   此时AB包并不会被卸载，因为还有D未被释放（AB包引用计数为1，表示共计有1个资源引用计数就是D）

**释放部分而不是全部的AB包中的资源，AB包并不会被卸载，且释放的资源所使用的内存并不会被释放**

要卸载AB包，可通过以下方法：

-   释放所有从该AB包加载的资源（以减少资源和AB包的引用计数），然后**将这些资源的引用都置为null**（否则这些部分资源仍有来自这些引用的引用计数），当AB包引用计数为0，表示AB包内没有任何资源被使用，AB包会被自动卸载，并释放之前加载的资源所占用的内存
-   仅释放部分资源，然后**将这些资源的引用都置为null**（否则这些部分资源仍有来自这些引用的引用计数），再调用 **```Resources.UnloadUnusedAssets```** ，AB包会被卸载且释放资源所占内存，但Addressable无法识别这些事件。且**这个操作很慢，最好只在Loading等性能无关紧要的时机执行**。
-   每个资源都是单独的AB包，当资源释放时，AB包当然不再有任何计数，自然就被卸载了。但AB包太多也可能影响性能，详见 https://docs.unity3d.com/Packages/com.unity.addressables@1.16/manual/AddressablesFAQ.html#Is-it-better-to-have-many-small-bundles-or-a-few-bigger-ones

##### 自动释放

如果**A包仅a资源、C包仅c资源没有释放，c资源是由被依赖的a资源自动加载的**，那么调用```Addressables.Release```释放a资源时，资源a、c都会被释放

除此之外的情形，都不会自动释放依赖资源

#### 预制体实例依赖资源丢失

如果预制体资源引用了其他资源，预制体资源如果被真正地释放了，那么依赖资源也会被释放，此时所有预制体实例对依赖资源的引用也会丢失

#### 资源引用或加载资源句柄引用丢失的情况下如何释放资源

再次加载资源，并用此时能访问到的资源句柄或者资源引用来释放资源

```csharp
var handle = Addressables.LoadAssetAsync<GameObject>(address);
var prefab = handle.WaitForCompletion();
// 必须要调用这两句
Addressables.Release(handle);
Addressables.Release(prefab);
```

### 参考

-   https://docs.unity3d.com/Packages/com.unity.addressables@1.15/manual/LoadingAddressableAssets.html
-   https://docs.unity3d.com/Packages/com.unity.addressables@1.15/manual/InstantiateAsync.html
-   https://docs.unity3d.com/Packages/com.unity.addressables@1.15/manual/MemoryManagement.html
-   https://unity.cn/projects/addressables
-   https://blog.csdn.net/m0_46184795/article/details/108266234