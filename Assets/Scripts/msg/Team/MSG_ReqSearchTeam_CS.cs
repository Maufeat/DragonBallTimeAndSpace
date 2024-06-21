using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqSearchTeam_CS")]
    [Serializable]
    public class MSG_ReqSearchTeam_CS : IExtensible
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "activityid", DataFormat = DataFormat.TwosComplement)]
        public uint activityid
        {
            get
            {
                return this._activityid;
            }
            set
            {
                this._activityid = value;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(3, IsRequired = false, Name = "nearby", DataFormat = DataFormat.Default)]
        public bool nearby
        {
            get
            {
                return this._nearby;
            }
            set
            {
                this._nearby = value;
            }
        }

        private uint _teamid;

        private uint _activityid;

        private bool _nearby;

        private IExtension extensionObject;
    }
}
