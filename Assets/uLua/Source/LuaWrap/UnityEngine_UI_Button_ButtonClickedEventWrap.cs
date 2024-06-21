using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LuaInterface;

public class UnityEngine_UI_Button_ButtonClickedEventWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateUnityEngine_UI_Button_ButtonClickedEvent),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.UI.Button.ButtonClickedEvent", typeof(Button.ButtonClickedEvent), regs, fields, typeof(UnityEngine.Events.UnityEvent));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_Button_ButtonClickedEvent(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Button.ButtonClickedEvent obj = new Button.ButtonClickedEvent();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Button.ButtonClickedEvent.New");
		}

		return 0;
	}

	static Type classType = typeof(Button.ButtonClickedEvent);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}
}

