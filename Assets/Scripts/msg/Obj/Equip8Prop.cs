using System;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "Equip8Prop")]
    public enum Equip8Prop
    {
        [ProtoEnum(Name = "BASIC_PROP", Value = 1)]
        BASIC_PROP = 1,
        [ProtoEnum(Name = "PROP_1", Value = 2)]
        PROP_1,
        [ProtoEnum(Name = "PROP_2", Value = 3)]
        PROP_2,
        [ProtoEnum(Name = "SUFFIX_1", Value = 4)]
        SUFFIX_1,
        [ProtoEnum(Name = "SUFFIX_2", Value = 5)]
        SUFFIX_2,
        [ProtoEnum(Name = "SUFFIX_3", Value = 6)]
        SUFFIX_3,
        [ProtoEnum(Name = "SUFFIX_4", Value = 7)]
        SUFFIX_4,
        [ProtoEnum(Name = "FIXED_PROP", Value = 8)]
        FIXED_PROP
    }
}
