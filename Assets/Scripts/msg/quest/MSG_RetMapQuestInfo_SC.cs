using System;
using System.Collections.Generic;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_RetMapQuestInfo_SC")]
    [Serializable]
    public class MSG_RetMapQuestInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "npclists", DataFormat = DataFormat.Default)]
        public List<npcQuestList> npclists
        {
            get
            {
                return this._npclists;
            }
        }

        private readonly List<npcQuestList> _npclists = new List<npcQuestList>();

        private IExtension extensionObject;
    }
}
