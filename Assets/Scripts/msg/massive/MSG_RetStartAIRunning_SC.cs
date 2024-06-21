using System;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_RetStartAIRunning_SC")]
    [Serializable]
    public class MSG_RetStartAIRunning_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
