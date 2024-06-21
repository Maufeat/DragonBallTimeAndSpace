using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using LuaInterface;
using Object = UnityEngine.Object;

public class UnityEngine_UI_DropdownWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("RefreshShownValue", RefreshShownValue),
			new LuaMethod("AddOptions", AddOptions),
			new LuaMethod("ClearOptions", ClearOptions),
			new LuaMethod("OnPointerClick", OnPointerClick),
			new LuaMethod("OnSubmit", OnSubmit),
			new LuaMethod("OnCancel", OnCancel),
			new LuaMethod("Show", Show),
			new LuaMethod("Hide", Hide),
			new LuaMethod("New", _CreateUnityEngine_UI_Dropdown),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("template", get_template, set_template),
			new LuaField("captionText", get_captionText, set_captionText),
			new LuaField("captionImage", get_captionImage, set_captionImage),
			new LuaField("itemText", get_itemText, set_itemText),
			new LuaField("itemImage", get_itemImage, set_itemImage),
			new LuaField("options", get_options, set_options),
			new LuaField("onValueChanged", get_onValueChanged, set_onValueChanged),
			new LuaField("value", get_value, set_value),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.UI.Dropdown", typeof(Dropdown), regs, fields, typeof(Selectable));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_Dropdown(IntPtr L)
	{
		LuaDLL.luaL_error(L, "Dropdown class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(Dropdown);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_template(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name template");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index template on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.template);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_captionText(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name captionText");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index captionText on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.captionText);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_captionImage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name captionImage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index captionImage on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.captionImage);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_itemText(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name itemText");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index itemText on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.itemText);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_itemImage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name itemImage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index itemImage on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.itemImage);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_options(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name options");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index options on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.options);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onValueChanged(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onValueChanged");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onValueChanged on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.onValueChanged);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_value(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name value");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index value on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.value);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_template(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name template");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index template on a nil value");
			}
		}

		obj.template = (RectTransform)LuaScriptMgr.GetUnityObject(L, 3, typeof(RectTransform));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_captionText(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name captionText");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index captionText on a nil value");
			}
		}

		obj.captionText = (Text)LuaScriptMgr.GetUnityObject(L, 3, typeof(Text));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_captionImage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name captionImage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index captionImage on a nil value");
			}
		}

		obj.captionImage = (Image)LuaScriptMgr.GetUnityObject(L, 3, typeof(Image));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_itemText(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name itemText");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index itemText on a nil value");
			}
		}

		obj.itemText = (Text)LuaScriptMgr.GetUnityObject(L, 3, typeof(Text));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_itemImage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name itemImage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index itemImage on a nil value");
			}
		}

		obj.itemImage = (Image)LuaScriptMgr.GetUnityObject(L, 3, typeof(Image));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_options(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name options");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index options on a nil value");
			}
		}

		obj.options = (List<Dropdown.OptionData>)LuaScriptMgr.GetNetObject(L, 3, typeof(List<Dropdown.OptionData>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onValueChanged(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onValueChanged");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onValueChanged on a nil value");
			}
		}

		obj.onValueChanged = (Dropdown.DropdownEvent)LuaScriptMgr.GetNetObject(L, 3, typeof(Dropdown.DropdownEvent));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_value(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Dropdown obj = (Dropdown)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name value");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index value on a nil value");
			}
		}

		obj.value = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshShownValue(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Dropdown obj = (Dropdown)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Dropdown");
		obj.RefreshShownValue();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddOptions(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Dropdown), typeof(List<Sprite>)))
		{
			Dropdown obj = (Dropdown)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Dropdown");
			List<Sprite> arg0 = (List<Sprite>)LuaScriptMgr.GetLuaObject(L, 2);
			obj.AddOptions(arg0);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Dropdown), typeof(List<string>)))
		{
			Dropdown obj = (Dropdown)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Dropdown");
			List<string> arg0 = (List<string>)LuaScriptMgr.GetLuaObject(L, 2);
			obj.AddOptions(arg0);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Dropdown), typeof(List<Dropdown.OptionData>)))
		{
			Dropdown obj = (Dropdown)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Dropdown");
			List<Dropdown.OptionData> arg0 = (List<Dropdown.OptionData>)LuaScriptMgr.GetLuaObject(L, 2);
			obj.AddOptions(arg0);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Dropdown.AddOptions");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearOptions(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Dropdown obj = (Dropdown)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Dropdown");
		obj.ClearOptions();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnPointerClick(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Dropdown obj = (Dropdown)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Dropdown");
		PointerEventData arg0 = (PointerEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(PointerEventData));
		obj.OnPointerClick(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnSubmit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Dropdown obj = (Dropdown)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Dropdown");
		BaseEventData arg0 = (BaseEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(BaseEventData));
		obj.OnSubmit(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnCancel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Dropdown obj = (Dropdown)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Dropdown");
		BaseEventData arg0 = (BaseEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(BaseEventData));
		obj.OnCancel(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Show(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Dropdown obj = (Dropdown)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Dropdown");
		obj.Show();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Hide(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Dropdown obj = (Dropdown)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Dropdown");
		obj.Hide();
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

