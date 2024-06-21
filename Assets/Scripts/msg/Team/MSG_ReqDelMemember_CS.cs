using System;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqDelMemember_CS")]
    [Serializable]
    public class MSG_ReqDelMemember_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "charid", DataFormat = DataFormat.Default)]
        public string charid
        {
            get
            {
                return this._charid;
            }
            set
            {
                this._charid = value;
            }
        }

        private string _charid;

        private IExtension extensionObject;
    }
}
