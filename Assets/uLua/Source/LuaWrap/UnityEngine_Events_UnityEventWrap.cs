using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class UnityEngine_Events_UnityEventWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("AddListener", AddListener),
			new LuaMethod("RemoveListener", RemoveListener),
			new LuaMethod("Invoke", Invoke),
			new LuaMethod("New", _CreateUnityEngine_Events_UnityEvent),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.Events.UnityEvent", typeof(UnityEngine.Events.UnityEvent), regs, fields, typeof(UnityEngine.Events.UnityEventBase));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_Events_UnityEvent(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			UnityEngine.Events.UnityEvent obj = new UnityEngine.Events.UnityEvent();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UnityEngine.Events.UnityEvent.New");
		}

		return 0;
	}

	static Type classType = typeof(UnityEngine.Events.UnityEvent);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddListener(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UnityEngine.Events.UnityEvent obj = (UnityEngine.Events.UnityEvent)LuaScriptMgr.GetNetObjectSelf(L, 1, "UnityEngine.Events.UnityEvent");
		UnityEngine.Events.UnityAction arg0 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg0 = (UnityEngine.Events.UnityAction)LuaScriptMgr.GetNetObject(L, 2, typeof(UnityEngine.Events.UnityAction));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
			arg0 = () =>
			{
				func.Call();
			};
		}

		obj.AddListener(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveListener(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UnityEngine.Events.UnityEvent obj = (UnityEngine.Events.UnityEvent)LuaScriptMgr.GetNetObjectSelf(L, 1, "UnityEngine.Events.UnityEvent");
		UnityEngine.Events.UnityAction arg0 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg0 = (UnityEngine.Events.UnityAction)LuaScriptMgr.GetNetObject(L, 2, typeof(UnityEngine.Events.UnityAction));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
			arg0 = () =>
			{
				func.Call();
			};
		}

		obj.RemoveListener(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Invoke(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UnityEngine.Events.UnityEvent obj = (UnityEngine.Events.UnityEvent)LuaScriptMgr.GetNetObjectSelf(L, 1, "UnityEngine.Events.UnityEvent");
		obj.Invoke();
		return 0;
	}
}

