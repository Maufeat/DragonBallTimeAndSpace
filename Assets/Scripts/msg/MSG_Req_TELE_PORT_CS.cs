using System;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Req_TELE_PORT_CS")]
    [Serializable]
    public class MSG_Req_TELE_PORT_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "teleportid", DataFormat = DataFormat.TwosComplement)]
        public ulong teleportid
        {
            get
            {
                return this._teleportid;
            }
            set
            {
                this._teleportid = value;
            }
        }

        private ulong _teleportid;

        private IExtension extensionObject;
    }
}
