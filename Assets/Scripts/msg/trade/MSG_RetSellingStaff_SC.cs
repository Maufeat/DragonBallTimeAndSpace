using System;
using System.Collections.Generic;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_RetSellingStaff_SC")]
    [Serializable]
    public class MSG_RetSellingStaff_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "itemlist", DataFormat = DataFormat.Default)]
        public List<TradeItem> itemlist
        {
            get
            {
                return this._itemlist;
            }
        }

        private readonly List<TradeItem> _itemlist = new List<TradeItem>();

        private IExtension extensionObject;
    }
}
