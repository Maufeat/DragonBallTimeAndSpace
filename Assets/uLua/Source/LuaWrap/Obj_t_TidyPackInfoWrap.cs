using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class Obj_t_TidyPackInfoWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateObj_t_TidyPackInfo),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("thisid", get_thisid, set_thisid),
			new LuaField("grid_x", get_grid_x, set_grid_x),
			new LuaField("grid_y", get_grid_y, set_grid_y),
		};

		LuaScriptMgr.RegisterLib(L, "Obj.t_TidyPackInfo", typeof(Obj.t_TidyPackInfo), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateObj_t_TidyPackInfo(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Obj.t_TidyPackInfo obj = new Obj.t_TidyPackInfo();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Obj.t_TidyPackInfo.New");
		}

		return 0;
	}

	static Type classType = typeof(Obj.t_TidyPackInfo);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_thisid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_TidyPackInfo obj = (Obj.t_TidyPackInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name thisid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index thisid on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.thisid);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_grid_x(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_TidyPackInfo obj = (Obj.t_TidyPackInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name grid_x");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index grid_x on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.grid_x);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_grid_y(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_TidyPackInfo obj = (Obj.t_TidyPackInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name grid_y");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index grid_y on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.grid_y);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_thisid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_TidyPackInfo obj = (Obj.t_TidyPackInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name thisid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index thisid on a nil value");
			}
		}

		obj.thisid = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_grid_x(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_TidyPackInfo obj = (Obj.t_TidyPackInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name grid_x");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index grid_x on a nil value");
			}
		}

		obj.grid_x = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_grid_y(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_TidyPackInfo obj = (Obj.t_TidyPackInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name grid_y");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index grid_y on a nil value");
			}
		}

		obj.grid_y = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}
}

