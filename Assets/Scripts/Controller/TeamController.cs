using System;
using System.Collections.Generic;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using Models;
using Team;
using UnityEngine;

public class TeamController : ControllerBase, ITeamController
{
    public UI_Team uiTeam
    {
        get
        {
            return UIManager.GetUIObject<UI_Team>();
        }
    }

    public bool IsMainPlayerHasTeam()
    {
        return this.myTeamInfo.mem.Count > 0;
    }

    public Memember GetMainPlayerMemeber()
    {
        for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
        {
            if (this.myTeamInfo.mem[i].mememberid == MainPlayer.Self.GetCharID().ToString())
            {
                return this.myTeamInfo.mem[i];
            }
        }
        return null;
    }

    public bool IsMainPlayerLeader()
    {
        bool result = false;
        Memember teamLaederInfo = this.GetTeamLaederInfo();
        if (teamLaederInfo != null && teamLaederInfo.mememberid == MainPlayer.Self.GetCharID().ToString())
        {
            result = true;
        }
        return result;
    }

    public bool IsMyTeamNember(EntitiesID eid)
    {
        if (eid.Etype == CharactorType.Player)
        {
            for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
            {
                Memember memember = this.myTeamInfo.mem[i];
                if (memember.mememberid == eid.Id.ToString())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckIfInTeam(string id)
    {
        for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
        {
            if (this.myTeamInfo.mem[i].mememberid == id)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckIfTeamFull()
    {
        return (long)this.myTeamInfo.mem.Count >= (long)((ulong)this.myTeamInfo.maxmember);
    }

    private MainUIController MainController
    {
        get
        {
            return ControllerManager.Instance.GetController<MainUIController>();
        }
    }

    public void ReadMainViewMessage(MessageType type)
    {
        if (type == MessageType.Team)
        {
            this.InviteCountNoRead = 0;
        }
        this.MainController.ReadMessage(type);
    }

    public void DeleteInviteMember(string id)
    {
        if (this.dicPlayerInvite.ContainsKey(ulong.Parse(id)))
        {
            Memember item = this.dicPlayerInvite[ulong.Parse(id)];
            this.listPlayerInvite.Remove(item);
            this.dicPlayerInvite.Remove(ulong.Parse(id));
        }
    }

    public void ClearInviteMember()
    {
        this.listPlayerInvite.Clear();
        this.dicPlayerInvite.Clear();
    }

    public void RefreshTeamNemberInMap()
    {
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        UIMapController MapController = ControllerManager.Instance.GetController<UIMapController>();
        MapController.DeleteIconbyType(GameMap.ItemType.Team);
        string b = string.Empty;
        for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
        {
            if (MainPlayer.Self.EID.IdStr == this.myTeamInfo.mem[i].mememberid)
            {
                b = this.myTeamInfo.mem[i].sceneid;
                break;
            }
        }
        int num = 0;
        int j = 0;
        while (j < this.myTeamInfo.mem.Count)
        {
            Memember memember = this.myTeamInfo.mem[j];
            CharactorBase playerByID = manager.GetPlayerByID(ulong.Parse(memember.mememberid));
            if (playerByID == null)
            {
                goto IL_170;
            }
            if (playerByID != MainPlayer.Self)
            {
                if (!(memember.sceneid != b))
                {
                    if (playerByID.CurrMoveData != null)
                    {
                        MapController.SetTeamIconInfo(playerByID.EID, playerByID.CurrServerPos, num);
                    }
                    Action<CharactorBase> teamplayerMoveHandle = this.GetTeamplayerMoveHandle(num);
                    CharactorBase charactorBase = playerByID;
                    charactorBase.OnMoveDataChange = (Action<CharactorBase>)Delegate.Combine(charactorBase.OnMoveDataChange, teamplayerMoveHandle);
                    CharactorBase charactorBase2 = playerByID;
                    charactorBase2.OnDestroyThisInNineScreen = (Action<CharactorBase>)Delegate.Combine(charactorBase2.OnDestroyThisInNineScreen, new Action<CharactorBase>(delegate (CharactorBase CB)
                    {
                        MapController.DeleteIcon(CB, GameMap.ItemType.Team);
                    }));
                    goto IL_170;
                }
            }
        IL_174:
            j++;
            continue;
        IL_170:
            num++;
            goto IL_174;
        }
    }

    private Action<CharactorBase> GetTeamplayerMoveHandle(int num)
    {
        UIMapController MapController = ControllerManager.Instance.GetController<UIMapController>();
        return delegate (CharactorBase CB)
        {
            MapController.SetTeamIconInfo(CB.EID, CB.CurrServerPos, num);
        };
    }

    public void EnterTeam()
    {
        if (this.uiTeam != null && this.uiTeam.mRoot != null)
        {
            this.uiTeam.mRoot.gameObject.SetActive(!this.uiTeam.mRoot.gameObject.activeSelf);
            return;
        }
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Team>("UI_Team", delegate ()
        {
            if (this.uiTeam != null)
            {
                this.uiTeam.mRoot.gameObject.SetActive(false);
            }
            if (!this._isInitLuaPanel)
            {
                LuaPanelBase luaPanelBase = new LuaPanelBase(this.uiTeam.mRoot.gameObject);
                luaPanelBase.Awake(false);
                UIManager.AddLuaUIPanel(luaPanelBase);
            }
            this.EnterNoTeam();
            this.RefreshTeamNemberInMap();
        }, UIManager.ParentType.CommonUI, false);
    }

    public void EnterCreateTeam(Action createCallBack)
    {
        this.createTeamCallback = createCallBack;
        if (this.uiTeam != null)
        {
            this.uiTeam.enterCreateTeam();
        }
        else
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Team>("UI_Team", delegate ()
            {
                if (this.uiTeam != null)
                {
                    this.uiTeam.enterCreateTeam();
                }
                if (!this._isInitLuaPanel)
                {
                    LuaPanelBase luaPanelBase = new LuaPanelBase(this.uiTeam.mRoot.gameObject);
                    luaPanelBase.Awake(false);
                    UIManager.AddLuaUIPanel(luaPanelBase);
                }
            }, UIManager.ParentType.CommonUI, false);
        }
    }

    public void UpDateTeamMemberHero(MSG_updateTeamMemeberHero_SC data)
    {
        for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
        {
            if (this.myTeamInfo.mem[i].mememberid == data.memid)
            {
                this.myTeamInfo.mem[i].heroid = data.heroid;
                break;
            }
        }
        ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
    }

    public void UpDateTeamMember(MSG_updateTeamMememberCareer_SC data)
    {
        for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
        {
            if (this.myTeamInfo.mem[i].mememberid == data.mememberid)
            {
                this.myTeamInfo.mem[i].occupation = data.career;
                this.myTeamInfo.mem[i].occupationlevel = data.careerlevel;
                break;
            }
        }
        ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
        if (this.uiTeam != null)
        {
            this.uiTeam.ViewTeamInfo();
        }
        this.RefreshTeamNemberInMap();
    }

    public void UpDateTeamMemberLevel(MSG_updateTeamMememberLevel_SC data)
    {
        for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
        {
            if (this.myTeamInfo.mem[i].mememberid == data.mememberid)
            {
                this.myTeamInfo.mem[i].level = data.level;
                this.myTeamInfo.mem[i].name = data.membername;
                break;
            }
        }
        ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
        if (this.uiTeam != null)
        {
            this.uiTeam.ViewTeamInfo();
        }
        this.RefreshTeamNemberInMap();
    }

    public void UpDateTeamMemberFight(MSG_updateTeamMemberFight_SC data)
    {
        for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
        {
            if (this.myTeamInfo.mem[i].mememberid == data.mememberid)
            {
                this.myTeamInfo.mem[i].fight = data.fight;
                break;
            }
        }
        ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
    }

    public void UpDateTeamMemberAvatar(MSG_updateTeamMemberAvatar_SC data)
    {
        for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
        {
            if (this.myTeamInfo.mem[i].mememberid == data.mememberid)
            {
                this.myTeamInfo.mem[i].hairstyle = data.hairstyle;
                this.myTeamInfo.mem[i].haircolor = data.haircolor;
                this.myTeamInfo.mem[i].headstyle = data.headstyle;
                this.myTeamInfo.mem[i].bodystyle = data.bodystyle;
                this.myTeamInfo.mem[i].antenna = data.antenna;
                this.myTeamInfo.mem[i].avatarid = data.avatarid;
                break;
            }
        }
        ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
    }

    public void ClearTeamInfo()
    {
        this.myTeamInfo.id = 0U;
        this.myTeamInfo.leaderid = "0";
        this.myTeamInfo.mem.Clear();
        this.myTeamInfo.maxmember = 0U;
    }

    private void AnswerJoinSure()
    {
        MSG_AnswerJoinTeam_CS msg_AnswerJoinTeam_CS = new MSG_AnswerJoinTeam_CS();
        msg_AnswerJoinTeam_CS.requesterid = this.notifyLeaderData.requesterid;
        msg_AnswerJoinTeam_CS.answer_type = AnswerType.AnswerType_Yes;
        this.teamNetWork.ApplyAnswer(msg_AnswerJoinTeam_CS);
    }

    private void AnswerJoinCancel()
    {
        MSG_AnswerJoinTeam_CS msg_AnswerJoinTeam_CS = new MSG_AnswerJoinTeam_CS();
        msg_AnswerJoinTeam_CS.requesterid = this.notifyLeaderData.requesterid;
        msg_AnswerJoinTeam_CS.answer_type = AnswerType.AnswerType_No;
        this.teamNetWork.ApplyAnswer(msg_AnswerJoinTeam_CS);
    }

    public void JoinTeamNotifyLeader(MSG_ReqJoinTeamNotifyLeader_SC member)
    {
        this.notifyLeaderData = member;
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox("提示", member.requestername + "申请加入您的队伍", "同意", "拒绝", UIManager.ParentType.Tips, new Action(this.AnswerJoinSure), new Action(this.AnswerJoinCancel), null);
    }

    public void CloseTeam()
    {
        this.InviteWindowIsOpen = false;
        this.ReadMainViewMessage(MessageType.Friend);
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_Team");
    }

    public void ReqNearBy()
    {
        this.teamNetWork.ReqNearByPlayer();
    }

    public void RetNearByPlayer_SC(MSG_ReqNearByUnteamedPlayer_SC data)
    {
        if (this.uiTeam != null)
        {
            this.uiTeam.ViewNearbyPlayer(data);
        }
    }

    public void ReqInviteIntoTeam_CS(string id)
    {
        this.teamNetWork.ReqInviteIntoTeam_CS(id);
    }

    public void SetInviteWindow(bool b)
    {
        this.InviteWindowOnOff = b;
        if (!b)
        {
            this.noticeTime = Time.realtimeSinceStartup;
        }
        else
        {
            this.noticeTime = 0f;
        }
    }

    private void InviteSure()
    {
        if (this.mCurInviteData != null)
        {
            this.teamNetWork.AnswerInviteTeam_CS(true, this.mCurInviteData.inviteeid, this.mCurInviteData.inviterid, this.mCurInviteData.teamid);
        }
    }

    private void InviteCancel()
    {
        if (this.mCurInviteData != null)
        {
            this.teamNetWork.AnswerInviteTeam_CS(false, this.mCurInviteData.inviteeid, this.mCurInviteData.inviterid, this.mCurInviteData.teamid);
        }
    }

    public void RetInviteIntoTeam_SC(MSG_RetNearByUnteamedInvite_SC data)
    {
        this.mCurInviteData = data;
        if (!GameSystemSettings.IsAllowTeamInvite())
        {
            this.InviteCancel();
            return;
        }
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox("提示", data.invitername + "邀请你加入队伍", "加入", "取消", UIManager.ParentType.Tips, new Action(this.InviteSure), new Action(this.InviteCancel), null);
    }

    private void ViewInviteWindow(MSG_RetNearByUnteamedInvite_SC data)
    {
        if (this.uiTeam != null)
        {
            this.uiTeam.ViewNotifyPlayer(data);
        }
        else
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Team>("UI_Team", delegate ()
            {
                if (this.uiTeam != null)
                {
                    this.uiTeam.ViewNotifyPlayer(data);
                }
                if (!this._isInitLuaPanel)
                {
                    LuaPanelBase luaPanelBase = new LuaPanelBase(this.uiTeam.mRoot.gameObject);
                    luaPanelBase.Awake(false);
                    UIManager.AddLuaUIPanel(luaPanelBase);
                }
            }, UIManager.ParentType.CommonUI, false);
        }
    }

    private void viewMainUIMessage()
    {
        this.MainController.AddMessage(MessageType.Team, this.InviteCountNoRead, delegate
        {
            this.EnterTeam();
            this.MainController.ReadMessage(MessageType.Team);
        });
    }

    public void AnswerInviteTeam_CS(bool b, string inviteeid, string inviterid, uint teamid)
    {
        this.teamNetWork.AnswerInviteTeam_CS(b, inviteeid, inviterid, teamid);
    }

    public void ReqVoteOut_CS(string id)
    {
        this.teamNetWork.ReqVoteOut_CS(id.ToString());
    }

    public void RetVoteOut_SC(MSG_ReqLanchVoteOut_SC data)
    {
        if (this.uiTeam != null)
        {
            this.uiTeam.ViewVoteOut(data);
        }
        else
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Team>("UI_Team", delegate ()
            {
                if (this.uiTeam != null)
                {
                    this.uiTeam.ViewVoteOut(data);
                }
                if (!this._isInitLuaPanel)
                {
                    LuaPanelBase luaPanelBase = new LuaPanelBase(this.uiTeam.mRoot.gameObject);
                    luaPanelBase.Awake(false);
                    UIManager.AddLuaUIPanel(luaPanelBase);
                }
            }, UIManager.ParentType.CommonUI, false);
        }
    }

    public void ReqVoteTeamMate(bool bout)
    {
        this.teamNetWork.ReqVoteTeamMate(bout);
    }

    public void LeaderIgnoreNotice()
    {
        this.teamNetWork.leaderIgnoNotice();
    }

    public void Addmember(Memember mem)
    {
        bool flag = false;
        if (this.myTeamInfo.id == 0U)
        {
            return;
        }
        for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
        {
            if (this.myTeamInfo.mem[i].mememberid == mem.mememberid)
            {
                flag = true;
            }
        }
        if (!flag)
        {
            this.myTeamInfo.mem.Add(mem);
            ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
        }
        if (this.uiTeam != null)
        {
            this.IntoTeamInfo();
        }
        LuaScriptMgr.Instance.CallLuaFunction("PVPMatchCtrl.OnTeamMemberChange", new object[]
        {
            Util.GetLuaTable("PVPMatchCtrl")
        });
    }

    private void setMyTeamLeaderIndex()
    {
        Memember memember = null;
        for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
        {
            if (this.myTeamInfo.mem[i].mememberid == this.myTeamInfo.leaderid)
            {
                memember = this.myTeamInfo.mem[i];
                break;
            }
        }
        if (memember != null)
        {
            this.myTeamInfo.mem.Remove(memember);
            this.myTeamInfo.mem.Insert(0, memember);
        }
        ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
    }

    private void IntoTeamInfo()
    {
        if (this.createTeamCallback != null)
        {
            this.createTeamCallback();
            this.createTeamCallback = null;
            this.CloseTeam();
        }
        else if (this.uiTeam != null)
        {
            this.uiTeam.ViewTeamInfo();
        }
        LuaScriptMgr.Instance.CallLuaFunction("UIMapCtrl.SetAutoBtnActive", new object[]
        {
            "UIMapCtrl"
        });
        this.RefreshTeamNemberInMap();
    }

    public void ReqChangeLeader(string id)
    {
        this.teamNetWork.ReqChangeLeader(id.ToString());
    }

    public void RetChangeLeader(MSG_ReqChangeLeader_SC leader)
    {
        this.myTeamInfo.leaderid = leader.newid;
        this.setMyTeamLeaderIndex();
        this.IntoTeamInfo();
        if (this.IsMainPlayerLeader())
        {
            AttactFollowTeamLeader component = MainPlayer.Self.GetComponent<AttactFollowTeamLeader>();
            if (component != null && component.AutoAttackOn)
            {
                component.SwitchModle(false);
                AutoAttack component2 = MainPlayer.Self.GetComponent<AutoAttack>();
                if (component2 != null)
                {
                    component2.SwitchModle(true);
                }
            }
        }
    }

    public void RefreshTeamMemberHP(MSG_updateTeamMememberHp_SC data)
    {
        if (data.memid == MainPlayer.Self.OtherPlayerData.MapUserData.charid.ToString())
        {
            ControllerManager.Instance.GetController<MainUIController>().ResetMainPlayerHp(data.hp, data.maxhp);
        }
        else
        {
            for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
            {
                if (data.memid == this.myTeamInfo.mem[i].mememberid)
                {
                    this.myTeamInfo.mem[i].hp = data.hp;
                    this.myTeamInfo.mem[i].maxhp = data.maxhp;
                    ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
                    break;
                }
            }
        }
        ControllerManager.Instance.GetController<PVPMatchController>().RefreshGroupItemHp(data);
    }

    public void CreateTeam(MSG_CreateTeam_CS createTeamInfo)
    {
        this.teamNetWork.CreateTeam(createTeamInfo);
    }

    public void SetTeamInfo(MSG_TeamMemeberList_SC teamInfoData)
    {
        this.myTeamInfo = teamInfoData;
        ControllerManager.Instance.GetController<OccupyController>().RefreshOccupyNpcShow();
        if (this.myTeamInfo == null)
        {
            return;
        }
        if (this.myTeamInfo.mem.Count == 0)
        {
            if (this.listPlayerInvite.Count == 0)
            {
                if (this.uiTeam != null)
                {
                    this.uiTeam.EnterNoTeam();
                }
            }
            else if (this.uiTeam != null)
            {
                this.uiTeam.EnterTeamListInviteList(null);
            }
        }
        else
        {
            this.ClearInviteMember();
            this.setMyTeamLeaderIndex();
            if (this.EnterTeamCallBack != null)
            {
                this.EnterTeamCallBack();
                this.EnterTeamCallBack = null;
            }
            else
            {
                this.IntoTeamInfo();
            }
        }
        this.TryOpenMatchInfo();
    }

    public void TryOpenMatchInfo()
    {
        object[] array = LuaScriptMgr.Instance.CallLuaFunction("TeamCtrl.GetMatchState", new object[]
        {
            Util.GetLuaTable("TeamCtrl")
        });
        int num = int.Parse(array[0].ToString());
        if (num == 2 && this.uiTeam != null)
        {
            this.uiTeam.OpenTeamMatch(null);
        }
    }

    public void ApplyAnswer(MSG_AnswerJoinTeam_CS data)
    {
        this.teamNetWork.ApplyAnswer(data);
    }

    public void ReqTeamList()
    {
        this.teamNetWork.ReqTeamList();
    }

    public void ReqDelMember(string memid)
    {
        this.teamNetWork.ReqDelMember(memid.ToString());
    }

    public void RetDelMemember_SC(MSG_ReqDelMemember_SC deleteMebmber)
    {
        Memember memember = null;
        UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
        string b = ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.GetCharID().ToString();
        if (deleteMebmber.charid == b)
        {
            this.ClearTeamInfo();
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            CharactorBase playerByID = manager.GetPlayerByID(ulong.Parse(deleteMebmber.charid));
            this.SetTeamInfo(new MSG_TeamMemeberList_SC
            {
                id = 0U
            });
            ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
            if (this.uiTeam != null)
            {
                this.uiTeam.SetTeamState(TeamUIState.teamList);
            }
            LuaScriptMgr.Instance.CallLuaFunction("UIMapCtrl.SetAutoBtnActive", new object[]
            {
                "UIMapCtrl"
            });
            this.RefreshTeamNemberInMap();
            controller.SetupCharactorPos(null);
            UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
            if (mainView != null)
            {
                mainView.HideGroupTeam();
            }
            UIMapController controller2 = ControllerManager.Instance.GetController<UIMapController>();
            if (controller2 != null)
            {
                controller2.HideBattlePanel();
                controller2.CloseDuoqiBtnTip();
            }
            UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
            if (uiobject != null)
            {
                uiobject.SwitchChatModeUI(false);
            }
            return;
        }
        EntitiesManager manager2 = ManagerCenter.Instance.GetManager<EntitiesManager>();
        CharactorBase playerByID2 = manager2.GetPlayerByID(ulong.Parse(deleteMebmber.charid));
        controller.DeleteIconbyType(GameMap.ItemType.Team);
        if (playerByID2 != null)
        {
            this.RefreshTeamNemberInMap();
            OtherPlayer otherPlayer = playerByID2 as OtherPlayer;
            RelationType rt = manager2.CheckRelationBaseMainPlayer(playerByID2);
            UIMapController MapController = ControllerManager.Instance.GetController<UIMapController>();
            GameMap.ItemType it = MapController.GetItemTypeByRelationType(rt);
            MapController.SetItemIconInfoByItemPlayerAndItemType(playerByID2, it);
            playerByID2.OnMoveDataChange = null;
            CharactorBase charactorBase = playerByID2;
            charactorBase.OnMoveDataChange = (Action<CharactorBase>)Delegate.Combine(charactorBase.OnMoveDataChange, new Action<CharactorBase>(delegate (CharactorBase cb)
            {
                MapController.SetItemIconInfoByItemPlayerAndItemType(cb, it);
            }));
            CharactorBase charactorBase2 = playerByID2;
            charactorBase2.OnDestroyThisInNineScreen = (Action<CharactorBase>)Delegate.Combine(charactorBase2.OnDestroyThisInNineScreen, new Action<CharactorBase>(delegate (CharactorBase cb)
            {
                MapController.DeleteIcon(cb, it);
            }));
        }
        for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
        {
            if (this.myTeamInfo.mem[i].mememberid == deleteMebmber.charid)
            {
                memember = this.myTeamInfo.mem[i];
                break;
            }
        }
        if (memember != null)
        {
            this.myTeamInfo.mem.Remove(memember);
            ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
            if (ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.GetCharID().ToString() == deleteMebmber.charid && deleteMebmber.outtype == OutType.OutType_Quit)
            {
                TipsWindow.ShowWindow(TipsType.BE_OUT_TEAM, null);
            }
            this.IntoTeamInfo();
            LuaScriptMgr.Instance.CallLuaFunction("PVPMatchCtrl.OnTeamMemberChange", new object[]
            {
                Util.GetLuaTable("PVPMatchCtrl")
            });
            return;
        }
        FFDebug.LogWarning(this, "   tmpmem   ==  null  ");
    }

    public void AnswerJointeamList(MSG_AnswerJoinTeam_SC teamList)
    {
        if (teamList.teaminfo.mem.Count != 0)
        {
            this.myTeamInfo = teamList.teaminfo;
            this.ClearInviteMember();
            this.setMyTeamLeaderIndex();
            this.IntoTeamInfo();
            UIManager.Instance.DeleteUI<UI_Team>();
        }
    }

    public void ApplyTeam(uint teamid)
    {
        this.teamNetWork.ApplyTeam(teamid);
    }

    public void RetJoinTeam(uint retType)
    {
        if (retType == 1U)
        {
            TipsWindow.ShowWindow(TipsType.TEAM_FULL, null);
        }
    }

    public void ReqApplyList()
    {
        this.teamNetWork.ReqApplyList();
    }

    public void RetApplyList(MSG_ReqApplyList_SC applyList)
    {
        if (this.uiTeam != null)
        {
            this.uiTeam.ViewApplyList(applyList);
        }
        else
        {
            this.EnterTeamCallBack = delegate ()
            {
                if (this.uiTeam != null)
                {
                    this.uiTeam.ViewApplyList(applyList);
                }
            };
            this.EnterTeam();
        }
    }

    public void RetNewApply(int newApplyCount)
    {
        if (newApplyCount > 0)
        {
            this.MainController.AddMessage(MessageType.Team, newApplyCount, delegate
            {
                this.ReqApplyList();
                this.MainController.ReadMessage(MessageType.Team);
            });
        }
    }

    public void EnterNoTeam()
    {
        this.teamNetWork.TeamMemberList_CS();
    }

    public override void Awake()
    {
        this.teamNetWork = new TeamNetWork();
        this.teamNetWork.Initialize();
        ConvenientProcess.RegisertConvenientFunction(2U, new Action<List<VarType>>(this.ConvenientReqJoinTeam));
        this.DropItems.Clear();
    }

    public void ConvenientReqJoinTeam(List<VarType> paras)
    {
        if (paras.Count != 2)
        {
            FFDebug.LogWarning(this, "Invalid arguments to method,count: " + paras.Count);
            return;
        }
        if (ControllerManager.Instance.GetController<TeamController>().myTeamInfo.id != 0U)
        {
            TipsWindow.ShowWindow(TipsType.IN_TEAM, null);
            return;
        }
        uint num = paras[0];
        string inviterid = paras[1];
        if (num == 0U)
        {
            this.AnswerInviteTeam_CS(true, ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.GetCharID().ToString(), inviterid, num);
        }
        else
        {
            this.ApplyTeam(num);
        }
        FFDebug.Log(this, FFLogType.Default, "申请加入队伍 " + paras[0].ToString() + paras[1]);
    }

    public override void OnUpdate()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "team_controller";
        }
    }

