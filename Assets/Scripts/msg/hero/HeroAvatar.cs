using System;
using System.Collections.Generic;
using System.ComponentModel;
using avatar;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "HeroAvatar")]
    [Serializable]
    public class HeroAvatar : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "avatars", DataFormat = DataFormat.TwosComplement)]
        public List<uint> avatars
        {
            get
            {
                return this._avatars;
            }
        }

        [ProtoMember(2, Name = "equAvatars", DataFormat = DataFormat.Default)]
        public List<AvatarPair> equAvatars
        {
            get
            {
                return this._equAvatars;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "attrAvatar", DataFormat = DataFormat.TwosComplement)]
        public uint attrAvatar
        {
            get
            {
                return this._attrAvatar;
            }
            set
            {
                this._attrAvatar = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "activeAvatar", DataFormat = DataFormat.TwosComplement)]
        public uint activeAvatar
        {
            get
            {
                return this._activeAvatar;
            }
            set
            {
                this._activeAvatar = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "tranSkill", DataFormat = DataFormat.TwosComplement)]
        public uint tranSkill
        {
            get
            {
                return this._tranSkill;
            }
            set
            {
                this._tranSkill = value;
            }
        }

        private readonly List<uint> _avatars = new List<uint>();

        private readonly List<AvatarPair> _equAvatars = new List<AvatarPair>();

        private uint _attrAvatar;

        private uint _activeAvatar;

        private uint _tranSkill;

        private IExtension extensionObject;
    }
}
