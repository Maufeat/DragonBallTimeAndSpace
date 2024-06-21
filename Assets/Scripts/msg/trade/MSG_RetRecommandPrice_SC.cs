using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_RetRecommandPrice_SC")]
    [Serializable]
    public class MSG_RetRecommandPrice_SC : IExtensible
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "baseid", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(3, IsRequired = false, Name = "price", DataFormat = DataFormat.TwosComplement)]
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

        private SELLTYPE _itemtype;

        private uint _baseid;

        private uint _price;

        private IExtension extensionObject;
    }
}
