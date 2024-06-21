using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class WayNodeTypeWrap
{
	static LuaMethod[] enums = new LuaMethod[]
	{
		new LuaMethod("Text", GetText),
		new LuaMethod("Way", GetWay),
		new LuaMethod("EndPreFix", GetEndPreFix),
		new LuaMethod("IntToEnum", IntToEnum),
	};

	public static void Register(IntPtr L)
	{
		LuaScriptMgr.RegisterLib(L, "WayNodeType", typeof(WayNodeType), enums);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetText(IntPtr L)
	{
		LuaScriptMgr.Push(L, WayNodeType.Text);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetWay(IntPtr L)
	{
		LuaScriptMgr.Push(L, WayNodeType.Way);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetEndPreFix(IntPtr L)
	{
		LuaScriptMgr.Push(L, WayNodeType.EndPreFix);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
		WayNodeType o = (WayNodeType)arg0;
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

