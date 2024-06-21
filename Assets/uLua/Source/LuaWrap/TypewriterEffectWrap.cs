using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class TypewriterEffectWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("StartTypeWrite", StartTypeWrite),
			new LuaMethod("BreakTypeWrite", BreakTypeWrite),
			new LuaMethod("OnFinish", OnFinish),
			new LuaMethod("New", _CreateTypewriterEffect),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("callback", get_callback, set_callback),
			new LuaField("charsPerSecond", get_charsPerSecond, set_charsPerSecond),
			new LuaField("IsActive", get_IsActive, set_IsActive),
		};

		LuaScriptMgr.RegisterLib(L, "TypewriterEffect", typeof(TypewriterEffect), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateTypewriterEffect(IntPtr L)
	{
		LuaDLL.luaL_error(L, "TypewriterEffect class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(TypewriterEffect);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_callback(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TypewriterEffect obj = (TypewriterEffect)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name callback");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index callback on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.callback);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_charsPerSecond(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TypewriterEffect obj = (TypewriterEffect)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name charsPerSecond");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index charsPerSecond on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.charsPerSecond);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsActive(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TypewriterEffect obj = (TypewriterEffect)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsActive");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsActive on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsActive);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_callback(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TypewriterEffect obj = (TypewriterEffect)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name callback");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index callback on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.callback = (Action)LuaScriptMgr.GetNetObject(L, 3, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.callback = () =>
			{
				func.Call();
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_charsPerSecond(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TypewriterEffect obj = (TypewriterEffect)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name charsPerSecond");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index charsPerSecond on a nil value");
			}
		}

		obj.charsPerSecond = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_IsActive(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TypewriterEffect obj = (TypewriterEffect)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsActive");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsActive on a nil value");
			}
		}

		obj.IsActive = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StartTypeWrite(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		TypewriterEffect obj = (TypewriterEffect)LuaScriptMgr.GetUnityObjectSelf(L, 1, "TypewriterEffect");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		Action arg1 = null;
		LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

		if (funcType3 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (Action)LuaScriptMgr.GetNetObject(L, 3, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
			arg1 = () =>
			{
				func.Call();
			};
		}

		obj.StartTypeWrite(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BreakTypeWrite(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		TypewriterEffect obj = (TypewriterEffect)LuaScriptMgr.GetUnityObjectSelf(L, 1, "TypewriterEffect");
		obj.BreakTypeWrite();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnFinish(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		TypewriterEffect obj = (TypewriterEffect)LuaScriptMgr.GetUnityObjectSelf(L, 1, "TypewriterEffect");
		obj.OnFinish();
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

