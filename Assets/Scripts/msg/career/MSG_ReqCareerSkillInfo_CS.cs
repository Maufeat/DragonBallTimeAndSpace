using System;
using ProtoBuf;

namespace career
{
    [ProtoContract(Name = "MSG_ReqCareerSkillInfo_CS")]
    [Serializable]
    public class MSG_ReqCareerSkillInfo_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
