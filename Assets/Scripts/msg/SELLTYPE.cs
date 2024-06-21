using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "SELLTYPE")]
    public enum SELLTYPE
    {
        [ProtoEnum(Name = "SELLNONE", Value = 0)]
        SELLNONE,
        [ProtoEnum(Name = "OBJECT", Value = 1)]
        OBJECT,
        [ProtoEnum(Name = "HERO", Value = 2)]
        HERO
    }
}
