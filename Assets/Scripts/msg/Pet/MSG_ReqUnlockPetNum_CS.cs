using System;
using System.ComponentModel;
using ProtoBuf;

namespace Pet
{
    [ProtoContract(Name = "MSG_ReqUnlockPetNum_CS")]
    [Serializable]
    public class MSG_ReqUnlockPetNum_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "unlockcount", DataFormat = DataFormat.TwosComplement)]
        public uint unlockcount
        {
            get
            {
                return this._unlockcount;
            }
            set
            {
                this._unlockcount = value;
            }
        }

        private uint _unlockcount;

        private IExtension extensionObject;
    }
}
