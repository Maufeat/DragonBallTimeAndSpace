using System;
using System.ComponentModel;
using ProtoBuf;

namespace npc
{
    [ProtoContract(Name = "MSG_Ret_RemoveHoldNpcData_SC")]
    [Serializable]
    public class MSG_Ret_RemoveHoldNpcData_SC : IExtensible
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

        private ulong _npc_tempid;

        private IExtension extensionObject;
    }
}
