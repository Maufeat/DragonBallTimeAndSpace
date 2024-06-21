using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_MergeObjs_CS")]
    [Serializable]
    public class MSG_MergeObjs_CS : IExtensible
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

        [ProtoMember(2, Name = "srcthisids", DataFormat = DataFormat.Default)]
        public List<string> srcthisids
        {
            get
            {
                return this._srcthisids;
            }
        }

        [ProtoMember(3, Name = "dstthisids", DataFormat = DataFormat.Default)]
        public List<string> dstthisids
        {
            get
            {
                return this._dstthisids;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "dst_packtype", DataFormat = DataFormat.TwosComplement)]
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

        private PackType _packtype;

        private readonly List<string> _srcthisids = new List<string>();

        private readonly List<string> _dstthisids = new List<string>();

        private PackType _dst_packtype;

        private IExtension extensionObject;
    }
}
