// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Test1"
{
	Properties
	{
		_Color ("Color", color) = (1,1,1,1)
		_ColorSpeed ("ColorSpeed", float) = 1
		_ColorStrength ("ColorStrength", Range(1,10)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			 #pragma vertex vert 
       		  #pragma fragment frag 

	    struct vertexInput 
	    {
            float4 pos : POSITION;
            float4 col : TEXCOORD0;
         };

         struct vertexOutput 
         {
            float4 pos : SV_POSITION;
            float4 col : TEXCOORD0;
         };

        fixed4 _Color;
        fixed _ColorSpeed;
 		fixed _ColorStrength;

         vertexOutput vert(vertexInput i) 
         {
           vertexOutput output; // we don't need to type 'struct' here
 
           output.pos = UnityObjectToClipPos(i.pos);
           output.col = _Color * _ColorStrength ;

           return output;
         }
 		 
         float4 frag(vertexOutput input) : COLOR // fragment shader
         {
            return input.col * sin(_Time.y*_ColorSpeed) * 0.5 + 0.5; 
         }
			ENDCG
		}
	}
}
