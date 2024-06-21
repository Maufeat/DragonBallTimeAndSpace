﻿using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class ParticleEmitterWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("ClearParticles", ClearParticles),
			new LuaMethod("Emit", Emit),
			new LuaMethod("Simulate", Simulate),
			new LuaMethod("New", _CreateParticleEmitter),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("emit", get_emit, set_emit),
			new LuaField("minSize", get_minSize, set_minSize),
			new LuaField("maxSize", get_maxSize, set_maxSize),
			new LuaField("minEnergy", get_minEnergy, set_minEnergy),
			new LuaField("maxEnergy", get_maxEnergy, set_maxEnergy),
			new LuaField("minEmission", get_minEmission, set_minEmission),
			new LuaField("maxEmission", get_maxEmission, set_maxEmission),
			new LuaField("emitterVelocityScale", get_emitterVelocityScale, set_emitterVelocityScale),
			new LuaField("worldVelocity", get_worldVelocity, set_worldVelocity),
			new LuaField("localVelocity", get_localVelocity, set_localVelocity),
			new LuaField("rndVelocity", get_rndVelocity, set_rndVelocity),
			new LuaField("useWorldSpace", get_useWorldSpace, set_useWorldSpace),
			new LuaField("rndRotation", get_rndRotation, set_rndRotation),
			new LuaField("angularVelocity", get_angularVelocity, set_angularVelocity),
			new LuaField("rndAngularVelocity", get_rndAngularVelocity, set_rndAngularVelocity),
			new LuaField("particles", get_particles, set_particles),
			new LuaField("particleCount", get_particleCount, null),
			new LuaField("enabled", get_enabled, set_enabled),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.ParticleEmitter", typeof(ParticleEmitter), regs, fields, typeof(Component));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateParticleEmitter(IntPtr L)
	{
		LuaDLL.luaL_error(L, "ParticleEmitter class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(ParticleEmitter);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_emit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name emit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index emit on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.emit);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_minSize(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name minSize");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index minSize on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.minSize);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_maxSize(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxSize");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxSize on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.maxSize);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_minEnergy(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name minEnergy");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index minEnergy on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.minEnergy);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_maxEnergy(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxEnergy");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxEnergy on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.maxEnergy);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_minEmission(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name minEmission");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index minEmission on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.minEmission);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_maxEmission(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxEmission");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxEmission on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.maxEmission);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_emitterVelocityScale(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name emitterVelocityScale");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index emitterVelocityScale on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.emitterVelocityScale);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_worldVelocity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name worldVelocity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index worldVelocity on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.worldVelocity);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_localVelocity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name localVelocity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index localVelocity on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.localVelocity);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_rndVelocity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rndVelocity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rndVelocity on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.rndVelocity);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_useWorldSpace(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name useWorldSpace");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index useWorldSpace on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.useWorldSpace);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_rndRotation(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rndRotation");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rndRotation on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.rndRotation);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_angularVelocity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name angularVelocity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index angularVelocity on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.angularVelocity);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_rndAngularVelocity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rndAngularVelocity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rndAngularVelocity on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.rndAngularVelocity);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_particles(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name particles");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index particles on a nil value");
			}
		}

		LuaScriptMgr.PushArray(L, obj.particles);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_particleCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name particleCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index particleCount on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.particleCount);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_enabled(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name enabled");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index enabled on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.enabled);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_emit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name emit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index emit on a nil value");
			}
		}

		obj.emit = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_minSize(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name minSize");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index minSize on a nil value");
			}
		}

		obj.minSize = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_maxSize(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxSize");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxSize on a nil value");
			}
		}

		obj.maxSize = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_minEnergy(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name minEnergy");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index minEnergy on a nil value");
			}
		}

		obj.minEnergy = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_maxEnergy(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxEnergy");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxEnergy on a nil value");
			}
		}

		obj.maxEnergy = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_minEmission(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name minEmission");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index minEmission on a nil value");
			}
		}

		obj.minEmission = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_maxEmission(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxEmission");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxEmission on a nil value");
			}
		}

		obj.maxEmission = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_emitterVelocityScale(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name emitterVelocityScale");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index emitterVelocityScale on a nil value");
			}
		}

		obj.emitterVelocityScale = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_worldVelocity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name worldVelocity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index worldVelocity on a nil value");
			}
		}

		obj.worldVelocity = LuaScriptMgr.GetVector3(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_localVelocity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name localVelocity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index localVelocity on a nil value");
			}
		}

		obj.localVelocity = LuaScriptMgr.GetVector3(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_rndVelocity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rndVelocity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rndVelocity on a nil value");
			}
		}

		obj.rndVelocity = LuaScriptMgr.GetVector3(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_useWorldSpace(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name useWorldSpace");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index useWorldSpace on a nil value");
			}
		}

		obj.useWorldSpace = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_rndRotation(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rndRotation");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rndRotation on a nil value");
			}
		}

		obj.rndRotation = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_angularVelocity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name angularVelocity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index angularVelocity on a nil value");
			}
		}

		obj.angularVelocity = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_rndAngularVelocity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rndAngularVelocity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rndAngularVelocity on a nil value");
			}
		}

		obj.rndAngularVelocity = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_particles(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name particles");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index particles on a nil value");
			}
		}

		obj.particles = LuaScriptMgr.GetArrayObject<Particle>(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_enabled(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		ParticleEmitter obj = (ParticleEmitter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name enabled");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index enabled on a nil value");
			}
		}

		obj.enabled = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearParticles(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ParticleEmitter obj = (ParticleEmitter)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ParticleEmitter");
		obj.ClearParticles();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Emit(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			ParticleEmitter obj = (ParticleEmitter)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ParticleEmitter");
			obj.Emit();
			return 0;
		}
		else if (count == 2)
		{
			ParticleEmitter obj = (ParticleEmitter)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ParticleEmitter");
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			obj.Emit(arg0);
			return 0;
		}
		else if (count == 6)
		{
			ParticleEmitter obj = (ParticleEmitter)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ParticleEmitter");
			Vector3 arg0 = LuaScriptMgr.GetVector3(L, 2);
			Vector3 arg1 = LuaScriptMgr.GetVector3(L, 3);
			float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
			float arg3 = (float)LuaScriptMgr.GetNumber(L, 5);
			Color arg4 = LuaScriptMgr.GetColor(L, 6);
			obj.Emit(arg0,arg1,arg2,arg3,arg4);
			return 0;
		}
		else if (count == 8)
		{
			ParticleEmitter obj = (ParticleEmitter)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ParticleEmitter");
			Vector3 arg0 = LuaScriptMgr.GetVector3(L, 2);
			Vector3 arg1 = LuaScriptMgr.GetVector3(L, 3);
			float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
			float arg3 = (float)LuaScriptMgr.GetNumber(L, 5);
			Color arg4 = LuaScriptMgr.GetColor(L, 6);
			float arg5 = (float)LuaScriptMgr.GetNumber(L, 7);
			float arg6 = (float)LuaScriptMgr.GetNumber(L, 8);
			obj.Emit(arg0,arg1,arg2,arg3,arg4,arg5,arg6);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ParticleEmitter.Emit");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Simulate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ParticleEmitter obj = (ParticleEmitter)LuaScriptMgr.GetUnityObjectSelf(L, 1, "ParticleEmitter");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		obj.Simulate(arg0);
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

