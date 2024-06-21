using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_PlayBellQTEResult_CS")]
    [Serializable]
    public class MSG_PlayBellQTEResult_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "qtelevel", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "result", DataFormat = DataFormat.TwosComplement)]
        public uint result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
            }
        }

        private uint _qtelevel;

        private uint _result;

        private IExtension extensionObject;
    }
}
