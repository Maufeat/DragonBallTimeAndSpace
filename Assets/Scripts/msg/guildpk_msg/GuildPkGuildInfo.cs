using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "GuildPkGuildInfo")]
    [Serializable]
    public class GuildPkGuildInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "guildid", DataFormat = DataFormat.TwosComplement)]
        public ulong guildid
        {
            get
            {
                return this._guildid;
            }
            set
            {
                this._guildid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "guildname", DataFormat = DataFormat.Default)]
        public string guildname
        {
            get
            {
                return this._guildname;
            }
            set
            {
                this._guildname = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "teamlimit", DataFormat = DataFormat.TwosComplement)]
        public uint teamlimit
        {
            get
            {
                return this._teamlimit;
            }
            set
            {
                this._teamlimit = value;
            }
        }

        [ProtoMember(4, Name = "teaminfo", DataFormat = DataFormat.Default)]
        public List<GuildPkTeamInfo> teaminfo
        {
            get
            {
                return this._teaminfo;
            }
        }

        private ulong _guildid;

        private string _guildname = string.Empty;

        private uint _teamlimit;

        private readonly List<GuildPkTeamInfo> _teaminfo = new List<GuildPkTeamInfo>();

        private IExtension extensionObject;
    }
}
