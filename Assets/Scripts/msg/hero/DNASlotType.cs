using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "DNASlotType")]
    public enum DNASlotType
    {
        [ProtoEnum(Name = "ATT", Value = 0)]
        ATT,
        [ProtoEnum(Name = "DEF", Value = 1)]
        DEF,
        [ProtoEnum(Name = "ALL", Value = 2)]
        ALL
    }
}
