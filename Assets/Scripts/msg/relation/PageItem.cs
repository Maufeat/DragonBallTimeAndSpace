using System;
using System.ComponentModel;
using ProtoBuf;

namespace relation
{
    [ProtoContract(Name = "PageItem")]
    [Serializable]
    public class PageItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "page_name", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string page_name
        {
            get
            {
                return this._page_name;
            }
            set
            {
                this._page_name = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "createtime", DataFormat = DataFormat.TwosComplement)]
        public uint createtime
        {
            get
            {
                return this._createtime;
            }
            set
            {
                this._createtime = value;
            }
        }

        private string _page_name = string.Empty;

        private uint _createtime;

        private IExtension extensionObject;
    }
}
