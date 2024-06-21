using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class CanvasGroupWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("IsRaycastLocationValid", IsRaycastLocationValid),
			new LuaMethod("New", _CreateCanvasGroup),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("alpha", get_alpha, set_alpha),
			new LuaField("interactable", get_interactable, set_interactable),
			new LuaField("blocksRaycasts", get_blocksRaycasts, set_blocksRaycasts),
			new LuaField("ignoreParentGroups", get_ignoreParentGroups, set_ignoreParentGroups),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.CanvasGroup", typeof(CanvasGroup), regs, fields, typeof(Component));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateCanvasGroup(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			CanvasGroup obj = new CanvasGroup();
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: CanvasGroup.New");
		}

		return 0;
	}

	static Type classType = typeof(CanvasGroup);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_alpha(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CanvasGroup obj = (CanvasGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name alpha");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index alpha on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.alpha);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_interactable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CanvasGroup obj = (CanvasGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name interactable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index interactable on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.interactable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_blocksRaycasts(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CanvasGroup obj = (CanvasGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name blocksRaycasts");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index blocksRaycasts on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.blocksRaycasts);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ignoreParentGroups(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CanvasGroup obj = (CanvasGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ignoreParentGroups");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ignoreParentGroups on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ignoreParentGroups);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_alpha(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CanvasGroup obj = (CanvasGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name alpha");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index alpha on a nil value");
			}
		}

		obj.alpha = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_interactable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CanvasGroup obj = (CanvasGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name interactable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index interactable on a nil value");
			}
		}

		obj.interactable = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_blocksRaycasts(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CanvasGroup obj = (CanvasGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name blocksRaycasts");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index blocksRaycasts on a nil value");
			}
		}

		obj.blocksRaycasts = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ignoreParentGroups(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CanvasGroup obj = (CanvasGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ignoreParentGroups");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ignoreParentGroups on a nil value");
			}
		}

		obj.ignoreParentGroups = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsRaycastLocationValid(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		CanvasGroup obj = (CanvasGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "CanvasGroup");
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
		Camera arg1 = (Camera)LuaScriptMgr.GetUnityObject(L, 3, typeof(Camera));
		bool o = obj.IsRaycastLocationValid(arg0,arg1);
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

