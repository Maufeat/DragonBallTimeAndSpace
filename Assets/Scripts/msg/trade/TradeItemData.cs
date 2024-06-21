using System;
using System.ComponentModel;
using hero;
using msg;
using Obj;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "TradeItemData")]
    [Serializable]
    public class TradeItemData : IExtensible
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

        [ProtoMember(2, IsRequired = false, Name = "objdata", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public t_Object objdata
        {
            get
            {
                return this._objdata;
            }
            set
            {
                this._objdata = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(3, IsRequired = false, Name = "herodata", DataFormat = DataFormat.Default)]
        public Hero herodata
        {
            get
            {
                return this._herodata;
            }
            set
            {
                this._herodata = value;
            }
        }

        private SELLTYPE _itemtype;

        private t_Object _objdata;

        private Hero _herodata;

        private IExtension extensionObject;
    }
}
