using System;
using System.Collections.Generic;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_MapScreenFuncNpc_SC")]
    [Serializable]
    public class MSG_Ret_MapScreenFuncNpc_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "data", DataFormat = DataFormat.Default)]
        public List<FuncNpcData> data
        {
            get
            {
                return this._data;
            }
        }

        private readonly List<FuncNpcData> _data = new List<FuncNpcData>();

        private IExtension extensionObject;
    }
}
