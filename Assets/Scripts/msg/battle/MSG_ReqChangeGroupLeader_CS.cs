using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_ReqChangeGroupLeader_CS")]
    [Serializable]
    public class MSG_ReqChangeGroupLeader_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "newCaptain", DataFormat = DataFormat.TwosComplement)]
        public ulong newCaptain
        {
            get
            {
                return this._newCaptain;
            }
            set
            {
                this._newCaptain = value;
            }
        }

        private ulong _newCaptain;

        private IExtension extensionObject;
    }
}
