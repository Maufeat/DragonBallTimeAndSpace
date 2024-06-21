using System;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_NEW_ROLE_CUTSCENE_SCS")]
    [Serializable]
    public class MSG_NEW_ROLE_CUTSCENE_SCS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
