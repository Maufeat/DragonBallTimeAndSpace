using System;
using System.Collections.Generic;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "MSG_Ret_AddInteractive_SC")]
    [Serializable]
    public class MSG_Ret_AddInteractive_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "data", DataFormat = DataFormat.Default)]
        public List<relation_item> data
        {
            get
            {
                return this._data;
            }
        }

        private readonly List<relation_item> _data = new List<relation_item>();

        private IExtension extensionObject;
    }
}
