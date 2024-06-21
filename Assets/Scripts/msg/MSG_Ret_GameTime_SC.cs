using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_GameTime_SC")]
    [Serializable]
    public class MSG_Ret_GameTime_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "gametime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong gametime
        {
            get
            {
                return this._gametime;
            }
            set
            {
                this._gametime = value;
            }
        }

        private ulong _gametime;

        private IExtension extensionObject;
    }
}
