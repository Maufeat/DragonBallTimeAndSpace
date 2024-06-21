using System;
using System.ComponentModel;
using ProtoBuf;

namespace market
{
    [ProtoContract(Name = "MSG_ReqBuyMarketItem_CS")]
    [Serializable]
    public class MSG_ReqBuyMarketItem_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "marketid", DataFormat = DataFormat.TwosComplement)]
        public uint marketid
        {
            get
            {
                return this._marketid;
            }
            set
            {
                this._marketid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "itemid", DataFormat = DataFormat.TwosComplement)]
        public uint itemid
        {
            get
            {
                return this._itemid;
            }
            set
            {
                this._itemid = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "itemnum", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint itemnum
        {
            get
            {
                return this._itemnum;
            }
            set
            {
                this._itemnum = value;
            }
        }

        private uint _marketid;

        private uint _id;

        private uint _itemid;

        private uint _itemnum;

        private IExtension extensionObject;
    }
}
