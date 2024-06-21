﻿using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class AnimationClipWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("SampleAnimation", SampleAnimation),
			new LuaMethod("SetCurve", SetCurve),
			new LuaMethod("EnsureQuaternionContinuity", EnsureQuaternionContinuity),
			new LuaMethod("ClearCurves", ClearCurves),
			new LuaMethod("AddEvent", AddEvent),
			new LuaMethod("New", _CreateAnimationClip),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("length", get_length, null),
			new LuaField("frameRate", get_frameRate, set_frameRate),
			new LuaField("wrapMode", get_wrapMode, set_wrapMode),
			new LuaField("localBounds", get_localBounds, set_localBounds),
			new LuaField("legacy", get_legacy, set_legacy),
			new LuaField("humanMotion", get_humanMotion, null),
			new LuaField("events", get_events, set_events),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.AnimationClip", typeof(AnimationClip), regs, fields, typeof(UnityEngine.Object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateAnimationClip(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			AnimationClip obj = new AnimationClip();
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: AnimationClip.New");
		}

		return 0;
	}

	static Type classType = typeof(AnimationClip);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_length(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimationClip obj = (AnimationClip)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name length");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index length on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.length);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_frameRate(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimationClip obj = (AnimationClip)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name frameRate");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index frameRate on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.frameRate);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_wrapMode(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimationClip obj = (AnimationClip)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name wrapMode");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index wrapMode on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.wrapMode);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_localBounds(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimationClip obj = (AnimationClip)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name localBounds");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index localBounds on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.localBounds);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_legacy(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimationClip obj = (AnimationClip)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name legacy");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index legacy on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.legacy);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_humanMotion(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimationClip obj = (AnimationClip)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name humanMotion");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index humanMotion on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.humanMotion);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_events(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimationClip obj = (AnimationClip)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name events");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index events on a nil value");
			}
		}

		LuaScriptMgr.PushArray(L, obj.events);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_frameRate(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimationClip obj = (AnimationClip)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name frameRate");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index frameRate on a nil value");
			}
		}

		obj.frameRate = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_wrapMode(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimationClip obj = (AnimationClip)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name wrapMode");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index wrapMode on a nil value");
			}
		}

		obj.wrapMode = (WrapMode)LuaScriptMgr.GetNetObject(L, 3, typeof(WrapMode));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_localBounds(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimationClip obj = (AnimationClip)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name localBounds");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index localBounds on a nil value");
			}
		}

		obj.localBounds = LuaScriptMgr.GetBounds(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_legacy(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimationClip obj = (AnimationClip)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name legacy");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index legacy on a nil value");
			}
		}

		obj.legacy = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_events(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimationClip obj = (AnimationClip)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name events");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index events on a nil value");
			}
		}

		obj.events = LuaScriptMgr.GetArrayObject<AnimationEvent>(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SampleAnimation(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		AnimationClip obj = (AnimationClip)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AnimationClip");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		obj.SampleAnimation(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCurve(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		AnimationClip obj = (AnimationClip)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AnimationClip");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		Type arg1 = LuaScriptMgr.GetTypeObject(L, 3);
		string arg2 = LuaScriptMgr.GetLuaString(L, 4);
		AnimationCurve arg3 = (AnimationCurve)LuaScriptMgr.GetNetObject(L, 5, typeof(AnimationCurve));
		obj.SetCurve(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EnsureQuaternionContinuity(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		AnimationClip obj = (AnimationClip)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AnimationClip");
		obj.EnsureQuaternionContinuity();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearCurves(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		AnimationClip obj = (AnimationClip)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AnimationClip");
		obj.ClearCurves();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddEvent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		AnimationClip obj = (AnimationClip)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AnimationClip");
		AnimationEvent arg0 = (AnimationEvent)LuaScriptMgr.GetNetObject(L, 2, typeof(AnimationEvent));
		obj.AddEvent(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Lua_Eq(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Object arg0 = LuaScriptMgr.GetLuaObject(L, 1) as Object;
		Object arg1 = LuaScriptMgr.GetLuaObject(L, 2) as Object;
		bool o = arg0 == arg1;
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

