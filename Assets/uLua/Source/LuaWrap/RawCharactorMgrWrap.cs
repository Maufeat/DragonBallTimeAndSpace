using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using LuaInterface;

public class RawCharactorMgrWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("OnUpdate", OnUpdate),
			new LuaMethod("OnReSet", OnReSet),
			new LuaMethod("Init", Init),
			new LuaMethod("GetRawCharacterMgr", GetRawCharacterMgr),
			new LuaMethod("DisposeCharactor", DisposeCharactor),
			new LuaMethod("ChangeModel", ChangeModel),
			new LuaMethod("ChangeWeapon", ChangeWeapon),
			new LuaMethod("GetRawCharactor", GetRawCharactor),
			new LuaMethod("GetModelObj", GetModelObj),
			new LuaMethod("DragRTView", DragRTView),
			new LuaMethod("New", _CreateRawCharactorMgr),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("UINeedRaw", get_UINeedRaw, set_UINeedRaw),
			new LuaField("ManagerName", get_ManagerName, null),
		};

		LuaScriptMgr.RegisterLib(L, "RawCharactorMgr", typeof(RawCharactorMgr), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateRawCharactorMgr(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			RawCharactorMgr obj = new RawCharactorMgr();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: RawCharactorMgr.New");
		}

		return 0;
	}

	static Type classType = typeof(RawCharactorMgr);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UINeedRaw(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RawCharactorMgr obj = (RawCharactorMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name UINeedRaw");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index UINeedRaw on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.UINeedRaw);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ManagerName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RawCharactorMgr obj = (RawCharactorMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ManagerName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ManagerName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ManagerName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_UINeedRaw(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RawCharactorMgr obj = (RawCharactorMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name UINeedRaw");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index UINeedRaw on a nil value");
			}
		}

		obj.UINeedRaw = (List<string>)LuaScriptMgr.GetNetObject(L, 3, typeof(List<string>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnUpdate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RawCharactorMgr obj = (RawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RawCharactorMgr");
		obj.OnUpdate();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnReSet(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RawCharactorMgr obj = (RawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RawCharactorMgr");
		obj.OnReSet();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RawCharactorMgr obj = (RawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RawCharactorMgr");
		obj.Init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetRawCharacterMgr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RawCharactorMgr obj = (RawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RawCharactorMgr");
		ModelUI arg0 = (ModelUI)LuaScriptMgr.GetNetObject(L, 2, typeof(ModelUI));
		RawCharactorMgr o = obj.GetRawCharacterMgr(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DisposeCharactor(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RawCharactorMgr obj = (RawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RawCharactorMgr");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.DisposeCharactor(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ChangeModel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RawCharactorMgr obj = (RawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RawCharactorMgr");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.ChangeModel(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ChangeWeapon(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RawCharactorMgr obj = (RawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RawCharactorMgr");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		obj.ChangeWeapon(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetRawCharactor(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		RawCharactorMgr obj = (RawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RawCharactorMgr");
		RawImage arg0 = (RawImage)LuaScriptMgr.GetUnityObject(L, 2, typeof(RawImage));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 3);
		string arg2 = LuaScriptMgr.GetLuaString(L, 4);
		int arg3 = (int)LuaScriptMgr.GetNumber(L, 5);
		obj.GetRawCharactor(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetModelObj(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RawCharactorMgr obj = (RawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RawCharactorMgr");
		GameObject o = obj.GetModelObj();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DragRTView(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RawCharactorMgr obj = (RawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RawCharactorMgr");
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
		obj.DragRTView(arg0);
		return 0;
	}
}

