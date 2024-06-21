using System;
using System.ComponentModel;
using ProtoBuf;

namespace career
{
    [ProtoContract(Name = "MSG_RetCareerSkillInfo_SC")]
    [Serializable]
    public class MSG_RetCareerSkillInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "skillinfo", DataFormat = DataFormat.Default)]
        public CareerSkillInfo skillinfo
        {
            get
            {
                return this._skillinfo;
            }
            set
            {
                this._skillinfo = value;
            }
        }

        private CareerSkillInfo _skillinfo;

        private IExtension extensionObject;
    }
}
