using System;
using System.ComponentModel;
using ProtoBuf;

namespace market
{
    [ProtoContract(Name = "MSG_ReqBuySelledItem_CS")]
    [Serializable]
    public class MSG_ReqBuySelledItem_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "index", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint index
        {
            get
            {
                return this._index;
            }
            set
            {
                this._index = value;
            }
        }

        private uint _index;

        private IExtension extensionObject;
    }
}
