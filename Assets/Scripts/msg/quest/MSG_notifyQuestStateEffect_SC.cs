using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_notifyQuestStateEffect_SC")]
    [Serializable]
    public class MSG_notifyQuestStateEffect_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "questid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint questid
        {
            get
            {
                return this._questid;
            }
            set
            {
                this._questid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "state", DataFormat = DataFormat.TwosComplement)]
        public uint state
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        private uint _questid;

        private uint _state;

        private IExtension extensionObject;
    }
}
