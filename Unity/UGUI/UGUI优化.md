## Profiler

收集分析数据后，可能会得出几个结论。

-   如果`Canvas.BuildBatch`似乎使用了过多的 CPU 时间，那么可能的问题是单个 Canvas 上的 `Canvas Renderer`组件数量过多。请参阅画布步骤的拆分画布部分。

-   如果在 GPU 上绘制 UI 花费了过多的时间，并且帧调试器指示片段着色器管道是瓶颈，则 UI 可能会超过 GPU 能够实现的像素填充率。最可能的原因是 UI overdraw。请参阅填充率、画布和输入步骤的修正填充率问题部分。

-   如果图形重建使用过多的CPU，如大部分CPU时间流向`Canvas.SendWillRenderCanvases`所示，则需要进行更深入的分析。`Graphic Rebuild`过程的某些部分可能是原因。

-   如果`WillRenderCanvas`的大部分时间都花在`IndexedSet_Sort`或`CanvasUpdateRegistry_SortLayoutList`内，那么就会花时间对脏布局组件列表进行排序。考虑减少画布上的布局组件数量。有关可能的补救措施，请参阅将布局替换为矩形转换和拆分画布部分。

-   如果似乎在`Text_OnPopulateMesh`上花费了过多的时间，那么罪魁祸首就是文本网格的生成。有关可能的补救措施，请参阅best fit和禁用画布部分，如果正在重建的大部分文本实际上没有更改其基础字符串数据，请考虑拆分画布中的建议。

-   如果时间花在`Shadow_ModifyMesh`或`Outline_ModifyMesh`（或任何其他 `ModifyMesh `实现）上，那么问题在于计算网格修改器所花费的时间过多。考虑删除这些组件并通过静态图像实现其视觉效果。

-   如果 `Canvas.SendWillRenderCanvases` 中没有特定的热点，或者它似乎每帧都在运行，则问题可能是动态元素与静态元素组合在一起，并迫使整个 Canvas 过于频繁地重建。请参阅拆分画布步骤。

## overdraw

-   减少重叠UI、UI不透明部分（常见的有：大尺寸UI中间镂空、中间往四周渐变的UI尺寸过大）
-   显示全屏UI时，不绘制之前的UI或禁用其他相机
    不绘制UI需要通过禁用Canvas组件、改变layer、设置CanvasRenderer组件的cull属性、父UI设置scale为0、将UI位置设置到UI相机视锥体之外，而不是设置UI或父物体CanvasGroup的alpha属性或物体的`SetActive(false)`
-   对于非全屏UI，可考虑将其他相机的全部或一部分渲染到RenderTexture上，设置给UI，以减少其他相机的实时渲染。

## 简化UI结构

-   减少不必要的UI
-   简化UI层次结构，考虑使用单个烘焙的UI而不是多个UI的组合（因为这些UI可能是静态的，永远不会变化）
-   使用材质属性来进行颜色乘法而不是通过额外的上层UI的颜色进行透明度混合以达到改变色调的目的

## Canvas Rebuild

UGUI会为每个UI组件生成几何图形，这包括运行动态布局代码，生成多边形以表示 UI Text组件中的字符，以及将尽可能多的几何图形合并到单个网格中，以最大程度地减少绘制调用。

重建过程：

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

性能瓶颈：

-   如果画布上可绘制 UI 元素的数量很大，则计算批处理本身将变得非常昂贵。这是因为对元素进行排序和分析的成本与画布上可绘制 UI 元素的数量呈线性增长。
-   如果画布经常弄脏，则可能会花费过多的时间刷新更改相对较少的画布

### Child order

Unity UI 是从后到前构造的，对象在层次结构中的顺序决定了它们的排序顺序。

层次结构中较早的对象被视为层次结构中较晚的对象后面的对象。批处理是通过从上到下遍历层次结构并收集使用**相同材料、相同纹理**且没有中间层的所有对象来构建的。

“中间层”是具有不同材质的图形对象，**其边界框与两个原本可批处理的对象重叠**，并放置在两个可批处理对象之间的层次结构中。中间层强制批处理被破坏。

优化：

-   调整UI位置以消除边界框的重叠
-   尽可能调整中间层到可批处理对象的上方或下方以尽可能合批

### Splitting Canvases

