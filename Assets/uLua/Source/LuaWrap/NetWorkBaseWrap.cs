using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class NetWorkBaseWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Initialize", Initialize),
			new LuaMethod("RegisterMsg", RegisterMsg),
			new LuaMethod("UnRegisterMsg", UnRegisterMsg),
			new LuaMethod("SendMsg", SendMsg),
			new LuaMethod("Uninitialize", Uninitialize),
			new LuaMethod("New", _CreateNetWorkBase),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "NetWorkBase", typeof(NetWorkBase), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateNetWorkBase(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			NetWorkBase obj = new NetWorkBase();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: NetWorkBase.New");
		}

		return 0;
	}

	static Type classType = typeof(NetWorkBase);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Initialize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		NetWorkBase obj = (NetWorkBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "NetWorkBase");
		obj.Initialize();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RegisterMsg(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		NetWorkBase obj = (NetWorkBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "NetWorkBase");
		obj.RegisterMsg();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnRegisterMsg(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		NetWorkBase obj = (NetWorkBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "NetWorkBase");
		obj.UnRegisterMsg();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SendMsg(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(NetWorkBase), typeof(ushort), typeof(byte[])))
		{
			NetWorkBase obj = (NetWorkBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "NetWorkBase");
			ushort arg0 = (ushort)LuaDLL.lua_tonumber(L, 2);
			byte[] objs1 = LuaScriptMgr.GetArrayNumber<byte>(L, 3);
			obj.SendMsg(arg0,objs1);
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(NetWorkBase), typeof(Net.StructCmd), typeof(bool)))
		{
			NetWorkBase obj = (NetWorkBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "NetWorkBase");
			Net.StructCmd arg0 = (Net.StructCmd)LuaScriptMgr.GetLuaObject(L, 2);
			bool arg1 = LuaDLL.lua_toboolean(L, 3);
			obj.SendMsg(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: NetWorkBase.SendMsg");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Uninitialize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		NetWorkBase obj = (NetWorkBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "NetWorkBase");
		obj.Uninitialize();
		return 0;
	}
}

