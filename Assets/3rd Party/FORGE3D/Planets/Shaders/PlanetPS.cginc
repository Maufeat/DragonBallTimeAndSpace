#ifndef FC_PLANETPS
#define FC_PLANETPS

inline PlanetData GetPlanetData(PlanetOutput p)
{
	PlanetData pData;

	pData.NormalDir = normalize(p.normalDir);	
	pData.LightDir = normalize(_SunPos - p.posWorld.xyz);	
	pData.LightDirTS = normalize(WorldToTangent(pData.LightDir, p));
	pData.ViewDir = normalize(_WorldSpaceCameraPos.xyz - p.posWorld.xyz);

	pData.UV = p.uv;
	pData.ScatterUV = ScatterUV(pData.NormalDir, pData.LightDir, pData.ViewDir);
	float4 uvScaled = float4(pData.UV.x * 2, pData.UV.y, pData.UV.z * 0.5, pData.UV.w * 0.5);

	pData.GradientMap = tex2D(_GradientMap, pData.UV.xy).r;
	
	pData.NormalMap = (UnpackPlanetNormal((MixPoleMap(_SurfaceNormalMap, pData.GradientMap, uvScaled, _HeightTile)).xyz, _NormalHeight));
	pData.NormalMap = normalize(TangentToWorld(pData.NormalMap, p));
	
	pData.OceanMap = tex2D(_OceanMask, pData.UV.xy).rg;
	pData.HeightMap = MixPoleMap(_HeightMap, pData.GradientMap, uvScaled, _HeightTile);
	 
	pData.SunsetMap = tex2D(_SunsetMap, pData.ScatterUV);	

	pData.DiffuseFactor = saturate(dot(pData.LightDir, pData.NormalDir));
	pData.DiffuseFactorNM = saturate(dot(pData.LightDir, pData.NormalMap));

	pData.Ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
	
	pData.shadow =1;
	
	pData.DiffuseFactor *= pData.shadow;
	pData.DiffuseFactorNM *= pData.shadow;

	return pData;
}

inline PlanetData GetMoonData(PlanetOutput p)
{
	PlanetData pData;

	pData.NormalDir = normalize(p.normalDir);	
	pData.LightDir = normalize(_SunPos - p.posWorld.xyz);	
	pData.LightDirTS = normalize(WorldToTangent(pData.LightDir, p));
	pData.ViewDir = normalize(_WorldSpaceCameraPos.xyz - p.posWorld.xyz);

	pData.UV = p.uv;
	pData.ScatterUV = ScatterUV(pData.NormalDir, pData.LightDir, pData.ViewDir);
	
	float4 heightUV = float4(pData.UV.x * 2, pData.UV.y * 1, pData.UV.z * 0.5, pData.UV.w * 0.5);

	pData.GradientMap = tex2D(_GradientMap, pData.UV.xy).r;
	
	pData.NormalMap = (UnpackPlanetNormal((MixPoleMap(_SurfaceNormalMap, pData.GradientMap, heightUV, _HeightTile)).xyz, _NormalHeight));
	pData.NormalMap = normalize(TangentToWorld(pData.NormalMap, p));	
	
	pData.HeightMap = MixPoleMap(_HeightMap, pData.GradientMap, heightUV, _HeightTile);
	pData.DetailMap = MixPoleMap(_DetailMap, pData.GradientMap, heightUV, _HeightTile);

	pData.DiffuseFactor = saturate(dot(pData.LightDir, pData.NormalDir));
	pData.DiffuseFactorNM = saturate(dot(pData.LightDir, pData.NormalMap));

	pData.Ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
	
	pData.shadow =1;
	
	pData.DiffuseFactor *= pData.shadow;
	pData.DiffuseFactorNM *= pData.shadow;

	return pData;
}

inline float3 AddBaseColor(PlanetData p)
{
	return p.HeightMap.r * _TintColor.rgb;
}

inline float3 AddVegetationColor(float3 color, PlanetData p)
{
	float3 vegColor = p.HeightMap.g * _VegetationColor.rgb;
	float mask = (_VegetationCoverage - p.HeightMap.b * p.HeightMap.r) * _VegetationFactors;
	mask = saturate(lerp(0, 1, mask));
	return lerp(color, vegColor, mask);
}

inline float3 AddDesertColor(float3 color, PlanetData p)
{
	float3 desColor = p.HeightMap.b  * _DesertColor.rgb;
	float mask = (_DesertCoverage - p.HeightMap.b * p.HeightMap.r) * _DesertFactors;
	mask = saturate(lerp(0, 1, mask));
	return lerp(color, desColor, mask);
}

inline float3 AddMountainColor(float3 color, PlanetData p)
{
	float3 mntColor = p.HeightMap.r * _MountainColor.rgb;
	float mask = (_MountainCoverage - p.HeightMap.b) * _MountainFactors;
	mask = saturate(lerp(0, 1, mask));
	return lerp(color, mntColor, mask);
}

