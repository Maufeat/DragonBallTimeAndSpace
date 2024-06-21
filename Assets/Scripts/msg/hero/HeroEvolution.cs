using System;
using System.Collections.Generic;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "HeroEvolution")]
    [Serializable]
    public class HeroEvolution : IExtensible
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
