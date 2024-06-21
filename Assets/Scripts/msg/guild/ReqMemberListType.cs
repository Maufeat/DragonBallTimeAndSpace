using System;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "ReqMemberListType")]
    public enum ReqMemberListType
    {
        [ProtoEnum(Name = "NORMAL", Value = 1)]
        NORMAL = 1,
        [ProtoEnum(Name = "APPLYFOR", Value = 2)]
        APPLYFOR
    }
}
