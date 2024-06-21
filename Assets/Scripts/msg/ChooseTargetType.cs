using System;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "ChooseTargetType")]
    public enum ChooseTargetType
    {
        [ProtoEnum(Name = "CHOOSE_TARGE_TTYPE_SET", Value = 1)]
        CHOOSE_TARGE_TTYPE_SET = 1,
        [ProtoEnum(Name = "CHOOSE_TARGE_TTYPE_CANCEL", Value = 2)]
        CHOOSE_TARGE_TTYPE_CANCEL
    }
}
