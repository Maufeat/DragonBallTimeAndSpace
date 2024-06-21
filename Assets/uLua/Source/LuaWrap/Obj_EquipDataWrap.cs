using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class Obj_EquipDataWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateObj_EquipData),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("weaponatt", get_weaponatt, set_weaponatt),
			new LuaField("pdam", get_pdam, set_pdam),
			new LuaField("mdam", get_mdam, set_mdam),
			new LuaField("pdef", get_pdef, set_pdef),
			new LuaField("mdef", get_mdef, set_mdef),
			new LuaField("maxhp", get_maxhp, set_maxhp),
			new LuaField("str", get_str, set_str),
			new LuaField("dex", get_dex, set_dex),
			new LuaField("intel", get_intel, set_intel),
			new LuaField("phy", get_phy, set_phy),
			new LuaField("bang", get_bang, set_bang),
			new LuaField("toughness", get_toughness, set_toughness),
			new LuaField("block", get_block, set_block),
			new LuaField("penetrate", get_penetrate, set_penetrate),
			new LuaField("accurate", get_accurate, set_accurate),
			new LuaField("hold", get_hold, set_hold),
			new LuaField("deflect", get_deflect, set_deflect),
			new LuaField("bangextradamage", get_bangextradamage, set_bangextradamage),
			new LuaField("toughnessreducedamage", get_toughnessreducedamage, set_toughnessreducedamage),
			new LuaField("blockreducedamage", get_blockreducedamage, set_blockreducedamage),
			new LuaField("penetrateextradamage", get_penetrateextradamage, set_penetrateextradamage),
			new LuaField("accurateextradamage", get_accurateextradamage, set_accurateextradamage),
			new LuaField("holdreducedamage", get_holdreducedamage, set_holdreducedamage),
			new LuaField("deflectreducedamage", get_deflectreducedamage, set_deflectreducedamage),
			new LuaField("maxmp", get_maxmp, set_maxmp),
			new LuaField("miss", get_miss, set_miss),
			new LuaField("hit", get_hit, set_hit),
			new LuaField("firemastery", get_firemastery, set_firemastery),
			new LuaField("icemastery", get_icemastery, set_icemastery),
			new LuaField("lightningmastery", get_lightningmastery, set_lightningmastery),
			new LuaField("brightmastery", get_brightmastery, set_brightmastery),
			new LuaField("darkmastery", get_darkmastery, set_darkmastery),
			new LuaField("fireresist", get_fireresist, set_fireresist),
			new LuaField("iceresist", get_iceresist, set_iceresist),
			new LuaField("lightningresist", get_lightningresist, set_lightningresist),
			new LuaField("brightresist", get_brightresist, set_brightresist),
			new LuaField("darkresist", get_darkresist, set_darkresist),
			new LuaField("firepen", get_firepen, set_firepen),
			new LuaField("icepen", get_icepen, set_icepen),
			new LuaField("lightningpen", get_lightningpen, set_lightningpen),
			new LuaField("brightpen", get_brightpen, set_brightpen),
			new LuaField("darkpen", get_darkpen, set_darkpen),
			new LuaField("blowint", get_blowint, set_blowint),
			new LuaField("knockint", get_knockint, set_knockint),
			new LuaField("floatint", get_floatint, set_floatint),
			new LuaField("superhitint", get_superhitint, set_superhitint),
			new LuaField("blowresist", get_blowresist, set_blowresist),
			new LuaField("knockresist", get_knockresist, set_knockresist),
			new LuaField("floatresist", get_floatresist, set_floatresist),
			new LuaField("superhitresist", get_superhitresist, set_superhitresist),
			new LuaField("stiffaddtime", get_stiffaddtime, set_stiffaddtime),
			new LuaField("stiffdectime", get_stiffdectime, set_stiffdectime),
			new LuaField("blowdectime", get_blowdectime, set_blowdectime),
			new LuaField("knockdectime", get_knockdectime, set_knockdectime),
			new LuaField("floatdectime", get_floatdectime, set_floatdectime),
			new LuaField("superhitdectime", get_superhitdectime, set_superhitdectime),
		};

		LuaScriptMgr.RegisterLib(L, "Obj.EquipData", typeof(Obj.EquipData), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateObj_EquipData(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Obj.EquipData obj = new Obj.EquipData();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Obj.EquipData.New");
		}

		return 0;
	}

	static Type classType = typeof(Obj.EquipData);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_weaponatt(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name weaponatt");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index weaponatt on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.weaponatt);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_pdam(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name pdam");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index pdam on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.pdam);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_mdam(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mdam");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mdam on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.mdam);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_pdef(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name pdef");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index pdef on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.pdef);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_mdef(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mdef");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mdef on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.mdef);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_maxhp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxhp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxhp on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.maxhp);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_str(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name str");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index str on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.str);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_dex(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name dex");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index dex on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.dex);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_intel(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name intel");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index intel on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.intel);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_phy(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name phy");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index phy on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.phy);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bang(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bang");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bang on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.bang);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_toughness(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name toughness");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index toughness on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.toughness);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_block(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name block");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index block on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.block);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_penetrate(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name penetrate");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index penetrate on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.penetrate);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_accurate(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name accurate");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index accurate on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.accurate);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_hold(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hold");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hold on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.hold);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_deflect(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name deflect");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index deflect on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.deflect);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bangextradamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bangextradamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bangextradamage on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.bangextradamage);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_toughnessreducedamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name toughnessreducedamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index toughnessreducedamage on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.toughnessreducedamage);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_blockreducedamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name blockreducedamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index blockreducedamage on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.blockreducedamage);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_penetrateextradamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name penetrateextradamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index penetrateextradamage on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.penetrateextradamage);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_accurateextradamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name accurateextradamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index accurateextradamage on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.accurateextradamage);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_holdreducedamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name holdreducedamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index holdreducedamage on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.holdreducedamage);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_deflectreducedamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name deflectreducedamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index deflectreducedamage on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.deflectreducedamage);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_maxmp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxmp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxmp on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.maxmp);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_miss(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name miss");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index miss on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.miss);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_hit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hit on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.hit);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_firemastery(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name firemastery");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index firemastery on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.firemastery);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_icemastery(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name icemastery");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index icemastery on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.icemastery);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lightningmastery(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lightningmastery");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lightningmastery on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lightningmastery);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_brightmastery(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name brightmastery");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index brightmastery on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.brightmastery);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_darkmastery(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name darkmastery");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index darkmastery on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.darkmastery);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_fireresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name fireresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index fireresist on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.fireresist);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_iceresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name iceresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index iceresist on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.iceresist);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lightningresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lightningresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lightningresist on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lightningresist);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_brightresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name brightresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index brightresist on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.brightresist);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_darkresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name darkresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index darkresist on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.darkresist);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_firepen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name firepen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index firepen on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.firepen);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_icepen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name icepen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index icepen on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.icepen);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lightningpen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lightningpen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lightningpen on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lightningpen);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_brightpen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name brightpen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index brightpen on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.brightpen);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_darkpen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name darkpen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index darkpen on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.darkpen);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_blowint(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name blowint");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index blowint on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.blowint);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_knockint(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name knockint");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index knockint on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.knockint);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_floatint(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name floatint");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index floatint on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.floatint);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_superhitint(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name superhitint");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index superhitint on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.superhitint);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_blowresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name blowresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index blowresist on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.blowresist);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_knockresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name knockresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index knockresist on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.knockresist);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_floatresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name floatresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index floatresist on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.floatresist);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_superhitresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name superhitresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index superhitresist on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.superhitresist);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_stiffaddtime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name stiffaddtime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index stiffaddtime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.stiffaddtime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_stiffdectime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name stiffdectime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index stiffdectime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.stiffdectime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_blowdectime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name blowdectime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index blowdectime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.blowdectime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_knockdectime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name knockdectime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index knockdectime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.knockdectime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_floatdectime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name floatdectime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index floatdectime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.floatdectime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_superhitdectime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name superhitdectime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index superhitdectime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.superhitdectime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_weaponatt(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name weaponatt");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index weaponatt on a nil value");
			}
		}

		obj.weaponatt = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_pdam(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name pdam");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index pdam on a nil value");
			}
		}

		obj.pdam = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_mdam(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mdam");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mdam on a nil value");
			}
		}

		obj.mdam = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_pdef(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name pdef");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index pdef on a nil value");
			}
		}

		obj.pdef = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_mdef(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mdef");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mdef on a nil value");
			}
		}

		obj.mdef = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_maxhp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxhp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxhp on a nil value");
			}
		}

		obj.maxhp = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_str(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name str");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index str on a nil value");
			}
		}

		obj.str = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_dex(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name dex");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index dex on a nil value");
			}
		}

		obj.dex = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_intel(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name intel");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index intel on a nil value");
			}
		}

		obj.intel = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_phy(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name phy");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index phy on a nil value");
			}
		}

		obj.phy = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bang(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bang");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bang on a nil value");
			}
		}

		obj.bang = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_toughness(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name toughness");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index toughness on a nil value");
			}
		}

		obj.toughness = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_block(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name block");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index block on a nil value");
			}
		}

		obj.block = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_penetrate(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name penetrate");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index penetrate on a nil value");
			}
		}

		obj.penetrate = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_accurate(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name accurate");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index accurate on a nil value");
			}
		}

		obj.accurate = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_hold(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hold");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hold on a nil value");
			}
		}

		obj.hold = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_deflect(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name deflect");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index deflect on a nil value");
			}
		}

		obj.deflect = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bangextradamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bangextradamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bangextradamage on a nil value");
			}
		}

		obj.bangextradamage = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_toughnessreducedamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name toughnessreducedamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index toughnessreducedamage on a nil value");
			}
		}

		obj.toughnessreducedamage = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_blockreducedamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name blockreducedamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index blockreducedamage on a nil value");
			}
		}

		obj.blockreducedamage = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_penetrateextradamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name penetrateextradamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index penetrateextradamage on a nil value");
			}
		}

		obj.penetrateextradamage = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_accurateextradamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name accurateextradamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index accurateextradamage on a nil value");
			}
		}

		obj.accurateextradamage = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_holdreducedamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name holdreducedamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index holdreducedamage on a nil value");
			}
		}

		obj.holdreducedamage = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_deflectreducedamage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name deflectreducedamage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index deflectreducedamage on a nil value");
			}
		}

		obj.deflectreducedamage = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_maxmp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxmp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxmp on a nil value");
			}
		}

		obj.maxmp = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_miss(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name miss");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index miss on a nil value");
			}
		}

		obj.miss = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_hit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hit on a nil value");
			}
		}

		obj.hit = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_firemastery(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name firemastery");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index firemastery on a nil value");
			}
		}

		obj.firemastery = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_icemastery(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name icemastery");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index icemastery on a nil value");
			}
		}

		obj.icemastery = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lightningmastery(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lightningmastery");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lightningmastery on a nil value");
			}
		}

		obj.lightningmastery = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_brightmastery(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name brightmastery");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index brightmastery on a nil value");
			}
		}

		obj.brightmastery = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_darkmastery(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name darkmastery");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index darkmastery on a nil value");
			}
		}

		obj.darkmastery = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_fireresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name fireresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index fireresist on a nil value");
			}
		}

		obj.fireresist = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_iceresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name iceresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index iceresist on a nil value");
			}
		}

		obj.iceresist = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lightningresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lightningresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lightningresist on a nil value");
			}
		}

		obj.lightningresist = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_brightresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name brightresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index brightresist on a nil value");
			}
		}

		obj.brightresist = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_darkresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name darkresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index darkresist on a nil value");
			}
		}

		obj.darkresist = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_firepen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name firepen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index firepen on a nil value");
			}
		}

		obj.firepen = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_icepen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name icepen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index icepen on a nil value");
			}
		}

		obj.icepen = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lightningpen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lightningpen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lightningpen on a nil value");
			}
		}

		obj.lightningpen = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_brightpen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name brightpen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index brightpen on a nil value");
			}
		}

		obj.brightpen = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_darkpen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name darkpen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index darkpen on a nil value");
			}
		}

		obj.darkpen = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_blowint(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name blowint");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index blowint on a nil value");
			}
		}

		obj.blowint = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_knockint(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name knockint");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index knockint on a nil value");
			}
		}

		obj.knockint = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_floatint(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name floatint");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index floatint on a nil value");
			}
		}

		obj.floatint = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_superhitint(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name superhitint");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index superhitint on a nil value");
			}
		}

		obj.superhitint = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_blowresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name blowresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index blowresist on a nil value");
			}
		}

		obj.blowresist = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_knockresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name knockresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index knockresist on a nil value");
			}
		}

		obj.knockresist = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_floatresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name floatresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index floatresist on a nil value");
			}
		}

		obj.floatresist = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_superhitresist(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name superhitresist");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index superhitresist on a nil value");
			}
		}

		obj.superhitresist = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_stiffaddtime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name stiffaddtime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index stiffaddtime on a nil value");
			}
		}

		obj.stiffaddtime = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_stiffdectime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name stiffdectime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index stiffdectime on a nil value");
			}
		}

		obj.stiffdectime = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_blowdectime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name blowdectime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index blowdectime on a nil value");
			}
		}

		obj.blowdectime = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_knockdectime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name knockdectime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index knockdectime on a nil value");
			}
		}

		obj.knockdectime = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_floatdectime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name floatdectime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index floatdectime on a nil value");
			}
		}

		obj.floatdectime = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_superhitdectime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Obj.EquipData obj = (Obj.EquipData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name superhitdectime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index superhitdectime on a nil value");
			}
		}

		obj.superhitdectime = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}
}

