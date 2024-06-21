using System;
using System.ComponentModel;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Req_OffSkill_CS")]
    [Serializable]
    public class MSG_Req_OffSkill_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "skillid", DataFormat = DataFormat.TwosComplement)]
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

        private uint _skillid;

        private IExtension extensionObject;
    }
}
