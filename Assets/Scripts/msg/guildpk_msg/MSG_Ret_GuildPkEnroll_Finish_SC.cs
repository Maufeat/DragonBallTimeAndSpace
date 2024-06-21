using System;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "MSG_Ret_GuildPkEnroll_Finish_SC")]
    [Serializable]
    public class MSG_Ret_GuildPkEnroll_Finish_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
