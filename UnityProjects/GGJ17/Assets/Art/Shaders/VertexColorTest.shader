Shader "Custom/VertexColorTest" {
	Properties {
		_Texture ("Base (RGB)", 2D) = "white" {}
		_DiffuseAmount ("Diffuse Amount", Range (0, 1)) = 0.5
		_ShadowColor ("Shadow Color", Color) = (0.5, 0.5, 0.5, 1)
		_LightColor ("Light Color", Color) = (0.5, 0.5, 0.5, 1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf DiffuseScale
		
		half _DiffuseAmount;
		half3 _ShadowColor;
		half3 _LightColor;
		#pragma lighting DiffuseScale exclude_path:prepass 
		inline fixed4 LightingDiffuseScale (SurfaceOutput s, half3 lightDir, fixed3 viewDir, half atten)
		{ 
		
			half3 normal = normalize(s.Normal);

			fixed diff = max (1-_DiffuseAmount, dot (normal, lightDir));
			
			//fixed nh =  saturate (dot (normal, normalize(lightDir + viewDir)));
			//fixed spec = pow (nh, s.Specular*128) * s.Gloss;
			
			fixed4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff * (_LightColor.rgb * diff) + _ShadowColor.rgb*(1-diff)) * atten;
		
			c.a = 1;//s.Alpha + _LightColor0.a * _SpecColor.a * spec * atten;
			return c;
		}
		
		struct Input {
			float2 uv_Texture : TEXCOORD0;
			float3 color: Color;
		};

		sampler2D _Texture;
		
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_Texture, IN.uv_Texture) * IN.color.rgb;
			o.Alpha = 1;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
