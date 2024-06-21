using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using LuaInterface;

public class LuaConfigManagerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("StartLoadLuaConfig", StartLoadLuaConfig),
			new LuaMethod("LoadLuaConfigComplete", LoadLuaConfigComplete),
			new LuaMethod("GetConfig", GetConfig),
			new LuaMethod("ForceLoadExcelConfig", ForceLoadExcelConfig),
			new LuaMethod("GetConfigTable", GetConfigTable),
			new LuaMethod("GetConfigTableList", GetConfigTableList),
			new LuaMethod("GetXmlConfigTable", GetXmlConfigTable),
			new LuaMethod("New", _CreateLuaConfigManager),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "LuaConfigManager", typeof(LuaConfigManager), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateLuaConfigManager(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			LuaConfigManager obj = new LuaConfigManager();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: LuaConfigManager.New");
		}

		return 0;
	}

	static Type classType = typeof(LuaConfigManager);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StartLoadLuaConfig(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Action arg0 = null;
		LuaTypes funcType1 = LuaDLL.lua_type(L, 1);

		if (funcType1 != LuaTypes.LUA_TFUNCTION)
		{
			 arg0 = (Action)LuaScriptMgr.GetNetObject(L, 1, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 1);
			arg0 = () =>
			{
				func.Call();
			};
		}

		LuaConfigManager.StartLoadLuaConfig(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadLuaConfigComplete(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		LuaConfigManager.LoadLuaConfigComplete();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetConfig(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaInterface.LuaTable o = LuaConfigManager.GetConfig(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ForceLoadExcelConfig(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaConfigManager.ForceLoadExcelConfig(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetConfigTable(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		ulong arg1 = (ulong)LuaScriptMgr.GetNumber(L, 2);
		LuaInterface.LuaTable o = LuaConfigManager.GetConfigTable(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetConfigTableList(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			List<LuaInterface.LuaTable> o = LuaConfigManager.GetConfigTableList(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 2)
		{
			LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
			string arg1 = LuaScriptMgr.GetLuaString(L, 2);
			List<LuaInterface.LuaTable> o = LuaConfigManager.GetConfigTableList(arg0,arg1);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: LuaConfigManager.GetConfigTableList");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetXmlConfigTable(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaInterface.LuaTable o = LuaConfigManager.GetXmlConfigTable(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

