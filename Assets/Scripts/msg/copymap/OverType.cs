using ProtoBuf;

namespace copymap
{
    [ProtoContract(Name = "OverType")]
    public enum OverType
    {
        [ProtoEnum(Name = "OVERMAP_ALLUSER_DEATH", Value = 1)]
        OVERMAP_ALLUSER_DEATH = 1,
        [ProtoEnum(Name = "OVERMAP_KILL_ONEBOSS", Value = 2)]
        OVERMAP_KILL_ONEBOSS,
        [ProtoEnum(Name = "OVERMAP_KILL_ALLBOSS", Value = 4)]
        OVERMAP_KILL_ALLBOSS = 4,
        [ProtoEnum(Name = "OVERMAP_TIME_OVER", Value = 8)]
        OVERMAP_TIME_OVER = 8
    }
}
