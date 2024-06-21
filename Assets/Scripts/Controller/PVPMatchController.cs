using System;
using System.Collections.Generic;
using battle;
using Framework.Managers;
using Game.Scene;
using Models;
using msg;
using rankpk_msg;
using Team;
using UnityEngine;

internal class PVPMatchController : ControllerBase
{
    public UI_PVPMatch pvpMatch
    {
        get
        {
            return UIManager.GetUIObject<UI_PVPMatch>();
        }
    }

    public UI_MatchComplete matchComplete
    {
        get
        {
            return UIManager.GetUIObject<UI_MatchComplete>();
        }
    }

    public float leftTime
    {
        get
        {
            return this._leftTime;
        }
        set
        {
            this._leftTime = value;
        }
    }

    public float totalNum
    {
        get
        {
            return this._totalNum;
        }
        set
        {
            this._totalNum = value;
        }
    }

    public StageType pvpState
    {
        get
        {
            return this._pvpState;
        }
        set
        {
            this._pvpState = value;
        }
    }

    public override string ControllerName
    {
        get
        {
            return "pvpmathch_controller";
        }
    }

    public void Reset()
    {
        this.prepareInfo = null;
        this._pvpState = StageType.None_Stage;
        this.mIsInMathingState = false;
    }

    public override void OnDestroy()
    {
        this.Reset();
        base.OnDestroy();
    }

