using System;
using System.Collections.Generic;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "MSG_RealTime_GuildPkTeam_Rank_SC")]
    [Serializable]
    public class MSG_RealTime_GuildPkTeam_Rank_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "teamrank", DataFormat = DataFormat.Default)]
        public List<realtime_guildteam_info> teamrank
        {
            get
            {
                return this._teamrank;
            }
        }

        private readonly List<realtime_guildteam_info> _teamrank = new List<realtime_guildteam_info>();

        private IExtension extensionObject;
    }
}
