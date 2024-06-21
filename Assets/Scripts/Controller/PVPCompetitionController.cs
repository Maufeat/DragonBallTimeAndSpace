using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Models;
using msg;
using rankpk_msg;
using Team;
using UnityEngine;

public class PVPCompetitionController : ControllerBase
{
    public override string ControllerName
    {
        get
        {
            return "pvpcompetition_controller";
        }
    }

    public UI_Score uiScore
    {
        get
        {
            return UIManager.GetUIObject<UI_Score>();
        }
    }

    public override void Awake()
    {
        this.mNetWork = new PVPCompetitionNetWork();
        this.mNetWork.Initialize();
    }

    public override void OnUpdate()
    {
    }

    public void ShowScoreUI(Action callback)
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Score>("UI_Score", delegate ()
        {
            if (this.uiScore != null)
            {
                this.uiScore.SetScoreTime(this._competitionTime);
                this.uiScore.SetCompetition(0);
                if (callback != null)
                {
                    callback();
                }
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public void CloseScoreUI()
    {
        this.teamLeftInfo = null;
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CloseScoreUI));
        if (this.uiScore != null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_Score");
        }
    }

    public void ReqRankPKCurStage_CS()
    {
        this.mNetWork.ReqRankPKCurStage_CS();
        this.EnableCompetitionDynamic(true);
        ControllerManager.Instance.GetController<SystemSettingController>().EnableEnemyInfo(false);
    }

    public void Req_ExitCopymap_SC()
    {
        ControllerManager.Instance.GetController<PVPMatchController>().pvpState = StageType.Finish;
        this.mNetWork.Req_ExitCopymap_SC();
    }

    public void RetFightCountDown_SC(uint during)
    {
        this.ShowScoreUI(delegate
        {
        });
        this._competitionTime = during;
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CompetitionTimer));
        Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.CompetitionTimer));
    }

    public void RetStartFight_SC(uint duration, MSG_RetTeamCurScore_SC score)
    {
        if (ManagerCenter.Instance.GetManager<CopyManager>().InCompetitionCopy)
        {
            ControllerManager.Instance.GetController<MainUIController>().ActiveTask(false);
        }
        this.EnableCompetitionDynamic(false);
        ControllerManager.Instance.GetController<PVPMatchController>().pvpState = StageType.Fight;
        this._competitionTime = duration;
        this.leftTimeFlag = 30;
        ControllerManager.Instance.GetController<SystemSettingController>().EnableEnemyInfo(true);
    }

    public void RetSpeedupFight_SC(uint during)
    {
        TipsWindow.ShowWindow(this.MSG_4003);
        this._competitionTime = during;
        if (this.uiScore != null)
        {
            this.uiScore.SetCompetition(1);
        }
    }

    public void RetFightFinish_SC(MSG_RetFightFinish_SC msg1)
    {
        if (this.uiScore != null)
        {
            this.uiScore.SetCompetition(2);
        }
        this.DisposeCompetitionData();
        Scheduler.Instance.AddTimer(msg1.duration - 2U, false, new Scheduler.OnScheduler(this.CloseScoreUI));
        ControllerManager.Instance.GetController<PVPMatchController>().pvpState = StageType.Finish;
        if (this.uiScore != null)
        {
            this.uiScore.SetWin(msg1.winteamid == GlobalRegister.GetTeamInfo().id, msg1.MeRankPKResult, msg1.EnemyRankPKResult);
            this.uiScore.SetAwards(msg1.rewards);
        }
        else
        {
            this.ShowScoreUI(delegate
            {
                this.uiScore.SetWin(msg1.winteamid == GlobalRegister.GetTeamInfo().id, msg1.MeRankPKResult, msg1.EnemyRankPKResult);
                this.uiScore.SetAwards(msg1.rewards);
            });
        }
    }

    public void RetTeamLeftMemSize_SC(MSG_RetTeamLeftMemSize_SC msg1)
    {
        this.teamLeftInfo = msg1;
        if (this.uiScore != null)
        {
            this.uiScore.SetTeamNumber(msg1.team1left, msg1.team2left);
        }
    }

    public void RetRankPKCurStage_SC(MSG_RetRankPKCurStage_SC msg1)
    {
        if (msg1.curstage == StageType.CountDown || msg1.curstage == StageType.Fight)
        {
            this.RetStartFight_SC(msg1.duration, msg1.score);
        }
    }

    private void CompetitionTimer()
    {
        if (this._competitionTime <= 0f)
        {
            return;
        }
        this._competitionTime -= 1f;
        this.CheckLeftTimeTip(this._competitionTime);
        if (this.uiScore != null)
        {
            this.uiScore.SetScoreTime(this._competitionTime);
        }
    }

    private void CheckLeftTimeTip(float competitionTime)
    {
        if (ControllerManager.Instance.GetController<PVPMatchController>().pvpState == StageType.Fight)
        {
            if (this._competitionTime < 30f && this.leftTimeFlag == 30)
            {
                this.leftTimeFlag = 20;
                TipsWindow.ShowWindow(this.MSG_4000);
            }
            else if (this._competitionTime < 20f && this.leftTimeFlag == 20)
            {
                this.leftTimeFlag = 10;
                TipsWindow.ShowWindow(this.MSG_4001);
            }
            else if (this._competitionTime < 10f && this.leftTimeFlag == 10)
            {
                this.leftTimeFlag = 0;
                TipsWindow.ShowWindow(this.MSG_4002);
            }
        }
    }

    public void RetPKGeneralConfig_SC(MSG_RetPKGeneralConfig_SC msg)
    {
        this.TeamNum = msg.teampknum;
    }

    public void DisposeCompetitionData()
    {
        this._prepareTime = 0f;
        this._competitionTime = 0f;
        this.TeamNum = 5U;
    }

    public void InitTeamData()
    {
        this._teamData.Clear();
        List<Memember> teamList = GlobalRegister.GetTeamList();
        if (teamList == null)
        {
            return;
        }
        for (int i = 0; i < teamList.Count; i++)
        {
            PVPCompetitionController.TeamData teamData = new PVPCompetitionController.TeamData();
            teamData.teamInfo = teamList[i];
            teamData.grading = 0U;
            teamData.bReady = false;
            if (teamData.teamInfo.mememberid == GlobalRegister.GetCharacterMapData(ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer).charid.ToString())
            {
                teamData.bLoaded = true;
            }
            else
            {
                teamData.bLoaded = false;
            }
            this._teamData.Add(teamData);
        }
    }

    public void InitHeroData()
    {
        this.OwnHeroDataList.Clear();
        object[] array = LuaScriptMgr.Instance.CallLuaFunction("HerosCtrl.GetOwnHeros", new object[0]);
        LuaTable luaTable = array[0] as LuaTable;
        foreach (object obj in luaTable.Values)
        {
            LuaTable item = obj as LuaTable;
            this.OwnHeroDataList.Add(item);
        }
    }

    public LuaTable GetHeroDataByHeroId(int heroId)
    {
        for (int i = 0; i < this.OwnHeroDataList.Count; i++)
        {
            if (this.OwnHeroDataList[i].GetField_Int("baseid") == heroId)
            {
                return this.OwnHeroDataList[i];
            }
        }
        return null;
    }

    private void EnableCompetitionDynamic(bool dynamicFlag)
    {
    }

    private void SetLineDynamicObstacle(Vector2 start, Vector2 end, bool dynamicFlag)
    {
        if (start.x == end.x)
        {
            Vector2 vector;
            Vector2 vector2;
            if (start.y > end.y)
            {
                vector = start;
                vector2 = end;
            }
            else
            {
                vector = end;
                vector2 = start;
            }
            for (int i = (int)vector2.y; i < (int)vector.y + 1; i++)
            {
                CellInfos.SetCellInfoDynamicObstacleFlag((uint)i, (uint)start.x, dynamicFlag);
            }
        }
        else
        {
            Vector2 vector3;
            Vector2 vector4;
            if (start.x > end.x)
            {
                vector3 = start;
                vector4 = end;
            }
            else
            {
                vector3 = end;
                vector4 = start;
            }
            for (int j = (int)vector4.x; j < (int)vector3.x + 1; j++)
            {
                double num = (double)(vector4.y - vector3.y) / (double)(vector4.x - vector3.x);
                double num2 = num * (double)((float)j - vector4.x) + (double)vector4.y;
                double num3 = num2 - (double)((int)num2);
                if (0.001 > num3 && num3 > -0.001)
                {
                    CellInfos.SetCellInfoDynamicObstacleFlag((uint)num2, (uint)j, dynamicFlag);
                }
            }
        }
    }

    public PVPCompetitionNetWork mNetWork;

    public MSG_RetTeamLeftMemSize_SC teamLeftInfo;

    public List<LuaTable> MyHero = new List<LuaTable>();

    private List<PVPCompetitionController.TeamData> _teamData = new List<PVPCompetitionController.TeamData>();

    private float _prepareTime;

    private float _competitionTime;

    public uint TeamNum;

    private uint MSG_4000 = 4000U;

    private uint MSG_4001 = 4001U;

    private uint MSG_4002 = 4002U;

    private uint MSG_4003 = 4003U;

    private int leftTimeFlag = 30;

    public List<LuaTable> OwnHeroDataList = new List<LuaTable>();

    public class TeamData
    {
        public uint grading;

        public bool bReady;

        public bool bLoaded;

        public Memember teamInfo;
    }
}
