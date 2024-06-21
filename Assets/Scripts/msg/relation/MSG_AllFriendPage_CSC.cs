using System;
using System.Collections.Generic;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "MSG_AllFriendPage_CSC")]
    [Serializable]
    public class MSG_AllFriendPage_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "pages", DataFormat = DataFormat.Default)]
        public List<PageItem> pages
        {
            get
            {
                return this._pages;
            }
        }

        private readonly List<PageItem> _pages = new List<PageItem>();

        private IExtension extensionObject;
    }
}
