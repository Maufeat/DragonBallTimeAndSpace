using System;
using System.Collections.Generic;
using Framework.Managers;
using guild;
using relation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GuildMemberSecondMenu
{
    public GuildMemberSecondMenu(UI_GuildInfoNew uiGuildInfoNew, Transform root)
    {
        this.mController = ControllerManager.Instance.GetController<GuildControllerNew>();
        this.mUIGuildInfoNew = uiGuildInfoNew;
        this.mRoot = root;
        this.btnSeeInfo = this.mRoot.Find("OffSet/ViewMemberInfo").GetComponent<Button>();
        this.btnRemovePosition = this.mRoot.Find("OffSet/RemovePosition").GetComponent<Button>();
        this.btnTranslateMaster = this.mRoot.Find("OffSet/TransferLeader").GetComponent<Button>();
        this.btnSetPosition = this.mRoot.Find("OffSet/Appoint").GetComponent<Button>();
        this.btnAddFriend = this.mRoot.Find("OffSet/AddFriend").GetComponent<Button>();
        this.btnPrivateChat = this.mRoot.Find("OffSet/PrivateChat").GetComponent<Button>();
        this.btnInviteTeam = this.mRoot.Find("OffSet/OrganizeTeam").GetComponent<Button>();
        this.btnApplyTeam = this.mRoot.Find("OffSet/ApplyTeam").GetComponent<Button>();
        this.btnFire = this.mRoot.Find("OffSet/Expel").GetComponent<Button>();
        this.objAppoint = this.mRoot.Find("OffSet/Appoint").gameObject;
        this.transThridMenu = this.mRoot.Find("OffSet/Appoint/Panel_thirdmenu");
        this.objThridItem = this.transThridMenu.Find("Item").gameObject;
        UIEventListener uieventListener = UIEventListener.Get(this.objAppoint);
        uieventListener.onEnter = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onEnter, new UIEventListener.VoidDelegate(this.obj_appoint_onenter));
        UIEventListener uieventListener2 = UIEventListener.Get(this.objAppoint);
        uieventListener2.onExit = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener2.onExit, new UIEventListener.VoidDelegate(this.obj_appoint_onexit));
        this.btnSeeInfo.onClick.AddListener(new UnityAction(this.btn_seeinfo_onclick));
        this.btnRemovePosition.onClick.AddListener(new UnityAction(this.btn_removeposition_onclick));
        this.btnTranslateMaster.onClick.AddListener(new UnityAction(this.btn_transaltemaster_onclick));
        this.btnAddFriend.onClick.AddListener(new UnityAction(this.btn_addfriend_onclick));
        this.btnPrivateChat.onClick.AddListener(new UnityAction(this.btn_privatechat_onclick));
        this.btnInviteTeam.onClick.AddListener(new UnityAction(this.btn_inviteteam_onclick));
        this.btnApplyTeam.onClick.AddListener(new UnityAction(this.btn_applyteam_onclick));
        this.btnFire.onClick.AddListener(new UnityAction(this.btn_fire_onclick));
        UIEventListener.Get(this.mRoot.gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_mask_onclick);
    }

    private void obj_appoint_onenter(PointerEventData eventData)
    {
        UIManager.Instance.ClearListChildrens(this.transThridMenu, 1);
        List<GuildPositionInfo> guildPositionInfoList = this.mController.GetGuildPositionInfoList();
        for (int i = 1; i < guildPositionInfoList.Count; i++)
        {
            GuildPositionInfo guildPositionInfo = guildPositionInfoList[i];
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.objThridItem);
            gameObject.transform.SetParent(this.transThridMenu);
            gameObject.transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            gameObject.name = guildPositionInfo.positionid.ToString();
            for (int j = 0; j < gameObject.transform.childCount; j++)
            {
                Text component = gameObject.transform.GetChild(j).GetComponent<Text>();
                if (component != null)
                {
                    component.text = guildPositionInfo.name;
                }
            }
            UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_setposition_onclick);
        }
        this.transThridMenu.gameObject.SetActive(true);
    }

    private void obj_appoint_onexit(PointerEventData eventData)
    {
        this.transThridMenu.gameObject.SetActive(false);
    }

    private void btn_removeposition_onclick()
    {
        this.mController.AssignPosition(this.mGuildMember.memberid, 99U);
        this.ClosePanel();
    }

    private void btn_transaltemaster_onclick()
    {
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox("提示", string.Format(CommonTools.GetTextById(662UL), this.mGuildMember.membername), "确认", "取消", UIManager.ParentType.Tips, new Action(this.translatemastersure), new Action(this.translatemastercancel), null);
        this.ClosePanel();
    }

    private void translatemastersure()
    {
        this.mController.ChangeGuildMaster(this.mGuildMember.memberid.ToString());
    }

    private void translatemastercancel()
    {
    }

    private void btn_setposition_onclick(PointerEventData eventData)
    {
        this.posIndex = uint.Parse(eventData.pointerPress.name);
        GuildPositionInfo guildPositionInfo = ControllerManager.Instance.GetController<GuildControllerNew>().GetGuildPositionInfo(this.posIndex);
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox("提示", string.Format(CommonTools.GetTextById(663UL), this.mGuildMember.membername, guildPositionInfo.name), "确认", "取消", UIManager.ParentType.Tips, new Action(this.setpositionsure), new Action(this.setpositioncancel), null);
        this.transThridMenu.gameObject.SetActive(false);
        this.ClosePanel();
    }

    private void setpositionsure()
    {
        this.mController.AssignPosition(this.mGuildMember.memberid, this.posIndex);
    }

    private void setpositioncancel()
    {
    }

    private void btn_fire_onclick()
    {
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox("提示", string.Format(CommonTools.GetTextById(664UL), this.mGuildMember.membername), "确认", "取消", UIManager.ParentType.Tips, new Action(this.firesure), new Action(this.firecancel), null);
        this.ClosePanel();
    }

    private void firesure()
    {
        this.mController.FireGuildMember(this.mGuildMember.memberid.ToString());
    }

    private void firecancel()
    {
    }

    private void btn_seeinfo_onclick()
    {
        this.ClosePanel();
    }

    private void btn_addfriend_onclick()
    {
        ControllerManager.Instance.GetController<FriendControllerNew>().ReqApplyFriend(this.mGuildMember.memberid);
        this.ClosePanel();
    }

    private void btn_privatechat_onclick()
    {
        UI_FriendPrivateChat uiFriendPrivateChat = UIManager.GetUIObject<UI_FriendPrivateChat>();
        relation_item itemData = ControllerManager.Instance.GetController<FriendControllerNew>().GetFriend(this.mGuildMember.memberid);
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
        this.ClosePanel();
    }

    private TeamController teamController
    {
        get
        {
            return ControllerManager.Instance.GetController<TeamController>();
        }
    }

    private void btn_inviteteam_onclick()
    {
        this.teamController.ReqInviteIntoTeam_CS(this.mGuildMember.memberid.ToString());
        this.ClosePanel();
    }

    private void btn_applyteam_onclick()
    {
        this.teamController.ApplyTeam((uint)this.mTeamid);
        this.ClosePanel();
    }

    private void btn_mask_onclick(PointerEventData eventData)
    {
        this.mRoot.gameObject.SetActive(false);
    }

    public void OpenPanel(PointerEventData eventData, guildMember member)
    {
        this.mGuildMember = member;
        this.teamController.SearchTeamidByMemberid(this.mGuildMember.memberid, new Action<ulong, bool>(this.SerachTeamidCb));
        Vector3 position;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(UIManager.FindInParents<Canvas>(this.mRoot.gameObject).transform as RectTransform, eventData.position, eventData.pressEventCamera, out position))
        {
            this.mRoot.GetChild(0).transform.position = position;
        }
    }

    private void SerachTeamidCb(ulong teamid, bool online)
    {
        this.mTeamid = teamid;
        this.mOnline = online;
        this.CheckBtnVisible();
        this.mRoot.gameObject.SetActive(true);
    }

    private void CheckBtnVisible()
    {
        this.btnSeeInfo.gameObject.SetActive(false);
        this.btnPrivateChat.gameObject.SetActive(false);
        if (ControllerManager.Instance.GetController<FriendControllerNew>().IsFriend(this.mGuildMember.memberid))
        {
            this.btnAddFriend.gameObject.SetActive(false);
            this.btnPrivateChat.gameObject.SetActive(false);
        }
        else
        {
            this.btnAddFriend.gameObject.SetActive(true);
            this.btnPrivateChat.gameObject.SetActive(false);
        }
        bool flag = this.teamController.IsMainPlayerHasTeam();
        this.btnInviteTeam.gameObject.SetActive(this.mOnline && (this.teamController.IsHasInviteAbility() || !flag) && this.mTeamid == 0UL);
        this.btnApplyTeam.gameObject.SetActive(this.mOnline && !flag && this.mTeamid != 0UL);
        this.btnRemovePosition.gameObject.SetActive(this.mController.IsGuildMaster());
        this.btnTranslateMaster.gameObject.SetActive(this.mController.IsGuildMaster());
        this.btnSetPosition.gameObject.SetActive(this.mController.IsGuildMaster());
        guildMember myGuildMember = this.mController.GetMyGuildMember();
        if (myGuildMember == null)
        {
            this.btnFire.gameObject.SetActive(false);
        }
        else
        {
            bool positionGuildPrivilege = this.mController.GetPositionGuildPrivilege(myGuildMember.positionid, GuildPrivilege.GUILDPRI_REMOVE_MEMBER);
            bool flag2 = myGuildMember.positionid < this.mGuildMember.positionid;
            this.btnFire.gameObject.SetActive(positionGuildPrivilege && flag2);
        }
    }

    private void ClosePanel()
    {
        this.mRoot.gameObject.SetActive(false);
    }

    private GuildControllerNew mController;

    private UI_GuildInfoNew mUIGuildInfoNew;

    private Transform mRoot;

    private Button btnSeeInfo;

    private Button btnRemovePosition;

    private Button btnTranslateMaster;

    private Button btnSetPosition;

    private Button btnAddFriend;

    private Button btnPrivateChat;

    private Button btnInviteTeam;

    private Button btnApplyTeam;

    private Button btnFire;

    private GameObject objAppoint;

    private Transform transThridMenu;

    private GameObject objThridItem;

    private guildMember mGuildMember;

    private ulong mTeamid;

    private bool mOnline;

    private uint posIndex;
}
