using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;

public class RichTextWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("GenerateRichText", GenerateRichText),
			new LuaMethod("CreatRichText", CreatRichText),
			new LuaMethod("ConvertRichText", ConvertRichText),
			new LuaMethod("New", _CreateRichText),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("chatData", get_chatData, set_chatData),
			new LuaField("LineCount", get_LineCount, null),
		};

		LuaScriptMgr.RegisterLib(L, "RichText", typeof(RichText), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateRichText(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 4)
		{
			Chat.ChatData arg0 = (Chat.ChatData)LuaScriptMgr.GetNetObject(L, 1, typeof(Chat.ChatData));
			string arg1 = LuaScriptMgr.GetLuaString(L, 2);
			GameObject arg2 = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
			float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
			RichText obj = new RichText(arg0,arg1,arg2,arg3);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: RichText.New");
		}

		return 0;
	}

	static Type classType = typeof(RichText);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_chatData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RichText obj = (RichText)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name chatData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index chatData on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.chatData);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_LineCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RichText obj = (RichText)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name LineCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index LineCount on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.LineCount);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_chatData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RichText obj = (RichText)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name chatData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index chatData on a nil value");
			}
		}

		obj.chatData = (Chat.ChatData)LuaScriptMgr.GetNetObject(L, 3, typeof(Chat.ChatData));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GenerateRichText(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RichText obj = (RichText)LuaScriptMgr.GetNetObjectSelf(L, 1, "RichText");
		obj.GenerateRichText();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreatRichText(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RichText obj = (RichText)LuaScriptMgr.GetNetObjectSelf(L, 1, "RichText");
		RichTextData arg0 = (RichTextData)LuaScriptMgr.GetNetObject(L, 2, typeof(RichTextData));
		obj.CreatRichText(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ConvertRichText(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RichText obj = (RichText)LuaScriptMgr.GetNetObjectSelf(L, 1, "RichText");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string o = obj.ConvertRichText(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

