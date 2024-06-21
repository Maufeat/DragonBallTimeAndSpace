using System;
using System.Collections.Generic;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using mobapk;
using Models;
using msg;
using UnityEngine;

public class AbattoirMatchController : ControllerBase, ITeamController
{
    private UI_AbattoirMatch uiAbattoirMatch
    {
        get
        {
            return UIManager.GetUIObject<UI_AbattoirMatch>();
        }
    }

    private UI_AbattoirTips uiAbattoirTips
    {
        get
        {
            return UIManager.GetUIObject<UI_AbattoirTips>();
        }
    }

    private UI_AbattoirTransfer uiAbattoirTansfer
    {
        get
        {
            return UIManager.GetUIObject<UI_AbattoirTransfer>();
        }
    }

    private UI_AbattoirReward uiAbattoirReward
    {
        get
        {
            return UIManager.GetUIObject<UI_AbattoirReward>();
        }
    }

    private UI_PVPMatch uiPvpMatch
    {
        get
        {
            return UIManager.GetUIObject<UI_PVPMatch>();
        }
    }

    private UI_MainView uiMainView
    {
        get
        {
            return UIManager.GetUIObject<UI_MainView>();
        }
    }

    private UI_Map uiMap
    {
        get
        {
            return UIManager.GetUIObject<UI_Map>();
        }
    }

    private MainUIController mainUIController
    {
        get
        {
            return ControllerManager.Instance.GetController<MainUIController>();
        }
    }

    private UIMapController mapController
    {
        get
        {
            return ControllerManager.Instance.GetController<UIMapController>();
        }
    }

    private PVPMatchController pvpMatchController
    {
        get
        {
            return ControllerManager.Instance.GetController<PVPMatchController>();
        }
    }

    private UIHpSystem uiHpSystem
    {
        get
        {
            return ControllerManager.Instance.GetController<UIHpSystem>();
        }
    }

    private GameScene gs
    {
        get
        {
            return ManagerCenter.Instance.GetManager<GameScene>();
        }
    }

