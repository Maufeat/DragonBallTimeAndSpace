using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_UserReqChangeLine_CS")]
    [Serializable]
    public class MSG_UserReqChangeLine_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "lineid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint lineid
        {
            get
            {
                return this._lineid;
            }
            set
            {
                this._lineid = value;
            }
        }

        private uint _lineid;

        private IExtension extensionObject;
    }
}
