
#ifndef FC_PLANETVS
#define FC_PLANETVS

PlanetOutput EarthLikeVert (PlanetInput v)
{
    PlanetOutput o;
    o.uv.xy = v.texcoord0;
    o.uv.zw = v.texcoord1;
    o.normalDir = normalize(mul(float4(v.normal, 0), _World2Object).xyz);  
    o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
    o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
    o.posWorld = mul(_Object2World, v.vertex);
    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);    
                
    return o;
}

#endif