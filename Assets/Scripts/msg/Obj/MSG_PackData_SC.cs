using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_PackData_SC")]
    [Serializable]
    public class MSG_PackData_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "packtype", DataFormat = DataFormat.TwosComplement)]
        public PackType packtype
        {
            get
            {
                return this._packtype;
            }
            set
            {
                this._packtype = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "width", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(3, IsRequired = false, Name = "height", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "validsize", DataFormat = DataFormat.TwosComplement)]
        public uint validsize
        {
            get
            {
                return this._validsize;
            }
            set
            {
                this._validsize = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(5, IsRequired = false, Name = "objects", DataFormat = DataFormat.Default)]
        public MSG_RefreshObjs_SC objects
        {
            get
            {
                return this._objects;
            }
            set
            {
                this._objects = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "unlockcount", DataFormat = DataFormat.TwosComplement)]
        public uint unlockcount
        {
            get
            {
                return this._unlockcount;
            }
            set
            {
                this._unlockcount = value;
            }
        }

        private PackType _packtype;

        private uint _width;

        private uint _height;

        private uint _validsize;

        private MSG_RefreshObjs_SC _objects;

        private uint _unlockcount;

        private IExtension extensionObject;
    }
}
