﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'mul(UNITY_MATRIX_MVP,*)'

Shader "UI/Default Grey"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	    _Color("Tint", Color) = (1,1,1,1)
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
			_ColorMask("Color Mask", Float) = 15
	}

		SubShader
	{
		Tags
	   {
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	    }

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}
			
		ColorMask[_ColorMask]

		// 源rgba*源a + 背景rgba*(1-源A值)   
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex vert     
#pragma fragment frag     
#include "UnityCG.cginc"     


		struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		half2 texcoord  : TEXCOORD0;
	};

	sampler2D _MainTex;
	fixed4 _Color;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = mul(UNITY_MATRIX_MVP,IN.vertex);
		OUT.texcoord = IN.texcoord;
#ifdef UNITY_HALF_TEXEL_OFFSET     
		OUT.vertex.xy -= (_ScreenParams.zw - 1.0);
#endif     
		OUT.color = IN.color * _Color;
		return OUT;
	}

	fixed4 frag(v2f IN) : SV_Target
	{
		half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
		float grey = dot(color.rgb, fixed3(0.22, 0.707, 0.071));
		return half4(grey,grey,grey,color.a);
	}
		ENDCG
	}
	}
}