Shader "FORGE3D/Planets/Terrestrial" 
{
	Properties
	{
		_GradientMap("Pole Gradient", 2D) = "black" {}
		_SurfaceNormalMap("Surface Normal", 2D) = "black" {}	
		_OceanMask("Ocean Mask", 2D) = "black" {}	
		_HeightMap ("Height Map", 2D) = "black" {}
		_CloudsMap ("Clouds Map", 2D) = "black" {}
        _CloudsCapMap ("Clouds Cap Map", 2D) = "black" {}
        _SunsetMap ("Sunset Map", 2D) = "black" {}        
        _CityLightUVMap ("City Light UV Map", 2D) = "black" {}
        _CityLightMaskMap ("City Light Mask Map", 2D) = "black" {}
        _CityLightMap ("City Light Map", 2D) = "black" {}
		
		_DeepWaterColor ("Deep Water Color", Color) = (0.5,0.5,0.5,1)
        _ShallowWaterColor ("Shallow Water Color", Color) = (0.5,0.5,0.5,1)
        _ShoreColor ("Shore Color", Color) = (0.5,0.5,0.5,1)
        _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
        _VegetationColor ("Vegetation Color", Color) = (0.5,0.5,0.5,1)
        _DesertColor ("Desert Color", Color) = (0.5,0.5,0.5,1)
        _MountainColor ("Mountains Color", Color) = (0.5,0.5,0.5,1)

        _WaterSpecularColor("Water Specular Color", Color) = (0.5,0.5,0.5,1)
        _LandSpecularColor("Land Specular Color", Color) = (0.5,0.5,0.5,1)

        _CloudsColor("Clouds Color", Color) = (0.5,0.5,0.5,1)       

        _CityLightColor("City Light Color", Color) = (0.5,0.5,0.5,1)

		_NormalHeight("Normal Height", Float) = 0
		_HeightTile("Height Tile", Float) = 0
		_DiffusePow("Diffuse Power", Float) = 0
        _ColorPassTrough("Global Multiplier", Float) = 0

	    _VegetationCoverage ("Vegetation Coverage", Float) = 0
        _VegetationFactors ("Vegetation Factors", Float) = 0
      	_DesertCoverage ("Desert Coverage", Float) = 0
        _DesertFactors ("Desert Factors", Float) = 0
        _MountainCoverage ("Mountains Coverage", Float) = 0
        _MountainFactors ("Mountains Factors", Float) = 0

     	_ShoreFactor ("Shore Factor", Float ) = 0
        _WaterDetailPow ("Water Detail Power", Float ) = 0
        _WaterDetailMult ("Water Detail Multiplier", Float ) = 0

       	_WaterFresnelPow ("Water Fresnel Power", Float ) = 0
        _WaterFresnelMult ("Water Fresnel Multiplier", Float ) = 0
        _WaterSpecularPow ("Water Specular Power", Float ) = 0
        _WaterSpecularMult ("Water Specular Multiplier", Float ) = 0
        _FresnelLandPow ("Fresnel Land Power", Float ) = 0
        _FresnelLandMult ("Fresnel Land Multiplier", Float ) = 0

        _Population ("Population", Float ) = 0

      	_CloudsShadows ("Clouds Shadow", Float ) = 0      	
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
            
            	float3 baseColor = AddBaseColor(p);  
            	baseColor = AddDesertColor(baseColor, p);
            	baseColor = AddVegetationColor(baseColor, p);             	
            	baseColor = AddMountainColor(baseColor, p);  				 
            	baseColor = AddWater(baseColor, p);    
				baseColor = saturate(baseColor * baseColor * _ColorPassTrough);
				baseColor = AddDiffuse(baseColor, p);
				baseColor = AddFresnelLand(baseColor, p);
				baseColor = AddCityLights(baseColor, p);
				baseColor = AddClouds(baseColor, p);				
                baseColor += (1 - p.DiffuseFactor) * p.Ambient;

            	return float4(baseColor, 1);
            }

			ENDCG
		} 
	} 

	FallBack Off
}
