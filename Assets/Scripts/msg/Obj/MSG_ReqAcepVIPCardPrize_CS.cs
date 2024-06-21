using System;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqAcepVIPCardPrize_CS")]
    [Serializable]
    public class MSG_ReqAcepVIPCardPrize_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
