Shader "Dragon/Shaodow-Receive" {
Properties {
 _ShadowTex ("Cookie", 2D) = "gray" { }
 _Alpha ("Main Color", Range(0,1)) = 0.5
 _Mask ("Mask", 2D) = "gray" { }
}
SubShader { 
 Tags { "QUEUE"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  Offset -1, -1
  GpuProgramID 34476
Program "vp" {
SubProgram "d3d9 " {
"vs_2_0
					
					//
					// Generated by Microsoft (R) HLSL Shader Compiler 6.3.9600.16384
					//
					// Parameters:
					//
					//   row_major float4x4 _Object2World;
					//   row_major float4x4 _Projector;
					//   row_major float4x4 glstate_matrix_mvp;
					//
					//
					// Registers:
					//
					//   Name               Reg   Size
					//   ------------------ ----- ----
					//   glstate_matrix_mvp c0       4
					//   _Projector         c4       4
					//   _Object2World      c8       3
					//
					
					    vs_2_0
					    dcl_position v0
					    dcl_normal v1
					    dp4 oPos.x, c0, v0
					    dp4 oPos.y, c1, v0
					    dp4 oPos.z, c2, v0
					    dp4 oPos.w, c3, v0
					    dp4 oT0.x, c4, v0
					    dp4 oT0.y, c5, v0
					    dp4 oT0.z, c6, v0
					    dp4 oT0.w, c7, v0
					    dp3 oT1.x, c8, v1
					    dp3 oT1.y, c9, v1
					    dp3 oT1.z, c10, v1
					
					// approximately 11 instruction slots used"
}
SubProgram "d3d11 " {
"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[6];
						mat4x4 _Projector;
						vec4 unused_0_2[6];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 glstate_matrix_mvp;
						vec4 unused_1_1[8];
						mat4x4 _Object2World;
						vec4 unused_1_3[6];
					};
					in  vec4 in_POSITION0;
					in  vec3 in_NORMAL0;
					out vec4 vs_TEXCOORD0;
					out vec3 vs_TEXCOORD1;
					vec4 u_xlat0;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * _Projector[1];
					    u_xlat0 = _Projector[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = _Projector[2] * in_POSITION0.zzzz + u_xlat0;
					    vs_TEXCOORD0 = _Projector[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
					    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
					    u_xlat0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + u_xlat0.xyz;
					    vs_TEXCOORD1.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + u_xlat0.xyz;
					    return;
					}"
}
SubProgram "d3d11_9x " {
"vs_4_0_level_9_1
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[6];
						mat4x4 _Projector;
						vec4 unused_0_2[6];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 glstate_matrix_mvp;
						vec4 unused_1_1[8];
						mat4x4 _Object2World;
						vec4 unused_1_3[6];
					};
					in  vec4 in_POSITION0;
					in  vec3 in_NORMAL0;
					out vec4 vs_TEXCOORD0;
					out vec3 vs_TEXCOORD1;
					vec4 u_xlat0;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * _Projector[1];
					    u_xlat0 = _Projector[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = _Projector[2] * in_POSITION0.zzzz + u_xlat0;
					    vs_TEXCOORD0 = _Projector[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
					    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
					    u_xlat0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + u_xlat0.xyz;
					    vs_TEXCOORD1.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + u_xlat0.xyz;
					    return;
					}"
}
}
Program "fp" {
SubProgram "d3d9 " {
"ps_2_0
					
					//
					// Generated by Microsoft (R) HLSL Shader Compiler 6.3.9600.16384
					//
					// Parameters:
					//
					//   sampler2D _Mask;
					//   sampler2D _ShadowTex;
					//   float4 _cameraPos;
					//
					//
					// Registers:
					//
					//   Name         Reg   Size
					//   ------------ ----- ----
					//   _cameraPos   c0       1
					//   _Mask        s0       1
					//   _ShadowTex   s1       1
					//
					
					    ps_2_0
					    def c1, -1, -0, 0.5, 0
					    dcl t0
					    dcl t1.xyz
					    dcl_2d s0
					    dcl_2d s1
					    texldp_pp r0, t0, s0
					    mul r0.x, r0.w, r0.w
					    cmp r1, -r0.x, c1.x, c1.y
					    texkill r1
					    mul_pp r0.w, r0.w, c1.z
					    dp3_pp r1.x, t1, c0
					    mov r2.w, c1.w
					    texldp_pp r3, t0, s1
					    mov_pp r2.xyz, r3
					    mul r1.y, r2.x, r2.x
					    mov r0.xyz, c1.w
					    cmp_pp r0, -r1.x, r2, r0
					    cmp_pp r0, -r1.y, r0, r2
					    mov_pp oC0, r0
					
					// approximately 14 instruction slots used (2 texture, 12 arithmetic)"
}
SubProgram "d3d11 " {
"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[14];
						vec4 _cameraPos;
						vec4 unused_0_2;
					};
					uniform  sampler2D _ShadowTex;
					uniform  sampler2D _Mask;
					in  vec4 vs_TEXCOORD0;
					in  vec3 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					float u_xlat1;
					vec4 u_xlat10_1;
					bool u_xlatb1;
					vec4 u_xlat2;
					bool u_xlatb4;
					bool u_xlatb6;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy / vs_TEXCOORD0.ww;
					    u_xlat10_1 = texture(_Mask, u_xlat0.xy);
					    u_xlatb6 = u_xlat10_1.w==0.0;
					    if(((int(u_xlatb6) * int(0xffffffffu)))!=0){discard;}
					    u_xlat0 = texture(_ShadowTex, u_xlat0.xy);
					    u_xlat1 = dot(vs_TEXCOORD1.xyz, _cameraPos.xyz);
					    u_xlatb4 = u_xlat0.x==0.0;
					    u_xlatb1 = 0.0<u_xlat1;
					    u_xlat2.w = u_xlat10_1.w * 0.5;
					    u_xlat2.x = float(0.0);
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat0.w = 0.0;
					    u_xlat2 = (bool(u_xlatb1)) ? u_xlat2 : u_xlat0;
					    SV_Target0 = (bool(u_xlatb4)) ? u_xlat2 : u_xlat0;
					    return;
					}"
}
SubProgram "d3d11_9x " {
"ps_4_0_level_9_1
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[14];
						vec4 _cameraPos;
						vec4 unused_0_2;
					};
					uniform  sampler2D _ShadowTex;
					uniform  sampler2D _Mask;
					in  vec4 vs_TEXCOORD0;
					in  vec3 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					float u_xlat1;
					vec4 u_xlat10_1;
					bool u_xlatb1;
					vec4 u_xlat2;
					bool u_xlatb4;
					bool u_xlatb6;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy / vs_TEXCOORD0.ww;
					    u_xlat10_1 = texture(_Mask, u_xlat0.xy);
					    u_xlatb6 = u_xlat10_1.w==0.0;
					    if(((int(u_xlatb6) * int(0xffffffffu)))!=0){discard;}
					    u_xlat0 = texture(_ShadowTex, u_xlat0.xy);
					    u_xlat1 = dot(vs_TEXCOORD1.xyz, _cameraPos.xyz);
					    u_xlatb4 = u_xlat0.x==0.0;
					    u_xlatb1 = 0.0<u_xlat1;
					    u_xlat2.w = u_xlat10_1.w * 0.5;
					    u_xlat2.x = float(0.0);
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat0.w = 0.0;
					    u_xlat2 = (bool(u_xlatb1)) ? u_xlat2 : u_xlat0;
					    SV_Target0 = (bool(u_xlatb4)) ? u_xlat2 : u_xlat0;
					    return;
					}"
}
}
 }
}
}