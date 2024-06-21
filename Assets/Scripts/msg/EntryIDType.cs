using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "EntryIDType")]
    [Serializable]
    public class EntryIDType : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        public ulong id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
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

        private ulong _id;

        private uint _type;

        private IExtension extensionObject;
    }
}
