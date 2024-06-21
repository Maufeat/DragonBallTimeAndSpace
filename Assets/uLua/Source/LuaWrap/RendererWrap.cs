﻿using System;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using LuaInterface;
using Object = UnityEngine.Object;

public class RendererWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("SetPropertyBlock", SetPropertyBlock),
			new LuaMethod("GetPropertyBlock", GetPropertyBlock),
			new LuaMethod("GetClosestReflectionProbes", GetClosestReflectionProbes),
			new LuaMethod("New", _CreateRenderer),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("isPartOfStaticBatch", get_isPartOfStaticBatch, null),
			new LuaField("worldToLocalMatrix", get_worldToLocalMatrix, null),
			new LuaField("localToWorldMatrix", get_localToWorldMatrix, null),
			new LuaField("enabled", get_enabled, set_enabled),
			new LuaField("shadowCastingMode", get_shadowCastingMode, set_shadowCastingMode),
			new LuaField("receiveShadows", get_receiveShadows, set_receiveShadows),
			new LuaField("material", get_material, set_material),
			new LuaField("sharedMaterial", get_sharedMaterial, set_sharedMaterial),
			new LuaField("materials", get_materials, set_materials),
			new LuaField("sharedMaterials", get_sharedMaterials, set_sharedMaterials),
			new LuaField("bounds", get_bounds, null),
			new LuaField("lightmapIndex", get_lightmapIndex, set_lightmapIndex),
			new LuaField("realtimeLightmapIndex", get_realtimeLightmapIndex, set_realtimeLightmapIndex),
			new LuaField("lightmapScaleOffset", get_lightmapScaleOffset, set_lightmapScaleOffset),
			new LuaField("realtimeLightmapScaleOffset", get_realtimeLightmapScaleOffset, set_realtimeLightmapScaleOffset),
			new LuaField("isVisible", get_isVisible, null),
			new LuaField("useLightProbes", get_useLightProbes, set_useLightProbes),
			new LuaField("probeAnchor", get_probeAnchor, set_probeAnchor),
			new LuaField("reflectionProbeUsage", get_reflectionProbeUsage, set_reflectionProbeUsage),
			new LuaField("sortingLayerName", get_sortingLayerName, set_sortingLayerName),
			new LuaField("sortingLayerID", get_sortingLayerID, set_sortingLayerID),
			new LuaField("sortingOrder", get_sortingOrder, set_sortingOrder),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.Renderer", typeof(Renderer), regs, fields, typeof(Component));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateRenderer(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Renderer obj = new Renderer();
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Renderer.New");
		}

		return 0;
	}

	static Type classType = typeof(Renderer);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isPartOfStaticBatch(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isPartOfStaticBatch");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isPartOfStaticBatch on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.isPartOfStaticBatch);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_worldToLocalMatrix(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name worldToLocalMatrix");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index worldToLocalMatrix on a nil value");
			}
		}

		LuaScriptMgr.PushValue(L, obj.worldToLocalMatrix);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_localToWorldMatrix(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name localToWorldMatrix");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index localToWorldMatrix on a nil value");
			}
		}

		LuaScriptMgr.PushValue(L, obj.localToWorldMatrix);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_enabled(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

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
	static int get_shadowCastingMode(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name shadowCastingMode");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index shadowCastingMode on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.shadowCastingMode);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_receiveShadows(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name receiveShadows");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index receiveShadows on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.receiveShadows);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_material(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name material");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index material on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.material);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sharedMaterial(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sharedMaterial");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sharedMaterial on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.sharedMaterial);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_materials(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name materials");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index materials on a nil value");
			}
		}

		LuaScriptMgr.PushArray(L, obj.materials);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sharedMaterials(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sharedMaterials");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sharedMaterials on a nil value");
			}
		}

		LuaScriptMgr.PushArray(L, obj.sharedMaterials);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bounds(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bounds");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bounds on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.bounds);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lightmapIndex(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lightmapIndex");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lightmapIndex on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lightmapIndex);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_realtimeLightmapIndex(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name realtimeLightmapIndex");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index realtimeLightmapIndex on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.realtimeLightmapIndex);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lightmapScaleOffset(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lightmapScaleOffset");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lightmapScaleOffset on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lightmapScaleOffset);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_realtimeLightmapScaleOffset(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name realtimeLightmapScaleOffset");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index realtimeLightmapScaleOffset on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.realtimeLightmapScaleOffset);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isVisible(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isVisible");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isVisible on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.isVisible);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_useLightProbes(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name useLightProbes");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index useLightProbes on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.useLightProbes);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_probeAnchor(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name probeAnchor");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index probeAnchor on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.probeAnchor);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_reflectionProbeUsage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name reflectionProbeUsage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index reflectionProbeUsage on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.reflectionProbeUsage);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sortingLayerName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sortingLayerName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sortingLayerName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.sortingLayerName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sortingLayerID(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sortingLayerID");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sortingLayerID on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.sortingLayerID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sortingOrder(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sortingOrder");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sortingOrder on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.sortingOrder);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_enabled(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

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
	static int set_shadowCastingMode(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name shadowCastingMode");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index shadowCastingMode on a nil value");
			}
		}

		obj.shadowCastingMode = (UnityEngine.Rendering.ShadowCastingMode)LuaScriptMgr.GetNetObject(L, 3, typeof(UnityEngine.Rendering.ShadowCastingMode));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_receiveShadows(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name receiveShadows");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index receiveShadows on a nil value");
			}
		}

		obj.receiveShadows = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_material(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name material");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index material on a nil value");
			}
		}

		obj.material = (Material)LuaScriptMgr.GetUnityObject(L, 3, typeof(Material));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_sharedMaterial(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sharedMaterial");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sharedMaterial on a nil value");
			}
		}

		obj.sharedMaterial = (Material)LuaScriptMgr.GetUnityObject(L, 3, typeof(Material));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_materials(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name materials");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index materials on a nil value");
			}
		}

		obj.materials = LuaScriptMgr.GetArrayObject<Material>(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_sharedMaterials(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sharedMaterials");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sharedMaterials on a nil value");
			}
		}

		obj.sharedMaterials = LuaScriptMgr.GetArrayObject<Material>(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lightmapIndex(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lightmapIndex");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lightmapIndex on a nil value");
			}
		}

		obj.lightmapIndex = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_realtimeLightmapIndex(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name realtimeLightmapIndex");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index realtimeLightmapIndex on a nil value");
			}
		}

		obj.realtimeLightmapIndex = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lightmapScaleOffset(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lightmapScaleOffset");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lightmapScaleOffset on a nil value");
			}
		}

		obj.lightmapScaleOffset = LuaScriptMgr.GetVector4(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_realtimeLightmapScaleOffset(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name realtimeLightmapScaleOffset");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index realtimeLightmapScaleOffset on a nil value");
			}
		}

		obj.realtimeLightmapScaleOffset = LuaScriptMgr.GetVector4(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_useLightProbes(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name useLightProbes");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index useLightProbes on a nil value");
			}
		}

		obj.useLightProbes = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_probeAnchor(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name probeAnchor");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index probeAnchor on a nil value");
			}
		}

		obj.probeAnchor = (Transform)LuaScriptMgr.GetUnityObject(L, 3, typeof(Transform));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_reflectionProbeUsage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name reflectionProbeUsage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index reflectionProbeUsage on a nil value");
			}
		}

		obj.reflectionProbeUsage = (UnityEngine.Rendering.ReflectionProbeUsage)LuaScriptMgr.GetNetObject(L, 3, typeof(UnityEngine.Rendering.ReflectionProbeUsage));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_sortingLayerName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sortingLayerName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sortingLayerName on a nil value");
			}
		}

		obj.sortingLayerName = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_sortingLayerID(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sortingLayerID");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sortingLayerID on a nil value");
			}
		}

		obj.sortingLayerID = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_sortingOrder(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Renderer obj = (Renderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sortingOrder");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sortingOrder on a nil value");
			}
		}

		obj.sortingOrder = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPropertyBlock(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Renderer obj = (Renderer)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Renderer");
		MaterialPropertyBlock arg0 = (MaterialPropertyBlock)LuaScriptMgr.GetNetObject(L, 2, typeof(MaterialPropertyBlock));
		obj.SetPropertyBlock(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPropertyBlock(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Renderer obj = (Renderer)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Renderer");
		MaterialPropertyBlock arg0 = (MaterialPropertyBlock)LuaScriptMgr.GetNetObject(L, 2, typeof(MaterialPropertyBlock));
		obj.GetPropertyBlock(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClosestReflectionProbes(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Renderer obj = (Renderer)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Renderer");
		List<UnityEngine.Rendering.ReflectionProbeBlendInfo> arg0 = (List<UnityEngine.Rendering.ReflectionProbeBlendInfo>)LuaScriptMgr.GetNetObject(L, 2, typeof(List<UnityEngine.Rendering.ReflectionProbeBlendInfo>));
		obj.GetClosestReflectionProbes(arg0);
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

