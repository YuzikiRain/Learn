### 2018

-   SRP（Scriptable Render Pipeline）：
    -   SRP模板包括LWRP（2019变为URP）、HDRP
    -   Shader Graph：可视化shader编辑器
    -   Post Processing Stack v2：插件，适用于2019，与2020开始内置的后处理不同
-   ECS（Entity Component System）、C# Job System、Burst Compiler
-   内置插件ProBuilder、ProGrid、PolyBrush，改进的DCC工作流

### 2019

-   HDRP内置后处理（与Post Processing Stack v2不同）
-   Animation Rigging
-   DOTS
-   UI Elements（不推荐，与传统UGUI差别过大，还未经过很多项目的验证）

#### 小功能

-   编辑器控制台中的堆栈可点击其链接追踪到指定位置
-   快捷键管理器
-   Incremental Garbage Collection：增量式垃圾回收
-   Unified Search：全局搜索插件
-   可配置Enter Playmode：更快地进入playmode
-   Timeline Signal（不推荐，需要太多额外组件，很繁琐，建议自己实现一套事件机制）
-   Scene视图可对物体进行显示、隐藏，而不是通过控制物体Active属性
-   Adaptive Performance：试用移动设备散热管理来提高性能
-   Unity & Havok Physics：仅能用于基于DOTS的项目
-   Script only patch builds
-   移动设备通知：即用型保留机制和基于计时器的游戏玩法
-   AR Foundation

### 2020

#### 2020.1

-   URP Camera Stacking
-   Device Simulator 设备模拟器：在编辑器中测试app在各种设备上的运行情况
-   Light Settings Assets 光照设置资产：每个场景的光照设置使用独立的资产而不再保存于场景中
-   Ray Tracing 光线追踪（预览版）：Alembic 支持更复杂的内容和动画

##### 小功能

-   Animation Rigging：支持双向动画切换（IK和FK）
-   可拆分的Inspector：物体右键菜单选择Properties
-   Package Manager支持自定义Package
-   Progress window & task progress：右下角的任务进度栏，支持异步，
-   FBX importer增加轴向转换设置，为SketchUp对象导入自定义属性
-   在场景上下文中编辑预制体：预制体会继承一部分上下文属性
-   Standalone Profiler 独立的性能分析器：在编辑器中或播放模式下可获得更干净的分析器数据(Window->Analysis->Profiler(Standalone process)
-   2D Physics支持逻辑帧模拟：Project Settings->Physics 2D设置Simulate Mode为Update，就可以在Update中使用Rigidbody.MovePosition等方法了（因为每次2d物理模拟都发生在Update而不是FixedUpdate里了）

#### 2020.2

-   Transparent Material：在HDRP的SSR解决方案中支持
-   Lighting debug view modes、Histogram exposure：HDRP可用
-   GPU/CPU Lightmapper：效果更好，光反射次数更多
-   Screen Space Ambient Occlusion（SSAO）：URP中可用（以Render Feature方式实现）
-   URP global illumination updates：光照模式区别具体见https://zhuanlan.zhihu.com/p/157992819
-   URP Complex Lit Shader：相比Lit Shader新增了Clear Coat属性

##### 小功能

-   Quick Search可跨场景搜索
-   Unity Hub新增更多模板：2d platformer microgame, Karting Microgame, FPS Microgame LEGO Microgame等
-   编辑器交互优化：
    -   Inspector中的可排序列表（支持Array和List）
    -   Mesh inspector中的可视化blend shapes
    -   一次可从Project视图拖拽多个Prefab到场景中
    -   可在preview视图中直接预览sprite sheets（精灵切片）
-   Unity Recorder通过Apple ProRes编码器生成高质量原生输出
-   2D Animation整合2D IK
-   更多2d预制资源：如带物理效果的sprite等
-   Samsung Adaptive Performance2.0
-   asmdef inpsector新增Root Namespace：用于组织代码和避免命名冲突
-   使用运行时API公开Profiler统计数据
-   C#8 支持：switch表达式、可空类型改进
-   Roslyn分析器支持：可在后台运行C#代码分析