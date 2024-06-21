using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UnityEngine_UI_ToggleGroupWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("NotifyToggleOn", NotifyToggleOn),
			new LuaMethod("UnregisterToggle", UnregisterToggle),
			new LuaMethod("RegisterToggle", RegisterToggle),
			new LuaMethod("AnyTogglesOn", AnyTogglesOn),
			new LuaMethod("ActiveToggles", ActiveToggles),
			new LuaMethod("SetAllTogglesOff", SetAllTogglesOff),
			new LuaMethod("New", _CreateUnityEngine_UI_ToggleGroup),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("allowSwitchOff", get_allowSwitchOff, set_allowSwitchOff),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.UI.ToggleGroup", typeof(ToggleGroup), regs, fields, typeof(UIBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_ToggleGroup(IntPtr L)
	{
		LuaDLL.luaL_error(L, "ToggleGroup class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(ToggleGroup);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_allowSwitchOff(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ToggleGroup obj = (ToggleGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name allowSwitchOff");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index allowSwitchOff on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.allowSwitchOff);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_allowSwitchOff(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ToggleGroup obj = (ToggleGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name allowSwitchOff");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index allowSwitchOff on a nil value");
			}
		}

		obj.allowSwitchOff = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int NotifyToggleOn(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ToggleGroup obj = (ToggleGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ToggleGroup");
		Toggle arg0 = (Toggle)LuaScriptMgr.GetUnityObject(L, 2, typeof(Toggle));
		obj.NotifyToggleOn(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnregisterToggle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ToggleGroup obj = (ToggleGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ToggleGroup");
		Toggle arg0 = (Toggle)LuaScriptMgr.GetUnityObject(L, 2, typeof(Toggle));
		obj.UnregisterToggle(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RegisterToggle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ToggleGroup obj = (ToggleGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ToggleGroup");
		Toggle arg0 = (Toggle)LuaScriptMgr.GetUnityObject(L, 2, typeof(Toggle));
		obj.RegisterToggle(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AnyTogglesOn(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ToggleGroup obj = (ToggleGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ToggleGroup");
		bool o = obj.AnyTogglesOn();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ActiveToggles(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ToggleGroup obj = (ToggleGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ToggleGroup");
		IEnumerable<Toggle> o = obj.ActiveToggles();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAllTogglesOff(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ToggleGroup obj = (ToggleGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ToggleGroup");
		obj.SetAllTogglesOff();
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

