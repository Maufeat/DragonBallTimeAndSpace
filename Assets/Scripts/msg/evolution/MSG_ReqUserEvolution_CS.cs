using System;
using System.ComponentModel;
using ProtoBuf;

namespace evolution
{
    [ProtoContract(Name = "MSG_ReqUserEvolution_CS")]
    [Serializable]
    public class MSG_ReqUserEvolution_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "evolutionId", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint evolutionId
        {
            get
            {
                return this._evolutionId;
            }
            set
            {
                this._evolutionId = value;
            }
        }

        private uint _evolutionId;

        private IExtension extensionObject;
    }
}
