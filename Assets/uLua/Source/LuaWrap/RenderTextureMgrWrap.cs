using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;

public class RenderTextureMgrWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Init", Init),
			new LuaMethod("UseRTGameObject", UseRTGameObject),
			new LuaMethod("FreeRTGameObject", FreeRTGameObject),
			new LuaMethod("ResetRootPosition", ResetRootPosition),
			new LuaMethod("OnUpdate", OnUpdate),
			new LuaMethod("OnReSet", OnReSet),
			new LuaMethod("New", _CreateRenderTextureMgr),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("ManagerName", get_ManagerName, null),
		};

		LuaScriptMgr.RegisterLib(L, "RenderTextureMgr", typeof(RenderTextureMgr), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateRenderTextureMgr(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			RenderTextureMgr obj = new RenderTextureMgr();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: RenderTextureMgr.New");
		}

		return 0;
	}

	static Type classType = typeof(RenderTextureMgr);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ManagerName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RenderTextureMgr obj = (RenderTextureMgr)o;

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
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RenderTextureMgr obj = (RenderTextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RenderTextureMgr");
		obj.Init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UseRTGameObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RenderTextureMgr obj = (RenderTextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RenderTextureMgr");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		GameObject o = obj.UseRTGameObject(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FreeRTGameObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RenderTextureMgr obj = (RenderTextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RenderTextureMgr");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.FreeRTGameObject(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetRootPosition(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		RenderTextureMgr obj = (RenderTextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RenderTextureMgr");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 3);
		obj.ResetRootPosition(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnUpdate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RenderTextureMgr obj = (RenderTextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RenderTextureMgr");
		obj.OnUpdate();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnReSet(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RenderTextureMgr obj = (RenderTextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "RenderTextureMgr");
		obj.OnReSet();
		return 0;
	}
}

