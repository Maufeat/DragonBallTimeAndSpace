using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LuaInterface;

public class UnityEngine_UI_Slider_SliderEventWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateUnityEngine_UI_Slider_SliderEvent),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.UI.Slider.SliderEvent", typeof(Slider.SliderEvent), regs, fields, typeof(UnityEngine.Events.UnityEvent<float>));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_Slider_SliderEvent(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Slider.SliderEvent obj = new Slider.SliderEvent();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Slider.SliderEvent.New");
		}

		return 0;
	}

	static Type classType = typeof(Slider.SliderEvent);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}
}

