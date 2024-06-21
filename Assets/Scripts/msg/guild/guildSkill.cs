using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "guildSkill")]
    [Serializable]
    public class guildSkill : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "skillid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint skillid
        {
            get
            {
                return this._skillid;
            }
            set
            {
                this._skillid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "skilllv", DataFormat = DataFormat.TwosComplement)]
        public uint skilllv
        {
            get
            {
                return this._skilllv;
            }
            set
            {
                this._skilllv = value;
            }
        }

        private uint _skillid;

        private uint _skilllv;

        private IExtension extensionObject;
    }
}
