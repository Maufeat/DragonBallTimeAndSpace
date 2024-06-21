using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class EntitiesIDWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("ToEntryIDType", ToEntryIDType),
			new LuaMethod("ToString", ToString),
			new LuaMethod("Equals", Equals),
			new LuaMethod("New", _CreateEntitiesID),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__tostring", Lua_ToString),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Id", get_Id, set_Id),
			new LuaField("Etype", get_Etype, set_Etype),
			new LuaField("IDTypeStr", get_IDTypeStr, null),
			new LuaField("IDTypeNPCStr", get_IDTypeNPCStr, null),
			new LuaField("IdStr", get_IdStr, null),
		};

		LuaScriptMgr.RegisterLib(L, "EntitiesID", typeof(EntitiesID), regs, fields, null);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateEntitiesID(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			msg.EntryIDType arg0 = (msg.EntryIDType)LuaScriptMgr.GetNetObject(L, 1, typeof(msg.EntryIDType));
			EntitiesID obj = new EntitiesID(arg0);
			LuaScriptMgr.PushValue(L, obj);
			return 1;
		}
		else if (count == 2)
		{
			ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 1);
			CharactorType arg1 = (CharactorType)LuaScriptMgr.GetNetObject(L, 2, typeof(CharactorType));
			EntitiesID obj = new EntitiesID(arg0,arg1);
			LuaScriptMgr.PushValue(L, obj);
			return 1;
		}
		else if (count == 0)
		{
			EntitiesID obj = new EntitiesID();
			LuaScriptMgr.PushValue(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: EntitiesID.New");
		}

		return 0;
	}

	static Type classType = typeof(EntitiesID);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Id(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);

		if (o == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Id");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Id on a nil value");
			}
		}

		EntitiesID obj = (EntitiesID)o;
		LuaScriptMgr.Push(L, obj.Id);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Etype(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);

		if (o == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Etype");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Etype on a nil value");
			}
		}

		EntitiesID obj = (EntitiesID)o;
		LuaScriptMgr.Push(L, obj.Etype);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IDTypeStr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);

		if (o == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IDTypeStr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IDTypeStr on a nil value");
			}
		}

		EntitiesID obj = (EntitiesID)o;
		LuaScriptMgr.Push(L, obj.IDTypeStr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IDTypeNPCStr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);

		if (o == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IDTypeNPCStr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IDTypeNPCStr on a nil value");
			}
		}

		EntitiesID obj = (EntitiesID)o;
		LuaScriptMgr.Push(L, obj.IDTypeNPCStr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IdStr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);

		if (o == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IdStr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IdStr on a nil value");
			}
		}

		EntitiesID obj = (EntitiesID)o;
		LuaScriptMgr.Push(L, obj.IdStr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Id(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);

		if (o == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Id");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Id on a nil value");
			}
		}

		EntitiesID obj = (EntitiesID)o;
		obj.Id = (ulong)LuaScriptMgr.GetNumber(L, 3);
		LuaScriptMgr.SetValueObject(L, 1, obj);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Etype(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);

		if (o == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Etype");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Etype on a nil value");
			}
		}

		EntitiesID obj = (EntitiesID)o;
		obj.Etype = (CharactorType)LuaScriptMgr.GetNetObject(L, 3, typeof(CharactorType));
		LuaScriptMgr.SetValueObject(L, 1, obj);
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
			LuaScriptMgr.Push(L, "Table: EntitiesID");
		}

		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToEntryIDType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		EntitiesID obj = (EntitiesID)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesID");
		msg.EntryIDType o = obj.ToEntryIDType();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		EntitiesID obj = (EntitiesID)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesID");
		string o = obj.ToString();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Equals(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(EntitiesID), typeof(msg.EntryIDType)))
		{
			EntitiesID obj = (EntitiesID)LuaScriptMgr.GetVarObject(L, 1);
			msg.EntryIDType arg0 = (msg.EntryIDType)LuaScriptMgr.GetLuaObject(L, 2);
			bool o = obj.Equals(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(EntitiesID), typeof(EntitiesID)))
		{
			EntitiesID obj = (EntitiesID)LuaScriptMgr.GetVarObject(L, 1);
			EntitiesID arg0 = (EntitiesID)LuaScriptMgr.GetLuaObject(L, 2);
			bool o = obj.Equals(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: EntitiesID.Equals");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Lua_Eq(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesID arg0 = (EntitiesID)LuaScriptMgr.GetVarObject(L, 1);
		EntitiesID arg1 = (EntitiesID)LuaScriptMgr.GetVarObject(L, 2);
		bool o = arg0 == arg1;
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

