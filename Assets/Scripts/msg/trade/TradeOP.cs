using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "TradeOP")]
    public enum TradeOP
    {
        [ProtoEnum(Name = "SELL", Value = 1)]
        SELL = 1,
        [ProtoEnum(Name = "BUY", Value = 2)]
        BUY
    }
}
