using System;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "PKMode")]
    public enum PKMode
    {
        [ProtoEnum(Name = "PKMode_None", Value = 0)]
        PKMode_None,
        [ProtoEnum(Name = "PKMode_Peace", Value = 1)]
        PKMode_Peace,
        [ProtoEnum(Name = "PKMode_Personal", Value = 2)]
        PKMode_Personal,
        [ProtoEnum(Name = "PKMode_Guild", Value = 3)]
        PKMode_Guild,
        [ProtoEnum(Name = "PKMode_Team", Value = 4)]
        PKMode_Team,
        [ProtoEnum(Name = "PKMode_Normal", Value = 5)]
        PKMode_Normal
    }
}
