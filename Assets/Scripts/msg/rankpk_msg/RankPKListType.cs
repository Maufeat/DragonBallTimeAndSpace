using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "RankPKListType")]
    public enum RankPKListType
    {
        [ProtoEnum(Name = "RankPKListType_All", Value = 0)]
        RankPKListType_All,
        [ProtoEnum(Name = "RankPKListType_Friend", Value = 1)]
        RankPKListType_Friend
    }
}
