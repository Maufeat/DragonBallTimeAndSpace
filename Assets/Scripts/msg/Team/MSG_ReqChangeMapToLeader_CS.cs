using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqChangeMapToLeader_CS")]
    [Serializable]
    public class MSG_ReqChangeMapToLeader_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "sceneid", DataFormat = DataFormat.Default)]
        public string sceneid
        {
            get
            {
                return this._sceneid;
            }
            set
            {
                this._sceneid = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(2, IsRequired = false, Name = "leaderpos", DataFormat = DataFormat.Default)]
        public MemberPos leaderpos
        {
            get
            {
                return this._leaderpos;
            }
            set
            {
                this._leaderpos = value;
            }
        }

        private string _sceneid = string.Empty;

        private MemberPos _leaderpos;

        private IExtension extensionObject;
    }
}
