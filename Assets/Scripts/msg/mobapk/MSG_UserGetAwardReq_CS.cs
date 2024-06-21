using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_UserGetAwardReq_CS")]
    [Serializable]
    public class MSG_UserGetAwardReq_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "idx", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint idx
        {
            get
            {
                return this._idx;
            }
            set
            {
                this._idx = value;
            }
        }

        private uint _idx;

        private IExtension extensionObject;
    }
}
