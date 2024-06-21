using System;
using System.ComponentModel;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_ReqDrinkBloodSkill_CS")]
    [Serializable]
    public class MSG_ReqDrinkBloodSkill_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "npctempid", DataFormat = DataFormat.TwosComplement)]
        public ulong npctempid
        {
            get
            {
                return this._npctempid;
            }
            set
            {
                this._npctempid = value;
            }
        }

        private ulong _npctempid;

        private IExtension extensionObject;
    }
}
