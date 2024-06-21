// Upgrade NOTE: replaced '_Object2World' with '_Object2World'
// Upgrade NOTE: replaced '_World2Object' with '_World2Object'

Shader "FORGE3D/Planets/SphereMap" {
    Properties {
        _Nebula ("Nebula", Cube) = "_Skybox" {}
        _Starmap ("Starmap", Cube) = "_Skybox" {}
        _StarsPower ("StarsPower", Float ) = 0
        _StarsTint ("StarsTint", Color) = (0.5,0.5,0.5,1)
        _NebulaTint ("NebulaTint", Color) = (0.5,0.5,0.5,1)
        _NebulaSaturation ("NebulaSaturation", Range(0, 1)) = 0
    }
    SubShader {
    	Blend One One
        Tags {
            "Queue"="Background"
            "RenderType"="Background"
           }
        LOD 200
        Pass {
         
            Cull Front
            ZWrite Off
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
          
            #include "UnityCG.cginc"
         
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform samplerCUBE _Nebula;
            uniform samplerCUBE _Starmap;
            uniform float _StarsPower;
            uniform float4 _StarsTint;
            uniform float4 _NebulaTint;
            uniform float _NebulaSaturation;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.normalDir = mul(float4(-v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);

                float3 normalDirection =  i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );

                float3 finalColor = ((_NebulaTint.rgb*lerp(texCUBE(_Nebula,viewReflectDirection).rgb,dot(texCUBE(_Nebula,viewReflectDirection).rgb,float3(0.3,0.59,0.11)),(1.0 - _NebulaSaturation)))+(pow(texCUBE(_Starmap,viewReflectDirection).rgb,_StarsPower)*_StarsTint.rgb));

                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "None"
    
}
