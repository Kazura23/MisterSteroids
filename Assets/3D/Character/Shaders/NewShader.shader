// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33223,y:32331,varname:node_9361,prsc:2|normal-9081-RGB,emission-3039-OUT,olwid-5583-OUT;n:type:ShaderForge.SFN_Tex2d,id:9081,x:32657,y:31901,ptovrint:False,ptlb:Normal_map,ptin:_Normal_map,varname:node_9081,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_LightAttenuation,id:6602,x:31554,y:32147,varname:node_6602,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:717,x:31212,y:32426,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:_Diffuse,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:8993b617f08498f43adcbd90697f1c5d,ntxv:0,isnm:False|UVIN-8070-UVOUT;n:type:ShaderForge.SFN_Vector1,id:476,x:32691,y:33122,varname:node_476,prsc:2,v1:0.001;n:type:ShaderForge.SFN_TexCoord,id:8070,x:31007,y:32354,varname:node_8070,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:2989,x:31444,y:32537,varname:node_2989,prsc:2|A-717-RGB,B-5800-OUT;n:type:ShaderForge.SFN_Lerp,id:3318,x:31793,y:32500,varname:node_3318,prsc:2|A-7882-OUT,B-2989-OUT,T-7032-OUT;n:type:ShaderForge.SFN_Desaturate,id:7882,x:31432,y:32408,varname:node_7882,prsc:2|COL-717-RGB,DES-9961-OUT;n:type:ShaderForge.SFN_Slider,id:9961,x:31163,y:32265,ptovrint:False,ptlb:saturation,ptin:_saturation,varname:node_6363,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-10,cur:0.38,max:10;n:type:ShaderForge.SFN_Lerp,id:1866,x:32086,y:32501,varname:node_1866,prsc:2|A-3318-OUT,B-8085-RGB,T-8654-OUT;n:type:ShaderForge.SFN_Color,id:8085,x:31841,y:32690,ptovrint:False,ptlb:shadow_color,ptin:_shadow_color,varname:node_8166,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:7069,x:31718,y:32200,varname:node_7069,prsc:2|A-6602-OUT,B-1500-OUT;n:type:ShaderForge.SFN_OneMinus,id:9446,x:31879,y:32200,varname:node_9446,prsc:2|IN-7069-OUT;n:type:ShaderForge.SFN_Posterize,id:9272,x:32176,y:32225,varname:node_9272,prsc:2|IN-9446-OUT,STPS-3049-OUT;n:type:ShaderForge.SFN_Multiply,id:8654,x:32176,y:32368,varname:node_8654,prsc:2|A-9272-OUT,B-9881-OUT;n:type:ShaderForge.SFN_Lerp,id:7622,x:32514,y:32598,varname:node_7622,prsc:2|A-10-OUT,B-1866-OUT,T-8654-OUT;n:type:ShaderForge.SFN_Vector1,id:5800,x:31104,y:32580,varname:node_5800,prsc:2,v1:1;n:type:ShaderForge.SFN_LightColor,id:2911,x:32044,y:32613,varname:node_2911,prsc:2;n:type:ShaderForge.SFN_Multiply,id:10,x:32281,y:32613,varname:node_10,prsc:2|A-1866-OUT,B-2911-RGB;n:type:ShaderForge.SFN_ObjectPosition,id:8066,x:32515,y:33201,varname:node_8066,prsc:2;n:type:ShaderForge.SFN_ViewPosition,id:3685,x:32515,y:33330,varname:node_3685,prsc:2;n:type:ShaderForge.SFN_Distance,id:8580,x:32706,y:33220,varname:node_8580,prsc:2|A-3685-XYZ,B-8066-XYZ;n:type:ShaderForge.SFN_Vector1,id:2463,x:32706,y:33383,varname:node_2463,prsc:2,v1:0.002;n:type:ShaderForge.SFN_Multiply,id:151,x:32855,y:33220,varname:node_151,prsc:2|A-8580-OUT,B-2463-OUT;n:type:ShaderForge.SFN_Add,id:5583,x:33002,y:33163,varname:node_5583,prsc:2|A-476-OUT,B-151-OUT;n:type:ShaderForge.SFN_Vector1,id:1500,x:31554,y:32280,varname:node_1500,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:3049,x:31995,y:32259,varname:node_3049,prsc:2,v1:1.08;n:type:ShaderForge.SFN_Vector1,id:9881,x:31895,y:32346,varname:node_9881,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Lerp,id:3039,x:32837,y:32725,varname:node_3039,prsc:2|A-7622-OUT,B-9672-OUT,T-1921-OUT;n:type:ShaderForge.SFN_Multiply,id:9672,x:32514,y:32733,varname:node_9672,prsc:2|A-7622-OUT,B-5949-OUT;n:type:ShaderForge.SFN_Slider,id:5949,x:32124,y:32766,ptovrint:False,ptlb:gloss_color,ptin:_gloss_color,varname:node_1029,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:2;n:type:ShaderForge.SFN_LightVector,id:4872,x:31627,y:32977,varname:node_4872,prsc:2;n:type:ShaderForge.SFN_HalfVector,id:9812,x:31634,y:33292,varname:node_9812,prsc:2;n:type:ShaderForge.SFN_Dot,id:5448,x:31878,y:33185,varname:node_5448,prsc:2,dt:1|A-2900-OUT,B-9812-OUT;n:type:ShaderForge.SFN_Power,id:4925,x:32090,y:33185,varname:node_4925,prsc:2|VAL-5448-OUT,EXP-3825-OUT;n:type:ShaderForge.SFN_Exp,id:3825,x:31878,y:33357,varname:node_3825,prsc:2,et:1|IN-6645-OUT;n:type:ShaderForge.SFN_ConstantLerp,id:6645,x:31634,y:33455,varname:node_6645,prsc:2,a:1,b:11|IN-1989-OUT;n:type:ShaderForge.SFN_Slider,id:1989,x:31277,y:33462,ptovrint:False,ptlb:gloss,ptin:_gloss,varname:node_9186,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_NormalVector,id:2900,x:31588,y:33148,prsc:2,pt:True;n:type:ShaderForge.SFN_Dot,id:3891,x:31878,y:33028,varname:node_3891,prsc:2,dt:1|A-4872-OUT,B-2900-OUT;n:type:ShaderForge.SFN_Multiply,id:1921,x:32233,y:33105,varname:node_1921,prsc:2|A-3891-OUT,B-4925-OUT;n:type:ShaderForge.SFN_Vector1,id:1769,x:32749,y:32892,varname:node_1769,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:7032,x:31258,y:32742,varname:node_7032,prsc:2,v1:2;proporder:9081-717-9961-8085-5949-1989;pass:END;sub:END;*/

