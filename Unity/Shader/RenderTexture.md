### GetTemporaryRT

在内部，Unity维护一个RenderTexture池来保存临时渲染纹理，调用该函数会从池中立即返回一个已经创建的（相同大小和格式）纹理，如果没有才会再创建一个RenderTexture到池中并返回它。

如果您要进行一系列后期处理blits，则对于性能而言，最好为每个blit获取并释放一个临时的RenderTexture，而不是先获取一个或两个RenderTexture并重用它们。这对移动（基于图块的）和多GPU系统最有利：GetTemporary将在内部执行[DiscardContents](https://docs.unity3d.com/ScriptReference/RenderTexture.DiscardContents.html)调用，这有助于避免对先前的渲染纹理内容进行昂贵的还原操作（疑问：自己维护的RenderTexture并调用DIscardContents是否就可以达到相同的效果？）

当这些临时纹理已经好几帧没有被使用后会被Destroy，以及内部执行DiscardContents的原因，您不能依赖于从GetTemporary函数获得的RenderTexture的任何特定内容。根据平台的不同，它可能是垃圾，也可能被清除为某种颜色。

### ReleaseTemporary

将RenderTexture回收到缓存池中以便GetTemporary重用，当好几帧没有人请求临时RenderTexture后，它们会被销毁

