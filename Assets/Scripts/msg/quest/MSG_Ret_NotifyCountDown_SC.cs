using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_Ret_NotifyCountDown_SC")]
    [Serializable]
    public class MSG_Ret_NotifyCountDown_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "bset", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool bset
        {
            get
            {
                return this._bset;
            }
            set
            {
                this._bset = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "delay", DataFormat = DataFormat.TwosComplement)]
        public uint delay
        {
            get
            {
                return this._delay;
            }
            set
            {
                this._delay = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "seconds", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint seconds
        {
            get
            {
                return this._seconds;
            }
            set
            {
                this._seconds = value;
            }
        }

        private bool _bset;

        private uint _delay;

        private uint _seconds;

        private IExtension extensionObject;
    }
}
