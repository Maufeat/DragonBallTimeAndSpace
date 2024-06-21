using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "CharacterFightData")]
    [Serializable]
    public class CharacterFightData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "curfightvalue", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint curfightvalue
        {
            get
            {
                return this._curfightvalue;
            }
            set
            {
                this._curfightvalue = value;
            }
        }

        private uint _curfightvalue;

        private IExtension extensionObject;
    }
}
