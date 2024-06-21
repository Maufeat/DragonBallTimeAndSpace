using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_RetFightCountDown_SC")]
    [Serializable]
    public class MSG_RetFightCountDown_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "duration", DataFormat = DataFormat.TwosComplement)]
        public uint duration
        {
            get
            {
                return this._duration;
            }
            set
            {
                this._duration = value;
            }
        }

        private uint _duration;

        private IExtension extensionObject;
    }
}
