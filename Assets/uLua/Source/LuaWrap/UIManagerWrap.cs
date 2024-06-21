using System;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using LuaInterface;

public class UIManagerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("GetUIObject", GetUIObject),
			new LuaMethod("AddUIPanel", AddUIPanel),
			new LuaMethod("AddLuaUIPanel", AddLuaUIPanel),
			new LuaMethod("GetLuaUIPanel", GetLuaUIPanel),
			new LuaMethod("IsLuaPanelExists", IsLuaPanelExists),
			new LuaMethod("IsUIPanelExists", IsUIPanelExists),
			new LuaMethod("RemoveUIPanel", RemoveUIPanel),
			new LuaMethod("RemoveLuaUIPanel", RemoveLuaUIPanel),
			new LuaMethod("Init", Init),
			new LuaMethod("ReLoad", ReLoad),
			new LuaMethod("GetUICamera", GetUICamera),
			new LuaMethod("GetUIParent", GetUIParent),
			new LuaMethod("maskFadeInOut", maskFadeInOut),
			new LuaMethod("onFadeInOut", onFadeInOut),
			new LuaMethod("ResetMaskInfo", ResetMaskInfo),
			new LuaMethod("SetUIVisible", SetUIVisible),
			new LuaMethod("ShowUI", ShowUI),
			new LuaMethod("ShowUIByNpcdlg", ShowUIByNpcdlg),
			new LuaMethod("getUIInformation", getUIInformation),
			new LuaMethod("DeleteUI", DeleteUI),
			new LuaMethod("DeleteAllUI", DeleteAllUI),
			new LuaMethod("DeleteAllUIWithOutList", DeleteAllUIWithOutList),
			new LuaMethod("OnMapChangeCloseUI", OnMapChangeCloseUI),
			new LuaMethod("RegUINameOpenByNpc", RegUINameOpenByNpc),
			new LuaMethod("UnRegUINameOpenByNpc", UnRegUINameOpenByNpc),
			new LuaMethod("CloseOpenByNpcUI", CloseOpenByNpcUI),
			new LuaMethod("DeleteUIWithNameList", DeleteUIWithNameList),
			new LuaMethod("DramaHideUI", DramaHideUI),
			new LuaMethod("DramaActiviteUI", DramaActiviteUI),
			new LuaMethod("LoadUI", LoadUI),
			new LuaMethod("SetMaskImage", SetMaskImage),
			new LuaMethod("SetMaskImageAlpha", SetMaskImageAlpha),
			new LuaMethod("RegisterUIName", RegisterUIName),
			new LuaMethod("GetItemBgSpriteName", GetItemBgSpriteName),
			new LuaMethod("GetItemTipBigBgImg", GetItemTipBigBgImg),
			new LuaMethod("ShowMainUI", ShowMainUI),
			new LuaMethod("OnUpdate", OnUpdate),
			new LuaMethod("OnReSet", OnReSet),
			new LuaMethod("ClearListChildrens", ClearListChildrens),
			new LuaMethod("SetRawImage", SetRawImage),
			new LuaMethod("GetUITransform", GetUITransform),
			new LuaMethod("GuideDeleteUI", GuideDeleteUI),
			new LuaMethod("New", _CreateUIManager),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Instance", get_Instance, null),
			new LuaField("Img_mask", get_Img_mask, null),
			new LuaField("UIRoot", get_UIRoot, null),
			new LuaField("ManagerName", get_ManagerName, null),
		};

		LuaScriptMgr.RegisterLib(L, "UIManager", typeof(UIManager), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUIManager(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			UIManager obj = new UIManager();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UIManager.New");
		}

		return 0;
	}

	static Type classType = typeof(UIManager);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, UIManager.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Img_mask(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIManager obj = (UIManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Img_mask");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Img_mask on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Img_mask);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UIRoot(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIManager obj = (UIManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name UIRoot");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index UIRoot on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.UIRoot);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ManagerName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIManager obj = (UIManager)o;

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
	static int GetUIObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		UIPanelBase o = UIManager.GetUIObject(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddUIPanel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIPanelBase arg0 = (UIPanelBase)LuaScriptMgr.GetNetObject(L, 1, typeof(UIPanelBase));
		UIManager.AddUIPanel(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddLuaUIPanel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaPanelBase arg0 = (LuaPanelBase)LuaScriptMgr.GetNetObject(L, 1, typeof(LuaPanelBase));
		UIManager.AddLuaUIPanel(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLuaUIPanel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaPanelBase o = UIManager.GetLuaUIPanel(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsLuaPanelExists(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = UIManager.IsLuaPanelExists(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsUIPanelExists(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(Type)))
		{
			Type arg0 = LuaScriptMgr.GetTypeObject(L, 1);
			bool o = UIManager.IsUIPanelExists(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(string)))
		{
			string arg0 = LuaScriptMgr.GetString(L, 1);
			bool o = UIManager.IsUIPanelExists(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UIManager.IsUIPanelExists");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveUIPanel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIPanelBase arg0 = (UIPanelBase)LuaScriptMgr.GetNetObject(L, 1, typeof(UIPanelBase));
		UIManager.RemoveUIPanel(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveLuaUIPanel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaPanelBase arg0 = (LuaPanelBase)LuaScriptMgr.GetNetObject(L, 1, typeof(LuaPanelBase));
		UIManager.RemoveLuaUIPanel(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		obj.Init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReLoad(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		obj.ReLoad();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetUICamera(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		Transform o = obj.GetUICamera();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetUIParent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		UIManager.ParentType arg0 = (UIManager.ParentType)LuaScriptMgr.GetNetObject(L, 2, typeof(UIManager.ParentType));
		Transform o = obj.GetUIParent(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int maskFadeInOut(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
		UIManager.OnMaskFinished arg3 = null;
		LuaTypes funcType5 = LuaDLL.lua_type(L, 5);

		if (funcType5 != LuaTypes.LUA_TFUNCTION)
		{
			 arg3 = (UIManager.OnMaskFinished)LuaScriptMgr.GetNetObject(L, 5, typeof(UIManager.OnMaskFinished));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 5);
			arg3 = () =>
			{
				func.Call();
			};
		}

		obj.maskFadeInOut(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onFadeInOut(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		UITweener arg0 = (UITweener)LuaScriptMgr.GetUnityObject(L, 2, typeof(UITweener));
		obj.onFadeInOut(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetMaskInfo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.ResetMaskInfo(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetUIVisible(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		UIManager.ParentType arg0 = (UIManager.ParentType)LuaScriptMgr.GetNetObject(L, 2, typeof(UIManager.ParentType));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.SetUIVisible(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowUI(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3)
		{
			UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			UIManager.ParentType arg1 = (UIManager.ParentType)LuaScriptMgr.GetNetObject(L, 3, typeof(UIManager.ParentType));
			obj.ShowUI(arg0,arg1);
			return 0;
		}
		else if (count == 4)
		{
			UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 3);
			UIManager.ParentType arg2 = (UIManager.ParentType)LuaScriptMgr.GetNetObject(L, 4, typeof(UIManager.ParentType));
			obj.ShowUI(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UIManager.ShowUI");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowUIByNpcdlg(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3)
		{
			UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			UIManager.ParentType arg1 = (UIManager.ParentType)LuaScriptMgr.GetNetObject(L, 3, typeof(UIManager.ParentType));
			obj.ShowUIByNpcdlg(arg0,arg1);
			return 0;
		}
		else if (count == 4)
		{
			UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 3);
			UIManager.ParentType arg2 = (UIManager.ParentType)LuaScriptMgr.GetNetObject(L, 4, typeof(UIManager.ParentType));
			obj.ShowUIByNpcdlg(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UIManager.ShowUIByNpcdlg");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getUIInformation(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		obj.getUIInformation(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DeleteUI(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(UIManager), typeof(UIPanelBase)))
		{
			UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
			UIPanelBase arg0 = (UIPanelBase)LuaScriptMgr.GetLuaObject(L, 2);
			obj.DeleteUI(arg0);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(UIManager), typeof(string)))
		{
			UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			obj.DeleteUI(arg0);
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(UIManager), typeof(UIPanelBase), typeof(bool)))
		{
			UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
			UIPanelBase arg0 = (UIPanelBase)LuaScriptMgr.GetLuaObject(L, 2);
			bool arg1 = LuaDLL.lua_toboolean(L, 3);
			obj.DeleteUI(arg0,arg1);
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(UIManager), typeof(string), typeof(bool)))
		{
			UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			bool arg1 = LuaDLL.lua_toboolean(L, 3);
			obj.DeleteUI(arg0,arg1);
			return 0;
		}
		else if (count == 4)
		{
			UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
			LuaPanelBase arg0 = (LuaPanelBase)LuaScriptMgr.GetNetObject(L, 2, typeof(LuaPanelBase));
			bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
			bool arg2 = LuaScriptMgr.GetBoolean(L, 4);
			obj.DeleteUI(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UIManager.DeleteUI");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DeleteAllUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.DeleteAllUI(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DeleteAllUIWithOutList(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(UIManager), typeof(List<string>)))
		{
			UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
			List<string> arg0 = (List<string>)LuaScriptMgr.GetLuaObject(L, 2);
			obj.DeleteAllUIWithOutList(arg0);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(UIManager), typeof(List<Type>)))
		{
			UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
			List<Type> arg0 = (List<Type>)LuaScriptMgr.GetLuaObject(L, 2);
			obj.DeleteAllUIWithOutList(arg0);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UIManager.DeleteAllUIWithOutList");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnMapChangeCloseUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		obj.OnMapChangeCloseUI();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RegUINameOpenByNpc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.RegUINameOpenByNpc(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnRegUINameOpenByNpc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.UnRegUINameOpenByNpc(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CloseOpenByNpcUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		obj.CloseOpenByNpcUI();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DeleteUIWithNameList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		List<string> arg0 = (List<string>)LuaScriptMgr.GetNetObject(L, 2, typeof(List<string>));
		obj.DeleteUIWithNameList(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DramaHideUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		obj.DramaHideUI(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DramaActiviteUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		obj.DramaActiviteUI(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		Action<GameObject> arg2 = null;
		LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

		if (funcType4 != LuaTypes.LUA_TFUNCTION)
		{
			 arg2 = (Action<GameObject>)LuaScriptMgr.GetNetObject(L, 4, typeof(Action<GameObject>));
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

		obj.LoadUI(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetMaskImage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.SetMaskImage(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetMaskImageAlpha(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		obj.SetMaskImageAlpha(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RegisterUIName(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			obj.RegisterUIName(arg0);
			return 0;
		}
		else if (count == 3)
		{
			UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			string arg1 = LuaScriptMgr.GetLuaString(L, 3);
			obj.RegisterUIName(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UIManager.RegisterUIName");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetItemBgSpriteName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		string o = obj.GetItemBgSpriteName(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetItemTipBigBgImg(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		string o = obj.GetItemTipBigBgImg(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMainUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		obj.ShowMainUI();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnUpdate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		obj.OnUpdate();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnReSet(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		obj.OnReSet();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearListChildrens(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		obj.ClearListChildrens(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetRawImage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		ImageType arg0 = (ImageType)LuaScriptMgr.GetNetObject(L, 2, typeof(ImageType));
		RawImage arg1 = (RawImage)LuaScriptMgr.GetUnityObject(L, 3, typeof(RawImage));
		string arg2 = LuaScriptMgr.GetLuaString(L, 4);
		obj.SetRawImage(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetUITransform(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		Transform o = UIManager.GetUITransform(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GuideDeleteUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIManager obj = (UIManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "UIManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.GuideDeleteUI(arg0);
		return 0;
	}
}

