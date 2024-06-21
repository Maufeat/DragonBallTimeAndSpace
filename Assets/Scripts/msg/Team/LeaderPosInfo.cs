using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "LeaderPosInfo")]
    [Serializable]
    public class LeaderPosInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "valid", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool valid
        {
            get
            {
                return this._valid;
            }
            set
            {
                this._valid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "sceneid", DataFormat = DataFormat.Default)]
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

        [ProtoMember(3, IsRequired = false, Name = "pos", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public MemberPos pos
        {
            get
            {
                return this._pos;
            }
            set
            {
                this._pos = value;
            }
        }

        private bool _valid;

        private string _sceneid = string.Empty;

        private MemberPos _pos;

        private IExtension extensionObject;
    }
}
