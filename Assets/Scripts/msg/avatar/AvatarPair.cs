using System;
using System.ComponentModel;
using ProtoBuf;

namespace avatar
{
    [ProtoContract(Name = "AvatarPair")]
    [Serializable]
    public class AvatarPair : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(AvatarPart.AVATAR_HEAD)]
        [ProtoMember(1, IsRequired = false, Name = "part", DataFormat = DataFormat.TwosComplement)]
        public AvatarPart part
        {
            get
            {
                return this._part;
            }
            set
            {
                this._part = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "avatarid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint avatarid
        {
            get
            {
                return this._avatarid;
            }
            set
            {
                this._avatarid = value;
            }
        }

        private AvatarPart _part = AvatarPart.AVATAR_HEAD;

        private uint _avatarid;

        private IExtension extensionObject;
    }
}
