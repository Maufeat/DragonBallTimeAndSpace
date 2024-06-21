using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_AddMemember_SC")]
    [Serializable]
    public class MSG_AddMemember_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "mem", DataFormat = DataFormat.Default)]
        public Memember mem
        {
            get
            {
                return this._mem;
            }
            set
            {
                this._mem = value;
            }
        }

        private Memember _mem;

        private IExtension extensionObject;
    }
}
