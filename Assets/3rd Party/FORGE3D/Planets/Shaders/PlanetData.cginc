#ifndef FC_PLANETDATA
#define FC_PLANETDATA

uniform float4 _SunPos;

uniform sampler2D _GradientMap;
uniform sampler2D _SurfaceNormalMap;
uniform sampler2D _DetailNormalMap;
uniform sampler2D _OceanMask;
uniform sampler2D _HeightMap;
uniform sampler2D _DetailMap;
uniform sampler2D _CloudsMap; 
uniform sampler2D _CloudsCapMap;
uniform sampler2D _SunsetMap; 
uniform sampler2D _CityLightUVMap;
uniform sampler2D _CityLightMaskMap;
uniform sampler2D _CityLightMap;
uniform sampler2D _LightningMaskMap;
uniform sampler2D _LightningMap;

uniform float4 _DeepWaterColor;
uniform float4 _ShallowWaterColor;
uniform float4 _ShoreColor;
uniform float4 _TintColor;
uniform float4 _VegetationColor;
uniform float4 _DesertColor;
uniform float4 _MountainColor;

uniform float4 _Ramp1Color;
uniform float4 _Ramp2Color;
uniform float4 _Ramp3Color;

uniform float4 _WaterSpecularColor;
uniform float4 _LandSpecularColor;

uniform float4 _CloudsColor;
uniform float4 _CloudsShadowColor;

uniform float4 _CityLightColor;

uniform float4 _LightningColor;

uniform float _ColorPassTrough;
uniform float _NormalHeight;
uniform float _HeightTile;
uniform float _DiffusePow;

uniform float _VegetationCoverage;
uniform float _VegetationFactors;
uniform float _DesertCoverage;
uniform float _DesertFactors;
uniform float _MountainCoverage;
uniform float _MountainFactors;

uniform float _ShoreFactor;
uniform float _WaterDetailPow;
uniform float _WaterDetailMult;

uniform float _WaterFresnelPow;
uniform float _WaterFresnelMult;
uniform float _WaterSpecularPow;
uniform float _WaterSpecularMult;
uniform float _FresnelLandPow;
uniform float _FresnelLandMult;

uniform float _Population;

uniform float _CloudsShadows;
uniform float _CloudsBrightness;
uniform float _CloudsSunset;
uniform float _CloudsAnimation;

uniform float _RampBias;

uniform float _LightningFrqA;
uniform float _LightningFrqB;

uniform float _LightningSineMult;
uniform float _LigntingMaskPow;

struct PlanetInput
{
    float4 vertex : POSITION;
    fixed4 color : COLOR;
    float3 normal : NORMAL;
    float4 tangent : TANGENT;
    float2 texcoord0 : TEXCOORD0;
    float2 texcoord1 : TEXCOORD1;
};

struct PlanetOutput
{
	float4 pos : SV_POSITION;
	float4 uv : TEXCOORD0;   	            
	float4 posWorld : TEXCOORD1;
	float3 normalDir : TEXCOORD2;
	float3 tangentDir : TEXCOORD3;
	float3 binormalDir : TEXCOORD4;  
	
};

struct PlanetData
{	
	float3 NormalDir;	
	float3 LightDir;
	float3 LightDirTS;
	float3 ViewDir;
	float4 UV;
	float2 ScatterUV;
	float GradientMap;
	float3 NormalMap;
	float2 OceanMap;
	float4 HeightMap;	
	float4 DetailMap;	
	float3 SunsetMap;
	float DiffuseFactor;
	float DiffuseFactorNM;
	float3 Ambient;
	float3 LightColor;
	float shadow;
};

#endif