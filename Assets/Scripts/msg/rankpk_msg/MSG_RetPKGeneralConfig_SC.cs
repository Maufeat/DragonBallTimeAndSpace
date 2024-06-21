using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_RetPKGeneralConfig_SC")]
    [Serializable]
    public class MSG_RetPKGeneralConfig_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "teampknum", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint teampknum
        {
            get
            {
                return this._teampknum;
            }
            set
            {
                this._teampknum = value;
            }
        }

        private uint _teampknum;

        private IExtension extensionObject;
    }
}
