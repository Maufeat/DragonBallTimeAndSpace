using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;

public class WayFindItemWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("TryInitWayNodeList", TryInitWayNodeList),
			new LuaMethod("TryInitNodeListByStream", TryInitNodeListByStream),
			new LuaMethod("InitUI", InitUI),
			new LuaMethod("New", _CreateWayFindItem),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("pathWayIds", get_pathWayIds, set_pathWayIds),
			new LuaField("wayNodeArray", get_wayNodeArray, set_wayNodeArray),
		};

		LuaScriptMgr.RegisterLib(L, "WayFindItem", typeof(WayFindItem), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateWayFindItem(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			string arg0 = LuaScriptMgr.GetString(L, 1);
			WayFindItem obj = new WayFindItem(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else if (count == 2)
		{
			string arg0 = LuaScriptMgr.GetString(L, 1);
			bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
			WayFindItem obj = new WayFindItem(arg0,arg1);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: WayFindItem.New");
		}

		return 0;
	}

	static Type classType = typeof(WayFindItem);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_pathWayIds(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayFindItem obj = (WayFindItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name pathWayIds");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index pathWayIds on a nil value");
			}
		}

		LuaScriptMgr.PushArray(L, obj.pathWayIds);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_wayNodeArray(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayFindItem obj = (WayFindItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name wayNodeArray");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index wayNodeArray on a nil value");
			}
		}

		LuaScriptMgr.PushArray(L, obj.wayNodeArray);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_pathWayIds(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayFindItem obj = (WayFindItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name pathWayIds");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index pathWayIds on a nil value");
			}
		}

		obj.pathWayIds = LuaScriptMgr.GetArrayNumber<uint>(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_wayNodeArray(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		WayFindItem obj = (WayFindItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name wayNodeArray");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index wayNodeArray on a nil value");
			}
		}

		obj.wayNodeArray = LuaScriptMgr.GetArrayObject<WayNode>(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TryInitWayNodeList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		WayFindItem obj = (WayFindItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "WayFindItem");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.TryInitWayNodeList(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TryInitNodeListByStream(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		WayFindItem obj = (WayFindItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "WayFindItem");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.TryInitNodeListByStream(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 7);
		WayFindItem obj = (WayFindItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "WayFindItem");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 3);
		int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
		string arg3 = LuaScriptMgr.GetLuaString(L, 5);
		uint arg4 = (uint)LuaScriptMgr.GetNumber(L, 6);
		uint arg5 = (uint)LuaScriptMgr.GetNumber(L, 7);
		obj.InitUI(arg0,arg1,arg2,arg3,arg4,arg5);
		return 0;
	}
}