    public void ReqTeamMemberPos()
    {
        if (this.myTeamInfo.mem.Count > 1)
        {
            this.teamNetWork.ReqTeamMemberPos();
        }
    }

    public void GetTeamMemberPos(List<MemberPos> List)
    {
        UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
        string b = string.Empty;
        for (int i = 0; i < List.Count; i++)
        {
            if (List[i].memberid == MainPlayer.Self.EID.IdStr)
            {
                b = List[i].sceneid;
                break;
            }
        }
        int num = 0;
        for (int j = 0; j < List.Count; j++)
        {
            MemberPos memberPos = List[j];
            EntitiesID eid = new EntitiesID(ulong.Parse(memberPos.memberid), CharactorType.Player);
            if (!eid.Equals(MainPlayer.Self.EID))
            {
                if (!(memberPos.sceneid != b))
                {
                    Vector2 serverPos = new Vector2(memberPos.x, memberPos.y);
                    controller.SetTeamIconInfo(eid, serverPos, num);
                    num++;
                }
            }
        }
    }

    public Memember GetTeamLaederInfo()
    {
        Memember result = null;
        if (this.myTeamInfo.mem.Count > 0)
        {
            for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
            {
                if (this.myTeamInfo.mem[i].mememberid == this.myTeamInfo.leaderid)
                {
                    result = this.myTeamInfo.mem[i];
                }
            }
        }
        return result;
    }

