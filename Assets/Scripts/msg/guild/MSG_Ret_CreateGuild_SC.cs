using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_CreateGuild_SC")]
    [Serializable]
    public class MSG_Ret_CreateGuild_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "guildname", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
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

        [DefaultValue(false)]
        [ProtoMember(2, IsRequired = false, Name = "issucc", DataFormat = DataFormat.Default)]
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

        private string _guildname = string.Empty;

        private bool _issucc;

        private IExtension extensionObject;
    }
}
