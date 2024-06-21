using System;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_ReqMapQuestInfo_CS")]
    [Serializable]
    public class MSG_ReqMapQuestInfo_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
