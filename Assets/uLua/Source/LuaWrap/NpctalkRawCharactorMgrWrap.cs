using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;

public class NpctalkRawCharactorMgrWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("OnUpdate", OnUpdate),
			new LuaMethod("OnReSet", OnReSet),
			new LuaMethod("Init", Init),
			new LuaMethod("InitAssetsPosData", InitAssetsPosData),
			new LuaMethod("DisposeCharactor", DisposeCharactor),
			new LuaMethod("CreateModelByNpcId", CreateModelByNpcId),
			new LuaMethod("GetRawCharactor", GetRawCharactor),
			new LuaMethod("New", _CreateNpctalkRawCharactorMgr),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("RTRawActor_L", get_RTRawActor_L, set_RTRawActor_L),
			new LuaField("RTRawActor_M", get_RTRawActor_M, set_RTRawActor_M),
			new LuaField("RTRawActor_R", get_RTRawActor_R, set_RTRawActor_R),
			new LuaField("ManagerName", get_ManagerName, null),
			new LuaField("RTGameRoot", get_RTGameRoot, null),
		};

		LuaScriptMgr.RegisterLib(L, "NpctalkRawCharactorMgr", typeof(NpctalkRawCharactorMgr), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateNpctalkRawCharactorMgr(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			NpctalkRawCharactorMgr obj = new NpctalkRawCharactorMgr();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: NpctalkRawCharactorMgr.New");
		}

		return 0;
	}

	static Type classType = typeof(NpctalkRawCharactorMgr);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_RTRawActor_L(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RTRawActor_L");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RTRawActor_L on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.RTRawActor_L);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_RTRawActor_M(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RTRawActor_M");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RTRawActor_M on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.RTRawActor_M);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_RTRawActor_R(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RTRawActor_R");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RTRawActor_R on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.RTRawActor_R);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ManagerName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)o;

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
	static int get_RTGameRoot(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RTGameRoot");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RTGameRoot on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.RTGameRoot);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_RTRawActor_L(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RTRawActor_L");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RTRawActor_L on a nil value");
			}
		}

		obj.RTRawActor_L = (NpctalkRawCharactor)LuaScriptMgr.GetNetObject(L, 3, typeof(NpctalkRawCharactor));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_RTRawActor_M(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RTRawActor_M");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RTRawActor_M on a nil value");
			}
		}

		obj.RTRawActor_M = (NpctalkRawCharactor)LuaScriptMgr.GetNetObject(L, 3, typeof(NpctalkRawCharactor));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_RTRawActor_R(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RTRawActor_R");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RTRawActor_R on a nil value");
			}
		}

		obj.RTRawActor_R = (NpctalkRawCharactor)LuaScriptMgr.GetNetObject(L, 3, typeof(NpctalkRawCharactor));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnUpdate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "NpctalkRawCharactorMgr");
		obj.OnUpdate();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnReSet(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "NpctalkRawCharactorMgr");
		obj.OnReSet();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "NpctalkRawCharactorMgr");
		obj.Init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitAssetsPosData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "NpctalkRawCharactorMgr");
		Action arg0 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg0 = (Action)LuaScriptMgr.GetNetObject(L, 2, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
			arg0 = () =>
			{
				func.Call();
			};
		}

		obj.InitAssetsPosData(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DisposeCharactor(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "NpctalkRawCharactorMgr");
		obj.DisposeCharactor();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreateModelByNpcId(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "NpctalkRawCharactorMgr");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		Action<GameObject> arg1 = null;
		LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

		if (funcType3 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (Action<GameObject>)LuaScriptMgr.GetNetObject(L, 3, typeof(Action<GameObject>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
			arg1 = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}

		obj.CreateModelByNpcId(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetRawCharactor(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		NpctalkRawCharactorMgr obj = (NpctalkRawCharactorMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "NpctalkRawCharactorMgr");
		RawImage arg0 = (RawImage)LuaScriptMgr.GetUnityObject(L, 2, typeof(RawImage));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 3);
		uint arg2 = (uint)LuaScriptMgr.GetNumber(L, 4);
		obj.GetRawCharactor(arg0,arg1,arg2);
		return 0;
	}
}

