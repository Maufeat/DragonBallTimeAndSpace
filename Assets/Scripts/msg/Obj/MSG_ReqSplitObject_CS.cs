using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqSplitObject_CS")]
    [Serializable]
    public class MSG_ReqSplitObject_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
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

        [ProtoMember(2, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(3, IsRequired = false, Name = "packtype", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(PackType.PACK_TYPE_NONE)]
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

        private uint _num;

        private PackType _packtype;

        private IExtension extensionObject;
    }
}
