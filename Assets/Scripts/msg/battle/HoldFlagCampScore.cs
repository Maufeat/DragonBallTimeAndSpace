using System;
using System.ComponentModel;
using ProtoBuf;

namespace battle
{
    [ProtoContract(Name = "HoldFlagCampScore")]
    [Serializable]
    public class HoldFlagCampScore : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "campId", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint campId
        {
            get
            {
                return this._campId;
            }
            set
            {
                this._campId = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "score", DataFormat = DataFormat.TwosComplement)]
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

        private uint _campId;

        private uint _score;

        private IExtension extensionObject;
    }
}
