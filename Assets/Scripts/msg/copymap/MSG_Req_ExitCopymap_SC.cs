using System;
using ProtoBuf;

namespace copymap
{
    [ProtoContract(Name = "MSG_Req_ExitCopymap_SC")]
    [Serializable]
    public class MSG_Req_ExitCopymap_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
