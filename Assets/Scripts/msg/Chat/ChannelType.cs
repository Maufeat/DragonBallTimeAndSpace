using System;
using ProtoBuf;

namespace Chat
{
    [ProtoContract(Name = "ChannelType")]
    public enum ChannelType
    {
        [ProtoEnum(Name = "ChannelType_None", Value = 0)]
        ChannelType_None,
        [ProtoEnum(Name = "ChannelType_Sys", Value = 1)]
        ChannelType_Sys,
        [ProtoEnum(Name = "ChannelType_Team", Value = 2)]
        ChannelType_Team,
        [ProtoEnum(Name = "ChannelType_Guild", Value = 3)]
        ChannelType_Guild,
        [ProtoEnum(Name = "ChannelType_Camp", Value = 4)]
        ChannelType_Camp,
        [ProtoEnum(Name = "ChannelType_World", Value = 5)]
        ChannelType_World,
        [ProtoEnum(Name = "ChannelType_Scene", Value = 6)]
        ChannelType_Scene,
        [ProtoEnum(Name = "ChannelType_Private", Value = 7)]
        ChannelType_Private,
        [ProtoEnum(Name = "ChannelType_GmTool", Value = 8)]
        ChannelType_GmTool,
        [ProtoEnum(Name = "ChannelType_Moba", Value = 9)]
        ChannelType_Moba,
        [ProtoEnum(Name = "ChannelType_Secret", Value = 10)]
        ChannelType_Secret
    }
}
