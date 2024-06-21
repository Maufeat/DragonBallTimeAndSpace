using System;
using System.Collections.Generic;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_RefreshPowerRank_SC")]
    [Serializable]
    public class MSG_RefreshPowerRank_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "items", DataFormat = DataFormat.Default)]
        public List<PowerItem> items
        {
            get
            {
                return this._items;
            }
        }

        private readonly List<PowerItem> _items = new List<PowerItem>();

        private IExtension extensionObject;
    }
}
