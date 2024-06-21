using System;
using ProtoBuf;

namespace quiz
{
    [ProtoContract(Name = "MSG_Req_RecvDay7ActivityPrize_CS")]
    [Serializable]
    public class MSG_Req_RecvDay7ActivityPrize_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
