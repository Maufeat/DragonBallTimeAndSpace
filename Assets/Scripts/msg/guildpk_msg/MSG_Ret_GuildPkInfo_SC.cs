using System;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "MSG_Ret_GuildPkInfo_SC")]
    [Serializable]
    public class MSG_Ret_GuildPkInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "guildinfo", DataFormat = DataFormat.Default)]
        public GuildPkGuildInfo guildinfo
        {
            get
            {
                return this._guildinfo;
            }
            set
            {
                this._guildinfo = value;
            }
        }

        private GuildPkGuildInfo _guildinfo;

        private IExtension extensionObject;
    }
}
