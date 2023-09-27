Shader "Custom/TwoSideEmission"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white"{}
		_EmissionColor("Emission Color", Color) = (1,1,1,1)
		_EmissionStrength("Emission Strength", Range(0, 1)) = 1
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "RenderPipline" = "UniversalPipeline"}

		Pass
		{
			Cull Off

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);

			CBUFFER_START(UnityPerMaterial)
			float4 _MainTex_ST;
			half4 _EmissionColor;
			half _EmissionStrength;
			CBUFFER_END

			struct Attributes
			{
				float4 positionOS : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Varyings
			{
				float4 positionHCS : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			Varyings vert(Attributes IN)
			{
				Varyings OUT;
				OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
				OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
				return OUT;
			}

			half4 frag(Varyings IN) : SV_TARGET0
			{
				half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
				half4 emission = _EmissionColor * _EmissionStrength;
				col.rgb += emission.rgb;
				return col;
			}

			ENDHLSL
		}

		
	}
}