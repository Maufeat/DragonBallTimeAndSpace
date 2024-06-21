using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_ReqGetVitalityAward_CS")]
    [Serializable]
    public class MSG_ReqGetVitalityAward_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "degree", DataFormat = DataFormat.TwosComplement)]
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

        private uint _degree;

        private IExtension extensionObject;
    }
}
