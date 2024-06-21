using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_InviteIntoTeam_CS")]
    [Serializable]
    public class MSG_InviteIntoTeam_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "inviteeid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string inviteeid
        {
            get
            {
                return this._inviteeid;
            }
            set
            {
                this._inviteeid = value;
            }
        }

        private string _inviteeid = string.Empty;

        private IExtension extensionObject;
    }
}
