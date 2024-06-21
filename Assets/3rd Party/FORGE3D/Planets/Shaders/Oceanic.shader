Shader "FORGE3D/Planets/Oceanic" 
{
	Properties
	{
		_GradientMap("Pole Gradient", 2D) = "black" {}
		_SurfaceNormalMap("Surface Normal", 2D) = "black" {}				
		_HeightMap ("Height Map", 2D) = "black" {}
		_CloudsMap ("Clouds Map", 2D) = "black" {}
        _CloudsCapMap ("Clouds Cap Map", 2D) = "black" {}
        _SunsetMap ("Sunset Map", 2D) = "black" {}
        
		_DeepWaterColor ("Deep Water Color", Color) = (0.5,0.5,0.5,1)
        _ShallowWaterColor ("Shallow Water Color", Color) = (0.5,0.5,0.5,1)  
        _WaterSpecularColor("Water Specular Color", Color) = (0.5,0.5,0.5,1)      
        _CloudsColor("Clouds Color", Color) = (0.5,0.5,0.5,1)  

		_NormalHeight("Normal Height", Float) = 0
		_HeightTile("Height Tile", Float) = 0
		_DiffusePow("Diffuse Power", Float) = 0
        _ColorPassTrough("Global Multiplier", Float) = 0

        _WaterDetailPow ("Water Detail Power", Float ) = 0
        _WaterDetailMult ("Water Detail Multiplier", Float ) = 0
       	_WaterFresnelPow ("Water Fresnel Power", Float ) = 0
        _WaterFresnelMult ("Water Fresnel Multiplier", Float ) = 0
        _WaterSpecularPow ("Water Specular Power", Float ) = 0
        _WaterSpecularMult ("Water Specular Multiplier", Float ) = 0
        
      	_CloudsShadows ("Clouds Shadows", Float ) = 0      	
      	_CloudsBrightness ("Clouds Brightness", Float ) = 0       
        _CloudsAnimation ("Clouds Animation", Float ) = 0
        _CloudsSunset ("Sunset Factor", Float ) = 0
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
           
            float4 EarthLikeFrag(PlanetOutput i) : COLOR
            {                
            	PlanetData p = GetPlanetData(i); 
                p.OceanMap.x = 1;
                p.OceanMap.y = 1;
            	float3 baseColor = 0;				 
            	baseColor = AddWater(baseColor, p); 
				baseColor = saturate(baseColor * baseColor * _ColorPassTrough);
				baseColor = AddDiffuse(baseColor, p);
				baseColor = AddClouds(baseColor, p);
                baseColor += (1 - p.DiffuseFactor) * p.Ambient;

            	return float4(baseColor, 1);
            }

			ENDCG
		}
	} 

	FallBack Off
}
