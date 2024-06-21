Shader "FORGE3D/Planets/Ice" 
{
	Properties
	{
		_GradientMap("Pole Gradient", 2D) = "black" {}
		_SurfaceNormalMap("Surface Normal", 2D) = "black" {}	
		
		_HeightMap ("Height Map", 2D) = "black" {}
		_DetailMap ("Detail Map", 2D) = "black" {}
        _ColorMap ("Color Map", 2D) = "black" {}
        _SunsetMap ("Sunset Map", 2D) = "black" {} 
	
        _IceRampColorLow("Ice Ramp Low", Color) = (0.5,0.5,0.5,1)
        _IceRampColorMid("Ice Ramp Med", Color) = (0.5,0.5,0.5,1)
        _IceRampColorHigh("Ice Ramp High", Color) = (0.5,0.5,0.5,1)
        _SpecularColor("Specular Color", Color) = (0.5,0.5,0.5,1)
        _FresnelColor("Fresnel Color", Color) = (0.5,0.5,0.5,1)

        _NormalHeight("Normal Height", Float) = 0
        _HeightTile("Height Tile", Float) = 0
       
        _IceDetail("Detail Multiplier", Float) = 0	

       	_FresnelPow ("Fresnel Power", Float ) = 0
        _FresnelMult ("Fresnel Multiplier", Float ) = 0
        _SpecularPow ("Specular Power", Float ) = 0
        _SpecularMult ("Specular Multiplier", Float ) = 0
        _IceFactors("Ramp Low | Ramp High | Ramp Bias | Unused", Vector) = (0,0,0,0)
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
                
            uniform float _IceDetail;
            uniform float4 _IceFactors;

            uniform float4 _IceRampColorLow;
            uniform float4 _IceRampColorMid;
            uniform float4 _IceRampColorHigh;

            uniform sampler2D _ColorMap;

            uniform float4 _SpecularColor;
            uniform float4 _FresnelColor;
            uniform float _FresnelPow;
            uniform float _FresnelMult;
            uniform float _SpecularPow;
            uniform float _SpecularMult;
            uniform float _SpecularHeightOff;
            uniform float _SpecularHeightOn;

            float4 EarthLikeFrag(PlanetOutput i) : COLOR
            {                
            	PlanetData p = GetPlanetData(i); 
                
                float4 uvColorMap = float4(p.UV.x * 1, p.UV.y, p.UV.z * 0.5, p.UV.w * 1);
                float4 colorMap = MixPoleMap(_ColorMap, p.GradientMap, uvColorMap, _HeightTile);

                float4 uvScaled = float4(p.UV.x * 2, p.UV.y, p.UV.z * 0.5, p.UV.w * 0.5);
                float4 detail = MixPoleMap(_DetailMap, p.GradientMap, uvScaled, _HeightTile);            	
                float detailTex = pow(detail.x *detail.y, 0.5);

                float detaledHeight = p.HeightMap.r + _IceDetail * 2 * (detailTex - 0.5);
                float detaledHeight01 = linstep(_IceFactors.x - _IceDetail, _IceFactors.y + _IceDetail, detaledHeight);
                float3 detailedDepthColor  = GetRampColor3(_IceRampColorLow.rgb, _IceRampColorMid.rgb, _IceRampColorHigh.rgb, detaledHeight01, _IceFactors.z);
                detailedDepthColor *= colorMap.rgb;

                float3 baseColor = detailedDepthColor * p.DiffuseFactorNM * 3;

                float3 halfDir = normalize(p.LightDir + p.ViewDir);
                float specDot = saturate(dot(p.NormalDir, halfDir));
                float3 spec = detailTex;
                spec *= _SpecularMult * pow(specDot, _SpecularPow) * _SpecularColor; 

                float3 fresnel = pow(saturate(1 -  dot(p.ViewDir, p.NormalDir)), _FresnelPow) * _FresnelMult * _FresnelColor * detail.z;
            	  
                baseColor += spec * p.DiffuseFactorNM;
                baseColor += fresnel * p.DiffuseFactorNM;
                baseColor *= p.SunsetMap;
                baseColor += (1 - p.DiffuseFactor) * p.Ambient;
                baseColor = saturate(baseColor);
 
            	return float4(baseColor, 1);
            }

			ENDCG
		} 
	}

	FallBack Off
}
