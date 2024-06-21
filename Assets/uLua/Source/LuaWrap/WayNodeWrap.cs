using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using LuaInterface;

public class WayNodeWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("PaseData", PaseData),
			new LuaMethod("New", _CreateWayNode),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("text", get_text, set_text),
			new LuaField("color", get_color, set_color),
			new LuaField("wayNodeType", get_wayNodeType, set_wayNodeType),
			new LuaField("endPreFix", get_endPreFix, set_endPreFix),
			new LuaField("wayIdIndex", get_wayIdIndex, set_wayIdIndex),
		};

		LuaScriptMgr.RegisterLib(L, "WayNode", typeof(WayNode), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateWayNode(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			WayNode obj = new WayNode();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: WayNode.New");
		}

		return 0;
	}

	static Type classType = typeof(WayNode);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_text(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayNode obj = (WayNode)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name text");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index text on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.text);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_color(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayNode obj = (WayNode)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name color");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index color on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.color);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_wayNodeType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayNode obj = (WayNode)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name wayNodeType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index wayNodeType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.wayNodeType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_endPreFix(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayNode obj = (WayNode)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name endPreFix");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index endPreFix on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.endPreFix);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_wayIdIndex(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayNode obj = (WayNode)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name wayIdIndex");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index wayIdIndex on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.wayIdIndex);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_text(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayNode obj = (WayNode)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name text");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index text on a nil value");
			}
		}

		obj.text = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_color(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayNode obj = (WayNode)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name color");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index color on a nil value");
			}
		}

		obj.color = LuaScriptMgr.GetColor(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_wayNodeType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayNode obj = (WayNode)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name wayNodeType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index wayNodeType on a nil value");
			}
		}

		obj.wayNodeType = (WayNodeType)LuaScriptMgr.GetNetObject(L, 3, typeof(WayNodeType));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_endPreFix(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayNode obj = (WayNode)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name endPreFix");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index endPreFix on a nil value");
			}
		}

		obj.endPreFix = (Dictionary<uint,uint>)LuaScriptMgr.GetNetObject(L, 3, typeof(Dictionary<uint,uint>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_wayIdIndex(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayNode obj = (WayNode)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name wayIdIndex");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index wayIdIndex on a nil value");
			}
		}

		obj.wayIdIndex = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PaseData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		WayNode obj = (WayNode)LuaScriptMgr.GetNetObjectSelf(L, 1, "WayNode");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.PaseData(arg0);
		return 0;
	}
}

