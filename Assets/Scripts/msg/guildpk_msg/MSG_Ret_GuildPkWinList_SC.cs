using System;
using System.Collections.Generic;
using ProtoBuf;

namespace guildpk_msg
{
    [ProtoContract(Name = "MSG_Ret_GuildPkWinList_SC")]
    [Serializable]
    public class MSG_Ret_GuildPkWinList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "winers", DataFormat = DataFormat.Default)]
        public List<GuildPkWinInfo> winers
        {
            get
            {
                return this._winers;
            }
        }

        private readonly List<GuildPkWinInfo> _winers = new List<GuildPkWinInfo>();

        private IExtension extensionObject;
    }
}
