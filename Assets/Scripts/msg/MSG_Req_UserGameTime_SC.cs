using System;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Req_UserGameTime_SC")]
    [Serializable]
    public class MSG_Req_UserGameTime_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
