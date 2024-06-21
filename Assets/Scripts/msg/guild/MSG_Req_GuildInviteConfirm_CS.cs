using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_GuildInviteConfirm_CS")]
    [Serializable]
    public class MSG_Req_GuildInviteConfirm_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "inviterid", DataFormat = DataFormat.Default)]
        public string inviterid
        {
            get
            {
                return this._inviterid;
            }
            set
            {
                this._inviterid = value;
            }
        }

        private string _inviterid = string.Empty;

        private IExtension extensionObject;
    }
}
