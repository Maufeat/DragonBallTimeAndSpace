using System;
using System.Collections.Generic;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_GuildSkill_SC")]
    [Serializable]
    public class MSG_Ret_GuildSkill_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "skillinfo", DataFormat = DataFormat.Default)]
        public List<guildSkill> skillinfo
        {
            get
            {
                return this._skillinfo;
            }
        }

        private readonly List<guildSkill> _skillinfo = new List<guildSkill>();

        private IExtension extensionObject;
    }
}
