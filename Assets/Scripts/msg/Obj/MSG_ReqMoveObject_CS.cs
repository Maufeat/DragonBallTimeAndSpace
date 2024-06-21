using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqMoveObject_CS")]
    [Serializable]
    public class MSG_ReqMoveObject_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string thisid
        {
            get
            {
                return this._thisid;
            }
            set
            {
                this._thisid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "grid_x", DataFormat = DataFormat.TwosComplement)]
        public uint grid_x
        {
            get
            {
                return this._grid_x;
            }
            set
            {
                this._grid_x = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "grid_y", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint grid_y
        {
            get
            {
                return this._grid_y;
            }
            set
            {
                this._grid_y = value;
            }
        }

        [ProtoMember(4, IsRequired = true, Name = "packtype", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(5, IsRequired = false, Name = "dst_packtype", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(PackType.PACK_TYPE_NONE)]
        public PackType dst_packtype
        {
            get
            {
                return this._dst_packtype;
            }
            set
            {
                this._dst_packtype = value;
            }
        }

        private string _thisid = string.Empty;

        private uint _grid_x;

        private uint _grid_y;

        private PackType _packtype;

        private PackType _dst_packtype;

        private IExtension extensionObject;
    }
}
