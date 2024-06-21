using System;
using System.Collections.Generic;
using ProtoBuf;

namespace evolution
{
    [ProtoContract(Name = "MSG_RetUserEvolution_SC")]
    [Serializable]
    public class MSG_RetUserEvolution_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "evolutions", DataFormat = DataFormat.TwosComplement)]
        public List<uint> evolutions
        {
            get
            {
                return this._evolutions;
            }
        }

        private readonly List<uint> _evolutions = new List<uint>();

        private IExtension extensionObject;
    }
}
