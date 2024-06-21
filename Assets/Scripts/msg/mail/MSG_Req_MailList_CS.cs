using System;
using ProtoBuf;

namespace mail
{
    [ProtoContract(Name = "MSG_Req_MailList_CS")]
    [Serializable]
    public class MSG_Req_MailList_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
