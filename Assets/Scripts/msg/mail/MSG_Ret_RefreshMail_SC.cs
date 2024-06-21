using System;
using System.ComponentModel;
using ProtoBuf;

namespace mail
{
    [ProtoContract(Name = "MSG_Ret_RefreshMail_SC")]
    [Serializable]
    public class MSG_Ret_RefreshMail_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "item", DataFormat = DataFormat.Default)]
        public mail_item item
        {
            get
            {
                return this._item;
            }
            set
            {
                this._item = value;
            }
        }

        private mail_item _item;

        private IExtension extensionObject;
    }
}
