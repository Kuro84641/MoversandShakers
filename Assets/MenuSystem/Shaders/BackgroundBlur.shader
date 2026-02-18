Shader "Custom/BackgroundBlur"
{
    Properties
    {
        _Radius ("Blur Radius (pixels)", Range(0, 32)) = 8
        _Dimming ("Dimming", Range(0, 1)) = 1
        _Tint ("Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" "RenderType"="Transparent" }

        Pass
        {
            Name "Blur"
            Tags { "LightMode"="UniversalForward" }

            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 screenPos   : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float _Radius;
                float _Dimming;
                half4 _Tint;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.screenPos.xy / IN.screenPos.w;

                float2 texel = _CameraOpaqueTexture_TexelSize.xy;
                float2 o1 = texel * max(_Radius, 0.0);
                float2 o2 = o1 * 0.5;

                float3 sum = 0;
                float w = 0;

                sum += SampleSceneColor(uv) * 0.20;  w += 0.20;

                sum += SampleSceneColor(uv + float2( o1.x, 0)) * 0.10; w += 0.10;
                sum += SampleSceneColor(uv + float2(-o1.x, 0)) * 0.10; w += 0.10;
                sum += SampleSceneColor(uv + float2(0,  o1.y)) * 0.10; w += 0.10;
                sum += SampleSceneColor(uv + float2(0, -o1.y)) * 0.10; w += 0.10;

                sum += SampleSceneColor(uv + float2( o1.x,  o1.y)) * 0.07; w += 0.07;
                sum += SampleSceneColor(uv + float2( o1.x, -o1.y)) * 0.07; w += 0.07;
                sum += SampleSceneColor(uv + float2(-o1.x,  o1.y)) * 0.07; w += 0.07;
                sum += SampleSceneColor(uv + float2(-o1.x, -o1.y)) * 0.07; w += 0.07;

                sum += SampleSceneColor(uv + float2( o2.x, 0)) * 0.04; w += 0.04;
                sum += SampleSceneColor(uv + float2(-o2.x, 0)) * 0.04; w += 0.04;
                sum += SampleSceneColor(uv + float2(0,  o2.y)) * 0.04; w += 0.04;
                sum += SampleSceneColor(uv + float2(0, -o2.y)) * 0.04; w += 0.04;

                float3 rgb = sum / max(w, 1e-5);
                rgb *= _Dimming;

                half4 outCol = half4((half3)rgb, _Tint.a);
                outCol.rgb *= _Tint.rgb;
                return outCol;
            }
            ENDHLSL
        }
    }
}