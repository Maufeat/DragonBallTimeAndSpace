using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace market
{
    [ProtoContract(Name = "MSG_RetMarketItemList_SC")]
    [Serializable]
    public class MSG_RetMarketItemList_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "marketdetail", DataFormat = DataFormat.Default)]
        public List<OneMarket> marketdetail
        {
            get
            {
                return this._marketdetail;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "guildskilllv", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint guildskilllv
        {
            get
            {
                return this._guildskilllv;
            }
            set
            {
                this._guildskilllv = value;
            }
        }

        private readonly List<OneMarket> _marketdetail = new List<OneMarket>();

        private uint _guildskilllv;

        private IExtension extensionObject;
    }
}
