Shader "PostEffectMirrorReflection" {
    Properties {
      _MirrorTexture ("Main Texture", 2D) = "white" {}
    }

    SubShader {
		Tags { "RenderType"="Opaque" }
		ColorMask RGBA
		//ZTest Always
		Lighting off
		ZWrite on
		Blend One One
			
		CGPROGRAM
		#pragma surface surf NoLighting
		struct Input {
			float4 screenPos;
		};
		sampler2D _MirrorTexture;
		
		void surf (Input IN, inout SurfaceOutput o) {
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			o.Albedo = tex2D (_MirrorTexture, screenUV).rgb;
			o.Alpha = 1;
		}
	  
		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;// saturate (s.Albedo * 0.2);
			//c.rgb *= c.rgb;
			c.a = 1;
			return c;
		}
		ENDCG
    } 
    Fallback "Diffuse"
}