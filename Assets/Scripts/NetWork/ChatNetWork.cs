using System;
using System.Collections.Generic;
using Chat;
using Framework.Managers;
using Net;

public class ChatNetWork : NetWorkBase
{
    private ChatControl chatController
    {
        get
        {
            return ControllerManager.Instance.GetController<ChatControl>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_ChannelChat_SC>(2115, new ProtoMsgCallback<MSG_Ret_ChannelChat_SC>(this.OnRetChannelChat));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_Chat_SC>(2117, new ProtoMsgCallback<MSG_Ret_Chat_SC>(this.OnRetChat));
    }

    public void ReqChannleChat(string contens)
    {
        base.SendMsg<MSG_Req_ChannleChat_CS>(CommandID.MSG_Req_ChannleChat_CS, new MSG_Req_ChannleChat_CS
        {
            str_chat = contens
        }, false);
    }

    public void OnRetChannelChat(MSG_Ret_ChannelChat_SC data)
    {
        if (data.channel_type == ChannelType.ChannelType_Moba)
        {
            ControllerManager.Instance.GetController<AbattoirPrayController>().OnReceiveTipsMsg(data);
        }
        else
        {
            this.chatController.OnReceiveChatMsg(data);
        }
    }

    public void ReqImportantBroadcast()
    {
        MSG_Req_ImportantBroadcast_CS t = new MSG_Req_ImportantBroadcast_CS();
        base.SendMsg<MSG_Req_ImportantBroadcast_CS>(CommandID.MSG_Req_ImportantBroadcast_CS, t, false);
    }

    public void ReqChat(ChannelType channel, ulong charid, string charname, uint charcountry, string content, uint showtype = 0U, List<ChatLink> links = null, ulong toid = 0UL, string toname = "", bool shake = false)
    {
        MSG_Req_Chat_CS msg_Req_Chat_CS = new MSG_Req_Chat_CS();
        msg_Req_Chat_CS.data = new ChatData();
        msg_Req_Chat_CS.data.channel = (uint)channel;
        msg_Req_Chat_CS.data.charid = charid;
        msg_Req_Chat_CS.data.charname = charname;
        msg_Req_Chat_CS.data.charcountry = charcountry;
        msg_Req_Chat_CS.data.chattime = (uint)(SingletonForMono<GameTime>.Instance.GetCurrServerTime() / 1000UL);
        msg_Req_Chat_CS.data.content = content;
        msg_Req_Chat_CS.data.show_type = showtype;
        msg_Req_Chat_CS.data.tocharid = toid;
        msg_Req_Chat_CS.data.toname = toname;
        if (links != null)
        {
            for (int i = 0; i < links.Count; i++)
            {
                msg_Req_Chat_CS.data.link.Add(links[i]);
            }
        }
        if (channel == ChannelType.ChannelType_Private)
        {
        }
        FFDebug.Log(this, FFLogType.PrivateChat, string.Concat(new object[]
        {
            "OnRetChat Charid: ",
            msg_Req_Chat_CS.data.charid,
            ": name: ",
            msg_Req_Chat_CS.data.charname,
            ": toid: ",
            toid,
            ": chaennel: ",
            msg_Req_Chat_CS.data.channel,
            ": charcountry: ",
            msg_Req_Chat_CS.data.charcountry,
            ": time: ",
            msg_Req_Chat_CS.data.chattime,
            "\n content: ",
            msg_Req_Chat_CS.data.content
        }));
        msg_Req_Chat_CS.shake = shake;
        base.SendMsg<MSG_Req_Chat_CS>(CommandID.MSG_Req_Chat_CS, msg_Req_Chat_CS, false);
    }

    public void OnRetChat(MSG_Ret_Chat_SC data)
    {
        FFDebug.Log(this, FFLogType.PrivateChat, string.Concat(new object[]
        {
            "OnRetChat Charid: ",
            data.data.charid,
            ": name: ",
            data.data.charname,
            ": toid: ",
            data.data.tocharid,
            ": chaennel: ",
            data.data.channel,
            ": charcountry: ",
            data.data.charcountry,
            ": time: ",
            data.data.chattime,
            "\n content: ",
            data.data.content
        }));
        if (data.data.link.Count > 0)
        {
            for (int i = 0; i < data.data.link.Count; i++)
            {
                ChatLink chatLink = data.data.link[i];
                string text = "...... link type : " + chatLink.linktype + " params : ";
                for (int j = 0; j < chatLink.data_args.Count; j++)
                {
                    text = text + " " + chatLink.data_args[j];
                }
                FFDebug.LogError(this, text);
            }
        }
        if (data.data.channel == 7U)
        {
            ControllerManager.Instance.GetController<FriendControllerNew>().OnReceiveChatData(new MyChatData(data.data, data.shake));
        }
        else if (data.data.channel == 8U)
        {
            ControllerManager.Instance.GetController<GmChatController>().OnReceiveChatData(new MyChatData(data.data, data.shake));
        }
        else
        {
            this.chatController.OnReciveChatData(data.data);
        }
    }
}