    public Memember GetTeamMememberInfo(ulong useid)
    {
        Memember result = null;
        if (this.myTeamInfo.mem.Count > 0)
        {
            for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
            {
                if (this.myTeamInfo.mem[i].mememberid == useid.ToString())
                {
                    result = this.myTeamInfo.mem[i];
                    break;
                }
            }
        }
        return result;
    }

    public void UpdateMemStateToTeam_SC(MSG_updateMemStateToTeam_SC msg)
    {
        if (this.myTeamInfo != null)
        {
            for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
            {
                if (this.myTeamInfo.mem[i].mememberid.Equals(msg.memid))
                {
                    this.myTeamInfo.mem[i].sceneid = msg.sceneid;
                    this.myTeamInfo.mem[i].state = msg.state;
                }
            }
            ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
            if (this.uiTeam != null)
            {
                this.uiTeam.ViewTeamInfo();
            }
        }
    }

    public void RetMemberBackTeam_SC(uint rettype)
    {
        if (rettype == 1U)
        {
            TipsWindow.ShowWindow(TipsType.SUCCESS_TO_TELL, null);
        }
        else
        {
            TipsWindow.ShowWindow(TipsType.LEADER_ASK_YOU_BACK, null);
        }
    }

    public void ReqMemberBackTeam_CS()
    {
        this.teamNetWork.ReqMemberBackTeam_CS();
    }

