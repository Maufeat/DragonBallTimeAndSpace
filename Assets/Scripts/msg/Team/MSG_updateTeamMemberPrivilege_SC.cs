using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_updateTeamMemberPrivilege_SC")]
    [Serializable]
    public class MSG_updateTeamMemberPrivilege_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "memberid", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "privilege", DataFormat = DataFormat.TwosComplement)]
        public uint privilege
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

        private ulong _memberid;

        private uint _privilege;

        private IExtension extensionObject;
    }
}
