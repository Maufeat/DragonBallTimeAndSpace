using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class Obj_t_ObjectWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateObj_t_Object),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("baseid", get_baseid, set_baseid),
			new LuaField("thisid", get_thisid, set_thisid),
			new LuaField("type", get_type, set_type),
			new LuaField("packtype", get_packtype, set_packtype),
			new LuaField("name", get_name, set_name),
			new LuaField("num", get_num, set_num),
			new LuaField("bind", get_bind, set_bind),
			new LuaField("grid_x", get_grid_x, set_grid_x),
			new LuaField("grid_y", get_grid_y, set_grid_y),
			new LuaField("quality", get_quality, set_quality),
			new LuaField("level", get_level, set_level),
			new LuaField("timer", get_timer, set_timer),
			new LuaField("equipprop", get_equipprop, set_equipprop),
			new LuaField("equiprand", get_equiprand, null),
			new LuaField("nextusetime", get_nextusetime, set_nextusetime),
			new LuaField("card_data", get_card_data, set_card_data),
			new LuaField("lock_end_time", get_lock_end_time, set_lock_end_time),
			new LuaField("tradetime", get_tradetime, set_tradetime),
		};

		LuaScriptMgr.RegisterLib(L, "Obj.t_Object", typeof(Obj.t_Object), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateObj_t_Object(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Obj.t_Object obj = new Obj.t_Object();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Obj.t_Object.New");
		}

		return 0;
	}

	static Type classType = typeof(Obj.t_Object);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_baseid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name baseid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index baseid on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.baseid);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_thisid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

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
	static int get_type(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name type");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index type on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.type);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_packtype(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name packtype");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index packtype on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.packtype);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_name(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

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
	static int get_num(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name num");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index num on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.num);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bind(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bind");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bind on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.bind);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_grid_x(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

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
		Obj.t_Object obj = (Obj.t_Object)o;

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
	static int get_quality(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name quality");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index quality on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.quality);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_level(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name level");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index level on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.level);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_timer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name timer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index timer on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.timer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_equipprop(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name equipprop");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index equipprop on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.equipprop);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_equiprand(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name equiprand");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index equiprand on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.equiprand);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_nextusetime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name nextusetime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index nextusetime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.nextusetime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_card_data(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name card_data");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index card_data on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.card_data);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lock_end_time(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lock_end_time");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lock_end_time on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lock_end_time);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_tradetime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name tradetime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index tradetime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.tradetime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_baseid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name baseid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index baseid on a nil value");
			}
		}

		obj.baseid = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_thisid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

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
	static int set_type(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name type");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index type on a nil value");
			}
		}

		obj.type = (Obj.ObjectType)LuaScriptMgr.GetNetObject(L, 3, typeof(Obj.ObjectType));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_packtype(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name packtype");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index packtype on a nil value");
			}
		}

		obj.packtype = (Obj.PackType)LuaScriptMgr.GetNetObject(L, 3, typeof(Obj.PackType));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_name(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

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
	static int set_num(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name num");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index num on a nil value");
			}
		}

		obj.num = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bind(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bind");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bind on a nil value");
			}
		}

		obj.bind = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_grid_x(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

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
		Obj.t_Object obj = (Obj.t_Object)o;

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

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_quality(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name quality");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index quality on a nil value");
			}
		}

		obj.quality = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_level(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name level");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index level on a nil value");
			}
		}

		obj.level = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_timer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name timer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index timer on a nil value");
			}
		}

		obj.timer = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_equipprop(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name equipprop");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index equipprop on a nil value");
			}
		}

		obj.equipprop = (Obj.EquipData)LuaScriptMgr.GetNetObject(L, 3, typeof(Obj.EquipData));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_nextusetime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name nextusetime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index nextusetime on a nil value");
			}
		}

		obj.nextusetime = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_card_data(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name card_data");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index card_data on a nil value");
			}
		}

		obj.card_data = (Obj.CardData)LuaScriptMgr.GetNetObject(L, 3, typeof(Obj.CardData));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lock_end_time(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lock_end_time");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lock_end_time on a nil value");
			}
		}

		obj.lock_end_time = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_tradetime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.t_Object obj = (Obj.t_Object)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name tradetime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index tradetime on a nil value");
			}
		}

		obj.tradetime = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}
}

