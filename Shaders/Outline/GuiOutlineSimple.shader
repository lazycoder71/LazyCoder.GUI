Shader "LazyCoder/UI/OutlineSimple"
{
    Properties
    {
        [HideInInspector] _StencilComp("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
        [HideInInspector]_MainTex("MainTex", 2D) = "white" {}
        _InsideColor("InsideColor", Color) = (0,0,0,0)
        _OutsideColor("OutsideColor", Color) = (0,0,0,0)
        _Falloff("Falloff", Range( 0 , 10)) = 1
        _InnerGlow("InnerGlow", Range( 0 , 1)) = 1
        _AlphaBoost("AlphaBoost", Range( 0 , 5)) = 1
        _SoftenEdgeWidth("SoftenEdgeWidth", Range( 0 , 1)) = 0
        _Speed("Speed", Vector) = (0,0,0,0)
        _Tiling("Tiling", Vector) = (0,0,0,0)

    }

    SubShader
    {
        LOD 0

        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
            CompFront [_StencilComp]
            PassFront [_StencilOp]
            FailFront Keep
            ZFailFront Keep
            CompBack Always
            PassBack Keep
            FailBack Keep
            ZFailBack Keep
        }


        Cull Off
        Lighting Off
        ZWrite Off
        ZTest[unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask[_ColorMask]


        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            #include "UnityShaderVariables.cginc"
            #define ASE_NEEDS_FRAG_COLOR


            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                half2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            uniform float4 _ClipRect;
            uniform float4 _OutsideColor;
            uniform float4 _InsideColor;
            uniform sampler2D _MainTex;
            uniform float2 _Tiling;
            uniform float2 _Speed;
            uniform float _Falloff;
            uniform float _InnerGlow;
            uniform float _AlphaBoost;
            uniform float _SoftenEdgeWidth;


            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = IN.vertex;


                OUT.worldPosition.xyz += float3(0, 0, 0);
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = IN.texcoord;

                OUT.color = IN.color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float mulTime33 = _Time.y * _Speed.x;
                float mulTime49 = _Time.y * _Speed.y;
                float2 appendResult31 = (float2(mulTime33, mulTime49));
                float2 texCoord16 = IN.texcoord.xy * _Tiling + appendResult31;
                float2 texCoord35 = IN.texcoord.xy * float2(1, 1) + float2(0, 0);
                float temp_output_63_0 = saturate(pow((1.0 - texCoord35.y), _Falloff));
                float lerpResult70 = lerp((tex2D(_MainTex, texCoord16).r * IN.color.a * temp_output_63_0),
                     (IN.color.a * temp_output_63_0), (temp_output_63_0 * _InnerGlow));
                float lerpResult101 = lerp(
                    1.0, saturate((texCoord35.y * (200.0 + (_SoftenEdgeWidth - 0.0) * (0.5 - 200.0) / (1.0 - 0.0)))),
                    saturate((0.0 + (_SoftenEdgeWidth - 0.0) * (1.0 - 0.0) / (0.01 - 0.0))));
                float temp_output_67_0 = saturate((lerpResult70 * _AlphaBoost * lerpResult101));
                float3 lerpResult59 = lerp((_OutsideColor).rgb, (_InsideColor).rgb, temp_output_67_0);
                float4 appendResult23 = (float4((lerpResult59 * (IN.color).rgb), temp_output_67_0));

                half4 color = appendResult23;

                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

                #ifdef UNITY_UI_ALPHACLIP
                clip(color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
    CustomEditor "ASEMaterialInspector"


}