using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class KeyWordFilterWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("InitFilter", InitFilter),
			new LuaMethod("TextFilter", TextFilter),
			new LuaMethod("ChatFilter", ChatFilter),
			new LuaMethod("New", _CreateKeyWordFilter),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "KeyWordFilter", typeof(KeyWordFilter), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateKeyWordFilter(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			KeyWordFilter obj = new KeyWordFilter();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: KeyWordFilter.New");
		}

		return 0;
	}

	static Type classType = typeof(KeyWordFilter);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitFilter(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		KeyWordFilter.InitFilter();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TextFilter(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = KeyWordFilter.TextFilter(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ChatFilter(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = KeyWordFilter.ChatFilter(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

