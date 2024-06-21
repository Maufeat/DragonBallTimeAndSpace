using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_MyTeamInfo_SC")]
    [Serializable]
    public class MSG_MyTeamInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "users", DataFormat = DataFormat.Default)]
        public List<TeamUser> users
        {
            get
            {
                return this._users;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "create_time", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint create_time
        {
            get
            {
                return this._create_time;
            }
            set
            {
                this._create_time = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "teamid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string teamid
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

        private readonly List<TeamUser> _users = new List<TeamUser>();

        private uint _create_time;

        private string _teamid = string.Empty;

        private IExtension extensionObject;
    }
}
