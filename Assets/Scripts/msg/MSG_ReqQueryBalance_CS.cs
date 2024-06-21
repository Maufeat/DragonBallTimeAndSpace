using System;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_ReqQueryBalance_CS")]
    [Serializable]
    public class MSG_ReqQueryBalance_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
