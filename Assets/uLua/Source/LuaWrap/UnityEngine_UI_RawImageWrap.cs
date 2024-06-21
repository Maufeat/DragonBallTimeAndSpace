using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UnityEngine_UI_RawImageWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("SetNativeSize", SetNativeSize),
			new LuaMethod("New", _CreateUnityEngine_UI_RawImage),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("mainTexture", get_mainTexture, null),
			new LuaField("texture", get_texture, set_texture),
			new LuaField("uvRect", get_uvRect, set_uvRect),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.UI.RawImage", typeof(RawImage), regs, fields, typeof(MaskableGraphic));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_RawImage(IntPtr L)
	{
		LuaDLL.luaL_error(L, "RawImage class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(RawImage);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_mainTexture(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RawImage obj = (RawImage)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mainTexture");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mainTexture on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.mainTexture);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_texture(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RawImage obj = (RawImage)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name texture");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index texture on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.texture);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_uvRect(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RawImage obj = (RawImage)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name uvRect");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index uvRect on a nil value");
			}
		}

		LuaScriptMgr.PushValue(L, obj.uvRect);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_texture(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RawImage obj = (RawImage)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name texture");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index texture on a nil value");
			}
		}

		obj.texture = (Texture)LuaScriptMgr.GetUnityObject(L, 3, typeof(Texture));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_uvRect(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RawImage obj = (RawImage)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name uvRect");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index uvRect on a nil value");
			}
		}

		obj.uvRect = (Rect)LuaScriptMgr.GetNetObject(L, 3, typeof(Rect));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetNativeSize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RawImage obj = (RawImage)LuaScriptMgr.GetUnityObjectSelf(L, 1, "RawImage");
		obj.SetNativeSize();
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

