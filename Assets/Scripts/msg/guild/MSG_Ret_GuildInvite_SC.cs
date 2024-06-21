using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_GuildInvite_SC")]
    [Serializable]
    public class MSG_Ret_GuildInvite_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint retcode
        {
            get
            {
                return this._retcode;
            }
            set
            {
                this._retcode = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "id", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(3, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(4, IsRequired = false, Name = "guildname", DataFormat = DataFormat.Default)]
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

        private uint _retcode;

        private string _id = string.Empty;

        private string _name = string.Empty;

        private string _guildname = string.Empty;

        private IExtension extensionObject;
    }
}
