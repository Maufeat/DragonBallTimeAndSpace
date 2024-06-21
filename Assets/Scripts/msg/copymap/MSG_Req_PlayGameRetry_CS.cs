using System;
using System.ComponentModel;
using ProtoBuf;

namespace copymap
{
    [ProtoContract(Name = "MSG_Req_PlayGameRetry_CS")]
    [Serializable]
    public class MSG_Req_PlayGameRetry_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        public uint type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        private uint _type;

        private IExtension extensionObject;
    }
}
