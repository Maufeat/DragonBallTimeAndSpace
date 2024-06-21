// Upgrade NOTE: replaced '_Object2World' with '_Object2World'
// Upgrade NOTE: replaced '_World2Object' with '_World2Object'

Shader "FORGE3D/Planets/Sun Corona"
{
	Properties
	{
		_CoronaNoise("Corona Noise Map", 2D) = "black" {}  

		_DischargeTileX("Discharge Tile X", float) = 0
		_DischargeTileY("Discharge Tile Y", float) = 0
		_DischargePanSpeed("Discharge Pan Speed", float) = 0		

		_CoronaFluidTile("Corona Fluid Tile", float) = 0
		_CoronaFluidInfluence("Corona Fluid Influence", float) = 0

		_SolarStormFalloff("Solar Storm Falloff", float) = 0
		_SolarStormPower("Solar Storm Power", float) = 0

		_CoronaTileX("Corona Tile X", float) = 0
		_CoronaTileY("Corona Tile Y", float) = 0
		_CoronaSpeed("Corona Speed", float) = 0
		_CoronaAmp("Corona Amp", float) = 0
		_CoronaExp("Corona Exp", float) = 0
		_CoronaBoost("Corona Boost", float) = 0
		_CoronaFalloff("Corona Falloff", float) = 0

		_CoronaColor("Corona Color", Color) = (0.5,0.5,0.5,0.5)

		_EdgeMaskFalloff("Edge Mask Falloff", float) = 0
		_EdgeMaskPower("Edge Mask Power", float) = 0

		_InvFade ("Fade Factor", float) = 1.0
	}

	Category
	{
	
		
		
				
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend One One
		AlphaTest Greater .01
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog { Mode Off }
		// Tags { "RenderType"="Opaque" }
		SubShader {	

		// Disable Dynamic Batching
		//Pass{}

		Pass {

			CGPROGRAM
		 	#pragma vertex vert
            #pragma fragment frag
           // #pragma only_renderers opengl d3d9 d3d11
            #pragma glsl
            #pragma target 3.0
            #pragma multi_compile_particles
            #include "UnityCG.cginc"
			
			float _DischargeTileX, _DischargeTileY, _DischargePanSpeed;

			float _CoronaFluidTile, _CoronaFluidInfluence;

			float _SolarStormFalloff, _SolarStormPower;

			float _CoronaTileX, _CoronaTileY, _CoronaSpeed, _CoronaAmp, _CoronaExp, _CoronaBoost, _CoronaFalloff;

			float4 _CoronaColor;

			float _EdgeMaskFalloff, _EdgeMaskPower;

			sampler2D _CoronaNoise;

			sampler2D  _CameraDepthTexture;
			uniform float4   _CameraDepthTexture_ST;
			float _InvFade;

            struct VertexInput
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;       
                float4 color : COLOR;
                float2 texcoord0 : TEXCOORD0;
            };

            struct VertexOutput
            {	
            	float4 color : COLOR;
                float4 pos : POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;              
                float2 uv : TEXCOORD2;   
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD3;
				#endif
            };

            VertexOutput vert (VertexInput v)
            {
                VertexOutput o;
               	o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;               
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.texcoord0;
                #ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.pos);
			//	o.projPos = clamp(o.projPos, 0, 1);
			//	o.projPos.y = 1 - o.projPos.y;
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;
                return o;
            }

          	float2 PolarCood(float2 uv, float xTile, float yTile)
          	{
          		float2 uvScaled = uv - 1;
          		float2 uvMult = pow(uvScaled, 2);
          		float a1 = sqrt(uvMult.x + uvMult.y);
      			float a2 = -atan2(uvScaled.y, uvScaled.x);

      			float twoPI = 3.142 * 2;

      			float2 uvOut;

      			if(a2 >= 0.0) uvOut.x = a2 / twoPI;
      			else uvOut.x = (a2 + twoPI) / twoPI;
      			
      			uvOut.x *= xTile;
      			uvOut.y = a1 * yTile;

          		return uvOut;
          	}

          	float2 Panner(float2 uv, float speedX, float speedY, float t)
          	{
          		return float2(uv.x + speedX * t , uv.y + speedY * t);
          	}

          	float2 Rotator(float2 uv, float speed)
          	{
      		  	uv -=0.5;

	            float s = sin ( speed );
	            float c = cos ( speed );
	           
	            float2x2 rotationMatrix = float2x2( c, -s, s, c);
	            rotationMatrix *=0.5;
	            rotationMatrix +=0.5;
	            rotationMatrix = rotationMatrix * 2-1;
	            uv = mul ( uv, rotationMatrix );
	            uv += 0.5;

	            return uv;
          	}

          	float CoronaMask(float2 _uv, float sizeX, float sizeY, float power, float falloff)
          	{
          		float2 uv= pow(_uv - 0.5, 2);
          		uv.x *= sizeX;
          		uv.y *= sizeY;
          		float mask = 1 - pow(sqrt(uv.x + uv.y) * power, falloff);
          		return clamp(mask, 0 ,1);
          	}

            float4 frag(VertexOutput i) : COLOR
            {
            	float2 uvA = PolarCood(i.uv * 2, _DischargeTileX * 0.5, _DischargeTileY * 0.5);
            	float2 uvB = PolarCood(i.uv * 2, _DischargeTileX, _DischargeTileY);
            	float2 uvC = PolarCood(i.uv * 2, _CoronaTileX, _CoronaTileY);

            	uvA = Panner(uvA, 0, -1, _DischargePanSpeed * _Time.x);
            	uvB = Panner(uvB, 0, -1, _DischargePanSpeed * _Time.x);
            	uvC = Panner(uvC, 0, -1, _CoronaSpeed * _Time.x);

            	float cNoiseA = tex2D(_CoronaNoise, uvA).g;        		
        		float cNoiseC = tex2D(_CoronaNoise, Rotator(i.uv * _CoronaFluidTile * 4, 0.2 * _Time.x)).r * _CoronaFluidInfluence;
        		float cNoiseB = tex2D(_CoronaNoise, uvB + cNoiseC).r;
        		float cNoiseD = tex2D(_CoronaNoise, uvC).g;

        		float sStorm = pow(cNoiseA * cNoiseB, _SolarStormFalloff) * _SolarStormPower;
        		float corona = pow(cNoiseD * _CoronaAmp, _CoronaExp);
            	
        		float cMaskA = (1 - CoronaMask(i.uv, 1, 1, 4, 3)) * 3.5;
        		float cMaskB = CoronaMask(i.uv, 1, 1, 2.25, 0.01) * cMaskA * _CoronaBoost;
        		cMaskB = clamp(pow(cMaskB, _CoronaFalloff), 0, 1);
        		corona += cMaskB;

        		float cMaskC = clamp(CoronaMask(i.uv, 0.65, 0.65, 3.75, 3), 0, 1);

        		float cNoiseE = pow(tex2D(_CoronaNoise, Rotator(i.uv * 2, 4)).r, 1.25) * 2 * cMaskC;

            	float3 finalColor = float3(corona.xxx + sStorm * cNoiseE) * _CoronaColor * 5; 

            	float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);  
            //	float viewDot = dot(viewDirection, mul(_Object2World, float4(1, 0, 0, 0)).rgb);            
              float viewDot = abs(dot(viewDirection, normalize(i.normalDir)));            
            	viewDot = pow(viewDot, _EdgeMaskFalloff) * _EdgeMaskPower;
            	viewDot = saturate(viewDot);

            	#ifdef SOFTPARTICLES_ON				
      				float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
      				float partZ = i.projPos.z;
      				float fade = clamp((_InvFade * (sceneZ-partZ)), 0, 1);
      				i.color.a = fade;
      				#endif

              return float4(finalColor * viewDot * cMaskB * i.color.a, 1);
           //   return float4(viewDot.xxx, 1);

             
            }

			ENDCG	
		}
	} 	
       
}
}
