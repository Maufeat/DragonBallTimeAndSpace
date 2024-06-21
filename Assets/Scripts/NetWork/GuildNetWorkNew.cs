using System;
using System.Collections.Generic;
using copymap;
using Framework.Managers;
using guild;
using guildpk_msg;
using Net;

public class GuildNetWorkNew : NetWorkBase
{
    private GuildControllerNew mController
    {
        get
        {
            return ControllerManager.Instance.GetController<GuildControllerNew>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_CreateGuild_SC>(60050, new ProtoMsgCallback<MSG_Ret_CreateGuild_SC>(this.OnRetCreatGuild));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildInvite_SC>(2153, new ProtoMsgCallback<MSG_Ret_GuildInvite_SC>(this.OnRetGuildInvite));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildInviteConfirm_SC>(2154, new ProtoMsgCallback<MSG_Ret_GuildInviteConfirm_SC>(this.OnRetGuildInviteConfirm));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildList_SC>(60056, new ProtoMsgCallback<MSG_Ret_GuildList_SC>(this.OnRetGuildList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_ApplyForGuild_SC>(60058, new ProtoMsgCallback<MSG_Ret_ApplyForGuild_SC>(this.OnRetApplyGuild));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildInfo_SC>(60054, new ProtoMsgCallback<MSG_Ret_GuildInfo_SC>(this.GuildInfoCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildMemberList_SC>(60062, new ProtoMsgCallback<MSG_Ret_GuildMemberList_SC>(this.GuildMemeberListCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_QuitGuild_SC>(2151, new ProtoMsgCallback<MSG_Ret_QuitGuild_SC>(this.ExitGuildCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_SetGuildNotify_SC>(60052, new ProtoMsgCallback<MSG_Ret_SetGuildNotify_SC>(this.SetGuildNotifyCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ReqUserCntData_CSC>(2179, new ProtoMsgCallback<MSG_ReqUserCntData_CSC>(this.UserCntDataCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_PositionInfo_SC>(60075, new ProtoMsgCallback<MSG_Ret_PositionInfo_SC>(this.PositionInfoCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_ChangeGuildMaster_SC>(2156, new ProtoMsgCallback<MSG_Ret_ChangeGuildMaster_SC>(this.ChangeGuildMasterCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_FireGuildMember_SC>(2152, new ProtoMsgCallback<MSG_Ret_FireGuildMember_SC>(this.FireGuildMemberCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_AnswerApplyForGuild_SC>(60060, new ProtoMsgCallback<MSG_Ret_AnswerApplyForGuild_SC>(this.AnswerApplyForGuildCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MyGuildApply_Result_SC>(60149, new ProtoMsgCallback<MSG_Ret_MyGuildApply_Result_SC>(this.MyGuildApplyResultCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildSkill_SC>(60161, new ProtoMsgCallback<MSG_Ret_GuildSkill_SC>(this.RetGuildSkillList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Req_LearnGuildSkill_CSC>(60162, new ProtoMsgCallback<MSG_Req_LearnGuildSkill_CSC>(this.OnMSG_Req_LearnGuildSkill_CSC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildLevelup_SC>(60077, new ProtoMsgCallback<MSG_Ret_GuildLevelup_SC>(this.MSG_Ret_GuildLevelup_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildPkInfo_SC>(60152, new ProtoMsgCallback<MSG_Ret_GuildPkInfo_SC>(this.OnGuildPkListInfo));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildPkEnroll_SC>(60151, new ProtoMsgCallback<MSG_Ret_GuildPkEnroll_SC>(this.OnEnterGuildPkLst));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Refresh_GuildPkMemberInfo_SC>(60165, new ProtoMsgCallback<MSG_Refresh_GuildPkMemberInfo_SC>(this.OnGuildkPkMemeberStateChange));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildPkMatchResult_SC>(60166, new ProtoMsgCallback<MSG_Ret_GuildPkMatchResult_SC>(this.OnMSG_Ret_GuildPkMatchResult_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RealTime_GuildPkTeam_Rank_SC>(2543, new ProtoMsgCallback<MSG_RealTime_GuildPkTeam_Rank_SC>(this.MSG_RealTime_GuildPkTeam_Rank_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildPkCountDown_SC>(2545, new ProtoMsgCallback<MSG_Ret_GuildPkCountDown_SC>(this.MSG_Ret_GuildPkCountDown_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildPkFight_SC>(2546, new ProtoMsgCallback<MSG_Ret_GuildPkFight_SC>(this.MSG_Ret_GuildPkFight_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_GuildPk_FinalResult_SC>(2547, new ProtoMsgCallback<MSG_GuildPk_FinalResult_SC>(this.MSG_GuildPk_FinalResult_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildPkRank_SC>(60157, new ProtoMsgCallback<MSG_Ret_GuildPkRank_SC>(this.MSG_Ret_GuildPkRank_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildPkWinList_SC>(60159, new ProtoMsgCallback<MSG_Ret_GuildPkWinList_SC>(this.OnMSG_Ret_GuildPkWinList_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GuildPkEnroll_Finish_SC>(2563, new ProtoMsgCallback<MSG_Ret_GuildPkEnroll_Finish_SC>(this.OnMSG_Ret_GuildPkEnroll_Finish_SC));
    }

    private void OnMSG_Ret_GuildPkEnroll_Finish_SC(MSG_Ret_GuildPkEnroll_Finish_SC data)
    {
        this.mController.DeleteGuildWarUI();
    }

    public void CreateGuild(string guildname, string iconName)
    {
        base.SendMsg<MSG_Req_CreateGuild_CS>(CommandID.MSG_Req_CreateGuild_CS, new MSG_Req_CreateGuild_CS
        {
            guildname = guildname,
            icon = iconName
        }, false);
    }

    private void OnRetCreatGuild(MSG_Ret_CreateGuild_SC data)
    {
        this.mController.CreateGuildCb(data.issucc);
    }

    public void GuildInvite(string joinmemberid)
    {
        base.SendMsg<MSG_Req_GuildInvite_CS>(CommandID.MSG_Req_GuildInvite_CS, new MSG_Req_GuildInvite_CS
        {
            joinmemberid = joinmemberid
        }, false);
    }

    private void OnRetGuildInvite(MSG_Ret_GuildInvite_SC data)
    {
        if (data.retcode == 1U)
        {
            this.mController.GuildInviteConfirm(data.id, data.name, data.guildname);
        }
    }

    public void GuildInviteConfirm(string inviterid)
    {
        base.SendMsg<MSG_Req_GuildInviteConfirm_CS>(CommandID.MSG_Req_GuildInviteConfirm_CS, new MSG_Req_GuildInviteConfirm_CS
        {
            inviterid = inviterid
        }, false);
    }

    private void OnRetGuildInviteConfirm(MSG_Ret_GuildInviteConfirm_SC data)
    {
        this.mController.GuildInviteConfirmCb(data.retcode);
    }

    public void GuildList()
    {
        base.SendMsg<MSG_Req_GuildList_CS>(CommandID.MSG_Req_GuildList_CS, new MSG_Req_GuildList_CS
        {
            page = 0U,
            countryid = 0U
        }, false);
    }

    private void OnRetGuildList(MSG_Ret_GuildList_SC data)
    {
        this.mController.GuildListCb(data.guildList);
    }

    public void ApplyGuild(ulong guildid, bool isApplyNotCancel)
    {
        base.SendMsg<MSG_Req_ApplyForGuild_CS>(CommandID.MSG_Req_ApplyForGuild_CS, new MSG_Req_ApplyForGuild_CS
        {
            guildid = guildid,
            flag = isApplyNotCancel
        }, false);
    }

    private void OnRetApplyGuild(MSG_Ret_ApplyForGuild_SC data)
    {
        this.mController.ApplyGuildCb(data.guildid, data.flag, data.issucc);
    }

    public void GuildInfo(ulong guildid)
    {
        base.SendMsg<MSG_Req_GuildInfo_CS>(CommandID.MSG_Req_GuildInfo_CS, new MSG_Req_GuildInfo_CS
        {
            guildid = guildid
        }, false);
    }

    private void GuildInfoCb(MSG_Ret_GuildInfo_SC data)
    {
        this.mController.GuildInfoCb(data.guildinfo, data.myinfo);
    }

    public void GuildMemberList(ReqMemberListType reqType)
    {
        base.SendMsg<MSG_Req_GuildMemberList_CS>(CommandID.MSG_Req_GuildMemberList_CS, new MSG_Req_GuildMemberList_CS
        {
            reqtype = reqType
        }, false);
    }

    private void GuildMemeberListCb(MSG_Ret_GuildMemberList_SC data)
    {
        this.mController.GuildMemeberListCb(data.members, data.reqtype);
    }

    public void ExitGuild()
    {
        MSG_Req_QuitGuild_CS t = new MSG_Req_QuitGuild_CS();
        base.SendMsg<MSG_Req_QuitGuild_CS>(CommandID.MSG_Req_QuitGuild_CS, t, false);
    }

    private void ExitGuildCb(MSG_Ret_QuitGuild_SC data)
    {
        if (data.retcode == 1U)
        {
            this.mController.ExitGuildCb();
        }
    }

    public void SetGuildNotify(string content)
    {
        base.SendMsg<MSG_Req_SetGuildNotify_CS>(CommandID.MSG_Req_SetGuildNotify_CS, new MSG_Req_SetGuildNotify_CS
        {
            notify = content
        }, false);
    }

    private void SetGuildNotifyCb(MSG_Ret_SetGuildNotify_SC data)
    {
        this.mController.SetGuildNotifyCb(data.issucc);
    }

    public void DonateGuild(uint num)
    {
        base.SendMsg<MSG_ReqDonateSalary_CS>(CommandID.MSG_ReqDonateSalary_CS, new MSG_ReqDonateSalary_CS
        {
            value = num
        }, false);
    }

    public void UserCntData(UserDataType type)
    {
        base.SendMsg<MSG_ReqUserCntData_CSC>(CommandID.MSG_ReqUserCntData_CSC, new MSG_ReqUserCntData_CSC
        {
            type = type
        }, false);
    }

    private void UserCntDataCb(MSG_ReqUserCntData_CSC data)
    {
        UserDataType type = data.type;
        if (type == UserDataType.GUILD_DAILY_COUNTRIBUTE)
        {
            this.mController.DailyContributeCb(data.value);
        }
    }

    public void AddGuildPosition(GuildPositionInfo guildPositionInfo)
    {
        base.SendMsg<MSG_Req_AddGuildPosition_CS>(CommandID.MSG_Req_AddGuildPosition_CS, new MSG_Req_AddGuildPosition_CS
        {
            posinfo = guildPositionInfo
        }, false);
    }

    public void DeleteGuildPosition(uint positionid)
    {
        base.SendMsg<MSG_Req_DeleteGuildPosition_CS>(CommandID.MSG_Req_DeleteGuildPosition_CS, new MSG_Req_DeleteGuildPosition_CS
        {
            positionid = positionid
        }, false);
    }

    public void ChangePositionName(uint positionid, string name)
    {
        base.SendMsg<MSG_Req_ChangePositionName_CS>(CommandID.MSG_Req_ChangePositionName_CS, new MSG_Req_ChangePositionName_CS
        {
            positionid = positionid,
            name = name
        }, false);
    }

    public void ChangePositionPrivilege(uint positionid, uint privilege)
    {
        base.SendMsg<MSG_Req_ChangePositionPri_CS>(CommandID.MSG_Req_ChangePositionPri_CS, new MSG_Req_ChangePositionPri_CS
        {
            positionid = positionid,
            privilege = privilege
        }, false);
    }

    public void AssignPosition(ulong memberid, uint positionid)
    {
        base.SendMsg<MSG_Req_AssignPosition_CS>(CommandID.MSG_Req_AssignPosition_CS, new MSG_Req_AssignPosition_CS
        {
            memberid = memberid,
            positionid = positionid
        }, false);
    }

    private void PositionInfoCb(MSG_Ret_PositionInfo_SC data)
    {
        this.mController.PositionInfoCb(data.posinfos);
    }

    public void ChangeGuildMaster(string newmasterid)
    {
        base.SendMsg<MSG_Req_ChangeGuildMaster_CS>(CommandID.MSG_Req_ChangeGuildMaster_CS, new MSG_Req_ChangeGuildMaster_CS
        {
            newmasterid = newmasterid
        }, false);
    }

    private void ChangeGuildMasterCb(MSG_Ret_ChangeGuildMaster_SC data)
    {
        if (data.retcode == 1U)
        {
            this.mController.ChangeGuildMasterCb();
        }
    }

    public void FireGuildMember(string leavememberid)
    {
        base.SendMsg<MSG_Req_FireGuildMember_CS>(CommandID.MSG_Req_FireGuildMember_CS, new MSG_Req_FireGuildMember_CS
        {
            leavememberid = leavememberid
        }, false);
    }

    private void FireGuildMemberCb(MSG_Ret_FireGuildMember_SC data)
    {
        if (data.retcode == 1U)
        {
            this.mController.FireGuildMemberCb(data.leavememberid);
        }
    }

    public void AnswerApplyForGuild(ulong applyid, string applyname, bool issucc)
    {
        MSG_Req_AnswerApplyForGuild_CS msg_Req_AnswerApplyForGuild_CS = new MSG_Req_AnswerApplyForGuild_CS();
        List<stApplyForItem> list = new List<stApplyForItem>();
        list.Add(new stApplyForItem
        {
            applyid = applyid,
            applyname = applyname
        });
        msg_Req_AnswerApplyForGuild_CS.applyinfo.AddRange(list);
        msg_Req_AnswerApplyForGuild_CS.accept = issucc;
        base.SendMsg<MSG_Req_AnswerApplyForGuild_CS>(CommandID.MSG_Req_AnswerApplyForGuild_CS, msg_Req_AnswerApplyForGuild_CS, false);
    }

    private void AnswerApplyForGuildCb(MSG_Ret_AnswerApplyForGuild_SC data)
    {
        if (data.retlist.Count > 0)
        {
            this.mController.AnswerApplyForGuildCb(data.retlist[0]);
        }
    }

    private void MyGuildApplyResultCb(MSG_Ret_MyGuildApply_Result_SC data)
    {
        this.mController.MyGuildApplyResultCb(data.result.applyid, data.result.issucc);
    }

    public void ReqGuildSkill()
    {
        MSG_Req_GuildSkill_CS t = new MSG_Req_GuildSkill_CS();
        base.SendMsg<MSG_Req_GuildSkill_CS>(CommandID.MSG_Req_GuildSkill_CS, t, false);
    }

    public void ReqLearnGuildSkill(uint sid)
    {
        base.SendMsg<MSG_Req_LearnGuildSkill_CSC>(CommandID.MSG_Req_LearnGuildSkill_CSC, new MSG_Req_LearnGuildSkill_CSC
        {
            skillinfo = new guildSkill()
            {
                skillid = sid,
                skilllv = sid % 100U
            }
        }, false);
    }

    public void RetGuildSkillList(MSG_Ret_GuildSkill_SC msg)
    {
        this.mController.RetGuildSkillList(msg.skillinfo);
    }

    private void OnMSG_Req_LearnGuildSkill_CSC(MSG_Req_LearnGuildSkill_CSC msg)
    {
        this.mController.OnGuildSkillLevelUp(msg);
    }

    private void MSG_Ret_GuildLevelup_SC(MSG_Ret_GuildLevelup_SC msg)
    {
        this.mController.OnGuildLevelUp(msg);
    }

    public void ReqGuildPkListInfo()
    {
        MSG_Req_GuildPkInfo_CS t = new MSG_Req_GuildPkInfo_CS();
        base.SendMsg<MSG_Req_GuildPkInfo_CS>(CommandID.MSG_Req_GuildPkInfo_CS, t, false);
    }

    private void OnGuildPkListInfo(MSG_Ret_GuildPkInfo_SC msg)
    {
        this.mController.OnGuildPkListInfo(msg);
    }

    public void ReqQuitCurFightTeam()
    {
        MSG_Req_GuildPkQuitTeam_CS t = new MSG_Req_GuildPkQuitTeam_CS();
        base.SendMsg<MSG_Req_GuildPkQuitTeam_CS>(CommandID.MSG_Req_GuildPkQuitTeam_CS, t, false);
    }

    public void ReqJoinFightTeam(uint teamID, uint pos)
    {
        base.SendMsg<MSG_Req_GuildPkJoinTeam_CS>(CommandID.MSG_Req_GuildPkJoinTeam_CS, new MSG_Req_GuildPkJoinTeam_CS
        {
            teamid = teamID,
            posid = pos
        }, false);
    }

    private void OnEnterGuildPkLst(MSG_Ret_GuildPkEnroll_SC msg)
    {
        if (msg.result)
        {
            this.mController.OnOtherPlayerJoinGuildPk();
        }
    }

    private void OnGuildkPkMemeberStateChange(MSG_Refresh_GuildPkMemberInfo_SC msg)
    {
        this.mController.OnGuildkPkMemeberStateChange(msg.member);
    }

    public void MSG_Req_GuildPkEnroll_CS()
    {
        MSG_Req_GuildPkEnroll_CS t = new MSG_Req_GuildPkEnroll_CS();
        base.SendMsg<MSG_Req_GuildPkEnroll_CS>(CommandID.MSG_Req_GuildPkEnroll_CS, t, false);
    }

    public void EnsureEnterGuildFight()
    {
        MSG_Req_EnterGuildPk_CS t = new MSG_Req_EnterGuildPk_CS();
        base.SendMsg<MSG_Req_EnterGuildPk_CS>(CommandID.MSG_Req_EnterGuildPk_CS, t, false);
    }

    private void OnMSG_Ret_GuildPkMatchResult_SC(MSG_Ret_GuildPkMatchResult_SC msg)
    {
        this.mController.OnMSG_Ret_GuildPkMatchResult_SC(msg);
    }

    public void ReqGuildPkRankLst()
    {
        MSG_Req_GuildPkRank_CS t = new MSG_Req_GuildPkRank_CS();
        base.SendMsg<MSG_Req_GuildPkRank_CS>(CommandID.MSG_Req_GuildPkRank_CS, t, false);
    }

    private void MSG_RealTime_GuildPkTeam_Rank_SC(MSG_RealTime_GuildPkTeam_Rank_SC msg)
    {
        this.mController.OnRefrashRealTimePkInfo(msg);
    }

    private void MSG_Ret_GuildPkCountDown_SC(MSG_Ret_GuildPkCountDown_SC msg)
    {
        this.mController.OnReadyStartFight(msg.lefttime);
    }

    private void MSG_Ret_GuildPkFight_SC(MSG_Ret_GuildPkFight_SC msg)
    {
        this.mController.OnReadyStartFight(msg.lefttime);
    }

    private void MSG_GuildPk_FinalResult_SC(MSG_GuildPk_FinalResult_SC msg)
    {
        this.mController.MSG_GuildPk_FinalResult_SC(msg.iswin, msg.teamlist);
    }

    public void LeaveFight()
    {
        MSG_Req_ExitCopymap_SC t = new MSG_Req_ExitCopymap_SC();
        base.SendMsg<MSG_Req_ExitCopymap_SC>(CommandID.MSG_Req_ExitCopymap_SC, t, false);
    }

    public void MSG_Req_GuildPkRank_CS()
    {
        MSG_Req_GuildPkRank_CS t = new MSG_Req_GuildPkRank_CS();
        base.SendMsg<MSG_Req_GuildPkRank_CS>(CommandID.MSG_Req_GuildPkRank_CS, t, false);
    }

    private void MSG_Ret_GuildPkRank_SC(MSG_Ret_GuildPkRank_SC msg)
    {
        this.mController.OnMSG_Ret_GuildPkRank_SC(msg.scorerank);
    }

    public void Req_GuildPkWinList_CS()
    {
        MSG_Req_GuildPkWinList_CS t = new MSG_Req_GuildPkWinList_CS();
        base.SendMsg<MSG_Req_GuildPkWinList_CS>(CommandID.MSG_Req_GuildPkWinList_CS, t, false);
    }

    private void OnMSG_Ret_GuildPkWinList_SC(MSG_Ret_GuildPkWinList_SC msg)
    {
        this.mController.OnMSG_Ret_GuildPkWinList_CS(msg.winers);
    }

    public override void Uninitialize()
    {
        base.Uninitialize();
    }
}
