using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace avatar
{
    [ProtoContract(Name = "MSG_RetUserAvatars_SC")]
    [Serializable]
    public class MSG_RetUserAvatars_SC : IExtensible
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "equipId", DataFormat = DataFormat.TwosComplement)]
        public uint equipId
        {
            get
            {
                return this._equipId;
            }
            set
            {
                this._equipId = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(3, IsRequired = false, Name = "tranSkillid", DataFormat = DataFormat.TwosComplement)]
        public ulong tranSkillid
        {
            get
            {
                return this._tranSkillid;
            }
            set
            {
                this._tranSkillid = value;
            }
        }

        private readonly List<uint> _avatars = new List<uint>();

        private uint _equipId;

        private ulong _tranSkillid;

        private IExtension extensionObject;
    }
}
