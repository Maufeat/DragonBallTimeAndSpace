using System;
using battle;
using Framework.Managers;
using Net;
using Team;

public class TeamNetWork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_TeamMemeberList_SC>(60110, new ProtoMsgCallback<MSG_TeamMemeberList_SC>(this.OnReceiveTeamMemberList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Team_List_SC>(60112, new ProtoMsgCallback<MSG_Team_List_SC>(this.OnReceiveTeamList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Join_Team_SC>(60114, new ProtoMsgCallback<MSG_Join_Team_SC>(this.OnJoinTmeaRet));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ReqJoinTeamNotifyLeader_SC>(60115, new ProtoMsgCallback<MSG_ReqJoinTeamNotifyLeader_SC>(this.RetJoinTeamNotifyLeader_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_AnswerJoinTeam_SC>(60117, new ProtoMsgCallback<MSG_AnswerJoinTeam_SC>(this.AnswerJoinTeam_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ReqApplyList_SC>(60119, new ProtoMsgCallback<MSG_ReqApplyList_SC>(this.RetApplyList_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ReqDelMemember_SC>(60121, new ProtoMsgCallback<MSG_ReqDelMemember_SC>(this.RetDelMemember_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ReqChangeLeader_SC>(60126, new ProtoMsgCallback<MSG_ReqChangeLeader_SC>(this.RetChangeLeader));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_AddMemember_SC>(2482, new ProtoMsgCallback<MSG_AddMemember_SC>(this.AddMember));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ReqLanchVoteOut_SC>(60123, new ProtoMsgCallback<MSG_ReqLanchVoteOut_SC>(this.RetVoteOut_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ReqNearByUnteamedPlayer_SC>(2481, new ProtoMsgCallback<MSG_ReqNearByUnteamedPlayer_SC>(this.RetNearByPlayer_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_InviteIntoTeam_SC>(60128, new ProtoMsgCallback<MSG_InviteIntoTeam_SC>(this.RetInviteIntoTeam_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_updateTeamMememberHp_SC>(2483, new ProtoMsgCallback<MSG_updateTeamMememberHp_SC>(this.UpdateTeamMenberHp));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_updateTeamMememberCareer_SC>(2484, new ProtoMsgCallback<MSG_updateTeamMememberCareer_SC>(this.RetUpDataTeamMember));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetNearByUnteamedInvite_SC>(60131, new ProtoMsgCallback<MSG_RetNearByUnteamedInvite_SC>(this.RetUnteamedInvite_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetTeamMemberPos_SC>(CommandID.MSG_RetTeamMemberPos_SC, new ProtoMsgCallback<MSG_RetTeamMemberPos_SC>(this.HandletTeamMemberPos));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetNewApply_SC>(2490, new ProtoMsgCallback<MSG_RetNewApply_SC>(this.RetNewApply));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_updateTeamMememberLevel_SC>(2486, new ProtoMsgCallback<MSG_updateTeamMememberLevel_SC>(this.UpDateTeamMemberLevel));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_updateTeamMemberFight_SC>(2487, new ProtoMsgCallback<MSG_updateTeamMemberFight_SC>(this.UpDateTeamMemberFight));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_updateTeamMemberAvatar_SC>(2575, new ProtoMsgCallback<MSG_updateTeamMemberAvatar_SC>(this.UpDateTeamMemberAvatar));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetTeamPublicDrop_SC>(2491, new ProtoMsgCallback<MSG_RetTeamPublicDrop_SC>(this.RetTeamPublicDrop));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetChooseTeamDrop_SC>(2493, new ProtoMsgCallback<MSG_RetChooseTeamDrop_SC>(this.RetChooseTeamDrop));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_updateTeamMemeberHero_SC>(2485, new ProtoMsgCallback<MSG_updateTeamMemeberHero_SC>(this.UpDateTeamMemberHero));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetSearchTeam_SC>(60134, new ProtoMsgCallback<MSG_RetSearchTeam_SC>(this.MSG_RetSearchTeam_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_updateMemStateToTeam_SC>(2494, new ProtoMsgCallback<MSG_updateMemStateToTeam_SC>(this.UpdateMemStateToTeam_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetMemberBackTeam_SC>(60143, new ProtoMsgCallback<MSG_RetMemberBackTeam_SC>(this.RetMemberBackTeam_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetLeaderMapPos_SC>(2496, new ProtoMsgCallback<MSG_RetLeaderMapPos_SC>(this.RetLeaderMapPos_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetChangeMapToLeader_SC>(2498, new ProtoMsgCallback<MSG_RetChangeMapToLeader_SC>(this.RetChangeMapToLeader_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetLeaderAttackTarget_SC>(2500, new ProtoMsgCallback<MSG_RetLeaderAttackTarget_SC>(this.RetLeaderAttackTarget_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_checkUserTeamInfo_SC>(2504, new ProtoMsgCallback<MSG_Ret_checkUserTeamInfo_SC>(this.RetCheckUserTeamInfo_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_updateTeamMemberPrivilege_SC>(2503, new ProtoMsgCallback<MSG_updateTeamMemberPrivilege_SC>(this.RetUpdateTeamMemberPrivilege_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_NotifyTeamDismiss_SC>(2505, new ProtoMsgCallback<MSG_NotifyTeamDismiss_SC>(this.RetNotifyTeamDismiss_SC));
    }

    public void UpDateTeamMemberHero(MSG_updateTeamMemeberHero_SC data)
    {
        ControllerManager.Instance.GetController<TeamController>().UpDateTeamMemberHero(data);
    }

    public void RetUpDataTeamMember(MSG_updateTeamMememberCareer_SC changeLeader)
    {
        ControllerManager.Instance.GetController<TeamController>().UpDateTeamMember(changeLeader);
    }

    public void UpdateTeamMenberHp(MSG_updateTeamMememberHp_SC memberData)
    {
        ControllerManager.Instance.GetController<TeamController>().RefreshTeamMemberHP(memberData);
    }

    public void ReqNearByPlayer()
    {
        base.SendMsg<MSG_ReqNearByUnteamedPlayer_CS>(CommandID.MSG_ReqNearByUnteamedPlayer_CS, new MSG_ReqNearByUnteamedPlayer_CS(), false);
    }

    public void AnswerInviteTeam_CS(bool b, string inviteeid, string inviterid, uint teamid)
    {
        base.SendMsg<MSG_AnswerInviteTeam_CS>(CommandID.MSG_AnswerInviteTeam_CS, new MSG_AnswerInviteTeam_CS
        {
            yesorno = b,
            inviteeid = inviteeid,
            inviterid = inviterid,
            teamid = teamid
        }, false);
    }

    public void RetNearByPlayer_SC(MSG_ReqNearByUnteamedPlayer_SC nearbyMember)
    {
        FFDebug.Log(this, FFLogType.Team, " MSG_ReqNearByUnteamedPlayer_SC ...... count = " + nearbyMember.mem.Count);
        ControllerManager.Instance.GetController<TeamController>().RetNearByPlayer_SC(nearbyMember);
    }

    public void ReqInviteIntoTeam_CS(string id)
    {
        base.SendMsg<MSG_InviteIntoTeam_CS>(CommandID.MSG_InviteIntoTeam_CS, new MSG_InviteIntoTeam_CS
        {
            inviteeid = id
        }, false);
    }

    public void RetInviteIntoTeam_SC(MSG_InviteIntoTeam_SC data)
    {
        if (data.errcode == 1U)
        {
            TipsWindow.ShowWindow(TipsType.TEAM_FULL, null);
            return;
        }
        if (data.errcode == 2U)
        {
            TipsWindow.ShowWindow(TipsType.INVITE_PLAYER_HAVE_TEAM, null);
        }
        else
        {
            TipsWindow.ShowWindow(TipsType.INVITE_SUCCESS_WAIT_ANSWER, null);
        }
    }

    public void AddMember(MSG_AddMemember_SC addMember)
    {
        ControllerManager.Instance.GetController<TeamController>().Addmember(addMember.mem);
    }

    public void RetChangeLeader(MSG_ReqChangeLeader_SC changeLeader)
    {
        ControllerManager.Instance.GetController<TeamController>().RetChangeLeader(changeLeader);
    }

    public void ReqChangeLeader(string id)
    {
        if (ControllerManager.Instance.GetController<DuoQiController>().InBattleState())
        {
            base.SendMsg<MSG_ReqChangeGroupLeader_CS>(CommandID.MSG_ReqChangeGroupLeader_CS, new MSG_ReqChangeGroupLeader_CS
            {
                newCaptain = ulong.Parse(id)
            }, false);
        }
        else
        {
            base.SendMsg<MSG_ReqChangeLeader_CS>(CommandID.MSG_ReqChangeLeader_CS, new MSG_ReqChangeLeader_CS
            {
                toid = id
            }, false);
        }
    }

    public void CreateTeam(MSG_CreateTeam_CS teamInfo)
    {
        base.SendMsg<MSG_CreateTeam_CS>(CommandID.MSG_CreateTeam_CS, teamInfo, false);
    }

    public void ApplyTeam(uint teamId)
    {
        base.SendMsg<MSG_Join_Team_CS>(CommandID.MSG_Join_Team_CS, new MSG_Join_Team_CS
        {
            teamid = teamId
        }, false);
    }

    public void TeamMemberList_CS()
    {
        base.SendMsg<MSG_TeamMemeberList_CS>(CommandID.MSG_TeamMemeberList_CS, new MSG_TeamMemeberList_CS(), false);
    }

    public void OnReceiveTeamMemberList(MSG_TeamMemeberList_SC teamData)
    {
        ControllerManager.Instance.GetController<TeamController>().SetTeamInfo(teamData);
    }

    public void OnJoinTmeaRet(MSG_Join_Team_SC data)
    {
        ControllerManager.Instance.GetController<TeamController>().RetJoinTeam(data.retcode);
    }

    public void OnReceiveTeamList(MSG_Team_List_SC teamList)
    {
    }

    public void RetJoinTeamNotifyLeader_SC(MSG_ReqJoinTeamNotifyLeader_SC memData)
    {
        ControllerManager.Instance.GetController<TeamController>().JoinTeamNotifyLeader(memData);
    }

    public void AnswerJoinTeam_SC(MSG_AnswerJoinTeam_SC teamList)
    {
        if (teamList.errcode == 1U)
        {
            TipsWindow.ShowWindow(TipsType.INVITE_PLAYER_HAVE_TEAM, null);
        }
        ControllerManager.Instance.GetController<TeamController>().AnswerJointeamList(teamList);
    }

    public void RetApplyList_SC(MSG_ReqApplyList_SC teamData)
    {
        ControllerManager.Instance.GetController<TeamController>().RetApplyList(teamData);
    }

    public void ReqApplyList()
    {
        base.SendMsg<MSG_ReqApplyList_CS>(CommandID.MSG_ReqApplyList_CS, new MSG_ReqApplyList_CS(), false);
    }

    public void ReqDelMember(string memid)
    {
        base.SendMsg<MSG_ReqDelMemember_CS>(CommandID.MSG_ReqDelMemember_CS, new MSG_ReqDelMemember_CS
        {
            charid = memid
        }, false);
    }

    public void RetDelMemember_SC(MSG_ReqDelMemember_SC teamData)
    {
        ControllerManager.Instance.GetController<TeamController>().RetDelMemember_SC(teamData);
    }

    public void ReqTeamList()
    {
        base.SendMsg<MSG_Team_List_CS>(CommandID.MSG_Team_List_CS, new MSG_Team_List_CS(), false);
    }

    public void ApplyAnswer(MSG_AnswerJoinTeam_CS tmpdata)
    {
        base.SendMsg<MSG_AnswerJoinTeam_CS>(CommandID.MSG_AnswerJoinTeam_CS, tmpdata, false);
    }

    public void RetVoteOut_SC(MSG_ReqLanchVoteOut_SC data)
    {
        if (data.errcode == 1U)
        {
            TipsWindow.ShowWindow(TipsType.CANNOT_REPEAT_VOTE, null);
            return;
        }
        ControllerManager.Instance.GetController<TeamController>().RetVoteOut_SC(data);
    }

    public void ReqVoteOut_CS(string id)
    {
        base.SendMsg<MSG_ReqLanchVoteOut_CS>(CommandID.MSG_ReqLanchVoteOut_CS, new MSG_ReqLanchVoteOut_CS
        {
            outid = id
        }, false);
    }

    public void ReqVoteTeamMate(bool bout)
    {
        base.SendMsg<MSG_ReqVote_CS>(CommandID.MSG_ReqVote_CS, new MSG_ReqVote_CS
        {
            yesorno = bout
        }, false);
    }

    public void ReqTeamMemberPos()
    {
        base.SendMsg<MSG_ReqTeamMemberPos_CS>(CommandID.MSG_ReqTeamMemberPos_CS, new MSG_ReqTeamMemberPos_CS(), false);
    }

    public void HandletTeamMemberPos(MSG_RetTeamMemberPos_SC data)
    {
        ControllerManager.Instance.GetController<TeamController>().GetTeamMemberPos(data.members);
    }

    public void leaderIgnoNotice()
    {
        base.SendMsg<MSG_LeaderIgnoreNotice_CS>(CommandID.MSG_LeaderIgnoreNotice_CS, new MSG_LeaderIgnoreNotice_CS(), false);
    }

    public void RetUnteamedInvite_SC(MSG_RetNearByUnteamedInvite_SC teamData)
    {
        ControllerManager.Instance.GetController<TeamController>().RetInviteIntoTeam_SC(teamData);
    }

    public void RetNewApply(MSG_RetNewApply_SC teamData)
    {
        ControllerManager.Instance.GetController<TeamController>().RetNewApply((int)teamData.count);
    }

    public void UpDateTeamMemberLevel(MSG_updateTeamMememberLevel_SC teamData)
    {
        ControllerManager.Instance.GetController<TeamController>().UpDateTeamMemberLevel(teamData);
    }

    public void UpDateTeamMemberFight(MSG_updateTeamMemberFight_SC teamData)
    {
        ControllerManager.Instance.GetController<TeamController>().UpDateTeamMemberFight(teamData);
    }

    public void UpDateTeamMemberAvatar(MSG_updateTeamMemberAvatar_SC teamData)
    {
        ControllerManager.Instance.GetController<TeamController>().UpDateTeamMemberAvatar(teamData);
    }

    public void RetTeamPublicDrop(MSG_RetTeamPublicDrop_SC teamDrop)
    {
        if (teamDrop.oneitem.Count > 0)
        {
            ControllerManager.Instance.GetController<TeamController>().RefreshDropItemData(teamDrop.oneitem);
        }
    }

    public void ReqChooseTeamDrop(string thisid, ChooseType type)
    {
        MSG_ReqChooseTeamDrop_CS msg_ReqChooseTeamDrop_CS = new MSG_ReqChooseTeamDrop_CS();
        ChooseTeamDropItem chooseTeamDropItem = new ChooseTeamDropItem();
        chooseTeamDropItem.thisid = thisid;
        chooseTeamDropItem.choose = type;
        msg_ReqChooseTeamDrop_CS.item.Add(chooseTeamDropItem);
        base.SendMsg<MSG_ReqChooseTeamDrop_CS>(CommandID.MSG_ReqChooseTeamDrop_CS, msg_ReqChooseTeamDrop_CS, false);
    }

    public void RetChooseTeamDrop(MSG_RetChooseTeamDrop_SC data)
    {
        for (int i = 0; i < data.item.Count; i++)
        {
            ControllerManager.Instance.GetController<TeamController>().ProcessChooseTeamDrop(data.item[i].thisid);
            if (data.item[i].errcode != 0U)
            {
                FFDebug.LogWarning(this, "RetChooseTeamDrop Failed code:" + data.item[i].errcode);
            }
        }
    }

    public void MSG_ReqSearchTeamByPage_CS(uint targetPage, bool isNearby)
    {
        FFDebug.Log(this, FFLogType.Team, string.Concat(new object[]
        {
            "MSG_ReqSearchTeamByPage_CS page = ",
            targetPage,
            " isnearby ",
            isNearby
        }));
        base.SendMsg<MSG_ReqSearchTeamByPage_CS>(CommandID.MSG_ReqSearchTeamByPage_CS, new MSG_ReqSearchTeamByPage_CS
        {
            page = targetPage,
            nearby = isNearby
        }, false);
    }

    public void MSG_ReqSearchTeam_CS(uint teamID, uint activityID)
    {
        FFDebug.Log(this, FFLogType.Team, string.Concat(new object[]
        {
            "MSG_ReqSearchTeam_CS id = ",
            teamID,
            " activityid = ",
            activityID
        }));
        base.SendMsg<MSG_ReqSearchTeam_CS>(CommandID.MSG_ReqSearchTeam_CS, new MSG_ReqSearchTeam_CS
        {
            teamid = teamID,
            activityid = activityID
        }, false);
    }

    public void MSG_RetSearchTeam_SC(MSG_RetSearchTeam_SC msg)
    {
        FFDebug.Log(this, FFLogType.Team, string.Concat(new object[]
        {
            " MSG_RetSearchTeam_SC page = ",
            msg.page,
            " msg.teamList.count ",
            msg.teamlist.Count
        }));
        ControllerManager.Instance.GetController<TeamController>().RetSearchTeam(msg);
    }

    public void UpdateMemStateToTeam_SC(MSG_updateMemStateToTeam_SC msg)
    {
        ControllerManager.Instance.GetController<TeamController>().UpdateMemStateToTeam_SC(msg);
    }

    public void RetMemberBackTeam_SC(MSG_RetMemberBackTeam_SC msg)
    {
        ControllerManager.Instance.GetController<TeamController>().RetMemberBackTeam_SC(msg.rettype);
    }

    public void ReqMemberBackTeam_CS()
    {
        MSG_ReqMemberBackTeam_CS t = new MSG_ReqMemberBackTeam_CS();
        base.SendMsg<MSG_ReqMemberBackTeam_CS>(CommandID.MSG_ReqMemberBackTeam_CS, t, false);
    }

    public void MSG_ReqLeaderMapPos_CS()
    {
        MSG_ReqLeaderMapPos_CS t = new MSG_ReqLeaderMapPos_CS();
        base.SendMsg<MSG_ReqLeaderMapPos_CS>(CommandID.MSG_ReqLeaderMapPos_CS, t, false);
    }

    public void MSG_ReqLeaderAttackTarget_CS()
    {
        MSG_ReqLeaderAttackTarget_CS t = new MSG_ReqLeaderAttackTarget_CS();
        base.SendMsg<MSG_ReqLeaderAttackTarget_CS>(CommandID.MSG_ReqLeaderAttackTarget_CS, t, false);
    }

    public void MSG_ReqChangeMapToLeader_CS(string sceneidstr, MemberPos leaderpos)
    {
        base.SendMsg<MSG_ReqChangeMapToLeader_CS>(CommandID.MSG_ReqChangeMapToLeader_CS, new MSG_ReqChangeMapToLeader_CS
        {
            sceneid = sceneidstr,
            leaderpos = leaderpos
        }, false);
    }

    public void RetLeaderMapPos_SC(MSG_RetLeaderMapPos_SC Data)
    {
        ControllerManager.Instance.GetController<TeamController>().RetLeaderMapPos_SC(Data);
    }

    public void RetChangeMapToLeader_SC(MSG_RetChangeMapToLeader_SC Data)
    {
        ControllerManager.Instance.GetController<TeamController>().RetChangeMapToLeader_SC(Data);
    }

    public void RetLeaderAttackTarget_SC(MSG_RetLeaderAttackTarget_SC Data)
    {
        ControllerManager.Instance.GetController<TeamController>().RetLeaderAttackTarget_SC(Data);
    }

    private TeamController mController
    {
        get
        {
            return ControllerManager.Instance.GetController<TeamController>();
        }
    }

    public void CheckUserTeamInfo(ulong memberid)
    {
        base.SendMsg<MSG_Req_checkUserTeamInfo_CS>(CommandID.MSG_Req_checkUserTeamInfo_CS, new MSG_Req_checkUserTeamInfo_CS
        {
            memberid = memberid
        }, false);
    }

    private void RetCheckUserTeamInfo_SC(MSG_Ret_checkUserTeamInfo_SC data)
    {
        this.mController.SearchTeamidByMemberidCb(data.teamid, data.online);
    }

    public void UpdateTeamMemberPrivilege(ulong memberid, TeamPrivilege teamPrivilege, bool set)
    {
        base.SendMsg<MSG_Req_SetMemberPrivilege_CS>(CommandID.MSG_Req_SetMemberPrivilege_CS, new MSG_Req_SetMemberPrivilege_CS
        {
            memberid = memberid,
            privilege = teamPrivilege,
            set = set
        }, false);
    }

    private void RetUpdateTeamMemberPrivilege_SC(MSG_updateTeamMemberPrivilege_SC data)
    {
        this.mController.UpdateTeamMemberPrivilegeCb(data.memberid, data.privilege);
    }

    public void DismissTeam()
    {
        MSG_Req_DismissTeam_CS t = new MSG_Req_DismissTeam_CS();
        base.SendMsg<MSG_Req_DismissTeam_CS>(CommandID.MSG_Req_DismissTeam_CS, t, false);
    }

    private void RetNotifyTeamDismiss_SC(MSG_NotifyTeamDismiss_SC data)
    {
        this.mController.DismissTeamCb(data.suc);
    }
}
