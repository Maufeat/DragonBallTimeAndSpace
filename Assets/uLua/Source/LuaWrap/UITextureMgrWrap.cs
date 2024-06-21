using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;

public class UITextureMgrWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("GetTexture", GetTexture),
			new LuaMethod("GetModelSkinnedAssets", GetModelSkinnedAssets),
			new LuaMethod("GetSpriteFromAtlas", GetSpriteFromAtlas),
			new LuaMethod("LoadAtlas", LoadAtlas),
			new LuaMethod("SetImageGrey", SetImageGrey),
			new LuaMethod("SetImageGrey4Head", SetImageGrey4Head),
			new LuaMethod("OnUpdate", OnUpdate),
			new LuaMethod("OnReSet", OnReSet),
			new LuaMethod("New", _CreateUITextureMgr),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("MapTextureAssets", get_MapTextureAssets, set_MapTextureAssets),
			new LuaField("Instance", get_Instance, null),
			new LuaField("ManagerName", get_ManagerName, null),
		};

		LuaScriptMgr.RegisterLib(L, "UITextureMgr", typeof(UITextureMgr), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUITextureMgr(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			UITextureMgr obj = new UITextureMgr();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UITextureMgr.New");
		}

		return 0;
	}

	static Type classType = typeof(UITextureMgr);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MapTextureAssets(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, UITextureMgr.MapTextureAssets);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, UITextureMgr.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ManagerName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITextureMgr obj = (UITextureMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ManagerName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ManagerName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ManagerName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_MapTextureAssets(IntPtr L)
	{
		UITextureMgr.MapTextureAssets = (BetterDictionary<string,UITextureAsset>)LuaScriptMgr.GetNetObject(L, 3, typeof(BetterDictionary<string,UITextureAsset>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTexture(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 4 && LuaScriptMgr.CheckTypes(L, 1, typeof(UITextureMgr), typeof(int), typeof(string), typeof(LuaInterface.LuaFunction)))
		{
			UITextureMgr obj = (UITextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "UITextureMgr");
			int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
			string arg1 = LuaScriptMgr.GetString(L, 3);
			LuaFunction arg2 = LuaScriptMgr.ToLuaFunction(L, 4);
			obj.GetTexture(arg0,arg1,arg2);
			return 0;
		}
		else if (count == 4 && LuaScriptMgr.CheckTypes(L, 1, typeof(UITextureMgr), typeof(ImageType), typeof(string), typeof(Action<UITextureAsset>)))
		{
			UITextureMgr obj = (UITextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "UITextureMgr");
			ImageType arg0 = (ImageType)LuaScriptMgr.GetLuaObject(L, 2);
			string arg1 = LuaScriptMgr.GetString(L, 3);
			Action<UITextureAsset> arg2 = null;
			LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

			if (funcType4 != LuaTypes.LUA_TFUNCTION)
			{
				 arg2 = (Action<UITextureAsset>)LuaScriptMgr.GetLuaObject(L, 4);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 4);
				arg2 = (param0) =>
				{
					int top = func.BeginPCall();
					LuaScriptMgr.PushObject(L, param0);
					func.PCall(top, 1);
					func.EndPCall(top);
				};
			}

			obj.GetTexture(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UITextureMgr.GetTexture");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetModelSkinnedAssets(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		UITextureMgr obj = (UITextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "UITextureMgr");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		Action<FFAssetBundle> arg2 = null;
		LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

		if (funcType4 != LuaTypes.LUA_TFUNCTION)
		{
			 arg2 = (Action<FFAssetBundle>)LuaScriptMgr.GetNetObject(L, 4, typeof(Action<FFAssetBundle>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 4);
			arg2 = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.PushObject(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}

		obj.GetModelSkinnedAssets(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSpriteFromAtlas(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		UITextureMgr obj = (UITextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "UITextureMgr");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		Action<Sprite> arg2 = null;
		LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

		if (funcType4 != LuaTypes.LUA_TFUNCTION)
		{
			 arg2 = (Action<Sprite>)LuaScriptMgr.GetNetObject(L, 4, typeof(Action<Sprite>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 4);
			arg2 = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}

		obj.GetSpriteFromAtlas(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadAtlas(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		UITextureMgr obj = (UITextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "UITextureMgr");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		Action<Sprite[]> arg1 = null;
		LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

		if (funcType3 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (Action<Sprite[]>)LuaScriptMgr.GetNetObject(L, 3, typeof(Action<Sprite[]>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
			arg1 = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.PushArray(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}

		obj.LoadAtlas(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetImageGrey(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(UITextureMgr), typeof(RawImage), typeof(bool)))
		{
			UITextureMgr obj = (UITextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "UITextureMgr");
			RawImage arg0 = (RawImage)LuaScriptMgr.GetLuaObject(L, 2);
			bool arg1 = LuaDLL.lua_toboolean(L, 3);
			obj.SetImageGrey(arg0,arg1);
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(UITextureMgr), typeof(Image), typeof(bool)))
		{
			UITextureMgr obj = (UITextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "UITextureMgr");
			Image arg0 = (Image)LuaScriptMgr.GetLuaObject(L, 2);
			bool arg1 = LuaDLL.lua_toboolean(L, 3);
			obj.SetImageGrey(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UITextureMgr.SetImageGrey");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetImageGrey4Head(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		UITextureMgr obj = (UITextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "UITextureMgr");
		RawImage arg0 = (RawImage)LuaScriptMgr.GetUnityObject(L, 2, typeof(RawImage));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.SetImageGrey4Head(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnUpdate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UITextureMgr obj = (UITextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "UITextureMgr");
		obj.OnUpdate();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnReSet(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UITextureMgr obj = (UITextureMgr)LuaScriptMgr.GetNetObjectSelf(L, 1, "UITextureMgr");
		obj.OnReSet();
		return 0;
	}
}