inline float3 AddWater(float3 color, PlanetData p)
{	
	float depth =  saturate(pow(p.HeightMap.b * p.HeightMap.r, _WaterDetailPow) * _WaterDetailMult);
	float shoreMask = saturate(pow(p.OceanMap.g, _ShoreFactor));

	float3 waterColor = lerp(_DeepWaterColor, _ShallowWaterColor, depth);
	waterColor = saturate(lerp(_ShoreColor.rgb + waterColor, waterColor, shoreMask));

	float waterSpec = saturate(dot(p.LightDir, -reflect(p.ViewDir, p.NormalDir)));
	waterSpec = saturate(pow(waterSpec, _WaterSpecularPow) * _WaterSpecularMult);

	float waterFresnel = saturate(pow(saturate(1 - dot(p.ViewDir, p.NormalDir)), _WaterFresnelPow) * _WaterFresnelMult);

	waterColor += (waterSpec + waterFresnel) * _WaterSpecularColor;
	return lerp(color, saturate(waterColor), p.OceanMap.r);	
}

inline float3 AddDiffuse(float3 color, PlanetData p)
{
	float diffuse = lerp(p.DiffuseFactorNM, p.DiffuseFactor,  p.OceanMap.r * 0.5);
	return color * (pow(diffuse, _DiffusePow) + 0.0);
}

inline float3 AddFresnelLand(float3 color, PlanetData p)
{
	float fresnel = saturate(pow(saturate(1 - dot(p.ViewDir, p.NormalMap)), _FresnelLandPow) * _FresnelLandMult);
	return color + (fresnel * (1 - p.OceanMap.r) * p.DiffuseFactor) * _LandSpecularColor.rgb;
}

inline float3 AddClouds(float3 color, PlanetData p)
{
	float2 shadowUVBelly = p.LightDirTS.xy * 0.005;
	float2 shadowUVPole = p.LightDir.xy * 0.005;

	float2 bellyUV = p.UV.xy;
	bellyUV.x += _CloudsAnimation * 0.15 * _Time;
	float2 poleUV = RotateUV(p.UV.zw, _Time, -_CloudsAnimation);

	float cloudBelly = tex2D(_CloudsMap, bellyUV * float2(2, 1)).r;
	float cloudPole = tex2D(_CloudsCapMap, poleUV).r;
	float cloudMix = lerp(cloudPole, cloudBelly, p.GradientMap + 0.1);

	float cloudBellyShadow = tex2D(_CloudsMap, (bellyUV + shadowUVBelly) * float2(2, 1)).r;
	float cloudPoleShadow = tex2D(_CloudsCapMap, poleUV + shadowUVPole).r;
	float cloudMixShadow = lerp(cloudPoleShadow, cloudBellyShadow, p.GradientMap + 0.1);
	
	cloudMixShadow = saturate(pow(1 - cloudMixShadow, _CloudsShadows));

	color *= cloudMixShadow;
	color += max(p.shadow, 0.005) * saturate(cloudMix * (0.002 + pow(p.SunsetMap, _CloudsSunset)) * _CloudsBrightness * _CloudsColor.rgb);
	return color;
}

inline float3 AddCityLights(float3 color, PlanetData p)
{
	float dayNightFactor = max(p.LightDirTS.z, 0);
	float mask = tex2D(_CityLightMaskMap, p.UV.xy).x;
	float2 detailUV = tex2D( _CityLightUVMap, p.UV.xy ).xy;
	float cityLightMap = tex2D(_CityLightMap, detailUV).x * mask;
	float3 cityLight = pow(cityLightMap, 1 / _Population) * _CityLightColor.rgb * (1 - p.OceanMap.x) * (1 - dayNightFactor);

	return color + cityLight * 2;
}

inline float3 AddLightning(float3 color, PlanetData p)
{
	float lightningMask = tex2D(_LightningMaskMap, 3 * p.UV.xy + _Time.x * float2(-0.01, -0.005) * 25).r;
    lightningMask *= tex2D(_LightningMaskMap, 10 * p.UV.xy + _Time.x * float2(0.01, 0.005) * 25).r;

    float sine = clamp( sin(2 * PI * _Time.y ) , 0.3, 1.0);
    sine *= clamp( sin(5 * PI * _Time.y ) , 0.5, 1.0);
    sine *= clamp( sin(10 * PI * _Time.y ) , 0.7, 1.0);
    lightningMask *= sine * _LightningSineMult;
    lightningMask = saturate(pow(lightningMask, _LigntingMaskPow));

    float2 uv1 = p.UV.xy * float2(2, 1) + 0.9 * floor( _Time.x * _LightningFrqA);
    float3 ligtningTex1 = 3 * tex2D( _LightningMap, uv1);

    float2 uv2 = p.UV.xy * float2(2, 1) + 0.9 * floor( _Time.x * _LightningFrqB);
    float3 ligtningTex2 = tex2D( _LightningMap, uv2);

    float ligtningTex = ligtningTex1.x * ligtningTex2.x;

    float finalMask = 100 * tex2D(_LightningMaskMap, p.UV.xy * float2(2, 1) + _Time.x * 2).r;
    float3 lightning = lightningMask * ligtningTex * finalMask;
    lightning = saturate(lightning) * _LightningColor.rgb * 100;

    // return lightningMask;
    return color + lightning;
}

#endif