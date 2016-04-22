Shader "MirrorShader" {
	Properties{
		_LeftEyeTexture("Left Eye Texture", 2D) = "white" {}
		_RightEyeTexture("Left Eye Texture", 2D) = "white" {}
		_Radius("Radius", Float) = 64
	}

	SubShader{
		Tags{ "RenderType" = "Opaque" }
		//ColorMask RGBA
		Lighting off
		//ZWrite on

		CGPROGRAM

		#include "UnityShaderVariables.cginc"

		#pragma surface surf StandardSpecular

		struct Input {
			float2 uv_LeftEyeTexture;
			float2 uv_RightEyeTexture;
			float4 screenPos;
			float3 worldPos;
		};

		sampler2D _LeftEyeTexture;
		sampler2D _RightEyeTexture;
		fixed3 _Origin;
		half _Radius;

		void surf(Input IN, inout SurfaceOutputStandardSpecular o) {
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			if (unity_CameraProjection[0][2] < 0) {
				o.Emission = tex2D(_LeftEyeTexture, screenUV).rgb;
			} else {
				o.Emission = tex2D(_RightEyeTexture, screenUV).rgb;
			}
		}

		ENDCG
	}

	Fallback "Diffuse"
}