using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "npcValidQuest")]
    [Serializable]
    public class npcValidQuest : IExtensible
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

        [ProtoMember(2, IsRequired = false, Name = "questid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint questid
        {
            get
            {
                return this._questid;
            }
            set
            {
                this._questid = value;
            }
        }

        private uint _npcid;

        private uint _questid;

        private IExtension extensionObject;
    }
}
