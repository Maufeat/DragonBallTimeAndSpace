﻿using System;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
using LuaInterface;
using Object = UnityEngine.Object;

public class AnimationWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Stop", Stop),
			new LuaMethod("Rewind", Rewind),
			new LuaMethod("Sample", Sample),
			new LuaMethod("IsPlaying", IsPlaying),
			new LuaMethod("get_Item", get_Item),
			new LuaMethod("Play", Play),
			new LuaMethod("CrossFade", CrossFade),
			new LuaMethod("Blend", Blend),
			new LuaMethod("CrossFadeQueued", CrossFadeQueued),
			new LuaMethod("PlayQueued", PlayQueued),
			new LuaMethod("AddClip", AddClip),
			new LuaMethod("RemoveClip", RemoveClip),
			new LuaMethod("GetClipCount", GetClipCount),
			new LuaMethod("SyncLayer", SyncLayer),
			new LuaMethod("GetEnumerator", GetEnumerator),
			new LuaMethod("GetClip", GetClip),
			new LuaMethod("New", _CreateAnimation),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("clip", get_clip, set_clip),
			new LuaField("playAutomatically", get_playAutomatically, set_playAutomatically),
			new LuaField("wrapMode", get_wrapMode, set_wrapMode),
			new LuaField("isPlaying", get_isPlaying, null),
			new LuaField("animatePhysics", get_animatePhysics, set_animatePhysics),
			new LuaField("cullingType", get_cullingType, set_cullingType),
			new LuaField("localBounds", get_localBounds, set_localBounds),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.Animation", typeof(Animation), regs, fields, typeof(Behaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateAnimation(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Animation obj = new Animation();
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Animation.New");
		}

		return 0;
	}

	static Type classType = typeof(Animation);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_clip(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Animation obj = (Animation)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name clip");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index clip on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.clip);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_playAutomatically(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Animation obj = (Animation)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name playAutomatically");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index playAutomatically on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.playAutomatically);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_wrapMode(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Animation obj = (Animation)o;

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
	static int get_isPlaying(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Animation obj = (Animation)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isPlaying");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isPlaying on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.isPlaying);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_animatePhysics(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Animation obj = (Animation)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name animatePhysics");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index animatePhysics on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.animatePhysics);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_cullingType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Animation obj = (Animation)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name cullingType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index cullingType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.cullingType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_localBounds(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Animation obj = (Animation)o;

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
	static int set_clip(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Animation obj = (Animation)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name clip");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index clip on a nil value");
			}
		}

		obj.clip = (AnimationClip)LuaScriptMgr.GetUnityObject(L, 3, typeof(AnimationClip));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_playAutomatically(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Animation obj = (Animation)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name playAutomatically");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index playAutomatically on a nil value");
			}
		}

		obj.playAutomatically = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_wrapMode(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Animation obj = (Animation)o;

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
	static int set_animatePhysics(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Animation obj = (Animation)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name animatePhysics");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index animatePhysics on a nil value");
			}
		}

		obj.animatePhysics = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_cullingType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Animation obj = (Animation)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name cullingType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index cullingType on a nil value");
			}
		}

		obj.cullingType = (AnimationCullingType)LuaScriptMgr.GetNetObject(L, 3, typeof(AnimationCullingType));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_localBounds(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Animation obj = (Animation)o;

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
	static int Stop(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			obj.Stop();
			return 0;
		}
		else if (count == 2)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			obj.Stop(arg0);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Animation.Stop");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Rewind(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			obj.Rewind();
			return 0;
		}
		else if (count == 2)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			obj.Rewind(arg0);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Animation.Rewind");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Sample(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
		obj.Sample();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsPlaying(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		bool o = obj.IsPlaying(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Item(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		AnimationState o = obj[arg0];
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Play(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			bool o = obj.Play();
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Animation), typeof(string)))
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			bool o = obj.Play(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Animation), typeof(PlayMode)))
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			PlayMode arg0 = (PlayMode)LuaScriptMgr.GetLuaObject(L, 2);
			bool o = obj.Play(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			PlayMode arg1 = (PlayMode)LuaScriptMgr.GetNetObject(L, 3, typeof(PlayMode));
			bool o = obj.Play(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Animation.Play");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CrossFade(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			obj.CrossFade(arg0);
			return 0;
		}
		else if (count == 3)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
			obj.CrossFade(arg0,arg1);
			return 0;
		}
		else if (count == 4)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
			PlayMode arg2 = (PlayMode)LuaScriptMgr.GetNetObject(L, 4, typeof(PlayMode));
			obj.CrossFade(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Animation.CrossFade");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Blend(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			obj.Blend(arg0);
			return 0;
		}
		else if (count == 3)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
			obj.Blend(arg0,arg1);
			return 0;
		}
		else if (count == 4)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
			float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
			obj.Blend(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Animation.Blend");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CrossFadeQueued(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			AnimationState o = obj.CrossFadeQueued(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
			AnimationState o = obj.CrossFadeQueued(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 4)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
			QueueMode arg2 = (QueueMode)LuaScriptMgr.GetNetObject(L, 4, typeof(QueueMode));
			AnimationState o = obj.CrossFadeQueued(arg0,arg1,arg2);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 5)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
			QueueMode arg2 = (QueueMode)LuaScriptMgr.GetNetObject(L, 4, typeof(QueueMode));
			PlayMode arg3 = (PlayMode)LuaScriptMgr.GetNetObject(L, 5, typeof(PlayMode));
			AnimationState o = obj.CrossFadeQueued(arg0,arg1,arg2,arg3);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Animation.CrossFadeQueued");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayQueued(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			AnimationState o = obj.PlayQueued(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			QueueMode arg1 = (QueueMode)LuaScriptMgr.GetNetObject(L, 3, typeof(QueueMode));
			AnimationState o = obj.PlayQueued(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 4)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			QueueMode arg1 = (QueueMode)LuaScriptMgr.GetNetObject(L, 3, typeof(QueueMode));
			PlayMode arg2 = (PlayMode)LuaScriptMgr.GetNetObject(L, 4, typeof(PlayMode));
			AnimationState o = obj.PlayQueued(arg0,arg1,arg2);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Animation.PlayQueued");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddClip(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			AnimationClip arg0 = (AnimationClip)LuaScriptMgr.GetUnityObject(L, 2, typeof(AnimationClip));
			string arg1 = LuaScriptMgr.GetLuaString(L, 3);
			obj.AddClip(arg0,arg1);
			return 0;
		}
		else if (count == 5)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			AnimationClip arg0 = (AnimationClip)LuaScriptMgr.GetUnityObject(L, 2, typeof(AnimationClip));
			string arg1 = LuaScriptMgr.GetLuaString(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			int arg3 = (int)LuaScriptMgr.GetNumber(L, 5);
			obj.AddClip(arg0,arg1,arg2,arg3);
			return 0;
		}
		else if (count == 6)
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			AnimationClip arg0 = (AnimationClip)LuaScriptMgr.GetUnityObject(L, 2, typeof(AnimationClip));
			string arg1 = LuaScriptMgr.GetLuaString(L, 3);
			int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
			int arg3 = (int)LuaScriptMgr.GetNumber(L, 5);
			bool arg4 = LuaScriptMgr.GetBoolean(L, 6);
			obj.AddClip(arg0,arg1,arg2,arg3,arg4);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Animation.AddClip");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveClip(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Animation), typeof(string)))
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			obj.RemoveClip(arg0);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Animation), typeof(AnimationClip)))
		{
			Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
			AnimationClip arg0 = (AnimationClip)LuaScriptMgr.GetLuaObject(L, 2);
			obj.RemoveClip(arg0);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Animation.RemoveClip");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClipCount(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
		int o = obj.GetClipCount();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SyncLayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		obj.SyncLayer(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetEnumerator(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
		IEnumerator o = obj.GetEnumerator();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClip(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Animation obj = (Animation)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Animation");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		AnimationClip o = obj.GetClip(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
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

