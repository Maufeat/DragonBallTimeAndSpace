using System;
using System.ComponentModel;
using ProtoBuf;

namespace mail
{
    [ProtoContract(Name = "MSG_Ret_RefreshMailState_SC")]
    [Serializable]
    public class MSG_Ret_RefreshMailState_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "mailid", DataFormat = DataFormat.TwosComplement)]
        public ulong mailid
        {
            get
            {
                return this._mailid;
            }
            set
            {
                this._mailid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "state", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        private ulong _mailid;

        private uint _state;

        private IExtension extensionObject;
    }
}
