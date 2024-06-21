using System;
using System.Collections.Generic;
using ProtoBuf;

namespace apprentice
{
    [ProtoContract(Name = "MassiveClientSetInfo")]
    [Serializable]
    public class MassiveClientSetInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "data", DataFormat = DataFormat.Default)]
        public List<ClientSetInfo> data
        {
            get
            {
                return this._data;
            }
        }

        private readonly List<ClientSetInfo> _data = new List<ClientSetInfo>();

        private IExtension extensionObject;
    }
}
