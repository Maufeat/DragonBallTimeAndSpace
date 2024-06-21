using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_ReqSellStaff_CS")]
    [Serializable]
    public class MSG_ReqSellStaff_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "itemtype", DataFormat = DataFormat.TwosComplement)]
        public SELLTYPE itemtype
        {
            get
            {
                return this._itemtype;
            }
            set
            {
                this._itemtype = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
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
        [ProtoMember(3, IsRequired = false, Name = "baseid", DataFormat = DataFormat.TwosComplement)]
        public uint baseid
        {
            get
            {
                return this._baseid;
            }
            set
            {
                this._baseid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "price", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(5, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
        public uint num
        {
            get
            {
                return this._num;
            }
            set
            {
                this._num = value;
            }
        }

        private SELLTYPE _itemtype;

        private string _thisid = string.Empty;

        private uint _baseid;

        private uint _price;

        private uint _num;

        private IExtension extensionObject;
    }
}
