using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "ReliveType")]
    public enum ReliveType
    {
        [ProtoEnum(Name = "RELIVE_NONE", Value = 0)]
        RELIVE_NONE,
        [ProtoEnum(Name = "RELIVE_HOME", Value = 1)]
        RELIVE_HOME,
        [ProtoEnum(Name = "RELIVE_ORIGIN", Value = 2)]
        RELIVE_ORIGIN,
        [ProtoEnum(Name = "RELIVE_GUILD", Value = 3)]
        RELIVE_GUILD,
        [ProtoEnum(Name = "RELIVE_PKRAND", Value = 4)]
        RELIVE_PKRAND,
        [ProtoEnum(Name = "RELIVE_COUNTRY", Value = 5)]
        RELIVE_COUNTRY,
        [ProtoEnum(Name = "RELIVE_NEAREST", Value = 6)]
        RELIVE_NEAREST,
        [ProtoEnum(Name = "RELIVE_BATTLE", Value = 7)]
        RELIVE_BATTLE,
        [ProtoEnum(Name = "RELIVE_SKILL", Value = 8)]
        RELIVE_SKILL
    }
}
