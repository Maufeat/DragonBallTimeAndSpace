using System;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "PackType")]
    public enum PackType
    {
        [ProtoEnum(Name = "PACK_TYPE_NONE", Value = 0)]
        PACK_TYPE_NONE,
        [ProtoEnum(Name = "PACK_TYPE_MAIN", Value = 1)]
        PACK_TYPE_MAIN,
        [ProtoEnum(Name = "PACK_TYPE_EQUIP", Value = 2)]
        PACK_TYPE_EQUIP,
        [ProtoEnum(Name = "PACK_TYPE_QUEST", Value = 3)]
        PACK_TYPE_QUEST,
        [ProtoEnum(Name = "PACK_TYPE_HERO_CARD", Value = 4)]
        PACK_TYPE_HERO_CARD,
        [ProtoEnum(Name = "PACK_TYPE_WAREHOUSE1", Value = 5)]
        PACK_TYPE_WAREHOUSE1,
        [ProtoEnum(Name = "PACK_TYPE_WAREHOUSE2", Value = 6)]
        PACK_TYPE_WAREHOUSE2,
        [ProtoEnum(Name = "PACK_TYPE_WAREHOUSE3", Value = 7)]
        PACK_TYPE_WAREHOUSE3,
        [ProtoEnum(Name = "PACK_TYPE_WAREHOUSE4", Value = 8)]
        PACK_TYPE_WAREHOUSE4,
        [ProtoEnum(Name = "PACK_TYPE_CPASULE", Value = 9)]
        PACK_TYPE_CPASULE,
        [ProtoEnum(Name = "PACK_TYPE_MAX", Value = 10)]
        PACK_TYPE_MAX
    }
}
