using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqChangeLeader_SC")]
    [Serializable]
    public class MSG_ReqChangeLeader_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "newid", DataFormat = DataFormat.Default)]
        public string newid
        {
            get
            {
                return this._newid;
            }
            set
            {
                this._newid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "newname", DataFormat = DataFormat.Default)]
        public string newname
        {
            get
            {
                return this._newname;
            }
            set
            {
                this._newname = value;
            }
        }

        private string _newid;

        private string _newname = string.Empty;

        private IExtension extensionObject;
    }
}
