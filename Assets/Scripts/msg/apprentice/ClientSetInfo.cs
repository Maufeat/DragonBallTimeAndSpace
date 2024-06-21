using System;
using System.ComponentModel;
using ProtoBuf;

namespace apprentice
{
    [ProtoContract(Name = "ClientSetInfo")]
    [Serializable]
    public class ClientSetInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "key", DataFormat = DataFormat.Default)]
        public string key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "value", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        private string _key = string.Empty;

        private uint _value;

        private IExtension extensionObject;
    }
}
