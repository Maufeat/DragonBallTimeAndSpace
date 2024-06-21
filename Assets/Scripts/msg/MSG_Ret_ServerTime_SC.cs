using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_ServerTime_SC")]
    [Serializable]
    public class MSG_Ret_ServerTime_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "servertime", DataFormat = DataFormat.TwosComplement)]
        public ulong servertime
        {
            get
            {
                return this._servertime;
            }
            set
            {
                this._servertime = value;
            }
        }

        private ulong _servertime;

        private IExtension extensionObject;
    }
}
