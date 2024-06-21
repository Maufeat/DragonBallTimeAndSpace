using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LuaInterface;

public class UnityEngine_UI_Toggle_ToggleEventWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateUnityEngine_UI_Toggle_ToggleEvent),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.UI.Toggle.ToggleEvent", typeof(Toggle.ToggleEvent), regs, fields, typeof(UnityEngine.Events.UnityEvent<bool>));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_Toggle_ToggleEvent(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Toggle.ToggleEvent obj = new Toggle.ToggleEvent();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Toggle.ToggleEvent.New");
		}

		return 0;
	}

	static Type classType = typeof(Toggle.ToggleEvent);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}
}

