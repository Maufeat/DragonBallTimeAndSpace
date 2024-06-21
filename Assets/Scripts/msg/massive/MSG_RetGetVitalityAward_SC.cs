using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_RetGetVitalityAward_SC")]
    [Serializable]
    public class MSG_RetGetVitalityAward_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "degree", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint degree
        {
            get
            {
                return this._degree;
            }
            set
            {
                this._degree = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "errcode", DataFormat = DataFormat.TwosComplement)]
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

        private uint _degree;

        private uint _errcode;

        private IExtension extensionObject;
    }
}
