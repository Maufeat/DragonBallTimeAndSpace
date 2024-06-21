using System;
using ProtoBuf;

namespace Chat
{
    [ProtoContract(Name = "MSG_Req_OfflineChat_CS")]
    [Serializable]
    public class MSG_Req_OfflineChat_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
