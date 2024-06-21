using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class SystemHelperWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("ReadAllBytes", ReadAllBytes),
			new LuaMethod("New", _CreateSystemHelper),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaScriptMgr.RegisterLib(L, "SystemHelper", regs);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSystemHelper(IntPtr L)
	{
		LuaDLL.luaL_error(L, "SystemHelper class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(SystemHelper);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReadAllBytes(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaStringBuffer o = SystemHelper.ReadAllBytes(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

