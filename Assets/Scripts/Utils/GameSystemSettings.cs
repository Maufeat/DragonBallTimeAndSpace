using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Framework.Managers;
using UnityEngine;

public class GameSystemSettings
{
    public static int GetSystemDeviceType()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            return 1;
        }
        if (SystemInfo.deviceType == DeviceType.Console)
        {
            return 2;
        }
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            return 3;
        }
        return 0;
    }

    public static string GetDeviceModel()
    {
        return SystemInfo.deviceModel;
    }

    public static int GetProcessorCount()
    {
        return SystemInfo.processorCount;
    }

    public static int GetProcessorFrequency()
    {
        return SystemInfo.processorFrequency;
    }

    public static int GetSystemMemorySize()
    {
        return SystemInfo.systemMemorySize;
    }

    public static int GetGraphicsMemorySize()
    {
        return SystemInfo.graphicsMemorySize;
    }

    public static bool PlayerPrefsHasKey(string strKey)
    {
        return PlayerPrefs.HasKey(strKey);
    }

    public static int PlayerPrefsGetInt(string strKey)
    {
        return PlayerPrefs.GetInt(strKey);
    }

    public static float PlayerPrefsGetFloat(string strKey)
    {
        return PlayerPrefs.GetFloat(strKey);
    }

    public static string PlayerPrefsGetString(string strKey)
    {
        return PlayerPrefs.GetString(strKey);
    }

    public static void PlayerPrefsSetInt(string strKey, int nValue)
    {
        PlayerPrefs.SetInt(strKey, nValue);
    }

    public static void PlayerPrefsSetFloat(string strKey, float fValue)
    {
        PlayerPrefs.SetFloat(strKey, fValue);
    }

    public static void PlayerPrefsSetString(string strKey, string strValue)
    {
        PlayerPrefs.SetString(strKey, strValue);
    }

    public static void PlayerPrefsDeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static CameraDistanceType GetCameraDistanceType()
    {
        return GameSystemSettings.disType;
    }

    public static void SetCameraDistanceType(CameraDistanceType dis)
    {
        GameSystemSettings.disType = dis;
    }

    public static bool GetCameraTrack()
    {
        return GameSystemSettings.bCameraTrack;
    }

    public static void SetCameraTrack(bool bTrack)
    {
        GameSystemSettings.bCameraTrack = bTrack;
    }

    public static CameraSpeedType GetCameraSpeedType()
    {
        return GameSystemSettings.speedType;
    }

    public static void SetCameraSpeedType(CameraSpeedType speed)
    {
        GameSystemSettings.speedType = speed;
    }

    public static CameraState GetCurrentCameraState()
    {
        return GameSystemSettings.curState;
    }

    public static void SetCameraState(CameraState state)
    {
        GameSystemSettings.curState = state;
    }

    public static bool GetIsLoadLowPriorityObject()
    {
        return GameSystemSettings.bLoadLowPriorityObject;
    }

    public static void SetLoadLowPriorityObject(bool bLoad)
    {
        GameSystemSettings.bLoadLowPriorityObject = bLoad;
    }

    public static bool GetActiveSceneCamera()
    {
        return GameSystemSettings.bActiveSceneCamera;
    }

    public static void SetActiveSceneCamera(bool bActive)
    {
        GameSystemSettings.bActiveSceneCamera = bActive;
    }

    public static bool IsHideNpc()
    {
        return GameSystemSettings.bHideNpc;
    }

    public static void SetHideNpc(bool bHide)
    {
        GameSystemSettings.bHideNpc = bHide;
    }

    public static bool IsHidePlayerName()
    {
        return GameSystemSettings.bHidePlayerName;
    }

    public static void SetHidePlayerName(bool bHide)
    {
        GameSystemSettings.bHidePlayerName = bHide;
    }

    public static bool IsAutoAddTeam()
    {
        return GameSystemSettings.bAutoAddTeam;
    }

    public static void SetAutoAddTeam(bool bActive)
    {
        GameSystemSettings.bAutoAddTeam = false;
    }

    public static bool IsAllowTeamInvite()
    {
        return GameSystemSettings.bAllowTeamInvite;
    }

    public static void SetAllowTeamInvite(bool bActive)
    {
        GameSystemSettings.bAllowTeamInvite = bActive;
    }

    public static bool IsAllowFamilyInvite()
    {
        return GameSystemSettings.bAllowGuildInvite;
    }

    public static void SetAllowGuildInvite(bool bActive)
    {
        GameSystemSettings.bAllowGuildInvite = bActive;
    }

    public static bool IsAllowFriendInvite()
    {
        return GameSystemSettings.bAllowFriendInvite;
    }

    public static void SetAllowFriendInvite(bool bActive)
    {
        GameSystemSettings.bAllowFriendInvite = bActive;
    }

    public static bool IsAutoAddFriend()
    {
        return GameSystemSettings.bAutoAddFriend;
    }

    public static void SetAutoAddFriend(bool bActive)
    {
        GameSystemSettings.bAutoAddFriend = bActive;
    }

    public static void SetMouseClickMove(bool bActive)
    {
        GameSystemSettings.bMouseClickMove = bActive;
    }

    public static bool GetMouseClickMove()
    {
        return GameSystemSettings.bMouseClickMove;
    }

    public static bool IsLowHealthWarning()
    {
        return GameSystemSettings.bLowHealthWarning;
    }

    public static void SetLowHealthWarning(bool bSet)
    {
        GameSystemSettings.bLowHealthWarning = bSet;
    }

    public static void SetApplicationFrameRate(int nFrame)
    {
        Application.targetFrameRate = nFrame;
    }

    [DllImport("__Internal")]
    private static extern void SetScreenBrightness(float fValue);

    [DllImport("__Internal")]
    private static extern float GetOriScreenBrightness();

    public static void SetScreebBrightnessValue(float fValue)
    {
    }

    public static float GetOriScreenBrightnessValue()
    {
        return 1f;
    }

    public static void SetMainPlayerInBattleState(bool bInBattle)
    {
        GameSystemSettings.bInBattleScene = bInBattle;
    }

    public static bool IsMainPlayerInBattleState()
    {
        return GameSystemSettings.bInBattleScene;
    }

    public static void MyPlayerPrefsSetInt(string sk, int iValue)
    {
        MyPlayerPrefs.SetInt(sk, iValue);
    }

    public static int MyPlayerPrefsGetInt(string sk)
    {
        return MyPlayerPrefs.GetInt(sk);
    }

    public static BetterDictionary<string, bool> nameSwitchData
    {
        get
        {
            if (GameSystemSettings._nameSwitchData.Count == 0)
            {
                GameSystemSettings.ReGetNameSwitchData();
            }
            return GameSystemSettings._nameSwitchData;
        }
    }

    private static void ReGetNameSwitchData()
    {
        for (int i = 0; i < GameSystemSettings.nameSwitchKeys.Length; i++)
        {
            GameSystemSettings._nameSwitchData[GameSystemSettings.nameSwitchKeys[i]] = (GameSystemSettings.MyPlayerPrefsGetInt(GameSystemSettings.nameSwitchKeys[i]) == 0);
        }
    }

    public static void OnNameSetingChange()
    {
        GameSystemSettings.ReGetNameSwitchData();
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        if (manager != null)
        {
            manager.CurrentNineScreenPlayers.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> opItem)
            {
                if (opItem.Value.hpdata != null)
                {
                    opItem.Value.hpdata.SetPlayerNameShowBySetting(null);
                }
            });
            manager.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> opItem)
            {
                if (opItem.Value.hpdata != null)
                {
                    opItem.Value.hpdata.SetPlayerNameShowBySetting(null);
                }
            });
        }
    }

    private static CameraDistanceType disType = CameraDistanceType.normal;

    private static bool bCameraTrack = true;

    private static CameraSpeedType speedType = CameraSpeedType.normal;

    private static CameraState curState;

    private static bool bLoadLowPriorityObject = true;

    private static bool bActiveSceneCamera = true;

    public static bool bHideNpc;

    private static bool bHidePlayerName;

    private static bool bAutoAddTeam;

    private static bool bAllowTeamInvite;

    private static bool bAllowGuildInvite;

    private static bool bAllowFriendInvite;

    private static bool bAutoAddFriend;

    private static bool bMouseClickMove = true;

    private static bool bLowHealthWarning;

    private static float fOriBrightness;

    private static bool bInBattleScene;

    private static string[] nameSwitchKeys = new string[]
    {
        "DB.SelfName",
        "DB.SelfHpBar",
        "DB.SelfGuild",
        "DB.SelfTitle",
        "DB.OthersName",
        "DB.OthersHpBar",
        "DB.OthersGuild",
        "DB.OthersTitle",
        "DB.EnemyName",
        "DB.EnemyHpBar",
        "DB.EnemyGuild",
        "DB.EnemyTitle",
        "DB.NPCName"
    };

    private static BetterDictionary<string, bool> _nameSwitchData = new BetterDictionary<string, bool>();
}
