using System;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_ReqUserTradeHistory_CS")]
    [Serializable]
    public class MSG_ReqUserTradeHistory_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
