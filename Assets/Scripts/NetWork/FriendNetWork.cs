using System;
using System.Collections.Generic;
using Chat;
using Framework.Managers;
using mail;
using Net;
using relation;

public class FriendNetWork : NetWorkBase
{
    private FriendControllerNew friendController
    {
        get
        {
            return ControllerManager.Instance.GetController<FriendControllerNew>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_RelationList_SC>(60091, new ProtoMsgCallback<MSG_Ret_RelationList_SC>(this.OnRetRelationList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_RefreshRelation_SC>(60092, new ProtoMsgCallback<MSG_Ret_RefreshRelation_SC>(this.OnRetRefreshRelation));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_AnswerApplyRelation_SC>(60095, new ProtoMsgCallback<MSG_Ret_AnswerApplyRelation_SC>(this.OnRetAnswerApplyRelation));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_DeleteRelation_SC>(60097, new ProtoMsgCallback<MSG_Ret_DeleteRelation_SC>(this.OnRetDeleteRelation));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_AddInteractive_SC>(60099, new ProtoMsgCallback<MSG_Ret_AddInteractive_SC>(this.OnRetAddInteractive));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_SearchRelation_SC>(60101, new ProtoMsgCallback<MSG_Ret_SearchRelation_SC>(this.OnRetSearchRelation));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_OfflineChat_SC>(60041, new ProtoMsgCallback<MSG_Ret_OfflineChat_SC>(this.OnRetOffLineChat));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_AllFriendPage_CSC>(60104, new ProtoMsgCallback<MSG_AllFriendPage_CSC>(this.OnRetAllFriendPage));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ModifyPageName_CSC>(60102, new ProtoMsgCallback<MSG_ModifyPageName_CSC>(this.OnRetModifyPageName));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_MoveFriendToPage_CSC>(60103, new ProtoMsgCallback<MSG_MoveFriendToPage_CSC>(this.OnRetMoveFriendToPage));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_BlackList_CSC>(60105, new ProtoMsgCallback<MSG_BlackList_CSC>(this.OnRetBlackList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_OperateBlackList_CSC>(60106, new ProtoMsgCallback<MSG_OperateBlackList_CSC>(this.OnRetOperateBlackList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ChangeNickName_CSC>(60107, new ProtoMsgCallback<MSG_ChangeNickName_CSC>(this.OnRetChangeNickName));
    }

    public void OnRetRelationList(MSG_Ret_RelationList_SC data)
    {
        if (data.type == 1U)
        {
            this.friendController.InitFriendList(data.relations);
        }
        else if (data.type == 3U)
        {
            this.friendController.InitFriendApplyForList(data.relations);
        }
        ControllerManager.Instance.GetController<FriendControllerNew>().RefreshFriendTip();
    }

    public void OnRetRefreshRelation(MSG_Ret_RefreshRelation_SC data)
    {
        FFDebug.Log(this, FFLogType.Network, "OnRetRefreshRelation  " + data.data.type);
        this.friendController.RefreshRelationItem(data.data);
    }

    public void ReqApplyRelation(ulong id)
    {
        base.SendMsg<MSG_Req_ApplyRelation_CS>(CommandID.MSG_Req_ApplyRelation_CS, new MSG_Req_ApplyRelation_CS
        {
            relationid = id
        }, false);
        FFDebug.Log(this, FFLogType.Network, "ReqApplyRelation " + id);
    }

    public void ReqAnswerApplyRelation(ulong id, uint type)
    {
        base.SendMsg<MSG_Req_AnswerApplyRelation_CS>(CommandID.MSG_Req_AnswerApplyRelation_CS, new MSG_Req_AnswerApplyRelation_CS
        {
            relationid = id,
            type = type
        }, false);
        FFDebug.Log(this, FFLogType.Network, "ReqAnswerApplyRelation " + id);
    }

    public void OnRetAnswerApplyRelation(MSG_Ret_AnswerApplyRelation_SC data)
    {
        if (data.issucc)
        {
            this.friendController.RemoveItemFromApplyList(data.relationid);
        }
    }

    public void ReqDeleteRelation(ulong id)
    {
        base.SendMsg<MSG_Req_DeleteRelation_CS>(CommandID.MSG_Req_DeleteRelation_CS, new MSG_Req_DeleteRelation_CS
        {
            relationid = id
        }, false);
        FFDebug.Log(this, FFLogType.Network, "ReqDeleteRelation " + id);
    }

    public void OnRetDeleteRelation(MSG_Ret_DeleteRelation_SC data)
    {
        if (data.issucc)
        {
            TipsWindow.ShowWindow(TipsType.BREAK_FRIENDSHIP, null);
            this.friendController.RemoveItemFromFriendList(data.relationid);
            ControllerManager.Instance.GetController<FriendControllerNew>().DelNewFriend(data.relationid.ToString());
        }
    }

    public void ReqOfflineInteractive()
    {
        base.SendMsg<MSG_Req_OfflineInteractive_CS>(CommandID.MSG_Req_OfflineInteractive_CS, new MSG_Req_OfflineInteractive_CS(), false);
    }

    public void OnRetAddInteractive(MSG_Ret_AddInteractive_SC data)
    {
        this.friendController.RefreshRecentList(data.data);
    }

    public void ReqSearchRelation(string condition, FriendNetWork.SearchCb _cb = null)
    {
        this._searchCb = _cb;
        base.SendMsg<MSG_Req_SearchRelation_CS>(CommandID.MSG_Req_SearchRelation_CS, new MSG_Req_SearchRelation_CS
        {
            condition = condition
        }, false);
    }

    public void OnRetSearchRelation(MSG_Ret_SearchRelation_SC data)
    {
        if (this._searchCb == null)
        {
            this.friendController.ShowSearchResult(data.relation);
        }
        else
        {
            this._searchCb(data.relation);
        }
    }

    public void ReqOffLineChat()
    {
        base.SendMsg<MSG_Req_OfflineChat_CS>(CommandID.MSG_Req_OfflineChat_CS, new MSG_Req_OfflineChat_CS(), false);
    }

    public void OnRetOffLineChat(MSG_Ret_OfflineChat_SC data)
    {
        this.friendController.OnReceiveOfflineChat(data.datas);
    }

    public void OnRetRefreshMail(MSG_Ret_RefreshMail_SC data)
    {
    }

    public void OnRetRefreshMailState(MSG_Ret_RefreshMailState_SC data)
    {
    }

    public void OnRetGetAttachment(MSG_Ret_GetAttachment_SC data)
    {
    }

    public void OnRetDeleteMail(MSG_Ret_DeleteMail_SC data)
    {
    }

    public void ReqMailList()
    {
        base.SendMsg<MSG_Req_MailList_CS>(CommandID.MSG_Req_MailList_CS, new MSG_Req_MailList_CS(), false);
    }

    public void ReqOpenMail(ulong mailid)
    {
        base.SendMsg<MSG_Req_OpenMail_CS>(CommandID.MSG_Req_OpenMail_CS, new MSG_Req_OpenMail_CS
        {
            mailid = mailid
        }, false);
    }

    public void ReqGetAttachment(ulong mailid)
    {
        base.SendMsg<MSG_Req_GetAttachment_CS>(CommandID.MSG_Req_GetAttachment_CS, new MSG_Req_GetAttachment_CS
        {
            mailid = mailid.ToString()
        }, false);
    }

    public void ReqGetAllAttachment()
    {
        base.SendMsg<MSG_Req_GetAllAttachment_CS>(CommandID.MSG_Req_GetAllAttachment_CS, new MSG_Req_GetAllAttachment_CS(), false);
    }

    public void ReqDeleteMail(ulong mailid)
    {
        base.SendMsg<MSG_Req_DeleteMail_CS>(CommandID.MSG_Req_DeleteMail_CS, new MSG_Req_DeleteMail_CS
        {
            mailid = mailid
        }, false);
    }

    public void ReqDeleteAllMail()
    {
        base.SendMsg<MSG_Req_DeleteAllMail_CS>(CommandID.MSG_Req_DeleteAllMail_CS, new MSG_Req_DeleteAllMail_CS(), false);
    }

    public void ReqAllFriendPage()
    {
        base.SendMsg<MSG_AllFriendPage_CSC>(CommandID.MSG_AllFriendPage_CSC, new MSG_AllFriendPage_CSC(), false);
    }

    private void OnRetAllFriendPage(MSG_AllFriendPage_CSC data)
    {
        UI_FriendNew uiobject = UIManager.GetUIObject<UI_FriendNew>();
        if (uiobject)
        {
            List<PageItem> pages = data.pages;
            pages.Sort((PageItem a, PageItem b) => a.createtime.CompareTo(b.createtime));
            List<string> list = new List<string>();
            for (int i = 0; i < pages.Count; i++)
            {
                list.Add(pages[i].page_name);
            }
            uiobject.friendTabTitleList = list;
            uiobject.SetupFriendTabListPanel();
        }
    }

    public void ReqModifyPageName(string page, string newPage, uint opcode)
    {
        base.SendMsg<MSG_ModifyPageName_CSC>(CommandID.MSG_ModifyPageName_CSC, new MSG_ModifyPageName_CSC
        {
            page = page,
            new_page = newPage,
            opcode = opcode
        }, false);
    }

    private void OnRetModifyPageName(MSG_ModifyPageName_CSC data)
    {
        if (data.success)
        {
            UI_FriendNew uiobject = UIManager.GetUIObject<UI_FriendNew>();
            if (uiobject)
            {
                if (data.opcode == 3U)
                {
                    uiobject.friendTabTitleList.Add(data.page);
                }
                else if (data.opcode == 2U)
                {
                    uiobject.friendTabTitleList.Remove(data.page);
                }
                else
                {
                    uiobject.friendTabTitleList[uiobject.friendTabTitleList.IndexOf(data.page)] = data.new_page;
                }
                uiobject.SetupFriendTabListPanel();
            }
        }
    }

    public void ReqMoveFriendPage(ulong charid, string page)
    {
        base.SendMsg<MSG_MoveFriendToPage_CSC>(CommandID.MSG_MoveFriendToPage_CSC, new MSG_MoveFriendToPage_CSC
        {
            charid = charid,
            page = page
        }, false);
    }

    private void OnRetMoveFriendToPage(MSG_MoveFriendToPage_CSC data)
    {
        if (data.success)
        {
            if (this.friendController.mFriendDic.ContainsKey(data.charid))
            {
                this.friendController.mFriendDic[data.charid].page = data.page;
            }
            this.friendController.mUIFriend.SetupFriendListPanel();
        }
    }

    public void ReqBlackList()
    {
        base.SendMsg<MSG_BlackList_CSC>(CommandID.MSG_BlackList_CSC, new MSG_BlackList_CSC(), false);
    }

    public void OnRetBlackList(MSG_BlackList_CSC data)
    {
        this.friendController.mBlackDic = new Dictionary<ulong, BlackItem>();
        for (int i = 0; i < data.blackList.Count; i++)
        {
            BlackItem blackItem = data.blackList[i];
            this.friendController.mBlackDic.Add(blackItem.charid, blackItem);
        }
        UI_FriendNew uiobject = UIManager.GetUIObject<UI_FriendNew>();
        if (uiobject)
        {
            uiobject.SetupBlackListPanel();
        }
    }

    public void ReqOperateBlackList(ulong charid, uint opcode)
    {
        base.SendMsg<MSG_OperateBlackList_CSC>(CommandID.MSG_OperateBlackList_CSC, new MSG_OperateBlackList_CSC
        {
            charid = charid,
            opcode = opcode
        }, false);
    }

    public void OnRetOperateBlackList(MSG_OperateBlackList_CSC data)
    {
        if (data.success)
        {
            if (data.opcode == 1U)
            {
                foreach (ulong num in this.friendController.mBlackDic.Keys)
                {
                    if (num == data.charid)
                    {
                        this.friendController.mBlackDic.RemoveAt(num);
                        break;
                    }
                }
            }
            else if (data.opcode == 2U)
            {
                this.friendController.mBlackDic.Add(data.data.charid, data.data);
            }
            else
            {
                this.friendController.mBlackDic = new Dictionary<ulong, BlackItem>();
            }
            UI_FriendNew uiobject = UIManager.GetUIObject<UI_FriendNew>();
            if (uiobject)
            {
                uiobject.SetupBlackListPanel();
            }
        }
    }

    public void ReqChangeNickName(ulong charid, string nickName)
    {
        base.SendMsg<MSG_ChangeNickName_CSC>(CommandID.MSG_ChangeNickName_CSC, new MSG_ChangeNickName_CSC
        {
            charid = charid,
            nickname = nickName
        }, false);
    }

    private void OnRetChangeNickName(MSG_ChangeNickName_CSC data)
    {
        if (data.success)
        {
            foreach (relation_item relation_item in this.friendController.mFriendDic.Values)
            {
                if (relation_item.relationid == data.charid)
                {
                    relation_item.nickName = data.nickname;
                    this.friendController.mUIFriend.SetupFriendListPanel();
                    break;
                }
            }
        }
    }

    private FriendNetWork.SearchCb _searchCb;

    public delegate void SearchCb(relation_item item);
}
