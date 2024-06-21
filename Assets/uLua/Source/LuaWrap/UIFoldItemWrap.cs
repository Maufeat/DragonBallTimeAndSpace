using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UIFoldItemWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("startTweenScale", startTweenScale),
			new LuaMethod("resetCantainView", resetCantainView),
			new LuaMethod("Dispose", Dispose),
			new LuaMethod("New", _CreateUIFoldItem),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("myButton", get_myButton, set_myButton),
			new LuaField("rect", get_rect, set_rect),
			new LuaField("isContainView", get_isContainView, set_isContainView),
			new LuaField("tmpElement", get_tmpElement, set_tmpElement),
			new LuaField("tweenFinish", get_tweenFinish, set_tweenFinish),
		};

		LuaScriptMgr.RegisterLib(L, "UIFoldItem", typeof(UIFoldItem), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUIFoldItem(IntPtr L)
	{
		LuaDLL.luaL_error(L, "UIFoldItem class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(UIFoldItem);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_myButton(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldItem obj = (UIFoldItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name myButton");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index myButton on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.myButton);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_rect(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldItem obj = (UIFoldItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rect");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rect on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.rect);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isContainView(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldItem obj = (UIFoldItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isContainView");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isContainView on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.isContainView);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_tmpElement(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldItem obj = (UIFoldItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name tmpElement");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index tmpElement on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.tmpElement);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_tweenFinish(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldItem obj = (UIFoldItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name tweenFinish");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index tweenFinish on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.tweenFinish);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_myButton(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldItem obj = (UIFoldItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name myButton");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index myButton on a nil value");
			}
		}

		obj.myButton = (Button)LuaScriptMgr.GetUnityObject(L, 3, typeof(Button));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_rect(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldItem obj = (UIFoldItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rect");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rect on a nil value");
			}
		}

		obj.rect = (RectTransform)LuaScriptMgr.GetUnityObject(L, 3, typeof(RectTransform));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isContainView(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldItem obj = (UIFoldItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isContainView");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isContainView on a nil value");
			}
		}

		obj.isContainView = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_tmpElement(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldItem obj = (UIFoldItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name tmpElement");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index tmpElement on a nil value");
			}
		}

		obj.tmpElement = (LayoutElement)LuaScriptMgr.GetUnityObject(L, 3, typeof(LayoutElement));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_tweenFinish(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIFoldItem obj = (UIFoldItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name tweenFinish");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index tweenFinish on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.tweenFinish = (Action)LuaScriptMgr.GetNetObject(L, 3, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.tweenFinish = () =>
			{
				func.Call();
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int startTweenScale(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIFoldItem obj = (UIFoldItem)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIFoldItem");
		obj.startTweenScale();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int resetCantainView(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIFoldItem obj = (UIFoldItem)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIFoldItem");
		obj.resetCantainView();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Dispose(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIFoldItem obj = (UIFoldItem)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIFoldItem");
		obj.Dispose();
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

