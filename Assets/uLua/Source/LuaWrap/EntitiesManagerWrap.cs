using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public class EntitiesManagerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("DoSendOrWaitSendWeGameRevenue", DoSendOrWaitSendWeGameRevenue),
			new LuaMethod("Init", Init),
			new LuaMethod("setPlayerShowState", setPlayerShowState),
			new LuaMethod("SectorSeachBaseMainPlayer", SectorSeachBaseMainPlayer),
			new LuaMethod("RectangleSeachBaseMainPlayer", RectangleSeachBaseMainPlayer),
			new LuaMethod("SearchNearNPCById", SearchNearNPCById),
			new LuaMethod("CheckRelationBaseMainPlayer", CheckRelationBaseMainPlayer),
			new LuaMethod("GetNpcRelationType", GetNpcRelationType),
			new LuaMethod("GetOtherPlayerRelationType", GetOtherPlayerRelationType),
			new LuaMethod("GetDistance", GetDistance),
			new LuaMethod("OnMainCameraChange", OnMainCameraChange),
			new LuaMethod("SummonNpc", SummonNpc),
			new LuaMethod("AddCharacter", AddCharacter),
			new LuaMethod("RemoveCharacter", RemoveCharacter),
			new LuaMethod("SetVisiblePlayerLimit", SetVisiblePlayerLimit),
			new LuaMethod("ShowPlayer", ShowPlayer),
			new LuaMethod("HidePlayer", HidePlayer),
			new LuaMethod("IsMasterOrApprentice", IsMasterOrApprentice),
			new LuaMethod("SetCurrentMapEnablePlayerLimit", SetCurrentMapEnablePlayerLimit),
			new LuaMethod("SetPlayerCountLimit", SetPlayerCountLimit),
			new LuaMethod("SetPlayerNameActive", SetPlayerNameActive),
			new LuaMethod("SetNPCActive", SetNPCActive),
			new LuaMethod("GetPlayerByID", GetPlayerByID),
			new LuaMethod("GetCharactorByID", GetCharactorByID),
			new LuaMethod("GetNpcsByBaseidInFun", GetNpcsByBaseidInFun),
			new LuaMethod("AddNpc", AddNpc),
			new LuaMethod("AddFunNpc", AddFunNpc),
			new LuaMethod("ClearFunNpc", ClearFunNpc),
			new LuaMethod("IsFunNpc", IsFunNpc),
			new LuaMethod("RemoveNpc", RemoveNpc),
			new LuaMethod("LogOutALLFuncNpcMap", LogOutALLFuncNpcMap),
			new LuaMethod("RefreshFuncNpcList", RefreshFuncNpcList),
			new LuaMethod("GetNpc", GetNpc),
			new LuaMethod("GetCharactorFromGameObject", GetCharactorFromGameObject),
			new LuaMethod("GetCharactorFromCharid", GetCharactorFromCharid),
			new LuaMethod("UnLoadCharactors", UnLoadCharactors),
			new LuaMethod("IsMainPlayer", IsMainPlayer),
			new LuaMethod("UnInitialize", UnInitialize),
			new LuaMethod("OnUpdate", OnUpdate),
			new LuaMethod("OnReSet", OnReSet),
			new LuaMethod("StartCacheEentityAction", StartCacheEentityAction),
			new LuaMethod("RunAllEentityActionCacheAndClear", RunAllEentityActionCacheAndClear),
			new LuaMethod("IsAbattoirSceneRelive", IsAbattoirSceneRelive),
			new LuaMethod("DoEentityActionOrCacheForLua", DoEentityActionOrCacheForLua),
			new LuaMethod("DoEentityActionOrCache", DoEentityActionOrCache),
			new LuaMethod("ClearEentityActionCache", ClearEentityActionCache),
			new LuaMethod("ClearAllEentityActionCache", ClearAllEentityActionCache),
			new LuaMethod("RefreshNPCShowState", RefreshNPCShowState),
			new LuaMethod("New", _CreateEntitiesManager),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("wegame", get_wegame, set_wegame),
			new LuaField("wegametest", get_wegametest, set_wegametest),
			new LuaField("PNetWork", get_PNetWork, set_PNetWork),
			new LuaField("nMaxPlayerLimitCount", get_nMaxPlayerLimitCount, set_nMaxPlayerLimitCount),
			new LuaField("nMinPlayerLimitCount", get_nMinPlayerLimitCount, set_nMinPlayerLimitCount),
			new LuaField("CurrentNineScreenPlayers", get_CurrentNineScreenPlayers, set_CurrentNineScreenPlayers),
			new LuaField("bdicCurrentVisiblePlayer", get_bdicCurrentVisiblePlayer, set_bdicCurrentVisiblePlayer),
			new LuaField("bdicCurrentHidePlayer", get_bdicCurrentHidePlayer, set_bdicCurrentHidePlayer),
			new LuaField("NpcList", get_NpcList, set_NpcList),
			new LuaField("FuncNpcMap", get_FuncNpcMap, set_FuncNpcMap),
			new LuaField("bdicNpcKindOne", get_bdicNpcKindOne, set_bdicNpcKindOne),
			new LuaField("onMainPlayer", get_onMainPlayer, set_onMainPlayer),
			new LuaField("MainPlayer", get_MainPlayer, set_MainPlayer),
			new LuaField("SelectTarget", get_SelectTarget, set_SelectTarget),
			new LuaField("PlayerStateInCompetition", get_PlayerStateInCompetition, null),
			new LuaField("ManagerName", get_ManagerName, null),
		};

		LuaScriptMgr.RegisterLib(L, "EntitiesManager", typeof(EntitiesManager), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateEntitiesManager(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			EntitiesManager obj = new EntitiesManager();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: EntitiesManager.New");
		}

		return 0;
	}

	static Type classType = typeof(EntitiesManager);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_wegame(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name wegame");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index wegame on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.wegame);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_wegametest(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name wegametest");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index wegametest on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.wegametest);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PNetWork(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name PNetWork");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index PNetWork on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.PNetWork);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_nMaxPlayerLimitCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name nMaxPlayerLimitCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index nMaxPlayerLimitCount on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.nMaxPlayerLimitCount);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_nMinPlayerLimitCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name nMinPlayerLimitCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index nMinPlayerLimitCount on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.nMinPlayerLimitCount);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CurrentNineScreenPlayers(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CurrentNineScreenPlayers");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CurrentNineScreenPlayers on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.CurrentNineScreenPlayers);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bdicCurrentVisiblePlayer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bdicCurrentVisiblePlayer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bdicCurrentVisiblePlayer on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.bdicCurrentVisiblePlayer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bdicCurrentHidePlayer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bdicCurrentHidePlayer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bdicCurrentHidePlayer on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.bdicCurrentHidePlayer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_NpcList(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name NpcList");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index NpcList on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.NpcList);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FuncNpcMap(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FuncNpcMap");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FuncNpcMap on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.FuncNpcMap);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bdicNpcKindOne(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bdicNpcKindOne");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bdicNpcKindOne on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.bdicNpcKindOne);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onMainPlayer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onMainPlayer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onMainPlayer on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.onMainPlayer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MainPlayer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name MainPlayer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index MainPlayer on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.MainPlayer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SelectTarget(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name SelectTarget");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index SelectTarget on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.SelectTarget);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PlayerStateInCompetition(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name PlayerStateInCompetition");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index PlayerStateInCompetition on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.PlayerStateInCompetition);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ManagerName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

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
	static int set_wegame(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name wegame");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index wegame on a nil value");
			}
		}

		obj.wegame = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_wegametest(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name wegametest");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index wegametest on a nil value");
			}
		}

		obj.wegametest = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_PNetWork(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name PNetWork");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index PNetWork on a nil value");
			}
		}

		obj.PNetWork = (PlayerNetWork)LuaScriptMgr.GetNetObject(L, 3, typeof(PlayerNetWork));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_nMaxPlayerLimitCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name nMaxPlayerLimitCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index nMaxPlayerLimitCount on a nil value");
			}
		}

		obj.nMaxPlayerLimitCount = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_nMinPlayerLimitCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name nMinPlayerLimitCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index nMinPlayerLimitCount on a nil value");
			}
		}

		obj.nMinPlayerLimitCount = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_CurrentNineScreenPlayers(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CurrentNineScreenPlayers");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CurrentNineScreenPlayers on a nil value");
			}
		}

		obj.CurrentNineScreenPlayers = (BetterDictionary<ulong,OtherPlayer>)LuaScriptMgr.GetNetObject(L, 3, typeof(BetterDictionary<ulong,OtherPlayer>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bdicCurrentVisiblePlayer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bdicCurrentVisiblePlayer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bdicCurrentVisiblePlayer on a nil value");
			}
		}

		obj.bdicCurrentVisiblePlayer = (BetterDictionary<ulong,OtherPlayer>)LuaScriptMgr.GetNetObject(L, 3, typeof(BetterDictionary<ulong,OtherPlayer>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bdicCurrentHidePlayer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bdicCurrentHidePlayer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bdicCurrentHidePlayer on a nil value");
			}
		}

		obj.bdicCurrentHidePlayer = (BetterDictionary<ulong,OtherPlayer>)LuaScriptMgr.GetNetObject(L, 3, typeof(BetterDictionary<ulong,OtherPlayer>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_NpcList(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name NpcList");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index NpcList on a nil value");
			}
		}

		obj.NpcList = (BetterDictionary<ulong,Npc>)LuaScriptMgr.GetNetObject(L, 3, typeof(BetterDictionary<ulong,Npc>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_FuncNpcMap(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FuncNpcMap");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FuncNpcMap on a nil value");
			}
		}

		obj.FuncNpcMap = (BetterDictionary<ulong,Npc>)LuaScriptMgr.GetNetObject(L, 3, typeof(BetterDictionary<ulong,Npc>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bdicNpcKindOne(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name bdicNpcKindOne");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index bdicNpcKindOne on a nil value");
			}
		}

		obj.bdicNpcKindOne = (BetterDictionary<ulong,Npc>)LuaScriptMgr.GetNetObject(L, 3, typeof(BetterDictionary<ulong,Npc>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onMainPlayer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name onMainPlayer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index onMainPlayer on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.onMainPlayer = (Action)LuaScriptMgr.GetNetObject(L, 3, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.onMainPlayer = () =>
			{
				func.Call();
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_MainPlayer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name MainPlayer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index MainPlayer on a nil value");
			}
		}

		obj.MainPlayer = (MainPlayer)LuaScriptMgr.GetNetObject(L, 3, typeof(MainPlayer));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_SelectTarget(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		EntitiesManager obj = (EntitiesManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name SelectTarget");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index SelectTarget on a nil value");
			}
		}

		obj.SelectTarget = (CharactorBase)LuaScriptMgr.GetNetObject(L, 3, typeof(CharactorBase));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DoSendOrWaitSendWeGameRevenue(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		obj.DoSendOrWaitSendWeGameRevenue(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		obj.Init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setPlayerShowState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		PlayerShowState arg0 = (PlayerShowState)LuaScriptMgr.GetNetObject(L, 2, typeof(PlayerShowState));
		obj.setPlayerShowState(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SectorSeachBaseMainPlayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		EntitiesManager.SeachResult[] o = obj.SectorSeachBaseMainPlayer(arg0,arg1);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RectangleSeachBaseMainPlayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		EntitiesManager.SeachResult[] o = obj.RectangleSeachBaseMainPlayer(arg0,arg1);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SearchNearNPCById(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
			uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
			Npc o = obj.SearchNearNPCById(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 3)
		{
			EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
			uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
			float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
			Npc o = obj.SearchNearNPCById(arg0,arg1);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: EntitiesManager.SearchNearNPCById");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckRelationBaseMainPlayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		CharactorBase arg0 = (CharactorBase)LuaScriptMgr.GetNetObject(L, 2, typeof(CharactorBase));
		RelationType o = obj.CheckRelationBaseMainPlayer(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNpcRelationType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		Npc arg0 = (Npc)LuaScriptMgr.GetNetObject(L, 2, typeof(Npc));
		RelationType o = obj.GetNpcRelationType(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetOtherPlayerRelationType(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(EntitiesManager), typeof(cs_CharacterMapData)))
		{
			EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
			cs_CharacterMapData arg0 = (cs_CharacterMapData)LuaScriptMgr.GetLuaObject(L, 2);
			RelationType o = obj.GetOtherPlayerRelationType(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(EntitiesManager), typeof(OtherPlayer)))
		{
			EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
			OtherPlayer arg0 = (OtherPlayer)LuaScriptMgr.GetLuaObject(L, 2);
			RelationType o = obj.GetOtherPlayerRelationType(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: EntitiesManager.GetOtherPlayerRelationType");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetDistance(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		CharactorBase arg0 = (CharactorBase)LuaScriptMgr.GetNetObject(L, 2, typeof(CharactorBase));
		CharactorBase arg1 = (CharactorBase)LuaScriptMgr.GetNetObject(L, 3, typeof(CharactorBase));
		float o = obj.GetDistance(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnMainCameraChange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		obj.OnMainCameraChange();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SummonNpc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 3);
		uint arg2 = (uint)LuaScriptMgr.GetNumber(L, 4);
		obj.SummonNpc(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddCharacter(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		OtherPlayer arg0 = (OtherPlayer)LuaScriptMgr.GetNetObject(L, 2, typeof(OtherPlayer));
		obj.AddCharacter(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveCharacter(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		OtherPlayer arg0 = (OtherPlayer)LuaScriptMgr.GetNetObject(L, 2, typeof(OtherPlayer));
		obj.RemoveCharacter(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetVisiblePlayerLimit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		obj.SetVisiblePlayerLimit(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowPlayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		OtherPlayer arg0 = (OtherPlayer)LuaScriptMgr.GetNetObject(L, 2, typeof(OtherPlayer));
		obj.ShowPlayer(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HidePlayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		OtherPlayer arg0 = (OtherPlayer)LuaScriptMgr.GetNetObject(L, 2, typeof(OtherPlayer));
		obj.HidePlayer(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsMasterOrApprentice(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 1);
		bool o = EntitiesManager.IsMasterOrApprentice(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCurrentMapEnablePlayerLimit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		obj.SetCurrentMapEnablePlayerLimit(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPlayerCountLimit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		obj.SetPlayerCountLimit(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPlayerNameActive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.SetPlayerNameActive(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetNPCActive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.SetNPCActive(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPlayerByID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
		OtherPlayer o = obj.GetPlayerByID(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCharactorByID(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(EntitiesManager), typeof(EntitiesID)))
		{
			EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
			EntitiesID arg0 = (EntitiesID)LuaScriptMgr.GetLuaObject(L, 2);
			CharactorBase o = obj.GetCharactorByID(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(EntitiesManager), typeof(msg.EntryIDType)))
		{
			EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
			msg.EntryIDType arg0 = (msg.EntryIDType)LuaScriptMgr.GetLuaObject(L, 2);
			CharactorBase o = obj.GetCharactorByID(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 3)
		{
			EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
			ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
			CharactorType arg1 = (CharactorType)LuaScriptMgr.GetNetObject(L, 3, typeof(CharactorType));
			CharactorBase o = obj.GetCharactorByID(arg0,arg1);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: EntitiesManager.GetCharactorByID");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNpcsByBaseidInFun(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		Npc[] o = obj.GetNpcsByBaseidInFun(arg0);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddNpc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		Npc arg0 = (Npc)LuaScriptMgr.GetNetObject(L, 2, typeof(Npc));
		obj.AddNpc(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddFunNpc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		Npc arg0 = (Npc)LuaScriptMgr.GetNetObject(L, 2, typeof(Npc));
		obj.AddFunNpc(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearFunNpc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		obj.ClearFunNpc();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsFunNpc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		Npc arg0 = (Npc)LuaScriptMgr.GetNetObject(L, 2, typeof(Npc));
		bool o = obj.IsFunNpc(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveNpc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		Npc arg0 = (Npc)LuaScriptMgr.GetNetObject(L, 2, typeof(Npc));
		obj.RemoveNpc(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogOutALLFuncNpcMap(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		string o = obj.LogOutALLFuncNpcMap();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshFuncNpcList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		List<Npc> arg0 = (List<Npc>)LuaScriptMgr.GetNetObject(L, 2, typeof(List<Npc>));
		obj.RefreshFuncNpcList(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNpc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
		Npc o = obj.GetNpc(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCharactorFromGameObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		CharactorBase o = obj.GetCharactorFromGameObject(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCharactorFromCharid(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
		CharactorBase o = obj.GetCharactorFromCharid(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnLoadCharactors(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		obj.UnLoadCharactors();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsMainPlayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
		bool o = obj.IsMainPlayer(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnInitialize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		obj.UnInitialize();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnUpdate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		obj.OnUpdate();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnReSet(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		obj.OnReSet();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StartCacheEentityAction(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		EntitiesID arg0 = (EntitiesID)LuaScriptMgr.GetNetObject(L, 2, typeof(EntitiesID));
		obj.StartCacheEentityAction(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RunAllEentityActionCacheAndClear(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		EntitiesID arg0 = (EntitiesID)LuaScriptMgr.GetNetObject(L, 2, typeof(EntitiesID));
		obj.RunAllEentityActionCacheAndClear(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsAbattoirSceneRelive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		Action<bool> arg2 = null;
		LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

		if (funcType4 != LuaTypes.LUA_TFUNCTION)
		{
			 arg2 = (Action<bool>)LuaScriptMgr.GetNetObject(L, 4, typeof(Action<bool>));
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

		bool o = obj.IsAbattoirSceneRelive(arg0,arg1,arg2);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DoEentityActionOrCacheForLua(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		Action<CharactorBase> arg2 = null;
		LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

		if (funcType4 != LuaTypes.LUA_TFUNCTION)
		{
			 arg2 = (Action<CharactorBase>)LuaScriptMgr.GetNetObject(L, 4, typeof(Action<CharactorBase>));
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

		obj.DoEentityActionOrCacheForLua(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DoEentityActionOrCache(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		EntitiesID arg0 = (EntitiesID)LuaScriptMgr.GetNetObject(L, 2, typeof(EntitiesID));
		Action<CharactorBase> arg1 = null;
		LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

		if (funcType3 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (Action<CharactorBase>)LuaScriptMgr.GetNetObject(L, 3, typeof(Action<CharactorBase>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
			arg1 = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.PushObject(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}

		obj.DoEentityActionOrCache(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearEentityActionCache(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		EntitiesID arg0 = (EntitiesID)LuaScriptMgr.GetNetObject(L, 2, typeof(EntitiesID));
		obj.ClearEentityActionCache(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearAllEentityActionCache(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		obj.ClearAllEentityActionCache();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshNPCShowState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		EntitiesManager obj = (EntitiesManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "EntitiesManager");
		obj.RefreshNPCShowState();
		return 0;
	}
}

