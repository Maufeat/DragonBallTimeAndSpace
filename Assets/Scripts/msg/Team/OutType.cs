using System;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "OutType")]
    public enum OutType
    {
        [ProtoEnum(Name = "OutType_Quit", Value = 1)]
        OutType_Quit = 1,
        [ProtoEnum(Name = "OutType_Fire", Value = 2)]
        OutType_Fire,
        [ProtoEnum(Name = "OutType_VoteOut", Value = 3)]
        OutType_VoteOut
    }
}
