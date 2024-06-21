// Upgrade NOTE: replaced '_Object2World' with '_Object2World'
// Upgrade NOTE: replaced '_World2Object' with '_World2Object'

Shader "FORGE3D/Planets/Atmosphere"
{
	Properties
	{
		_Scattering ("Scattering Gradient", 2D) = "white" {}
        _Color  ("Scattering Color", Color) = (0.5,0.5,0.5,1)
        _InnerRingColor ("Glow Color", Color) = (0.5,0.5,0.5,1)
        
        _ScatteringOffset ("Scattering Offset", Float ) = 0
        _ScatteringFactor ("Scattering Factor", Float ) = 0
        _Intensity ("Scattering Intensity", Float ) = 0
        _InnterRingFactor ("Glow Factor", Float ) = 0
        _InnerRingOffset ("Glow Offset", Float ) = 0
      
        _Offset ("Vertex Offset", Float ) = 0        
	}

	SubShader
	{
		Tags
		{
		    "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"          
		}
		
		Pass
		{
			Blend One One
			AlphaTest Greater 0.1
			ColorMask RGBA
			Cull Back Lighting Off ZWrite Off Fog { Color (0,0,0,0) }

			CGPROGRAM
		 	#pragma vertex vert
            #pragma fragment frag
         //  #pragma only_renderers opengl d3d9 d3d11
            #pragma glsl
            #pragma target 3.0
           


			uniform float _ScatteringFactor;
            uniform sampler2D _Scattering; uniform float4 _Scattering_ST;
            uniform float _Intensity;
            uniform float _ScatteringOffset;
            uniform float _InnterRingFactor;
            uniform float4 _Color;
            uniform float4 _InnerRingColor;
            uniform float _InnerRingOffset;
            uniform float _OutterRingFactorA;
            uniform float _OutterRingFactorB;
            uniform float4 _OutterRingColor;
            uniform float _OutterRingIntensity;
            uniform float _Offset;
            uniform float _Wrap;

            uniform float _Pow;

            uniform float4 _SunPos;
        

            struct VertexInput
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct VertexOutput
            {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };

            VertexOutput vert (VertexInput v)
            {
                VertexOutput o;

                o.normalDir = normalize(mul(float4(v.normal, 0), _World2Object).xyz);      
                
               v.vertex.xyz += (_Offset*v.normal);         
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }

        

            float4 frag(VertexOutput i) : COLOR
            {
            	float3 normalDirection =  i.normalDir;
               	float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);           	 
                float3 lightDirection = normalize(_SunPos.xyz - i.posWorld.xyz);

               	float ndl = (dot(normalDirection, lightDirection));
               	float ndv = saturate(dot(normalDirection, viewDirection));              

               	float offsetScattering = saturate((abs(dot(normalDirection, viewDirection)) + _ScatteringOffset / 10.0) * 1000);
                float offsetInnerRing = saturate((abs(dot(normalDirection, viewDirection)) + _InnerRingOffset / 10.0) * 1000);

           		float3 scatterMap = tex2D(_Scattering, ndv.xx).xyz * _Color;
           		scatterMap *= offsetScattering * pow(1 - ndv, _ScatteringFactor) * _Intensity;
           		
           		float3 innerRing = saturate(_InnerRingColor.xyz * pow(1 - dot(viewDirection, normalDirection), _InnterRingFactor)) * offsetInnerRing;             
                float3 finalColor = saturate(pow(ndl + max(0.1, 0.7 * (dot(-lightDirection, viewDirection))), 3)) * (scatterMap + innerRing * 2);
              
          
			  return float4(finalColor, 1);
             
            }

			ENDCG	
		}
	} 	
}
