### UGUI的性能优化

-   减少不必要的Draw Call
-   避免Rebuild或减少Rebuild的开销
-   触发UI事件遍历了过多的启用Raycast Target的Graphic（Image、Text等）

### batch

如果符合条件，Canvas可以将一些Draw Call合批，以减少Draw Call数量（不同的Canvas之间不能batch）

#### 调试UI batch

打开Profiler，在UI这一栏可以看到有多少次batch，以及**打断合批的原因 break batching reason **

#### 合批条件

相同的material（当然也包括相同的纹理），[depth](https://zhuanlan.zhihu.com/p/339387759)相邻的UI元素可以合并

### 重建 Rebuild

包括RebuildLayout、UpdateGeometry、UpdateMaterial，分别有对应的设脏标识来触发```m_LayoutRebuildQueue.AddUnique``` 和 ```m_GraphicRebuildQueue.AddUnique```，下一帧再处理

Rebuild会触发batch，所以

-   RebuildLayout：由于布局控制器所管理的布局元素发生了变化（如顶点位置变化），会调用```LayoutRebuilder.MarkLayoutForRebuild```标记脏
-   UpdateGeometry：调用DoMeshGeneration，它再调用OnPopulateMesh
    -   OnPopulateMesh：virtual的生成VertexHelper的方法（VertexHelper是一个辅助类，仅包含顶点数据如位置、法线切线、顶点色、UV等，此时还未生成mesh），Image、RawImage、Text都分别重写了它，其中Image根据Image Type，Text则根据Text的设置（如overflow模式、字体大小等）来生成VertexHelper
        -   再次修改VertexHelper：从自身和子物体中取得所有IMeshModifier接口（如果实现了的话），逐个调用ModifyMesh方法来修改VertexHelper
        -   通过VertexHelper生成网格：```s_VertexHelper.FillMesh(workerMesh); canvasRenderer.SetMesh(workerMesh);```
-   UpdateMaterial：
    -   设置材质：```canvasRenderer.SetMaterial(materialForRendering, 0);```
    -   设置纹理：``` canvasRenderer.SetTexture(mainTexture);```

#### 触发重建的情况

-   SetVerticesDirty：OnEnable

### UI事件

为画布（或特定子物体）添加GraphicRaycaster组件，使得其所有启用Raycast Target的子物体能在发生UI事件时被遍历到

#### 优化

-   如果一个画布内的所有UI都不需要UI事件，可以直接移除GraphicRaycaster组件
-   如果某个Graphic组件不需要

GraphicRaycaster的父类BaseRaycaster在OnEnable里通过```RaycasterManager.AddRaycaster```添加自身到RaycasterManager的Raycaster列表中，当EventSystem产生事件时，会遍历RaycasterManager的Raycaster列表，为每个Raycaster调用Raycast函数来检查GraphicRaycaster所在的同一画布内的所有Graphic进行射线检测（考虑了Raycast Target是否启用，canvasRenderer是否被剔除，画布所用相机的模式等）

### 参考

-   https://learn.unity.com/tutorial/optimizing-unity-ui#5c7f8528edbc2a002053b5a2
-   https://zhuanlan.zhihu.com/p/366779113

