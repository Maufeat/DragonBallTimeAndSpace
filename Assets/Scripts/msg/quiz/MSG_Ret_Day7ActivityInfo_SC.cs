using System;
using System.Collections.Generic;
using ProtoBuf;

namespace quiz
{
    [ProtoContract(Name = "MSG_Ret_Day7ActivityInfo_SC")]
    [Serializable]
    public class MSG_Ret_Day7ActivityInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "data", DataFormat = DataFormat.Default)]
        public List<Day7ActivityInfo> data
        {
            get
            {
                return this._data;
            }
        }

        private readonly List<Day7ActivityInfo> _data = new List<Day7ActivityInfo>();

        private IExtension extensionObject;
    }
}
