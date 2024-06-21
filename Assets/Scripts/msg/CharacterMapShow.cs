using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "CharacterMapShow")]
    [Serializable]
    public class CharacterMapShow : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "face", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint face
        {
            get
            {
                return this._face;
            }
            set
            {
                this._face = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "weapon", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint weapon
        {
            get
            {
                return this._weapon;
            }
            set
            {
                this._weapon = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "coat", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint coat
        {
            get
            {
                return this._coat;
            }
            set
            {
                this._coat = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "occupation", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint occupation
        {
            get
            {
                return this._occupation;
            }
            set
            {
                this._occupation = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "heroid", DataFormat = DataFormat.TwosComplement)]
        public uint heroid
        {
            get
            {
                return this._heroid;
            }
            set
            {
                this._heroid = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "facestyle", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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
        [ProtoMember(7, IsRequired = false, Name = "hairstyle", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(8, IsRequired = false, Name = "haircolor", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(9, IsRequired = false, Name = "antenna", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(10, IsRequired = false, Name = "bodystyle", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(11, IsRequired = false, Name = "avatarId", DataFormat = DataFormat.TwosComplement)]
        public uint avatarId
        {
            get
            {
                return this._avatarId;
            }
            set
            {
                this._avatarId = value;
            }
        }

        private uint _face;

        private uint _weapon;

        private uint _coat;

        private uint _occupation;

        private uint _heroid;

        private uint _facestyle;

        private uint _hairstyle;

        private uint _haircolor;

        private uint _antenna;

        private uint _bodystyle;

        private uint _avatarId;

        private IExtension extensionObject;
    }
}
