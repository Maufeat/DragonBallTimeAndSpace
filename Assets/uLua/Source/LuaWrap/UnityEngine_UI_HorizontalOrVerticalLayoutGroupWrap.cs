using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UnityEngine_UI_HorizontalOrVerticalLayoutGroupWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateUnityEngine_UI_HorizontalOrVerticalLayoutGroup),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("spacing", get_spacing, set_spacing),
			new LuaField("childForceExpandWidth", get_childForceExpandWidth, set_childForceExpandWidth),
			new LuaField("childForceExpandHeight", get_childForceExpandHeight, set_childForceExpandHeight),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.UI.HorizontalOrVerticalLayoutGroup", typeof(HorizontalOrVerticalLayoutGroup), regs, fields, typeof(LayoutGroup));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_HorizontalOrVerticalLayoutGroup(IntPtr L)
	{
		LuaDLL.luaL_error(L, "HorizontalOrVerticalLayoutGroup class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(HorizontalOrVerticalLayoutGroup);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_spacing(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		HorizontalOrVerticalLayoutGroup obj = (HorizontalOrVerticalLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name spacing");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index spacing on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.spacing);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_childForceExpandWidth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		HorizontalOrVerticalLayoutGroup obj = (HorizontalOrVerticalLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name childForceExpandWidth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index childForceExpandWidth on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.childForceExpandWidth);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_childForceExpandHeight(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		HorizontalOrVerticalLayoutGroup obj = (HorizontalOrVerticalLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name childForceExpandHeight");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index childForceExpandHeight on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.childForceExpandHeight);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_spacing(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		HorizontalOrVerticalLayoutGroup obj = (HorizontalOrVerticalLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name spacing");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index spacing on a nil value");
			}
		}

		obj.spacing = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_childForceExpandWidth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		HorizontalOrVerticalLayoutGroup obj = (HorizontalOrVerticalLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name childForceExpandWidth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index childForceExpandWidth on a nil value");
			}
		}

		obj.childForceExpandWidth = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_childForceExpandHeight(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		HorizontalOrVerticalLayoutGroup obj = (HorizontalOrVerticalLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name childForceExpandHeight");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index childForceExpandHeight on a nil value");
			}
		}

		obj.childForceExpandHeight = LuaScriptMgr.GetBoolean(L, 3);
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

