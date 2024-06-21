using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqTransMoney_CS")]
    [Serializable]
    public class MSG_ReqTransMoney_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(PackType.PACK_TYPE_NONE)]
        [ProtoMember(1, IsRequired = false, Name = "packtype", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(2, IsRequired = false, Name = "dst_packtype", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(3, IsRequired = false, Name = "resourceID", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint resourceID
        {
            get
            {
                return this._resourceID;
            }
            set
            {
                this._resourceID = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "quantity", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint quantity
        {
            get
            {
                return this._quantity;
            }
            set
            {
                this._quantity = value;
            }
        }

        private PackType _packtype;

        private PackType _dst_packtype;

        private uint _resourceID;

        private uint _quantity;

        private IExtension extensionObject;
    }
}
