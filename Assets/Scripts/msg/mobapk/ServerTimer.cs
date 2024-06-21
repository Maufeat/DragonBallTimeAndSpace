using System;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "ServerTimer")]
    public enum ServerTimer
    {
        [ProtoEnum(Name = "MobaPk_Confirm_RestTime", Value = 1)]
        MobaPk_Confirm_RestTime = 1,
        [ProtoEnum(Name = "MobaPk_Start_RestTime", Value = 2)]
        MobaPk_Start_RestTime,
        [ProtoEnum(Name = "MobaPk_Relive_RestTime", Value = 3)]
        MobaPk_Relive_RestTime,
        [ProtoEnum(Name = "MobaPk_KickoutLastOne_RestTime", Value = 4)]
        MobaPk_KickoutLastOne_RestTime,
        [ProtoEnum(Name = "MobaPk_Pray_RestTime", Value = 5)]
        MobaPk_Pray_RestTime
    }
}
