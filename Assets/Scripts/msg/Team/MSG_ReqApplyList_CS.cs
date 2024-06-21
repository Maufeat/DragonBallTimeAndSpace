using System;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqApplyList_CS")]
    [Serializable]
    public class MSG_ReqApplyList_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
