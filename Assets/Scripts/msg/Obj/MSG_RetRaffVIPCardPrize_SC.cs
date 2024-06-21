using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_RetRaffVIPCardPrize_SC")]
    [Serializable]
    public class MSG_RetRaffVIPCardPrize_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "vipcardinfo", DataFormat = DataFormat.Default)]
        public VIPCardInfo vipcardinfo
        {
            get
            {
                return this._vipcardinfo;
            }
            set
            {
                this._vipcardinfo = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        public uint id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "quantity", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint quantity
        {
            get
            {
                return this._quantity;
            }
            set
            {
                this._quantity = value;
            }
        }

        private VIPCardInfo _vipcardinfo;

        private uint _id;

        private uint _quantity;

        private IExtension extensionObject;
    }
}
