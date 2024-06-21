using System;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "HeroShow")]
    [Serializable]
    public class HeroShow : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "facestyle", DataFormat = DataFormat.TwosComplement)]
        public uint facestyle
        {
            get
            {
                return this._facestyle;
            }
            set
            {
                this._facestyle = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "hairstyle", DataFormat = DataFormat.TwosComplement)]
        public uint hairstyle
        {
            get
            {
                return this._hairstyle;
            }
            set
            {
                this._hairstyle = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "bodystyle", DataFormat = DataFormat.TwosComplement)]
        public uint bodystyle
        {
            get
            {
                return this._bodystyle;
            }
            set
            {
                this._bodystyle = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "haircolor", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint haircolor
        {
            get
            {
                return this._haircolor;
            }
            set
            {
                this._haircolor = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "antenna", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint antenna
        {
            get
            {
                return this._antenna;
            }
            set
            {
                this._antenna = value;
            }
        }

        private uint _facestyle;

        private uint _hairstyle;

        private uint _bodystyle;

        private uint _haircolor;

        private uint _antenna;

        private IExtension extensionObject;
    }
}
