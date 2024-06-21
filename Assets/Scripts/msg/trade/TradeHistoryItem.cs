using System;
using System.ComponentModel;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "TradeHistoryItem")]
    [Serializable]
    public class TradeHistoryItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "data", DataFormat = DataFormat.Default)]
        public TradeItemData data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "price", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint price
        {
            get
            {
                return this._price;
            }
            set
            {
                this._price = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "tradetime", DataFormat = DataFormat.TwosComplement)]
        public uint tradetime
        {
            get
            {
                return this._tradetime;
            }
            set
            {
                this._tradetime = value;
            }
        }

        private TradeItemData _data;

        private uint _price;

        private uint _tradetime;

        private IExtension extensionObject;
    }
}
