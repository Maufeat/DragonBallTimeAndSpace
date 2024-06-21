using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using LuaInterface;

public class RuneDataSetattributeWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("addBystring", addBystring),
			new LuaMethod("addByOtherRune", addByOtherRune),
			new LuaMethod("clear", clear),
			new LuaMethod("New", _CreateRuneDataSetattribute),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("list", get_list, set_list),
		};

		LuaScriptMgr.RegisterLib(L, "RuneDataSetattribute", typeof(RuneDataSetattribute), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateRuneDataSetattribute(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			RuneDataSetattribute obj = new RuneDataSetattribute();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: RuneDataSetattribute.New");
		}

		return 0;
	}

	static Type classType = typeof(RuneDataSetattribute);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_list(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RuneDataSetattribute obj = (RuneDataSetattribute)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name list");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index list on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.list);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_list(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RuneDataSetattribute obj = (RuneDataSetattribute)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name list");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index list on a nil value");
			}
		}

		obj.list = (List<uint>)LuaScriptMgr.GetNetObject(L, 3, typeof(List<uint>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int addBystring(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RuneDataSetattribute obj = (RuneDataSetattribute)LuaScriptMgr.GetNetObjectSelf(L, 1, "RuneDataSetattribute");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.addBystring(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int addByOtherRune(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RuneDataSetattribute obj = (RuneDataSetattribute)LuaScriptMgr.GetNetObjectSelf(L, 1, "RuneDataSetattribute");
		RuneDataSetattribute arg0 = (RuneDataSetattribute)LuaScriptMgr.GetNetObject(L, 2, typeof(RuneDataSetattribute));
		obj.addByOtherRune(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int clear(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RuneDataSetattribute obj = (RuneDataSetattribute)LuaScriptMgr.GetNetObjectSelf(L, 1, "RuneDataSetattribute");
		obj.clear();
		return 0;
	}
}

