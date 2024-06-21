using System;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "HeroTrain")]
    [Serializable]
    public class HeroTrain : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "pdamage", DataFormat = DataFormat.TwosComplement)]
        public uint pdamage
        {
            get
            {
                return this._pdamage;
            }
            set
            {
                this._pdamage = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "mdamage", DataFormat = DataFormat.TwosComplement)]
        public uint mdamage
        {
            get
            {
                return this._mdamage;
            }
            set
            {
                this._mdamage = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "pdefence", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint pdefence
        {
            get
            {
                return this._pdefence;
            }
            set
            {
                this._pdefence = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "mdefence", DataFormat = DataFormat.TwosComplement)]
        public uint mdefence
        {
            get
            {
                return this._mdefence;
            }
            set
            {
                this._mdefence = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "maxhp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint maxhp
        {
            get
            {
                return this._maxhp;
            }
            set
            {
                this._maxhp = value;
            }
        }

        private uint _pdamage;

        private uint _mdamage;

        private uint _pdefence;

        private uint _mdefence;

        private uint _maxhp;

        private IExtension extensionObject;
    }
}
