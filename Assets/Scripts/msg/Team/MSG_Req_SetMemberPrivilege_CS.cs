using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_Req_SetMemberPrivilege_CS")]
    [Serializable]
    public class MSG_Req_SetMemberPrivilege_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "memberid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong memberid
        {
            get
            {
                return this._memberid;
            }
            set
            {
                this._memberid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "privilege", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(TeamPrivilege.TeamPrivilege_Invite)]
        public TeamPrivilege privilege
        {
            get
            {
                return this._privilege;
            }
            set
            {
                this._privilege = value;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(3, IsRequired = false, Name = "set", DataFormat = DataFormat.Default)]
        public bool set
        {
            get
            {
                return this._set;
            }
            set
            {
                this._set = value;
            }
        }

        private ulong _memberid;

        private TeamPrivilege _privilege = TeamPrivilege.TeamPrivilege_Invite;

        private bool _set;

        private IExtension extensionObject;
    }
}
