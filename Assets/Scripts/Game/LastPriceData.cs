public class LastPriceData
{
    public LastPriceData(TradeObjType tradeType, uint baseid, uint price, float ratio)
    {
        this.TradeType = tradeType;
        this.Baseid = baseid;
        this.Price = price;
        this.Ratio = ratio;
    }

    public TradeObjType TradeType;

    public uint Baseid;

    public uint Price;

    public float Ratio;
}
