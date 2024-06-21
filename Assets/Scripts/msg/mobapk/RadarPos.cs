using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "RadarPos")]
    [Serializable]
    public class RadarPos : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "x", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint x
        {
            get
            {
                return this._x;
            }
            set
            {
                this._x = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "y", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint y
        {
            get
            {
                return this._y;
            }
            set
            {
                this._y = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint num
        {
            get
            {
                return this._num;
            }
            set
            {
                this._num = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "uid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint uid
        {
            get
            {
                return this._uid;
            }
            set
            {
                this._uid = value;
            }
        }

        private uint _x;

        private uint _y;

        private uint _num;

        private uint _uid;

        private IExtension extensionObject;
    }
}
