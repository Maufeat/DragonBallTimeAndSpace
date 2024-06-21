using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using apprentice;
using AudioStudio;
using Engine;
using Framework.Managers;
using LuaInterface;
using Models;
using UnityEngine;

public class SystemSettingController : ControllerBase
{
    [DllImport("User32.dll")]
    public static extern IntPtr GetSystemMetrics(int nIndex);

    public override string ControllerName
    {
        get
        {
            return "systemsetting_controller";
        }
    }

    private EntitiesManager entitiesManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<EntitiesManager>();
        }
    }

    public UI_GameSetting gameSetting
    {
        get
        {
            return UIManager.GetUIObject<UI_GameSetting>();
        }
    }

    public override void Awake()
    {
        this.mNetWork = new SystemSettingNetWork();
        this.InitConfig();
        this.LoadDefaultData();
        GameSystemSettings.SetCameraTrack(false);
        this.GetLocalData();
    }

    private void InitConfig()
    {
        this.WindowsWidth = (int)SystemSettingController.GetSystemMetrics(0);
        this.WindowsHeight = (int)SystemSettingController.GetSystemMetrics(1);
        this.systemConfig = LuaConfigManager.GetXmlConfigTable("systemSettingConfig");
        for (int i = 0; i <= this.MAX_SCREEN_SETTING_NUM; i++)
        {
            LuaTable cacheField_Table = this.systemConfig.GetCacheField_Table("screensetting").GetCacheField_Table(i.ToString());
            if (cacheField_Table == null)
            {
                break;
            }
            this.screenSettingConfig.Add(cacheField_Table);
        }
        for (int j = 1; j <= this.MAX_SCREEN_SETTING_NUM; j++)
        {
            LuaTable cacheField_Table2 = this.systemConfig.GetCacheField_Table("windowratio").GetCacheField_Table(j.ToString());
            if (cacheField_Table2 == null)
            {
                break;
            }
            this.windowratioConfig.Add(cacheField_Table2);
        }
        for (int k = 0; k < this.SETTING_VSYNC; k++)
        {
            string field_String = this.systemConfig.GetCacheField_Table("vsync").GetCacheField_Table(k.ToString()).GetField_String("name");
            this.vsyncConfig.Add(field_String);
        }
        for (int l = 0; l < this.SETTING_MSAA; l++)
        {
            string field_String2 = this.systemConfig.GetCacheField_Table("antialiasing").GetCacheField_Table(l.ToString()).GetField_String("name");
            this.antialiasingConfig.Add(field_String2);
        }
        for (int m = 0; m < this.SETTING_SHADOW; m++)
        {
            string field_String3 = this.systemConfig.GetCacheField_Table("shadows").GetCacheField_Table(m.ToString()).GetField_String("name");
            this.shadowsConfig.Add(field_String3);
        }
        for (int n = 0; n < this.SETTING_SHADOW; n++)
        {
            string field_String4 = this.systemConfig.GetCacheField_Table("shadowdistance").GetCacheField_Table(n.ToString()).GetField_String("name");
            this.shadowdistanceConfig.Add(field_String4);
        }
        for (int num = 0; num < Screen.resolutions.Length; num++)
        {
            this.resolutionList.Add(Screen.resolutions[num].width.ToString() + " X " + Screen.resolutions[num].height.ToString());
        }
    }

    public void ShowUI()
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_GameSetting>("UI_GameSetting", delegate ()
        {
            if (this.gameSetting != null)
            {
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public void CloseUI()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_GameSetting");
    }

    private void SetLoadSceneCompletedSystemSettings()
    {
        LuaScriptMgr.Instance.CallLuaFunction("MentoringCtrl.ReqMasterApprenticeInfo", new object[]
        {
            Util.GetLuaTable("MentoringCtrl")
        });
        LuaScriptMgr.Instance.CallLuaFunction("MentoringCtrl.ReqApprenticeList_CS", new object[]
        {
            Util.GetLuaTable("MentoringCtrl")
        });
    }

    private void SetBackgroundMusic(bool bOpen)
    {
    }

    private void SetEffectMusic(bool bOpen)
    {
    }

    private void SetMaskNPC(bool bOpen)
    {
        GameSystemSettings.SetHideNpc(bOpen);
        this.entitiesManager.SetNPCActive(!bOpen);
    }

    private void SetMaskName(bool bOpen)
    {
        GameSystemSettings.SetHidePlayerName(bOpen);
        this.entitiesManager.SetPlayerNameActive(!bOpen);
    }

    private void SetAllowTeamInvite(bool bOpen)
    {
        GameSystemSettings.SetAllowTeamInvite(bOpen);
    }

    private void SetAllowFriendInvite(bool bOpen)
    {
        GameSystemSettings.SetAllowFriendInvite(bOpen);
    }

    private void SetLowHealthWarning(bool bOpen)
    {
        GameSystemSettings.SetLowHealthWarning(bOpen);
        ControllerManager.Instance.GetController<MainUIController>().ResetMainPlayerHp(MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.hp, MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.maxhp);
    }

    private void SetAllowGuildInvite(bool bOpen)
    {
        GameSystemSettings.SetAllowGuildInvite(bOpen);
    }

    private void SetMouseClickMove(bool bOpen)
    {
        this.bMouseClickMove = bOpen;
        GameSystemSettings.SetMouseClickMove(bOpen);
    }

    public void OnLoginOut()
    {
        ControllerManager.Instance.GetController<GuideController>().Reset();
        ControllerManager.Instance.GetController<PVPMatchController>().Reset();
        GlobalRegister.LogoutGame();
    }

    public void OnQuitGame()
    {
        ManagerCenter.Instance.GetManager<GameMainManager>().QuitGame();
    }

    public void SetScreenResolution(int height, int width, bool isFull)
    {
        GlobalRegister.SetScreenResolution(height, width, isFull);
    }

    public void SaveStorageData(SystemSettingData_Funtion dataFuntion, SystemSettingData_Show dataShow)
    {
        MassiveClientSetInfo t = new MassiveClientSetInfo();
        dataFuntion.AddKeyValue(ref t);
        dataShow.AddKeyValue(ref t);
        string content = ServerStorageManager.Instance.SerializeClassLocal<MassiveClientSetInfo>(t);
        ServerStorageManager.Instance.AddUpdateData(ServerStorageKey.SystemData, content, 1U);
    }

    public void SaveLocalData(ResolutionInfo dataScreen, SystemSettingData_Sound dataSound, SystemSettingData_Quality dataQuality)
    {
        string value = ServerStorageManager.Instance.SerializeClassLocal<SystemSettingData_Sound>(dataSound);
        MyPlayerPrefs.SetString("sound_data", value);
        string value2 = ServerStorageManager.Instance.SerializeClassLocal<SystemSettingData_Quality>(dataQuality);
        MyPlayerPrefs.SetString("quality_data", value2);
        if (ServerStorageManager.Instance.SerializeClass<ResolutionInfo>(this.screenData) != ServerStorageManager.Instance.SerializeClass<ResolutionInfo>(dataScreen) || this.screenData.UIScale != dataScreen.UIScale)
        {
            ResolutionManager.Instance.SaveResolutionInfo(dataScreen);
        }
        this.GetLocalData();
    }

    public void RegAutoFightDataCallBack(Action<string> callBack)
    {
        ServerStorageManager.Instance.RegReqCallBack(ServerStorageKey.AutoFight, new Action<MSG_Req_OperateClientDatas_CS>(this.OnAutoFight));
        this.onAutoFight = callBack;
    }

    private void OnAutoFight(MSG_Req_OperateClientDatas_CS msg)
    {
        if (this.onAutoFight != null && msg.key.Contains(ServerStorageKey.AutoFight.ToString()))
        {
            this.onAutoFight(msg.value);
        }
    }

    public void LoadSystemSettingData()
    {
        this.GetStorageData(ServerStorageKey.SystemData);
    }

    public void GetStorageData(ServerStorageKey type)
    {
        ServerStorageManager.Instance.GetData(type, 1U);
    }

    public void GetLocalData()
    {
        ResolutionInfo resolutionInfo = ResolutionManager.Instance.GetResolutionInfo();
        if (resolutionInfo.ReferenceResolution > this.resolutionList.Count)
        {
            resolutionInfo.ReferenceResolution = this.resolutionList.Count - 1;
            resolutionInfo.Width = this.resolutionList[resolutionInfo.ReferenceResolution].Split(new char[]
            {
                'X'
            })[0];
            resolutionInfo.Height = this.resolutionList[resolutionInfo.ReferenceResolution].Split(new char[]
            {
                'X'
            })[1];
        }
        this.screenData = new ResolutionInfo();
        this.screenData.FullScreen = resolutionInfo.FullScreen;
        this.screenData.Height = resolutionInfo.Height;
        this.screenData.Width = resolutionInfo.Width;
        this.screenData.ReferenceResolution = (this.screenData.CurResolution = resolutionInfo.ReferenceResolution);
        this.screenData.ModeIndex = resolutionInfo.ModeIndex;
        this.screenData.CameraMaxdistance = resolutionInfo.CameraMaxdistance;
        this.screenData.UIScale = resolutionInfo.UIScale;
        this.screenData.mouseSpeed = resolutionInfo.mouseSpeed;
        this.screenData.pixelpercent = resolutionInfo.pixelpercent;
        this.ApplyScreenOtherData(this.screenData);
        bool flag = false;
        string @string = MyPlayerPrefs.GetString("sound_data");
        if (string.IsNullOrEmpty(@string))
        {
            this.CreateLocalAllData();
        }
        else
        {
            SystemSettingData_Sound systemSettingData_Sound = ServerStorageManager.Instance.DeserializeClassLocal<SystemSettingData_Sound>(@string);
            if (systemSettingData_Sound != null)
            {
                this.soundData = systemSettingData_Sound;
                this.ApplySoundData();
            }
            else
            {
                this.CreateSoundData();
                flag = true;
            }
        }
        string string2 = MyPlayerPrefs.GetString("quality_data");
        if (string.IsNullOrEmpty(string2))
        {
            this.CreateLocalAllData();
        }
        else
        {
            SystemSettingData_Quality systemSettingData_Quality = ServerStorageManager.Instance.DeserializeClassLocal<SystemSettingData_Quality>(string2);
            if (systemSettingData_Quality != null)
            {
                this.qualityData = systemSettingData_Quality;
                this.ApplyQualityData(null);
            }
            else
            {
                this.CreateQualityData();
                flag = true;
            }
        }
        if (flag)
        {
            this.SaveLocalData(this.screenData, this.soundData, this.qualityData);
        }
        if (this.gameSetting != null)
        {
            this.gameSetting.InitGameSetting_Sys(this.screenData, this.qualityData);
        }
    }

    public void StorageDataCallBack(string keys, string value)
    {
        SystemSettingData_Funtion systemSettingData_Funtion = new SystemSettingData_Funtion();
        SystemSettingData_Show systemSettingData_Show = new SystemSettingData_Show();
        MassiveClientSetInfo massiveClientSetInfo = ServerStorageManager.Instance.DeserializeClassLocal<MassiveClientSetInfo>(value);
        if (massiveClientSetInfo == null)
        {
            this.CreateStorageAllData();
            return;
        }
        for (int i = 0; i < massiveClientSetInfo.data.Count; i++)
        {
            systemSettingData_Funtion.CheckKeyValue(massiveClientSetInfo.data[i].key, massiveClientSetInfo.data[i].value);
            systemSettingData_Show.CheckKeyValue(massiveClientSetInfo.data[i].key, massiveClientSetInfo.data[i].value);
        }
        this.funtionData = systemSettingData_Funtion;
        this.showData = systemSettingData_Show;
        this.ApplyFuntionData();
        this.ApplyShowData();
    }

    private void CreateStorageAllData()
    {
        this.CreateFuntionData();
        this.ApplyFuntionData();
        this.CreateShowData();
        this.ApplyShowData();
        this.SaveStorageData(this.funtionData, this.showData);
    }

    private void CreateLocalAllData()
    {
        this.CreateSoundData();
        this.ApplySoundData();
        this.CreateQualityData();
        this.ApplyQualityData(null);
        this.SaveLocalData(this.screenData, this.soundData, this.qualityData);
    }

    private void CreateFuntionData()
    {
        this.funtionData = new SystemSettingData_Funtion();
        this.funtionData.AllowPartyInvite = this.defaultFuntionData.AllowPartyInvite;
        this.funtionData.AllowGuildInvite = this.defaultFuntionData.AllowGuildInvite;
        this.funtionData.AllowFriendInvite = this.defaultFuntionData.AllowFriendInvite;
        this.funtionData.LowHealthWarning = this.defaultFuntionData.LowHealthWarning;
        this.funtionData.IfMouse = this.defaultFuntionData.IfMouse;
        this.funtionData.Close2ndPW = this.defaultFuntionData.Close2ndPW;
    }

    private void ApplyFuntionData()
    {
        this.SetAllowTeamInvite(this.funtionData.AllowPartyInvite);
        this.SetAllowGuildInvite(this.funtionData.AllowGuildInvite);
        this.SetAllowFriendInvite(this.funtionData.AllowFriendInvite);
        this.SetLowHealthWarning(this.funtionData.LowHealthWarning);
        this.SetMouseClickMove(this.funtionData.IfMouse);
        ControllerManager.Instance.GetController<SecondPwdControl>().CloseSecondPwd = this.funtionData.Close2ndPW;
        if (this.gameSetting != null)
        {
            this.gameSetting.InitGameSetting_Game();
        }
    }

    public void ApplyScreenData(ResolutionInfo data, SystemSettingData_Quality quality)
    {
        this.SetScreenResolution(data.Height.ToInt(), data.Width.ToInt(), data.FullScreen == 1);
        this.ApplyScreenOtherData(data);
    }

    private void ApplyScreenOtherData(ResolutionInfo data)
    {
        if (null != CameraController.Self)
        {
            CameraController.Self.SetMaxDistance((float)data.CameraMaxdistance);
        }
        CommonTools.SetUIScale(data.UIScale);
        CommonTools.SetMouseSpeed(data.mouseSpeed);
        CommonTools.SetPixelPercent(data.pixelpercent);
    }

    private void CreateShowData()
    {
        this.showData = new SystemSettingData_Show();
        this.showData.PlayerBarShow = this.defaultShowData.PlayerBarShow;
        this.showData.PlayerNameShow = this.defaultShowData.PlayerNameShow;
        this.showData.PlayerGuildShow = this.defaultShowData.PlayerGuildShow;
        this.showData.OthersBarShow = this.defaultShowData.OthersBarShow;
        this.showData.OthersNameShow = this.defaultShowData.OthersNameShow;
        this.showData.OthersGuildShow = this.defaultShowData.OthersGuildShow;
        this.showData.EnemyBarShow = this.defaultShowData.EnemyBarShow;
        this.showData.EnemyNameShow = this.defaultShowData.EnemyNameShow;
        this.showData.EnemyGuildShow = this.defaultShowData.EnemyGuildShow;
        this.showData.NpcNameShow = this.defaultShowData.NpcNameShow;
    }

    private void ApplyShowData()
    {
        GameSystemSettings.MyPlayerPrefsSetInt("DB.SelfHpBar", (!this.showData.PlayerBarShow) ? 0 : 1);
        GameSystemSettings.MyPlayerPrefsSetInt("DB.SelfName", (!this.showData.PlayerNameShow) ? 0 : 1);
        GameSystemSettings.MyPlayerPrefsSetInt("DB.SelfGuild", (!this.showData.PlayerGuildShow) ? 0 : 1);
        GameSystemSettings.MyPlayerPrefsSetInt("DB.OthersHpBar", (!this.showData.OthersBarShow) ? 0 : 1);
        GameSystemSettings.MyPlayerPrefsSetInt("DB.OthersName", (!this.showData.OthersNameShow) ? 0 : 1);
        GameSystemSettings.MyPlayerPrefsSetInt("DB.OthersGuild", (!this.showData.OthersGuildShow) ? 0 : 1);
        GameSystemSettings.MyPlayerPrefsSetInt("DB.EnemyHpBar", (!this.showData.EnemyBarShow) ? 0 : 1);
        GameSystemSettings.MyPlayerPrefsSetInt("DB.EnemyName", (!this.showData.EnemyNameShow) ? 0 : 1);
        GameSystemSettings.MyPlayerPrefsSetInt("DB.EnemyGuild", (!this.showData.EnemyGuildShow) ? 0 : 1);
        GameSystemSettings.MyPlayerPrefsSetInt("DB.NPCName", (!this.showData.NpcNameShow) ? 0 : 1);
        GameSystemSettings.OnNameSetingChange();
        if (this.gameSetting != null)
        {
            this.gameSetting.InitGameSetting_Game();
        }
    }

    private void CreateSoundData()
    {
        this.soundData = new SystemSettingData_Sound();
        this.soundData.BgMusic = this.defaultSoundData.BgMusic;
        this.soundData.EffectMusic = this.defaultSoundData.EffectMusic;
        this.soundData.MainSound = this.defaultSoundData.MainSound;
        this.soundData.IsMain = this.defaultSoundData.IsMain;
        this.soundData.IsOther = this.defaultSoundData.IsOther;
        this.soundData.IsTeam = this.defaultSoundData.IsTeam;
        this.soundData.IsNPC = this.defaultSoundData.IsNPC;
    }

    private void CreateQualityData()
    {
        this.qualityData = new SystemSettingData_Quality();
        this.qualityData.IsAutoSetting = this.defaultQualityData.IsAutoSetting;
        this.qualityData.QualityLevel = this.defaultQualityData.QualityLevel;
        this.qualityData.Antialiasing = this.defaultQualityData.Antialiasing;
        this.qualityData.Vsync = this.defaultQualityData.Vsync;
        this.qualityData.Shadow = this.defaultQualityData.Shadow;
        this.qualityData.ShadowDistance = this.defaultQualityData.ShadowDistance;
    }

    private void ApplySoundData()
    {
        AudioCtrl.SetMusicVolume((float)this.soundData.BgMusic * (float)this.soundData.MainSound * 0.01f);
        AudioCtrl.SetSoundVolume((float)this.soundData.EffectMusic * (float)this.soundData.MainSound * 0.01f);
        AudioCtrl.SetVoiceVolume((float)this.soundData.Voice * (float)this.soundData.MainSound * 0.01f);
    }

    public void ApplyQualityData(SystemSettingData_Quality currData = null)
    {
        SystemSettingData_Quality systemSettingData_Quality;
        if (currData != null)
        {
            systemSettingData_Quality = currData;
        }
        else
        {
            systemSettingData_Quality = this.qualityData;
        }
        QualityMSAA qualityMSAA = QualityMSAA.Low;
        bool flag = false;
        QualityShadow qualityShadow = QualityShadow.Low;
        QualityShadow qualityShadow2 = QualityShadow.Low;
        switch (systemSettingData_Quality.QualityLevel)
        {
            case 1:
                QualitySettings.SetQualityLevel(1);
                break;
            case 2:
                QualitySettings.SetQualityLevel(3);
                qualityMSAA = QualityMSAA.Mid;
                flag = true;
                qualityShadow = (qualityShadow2 = QualityShadow.Mid);
                break;
            case 3:
                QualitySettings.SetQualityLevel(5);
                qualityMSAA = QualityMSAA.High;
                flag = true;
                qualityShadow = (qualityShadow2 = QualityShadow.High);
                break;
            default:
                QualitySettings.SetQualityLevel(0);
                break;
        }
        if (systemSettingData_Quality.IsAutoSetting)
        {
            qualityMSAA = (QualityMSAA)systemSettingData_Quality.Antialiasing;
            flag = systemSettingData_Quality.Vsync;
            qualityShadow2 = (QualityShadow)systemSettingData_Quality.Shadow;
            qualityShadow = (QualityShadow)systemSettingData_Quality.ShadowDistance;
        }
        int num = (int)qualityMSAA;
        if (num <= 2)
        {
            QualitySettings.antiAliasing = num * 2;
        }
        else
        {
            QualitySettings.antiAliasing = (num - 1) * 4;
        }
        QualitySettings.vSyncCount = ((!flag) ? 0 : 1);
        QualitySettings.shadowCascades = ((int)qualityShadow2 * (int)QualityShadow.High);
        QualitySettings.shadowDistance = (float)((qualityShadow != QualityShadow.Low) ? ((systemSettingData_Quality.ShadowDistance != 1) ? 150 : 100) : 50);
    }

    public float GetMaxCameraDistance()
    {
        if (this.screenData != null)
        {
            return (float)this.screenData.CameraMaxdistance;
        }
        return LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("CameraMaxdistance").GetCacheField_Float("value");
    }

    public int CheckResolutionIndex(string resolution)
    {
        for (int i = 0; i < this.resolutionList.Count; i++)
        {
            if (this.resolutionList[i] == resolution)
            {
                return i;
            }
        }
        return 99;
    }

    private void LoadDefaultData()
    {
        this.InitDefaultFuntionData();
        this.InitDefaultSoundData();
        this.InitDefaultShowData();
        this.InitDefaultQualityData();
        this.InitDefaultScreenData();
    }

    private void InitDefaultFuntionData()
    {
        this.defaultFuntionData = new SystemSettingData_Funtion();
        this.defaultFuntionData.AllowPartyInvite = (this.systemConfig.GetField_Int("allowPartyInvite") == 1);
        this.defaultFuntionData.AllowGuildInvite = (this.systemConfig.GetField_Int("allowGuildInvite") == 1);
        this.defaultFuntionData.AllowFriendInvite = (this.systemConfig.GetField_Int("allowFriendInvite") == 1);
        this.defaultFuntionData.LowHealthWarning = (this.systemConfig.GetField_Int("lowHealthWarning") == 1);
        this.defaultFuntionData.IfMouse = (this.systemConfig.GetField_Int("ifMouse") == 1);
        this.defaultFuntionData.Close2ndPW = (this.systemConfig.GetField_Int("CloseSecondPasswordWork") == 1);
    }

    private void InitDefaultSoundData()
    {
        this.defaultSoundData = new SystemSettingData_Sound();
        this.defaultSoundData.BgMusic = this.systemConfig.GetField_Int("bgMusic");
        this.defaultSoundData.EffectMusic = this.systemConfig.GetField_Int("effectMusic");
        this.defaultSoundData.MainSound = this.systemConfig.GetField_Int("Music");
        this.defaultSoundData.IsMain = (this.systemConfig.GetField_Int("BEMplayer") == 1);
        this.defaultSoundData.IsOther = (this.systemConfig.GetField_Int("BEMteam") == 1);
        this.defaultSoundData.IsTeam = (this.systemConfig.GetField_Int("BEMothers") == 1);
        this.defaultSoundData.IsNPC = (this.systemConfig.GetField_Int("BEMenemy") == 1);
    }

    private void InitDefaultShowData()
    {
        this.defaultShowData = new SystemSettingData_Show();
        this.defaultShowData.PlayerBarShow = (this.systemConfig.GetField_Int("playerBarShow") == 1);
        this.defaultShowData.PlayerNameShow = (this.systemConfig.GetField_Int("playerNameShow") == 1);
        this.defaultShowData.PlayerGuildShow = (this.systemConfig.GetField_Int("playerGuildShow") == 1);
        this.defaultShowData.OthersBarShow = (this.systemConfig.GetField_Int("othersBarShow") == 1);
        this.defaultShowData.OthersNameShow = (this.systemConfig.GetField_Int("othersNameShow") == 1);
        this.defaultShowData.OthersGuildShow = (this.systemConfig.GetField_Int("othersGuildShow") == 1);
        this.defaultShowData.EnemyBarShow = (this.systemConfig.GetField_Int("enemyBarShow") == 1);
        this.defaultShowData.EnemyNameShow = (this.systemConfig.GetField_Int("enemyNameShow") == 1);
        this.defaultShowData.EnemyGuildShow = (this.systemConfig.GetField_Int("enemyGuildShow") == 1);
        this.defaultShowData.NpcNameShow = (this.systemConfig.GetField_Int("npcNameShow") == 1);
    }

    private void InitDefaultQualityData()
    {
        this.defaultQualityData = new SystemSettingData_Quality();
        this.defaultQualityData.IsAutoSetting = true;
        this.defaultQualityData.QualityLevel = this.systemConfig.GetField_Int("defaultqualitylevel");
        this.defaultQualityData.Antialiasing = this.systemConfig.GetField_Int("defaultantialiasing");
        this.defaultQualityData.Vsync = (this.systemConfig.GetField_Int("defaultvsync") == 1);
        this.defaultQualityData.Shadow = this.systemConfig.GetField_Int("defaultshadows");
        this.defaultQualityData.ShadowDistance = this.systemConfig.GetField_Int("defaultshadowdistance");
    }

    private void InitDefaultScreenData()
    {
        this.defaultScreenData = new ResolutionInfo();
        this.defaultScreenData.FullScreen = 1;
        this.defaultScreenData.Height = Screen.resolutions[Screen.resolutions.Length - 1].height.ToString();
        this.defaultScreenData.Width = Screen.resolutions[Screen.resolutions.Length - 1].width.ToString();
        this.defaultScreenData.ReferenceResolution = (this.defaultScreenData.CurResolution = Screen.resolutions.Length - 1);
        this.defaultScreenData.ModeIndex = 0;
        this.defaultScreenData.CameraMaxdistance = 10;
        this.defaultScreenData.UIScale = 100;
        this.defaultScreenData.mouseSpeed = 10U;
        this.defaultScreenData.pixelpercent = 1U;
    }

    public void EnableEnemyInfo(bool active)
    {
        GameSystemSettings.MyPlayerPrefsSetInt("DB.EnemyHpBar", (!active) ? 0 : ((!this.showData.EnemyBarShow) ? 0 : 1));
        GameSystemSettings.MyPlayerPrefsSetInt("DB.EnemyName", (!active) ? 0 : ((!this.showData.EnemyNameShow) ? 0 : 1));
        GameSystemSettings.MyPlayerPrefsSetInt("DB.EnemyGuild", (!active) ? 0 : ((!this.showData.EnemyGuildShow) ? 0 : 1));
    }

    public void SendOutStuck()
    {
        this.CloseUI();
        this.mNetWork.SendOutStuck();
    }

    private const int SM_CXSCREEN = 0;

    private const int SM_CYSCREEN = 1;

    private int MAX_SCREEN_SETTING_NUM = 20;

    private int WINDOW_RATIO_NUM = 3;

    private int SETTING_VSYNC = 2;

    private int SETTING_MSAA = 4;

    private int SETTING_SHADOW = 3;

    public int WindowsWidth;

    public int WindowsHeight;

    public SystemSettingNetWork mNetWork;

    private bool bMouseClickMove = true;

    public LuaTable systemConfig;

    public List<LuaTable> screenSettingConfig = new List<LuaTable>();

    public List<LuaTable> windowratioConfig = new List<LuaTable>();

    public List<string> resolutionList = new List<string>();

    public List<string> vsyncConfig = new List<string>();

    public List<string> antialiasingConfig = new List<string>();

    public List<string> shadowsConfig = new List<string>();

    public List<string> shadowdistanceConfig = new List<string>();

    public SystemSettingData_Funtion funtionData;

    public ResolutionInfo screenData;

    public SystemSettingData_Show showData;

    public SystemSettingData_Sound soundData;

    public SystemSettingData_Quality qualityData;

    public SystemSettingData_Funtion defaultFuntionData;

    public ResolutionInfo defaultScreenData;

    public SystemSettingData_Show defaultShowData;

    public SystemSettingData_Sound defaultSoundData;

    public SystemSettingData_Quality defaultQualityData;

    private Action<string> onAutoFight;
}
