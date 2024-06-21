using System;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "MSG_Req_GuildPkRank_CS")]
    [Serializable]
    public class MSG_Req_GuildPkRank_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
