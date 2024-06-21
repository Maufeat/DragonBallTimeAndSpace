using System;
using System.ComponentModel;
using ProtoBuf;

namespace market
{
    [ProtoContract(Name = "MSG_ExchangeRatio_CSC")]
    [Serializable]
    public class MSG_ExchangeRatio_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "tone2nations", DataFormat = DataFormat.TwosComplement)]
        public uint tone2nations
        {
            get
            {
                return this._tone2nations;
            }
            set
            {
                this._tone2nations = value;
            }
        }

        private uint _tone2nations;

        private IExtension extensionObject;
    }
}
