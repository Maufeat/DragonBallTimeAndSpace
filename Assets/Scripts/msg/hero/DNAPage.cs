using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "DNAPage")]
    public enum DNAPage
    {
        [ProtoEnum(Name = "NONE", Value = 0)]
        NONE,
        [ProtoEnum(Name = "PAGE1", Value = 1)]
        PAGE1,
        [ProtoEnum(Name = "PAGE2", Value = 2)]
        PAGE2,
        [ProtoEnum(Name = "PAGE3", Value = 3)]
        PAGE3,
        [ProtoEnum(Name = "PAGE4", Value = 4)]
        PAGE4,
        [ProtoEnum(Name = "MAX", Value = 5)]
        MAX
    }
}
