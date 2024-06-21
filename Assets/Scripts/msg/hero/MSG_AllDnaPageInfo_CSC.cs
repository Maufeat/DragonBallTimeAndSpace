using System;
using System.Collections.Generic;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "MSG_AllDnaPageInfo_CSC")]
    [Serializable]
    public class MSG_AllDnaPageInfo_CSC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "pages", DataFormat = DataFormat.Default)]
        public List<MSG_DnaPageInfo_CSC> pages
        {
            get
            {
                return this._pages;
            }
        }

        private readonly List<MSG_DnaPageInfo_CSC> _pages = new List<MSG_DnaPageInfo_CSC>();

        private IExtension extensionObject;
    }
}
