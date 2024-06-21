using System;
using System.Collections.Generic;
using ProtoBuf;

namespace npc
{
    [ProtoContract(Name = "MSG_Ret_BatchAddHoldNpcData_SC")]
    [Serializable]
    public class MSG_Ret_BatchAddHoldNpcData_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "datas", DataFormat = DataFormat.Default)]
        public List<HoldNpcData> datas
        {
            get
            {
                return this._datas;
            }
        }

        private readonly List<HoldNpcData> _datas = new List<HoldNpcData>();

        private IExtension extensionObject;
    }
}