    public bool IgnoreWhite
    {
        get
        {
            return ControllerManager.Instance.GetController<UserSysSettingController>().GetSyssettingState(SYSSETTING.SETTING_ROLL_WHITE);
        }
    }

    public bool IgnoreGreen
    {
        get
        {
            return ControllerManager.Instance.GetController<UserSysSettingController>().GetSyssettingState(SYSSETTING.SETTING_ROLL_GREEN);
        }
    }

    public bool IgnoreBlue
    {
        get
        {
            return ControllerManager.Instance.GetController<UserSysSettingController>().GetSyssettingState(SYSSETTING.SETTING_ROLL_BLUE);
        }
    }

    public UI_PublicDrop uiPublicDrop
    {
        get
        {
            return UIManager.GetUIObject<UI_PublicDrop>();
        }
    }

    private void OnDataChange()
    {
        if (this.DropItems.Count == 0 && this.uiPublicDrop != null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_TeamAssign");
        }
        this.CheckShowPublicDropOnMain();
    }

    public void RefreshDropItemData(List<teamDropItem> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)data[i].objid);
            if (configTable.GetField_Uint("quality") == 1U && this.IgnoreWhite)
            {
                this.ReqChoose(data[i].thisid, ChooseType.ChooseType_Need);
            }
            else if (configTable.GetField_Uint("quality") == 2U && this.IgnoreGreen)
            {
                this.ReqChoose(data[i].thisid, ChooseType.ChooseType_Need);
            }
            else if (configTable.GetField_Uint("quality") == 3U && this.IgnoreBlue)
            {
                this.ReqChoose(data[i].thisid, ChooseType.ChooseType_Need);
            }
            else
            {
                this.DropItems[ulong.Parse(data[i].thisid)] = data[i];
            }
        }
        if (this.uiPublicDrop != null)
        {
            this.uiPublicDrop.InitView();
            this.uiPublicDrop.ShowThis();
        }
        else if (this.DropItems.Count > 0)
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_PublicDrop>("UI_TeamAssign", delegate ()
            {
                if (this.uiPublicDrop != null)
                {
                    this.uiPublicDrop.InitView();
                    this.uiPublicDrop.ShowThis();
                }
            }, UIManager.ParentType.CommonUI, false);
        }
        this.OnDataChange();
    }

    public void ProcessChooseTeamDrop(string id)
    {
        if (this.DropItems.ContainsKey(ulong.Parse(id)))
        {
            if (this.uiPublicDrop != null)
            {
                this.uiPublicDrop.OnProcessItem(ulong.Parse(id));
            }
            this.DropItems.Remove(ulong.Parse(id));
        }
        else
        {
            FFDebug.LogWarning(this, "ProcessChooseTeamDrop error does exit drop item with id: " + id);
        }
        this.OnDataChange();
    }

    public void ReqChoose(string id, ChooseType type)
    {
        this.teamNetWork.ReqChooseTeamDrop(id, type);
    }

    public void CheckShowPublicDropOnMain()
    {
        if (this.DropItems.Count > 0 && this.uiPublicDrop != null && !this.uiPublicDrop.isViewOn)
        {
            ControllerManager.Instance.GetController<MainUIController>().AddMessage(MessageType.Roll, 1, delegate
            {
                this.uiPublicDrop.ShowThis();
            });
            return;
        }
        ControllerManager.Instance.GetController<MainUIController>().ReadMessage(MessageType.Roll);
    }

    public void UpdateTeamActivity(uint actID)
    {
        this.myTeamInfo.activityid = actID;
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("team").GetCacheField_Table("activitylist").GetCacheField_Table(actID.ToString());
        if (cacheField_Table == null)
        {
            FFDebug.LogWarning(this, " UpdateTeamActivity cant find activity id = " + actID.ToString());
            return;
        }
        this.myTeamInfo.maxmember = cacheField_Table.GetCacheField_Uint("teamnum_limit");
        if (this.uiTeam != null)
        {
            this.uiTeam.UpdateTeamActivity(actID, this.myTeamInfo.maxmember);
        }
    }

    public void ReqSearchTeamByPage(uint targetPage, bool isNearBy)
    {
        this.teamNetWork.MSG_ReqSearchTeamByPage_CS(targetPage, isNearBy);
    }

    public void RetSearchTeam(MSG_RetSearchTeam_SC msg)
    {
        if (this.uiTeam != null)
        {
            this.uiTeam.ViewTeamListNew(msg);
        }
    }

    public void MSG_ReqLeaderMapPos_CS()
    {
        this.teamNetWork.MSG_ReqLeaderMapPos_CS();
    }

    public void MSG_ReqChangeMapToLeader_CS(string sceneidstr, MemberPos Mempos)
    {
        this.teamNetWork.MSG_ReqChangeMapToLeader_CS(sceneidstr, Mempos);
    }

    public void RetLeaderMapPos_SC(MSG_RetLeaderMapPos_SC data)
    {
        if (!data.pos.valid)
        {
            return;
        }
        ulong num = 0UL;
        ulong.TryParse(data.pos.sceneid, out num);
        if (ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.sceneID() == num)
        {
            if (MainPlayer.Self.GetComponent<FollowTeamLeader>() != null)
            {
                MainPlayer.Self.GetComponent<FollowTeamLeader>().FindPathToleader(data.pos.pos.x, data.pos.pos.y);
            }
        }
        else
        {
            this.MSG_ReqChangeMapToLeader_CS(data.pos.sceneid, data.pos.pos);
        }
    }

    public void RetChangeMapToLeader_SC(MSG_RetChangeMapToLeader_SC data)
    {
        if (data.retcode == 1U)
        {
            return;
        }
        if (MainPlayer.Self.GetComponent<FollowTeamLeader>() != null)
        {
            MainPlayer.Self.GetComponent<FollowTeamLeader>().FindPathToleader(data.info.pos.x, data.info.pos.y);
        }
    }

    public void MSG_ReqLeaderAttackTarget_CS()
    {
        this.teamNetWork.MSG_ReqLeaderAttackTarget_CS();
    }

    public void RetLeaderAttackTarget_SC(MSG_RetLeaderAttackTarget_SC Data)
    {
        if (MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>() != null)
        {
            MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().attackFollowTargetSelect.RetSetTarget(Data.target);
        }
    }

    public void DismissTeam()
    {
        this.teamNetWork.DismissTeam();
    }

    public void DismissTeamCb(bool issucc)
    {
        this.myTeamInfo = new MSG_TeamMemeberList_SC();
        ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
    }

    public void SetMemberPrivilege(ulong memberid, TeamPrivilege type, bool isSet)
    {
        this.teamNetWork.UpdateTeamMemberPrivilege(memberid, type, isSet);
    }

    public void UpdateTeamMemberPrivilegeCb(ulong memberid, uint privilege)
    {
        for (int i = 0; i < this.myTeamInfo.mem.Count; i++)
        {
            if (this.myTeamInfo.mem[i].mememberid == memberid.ToString())
            {
                this.myTeamInfo.mem[i].privilege = privilege;
                ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
                return;
            }
        }
    }

    public void SearchTeamidByMemberid(ulong memberid, Action<ulong, bool> cb)
    {
        this._serarchMemberid = memberid;
        this._searchTeamidCb = cb;
        this.teamNetWork.CheckUserTeamInfo(memberid);
    }

    public void SearchTeamidByMemberidCb(ulong teamid, bool online)
    {
        if (teamid == 0UL && this.GetTeamMememberInfo(this._serarchMemberid) != null)
        {
            teamid = (ulong)this.myTeamInfo.id;
        }
        this._searchTeamidCb(teamid, online);
    }

    public bool IsHasInviteAbility()
    {
        if (!this.IsMainPlayerHasTeam())
        {
            return false;
        }
        uint num = this.GetMainPlayerMemeber().privilege & 1U;
        return num != 0U;
    }

    public bool IsHasInviteAbility(uint privilege)
    {
        uint num = privilege & 1U;
        return num != 0U;
    }

    public bool IsLeader(string mememberid)
    {
        return this.myTeamInfo.leaderid == mememberid;
    }

    private TeamNetWork teamNetWork;

    public MSG_TeamMemeberList_SC myTeamInfo = new MSG_TeamMemeberList_SC();

    public BetterDictionary<ulong, teamDropItem> DropItems = new BetterDictionary<ulong, teamDropItem>();

    private bool _isInitLuaPanel;

    private Action createTeamCallback;

    private MSG_ReqJoinTeamNotifyLeader_SC notifyLeaderData;

    public bool InviteWindowOnOff = true;

    public bool InviteWindowIsOpen;

    public float noticeTime;

    public Dictionary<ulong, Memember> dicPlayerInvite = new Dictionary<ulong, Memember>();

    public List<Memember> listPlayerInvite = new List<Memember>();

    private int InviteCountNoRead;

    private MSG_RetNearByUnteamedInvite_SC mCurInviteData;

    private Action EnterTeamCallBack;

    private ulong _serarchMemberid;

    private Action<ulong, bool> _searchTeamidCb;
}
