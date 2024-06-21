using System;
using System.ComponentModel;
using ProtoBuf;

namespace map
{
    [ProtoContract(Name = "MapDisAreaFileHeader")]
    [Serializable]
    public class MapDisAreaFileHeader : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "width", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "height", DataFormat = DataFormat.TwosComplement)]
        public uint height
        {
            get
            {
                return this._height;
            }
            set
            {
                this._height = value;
            }
        }

        private uint _width;

        private uint _height;

        private IExtension extensionObject;
    }
}
