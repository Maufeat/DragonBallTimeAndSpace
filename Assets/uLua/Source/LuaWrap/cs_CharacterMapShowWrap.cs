using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class cs_CharacterMapShowWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _Createcs_CharacterMapShow),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("bodystyle", get_bodystyle, set_bodystyle),
			new LuaField("haircolor", get_haircolor, set_haircolor),
			new LuaField("hairstyle", get_hairstyle, set_hairstyle),
			new LuaField("facestyle", get_facestyle, set_facestyle),
			new LuaField("antenna", get_antenna, set_antenna),
			new LuaField("heroid", get_heroid, set_heroid),
			new LuaField("face", get_face, set_face),
			new LuaField("weapon", get_weapon, set_weapon),
			new LuaField("coat", get_coat, set_coat),
			new LuaField("occupation", get_occupation, null),
			new LuaField("avatarid", get_avatarid, set_avatarid),
		};

		LuaScriptMgr.RegisterLib(L, "cs_CharacterMapShow", typeof(cs_CharacterMapShow), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _Createcs_CharacterMapShow(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			cs_CharacterMapShow obj = new cs_CharacterMapShow();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else if (count == 1)
		{
			msg.CharacterMapShow arg0 = (msg.CharacterMapShow)LuaScriptMgr.GetNetObject(L, 1, typeof(msg.CharacterMapShow));
			cs_CharacterMapShow obj = new cs_CharacterMapShow(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: cs_CharacterMapShow.New");
		}

		return 0;
	}

	static Type classType = typeof(cs_CharacterMapShow);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bodystyle(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bodystyle");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bodystyle on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.bodystyle);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_haircolor(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name haircolor");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index haircolor on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.haircolor);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_hairstyle(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hairstyle");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hairstyle on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.hairstyle);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_facestyle(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name facestyle");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index facestyle on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.facestyle);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_antenna(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name antenna");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index antenna on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.antenna);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_heroid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name heroid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index heroid on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.heroid);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_face(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name face");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index face on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.face);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_weapon(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name weapon");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index weapon on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.weapon);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_coat(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name coat");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index coat on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.coat);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_occupation(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name occupation");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index occupation on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.occupation);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_avatarid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name avatarid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index avatarid on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.avatarid);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bodystyle(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bodystyle");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bodystyle on a nil value");
			}
		}

		obj.bodystyle = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_haircolor(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name haircolor");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index haircolor on a nil value");
			}
		}

		obj.haircolor = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_hairstyle(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hairstyle");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hairstyle on a nil value");
			}
		}

		obj.hairstyle = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_facestyle(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name facestyle");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index facestyle on a nil value");
			}
		}

		obj.facestyle = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_antenna(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name antenna");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index antenna on a nil value");
			}
		}

		obj.antenna = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_heroid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name heroid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index heroid on a nil value");
			}
		}

		obj.heroid = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_face(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name face");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index face on a nil value");
			}
		}

		obj.face = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_weapon(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name weapon");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index weapon on a nil value");
			}
		}

		obj.weapon = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_coat(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name coat");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index coat on a nil value");
			}
		}

		obj.coat = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_avatarid(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		cs_CharacterMapShow obj = (cs_CharacterMapShow)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name avatarid");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index avatarid on a nil value");
			}
		}

		obj.avatarid = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}
}

