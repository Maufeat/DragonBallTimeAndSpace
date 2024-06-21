using System;
using System.Collections.Generic;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Req_Summon_Npc_CS")]
    [Serializable]
    public class MSG_Req_Summon_Npc_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "npcs", DataFormat = DataFormat.Default)]
        public List<TempNpcInfo> npcs
        {
            get
            {
                return this._npcs;
            }
        }

        private readonly List<TempNpcInfo> _npcs = new List<TempNpcInfo>();

        private IExtension extensionObject;
    }
}
