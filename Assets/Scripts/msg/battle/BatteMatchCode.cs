using System;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "BatteMatchCode")]
    public enum BatteMatchCode
    {
        [ProtoEnum(Name = "BATTLE_MATCH_SUCCESS", Value = 1)]
        BATTLE_MATCH_SUCCESS = 1,
        [ProtoEnum(Name = "BATTLE_MATCH_LEVEL_FAILED", Value = 2)]
        BATTLE_MATCH_LEVEL_FAILED,
        [ProtoEnum(Name = "BATTLE_MATCH_COPYMAP_FAILED", Value = 3)]
        BATTLE_MATCH_COPYMAP_FAILED,
        [ProtoEnum(Name = "BATTLE_MATCH_MATCHING_FAILED", Value = 4)]
        BATTLE_MATCH_MATCHING_FAILED,
        [ProtoEnum(Name = "BATTLE_MATCH_TEAMLEVEL_FAILED", Value = 5)]
        BATTLE_MATCH_TEAMLEVEL_FAILED,
        [ProtoEnum(Name = "BATTLE_MATCH_TEAMRIGHT_FAILED", Value = 6)]
        BATTLE_MATCH_TEAMRIGHT_FAILED,
        [ProtoEnum(Name = "BATTLE_MATCH_MEMBEROFFLINE_FAILED", Value = 7)]
        BATTLE_MATCH_MEMBEROFFLINE_FAILED,
        [ProtoEnum(Name = "BATTLE_MATCH_TIME_FAILED", Value = 8)]
        BATTLE_MATCH_TIME_FAILED
    }
}
