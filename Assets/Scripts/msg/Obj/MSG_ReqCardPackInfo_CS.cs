using ProtoBuf;
using System;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqCardPackInfo_CS")]
    [Serializable]
    public class MSG_ReqCardPackInfo_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
