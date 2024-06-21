using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_ReqHoldFlagCaptureDB_CS")]
    [Serializable]
    public class MSG_ReqHoldFlagCaptureDB_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "npcid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong npcid
        {
            get
            {
                return this._npcid;
            }
            set
            {
                this._npcid = value;
            }
        }

        private ulong _npcid;

        private IExtension extensionObject;
    }
}
