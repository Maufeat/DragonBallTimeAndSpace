using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class PropsBaseWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("ResetData", ResetData),
			new LuaMethod("ResetData_Id", ResetData_Id),
			new LuaMethod("SetNewState", SetNewState),
			new LuaMethod("DeleteFromPackage", DeleteFromPackage),
			new LuaMethod("CopyData", CopyData),
			new LuaMethod("New", _CreatePropsBase),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("_obj", get__obj, set__obj),
			new LuaField("config", get_config, set_config),
			new LuaField("orderIndex", get_orderIndex, set_orderIndex),
			new LuaField("ThisidStr", get_ThisidStr, set_ThisidStr),
			new LuaField("IsNew", get_IsNew, null),
			new LuaField("FightValue", get_FightValue, null),
			new LuaField("Count", get_Count, null),
		};

		LuaScriptMgr.RegisterLib(L, "PropsBase", typeof(PropsBase), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreatePropsBase(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			PropsBase obj = new PropsBase();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(Obj.t_Object)))
		{
			Obj.t_Object arg0 = (Obj.t_Object)LuaScriptMgr.GetNetObject(L, 1, typeof(Obj.t_Object));
			PropsBase obj = new PropsBase(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(LuaInterface.LuaTable)))
		{
			LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
			PropsBase obj = new PropsBase(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else if (count == 2)
		{
			uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
			uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
			PropsBase obj = new PropsBase(arg0,arg1);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: PropsBase.New");
		}

		return 0;
	}

	static Type classType = typeof(PropsBase);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get__obj(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PropsBase obj = (PropsBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name _obj");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index _obj on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj._obj);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_config(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PropsBase obj = (PropsBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name config");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index config on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.config);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_orderIndex(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PropsBase obj = (PropsBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name orderIndex");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index orderIndex on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.orderIndex);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ThisidStr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PropsBase obj = (PropsBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ThisidStr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ThisidStr on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ThisidStr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNew(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PropsBase obj = (PropsBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsNew");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsNew on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsNew);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FightValue(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PropsBase obj = (PropsBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FightValue");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FightValue on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.FightValue);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Count(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PropsBase obj = (PropsBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Count");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Count on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Count);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set__obj(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PropsBase obj = (PropsBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name _obj");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index _obj on a nil value");
			}
		}

		obj._obj = (Obj.t_Object)LuaScriptMgr.GetNetObject(L, 3, typeof(Obj.t_Object));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_config(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PropsBase obj = (PropsBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name config");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index config on a nil value");
			}
		}

		obj.config = LuaScriptMgr.GetLuaTable(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_orderIndex(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PropsBase obj = (PropsBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name orderIndex");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index orderIndex on a nil value");
			}
		}

		obj.orderIndex = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ThisidStr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		PropsBase obj = (PropsBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ThisidStr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ThisidStr on a nil value");
			}
		}

		obj.ThisidStr = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		PropsBase obj = (PropsBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "PropsBase");
		Obj.t_Object arg0 = (Obj.t_Object)LuaScriptMgr.GetNetObject(L, 2, typeof(Obj.t_Object));
		obj.ResetData(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetData_Id(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		PropsBase obj = (PropsBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "PropsBase");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		obj.ResetData_Id(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetNewState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		PropsBase obj = (PropsBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "PropsBase");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.SetNewState(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DeleteFromPackage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		PropsBase obj = (PropsBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "PropsBase");
		obj.DeleteFromPackage();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CopyData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		PropsBase obj = (PropsBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "PropsBase");
		PropsBase arg0 = (PropsBase)LuaScriptMgr.GetNetObject(L, 2, typeof(PropsBase));
		obj.CopyData(arg0);
		return 0;
	}
}

