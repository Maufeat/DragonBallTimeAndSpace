using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "GuildPkTeamInfo")]
    [Serializable]
    public class GuildPkTeamInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "teamid", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(2, IsRequired = false, Name = "unlocklv", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint unlocklv
        {
            get
            {
                return this._unlocklv;
            }
            set
            {
                this._unlocklv = value;
            }
        }

        [ProtoMember(3, Name = "members", DataFormat = DataFormat.Default)]
        public List<GuildPkMemberInfo> members
        {
            get
            {
                return this._members;
            }
        }

        private uint _teamid;

        private uint _unlocklv;

        private readonly List<GuildPkMemberInfo> _members = new List<GuildPkMemberInfo>();

        private IExtension extensionObject;
    }
}
