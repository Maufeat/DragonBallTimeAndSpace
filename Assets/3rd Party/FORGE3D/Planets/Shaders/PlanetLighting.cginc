#ifndef FC_PLANETLIGHTING
#define FC_PLANETLIGHTING

#define PI 3.14159265359

inline float3 TangentToWorld(float3 source, PlanetOutput p)
{	
	float3x3 tangentTransform = float3x3( p.tangentDir, p.binormalDir, p.normalDir);
	return mul(source, tangentTransform);
}

inline float3 WorldToTangent(float3 source, PlanetOutput p)
{	
	float3x3 tangentTransform = float3x3( p.tangentDir, p.binormalDir, p.normalDir);
	return mul(tangentTransform, source);
}

inline float3 UnpackPlanetNormal(float3 normal, float height)
{
	float2 xy = (normal.xy * 2 - 1) * height;
	float z = sqrt(1 - saturate(dot(xy, xy)));
	return float3(xy, z);
}

inline float4 MixPoleMap(sampler2D tex, float gradient, float4 uv, float tile)
{
	float2 uv0_ = uv.xy * floor(tile);
	float2 uv1_ = uv.zw * floor(tile);
	float4 bellyMap = tex2D(tex, uv0_);
	float4 poleMap = tex2D(tex, uv1_);
	float mask = saturate(pow(1 - gradient, 3) * 2);
	return lerp(bellyMap, poleMap, mask);
}

inline float2 ScatterUV(float3 normal, float3 lightDir, float3 viewDir)
{
	return float2(dot(normal, lightDir) / 2.0 + 0.5, dot(normal, viewDir));
}

inline float2 RotateUV(float2 uv, float time, float speed)
{	
	uv -= 0.5;
	float s = sin ( speed * time );
	float c = cos ( speed * time );
	float2x2 rotationMatrix = float2x2( c, -s, s, c);
	rotationMatrix *= 0.5;
	rotationMatrix += 0.5;
	rotationMatrix = rotationMatrix * 2 - 1;
	uv = mul ( uv, rotationMatrix );
	return uv + 0.5;
}

inline float3 GetRampColor3( float3 color1, float3 color2, float3 color3, float f, float bias )
{
	float3 c = lerp( color1, color2, saturate( f / bias ) );
	return lerp( c, color3, saturate( f / bias - 1.0 ) );
}

inline float linstep( float min, float max, float v )
{
	return clamp( ( v - min ) / ( max - min ), 0, 1 );
}

#endif