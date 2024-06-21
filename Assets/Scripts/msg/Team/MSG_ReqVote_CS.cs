using System;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqVote_CS")]
    [Serializable]
    public class MSG_ReqVote_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "yesorno", DataFormat = DataFormat.Default)]
        public bool yesorno
        {
            get
            {
                return this._yesorno;
            }
            set
            {
                this._yesorno = value;
            }
        }

        private bool _yesorno;

        private IExtension extensionObject;
    }
}
