using System;
using System.ComponentModel;
using ProtoBuf;

namespace npc
{
    [ProtoContract(Name = "MSG_Ret_Holdon_Interrupt_SC")]
    [Serializable]
    public class MSG_Ret_Holdon_Interrupt_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "baseid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint baseid
        {
            get
            {
                return this._baseid;
            }
            set
            {
                this._baseid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "npctype", DataFormat = DataFormat.TwosComplement)]
        public uint npctype
        {
            get
            {
                return this._npctype;
            }
            set
            {
                this._npctype = value;
            }
        }

        private uint _baseid;

        private uint _npctype;

        private IExtension extensionObject;
    }
}
