Shader "Hidden/SpeedEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex, _Camera1;
			float _ReduceVis;
			float _SlowMot;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.uv;
				fixed4 color = tex2D(_MainTex, uv);
				float val = 1 - _ReduceVis * 0.1;
				float val2 = 1 - val;

				color.rgb *= val+val2*sin(uv.x*3.14159);
				color.rgb *= val+val2*sin(uv.y*3.14159);

				fixed4 colorMot  = color;
				colorMot.r /= _SlowMot;
				colorMot.g *= _SlowMot;
				colorMot.b *= _SlowMot;

				return lerp(color, colorMot, 0.1);
			}
			ENDCG
		}
	}
}
