using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class UI_NpcDlgWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("UnInit", UnInit),
			new LuaMethod("CallCsFunctionImmediate", CallCsFunctionImmediate),
			new LuaMethod("EndDlg", EndDlg),
			new LuaMethod("AddTalk", AddTalk),
			new LuaMethod("AddTalkByID", AddTalkByID),
			new LuaMethod("AddDialogItem", AddDialogItem),
			new LuaMethod("AddDialogItemByID", AddDialogItemByID),
			new LuaMethod("AddDramaTalk", AddDramaTalk),
			new LuaMethod("AddDramaTalkByID", AddDramaTalkByID),
			new LuaMethod("AddDramaItem", AddDramaItem),
			new LuaMethod("AddDramaItemByID", AddDramaItemByID),
			new LuaMethod("AddDramaGroupByID", AddDramaGroupByID),
			new LuaMethod("ResetNPCDir", ResetNPCDir),
			new LuaMethod("New", _CreateUI_NpcDlg),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UI_NpcDlg", typeof(UI_NpcDlg), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUI_NpcDlg(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			UI_NpcDlg obj = new UI_NpcDlg();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UI_NpcDlg.New");
		}

		return 0;
	}

	static Type classType = typeof(UI_NpcDlg);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnInit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UI_NpcDlg obj = (UI_NpcDlg)LuaScriptMgr.GetNetObjectSelf(L, 1, "UI_NpcDlg");
		obj.UnInit();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CallCsFunctionImmediate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UI_NpcDlg obj = (UI_NpcDlg)LuaScriptMgr.GetNetObjectSelf(L, 1, "UI_NpcDlg");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.CallCsFunctionImmediate(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EndDlg(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UI_NpcDlg obj = (UI_NpcDlg)LuaScriptMgr.GetNetObjectSelf(L, 1, "UI_NpcDlg");
		obj.EndDlg();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddTalk(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UI_NpcDlg obj = (UI_NpcDlg)LuaScriptMgr.GetNetObjectSelf(L, 1, "UI_NpcDlg");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.AddTalk(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddTalkByID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UI_NpcDlg obj = (UI_NpcDlg)LuaScriptMgr.GetNetObjectSelf(L, 1, "UI_NpcDlg");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		obj.AddTalkByID(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddDialogItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		UI_NpcDlg obj = (UI_NpcDlg)LuaScriptMgr.GetNetObjectSelf(L, 1, "UI_NpcDlg");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		string arg2 = LuaScriptMgr.GetLuaString(L, 4);
		obj.AddDialogItem(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddDialogItemByID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		UI_NpcDlg obj = (UI_NpcDlg)LuaScriptMgr.GetNetObjectSelf(L, 1, "UI_NpcDlg");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 3);
		string arg2 = LuaScriptMgr.GetLuaString(L, 4);
		obj.AddDialogItemByID(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddDramaTalk(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UI_NpcDlg obj = (UI_NpcDlg)LuaScriptMgr.GetNetObjectSelf(L, 1, "UI_NpcDlg");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.AddDramaTalk(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddDramaTalkByID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UI_NpcDlg obj = (UI_NpcDlg)LuaScriptMgr.GetNetObjectSelf(L, 1, "UI_NpcDlg");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		obj.AddDramaTalkByID(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddDramaItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		UI_NpcDlg obj = (UI_NpcDlg)LuaScriptMgr.GetNetObjectSelf(L, 1, "UI_NpcDlg");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		string arg2 = LuaScriptMgr.GetLuaString(L, 4);
		obj.AddDramaItem(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddDramaItemByID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		UI_NpcDlg obj = (UI_NpcDlg)LuaScriptMgr.GetNetObjectSelf(L, 1, "UI_NpcDlg");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 3);
		string arg2 = LuaScriptMgr.GetLuaString(L, 4);
		obj.AddDramaItemByID(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddDramaGroupByID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		UI_NpcDlg obj = (UI_NpcDlg)LuaScriptMgr.GetNetObjectSelf(L, 1, "UI_NpcDlg");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		obj.AddDramaGroupByID(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetNPCDir(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UI_NpcDlg obj = (UI_NpcDlg)LuaScriptMgr.GetNetObjectSelf(L, 1, "UI_NpcDlg");
		obj.ResetNPCDir();
		return 0;
	}
}

