using System;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "MSG_ReqChangeCurDnaPage_CS")]
    [Serializable]
    public class MSG_ReqChangeCurDnaPage_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "page", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(DNAPage.NONE)]
        public DNAPage page
        {
            get
            {
                return this._page;
            }
            set
            {
                this._page = value;
            }
        }

        private DNAPage _page;

        private IExtension extensionObject;
    }
}
