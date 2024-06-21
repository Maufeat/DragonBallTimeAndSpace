using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqPutOnCard_CS")]
    [Serializable]
    public class MSG_ReqPutOnCard_CS : IExtensible
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

        [ProtoMember(2, IsRequired = false, Name = "grid_x", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "grid_y", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(PackType.PACK_TYPE_NONE)]
        [ProtoMember(4, IsRequired = false, Name = "packtype", DataFormat = DataFormat.TwosComplement)]
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

        private string _thisid = string.Empty;

        private uint _grid_x;

        private uint _grid_y;

        private PackType _packtype;

        private IExtension extensionObject;
    }
}
