using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_ServerLoginFailed_SC")]
    [Serializable]
    public class MSG_Ret_ServerLoginFailed_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "returncode", DataFormat = DataFormat.TwosComplement)]
        public uint returncode
        {
            get
            {
                return this._returncode;
            }
            set
            {
                this._returncode = value;
            }
        }

        private uint _returncode;

        private IExtension extensionObject;
    }
}
