using System;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_ReqVitalityDegree_CS")]
    [Serializable]
    public class MSG_ReqVitalityDegree_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
