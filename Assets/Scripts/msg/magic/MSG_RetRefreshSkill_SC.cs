using System;
using System.Collections.Generic;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_RetRefreshSkill_SC")]
    [Serializable]
    public class MSG_RetRefreshSkill_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "skills", DataFormat = DataFormat.Default)]
        public List<SkillData> skills
        {
            get
            {
                return this._skills;
            }
        }

        private readonly List<SkillData> _skills = new List<SkillData>();

        private IExtension extensionObject;
    }
}
