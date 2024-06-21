using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "npcQuestList")]
    [Serializable]
    public class npcQuestList : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "npcid", DataFormat = DataFormat.TwosComplement)]
        public uint npcid
        {
            get
            {
                return this._npcid;
            }
            set
            {
                this._npcid = value;
            }
        }

        [ProtoMember(2, Name = "quests", DataFormat = DataFormat.Default)]
        public List<QuestStateInfo> quests
        {
            get
            {
                return this._quests;
            }
        }

        [DefaultValue(0)]
        [ProtoMember(3, IsRequired = false, Name = "state", DataFormat = DataFormat.TwosComplement)]
        public int state
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        private uint _npcid;

        private readonly List<QuestStateInfo> _quests = new List<QuestStateInfo>();

        private int _state;

        private IExtension extensionObject;
    }
}
