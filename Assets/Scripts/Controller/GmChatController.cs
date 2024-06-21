using System;
using System.Collections.Generic;
using Chat;
using Framework.Managers;
using Models;

public class GmChatController : ControllerBase
{
    private FriendControllerNew friendControllerNew
    {
        get
        {
            return ControllerManager.Instance.GetController<FriendControllerNew>();
        }
    }

    private UI_Map UIMap
    {
        get
        {
            return UIManager.GetUIObject<UI_Map>();
        }
    }

    private UI_GmChat UIGmChat
    {
        get
        {
            return UIManager.GetUIObject<UI_GmChat>();
        }
    }

    public void SendChat(ulong toid, string content, uint showtype = 0U, List<ChatLink> links = null, bool shake = false)
    {
        ControllerManager.Instance.GetController<ChatControl>().CNetWork.ReqChat(ChannelType.ChannelType_GmTool, MainPlayer.Self.GetCharID(), MainPlayer.Self.OtherPlayerData.MapUserData.name, MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.country, content, showtype, links, toid, string.Empty, shake);
    }

    public void OnReceiveChatData(MyChatData data)
    {
        this.mNewGmData = data;
        this.friendControllerNew.AddPrivateChatLocalData(data);
        if (this.UIGmChat != null)
        {
            this.UIGmChat.AddPrivateChatItem(data);
        }
        else
        {
            this.ShowBtnGm(true);
        }
    }

    public void ShowBtnGm(bool hasNew)
    {
        this.UIMap.btnGm.gameObject.SetActive(true);
        this.UIMap.imgGmNewMsg.gameObject.SetActive(hasNew);
    }

    public void OpenGmChatPanel()
    {
        UIManager.Instance.ShowUI<UI_GmChat>("UI_GmChat", null, UIManager.ParentType.CommonUI, false);
    }

    public override void Awake()
    {
        base.Awake();
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
            return "gmchat_controller";
        }
    }

    public bool mMapInitGmBtnShow;

    public MyChatData mNewGmData;
}