    private EntitiesManager entitiesManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<EntitiesManager>();
        }
    }

    public int getRadarRadius
    {
        get
        {
            return (int)this.radarRadius;
        }
    }

    public uint getReadyRestTime
    {
        get
        {
            float num = Time.realtimeSinceStartup - this._readyRestTime_GetTime;
            float num2 = this.readyRestTime - num;
            if (num2 < 0f)
            {
                num2 = 0f;
            }
            return (uint)Mathf.FloorToInt(num2);
        }
    }

    public int getStartRestTime
    {
        get
        {
            float num = Time.realtimeSinceStartup - this._startRestTime_GetTime;
            float num2 = this.startRestTime - num;
            if (num2 < 0f)
            {
                num2 = 0f;
            }
            return Mathf.FloorToInt(num2);
        }
    }

    public int getReliveRestTime
    {
        get
        {
            float num = Time.realtimeSinceStartup - this._reliveRestTime_GetTime;
            float num2 = this.reliveRestTime - num;
            if (num2 < 0f)
            {
                num2 = 0f;
            }
            return Mathf.FloorToInt(num2);
        }
    }

    public int getKickoutRestTime
    {
        get
        {
            float num = Time.realtimeSinceStartup - this._kickoutRestTime_GetTime;
            float num2 = this.kickoutRestTime - num;
            if (num2 < 0f)
            {
                num2 = 0f;
            }
            return Mathf.FloorToInt(num2);
        }
    }

    public int getPrayRestTime
    {
        get
        {
            float num = Time.realtimeSinceStartup - this._prayRestTime_GetTime;
            float num2 = this.prayRestTime - num;
            if (num2 < 0f)
            {
                num2 = 0f;
            }
            return Mathf.FloorToInt(num2);
        }
    }

    public string mastercopyid
    {
        get
        {
            if (string.IsNullOrEmpty(this._mastercopyid))
            {
                if (this.mobapk == null)
                {
                    this.mobapk = LuaConfigManager.GetXmlConfigTable("mobapk");
                }
                if (this.mobapk == null)
                {
                    FFDebug.LogError(this, "不存在LuaConfigManager.GetXmlConfigTable(mobapk)");
                    this._mastercopyid = "null";
                    this._copyid = "null";
                }
                else
                {
                    LuaTable luaTable = this.mobapk.GetCacheField_Table("mobacopymapindex")[1] as LuaTable;
                    this._mastercopyid = luaTable.GetField_Uint("mastercopyid").ToString();
                    this._copyid = luaTable.GetField_Uint("copyid").ToString();
                }
            }
            return this._mastercopyid;
        }
    }

    public string copyid
    {
        get
        {
            if (string.IsNullOrEmpty(this._copyid))
            {
                if (this.mobapk == null)
                {
                    this.mobapk = LuaConfigManager.GetXmlConfigTable("mobapk");
                }
                if (this.mobapk == null)
                {
                    FFDebug.LogError(this, "不存在LuaConfigManager.GetXmlConfigTable(mobapk)");
                    this._mastercopyid = "null";
                    this._copyid = "null";
                }
                else
                {
                    LuaTable luaTable = this.mobapk.GetCacheField_Table("mobacopymapindex")[1] as LuaTable;
                    this._mastercopyid = luaTable.GetField_Uint("mastercopyid").ToString();
                    this._copyid = luaTable.GetField_Uint("copyid").ToString();
                }
            }
            return this._copyid;
        }
    }

    public int getRankCount
    {
        get
        {
            return this.rankList.Count;
        }
    }

    public int getDragonBallCount
    {
        get
        {
            return this.dragonBallPosList.Count;
        }
    }

    public string getSelfTeamColor
    {
        get
        {
            if (this.myTeamInfo != null)
            {
                return this.myTeamInfo.teamid;
            }
            return string.Empty;
        }
    }

    public void ForRankList(Action<int, string, uint> dele)
    {
        if (dele == null)
        {
            return;
        }
        for (int i = 0; i < this.rankList.Count; i++)
        {
            dele(i, this.rankList[i].color, this.rankList[i].power);
        }
    }

    public void ForDragonBallPosList(Action<int, uint, uint, uint> dele)
    {
        if (dele == null)
        {
            return;
        }
        for (int i = 0; i < this.dragonBallPosList.Count; i++)
        {
            dele(i, this.dragonBallPosList[i].num, this.dragonBallPosList[i].x, this.dragonBallPosList[i].y);
        }
    }

    public AbattoirMatchState getState
    {
        get
        {
            return this.matchState;
        }
    }

    public AbattoirFightState getFightState
    {
        get
        {
            return this.fightState;
        }
    }

    public TeamUser GetMainPlayerUser(MSG_MyTeamInfo_SC info = null)
    {
        if (info == null)
        {
            info = this.myTeamInfo;
        }
        if (info == null)
        {
            return null;
        }
        for (int i = 0; i < info.users.Count; i++)
        {
            if (info.users[i].uid == MainPlayer.Self.GetCharID())
            {
                return info.users[i];
            }
        }
        return null;
    }

    public int myTeamNemberCount
    {
        get
        {
            if (this.myTeamInfo != null)
            {
                return this.myTeamInfo.users.Count;
            }
            return 0;
        }
    }

    public bool IsMyTeamNember(EntitiesID eid)
    {
        if (eid.Etype == CharactorType.Player && this.myTeamInfo != null)
        {
            for (int i = 0; i < this.myTeamInfo.users.Count; i++)
            {
                TeamUser teamUser = this.myTeamInfo.users[i];
                if (teamUser.uid == eid.Id)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsAbattoirTeamNember(ulong playerUid)
    {
        if (this.teamList != null)
        {
            for (int i = 0; i < this.teamList.Count; i++)
            {
                if (this.teamList[i].uid == playerUid)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override void Awake()
    {
        this.abattoirNetWork = new AbattoirMatchNetWork();
        this.abattoirNetWork.Initialize();
    }

    public void SetMatchState(AbattoirMatchState nextState)
    {
        this.matchState = nextState;
        switch (this.matchState)
        {
            case AbattoirMatchState.Matching:
            case AbattoirMatchState.Readying:
            case AbattoirMatchState.WaitingStart:
                return;
        }
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI<UI_AbattoirMatch>();
    }

    public void SetFightState(AbattoirFightState nextState)
    {
        if (this.fightState == nextState)
        {
            return;
        }
        AbattoirFightState abattoirFightState = this.fightState;
        this.fightState = nextState;
        if (abattoirFightState == AbattoirFightState.USTATE_SOUL)
        {
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_AbattoirTips");
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("ui_revive");
        }
        else if (nextState == AbattoirFightState.USTATE_SOUL)
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_AbattoirTips>("UI_AbattoirTips", delegate ()
            {
                if (this.uiAbattoirTips != null)
                {
                    this.uiAbattoirTips.ShowTime(this.getReliveRestTime);
                }
            }, UIManager.ParentType.Tips, true);
        }
        if (nextState == AbattoirFightState.USTATE_REWARD)
        {
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_AbattoirTips");
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("ui_revive");
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.fightState == AbattoirFightState.USTATE_SOUL)
        {
            if (this.uiAbattoirTips == null)
            {
                return;
            }
            bool flag = this.CheckInReliveArea();
            if (flag)
            {
                this.uiAbattoirTips.ShowTime(this.getReliveRestTime);
            }
            else
            {
                this.uiAbattoirTips.ShowOutArea();
            }
        }
        if (this.fightState != AbattoirFightState.NONE)
        {
        }
    }

    private bool CheckInReliveArea()
    {
        Vector2 currServerPos = MainPlayer.Self.CurrServerPos;
        List<AbattoirMatchController.ReliveAreaData> list = this.GetReliveDataList();
        for (int i = 0; i < list.Count; i++)
        {
            AbattoirMatchController.ReliveAreaData reliveAreaData = list[i];
            if (Mathf.Abs(currServerPos.x - reliveAreaData.x) < reliveAreaData.radius && Mathf.Abs(currServerPos.y - reliveAreaData.y) < reliveAreaData.radius)
            {
                return true;
            }
        }
        return false;
    }

    public void RefreshAtReliveErea(bool isAt)
    {
        if (isAt)
        {
        }
    }

    public override string ControllerName
    {
        get
        {
            return base.GetType().ToString();
        }
        set
        {
        }
    }

    public void SendReqMatch()
    {
        int field_Int = LuaConfigManager.GetXmlConfigTable("mobapk").GetField_Int("enterlevel");
        if ((ulong)MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.level < (ulong)((long)field_Int))
        {
            TipsWindow.ShowWindow(5003U);
            return;
        }
        if (this.pvpMatchController.mIsInMathingState)
        {
            TipsWindow.ShowWindow(4042U);
            return;
        }
        if (this.pvpMatchController.pvpState != StageType.None_Stage)
        {
            TipsWindow.ShowWindow(4042U);
            return;
        }
        CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
        if (manager.InCopy)
        {
            TipsWindow.ShowWindow(4041U);
            return;
        }
        AbattoirMatchState abattoirMatchState = this.matchState;
        if (abattoirMatchState != AbattoirMatchState.None)
        {
            FFDebug.LogError(this, "已匹配");
        }
        this.SetMatchState(AbattoirMatchState.Matching);
        MSG_UserMatchReq_CS msg_UserMatchReq_CS = new MSG_UserMatchReq_CS();
        msg_UserMatchReq_CS.is_match = true;
        this.abattoirNetWork.SendMatchReq(msg_UserMatchReq_CS);
        UIManager.GetUIObject<UI_PVPMatch>().btnAbattoirMatch.gameObject.SetActive(false);
        UIManager.GetUIObject<UI_PVPMatch>().btnAbattoirCancelMatch.gameObject.SetActive(true);
        this.matchStartTime = Time.realtimeSinceStartup;
        ControllerManager.Instance.GetController<MainUIController>().OpenStartMatchBox(this.matchStartTime, PvpMatchType.Abattoir);
    }

    public void SendCancelMatch()
    {
        switch (this.matchState)
        {
            case AbattoirMatchState.None:
                FFDebug.LogError(this, "未在匹配");
                break;
            case AbattoirMatchState.Matching:
                break;
            case AbattoirMatchState.Readying:
            case AbattoirMatchState.WaitingStart:
                FFDebug.LogError(this, "已匹配成功");
                break;
            case AbattoirMatchState.Entering:
                FFDebug.LogError(this, "已开始");
                break;
            default:
                FFDebug.LogError(this, "处于其他状态");
                break;
        }
        MSG_UserMatchReq_CS msg_UserMatchReq_CS = new MSG_UserMatchReq_CS();
        msg_UserMatchReq_CS.is_match = false;
        this.abattoirNetWork.SendMatchReq(msg_UserMatchReq_CS);
        this.ClearMatchData();
        UIManager.GetUIObject<UI_PVPMatch>().btnAbattoirMatch.gameObject.SetActive(true);
        UIManager.GetUIObject<UI_PVPMatch>().btnAbattoirCancelMatch.gameObject.SetActive(false);
        ControllerManager.Instance.GetController<MainUIController>().CloseMatchBox();
    }

    public void SendMatchReady()
    {
        switch (this.matchState)
        {
            case AbattoirMatchState.None:
                FFDebug.LogError(this, "未在匹配");
                break;
            case AbattoirMatchState.Matching:
                FFDebug.LogError(this, "还未匹配到");
                break;
            case AbattoirMatchState.Readying:
                break;
            case AbattoirMatchState.WaitingStart:
                FFDebug.LogError(this, "已准备");
                break;
            case AbattoirMatchState.Entering:
                FFDebug.LogError(this, "已开始");
                break;
            default:
                FFDebug.LogError(this, "处于其他状态");
                break;
        }
        this.SetMatchState(AbattoirMatchState.WaitingStart);
        MSG_MatchReady_CS msg_MatchReady_CS = new MSG_MatchReady_CS();
        msg_MatchReady_CS.id = this.matchId;
        this.abattoirNetWork.SendMatchReady(msg_MatchReady_CS);
    }

    public ulong GetCurExp()
    {
        TeamUser mainPlayerUser = this.GetMainPlayerUser(null);
        return (ulong)this.GetUserShowExp(mainPlayerUser);
    }

    public uint GetCurLv()
    {
        TeamUser mainPlayerUser = this.GetMainPlayerUser(null);
        if (mainPlayerUser == null)
        {
            return 0U;
        }
        return mainPlayerUser.level;
    }

    private void TryInitLvConfigs()
    {
        if (this.lvDic == null)
        {
            this.lvDic = new Dictionary<uint, uint>();
            if (this.mobapk == null)
            {
                this.mobapk = LuaConfigManager.GetXmlConfigTable("mobapk");
            }
            if (this.mobapk == null)
            {
                FFDebug.LogError(this, "不存在LuaConfigManager.GetXmlConfigTable(mobapk)");
                return;
            }
            LuaTable field_Table = this.mobapk.GetField_Table("mobaexp");
            for (int i = 0; i < field_Table.Count; i++)
            {
                LuaTable luaTable = field_Table[i + 1] as LuaTable;
                if (luaTable == null)
                {
                    FFDebug.LogError(this, "mobapk -> transgroup -> item==null:" + field_Table.ToString());
                }
                else
                {
                    uint field_Uint = luaTable.GetField_Uint("mobalevel");
                    uint field_Uint2 = luaTable.GetField_Uint("mobaexp");
                    if (!this.lvDic.ContainsKey(field_Uint))
                    {
                        this.lvDic.Add(field_Uint, field_Uint2);
                    }
                    else
                    {
                        FFDebug.LogError(this, "mobapk -> mobaexp -> 有重复等级项 lv：" + field_Uint);
                    }
                }
            }
        }
    }

    public bool TryGetLevelExp(uint level, out uint exp)
    {
        this.TryInitLvConfigs();
        exp = 0U;
        uint num = 0U;
        if (level > 0U && !this.lvDic.TryGetValue(level, out num))
        {
            return false;
        }
        uint num2;
        if (!this.lvDic.TryGetValue(level + 1U, out num2))
        {
            return false;
        }
        if (num2 < num)
        {
            return true;
        }
        exp = num2 - num;
        return true;
    }

    public uint GetCurLevelExp(uint level)
    {
        uint result;
        if (!this.TryGetLevelExp(level, out result))
        {
            result = 0U;
        }
        return result;
    }

    public uint GetUserShowExp(TeamUser user)
    {
        if (user == null)
        {
            return 0U;
        }
        this.TryInitLvConfigs();
        uint num = 0U;
        if (user.level <= 0U || !this.lvDic.TryGetValue(user.level, out num))
        {
        }
        return (user.exp <= num) ? 0U : (user.exp - num);
    }

    public void UpdateTeamMatchInfo(MSG_MatchInfo_SC msgData)
    {
        switch (this.matchState)
        {
            case AbattoirMatchState.None:
                FFDebug.LogError(this, "未在匹配");
                break;
            case AbattoirMatchState.Matching:
                this.SetMatchState(AbattoirMatchState.Readying);
                break;
            case AbattoirMatchState.Readying:
            case AbattoirMatchState.WaitingStart:
                break;
            case AbattoirMatchState.Entering:
                FFDebug.LogError(this, "已开始");
                break;
            default:
                FFDebug.LogError(this, "处于其他状态");
                break;
        }
        this.matchId = msgData.id;
        this.matchNum = msgData.num;
        this.readyNum = msgData.ready_num;
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_AbattoirMatch>("UI_AbattoirMatch", delegate ()
        {
            this.pvpMatchController.CloseUI();
            this.uiAbattoirMatch.OpenShow(this.getReadyRestTime, this.readyNum, this.matchNum, this.matchState > AbattoirMatchState.Readying);
        }, UIManager.ParentType.CommonUI, true);
        ControllerManager.Instance.GetController<MainUIController>().CloseMatchBox();
    }

    public void DismissGroup(MSG_DismissGroup_SC msgData)
    {
        AbattoirMatchState lastState = this.matchState;
        this.ClearMatchData();
        this.CloseAbattoirTeamUI();
        ControllerManager.Instance.GetController<MainUIController>().CloseMatchBox();
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_PVPMatch>("UI_PVPMatch", delegate ()
        {
            UIManager.GetUIObject<UI_PVPMatch>().btnAbattoirMatch.gameObject.SetActive(true);
            UIManager.GetUIObject<UI_PVPMatch>().btnAbattoirCancelMatch.gameObject.SetActive(false);
            if (lastState == AbattoirMatchState.WaitingStart)
            {
                this.SendReqMatch();
            }
        }, UIManager.ParentType.CommonUI, true);
    }

    public void MyTeamInfo(MSG_MyTeamInfo_SC msgData)
    {
        MSG_MyTeamInfo_SC info = this.myTeamInfo;
        TeamUser mainPlayerUser = this.GetMainPlayerUser(info);
        uint num = (mainPlayerUser == null) ? 0U : mainPlayerUser.level;
        uint userShowExp = this.GetUserShowExp(mainPlayerUser);
        TeamUser mainPlayerUser2 = this.GetMainPlayerUser(msgData);
        uint num2 = mainPlayerUser2.level;
        uint num3 = this.GetUserShowExp(mainPlayerUser2);
        if (num2 != num || num3 != userShowExp)
        {
            LuaScriptMgr.Instance.CallLuaFunction("HerosCtrl.UpdateHeroLevelAndExp", new object[]
            {
                mainPlayerUser2.uid,
                num2,
                num3
            });
        }
        this.myTeamInfo = msgData;
        if (num2 != num || num3 != userShowExp)
        {
            num2 = this.mainUIController.GetCurLv();
            num3 = (uint)this.mainUIController.GetCurExp();
            this.mainUIController.RefreshPlayerEXP(num2, num3);
            uint secondLv = this.mainUIController.GetSecondLv();
            uint exp = (uint)this.mainUIController.GetSecondExp();
            this.mainUIController.RefreshHeroEXP(secondLv, exp);
            UI_Character uiobject = UIManager.GetUIObject<UI_Character>();
            if (uiobject != null)
            {
                uiobject.RefreshExpPanel();
            }
            UI_HeroHandbook uiobject2 = UIManager.GetUIObject<UI_HeroHandbook>();
            if (uiobject2 != null)
            {
                uiobject2.SetupInfo();
            }
            if (MainPlayer.Self.MainPlayeData != null)
            {
                MainPlayer.Self.MainPlayeData.RefreshExp(num3, num2);
            }
            if (MainPlayer.Self.OtherPlayerData != null)
            {
                MainPlayer.Self.OtherPlayerData.RefreshPlayerLevel(num2);
            }
        }
        if (this._startRestTime_GetTime > 0f)
        {
            if (this.uiMainView != null)
            {
                this.uiMainView.refreshAbattoirTeamInfo();
                this.RefreshTeamNemberInMap();
                if (this.uiMap != null && this.uiMap.IsOnCurrAreamap)
                {
                    this.uiMap.RefreshAbattoirAreaList();
                }
            }
            return;
        }
        if (this.mobapk == null)
        {
            this.mobapk = LuaConfigManager.GetXmlConfigTable("mobapk");
            if (this.mobapk == null)
            {
                FFDebug.LogError(this, "不存在LuaConfigManager.GetXmlConfigTable(mobapk)");
                return;
            }
        }
        int field_Int = this.mobapk.GetField_Int("countdown");
        this.startRestTime = (uint)field_Int;
        this._startRestTime_GetTime = Time.realtimeSinceStartup;
        MainPlayer.Self.OnRestTime = true;
        Scheduler.Instance.AddTimer(this.startRestTime, false, new Scheduler.OnScheduler(this.AfterRestTime));
        if (this.gs.isAbattoirScene)
        {
            this.OnSceneLoaded();
        }
        else
        {
            this.gs.RegOnSceneLoadCallBack(new Action(this.OnSceneLoaded));
        }
    }

    public void ShowRadarAreaIcon(bool show)
    {
        if (this.uiMap != null && this.uiMap.IsOnCurrAreamap)
        {
            this.uiMap.ShowRadarAreaIcon(show);
        }
    }

    public void RefreshServerTime(MSG_ServerTimer_SC msgData)
    {
        switch (msgData.id)
        {
            case ServerTimer.MobaPk_Confirm_RestTime:
                this.readyRestTime = msgData.resttime;
                this._readyRestTime_GetTime = Time.realtimeSinceStartup;
                if (this.uiAbattoirMatch != null)
                {
                    this.uiAbattoirMatch.OpenShow(this.getReadyRestTime, this.readyNum, this.matchNum, this.matchState > AbattoirMatchState.Readying);
                }
                break;
            case ServerTimer.MobaPk_Start_RestTime:
                this.startRestTime = msgData.resttime;
                this._startRestTime_GetTime = Time.realtimeSinceStartup;
                UIManager.Instance.ShowUI<UI_CoolDown>("UI_CoolDown", delegate ()
                {
                    UIManager.GetUIObject<UI_CoolDown>().CoolDown(this.getStartRestTime);
                }, UIManager.ParentType.CommonUI, false);
                break;
            case ServerTimer.MobaPk_Relive_RestTime:
                this.reliveRestTime = msgData.resttime;
                this._reliveRestTime_GetTime = Time.realtimeSinceStartup;
                break;
            case ServerTimer.MobaPk_KickoutLastOne_RestTime:
                this.kickoutRestTime = msgData.resttime;
                this._kickoutRestTime_GetTime = Time.realtimeSinceStartup;
                break;
            case ServerTimer.MobaPk_Pray_RestTime:
                {
                    this.prayRestTime = msgData.resttime;
                    this._prayRestTime_GetTime = Time.realtimeSinceStartup;
                    AbattoirPrayController controller = ControllerManager.Instance.GetController<AbattoirPrayController>();
                    controller.UpdatePrayTime(this.getPrayRestTime);
                    break;
                }
        }
    }

    public void RefreshRadarPos(MSG_RefreshRadarPos_CSC msgData)
    {
        this.radarRadius = msgData.radius;
        if (this.uiMap != null && this.uiMap.IsOnCurrAreamap)
        {
            this.uiMap.RefreshAbattoirAreaList();
        }
        for (int i = 0; i < msgData.pos.Count; i++)
        {
            RadarPos radarPos = msgData.pos[i];
            this.mapController.SetDraginBallIconInfo(radarPos.uid + ":" + i, radarPos.num, radarPos.x, radarPos.y);
        }
        for (int j = 0; j < this.dragonBallPosList.Count; j++)
        {
            RadarPos radarPos2 = this.dragonBallPosList[j];
            if (j >= msgData.pos.Count)
            {
                break;
            }
            RadarPos radarPos3 = msgData.pos[j];
            if (j >= msgData.pos.Count || radarPos2.uid != radarPos3.uid || radarPos2.num != radarPos3.num)
            {
                RadarPos radarPos4 = this.dragonBallPosList[j];
                this.mapController.DeleteDraginBallIcon(radarPos4.uid + ":" + j, radarPos4.num);
            }
        }
        this.dragonBallPosList.Clear();
        this.dragonBallPosList.AddRange(msgData.pos);
    }

    public void RefreashTeamListAndColor(MSG_BstUserTeamInfo_SC msgData)
    {
        this.teamList = msgData.infos;
        if (this.uiHpSystem != null)
        {
            this.uiHpSystem.ResetPKModel();
        }
        if (this.uiMainView != null)
        {
            this.uiMainView.refreshSelfTeamColorSpr(true);
        }
        Scheduler.Instance.AddTimer(5f, true, new Scheduler.OnScheduler(this.SendReqRadarPos));
    }

    public void SendReqRadarPos()
    {
        this.abattoirNetWork.SendReqRadarPos();
    }

    public void RefreshPowerRank(MSG_RefreshPowerRank_SC msgData)
    {
        switch (this.matchState)
        {
            case AbattoirMatchState.None:
            case AbattoirMatchState.Matching:
            case AbattoirMatchState.Readying:
            case AbattoirMatchState.WaitingStart:
                FFDebug.LogWarning(this, "不在角斗场");
                break;
        }
        uint num = 0U;
        for (int i = 0; i < this.rankList.Count; i++)
        {
            if (this.rankList[i].color == this.getSelfTeamColor)
            {
                num = this.rankList[i].power;
            }
        }
        this.rankList.Clear();
        this.rankList.AddRange(msgData.items);
        if (this.uiMainView != null)
        {
            this.uiMainView.refreshAbattoirRankList();
        }
        for (int j = 0; j < this.rankList.Count; j++)
        {
            if (this.rankList[j].color == this.getSelfTeamColor && num != this.rankList[j].power)
            {
                this.RefreshSkillShow();
            }
        }
    }

    public void GameOver(MSG_GameOver_SC msgData)
    {
        this.rewards.Clear();
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_AbattoirReward>("UI_AbattoirReward", delegate ()
        {
            if (this.uiAbattoirReward != null)
            {
                if (this.myTeamNemberCount == 0)
                {
                    ManagerCenter.Instance.GetManager<UIManager>().DeleteUI(this.uiAbattoirReward);
                    return;
                }
                int field_Int = LuaConfigManager.GetXmlConfigTable("mobapk").GetField_Int("rewardtime");
                this.uiAbattoirReward.OpenShow(this.myTeamNemberCount, (int)msgData.rank, field_Int);
                for (int i = 0; i < this.rewards.Count; i++)
                {
                    this.SetRewardItem(this.rewards[i]);
                }
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public void RefreshReward(MSG_RewardBagInfo_SC msgData)
    {
        for (int i = 0; i < msgData.infos.Count; i++)
        {
            GetBagInfo getBagInfo = msgData.infos[i];
            this.rewards.Add(getBagInfo);
            this.SetRewardItem(getBagInfo);
        }
    }

    public void OnMobaLevelUp(MSG_MobaLevelUp_SC msgData)
    {
        int oldlevel = (int)msgData.oldlevel;
        int newlevel = (int)msgData.newlevel;
        for (int i = oldlevel; i < newlevel; i++)
        {
            this.mainUIController.LevelUpEffect(msgData.uid, (CharactorType)msgData.type);
        }
    }

    public void OnClientEffect(MSG_ClientEffect_SC msgData)
    {
        uint effectid = msgData.effectid;
        ulong uid = msgData.uid;
        this.InitEffectConfig();
        string text;
        if (!this.effectDic.TryGetValue(effectid, out text))
        {
            FFDebug.LogError(this, "配置mobapk.effect列表不存在特效 id：" + effectid);
            return;
        }
        OtherPlayer playerByID = this.entitiesManager.GetPlayerByID(uid);
        if (playerByID == null)
        {
            FFDebug.LogError(this, "MSG_ClientEffect_SC 没有找到玩家 uid：" + uid);
            return;
        }
        FFEffectControl component = playerByID.GetComponent<FFEffectControl>();
        switch (effectid)
        {
            case 101U:
            case 102U:
            case 103U:
                component.AddEffectGroupOnce(text);
                return;
            case 104U:
            case 105U:
                {
                    Vector3 targetpos = Vector3.zero;
                    if (effectid == 104U)
                    {
                        uint posx = msgData.posx;
                        uint posy = msgData.posy;
                        targetpos = GraphUtils.GetWorldPosByServerPos(new Vector2(posx, posy));
                        targetpos.y = MapHightDataHolder.GetMapHeight(targetpos.x, targetpos.z);
                    }
                    else
                    {
                        component = MainPlayer.Self.GetComponent<FFEffectControl>();
                        targetpos = playerByID.ModelObj.transform.position;
                    }
                    FFEffectManager manager = ManagerCenter.Instance.GetManager<FFEffectManager>();
                    string[] array = manager.GetGroup(text);
                    if (array.Length == 0)
                    {
                        array = new string[]
                        {
                    text
                        };
                    }
                    component.AddEffect(array, targetpos);
                    for (int i = 0; i < array.Length; i++)
                    {
                        EffectClip clip = manager.GetClip(array[i]);
                        if (!clip.IsPointPosition)
                        {
                            FFDebug.LogError(this, string.Concat(new object[]
                            {
                        "EffectMgr.GetClip Error ",
                        effectid,
                        "特效clip.IsPointPosition应该等于true effectName:",
                        array[i]
                            }));
                        }
                    }
                    return;
                }
            default:
                return;
        }
    }

    private void InitEffectConfig()
    {
        if (this.effectDic != null)
        {
            return;
        }
        this.effectDic = new Dictionary<uint, string>();
        if (this.mobapk == null)
        {
            this.mobapk = LuaConfigManager.GetXmlConfigTable("mobapk");
            if (this.mobapk == null)
            {
                FFDebug.LogError(this, "不存在LuaConfigManager.GetXmlConfigTable(mobapk)");
                return;
            }
        }
        LuaTable cacheField_Table = this.mobapk.GetCacheField_Table("effect");
        if (cacheField_Table == null)
        {
            return;
        }
        for (int i = 0; i < cacheField_Table.Count; i++)
        {
            LuaTable luaTable = cacheField_Table[i + 1] as LuaTable;
            this.effectDic.Add(luaTable.GetField_Uint("id"), luaTable.GetField_String("name"));
        }
    }

    private void SetRewardItem(GetBagInfo item)
    {
        if (this.uiAbattoirReward == null)
        {
            return;
        }
        this.uiAbattoirReward.SetItem(item.idx, item.name, item.objectid, item.count);
    }

    public void SendGetReward(int index)
    {
        MSG_UserGetAwardReq_CS msg_UserGetAwardReq_CS = new MSG_UserGetAwardReq_CS();
        msg_UserGetAwardReq_CS.idx = (uint)((index <= 0) ? 0 : index);
        this.abattoirNetWork.SendChooseReward(msg_UserGetAwardReq_CS);
    }

    private void AfterRestTime()
    {
        if (MainPlayer.Self != null)
        {
            MainPlayer.Self.OnRestTime = false;
        }
    }

    public void AbattoirGameOver()
    {
        this.ClearMatchData();
    }

    private void OnSceneLoaded()
    {
        if (!this.gs.isAbattoirScene)
        {
            return;
        }
        this.SetMatchState(AbattoirMatchState.Entered);
        this.gs.UnRegOnSceneLoadCallBack(new Action(this.OnSceneLoaded));
        this.gs.RegOnSceneLoadCallBack(new Action(this.OnSceneUnloaded));
        ServerStorageManager.Instance.GetData(ServerStorageKey.AbattoirShortcuts, 0U);
        if (this.uiMainView != null && this.uiMainView.abattoirRankPanel != null)
        {
            this.uiMainView.refreshAbattoirRankList();
            this.uiMainView.abattoirRankPanel.gameObject.SetActive(true);
        }
        if (this.uiMap != null)
        {
            this.uiMap.SetMiddleExitCopyState(true);
        }
    }

    private void OnSceneUnloaded()
    {
        FFDebug.LogError(this, "OnSceneUnloaded");
        this.AbattoirGameOver();
        this.gs.UnRegOnSceneLoadCallBack(new Action(this.OnSceneUnloaded));
    }

    private void ClearMatchData()
    {
        this.matchId = 0UL;
        this.matchNum = 0U;
        this.readyNum = 0U;
        this.matchStartTime = 0f;
        this.readyRestTime = 0U;
        this._readyRestTime_GetTime = 0f;
        this.startRestTime = 0U;
        this._startRestTime_GetTime = 0f;
        this.reliveRestTime = 0U;
        this._reliveRestTime_GetTime = 0f;
        this.kickoutRestTime = 0U;
        this._kickoutRestTime_GetTime = 0f;
        this.prayRestTime = 0U;
        this._prayRestTime_GetTime = 0f;
        this.SetMatchState(AbattoirMatchState.None);
        this.SetFightState(AbattoirFightState.NONE);
        this.dragonBallPosList.Clear();
        this.rankList.Clear();
        this.colorDic.Clear();
        this.myTeamInfo = null;
        this.teamList = null;
        ServerStorageManager.Instance.GetData(ServerStorageKey.Shortcuts, 0U);
        SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
        if (controller != null)
        {
            ServerStorageManager.Instance.GetData(controller.skillslotkey, 0U);
        }
        if (this.uiMainView != null)
        {
            this.uiMainView.abattoirRankPanel.gameObject.SetActive(false);
            this.uiMainView.refreshTeamInfo();
        }
        this.gs.UnRegOnSceneLoadCallBack(new Action(this.OnSceneLoaded));
        ControllerManager.Instance.GetController<CoolDownController>().CloseUI();
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.abattoirNetWork.SendReqRadarPos));
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.AfterRestTime));
        this.AfterRestTime();
    }

    public void CloseAbattoirTeamUI()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI(this.uiAbattoirMatch);
    }

    public RelationType GetOtherPlayerRelationTypeToMainPlayer(OtherPlayer other)
    {
        if (this.myTeamInfo == null)
        {
            FFDebug.LogError(this, "myTeamInfo==null");
            return RelationType.None;
        }
        for (int i = 0; i < this.myTeamInfo.users.Count; i++)
        {
            if (this.myTeamInfo.users[i].uid == other.EID.Id)
            {
                return RelationType.Friend;
            }
        }
        return RelationType.Enemy;
    }

    public RelationType GetNpcRelationTypeToMainPlayer(Npc npc)
    {
        RelationType result = RelationType.None;
        if (npc == null)
        {
            return result;
        }
        if (npc.CharState != CharactorState.CreatComplete)
        {
            return result;
        }
        NpcData npcData = npc.NpcData;
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npcData.MapNpcData.baseid);
        if (configTable == null)
        {
            return result;
        }
        NpcType cacheField_Uint = (NpcType)configTable.GetCacheField_Uint("kind");
        if (cacheField_Uint == NpcType.NPC_TYPE_MOBAPK_BOSS || cacheField_Uint == NpcType.NPC_TYPE_MOBAPK_NORMAL)
        {
            result = RelationType.Enemy;
        }
        return result;
    }

    public Color ColorByCamp(ulong playerUid)
    {
        string colorConfigByUid = this.GetColorConfigByUid(playerUid);
        return Const.GetColorByName(colorConfigByUid);
    }

    public string GetColorConfigByUid(ulong playerUid)
    {
        if (this.teamList != null)
        {
            for (int i = 0; i < this.teamList.Count; i++)
            {
                if (this.teamList[i].uid == playerUid)
                {
                    return this.teamList[i].team_color;
                }
            }
        }
        return string.Empty;
    }

    private void InitColorDic()
    {
        if (this.colorImageDic != null)
        {
            return;
        }
        this.colorImageDic = new Dictionary<string, LuaTable>();
        if (this.mobapk == null)
        {
            this.mobapk = LuaConfigManager.GetXmlConfigTable("mobapk");
        }
        if (this.mobapk == null)
        {
            FFDebug.LogError(this, "不存在LuaConfigManager.GetXmlConfigTable(mobapk)");
            return;
        }
        LuaTable field_Table = this.mobapk.GetField_Table("team_color");
        if (field_Table == null)
        {
            return;
        }
        try
        {
            for (int i = 0; i < field_Table.Count; i++)
            {
                LuaTable luaTable = field_Table[i + 1] as LuaTable;
                if (luaTable == null)
                {
                    FFDebug.LogError(this, "mobapk -> team_color -> item==null:" + field_Table.ToString());
                }
                else
                {
                    string cacheField_String = luaTable.GetCacheField_String("value");
                    if (!string.IsNullOrEmpty(cacheField_String))
                    {
                        this.colorImageDic.Add(cacheField_String, luaTable);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError(this, "mobapk -> team_color -> value值可能重复：" + ex.ToString());
        }
    }

    public string GetColorImageSpFillNameByUid(ulong playerUid)
    {
        string colorConfigByUid = this.GetColorConfigByUid(playerUid);
        if (this.colorImageDic == null)
        {
            this.InitColorDic();
        }
        LuaTable luaTable;
        if (!this.colorImageDic.TryGetValue(colorConfigByUid, out luaTable))
        {
            return string.Empty;
        }
        return luaTable.GetCacheField_String("spFillName");
    }

    public string GetColorImageSpBgNameByUid(ulong playerUid)
    {
        string colorConfigByUid = this.GetColorConfigByUid(playerUid);
        if (this.colorImageDic == null)
        {
            this.InitColorDic();
        }
        LuaTable luaTable;
        if (!this.colorImageDic.TryGetValue(colorConfigByUid, out luaTable))
        {
            return string.Empty;
        }
        return luaTable.GetCacheField_String("spBgName");
    }

    public string GetColorStrBGIconNameByUid(ulong playerUid)
    {
        string colorConfigByUid = this.GetColorConfigByUid(playerUid);
        if (this.colorImageDic == null)
        {
            this.InitColorDic();
        }
        LuaTable luaTable;
        if (!this.colorImageDic.TryGetValue(colorConfigByUid, out luaTable))
        {
            return string.Empty;
        }
        return luaTable.GetCacheField_String("strBGIcon");
    }

    public string GetColorStrHPFGIconNameByUid(ulong playerUid)
    {
        string colorConfigByUid = this.GetColorConfigByUid(playerUid);
        if (this.colorImageDic == null)
        {
            this.InitColorDic();
        }
        LuaTable luaTable;
        if (!this.colorImageDic.TryGetValue(colorConfigByUid, out luaTable))
        {
            return string.Empty;
        }
        return luaTable.GetCacheField_String("strHPFGIcon");
    }

    public string GetColorStrBGNameByUid(ulong playerUid)
    {
        string colorConfigByUid = this.GetColorConfigByUid(playerUid);
        if (this.colorImageDic == null)
        {
            this.InitColorDic();
        }
        LuaTable luaTable;
        if (!this.colorImageDic.TryGetValue(colorConfigByUid, out luaTable))
        {
            return string.Empty;
        }
        return luaTable.GetCacheField_String("strBG");
    }

    public string GetRankColorImageNameByConfig(string colorConfig)
    {
        if (this.colorImageDic == null)
        {
            this.InitColorDic();
        }
        LuaTable luaTable;
        if (!this.colorImageDic.TryGetValue(colorConfig, out luaTable))
        {
            return string.Empty;
        }
        return luaTable.GetCacheField_String("strHPFGIcon");
    }

    public List<AbattoirMatchController.ReliveAreaData> GetReliveDataList()
    {
        if (this.reliveDataList == null)
        {
            this.reliveDataList = new List<AbattoirMatchController.ReliveAreaData>();
            List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("coordinates_arena_config");
            if (configTableList == null)
            {
                FFDebug.LogError(this, "不存在LuaConfigManager.GetXmlConfigTable(mobapk)");
            }
            else
            {
                foreach (LuaTable luaTable in configTableList)
                {
                    if (luaTable.GetCacheField_Uint("coordinatetype") == 2U)
                    {
                        this.reliveDataList.Add(new AbattoirMatchController.ReliveAreaData
                        {
                            x = luaTable.GetCacheField_Uint("coordinatex"),
                            y = luaTable.GetCacheField_Uint("coordinatey"),
                            radius = luaTable.GetCacheField_Uint("radius")
                        });
                    }
                }
            }
        }
        return this.reliveDataList;
    }

    public void RefreshTeamNemberInMap()
    {
        this.mapController.DeleteIconbyType(GameMap.ItemType.Team);
        if (this.myTeamInfo == null)
        {
            FFDebug.LogError(this, "myTeamInfo==null");
            return;
        }
        int num = 0;
        for (int i = 0; i < this.myTeamInfo.users.Count; i++)
        {
            TeamUser teamUser = this.myTeamInfo.users[i];
            if (teamUser.uid != MainPlayer.Self.EID.Id)
            {
                EntitiesID entitiesID;
                if (!this.tempEIDDic.TryGetValue(teamUser.uid, out entitiesID))
                {
                    entitiesID = new EntitiesID(teamUser.uid, CharactorType.Player);
                    this.tempEIDDic.Add(teamUser.uid, entitiesID);
                }
                if (teamUser.online)
                {
                    this.mapController.SetTeamIconInfo(entitiesID, new Vector2(teamUser.x, teamUser.y), num);
                }
                else
                {
                    this.mapController.DeleteIcon(entitiesID, GameMap.ItemType.Team);
                }
                num++;
            }
        }
    }

    public void ShowReliveInSitu(bool show)
    {
        LuaScriptMgr.Instance.CallLuaFunction("ReviveCtrl.ShowReliveInSitu", new object[]
        {
            Util.GetLuaTable("ReviveCtrl"),
            show
        });
    }

    public void UseCapsuleItemByPos(string thisID, float x, float y)
    {
        MSG_UseSpecialCapsule_CS msg_UseSpecialCapsule_CS = new MSG_UseSpecialCapsule_CS();
        msg_UseSpecialCapsule_CS.thisid = thisID;
        msg_UseSpecialCapsule_CS.x = ((x <= 0f) ? 0U : ((uint)x));
        msg_UseSpecialCapsule_CS.y = ((y <= 0f) ? 0U : ((uint)y));
        this.abattoirNetWork.SendUseCapsuleItemByPos(msg_UseSpecialCapsule_CS);
    }

    public void OpenTransfer(Action<int> callback)
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_AbattoirTransfer>("UI_AbattoirTransfer", delegate ()
        {
            if (this.uiAbattoirTansfer != null)
            {
                this.uiAbattoirTansfer.Refresh(callback);
            }
            else
            {
                callback(-1);
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public string GetColor2EndTargetImageBgNameByUid(ulong playerUid)
    {
        string colorConfigByUid = this.GetColorConfigByUid(playerUid);
        if (this.colorImageDic == null)
        {
            this.InitColorDic();
        }
        LuaTable luaTable;
        if (!this.colorImageDic.TryGetValue(colorConfigByUid, out luaTable))
        {
            return string.Empty;
        }
        return luaTable.GetCacheField_String("img2ndBg");
    }

    public string GetColor2EndTargetImageFillBgNameByUid(ulong playerUid)
    {
        string colorConfigByUid = this.GetColorConfigByUid(playerUid);
        if (this.colorImageDic == null)
        {
            this.InitColorDic();
        }
        LuaTable luaTable;
        if (!this.colorImageDic.TryGetValue(colorConfigByUid, out luaTable))
        {
            return string.Empty;
        }
        return luaTable.GetCacheField_String("img2ndSliderBg");
    }

    public string GetColor2EndTargetImageSpFillNameByUid(ulong playerUid)
    {
        string colorConfigByUid = this.GetColorConfigByUid(playerUid);
        if (this.colorImageDic == null)
        {
            this.InitColorDic();
        }
        LuaTable luaTable;
        if (!this.colorImageDic.TryGetValue(colorConfigByUid, out luaTable))
        {
            return string.Empty;
        }
        return luaTable.GetCacheField_String("img2ndSliderFill");
    }

    private void RefreshSkillShow()
    {
        for (int i = 0; i < this.uiMainView.SkillButtonList.Count; i++)
        {
            this.uiMainView.SkillButtonList[i].TryUpdateButtonColor(0U);
        }
    }

    public bool CheckCanEnter(uint skillid)
    {
        if (this.myTeamInfo == null || this.rankList == null)
        {
            return true;
        }
        this.InitMobapkConfig();
        if (!MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState(1353))
        {
            for (int i = 0; i < this.rankList.Count; i++)
            {
                if (this.rankList[i].color == this.getSelfTeamColor && this.rankList[i].power >= this.unLockSkillPower)
                {
                    return true;
                }
            }
            uint heroid = this.GetMainPlayerUser(null).heroid;
            List<uint> list;
            if (this.heroSkillOpenDic.TryGetValue(heroid, out list))
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j] == skillid)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private void InitMobapkConfig()
    {
        if (this.mobapkInited)
        {
            return;
        }
        this.mobapkInited = true;
        this.heroSkillOpenDic = new Dictionary<uint, List<uint>>();
        this.unLockSkillPower = 0U;
        try
        {
            if (this.mobapk == null)
            {
                this.mobapk = LuaConfigManager.GetXmlConfigTable("mobapk");
            }
            if (this.mobapk == null)
            {
                FFDebug.LogError(this, "不存在LuaConfigManager.GetXmlConfigTable(mobapk)");
            }
            else
            {
                LuaTable cacheField_Table = this.mobapk.GetCacheField_Table("lockskill");
                foreach (object field in cacheField_Table.Keys)
                {
                    LuaTable luaTable = cacheField_Table[field] as LuaTable;
                    List<uint> list = new List<uint>();
                    string field_String = luaTable.GetField_String("skillid");
                    if (!string.IsNullOrEmpty(field_String))
                    {
                        string[] array = field_String.Split(new char[]
                        {
                            ','
                        });
                        for (int i = 0; i < array.Length; i++)
                        {
                            list.Add(uint.Parse(array[i]));
                        }
                    }
                    this.heroSkillOpenDic.Add(luaTable.GetField_Uint("heroid"), list);
                }
                this.unLockSkillPower = this.mobapk.GetField_Uint("unlockpower");
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError(this, "解析mobapk报错：" + ex.ToString());
        }
    }

    private AbattoirMatchNetWork abattoirNetWork;

    public MSG_MyTeamInfo_SC myTeamInfo;

    public List<UserTeamInfo> teamList;

    public Dictionary<string, LuaTable> colorImageDic;

    public Dictionary<uint, uint> lvDic;

    private ulong matchId;

    private uint matchNum;

    private uint readyNum;

    private float matchStartTime;

    private AbattoirMatchState matchState;

    private AbattoirFightState fightState;

    private List<RadarPos> dragonBallPosList = new List<RadarPos>();

    private uint radarRadius;

    private List<PowerItem> rankList = new List<PowerItem>();

    private readonly RadarPos emtyDragonBall = new RadarPos();

    private uint readyRestTime;

    private float _readyRestTime_GetTime;

    private uint startRestTime;

    private float _startRestTime_GetTime;

    private uint reliveRestTime;

    private float _reliveRestTime_GetTime;

    private uint kickoutRestTime;

    private float _kickoutRestTime_GetTime;

    private uint prayRestTime;

    private float _prayRestTime_GetTime;

    private LuaTable colortb;

    private LuaTable colortypetb;

    private Dictionary<string, Color> colorDic = new Dictionary<string, Color>();

    private LuaTable mobapk;

    private string _mastercopyid;

    private string _copyid;

    private List<AbattoirMatchController.ReliveAreaData> reliveDataList;

    private Dictionary<ulong, EntitiesID> tempEIDDic = new Dictionary<ulong, EntitiesID>();

    private List<GetBagInfo> rewards = new List<GetBagInfo>();

    public Dictionary<int, SaveDataItem> mAbattoirSaveDataItemDic;

    private Dictionary<uint, string> effectDic;

    private Dictionary<uint, List<uint>> heroSkillOpenDic = new Dictionary<uint, List<uint>>();

    private uint unLockSkillPower;

    private bool mobapkInited;

    public class ReliveAreaData
    {
        public uint x;

        public uint y;

        public uint radius;
    }
}
