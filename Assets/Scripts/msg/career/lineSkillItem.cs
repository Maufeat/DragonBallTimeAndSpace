using System;
using System.ComponentModel;
using ProtoBuf;

namespace career
{
    [ProtoContract(Name = "lineSkillItem")]
    [Serializable]
    public class lineSkillItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "line", DataFormat = DataFormat.TwosComplement)]
        public uint line
        {
            get
            {
                return this._line;
            }
            set
            {
                this._line = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "skill", DataFormat = DataFormat.TwosComplement)]
        public uint skill
        {
            get
            {
                return this._skill;
            }
            set
            {
                this._skill = value;
            }
        }

        private uint _line;

        private uint _skill;

        private IExtension extensionObject;
    }
}
