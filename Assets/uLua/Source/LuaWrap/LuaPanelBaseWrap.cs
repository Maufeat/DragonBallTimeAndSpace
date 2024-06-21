using System;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using LuaInterface;

public class LuaPanelBaseWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("SetActive", SetActive),
			new LuaMethod("Dispose", Dispose),
			new LuaMethod("Awake", Awake),
			new LuaMethod("OnDispose", OnDispose),
			new LuaMethod("GetTextModel", GetTextModel),
			new LuaMethod("GetModelContent", GetModelContent),
			new LuaMethod("SetTextModel", SetTextModel),
			new LuaMethod("AddClick", AddClick),
			new LuaMethod("AddUIClickListener", AddUIClickListener),
			new LuaMethod("AddUIDragListener", AddUIDragListener),
			new LuaMethod("AddObjectClick", AddObjectClick),
			new LuaMethod("ClearClick", ClearClick),
			new LuaMethod("GetTexture", GetTexture),
			new LuaMethod("GetSprite", GetSprite),
			new LuaMethod("GetSpriteFromCommonAtlas", GetSpriteFromCommonAtlas),
			new LuaMethod("SetImageGrey", SetImageGrey),
			new LuaMethod("SetRawImageGrey", SetRawImageGrey),
			new LuaMethod("New", _CreateLuaPanelBase),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("uiName", get_uiName, set_uiName),
			new LuaField("uiRoot", get_uiRoot, set_uiRoot),
			new LuaField("usedTextureAssets", get_usedTextureAssets, set_usedTextureAssets),
			new LuaField("LuaScriptMgrInstance", get_LuaScriptMgrInstance, null),
			new LuaField("byNpcdlg", get_byNpcdlg, null),
		};

		LuaScriptMgr.RegisterLib(L, "LuaPanelBase", typeof(LuaPanelBase), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateLuaPanelBase(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
			LuaPanelBase obj = new LuaPanelBase(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: LuaPanelBase.New");
		}

		return 0;
	}

	static Type classType = typeof(LuaPanelBase);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_uiName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaPanelBase obj = (LuaPanelBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name uiName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index uiName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.uiName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_uiRoot(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaPanelBase obj = (LuaPanelBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name uiRoot");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index uiRoot on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.uiRoot);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_usedTextureAssets(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaPanelBase obj = (LuaPanelBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name usedTextureAssets");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index usedTextureAssets on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.usedTextureAssets);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_LuaScriptMgrInstance(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, LuaPanelBase.LuaScriptMgrInstance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_byNpcdlg(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaPanelBase obj = (LuaPanelBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name byNpcdlg");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index byNpcdlg on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.byNpcdlg);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_uiName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaPanelBase obj = (LuaPanelBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name uiName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index uiName on a nil value");
			}
		}

		obj.uiName = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_uiRoot(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaPanelBase obj = (LuaPanelBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name uiRoot");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index uiRoot on a nil value");
			}
		}

		obj.uiRoot = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_usedTextureAssets(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		LuaPanelBase obj = (LuaPanelBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name usedTextureAssets");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index usedTextureAssets on a nil value");
			}
		}

		obj.usedTextureAssets = (List<UITextureAsset>)LuaScriptMgr.GetNetObject(L, 3, typeof(List<UITextureAsset>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetActive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.SetActive(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Dispose(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		obj.Dispose();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Awake(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.Awake(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDispose(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		obj.OnDispose();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTextModel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string o = obj.GetTextModel(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetModelContent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		string o = obj.GetModelContent(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetTextModel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		Text arg0 = (Text)LuaScriptMgr.GetUnityObject(L, 2, typeof(Text));
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
		obj.SetTextModel(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddClick(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(LuaPanelBase), typeof(Button), typeof(LuaInterface.LuaFunction)))
		{
			LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
			Button arg0 = (Button)LuaScriptMgr.GetLuaObject(L, 2);
			LuaFunction arg1 = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.AddClick(arg0,arg1);
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(LuaPanelBase), typeof(string), typeof(LuaInterface.LuaFunction)))
		{
			LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			LuaFunction arg1 = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.AddClick(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: LuaPanelBase.AddClick");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddUIClickListener(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 3);
		obj.AddUIClickListener(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddUIDragListener(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 3);
		obj.AddUIDragListener(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddObjectClick(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 3);
		obj.AddObjectClick(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearClick(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		obj.ClearClick();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTexture(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 4);
		obj.GetTexture(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSprite(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 4);
		obj.GetSprite(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSpriteFromCommonAtlas(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 3);
		obj.GetSpriteFromCommonAtlas(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetImageGrey(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		Image arg0 = (Image)LuaScriptMgr.GetUnityObject(L, 2, typeof(Image));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.SetImageGrey(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetRawImageGrey(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		LuaPanelBase obj = (LuaPanelBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "LuaPanelBase");
		RawImage arg0 = (RawImage)LuaScriptMgr.GetUnityObject(L, 2, typeof(RawImage));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.SetRawImageGrey(arg0,arg1);
		return 0;
	}
}

