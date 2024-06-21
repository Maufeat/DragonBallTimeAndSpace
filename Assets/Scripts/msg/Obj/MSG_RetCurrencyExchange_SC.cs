using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_RetCurrencyExchange_SC")]
    [Serializable]
    public class MSG_RetCurrencyExchange_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "result", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
            }
        }

        private uint _result;

        private IExtension extensionObject;
    }
}
