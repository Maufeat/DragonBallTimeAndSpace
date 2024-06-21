using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class LuaNetWorkManagerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Initialize", Initialize),
			new LuaMethod("ResetCache", ResetCache),
			new LuaMethod("RegisterMsg", RegisterMsg),
			new LuaMethod("UnRegisterMsg", UnRegisterMsg),
			new LuaMethod("IsRegisterInLua", IsRegisterInLua),
			new LuaMethod("OnMessage", OnMessage),
			new LuaMethod("SendMsg", SendMsg),
			new LuaMethod("Uninitialize", Uninitialize),
			new LuaMethod("OnUpdate", OnUpdate),
			new LuaMethod("OnReSet", OnReSet),
			new LuaMethod("New", _CreateLuaNetWorkManager),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Instance", get_Instance, null),
			new LuaField("ManagerName", get_ManagerName, null),
		};

		LuaScriptMgr.RegisterLib(L, "LuaNetWorkManager", typeof(LuaNetWorkManager), regs, fields, typeof(NetWorkBase));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateLuaNetWorkManager(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			LuaNetWorkManager obj = new LuaNetWorkManager();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: LuaNetWorkManager.New");
		}

		return 0;
	}

	static Type classType = typeof(LuaNetWorkManager);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, LuaNetWorkManager.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ManagerName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaNetWorkManager obj = (LuaNetWorkManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ManagerName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ManagerName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ManagerName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Initialize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaNetWorkManager obj = (LuaNetWorkManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaNetWorkManager");
		obj.Initialize();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetCache(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaNetWorkManager obj = (LuaNetWorkManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaNetWorkManager");
		obj.ResetCache();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RegisterMsg(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaNetWorkManager obj = (LuaNetWorkManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaNetWorkManager");
		ushort arg0 = (ushort)LuaScriptMgr.GetNumber(L, 2);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 3);
		obj.RegisterMsg(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnRegisterMsg(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaNetWorkManager obj = (LuaNetWorkManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaNetWorkManager");
		ushort arg0 = (ushort)LuaScriptMgr.GetNumber(L, 2);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 3);
		obj.UnRegisterMsg(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsRegisterInLua(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaNetWorkManager obj = (LuaNetWorkManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaNetWorkManager");
		ushort arg0 = (ushort)LuaScriptMgr.GetNumber(L, 2);
		bool o = obj.IsRegisterInLua(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnMessage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaNetWorkManager obj = (LuaNetWorkManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaNetWorkManager");
		Net.NullCmd arg0 = (Net.NullCmd)LuaScriptMgr.GetNetObject(L, 2, typeof(Net.NullCmd));
		obj.OnMessage(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SendMsg(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaNetWorkManager obj = (LuaNetWorkManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaNetWorkManager");
		ushort arg0 = (ushort)LuaScriptMgr.GetNumber(L, 2);
		LuaStringBuffer arg1 = LuaScriptMgr.GetStringBuffer(L, 3);
		obj.SendMsg(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Uninitialize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaNetWorkManager obj = (LuaNetWorkManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaNetWorkManager");
		obj.Uninitialize();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnUpdate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaNetWorkManager obj = (LuaNetWorkManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaNetWorkManager");
		obj.OnUpdate();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnReSet(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaNetWorkManager obj = (LuaNetWorkManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaNetWorkManager");
		obj.OnReSet();
		return 0;
	}
}

