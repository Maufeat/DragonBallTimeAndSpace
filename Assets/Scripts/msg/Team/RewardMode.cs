using System;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "RewardMode")]
    public enum RewardMode
    {
        [ProtoEnum(Name = "Mode_Roll", Value = 1)]
        Mode_Roll = 1,
        [ProtoEnum(Name = "Mode_Dispath", Value = 2)]
        Mode_Dispath
    }
}
