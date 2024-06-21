using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "VitalityItem")]
    [Serializable]
    public class VitalityItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        public uint id
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "attendtimes", DataFormat = DataFormat.TwosComplement)]
        public uint attendtimes
        {
            get
            {
                return this._attendtimes;
            }
            set
            {
                this._attendtimes = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "vitalitydegree", DataFormat = DataFormat.TwosComplement)]
        public uint vitalitydegree
        {
            get
            {
                return this._vitalitydegree;
            }
            set
            {
                this._vitalitydegree = value;
            }
        }

        private uint _id;

        private uint _attendtimes;

        private uint _vitalitydegree;

        private IExtension extensionObject;
    }
}
