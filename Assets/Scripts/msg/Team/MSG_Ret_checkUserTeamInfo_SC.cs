using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_Ret_checkUserTeamInfo_SC")]
    [Serializable]
    public class MSG_Ret_checkUserTeamInfo_SC : IExtensible
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

        [ProtoMember(2, IsRequired = false, Name = "teamid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong teamid
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

        [ProtoMember(3, IsRequired = false, Name = "online", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool online
        {
            get
            {
                return this._online;
            }
            set
            {
                this._online = value;
            }
        }

        private ulong _memberid;

        private ulong _teamid;

        private bool _online;

        private IExtension extensionObject;
    }
}
