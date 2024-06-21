using System;
using System.ComponentModel;
using ProtoBuf;

namespace avatar
{
    [ProtoContract(Name = "MSG_ReqEquipAvatar_CS")]
    [Serializable]
    public class MSG_ReqEquipAvatar_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "avatarId", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        private uint _avatarId;

        private IExtension extensionObject;
    }
}
