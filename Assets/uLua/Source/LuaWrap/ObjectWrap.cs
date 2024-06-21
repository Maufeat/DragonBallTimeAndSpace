﻿using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class ObjectWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("FindObjectsOfType", FindObjectsOfType),
			new LuaMethod("DontDestroyOnLoad", DontDestroyOnLoad),
			new LuaMethod("ToString", ToString),
			new LuaMethod("Equals", Equals),
			new LuaMethod("GetHashCode", GetHashCode),
			new LuaMethod("GetInstanceID", GetInstanceID),
			new LuaMethod("Instantiate", Instantiate),
			new LuaMethod("FindObjectOfType", FindObjectOfType),
			new LuaMethod("DestroyObject", DestroyObject),
			new LuaMethod("DestroyImmediate", DestroyImmediate),
			new LuaMethod("Destroy", Destroy),
			new LuaMethod("New", _CreateObject),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__tostring", Lua_ToString),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("name", get_name, set_name),
			new LuaField("hideFlags", get_hideFlags, set_hideFlags),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.Object", typeof(Object), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateObject(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Object obj = new Object();
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Object.New");
		}

		return 0;
	}

	static Type classType = typeof(Object);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_name(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Object obj = (Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name name");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index name on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.name);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_hideFlags(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Object obj = (Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hideFlags");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hideFlags on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.hideFlags);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_name(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Object obj = (Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name name");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index name on a nil value");
			}
		}

		obj.name = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_hideFlags(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Object obj = (Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hideFlags");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hideFlags on a nil value");
			}
		}

		obj.hideFlags = (HideFlags)LuaScriptMgr.GetNetObject(L, 3, typeof(HideFlags));
		return 0;
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
			LuaScriptMgr.Push(L, "Table: UnityEngine.Object");
		}

		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindObjectsOfType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type arg0 = LuaScriptMgr.GetTypeObject(L, 1);
		Object[] o = Object.FindObjectsOfType(arg0);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DontDestroyOnLoad(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Object arg0 = (Object)LuaScriptMgr.GetUnityObject(L, 1, typeof(Object));
		Object.DontDestroyOnLoad(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Object obj = (Object)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Object");
		string o = obj.ToString();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Equals(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Object obj = LuaScriptMgr.GetVarObject(L, 1) as Object;
		object arg0 = LuaScriptMgr.GetVarObject(L, 2);
		bool o = obj != null ? obj.Equals(arg0) : arg0 == null;
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetHashCode(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Object obj = (Object)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Object");
		int o = obj.GetHashCode();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInstanceID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Object obj = (Object)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Object");
		int o = obj.GetInstanceID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Instantiate(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Object arg0 = (Object)LuaScriptMgr.GetUnityObject(L, 1, typeof(Object));
			Object o = Object.Instantiate(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			Object arg0 = (Object)LuaScriptMgr.GetUnityObject(L, 1, typeof(Object));
			Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
			Quaternion arg2 = LuaScriptMgr.GetQuaternion(L, 3);
			Object o = Object.Instantiate(arg0,arg1,arg2);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Object.Instantiate");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindObjectOfType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type arg0 = LuaScriptMgr.GetTypeObject(L, 1);
		Object o = Object.FindObjectOfType(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
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

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyObject(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Object arg0 = (Object)LuaScriptMgr.GetLuaObject(L, 1);
			LuaScriptMgr.__gc(L);
			Object.DestroyObject(arg0);
			return 0;
		}
		else if (count == 2)
		{
			Object arg0 = (Object)LuaScriptMgr.GetLuaObject(L, 1);
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
			Object.DestroyObject(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Object.DestroyObject");
		}

		return 0;

	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyImmediate(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Object arg0 = (Object)LuaScriptMgr.GetLuaObject(L, 1);
			LuaScriptMgr.__gc(L);
			Object.DestroyImmediate(arg0);
			return 0;
		}
		else if (count == 2)
		{
			Object arg0 = (Object)LuaScriptMgr.GetLuaObject(L, 1);
			bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
			Object.DestroyImmediate(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Object.DestroyImmediate");
		}

		return 0;

	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Destroy(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Object arg0 = (Object)LuaScriptMgr.GetLuaObject(L, 1);
			LuaScriptMgr.__gc(L);
			Object.Destroy(arg0);
			return 0;
		}
		else if (count == 2)
		{
			Object arg0 = (Object)LuaScriptMgr.GetLuaObject(L, 1);
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
			Object.Destroy(arg0, arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Object.Destroy");
		}

		return 0;

	}
}

