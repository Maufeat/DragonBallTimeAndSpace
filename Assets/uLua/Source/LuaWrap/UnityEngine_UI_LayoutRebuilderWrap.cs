using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using LuaInterface;

public class UnityEngine_UI_LayoutRebuilderWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("IsDestroyed", IsDestroyed),
			new LuaMethod("ForceRebuildLayoutImmediate", ForceRebuildLayoutImmediate),
			new LuaMethod("Rebuild", Rebuild),
			new LuaMethod("MarkLayoutForRebuild", MarkLayoutForRebuild),
			new LuaMethod("LayoutComplete", LayoutComplete),
			new LuaMethod("GraphicUpdateComplete", GraphicUpdateComplete),
			new LuaMethod("GetHashCode", GetHashCode),
			new LuaMethod("Equals", Equals),
			new LuaMethod("ToString", ToString),
			new LuaMethod("New", _CreateUnityEngine_UI_LayoutRebuilder),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__tostring", Lua_ToString),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("transform", get_transform, null),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.UI.LayoutRebuilder", typeof(LayoutRebuilder), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_LayoutRebuilder(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			LayoutRebuilder obj = new LayoutRebuilder();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: LayoutRebuilder.New");
		}

		return 0;
	}

	static Type classType = typeof(LayoutRebuilder);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_transform(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LayoutRebuilder obj = (LayoutRebuilder)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name transform");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index transform on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.transform);
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
			LuaScriptMgr.Push(L, "Table: UnityEngine.UI.LayoutRebuilder");
		}

		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsDestroyed(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LayoutRebuilder obj = (LayoutRebuilder)LuaScriptMgr.GetNetObjectSelf(L, 1, "LayoutRebuilder");
		bool o = obj.IsDestroyed();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ForceRebuildLayoutImmediate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RectTransform arg0 = (RectTransform)LuaScriptMgr.GetUnityObject(L, 1, typeof(RectTransform));
		LayoutRebuilder.ForceRebuildLayoutImmediate(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Rebuild(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LayoutRebuilder obj = (LayoutRebuilder)LuaScriptMgr.GetNetObjectSelf(L, 1, "LayoutRebuilder");
		CanvasUpdate arg0 = (CanvasUpdate)LuaScriptMgr.GetNetObject(L, 2, typeof(CanvasUpdate));
		obj.Rebuild(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MarkLayoutForRebuild(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RectTransform arg0 = (RectTransform)LuaScriptMgr.GetUnityObject(L, 1, typeof(RectTransform));
		LayoutRebuilder.MarkLayoutForRebuild(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LayoutComplete(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LayoutRebuilder obj = (LayoutRebuilder)LuaScriptMgr.GetNetObjectSelf(L, 1, "LayoutRebuilder");
		obj.LayoutComplete();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GraphicUpdateComplete(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LayoutRebuilder obj = (LayoutRebuilder)LuaScriptMgr.GetNetObjectSelf(L, 1, "LayoutRebuilder");
		obj.GraphicUpdateComplete();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetHashCode(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LayoutRebuilder obj = (LayoutRebuilder)LuaScriptMgr.GetNetObjectSelf(L, 1, "LayoutRebuilder");
		int o = obj.GetHashCode();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Equals(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LayoutRebuilder obj = LuaScriptMgr.GetVarObject(L, 1) as LayoutRebuilder;
		object arg0 = LuaScriptMgr.GetVarObject(L, 2);
		bool o = obj != null ? obj.Equals(arg0) : arg0 == null;
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LayoutRebuilder obj = (LayoutRebuilder)LuaScriptMgr.GetNetObjectSelf(L, 1, "LayoutRebuilder");
		string o = obj.ToString();
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

