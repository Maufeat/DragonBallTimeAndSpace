using System;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "MSG_ReqHoldFlagPutDownDB_CS")]
    [Serializable]
    public class MSG_ReqHoldFlagPutDownDB_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
