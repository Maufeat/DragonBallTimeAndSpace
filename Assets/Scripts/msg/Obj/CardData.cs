using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "CardData")]
    [Serializable]
    public class CardData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "cardtype", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint cardtype
        {
            get
            {
                return this._cardtype;
            }
            set
            {
                this._cardtype = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "cardstar", DataFormat = DataFormat.TwosComplement)]
        public uint cardstar
        {
            get
            {
                return this._cardstar;
            }
            set
            {
                this._cardstar = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "cardgroup", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint cardgroup
        {
            get
            {
                return this._cardgroup;
            }
            set
            {
                this._cardgroup = value;
            }
        }

        [ProtoMember(4, Name = "base_effect", DataFormat = DataFormat.Default)]
        public List<CardEffectItem> base_effect
        {
            get
            {
                return this._base_effect;
            }
        }

        [ProtoMember(5, Name = "rand_effect", DataFormat = DataFormat.Default)]
        public List<CardEffectItem> rand_effect
        {
            get
            {
                return this._rand_effect;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "durability", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint durability
        {
            get
            {
                return this._durability;
            }
            set
            {
                this._durability = value;
            }
        }

        private uint _cardtype;

        private uint _cardstar;

        private uint _cardgroup;

        private readonly List<CardEffectItem> _base_effect = new List<CardEffectItem>();

        private readonly List<CardEffectItem> _rand_effect = new List<CardEffectItem>();

        private uint _durability;

        private IExtension extensionObject;
    }
}
