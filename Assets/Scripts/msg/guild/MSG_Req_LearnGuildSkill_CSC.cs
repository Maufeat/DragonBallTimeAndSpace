using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_LearnGuildSkill_CSC")]
    [Serializable]
    public class MSG_Req_LearnGuildSkill_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "skillinfo", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public guildSkill skillinfo
        {
            get
            {
                return this._skillinfo;
            }
            set
            {
                this._skillinfo = value;
            }
        }

        private guildSkill _skillinfo;

        private IExtension extensionObject;
    }
}
