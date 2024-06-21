using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "AnswerType")]
    public enum AnswerType
    {
        [ProtoEnum(Name = "AnswerType_Yes", Value = 1)]
        AnswerType_Yes = 1,
        [ProtoEnum(Name = "AnswerType_No", Value = 2)]
        AnswerType_No
    }
}
