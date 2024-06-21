using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_ReqTradeItemHistory_CS")]
    [Serializable]
    public class MSG_ReqTradeItemHistory_CS : IExtensible
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

        private SELLTYPE _itemtype;

        private uint _baseid;

        private IExtension extensionObject;
    }
}
