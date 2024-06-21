using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UnityEngine_UI_VerticalLayoutGroupWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("CalculateLayoutInputHorizontal", CalculateLayoutInputHorizontal),
			new LuaMethod("CalculateLayoutInputVertical", CalculateLayoutInputVertical),
			new LuaMethod("SetLayoutHorizontal", SetLayoutHorizontal),
			new LuaMethod("SetLayoutVertical", SetLayoutVertical),
			new LuaMethod("New", _CreateUnityEngine_UI_VerticalLayoutGroup),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.UI.VerticalLayoutGroup", typeof(VerticalLayoutGroup), regs, fields, typeof(HorizontalOrVerticalLayoutGroup));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_VerticalLayoutGroup(IntPtr L)
	{
		LuaDLL.luaL_error(L, "VerticalLayoutGroup class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(VerticalLayoutGroup);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CalculateLayoutInputHorizontal(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		VerticalLayoutGroup obj = (VerticalLayoutGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "VerticalLayoutGroup");
		obj.CalculateLayoutInputHorizontal();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CalculateLayoutInputVertical(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		VerticalLayoutGroup obj = (VerticalLayoutGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "VerticalLayoutGroup");
		obj.CalculateLayoutInputVertical();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLayoutHorizontal(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		VerticalLayoutGroup obj = (VerticalLayoutGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "VerticalLayoutGroup");
		obj.SetLayoutHorizontal();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLayoutVertical(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		VerticalLayoutGroup obj = (VerticalLayoutGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "VerticalLayoutGroup");
		obj.SetLayoutVertical();
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

