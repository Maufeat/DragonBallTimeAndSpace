using System;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Show_Path_Way_End_CS")]
    [Serializable]
    public class MSG_Show_Path_Way_End_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
