using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_ReqBuyVIPCard_CS")]
    [Serializable]
    public class MSG_ReqBuyVIPCard_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "cardid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint cardid
        {
            get
            {
                return this._cardid;
            }
            set
            {
                this._cardid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "count", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(1L)]
        public uint count
        {
            get
            {
                return this._count;
            }
            set
            {
                this._count = value;
            }
        }

        private uint _cardid;

        private uint _count = 1U;

        private IExtension extensionObject;
    }
}
