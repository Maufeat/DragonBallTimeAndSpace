﻿using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class IgnoreTimeScaleWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateIgnoreTimeScale),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("realTime", get_realTime, null),
			new LuaField("realTimeDelta", get_realTimeDelta, null),
		};

		LuaScriptMgr.RegisterLib(L, "IgnoreTimeScale", typeof(IgnoreTimeScale), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateIgnoreTimeScale(IntPtr L)
	{
		LuaDLL.luaL_error(L, "IgnoreTimeScale class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(IgnoreTimeScale);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_realTime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		IgnoreTimeScale obj = (IgnoreTimeScale)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name realTime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index realTime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.realTime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_realTimeDelta(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		IgnoreTimeScale obj = (IgnoreTimeScale)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name realTimeDelta");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index realTimeDelta on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.realTimeDelta);
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

