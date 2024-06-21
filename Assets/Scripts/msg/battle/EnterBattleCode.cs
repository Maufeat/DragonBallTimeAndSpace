using System;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "EnterBattleCode")]
    public enum EnterBattleCode
    {
        [ProtoEnum(Name = "BATTLE_ENTER_SUCCESS", Value = 1)]
        BATTLE_ENTER_SUCCESS = 1,
        [ProtoEnum(Name = "BATTLE_ENTER_INCOPYMAP_FAILED", Value = 2)]
        BATTLE_ENTER_INCOPYMAP_FAILED,
        [ProtoEnum(Name = "BATTLE_ENTER_ENTERROOM_FAILED", Value = 3)]
        BATTLE_ENTER_ENTERROOM_FAILED,
        [ProtoEnum(Name = "BATTLE_ENTER_OVERTIME_FAILED", Value = 4)]
        BATTLE_ENTER_OVERTIME_FAILED,
        [ProtoEnum(Name = "BATTLE_ENTER_NOROOM_FAILED", Value = 5)]
        BATTLE_ENTER_NOROOM_FAILED,
        [ProtoEnum(Name = "BATTLE_ENTER_KICKED_FAILED", Value = 6)]
        BATTLE_ENTER_KICKED_FAILED
    }
}
