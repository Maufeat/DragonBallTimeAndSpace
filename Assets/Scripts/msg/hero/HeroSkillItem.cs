using System;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "HeroSkillItem")]
    [Serializable]
    public class HeroSkillItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "skillbaseid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint skillbaseid
        {
            get
            {
                return this._skillbaseid;
            }
            set
            {
                this._skillbaseid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "skilllevel", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint skilllevel
        {
            get
            {
                return this._skilllevel;
            }
            set
            {
                this._skilllevel = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "skillorgid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint skillorgid
        {
            get
            {
                return this._skillorgid;
            }
            set
            {
                this._skillorgid = value;
            }
        }

        private uint _skillbaseid;

        private uint _skilllevel;

        private uint _skillorgid;

        private IExtension extensionObject;
    }
}
