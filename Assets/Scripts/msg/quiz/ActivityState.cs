using System;
using ProtoBuf;

namespace quiz
{
    [ProtoContract(Name = "ActivityState")]
    public enum ActivityState
    {
        [ProtoEnum(Name = "ACTIVITY_STATE_UNOPEN", Value = 0)]
        ACTIVITY_STATE_UNOPEN,
        [ProtoEnum(Name = "ACTIVITY_STATE_OPEN", Value = 1)]
        ACTIVITY_STATE_OPEN,
        [ProtoEnum(Name = "ACTIVITY_STATE_COMPLETE", Value = 2)]
        ACTIVITY_STATE_COMPLETE,
        [ProtoEnum(Name = "ACTIVITY_STATE_GOTPRIZE", Value = 3)]
        ACTIVITY_STATE_GOTPRIZE,
        [ProtoEnum(Name = "ACTIVITY_STATE_UNCOMPLETE", Value = 4)]
        ACTIVITY_STATE_UNCOMPLETE
    }
}
