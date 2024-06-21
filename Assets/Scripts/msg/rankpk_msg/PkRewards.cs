using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "PkRewards")]
    [Serializable]
    public class PkRewards : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "heroid", DataFormat = DataFormat.TwosComplement)]
        public uint heroid
        {
            get
            {
                return this._heroid;
            }
            set
            {
                this._heroid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "time", DataFormat = DataFormat.TwosComplement)]
        public uint time
        {
            get
            {
                return this._time;
            }
            set
            {
                this._time = value;
            }
        }

        [ProtoMember(3, Name = "rewards", DataFormat = DataFormat.Default)]
        public List<RewardsNumber> rewards
        {
            get
            {
                return this._rewards;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(4, IsRequired = false, Name = "pkresult", DataFormat = DataFormat.Default)]
        public bool pkresult
        {
            get
            {
                return this._pkresult;
            }
            set
            {
                this._pkresult = value;
            }
        }

        private uint _heroid;

        private uint _time;

        private readonly List<RewardsNumber> _rewards = new List<RewardsNumber>();

        private bool _pkresult;

        private IExtension extensionObject;
    }
}
