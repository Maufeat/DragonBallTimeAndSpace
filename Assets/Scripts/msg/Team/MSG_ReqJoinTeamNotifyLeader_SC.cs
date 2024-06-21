using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqJoinTeamNotifyLeader_SC")]
    [Serializable]
    public class MSG_ReqJoinTeamNotifyLeader_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "requesterid", DataFormat = DataFormat.Default)]
        public string requesterid
        {
            get
            {
                return this._requesterid;
            }
            set
            {
                this._requesterid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "requestername", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string requestername
        {
            get
            {
                return this._requestername;
            }
            set
            {
                this._requestername = value;
            }
        }

        private string _requesterid;

        private string _requestername = string.Empty;

        private IExtension extensionObject;
    }
}
