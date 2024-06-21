using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class GameSystemSettingsWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("GetSystemDeviceType", GetSystemDeviceType),
			new LuaMethod("GetDeviceModel", GetDeviceModel),
			new LuaMethod("GetProcessorCount", GetProcessorCount),
			new LuaMethod("GetProcessorFrequency", GetProcessorFrequency),
			new LuaMethod("GetSystemMemorySize", GetSystemMemorySize),
			new LuaMethod("GetGraphicsMemorySize", GetGraphicsMemorySize),
			new LuaMethod("PlayerPrefsHasKey", PlayerPrefsHasKey),
			new LuaMethod("PlayerPrefsGetInt", PlayerPrefsGetInt),
			new LuaMethod("PlayerPrefsGetFloat", PlayerPrefsGetFloat),
			new LuaMethod("PlayerPrefsGetString", PlayerPrefsGetString),
			new LuaMethod("PlayerPrefsSetInt", PlayerPrefsSetInt),
			new LuaMethod("PlayerPrefsSetFloat", PlayerPrefsSetFloat),
			new LuaMethod("PlayerPrefsSetString", PlayerPrefsSetString),
			new LuaMethod("PlayerPrefsDeleteAll", PlayerPrefsDeleteAll),
			new LuaMethod("GetCameraDistanceType", GetCameraDistanceType),
			new LuaMethod("SetCameraDistanceType", SetCameraDistanceType),
			new LuaMethod("GetCameraTrack", GetCameraTrack),
			new LuaMethod("SetCameraTrack", SetCameraTrack),
			new LuaMethod("GetCameraSpeedType", GetCameraSpeedType),
			new LuaMethod("SetCameraSpeedType", SetCameraSpeedType),
			new LuaMethod("GetCurrentCameraState", GetCurrentCameraState),
			new LuaMethod("SetCameraState", SetCameraState),
			new LuaMethod("GetIsLoadLowPriorityObject", GetIsLoadLowPriorityObject),
			new LuaMethod("SetLoadLowPriorityObject", SetLoadLowPriorityObject),
			new LuaMethod("GetActiveSceneCamera", GetActiveSceneCamera),
			new LuaMethod("SetActiveSceneCamera", SetActiveSceneCamera),
			new LuaMethod("IsHideNpc", IsHideNpc),
			new LuaMethod("SetHideNpc", SetHideNpc),
			new LuaMethod("IsHidePlayerName", IsHidePlayerName),
			new LuaMethod("SetHidePlayerName", SetHidePlayerName),
			new LuaMethod("IsAutoAddTeam", IsAutoAddTeam),
			new LuaMethod("SetAutoAddTeam", SetAutoAddTeam),
			new LuaMethod("IsAllowTeamInvite", IsAllowTeamInvite),
			new LuaMethod("SetAllowTeamInvite", SetAllowTeamInvite),
			new LuaMethod("IsAllowFamilyInvite", IsAllowFamilyInvite),
			new LuaMethod("SetAllowGuildInvite", SetAllowGuildInvite),
			new LuaMethod("IsAllowFriendInvite", IsAllowFriendInvite),
			new LuaMethod("SetAllowFriendInvite", SetAllowFriendInvite),
			new LuaMethod("IsAutoAddFriend", IsAutoAddFriend),
			new LuaMethod("SetAutoAddFriend", SetAutoAddFriend),
			new LuaMethod("SetMouseClickMove", SetMouseClickMove),
			new LuaMethod("GetMouseClickMove", GetMouseClickMove),
			new LuaMethod("IsLowHealthWarning", IsLowHealthWarning),
			new LuaMethod("SetLowHealthWarning", SetLowHealthWarning),
			new LuaMethod("SetApplicationFrameRate", SetApplicationFrameRate),
			new LuaMethod("SetScreebBrightnessValue", SetScreebBrightnessValue),
			new LuaMethod("GetOriScreenBrightnessValue", GetOriScreenBrightnessValue),
			new LuaMethod("SetMainPlayerInBattleState", SetMainPlayerInBattleState),
			new LuaMethod("IsMainPlayerInBattleState", IsMainPlayerInBattleState),
			new LuaMethod("MyPlayerPrefsSetInt", MyPlayerPrefsSetInt),
			new LuaMethod("MyPlayerPrefsGetInt", MyPlayerPrefsGetInt),
			new LuaMethod("OnNameSetingChange", OnNameSetingChange),
			new LuaMethod("New", _CreateGameSystemSettings),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("bHideNpc", get_bHideNpc, set_bHideNpc),
			new LuaField("nameSwitchData", get_nameSwitchData, null),
		};

		LuaScriptMgr.RegisterLib(L, "GameSystemSettings", typeof(GameSystemSettings), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateGameSystemSettings(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			GameSystemSettings obj = new GameSystemSettings();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: GameSystemSettings.New");
		}

		return 0;
	}

	static Type classType = typeof(GameSystemSettings);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bHideNpc(IntPtr L)
	{
		LuaScriptMgr.Push(L, GameSystemSettings.bHideNpc);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_nameSwitchData(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, GameSystemSettings.nameSwitchData);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_bHideNpc(IntPtr L)
	{
		GameSystemSettings.bHideNpc = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSystemDeviceType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = GameSystemSettings.GetSystemDeviceType();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetDeviceModel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = GameSystemSettings.GetDeviceModel();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetProcessorCount(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = GameSystemSettings.GetProcessorCount();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetProcessorFrequency(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = GameSystemSettings.GetProcessorFrequency();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSystemMemorySize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = GameSystemSettings.GetSystemMemorySize();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetGraphicsMemorySize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = GameSystemSettings.GetGraphicsMemorySize();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayerPrefsHasKey(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = GameSystemSettings.PlayerPrefsHasKey(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayerPrefsGetInt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		int o = GameSystemSettings.PlayerPrefsGetInt(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayerPrefsGetFloat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		float o = GameSystemSettings.PlayerPrefsGetFloat(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayerPrefsGetString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = GameSystemSettings.PlayerPrefsGetString(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayerPrefsSetInt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		GameSystemSettings.PlayerPrefsSetInt(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayerPrefsSetFloat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		GameSystemSettings.PlayerPrefsSetFloat(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayerPrefsSetString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		GameSystemSettings.PlayerPrefsSetString(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayerPrefsDeleteAll(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GameSystemSettings.PlayerPrefsDeleteAll();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCameraDistanceType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		CameraDistanceType o = GameSystemSettings.GetCameraDistanceType();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCameraDistanceType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CameraDistanceType arg0 = (CameraDistanceType)LuaScriptMgr.GetNetObject(L, 1, typeof(CameraDistanceType));
		GameSystemSettings.SetCameraDistanceType(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCameraTrack(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GameSystemSettings.GetCameraTrack();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCameraTrack(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GameSystemSettings.SetCameraTrack(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCameraSpeedType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		CameraSpeedType o = GameSystemSettings.GetCameraSpeedType();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCameraSpeedType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CameraSpeedType arg0 = (CameraSpeedType)LuaScriptMgr.GetNetObject(L, 1, typeof(CameraSpeedType));
		GameSystemSettings.SetCameraSpeedType(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurrentCameraState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		CameraState o = GameSystemSettings.GetCurrentCameraState();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCameraState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CameraState arg0 = (CameraState)LuaScriptMgr.GetNetObject(L, 1, typeof(CameraState));
		GameSystemSettings.SetCameraState(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetIsLoadLowPriorityObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GameSystemSettings.GetIsLoadLowPriorityObject();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLoadLowPriorityObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GameSystemSettings.SetLoadLowPriorityObject(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetActiveSceneCamera(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GameSystemSettings.GetActiveSceneCamera();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetActiveSceneCamera(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GameSystemSettings.SetActiveSceneCamera(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsHideNpc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GameSystemSettings.IsHideNpc();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetHideNpc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GameSystemSettings.SetHideNpc(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsHidePlayerName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GameSystemSettings.IsHidePlayerName();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetHidePlayerName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GameSystemSettings.SetHidePlayerName(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsAutoAddTeam(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GameSystemSettings.IsAutoAddTeam();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAutoAddTeam(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GameSystemSettings.SetAutoAddTeam(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsAllowTeamInvite(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GameSystemSettings.IsAllowTeamInvite();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAllowTeamInvite(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GameSystemSettings.SetAllowTeamInvite(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsAllowFamilyInvite(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GameSystemSettings.IsAllowFamilyInvite();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAllowGuildInvite(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GameSystemSettings.SetAllowGuildInvite(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsAllowFriendInvite(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GameSystemSettings.IsAllowFriendInvite();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAllowFriendInvite(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GameSystemSettings.SetAllowFriendInvite(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsAutoAddFriend(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GameSystemSettings.IsAutoAddFriend();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAutoAddFriend(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GameSystemSettings.SetAutoAddFriend(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetMouseClickMove(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GameSystemSettings.SetMouseClickMove(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMouseClickMove(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GameSystemSettings.GetMouseClickMove();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsLowHealthWarning(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GameSystemSettings.IsLowHealthWarning();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLowHealthWarning(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GameSystemSettings.SetLowHealthWarning(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetApplicationFrameRate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		GameSystemSettings.SetApplicationFrameRate(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetScreebBrightnessValue(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 1);
		GameSystemSettings.SetScreebBrightnessValue(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetOriScreenBrightnessValue(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		float o = GameSystemSettings.GetOriScreenBrightnessValue();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetMainPlayerInBattleState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GameSystemSettings.SetMainPlayerInBattleState(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsMainPlayerInBattleState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GameSystemSettings.IsMainPlayerInBattleState();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MyPlayerPrefsSetInt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		GameSystemSettings.MyPlayerPrefsSetInt(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MyPlayerPrefsGetInt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		int o = GameSystemSettings.MyPlayerPrefsGetInt(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnNameSetingChange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GameSystemSettings.OnNameSetingChange();
		return 0;
	}
}

