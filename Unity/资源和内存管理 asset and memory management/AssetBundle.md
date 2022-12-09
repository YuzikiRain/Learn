## 资源加载

-   [Unity - Manual: Using AssetBundles Natively (unity3d.com)](https://docs.unity3d.com/Manual/AssetBundles-Native.html)
-   [Assets, Resources and AssetBundles - Unity Learn](https://learn.unity.com/tutorial/assets-resources-and-assetbundles)

unity使用AssetBundle来打包资源，在构建的程序上使用的资源以AB包为单位存储在硬盘上

要使用一个不需要实例化的资源，比如Sprite等：

-   将ab包从硬盘解压并读取到内存：`AssetBundle bundle = AssetBundle.LoadFromFile(assetBundlePath)`
-   从内存中的ab包加载对应的资源，并保持资源到ab包的引用：`Sprite sprite = bundle.LoadAsset<GameObject>(assetPath);`

此时内存中存在解压的ab包（包含sprite），sprite资源，也就是说sprite资源实际上在内存中存在2份

### AssetBundle.LoadFromMemory

如果使用`AssetBundle.LoadFromMemory`，则是使用C#的byte数组加载ab包，然后将源数据从byte数组复制到新分配的连续本机内存块中，最终加载的资产将在内存中存在3份：c#的byte数组、ab包在本机内存的副本、资产本身的副本。因此不推荐使用该API

### AssetBundle.LoadFromFile

仅加载AB包的文件头，而不加载剩余的二进制资产数据，这些资产数据将在实际调用`bundle.LoadAsset`时按需加载

#### 编辑器环境

在 Unity 编辑器中，该API会将整个 AssetBundle 加载到内存中，就好像字节是从磁盘上读出来的，并且使用了 `AssetBundle.LoadFromMemoryAsync`。如果在 Unity 编辑器中分析项目，则此 API 可能会导致在 AssetBundle 加载期间出现内存峰值。这不会影响设备上的性能

## 压缩

[Unity - Manual: AssetBundle compression (unity3d.com)](https://docs.unity3d.com/Manual/AssetBundles-Cache.html)

### AB包的组成

传统AB和Addressable的ab包功能都类似，由包头和数据段组成

包头包含有关AssetBundle 的信息，比如标识符、压缩类型和内容清单。清单是一个以Objects name为键的查找表。每个条目都提供一个字节索引，用来指示该Objects在AssetBundle数据段的位置。在大多数平台上，这个查找表是用平衡搜索树实现的。具体来说，Windows和OSX派生平台(包括IOS)都采用了红黑树。因此，构建清单所需的时间会随着AssetBundle中Assets的数量增加而线性增加。

数据段包含通过序列化AssetBundle中的Assets而生成的原始数据。

- 如果指定LZMA为压缩方案的话，则对所有序列化Assets后的完整字节数组进行压缩。
- 如果指定了LZ4，则单独压缩单独Assets的字节。
- 如果不使用压缩，数据段将保持为原始字节流。

### LZ4

如果使用LZ4压缩，AB包中的资源将会分块压缩，文件头中将维护资产路径到二进制资产数据的偏移的映射表。

加载某个资源时，从资源映射表中可以快速得到所在的块的偏移，然后仅加载该块。其余部分不会被加载，因此内存中仅有最小化的资源副本

### LZMA

此压缩格式是表示整个 AssetBundle 的数据流，这意味着如果您需要从这些存档中读取Asset，则必须解压缩整个流。

### Uncompressed

未压缩的资产捆绑包。Unity 在使用BuildAssetBundleOptions 时构建的。UncompressedAssetBundle不需要解压缩，但会占用更多磁盘空间。未压缩的资产捆绑包是 16 字节对齐的。

## 卸载资源

要完全卸载一个资源，需要：

-   卸载资源本身：2种方式
    -   `Resources.UnloadAsset`：可卸载非GameObject和Component的资源，比如Texture、Mesh等真正的资源。而由Prefab加载出来的Object或Component，则不能通过该函数来进行释放。
    -   `GameObject.Destroy`（对于prefab实例化的实例）
-   卸载AssetBundle：[AssetBundle.Unload(bool unloadAllLoadedObjects)](https://docs.unity3d.com/ScriptReference/AssetBundle.Unload.html)，对于参数unloadAllLoadedObjects
    -   false：仅释放ab包的内存副本
    -   true：不仅释放ab包的内存副本，还会释放ab包内所有已经加载的资源
-   卸载无引用的资源：`Resources.UnloadUnusedAssets();`

### AssetBundle.Unload(false)

该方式不会释放ab包的已加载资源，需要使用者手动管理并释放

一个例子如下：

假设预制体P从名为A的AB包中加载，引用了材质M（来自名为B的AB包）

-   在加载P之后（称为P1），A包会被加载，且B包也会因为含有P依赖的M而自动加载
-   用P1来Instantiate一个实例obj，该实例也引用相同的材质M
-   在调用`AssetBundle.Unload(false)`后，P1和A包之间的链接被断开，ab包被卸载。
    但是P1没有被卸载，且实例obj不受影响，因为其材质M也没有被卸载，因为是Instantiate出来的副本而不是prefab资源本身，所以也不会被释放。
    调用`GameObject.Destroy`来销毁obj，obj被释放，但其引用的材质M仍存在，仍需要手动释放
-   再次加载A包，并加载资源P
-   此时资源P已经不再是之前的P1了，而是一个新的资源，称为P2。如果之前没有持有P1的引用，则无法再释放P1了（只能通过`Resources.UnloadUnusedAssets`）

如果改为调用`AssetBundle.Unload(true)`，则P1、A包、材质M都会被卸载，实例obj会丢失材质M而显示洋红色，实例obj同样不会被释放

因此一般存在2种卸载资源的策略：

-   每次加载资源后，立即调用`AssetBundle.Unload(false)`释放ab包，然后手动管理加载的资源的引用，资源不再使用时手动释放。一些无法被引用的资源则通过`Resources.UnloadUnusedAssets`统一释放（最好在loading等性能无关的地方）
    -   优点：立即释放ab包的内存
    -   缺点：需要通过`Resources.UnloadUnusedAssets`释放无引用资源，需要在合适时机调用，否则卡顿明显。如果选择在loading时才调用，则loading之前一些内存一直不会被释放
-   每次加载资源后，手动管理加载的资源的引用（采用引用计数等），当某个包的所有资源不再被使用时（比如引用计数为0），立即调用`AssetBundle.Unload(true)`释放包和包的所有已加载资源
    -   优点：如果管理得当，则永远不需要通过`Resources.UnloadUnusedAssets`来释放，避免了卡顿
    -   缺点：没有立即释放ab包的内存，导致内存占用偏多

### Resources.UnloadUnusedAssets

将所有未被引用（资源引用和代码引用）的已加载资源释放

该函数已经有异步版本，不同于之前的同步方法会阻塞主线程较长时间

一些加载的资源的代码引用丢失了，无法通过`Resources.UnloadAsset`进行释放，或者是一些资源是因为其他资源依赖了它们而被自动加载的（但是使用者没有它们的引用），此时只能通过该函数进行卸载（在此之前要确保这些资源没有再被其他资源或代码引用）

[Unity - Scripting API： Resources.UnloadUnusedAssets (unity3d.com)](https://docs.unity3d.com/ScriptReference/Resources.UnloadUnusedAssets.html)

## 资源依赖和分组策略

### 隐式依赖

如果资源a引用了b资源，a资源显式地指定了所在的包，而b没有显式指定所在包，那么b也会被打到a所在的包中。如果还有另一个资源c也引用了b资源，那么b还会再被打包到c所在的包里，a和c资源被加载时会分别去自动加载所依赖的在各自包中的资源b

**这两个b资源互相之间没有任何联系**。这样可以避免包之间的依赖。但是硬盘和内存中会存在多个相同的资源。

### 显式依赖

如果ABC都分别各自显式地指定了一个包，那么此时A或B资源加载时就会自动加载C资源所在的C包，从而产生包之间的依赖。

### 隐式加载

一个资源可能在加载时又引用了其他资源，比如timeline资源（在A包中）中使用了AudioClip（在B包中），**这种情况不会在打包时产生隐式依赖而打包到同一个包里**，但是一旦timeline资源被加载并使用，它又会去加载所需要的AudioClip，如果AudioClip所在的B包里有任何资源没有被释放，B包就不可能被卸载（调用Resources.UnloadUnusedAssets只会释放无引用资源），那么依赖了AudioClip的timeline也一直无法释放。

隐式依赖打包策略原本是为了解决这种问题，但是这种情况却无效

### 分组策略

[Unity - Manual: Preparing Assets for AssetBundles (unity3d.com)](https://docs.unity3d.com/Manual/AssetBundles-Preparing.html)