Shader "Cunstom/New/Character_Toon_shader" {
    Properties {
        _Normal_map ("Normal_map", 2D) = "bump" {}
        _Diffuse ("Diffuse", 2D) = "white" {}
        _saturation ("saturation", Range(-10, 10)) = 0.38
        _shadow_color ("shadow_color", Color) = (0.5,0.5,0.5,1)
        _gloss_color ("gloss_color", Range(1, 2)) = 1
        _gloss ("gloss", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d11 glcore gles 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(0)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*(0.001+(distance(_WorldSpaceCameraPos,objPos.rgb)*0.002)),1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                return fixed4(float3(0,0,0),0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Normal_map; uniform float4 _Normal_map_ST;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _saturation;
            uniform float4 _shadow_color;
            uniform float _gloss_color;
            uniform float _gloss;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_map_var = UnpackNormal(tex2D(_Normal_map,TRANSFORM_TEX(i.uv0, _Normal_map)));
                float3 normalLocal = _Normal_map_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float node_3049 = 1.08;
                float node_8654 = (floor((1.0 - (attenuation*1.0)) * node_3049) / (node_3049 - 1)*0.1);
                float3 node_1866 = lerp(lerp(lerp(_Diffuse_var.rgb,dot(_Diffuse_var.rgb,float3(0.3,0.59,0.11)),_saturation),(_Diffuse_var.rgb*1.0),2.0),_shadow_color.rgb,node_8654);
                float3 node_7622 = lerp((node_1866*_LightColor0.rgb),node_1866,node_8654);
                float3 emissive = lerp(node_7622,(node_7622*_gloss_color),(max(0,dot(lightDirection,normalDirection))*pow(max(0,dot(normalDirection,halfDirection)),exp2(lerp(1,11,_gloss)))));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Normal_map; uniform float4 _Normal_map_ST;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _saturation;
            uniform float4 _shadow_color;
            uniform float _gloss_color;
            uniform float _gloss;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_map_var = UnpackNormal(tex2D(_Normal_map,TRANSFORM_TEX(i.uv0, _Normal_map)));
                float3 normalLocal = _Normal_map_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 finalColor = 0;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
