using System;
using System.ComponentModel;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_ReqSublistPage_CS")]
    [Serializable]
    public class MSG_ReqSublistPage_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "page", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint page
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

        private uint _page;

        private IExtension extensionObject;
    }
}
