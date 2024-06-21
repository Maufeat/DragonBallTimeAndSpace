using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UICenterOnChildWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Reset", Reset),
			new LuaMethod("ResetAll", ResetAll),
			new LuaMethod("CenterOnTarget", CenterOnTarget),
			new LuaMethod("OnEndDrag", OnEndDrag),
			new LuaMethod("OnDrag", OnDrag),
			new LuaMethod("New", _CreateUICenterOnChild),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("centerSpeed", get_centerSpeed, set_centerSpeed),
			new LuaField("moveType", get_moveType, set_moveType),
			new LuaField("onCenter", get_onCenter, set_onCenter),
			new LuaField("centerObj", get_centerObj, set_centerObj),
		};

		LuaScriptMgr.RegisterLib(L, "UICenterOnChild", typeof(UICenterOnChild), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUICenterOnChild(IntPtr L)
	{
		LuaDLL.luaL_error(L, "UICenterOnChild class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(UICenterOnChild);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_centerSpeed(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UICenterOnChild obj = (UICenterOnChild)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name centerSpeed");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index centerSpeed on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.centerSpeed);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_moveType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UICenterOnChild obj = (UICenterOnChild)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name moveType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index moveType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.moveType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onCenter(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UICenterOnChild obj = (UICenterOnChild)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onCenter");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onCenter on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.onCenter);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_centerObj(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UICenterOnChild obj = (UICenterOnChild)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name centerObj");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index centerObj on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.centerObj);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_centerSpeed(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UICenterOnChild obj = (UICenterOnChild)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name centerSpeed");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index centerSpeed on a nil value");
			}
		}

		obj.centerSpeed = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_moveType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UICenterOnChild obj = (UICenterOnChild)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name moveType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index moveType on a nil value");
			}
		}

		obj.moveType = (MoveType)LuaScriptMgr.GetNetObject(L, 3, typeof(MoveType));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onCenter(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UICenterOnChild obj = (UICenterOnChild)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onCenter");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onCenter on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.onCenter = (Action<GameObject>)LuaScriptMgr.GetNetObject(L, 3, typeof(Action<GameObject>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.onCenter = (param0) =>
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
	static int set_centerObj(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UICenterOnChild obj = (UICenterOnChild)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name centerObj");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index centerObj on a nil value");
			}
		}

		obj.centerObj = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Reset(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UICenterOnChild obj = (UICenterOnChild)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICenterOnChild");
		obj.Reset();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetAll(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UICenterOnChild obj = (UICenterOnChild)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICenterOnChild");
		obj.ResetAll();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CenterOnTarget(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UICenterOnChild obj = (UICenterOnChild)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICenterOnChild");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		obj.CenterOnTarget(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnEndDrag(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UICenterOnChild obj = (UICenterOnChild)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICenterOnChild");
		PointerEventData arg0 = (PointerEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(PointerEventData));
		obj.OnEndDrag(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDrag(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UICenterOnChild obj = (UICenterOnChild)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UICenterOnChild");
		PointerEventData arg0 = (PointerEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(PointerEventData));
		obj.OnDrag(arg0);
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

