using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_ApplyForGuild_CS")]
    [Serializable]
    public class MSG_Req_ApplyForGuild_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "guildid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
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

        [ProtoMember(2, IsRequired = false, Name = "flag", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool flag
        {
            get
            {
                return this._flag;
            }
            set
            {
                this._flag = value;
            }
        }

        private ulong _guildid;

        private bool _flag;

        private IExtension extensionObject;
    }
}
