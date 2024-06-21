using System;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "MSG_ReqCombineDna_CS")]
    [Serializable]
    public class MSG_ReqCombineDna_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(DNAPage.NONE)]
        [ProtoMember(1, IsRequired = false, Name = "page", DataFormat = DataFormat.TwosComplement)]
        public DNAPage page
        {
            get
            {
                return this._page;
            }
            set
            {
                this._page = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "pos", DataFormat = DataFormat.TwosComplement)]
        public uint pos
        {
            get
            {
                return this._pos;
            }
            set
            {
                this._pos = value;
            }
        }

        [DefaultValue(DNASlotType.ATT)]
        [ProtoMember(3, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        public DNASlotType type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
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

        private DNAPage _page;

        private uint _pos;

        private DNASlotType _type;

        private uint _num;

        private IExtension extensionObject;
    }
}
