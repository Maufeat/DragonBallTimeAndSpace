using System;
using System.ComponentModel;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_RetDrinkBloodSkill_SC")]
    [Serializable]
    public class MSG_RetDrinkBloodSkill_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(1L)]
        [ProtoMember(1, IsRequired = false, Name = "errcode", DataFormat = DataFormat.TwosComplement)]
        public uint errcode
        {
            get
            {
                return this._errcode;
            }
            set
            {
                this._errcode = value;
            }
        }

        private uint _errcode = 1U;

        private IExtension extensionObject;
    }
}
