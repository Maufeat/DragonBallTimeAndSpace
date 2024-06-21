using System;
using System.Collections.Generic;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "HeroSkill")]
    [Serializable]
    public class HeroSkill : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "skill", DataFormat = DataFormat.Default)]
        public List<HeroSkillItem> skill
        {
            get
            {
                return this._skill;
            }
        }

        private readonly List<HeroSkillItem> _skill = new List<HeroSkillItem>();

        private IExtension extensionObject;
    }
}
