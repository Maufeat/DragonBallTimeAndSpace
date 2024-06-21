using System;
using System.Collections.Generic;
using ProtoBuf;

namespace mail
{
    [ProtoContract(Name = "MSG_Ret_MailList_SC")]
    [Serializable]
    public class MSG_Ret_MailList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "items", DataFormat = DataFormat.Default)]
        public List<mail_item> items
        {
            get
            {
                return this._items;
            }
        }

        private readonly List<mail_item> _items = new List<mail_item>();

        private IExtension extensionObject;
    }
}
