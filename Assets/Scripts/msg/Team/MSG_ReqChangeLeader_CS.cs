using System;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqChangeLeader_CS")]
    [Serializable]
    public class MSG_ReqChangeLeader_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "toid", DataFormat = DataFormat.Default)]
        public string toid
        {
            get
            {
                return this._toid;
            }
            set
            {
                this._toid = value;
            }
        }

        private string _toid;

        private IExtension extensionObject;
    }
}
