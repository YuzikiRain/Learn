## Mask

## RectMask2D

旧版本（2019.4之前）的方式

``` c#
inline float UnityGet2DClipping (in float2 position, in float4 clipRect)
{
    float2 inside = step(clipRect.xy, position.xy) * step(position.xy, clipRect.zw);
    return inside.x * inside.y;
}
```

2019.4

``` c#
// fs
#ifdef UNITY_UI_CLIP_RECT
half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);  
color.a *= m.x * m.y;
#endif
```

注意：

- 以下Rect矩形指的都是RectMask2D所在物体的Rect

### 裁剪

- `_ClipRect.xy`是矩形左下角坐标，`_ClipRect.zw`是矩形右上角坐标，**`_ClipRect.zw - _ClipRect.xy`表示矩形的宽高**
- `OUT.mask.xy`等于**矩形左下角到点的位置组成的向量**加上到**矩形右上角到点的位置组成的向量**：
    `OUT.mask.xy = v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw`，等同于`(v.vertex.xy - clampedRect.xy) + (v.vertex.xy - clampedRect.zw)`
- `OUT.mask.zw`与`_UIMaskSoftnessX`和`_UIMaskSoftnessY`（在属性中限制为大于0）成反比
    `OUT.mask.zw = half2(0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));`

以x为例，点在矩形

- 左右边界上，`abs(OUT.mask.x) = 矩形宽度`
- 内部，`abs(OUT.mask.x) < 矩形宽度`
- **外部，`abs(OUT.mask.x) > 矩形宽度`**

y也是同理

**在矩形外部则`_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy) < 0`**，用saturate截取到0~1并应用到alpha上，最后实现了矩形裁剪

### Softness

同上，如果在矩形内部，越靠近矩形中心，则`(_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy))`越接近1。函数为`f(x)=x`

但图片sprite都大于等于1像素，对于一个高度为100的矩形，即在距离边缘1像素的范围内x从0变化到1，y也已经从0变化到1了。之后大于1部分都会被截取到1，因此只有（在编辑器下的Scene视图）放大许多倍才能看清楚

未放大，边缘1像素看不清楚

![image-20221014161501869](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20221014161501869.png)

放大后才能看清楚

![image-20221014161347038](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20221014161347038.png)

要做线性映射，则要分别除以`_UIMaskSoftnessX、_UIMaskSoftnessY`的两倍，即函数变为`f(x)=x/100`。两倍的原因是，比如`_UIMaskSoftnessY`为50，则对于一个高度为100的矩形在中心部分（距离边缘50像素）的位置上函数结果刚好为1

#### 线性映射：

``` glsl
#ifdef UNITY_UI_CLIP_RECT
half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) / half2(_UIMaskSoftnessX * 2, _UIMaskSoftnessY * 2));
color.a *= m.x * m.y;
#endif
```

计算结果是线性的，但人眼感知是非线性的会察觉到明显的条带边缘，因此边缘处不能只是用线性lerp，可以简单地替换为smoothstep来修复

``` glsl
#ifdef UNITY_UI_CLIP_RECT
half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) / half2(_UIMaskSoftnessX * 2, _UIMaskSoftnessY * 2));
m = smoothstep(0, 1, m);
color.a *= m.x * m.y;
#endif
```

#### 原本的方法

```glsl
#ifdef UNITY_UI_CLIP_RECT
half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
color.a *= m.x * m.y;
#endif
```

参考：[【UGUI源码分析】Unity遮罩之RectMask2D详细解读 - iwiniwin - 博客园 (cnblogs.com)](https://www.cnblogs.com/iwiniwin/p/15170384.html)

## SoftMaskForUGUI

原理：采样mask图，将其提供的alpha值传给所有子物体UI作为实际的alpha使用，这些UI的材质使用的shader全部替换为了支持SoftMask的特定shader

缺点：如果子UI重叠，重叠的部分对应的纹理像素是不透明的，但最后输出的片元的alpha值使用了mask的alpha，造成最后渲染的UI不会完全遮挡先前的UI，而是进行了透明度混合

![【Unity UI】uGUIできれいなソフトマスクを作る方法_29](https://i.gyazo.com/492f29500110b3d49cfe3f0e3234d449.jpg#750__231)

和默认UI不同的地方：

- `Blend One OneMinusSrcAlpha`改成为`Blend SrcAlpha OneMinusSrcAlpha`
- 片元着色器中`color.rgb *= color.a` 改成了 `color.a *= SoftMask()`

``` glsl
Shader "Hidden/UI/Default (SoftMaskable)"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

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

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        // 默认UI：Blend One OneMinusSrcAlpha，因为默认UI在片元着色器里直接用color.rgb *= color.a应用了alpha
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP


            #include "Packages/com.coffee.softmask-for-ugui/Shaders/SoftMask.cginc"	// Add for soft mask
            #pragma shader_feature __ SOFTMASK_EDITOR	// Add for soft mask

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

                #ifdef UNITY_UI_ALPHACLIP
                    clip (color.a - 0.001);
                #endif

                // 实际应用了mask的alpha的地方
                // 默认UI：color.rgb *= color.a
                color.a *= SoftMask(IN.vertex, IN.worldPosition);	// Add for soft mask

                return color;
            }
            ENDCG
        }
    }
}

```

参考：

- [mob-sakai/SoftMaskForUGUI: UI Soft Mask is a smooth masking component for Unity UI (uGUI) elements. (github.com)](https://github.com/mob-sakai/SoftMaskForUGUI)
- [【Unity UI】uGUIできれいなソフトマスクを作る方法 - 渋谷ほととぎす通信 (shibuya24.info)](https://shibuya24.info/entry/unity-ui-softmask)
- [Unity遮罩之Mask、RectMask2D与Sprite Mask适用场景分析 - iwiniwin - 博客园 (cnblogs.com)](https://www.cnblogs.com/iwiniwin/p/15191362.html)

