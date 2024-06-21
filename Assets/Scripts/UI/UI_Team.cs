using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Team;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Team : UIPanelBase
{
    private TeamController teamController
    {
        get
        {
            return ControllerManager.Instance.GetController<TeamController>();
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.mRoot = root;
        this.InitGameObject();
        this.InitEvent();
    }

    public override void OnDispose()
    {
        base.OnDispose();
        this.UnInit();
        if (this.mRoot != null)
        {
            UnityEngine.Object.Destroy(this.mRoot.gameObject);
            this.mRoot = null;
        }
    }

    public void UnInit()
    {
        this.UIstate = TeamUIState.teamList;
        this.teamMember.Clear();
        for (int i = 0; i < this.listTeamItem.Count; i++)
        {
            UnityEngine.Object.Destroy(this.listTeamItem[i]);
        }
        for (int j = 0; j < this.nearbyGameObject.Count; j++)
        {
            UnityEngine.Object.Destroy(this.nearbyGameObject[j]);
        }
        for (int k = 0; k < this.applyListGameObject.Count; k++)
        {
            UnityEngine.Object.Destroy(this.applyListGameObject[k]);
        }
        this.listTeamItem.Clear();
        this.nearbyGameObject.Clear();
        this.applyListGameObject.Clear();
        this.selectTeamItem = null;
    }

    private void InitGameObject()
    {
        this.panelTeam = this.mRoot.Find("Offset_Team/PanelTeam");
        this.PanelTeamlist = this.mRoot.Find("Offset_Team/PanelTeamlist");
        this.PanelCreateTeam = this.mRoot.Find("Offset_Team/PanelCreateTeam");
        this.PanelInvite = this.mRoot.Find("Offset_Team/PanelInvite");
        this.PanelDismiss = this.mRoot.Find("Offset_Team/PanelDismiss");
        this.SecondMenu = this.panelTeam.Find("secondmenu");
        this.create = this.PanelCreateTeam.Find("create");
        this.teaminfo = this.panelTeam.Find("teaminfo");
        this.nearby = this.panelTeam.Find("nearby");
        this.applylist = this.panelTeam.Find("applylist");
        this.teamlist = this.PanelTeamlist.Find("teamlist");
        this.teamListInvitelist = this.PanelTeamlist.Find("invitelist");
        this.teamListNearby = this.PanelTeamlist.Find("nearby");
        this.PanelSearch = this.teamlist.Find("PanelSearch");
        this.close = this.panelTeam.Find("bg/btn_close").gameObject;
        this.teamListSecondMenu = this.PanelTeamlist.Find("secondmenu");
        this.teamListCreat = this.teamlist.Find("btn_create").gameObject;
        this.txt_MyTeam = this.SecondMenu.Find("btn_myteam/txt_tabinfo").GetComponent<Text>();
        this.txt_NearBy = this.SecondMenu.Find("btn_invite/txt_tabinfo").GetComponent<Text>();
        this.txt_Apply = this.SecondMenu.Find("btn_applylist/txt_tabinfo").GetComponent<Text>();
        this.tog_MyTeam = this.SecondMenu.Find("btn_myteam").GetComponent<Toggle>();
        this.tog_NearBy = this.SecondMenu.Find("btn_invite").GetComponent<Toggle>();
        this.tog_Apply = this.SecondMenu.Find("btn_applylist").GetComponent<Toggle>();
        this.uiinfo_MyTeam = this.SecondMenu.Find("btn_myteam/txt_tabinfo").GetComponent<UIInformationList>();
        this.uiinfo_NearBy = this.SecondMenu.Find("btn_invite/txt_tabinfo").GetComponent<UIInformationList>();
        this.uiinfo_Apply = this.SecondMenu.Find("btn_applylist/txt_tabinfo").GetComponent<UIInformationList>();
        this.txt_NoTeamMyTeam = this.teamListSecondMenu.Find("btn_search/txt_tabinfo").GetComponent<Text>();
        this.txt_NoTeamNearBy = this.teamListSecondMenu.Find("btn_Nearby/txt_tabinfo").GetComponent<Text>();
        this.txt_NoTeamApply = this.teamListSecondMenu.Find("btn_applylist/txt_tabinfo").GetComponent<Text>();
        this.tog_NoTeamMyTeam = this.teamListSecondMenu.Find("btn_search").GetComponent<Toggle>();
        this.tog_NoTeamNearBy = this.teamListSecondMenu.Find("btn_Nearby").GetComponent<Toggle>();
        this.tog_NoTeamApply = this.teamListSecondMenu.Find("btn_applylist").GetComponent<Toggle>();
        this.uiinfo_NoTeamMyTeam = this.teamListSecondMenu.Find("btn_search/txt_tabinfo").GetComponent<UIInformationList>();
        this.uiinfo_NoTeamNearBy = this.teamListSecondMenu.Find("btn_Nearby/txt_tabinfo").GetComponent<UIInformationList>();
        this.uiinfo_NoTeamApply = this.teamListSecondMenu.Find("btn_applylist/txt_tabinfo").GetComponent<UIInformationList>();
        this.PanelMatchTrans = this.mRoot.Find("Offset_Team/PanelMatching");
    }

    private void InitEvent()
    {
        this.InitTeamEvent();
        UIEventListener.Get(this.teamListCreat).onClick = delegate (PointerEventData evtData)
        {
            this.enterCreateTeam();
        };
        UIEventListener.Get(this.teamListNearby.Find("btn_create").gameObject).onClick = delegate (PointerEventData evtData)
        {
            this.enterCreateTeam();
        };
        UIEventListener.Get(this.teamListInvitelist.Find("btn_create").gameObject).onClick = delegate (PointerEventData evtData)
        {
            this.enterCreateTeam();
        };
        UIEventListener.Get(this.close).onClick = delegate (PointerEventData evtData)
        {
            this.Close();
        };
        UIEventListener.Get(this.PanelTeamlist.Find("bg/btn_close").gameObject).onClick = delegate (PointerEventData dat)
        {
            this.Close();
        };
        UIEventListener.Get(this.teamlist.Find("btn_apply").gameObject).onClick = delegate (PointerEventData dat)
        {
            this.applyTeam();
        };
        UIEventListener.Get(this.teamlist.Find("btn_search").gameObject).onClick = delegate (PointerEventData dat)
        {
            this.OpenSearchTeam();
        };
        UIEventListener.Get(this.teamlist.Find("btn_activity").gameObject).onClick = delegate (PointerEventData dat)
        {
            this.OpenTeamMatch(dat);
        };
    }

    public void Close()
    {
        this.teamController.CloseTeam();
    }

    public void SetTeamState(TeamUIState state)
    {
        for (int i = 0; i < this.listTeamItem.Count; i++)
        {
            this.listTeamItem[i].SetActive(false);
        }
        this.mRoot.gameObject.SetActive(true);
        this.PanelSearch.gameObject.SetActive(false);
        this.PanelTeamlist.gameObject.SetActive(false);
        this.PanelCreateTeam.gameObject.SetActive(false);
        this.PanelDismiss.gameObject.SetActive(false);
        this.PanelInvite.gameObject.SetActive(false);
        this.panelTeam.gameObject.SetActive(false);
        this.teaminfo.gameObject.SetActive(false);
        this.nearby.gameObject.SetActive(false);
        this.applylist.gameObject.SetActive(false);
        this.teamlist.gameObject.SetActive(false);
        this.create.gameObject.SetActive(false);
        this.PanelMatchTrans.gameObject.SetActive(false);
        this.setSecondMenu(state);
        switch (state)
        {
            case TeamUIState.teamInfo:
                this.panelTeam.gameObject.SetActive(true);
                this.teaminfo.gameObject.SetActive(true);
                break;
            case TeamUIState.applyList:
                this.panelTeam.gameObject.SetActive(true);
                this.applylist.gameObject.SetActive(true);
                break;
            case TeamUIState.teamList:
                this.PanelTeamlist.gameObject.SetActive(true);
                this.teamlist.gameObject.SetActive(true);
                this.teamListNearby.gameObject.SetActive(false);
                this.teamListInvitelist.gameObject.SetActive(false);
                break;
            case TeamUIState.nearBy:
                this.panelTeam.gameObject.SetActive(true);
                this.nearby.gameObject.SetActive(true);
                break;
            case TeamUIState.notifyLeader:
            case TeamUIState.notifyPlayer:
                this.PanelInvite.gameObject.SetActive(true);
                break;
            case TeamUIState.voteOut:
                this.PanelDismiss.gameObject.SetActive(true);
                break;
            case TeamUIState.searchTeam:
                this.PanelTeamlist.gameObject.SetActive(true);
                this.PanelSearch.gameObject.SetActive(true);
                this.teamlist.gameObject.SetActive(true);
                break;
            case TeamUIState.teamListNearby:
                this.PanelTeamlist.gameObject.SetActive(true);
                this.teamListNearby.gameObject.SetActive(true);
                this.teamlist.gameObject.SetActive(false);
                this.teamListInvitelist.gameObject.SetActive(false);
                break;
            case TeamUIState.teamListInvite:
                this.PanelTeamlist.gameObject.SetActive(true);
                this.teamListNearby.gameObject.SetActive(false);
                this.teamlist.gameObject.SetActive(false);
                this.teamListInvitelist.gameObject.SetActive(true);
                break;
            case TeamUIState.matching:
                this.PanelMatchTrans.gameObject.SetActive(true);
                break;
        }
        this.UIstate = state;
    }

    private void setSecondMenu(TeamUIState state)
    {
        if (this.uiinfo_MyTeam.listInformation.Count < 2 || this.uiinfo_NearBy.listInformation.Count < 2 || this.uiinfo_Apply.listInformation.Count < 2 || this.uiinfo_NoTeamMyTeam.listInformation.Count < 2 || this.uiinfo_NoTeamNearBy.listInformation.Count < 2 || this.uiinfo_NoTeamApply.listInformation.Count < 2)
        {
            FFDebug.Log(this, FFLogType.UI, "---------------------------------------    teamUI   second menu uiinformation is wrong! ");
            return;
        }
        switch (state)
        {
            case TeamUIState.teamInfo:
                this.tog_MyTeam.isOn = true;
                this.tog_NearBy.isOn = false;
                this.tog_Apply.isOn = false;
                this.txt_MyTeam.text = this.uiinfo_MyTeam.listInformation[0].content;
                this.txt_NearBy.text = this.uiinfo_NearBy.listInformation[1].content;
                this.txt_Apply.text = this.uiinfo_Apply.listInformation[1].content;
                break;
            case TeamUIState.applyList:
                this.tog_MyTeam.isOn = false;
                this.tog_NearBy.isOn = false;
                this.tog_Apply.isOn = true;
                this.txt_MyTeam.text = this.uiinfo_MyTeam.listInformation[1].content;
                this.txt_NearBy.text = this.uiinfo_NearBy.listInformation[1].content;
                this.txt_Apply.text = this.uiinfo_Apply.listInformation[0].content;
                break;
            case TeamUIState.teamList:
                this.tog_NoTeamMyTeam.isOn = true;
                this.tog_NoTeamNearBy.isOn = false;
                this.tog_NoTeamApply.isOn = false;
                this.txt_NoTeamMyTeam.text = this.uiinfo_NoTeamMyTeam.listInformation[0].content;
                this.txt_NoTeamNearBy.text = this.uiinfo_NoTeamNearBy.listInformation[1].content;
                this.txt_NoTeamApply.text = this.uiinfo_NoTeamApply.listInformation[1].content;
                break;
            case TeamUIState.nearBy:
                this.tog_MyTeam.isOn = false;
                this.tog_NearBy.isOn = true;
                this.tog_Apply.isOn = false;
                this.txt_MyTeam.text = this.uiinfo_MyTeam.listInformation[1].content;
                this.txt_NearBy.text = this.uiinfo_NearBy.listInformation[0].content;
                this.txt_Apply.text = this.uiinfo_Apply.listInformation[1].content;
                break;
            case TeamUIState.teamListNearby:
                this.tog_NoTeamMyTeam.isOn = false;
                this.tog_NoTeamNearBy.isOn = true;
                this.tog_NoTeamApply.isOn = false;
                this.txt_NoTeamMyTeam.text = this.uiinfo_NoTeamMyTeam.listInformation[1].content;
                this.txt_NoTeamNearBy.text = this.uiinfo_NoTeamNearBy.listInformation[0].content;
                this.txt_NoTeamApply.text = this.uiinfo_NoTeamApply.listInformation[1].content;
                break;
            case TeamUIState.teamListInvite:
                this.tog_NoTeamMyTeam.isOn = false;
                this.tog_NoTeamNearBy.isOn = false;
                this.tog_NoTeamApply.isOn = true;
                this.txt_NoTeamMyTeam.text = this.uiinfo_NoTeamMyTeam.listInformation[1].content;
                this.txt_NoTeamNearBy.text = this.uiinfo_NoTeamNearBy.listInformation[1].content;
                this.txt_NoTeamApply.text = this.uiinfo_NoTeamApply.listInformation[0].content;
                break;
        }
    }

    public void ViewTeamInfo()
    {
        this.SetTeamState(TeamUIState.teamInfo);
        int cacheField_Int = LuaConfigManager.GetXmlConfigTable("teamListMaxCount").GetCacheField_Table("mapinfo").GetCacheField_Table("teamlist").GetCacheField_Int("type");
        this.SecondMenu.Find("teamname/txt_teamname").GetComponent<Text>().text = ControllerManager.Instance.GetController<TeamController>().myTeamInfo.name;
        this.SecondMenu.Find("teamname/txt_teamid").GetComponent<Text>().text = "[ID  " + ControllerManager.Instance.GetController<TeamController>().myTeamInfo.id.ToString() + "]";
        UIFoldOutList component = this.teaminfo.Find("memberlist/UIFoldOutList").gameObject.GetComponent<UIFoldOutList>();
        component.InitListAction = new Action<int, GameObject>(this.setTeamInfoListItem);
        component.InitList(this.teamController.myTeamInfo.mem.Count, cacheField_Int, 1);
        UIInformationList component2 = this.panelTeam.Find("bg/txt_members").GetComponent<UIInformationList>();
        string contentByID = ControllerManager.Instance.GetController<TextModelController>().GetContentByID(component2.listInformation[0].id);
        this.panelTeam.Find("bg/txt_members").GetComponent<Text>().text = string.Format(contentByID, this.teamController.myTeamInfo.mem.Count.ToString(), this.teamController.myTeamInfo.maxmember.ToString());
        Text component3 = this.SecondMenu.Find("teamname/txt_target").GetComponent<Text>();
        Text component4 = this.SecondMenu.Find("teamname/txt_num").GetComponent<Text>();
        uint activityid = ControllerManager.Instance.GetController<TeamController>().myTeamInfo.activityid;
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("team").GetCacheField_Table("activitylist").GetCacheField_Table(activityid.ToString());
        component3.text = cacheField_Table.GetCacheField_String("name");
        uint maxmember = ControllerManager.Instance.GetController<TeamController>().myTeamInfo.maxmember;
        component4.text = string.Format(component4.gameObject.GetComponent<UIInformationList>().listInformation[0].content, maxmember);
    }

    private void setTeamInfoListItem(int i, GameObject ga)
    {
        if (ga == null)
        {
            return;
        }
        this.setTeamMember(ga, ControllerManager.Instance.GetController<TeamController>().myTeamInfo.mem[i]);
    }

    private void InitTeamEvent()
    {
        UIEventListener.Get(this.SecondMenu.Find("btn_invite").gameObject).onClick = new UIEventListener.VoidDelegate(this.enterNearby);
        UIEventListener.Get(this.SecondMenu.Find("btn_applylist").gameObject).onClick = new UIEventListener.VoidDelegate(this.applyList);
        UIEventListener.Get(this.panelTeam.Find("bg/btn_leave").gameObject).onClick = new UIEventListener.VoidDelegate(this.leaveTeam);
        UIEventListener.Get(this.panelTeam.Find("bg/btn_activity").gameObject).onClick = new UIEventListener.VoidDelegate(this.OpenTeamMatch);
        UIEventListener.Get(this.SecondMenu.Find("btn_myteam").gameObject).onClick = new UIEventListener.VoidDelegate(this.teamInfo);
    }

    private void InitTeamListEvent()
    {
        UIEventListener.Get(this.teamListSecondMenu.Find("btn_search").gameObject).onClick = new UIEventListener.VoidDelegate(this.EnterTeamList);
        UIEventListener.Get(this.teamListSecondMenu.Find("btn_Nearby").gameObject).onClick = new UIEventListener.VoidDelegate(this.enterNearby);
        UIEventListener.Get(this.teamListSecondMenu.Find("btn_applylist").gameObject).onClick = new UIEventListener.VoidDelegate(this.EnterTeamListInviteList);
    }

    private void EnterTeamList(PointerEventData data)
    {
        this.SetTeamState(TeamUIState.teamList);
    }

    private void enterNearby(PointerEventData data)
    {
        this.teamController.ReqNearBy();
    }

    private void applyList(PointerEventData data)
    {
        this.teamController.ReqApplyList();
    }

    private void leaveTeam(PointerEventData data)
    {
        string memid = ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.GetCharID().ToString();
        this.teamController.ReqDelMember(memid);
        this.teamController.myTeamInfo.id = 0U;
        ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
        this.Close();
    }

    public void OpenTeamMatch(PointerEventData data)
    {
        uint num = 1U;
        uint num2 = 1U;
        uint num3 = 0U;
        uint num4 = 0U;
        MSG_TeamMemeberList_SC myTeamInfo = ControllerManager.Instance.GetController<TeamController>().myTeamInfo;
        if (myTeamInfo != null && myTeamInfo.mem.Count > 0)
        {
            num2 = 2U;
            num = myTeamInfo.activityid;
            num3 = (uint)myTeamInfo.mem.Count;
            num4 = myTeamInfo.maxmember;
        }
        LuaScriptMgr.Instance.CallLuaFunction("TeamCtrl.OpenMatch", new object[]
        {
            Util.GetLuaTable("TeamCtrl"),
            num,
            num2,
            num3,
            num4
        });
    }

    private void setTeamMember(GameObject go, Memember member)
    {
        go.transform.Find("txt_tdname").GetComponent<Text>().text = member.name;
        go.transform.Find("txt_tdlv").GetComponent<Text>().text = member.level.ToString();
        Transform transform = go.transform.Find("btnlist/btnlist");
        if (member.mememberid == ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.GetCharID().ToString())
        {
            go.GetComponent<Button>().enabled = false;
            transform.Find("btn_leader").gameObject.SetActive(false);
            transform.Find("btn_info").gameObject.SetActive(false);
            transform.Find("btn_add").gameObject.SetActive(false);
        }
        else
        {
            go.GetComponent<Button>().enabled = true;
            if (member.state == MemState.AWAY)
            {
                transform.Find("btn_leader/img_disable").gameObject.SetActive(true);
                ControllerManager.Instance.GetController<TextModelController>().SetTextModel(transform.Find("btn_leader/Text").GetComponent<Text>(), string.Empty, 1);
            }
            else
            {
                ControllerManager.Instance.GetController<TextModelController>().SetTextModel(transform.Find("btn_leader/Text").GetComponent<Text>(), string.Empty, 0);
                transform.Find("btn_leader/img_disable").gameObject.SetActive(false);
            }
            transform.Find("btn_leader").gameObject.SetActive(true);
            transform.Find("btn_info").gameObject.SetActive(true);
            transform.Find("btn_add").gameObject.SetActive(true);
        }
        if (ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.GetCharID().ToString() != ControllerManager.Instance.GetController<TeamController>().myTeamInfo.leaderid)
        {
            transform.Find("btn_leader").gameObject.SetActive(false);
        }
        else
        {
            UIEventListener.Get(transform.Find("btn_leader").gameObject).onClick = delegate (PointerEventData data)
            {
                this.turnLeader(member);
            };
        }
        if (ControllerManager.Instance.GetController<TeamController>().myTeamInfo.leaderid == member.mememberid)
        {
            go.transform.Find("txt_tdname").GetComponent<Text>().color = Color.red;
        }
        else
        {
            go.transform.Find("txt_tdname").GetComponent<Text>().color = Color.white;
        }
        UIEventListener.Get(transform.Find("btn_info").gameObject).onClick = delegate (PointerEventData data)
        {
            this.viewMemberInfo(member);
        };
        UIEventListener.Get(transform.Find("btn_add").gameObject).onClick = delegate (PointerEventData data)
        {
            this.kickOut(member);
        };
        Text component = go.transform.Find("txt_tdstr").GetComponent<Text>();
        component.text = member.fight.ToString();
        RawImage Img_Hero = go.transform.Find("img_icon").GetComponent<RawImage>();
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)member.heroid);
        if (configTable == null)
        {
            FFDebug.LogWarning(this, "cant find hero config id = " + member.heroid);
            return;
        }
        base.GetTexture(ImageType.ROLES, configTable.GetField_String("icon"), delegate (Texture2D item)
        {
            if (Img_Hero != null && item != null)
            {
                Sprite sprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
                Img_Hero.texture = sprite.texture;
                Img_Hero.color = Color.white;
            }
        });
    }

    private void turnLeader(Memember memberData)
    {
        ControllerManager.Instance.GetController<TeamController>().ReqChangeLeader(memberData.mememberid);
    }

    private void addMember(Memember member)
    {
        if (this.teamController.myTeamInfo.activityid > 1U)
        {
            LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("team").GetCacheField_Table("activitylist").GetCacheField_Table(this.teamController.myTeamInfo.activityid.ToString());
            uint cacheField_Uint = cacheField_Table.GetCacheField_Uint("levellimit");
            if (member.level < cacheField_Uint)
            {
                TipsWindow.ShowWindow(TipsType.LEVEL_LOW_CANNOT_JOIN, null);
                return;
            }
        }
        this.inviteIntoTeam(member.mememberid);
    }

    private void viewMemberInfo(Memember memberData)
    {
    }

    private void kickOut(Memember memberData)
    {
        if (ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.GetCharID().ToString() == this.teamController.myTeamInfo.leaderid)
        {
            this.teamController.ReqDelMember(memberData.mememberid);
        }
        else
        {
            this.teamController.ReqVoteOut_CS(memberData.mememberid);
        }
    }

    public void UpdateTeamActivity(uint actID, uint maxNum)
    {
        Text component = this.SecondMenu.Find("teamname/txt_target").GetComponent<Text>();
        Text component2 = this.SecondMenu.Find("teamname/txt_num").GetComponent<Text>();
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("team").GetCacheField_Table("activitylist").GetCacheField_Table(actID.ToString());
        component.text = cacheField_Table.GetCacheField_String("name");
        component2.text = string.Format(component2.gameObject.GetComponent<UIInformationList>().listInformation[0].content, maxNum);
    }

    public void CheckCloseAllTeam()
    {
        if (this.UIstate == TeamUIState.matching)
        {
            this.teamController.CloseTeam();
        }
    }

    public void ViewApplyList(MSG_ReqApplyList_SC applyListData)
    {
        this.teamController.ReadMainViewMessage(MessageType.Friend);
        this.SetTeamState(TeamUIState.applyList);
        this.m_appleData = applyListData;
        if (this.m_appleData.applyer.Count == 0)
        {
            this.applylist.Find("txt_nolist").gameObject.SetActive(true);
        }
        else
        {
            this.applylist.Find("txt_nolist").gameObject.SetActive(false);
        }
        int cacheField_Int = LuaConfigManager.GetXmlConfigTable("teamListMaxCount").GetCacheField_Table("mapinfo").GetCacheField_Table("applylist").GetCacheField_Int("type");
        UIFoldOutList component = this.applylist.Find("plaerlist/UIFoldOutList").gameObject.GetComponent<UIFoldOutList>();
        component.InitListAction = new Action<int, GameObject>(this.setApplyListItem);
        component.InitList(this.m_appleData.applyer.Count, cacheField_Int, 1);
    }

    public void EnterTeamListInviteList(PointerEventData data)
    {
        this.teamController.ReadMainViewMessage(MessageType.Team);
        this.InitTeamListEvent();
        this.SetTeamState(TeamUIState.teamListInvite);
        this.m_InviteMem = this.teamController.listPlayerInvite;
        if (this.m_InviteMem.Count == 0)
        {
            this.teamListInvitelist.Find("txt_nolist").gameObject.SetActive(true);
        }
        else
        {
            this.teamListInvitelist.Find("txt_nolist").gameObject.SetActive(false);
        }
        int field_Int = LuaConfigManager.GetXmlConfigTable("teamListMaxCount").GetField_Table("mapinfo").GetField_Table("applylist").GetField_Int("type");
        UIFoldOutList component = this.teamListInvitelist.Find("plaerlist/UIFoldOutList").gameObject.GetComponent<UIFoldOutList>();
        component.InitListAction = new Action<int, GameObject>(this.setApplyListItem);
        component.InitList(this.m_InviteMem.Count, field_Int, 1);
    }

    private void noTeamRejectInvite(string id)
    {
        this.teamController.DeleteInviteMember(id);
        this.EnterTeamListInviteList(null);
    }

    private void setApplyListItem(int i, GameObject ga)
    {
        if (this.UIstate == TeamUIState.teamListInvite)
        {
            this.setApplyItem(ga, this.m_InviteMem[i]);
        }
        else
        {
            this.setApplyItem(ga, this.m_appleData.applyer[i]);
        }
    }

    private void setApplyItem(GameObject ga, Memember member)
    {
        ga.transform.Find("txt_tdname").GetComponent<Text>().text = member.name;
        ga.transform.Find("txt_tdlv").GetComponent<Text>().text = member.level.ToString();
        Transform transform = ga.transform.Find("btnlist/btnlist");
        if (ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.GetCharID().ToString() != ControllerManager.Instance.GetController<TeamController>().myTeamInfo.leaderid && ControllerManager.Instance.GetController<TeamController>().myTeamInfo.id != 0U)
        {
            transform.Find("btn_reject").gameObject.SetActive(false);
            transform.Find("btn_agree").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("btn_reject").gameObject.SetActive(true);
            transform.Find("btn_agree").gameObject.SetActive(true);
            UIEventListener.Get(transform.Find("btn_info").gameObject).onClick = delegate (PointerEventData data)
            {
                this.viewMemberInfo(member);
            };
            UIEventListener.Get(transform.Find("btn_reject").gameObject).onClick = delegate (PointerEventData data)
            {
                if (this.UIstate == TeamUIState.teamListInvite)
                {
                    this.noTeamRejectInvite(member.mememberid);
                }
                else
                {
                    this.rejectApply(member.mememberid);
                }
            };
            UIEventListener.Get(transform.Find("btn_agree").gameObject).onClick = delegate (PointerEventData data)
            {
                if (this.UIstate == TeamUIState.teamListInvite)
                {
                    this.AnswerInviteTeam_CS(true, ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.GetCharID().ToString(), member.mememberid);
                }
                else
                {
                    this.agreeApply(member.mememberid.ToString());
                }
            };
        }
        Text component = ga.transform.Find("txt_tdstr").GetComponent<Text>();
        component.text = member.fight.ToString();
        RawImage Img_Hero = ga.transform.Find("img_icon").GetComponent<RawImage>();
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)member.heroid);
        if (configTable == null)
        {
            FFDebug.LogWarning(this, "cant find hero config id = " + member.heroid);
            return;
        }
        base.GetTexture(ImageType.ROLES, configTable.GetField_String("icon"), delegate (Texture2D item)
        {
            if (Img_Hero != null && item != null)
            {
                Sprite sprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
                Img_Hero.texture = sprite.texture;
                Img_Hero.color = Color.white;
            }
        });
    }

    private void rejectApply(string id)
    {
        MSG_AnswerJoinTeam_CS msg_AnswerJoinTeam_CS = new MSG_AnswerJoinTeam_CS();
        msg_AnswerJoinTeam_CS.requesterid = id;
        msg_AnswerJoinTeam_CS.answer_type = AnswerType.AnswerType_No;
        this.teamController.ApplyAnswer(msg_AnswerJoinTeam_CS);
        Memember memember = null;
        if (this.m_appleData != null)
        {
            for (int i = 0; i < this.m_appleData.applyer.Count; i++)
            {
                if (id == this.m_appleData.applyer[i].mememberid)
                {
                    memember = this.m_appleData.applyer[i];
                }
            }
            if (memember != null)
            {
                this.m_appleData.applyer.Remove(memember);
            }
            this.ViewApplyList(this.m_appleData);
        }
        else
        {
            ControllerManager.Instance.GetController<TeamController>().CloseTeam();
        }
    }

    private void agreeApply(string id)
    {
        MSG_AnswerJoinTeam_CS msg_AnswerJoinTeam_CS = new MSG_AnswerJoinTeam_CS();
        msg_AnswerJoinTeam_CS.requesterid = id;
        msg_AnswerJoinTeam_CS.answer_type = AnswerType.AnswerType_Yes;
        ControllerManager.Instance.GetController<TeamController>().ApplyAnswer(msg_AnswerJoinTeam_CS);
    }

    private void teamInfo(PointerEventData data)
    {
        this.ViewTeamInfo();
    }

    public void ViewNearbyPlayer(MSG_ReqNearByUnteamedPlayer_SC nearbyList)
    {
        this.nearByData = nearbyList;
        this.maxCountNearBy = LuaConfigManager.GetXmlConfigTable("teamListMaxCount").GetCacheField_Table("mapinfo").GetCacheField_Table("playernearbylist").GetCacheField_Int("type");
        UIFoldOutList component;
        if (this.UIstate == TeamUIState.teamList || this.UIstate == TeamUIState.teamListNearby || this.UIstate == TeamUIState.teamListInvite)
        {
            this.SetTeamState(TeamUIState.teamListNearby);
            component = this.teamListNearby.Find("plaerlist/UIFoldOutList").gameObject.GetComponent<UIFoldOutList>();
            if (this.nearByData.mem.Count == 0)
            {
                this.teamListNearby.Find("txt_nolist").gameObject.SetActive(true);
            }
            else
            {
                this.teamListNearby.Find("txt_nolist").gameObject.SetActive(false);
            }
        }
        else
        {
            this.SetTeamState(TeamUIState.nearBy);
            component = this.nearby.Find("plaerlist/UIFoldOutList").gameObject.GetComponent<UIFoldOutList>();
            if (this.nearByData.mem.Count == 0)
            {
                this.nearby.Find("txt_nolist").gameObject.SetActive(true);
            }
            else
            {
                this.nearby.Find("txt_nolist").gameObject.SetActive(false);
            }
        }
        component.InitListAction = new Action<int, GameObject>(this.setNearByListItem);
        component.InitList(this.nearByData.mem.Count, this.maxCountNearBy, 1);
    }

    private void setNearByListItem(int i, GameObject ga)
    {
        this.setNearbyItem(ga, this.nearByData.mem[i]);
    }

    private void setNearbyItem(GameObject ga, Memember member)
    {
        ga.transform.Find("txt_tdname").GetComponent<Text>().text = member.name;
        ga.transform.Find("txt_tdlv").GetComponent<Text>().text = member.level.ToString();
        Transform transform = ga.transform.Find("btnlist");
        UIEventListener.Get(transform.Find("btnlist/btn_info").gameObject).onClick = delegate (PointerEventData data)
        {
            this.viewMemberInfo(member);
        };
        UIEventListener.Get(transform.Find("btnlist/btn_talk").gameObject).onClick = delegate (PointerEventData data)
        {
            this.privateChat(member);
        };
        if (ControllerManager.Instance.GetController<TeamController>().CheckIfTeamFull() && ControllerManager.Instance.GetController<TeamController>().myTeamInfo.id != 0U)
        {
            transform.Find("btnlist/btn_add").GetComponent<Image>().color = Color.grey;
        }
        else
        {
            transform.Find("btnlist/btn_add").GetComponent<Image>().color = Color.white;
        }
        UIEventListener.Get(transform.Find("btnlist/btn_add").gameObject).onClick = delegate (PointerEventData data)
        {
            this.addMember(member);
        };
        Text component = ga.transform.Find("txt_tdstr").GetComponent<Text>();
        component.text = member.fight.ToString();
        RawImage Img_Hero = ga.transform.Find("img_icon").GetComponent<RawImage>();
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)member.heroid);
        if (configTable == null)
        {
            FFDebug.LogWarning(this, "cant find hero config id = " + member.heroid);
            return;
        }
        base.GetTexture(ImageType.ROLES, configTable.GetField_String("icon"), delegate (Texture2D item)
        {
            if (Img_Hero != null && item != null)
            {
                Sprite sprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
                Img_Hero.texture = sprite.texture;
                Img_Hero.color = Color.white;
            }
        });
    }

    private void privateChat(Memember memember)
    {
    }

    private void inviteIntoTeamIfNoTeam(string id)
    {
        ControllerManager.Instance.GetController<TeamController>().ReqInviteIntoTeam_CS(id);
    }

    private void inviteIntoTeam(string id)
    {
        if (this.teamController.myTeamInfo.id != 0U && this.teamController.CheckIfTeamFull())
        {
            TipsWindow.ShowWindow(TipsType.TEAM_FULL, null);
            return;
        }
        if (this.teamController.CheckIfInTeam(id))
        {
            TipsWindow.ShowWindow(TipsType.INVITE_PLAYER_HAVE_TEAM, null);
            return;
        }
        this.teamController.ReqInviteIntoTeam_CS(id);
    }

    public bool IsReqNearBy
    {
        get
        {
            return this._isReqNearBy;
        }
        set
        {
            this._isReqNearBy = value;
        }
    }

    public void ViewTeamListNew(MSG_RetSearchTeam_SC data)
    {
        this.SetTeamState(TeamUIState.teamList);
        this.InitTeamListEvent();
        this.teamData2 = data;
        this.curPage = data.page;
        this.maxPage = data.totalpage;
        this.viewTeamListData(this.teamData2.teamlist);
        this.selectTeamObj = null;
    }

    private void viewTeamListData(List<MSG_TeamMemeberList_SC> teamListData)
    {
        this.mTeamListData = teamListData;
        UIFoldOutList component = this.teamlist.Find("ScrollbarRect").GetComponent<UIFoldOutList>();
        component.bUIFoldout = false;
        component.InitListAction = new Action<int, GameObject>(this.setTeamListItem);
        component.SetPageInfo((int)this.curPage, (int)this.maxPage, teamListData.Count, new UnityAction(this.ReqPrePageInfo), new UnityAction(this.ReqNextPageInfo));
        if (this.mTeamListData.Count != 0)
        {
            this.teamlist.Find("txt_nolist").gameObject.SetActive(false);
        }
        else
        {
            this.teamlist.Find("txt_nolist").gameObject.SetActive(true);
        }
    }

    private void ReqPrePageInfo()
    {
        if (this.curPage > 1U)
        {
            this.teamController.ReqSearchTeamByPage(this.curPage - 1U, this._isReqNearBy);
        }
    }

    private void ReqNextPageInfo()
    {
        if (this.curPage < this.maxPage)
        {
            this.teamController.ReqSearchTeamByPage(this.curPage + 1U, this._isReqNearBy);
        }
    }

    private void setTeamListItem(int i, GameObject ga)
    {
        this.viewTeamItem(ga, this.mTeamListData[i]);
    }

    private void viewTeamItem(GameObject ga, MSG_TeamMemeberList_SC teamItem)
    {
        ga.transform.Find("txt_tdid").GetComponent<Text>().text = teamItem.id.ToString();
        ga.transform.Find("txt_tdname").GetComponent<Text>().text = teamItem.name;
        UIInformationList component = ga.transform.Find("txt_tdmembers").GetComponent<UIInformationList>();
        string contentByID = ControllerManager.Instance.GetController<TextModelController>().GetContentByID(component.listInformation[0].id);
        ga.transform.Find("txt_tdmembers").GetComponent<Text>().text = string.Format(contentByID, teamItem.curmember.ToString(), teamItem.maxmember.ToString());
        ga.transform.Find("txt_tdneed").GetComponent<Text>().text = teamItem.note;
        UIEventListener.Get(ga.gameObject).onClick = delegate (PointerEventData data)
        {
            if (this.selectTeamObj != null)
            {
                this.selectTeamObj.transform.Find("sp_back/sp_check").gameObject.SetActive(false);
            }
            GameObject gameObject = ga.transform.Find("sp_back/sp_check").gameObject;
            gameObject.gameObject.SetActive(true);
            this.selectTeamObj = ga;
            this.selcetTeam(teamItem);
        };
        if (teamItem.activityid > 0U)
        {
            LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("team").GetCacheField_Table("activitylist").GetCacheField_Table(teamItem.activityid.ToString());
            ga.transform.Find("txt_tdneed").GetComponent<Text>().text = cacheField_Table.GetCacheField_String("name");
        }
    }

    private void selcetTeam(MSG_TeamMemeberList_SC teamItem)
    {
        this.selectTeamItem = teamItem;
    }

    private void applyTeam()
    {
        if (this.selectTeamItem != null)
        {
            ControllerManager.Instance.GetController<TeamController>().ApplyTeam(this.selectTeamItem.id);
            TipsWindow.ShowWindow(TipsType.APLLY_INTO_TEAM, new string[]
            {
                this.selectTeamItem.name
            });
        }
        else
        {
            TipsWindow.ShowWindow(TipsType.SELECT_PREFER_TEAM, null);
        }
    }

    private void OpenSearchTeam()
    {
        this.enterSearchTeam();
    }

    private void enterSearchTeam()
    {
        LuaScriptMgr.Instance.CallLuaFunction("TeamCtrl.OpenSearchTeam", new object[]
        {
            Util.GetLuaTable("TeamCtrl")
        });
    }

    private void ReqSearchTeam(PointerEventData data)
    {
    }

    private void viewOneTeam(MSG_TeamMemeberList_SC teamListData)
    {
        List<MSG_TeamMemeberList_SC> list = new List<MSG_TeamMemeberList_SC>();
        list.Add(teamListData);
        this.SetTeamState(TeamUIState.teamList);
        this.viewTeamListData(list);
    }

    public void enterCreateTeam()
    {
        LuaScriptMgr.Instance.CallLuaFunction("TeamCtrl.EnterCreateTeam", new object[]
        {
            Util.GetLuaTable("TeamCtrl")
        });
    }

    private void disableCreateTeam()
    {
        this.PanelCreateTeam.gameObject.SetActive(false);
    }

    private void createTeam()
    {
        MSG_CreateTeam_CS msg_CreateTeam_CS = new MSG_CreateTeam_CS();
        msg_CreateTeam_CS.name = this.create.Find("input_teamname").GetComponent<InputField>().text;
        if (StringTool.Instance.StringCount(msg_CreateTeam_CS.name) > 10)
        {
            TipsWindow.ShowWindow(TipsType.TEAM_NAME_LESS_TEN, null);
            return;
        }
        this.teamController.CreateTeam(msg_CreateTeam_CS);
    }

    public void EnterNoTeam()
    {
        LuaScriptMgr.Instance.CallLuaFunction("TeamCtrl.MSG_ReqSearchTeam_CS", new object[]
        {
            Util.GetLuaTable("TeamCtrl"),
            0,
            0,
            true
        });
        this._isReqNearBy = true;
        this.ViewTeamListNew(new MSG_RetSearchTeam_SC());
    }

    public void ViewNotifyLeader(MSG_ReqJoinTeamNotifyLeader_SC member)
    {
        this.SetTeamState(TeamUIState.notifyLeader);
        this.setNotifyLeader(member);
    }

    private void setNotifyLeader(MSG_ReqJoinTeamNotifyLeader_SC member)
    {
        UIInformationList component = this.PanelInvite.Find("txt_title").GetComponent<UIInformationList>();
        string id = component.listInformation[0].id;
        string contentByID = ControllerManager.Instance.GetController<TextModelController>().GetContentByID(id);
        this.PanelInvite.Find("txt_title").GetComponent<Text>().text = contentByID;
        component = this.PanelInvite.Find("txt_info2").GetComponent<UIInformationList>();
        id = component.listInformation[0].id;
        contentByID = ControllerManager.Instance.GetController<TextModelController>().GetContentByID(id);
        this.PanelInvite.Find("txt_info2").GetComponent<Text>().text = contentByID;
        this.PanelInvite.Find("txt_name").GetComponent<Text>().text = member.requestername;
        UIEventListener.Get(this.PanelInvite.Find("btn_ok").gameObject).onClick = delegate (PointerEventData evtData)
        {
            this.agreeApply(member.requesterid);
            if (this.PanelInvite.Find("Toggle").GetComponent<Toggle>().isOn)
            {
                this.teamController.LeaderIgnoreNotice();
            }
        };
        UIEventListener.Get(this.PanelInvite.Find("btn_cancel").gameObject).onClick = delegate (PointerEventData evtData)
        {
            this.rejectApply(member.requesterid);
            if (this.PanelInvite.Find("Toggle").GetComponent<Toggle>().isOn)
            {
                this.teamController.LeaderIgnoreNotice();
            }
        };
        UIEventListener.Get(this.PanelInvite.Find("btn_close").gameObject).onClick = delegate (PointerEventData evtData)
        {
            this.Close();
        };
    }

    public void ViewNotifyPlayer(MSG_RetNearByUnteamedInvite_SC member)
    {
        this.teamController.InviteWindowIsOpen = true;
        this.SetTeamState(TeamUIState.notifyPlayer);
        this.setNotifyPlayer(member);
    }

    private void setNotifyPlayer(MSG_RetNearByUnteamedInvite_SC member)
    {
        UIInformationList component = this.PanelInvite.Find("txt_title").GetComponent<UIInformationList>();
        string id = component.listInformation[1].id;
        string contentByID = ControllerManager.Instance.GetController<TextModelController>().GetContentByID(id);
        this.PanelInvite.Find("txt_title").GetComponent<Text>().text = contentByID;
        component = this.PanelInvite.Find("txt_info2").GetComponent<UIInformationList>();
        id = component.listInformation[1].id;
        contentByID = ControllerManager.Instance.GetController<TextModelController>().GetContentByID(id);
        this.PanelInvite.Find("txt_info2").GetComponent<Text>().text = contentByID;
        this.PanelInvite.Find("txt_name").GetComponent<Text>().text = member.invitername;
        UIEventListener.Get(this.PanelInvite.Find("btn_ok").gameObject).onClick = delegate (PointerEventData evtData)
        {
            this.AnswerInviteTeam_CS(true, member.inviteeid, member.inviterid);
            if (this.PanelInvite.Find("Toggle").GetComponent<Toggle>().isOn)
            {
                this.teamController.SetInviteWindow(false);
            }
            else
            {
                this.teamController.SetInviteWindow(true);
            }
        };
        UIEventListener.Get(this.PanelInvite.Find("btn_cancel").gameObject).onClick = delegate (PointerEventData evtData)
        {
            this.AnswerInviteTeam_CS(false, member);
            if (this.PanelInvite.Find("Toggle").GetComponent<Toggle>().isOn)
            {
                this.teamController.SetInviteWindow(false);
            }
            else
            {
                this.teamController.SetInviteWindow(true);
            }
        };
        UIEventListener.Get(this.PanelInvite.Find("btn_close").gameObject).onClick = delegate (PointerEventData evtData)
        {
            this.teamController.CloseTeam();
            if (this.PanelInvite.Find("Toggle").GetComponent<Toggle>().isOn)
            {
                this.teamController.SetInviteWindow(false);
            }
            else
            {
                this.teamController.SetInviteWindow(true);
            }
        };
    }

    public void AnswerInviteTeam_CS(bool b, string inviteeid, string inviterid)
    {
    }

    public void AnswerInviteTeam_CS(bool b, MSG_RetNearByUnteamedInvite_SC mem)
    {
        this.teamController.CloseTeam();
    }

    public void ViewVoteOut(MSG_ReqLanchVoteOut_SC data)
    {
        this.SetTeamState(TeamUIState.voteOut);
        this.voteOut(data);
    }

    private void voteOut(MSG_ReqLanchVoteOut_SC data)
    {
        this.totaletime = float.Parse(data.duration);
        string id = this.PanelDismiss.Find("txt_name").GetComponent<UIInformationList>().listInformation[0].id;
        string contentByID = ControllerManager.Instance.GetController<TextModelController>().GetContentByID(id);
        this.PanelDismiss.Find("txt_name").GetComponent<Text>().text = string.Format(contentByID, data.lanchername, data.outername);
        id = this.PanelDismiss.Find("txt_info2").GetComponent<UIInformationList>().listInformation[0].id;
        contentByID = ControllerManager.Instance.GetController<TextModelController>().GetContentByID(id);
        this.PanelDismiss.Find("txt_info2").GetComponent<Text>().text = contentByID;
        Slider component = this.PanelDismiss.Find("Slider").GetComponent<Slider>();
        this.curTime = (int)this.totaletime;
        this.PanelDismiss.Find("txt_time").GetComponent<Text>().text = this.curTime.ToString();
        UIEventListener.Get(this.PanelDismiss.Find("btn_ok").gameObject).onClick = delegate (PointerEventData evtData)
        {
            this.voteOutTeamMate(true);
        };
        UIEventListener.Get(this.PanelDismiss.Find("btn_cancel").gameObject).onClick = delegate (PointerEventData evtData)
        {
            this.voteOutTeamMate(false);
        };
        UIEventListener.Get(this.PanelDismiss.Find("btn_close").gameObject).onClick = delegate (PointerEventData evtData)
        {
            this.voteOutTeamMate(false);
        };
        SingletonForMono<InputController>.Instance.StartCoroutine(this.DoCount());
    }

    private IEnumerator DoCount()
    {
        while (this.curTime > 0)
        {
            yield return new WaitForSeconds(1f);
            this.curTime--;
            Slider timeSlider = this.PanelDismiss.Find("Slider").GetComponent<Slider>();
            timeSlider.value = (float)this.curTime / this.totaletime;
            this.PanelDismiss.Find("txt_time").GetComponent<Text>().text = this.curTime.ToString();
        }
        yield return new WaitForSeconds(0f);
        this.voteOutTeamMate(false);
        yield break;
    }

    private void voteOutTeamMate(bool b)
    {
        this.teamController.ReqVoteTeamMate(b);
        this.curTime = 0;
        this.Close();
    }

    public TeamUIState UIstate = TeamUIState.teamList;

    public Transform mRoot;

    public Transform panelTeam;

    public Transform PanelTeamlist;

    public Transform PanelCreateTeam;

    private Transform PanelInvite;

    private Transform PanelDismiss;

    private Transform SecondMenu;

    private Text txt_MyTeam;

    private Text txt_NearBy;

    private Text txt_Apply;

    private Toggle tog_MyTeam;

    private Toggle tog_NearBy;

    private Toggle tog_Apply;

    private UIInformationList uiinfo_MyTeam;

    private UIInformationList uiinfo_NearBy;

    private UIInformationList uiinfo_Apply;

    private Text txt_NoTeamMyTeam;

    private Text txt_NoTeamNearBy;

    private Text txt_NoTeamApply;

    private Toggle tog_NoTeamMyTeam;

    private Toggle tog_NoTeamNearBy;

    private Toggle tog_NoTeamApply;

    private UIInformationList uiinfo_NoTeamMyTeam;

    private UIInformationList uiinfo_NoTeamNearBy;

    private UIInformationList uiinfo_NoTeamApply;

    private Transform create;

    private Transform teaminfo;

    private Transform nearby;

    private Transform applylist;

    private Transform teamlist;

    private Transform PanelSearch;

    private GameObject close;

    private Transform teamListSecondMenu;

    private Transform PanelMatchTrans;

    private GameObject teamListCreat;

    private Transform teamListInvitelist;

    private Transform teamListNearby;

    private List<GameObject> teamMember = new List<GameObject>();

    private List<GameObject> applyListGameObject = new List<GameObject>();

    private MSG_ReqApplyList_SC m_appleData;

    private List<Memember> m_InviteMem = new List<Memember>();

    private int currentPageNearBy = 1;

    private int maxCountNearBy;

    private MSG_ReqNearByUnteamedPlayer_SC nearByData;

    private List<GameObject> nearbyGameObject = new List<GameObject>();

    private MSG_TeamMemeberList_SC selectTeamItem;

    private List<GameObject> listTeamItem = new List<GameObject>();

    private GameObject selectTeamObj;

    private MSG_RetSearchTeam_SC teamData2;

    private int maxCount;

    private uint curPage;

    private uint maxPage;

    private bool _isReqNearBy;

    private int curPostion = 60;

    private List<MSG_TeamMemeberList_SC> mTeamListData;

    private float totaletime = 10f;

    private int curTime;
}
