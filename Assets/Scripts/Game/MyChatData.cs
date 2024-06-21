using System;
using System.Collections.Generic;
using Chat;
using ProtoBuf;

[ProtoContract]
public class MyChatData : ICloneable
{
    public MyChatData()
    {
        this.readed = false;
    }

    public MyChatData(ChatData chatData, bool _shake = false)
    {
        this.charid = chatData.charid.ToString();
        this.tocharid = chatData.tocharid.ToString();
        this.content = chatData.content.ToString();
        this.channel = chatData.channel.ToString();
        this.charname = chatData.charname.ToString();
        this.chattime = chatData.chattime.ToString();
        this.show_type = chatData.show_type.ToString();
        this.charcountry = chatData.charcountry.ToString();
        this.link = new List<MyChatLink>();
        if (chatData.link != null)
        {
            for (int i = 0; i < chatData.link.Count; i++)
            {
                MyChatLink myChatLink = new MyChatLink();
                myChatLink.linktype = chatData.link[i].linktype.ToString();
                myChatLink.data_args = chatData.link[i].data_args;
                this.link.Add(myChatLink);
            }
        }
        this.shake = _shake;
        this.readed = false;
    }

    [ProtoMember(1)]
    public string charid { get; set; }

    [ProtoMember(2)]
    public string tocharid { get; set; }

    [ProtoMember(3)]
    public string content { get; set; }

    [ProtoMember(4)]
    public string channel { get; set; }

    [ProtoMember(5)]
    public string charname { get; set; }

    [ProtoMember(6)]
    public string chattime { get; set; }

    [ProtoMember(7)]
    public string show_type { get; set; }

    [ProtoMember(8)]
    public string charcountry { get; set; }

    [ProtoMember(9)]
    public bool shake { get; set; }

    [ProtoMember(10)]
    public bool readed { get; set; }

    [ProtoMember(11)]
    public List<MyChatLink> link { get; set; }

    public object Clone()
    {
        return base.MemberwiseClone();
    }
}
