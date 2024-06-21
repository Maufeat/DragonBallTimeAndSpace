using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_ReqBuyItem_CS")]
    [Serializable]
    public class MSG_ReqBuyItem_CS : IExtensible
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

        [ProtoMember(2, IsRequired = false, Name = "baseid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [ProtoMember(3, IsRequired = false, Name = "levelstar", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint levelstar
        {
            get
            {
                return this._levelstar;
            }
            set
            {
                this._levelstar = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(5, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
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

        private SELLTYPE _itemtype;

        private uint _baseid;

        private uint _levelstar;

        private uint _num;

        private string _thisid = string.Empty;

        private IExtension extensionObject;
    }
}
