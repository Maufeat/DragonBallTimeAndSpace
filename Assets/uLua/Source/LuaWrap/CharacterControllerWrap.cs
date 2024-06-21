﻿using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class CharacterControllerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("SimpleMove", SimpleMove),
			new LuaMethod("Move", Move),
			new LuaMethod("New", _CreateCharacterController),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("isGrounded", get_isGrounded, null),
			new LuaField("velocity", get_velocity, null),
			new LuaField("collisionFlags", get_collisionFlags, null),
			new LuaField("radius", get_radius, set_radius),
			new LuaField("height", get_height, set_height),
			new LuaField("center", get_center, set_center),
			new LuaField("slopeLimit", get_slopeLimit, set_slopeLimit),
			new LuaField("stepOffset", get_stepOffset, set_stepOffset),
			new LuaField("skinWidth", get_skinWidth, set_skinWidth),
			new LuaField("detectCollisions", get_detectCollisions, set_detectCollisions),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.CharacterController", typeof(CharacterController), regs, fields, typeof(Collider));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateCharacterController(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			CharacterController obj = new CharacterController();
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: CharacterController.New");
		}

		return 0;
	}

	static Type classType = typeof(CharacterController);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isGrounded(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isGrounded");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isGrounded on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.isGrounded);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_velocity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name velocity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index velocity on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.velocity);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_collisionFlags(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name collisionFlags");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index collisionFlags on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.collisionFlags);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_radius(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name radius");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index radius on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.radius);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_height(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name height");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index height on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.height);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_center(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name center");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index center on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.center);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_slopeLimit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name slopeLimit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index slopeLimit on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.slopeLimit);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_stepOffset(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name stepOffset");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index stepOffset on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.stepOffset);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_skinWidth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name skinWidth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index skinWidth on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.skinWidth);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_detectCollisions(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name detectCollisions");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index detectCollisions on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.detectCollisions);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_radius(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name radius");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index radius on a nil value");
			}
		}

		obj.radius = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_height(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name height");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index height on a nil value");
			}
		}

		obj.height = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_center(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name center");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index center on a nil value");
			}
		}

		obj.center = LuaScriptMgr.GetVector3(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_slopeLimit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name slopeLimit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index slopeLimit on a nil value");
			}
		}

		obj.slopeLimit = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_stepOffset(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name stepOffset");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index stepOffset on a nil value");
			}
		}

		obj.stepOffset = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_skinWidth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name skinWidth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index skinWidth on a nil value");
			}
		}

		obj.skinWidth = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_detectCollisions(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharacterController obj = (CharacterController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name detectCollisions");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index detectCollisions on a nil value");
			}
		}

		obj.detectCollisions = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SimpleMove(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharacterController obj = (CharacterController)LuaScriptMgr.GetUnityObjectSelf(L, 1, "CharacterController");
		Vector3 arg0 = LuaScriptMgr.GetVector3(L, 2);
		bool o = obj.SimpleMove(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Move(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharacterController obj = (CharacterController)LuaScriptMgr.GetUnityObjectSelf(L, 1, "CharacterController");
		Vector3 arg0 = LuaScriptMgr.GetVector3(L, 2);
		CollisionFlags o = obj.Move(arg0);
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

