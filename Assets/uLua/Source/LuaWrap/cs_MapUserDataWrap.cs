using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class cs_MapUserDataWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("isSameExceptSpeetAndPosition", isSameExceptSpeetAndPosition),
			new LuaMethod("New", _Createcs_MapUserData),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("charid", get_charid, set_charid),
			new LuaField("name", get_name, set_name),
			new LuaField("guildname", get_guildname, set_guildname),
			new LuaField("mapshow", get_mapshow, set_mapshow),
			new LuaField("bakmapshow", get_bakmapshow, set_bakmapshow),
			new LuaField("mapdata", get_mapdata, set_mapdata),
			new LuaField("appearanceid", get_appearanceid, null),
		};

		LuaScriptMgr.RegisterLib(L, "cs_MapUserData", typeof(cs_MapUserData), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _Createcs_MapUserData(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			cs_MapUserData obj = new cs_MapUserData();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else if (count == 1)
		{
			msg.MapUserData arg0 = (msg.MapUserData)LuaScriptMgr.GetNetObject(L, 1, typeof(msg.MapUserData));
			cs_MapUserData obj = new cs_MapUserData(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: cs_MapUserData.New");
		}

		return 0;
	}

	static Type classType = typeof(cs_MapUserData);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_charid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_MapUserData obj = (cs_MapUserData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name charid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index charid on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.charid);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_name(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_MapUserData obj = (cs_MapUserData)o;

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
	static int get_guildname(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_MapUserData obj = (cs_MapUserData)o;

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
	static int get_mapshow(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_MapUserData obj = (cs_MapUserData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mapshow");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mapshow on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.mapshow);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bakmapshow(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_MapUserData obj = (cs_MapUserData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bakmapshow");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bakmapshow on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.bakmapshow);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_mapdata(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_MapUserData obj = (cs_MapUserData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mapdata");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mapdata on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.mapdata);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_appearanceid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_MapUserData obj = (cs_MapUserData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name appearanceid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index appearanceid on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.appearanceid);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_charid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_MapUserData obj = (cs_MapUserData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name charid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index charid on a nil value");
			}
		}

		obj.charid = (ulong)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_name(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_MapUserData obj = (cs_MapUserData)o;

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
	static int set_guildname(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_MapUserData obj = (cs_MapUserData)o;

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
	static int set_mapshow(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_MapUserData obj = (cs_MapUserData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mapshow");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mapshow on a nil value");
			}
		}

		obj.mapshow = (cs_CharacterMapShow)LuaScriptMgr.GetNetObject(L, 3, typeof(cs_CharacterMapShow));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bakmapshow(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_MapUserData obj = (cs_MapUserData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bakmapshow");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bakmapshow on a nil value");
			}
		}

		obj.bakmapshow = (cs_CharacterMapShow)LuaScriptMgr.GetNetObject(L, 3, typeof(cs_CharacterMapShow));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_mapdata(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_MapUserData obj = (cs_MapUserData)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mapdata");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mapdata on a nil value");
			}
		}

		obj.mapdata = (cs_CharacterMapData)LuaScriptMgr.GetNetObject(L, 3, typeof(cs_CharacterMapData));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isSameExceptSpeetAndPosition(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		cs_MapUserData obj = (cs_MapUserData)LuaScriptMgr.GetNetObjectSelf(L, 1, "cs_MapUserData");
		cs_MapUserData arg0 = (cs_MapUserData)LuaScriptMgr.GetNetObject(L, 2, typeof(cs_MapUserData));
		bool o = obj.isSameExceptSpeetAndPosition(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

