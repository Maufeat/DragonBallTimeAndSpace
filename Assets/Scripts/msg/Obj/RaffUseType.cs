using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "RaffUseType")]
    public enum RaffUseType
    {
        [ProtoEnum(Name = "RAFFUSETYPE_FREETIMES", Value = 0)]
        RAFFUSETYPE_FREETIMES,
        [ProtoEnum(Name = "RAFFUSETYPE_OBJECT", Value = 1)]
        RAFFUSETYPE_OBJECT
    }
}
