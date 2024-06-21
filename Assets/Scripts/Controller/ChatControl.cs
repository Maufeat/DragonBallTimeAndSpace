using System;
using System.Collections.Generic;
using System.Text;
using Chat;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChatControl : ControllerBase
{
    public void Initilize(Transform root)
    {
        this.mUiChat = new UI_Chat();
        this.mUiChat.Init(root);
        this.CNetWork.ReqImportantBroadcast();
    }

    public override void Awake()
    {
        this.Init();
    }

    public override void OnDestroy()
    {
        if (this.mUiChat != null)
        {
            this.mUiChat.OnDestroy();
        }
        base.OnDestroy();
    }

    public void OnMainViewDestroy()
    {
        if (this.mUiChat != null)
        {
            this.mUiChat.OnDestroy();
        }
        this.mUiChat = null;
    }

    public void Init()
    {
        this.CNetWork = new ChatNetWork();
        this.CNetWork.Initialize();
        this.allChatDataDic.Clear();
        this.ChatChannelDatas[ChannelType.ChannelType_Sys] = new ChatChannelData(ChannelType.ChannelType_Sys);
        this.ChatChannelDatas[ChannelType.ChannelType_Team] = new ChatChannelData(ChannelType.ChannelType_Team);
        this.ChatChannelDatas[ChannelType.ChannelType_Guild] = new ChatChannelData(ChannelType.ChannelType_Guild);
        this.ChatChannelDatas[ChannelType.ChannelType_Camp] = new ChatChannelData(ChannelType.ChannelType_Camp);
        this.ChatChannelDatas[ChannelType.ChannelType_World] = new ChatChannelData(ChannelType.ChannelType_World);
        this.ChatChannelDatas[ChannelType.ChannelType_Scene] = new ChatChannelData(ChannelType.ChannelType_Scene);
        this.ChatChannelDatas[ChannelType.ChannelType_Private] = new ChatChannelData(ChannelType.ChannelType_Private);
        this.ChatChannelDatas[ChannelType.ChannelType_GmTool] = new ChatChannelData(ChannelType.ChannelType_GmTool);
        this.ChatChannelDatas[ChannelType.ChannelType_Moba] = new ChatChannelData(ChannelType.ChannelType_Moba);
        this.ChatChannelDatas[ChannelType.ChannelType_Secret] = new ChatChannelData(ChannelType.ChannelType_Secret);
        foreach (ChatChannelData chatChannelData in this.ChatChannelDatas.Values)
        {
            this.ChatChannelDataList.Add(chatChannelData);
            this.allChatDataDic.Add(chatChannelData.ChannelType, new List<ChatData>());
        }
    }

    public override void OnUpdate()
    {
        for (int i = 0; i < this.ChatChannelDataList.Count; i++)
        {
            this.ChatChannelDataList[i].Update();
        }
        if (this.mUiChat != null)
        {
            this.mUiChat.Update();
        }
    }

    public void SendChat(ChannelType channel, string content, uint showtype = 0U, List<ChatLink> links = null)
    {
        if (channel == ChannelType.ChannelType_World)
        {
            HeroHandbookController controller = ControllerManager.Instance.GetController<HeroHandbookController>();
            int selfCreateHeroLevel = controller.GetSelfCreateHeroLevel();
            if (selfCreateHeroLevel < 20)
            {
                TipsWindow.ShowNotice("角色等级达到20级才能使用世界聊天！");
                return;
            }
        }
        if (content.Length > 2)
        {
            string a = content.Substring(0, 2);
            if (a == "//")
            {
                this.CNetWork.ReqChat(channel, MainPlayer.Self.GetCharID(), MainPlayer.Self.OtherPlayerData.MapUserData.name, MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.country, content, showtype, links, 0UL, string.Empty, false);
                return;
            }
        }
        if (channel == ChannelType.ChannelType_Sys)
        {
            FFDebug.LogWarning(this, "CurrentChannel == ChannelType.ChannelType_Sys");
            return;
        }
        if (!this.ChatChannelDatas[channel].CheckSend())
        {
            return;
        }
        ulong toid = 0UL;
        string text = string.Empty;
        switch (channel)
        {
            case ChannelType.ChannelType_Team:
                if (!ManagerCenter.Instance.GetManager<GameScene>().isAbattoirScene)
                {
                    if (MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.teamid == 0U)
                    {
                        TipsWindow.ShowWindow(1306U);
                        return;
                    }
                }
                break;
            case ChannelType.ChannelType_Guild:
                if (MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.guildid == 0UL)
                {
                    TipsWindow.ShowWindow(1307U);
                    return;
                }
                break;
            case ChannelType.ChannelType_Secret:
                {
                    if (!content.StartsWith("@"))
                    {
                        FFDebug.LogError(this, "密语输入格式错误！@+名字+空格+密聊内容");
                        return;
                    }
                    int num = content.IndexOf(" ");
                    if (num <= 1)
                    {
                        FFDebug.LogError(this, "密语输入格式错误！@+名字+空格+密聊内容");
                        return;
                    }
                    text = content.Substring(1, num - 1);
                    if (num >= content.Length - 1)
                    {
                        FFDebug.LogError(this, "密聊内容 不能为空");
                        return;
                    }
                    content = content.Substring(num + 1);
                    if (MainPlayer.Self.OtherPlayerData.MapUserData.name == text)
                    {
                        FFDebug.LogError(this, "玩家不能和自己互动");
                        return;
                    }
                    break;
                }
        }
        ChatChannelData chatChannelData = this.ChatChannelDatas[channel];
        if (chatChannelData.laststring.CompareTo(content) == 0)
        {
            chatChannelData.repeattime += 1U;
            FFDebug.LogWarning(this, channel + " " + chatChannelData.repeattime);
            if (chatChannelData.repeattime >= 3U)
            {
                FFDebug.LogWarning(this, "repeattimes >= 3");
                TipsWindow.ShowWindow(1304U);
            }
        }
        else
        {
            chatChannelData.laststring = content;
            chatChannelData.repeattime = 0U;
        }
        this.CNetWork.ReqChat(channel, MainPlayer.Self.GetCharID(), MainPlayer.Self.OtherPlayerData.MapUserData.name, MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.country, content, showtype, links, toid, text, false);
    }

    private void ReceiveDoSomething(uint textid, string content)
    {
        if (textid != 902U && textid != 903U)
        {
            if (textid == 678U)
            {
                UIManager.Instance.DeleteUI<UI_GuildInfoNew>();
                ControllerManager.Instance.GetController<GuildControllerNew>().OnEnterOrExitGuildFrashTaskUI();
                ControllerManager.Instance.GetController<GuildControllerNew>().DeleteGuildWarUI();
            }
        }
        else
        {
            ControllerManager.Instance.GetController<MainUIController>().ShowBattleLog(content, true);
        }
    }

    public void OnReceiveChatMsg(MSG_Ret_ChannelChat_SC msg)
    {
        this.ReceiveDoSomething(msg.textid, msg.str_chat);
        this.ProcessTipsInChat(msg);
        if (msg.textid != 0U)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("textconfig", (ulong)msg.textid);
            if (configTable == null)
            {
                FFDebug.LogWarning(this, "t_text_config not have id " + msg.textid);
                return;
            }
            if (configTable.GetField_Uint("showtype") == 0U)
            {
                return;
            }
            NoticeModel noticeModel = new NoticeModel();
            if (configTable.GetField_Uint("texttype") == 1U)
            {
                noticeModel.content = configTable.GetField_String("tips");
            }
            else
            {
                noticeModel.content = msg.str_chat;
            }
            switch (configTable.GetField_Uint("showtype"))
            {
                case 1U:
                    noticeModel.content = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(noticeModel.content);
                    ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, noticeModel.content, MsgBoxController.MsgOptionConfirm, UIManager.ParentType.CommonUI, null, null);
                    break;
                case 2U:
                    {
                        string str = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(configTable.GetField_String("channeltext"));
                        noticeModel.content = str + ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(noticeModel.content);
                        noticeModel.channeltype = msg.channel_type;
                        TipsWindow.ShowNotice(noticeModel);
                        break;
                    }
                case 3U:
                    noticeModel.content = ControllerManager.Instance.GetController<TextModelController>().RemoveTextModel(noticeModel.content);
                    noticeModel.texEffectColor = Const.TextColorTipsRed;
                    TipsWindow.ShowWindow(noticeModel);
                    break;
                case 4U:
                    noticeModel.content = ControllerManager.Instance.GetController<TextModelController>().RemoveTextModel(noticeModel.content);
                    noticeModel.texEffectColor = Const.TextColorTipsGreen;
                    TipsWindow.ShowWindow(noticeModel);
                    break;
                case 6U:
                    noticeModel.content = ControllerManager.Instance.GetController<TextModelController>().RemoveTextModel(noticeModel.content);
                    TipsWindow.ShowTaskTips(noticeModel.content);
                    break;
            }
        }
        else
        {
            TipsWindow.ShowWindow(msg.str_chat);
            this.AddChatItemBySystem(msg.str_chat);
        }
    }

    public void AddChatItemBySystem(string content)
    {
        ChatData chatData = new ChatData();
        chatData.channel = 1U;
        chatData.content = content;
        if (this.mUiChat != null)
        {
            this.mUiChat.AddChatItem(chatData);
        }
    }

    private void ProcessTipsInChat(MSG_Ret_ChannelChat_SC data)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("textconfig", (ulong)data.textid);
        if (configTable == null)
        {
            return;
        }
        if (configTable.GetField_Uint("showchat") == 0U)
        {
            return;
        }
        ChatData chatData = new ChatData();
        chatData.channel = (uint)data.channel_type;
        chatData.charname = data.src_name;
        if (configTable.GetField_Uint("texttype") == 1U)
        {
            chatData.content = configTable.GetField_String("tips");
        }
        else
        {
            chatData.content = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(data.str_chat);
        }
        if (configTable.GetField_Uint("showtype") == 2U)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string field_String = LuaConfigManager.GetConfigTable("textconfig", (ulong)data.textid).GetField_String("channeltext");
            stringBuilder.Append(ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(field_String));
            stringBuilder.Append(chatData.content);
            chatData.content = stringBuilder.ToString();
            uint cacheField_Uint = LuaConfigManager.GetConfigTable("textconfig", (ulong)data.textid).GetCacheField_Uint("rightsys");
            if (cacheField_Uint == 1U)
            {
                ControllerManager.Instance.GetController<MainUIController>().AddRightSysLog(chatData);
                return;
            }
            foreach (ChannelType key in this.allChatDataDic.Keys)
            {
                this.allChatDataDic[key].Add(chatData);
                while ((long)this.allChatDataDic[key].Count > (long)((ulong)this.ChatChannelDatas[key].ChannelConfig.GetField_Uint("cachedount")))
                {
                    this.allChatDataDic[key].RemoveAt(0);
                }
            }
            if (this.mUiChat != null)
            {
                this.mUiChat.AddChatItem(chatData);
            }
        }
        else
        {
            uint cacheField_Uint2 = LuaConfigManager.GetConfigTable("textconfig", (ulong)data.textid).GetCacheField_Uint("rightsys");
            if (cacheField_Uint2 == 1U)
            {
                ControllerManager.Instance.GetController<MainUIController>().AddRightSysLog(chatData);
                return;
            }
            this.allChatDataDic[ChannelType.ChannelType_Sys].Add(chatData);
            while ((long)this.allChatDataDic[ChannelType.ChannelType_Sys].Count > (long)((ulong)this.ChatChannelDatas[ChannelType.ChannelType_Sys].ChannelConfig.GetField_Uint("cachedount")))
            {
                this.allChatDataDic[ChannelType.ChannelType_Sys].RemoveAt(0);
            }
            if (this.mUiChat != null && this.mUiChat.CurChannelEqual(ChannelType.ChannelType_Sys))
            {
                this.mUiChat.AddChatItem(chatData);
            }
        }
        ControllerManager.Instance.GetController<MainUIController>().AddChatItem(chatData);
    }

    public void OnReciveChatData(ChatData data)
    {
        this.allChatDataDic[(ChannelType)data.channel].Add(data);
        while ((long)this.allChatDataDic[(ChannelType)data.channel].Count > (long)((ulong)this.ChatChannelDatas[(ChannelType)data.channel].ChannelConfig.GetField_Uint("cachedount")))
        {
            this.allChatDataDic[(ChannelType)data.channel].RemoveAt(0);
        }
        if (this.mUiChat != null)
        {
            this.mUiChat.AddChatItem(data);
        }
    }

    public override string ControllerName
    {
        get
        {
            return "chat_controller";
        }
    }

    public void InputFieldActive(bool isSend = false)
    {
        if (this.mUiChat != null)
        {
            if (isSend)
            {
                if (this.mUiChat.lbl_input.text != string.Empty)
                {
                    this.mUiChat.btn_send_on_click(null);
                }
                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                this.mUiChat.lbl_input.ActivateInputField();
            }
        }
    }

    public void MiyuToMember(string name)
    {
        if (this.mUiChat != null)
        {
            this.mUiChat.SecritChat(name);
        }
    }

    public ChatNetWork CNetWork;

    public UI_Chat mUiChat;

    public Dictionary<ChannelType, List<ChatData>> allChatDataDic = new Dictionary<ChannelType, List<ChatData>>();

    public BetterDictionary<ChannelType, ChatChannelData> ChatChannelDatas = new BetterDictionary<ChannelType, ChatChannelData>();

    public List<ChatChannelData> ChatChannelDataList = new List<ChatChannelData>();
}
