using System;
using ProtoBuf;

namespace market
{
    [ProtoContract(Name = "MarketType")]
    public enum MarketType
    {
        [ProtoEnum(Name = "MarketType_1", Value = 1)]
        MarketType_1 = 1,
        [ProtoEnum(Name = "MarketType_2", Value = 2)]
        MarketType_2,
        [ProtoEnum(Name = "MarketType_3", Value = 3)]
        MarketType_3,
        [ProtoEnum(Name = "MarketType_4", Value = 4)]
        MarketType_4,
        [ProtoEnum(Name = "MarketType_5", Value = 5)]
        MarketType_5,
        [ProtoEnum(Name = "MarketType_6", Value = 6)]
        MarketType_6,
        [ProtoEnum(Name = "MarketType_7", Value = 7)]
        MarketType_7
    }
}
