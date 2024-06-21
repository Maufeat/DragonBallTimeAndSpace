using System;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_DismissGroup_SC")]
    [Serializable]
    public class MSG_DismissGroup_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
