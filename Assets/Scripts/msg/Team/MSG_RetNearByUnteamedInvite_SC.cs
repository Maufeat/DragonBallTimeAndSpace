using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_RetNearByUnteamedInvite_SC")]
    [Serializable]
    public class MSG_RetNearByUnteamedInvite_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "inviter", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public Memember inviter
        {
            get
            {
                return this._inviter;
            }
            set
            {
                this._inviter = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "invitername", DataFormat = DataFormat.Default)]
        public string invitername
        {
            get
            {
                return this._invitername;
            }
            set
            {
                this._invitername = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(3, IsRequired = false, Name = "inviterid", DataFormat = DataFormat.Default)]
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
        [ProtoMember(4, IsRequired = false, Name = "inviteename", DataFormat = DataFormat.Default)]
        public string inviteename
        {
            get
            {
                return this._inviteename;
            }
            set
            {
                this._inviteename = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(5, IsRequired = false, Name = "inviteeid", DataFormat = DataFormat.Default)]
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

        [DefaultValue("")]
        [ProtoMember(6, IsRequired = false, Name = "teamname", DataFormat = DataFormat.Default)]
        public string teamname
        {
            get
            {
                return this._teamname;
            }
            set
            {
                this._teamname = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "teamid", DataFormat = DataFormat.TwosComplement)]
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

        private Memember _inviter;

        private string _invitername = string.Empty;

        private string _inviterid = string.Empty;

        private string _inviteename = string.Empty;

        private string _inviteeid = string.Empty;

        private string _teamname = string.Empty;

        private uint _teamid;

        private IExtension extensionObject;
    }
}
