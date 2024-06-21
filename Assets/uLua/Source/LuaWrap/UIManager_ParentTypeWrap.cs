using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class UIManager_ParentTypeWrap
{
	static LuaMethod[] enums = new LuaMethod[]
	{
		new LuaMethod("UIRoot", GetUIRoot),
		new LuaMethod("Map", GetMap),
		new LuaMethod("Main", GetMain),
		new LuaMethod("CommonUI", GetCommonUI),
		new LuaMethod("Loading", GetLoading),
		new LuaMethod("HPRoot", GetHPRoot),
		new LuaMethod("Tips", GetTips),
		new LuaMethod("UICamera", GetUICamera),
		new LuaMethod("Guide", GetGuide),
		new LuaMethod("IntToEnum", IntToEnum),
	};

	public static void Register(IntPtr L)
	{
		LuaScriptMgr.RegisterLib(L, "UIManager.ParentType", typeof(UIManager.ParentType), enums);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetUIRoot(IntPtr L)
	{
		LuaScriptMgr.Push(L, UIManager.ParentType.UIRoot);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMap(IntPtr L)
	{
		LuaScriptMgr.Push(L, UIManager.ParentType.Map);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMain(IntPtr L)
	{
		LuaScriptMgr.Push(L, UIManager.ParentType.Main);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCommonUI(IntPtr L)
	{
		LuaScriptMgr.Push(L, UIManager.ParentType.CommonUI);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLoading(IntPtr L)
	{
		LuaScriptMgr.Push(L, UIManager.ParentType.Loading);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetHPRoot(IntPtr L)
	{
		LuaScriptMgr.Push(L, UIManager.ParentType.HPRoot);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTips(IntPtr L)
	{
		LuaScriptMgr.Push(L, UIManager.ParentType.Tips);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetUICamera(IntPtr L)
	{
		LuaScriptMgr.Push(L, UIManager.ParentType.UICamera);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetGuide(IntPtr L)
	{
		LuaScriptMgr.Push(L, UIManager.ParentType.Guide);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
		UIManager.ParentType o = (UIManager.ParentType)arg0;
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

