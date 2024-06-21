using System;
using System.Collections.Generic;
using ProtoBuf;

namespace market
{
    [ProtoContract(Name = "MSG_ReqMarketItemList_CS")]
    [Serializable]
    public class MSG_ReqMarketItemList_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "marketid", DataFormat = DataFormat.TwosComplement)]
        public List<uint> marketid
        {
            get
            {
                return this._marketid;
            }
        }

        private readonly List<uint> _marketid = new List<uint>();

        private IExtension extensionObject;
    }
}
