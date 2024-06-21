using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_FireGuildMember_CS")]
    [Serializable]
    public class MSG_Req_FireGuildMember_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "leavememberid", DataFormat = DataFormat.Default)]
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

        private string _leavememberid = string.Empty;

        private IExtension extensionObject;
    }
}
