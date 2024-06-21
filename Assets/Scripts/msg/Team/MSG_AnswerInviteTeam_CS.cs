using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_AnswerInviteTeam_CS")]
    [Serializable]
    public class MSG_AnswerInviteTeam_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(false)]
        [ProtoMember(1, IsRequired = false, Name = "yesorno", DataFormat = DataFormat.Default)]
        public bool yesorno
        {
            get
            {
                return this._yesorno;
            }
            set
            {
                this._yesorno = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "inviterid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
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

        [DefaultValue("")]
        [ProtoMember(3, IsRequired = false, Name = "inviteeid", DataFormat = DataFormat.Default)]
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

        [ProtoMember(4, IsRequired = false, Name = "teamid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint teamid
        {
            get
            {
                return this._teamid;
            }
            set
            {
                this._teamid = value;
            }
        }

        private bool _yesorno;

        private string _inviterid = string.Empty;

        private string _inviteeid = string.Empty;

        private uint _teamid;

        private IExtension extensionObject;
    }
}
