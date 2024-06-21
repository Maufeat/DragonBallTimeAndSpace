using System;
using System.ComponentModel;
using ProtoBuf;

namespace npc
{
    [ProtoContract(Name = "MSG_Ret_HoldonSuccess")]
    [Serializable]
    public class MSG_Ret_HoldonSuccess : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "npc_tempid", DataFormat = DataFormat.TwosComplement)]
        public ulong npc_tempid
        {
            get
            {
                return this._npc_tempid;
            }
            set
            {
                this._npc_tempid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        public uint type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        private ulong _npc_tempid;

        private uint _type;

        private IExtension extensionObject;
    }
}
