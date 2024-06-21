using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_GuildInfo_SC")]
    [Serializable]
    public class MSG_Ret_GuildInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "guildinfo", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public guildInfo guildinfo
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

        [ProtoMember(2, IsRequired = false, Name = "myinfo", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public guildMember myinfo
        {
            get
            {
                return this._myinfo;
            }
            set
            {
                this._myinfo = value;
            }
        }

        private guildInfo _guildinfo;

        private guildMember _myinfo;

        private IExtension extensionObject;
    }
}
