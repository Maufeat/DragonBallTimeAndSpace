using System;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "ChooseType")]
    public enum ChooseType
    {
        [ProtoEnum(Name = "ChooseType_Need", Value = 1)]
        ChooseType_Need = 1,
        [ProtoEnum(Name = "ChooseType_Greed", Value = 2)]
        ChooseType_Greed,
        [ProtoEnum(Name = "ChooseType_Giveup", Value = 3)]
        ChooseType_Giveup
    }
}
