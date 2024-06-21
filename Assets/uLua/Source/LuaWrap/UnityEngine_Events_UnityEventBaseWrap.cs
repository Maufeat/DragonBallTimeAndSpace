using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UnityEngine_Events_UnityEventBaseWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("GetPersistentEventCount", GetPersistentEventCount),
			new LuaMethod("GetPersistentTarget", GetPersistentTarget),
			new LuaMethod("GetPersistentMethodName", GetPersistentMethodName),
			new LuaMethod("SetPersistentListenerState", SetPersistentListenerState),
			new LuaMethod("RemoveAllListeners", RemoveAllListeners),
			new LuaMethod("ToString", ToString),
			new LuaMethod("GetValidMethodInfo", GetValidMethodInfo),
			new LuaMethod("New", _CreateUnityEngine_Events_UnityEventBase),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__tostring", Lua_ToString),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.Events.UnityEventBase", typeof(UnityEngine.Events.UnityEventBase), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_Events_UnityEventBase(IntPtr L)
	{
		LuaDLL.luaL_error(L, "UnityEngine.Events.UnityEventBase class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(UnityEngine.Events.UnityEventBase);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Lua_ToString(IntPtr L)
	{
		object obj = LuaScriptMgr.GetLuaObject(L, 1);

		if (obj != null)
		{
			LuaScriptMgr.Push(L, obj.ToString());
		}
		else
		{
			LuaScriptMgr.Push(L, "Table: UnityEngine.Events.UnityEventBase");
		}

		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPersistentEventCount(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UnityEngine.Events.UnityEventBase obj = (UnityEngine.Events.UnityEventBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "UnityEngine.Events.UnityEventBase");
		int o = obj.GetPersistentEventCount();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPersistentTarget(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UnityEngine.Events.UnityEventBase obj = (UnityEngine.Events.UnityEventBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "UnityEngine.Events.UnityEventBase");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		Object o = obj.GetPersistentTarget(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPersistentMethodName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UnityEngine.Events.UnityEventBase obj = (UnityEngine.Events.UnityEventBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "UnityEngine.Events.UnityEventBase");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		string o = obj.GetPersistentMethodName(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPersistentListenerState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		UnityEngine.Events.UnityEventBase obj = (UnityEngine.Events.UnityEventBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "UnityEngine.Events.UnityEventBase");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		UnityEngine.Events.UnityEventCallState arg1 = (UnityEngine.Events.UnityEventCallState)LuaScriptMgr.GetNetObject(L, 3, typeof(UnityEngine.Events.UnityEventCallState));
		obj.SetPersistentListenerState(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveAllListeners(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UnityEngine.Events.UnityEventBase obj = (UnityEngine.Events.UnityEventBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "UnityEngine.Events.UnityEventBase");
		obj.RemoveAllListeners();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UnityEngine.Events.UnityEventBase obj = (UnityEngine.Events.UnityEventBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "UnityEngine.Events.UnityEventBase");
		string o = obj.ToString();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetValidMethodInfo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		object arg0 = LuaScriptMgr.GetVarObject(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		Type[] objs2 = LuaScriptMgr.GetArrayObject<Type>(L, 3);
		System.Reflection.MethodInfo o = UnityEngine.Events.UnityEventBase.GetValidMethodInfo(arg0,arg1,objs2);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}
}

