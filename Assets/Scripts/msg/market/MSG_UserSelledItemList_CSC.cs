using System;
using System.Collections.Generic;
using ProtoBuf;

namespace market
{
    [ProtoContract(Name = "MSG_UserSelledItemList_CSC")]
    [Serializable]
    public class MSG_UserSelledItemList_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "objs", DataFormat = DataFormat.Default)]
        public List<SelledItem> objs
        {
            get
            {
                return this._objs;
            }
        }

        private readonly List<SelledItem> _objs = new List<SelledItem>();

        private IExtension extensionObject;
    }
}
