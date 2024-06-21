using System;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_NotifyAccountReuse")]
    [Serializable]
    public class MSG_NotifyAccountReuse : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
