using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class ManagerRegisterWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("GetEntitiesManager", GetEntitiesManager),
			new LuaMethod("New", _CreateManagerRegister),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaScriptMgr.RegisterLib(L, "ManagerRegister", regs);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateManagerRegister(IntPtr L)
	{
		LuaDLL.luaL_error(L, "ManagerRegister class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(ManagerRegister);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetEntitiesManager(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		EntitiesManager o = ManagerRegister.GetEntitiesManager();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}
}

