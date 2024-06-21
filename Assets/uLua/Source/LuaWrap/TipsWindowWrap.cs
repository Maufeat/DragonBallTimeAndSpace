using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public class TipsWindowWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("InitWindow", InitWindow),
			new LuaMethod("ShowWindow", ShowWindow),
			new LuaMethod("ShowNotice", ShowNotice),
			new LuaMethod("ShowTaskTips", ShowTaskTips),
			new LuaMethod("New", _CreateTipsWindow),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("noticeContain", get_noticeContain, set_noticeContain),
			new LuaField("tipsUI", get_tipsUI, null),
		};

		LuaScriptMgr.RegisterLib(L, "TipsWindow", typeof(TipsWindow), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateTipsWindow(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			TipsWindow obj = new TipsWindow();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: TipsWindow.New");
		}

		return 0;
	}

	static Type classType = typeof(TipsWindow);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_noticeContain(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, TipsWindow.noticeContain);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_tipsUI(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, TipsWindow.tipsUI);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_noticeContain(IntPtr L)
	{
		TipsWindow.noticeContain = (Queue<NoticeModel>)LuaScriptMgr.GetNetObject(L, 3, typeof(Queue<NoticeModel>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitWindow(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		TipsWindow.InitWindow();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowWindow(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(NoticeModel)))
		{
			NoticeModel arg0 = (NoticeModel)LuaScriptMgr.GetLuaObject(L, 1);
			TipsWindow.ShowWindow(arg0);
			return 0;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(uint)))
		{
			uint arg0 = (uint)LuaDLL.lua_tonumber(L, 1);
			TipsWindow.ShowWindow(arg0);
			return 0;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(string)))
		{
			string arg0 = LuaScriptMgr.GetString(L, 1);
			TipsWindow.ShowWindow(arg0);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(string), typeof(LuaTable)))
		{
			string arg0 = LuaScriptMgr.GetString(L, 1);
			Color arg1 = LuaScriptMgr.GetColor(L, 2);
			TipsWindow.ShowWindow(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(uint), typeof(string[])))
		{
			uint arg0 = (uint)LuaDLL.lua_tonumber(L, 1);
			string[] objs1 = LuaScriptMgr.GetArrayString(L, 2);
			TipsWindow.ShowWindow(arg0,objs1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(TipsType), typeof(string[])))
		{
			TipsType arg0 = (TipsType)LuaScriptMgr.GetLuaObject(L, 1);
			string[] objs1 = LuaScriptMgr.GetArrayString(L, 2);
			TipsWindow.ShowWindow(arg0,objs1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: TipsWindow.ShowWindow");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowNotice(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(NoticeModel)))
		{
			NoticeModel arg0 = (NoticeModel)LuaScriptMgr.GetLuaObject(L, 1);
			TipsWindow.ShowNotice(arg0);
			return 0;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(uint)))
		{
			uint arg0 = (uint)LuaDLL.lua_tonumber(L, 1);
			TipsWindow.ShowNotice(arg0);
			return 0;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(string)))
		{
			string arg0 = LuaScriptMgr.GetString(L, 1);
			TipsWindow.ShowNotice(arg0);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: TipsWindow.ShowNotice");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowTaskTips(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		TipsWindow.ShowTaskTips(arg0);
		return 0;
	}
}

