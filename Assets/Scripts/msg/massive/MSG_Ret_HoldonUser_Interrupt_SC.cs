using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_Ret_HoldonUser_Interrupt_SC")]
    [Serializable]
    public class MSG_Ret_HoldonUser_Interrupt_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong thisid
        {
            get
            {
                return this._thisid;
            }
            set
            {
                this._thisid = value;
            }
        }

        private ulong _thisid;

        private IExtension extensionObject;
    }
}
