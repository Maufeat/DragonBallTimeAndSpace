using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_RetNewApply_SC")]
    [Serializable]
    public class MSG_RetNewApply_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "count", DataFormat = DataFormat.TwosComplement)]
        public uint count
        {
            get
            {
                return this._count;
            }
            set
            {
                this._count = value;
            }
        }

        private uint _count;

        private IExtension extensionObject;
    }
}
