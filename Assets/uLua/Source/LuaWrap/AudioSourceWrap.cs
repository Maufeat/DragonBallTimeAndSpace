﻿using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class AudioSourceWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Play", Play),
			new LuaMethod("PlayDelayed", PlayDelayed),
			new LuaMethod("PlayScheduled", PlayScheduled),
			new LuaMethod("SetScheduledStartTime", SetScheduledStartTime),
			new LuaMethod("SetScheduledEndTime", SetScheduledEndTime),
			new LuaMethod("Stop", Stop),
			new LuaMethod("Pause", Pause),
			new LuaMethod("UnPause", UnPause),
			new LuaMethod("PlayOneShot", PlayOneShot),
			new LuaMethod("PlayClipAtPoint", PlayClipAtPoint),
			new LuaMethod("SetCustomCurve", SetCustomCurve),
			new LuaMethod("GetCustomCurve", GetCustomCurve),
			new LuaMethod("GetOutputData", GetOutputData),
			new LuaMethod("GetSpectrumData", GetSpectrumData),
			new LuaMethod("SetSpatializerFloat", SetSpatializerFloat),
			new LuaMethod("GetSpatializerFloat", GetSpatializerFloat),
			new LuaMethod("New", _CreateAudioSource),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("volume", get_volume, set_volume),
			new LuaField("pitch", get_pitch, set_pitch),
			new LuaField("time", get_time, set_time),
			new LuaField("timeSamples", get_timeSamples, set_timeSamples),
			new LuaField("clip", get_clip, set_clip),
			new LuaField("outputAudioMixerGroup", get_outputAudioMixerGroup, set_outputAudioMixerGroup),
			new LuaField("isPlaying", get_isPlaying, null),
			new LuaField("isVirtual", get_isVirtual, null),
			new LuaField("loop", get_loop, set_loop),
			new LuaField("ignoreListenerVolume", get_ignoreListenerVolume, set_ignoreListenerVolume),
			new LuaField("playOnAwake", get_playOnAwake, set_playOnAwake),
			new LuaField("ignoreListenerPause", get_ignoreListenerPause, set_ignoreListenerPause),
			new LuaField("velocityUpdateMode", get_velocityUpdateMode, set_velocityUpdateMode),
			new LuaField("panStereo", get_panStereo, set_panStereo),
			new LuaField("spatialBlend", get_spatialBlend, set_spatialBlend),
			new LuaField("spatialize", get_spatialize, set_spatialize),
			new LuaField("reverbZoneMix", get_reverbZoneMix, set_reverbZoneMix),
			new LuaField("bypassEffects", get_bypassEffects, set_bypassEffects),
			new LuaField("bypassListenerEffects", get_bypassListenerEffects, set_bypassListenerEffects),
			new LuaField("bypassReverbZones", get_bypassReverbZones, set_bypassReverbZones),
			new LuaField("dopplerLevel", get_dopplerLevel, set_dopplerLevel),
			new LuaField("spread", get_spread, set_spread),
			new LuaField("priority", get_priority, set_priority),
			new LuaField("mute", get_mute, set_mute),
			new LuaField("minDistance", get_minDistance, set_minDistance),
			new LuaField("maxDistance", get_maxDistance, set_maxDistance),
			new LuaField("rolloffMode", get_rolloffMode, set_rolloffMode),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.AudioSource", typeof(AudioSource), regs, fields, typeof(Behaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateAudioSource(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			AudioSource obj = new AudioSource();
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: AudioSource.New");
		}

		return 0;
	}

	static Type classType = typeof(AudioSource);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_volume(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name volume");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index volume on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.volume);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_pitch(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name pitch");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index pitch on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.pitch);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_time(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name time");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index time on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.time);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_timeSamples(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name timeSamples");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index timeSamples on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.timeSamples);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_clip(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

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
	static int get_outputAudioMixerGroup(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name outputAudioMixerGroup");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index outputAudioMixerGroup on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.outputAudioMixerGroup);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isPlaying(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

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
	static int get_isVirtual(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isVirtual");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isVirtual on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.isVirtual);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_loop(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name loop");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index loop on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.loop);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ignoreListenerVolume(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ignoreListenerVolume");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ignoreListenerVolume on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ignoreListenerVolume);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_playOnAwake(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name playOnAwake");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index playOnAwake on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.playOnAwake);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ignoreListenerPause(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ignoreListenerPause");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ignoreListenerPause on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ignoreListenerPause);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_velocityUpdateMode(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name velocityUpdateMode");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index velocityUpdateMode on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.velocityUpdateMode);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_panStereo(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name panStereo");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index panStereo on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.panStereo);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_spatialBlend(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name spatialBlend");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index spatialBlend on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.spatialBlend);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_spatialize(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name spatialize");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index spatialize on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.spatialize);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_reverbZoneMix(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name reverbZoneMix");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index reverbZoneMix on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.reverbZoneMix);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bypassEffects(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bypassEffects");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bypassEffects on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.bypassEffects);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bypassListenerEffects(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bypassListenerEffects");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bypassListenerEffects on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.bypassListenerEffects);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bypassReverbZones(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bypassReverbZones");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bypassReverbZones on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.bypassReverbZones);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_dopplerLevel(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name dopplerLevel");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index dopplerLevel on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.dopplerLevel);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_spread(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name spread");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index spread on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.spread);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_priority(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name priority");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index priority on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.priority);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_mute(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mute");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mute on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.mute);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_minDistance(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name minDistance");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index minDistance on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.minDistance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_maxDistance(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxDistance");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxDistance on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.maxDistance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_rolloffMode(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rolloffMode");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rolloffMode on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.rolloffMode);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_volume(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name volume");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index volume on a nil value");
			}
		}

		obj.volume = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_pitch(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name pitch");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index pitch on a nil value");
			}
		}

		obj.pitch = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_time(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name time");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index time on a nil value");
			}
		}

		obj.time = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_timeSamples(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name timeSamples");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index timeSamples on a nil value");
			}
		}

		obj.timeSamples = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_clip(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

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

		obj.clip = (AudioClip)LuaScriptMgr.GetUnityObject(L, 3, typeof(AudioClip));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_outputAudioMixerGroup(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name outputAudioMixerGroup");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index outputAudioMixerGroup on a nil value");
			}
		}

		obj.outputAudioMixerGroup = (UnityEngine.Audio.AudioMixerGroup)LuaScriptMgr.GetUnityObject(L, 3, typeof(UnityEngine.Audio.AudioMixerGroup));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_loop(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name loop");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index loop on a nil value");
			}
		}

		obj.loop = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ignoreListenerVolume(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ignoreListenerVolume");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ignoreListenerVolume on a nil value");
			}
		}

		obj.ignoreListenerVolume = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_playOnAwake(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name playOnAwake");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index playOnAwake on a nil value");
			}
		}

		obj.playOnAwake = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ignoreListenerPause(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ignoreListenerPause");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ignoreListenerPause on a nil value");
			}
		}

		obj.ignoreListenerPause = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_velocityUpdateMode(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name velocityUpdateMode");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index velocityUpdateMode on a nil value");
			}
		}

		obj.velocityUpdateMode = (AudioVelocityUpdateMode)LuaScriptMgr.GetNetObject(L, 3, typeof(AudioVelocityUpdateMode));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_panStereo(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name panStereo");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index panStereo on a nil value");
			}
		}

		obj.panStereo = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_spatialBlend(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name spatialBlend");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index spatialBlend on a nil value");
			}
		}

		obj.spatialBlend = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_spatialize(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name spatialize");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index spatialize on a nil value");
			}
		}

		obj.spatialize = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_reverbZoneMix(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name reverbZoneMix");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index reverbZoneMix on a nil value");
			}
		}

		obj.reverbZoneMix = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bypassEffects(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bypassEffects");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bypassEffects on a nil value");
			}
		}

		obj.bypassEffects = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bypassListenerEffects(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bypassListenerEffects");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bypassListenerEffects on a nil value");
			}
		}

		obj.bypassListenerEffects = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bypassReverbZones(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bypassReverbZones");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bypassReverbZones on a nil value");
			}
		}

		obj.bypassReverbZones = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_dopplerLevel(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name dopplerLevel");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index dopplerLevel on a nil value");
			}
		}

		obj.dopplerLevel = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_spread(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name spread");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index spread on a nil value");
			}
		}

		obj.spread = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_priority(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name priority");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index priority on a nil value");
			}
		}

		obj.priority = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_mute(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mute");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mute on a nil value");
			}
		}

		obj.mute = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_minDistance(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name minDistance");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index minDistance on a nil value");
			}
		}

		obj.minDistance = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_maxDistance(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxDistance");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxDistance on a nil value");
			}
		}

		obj.maxDistance = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_rolloffMode(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioSource obj = (AudioSource)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rolloffMode");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rolloffMode on a nil value");
			}
		}

		obj.rolloffMode = (AudioRolloffMode)LuaScriptMgr.GetNetObject(L, 3, typeof(AudioRolloffMode));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Play(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
			obj.Play();
			return 0;
		}
		else if (count == 2)
		{
			AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
			ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
			obj.Play(arg0);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: AudioSource.Play");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayDelayed(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		obj.PlayDelayed(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayScheduled(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 2);
		obj.PlayScheduled(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetScheduledStartTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 2);
		obj.SetScheduledStartTime(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetScheduledEndTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 2);
		obj.SetScheduledEndTime(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Stop(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
		obj.Stop();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Pause(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
		obj.Pause();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnPause(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
		obj.UnPause();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayOneShot(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
			AudioClip arg0 = (AudioClip)LuaScriptMgr.GetUnityObject(L, 2, typeof(AudioClip));
			obj.PlayOneShot(arg0);
			return 0;
		}
		else if (count == 3)
		{
			AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
			AudioClip arg0 = (AudioClip)LuaScriptMgr.GetUnityObject(L, 2, typeof(AudioClip));
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
			obj.PlayOneShot(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: AudioSource.PlayOneShot");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayClipAtPoint(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			AudioClip arg0 = (AudioClip)LuaScriptMgr.GetUnityObject(L, 1, typeof(AudioClip));
			Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
			AudioSource.PlayClipAtPoint(arg0,arg1);
			return 0;
		}
		else if (count == 3)
		{
			AudioClip arg0 = (AudioClip)LuaScriptMgr.GetUnityObject(L, 1, typeof(AudioClip));
			Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
			float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
			AudioSource.PlayClipAtPoint(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: AudioSource.PlayClipAtPoint");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCustomCurve(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
		AudioSourceCurveType arg0 = (AudioSourceCurveType)LuaScriptMgr.GetNetObject(L, 2, typeof(AudioSourceCurveType));
		AnimationCurve arg1 = (AnimationCurve)LuaScriptMgr.GetNetObject(L, 3, typeof(AnimationCurve));
		obj.SetCustomCurve(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCustomCurve(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
		AudioSourceCurveType arg0 = (AudioSourceCurveType)LuaScriptMgr.GetNetObject(L, 2, typeof(AudioSourceCurveType));
		AnimationCurve o = obj.GetCustomCurve(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetOutputData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
		float[] objs0 = LuaScriptMgr.GetArrayNumber<float>(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		obj.GetOutputData(objs0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSpectrumData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
		float[] objs0 = LuaScriptMgr.GetArrayNumber<float>(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		FFTWindow arg2 = (FFTWindow)LuaScriptMgr.GetNetObject(L, 4, typeof(FFTWindow));
		obj.GetSpectrumData(objs0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetSpatializerFloat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		bool o = obj.SetSpatializerFloat(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSpatializerFloat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		AudioSource obj = (AudioSource)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioSource");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		float arg1;
		bool o = obj.GetSpatializerFloat(arg0,out arg1);
		LuaScriptMgr.Push(L, o);
		LuaScriptMgr.Push(L, arg1);
		return 2;
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

