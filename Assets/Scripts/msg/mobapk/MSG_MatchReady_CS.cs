using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_MatchReady_CS")]
    [Serializable]
    public class MSG_MatchReady_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        public ulong id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        private ulong _id;

        private IExtension extensionObject;
    }
}
