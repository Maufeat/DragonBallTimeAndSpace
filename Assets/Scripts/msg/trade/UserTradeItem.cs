using System;
using System.ComponentModel;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "UserTradeItem")]
    [Serializable]
    public class UserTradeItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(TradeOP.SELL)]
        [ProtoMember(1, IsRequired = false, Name = "op", DataFormat = DataFormat.TwosComplement)]
        public TradeOP op
        {
            get
            {
                return this._op;
            }
            set
            {
                this._op = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(2, IsRequired = false, Name = "item", DataFormat = DataFormat.Default)]
        public TradeHistoryItem item
        {
            get
            {
                return this._item;
            }
            set
            {
                this._item = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "judgeduetime", DataFormat = DataFormat.TwosComplement)]
        public uint judgeduetime
        {
            get
            {
                return this._judgeduetime;
            }
            set
            {
                this._judgeduetime = value;
            }
        }

        private TradeOP _op = TradeOP.SELL;

        private TradeHistoryItem _item;

        private uint _judgeduetime;

        private IExtension extensionObject;
    }
}
