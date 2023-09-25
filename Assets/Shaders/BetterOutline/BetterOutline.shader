Shader "Custom/BetterOutline"
{
    Properties
    {
        _OutlineWidth ("Outline Width", Range(0.01, 2)) = 0.24
        _OutlineColor ("Outline Color", Color) = (0.5, 0.5, 0.5, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipline" = "UniversalPipeline" }

        Pass
        {
            Tags { "LightMode" = "UniversalForward"}

            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            half _OutlineWidth;
            half4 _OutlineColor;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 vertColor : COLOR;
                float4 tangent : TANGENT;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float4 pos = TransformObjectToHClip(IN.positionOS.xyz);
                float3 viewNormal = mul((float3x3)UNITY_MATRIX_IT_MV, IN.tangent.xyz);
                float3 ndcNormal = normalize(mul((float3x3)UNITY_MATRIX_VP, viewNormal.xyz)) * pos.w;
                float4 nearUpperRight = mul(UNITY_MATRIX_IT_MV, float4(1, 1, UNITY_NEAR_CLIP_VALUE, _ProjectionParams.y));
                float aspect = abs(nearUpperRight.y / nearUpperRight.x);
                ndcNormal.x *= aspect;
                pos.xy += 0.01 * _OutlineWidth * ndcNormal.xy;
                OUT.positionHCS = pos;
                return OUT;
            }

            half4 frag() : SV_TARGET0
            {
                return _OutlineColor;
            }

            ENDHLSL
        }
    }
}
