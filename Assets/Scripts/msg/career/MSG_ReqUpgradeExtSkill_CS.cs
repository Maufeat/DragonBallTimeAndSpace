using System;
using System.ComponentModel;
using ProtoBuf;

namespace career
{
    [ProtoContract(Name = "MSG_ReqUpgradeExtSkill_CS")]
    [Serializable]
    public class MSG_ReqUpgradeExtSkill_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "extskillid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint extskillid
        {
            get
            {
                return this._extskillid;
            }
            set
            {
                this._extskillid = value;
            }
        }

        private uint _extskillid;

        private IExtension extensionObject;
    }
}
