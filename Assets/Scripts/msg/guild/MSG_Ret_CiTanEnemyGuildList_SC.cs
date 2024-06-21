using System;
using System.Collections.Generic;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_CiTanEnemyGuildList_SC")]
    [Serializable]
    public class MSG_Ret_CiTanEnemyGuildList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "guildlist", DataFormat = DataFormat.Default)]
        public List<CiTanEnemyGuildItem> guildlist
        {
            get
            {
                return this._guildlist;
            }
        }

        private readonly List<CiTanEnemyGuildItem> _guildlist = new List<CiTanEnemyGuildItem>();

        private IExtension extensionObject;
    }
}
