using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LuaInterface;

public class UnityEngine_UI_Scrollbar_ScrollEventWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateUnityEngine_UI_Scrollbar_ScrollEvent),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.UI.Scrollbar.ScrollEvent", typeof(Scrollbar.ScrollEvent), regs, fields, typeof(UnityEngine.Events.UnityEvent<float>));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_Scrollbar_ScrollEvent(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Scrollbar.ScrollEvent obj = new Scrollbar.ScrollEvent();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Scrollbar.ScrollEvent.New");
		}

		return 0;
	}

	static Type classType = typeof(Scrollbar.ScrollEvent);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}
}

