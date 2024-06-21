using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;

public class OtherPlayerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Init", Init),
			new LuaMethod("GetCharID", GetCharID),
			new LuaMethod("GetOccupation", GetOccupation),
			new LuaMethod("GetCurLevel", GetCurLevel),
			new LuaMethod("GetModePath", GetModePath),
			new LuaMethod("GetAnimatorControllerPath", GetAnimatorControllerPath),
			new LuaMethod("RefreshPlayerMapUserData", RefreshPlayerMapUserData),
			new LuaMethod("CreatPlayerModel", CreatPlayerModel),
			new LuaMethod("RefreshModelAndAnimatorCnotroller", RefreshModelAndAnimatorCnotroller),
			new LuaMethod("SetCollider", SetCollider),
			new LuaMethod("SetPlayerLayer", SetPlayerLayer),
			new LuaMethod("InitCamera", InitCamera),
			new LuaMethod("InitComponent", InitComponent),
			new LuaMethod("Update", Update),
			new LuaMethod("Die", Die),
			new LuaMethod("DelayRelive", DelayRelive),
			new LuaMethod("DestroyThisInNineScreen", DestroyThisInNineScreen),
			new LuaMethod("TrueDestroy", TrueDestroy),
			new LuaMethod("DestroyModel", DestroyModel),
			new LuaMethod("RevertHpMp", RevertHpMp),
			new LuaMethod("HandleHit", HandleHit),
			new LuaMethod("CancelSelect", CancelSelect),
			new LuaMethod("TargetSelect", TargetSelect),
			new LuaMethod("OnRelationChange", OnRelationChange),
			new LuaMethod("HitOther", HitOther),
			new LuaMethod("New", _CreateOtherPlayer),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("ModelHoldHelper", get_ModelHoldHelper, set_ModelHoldHelper),
			new LuaField("onModelCreate", get_onModelCreate, set_onModelCreate),
			new LuaField("OtherPlayerData", get_OtherPlayerData, null),
			new LuaField("InBattleState", get_InBattleState, null),
		};

		LuaScriptMgr.RegisterLib(L, "OtherPlayer", typeof(OtherPlayer), regs, fields, typeof(CharactorBase));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateOtherPlayer(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			OtherPlayer obj = new OtherPlayer();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: OtherPlayer.New");
		}

		return 0;
	}

	static Type classType = typeof(OtherPlayer);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ModelHoldHelper(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		OtherPlayer obj = (OtherPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ModelHoldHelper");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ModelHoldHelper on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.ModelHoldHelper);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onModelCreate(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		OtherPlayer obj = (OtherPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onModelCreate");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onModelCreate on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.onModelCreate);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_OtherPlayerData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		OtherPlayer obj = (OtherPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OtherPlayerData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OtherPlayerData on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.OtherPlayerData);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_InBattleState(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		OtherPlayer obj = (OtherPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name InBattleState");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index InBattleState on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.InBattleState);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ModelHoldHelper(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		OtherPlayer obj = (OtherPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ModelHoldHelper");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ModelHoldHelper on a nil value");
			}
		}

		obj.ModelHoldHelper = (PlayerCharactorCreateHelper)LuaScriptMgr.GetNetObject(L, 3, typeof(PlayerCharactorCreateHelper));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onModelCreate(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		OtherPlayer obj = (OtherPlayer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onModelCreate");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onModelCreate on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.onModelCreate = (Action)LuaScriptMgr.GetNetObject(L, 3, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.onModelCreate = () =>
			{
				func.Call();
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.Init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCharID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		ulong o = obj.GetCharID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetOccupation(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		uint o = obj.GetOccupation();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurLevel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		uint o = obj.GetCurLevel();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetModePath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		string o = obj.GetModePath(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAnimatorControllerPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		string o = obj.GetAnimatorControllerPath(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshPlayerMapUserData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.RefreshPlayerMapUserData();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreatPlayerModel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.CreatPlayerModel();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshModelAndAnimatorCnotroller(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.RefreshModelAndAnimatorCnotroller();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCollider(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.SetCollider();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPlayerLayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		obj.SetPlayerLayer(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitCamera(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.InitCamera();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitComponent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.InitComponent();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Update(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.Update();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Die(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.Die();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DelayRelive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.DelayRelive();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyThisInNineScreen(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.DestroyThisInNineScreen();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TrueDestroy(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.TrueDestroy();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyModel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.DestroyModel();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RevertHpMp(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		magic.MSG_Ret_HpMpPop_SC arg0 = (magic.MSG_Ret_HpMpPop_SC)LuaScriptMgr.GetNetObject(L, 2, typeof(magic.MSG_Ret_HpMpPop_SC));
		obj.RevertHpMp(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HandleHit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		magic.MSG_Ret_MagicAttack_SC arg0 = (magic.MSG_Ret_MagicAttack_SC)LuaScriptMgr.GetNetObject(L, 2, typeof(magic.MSG_Ret_MagicAttack_SC));
		obj.HandleHit(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CancelSelect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.CancelSelect();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TargetSelect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.TargetSelect();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnRelationChange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		obj.OnRelationChange();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HitOther(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		OtherPlayer obj = (OtherPlayer)LuaScriptMgr.GetNetObjectSelf(L, 1, "OtherPlayer");
		magic.MSG_Ret_MagicAttack_SC arg0 = (magic.MSG_Ret_MagicAttack_SC)LuaScriptMgr.GetNetObject(L, 2, typeof(magic.MSG_Ret_MagicAttack_SC));
		CharactorBase[] objs1 = LuaScriptMgr.GetArrayObject<CharactorBase>(L, 3);
		obj.HitOther(arg0,objs1);
		return 0;
	}
}

