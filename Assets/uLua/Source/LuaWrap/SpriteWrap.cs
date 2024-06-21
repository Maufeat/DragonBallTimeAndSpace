using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class SpriteWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Create", Create),
			new LuaMethod("OverrideGeometry", OverrideGeometry),
			new LuaMethod("New", _CreateSprite),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("bounds", get_bounds, null),
			new LuaField("rect", get_rect, null),
			new LuaField("pixelsPerUnit", get_pixelsPerUnit, null),
			new LuaField("texture", get_texture, null),
			new LuaField("associatedAlphaSplitTexture", get_associatedAlphaSplitTexture, null),
			new LuaField("textureRect", get_textureRect, null),
			new LuaField("textureRectOffset", get_textureRectOffset, null),
			new LuaField("packed", get_packed, null),
			new LuaField("packingMode", get_packingMode, null),
			new LuaField("packingRotation", get_packingRotation, null),
			new LuaField("pivot", get_pivot, null),
			new LuaField("border", get_border, null),
			new LuaField("vertices", get_vertices, null),
			new LuaField("triangles", get_triangles, null),
			new LuaField("uv", get_uv, null),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.Sprite", typeof(Sprite), regs, fields, typeof(Object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSprite(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Sprite obj = new Sprite();
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Sprite.New");
		}

		return 0;
	}

	static Type classType = typeof(Sprite);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bounds(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

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
	static int get_rect(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rect");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rect on a nil value");
			}
		}

		LuaScriptMgr.PushValue(L, obj.rect);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_pixelsPerUnit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name pixelsPerUnit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index pixelsPerUnit on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.pixelsPerUnit);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_texture(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name texture");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index texture on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.texture);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_associatedAlphaSplitTexture(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name associatedAlphaSplitTexture");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index associatedAlphaSplitTexture on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.associatedAlphaSplitTexture);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_textureRect(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name textureRect");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index textureRect on a nil value");
			}
		}

		LuaScriptMgr.PushValue(L, obj.textureRect);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_textureRectOffset(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name textureRectOffset");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index textureRectOffset on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.textureRectOffset);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_packed(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name packed");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index packed on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.packed);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_packingMode(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name packingMode");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index packingMode on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.packingMode);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_packingRotation(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name packingRotation");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index packingRotation on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.packingRotation);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_pivot(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name pivot");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index pivot on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.pivot);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_border(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name border");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index border on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.border);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_vertices(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name vertices");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index vertices on a nil value");
			}
		}

		LuaScriptMgr.PushArray(L, obj.vertices);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_triangles(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name triangles");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index triangles on a nil value");
			}
		}

		LuaScriptMgr.PushArray(L, obj.triangles);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_uv(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Sprite obj = (Sprite)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name uv");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index uv on a nil value");
			}
		}

		LuaScriptMgr.PushArray(L, obj.uv);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Create(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3)
		{
			Texture2D arg0 = (Texture2D)LuaScriptMgr.GetUnityObject(L, 1, typeof(Texture2D));
			Rect arg1 = (Rect)LuaScriptMgr.GetNetObject(L, 2, typeof(Rect));
			Vector2 arg2 = LuaScriptMgr.GetVector2(L, 3);
			Sprite o = Sprite.Create(arg0,arg1,arg2);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 4)
		{
			Texture2D arg0 = (Texture2D)LuaScriptMgr.GetUnityObject(L, 1, typeof(Texture2D));
			Rect arg1 = (Rect)LuaScriptMgr.GetNetObject(L, 2, typeof(Rect));
			Vector2 arg2 = LuaScriptMgr.GetVector2(L, 3);
			float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
			Sprite o = Sprite.Create(arg0,arg1,arg2,arg3);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 5)
		{
			Texture2D arg0 = (Texture2D)LuaScriptMgr.GetUnityObject(L, 1, typeof(Texture2D));
			Rect arg1 = (Rect)LuaScriptMgr.GetNetObject(L, 2, typeof(Rect));
			Vector2 arg2 = LuaScriptMgr.GetVector2(L, 3);
			float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
			uint arg4 = (uint)LuaScriptMgr.GetNumber(L, 5);
			Sprite o = Sprite.Create(arg0,arg1,arg2,arg3,arg4);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 6)
		{
			Texture2D arg0 = (Texture2D)LuaScriptMgr.GetUnityObject(L, 1, typeof(Texture2D));
			Rect arg1 = (Rect)LuaScriptMgr.GetNetObject(L, 2, typeof(Rect));
			Vector2 arg2 = LuaScriptMgr.GetVector2(L, 3);
			float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
			uint arg4 = (uint)LuaScriptMgr.GetNumber(L, 5);
			SpriteMeshType arg5 = (SpriteMeshType)LuaScriptMgr.GetNetObject(L, 6, typeof(SpriteMeshType));
			Sprite o = Sprite.Create(arg0,arg1,arg2,arg3,arg4,arg5);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 7)
		{
			Texture2D arg0 = (Texture2D)LuaScriptMgr.GetUnityObject(L, 1, typeof(Texture2D));
			Rect arg1 = (Rect)LuaScriptMgr.GetNetObject(L, 2, typeof(Rect));
			Vector2 arg2 = LuaScriptMgr.GetVector2(L, 3);
			float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
			uint arg4 = (uint)LuaScriptMgr.GetNumber(L, 5);
			SpriteMeshType arg5 = (SpriteMeshType)LuaScriptMgr.GetNetObject(L, 6, typeof(SpriteMeshType));
			Vector4 arg6 = LuaScriptMgr.GetVector4(L, 7);
			Sprite o = Sprite.Create(arg0,arg1,arg2,arg3,arg4,arg5,arg6);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Sprite.Create");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OverrideGeometry(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Sprite obj = (Sprite)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Sprite");
		Vector2[] objs0 = LuaScriptMgr.GetArrayObject<Vector2>(L, 2);
		ushort[] objs1 = LuaScriptMgr.GetArrayNumber<ushort>(L, 3);
		obj.OverrideGeometry(objs0,objs1);
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

