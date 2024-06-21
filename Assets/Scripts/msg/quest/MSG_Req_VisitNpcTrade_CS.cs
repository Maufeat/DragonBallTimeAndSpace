using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_Req_VisitNpcTrade_CS")]
    [Serializable]
    public class MSG_Req_VisitNpcTrade_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "npc_temp_id", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong npc_temp_id
        {
            get
            {
                return this._npc_temp_id;
            }
            set
            {
                this._npc_temp_id = value;
            }
        }

        [ProtoMember(2, Name = "allcrc", DataFormat = DataFormat.Default)]
        public List<questCRC> allcrc
        {
            get
            {
                return this._allcrc;
            }
        }

        private ulong _npc_temp_id;

        private readonly List<questCRC> _allcrc = new List<questCRC>();

        private IExtension extensionObject;
    }
}
