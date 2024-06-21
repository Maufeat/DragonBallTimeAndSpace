using System;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_ReqHoldFlagReport_CS")]
    [Serializable]
    public class MSG_ReqHoldFlagReport_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
