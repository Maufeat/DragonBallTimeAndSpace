using System;
using System.ComponentModel;
using ProtoBuf;

namespace mail
{
    [ProtoContract(Name = "MSG_Req_DeleteMail_CS")]
    [Serializable]
    public class MSG_Req_DeleteMail_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "mailid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
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

        private ulong _mailid;

        private IExtension extensionObject;
    }
}
