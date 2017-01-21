Shader "Custom/FakeCaustics" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (0.5, 0.5, 0.5, 1)
		_CTexture("Caustic (RGB)", 2D) = "white" {}
		_CausticColor("Caustic Color", Color) = (0.5, 0.5, 0.5, 1)
		_PanSpeed("PanSpeed", Range (0,10))=1
		_DiffuseAmount ("Diffuse Amount", Range (0, 1)) = 0.5
		_ShadowColor ("Shadow Color", Color) = (0.5, 0.5, 0.5, 1)
		_LightColor("Light Color", Color) = (0.5, 0.5, 0.5, 1)
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
			float2 uv_MainTex : TEXCOORD0;
			float2 uv_CTexture : TEXCOORD1;
			float3 color: Color;
		};

		sampler2D _MainTex;
		sampler2D _CTexture;
		half4 _Color;
		half3 _CausticColor;
		float _PanSpeed;
		
		void surf (Input IN, inout SurfaceOutput o) {
			float2 pan = IN.uv_CTexture + _Time.r * _PanSpeed;
			//pan.x = pan.x + _Time.r;
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex)*_Color + tex2D(_CTexture, pan)*IN.color.rgb*_CausticColor;
			o.Alpha = 1;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
