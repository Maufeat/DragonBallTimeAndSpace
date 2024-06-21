using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class IrregularGridLayOutWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("FitChildItem", FitChildItem),
			new LuaMethod("New", _CreateIrregularGridLayOut),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "IrregularGridLayOut", typeof(IrregularGridLayOut), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateIrregularGridLayOut(IntPtr L)
	{
		LuaDLL.luaL_error(L, "IrregularGridLayOut class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(IrregularGridLayOut);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FitChildItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		IrregularGridLayOut obj = (IrregularGridLayOut)LuaScriptMgr.GetUnityObjectSelf(L, 1, "IrregularGridLayOut");
		obj.FitChildItem();
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

