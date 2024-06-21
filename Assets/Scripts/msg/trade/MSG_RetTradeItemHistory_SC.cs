using System;
using System.Collections.Generic;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_RetTradeItemHistory_SC")]
    [Serializable]
    public class MSG_RetTradeItemHistory_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "item", DataFormat = DataFormat.Default)]
        public List<TradeHistoryItem> item
        {
            get
            {
                return this._item;
            }
        }

        private readonly List<TradeHistoryItem> _item = new List<TradeHistoryItem>();

        private IExtension extensionObject;
    }
}
