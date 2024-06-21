using System;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "Hole")]
    [Serializable]
    public class Hole : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "pos", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(2, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [ProtoMember(3, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(4, IsRequired = false, Name = "page", DataFormat = DataFormat.TwosComplement)]
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

        private uint _pos;

        private uint _id;

        private uint _level;

        private DNAPage _page;

        private DNASlotType _type;

        private IExtension extensionObject;
    }
}
