using System;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "CancelBatteMatchCode")]
    public enum CancelBatteMatchCode
    {
        [ProtoEnum(Name = "BATTLE_CANCELMATCH_SUCCESS", Value = 1)]
        BATTLE_CANCELMATCH_SUCCESS = 1,
        [ProtoEnum(Name = "BATTLE_CANCELMATCH_TEAMRIGHT_FAILED", Value = 2)]
        BATTLE_CANCELMATCH_TEAMRIGHT_FAILED,
        [ProtoEnum(Name = "BATTLE_CANCELMATCH_NOMATCH_FAILED", Value = 3)]
        BATTLE_CANCELMATCH_NOMATCH_FAILED,
        [ProtoEnum(Name = "BATTLE_CANCELMATCH_INTER_FAILED", Value = 4)]
        BATTLE_CANCELMATCH_INTER_FAILED,
        [ProtoEnum(Name = "BATTLE_CANCELMATCH_MATCHOVER_FAILED", Value = 5)]
        BATTLE_CANCELMATCH_MATCHOVER_FAILED
    }
}
