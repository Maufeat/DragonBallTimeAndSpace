using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqSwapObject_CS")]
    [Serializable]
    public class MSG_ReqSwapObject_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "src_thisid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string src_thisid
        {
            get
            {
                return this._src_thisid;
            }
            set
            {
                this._src_thisid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "dst_thisid", DataFormat = DataFormat.Default)]
        public string dst_thisid
        {
            get
            {
                return this._dst_thisid;
            }
            set
            {
                this._dst_thisid = value;
            }
        }

        [DefaultValue(PackType.PACK_TYPE_NONE)]
        [ProtoMember(3, IsRequired = false, Name = "dst_packtype", DataFormat = DataFormat.TwosComplement)]
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

        private string _src_thisid = string.Empty;

        private string _dst_thisid = string.Empty;

        private PackType _dst_packtype;

        private PackType _packtype;

        private IExtension extensionObject;
    }
}
