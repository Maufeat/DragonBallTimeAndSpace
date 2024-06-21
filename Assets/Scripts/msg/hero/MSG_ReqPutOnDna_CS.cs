using System;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "MSG_ReqPutOnDna_CS")]
    [Serializable]
    public class MSG_ReqPutOnDna_CS : IExtensible
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

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        public uint id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint level
        {
            get
            {
                return this._level;
            }
            set
            {
                this._level = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
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

        private uint _id;

        private uint _level;

        private DNASlotType _type;

        private IExtension extensionObject;
    }
}
