using System;
using battle;
using Framework.Managers;
using Net;
using rankpk_msg;
using UnityEngine;

public class PVPMatchNetWork : NetWorkBase
{
    private PVPMatchController controller
    {
        get
        {
            if (this._controller == null)
            {
                this._controller = ControllerManager.Instance.GetController<PVPMatchController>();
            }
            return this._controller;
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetRankPKCurStage_SC>(CommandID.MSG_RetRankPKCurStage_SC, new ProtoMsgCallback<MSG_RetRankPKCurStage_SC>(this.RetRankPKCurStage_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MatchMemberInfo_SC>(CommandID.MSG_Ret_MatchMemberInfo_SC, new ProtoMsgCallback<MSG_Ret_MatchMemberInfo_SC>(this.Ret_MatchMemberInfo_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_StartMatch_SC>(CommandID.MSG_Ret_StartMatch_SC, new ProtoMsgCallback<MSG_Ret_StartMatch_SC>(this.Ret_StartMatch_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_CancelMatch_SC>(CommandID.MSG_Ret_CancelMatch_SC, new ProtoMsgCallback<MSG_Ret_CancelMatch_SC>(this.Ret_CancelMatch_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MatchResult_SC>(CommandID.MSG_Ret_MatchResult_SC, new ProtoMsgCallback<MSG_Ret_MatchResult_SC>(this.Ret_MatchResult_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetRewardsEveryday_SC>(CommandID.MSG_RetRewardsEveryday_SC, new ProtoMsgCallback<MSG_RetRewardsEveryday_SC>(this.RetRewardsEveryday_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetGetSeasonRewards_SC>(CommandID.MSG_RetGetSeasonRewards_SC, new ProtoMsgCallback<MSG_RetGetSeasonRewards_SC>(this.RetGetSeasonRewards_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RankPkReqPrepare_SC>(CommandID.MSG_RankPkReqPrepare_SC, new ProtoMsgCallback<MSG_RankPkReqPrepare_SC>(this.RankPkReqPrepare_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_GoToBattle_SC>(CommandID.MSG_GoToBattle_SC, new ProtoMsgCallback<MSG_GoToBattle_SC>(this.GoToBattle_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetRankPKList_SC>(CommandID.MSG_RetRankPKList_SC, new ProtoMsgCallback<MSG_RetRankPKList_SC>(this.RetRankPKList_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ReqBattleMatch_SC>(CommandID.MSG_ReqBattleMatch_SC, new ProtoMsgCallback<MSG_ReqBattleMatch_SC>(this.RetBattleMatch_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ReqBattleCancelMatch_SC>(CommandID.MSG_ReqBattleCancelMatch_SC, new ProtoMsgCallback<MSG_ReqBattleCancelMatch_SC>(this.RetBattleCancelMatch_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetMatchMember_SC>(CommandID.MSG_RetMatchMember_SC, new ProtoMsgCallback<MSG_RetMatchMember_SC>(this.RetMatchMember_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetBattleValid_SC>(CommandID.MSG_RetBattleValid_SC, new ProtoMsgCallback<MSG_RetBattleValid_SC>(this.RetBattleValid_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_retEnterBattle_SC>(CommandID.MSG_retEnterBattle_SC, new ProtoMsgCallback<MSG_retEnterBattle_SC>(this.RetEnterBattle_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetBattleTimes_SC>(CommandID.MSG_RetBattleTimes_SC, new ProtoMsgCallback<MSG_RetBattleTimes_SC>(this.RetBattleTimes_CS));
    }

    public void MSG_ReqRankPKCurStage_CS()
    {
        MSG_ReqRankPKCurStage_CS t = new MSG_ReqRankPKCurStage_CS();
        base.SendMsg<MSG_ReqRankPKCurStage_CS>(CommandID.MSG_ReqRankPKCurStage_CS, t, false);
    }

    public void Req_MatchMemberInfo_CS()
    {
        MSG_Req_MatchMemberInfo_CS t = new MSG_Req_MatchMemberInfo_CS();
        base.SendMsg<MSG_Req_MatchMemberInfo_CS>(CommandID.MSG_Req_MatchMemberInfo_CS, t, false);
    }

    public void Req_StartMatch_CS()
    {
        MSG_Req_StartMatch_CS t = new MSG_Req_StartMatch_CS();
        base.SendMsg<MSG_Req_StartMatch_CS>(CommandID.MSG_Req_StartMatch_CS, t, false);
    }

    public void Req_CancelMatch_CS()
    {
        MSG_Req_CancelMatch_CS t = new MSG_Req_CancelMatch_CS();
        base.SendMsg<MSG_Req_CancelMatch_CS>(CommandID.MSG_Req_CancelMatch_CS, t, false);
    }

    public void RankPkReqPrepare_CS()
    {
        MSG_RankPkReqPrepare_CS t = new MSG_RankPkReqPrepare_CS();
        base.SendMsg<MSG_RankPkReqPrepare_CS>(CommandID.MSG_RankPkReqPrepare_CS, t, false);
    }

    public void Req_RewardsEveryday_CS()
    {
        MSG_ReqRewardsEveryday_CS t = new MSG_ReqRewardsEveryday_CS();
        base.SendMsg<MSG_ReqRewardsEveryday_CS>(CommandID.MSG_ReqRewardsEveryday_CS, t, false);
    }

    public void ReqGetSeasonRewards_CS()
    {
        MSG_ReqGetSeasonRewards_CS t = new MSG_ReqGetSeasonRewards_CS();
        base.SendMsg<MSG_ReqGetSeasonRewards_CS>(CommandID.MSG_ReqGetSeasonRewards_CS, t, false);
    }

    public void Req_GotoBattle_CS()
    {
        MSG_Req_GotoBattle_CS t = new MSG_Req_GotoBattle_CS();
        base.SendMsg<MSG_Req_GotoBattle_CS>(CommandID.MSG_Req_GotoBattle_CS, t, false);
    }

    public void ReqRankPKList_CS(RankPKListType type)
    {
        base.SendMsg<MSG_ReqRankPKList_CS>(CommandID.MSG_ReqRankPKList_CS, new MSG_ReqRankPKList_CS
        {
            type = type
        }, false);
    }

    private void RetRankPKCurStage_SC(MSG_RetRankPKCurStage_SC msg)
    {
        this.controller.RetRankPKCurStage_SC(msg);
    }

    private void Ret_MatchMemberInfo_SC(MSG_Ret_MatchMemberInfo_SC msg)
    {
        this.controller.Ret_MatchMemberInfo_SC(msg);
    }

    private void Ret_StartMatch_SC(MSG_Ret_StartMatch_SC msg)
    {
        if (msg.retcode == 1U)
        {
            TipsWindow.ShowWindow(4039U);
            ControllerManager.Instance.GetController<PVPMatchController>().StartMatch();
        }
        else if (msg.retcode == 0U)
        {
            TipsWindow.ShowWindow(4064U);
        }
    }

    private void Ret_CancelMatch_SC(MSG_Ret_CancelMatch_SC msg2)
    {
        if (msg2.retcode == 1U)
        {
            ControllerManager.Instance.GetController<PVPMatchController>().CancelMatch();
        }
    }

    private void Ret_MatchResult_SC(MSG_Ret_MatchResult_SC msg)
    {
        Debug.Log(string.Concat(new object[]
        {
            "Ret_MatchResult_SC",
            msg.lefttime,
            " ",
            msg.totalnum
        }));
        if (msg.retcode == 1U)
        {
            ControllerManager.Instance.GetController<PVPMatchController>().totalNum = msg.totalnum;
            ControllerManager.Instance.GetController<PVPMatchController>().readDuaration = msg.lefttime;
            ControllerManager.Instance.GetController<PVPMatchController>().leftTime = msg.lefttime;
            ControllerManager.Instance.GetController<PVPMatchController>().readyCoolingdownStartTime = Time.realtimeSinceStartup;
            ControllerManager.Instance.GetController<PVPMatchController>().ResultMatch();
        }
    }

    private void RetRewardsEveryday_SC(MSG_RetRewardsEveryday_SC msg)
    {
    }

    private void RetGetSeasonRewards_SC(MSG_RetGetSeasonRewards_SC msg)
    {
    }

    private void RankPkReqPrepare_SC(MSG_RankPkReqPrepare_SC msg)
    {
    }

    private void GoToBattle_SC(MSG_GoToBattle_SC msg)
    {
        this._controller.GoToBattle_SC(msg.retcode);
    }

    private void RetRankPKList_SC(MSG_RetRankPKList_SC msg)
    {
        this._controller.RetRankPKList_SC(msg);
    }

    public void ReqBattleMatch(uint battleid)
    {
        base.SendMsg<MSG_ReqBattleMatch_CS>(CommandID.MSG_ReqBattleMatch_CS, new MSG_ReqBattleMatch_CS
        {
            battleId = battleid
        }, false);
    }

    private void RetBattleMatch_SC(MSG_ReqBattleMatch_SC data)
    {
        this.controller.RetBattleMatch(data.errorCode == BatteMatchCode.BATTLE_MATCH_SUCCESS);
        if (data.errorCode == BatteMatchCode.BATTLE_MATCH_SUCCESS)
        {
            ControllerManager.Instance.GetController<UIMapController>().RetBattleMatch((uint)data.averWaitTime);
        }
    }

    public void ReqBattleCancelMatch(uint battleid)
    {
        base.SendMsg<MSG_ReqBattleCancelMatch_CS>(CommandID.MSG_ReqBattleCancelMatch_CS, new MSG_ReqBattleCancelMatch_CS
        {
            battleId = battleid
        }, false);
    }

    private void RetBattleCancelMatch_SC(MSG_ReqBattleCancelMatch_SC data)
    {
        if (data.errorCode != CancelBatteMatchCode.BATTLE_CANCELMATCH_SUCCESS && data.errorCode != CancelBatteMatchCode.BATTLE_CANCELMATCH_INTER_FAILED)
        {
            return;
        }
        this.controller.RetBattleCancelMatch(data.errorCode == CancelBatteMatchCode.BATTLE_CANCELMATCH_SUCCESS);
        if (data.errorCode == CancelBatteMatchCode.BATTLE_CANCELMATCH_SUCCESS)
        {
            ControllerManager.Instance.GetController<UIMapController>().RetCancelBattleMatch();
        }
    }

    private void RetBattleValid_SC(MSG_RetBattleValid_SC mdata)
    {
        this.controller.RetBattleValid(mdata.endTimeStamp);
        ControllerManager.Instance.GetController<UIMapController>().CloseDuoqiBtnTip();
    }

    public void ReqEnterBattle()
    {
        MSG_ReqEnterBattle_CS t = new MSG_ReqEnterBattle_CS();
        base.SendMsg<MSG_ReqEnterBattle_CS>(CommandID.MSG_ReqEnterBattle_CS, t, false);
    }

    private void RetEnterBattle_SC(MSG_retEnterBattle_SC mdata)
    {
        if (mdata.errorCode == EnterBattleCode.BATTLE_ENTER_KICKED_FAILED)
        {
            this.controller.RetBattleCancelMatch(true);
            ControllerManager.Instance.GetController<UIMapController>().RetCancelBattleMatch();
        }
        else if (mdata.errorCode == EnterBattleCode.BATTLE_ENTER_SUCCESS)
        {
            this.controller.RetBattleCancelMatch(true);
            ControllerManager.Instance.GetController<UIMapController>().ShowBattlePanel();
            MainUIController mainUIController = ControllerManager.Instance.GetController<MainUIController>();
            UI_MainView mainView = mainUIController.mainView;
            if (mainView != null)
            {
                mainView.ShowGroupTeam();
            }
            else
            {
                MainUIController mainUIController2 = mainUIController;
                mainUIController2.onMainShow = (Action)Delegate.Combine(mainUIController2.onMainShow, new Action(delegate ()
                {
                    mainUIController.mainView.ShowGroupTeam();
                }));
            }
        }
    }

    private void RetMatchMember_SC(MSG_RetMatchMember_SC data)
    {
        this.controller.RetMatchMember(data.members);
    }

    public void ReqBattleTimes(uint battleid)
    {
        base.SendMsg<MSG_ReqBattleTimes_CS>(CommandID.MSG_ReqBattleTimes_CS, new MSG_ReqBattleTimes_CS
        {
            battleId = battleid
        }, false);
    }

    private void RetBattleTimes_CS(MSG_RetBattleTimes_SC data)
    {
        this.controller.RetBattleTimes(data.winBattleTimes);
    }

    private PVPMatchController _controller;
}
