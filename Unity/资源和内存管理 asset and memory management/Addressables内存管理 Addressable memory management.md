### AB包的组成

传统AB和Addressable的ab包功能都类似，由包头和数据段组成

包头包含有关AssetBundle 的信息，比如标识符、压缩类型和内容清单。清单是一个以Objects name为键的查找表。每个条目都提供一个字节索引，用来指示该Objects在AssetBundle数据段的位置。在大多数平台上，这个查找表是用平衡搜索树实现的。具体来说，Windows和OSX派生平台(包括IOS)都采用了红黑树。因此，构建清单所需的时间会随着AssetBundle中Assets的数量增加而线性增加。

数据段包含通过序列化AssetBundle中的Assets而生成的原始数据。如果指定LZMA为压缩方案的话，则对所有序列化Assets后的完整字节数组进行压缩。如果指定了LZ4，则单独压缩单独Assets的字节。如果不使用压缩，数据段将保持为原始字节流。

### 加载和引用计数

使用 ```Addressables.LoadAssetAsync``` 或 ```Addressables.InstantiateAsync``` 加载资源时，会自动加载资源所在AB包，以及加载所有依赖的AB包，并保持它们一直加载，直到您调用```Addressables.Release```

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

-   释放所有从该AB包加载的资源（以减少资源和AB包的引用计数），当AB包引用计数为0，表示AB包内没有任何资源被使用，AB包会被自动卸载，并释放之前加载的资源所占用的内存
-   仅释放部分资源，然后**将这些资源的引用都置为null**（必要操作，否则这些部分资源仍有来自这些引用的引用计数），再调用 **```Resources.UnloadUnusedAssets```** ，AB包会被卸载且释放资源所占内存，但Addressable无法识别这些事件。且**这个操作很慢，最好只在Loading等性能无关紧要的时机执行**。
-   每个资源都是单独的AB包，当资源释放时，AB包当然不再有任何计数，自然就被卸载了。但AB包太多也可能影响性能，详见 https://docs.unity3d.com/Packages/com.unity.addressables@1.16/manual/AddressablesFAQ.html#Is-it-better-to-have-many-small-bundles-or-a-few-bigger-ones

#### 预制体资源

实例化加载的预制体资源，然后立即释放预制体资源。如果预制体资源引用了其他资源，释放预制体资源会释放其引用的其他资源，导致实例化的物体也丢失资源引用

#### Resources.UnloadUnusedAssets

将所有未被引用的已加载资源释放

有的资源已经没有任何引用了但未被卸载，因为其引用或句柄引用丢失了（置为null或者引用它的实例被销毁了等情况），只能通过该函数进行释放。

### 参考

-   https://docs.unity3d.com/Packages/com.unity.addressables@1.15/manual/LoadingAddressableAssets.html
-   https://docs.unity3d.com/Packages/com.unity.addressables@1.15/manual/InstantiateAsync.html
-   https://docs.unity3d.com/Packages/com.unity.addressables@1.15/manual/MemoryManagement.html