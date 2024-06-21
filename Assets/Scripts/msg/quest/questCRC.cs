using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "questCRC")]
    [Serializable]
    public class questCRC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "quest_id", DataFormat = DataFormat.TwosComplement)]
        public uint quest_id
        {
            get
            {
                return this._quest_id;
            }
            set
            {
                this._quest_id = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "branch_id", DataFormat = DataFormat.TwosComplement)]
        public uint branch_id
        {
            get
            {
                return this._branch_id;
            }
            set
            {
                this._branch_id = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "crc", DataFormat = DataFormat.TwosComplement)]
        public uint crc
        {
            get
            {
                return this._crc;
            }
            set
            {
                this._crc = value;
            }
        }

        private uint _quest_id;

        private uint _branch_id;

        private uint _crc;

        private IExtension extensionObject;
    }
}
