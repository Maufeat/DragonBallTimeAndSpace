Shader "FORGE3D/Planets/Moon" 
{
	Properties
	{
		_GradientMap("Pole Gradient", 2D) = "black" {}
		_SurfaceNormalMap("Surface Normal", 2D) = "black" {}	
        _DetailNormalMap("Detail Normal", 2D) = "black" {}  	
		_ColorMap ("Color Map", 2D) = "black" {}
        _SpecMap ("Specular Map", 2D) = "black" {}        
        _LandSpecularColor("Land Specular Color", Color) = (0.5,0.5,0.5,1) 
		_NormalHeight("Normal Height", Float) = 0
		_HeightTile("Height Tile", Float) = 0
        _SpecTile("Specular Tile", Float) = 0
		_DiffusePow("Diffuse Power", Float) = 0
        _FresnelLandPow ("Fresnel Land Power", Float ) = 0
        _FresnelLandMult ("Fresnel Land Multuplier", Float ) = 0
        _SpecLandPow ("Specular Land Power", Float ) = 0
        _SpecLandMult ("Specular Land Multiplier", Float ) = 0   
        _SpecWrap("Specular Wrap", Float) = 0   
        _NormalBias("Normal Bias", Float) = 0
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
            #pragma fragment MoonFrag
            
          	#include "UnityCG.cginc"
            #include "AutoLight.cginc" 
            #include "PlanetData.cginc"
            #include "PlanetLighting.cginc"
            #include "PlanetVS.cginc"
			#include "PlanetPS.cginc"
 
            uniform sampler2D _ColorMap;
            uniform sampler2D _SpecMap;
            uniform float _NormalBias;
            uniform float _SpecTile;
            uniform float _SpecLandPow;
            uniform float _SpecLandMult;
            uniform float _SpecWrap;

            float4 MoonFrag(PlanetOutput i) : COLOR
            {
                PlanetData p = GetMoonData(i);  

                float4 uvScaled = float4(p.UV.x * 2, p.UV.y, p.UV.z * 0.5, p.UV.w * 0.5);
                float4 surf = MixPoleMap(_SurfaceNormalMap, p.GradientMap, uvScaled, _HeightTile);
                float4 detail = MixPoleMap(_DetailNormalMap, p.GradientMap, uvScaled, _HeightTile);
                float4 nor = (lerp(surf, detail, _NormalBias));

                float3 NormalMap = (UnpackPlanetNormal(nor, _NormalHeight));            
                NormalMap = (TangentToWorld(NormalMap, i));

                float4 specMap = 
                pow(MixPoleMap(_SpecMap, p.GradientMap, uvScaled, _SpecTile) * 
                saturate(dot(p.NormalDir, p.LightDir + p.ViewDir)), _SpecLandPow) * 
                _SpecLandMult * lerp(saturate(dot(NormalMap, p.LightDir)), 1, _SpecWrap)  * max(p.shadow, 0.1);

                float baseColor = pow(saturate(dot(NormalMap, p.LightDir)), _DiffusePow)  * max(p.shadow, 0.01);
                float4 color = MixPoleMap(_ColorMap, p.GradientMap, uvScaled, _HeightTile);

                float3 fresnel = pow(saturate(1 - dot(p.ViewDir, NormalMap)), _FresnelLandPow) * _FresnelLandMult * color.xyz * _LandSpecularColor;

                float3 finalColor = specMap.xyz + ( color + saturate(fresnel) ) * baseColor + (1 - baseColor) * p.Ambient;

                return float4(finalColor, 1);
            }

			ENDCG
		}
	}

	FallBack Off
}
