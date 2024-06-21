using System;
using System.ComponentModel;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "MSG_Req_GuildPkJoinTeam_CS")]
    [Serializable]
    public class MSG_Req_GuildPkJoinTeam_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "teamid", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "posid", DataFormat = DataFormat.TwosComplement)]
        public uint posid
        {
            get
            {
                return this._posid;
            }
            set
            {
                this._posid = value;
            }
        }

        private uint _teamid;

        private uint _posid;

        private IExtension extensionObject;
    }
}
