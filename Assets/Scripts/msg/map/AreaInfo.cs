using System;
using System.ComponentModel;
using ProtoBuf;

namespace map
{
    [ProtoContract(Name = "AreaInfo")]
    [Serializable]
    public class AreaInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "posX", DataFormat = DataFormat.TwosComplement)]
        public uint posX
        {
            get
            {
                return this._posX;
            }
            set
            {
                this._posX = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "posY", DataFormat = DataFormat.TwosComplement)]
        public uint posY
        {
            get
            {
                return this._posY;
            }
            set
            {
                this._posY = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "width", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(4, IsRequired = false, Name = "height", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0)]
        [ProtoMember(5, IsRequired = false, Name = "modelID", DataFormat = DataFormat.TwosComplement)]
        public int modelID
        {
            get
            {
                return this._modelID;
            }
            set
            {
                this._modelID = value;
            }
        }

        private uint _posX;

        private uint _posY;

        private uint _width;

        private uint _height;

        private int _modelID;

        private IExtension extensionObject;
    }
}
