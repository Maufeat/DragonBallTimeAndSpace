using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "GiftItem")]
    [Serializable]
    public class GiftItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "objid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint objid
        {
            get
            {
                return this._objid;
            }
            set
            {
                this._objid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(3, IsRequired = false, Name = "bind", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint bind
        {
            get
            {
                return this._bind;
            }
            set
            {
                this._bind = value;
            }
        }

        private uint _objid;

        private uint _num;

        private uint _bind;

        private IExtension extensionObject;
    }
}
