Shader "Custom/CercleMask" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("BumpMap", 2D) = "bump" {}
		_LessGray ("LessGray", Range (0,2)) = 1

		_EmissionColor ("EmissionColor", Color) = (1,1,1,1)
		_EmissionMainTex ("EmissionAlbedo (RGB)", 2D) = "white" {}
		
		_ColorStrength ("ColorStrength", Range(1,10)) = 1
		_EmissionColorStrength ("EmissionColorStrength", Range(1,10)) = 1
		
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		//_Position ("WorldPos", Vector) = (0,0,0,0)
		//_Radius ("SphereRadius", Range(0,100)) = 0
		//_SoftNess("SphereSoftness", Range(0,100)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex, _EmissionMainTex, _BumpMap;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_EmissionTex;
			float3 worldPos;
			float2 uv_BumpMap;
		};

		fixed4 _Color, _EmissionColor;
		half _Glossiness;
		half _Metallic;
		half _ColorStrength, _EmissionColorStrength;
		half _LessGray;

		//Spherical mask
		uniform float4 GlobaleMask_Position;
		uniform half GlobaleMask_Radius;
		uniform half GlobaleMask_SoftNess;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// default color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			//récupère seulement le gris de la texture
			half grayscale = (c.r+c.g+c.b)*.333; 
			half satG = saturate (grayscale * _LessGray);
			fixed3 c_g = fixed3(satG,satG,satG);

			//Emission
			fixed4 e = tex2D(_EmissionMainTex, IN.uv_EmissionTex) * _EmissionColor * _EmissionColorStrength;

			//Radius d'application de la couleur
			half d = distance (GlobaleMask_Position, IN.worldPos);
			half sum = saturate((d - GlobaleMask_Radius) / -GlobaleMask_SoftNess);
			fixed4 lerpColor = lerp(fixed4(c_g,1),c * _ColorStrength,sum);
			fixed4 lerpEmiision = lerp(fixed4(0,0,0,0),e,sum);

			o.Albedo = lerpColor.rgb;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
			
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Emission = lerpEmiision.rgb;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}
