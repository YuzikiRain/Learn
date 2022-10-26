## 用于设置自定义的softness的monobehavior脚本

使用额外的脚本设置设置自定义softness参数，padding则仍需要用默认的RectMask2D

``` c#
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BorderlessFramework.Utility
{
    public class CustomRectMask2D : UIBehaviour
    {
        [Header("softness+padding值不要超过该物体的Rect中心到RectMask2D物体对应边界的距离")]
        [SerializeField] [Max(0)] private float softnessLeft;
        [SerializeField] [Max(0)] private float softnessRight;
        [SerializeField] [Max(0)] private float softnessTop;
        [SerializeField] [Max(0)] private float softnessBottom;

        private int softnessLeftPropertyID = Shader.PropertyToID("_SoftnessLeft");
        private int softnessRightPropertyID = Shader.PropertyToID("_SoftnessRight");
        private int softnessTopPropertyID = Shader.PropertyToID("_SoftnessTop");
        private int softnessBottomPropertyID = Shader.PropertyToID("_SoftnessBottom");

        private const string shaderName = "UI/RectMask2D Smoothstep FourBorderSoftness";

        /// <summary>
        /// 如果使用脚本修改了该脚本softness，需要调用该方法后才会生效
        /// </summary>
        public void Refresh(bool isOn)
        {
            var maskableGraphics = GetComponentsInChildren<MaskableGraphic>();
            foreach (var maskableGraphic in maskableGraphics)
            {
                if (maskableGraphic.maskable && maskableGraphic.material.shader.name == shaderName)
                {
                    maskableGraphic.material.SetFloat(softnessLeftPropertyID, isOn ? softnessLeft : 0f);
                    maskableGraphic.material.SetFloat(softnessRightPropertyID, isOn ? softnessRight : 0f);
                    maskableGraphic.material.SetFloat(softnessTopPropertyID, isOn ? softnessTop : 0f);
                    maskableGraphic.material.SetFloat(softnessBottomPropertyID, isOn ? softnessBottom : 0f);
                    maskableGraphic.SetMaterialDirty();
                }
            }
        }

        protected override void OnEnable()
        {
            Refresh(true);
        }

        protected override void OnDisable()
        {
            Refresh(false);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (!Application.isPlaying) Refresh(enabled);
        }

        protected override void Reset()
        {
            softnessLeft = softnessRight = softnessTop = softnessBottom = 0;
            Refresh(false);
        }
#endif

    }
}

```

## MaxAttribute相关脚本

MaxAttribute放在非Editor目录下

``` c#
using UnityEngine;

namespace BorderlessFramework.Utility
{
    public class MaxAttribute : PropertyAttribute
    {
        public float max;

        public MaxAttribute(float max)
        {
            this.max = max;
        }
    }
}
```

MaxPropertyDrawer放在Editor目录下

``` c#
using UnityEditor;
using UnityEngine;

namespace BorderlessFramework.Utility
{
    [CustomPropertyDrawer(typeof(MaxAttribute))]
    public class MaxPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MaxAttribute minAttribute = attribute as MaxAttribute;
            property.floatValue = Mathf.Max(property.floatValue, minAttribute.max);
            EditorGUI.PropertyField(position, property);
        }
    }
}
```

## 从UI-Default修改的shader

对于任何需要受影响的UI，需要设置material为使用了该shader的自定义material才会有效果

``` glsl
Shader "UI/RectMask2D Smoothstep FourBorderSoftness"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255

        _ColorMask("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0

        // 序列化这些属性在Material上，但不显示在Inspector上
        [HideInInspector]_SoftnessLeft("_SoftnessLeft", Float) = 0
        [HideInInspector]_SoftnessRight("_SoftnessRight", Float) = 0
        [HideInInspector]_SoftnessTop("_SoftnessTop", Float) = 0
        [HideInInspector]_SoftnessBottom("_SoftnessBottom", Float) = 0
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend One OneMinusSrcAlpha
            ColorMask[_ColorMask]

            Pass
            {
                Name "Default"
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0

                #include "UnityCG.cginc"
                #include "UnityUI.cginc"

                #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
                #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

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
                    fixed4 color : COLOR;
                    float2 texcoord  : TEXCOORD0;
                    float4 worldPosition : TEXCOORD1;
                    half4  mask : TEXCOORD2;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                sampler2D _MainTex;
                fixed4 _Color;
                fixed4 _TextureSampleAdd;
                float4 _ClipRect;
                float4 _MainTex_ST;
                float _UIMaskSoftnessX;
                float _UIMaskSoftnessY;
                float _SoftnessLeft;
                float _SoftnessRight;
                float _SoftnessTop;
                float _SoftnessBottom;

                v2f vert(appdata_t v)
                {
                    v2f OUT;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                    float4 vPosition = UnityObjectToClipPos(v.vertex);
                    OUT.worldPosition = v.vertex;
                    OUT.vertex = vPosition;

                    float2 pixelSize = vPosition.w;
                    pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                    float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                    float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
                    OUT.texcoord = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
                    OUT.mask = half4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));
                    OUT.color = v.color * _Color;
                    return OUT;
                }

                fixed4 frag(v2f IN) : SV_Target
                {
                    half4 color = IN.color * (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd);
#ifdef UNITY_UI_CLIP_RECT
                    half kx = step(IN.mask.x, 0);
                    half ky = step(IN.mask.y, 0);
                    half2 division = half2(
                        (kx * _SoftnessLeft + (1 - kx) * _SoftnessRight) * 2,
                        (ky * _SoftnessBottom + (1 - ky) * _SoftnessTop) * 2);
                    half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) / division);
                    // 让最后边缘部分不是线性变化，否则会出现明显的条带
                    m = smoothstep(0, 1, m);
                    color.a *= m.x * m.y;
#endif

                    #ifdef UNITY_UI_ALPHACLIP
                    clip(color.a - 0.001);
                    #endif

                    color.rgb *= color.a;

                    return color;
                }
            ENDCG
            }
        }
}

```

## 注意

计算softness时，只能进行缩放，使得计算得到的alpha由边缘向Rect矩形的中心线性增长
如果softness+padding超过该物体的Rect中心到RectMask2D物体对应边界的距离，则中心的alpha会小于1，但另一边的alpha全为1，会出现明显的边界（从小于1的值跳变到1）

![image-20221025172809900](https://fastly.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20221025172809900.png)

## 另一种方式

仍使用自带的RectMask2D，不想要softness的一侧设置padding为负数。缺点是设置了padding的那一侧虽然没有softness了，但是也会没有mask效果