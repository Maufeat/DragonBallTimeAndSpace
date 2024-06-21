using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class UITextureAssetWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("AddUse", AddUse),
			new LuaMethod("TryUnload", TryUnload),
			new LuaMethod("New", _CreateUITextureAsset),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("textureObj", get_textureObj, null),
		};

		LuaScriptMgr.RegisterLib(L, "UITextureAsset", typeof(UITextureAsset), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUITextureAsset(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			UIAssetObj arg0 = (UIAssetObj)LuaScriptMgr.GetNetObject(L, 1, typeof(UIAssetObj));
			UITextureAsset obj = new UITextureAsset(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UITextureAsset.New");
		}

		return 0;
	}

	static Type classType = typeof(UITextureAsset);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_textureObj(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UITextureAsset obj = (UITextureAsset)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name textureObj");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index textureObj on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.textureObj);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddUse(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UITextureAsset obj = (UITextureAsset)LuaScriptMgr.GetNetObjectSelf(L, 1, "UITextureAsset");
		obj.AddUse();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TryUnload(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UITextureAsset obj = (UITextureAsset)LuaScriptMgr.GetNetObjectSelf(L, 1, "UITextureAsset");
		obj.TryUnload();
		return 0;
	}
}

