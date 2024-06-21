using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UIFoldOutListWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("ClickFoldItems", ClickFoldItems),
			new LuaMethod("AddItem", AddItem),
			new LuaMethod("InitList", InitList),
			new LuaMethod("resetUIFoldOut", resetUIFoldOut),
			new LuaMethod("SetPageInfo", SetPageInfo),
			new LuaMethod("GetCurrentPage", GetCurrentPage),
			new LuaMethod("GetPageCount", GetPageCount),
			new LuaMethod("SetScrollBarAutoDisable", SetScrollBarAutoDisable),
			new LuaMethod("New", _CreateUIFoldOutList),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("FoldItems", get_FoldItems, set_FoldItems),
			new LuaField("Item", get_Item, set_Item),
			new LuaField("bUIFoldout", get_bUIFoldout, set_bUIFoldout),
			new LuaField("InitListAction", get_InitListAction, set_InitListAction),
		};

		LuaScriptMgr.RegisterLib(L, "UIFoldOutList", typeof(UIFoldOutList), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUIFoldOutList(IntPtr L)
	{
		LuaDLL.luaL_error(L, "UIFoldOutList class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(UIFoldOutList);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FoldItems(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldOutList obj = (UIFoldOutList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FoldItems");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FoldItems on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.FoldItems);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Item(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldOutList obj = (UIFoldOutList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Item");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Item on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Item);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bUIFoldout(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldOutList obj = (UIFoldOutList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bUIFoldout");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bUIFoldout on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.bUIFoldout);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_InitListAction(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldOutList obj = (UIFoldOutList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name InitListAction");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index InitListAction on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.InitListAction);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_FoldItems(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldOutList obj = (UIFoldOutList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FoldItems");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FoldItems on a nil value");
			}
		}

		obj.FoldItems = (List<UIFoldItem>)LuaScriptMgr.GetNetObject(L, 3, typeof(List<UIFoldItem>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Item(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldOutList obj = (UIFoldOutList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Item");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Item on a nil value");
			}
		}

		obj.Item = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bUIFoldout(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldOutList obj = (UIFoldOutList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bUIFoldout");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bUIFoldout on a nil value");
			}
		}

		obj.bUIFoldout = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_InitListAction(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldOutList obj = (UIFoldOutList)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name InitListAction");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index InitListAction on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.InitListAction = (Action<int,GameObject>)LuaScriptMgr.GetNetObject(L, 3, typeof(Action<int,GameObject>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.InitListAction = (param0, param1) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				LuaScriptMgr.Push(L, param1);
				func.PCall(top, 2);
				func.EndPCall(top);
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClickFoldItems(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIFoldOutList obj = (UIFoldOutList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIFoldOutList");
		UIFoldItem arg0 = (UIFoldItem)LuaScriptMgr.GetUnityObject(L, 2, typeof(UIFoldItem));
		obj.ClickFoldItems(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddItem(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			UIFoldOutList obj = (UIFoldOutList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIFoldOutList");
			GameObject o = obj.AddItem();
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2)
		{
			UIFoldOutList obj = (UIFoldOutList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIFoldOutList");
			UIFoldItem arg0 = (UIFoldItem)LuaScriptMgr.GetUnityObject(L, 2, typeof(UIFoldItem));
			obj.AddItem(arg0);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UIFoldOutList.AddItem");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		UIFoldOutList obj = (UIFoldOutList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIFoldOutList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
		obj.InitList(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int resetUIFoldOut(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIFoldOutList obj = (UIFoldOutList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIFoldOutList");
		obj.resetUIFoldOut();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPageInfo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 6);
		UIFoldOutList obj = (UIFoldOutList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIFoldOutList");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
		UnityEngine.Events.UnityAction arg3 = null;
		LuaTypes funcType5 = LuaDLL.lua_type(L, 5);

		if (funcType5 != LuaTypes.LUA_TFUNCTION)
		{
			 arg3 = (UnityEngine.Events.UnityAction)LuaScriptMgr.GetNetObject(L, 5, typeof(UnityEngine.Events.UnityAction));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 5);
			arg3 = () =>
			{
				func.Call();
			};
		}

		UnityEngine.Events.UnityAction arg4 = null;
		LuaTypes funcType6 = LuaDLL.lua_type(L, 6);

		if (funcType6 != LuaTypes.LUA_TFUNCTION)
		{
			 arg4 = (UnityEngine.Events.UnityAction)LuaScriptMgr.GetNetObject(L, 6, typeof(UnityEngine.Events.UnityAction));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 6);
			arg4 = () =>
			{
				func.Call();
			};
		}

		obj.SetPageInfo(arg0,arg1,arg2,arg3,arg4);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurrentPage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIFoldOutList obj = (UIFoldOutList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIFoldOutList");
		int o = obj.GetCurrentPage();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPageCount(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIFoldOutList obj = (UIFoldOutList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIFoldOutList");
		int o = obj.GetPageCount();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetScrollBarAutoDisable(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIFoldOutList obj = (UIFoldOutList)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIFoldOutList");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.SetScrollBarAutoDisable(arg0);
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

