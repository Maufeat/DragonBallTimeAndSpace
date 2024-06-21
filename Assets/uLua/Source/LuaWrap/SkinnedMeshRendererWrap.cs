﻿using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class SkinnedMeshRendererWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("BakeMesh", BakeMesh),
			new LuaMethod("GetBlendShapeWeight", GetBlendShapeWeight),
			new LuaMethod("SetBlendShapeWeight", SetBlendShapeWeight),
			new LuaMethod("New", _CreateSkinnedMeshRenderer),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("bones", get_bones, set_bones),
			new LuaField("rootBone", get_rootBone, set_rootBone),
			new LuaField("quality", get_quality, set_quality),
			new LuaField("sharedMesh", get_sharedMesh, set_sharedMesh),
			new LuaField("updateWhenOffscreen", get_updateWhenOffscreen, set_updateWhenOffscreen),
			new LuaField("localBounds", get_localBounds, set_localBounds),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.SkinnedMeshRenderer", typeof(SkinnedMeshRenderer), regs, fields, typeof(Renderer));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSkinnedMeshRenderer(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SkinnedMeshRenderer obj = new SkinnedMeshRenderer();
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SkinnedMeshRenderer.New");
		}

		return 0;
	}

	static Type classType = typeof(SkinnedMeshRenderer);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bones(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bones");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bones on a nil value");
			}
		}

		LuaScriptMgr.PushArray(L, obj.bones);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_rootBone(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rootBone");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rootBone on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.rootBone);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_quality(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name quality");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index quality on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.quality);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sharedMesh(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sharedMesh");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sharedMesh on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.sharedMesh);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_updateWhenOffscreen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name updateWhenOffscreen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index updateWhenOffscreen on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.updateWhenOffscreen);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_localBounds(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)o;

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
	static int set_bones(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bones");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bones on a nil value");
			}
		}

		obj.bones = LuaScriptMgr.GetArrayObject<Transform>(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_rootBone(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rootBone");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rootBone on a nil value");
			}
		}

		obj.rootBone = (Transform)LuaScriptMgr.GetUnityObject(L, 3, typeof(Transform));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_quality(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name quality");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index quality on a nil value");
			}
		}

		obj.quality = (SkinQuality)LuaScriptMgr.GetNetObject(L, 3, typeof(SkinQuality));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_sharedMesh(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sharedMesh");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sharedMesh on a nil value");
			}
		}

		obj.sharedMesh = (Mesh)LuaScriptMgr.GetUnityObject(L, 3, typeof(Mesh));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_updateWhenOffscreen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name updateWhenOffscreen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index updateWhenOffscreen on a nil value");
			}
		}

		obj.updateWhenOffscreen = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_localBounds(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)o;

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
	static int BakeMesh(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SkinnedMeshRenderer");
		Mesh arg0 = (Mesh)LuaScriptMgr.GetUnityObject(L, 2, typeof(Mesh));
		obj.BakeMesh(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetBlendShapeWeight(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SkinnedMeshRenderer");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		float o = obj.GetBlendShapeWeight(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetBlendShapeWeight(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SkinnedMeshRenderer obj = (SkinnedMeshRenderer)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SkinnedMeshRenderer");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		obj.SetBlendShapeWeight(arg0,arg1);
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

