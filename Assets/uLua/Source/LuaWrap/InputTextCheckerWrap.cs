using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class InputTextCheckerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("CheckTextEmpty", CheckTextEmpty),
			new LuaMethod("CheckTextHasSpecialChar", CheckTextHasSpecialChar),
			new LuaMethod("CheckTextIsAllNum", CheckTextIsAllNum),
			new LuaMethod("CheckTextHasBlank", CheckTextHasBlank),
			new LuaMethod("CheckTextLength", CheckTextLength),
			new LuaMethod("CheckNumOrEngTextLength", CheckNumOrEngTextLength),
			new LuaMethod("CheckTextHasDirtyWord", CheckTextHasDirtyWord),
			new LuaMethod("New", _CreateInputTextChecker),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "InputTextChecker", typeof(InputTextChecker), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateInputTextChecker(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			InputTextChecker obj = new InputTextChecker();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: InputTextChecker.New");
		}

		return 0;
	}

	static Type classType = typeof(InputTextChecker);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckTextEmpty(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		bool o = InputTextChecker.CheckTextEmpty(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckTextHasSpecialChar(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
			bool o = InputTextChecker.CheckTextHasSpecialChar(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			string arg1 = LuaScriptMgr.GetLuaString(L, 2);
			uint arg2 = (uint)LuaScriptMgr.GetNumber(L, 3);
			bool o = InputTextChecker.CheckTextHasSpecialChar(arg0,arg1,arg2);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: InputTextChecker.CheckTextHasSpecialChar");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckTextIsAllNum(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		bool o = InputTextChecker.CheckTextIsAllNum(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckTextHasBlank(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		bool o = InputTextChecker.CheckTextHasBlank(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckTextLength(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
		uint arg3 = (uint)LuaScriptMgr.GetNumber(L, 4);
		bool o = InputTextChecker.CheckTextLength(arg0,arg1,arg2,arg3);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckNumOrEngTextLength(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
		uint arg3 = (uint)LuaScriptMgr.GetNumber(L, 4);
		bool o = InputTextChecker.CheckNumOrEngTextLength(arg0,arg1,arg2,arg3);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckTextHasDirtyWord(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		bool o = InputTextChecker.CheckTextHasDirtyWord(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

