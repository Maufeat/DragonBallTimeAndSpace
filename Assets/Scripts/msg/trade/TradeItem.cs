using System;
using System.ComponentModel;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "TradeItem")]
    [Serializable]
    public class TradeItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string thisid
        {
            get
            {
                return this._thisid;
            }
            set
            {
                this._thisid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "price", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(3, IsRequired = false, Name = "selltime", DataFormat = DataFormat.TwosComplement)]
        public uint selltime
        {
            get
            {
                return this._selltime;
            }
            set
            {
                this._selltime = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(4, IsRequired = false, Name = "data", DataFormat = DataFormat.Default)]
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

        [DefaultValue(false)]
        [ProtoMember(5, IsRequired = false, Name = "isshow", DataFormat = DataFormat.Default)]
        public bool isshow
        {
            get
            {
                return this._isshow;
            }
            set
            {
                this._isshow = value;
            }
        }

        private string _thisid = string.Empty;

        private uint _price;

        private uint _selltime;

        private TradeItemData _data;

        private bool _isshow;

        private IExtension extensionObject;
    }
}
