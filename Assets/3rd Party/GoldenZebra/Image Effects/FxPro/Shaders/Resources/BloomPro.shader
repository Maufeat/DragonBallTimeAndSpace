Shader "Hidden/BloomPro"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Bloom ("Bloom (RGB)", 2D) = "black" {}
  }
  SubShader
  {
    Tags
    { 
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
      }
      ZWrite Off
      Cull Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      uniform sampler2D _Bloom;

      struct appdata_t
      {
          float4 vertex : POSITION;
          float4 texcoord : TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 : TEXCOORD0;
          float4 vertex : SV_POSITION;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1.w = 1.0;
          tmpvar_1.xyz = in_v.vertex.xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(_Object2World, tmpvar_1));
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          return out_v;
      }
      
      struct OUT_Data_Frag
      {
          float4 color : SV_Target;
      };
      
      OUT_Data_Frag frag(OUT_Data_Vert in_f)
      {
          OUT_Data_Frag out_f;
          out_f.color = tex2D(_Bloom, in_f.xlv_TEXCOORD0);
          return out_f;
      }
      
      ENDCG
    } // end phase
    Pass // ind: 2, name: 
    {
      Tags
      { 
      }
      ZWrite Off
      Cull Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      uniform sampler2D _MainTex;
      uniform float4 _Parameter;

      struct appdata_t
      {
          float4 vertex : POSITION;
          float4 texcoord : TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 : TEXCOORD0;
          float4 vertex : SV_POSITION;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1.w = 1.0;
          tmpvar_1.xyz = in_v.vertex.xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(_Object2World, tmpvar_1));
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          return out_v;
      }
      
      struct OUT_Data_Frag
      {
          float4 color : SV_Target;
      };
      
      OUT_Data_Frag frag(OUT_Data_Vert in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_2 = tex2D(_MainTex, in_f.xlv_TEXCOORD0) * 4.0;
          float4 tmpvar_3 = max((tmpvar_2 / 4.0) - _Parameter.z, float4(0.0, 0.0, 0.0, 0.0));
          out_f.color = tmpvar_3 * _Parameter.w;
          return out_f;
      }
      
      ENDCG
    } // end phase
    Pass // ind: 3, name: 
    {
      Tags
      { 
      }
      ZTest Always
      ZWrite Off
      Cull Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      uniform float4 _MainTex_TexelSize;
      uniform float4 _Parameter;
      uniform sampler2D _MainTex;

      struct appdata_t
      {
          float4 vertex : POSITION;
          float4 texcoord : TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 : TEXCOORD0;
          float2 xlv_TEXCOORD1 : TEXCOORD1;
          float4 vertex : SV_POSITION;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1.w = 1.0;
          tmpvar_1.xyz = in_v.vertex.xyz;
          float4 tmpvar_2;
          tmpvar_2.zw = float2(1.0, 1.0);
          tmpvar_2.xy = in_v.texcoord.xy;
          out_v.vertex = mul(unity_MatrixVP, mul(_Object2World, tmpvar_1));
          out_v.xlv_TEXCOORD0 = tmpvar_2;
          out_v.xlv_TEXCOORD1 = _MainTex_TexelSize.xy * _Parameter.x;
          return out_v;
      }
      
      struct OUT_Data_Frag
      {
          float4 color : SV_Target;
      };
      
      OUT_Data_Frag frag(OUT_Data_Vert in_f)
      {
          OUT_Data_Frag out_f;
          float4 tap_1 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy - in_f.xlv_TEXCOORD1 * 2.0);
          float4 color_2 = tap_1 * float4(0.0625, 0.0625, 0.0625, 0.0);
          float2 coords_3 = in_f.xlv_TEXCOORD0.xy + in_f.xlv_TEXCOORD1;
          tap_1 = tex2D(_MainTex, coords_3);
          color_2 += tap_1 * float4(0.25, 0.25, 0.25, 0.0);
          coords_3 += in_f.xlv_TEXCOORD1;
          tap_1 = tex2D(_MainTex, coords_3);
          color_2 += tap_1 * float4(0.375, 0.375, 0.375, 1.0);
          coords_3 += in_f.xlv_TEXCOORD1;
          tap_1 = tex2D(_MainTex, coords_3);
          color_2 += tap_1 * float4(0.25, 0.25, 0.25, 0.0);
          coords_3 += in_f.xlv_TEXCOORD1;
          out_f.color = color_2;
          return out_f;
      }
      
      ENDCG
    } // end phase
    Pass // ind: 4, name: 
    {
      Tags
      { 
      }
      ZTest Always
      ZWrite Off
      Cull Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      uniform float4 _MainTex_TexelSize;
      uniform float4 _Parameter;
      uniform sampler2D _MainTex;

      struct appdata_t
      {
          float4 vertex : POSITION;
          float4 texcoord : TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 : TEXCOORD0;
          float2 xlv_TEXCOORD1 : TEXCOORD1;
          float4 vertex : SV_POSITION;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1.w = 1.0;
          tmpvar_1.xyz = in_v.vertex.xyz;
          float4 tmpvar_2;
          tmpvar_2.zw = float2(1.0, 1.0);
          tmpvar_2.xy = in_v.texcoord.xy;
          out_v.vertex = mul(unity_MatrixVP, mul(_Object2World, tmpvar_1));
          out_v.xlv_TEXCOORD0 = tmpvar_2;
          out_v.xlv_TEXCOORD1 = _MainTex_TexelSize.xy * _Parameter.x;
          return out_v;
      }
      
      struct OUT_Data_Frag
      {
          float4 color : SV_Target;
      };
      
      OUT_Data_Frag frag(OUT_Data_Vert in_f)
      {
          OUT_Data_Frag out_f;
          float4 tap_1 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy - in_f.xlv_TEXCOORD1 * 2.0);
          float4 color_2 = tap_1 * float4(0.0625, 0.0625, 0.0625, 0.0);
          float2 coords_3 = in_f.xlv_TEXCOORD0.xy + in_f.xlv_TEXCOORD1;
          tap_1 = tex2D(_MainTex, coords_3);
          color_2 += tap_1 * float4(0.25, 0.25, 0.25, 0.0);
          coords_3 += in_f.xlv_TEXCOORD1;
          tap_1 = tex2D(_MainTex, coords_3);
          color_2 += tap_1 * float4(0.375, 0.375, 0.375, 1.0);
          coords_3 += in_f.xlv_TEXCOORD1;
          tap_1 = tex2D(_MainTex, coords_3);
          color_2 += tap_1 * float4(0.25, 0.25, 0.25, 0.0);
          coords_3 += in_f.xlv_TEXCOORD1;
          out_f.color = color_2;
          return out_f;
      }
      
      ENDCG
    } // end phase
  }
  FallBack Off
}
