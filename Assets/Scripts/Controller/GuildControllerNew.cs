using System;
using System.Collections.Generic;
using Framework.Managers;
using guild;
using guildpk_msg;
using LuaInterface;
using Models;

public class GuildControllerNew : ControllerBase
{
    public UI_GuildInfoNew UIGuildInfo
    {
        get
        {
            return UIManager.GetUIObject<UI_GuildInfoNew>();
        }
    }

    public UI_GuildListNew UIGuildList
    {
        get
        {
            return UIManager.GetUIObject<UI_GuildListNew>();
        }
    }
     
    public UI_GuildCreatNew UIGuildCreat
    {
        get
        {
            return UIManager.GetUIObject<UI_GuildCreatNew>();
        }
    }

    public UI_GuildWar UIGuildWar
    {
        get
        {
            return UIManager.GetUIObject<UI_GuildWar>();
        }
    }

    public override void Awake()
    {
        this.OWN_GUILD_LIMIT = CommonTools.GetTextById(674UL);
        this.INVITE_GUILD_LEVEL_LIMIT = CommonTools.GetTextById(675UL);
        this.APPLY_GUILD_LEVEL_LIMIT = CommonTools.GetTextById(676UL);
        try
        {
            this.ltGuildConfig = LuaConfigManager.GetXmlConfigTable("guildConfig");
        }
        catch (Exception)
        {
        }
        this.mNetWork = new GuildNetWorkNew();
        this.mNetWork.Initialize();
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.OpenGuildCreatePanel));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.OpenGuildWar));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.EnrollGuildPk));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.OpenGuildPkRank));
    }

    public List<guildListItem> GetGuildItemList()
    {
        return this.mGuildItemList;
    }

    public guildMember GetMyGuildMember()
    {
        return this.mMyGuildMemberInfo;
    }

    public List<guildSkill> GetGuildSkill()
    {
        return this.guildSkillinfo;
    }

    public guildInfo GetGuildBaseInfo()
    {
        return this.guildInfo;
    }

    public bool IsGuildMaster()
    {
        return this.mMyGuildMemberInfo.positionid == 1U;
    }

    public List<GuildPositionInfo> GetGuildPositionInfoList()
    {
        return this.guildPositionInfoList;
    }

    public GuildPositionInfo GetGuildPositionInfo(uint positionid)
    {
        for (int i = 0; i < this.guildPositionInfoList.Count; i++)
        {
            if (this.guildPositionInfoList[i].positionid == positionid)
            {
                return this.guildPositionInfoList[i];
            }
        }
        return null;
    }

    public List<guildMember> GetNormalMembers()
    {
        return this.mNormalMemberList;
    }

    public guildMember GetMember(ulong memberid)
    {
        for (int i = 0; i < this.mNormalMemberList.Count; i++)
        {
            if (this.mNormalMemberList[i].memberid == memberid)
            {
                return this.mNormalMemberList[i];
            }
        }
        return null;
    }

    public void UpdateListMemberData(guildMember member)
    {
        for (int i = 0; i < this.mNormalMemberList.Count; i++)
        {
            guildMember guildMember = this.mNormalMemberList[i];
            if (guildMember.memberid == member.memberid)
            {
                guildMember.contribute = member.contribute;
                guildMember.donatesalary = member.donatesalary;
            }
        }
    }

    public bool GetMemberGuildPrivilege(ulong memberid, GuildPrivilege guildPrivilege)
    {
        guildMember member = this.GetMember(memberid);
        return this.GetPositionGuildPrivilege(member.positionid, guildPrivilege);
    }

    public uint GetFlagByPrivilege(List<GuildPrivilege> guildPrivilegeList)
    {
        uint num = 0U;
        for (int i = 0; i < guildPrivilegeList.Count; i++)
        {
            num = (uint)((int)num + guildPrivilegeList[i]);
        }
        return num;
    }

    public bool GetPositionGuildPrivilegeSelf(GuildPrivilege guildPrivilege)
    {
        return this.GetPositionGuildPrivilege(this.mMyGuildMemberInfo.positionid, guildPrivilege);
    }

    public bool GetPositionGuildPrivilege(uint positionid, GuildPrivilege guildPrivilege)
    {
        for (int i = 0; i < this.guildPositionInfoList.Count; i++)
        {
            if (this.guildPositionInfoList[i].positionid == positionid)
            {
                return GlobalRegister.get_flag(this.guildPositionInfoList[i].privilege, (uint)guildPrivilege);
            }
        }
        return false;
    }

    public ulong GetMyGuildId()
    {
        return MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.guildid;
    }

    private bool IsInGuild()
    {
        return this.GetMyGuildId() != 0UL;
    }

    public void OpenGuildPanel()
    {
        UIManager.Instance.DeleteUI("UI_Family", true);
        if (this.IsInGuild())
        {
            this.OpenGuildInfoPanel();
        }
        else
        {
            this.OpenGuildListPanel();
        }
    }

    public void OpenCloseGuildPanel()
    {
        if (UIManager.GetUIObject("UI_Family") != null)
        {
            UIManager.Instance.DeleteUI("UI_Family", true);
        }
        else
        {
            this.OpenGuildPanel();
        }
    }

    public void OpenGuildCreatePanel(List<VarType> paras)
    {
        UIManager.Instance.DeleteUI("UI_Family", true);
        if (this.IsInGuild())
        {
            TipsWindow.ShowWindow(this.OWN_GUILD_LIMIT);
            return;
        }
        UIManager.Instance.ShowUI<UI_GuildCreatNew>("UI_Family", null, UIManager.ParentType.CommonUI, false);
    }

    public void OpenGuildWar(List<VarType> paras)
    {
        GuildWarUIType type = (GuildWarUIType)int.Parse(paras[0]);
        Action action = delegate ()
        {
            this.UIGuildWar.SetOpenType(type);
            switch (type)
            {
                case GuildWarUIType.TeamMatch:
                    this.mNetWork.ReqGuildPkListInfo();
                    break;
                case GuildWarUIType.RankingInfo:
                    this.mNetWork.ReqGuildPkRankLst();
                    break;
            }
        };
        if (this.UIGuildWar == null)
        {
            this.ShowGuildWarUI(action);
        }
        else
        {
            action();
        }
    }

    public void CreateGuild(string name, string iconName)
    {
        this.mNetWork.CreateGuild(name, iconName);
    }

    public void CreateGuildCb(bool isSuccess)
    {
        if (isSuccess)
        {
            UIManager.Instance.DeleteUI<UI_GuildCreatNew>();
        }
    }

    public bool HasInvitePower()
    {
        return this.IsInGuild() && this.GetPositionGuildPrivilegeSelf(GuildPrivilege.GUILDPRI_ACCEPT_JOIN);
    }

    public void GuildInvite(string joinmemberid)
    {
        this.mNetWork.GuildInvite(joinmemberid);
    }

    public void GuildInviteConfirm(string inviterId, string inviterName, string guildName)
    {
        if (!GameSystemSettings.IsAllowFamilyInvite())
        {
            return;
        }
        UIManager.Instance.ShowUI<UI_MessageBox>("UI_MessageBox", delegate ()
        {
            UI_MessageBox uiobject = UIManager.GetUIObject<UI_MessageBox>();
            uiobject.SetOkCb(new MessageOkCb2(this.ReqGuildInviteOkCb), guildName);
            string content = inviterName + "邀请你加入公会" + guildName;
            uiobject.SetContent(content, "提示", true);
        }, UIManager.ParentType.CommonUI, false);
    }

    private void ReqGuildInviteOkCb(string inviterId)
    {
        this.mNetWork.GuildInviteConfirm(inviterId);
    }

    public void GuildInviteConfirmCb(uint retcode)
    {
    }

    private void OpenGuildListPanel()
    {
        UIManager.Instance.ShowUI<UI_GuildListNew>("UI_Family", delegate ()
        {
            this.mNetWork.GuildList();
        }, UIManager.ParentType.CommonUI, false);
    }

    public void GuildListCb(List<guildListItem> guildItemList)
    {
        this.mGuildItemList = guildItemList;
        this.UIGuildList.GuildListCb();
    }

    public void ApplyGuild(ulong guildid, bool isApplyNotCancel)
    {
        if (MainPlayer.Self.GetCurLevel() < this.ltGuildConfig.GetField_Uint("joinLevel"))
        {
            TipsWindow.ShowWindow(string.Format(this.APPLY_GUILD_LEVEL_LIMIT, this.ltGuildConfig.GetField_Uint("joinLevel")));
            return;
        }
        this.mNetWork.ApplyGuild(guildid, isApplyNotCancel);
    }

    public void ApplyGuildCb(ulong guildid, bool isApplyNotCancel, bool isSuccess)
    {
        if (this.UIGuildList != null)
        {
            this.UIGuildList.ApplyGuildCb(guildid, isApplyNotCancel, isSuccess);
            if (isSuccess)
            {
                for (int i = 0; i < this.mGuildItemList.Count; i++)
                {
                    if (this.mGuildItemList[i].guild.guildid == guildid)
                    {
                        this.mGuildItemList[i].guildtype = ((!isApplyNotCancel) ? 0U : 2U);
                        break;
                    }
                }
            }
        }
    }

    public void GuildInfoSelf(Action onGetGuildInfoBack = null)
    {
        this.mNetWork.GuildInfo(this.GetMyGuildId());
        if (this.GetMyGuildId() == 0UL && onGetGuildInfoBack != null)
        {
            onGetGuildInfoBack();
            onGetGuildInfoBack = null;
            return;
        }
        this.onGetGuildInfoBack = onGetGuildInfoBack;
    }

    private void OpenGuildInfoPanel()
    {
        UIManager.Instance.ShowUI<UI_GuildInfoNew>("UI_Family", delegate ()
        {
            this.GuildInfoSelf(null);
            this.GuildMemberList(ReqMemberListType.NORMAL);
            this.mNetWork.UserCntData(UserDataType.GUILD_DAILY_COUNTRIBUTE);
            this.UIGuildInfo.TryGetContributeSkillLv();
        }, UIManager.ParentType.CommonUI, false);
    }

    public void GuildMemberList(ReqMemberListType type)
    {
        this.mNetWork.GuildMemberList(type);
    }

    public void GuildInfoCb(guildInfo guildInfo, guildMember myinfo)
    {
        if (guildInfo == null)
        {
            return;
        }
        this.guildPositionInfoList = guildInfo.posinfo;
        this.guildInfo = guildInfo;
        this.mMyGuildMemberInfo = myinfo;
        if (this.UIGuildInfo != null)
        {
            this.UIGuildInfo.SetupInfo(guildInfo, myinfo);
        }
        if (this.onGetGuildInfoBack != null)
        {
            this.onGetGuildInfoBack();
            this.onGetGuildInfoBack = null;
        }
    }

    public void GuildMemeberListCb(List<guildMember> memebers, ReqMemberListType reqType)
    {
        if (reqType == ReqMemberListType.NORMAL)
        {
            this.mNormalMemberList = memebers;
            if (this.UIGuildInfo != null)
            {
                this.UIGuildInfo.SetupListPanel(memebers);
            }
        }
        else
        {
            this.mApplyMemberList = memebers;
            if (this.UIGuildInfo != null)
            {
                this.UIGuildInfo.SetupApplyListBySort(UI_GuildInfoNew.APPLY_SORT.DEFAULT);
            }
        }
    }

    public void ExitGuild()
    {
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox("提示", CommonTools.GetTextById(661UL), "确认", "取消", UIManager.ParentType.Tips, new Action(this.exitguildsure), new Action(this.exitguildcancel), null);
    }

    private void exitguildsure()
    {
        this.mNetWork.ExitGuild();
        if (this.guildSkillinfo != null)
        {
            this.guildSkillinfo.Clear();
        }
    }

    private void exitguildcancel()
    {
    }

    public void ExitGuildCb()
    {
        UIManager.Instance.DeleteUI<UI_GuildInfoNew>();
        this.OnEnterOrExitGuildFrashTaskUI();
        this.DeleteGuildWarUI();
    }

    public void OnEnterOrExitGuildFrashTaskUI()
    {
        Scheduler.Instance.AddTimer(0.2f, false, delegate
        {
            LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.OnEnterOrExitGuild", new object[]
            {
                Util.GetLuaTable("MainUICtrl")
            });
            LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.ShowTaskList", new object[]
            {
                Util.GetLuaTable("MainUICtrl")
            });
        });
    }

    public void SetGuildNotify(string content)
    {
        this.mNetWork.SetGuildNotify(content);
    }

    public void SetGuildNotifyCb(bool issucc)
    {
        this.UIGuildInfo.SetGuildNotifyCb(issucc);
    }

    public void DonateGuild(uint num)
    {
        this.mNetWork.DonateGuild(num);
    }

    public void DailyContributeCb(uint value)
    {
        if (this.UIGuildInfo != null)
        {
            this.UIGuildInfo.DailyContributeCb(value);
        }
    }

    public void AddGuildPosition(GuildPositionInfo guildPositionInfo)
    {
        this.mNetWork.AddGuildPosition(guildPositionInfo);
    }

    public void DeleteGuildPosition(uint positionid)
    {
        this.mNetWork.DeleteGuildPosition(positionid);
    }

    internal void OnGuildSkillLevelUp(MSG_Req_LearnGuildSkill_CSC msg)
    {
        if (this.UIGuildInfo != null)
        {
            bool flag = false;
            for (int i = 0; i < this.guildSkillinfo.Count; i++)
            {
                if (this.guildSkillinfo[i].skillid == msg.skillinfo.skillid)
                {
                    this.guildSkillinfo[i].skilllv = msg.skillinfo.skilllv;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                this.guildSkillinfo.Add(msg.skillinfo);
            }
            this.UIGuildInfo.OnSkillLstDataBack(this.guildSkillinfo);
        }
        UI_NPCshop uiobject = UIManager.GetUIObject<UI_NPCshop>();
        if (uiobject != null)
        {
            ShopController controller = ControllerManager.Instance.GetController<ShopController>();
            if (controller != null)
            {
                controller.OnSkillLvUp(msg.skillinfo);
            }
        }
    }

    internal void OnGuildLevelUp(MSG_Ret_GuildLevelup_SC msg)
    {
        this.GuildInfoSelf(null);
        this.mNetWork.ReqGuildSkill();
    }

    public void ChangePositionName(uint positionid, string name)
    {
        this.mNetWork.ChangePositionName(positionid, name);
    }

    public void ChangePositionPrivilege(uint positionid, uint privilege)
    {
        this.mNetWork.ChangePositionPrivilege(positionid, privilege);
    }

    internal void RetGuildSkillList(List<guildSkill> skillinfo)
    {
        this.guildSkillinfo = skillinfo;
        if (this.UIGuildInfo != null)
        {
            this.UIGuildInfo.OnSkillLstDataBack(skillinfo);
        }
        for (int i = 0; i < skillinfo.Count; i++)
        {
            uint num = skillinfo[i].skillid / 100U;
            if (num == 4U && this.getFastSwitchRatioBack != null)
            {
                try
                {
                    List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("skill_guild");
                    for (int j = 0; j < configTableList.Count; j++)
                    {
                        if (configTableList[j].GetCacheField_Uint("skillid") == skillinfo[i].skillid)
                        {
                            LuaTable luaTable = configTableList[j];
                            uint num2 = 0U;
                            uint.TryParse(luaTable.GetCacheField_String("skillstaus"), out num2);
                            this.getFastSwitchRatioBack((100U - num2) / 100f);
                            this.getFastSwitchRatioBack = null;
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            if (this.getGuildLvBack.ContainsKey(num))
            {
                this.getGuildLvBack[num](skillinfo[i].skillid % 100U);
                this.getGuildLvBack.Remove(num);
            }
        }
        if (this.getFastSwitchRatioBack != null)
        {
            this.getFastSwitchRatioBack(1f);
            this.getFastSwitchRatioBack = null;
        }
        if (this.onGetGuildSkillLstBack != null)
        {
            this.onGetGuildSkillLstBack();
            this.onGetGuildSkillLstBack = null;
        }
    }

    public bool CheckGuildSkillIsLearn(uint skillId)
    {
        bool result = false;
        if (this.guildSkillinfo != null)
        {
            for (int i = 0; i < this.guildSkillinfo.Count; i++)
            {
                if (this.guildSkillinfo[i].skillid == skillId)
                {
                    result = true;
                    break;
                }
            }
        }
        return result;
    }

    public void ReqGuildSkillInfo(Action onGuildSkillLstBack = null)
    {
        this.mNetWork.ReqGuildSkill();
        if (this.GetMyGuildId() == 0UL)
        {
            if (onGuildSkillLstBack != null)
            {
                onGuildSkillLstBack();
                onGuildSkillLstBack = null;
            }
            return;
        }
        this.onGetGuildSkillLstBack = onGuildSkillLstBack;
    }

    public void ReqLearnGuildSkill(uint skillId)
    {
        this.mNetWork.ReqLearnGuildSkill(skillId);
        this.UIGuildInfo.TryGetContributeSkillLv();
    }

    public void TryGetFastSwitchSkillBufDat(Action<float> getRatioBack)
    {
        this.ReqGuildSkillInfo(null);
        this.getFastSwitchRatioBack = getRatioBack;
    }

    public void TryGetGuildSkillLv(Action<uint> back, uint skType)
    {
        this.getGuildLvKillType = skType;
        if (this.getGuildLvBack.ContainsKey(skType))
        {
            Dictionary<uint, Action<uint>> dictionary2;
            Dictionary<uint, Action<uint>> dictionary = dictionary2 = this.getGuildLvBack;
            Action<uint> a = dictionary2[skType];
            dictionary[skType] = (Action<uint>)Delegate.Combine(a, back);
        }
        else
        {
            this.getGuildLvBack[skType] = back;
        }
    }

    public LuaTable GetGuildSkillConfigByID(uint id)
    {
        try
        {
            List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("skill_guild");
            for (int i = 0; i < configTableList.Count; i++)
            {
                uint cacheField_Uint = configTableList[i].GetCacheField_Uint("skillid");
                if (cacheField_Uint == id)
                {
                    return configTableList[i];
                }
            }
        }
        catch (Exception)
        {
        }
        return null;
    }

    public LuaTable GetLevelConfigByLevel(uint id)
    {
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        return controller.GetLvConfig(id);
    }

    public void AssignPosition(ulong memberid, uint positionid)
    {
        this.mNetWork.AssignPosition(memberid, positionid);
    }

    public GuildPositionInfo GetPositionInfo(uint positionid)
    {
        for (int i = 0; i < this.guildPositionInfoList.Count; i++)
        {
            if (this.guildPositionInfoList[i].positionid == positionid)
            {
                return this.guildPositionInfoList[i];
            }
        }
        return null;
    }

    public void PositionInfoCb(List<GuildPositionInfo> positionInfoList)
    {
        this.guildPositionInfoList = positionInfoList;
        if (this.UIGuildInfo != null)
        {
            this.UIGuildInfo.RefreshPositionPanel();
        }
    }

    public void ChangeGuildMaster(string newmasterid)
    {
        this.mNetWork.ChangeGuildMaster(newmasterid);
    }

    public void ChangeGuildMasterCb()
    {
    }

    public void FireGuildMember(string leavememberid)
    {
        this.mNetWork.FireGuildMember(leavememberid);
    }

    public void FireGuildMemberCb(string leavememberid)
    {
    }

    private guildMember GetApplyMember(ulong applyid)
    {
        for (int i = 0; i < this.mApplyMemberList.Count; i++)
        {
            if (this.mApplyMemberList[i].memberid == applyid)
            {
                return this.mApplyMemberList[i];
            }
        }
        return null;
    }

    public List<guildMember> GetApplyMemberList()
    {
        return this.mApplyMemberList;
    }

    public void AnswerApplyForGuild(ulong applyid, bool issucc)
    {
        guildMember applyMember = this.GetApplyMember(applyid);
        if (applyMember != null)
        {
            this.mNetWork.AnswerApplyForGuild(applyid, applyMember.membername, issucc);
        }
    }

    public void AnswerApplyForGuildCb(stApplyForItem data)
    {
        guildMember applyMember = this.GetApplyMember(data.applyid);
        if (applyMember != null)
        {
            this.mApplyMemberList.Remove(applyMember);
            this.UIGuildInfo.SetupApplyListBySort(UI_GuildInfoNew.APPLY_SORT.NONE);
            if (data.issucc)
            {
                this.mNetWork.GuildMemberList(ReqMemberListType.NORMAL);
            }
        }
    }

    public void MyGuildApplyResultCb(ulong applyid, bool issucc)
    {
        if (applyid == MainPlayer.Self.EID.Id && issucc)
        {
            if (this.UIGuildList != null)
            {
                UIManager.Instance.DeleteUI<UI_GuildListNew>();
                Scheduler.Instance.AddTimer(0.2f, false, delegate
                {
                    this.OpenGuildPanel();
                });
            }
            this.OnEnterOrExitGuildFrashTaskUI();
        }
    }

    public void OnGuildPkListInfo(MSG_Ret_GuildPkInfo_SC msg)
    {
        this.pkInfoData = msg;
        if (this.UIGuildWar != null)
        {
            this.UIGuildWar.OnGuildPkListInfo(msg);
        }
    }

    public void ReqQuitCurFightTeam()
    {
        this.mNetWork.ReqQuitCurFightTeam();
    }

    public void ReqJoinFightTeam(uint teamID, uint pos)
    {
        this.mNetWork.ReqJoinFightTeam(teamID, pos);
    }

    public void OnOtherPlayerJoinGuildPk()
    {
        if (this.UIGuildWar == null)
        {
            List<VarType> list = new List<VarType>();
            VarType item = new VarType("0");
            list.Add(item);
            this.OpenGuildWar(list);
        }
        else
        {
            this.mNetWork.ReqGuildPkListInfo();
        }
    }

    public void OnGuildkPkMemeberStateChange(GuildPkMemberInfo memberInfo)
    {
        if (this.pkInfoData != null)
        {
            for (int i = 0; i < this.pkInfoData.guildinfo.teaminfo.Count; i++)
            {
                for (int j = 0; j < this.pkInfoData.guildinfo.teaminfo[i].members.Count; j++)
                {
                    if (this.pkInfoData.guildinfo.teaminfo[i].members[j].charid == memberInfo.charid)
                    {
                        this.pkInfoData.guildinfo.teaminfo[i].members.Remove(this.pkInfoData.guildinfo.teaminfo[i].members[j]);
                    }
                }
                if (this.pkInfoData.guildinfo.teaminfo[i].teamid == memberInfo.teamid)
                {
                    this.pkInfoData.guildinfo.teaminfo[i].members.Add(memberInfo);
                }
            }
        }
        if (this.UIGuildWar != null)
        {
            this.UIGuildWar.OnGuildPkListInfo(this.pkInfoData);
        }
    }

    private void EnrollGuildPk(List<VarType> vtLst)
    {
        this.mNetWork.MSG_Req_GuildPkEnroll_CS();
    }

    public void OnMSG_Ret_GuildPkMatchResult_SC(MSG_Ret_GuildPkMatchResult_SC msg)
    {
        CountDownController controller = ControllerManager.Instance.GetController<CountDownController>();
        controller.ShowCountDownPanel("家族战匹配完成，开始进入家族战倒计时，确认进入请点击确认", msg.lefttime, delegate
        {
            this.mNetWork.EnsureEnterGuildFight();
        }, delegate
        {
        }, delegate
        {
        }, false);
    }

    internal void OnRefrashRealTimePkInfo(MSG_RealTime_GuildPkTeam_Rank_SC msg)
    {
        if (this.UIGuildWar == null)
        {
            List<VarType> list = new List<VarType>();
            VarType item = new VarType(1.ToString());
            list.Add(item);
            this.OpenGuildWar(list);
        }
        else
        {
            this.UIGuildWar.FrashFightingInfo(msg.teamrank);
        }
    }

    public void OnReadyStartFight(uint lefttime)
    {
        if (this.UIGuildWar == null)
        {
            this.ShowGuildWarUI(delegate
            {
                this.UIGuildWar.FrashReadyFightTimeLeft(lefttime);
            });
        }
        else
        {
            this.UIGuildWar.FrashReadyFightTimeLeft(lefttime);
        }
    }

    public void LeaveFight()
    {
        this.mNetWork.LeaveFight();
    }

    private void ShowGuildWarUI(Action onShow)
    {
        UIManager.Instance.ShowUI<UI_GuildWar>("UI_GuildWar", delegate ()
        {
            if (onShow != null)
            {
                onShow();
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public void DeleteGuildWarUI()
    {
        UIManager.Instance.DeleteUI<UI_GuildWar>();
    }

    public void MSG_GuildPk_FinalResult_SC(bool isWin, List<finalresult_guildteam_info> teamlist)
    {
        if (this.UIGuildWar == null)
        {
            this.ShowGuildWarUI(delegate
            {
                this.UIGuildWar.OnFightOver(isWin, teamlist);
            });
        }
        else
        {
            this.UIGuildWar.OnFightOver(isWin, teamlist);
        }
    }

    public void OpenGuildPkRank(List<VarType> vtLst)
    {
        this.mNetWork.MSG_Req_GuildPkRank_CS();
    }

    public void OnMSG_Ret_GuildPkRank_SC(List<GuildPkGuildScore> scoreList)
    {
        if (this.UIGuildWar == null)
        {
            this.ShowGuildWarUI(delegate
            {
                this.UIGuildWar.OnGetRankScore(scoreList);
            });
        }
        else
        {
            this.UIGuildWar.OnGetRankScore(scoreList);
        }
    }

    public void TrGetWinTeamRankData(Action<List<GuildPkWinInfo>> backAction)
    {
        this.mNetWork.Req_GuildPkWinList_CS();
        this.onWinTeamRandDataBackAction = null;
        this.onWinTeamRandDataBackAction = backAction;
    }

    public void OnMSG_Ret_GuildPkWinList_CS(List<GuildPkWinInfo> data)
    {
        if (this.onWinTeamRandDataBackAction != null)
        {
            this.onWinTeamRandDataBackAction(data);
        }
    }

    public void OnMSG_Req_GuildPkWinList_CS(MSG_Req_GuildPkWinList_CS msg)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override string ControllerName
    {
        get
        {
            return "guild_controller";
        }
    }

    public bool IsCreateNewPanelOpen()
    {
        return this.UIGuildCreat != null;
    }

    public GuildNetWorkNew mNetWork;

    private string OWN_GUILD_LIMIT = string.Empty;

    private string INVITE_GUILD_LEVEL_LIMIT = string.Empty;

    private string APPLY_GUILD_LEVEL_LIMIT = string.Empty;

    private LuaTable ltGuildConfig;

    private List<GuildPositionInfo> guildPositionInfoList;

    private guildInfo guildInfo;

    private List<guildMember> mNormalMemberList;

    private List<guildMember> mApplyMemberList;

    private guildMember mMyGuildMemberInfo;

    private List<guildListItem> mGuildItemList;

    public List<guildSkill> guildSkillinfo;

    private Action onGetGuildSkillLstBack;

    private Action onGetGuildInfoBack;

    private Action<float> getFastSwitchRatioBack;

    private Dictionary<uint, Action<uint>> getGuildLvBack = new Dictionary<uint, Action<uint>>();

    private uint getGuildLvKillType;

    private MSG_Ret_GuildPkInfo_SC pkInfoData;

    private Action<List<GuildPkWinInfo>> onWinTeamRandDataBackAction;
}
