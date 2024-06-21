using System;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using LuaInterface;
using Object = UnityEngine.Object;

public class UITweenerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Sample", Sample),
			new LuaMethod("Play", Play),
			new LuaMethod("Reset", Reset),
			new LuaMethod("Toggle", Toggle),
			new LuaMethod("ResetAndPlayForward", ResetAndPlayForward),
			new LuaMethod("ResetAndPlayReverse", ResetAndPlayReverse),
			new LuaMethod("New", _CreateUITweener),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("strName", get_strName, set_strName),
			new LuaField("onFinished", get_onFinished, set_onFinished),
			new LuaField("onFinishedObjActive", get_onFinishedObjActive, set_onFinishedObjActive),
			new LuaField("onFinishedObjDisActive", get_onFinishedObjDisActive, set_onFinishedObjDisActive),
			new LuaField("onFinishedObjDisActiveList", get_onFinishedObjDisActiveList, set_onFinishedObjDisActiveList),
			new LuaField("method", get_method, set_method),
			new LuaField("style", get_style, set_style),
			new LuaField("animationCurve", get_animationCurve, set_animationCurve),
			new LuaField("ignoreTimeScale", get_ignoreTimeScale, set_ignoreTimeScale),
			new LuaField("delay", get_delay, set_delay),
			new LuaField("duration", get_duration, set_duration),
			new LuaField("steeperCurves", get_steeperCurves, set_steeperCurves),
			new LuaField("tweenGroup", get_tweenGroup, set_tweenGroup),
			new LuaField("eventReceiver", get_eventReceiver, set_eventReceiver),
			new LuaField("callWhenFinished", get_callWhenFinished, set_callWhenFinished),
			new LuaField("IsFirstOne", get_IsFirstOne, set_IsFirstOne),
			new LuaField("amountPerDelta", get_amountPerDelta, null),
			new LuaField("tweenFactor", get_tweenFactor, null),
			new LuaField("direction", get_direction, null),
		};

		LuaScriptMgr.RegisterLib(L, "UITweener", typeof(UITweener), regs, fields, typeof(IgnoreTimeScale));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUITweener(IntPtr L)
	{
		LuaDLL.luaL_error(L, "UITweener class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(UITweener);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_strName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name strName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index strName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.strName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onFinished(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onFinished");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onFinished on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.onFinished);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onFinishedObjActive(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onFinishedObjActive");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onFinishedObjActive on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.onFinishedObjActive);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onFinishedObjDisActive(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onFinishedObjDisActive");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onFinishedObjDisActive on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.onFinishedObjDisActive);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onFinishedObjDisActiveList(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onFinishedObjDisActiveList");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onFinishedObjDisActiveList on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.onFinishedObjDisActiveList);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_method(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name method");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index method on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.method);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_style(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name style");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index style on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.style);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_animationCurve(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name animationCurve");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index animationCurve on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.animationCurve);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ignoreTimeScale(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ignoreTimeScale");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ignoreTimeScale on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ignoreTimeScale);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_delay(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name delay");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index delay on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.delay);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_duration(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name duration");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index duration on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.duration);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_steeperCurves(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name steeperCurves");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index steeperCurves on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.steeperCurves);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_tweenGroup(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name tweenGroup");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index tweenGroup on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.tweenGroup);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_eventReceiver(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name eventReceiver");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index eventReceiver on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.eventReceiver);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_callWhenFinished(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name callWhenFinished");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index callWhenFinished on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.callWhenFinished);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsFirstOne(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsFirstOne");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsFirstOne on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsFirstOne);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_amountPerDelta(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name amountPerDelta");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index amountPerDelta on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.amountPerDelta);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_tweenFactor(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name tweenFactor");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index tweenFactor on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.tweenFactor);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_direction(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name direction");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index direction on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.direction);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_strName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name strName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index strName on a nil value");
			}
		}

		obj.strName = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onFinished(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onFinished");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onFinished on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.onFinished = (UITweener.OnFinished)LuaScriptMgr.GetNetObject(L, 3, typeof(UITweener.OnFinished));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.onFinished = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onFinishedObjActive(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onFinishedObjActive");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onFinishedObjActive on a nil value");
			}
		}

		obj.onFinishedObjActive = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onFinishedObjDisActive(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onFinishedObjDisActive");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onFinishedObjDisActive on a nil value");
			}
		}

		obj.onFinishedObjDisActive = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onFinishedObjDisActiveList(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onFinishedObjDisActiveList");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onFinishedObjDisActiveList on a nil value");
			}
		}

		obj.onFinishedObjDisActiveList = (List<GameObject>)LuaScriptMgr.GetNetObject(L, 3, typeof(List<GameObject>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_method(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name method");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index method on a nil value");
			}
		}

		obj.method = (UITweener.Method)LuaScriptMgr.GetNetObject(L, 3, typeof(UITweener.Method));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_style(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name style");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index style on a nil value");
			}
		}

		obj.style = (UITweener.Style)LuaScriptMgr.GetNetObject(L, 3, typeof(UITweener.Style));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_animationCurve(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name animationCurve");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index animationCurve on a nil value");
			}
		}

		obj.animationCurve = (AnimationCurve)LuaScriptMgr.GetNetObject(L, 3, typeof(AnimationCurve));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ignoreTimeScale(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ignoreTimeScale");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ignoreTimeScale on a nil value");
			}
		}

		obj.ignoreTimeScale = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_delay(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name delay");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index delay on a nil value");
			}
		}

		obj.delay = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_duration(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name duration");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index duration on a nil value");
			}
		}

		obj.duration = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_steeperCurves(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name steeperCurves");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index steeperCurves on a nil value");
			}
		}

		obj.steeperCurves = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_tweenGroup(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name tweenGroup");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index tweenGroup on a nil value");
			}
		}

		obj.tweenGroup = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_eventReceiver(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name eventReceiver");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index eventReceiver on a nil value");
			}
		}

		obj.eventReceiver = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_callWhenFinished(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name callWhenFinished");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index callWhenFinished on a nil value");
			}
		}

		obj.callWhenFinished = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_IsFirstOne(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITweener obj = (UITweener)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsFirstOne");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsFirstOne on a nil value");
			}
		}

		obj.IsFirstOne = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Sample(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		UITweener obj = (UITweener)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UITweener");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.Sample(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Play(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UITweener obj = (UITweener)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UITweener");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.Play(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Reset(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UITweener obj = (UITweener)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UITweener");
		obj.Reset();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Toggle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UITweener obj = (UITweener)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UITweener");
		obj.Toggle();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetAndPlayForward(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UITweener obj = (UITweener)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UITweener");
		obj.ResetAndPlayForward();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetAndPlayReverse(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UITweener obj = (UITweener)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UITweener");
		obj.ResetAndPlayReverse();
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

