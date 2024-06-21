Shader "FORGE3D/Planets/Ring" 
{
    Properties
    {
        _DetailMap("Detail Map", 2D) = "black" {}
        _DiffuseColor("Ring Color", Color) = (0, 0, 0 ,0)       
        _RingFactors("Ring Size | Shadow Penumbra | Ring Offset | Unused", Vector) = (0, 0, 0 ,0)             
    }

    SubShader
    { 
        Tags
        {
            "IgnoreProjector"="True"
            "Queue"="Transparent-1"
            "RenderType"="Transparent" 
          //  "LightMode"="ForwardBase"
        }
        LOD 200
        Fog {Density 0.0025}
        Blend SrcAlpha OneMinusSrcAlpha       
        Cull Off
        ZWrite Off 
           
        Pass  
        {
            CGPROGRAM
            
            //#pragma only_renderers opengl d3d9 d3d11
            #pragma glsl
            #pragma target 3.0
            #pragma vertex RingVert
            #pragma fragment RingFrag
            
            #include "UnityCG.cginc"
            #include "AutoLight.cginc" 
            #include "PlanetData.cginc"
            #include "PlanetLighting.cginc"
            #include "PlanetVS.cginc"
            #include "PlanetPS.cginc"
            
            
            uniform float4 _RingFactors;
            uniform float4 _DiffuseColor;

            struct RingOutput
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;                
                float4 posWorld : TEXCOORD1;
                float4 posObj : TEXCOORD2;
                float3 normalDir : TEXCOORD3;      
                float4 posSun : TEXCOORD4;                    
            };
            
            RingOutput RingVert (PlanetInput v)
            {
                RingOutput o;
                o.uv.xy = v.texcoord0;
                float3 scale = 1 / float3( length(_World2Object[0].xyz), length(_World2Object[1].xyz), length(_World2Object[2].xyz) );

                o.normalDir = normalize(mul(float4(v.normal, 0), _World2Object).xyz);
                o.posSun = mul(_Object2World, v.vertex);
                o.posObj = mul(v.vertex, _World2Object) * scale.x;
                o.posWorld =  mul(_World2Object, v.vertex );        
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);                
                return o;
            }
            
            float4 RingFrag(RingOutput i) : COLOR
            {     
             //   float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

                float3 lightDir = normalize(_SunPos.xyz - i.posSun.xyz);
                float3 ambientColor  = UNITY_LIGHTMODEL_AMBIENT.rgb;

                float d = length( i.posObj.xyz );
              clip( 1 - d / _RingFactors.x );
                clip( i.posWorld.w );
              clip( d / ( 1 + _RingFactors.z ) - 1 );

                float inRadius = _RingFactors.z + 1;
                float2 pUV = float2(( d - inRadius) /  (_RingFactors.x - inRadius), 0.5);


                float4 detailColor = tex2D(_DetailMap, pUV);
              
                float3 finalColor = _DiffuseColor.rgb * detailColor.x * detailColor.a;
                float light = saturate(abs(dot(i.normalDir, lightDir)) + 0.1);
           

                float dist = length( cross(lightDir, i.posObj) );
                float shadow = smoothstep( 1 - _RingFactors.y, 1 + _RingFactors.y, dist);
                shadow += saturate( dot(i.posObj, lightDir));
        
                   shadow = saturate(shadow);

               finalColor = finalColor * light * shadow + (1 - shadow + 0.5) * ambientColor * detailColor.a;

             
                return float4(finalColor, saturate(detailColor.x * 2));
              
                //return float4(lightDir, 1);
            }
            ENDCG
        }
    } 

} 