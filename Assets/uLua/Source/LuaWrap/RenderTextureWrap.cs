﻿using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class RenderTextureWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("GetTemporary", GetTemporary),
			new LuaMethod("ReleaseTemporary", ReleaseTemporary),
			new LuaMethod("Create", Create),
			new LuaMethod("Release", Release),
			new LuaMethod("IsCreated", IsCreated),
			new LuaMethod("DiscardContents", DiscardContents),
			new LuaMethod("MarkRestoreExpected", MarkRestoreExpected),
			new LuaMethod("SetGlobalShaderProperty", SetGlobalShaderProperty),
			new LuaMethod("GetTexelOffset", GetTexelOffset),
			new LuaMethod("SupportsStencil", SupportsStencil),
			new LuaMethod("New", _CreateRenderTexture),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("width", get_width, set_width),
			new LuaField("height", get_height, set_height),
			new LuaField("depth", get_depth, set_depth),
			new LuaField("isPowerOfTwo", get_isPowerOfTwo, set_isPowerOfTwo),
			new LuaField("sRGB", get_sRGB, null),
			new LuaField("format", get_format, set_format),
			new LuaField("useMipMap", get_useMipMap, set_useMipMap),
			new LuaField("generateMips", get_generateMips, set_generateMips),
			new LuaField("isCubemap", get_isCubemap, set_isCubemap),
			new LuaField("isVolume", get_isVolume, set_isVolume),
			new LuaField("volumeDepth", get_volumeDepth, set_volumeDepth),
			new LuaField("antiAliasing", get_antiAliasing, set_antiAliasing),
			new LuaField("enableRandomWrite", get_enableRandomWrite, set_enableRandomWrite),
			new LuaField("colorBuffer", get_colorBuffer, null),
			new LuaField("depthBuffer", get_depthBuffer, null),
			new LuaField("active", get_active, set_active),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.RenderTexture", typeof(RenderTexture), regs, fields, typeof(Texture));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateRenderTexture(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
			RenderTexture obj = new RenderTexture(arg0,arg1,arg2);
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else if (count == 4)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
			RenderTextureFormat arg3 = (RenderTextureFormat)LuaScriptMgr.GetNetObject(L, 4, typeof(RenderTextureFormat));
			RenderTexture obj = new RenderTexture(arg0,arg1,arg2,arg3);
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else if (count == 5)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
			RenderTextureFormat arg3 = (RenderTextureFormat)LuaScriptMgr.GetNetObject(L, 4, typeof(RenderTextureFormat));
			RenderTextureReadWrite arg4 = (RenderTextureReadWrite)LuaScriptMgr.GetNetObject(L, 5, typeof(RenderTextureReadWrite));
			RenderTexture obj = new RenderTexture(arg0,arg1,arg2,arg3,arg4);
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: RenderTexture.New");
		}

		return 0;
	}

	static Type classType = typeof(RenderTexture);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_width(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name width");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index width on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.width);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_height(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name height");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index height on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.height);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_depth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name depth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index depth on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.depth);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isPowerOfTwo(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isPowerOfTwo");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isPowerOfTwo on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.isPowerOfTwo);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sRGB(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sRGB");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sRGB on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.sRGB);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_format(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

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
	static int get_useMipMap(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name useMipMap");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index useMipMap on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.useMipMap);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_generateMips(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name generateMips");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index generateMips on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.generateMips);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isCubemap(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isCubemap");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isCubemap on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.isCubemap);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isVolume(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isVolume");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isVolume on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.isVolume);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_volumeDepth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name volumeDepth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index volumeDepth on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.volumeDepth);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_antiAliasing(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name antiAliasing");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index antiAliasing on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.antiAliasing);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_enableRandomWrite(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name enableRandomWrite");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index enableRandomWrite on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.enableRandomWrite);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_colorBuffer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name colorBuffer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index colorBuffer on a nil value");
			}
		}

		LuaScriptMgr.PushValue(L, obj.colorBuffer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_depthBuffer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name depthBuffer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index depthBuffer on a nil value");
			}
		}

		LuaScriptMgr.PushValue(L, obj.depthBuffer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_active(IntPtr L)
	{
		LuaScriptMgr.Push(L, RenderTexture.active);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_width(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name width");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index width on a nil value");
			}
		}

		obj.width = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_height(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name height");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index height on a nil value");
			}
		}

		obj.height = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_depth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name depth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index depth on a nil value");
			}
		}

		obj.depth = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isPowerOfTwo(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isPowerOfTwo");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isPowerOfTwo on a nil value");
			}
		}

		obj.isPowerOfTwo = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_format(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

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

		obj.format = (RenderTextureFormat)LuaScriptMgr.GetNetObject(L, 3, typeof(RenderTextureFormat));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_useMipMap(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name useMipMap");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index useMipMap on a nil value");
			}
		}

		obj.useMipMap = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_generateMips(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name generateMips");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index generateMips on a nil value");
			}
		}

		obj.generateMips = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isCubemap(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isCubemap");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isCubemap on a nil value");
			}
		}

		obj.isCubemap = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isVolume(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isVolume");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isVolume on a nil value");
			}
		}

		obj.isVolume = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_volumeDepth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name volumeDepth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index volumeDepth on a nil value");
			}
		}

		obj.volumeDepth = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_antiAliasing(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name antiAliasing");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index antiAliasing on a nil value");
			}
		}

		obj.antiAliasing = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_enableRandomWrite(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTexture obj = (RenderTexture)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name enableRandomWrite");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index enableRandomWrite on a nil value");
			}
		}

		obj.enableRandomWrite = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_active(IntPtr L)
	{
		RenderTexture.active = (RenderTexture)LuaScriptMgr.GetUnityObject(L, 3, typeof(RenderTexture));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTemporary(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			RenderTexture o = RenderTexture.GetTemporary(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
			RenderTexture o = RenderTexture.GetTemporary(arg0,arg1,arg2);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 4)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
			RenderTextureFormat arg3 = (RenderTextureFormat)LuaScriptMgr.GetNetObject(L, 4, typeof(RenderTextureFormat));
			RenderTexture o = RenderTexture.GetTemporary(arg0,arg1,arg2,arg3);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 5)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
			RenderTextureFormat arg3 = (RenderTextureFormat)LuaScriptMgr.GetNetObject(L, 4, typeof(RenderTextureFormat));
			RenderTextureReadWrite arg4 = (RenderTextureReadWrite)LuaScriptMgr.GetNetObject(L, 5, typeof(RenderTextureReadWrite));
			RenderTexture o = RenderTexture.GetTemporary(arg0,arg1,arg2,arg3,arg4);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 6)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
			RenderTextureFormat arg3 = (RenderTextureFormat)LuaScriptMgr.GetNetObject(L, 4, typeof(RenderTextureFormat));
			RenderTextureReadWrite arg4 = (RenderTextureReadWrite)LuaScriptMgr.GetNetObject(L, 5, typeof(RenderTextureReadWrite));
			int arg5 = (int)LuaScriptMgr.GetNumber(L, 6);
			RenderTexture o = RenderTexture.GetTemporary(arg0,arg1,arg2,arg3,arg4,arg5);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: RenderTexture.GetTemporary");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReleaseTemporary(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RenderTexture arg0 = (RenderTexture)LuaScriptMgr.GetUnityObject(L, 1, typeof(RenderTexture));
		RenderTexture.ReleaseTemporary(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Create(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RenderTexture obj = (RenderTexture)LuaScriptMgr.GetUnityObjectSelf(L, 1, "RenderTexture");
		bool o = obj.Create();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Release(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RenderTexture obj = (RenderTexture)LuaScriptMgr.GetUnityObjectSelf(L, 1, "RenderTexture");
		obj.Release();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsCreated(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RenderTexture obj = (RenderTexture)LuaScriptMgr.GetUnityObjectSelf(L, 1, "RenderTexture");
		bool o = obj.IsCreated();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DiscardContents(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			RenderTexture obj = (RenderTexture)LuaScriptMgr.GetUnityObjectSelf(L, 1, "RenderTexture");
			obj.DiscardContents();
			return 0;
		}
		else if (count == 3)
		{
			RenderTexture obj = (RenderTexture)LuaScriptMgr.GetUnityObjectSelf(L, 1, "RenderTexture");
			bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
			bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
			obj.DiscardContents(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: RenderTexture.DiscardContents");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MarkRestoreExpected(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RenderTexture obj = (RenderTexture)LuaScriptMgr.GetUnityObjectSelf(L, 1, "RenderTexture");
		obj.MarkRestoreExpected();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetGlobalShaderProperty(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RenderTexture obj = (RenderTexture)LuaScriptMgr.GetUnityObjectSelf(L, 1, "RenderTexture");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.SetGlobalShaderProperty(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTexelOffset(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RenderTexture obj = (RenderTexture)LuaScriptMgr.GetUnityObjectSelf(L, 1, "RenderTexture");
		Vector2 o = obj.GetTexelOffset();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SupportsStencil(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RenderTexture arg0 = (RenderTexture)LuaScriptMgr.GetUnityObject(L, 1, typeof(RenderTexture));
		bool o = RenderTexture.SupportsStencil(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
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

