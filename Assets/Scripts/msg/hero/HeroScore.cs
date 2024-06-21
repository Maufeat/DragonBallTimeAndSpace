using System;
using System.ComponentModel;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "HeroScore")]
    [Serializable]
    public class HeroScore : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong thisid
        {
            get
            {
                return this._thisid;
            }
            set
            {
                this._thisid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "score", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint score
        {
            get
            {
                return this._score;
            }
            set
            {
                this._score = value;
            }
        }

        private ulong _thisid;

        private uint _score;

        private IExtension extensionObject;
    }
}
