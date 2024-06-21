using System;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqLanchVoteOut_CS")]
    [Serializable]
    public class MSG_ReqLanchVoteOut_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "outid", DataFormat = DataFormat.Default)]
        public string outid
        {
            get
            {
                return this._outid;
            }
            set
            {
                this._outid = value;
            }
        }

        private string _outid;

        private IExtension extensionObject;
    }
}
