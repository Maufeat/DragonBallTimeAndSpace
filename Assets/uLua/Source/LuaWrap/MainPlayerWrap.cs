using System;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using LuaInterface;

public class MainPlayerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("OnNpcHatredListChange", OnNpcHatredListChange),
			new LuaMethod("BreakAutoAttack", BreakAutoAttack),
			new LuaMethod("SwitchAutoAttack", SwitchAutoAttack),
			new LuaMethod("Init", Init),
			new LuaMethod("InitMainPlayer", InitMainPlayer),
			new LuaMethod("CreatPlayerModel", CreatPlayerModel),
			new LuaMethod("InitComponent", InitComponent),
			new LuaMethod("OnClickEvent", OnClickEvent),
			new LuaMethod("MoveToByDir", MoveToByDir),
			new LuaMethod("GetBestPointNearbyBlockPoint", GetBestPointNearbyBlockPoint),
			new LuaMethod("GetBestPointNearbyBlockPoint1", GetBestPointNearbyBlockPoint1),
			new LuaMethod("ForceSetPositionTo", ForceSetPositionTo),
			new LuaMethod("GetClickPoint", GetClickPoint),
			new LuaMethod("TryReleaseSkill", TryReleaseSkill),
			new LuaMethod("Die", Die),
			new LuaMethod("OnMoveOneStep", OnMoveOneStep),
			new LuaMethod("DestroyThisInNineScreen", DestroyThisInNineScreen),
			new LuaMethod("HandleHit", HandleHit),
			new LuaMethod("HitOther", HitOther),
			new LuaMethod("RevertHpMp", RevertHpMp),
			new LuaMethod("UpdateCurrency", UpdateCurrency),
			new LuaMethod("RefreshCurrency", RefreshCurrency),
			new LuaMethod("GetCurrencyByID", GetCurrencyByID),
			new LuaMethod("GetMainPlayerName", GetMainPlayerName),
			new LuaMethod("GetSelfDir", GetSelfDir),
			new LuaMethod("GetSelfCurPos", GetSelfCurPos),
			new LuaMethod("GetMainPlayerFightValue", GetMainPlayerFightValue),
			new LuaMethod("MainPackageTableList", MainPackageTableList),
			new LuaMethod("TargetSelect", TargetSelect),
			new LuaMethod("CancelSelect", CancelSelect),
			new LuaMethod("New", _CreateMainPlayer),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("IsSendSingleMove", get_IsSendSingleMove, set_IsSendSingleMove),
			new LuaField("lastPosition2D", get_lastPosition2D, set_lastPosition2D),
			new LuaField("OnPlayerCuccencyUpdateEvent", get_OnPlayerCuccencyUpdateEvent, set_OnPlayerCuccencyUpdateEvent),
			new LuaField("autoAttack", get_autoAttack, null),
			new LuaField("Self", get_Self, null),
			new LuaField("MainPlayeData", get_MainPlayeData, null),
			new LuaField("CurMP", get_CurMP, null),
		};

		LuaScriptMgr.RegisterLib(L, "MainPlayer", typeof(MainPlayer), regs, fields, typeof(OtherPlayer));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateMainPlayer(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			MainPlayer obj = new MainPlayer();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: MainPlayer.New");
		}

		return 0;
	}

	static Type classType = typeof(MainPlayer);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsSendSingleMove(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MainPlayer obj = (MainPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsSendSingleMove");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsSendSingleMove on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsSendSingleMove);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lastPosition2D(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MainPlayer obj = (MainPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lastPosition2D");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lastPosition2D on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lastPosition2D);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_OnPlayerCuccencyUpdateEvent(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MainPlayer obj = (MainPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnPlayerCuccencyUpdateEvent");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnPlayerCuccencyUpdateEvent on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.OnPlayerCuccencyUpdateEvent);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_autoAttack(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MainPlayer obj = (MainPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name autoAttack");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index autoAttack on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.autoAttack);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Self(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, MainPlayer.Self);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MainPlayeData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MainPlayer obj = (MainPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name MainPlayeData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index MainPlayeData on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.MainPlayeData);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CurMP(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MainPlayer obj = (MainPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CurMP");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CurMP on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.CurMP);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_IsSendSingleMove(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MainPlayer obj = (MainPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsSendSingleMove");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsSendSingleMove on a nil value");
			}
		}

		obj.IsSendSingleMove = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lastPosition2D(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MainPlayer obj = (MainPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lastPosition2D");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lastPosition2D on a nil value");
			}
		}

		obj.lastPosition2D = LuaScriptMgr.GetVector2(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_OnPlayerCuccencyUpdateEvent(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MainPlayer obj = (MainPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnPlayerCuccencyUpdateEvent");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnPlayerCuccencyUpdateEvent on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.OnPlayerCuccencyUpdateEvent = (MainPlayer.VoidDelegate)LuaScriptMgr.GetNetObject(L, 3, typeof(MainPlayer.VoidDelegate));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.OnPlayerCuccencyUpdateEvent = () =>
			{
				func.Call();
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnNpcHatredListChange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		CharactorBase arg0 = (CharactorBase)LuaScriptMgr.GetNetObject(L, 2, typeof(CharactorBase));
		obj.OnNpcHatredListChange(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BreakAutoAttack(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		obj.BreakAutoAttack();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SwitchAutoAttack(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.SwitchAutoAttack(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		obj.Init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitMainPlayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		obj.InitMainPlayer();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreatPlayerModel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		obj.CreatPlayerModel();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitComponent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		obj.InitComponent();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnClickEvent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		ScreenEvent arg0 = (ScreenEvent)LuaScriptMgr.GetNetObject(L, 2, typeof(ScreenEvent));
		obj.OnClickEvent(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MoveToByDir(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.MoveToByDir(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetBestPointNearbyBlockPoint(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
		Vector2 o = obj.GetBestPointNearbyBlockPoint(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetBestPointNearbyBlockPoint1(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		cs_MoveData o = obj.GetBestPointNearbyBlockPoint1(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ForceSetPositionTo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
		obj.ForceSetPositionTo(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClickPoint(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		Vector3 arg0 = LuaScriptMgr.GetVector3(L, 2);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		Vector3 o = obj.GetClickPoint(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TryReleaseSkill(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		obj.TryReleaseSkill(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Die(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		obj.Die();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnMoveOneStep(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		obj.OnMoveOneStep();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyThisInNineScreen(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		obj.DestroyThisInNineScreen();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HandleHit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		magic.MSG_Ret_MagicAttack_SC arg0 = (magic.MSG_Ret_MagicAttack_SC)LuaScriptMgr.GetNetObject(L, 2, typeof(magic.MSG_Ret_MagicAttack_SC));
		obj.HandleHit(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HitOther(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		magic.MSG_Ret_MagicAttack_SC arg0 = (magic.MSG_Ret_MagicAttack_SC)LuaScriptMgr.GetNetObject(L, 2, typeof(magic.MSG_Ret_MagicAttack_SC));
		CharactorBase[] objs1 = LuaScriptMgr.GetArrayObject<CharactorBase>(L, 3);
		obj.HitOther(arg0,objs1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RevertHpMp(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		magic.MSG_Ret_HpMpPop_SC arg0 = (magic.MSG_Ret_HpMpPop_SC)LuaScriptMgr.GetNetObject(L, 2, typeof(magic.MSG_Ret_HpMpPop_SC));
		obj.RevertHpMp(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UpdateCurrency(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		obj.UpdateCurrency();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshCurrency(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		obj.RefreshCurrency();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurrencyByID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		uint o = obj.GetCurrencyByID(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMainPlayerName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		string o = obj.GetMainPlayerName();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSelfDir(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		uint o = obj.GetSelfDir();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSelfCurPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		Vector2 o = obj.GetSelfCurPos();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMainPlayerFightValue(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		uint o = obj.GetMainPlayerFightValue();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MainPackageTableList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		List<PropsBase> o = obj.MainPackageTableList();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TargetSelect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		obj.TargetSelect();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CancelSelect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		MainPlayer obj = (MainPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "MainPlayer");
		obj.CancelSelect();
		return 0;
	}
}

