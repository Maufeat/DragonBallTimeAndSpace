using System;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "MSG_ReqPutOffDna_CS")]
    [Serializable]
    public class MSG_ReqPutOffDna_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "page", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(DNAPage.NONE)]
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

        [ProtoMember(2, IsRequired = false, Name = "pos", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [ProtoMember(3, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(DNASlotType.ATT)]
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

        private DNAPage _page;

        private uint _pos;

        private DNASlotType _type;

        private IExtension extensionObject;
    }
}
