using System;
using ProtoBuf;

namespace Pet
{
    [ProtoContract(Name = "PetState")]
    public enum PetState
    {
        [ProtoEnum(Name = "PetState_None", Value = 0)]
        PetState_None,
        [ProtoEnum(Name = "PetState_Rest", Value = 1)]
        PetState_Rest,
        [ProtoEnum(Name = "PetState_Assist", Value = 2)]
        PetState_Assist,
        [ProtoEnum(Name = "PetState_Fight", Value = 3)]
        PetState_Fight,
        [ProtoEnum(Name = "PetState_Dying", Value = 4)]
        PetState_Dying
    }
}
