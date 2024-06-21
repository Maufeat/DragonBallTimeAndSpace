using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_ReqDonateSalary_CS")]
    [Serializable]
    public class MSG_ReqDonateSalary_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "value", DataFormat = DataFormat.TwosComplement)]
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

        private uint _value;

        private IExtension extensionObject;
    }
}
