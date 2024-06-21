using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_Ret_Wegame_Fcm_Info")]
    [Serializable]
    public class MSG_Ret_Wegame_Fcm_Info : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "ratio", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint ratio
        {
            get
            {
                return this._ratio;
            }
            set
            {
                this._ratio = value;
            }
        }

        private uint _ratio;

        private IExtension extensionObject;
    }
}
