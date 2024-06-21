using System;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "MSG_Req_OfflineInteractive_CS")]
    [Serializable]
    public class MSG_Req_OfflineInteractive_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
