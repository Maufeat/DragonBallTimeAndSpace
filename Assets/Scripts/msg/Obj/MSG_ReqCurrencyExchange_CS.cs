using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqCurrencyExchange_CS")]
    [Serializable]
    public class MSG_ReqCurrencyExchange_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "usequantity", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint usequantity
        {
            get
            {
                return this._usequantity;
            }
            set
            {
                this._usequantity = value;
            }
        }

        private uint _usequantity;

        private IExtension extensionObject;
    }
}
