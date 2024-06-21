using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_ApplyForGuild_SC")]
    [Serializable]
    public class MSG_Ret_ApplyForGuild_SC : IExtensible
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

        [ProtoMember(2, IsRequired = false, Name = "issucc", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool issucc
        {
            get
            {
                return this._issucc;
            }
            set
            {
                this._issucc = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "flag", DataFormat = DataFormat.Default)]
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

        private bool _issucc;

        private bool _flag;

        private IExtension extensionObject;
    }
}
