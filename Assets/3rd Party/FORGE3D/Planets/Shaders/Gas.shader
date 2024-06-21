Shader "FORGE3D/Planets/Gas" 
{
	Properties
	{
	    _GradientMap("Pole Gradient", 2D) = "black" {}
		_HeightMap("Height Map", 2D) = "black" {}
        _DistMap("Distortion Map", 2D) = "black" {}        
        _PoleColor ("Pole Color", Color) = (0.5,0.5,0.5,1)
        _SpecularColor("Specular Color", Color) = (0.5,0.5,0.5,1)
        _FresnelColor("Fresnel Color", Color) = (0.5,0.5,0.5,1)
        _SpecularPow ("Specular Power", Float ) = 0
        _SpecularMult ("Specular Multiplier", Float ) = 0        
        _FresnelPow ("Fresnel Power", Float ) = 0
        _FresnelMult ("Fresnel Multiplier", Float ) = 0
        _DistFact("Dist Speed | Dist Factor | Height Speed | Unused", Vector) = (0, 0, 0, 0)
	}

	SubShader
	{ 
		Tags
        {
            "IgnoreProjector"="True"
             "Queue"="Geometry"
            "RenderType"="Opaque" 
            "LightMode"="ForwardBase"
        }
		LOD 200
		Fog {Density 0.0025} 
		 
		Pass  
		{
			CGPROGRAM
           
		  	//#pragma only_renderers opengl d3d9 d3d11
            #pragma glsl
            #pragma target 3.0
			#pragma vertex EarthLikeVert
            #pragma fragment EarthLikeFrag
            
          	#include "UnityCG.cginc"
            #include "AutoLight.cginc" 
            #include "PlanetData.cginc"
            #include "PlanetLighting.cginc"
            #include "PlanetVS.cginc"
			#include "PlanetPS.cginc"
            
			uniform float4 _HeightMap_ST; 

            uniform sampler2D _DistMap;
            uniform float4 _PoleColor;
            uniform float4 _LowColor;
            uniform float4 _HighColor;

            uniform float4 _DistFact;

            uniform float4 _SpecularColor;
            uniform float _SpecularPow;
            uniform float _SpecularMult;

            uniform float4 _FresnelColor;
            uniform float _FresnelPow;
            uniform float _FresnelMult;

            float4 EarthLikeFrag(PlanetOutput i) : COLOR
            {                
            	PlanetData p = GetPlanetData(i); 
                p.HeightMap.x = 0;
                p.HeightMap.y = 0;

                float2 uv = i.uv.xy;

                float distA = tex2D(_DistMap, uv - float2(_Time.x * _DistFact.x, 0));
                float distB = tex2D(_DistMap, uv + float2(_Time.x * _DistFact.x, 0));                
               
                float dist = saturate(distA * distB) * _DistFact.y;

                float3 heightMap = tex2D(_HeightMap, _HeightMap_ST.xy * (i.uv.xy * float2(2, 1) + dist  + float2(_Time.x * _DistFact.z, 0)));           

                float3 halfDir = normalize(p.LightDir + p.ViewDir);
                float specDot = saturate(dot(p.NormalDir, halfDir));
                float3 spec = 1 * 
                _SpecularMult * pow(specDot, _SpecularPow) * _SpecularColor; 

                float3 fresnel = 1 * p.DiffuseFactor * _FresnelColor * saturate(pow(saturate(1 - dot(p.ViewDir, p.NormalMap)), _FresnelPow) * _FresnelMult);

                float3 color = lerp(heightMap, _PoleColor, pow(1 - p.GradientMap, 2)) * pow(p.DiffuseFactor, 1);
                color += spec * p.DiffuseFactor; 
                color += fresnel;
                color += (1 - p.DiffuseFactor) * p.Ambient;

            	return float4(color,1);
            }

			ENDCG
		}
	}

	FallBack Off
}
