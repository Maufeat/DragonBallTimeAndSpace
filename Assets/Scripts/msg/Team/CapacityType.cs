using System;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "CapacityType")]
    public enum CapacityType
    {
        [ProtoEnum(Name = "Capacity_Small", Value = 1)]
        Capacity_Small = 1,
        [ProtoEnum(Name = "Capacity_Big", Value = 2)]
        Capacity_Big
    }
}
