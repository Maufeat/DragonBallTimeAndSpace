using System;
using System.Collections.Generic;
using Framework.Managers;
using relation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendSecondMenu : MonoBehaviour
{
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

    private void OnInit()
    {
        this.obj_buttons = new GameObject[base.transform.childCount];
        for (int i = 0; i < this.obj_buttons.Length; i++)
        {
            this.obj_buttons[i] = base.transform.GetChild(i).gameObject;
            this.obj_buttons[i].SetActive(true);
            this.obj_buttons[i].name = i.ToString();
            UIEventListener.Get(this.obj_buttons[i]).onClick = new UIEventListener.VoidDelegate(this.btn_common_on_click);
        }
        GameObject gameObject = this.obj_buttons[1].gameObject;
        UIEventListener.Get(gameObject).onEnter = new UIEventListener.VoidDelegate(this.btn_movefriendto_on_enter);
        UIEventListener.Get(gameObject).onExit = new UIEventListener.VoidDelegate(this.btn_movefriendto_on_exit);
        this.obj_group1 = this.obj_buttons[1].FindChild("ThirdMenu/group1").gameObject;
        this.obj_group1.SetActive(false);
        this.btn_mask = base.transform.parent.Find("Mask").gameObject;
        this.btn_mask.SetActive(true);
        UIEventListener.Get(this.btn_mask).onClick = new UIEventListener.VoidDelegate(this.btn_close_on_click);
    }

    private void btn_movefriendto_on_enter(PointerEventData eventData)
    {
        this.obj_group1.transform.parent.gameObject.SetActive(true);
    }

    private void btn_movefriendto_on_exit(PointerEventData eventData)
    {
        this.obj_group1.transform.parent.gameObject.SetActive(false);
    }

    private void btn_common_on_click(PointerEventData eventData)
    {
        switch (int.Parse(eventData.pointerPress.name))
        {
            case 2:
                this.mFriendNetWork.ReqOperateBlackList(this.itemData.relationid, 1U);
                break;
            case 3:
                this.mFriendControllerNew.mUIFriend.transInputOpePanel.name = this.itemData.relationid.ToString();
                this.mFriendControllerNew.mUIFriend.SetupShowOpePanel(UI_FriendNew.InputPageOpeType.SetNickName);
                break;
            case 4:
                {
                    ControllerManager.Instance.GetController<FriendControllerNew>().AddNewFriend(this.itemData.relationid.ToString());
                    UI_FriendPrivateChat uiFriendPrivateChat = UIManager.GetUIObject<UI_FriendPrivateChat>();
                    if (uiFriendPrivateChat == null)
                    {
                        UIManager.Instance.ShowUI<UI_FriendPrivateChat>("UI_FriendPrivateChat", delegate ()
                        {
                            uiFriendPrivateChat = UIManager.GetUIObject<UI_FriendPrivateChat>();
                            uiFriendPrivateChat.AddPrivateChatFriend(this.itemData);
                        }, UIManager.ParentType.CommonUI, false);
                    }
                    else
                    {
                        uiFriendPrivateChat.AddPrivateChatFriend(this.itemData);
                    }
                    break;
                }
            case 6:
                ControllerManager.Instance.GetController<TeamController>().ReqInviteIntoTeam_CS(this.itemData.relationid.ToString());
                break;
            case 9:
                {
                    string typestr = "2";
                    UIManager.Instance.ShowUI<UI_MessageBox>("UI_MessageBox", delegate ()
                    {
                        UI_MessageBox uiobject = UIManager.GetUIObject<UI_MessageBox>();
                        uiobject.SetOkCb(new MessageOkCb2(this.ReqAddBlackOkCb), typestr);
                        string contentByID = ControllerManager.Instance.GetController<TextModelController>().GetContentByID("900021");
                        uiobject.SetContent(contentByID, "提示", true);
                    }, UIManager.ParentType.CommonUI, false);
                    break;
                }
            case 10:
                {
                    string rid = this.itemData.relationid.ToString();
                    UIManager.Instance.ShowUI<UI_MessageBox>("UI_MessageBox", delegate ()
                    {
                        UI_MessageBox uiobject = UIManager.GetUIObject<UI_MessageBox>();
                        uiobject.SetOkCb(new MessageOkCb2(this.ReqDeleteRelationOkCb), rid);
                        string contentByID = ControllerManager.Instance.GetController<TextModelController>().GetContentByID("1300105");
                        uiobject.SetContent(string.Format(contentByID, this.itemData.relationname), "提示", true);
                    }, UIManager.ParentType.CommonUI, false);
                    break;
                }
        }
        this.btn_close_on_click(null);
    }

    private void ReqAddBlackOkCb(string data)
    {
        this.mFriendNetWork.ReqOperateBlackList(this.itemData.relationid, uint.Parse(data));
    }

    private void ReqDeleteRelationOkCb(string data)
    {
        this.mFriendNetWork.ReqDeleteRelation(this.itemData.relationid);
    }

    private void btn_close_on_click(PointerEventData eventData)
    {
        this.btn_mask.SetActive(false);
        base.gameObject.SetActive(false);
    }

    public void Initilize(relation_item item, FriendTabType tabType, List<string> friendGroupList, Vector3 pos)
    {
        this.OnInit();
        this.itemData = item;
        this.obj_buttons[1].SetActive(friendGroupList.Count > 1);
        if (friendGroupList.Count > 1)
        {
            UIManager.Instance.ClearListChildrens(this.obj_group1.transform.parent, 1);
            for (int i = 0; i < friendGroupList.Count; i++)
            {
                if (friendGroupList[i] != item.page)
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obj_group1);
                    gameObject.transform.SetParent(this.obj_group1.transform.parent);
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.SetActive(true);
                    string page = friendGroupList[i];
                    gameObject.transform.Find("Text").GetComponent<Text>().text = page;
                    gameObject.GetComponent<Button>().onClick.AddListener(delegate ()
                    {
                        this.mFriendNetWork.ReqMoveFriendPage(this.itemData.relationid, page);
                        this.btn_close_on_click(null);
                    });
                }
            }
        }
        this.obj_buttons[0].SetActive(false);
        this.obj_buttons[5].SetActive(false);
        this.teamController.SearchTeamidByMemberid(this.itemData.relationid, new Action<ulong, bool>(this.SearchTeamidCb));
        this.obj_buttons[8].SetActive(false);
        switch (tabType)
        {
            case FriendTabType.Friend:
                this.obj_buttons[2].SetActive(false);
                break;
            case FriendTabType.Apply:
            case FriendTabType.Recent:
                this.obj_buttons[1].SetActive(false);
                this.obj_buttons[4].SetActive(false);
                this.obj_buttons[2].SetActive(false);
                this.obj_buttons[3].SetActive(false);
                this.obj_buttons[10].SetActive(false);
                break;
            case FriendTabType.Black:
                for (int j = 0; j < this.obj_buttons.Length; j++)
                {
                    this.obj_buttons[j].SetActive(false);
                }
                this.obj_buttons[2].SetActive(true);
                break;
        }
        base.transform.position = pos;
    }

    private TeamController teamController
    {
        get
        {
            return ControllerManager.Instance.GetController<TeamController>();
        }
    }

    private void SearchTeamidCb(ulong mTeamid, bool online)
    {
        bool flag = this.teamController.IsMainPlayerHasTeam();
        bool flag2 = this.teamController.IsMainPlayerLeader();
        this.obj_buttons[6].SetActive(online && (this.teamController.IsHasInviteAbility() || !flag) && mTeamid == 0UL);
        this.obj_buttons[7].SetActive(online && !flag && mTeamid != 0UL);
        base.gameObject.SetActive(true);
    }

    private GameObject obj_group1;

    private GameObject btn_mask;

    private GameObject[] obj_buttons;

    private relation_item itemData;

    private enum ButtonType
    {
        SeeInfo,
        MoveFriendTo,
        DelBlack,
        SetEasyName,
        PrivateChat,
        Miyu,
        TeamInvte,
        TeamApply,
        ShiTu,
        AddBlack,
        DelFriend
    }
}
