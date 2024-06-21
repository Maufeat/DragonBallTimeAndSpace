using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UnityEngine_UI_InputFieldWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("MoveTextEnd", MoveTextEnd),
			new LuaMethod("MoveTextStart", MoveTextStart),
			new LuaMethod("OnBeginDrag", OnBeginDrag),
			new LuaMethod("OnDrag", OnDrag),
			new LuaMethod("OnEndDrag", OnEndDrag),
			new LuaMethod("OnPointerDown", OnPointerDown),
			new LuaMethod("ProcessEvent", ProcessEvent),
			new LuaMethod("OnUpdateSelected", OnUpdateSelected),
			new LuaMethod("ForceLabelUpdate", ForceLabelUpdate),
			new LuaMethod("Rebuild", Rebuild),
			new LuaMethod("LayoutComplete", LayoutComplete),
			new LuaMethod("GraphicUpdateComplete", GraphicUpdateComplete),
			new LuaMethod("ActivateInputField", ActivateInputField),
			new LuaMethod("OnSelect", OnSelect),
			new LuaMethod("OnPointerClick", OnPointerClick),
			new LuaMethod("DeactivateInputField", DeactivateInputField),
			new LuaMethod("OnDeselect", OnDeselect),
			new LuaMethod("OnSubmit", OnSubmit),
			new LuaMethod("New", _CreateUnityEngine_UI_InputField),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("shouldHideMobileInput", get_shouldHideMobileInput, set_shouldHideMobileInput),
			new LuaField("text", get_text, set_text),
			new LuaField("isFocused", get_isFocused, null),
			new LuaField("caretBlinkRate", get_caretBlinkRate, set_caretBlinkRate),
			new LuaField("caretWidth", get_caretWidth, set_caretWidth),
			new LuaField("textComponent", get_textComponent, set_textComponent),
			new LuaField("placeholder", get_placeholder, set_placeholder),
			new LuaField("caretColor", get_caretColor, set_caretColor),
			new LuaField("customCaretColor", get_customCaretColor, set_customCaretColor),
			new LuaField("selectionColor", get_selectionColor, set_selectionColor),
			new LuaField("onEndEdit", get_onEndEdit, set_onEndEdit),
			new LuaField("onValueChanged", get_onValueChanged, set_onValueChanged),
			new LuaField("onValidateInput", get_onValidateInput, set_onValidateInput),
			new LuaField("characterLimit", get_characterLimit, set_characterLimit),
			new LuaField("contentType", get_contentType, set_contentType),
			new LuaField("lineType", get_lineType, set_lineType),
			new LuaField("inputType", get_inputType, set_inputType),
			new LuaField("keyboardType", get_keyboardType, set_keyboardType),
			new LuaField("characterValidation", get_characterValidation, set_characterValidation),
			new LuaField("readOnly", get_readOnly, set_readOnly),
			new LuaField("multiLine", get_multiLine, null),
			new LuaField("asteriskChar", get_asteriskChar, set_asteriskChar),
			new LuaField("wasCanceled", get_wasCanceled, null),
			new LuaField("caretPosition", get_caretPosition, set_caretPosition),
			new LuaField("selectionAnchorPosition", get_selectionAnchorPosition, set_selectionAnchorPosition),
			new LuaField("selectionFocusPosition", get_selectionFocusPosition, set_selectionFocusPosition),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.UI.InputField", typeof(InputField), regs, fields, typeof(Selectable));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_InputField(IntPtr L)
	{
		LuaDLL.luaL_error(L, "InputField class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(InputField);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_shouldHideMobileInput(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name shouldHideMobileInput");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index shouldHideMobileInput on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.shouldHideMobileInput);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_text(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name text");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index text on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.text);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isFocused(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isFocused");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isFocused on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.isFocused);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_caretBlinkRate(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name caretBlinkRate");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index caretBlinkRate on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.caretBlinkRate);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_caretWidth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name caretWidth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index caretWidth on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.caretWidth);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_textComponent(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name textComponent");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index textComponent on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.textComponent);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_placeholder(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name placeholder");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index placeholder on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.placeholder);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_caretColor(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name caretColor");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index caretColor on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.caretColor);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_customCaretColor(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name customCaretColor");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index customCaretColor on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.customCaretColor);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_selectionColor(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name selectionColor");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index selectionColor on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.selectionColor);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onEndEdit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onEndEdit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onEndEdit on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.onEndEdit);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onValueChanged(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

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
	static int get_onValidateInput(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onValidateInput");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onValidateInput on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.onValidateInput);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_characterLimit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name characterLimit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index characterLimit on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.characterLimit);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_contentType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name contentType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index contentType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.contentType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lineType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lineType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lineType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lineType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_inputType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name inputType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index inputType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.inputType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_keyboardType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name keyboardType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index keyboardType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.keyboardType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_characterValidation(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name characterValidation");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index characterValidation on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.characterValidation);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_readOnly(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name readOnly");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index readOnly on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.readOnly);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_multiLine(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name multiLine");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index multiLine on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.multiLine);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_asteriskChar(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name asteriskChar");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index asteriskChar on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.asteriskChar);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_wasCanceled(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name wasCanceled");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index wasCanceled on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.wasCanceled);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_caretPosition(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name caretPosition");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index caretPosition on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.caretPosition);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_selectionAnchorPosition(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name selectionAnchorPosition");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index selectionAnchorPosition on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.selectionAnchorPosition);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_selectionFocusPosition(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name selectionFocusPosition");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index selectionFocusPosition on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.selectionFocusPosition);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_shouldHideMobileInput(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name shouldHideMobileInput");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index shouldHideMobileInput on a nil value");
			}
		}

		obj.shouldHideMobileInput = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_text(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name text");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index text on a nil value");
			}
		}

		obj.text = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_caretBlinkRate(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name caretBlinkRate");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index caretBlinkRate on a nil value");
			}
		}

		obj.caretBlinkRate = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_caretWidth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name caretWidth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index caretWidth on a nil value");
			}
		}

		obj.caretWidth = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_textComponent(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name textComponent");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index textComponent on a nil value");
			}
		}

		obj.textComponent = (Text)LuaScriptMgr.GetUnityObject(L, 3, typeof(Text));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_placeholder(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name placeholder");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index placeholder on a nil value");
			}
		}

		obj.placeholder = (Graphic)LuaScriptMgr.GetUnityObject(L, 3, typeof(Graphic));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_caretColor(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name caretColor");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index caretColor on a nil value");
			}
		}

		obj.caretColor = LuaScriptMgr.GetColor(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_customCaretColor(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name customCaretColor");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index customCaretColor on a nil value");
			}
		}

		obj.customCaretColor = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_selectionColor(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name selectionColor");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index selectionColor on a nil value");
			}
		}

		obj.selectionColor = LuaScriptMgr.GetColor(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onEndEdit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onEndEdit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onEndEdit on a nil value");
			}
		}

		obj.onEndEdit = (InputField.SubmitEvent)LuaScriptMgr.GetNetObject(L, 3, typeof(InputField.SubmitEvent));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onValueChanged(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

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

		obj.onValueChanged = (InputField.OnChangeEvent)LuaScriptMgr.GetNetObject(L, 3, typeof(InputField.OnChangeEvent));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onValidateInput(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onValidateInput");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onValidateInput on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.onValidateInput = (InputField.OnValidateInput)LuaScriptMgr.GetNetObject(L, 3, typeof(InputField.OnValidateInput));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.onValidateInput = (param0, param1, param2) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				LuaScriptMgr.Push(L, param1);
				LuaScriptMgr.Push(L, param2);
				func.PCall(top, 3);
				object[] objs = func.PopValues(top);
				func.EndPCall(top);
				return (char)objs[0];
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_characterLimit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name characterLimit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index characterLimit on a nil value");
			}
		}

		obj.characterLimit = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_contentType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name contentType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index contentType on a nil value");
			}
		}

		obj.contentType = (InputField.ContentType)LuaScriptMgr.GetNetObject(L, 3, typeof(InputField.ContentType));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lineType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lineType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lineType on a nil value");
			}
		}

		obj.lineType = (InputField.LineType)LuaScriptMgr.GetNetObject(L, 3, typeof(InputField.LineType));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_inputType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name inputType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index inputType on a nil value");
			}
		}

		obj.inputType = (InputField.InputType)LuaScriptMgr.GetNetObject(L, 3, typeof(InputField.InputType));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_keyboardType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name keyboardType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index keyboardType on a nil value");
			}
		}

		obj.keyboardType = (TouchScreenKeyboardType)LuaScriptMgr.GetNetObject(L, 3, typeof(TouchScreenKeyboardType));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_characterValidation(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name characterValidation");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index characterValidation on a nil value");
			}
		}

		obj.characterValidation = (InputField.CharacterValidation)LuaScriptMgr.GetNetObject(L, 3, typeof(InputField.CharacterValidation));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_readOnly(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name readOnly");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index readOnly on a nil value");
			}
		}

		obj.readOnly = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_asteriskChar(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name asteriskChar");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index asteriskChar on a nil value");
			}
		}

		obj.asteriskChar = (char)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_caretPosition(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name caretPosition");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index caretPosition on a nil value");
			}
		}

		obj.caretPosition = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_selectionAnchorPosition(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name selectionAnchorPosition");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index selectionAnchorPosition on a nil value");
			}
		}

		obj.selectionAnchorPosition = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_selectionFocusPosition(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		InputField obj = (InputField)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name selectionFocusPosition");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index selectionFocusPosition on a nil value");
			}
		}

		obj.selectionFocusPosition = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MoveTextEnd(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.MoveTextEnd(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MoveTextStart(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.MoveTextStart(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnBeginDrag(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		PointerEventData arg0 = (PointerEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(PointerEventData));
		obj.OnBeginDrag(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDrag(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		PointerEventData arg0 = (PointerEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(PointerEventData));
		obj.OnDrag(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnEndDrag(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		PointerEventData arg0 = (PointerEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(PointerEventData));
		obj.OnEndDrag(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnPointerDown(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		PointerEventData arg0 = (PointerEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(PointerEventData));
		obj.OnPointerDown(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ProcessEvent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		Event arg0 = (Event)LuaScriptMgr.GetNetObject(L, 2, typeof(Event));
		obj.ProcessEvent(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnUpdateSelected(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		BaseEventData arg0 = (BaseEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(BaseEventData));
		obj.OnUpdateSelected(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ForceLabelUpdate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		obj.ForceLabelUpdate();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Rebuild(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		CanvasUpdate arg0 = (CanvasUpdate)LuaScriptMgr.GetNetObject(L, 2, typeof(CanvasUpdate));
		obj.Rebuild(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LayoutComplete(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		obj.LayoutComplete();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GraphicUpdateComplete(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		obj.GraphicUpdateComplete();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ActivateInputField(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		obj.ActivateInputField();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnSelect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		BaseEventData arg0 = (BaseEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(BaseEventData));
		obj.OnSelect(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnPointerClick(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		PointerEventData arg0 = (PointerEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(PointerEventData));
		obj.OnPointerClick(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DeactivateInputField(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		obj.DeactivateInputField();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDeselect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		BaseEventData arg0 = (BaseEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(BaseEventData));
		obj.OnDeselect(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnSubmit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		InputField obj = (InputField)LuaScriptMgr.GetUnityObjectSelf(L, 1, "InputField");
		BaseEventData arg0 = (BaseEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(BaseEventData));
		obj.OnSubmit(arg0);
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

