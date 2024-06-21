using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "ATTACKRESULT")]
    public enum ATTACKRESULT
    {
        [ProtoEnum(Name = "ATTACKRESULT_NONE", Value = 0)]
        ATTACKRESULT_NONE,
        [ProtoEnum(Name = "ATTACKRESULT_MISS", Value = 1)]
        ATTACKRESULT_MISS,
        [ProtoEnum(Name = "ATTACKRESULT_NORMAL", Value = 2)]
        ATTACKRESULT_NORMAL,
        [ProtoEnum(Name = "ATTACKRESULT_BANG", Value = 3)]
        ATTACKRESULT_BANG,
        [ProtoEnum(Name = "ATTACKRESULT_HOLD", Value = 4)]
        ATTACKRESULT_HOLD,
        [ProtoEnum(Name = "ATTACKRESULT_BLOCK", Value = 5)]
        ATTACKRESULT_BLOCK,
        [ProtoEnum(Name = "ATTACKRESULT_DEFLECT", Value = 6)]
        ATTACKRESULT_DEFLECT,
        [ProtoEnum(Name = "ATTACKRESULT_HIT", Value = 8)]
        ATTACKRESULT_HIT = 8
    }
}
