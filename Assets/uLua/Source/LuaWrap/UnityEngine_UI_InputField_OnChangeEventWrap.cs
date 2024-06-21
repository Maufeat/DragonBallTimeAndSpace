using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LuaInterface;

public class UnityEngine_UI_InputField_OnChangeEventWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateUnityEngine_UI_InputField_OnChangeEvent),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.UI.InputField.OnChangeEvent", typeof(InputField.OnChangeEvent), regs, fields, typeof(UnityEngine.Events.UnityEvent<string>));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_InputField_OnChangeEvent(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			InputField.OnChangeEvent obj = new InputField.OnChangeEvent();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: InputField.OnChangeEvent.New");
		}

		return 0;
	}

	static Type classType = typeof(InputField.OnChangeEvent);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}
}

