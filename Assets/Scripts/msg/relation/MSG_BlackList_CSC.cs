using System;
using System.Collections.Generic;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "MSG_BlackList_CSC")]
    [Serializable]
    public class MSG_BlackList_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "blackList", DataFormat = DataFormat.Default)]
        public List<BlackItem> blackList
        {
            get
            {
                return this._blackList;
            }
        }

        private readonly List<BlackItem> _blackList = new List<BlackItem>();

        private IExtension extensionObject;
    }
}
