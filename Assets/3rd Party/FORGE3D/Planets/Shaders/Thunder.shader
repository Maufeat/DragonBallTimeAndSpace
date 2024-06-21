Shader "FORGE3D/Planets/Thunderstorm" 
{
	Properties
	{
        _GradientMap("Pole Gradient", 2D) = "black" {}
        _DistMap("Dist Map", 2D) = "black" {}
	    _HeightMap("Height Map", 2D) = "black" {}   
        _LightningMaskMap("Lightning Mask Map", 2D) = "black" {}
        _LightningMap("Lightning Map", 2D) = "black" {} 
        _SunsetMap("Sunset Map", 2D) = "black" {}

        _SpecColor("Spec Color", Color) = (0, 0, 0, 0)
        _FresnelColor("Fresnel Color", Color) = (0, 0, 0, 0)
        _LightningColor("Lightning Color", Color) = (0, 0, 0, 0)
        _TintColorA("Tint Color Low", Color) = (0, 0, 0, 0)
        _TintColorB("Tint Color High", Color) = (0, 0, 0, 0)

        _HeightTile("Height Tile", Float) = 0
        _SpecPow("Spec Pow", Float) = 0
        _SpecMult("Spec Mult", Float) = 0       
        _FresnelPow("Fresnel Pow", Float) = 0
        _FresnelMult("Fresnel Mult", Float) = 0       
		
        _LightningFrqA("Lightning Frq A", Float) = 0
        _LightningFrqB("Lightning Frq B", Float) = 0
        _LightningSineMult("Lightning Sine Mult", Float) = 0
        _LigntingMaskPow("Lightning Mask Pow", Float) = 0
       
        _DistFact("Dist Factor", Vector) = (0, 0, 0, 0)
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
            #pragma fragment ThunderFrag
            
          	#include "UnityCG.cginc"
            #include "AutoLight.cginc" 
            #include "PlanetData.cginc"
            #include "PlanetLighting.cginc"
            #include "PlanetVS.cginc" 
			#include "PlanetPS.cginc"
            
            uniform float4 _TintColorA;
            uniform float4 _TintColorB;
            uniform float4 _SpecColor;
            uniform float _SpecPow;
            uniform float _SpecMult;
            uniform float4 _FresnelColor;
            uniform float _FresnelPow;
            uniform float _FresnelMult;
            uniform float4 _DistFact;
            uniform sampler2D _DistMap; 
            
            float4 ThunderFrag(PlanetOutput i) : COLOR
            {                
            	PlanetData p = GetPlanetData(i); 
                p.OceanMap.x = 1;
                p.OceanMap.y = 1;

                float2 bDistUV = i.uv.xy * float2(2, 1) * 5;
                float2 pDistUV = i.uv.zw * float2(1, 1) * 5;

                float distA = tex2D(_DistMap, bDistUV + float2(_Time.x * _DistFact.x, 0));
                float distB = tex2D(_DistMap, bDistUV + float2(_Time.x * _DistFact.x, 0) * 2);                
               
                float bDist = saturate(distA * distB) * _DistFact.y;

                distA = tex2D(_DistMap, pDistUV - float2(_Time.x * _DistFact.x, _Time.x * _DistFact.x));
                distB = tex2D(_DistMap, pDistUV + float2(_Time.x * _DistFact.x, _Time.x * _DistFact.x)); 
             
                float pDist = saturate(distA * distB) * _DistFact.y;

                float mask = saturate(pow(1 - p.GradientMap, 3) * 2);              

                float4 finalDist = float4(i.uv.x * 2, i.uv.y, i.uv.z * 0.5, i.uv.w * 0.5) + lerp(bDist, pDist, mask) ;
                finalDist.xzw += float(_Time.x * _DistFact.z);

                float cloudMask = MixPoleMap(_HeightMap, p.GradientMap, finalDist, _HeightTile).r;                
                
                float3 baseColor = pow(lerp(_TintColorA.rgb, _TintColorB.rgb, cloudMask), 1) * 1.0;
                baseColor += cloudMask * saturate(pow(1 - dot(p.ViewDir, p.NormalDir), _FresnelPow)) * _FresnelMult * _FresnelColor;

               float3 spec = saturate(dot(p.LightDir, -reflect(p.ViewDir, p.NormalDir)));
                spec = pow(spec, _SpecPow) * _SpecColor.rgb * _SpecMult;

                baseColor += spec * cloudMask;

                baseColor *= p.DiffuseFactor * p.SunsetMap;
                baseColor += (1 - p.DiffuseFactor) * p.Ambient;
                baseColor = AddLightning(baseColor, p);

               

            	return float4(baseColor,1);
            }
			ENDCG
		}
	}
	FallBack Off
}
