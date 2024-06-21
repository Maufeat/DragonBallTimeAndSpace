using System;
using System.ComponentModel;
using ProtoBuf;

namespace npc
{
    [ProtoContract(Name = "MSG_NPC_Death_SC")]
    [Serializable]
    public class MSG_NPC_Death_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "tempid", DataFormat = DataFormat.TwosComplement)]
        public ulong tempid
        {
            get
            {
                return this._tempid;
            }
            set
            {
                this._tempid = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(2, IsRequired = false, Name = "attid", DataFormat = DataFormat.TwosComplement)]
        public ulong attid
        {
            get
            {
                return this._attid;
            }
            set
            {
                this._attid = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "lasthitskill", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint lasthitskill
        {
            get
            {
                return this._lasthitskill;
            }
            set
            {
                this._lasthitskill = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "atttype", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint atttype
        {
            get
            {
                return this._atttype;
            }
            set
            {
                this._atttype = value;
            }
        }

        private ulong _tempid;

        private ulong _attid;

        private uint _lasthitskill;

        private uint _atttype;

        private IExtension extensionObject;
    }
}
