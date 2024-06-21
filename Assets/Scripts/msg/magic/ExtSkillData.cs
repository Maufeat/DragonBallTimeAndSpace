using System;
using System.ComponentModel;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "ExtSkillData")]
    [Serializable]
    public class ExtSkillData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint id
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
        [ProtoMember(2, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
        public uint level
        {
            get
            {
                return this._level;
            }
            set
            {
                this._level = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "masterskill", DataFormat = DataFormat.TwosComplement)]
        public uint masterskill
        {
            get
            {
                return this._masterskill;
            }
            set
            {
                this._masterskill = value;
            }
        }

        private uint _id;

        private uint _level;

        private uint _masterskill;

        private IExtension extensionObject;
    }
}
