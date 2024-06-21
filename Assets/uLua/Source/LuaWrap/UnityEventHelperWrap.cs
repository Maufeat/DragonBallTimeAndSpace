using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;

public class UnityEventHelperWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("AddListener", AddListener),
			new LuaMethod("RemoveAllListeners", RemoveAllListeners),
			new LuaMethod("New", _CreateUnityEventHelper),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UnityEventHelper", typeof(UnityEventHelper), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEventHelper(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			UnityEventHelper obj = new UnityEventHelper();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UnityEventHelper.New");
		}

		return 0;
	}

	static Type classType = typeof(UnityEventHelper);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddListener(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(UnityEngine.Events.UnityEvent<GameObject>), typeof(LuaInterface.LuaFunction)))
		{
			UnityEngine.Events.UnityEvent<GameObject> arg0 = (UnityEngine.Events.UnityEvent<GameObject>)LuaScriptMgr.GetLuaObject(L, 1);
			LuaFunction arg1 = LuaScriptMgr.ToLuaFunction(L, 2);
			UnityEventHelper.AddListener(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(UnityEngine.Events.UnityEvent<bool>), typeof(LuaInterface.LuaFunction)))
		{
			UnityEngine.Events.UnityEvent<bool> arg0 = (UnityEngine.Events.UnityEvent<bool>)LuaScriptMgr.GetLuaObject(L, 1);
			LuaFunction arg1 = LuaScriptMgr.ToLuaFunction(L, 2);
			UnityEventHelper.AddListener(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(UnityEngine.Events.UnityEvent<string>), typeof(LuaInterface.LuaFunction)))
		{
			UnityEngine.Events.UnityEvent<string> arg0 = (UnityEngine.Events.UnityEvent<string>)LuaScriptMgr.GetLuaObject(L, 1);
			LuaFunction arg1 = LuaScriptMgr.ToLuaFunction(L, 2);
			UnityEventHelper.AddListener(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(UnityEngine.Events.UnityEvent), typeof(LuaInterface.LuaFunction)))
		{
			UnityEngine.Events.UnityEvent arg0 = (UnityEngine.Events.UnityEvent)LuaScriptMgr.GetLuaObject(L, 1);
			LuaFunction arg1 = LuaScriptMgr.ToLuaFunction(L, 2);
			UnityEventHelper.AddListener(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(UnityEngine.Events.UnityEvent<float>), typeof(LuaInterface.LuaFunction)))
		{
			UnityEngine.Events.UnityEvent<float> arg0 = (UnityEngine.Events.UnityEvent<float>)LuaScriptMgr.GetLuaObject(L, 1);
			LuaFunction arg1 = LuaScriptMgr.ToLuaFunction(L, 2);
			UnityEventHelper.AddListener(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(UnityEngine.Events.UnityEvent<Vector2>), typeof(LuaInterface.LuaFunction)))
		{
			UnityEngine.Events.UnityEvent<Vector2> arg0 = (UnityEngine.Events.UnityEvent<Vector2>)LuaScriptMgr.GetLuaObject(L, 1);
			LuaFunction arg1 = LuaScriptMgr.ToLuaFunction(L, 2);
			UnityEventHelper.AddListener(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UnityEventHelper.AddListener");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveAllListeners(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UnityEngine.Events.UnityEventBase arg0 = (UnityEngine.Events.UnityEventBase)LuaScriptMgr.GetNetObject(L, 1, typeof(UnityEngine.Events.UnityEventBase));
		UnityEventHelper.RemoveAllListeners(arg0);
		return 0;
	}
}

