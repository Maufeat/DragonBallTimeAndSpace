using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_SetGuildNotify_CS")]
    [Serializable]
    public class MSG_Req_SetGuildNotify_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(3, IsRequired = false, Name = "notify", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string notify
        {
            get
            {
                return this._notify;
            }
            set
            {
                this._notify = value;
            }
        }

        private string _notify = string.Empty;

        private IExtension extensionObject;
    }
}
