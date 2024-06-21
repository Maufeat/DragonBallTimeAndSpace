using System;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_Req_GotoBattle_CS")]
    [Serializable]
    public class MSG_Req_GotoBattle_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
