using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqNearByUnteamedPlayer_SC")]
    [Serializable]
    public class MSG_ReqNearByUnteamedPlayer_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "mem", DataFormat = DataFormat.Default)]
        public List<Memember> mem
        {
            get
            {
                return this._mem;
            }
        }

        private readonly List<Memember> _mem = new List<Memember>();

        private IExtension extensionObject;
    }
}
