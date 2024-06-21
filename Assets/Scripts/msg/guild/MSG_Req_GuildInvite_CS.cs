using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_GuildInvite_CS")]
    [Serializable]
    public class MSG_Req_GuildInvite_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "joinmemberid", DataFormat = DataFormat.Default)]
        public string joinmemberid
        {
            get
            {
                return this._joinmemberid;
            }
            set
            {
                this._joinmemberid = value;
            }
        }

        private string _joinmemberid = string.Empty;

        private IExtension extensionObject;
    }
}
