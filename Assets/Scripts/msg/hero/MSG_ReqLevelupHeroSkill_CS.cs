using System;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "MSG_ReqLevelupHeroSkill_CS")]
    [Serializable]
    public class MSG_ReqLevelupHeroSkill_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "herothisid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string herothisid
        {
            get
            {
                return this._herothisid;
            }
            set
            {
                this._herothisid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "skillbaseid", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "skilllevel", DataFormat = DataFormat.TwosComplement)]
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

        private string _herothisid = string.Empty;

        private uint _skillbaseid;

        private uint _skilllevel;

        private IExtension extensionObject;
    }
}
