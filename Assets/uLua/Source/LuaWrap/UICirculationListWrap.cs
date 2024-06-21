using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UICirculationListWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Init", Init),
			new LuaMethod("ReBuild", ReBuild),
			new LuaMethod("RefreshWhenListCountChanged", RefreshWhenListCountChanged),
			new LuaMethod("SetNormalizedPositionByItemIndex", SetNormalizedPositionByItemIndex),
			new LuaMethod("RecordNormalizedPosition", RecordNormalizedPosition),
			new LuaMethod("RevertNormalizedPosition", RevertNormalizedPosition),
			new LuaMethod("SetNormalizedPosition", SetNormalizedPosition),
			new LuaMethod("RefreshShowItem", RefreshShowItem),
			new LuaMethod("New", _CreateUICirculationList),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("m_itemMinHeight", get_m_itemMinHeight, set_m_itemMinHeight),
			new LuaField("AllItems", get_AllItems, set_AllItems),
		};

		LuaScriptMgr.RegisterLib(L, "UICirculationList", typeof(UICirculationList), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUICirculationList(IntPtr L)
	{
		LuaDLL.luaL_error(L, "UICirculationList class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(UICirculationList);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_itemMinHeight(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UICirculationList obj = (UICirculationList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_itemMinHeight");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_itemMinHeight on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.m_itemMinHeight);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AllItems(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UICirculationList obj = (UICirculationList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AllItems");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AllItems on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.AllItems);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_itemMinHeight(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UICirculationList obj = (UICirculationList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_itemMinHeight");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_itemMinHeight on a nil value");
			}
		}

		obj.m_itemMinHeight = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_AllItems(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UICirculationList obj = (UICirculationList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AllItems");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AllItems on a nil value");
			}
		}

		obj.AllItems = (List<GameObject>)LuaScriptMgr.GetNetObject(L, 3, typeof(List<GameObject>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3)
		{
			UICirculationList obj = (UICirculationList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICirculationList");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			Action<Transform,int> arg1 = null;
			LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

			if (funcType3 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (Action<Transform,int>)LuaScriptMgr.GetNetObject(L, 3, typeof(Action<Transform,int>));
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
		else if (count == 4)
		{
			UICirculationList obj = (UICirculationList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICirculationList");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
			Action<Transform,int> arg2 = null;
			LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

			if (funcType4 != LuaTypes.LUA_TFUNCTION)
			{
				 arg2 = (Action<Transform,int>)LuaScriptMgr.GetNetObject(L, 4, typeof(Action<Transform,int>));
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 4);
				arg2 = (param0, param1) =>
				{
					int top = func.BeginPCall();
					LuaScriptMgr.Push(L, param0);
					LuaScriptMgr.Push(L, param1);
					func.PCall(top, 2);
					func.EndPCall(top);
				};
			}

			obj.Init(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UICirculationList.Init");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReBuild(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UICirculationList obj = (UICirculationList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICirculationList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		obj.ReBuild(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshWhenListCountChanged(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UICirculationList obj = (UICirculationList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICirculationList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		obj.RefreshWhenListCountChanged(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetNormalizedPositionByItemIndex(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UICirculationList obj = (UICirculationList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICirculationList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		obj.SetNormalizedPositionByItemIndex(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RecordNormalizedPosition(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UICirculationList obj = (UICirculationList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICirculationList");
		obj.RecordNormalizedPosition();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RevertNormalizedPosition(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UICirculationList obj = (UICirculationList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICirculationList");
		obj.RevertNormalizedPosition();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetNormalizedPosition(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UICirculationList obj = (UICirculationList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICirculationList");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		obj.SetNormalizedPosition(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshShowItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UICirculationList obj = (UICirculationList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICirculationList");
		obj.RefreshShowItem();
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

