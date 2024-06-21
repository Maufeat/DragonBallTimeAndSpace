using System;
using System.Collections.Generic;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "MSG_Ret_GuildPkRank_SC")]
    [Serializable]
    public class MSG_Ret_GuildPkRank_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "scorerank", DataFormat = DataFormat.Default)]
        public List<GuildPkGuildScore> scorerank
        {
            get
            {
                return this._scorerank;
            }
        }

        private readonly List<GuildPkGuildScore> _scorerank = new List<GuildPkGuildScore>();

        private IExtension extensionObject;
    }
}
