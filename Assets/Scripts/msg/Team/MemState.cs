using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MemState")]
    public enum MemState
    {
        [ProtoEnum(Name = "NORMAL", Value = 0)]
        NORMAL,
        [ProtoEnum(Name = "AWAY", Value = 1)]
        AWAY,
        [ProtoEnum(Name = "OFFLINE", Value = 2)]
        OFFLINE,
        [ProtoEnum(Name = "HOSTING", Value = 3)]
        HOSTING
    }
}
