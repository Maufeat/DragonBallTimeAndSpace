// Upgrade NOTE: replaced '_Object2World' with '_Object2World'

Shader "FORGE3D/Planets/Sun Gamma" 
{
      Properties
    {
        _GradientMap("Pole Gradient", 2D) = "black" {}
        _DistMap("Distortion Map", 2D) = "black" {}  
        _SurfaceDetail("Surface Detail", 2D) = "black" {}  

        _Cool("Cool Zone", Color) = (0.5,0.5,0.5,0.5)
        _Warm("Warm Zone", Color) = (0.5,0.5,0.5,0.5)
        _Hot("Hot Zone", Color) = (0.5,0.5,0.5,0.5)
        _Atmo("Atmo Zone", Color) = (0.5,0.5,0.5,0.5)

        _VertexTile("Vertex Tile", float) = 0
        _VertexSpeed("Vertex Speed", float) = 0
        _VertexPower("Vertex Power", float) = 0
        _VertexFalloff("VertexFalloff", float) = 0

        _AtmoFalloff("Atmo Falloff", float) = 0
        _AtmoPower("Atmo Power", float) = 0

        _Intensity("Intensity", float) = 0      
        _DistFact("Dist Speed | Dist Factor | Height Speed | Unused", Vector) = (0, 0, 0, 0)  
    }

  
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

    CGPROGRAM
        #pragma surface surf Lambert vertex:vert    
        #pragma target 3.0
     
        sampler2D _SurfaceDetail;       
        sampler2D _GradientMap;
        sampler2D _DistMap;

        float4 _Cool, _Warm, _Hot, _Atmo;
        float _AtmoFalloff, _AtmoPower;
        float _Intensity;
        float4 _DistFact;

        float _VertexTile, _VertexSpeed, _VertexPower, _VertexFalloff;

        void vert(inout appdata_full v)
        {
           float3 vMod;

           float4 vPos = mul(UNITY_MATRIX_MV, v.vertex);
            
           float4 posWorld = mul(_Object2World, v.vertex);
           float4 posObj = mul(_Object2World, float4(0.0,0.0,0.0,1.0)); 

           vMod = (posWorld - posObj) * _VertexTile ;
          
           vMod += (_Time.x * _VertexSpeed);
           vMod = sin(vMod) * _VertexPower;

           float3 cam = normalize(_WorldSpaceCameraPos - posWorld);
           float rDot = 1 - abs(dot(v.normal, cam));
           rDot = pow(rDot, _VertexFalloff);

           v.vertex.xyz += vMod * rDot;
        }

        struct Input
        {
            float2 uv_SurfaceDetail;
            float2 uv2_GradientMap;
            float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutput o) 
        {
            float4 uv = float4(IN.uv_SurfaceDetail, IN.uv2_GradientMap);     
            uv = float4(uv.x * 2, uv.y, uv.z * 0.5, uv.w * 0.5);

            float gMap = tex2D(_GradientMap, uv.xy).r;      
            
            float2 uv0_ = uv.xy * floor(4.0) - float2(_Time.x * _DistFact.x, 0);
            float2 uv1_ = uv.zw * floor(4.0) - float2(_Time.x * _DistFact.x, 0);;
            float4 bellyMap = tex2D(_DistMap, uv0_);
            float4 poleMap = tex2D(_DistMap, uv1_);
            float mask = saturate(pow(1 - gMap, 3) * 2);

            float distA = lerp(bellyMap, poleMap, mask);         

            uv0_ = uv.xy * floor(4.0) + float2(_Time.x * _DistFact.x, 0);
            uv1_ = uv.zw * floor(4.0) + float2(_Time.x * _DistFact.x, 0);;
            bellyMap = tex2D(_DistMap, uv0_);
            poleMap = tex2D(_DistMap, uv1_);
            mask = saturate(pow(1 - gMap, 3) * 2);

            float distB = lerp(bellyMap, poleMap, mask);   

            float dist = saturate(distA * distB) * _DistFact.y;

            float4 r,g;

            uv0_ = uv.xy * floor(2.0) + dist + float2(_Time.x * _DistFact.z, 0);
            uv1_ = uv.zw * floor(2.0) + dist + float2(_Time.x * _DistFact.z, 0);
            bellyMap = tex2D(_SurfaceDetail, uv0_);
            poleMap = tex2D(_SurfaceDetail, uv1_);
            mask = saturate(pow(1 - gMap, 3) * 2);

            r =  lerp(bellyMap, poleMap, mask);

            uv0_ = uv.xy * floor(1.0) + dist + float2(_Time.x * _DistFact.z, 0);
            uv1_ = uv.zw * floor(2.0) + dist + float2(_Time.x * _DistFact.z, 0);
            bellyMap = tex2D(_SurfaceDetail, uv0_);
            poleMap = tex2D(_SurfaceDetail, uv1_);
            mask = saturate(pow(1 - gMap, 3) * 2);

            g =  lerp(bellyMap, poleMap, mask);

            float3 color =  pow(r.g, 1) * 5;
            float3 color2 = pow(g.r, 1) * 5;
            color = lerp (_Cool * _Cool.a * 1, _Warm * _Warm.a * 1, color) ;
            color2 = lerp (_Hot * _Hot.a * 1, _Hot * _Hot.a * 1, color2) ;
            color *= r.g * 1;
            color += color2 * g.r ;
           


            float3 atmo = saturate(pow(1 - dot(normalize(IN.viewDir), normalize(o.Normal)), _AtmoFalloff) * _AtmoPower) * _Atmo * _Atmo.a * 10;

            o.Albedo = 0;
            o.Alpha = 1;
            o.Emission = atmo + color * _Intensity;
            //o.Emission = dist;
        }
        ENDCG
    }

    Fallback "VertexLit"
}

