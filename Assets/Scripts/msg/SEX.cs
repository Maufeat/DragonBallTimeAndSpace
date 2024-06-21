using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "SEX")]
    public enum SEX
    {
        [ProtoEnum(Name = "NONE", Value = 1)]
        NONE = 1,
        [ProtoEnum(Name = "MALE", Value = 2)]
        MALE,
        [ProtoEnum(Name = "FEMALE", Value = 3)]
        FEMALE
    }
}
