using System;
using System.Collections.Generic;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_RetSublistPage_SC")]
    [Serializable]
    public class MSG_RetSublistPage_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "item", DataFormat = DataFormat.Default)]
        public List<TradeItem> item
        {
            get
            {
                return this._item;
            }
        }

        private readonly List<TradeItem> _item = new List<TradeItem>();

        private IExtension extensionObject;
    }
}
