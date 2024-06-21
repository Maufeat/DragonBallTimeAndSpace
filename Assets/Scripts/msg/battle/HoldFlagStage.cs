using System;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "HoldFlagStage")]
    public enum HoldFlagStage
    {
        [ProtoEnum(Name = "HOLD_FLAG_STAGE_NONE", Value = 0)]
        HOLD_FLAG_STAGE_NONE,
        [ProtoEnum(Name = "HOLD_FLAG_STAGE_ENTER", Value = 1)]
        HOLD_FLAG_STAGE_ENTER,
        [ProtoEnum(Name = "HOLD_FLAG_STAGE_PREPARE", Value = 2)]
        HOLD_FLAG_STAGE_PREPARE,
        [ProtoEnum(Name = "HOLD_FLAG_STAGE_FIGHT", Value = 3)]
        HOLD_FLAG_STAGE_FIGHT,
        [ProtoEnum(Name = "HOLD_FLAG_STAGE_ACCOUNT", Value = 4)]
        HOLD_FLAG_STAGE_ACCOUNT,
        [ProtoEnum(Name = "HOLD_FLAG_STAGE_MAX", Value = 5)]
        HOLD_FLAG_STAGE_MAX
    }
}
