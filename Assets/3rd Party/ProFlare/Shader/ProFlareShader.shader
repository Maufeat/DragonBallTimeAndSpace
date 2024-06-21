Shader "ProFlares/Textured Flare Shader"
{
	Properties 
   	{
    	_MainTex ( "Texture", 2D )	= "black" {}   
  	}
    
	SubShader 
	{
		Tags { "Queue"="Transparent+100" "IgnoreProjector"="True" "RenderType"="Transparent" }

    	Pass 
		{    
			ZWrite Off
      	 	ZTest Always 
      	 	Blend One One
     		
			CGPROGRAM
			
 			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			sampler2D	_MainTex;
 
           	struct VertInput
            {
                half4 vertex	: POSITION;
                half2 texcoord	: TEXCOORD0;
			  	fixed4 color	: COLOR;
            };

           	struct Verts
            {
                half4 pos		: SV_POSITION;
                half2 uv		: TEXCOORD0;
			  	fixed4 _color   : COLOR;
            };

			Verts vert ( VertInput vert )
			{
				Verts v;

				v._color		= vert.color*(vert.color.a*3);
    			v.pos			= mul ( UNITY_MATRIX_MVP, vert.vertex );
   				v.uv 	  		= (vert.texcoord.xy);
   				
				return v;
			}
 	
			fixed4 frag ( Verts v ):COLOR
			{
    			return tex2D ( _MainTex, v.uv ) * v._color;
			}

			ENDCG
		}
 	}
}


