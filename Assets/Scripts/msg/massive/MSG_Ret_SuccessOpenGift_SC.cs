using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_Ret_SuccessOpenGift_SC")]
    [Serializable]
    public class MSG_Ret_SuccessOpenGift_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "giftid", DataFormat = DataFormat.TwosComplement)]
        public uint giftid
        {
            get
            {
                return this._giftid;
            }
            set
            {
                this._giftid = value;
            }
        }

        [ProtoMember(2, Name = "objs", DataFormat = DataFormat.Default)]
        public List<GiftItem> objs
        {
            get
            {
                return this._objs;
            }
        }

        private uint _giftid;

        private readonly List<GiftItem> _objs = new List<GiftItem>();

        private IExtension extensionObject;
    }
}
