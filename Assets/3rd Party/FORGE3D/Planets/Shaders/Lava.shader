Shader "FORGE3D/Planets/Lava" 
{
    Properties
    {
        _GradientMap("Pole Gradient", 2D) = "black" {}
        _SurfaceNormalMap("Surface Normal", 2D) = "black" {} 
        
        _HeightMap ("Height Map", 2D) = "black" {}
        _DetailMap ("Detail Map", 2D) = "black" {}
        _MagmaDetailMap("Magma Detail Map", 2D) = "white" {}
        _SunsetMap ("Sunset Map", 2D) = "black" {}        
        _DistMap("Distortion Map", 2D) = "black" {}
      
        _MagmaColorA("Magma Color A", Color) = (0.5,0.5,0.5,1)
        _MagmaColorB("Magma Color B", Color) = (0.5,0.5,0.5,1)
    
        _LavaColorLow("Lava Color Low", Color) = (0.5,0.5,0.5,1)
        _LavaColorMid("Lava Color Mid", Color) = (0.5,0.5,0.5,1)
        _LavaColorHigh("Lava Color High", Color) = (0.5,0.5,0.5,1)

        _SpecularColor("Specular Color", Color) = (0.5,0.5,0.5,1)
        _FresnelColor("Fresnel Color", Color) = (0.5,0.5,0.5,1)     
     
        _NormalHeight("Normal Height", Float) = 0
        _HeightTile("Height Tile", Float) = 0
       
        _IceDetail("Detail Strength", Float) = 0

        _FresnelPow ("Fresnel Pow", Float ) = 0
        _FresnelMult ("Fresnel Mult", Float ) = 0
        _SpecularPow ("Specular Pow", Float ) = 0
        _SpecularMult ("Specular Mult", Float ) = 0   

        _DetailFactors("Lava Mask Power | Lava Mask Multiplier | Unused", Vector) = (0,0,0,0)
        _IceFactors("Ramp Low | Ramp High | Ramp Bias | Unused", Vector) = (0,0,0,0)            
        _MagmaFactors("Magma Pow | Magma Mult | Detail Pow | Detail Mult", Vector) = (0,0,0,0)
        _MagmaTiling("Tiling Base | Tiling Detail | Unused", Vector) = (0,0,0,0)
        _DistFact("Dist Speed | Dist Factor | Height Speed | Glow Mult", Vector) = (0, 0, 0, 0)     
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

            uniform float4 _MagmaColorA;
            uniform float4 _MagmaColorB;

            uniform float4 _LavaColorLow;
            uniform float4 _LavaColorMid;
            uniform float4 _LavaColorHigh;

            uniform float4 _DetailFactors;

            uniform sampler2D _ColorMap;
            uniform sampler2D _MagmaDetailMap;

            uniform float4 _SpecularColor;
            uniform float4 _FresnelColor;
            uniform float _FresnelPow;
            uniform float _FresnelMult;
            uniform float _SpecularPow;
            uniform float _SpecularMult;
            uniform float _SpecularHeightOff;
            uniform float _SpecularHeightOn;

            uniform float4 _MagmaFactors;
            uniform float4 _MagmaTiling;

            uniform float4 _DistFact;

            uniform sampler2D _DistMap;

            float4 EarthLikeFrag(PlanetOutput i) : COLOR
            {                
                PlanetData p = GetPlanetData(i);                 
               
                float2 bDistUV = i.uv.xy * float2(2, 1) * _MagmaTiling.z;
                float2 pDistUV = i.uv.zw * float2(1, 1) * _MagmaTiling.z;

                float distA = tex2D(_DistMap, bDistUV + float2(_Time.x * _DistFact.x, _Time.x * _DistFact.x));
                float distB = tex2D(_DistMap, bDistUV + float2(_Time.x * _DistFact.x, _Time.x * _DistFact.x) * 2);                
               
                float bDist = saturate(distA * distB) * _DistFact.y;

                distA = tex2D(_DistMap, pDistUV + float2(_Time.x * _DistFact.x, _Time.x * _DistFact.x));
                distB = tex2D(_DistMap, pDistUV + float2(_Time.x * _DistFact.x, _Time.x * _DistFact.x) * 2);   
             
                float pDist = saturate(distA * distB) * _DistFact.y;

                float mask = saturate(pow(1 - p.GradientMap, 3) * 2);
                
                float4 finalDist = float4(i.uv.x * 2, i.uv.y, i.uv.z * 1, i.uv.w * 1) + lerp(bDist, pDist, mask) ;
                finalDist.xzw += float(_Time.x * _DistFact.z);
                float4 magmaAnimMask = MixPoleMap(_DistMap, p.GradientMap, finalDist, _MagmaTiling.z);

                float4 uvScaled = float4(p.UV.x * 2, p.UV.y, p.UV.z * 0.5, p.UV.w * 0.5);
                float4 detail = MixPoleMap(_DetailMap, p.GradientMap, uvScaled, _HeightTile);               
                float detailTex = pow(detail.x * detail.g, 0.5);

                float detaledHeight = p.HeightMap.r + _IceDetail * 2 * (detailTex - 0.5);
                float detaledHeight01 = linstep(_IceFactors.x - _IceDetail, _IceFactors.y + _IceDetail, detaledHeight);
                float3 detailedDepthColor  = GetRampColor3(_LavaColorLow.rgb, _LavaColorMid.rgb, _LavaColorHigh.rgb, detaledHeight01, _IceFactors.z);                

                float3 baseColor = detailedDepthColor * p.DiffuseFactorNM;
                
                baseColor *= p.SunsetMap;
            
                float4 magmaDetial = MixPoleMap(_MagmaDetailMap, p.GradientMap, uvScaled, _MagmaTiling.y); 
                magmaDetial *= saturate(pow(magmaDetial, _MagmaFactors.z) ) * _MagmaFactors.w;
                magmaDetial *= p.DiffuseFactorNM + 0.3;
                magmaDetial = min(magmaDetial, 5);
            

                float3 halfDir = normalize(p.LightDir + p.ViewDir);
                float specDot = saturate(dot(p.NormalDir, halfDir));
                float3 spec = detailTex;
                spec *= _SpecularMult * pow(specDot, _SpecularPow) * _SpecularColor; 

                float3 fresnel = pow(saturate(1 -  dot(p.ViewDir, p.NormalDir)), _FresnelPow) * _FresnelMult * _FresnelColor * detail.x;
                
                float lavaMaskMap = MixPoleMap(_HeightMap, p.GradientMap, uvScaled, _MagmaTiling.x);   

                float lavaMask = pow(saturate(1 - lavaMaskMap), _DetailFactors.x) * _DetailFactors.y;
                lavaMask *= pow(dot(p.ViewDir, p.NormalDir), _MagmaFactors.x) * _MagmaFactors.y;
                lavaMask += magmaDetial.x;
                lavaMask *= p.DiffuseFactorNM + 0.3;

                float3 lavaColor = lerp(_MagmaColorA, _MagmaColorB, saturate(lavaMask) ) * magmaAnimMask.r * _DistFact.w;

                baseColor += spec * p.DiffuseFactorNM;
                baseColor += fresnel * p.DiffuseFactorNM;
                
                baseColor += (1 - p.DiffuseFactor) * p.Ambient;
                baseColor += (lavaColor * lavaMask);
                baseColor = max(float3(0, 0, 0), baseColor);
                return float4(baseColor, 1);
            }

            ENDCG
        }
    }
    FallBack Off
}
