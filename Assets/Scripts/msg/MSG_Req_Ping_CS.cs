using System;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Req_Ping_CS")]
    [Serializable]
    public class MSG_Req_Ping_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
