using System;
using System.Collections.Generic;
using System.ComponentModel;
using magic;
using ProtoBuf;

namespace career
{
    [ProtoContract(Name = "careerunlockItem")]
    [Serializable]
    public class careerunlockItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "skill", DataFormat = DataFormat.Default)]
        public SkillData skill
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

        [ProtoMember(2, Name = "extskill", DataFormat = DataFormat.Default)]
        public List<ExtSkillData> extskill
        {
            get
            {
                return this._extskill;
            }
        }

        private SkillData _skill;

        private readonly List<ExtSkillData> _extskill = new List<ExtSkillData>();

        private IExtension extensionObject;
    }
}
