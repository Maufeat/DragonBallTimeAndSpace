using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class cs_CharacterMapDataWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _Createcs_CharacterMapData),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("level", get_level, set_level),
			new LuaField("pos", get_pos, set_pos),
			new LuaField("dir", get_dir, set_dir),
			new LuaField("movespeed", get_movespeed, set_movespeed),
			new LuaField("hp", get_hp, set_hp),
			new LuaField("maxhp", get_maxhp, set_maxhp),
			new LuaField("state", get_state, null),
			new LuaField("lstState", get_lstState, null),
			new LuaField("teamid", get_teamid, set_teamid),
			new LuaField("guildid", get_guildid, set_guildid),
			new LuaField("country", get_country, set_country),
			new LuaField("guildname", get_guildname, set_guildname),
			new LuaField("titlename", get_titlename, set_titlename),
		};

		LuaScriptMgr.RegisterLib(L, "cs_CharacterMapData", typeof(cs_CharacterMapData), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _Createcs_CharacterMapData(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			cs_CharacterMapData obj = new cs_CharacterMapData();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(NpcMasterData)))
		{
			NpcMasterData arg0 = (NpcMasterData)LuaScriptMgr.GetNetObject(L, 1, typeof(NpcMasterData));
			cs_CharacterMapData obj = new cs_CharacterMapData(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(msg.CharacterMapData)))
		{
			msg.CharacterMapData arg0 = (msg.CharacterMapData)LuaScriptMgr.GetNetObject(L, 1, typeof(msg.CharacterMapData));
			cs_CharacterMapData obj = new cs_CharacterMapData(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: cs_CharacterMapData.New");
		}

		return 0;
	}

	static Type classType = typeof(cs_CharacterMapData);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_level(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

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
	static int get_pos(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name pos");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index pos on a nil value");
			}
		}

		LuaScriptMgr.PushValue(L, obj.pos);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_dir(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name dir");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index dir on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.dir);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_movespeed(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name movespeed");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index movespeed on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.movespeed);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_hp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hp on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.hp);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_maxhp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

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
	static int get_state(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name state");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index state on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.state);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lstState(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lstState");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lstState on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.lstState);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_teamid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name teamid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index teamid on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.teamid);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_guildid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name guildid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index guildid on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.guildid);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_country(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name country");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index country on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.country);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_guildname(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name guildname");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index guildname on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.guildname);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_titlename(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name titlename");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index titlename on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.titlename);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_level(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

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
	static int set_pos(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name pos");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index pos on a nil value");
			}
		}

		obj.pos = (cs_FloatMovePos)LuaScriptMgr.GetNetObject(L, 3, typeof(cs_FloatMovePos));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_dir(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name dir");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index dir on a nil value");
			}
		}

		obj.dir = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_movespeed(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name movespeed");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index movespeed on a nil value");
			}
		}

		obj.movespeed = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_hp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hp on a nil value");
			}
		}

		obj.hp = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_maxhp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

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
	static int set_teamid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name teamid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index teamid on a nil value");
			}
		}

		obj.teamid = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_guildid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name guildid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index guildid on a nil value");
			}
		}

		obj.guildid = (ulong)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_country(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name country");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index country on a nil value");
			}
		}

		obj.country = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_guildname(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name guildname");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index guildname on a nil value");
			}
		}

		obj.guildname = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_titlename(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapData obj = (cs_CharacterMapData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name titlename");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index titlename on a nil value");
			}
		}

		obj.titlename = LuaScriptMgr.GetString(L, 3);
		return 0;
	}
}

