using System;
using System.Collections.Generic;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_RetUserTradeHistory_SC")]
    [Serializable]
    public class MSG_RetUserTradeHistory_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "one", DataFormat = DataFormat.Default)]
        public List<UserTradeItem> one
        {
            get
            {
                return this._one;
            }
        }

        private readonly List<UserTradeItem> _one = new List<UserTradeItem>();

        private IExtension extensionObject;
    }
}
