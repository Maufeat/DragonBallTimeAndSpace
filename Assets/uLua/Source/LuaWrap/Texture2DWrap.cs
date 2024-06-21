﻿using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class Texture2DWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("CreateExternalTexture", CreateExternalTexture),
			new LuaMethod("UpdateExternalTexture", UpdateExternalTexture),
			new LuaMethod("SetPixel", SetPixel),
			new LuaMethod("GetPixel", GetPixel),
			new LuaMethod("GetPixelBilinear", GetPixelBilinear),
			new LuaMethod("SetPixels", SetPixels),
			new LuaMethod("SetPixels32", SetPixels32),
			new LuaMethod("LoadImage", LoadImage),
			new LuaMethod("LoadRawTextureData", LoadRawTextureData),
			new LuaMethod("GetRawTextureData", GetRawTextureData),
			new LuaMethod("GetPixels", GetPixels),
			new LuaMethod("GetPixels32", GetPixels32),
			new LuaMethod("Apply", Apply),
			new LuaMethod("Resize", Resize),
			new LuaMethod("Compress", Compress),
			new LuaMethod("PackTextures", PackTextures),
			new LuaMethod("ReadPixels", ReadPixels),
			new LuaMethod("EncodeToPNG", EncodeToPNG),
			new LuaMethod("EncodeToJPG", EncodeToJPG),
			new LuaMethod("New", _CreateTexture2D),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("mipmapCount", get_mipmapCount, null),
			new LuaField("format", get_format, null),
			new LuaField("whiteTexture", get_whiteTexture, null),
			new LuaField("blackTexture", get_blackTexture, null),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.Texture2D", typeof(Texture2D), regs, fields, typeof(Texture));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateTexture2D(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			Texture2D obj = new Texture2D(arg0,arg1);
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else if (count == 4)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			TextureFormat arg2 = (TextureFormat)LuaScriptMgr.GetNetObject(L, 3, typeof(TextureFormat));
			bool arg3 = LuaScriptMgr.GetBoolean(L, 4);
			Texture2D obj = new Texture2D(arg0,arg1,arg2,arg3);
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else if (count == 5)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			TextureFormat arg2 = (TextureFormat)LuaScriptMgr.GetNetObject(L, 3, typeof(TextureFormat));
			bool arg3 = LuaScriptMgr.GetBoolean(L, 4);
			bool arg4 = LuaScriptMgr.GetBoolean(L, 5);
			Texture2D obj = new Texture2D(arg0,arg1,arg2,arg3,arg4);
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Texture2D.New");
		}

		return 0;
	}

	static Type classType = typeof(Texture2D);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_mipmapCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Texture2D obj = (Texture2D)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mipmapCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mipmapCount on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.mipmapCount);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_format(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Texture2D obj = (Texture2D)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name format");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index format on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.format);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_whiteTexture(IntPtr L)
	{
		LuaScriptMgr.Push(L, Texture2D.whiteTexture);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_blackTexture(IntPtr L)
	{
		LuaScriptMgr.Push(L, Texture2D.blackTexture);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreateExternalTexture(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 6);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		TextureFormat arg2 = (TextureFormat)LuaScriptMgr.GetNetObject(L, 3, typeof(TextureFormat));
		bool arg3 = LuaScriptMgr.GetBoolean(L, 4);
		bool arg4 = LuaScriptMgr.GetBoolean(L, 5);
		IntPtr arg5 = (IntPtr)LuaScriptMgr.GetNumber(L, 6);
		Texture2D o = Texture2D.CreateExternalTexture(arg0,arg1,arg2,arg3,arg4,arg5);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UpdateExternalTexture(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
		IntPtr arg0 = (IntPtr)LuaScriptMgr.GetNumber(L, 2);
		obj.UpdateExternalTexture(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPixel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		Color arg2 = LuaScriptMgr.GetColor(L, 4);
		obj.SetPixel(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPixel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		Color o = obj.GetPixel(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPixelBilinear(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		Color o = obj.GetPixelBilinear(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPixels(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			Color[] objs0 = LuaScriptMgr.GetArrayObject<Color>(L, 2);
			obj.SetPixels(objs0);
			return 0;
		}
		else if (count == 3)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			Color[] objs0 = LuaScriptMgr.GetArrayObject<Color>(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			obj.SetPixels(objs0,arg1);
			return 0;
		}
		else if (count == 6)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			int arg3 = (int)LuaScriptMgr.GetNumber(L, 5);
			Color[] objs4 = LuaScriptMgr.GetArrayObject<Color>(L, 6);
			obj.SetPixels(arg0,arg1,arg2,arg3,objs4);
			return 0;
		}
		else if (count == 7)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			int arg3 = (int)LuaScriptMgr.GetNumber(L, 5);
			Color[] objs4 = LuaScriptMgr.GetArrayObject<Color>(L, 6);
			int arg5 = (int)LuaScriptMgr.GetNumber(L, 7);
			obj.SetPixels(arg0,arg1,arg2,arg3,objs4,arg5);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Texture2D.SetPixels");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPixels32(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			Color32[] objs0 = LuaScriptMgr.GetArrayObject<Color32>(L, 2);
			obj.SetPixels32(objs0);
			return 0;
		}
		else if (count == 3)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			Color32[] objs0 = LuaScriptMgr.GetArrayObject<Color32>(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			obj.SetPixels32(objs0,arg1);
			return 0;
		}
		else if (count == 6)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			int arg3 = (int)LuaScriptMgr.GetNumber(L, 5);
			Color32[] objs4 = LuaScriptMgr.GetArrayObject<Color32>(L, 6);
			obj.SetPixels32(arg0,arg1,arg2,arg3,objs4);
			return 0;
		}
		else if (count == 7)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			int arg3 = (int)LuaScriptMgr.GetNumber(L, 5);
			Color32[] objs4 = LuaScriptMgr.GetArrayObject<Color32>(L, 6);
			int arg5 = (int)LuaScriptMgr.GetNumber(L, 7);
			obj.SetPixels32(arg0,arg1,arg2,arg3,objs4,arg5);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Texture2D.SetPixels32");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadImage(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			byte[] objs0 = LuaScriptMgr.GetArrayNumber<byte>(L, 2);
			bool o = obj.LoadImage(objs0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			byte[] objs0 = LuaScriptMgr.GetArrayNumber<byte>(L, 2);
			bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
			bool o = obj.LoadImage(objs0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Texture2D.LoadImage");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadRawTextureData(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			byte[] objs0 = LuaScriptMgr.GetArrayNumber<byte>(L, 2);
			obj.LoadRawTextureData(objs0);
			return 0;
		}
		else if (count == 3)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			IntPtr arg0 = (IntPtr)LuaScriptMgr.GetNumber(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			obj.LoadRawTextureData(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Texture2D.LoadRawTextureData");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetRawTextureData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
		byte[] o = obj.GetRawTextureData();
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPixels(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			Color[] o = obj.GetPixels();
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 2)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			Color[] o = obj.GetPixels(arg0);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 5)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			int arg3 = (int)LuaScriptMgr.GetNumber(L, 5);
			Color[] o = obj.GetPixels(arg0,arg1,arg2,arg3);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 6)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			int arg3 = (int)LuaScriptMgr.GetNumber(L, 5);
			int arg4 = (int)LuaScriptMgr.GetNumber(L, 6);
			Color[] o = obj.GetPixels(arg0,arg1,arg2,arg3,arg4);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Texture2D.GetPixels");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPixels32(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			Color32[] o = obj.GetPixels32();
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 2)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			Color32[] o = obj.GetPixels32(arg0);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Texture2D.GetPixels32");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Apply(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			obj.Apply();
			return 0;
		}
		else if (count == 2)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
			obj.Apply(arg0);
			return 0;
		}
		else if (count == 3)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
			bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
			obj.Apply(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Texture2D.Apply");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Resize(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			bool o = obj.Resize(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 5)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			TextureFormat arg2 = (TextureFormat)LuaScriptMgr.GetNetObject(L, 4, typeof(TextureFormat));
			bool arg3 = LuaScriptMgr.GetBoolean(L, 5);
			bool o = obj.Resize(arg0,arg1,arg2,arg3);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Texture2D.Resize");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Compress(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.Compress(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PackTextures(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			Texture2D[] objs0 = LuaScriptMgr.GetArrayObject<Texture2D>(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			Rect[] o = obj.PackTextures(objs0,arg1);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 4)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			Texture2D[] objs0 = LuaScriptMgr.GetArrayObject<Texture2D>(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			Rect[] o = obj.PackTextures(objs0,arg1,arg2);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 5)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			Texture2D[] objs0 = LuaScriptMgr.GetArrayObject<Texture2D>(L, 2);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			bool arg3 = LuaScriptMgr.GetBoolean(L, 5);
			Rect[] o = obj.PackTextures(objs0,arg1,arg2,arg3);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Texture2D.PackTextures");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReadPixels(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 4)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			Rect arg0 = (Rect)LuaScriptMgr.GetNetObject(L, 2, typeof(Rect));
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			obj.ReadPixels(arg0,arg1,arg2);
			return 0;
		}
		else if (count == 5)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			Rect arg0 = (Rect)LuaScriptMgr.GetNetObject(L, 2, typeof(Rect));
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			bool arg3 = LuaScriptMgr.GetBoolean(L, 5);
			obj.ReadPixels(arg0,arg1,arg2,arg3);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Texture2D.ReadPixels");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EncodeToPNG(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
		byte[] o = obj.EncodeToPNG();
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EncodeToJPG(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			byte[] o = obj.EncodeToJPG();
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 2)
		{
			Texture2D obj = (Texture2D)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Texture2D");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			byte[] o = obj.EncodeToJPG(arg0);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Texture2D.EncodeToJPG");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Lua_Eq(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Object arg0 = LuaScriptMgr.GetLuaObject(L, 1) as Object;
		Object arg1 = LuaScriptMgr.GetLuaObject(L, 2) as Object;
		bool o = arg0 == arg1;
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

