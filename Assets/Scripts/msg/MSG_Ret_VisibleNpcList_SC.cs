using System;
using System.Collections.Generic;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_VisibleNpcList_SC")]
    [Serializable]
    public class MSG_Ret_VisibleNpcList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "npc", DataFormat = DataFormat.TwosComplement)]
        public List<uint> npc
        {
            get
            {
                return this._npc;
            }
        }

        private readonly List<uint> _npc = new List<uint>();

        private IExtension extensionObject;
    }
}
