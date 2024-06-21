using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;
using Object = UnityEngine.Object;

public class CountDownItemWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("InitItem", InitItem),
			new LuaMethod("StartCountDown", StartCountDown),
			new LuaMethod("StopCountDown", StopCountDown),
			new LuaMethod("Update", Update),
			new LuaMethod("New", _CreateCountDownItem),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("root", get_root, set_root),
			new LuaField("Txt_Time", get_Txt_Time, set_Txt_Time),
			new LuaField("fDuartion", get_fDuartion, set_fDuartion),
			new LuaField("fCurTime", get_fCurTime, set_fCurTime),
			new LuaField("OnCountDownComplete", get_OnCountDownComplete, set_OnCountDownComplete),
		};

		LuaScriptMgr.RegisterLib(L, "CountDownItem", typeof(CountDownItem), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateCountDownItem(IntPtr L)
	{
		LuaDLL.luaL_error(L, "CountDownItem class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(CountDownItem);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_root(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CountDownItem obj = (CountDownItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name root");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index root on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.root);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Txt_Time(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CountDownItem obj = (CountDownItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Txt_Time");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Txt_Time on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Txt_Time);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_fDuartion(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CountDownItem obj = (CountDownItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name fDuartion");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index fDuartion on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.fDuartion);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_fCurTime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CountDownItem obj = (CountDownItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name fCurTime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index fCurTime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.fCurTime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_OnCountDownComplete(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CountDownItem obj = (CountDownItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnCountDownComplete");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnCountDownComplete on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.OnCountDownComplete);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_root(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CountDownItem obj = (CountDownItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name root");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index root on a nil value");
			}
		}

		obj.root = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Txt_Time(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CountDownItem obj = (CountDownItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Txt_Time");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Txt_Time on a nil value");
			}
		}

		obj.Txt_Time = (Text)LuaScriptMgr.GetUnityObject(L, 3, typeof(Text));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_fDuartion(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CountDownItem obj = (CountDownItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name fDuartion");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index fDuartion on a nil value");
			}
		}

		obj.fDuartion = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_fCurTime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CountDownItem obj = (CountDownItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name fCurTime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index fCurTime on a nil value");
			}
		}

		obj.fCurTime = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_OnCountDownComplete(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CountDownItem obj = (CountDownItem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnCountDownComplete");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnCountDownComplete on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.OnCountDownComplete = (Action<bool>)LuaScriptMgr.GetNetObject(L, 3, typeof(Action<bool>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.OnCountDownComplete = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		CountDownItem obj = (CountDownItem)LuaScriptMgr.GetUnityObjectSelf(L, 1, "CountDownItem");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		Text arg1 = (Text)LuaScriptMgr.GetUnityObject(L, 3, typeof(Text));
		uint arg2 = (uint)LuaScriptMgr.GetNumber(L, 4);
		obj.InitItem(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StartCountDown(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CountDownItem obj = (CountDownItem)LuaScriptMgr.GetUnityObjectSelf(L, 1, "CountDownItem");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		obj.StartCountDown(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StopCountDown(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CountDownItem obj = (CountDownItem)LuaScriptMgr.GetUnityObjectSelf(L, 1, "CountDownItem");
		obj.StopCountDown();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Update(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CountDownItem obj = (CountDownItem)LuaScriptMgr.GetUnityObjectSelf(L, 1, "CountDownItem");
		obj.Update();
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

