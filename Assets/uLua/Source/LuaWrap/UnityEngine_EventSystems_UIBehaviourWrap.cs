using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UnityEngine_EventSystems_UIBehaviourWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("IsActive", IsActive),
			new LuaMethod("IsDestroyed", IsDestroyed),
			new LuaMethod("New", _CreateUnityEngine_EventSystems_UIBehaviour),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.EventSystems.UIBehaviour", typeof(UIBehaviour), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_EventSystems_UIBehaviour(IntPtr L)
	{
		LuaDLL.luaL_error(L, "UIBehaviour class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(UIBehaviour);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsActive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIBehaviour obj = (UIBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBehaviour");
		bool o = obj.IsActive();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsDestroyed(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIBehaviour obj = (UIBehaviour)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBehaviour");
		bool o = obj.IsDestroyed();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Lua_Eq(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Object arg0 = LuaScriptMgr.GetLuaObject(L, 1) as Object;
		Object arg1 = LuaScriptMgr.GetLuaObject(L, 2) as Object;
		bool o = arg0 == arg1;
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

