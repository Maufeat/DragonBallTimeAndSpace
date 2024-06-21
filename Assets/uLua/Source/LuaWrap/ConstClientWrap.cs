using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class ConstClientWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateConstClient),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("BuglyIOSAppID", get_BuglyIOSAppID, null),
			new LuaField("BuglyAndroidAppID", get_BuglyAndroidAppID, null),
			new LuaField("TargetFrameRate", get_TargetFrameRate, null),
			new LuaField("MaxLevelColor", get_MaxLevelColor, null),
		};

		LuaScriptMgr.RegisterLib(L, "ConstClient", typeof(ConstClient), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateConstClient(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			ConstClient obj = new ConstClient();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ConstClient.New");
		}

		return 0;
	}

	static Type classType = typeof(ConstClient);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BuglyIOSAppID(IntPtr L)
	{
		LuaScriptMgr.Push(L, ConstClient.BuglyIOSAppID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BuglyAndroidAppID(IntPtr L)
	{
		LuaScriptMgr.Push(L, ConstClient.BuglyAndroidAppID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TargetFrameRate(IntPtr L)
	{
		LuaScriptMgr.Push(L, ConstClient.TargetFrameRate);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MaxLevelColor(IntPtr L)
	{
		LuaScriptMgr.Push(L, ConstClient.MaxLevelColor);
		return 1;
	}
}

