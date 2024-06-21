using System;
using System.ComponentModel;
using ProtoBuf;

namespace mail
{
    [ProtoContract(Name = "MSG_Req_GetAttachment_CS")]
    [Serializable]
    public class MSG_Req_GetAttachment_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "mailid", DataFormat = DataFormat.Default)]
        public string mailid
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

        private string _mailid = string.Empty;

        private IExtension extensionObject;
    }
}
