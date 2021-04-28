### AB包的组成

传统AB和Addressable的ab包功能都类似，由包头和数据段组成

包头包含有关AssetBundle 的信息，比如标识符、压缩类型和内容清单。清单是一个以Objects name为键的查找表。每个条目都提供一个字节索引，用来指示该Objects在AssetBundle数据段的位置。在大多数平台上，这个查找表是用平衡搜索树实现的。具体来说，Windows和OSX派生平台(包括IOS)都采用了红黑树。因此，构建清单所需的时间会随着AssetBundle中Assets的数量增加而线性增加。

数据段包含通过序列化AssetBundle中的Assets而生成的原始数据。如果指定LZMA为压缩方案的话，则对所有序列化Assets后的完整字节数组进行压缩。如果指定了LZ4，则单独压缩单独Assets的字节。如果不使用压缩，数据段将保持为原始字节流。

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

#### 隐式依赖

如果资源A引用了B资源，A资源显式地指定了所在的包，而B没有，那么B也会被打到A所在的包中。如果还有另一个资源C也引用了B资源，那么B还会再被打包到C所在的包里，A和C资源被加载时会分别去自动加载所依赖的在各自包中的资源B，这两个B资源互相之间没有任何关系。这样可以避免包之间的依赖。

#### 显式依赖

如果ABC都分别各自显式地指定了一个包，那么此时A或B资源加载时就会自动加载C资源所在的C包，从而产生包之间的依赖。
如果C包因为有部分资源没有释放而无法卸载，即使释放了A资源和A中的其他所有资源，A包也无法释放，因为它所依赖的C包还未被释放

### 隐式加载

一个资源可能在加载时又引用了其他资源，比如timeline资源（在A包中）中使用了AudioClip（在B包中），**这种情况不会在打包时产生隐式依赖而打包到同一个包里**，但是一旦timeline资源被加载并使用，它又会去加载所需要的AudioClip，如果AudioClip所在的B包里有任何资源没有被释放，B包就不可能被卸载（调用Resources.UnloadUnusedAssets只会释放无引用资源），那么依赖了AudioClip的timeline也一直无法释放。

隐式依赖打包策略原本是为了解决这种问题，但是这种情况却无效

#### 预制体资源

实例化加载的预制体资源，然后立即释放预制体资源。如果预制体资源引用了其他资源，释放预制体资源会释放其引用的其他资源，导致实例化的物体也丢失资源引用

#### Resources.UnloadUnusedAssets

将所有未被引用的已加载资源释放

有的资源已经没有任何引用了但未被卸载，因为其引用或句柄引用丢失了（置为null或者引用它的实例被销毁了等情况），只能通过该函数进行释放。

### 参考

-   https://docs.unity3d.com/Packages/com.unity.addressables@1.15/manual/LoadingAddressableAssets.html
-   https://docs.unity3d.com/Packages/com.unity.addressables@1.15/manual/InstantiateAsync.html
-   https://docs.unity3d.com/Packages/com.unity.addressables@1.15/manual/MemoryManagement.html
-   https://unity.cn/projects/addressables