using System;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "GuildPkWinInfo")]
    [Serializable]
    public class GuildPkWinInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "uid", DataFormat = DataFormat.TwosComplement)]
        public ulong uid
        {
            get
            {
                return this._uid;
            }
            set
            {
                this._uid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "win_guild_name", DataFormat = DataFormat.Default)]
        public string win_guild_name
        {
            get
            {
                return this._win_guild_name;
            }
            set
            {
                this._win_guild_name = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(3, IsRequired = false, Name = "win_leader_name", DataFormat = DataFormat.Default)]
        public string win_leader_name
        {
            get
            {
                return this._win_leader_name;
            }
            set
            {
                this._win_leader_name = value;
            }
        }

        private ulong _uid;

        private string _win_guild_name = string.Empty;

        private string _win_leader_name = string.Empty;

        private IExtension extensionObject;
    }
}
