using System;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_NotifyUserKickout_SC")]
    [Serializable]
    public class MSG_Ret_NotifyUserKickout_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
