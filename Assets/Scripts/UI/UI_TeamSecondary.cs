using System;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using relation;
using Team;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TeamSecondary : UIPanelBase
{
    private TeamController teamController
    {
        get
        {
            return ControllerManager.Instance.GetController<TeamController>();
        }
    }

    private FriendControllerNew mFriendControllerNew
    {
        get
        {
            return ControllerManager.Instance.GetController<FriendControllerNew>();
        }
    }

    private FriendNetWork mFriendNetWork
    {
        get
        {
            return this.mFriendControllerNew.mFriendNetWork;
        }
    }

    public override void OnInit(Transform _root)
    {
        Transform transform = _root.Find("Panel");
        this.Root = transform.gameObject;
        this.btnTeamInvite = transform.Find("btnTeamInvite").gameObject;
        this.btnTeamApply = transform.Find("btnTeamApply").gameObject;
        this.btnFireTeam = transform.Find("btnFireTeam").gameObject;
        this.btnGiveAbility = transform.Find("btnGiveAbility").gameObject;
        this.btnDeleAbility = transform.Find("btnDeleAbility").gameObject;
        this.btnLeaveTeam = transform.Find("LeaveTeam").gameObject;
        this.btnTurnLeader = transform.Find("TurnLeader").gameObject;
        this.btnViewMemberInfo = transform.Find("ViewMemberInfo").gameObject;
        this.btnShitu = transform.Find("Shitu").gameObject;
        this.btnPrivateChat = transform.Find("PrivateChat").gameObject;
        this.btnMiyuChat = transform.Find("MiyuButton").gameObject;
        this.btnAddFriend = transform.Find("AddFriend").gameObject;
        this.btnKickOut = transform.Find("KickOut").gameObject;
        this.btnBlackNameBook = transform.Find("BlackNameBook").gameObject;
        this.btnTanheLeader = transform.Find("TanheLeader").gameObject;
        this.btnGuildInvite = transform.Find("Invitation").gameObject;
        UIEventListener.Get(this.btnTeamInvite).onClick = new UIEventListener.VoidDelegate(this.btnTeamInvite_on_click);
        UIEventListener.Get(this.btnTeamApply).onClick = new UIEventListener.VoidDelegate(this.btnTeamApply_on_click);
        UIEventListener.Get(this.btnFireTeam).onClick = new UIEventListener.VoidDelegate(this.btnFireTeam_on_click);
        UIEventListener.Get(this.btnGiveAbility).onClick = new UIEventListener.VoidDelegate(this.btnGiveAbility_on_click);
        UIEventListener.Get(this.btnDeleAbility).onClick = new UIEventListener.VoidDelegate(this.btnDeleAbility_on_click);
        _root.GetComponent<Image>().raycastTarget = false;
        UIEventListener.Get(_root.gameObject).onClick = new UIEventListener.VoidDelegate(this.btnMask_on_click);
        UIEventListener.Get(this.btnLeaveTeam).onClick = new UIEventListener.VoidDelegate(this.btnLeaveTeam_on_click);
        UIEventListener.Get(this.btnTurnLeader).onClick = new UIEventListener.VoidDelegate(this.btnTurnLeader_on_click);
        UIEventListener.Get(this.btnViewMemberInfo).onClick = new UIEventListener.VoidDelegate(this.btnViewMemberInfo_on_click);
        UIEventListener.Get(this.btnShitu).onClick = new UIEventListener.VoidDelegate(this.btnShitu_on_click);
        UIEventListener.Get(this.btnPrivateChat).onClick = new UIEventListener.VoidDelegate(this.btnPrivateChat_on_click);
        UIEventListener.Get(this.btnMiyuChat).onClick = new UIEventListener.VoidDelegate(this.btnMiyuChat_on_click);
        UIEventListener.Get(this.btnAddFriend).onClick = new UIEventListener.VoidDelegate(this.btnAddFriend_on_click);
        UIEventListener.Get(this.btnKickOut).onClick = new UIEventListener.VoidDelegate(this.btnKickOut_on_click);
        UIEventListener.Get(this.btnBlackNameBook).onClick = new UIEventListener.VoidDelegate(this.btnBlcakNameBook_on_click);
        UIEventListener.Get(this.btnTanheLeader).onClick = new UIEventListener.VoidDelegate(this.btnTanheLeader_on_click);
        UIEventListener.Get(this.btnGuildInvite).onClick = new UIEventListener.VoidDelegate(this.btnGuildInvite_on_click);
        base.OnInit(_root);
    }

    private void Close()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_TeamSecondary");
    }

    private void btnTeamInvite_on_click(PointerEventData data)
    {
        this.teamController.ReqInviteIntoTeam_CS(this.memeber.mememberid);
        this.Close();
    }

    private void btnTeamApply_on_click(PointerEventData data)
    {
        this.teamController.ApplyTeam((uint)this.mTeamid);
        this.Close();
    }

    private void btnFireTeam_on_click(PointerEventData data)
    {
        this.teamController.DismissTeam();
        this.Close();
    }

    private void btnGiveAbility_on_click(PointerEventData data)
    {
        this.teamController.SetMemberPrivilege(ulong.Parse(this.memeber.mememberid), TeamPrivilege.TeamPrivilege_Invite, true);
        this.Close();
    }

    private void btnDeleAbility_on_click(PointerEventData data)
    {
        this.teamController.SetMemberPrivilege(ulong.Parse(this.memeber.mememberid), TeamPrivilege.TeamPrivilege_Invite, false);
        this.Close();
    }

    private void btnTanheLeader_on_click(PointerEventData data)
    {
        this.teamController.ReqVoteOut_CS(this.memeber.mememberid);
        this.Close();
    }

    private void btnGuildInvite_on_click(PointerEventData data)
    {
        GuildControllerNew controller = ControllerManager.Instance.GetController<GuildControllerNew>();
        controller.GuildInvite(this.memeber.mememberid);
        this.Close();
    }

    private void btnMask_on_click(PointerEventData data)
    {
        this.Close();
    }

    private void btnLeaveTeam_on_click(PointerEventData data)
    {
        string memid = ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.GetCharID().ToString();
        this.teamController.ReqDelMember(memid);
        this.teamController.myTeamInfo.id = 0U;
        ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
        this.Close();
    }

    private void btnTurnLeader_on_click(PointerEventData data)
    {
        ControllerManager.Instance.GetController<TeamController>().ReqChangeLeader(this.memeber.mememberid);
        this.Close();
    }

    private void btnViewMemberInfo_on_click(PointerEventData data)
    {
        this.Close();
    }

    private void btnShitu_on_click(PointerEventData data)
    {
        this.Close();
    }

    private void btnPrivateChat_on_click(PointerEventData data)
    {
        UI_FriendPrivateChat uiFriendPrivateChat = UIManager.GetUIObject<UI_FriendPrivateChat>();
        relation_item itemData = ControllerManager.Instance.GetController<FriendControllerNew>().GetFriend(ulong.Parse(this.memeber.mememberid));
        if (itemData != null)
        {
            if (uiFriendPrivateChat == null)
            {
                UIManager.Instance.ShowUI<UI_FriendPrivateChat>("UI_FriendPrivateChat", delegate ()
                {
                    uiFriendPrivateChat = UIManager.GetUIObject<UI_FriendPrivateChat>();
                    uiFriendPrivateChat.AddPrivateChatFriend(itemData);
                }, UIManager.ParentType.CommonUI, false);
            }
            else
            {
                uiFriendPrivateChat.AddPrivateChatFriend(itemData);
            }
        }
        this.Close();
    }

    private void btnMiyuChat_on_click(PointerEventData data)
    {
        ControllerManager.Instance.GetController<ChatControl>().MiyuToMember(this.memeber.name);
        this.Close();
    }

    private void btnAddFriend_on_click(PointerEventData data)
    {
        ControllerManager.Instance.GetController<FriendControllerNew>().ReqApplyFriend(ulong.Parse(this.memeber.mememberid));
        this.Close();
    }

    private void btnKickOut_on_click(PointerEventData data)
    {
        this.teamController.ReqDelMember(this.memeber.mememberid);
        this.Close();
    }

    private void btnBlcakNameBook_on_click(PointerEventData data)
    {
        string typestr = "2";
        UIManager.Instance.ShowUI<UI_MessageBox>("UI_MessageBox", delegate ()
        {
            UI_MessageBox uiobject = UIManager.GetUIObject<UI_MessageBox>();
            uiobject.SetOkCb(new MessageOkCb2(this.ReqAddBlackOkCb), typestr);
            string contentByID = ControllerManager.Instance.GetController<TextModelController>().GetContentByID("900021");
            uiobject.SetContent(contentByID, "提示", true);
        }, UIManager.ParentType.CommonUI, false);
    }

    private void ReqAddBlackOkCb(string data)
    {
        this.mFriendNetWork.ReqOperateBlackList(ulong.Parse(this.memeber.mememberid), uint.Parse(data));
    }

    public void Initilize(Memember _memeber, PointerEventData _eventData, bool isGroupItem = false)
    {
        if (ManagerCenter.Instance.GetManager<GameScene>().isAbattoirScene)
        {
            this.InitilizeAbattoir(_memeber, _eventData);
            return;
        }
        this.mIsGroupItem = isGroupItem;
        this.memeber = _memeber;
        this.eventData = _eventData;
        this.teamController.SearchTeamidByMemberid(ulong.Parse(this.memeber.mememberid), new Action<ulong, bool>(this.InitilizeDo));
        this.SetTeamPos();
    }

    private void InitilizeDo(ulong teamid, bool online)
    {
        this.mTeamid = teamid;
        bool flag = false;
        uint mapid = ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID();
        LuaTable copymapInfoByMapid = CommonTools.GetCopymapInfoByMapid(mapid);
        if (copymapInfoByMapid != null)
        {
            flag = (copymapInfoByMapid.GetField_Uint("showinvite") == 1U);
        }
        if (this.memeber.mememberid == MainPlayer.Self.OtherPlayerData.MapUserData.charid.ToString())
        {
            this.btnTeamInvite.SetActive(false);
            this.btnTeamApply.SetActive(false);
            this.btnFireTeam.SetActive(this.teamController.IsMainPlayerLeader() && !flag);
            this.btnGiveAbility.SetActive(false);
            this.btnDeleAbility.SetActive(false);
            this.btnLeaveTeam.SetActive(this.teamController.IsMainPlayerHasTeam() && !flag);
            this.btnTurnLeader.SetActive(false);
            this.btnViewMemberInfo.SetActive(false);
            this.btnShitu.SetActive(false);
            this.btnPrivateChat.SetActive(false);
            this.btnAddFriend.SetActive(false);
            this.btnKickOut.SetActive(false);
            this.btnBlackNameBook.SetActive(false);
            this.btnTanheLeader.SetActive(false);
            this.btnGuildInvite.SetActive(false);
            this.btnMiyuChat.SetActive(false);
        }
        else
        {
            bool flag2 = this.teamController.IsMainPlayerHasTeam();
            bool flag3 = this.teamController.IsMainPlayerLeader();
            this.btnTeamInvite.SetActive(online && (this.teamController.IsHasInviteAbility() || !flag2) && this.mTeamid == 0UL);
            this.btnTeamApply.SetActive(online && !flag2 && this.mTeamid != 0UL);
            this.btnFireTeam.SetActive(false);
            bool flag4 = (ulong)this.teamController.myTeamInfo.id == this.mTeamid;
            bool flag5 = (this.memeber.privilege & 1U) != 0U;
            if (this.mIsGroupItem)
            {
                if (UIManager.GetUIObject<UI_MainView>().mMemDic != null)
                {
                    ulong captain = UIManager.GetUIObject<UI_MainView>().mMemDic[0].captain;
                    flag3 = (captain == MainPlayer.Self.GetCharID());
                }
                flag4 = true;
                flag5 = false;
            }
            this.btnGiveAbility.SetActive(flag3 && flag4 && !flag5);
            this.btnDeleAbility.SetActive(flag3 && flag4 && flag5);
            this.btnLeaveTeam.SetActive(false);
            this.btnKickOut.SetActive(flag3 && flag4 && !flag);
            this.btnTanheLeader.SetActive(false);
            this.btnTurnLeader.SetActive(flag3 && flag4);
            this.btnViewMemberInfo.SetActive(false);
            this.btnShitu.SetActive(false);
            this.btnBlackNameBook.SetActive(true);
            FriendControllerNew controller = ControllerManager.Instance.GetController<FriendControllerNew>();
            ulong num = ulong.Parse(this.memeber.mememberid);
            if (controller.IsFriend(num))
            {
                this.btnAddFriend.SetActive(false);
                this.btnPrivateChat.SetActive(false);
            }
            else if (controller.mBlackDic.ContainsKey(num))
            {
                this.btnBlackNameBook.SetActive(false);
            }
            else
            {
                this.btnAddFriend.SetActive(true);
                this.btnPrivateChat.SetActive(false);
            }
            this.btnMiyuChat.SetActive(true);
            GuildControllerNew controller2 = ControllerManager.Instance.GetController<GuildControllerNew>();
            this.btnGuildInvite.SetActive(controller2 != null && controller2.HasInvitePower());
        }
        Scheduler.Instance.AddFrame(2U, false, new Scheduler.OnScheduler(this.CheckTeamPos));
        for (int i = 0; i < this.Root.transform.childCount; i++)
        {
            if (this.Root.transform.GetChild(i).gameObject.activeSelf)
            {
                this.Root.SetActive(true);
                return;
            }
        }
        this.Close();
    }

    private void SetGroupItem()
    {
        this.btnMiyuChat.SetActive(true);
        this.btnTeamInvite.SetActive(false);
        this.btnTeamApply.SetActive(false);
        this.btnFireTeam.SetActive(false);
        this.btnGiveAbility.SetActive(false);
        this.btnDeleAbility.SetActive(false);
        this.btnLeaveTeam.SetActive(false);
        this.btnKickOut.SetActive(false);
        this.btnTanheLeader.SetActive(false);
        this.btnShitu.SetActive(false);
        this.btnPrivateChat.SetActive(false);
        this.btnBlackNameBook.SetActive(false);
        this.btnGuildInvite.SetActive(false);
    }

    private void SetTeamPos()
    {
        this.Root.transform.localPosition = CommonTools.GetSecondPanelPos();
    }

    private void CheckTeamPos()
    {
        if (this.Root == null)
        {
            return;
        }
        this.Root.transform.localPosition = CommonTools.GetSecondPanelAreaPos((this.Root.transform as RectTransform).sizeDelta);
    }

    private int GetViewHeight()
    {
        int result = 40;
        int num = 0;
        for (int i = 0; i < this.Root.transform.childCount; i++)
        {
            if (this.Root.transform.GetChild(i).gameObject.activeSelf)
            {
                num++;
            }
        }
        if (num == 1)
        {
            result = 55;
        }
        else if (num > 1)
        {
            result = 55 + (num - 1) * this.itemHeight;
        }
        return result;
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    public void InitilizeAbattoir(Memember _memeber, PointerEventData _eventData)
    {
        this.memeber = _memeber;
        this.eventData = _eventData;
        this.btnTeamInvite.SetActive(false);
        this.btnTeamApply.SetActive(false);
        this.btnFireTeam.SetActive(false);
        this.btnGiveAbility.SetActive(false);
        this.btnDeleAbility.SetActive(false);
        this.btnLeaveTeam.SetActive(false);
        this.btnKickOut.SetActive(false);
        this.btnTanheLeader.SetActive(false);
        this.btnTurnLeader.SetActive(false);
        this.btnViewMemberInfo.SetActive(false);
        this.btnShitu.SetActive(false);
        if (ControllerManager.Instance.GetController<FriendControllerNew>().IsFriend(ulong.Parse(this.memeber.mememberid)))
        {
            this.btnAddFriend.SetActive(false);
            this.btnBlackNameBook.SetActive(true);
            this.btnPrivateChat.SetActive(false);
        }
        else
        {
            this.btnAddFriend.SetActive(true);
            this.btnBlackNameBook.SetActive(false);
            this.btnPrivateChat.SetActive(false);
        }
        this.btnGuildInvite.SetActive(false);
        this.btnMiyuChat.SetActive(true);
        this.SetTeamPos();
        Scheduler.Instance.AddFrame(2U, false, new Scheduler.OnScheduler(this.CheckTeamPos));
        for (int i = 0; i < this.Root.transform.childCount; i++)
        {
            if (this.Root.transform.GetChild(i).gameObject.activeSelf)
            {
                this.Root.SetActive(true);
                return;
            }
        }
        this.Close();
    }

    public GameObject btnTeamInvite;

    public GameObject btnTeamApply;

    public GameObject btnFireTeam;

    public GameObject btnGiveAbility;

    public GameObject btnDeleAbility;

    public GameObject btnLeaveTeam;

    public GameObject btnKickOut;

    public GameObject btnTurnLeader;

    public GameObject btnTanheLeader;

    public GameObject btnViewMemberInfo;

    public GameObject btnShitu;

    public GameObject btnPrivateChat;

    public GameObject btnMiyuChat;

    public GameObject btnAddFriend;

    public GameObject btnBlackNameBook;

    public GameObject btnGuildInvite;

    private GameObject Root;

    private Memember memeber;

    private ulong mTeamid;

    private PointerEventData eventData;

    private bool mIsGroupItem;

    private int itemHeight = 30;
}