-   将静态元素和动态元素分离到不同的父Canvas或子Canvas中（在大多数其他情况下，子画布更方便，因为它们从其父画布继承其显示设置）
-   Canvas之间无法进行跨Canvas的合批，因此不要为了一些太简单的UI而拆分Canvas

## Graphic Raycaster

Graphic Raycaster是一个相对简单的实现，它迭代所有将“Raycast Target”设置设置为 true 的图形组件。

对于每个Raycast Target，Raycaster都会执行一组测试。如果通过了所有测试，则会将其添加到命中列表中。

#### 光线投射实现详细信息

测试是：

-   如果Raycast Target处于活动状态、已启用且已绘制（即具有几何体）
-   如果输入点位于Raycast Target所附加到的RectTransform内
-   如果光线投射目标具有或属于任何[ICanvasRaycastFilter](http://docs.unity3d.com/ScriptReference/ICanvasRaycastFilter.html) 组件的子组件（在任何深度），并且该Raycast Filter组件允许光线投射。

然后，按深度对命中Raycast Target列表进行排序，过滤反转目标，并过滤以确保删除摄像机后面渲染的元素（即在屏幕中不可见）。

如果在Graphic Raycaster的*“Blocking Objects”*属性上设置了相应的标志，则图形光线投射器也可以将光线投射到 3D 或 2D 物理系统中。（从脚本中，该属性被命名为[blockingObjects](http://docs.unity3d.com/ScriptReference/UI.GraphicRaycaster-blockingObjects.html)）

如果启用了 2D 或 3D 遮挡对象，则在光线投射遮挡物理图层上的 2D 或 3D 对象下方绘制的任何光线投射目标也将从命中列表中消除。

然后返回最终的命中列表。

优化：

-   只为需要接受事件的UI组件启用Raycast Target
-   如果有多个需要响应事件的复合UI（比如Image和Text都改变颜色的一个按钮），最好将Raycast Target放在复合UI的根目录。当单个Raycast Target接收到事件时，它可以将事件转发到复合UI的每个相关组件。

## 优化UI控件

### Text

#### Mesh Rebuild

每当更改 UI 文本组件时或未修改Text组件但禁用并重新启用该组件时，Text组件都必须重新计算用于显示实际文本的多边形。

如果要显示或隐藏文本，不要通过启用、禁用该组件或包含该组件的物体的方式，而是使用禁用Canvas等方式。

### 动态字体和字体图集

#### 专用的字体渲染器

对于包含固定字符集的情况（比如数字0~9、艺术字等），使用自定义字体

另一种方式是使用Image、Sprite等来代替文字

## shader

使用没有alphaclip、mask等花哨功能的自定义shader

``` glsl
SubShader
{
    Tags
    { 
        "Queue"="Transparent" 
        "IgnoreProjector"="True" 
        "RenderType"="Transparent" 
        "PreviewType"="Plane"
        "CanUseSpriteAtlas"="True"
    }

    Cull Off
    Lighting Off
    ZWrite Off
    ZTest [unity_GUIZTestMode]
    Blend SrcAlpha OneMinusSrcAlpha

    Pass
    {
    CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"
        #include "UnityUI.cginc"

        struct appdata_t
        {
            float4 vertex   : POSITION;
            float4 color    : COLOR;
            float2 texcoord : TEXCOORD0;
        };

        struct v2f
        {
            float4 vertex   : SV_POSITION;
            fixed4 color    : COLOR;
            half2 texcoord  : TEXCOORD0;
            float4 worldPosition : TEXCOORD1;
        };

        fixed4 _Color;
        fixed4 _TextureSampleAdd;
        v2f vert(appdata_t IN)
        {
            v2f OUT;
            OUT.worldPosition = IN.vertex;
            OUT.vertex = mul(UNITY_MATRIX_MVP, OUT.worldPosition);

            OUT.texcoord = IN.texcoord;

            #ifdef UNITY_HALF_TEXEL_OFFSET
            OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
            #endif

            OUT.color = IN.color * _Color;
            return OUT;
        }

        sampler2D _MainTex;
        fixed4 frag(v2f IN) : SV_Target
        {
            return (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
        }
    ENDCG
    }
}
```

-   [Optimizing Unity UI - Unity Learn](https://learn.unity.com/tutorial/optimizing-unity-ui#5c7f8528edbc2a002053b5a2)

-   [Fundamentals of Unity UI](https://learn.unity.com/tutorial/optimizing-unity-ui#5c7f8528edbc2a002053b5a0)