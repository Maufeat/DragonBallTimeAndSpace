using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_PlayBellQTE_SC")]
    [Serializable]
    public class MSG_PlayBellQTE_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "qtelevel", DataFormat = DataFormat.TwosComplement)]
        public uint qtelevel
        {
            get
            {
                return this._qtelevel;
            }
            set
            {
                this._qtelevel = value;
            }
        }

        private uint _qtelevel;

        private IExtension extensionObject;
    }
}