    public void ShowUI()
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.isAbattoirScene)
        {
            TipsWindow.ShowWindow(5001U);
            return;
        }
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_PVPMatch>("UI_PVPMatch", delegate ()
        {
            if (this.pvpMatch != null)
            {
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public void ShowMatchCompleteUI()
    {
        if (ControllerManager.Instance.GetController<PVPMatchController>().pvpState != StageType.Prepare)
        {
            return;
        }
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_MatchComplete>("UI_MatchComplete", delegate ()
        {
            if (this.matchComplete != null)
            {
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public override void Awake()
    {
        this.mNetWork = new PVPMatchNetWork();
        this.mNetWork.Initialize();
        this._pvpState = StageType.None_Stage;
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.OnOpenPVPMatch));
    }

    public override void OnUpdate()
    {
        if (this._leftTime > 0f)
        {
            this._leftTime = this.readDuaration - (Time.realtimeSinceStartup - this.readyCoolingdownStartTime);
        }
        if (this.matchComplete != null)
        {
            this.matchComplete.MatchCompleteUpdate();
        }
    }

    public void CloseUI()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_PVPMatch");
    }

    public void CloseMatchCompleteUI()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_MatchComplete");
    }

    public void Ret_MatchMemberInfo_SC(MSG_Ret_MatchMemberInfo_SC msg)
    {
        for (int i = 0; i < msg.members.Count; i++)
        {
            if (this.pvpMatch != null && (ulong)msg.members[i].charid == MainPlayer.Self.OtherPlayerData.MapUserData.charid)
            {
                this.pvpMatch.InitRoleInfo(msg.members[i], msg);
                break;
            }
        }
    }

    public void RetRankPKCurStage_SC(MSG_RetRankPKCurStage_SC msg)
    {
        if (this.pvpMatch != null)
        {
            this.pvpMatch.InitPvpState(msg);
        }
    }

    public void StartMatch()
    {
        this._pvpState = StageType.Match;
        if (this.pvpMatch != null)
        {
            this.pvpMatch.SwitchState(this._pvpState);
        }
        this.matchStartTime = Time.realtimeSinceStartup;
        ControllerManager.Instance.GetController<MainUIController>().OpenStartMatchBox(this.matchStartTime, PvpMatchType.Wudaohui);
    }

    public void CancelMatch()
    {
        this._pvpState = StageType.None_Stage;
        if (this.pvpMatch != null)
        {
            this.pvpMatch.SwitchState(this._pvpState);
        }
        ControllerManager.Instance.GetController<MainUIController>().CloseMatchBox();
    }

    public void ResultMatch()
    {
        this._pvpState = StageType.Prepare;
        if (this.pvpMatch != null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_PVPMatch");
        }
        this.ShowMatchCompleteUI();
    }

    public void OpenMatchCountDown(MSG_RankPkReqPrepare_SC info)
    {
        this.prepareInfo = info;
        Debug.Log(string.Concat(new object[]
        {
            "OpenMatchCountDown",
            info.readystate,
            " ",
            info.readynum,
            " ",
            info.totalnum,
            " ",
            info.lefttime,
            " ",
            info.enemyreadynum,
            " ",
            info.enemytotalnum
        }));
        if (this.matchComplete != null)
        {
            this.matchComplete.UpdateMatchInfo(info);
        }
    }

    public void GoToBattle_SC(uint retcode)
    {
        this.prepareInfo = null;
        if (retcode == 0U)
        {
            this._pvpState = StageType.CountDown;
        }
        else
        {
            this.CancelMatch();
            this.mNetWork.Req_CancelMatch_CS();
        }
        if (this.matchComplete != null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_MatchComplete");
        }
        if (this.pvpMatch != null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_PVPMatch");
        }
    }

    public void OnOpenPVPMatch(List<VarType> varParams)
    {
        this.ShowUI();
    }

    public void RetRankPKList_SC(MSG_RetRankPKList_SC msg)
    {
        if (this.pvpMatch != null)
        {
            this.pvpMatch.InitRankInfo(msg);
        }
    }

    public void ReqBattleMatch(uint battleid)
    {
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        if (controller.getState != AbattoirMatchState.None)
        {
            TipsWindow.ShowWindow(4042U);
            if (controller.getState == AbattoirMatchState.Matching && this.pvpMatch != null)
            {
                this.pvpMatch.SetBattleBtnMatchingState(true);
            }
            return;
        }
        if (this.pvpState != StageType.None_Stage)
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
        this.mNetWork.ReqBattleMatch(battleid);
    }

    public void RetBattleMatch(bool issucc)
    {
        if (this.pvpMatch != null)
        {
            this.pvpMatch.RetBattleMatchCb(issucc);
        }
        if (issucc)
        {
            this.mIsInMathingState = true;
        }
    }

    public void ReqBattleCancelMatch(uint battleid)
    {
        this.mNetWork.ReqBattleCancelMatch(battleid);
    }

    public void RetBattleCancelMatch(bool issucc)
    {
        if (this.pvpMatch != null)
        {
            this.pvpMatch.RetBattleMatchCancelCb(issucc);
        }
        if (issucc)
        {
            this.mIsInMathingState = false;
        }
    }

    public void RetBattleValid(uint endtime)
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.DoEnterCountDown));
        this.mEnterSecond = endtime - SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond();
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox("提示", string.Format(CommonTools.GetTextById(4062UL), this.mEnterSecond), "确定", "取消", UIManager.ParentType.Tips, new Action(this.EnterBattle), delegate ()
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.DoEnterCountDown));
        }, null);
        Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.DoEnterCountDown));
    }

    private void DoEnterCountDown()
    {
        this.mEnterSecond -= 1U;
        if (this.mEnterSecond < 1U)
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.DoEnterCountDown));
            ControllerManager.Instance.GetController<MsgBoxController>().CloseMsgBox();
            return;
        }
        UI_MsgBox msgBox = ControllerManager.Instance.GetController<MsgBoxController>().msgBox;
        if (msgBox != null)
        {
            msgBox.RefreshDescrible(string.Format(CommonTools.GetTextById(4062UL), this.mEnterSecond));
        }
    }

    private void EnterBattle()
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.DoEnterCountDown));
        this.mNetWork.ReqEnterBattle();
    }

    public void RetMatchMember(List<MatchMember> memList)
    {
        for (int i = 0; i < memList.Count; i++)
        {
            if (memList[i].userid == MainPlayer.Self.BaseData.BaseData.id)
            {
                ControllerManager.Instance.GetController<DuoQiController>().RefreshMyCampId(memList[i].camp);
                break;
            }
        }
        MainUIController mainUIController = ControllerManager.Instance.GetController<MainUIController>();
        UI_MainView mainView = mainUIController.mainView;
        if (mainView != null)
        {
            mainView.SteupGroupTeam(memList);
        }
        else
        {
            MainUIController mainUIController2 = mainUIController;
            mainUIController2.onMainShow = (Action)Delegate.Combine(mainUIController2.onMainShow, new Action(delegate ()
            {
                mainUIController.mainView.SteupGroupTeam(memList);
            }));
        }
    }

    public void RefreshGroupItemHp(MSG_updateTeamMememberHp_SC data)
    {
        MainUIController mainUIController = ControllerManager.Instance.GetController<MainUIController>();
        UI_MainView mainView = mainUIController.mainView;
        if (mainView != null)
        {
            mainView.RefreshGroupItemHp(data);
        }
        else
        {
            MainUIController mainUIController2 = mainUIController;
            mainUIController2.onMainShow = (Action)Delegate.Combine(mainUIController2.onMainShow, new Action(delegate ()
            {
                mainUIController.mainView.RefreshGroupItemHp(data);
            }));
        }
    }

    public void ReqBattleTimes()
    {
        this.mNetWork.ReqBattleTimes(1U);
    }

    public void RetBattleTimes(uint times)
    {
        if (this.pvpMatch != null)
        {
            this.pvpMatch.RetBattleTimes(times);
        }
    }

    public void WudaohuiStartMatch()
    {
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        if (controller.getState != AbattoirMatchState.None)
        {
            TipsWindow.ShowWindow(4042U);
            return;
        }
        if (this.mIsInMathingState)
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
        this.mNetWork.Req_StartMatch_CS();
    }

    public void WudaohuiCancelMatch()
    {
        this.mNetWork.Req_CancelMatch_CS();
    }

    public PVPMatchNetWork mNetWork;

    private float matchStartTime;

    public bool mIsInMathingState;

    public MSG_RankPkReqPrepare_SC prepareInfo;

    public float readDuaration;

    public float readyCoolingdownStartTime;

    private float _leftTime;

    private float _totalNum;

    private StageType _pvpState;

    private uint mEnterSecond;
}
