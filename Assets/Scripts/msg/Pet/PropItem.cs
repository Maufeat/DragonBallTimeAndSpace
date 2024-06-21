using System;
using System.ComponentModel;
using ProtoBuf;

namespace Pet
{
    [ProtoContract(Name = "PropItem")]
    [Serializable]
    public class PropItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "pdamage", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [ProtoMember(2, IsRequired = false, Name = "pdefence", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(3, IsRequired = false, Name = "maxhp", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "toughness", DataFormat = DataFormat.TwosComplement)]
        public uint toughness
        {
            get
            {
                return this._toughness;
            }
            set
            {
                this._toughness = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "bang", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint bang
        {
            get
            {
                return this._bang;
            }
            set
            {
                this._bang = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "accurate", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint accurate
        {
            get
            {
                return this._accurate;
            }
            set
            {
                this._accurate = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "hold", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint hold
        {
            get
            {
                return this._hold;
            }
            set
            {
                this._hold = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(8, IsRequired = false, Name = "hit", DataFormat = DataFormat.TwosComplement)]
        public uint hit
        {
            get
            {
                return this._hit;
            }
            set
            {
                this._hit = value;
            }
        }

        [ProtoMember(9, IsRequired = false, Name = "miss", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint miss
        {
            get
            {
                return this._miss;
            }
            set
            {
                this._miss = value;
            }
        }

        private uint _pdamage;

        private uint _pdefence;

        private uint _maxhp;

        private uint _toughness;

        private uint _bang;

        private uint _accurate;

        private uint _hold;

        private uint _hit;

        private uint _miss;

        private IExtension extensionObject;
    }
}
