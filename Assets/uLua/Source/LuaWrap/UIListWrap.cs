using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UIListWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Init", Init),
			new LuaMethod("ResreshObj", ResreshObj),
			new LuaMethod("Clear", Clear),
			new LuaMethod("New", _CreateUIList),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("_count", get__count, set__count),
		};

		LuaScriptMgr.RegisterLib(L, "UIList", typeof(UIList), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUIList(IntPtr L)
	{
		LuaDLL.luaL_error(L, "UIList class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(UIList);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get__count(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIList obj = (UIList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name _count");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index _count on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj._count);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set__count(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIList obj = (UIList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name _count");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index _count on a nil value");
			}
		}

		obj._count = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		UIList obj = (UIList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		Action<GameObject,int> arg1 = null;
		LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

		if (funcType3 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (Action<GameObject,int>)LuaScriptMgr.GetNetObject(L, 3, typeof(Action<GameObject,int>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
			arg1 = (param0, param1) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				LuaScriptMgr.Push(L, param1);
				func.PCall(top, 2);
				func.EndPCall(top);
			};
		}

		obj.Init(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResreshObj(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIList obj = (UIList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIList");
		Action<GameObject,int> arg0 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg0 = (Action<GameObject,int>)LuaScriptMgr.GetNetObject(L, 2, typeof(Action<GameObject,int>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
			arg0 = (param0, param1) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				LuaScriptMgr.Push(L, param1);
				func.PCall(top, 2);
				func.EndPCall(top);
			};
		}

		obj.ResreshObj(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Clear(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIList obj = (UIList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIList");
		obj.Clear();
		return 0;
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

