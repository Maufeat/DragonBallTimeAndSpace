using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_FireGuildMember_SC")]
    [Serializable]
    public class MSG_Ret_FireGuildMember_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
        public uint retcode
        {
            get
            {
                return this._retcode;
            }
            set
            {
                this._retcode = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "leavememberid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string leavememberid
        {
            get
            {
                return this._leavememberid;
            }
            set
            {
                this._leavememberid = value;
            }
        }

        private uint _retcode;

        private string _leavememberid = string.Empty;

        private IExtension extensionObject;
    }
}
