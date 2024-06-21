using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "VitalityReward")]
    [Serializable]
    public class VitalityReward : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "degree", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint degree
        {
            get
            {
                return this._degree;
            }
            set
            {
                this._degree = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "state", DataFormat = DataFormat.TwosComplement)]
        public uint state
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        private uint _degree;

        private uint _state;

        private IExtension extensionObject;
    }
}
