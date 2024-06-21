using System;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "StageType")]
    public enum StageType
    {
        [ProtoEnum(Name = "None_Stage", Value = 0)]
        None_Stage,
        [ProtoEnum(Name = "Match", Value = 1)]
        Match,
        [ProtoEnum(Name = "Login", Value = 2)]
        Login,
        [ProtoEnum(Name = "Prepare", Value = 3)]
        Prepare,
        [ProtoEnum(Name = "PreFight", Value = 4)]
        PreFight,
        [ProtoEnum(Name = "CountDown", Value = 5)]
        CountDown,
        [ProtoEnum(Name = "Fight", Value = 6)]
        Fight,
        [ProtoEnum(Name = "Speedup", Value = 7)]
        Speedup,
        [ProtoEnum(Name = "Finish", Value = 8)]
        Finish,
        [ProtoEnum(Name = "Max_Stage", Value = 9)]
        Max_Stage
    }
}
