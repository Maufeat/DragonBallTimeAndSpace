using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UnityEngine_UI_GridLayoutGroupWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("CalculateLayoutInputHorizontal", CalculateLayoutInputHorizontal),
			new LuaMethod("CalculateLayoutInputVertical", CalculateLayoutInputVertical),
			new LuaMethod("SetLayoutHorizontal", SetLayoutHorizontal),
			new LuaMethod("SetLayoutVertical", SetLayoutVertical),
			new LuaMethod("New", _CreateUnityEngine_UI_GridLayoutGroup),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("startCorner", get_startCorner, set_startCorner),
			new LuaField("startAxis", get_startAxis, set_startAxis),
			new LuaField("cellSize", get_cellSize, set_cellSize),
			new LuaField("spacing", get_spacing, set_spacing),
			new LuaField("constraint", get_constraint, set_constraint),
			new LuaField("constraintCount", get_constraintCount, set_constraintCount),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.UI.GridLayoutGroup", typeof(GridLayoutGroup), regs, fields, typeof(LayoutGroup));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_GridLayoutGroup(IntPtr L)
	{
		LuaDLL.luaL_error(L, "GridLayoutGroup class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(GridLayoutGroup);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_startCorner(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name startCorner");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index startCorner on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.startCorner);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_startAxis(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name startAxis");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index startAxis on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.startAxis);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_cellSize(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name cellSize");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index cellSize on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.cellSize);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_spacing(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name spacing");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index spacing on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.spacing);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_constraint(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name constraint");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index constraint on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.constraint);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_constraintCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name constraintCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index constraintCount on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.constraintCount);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_startCorner(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name startCorner");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index startCorner on a nil value");
			}
		}

		obj.startCorner = (GridLayoutGroup.Corner)LuaScriptMgr.GetNetObject(L, 3, typeof(GridLayoutGroup.Corner));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_startAxis(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name startAxis");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index startAxis on a nil value");
			}
		}

		obj.startAxis = (GridLayoutGroup.Axis)LuaScriptMgr.GetNetObject(L, 3, typeof(GridLayoutGroup.Axis));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_cellSize(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name cellSize");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index cellSize on a nil value");
			}
		}

		obj.cellSize = LuaScriptMgr.GetVector2(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_spacing(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name spacing");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index spacing on a nil value");
			}
		}

		obj.spacing = LuaScriptMgr.GetVector2(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_constraint(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name constraint");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index constraint on a nil value");
			}
		}

		obj.constraint = (GridLayoutGroup.Constraint)LuaScriptMgr.GetNetObject(L, 3, typeof(GridLayoutGroup.Constraint));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_constraintCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name constraintCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index constraintCount on a nil value");
			}
		}

		obj.constraintCount = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CalculateLayoutInputHorizontal(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GridLayoutGroup");
		obj.CalculateLayoutInputHorizontal();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CalculateLayoutInputVertical(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GridLayoutGroup");
		obj.CalculateLayoutInputVertical();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLayoutHorizontal(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GridLayoutGroup");
		obj.SetLayoutHorizontal();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLayoutVertical(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GridLayoutGroup obj = (GridLayoutGroup)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GridLayoutGroup");
		obj.SetLayoutVertical();
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

