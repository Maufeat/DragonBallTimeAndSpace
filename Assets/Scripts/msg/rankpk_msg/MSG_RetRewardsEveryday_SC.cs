using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_RetRewardsEveryday_SC")]
    [Serializable]
    public class MSG_RetRewardsEveryday_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "battle_number", DataFormat = DataFormat.TwosComplement)]
        public uint battle_number
        {
            get
            {
                return this._battle_number;
            }
            set
            {
                this._battle_number = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "success_number", DataFormat = DataFormat.TwosComplement)]
        public uint success_number
        {
            get
            {
                return this._success_number;
            }
            set
            {
                this._success_number = value;
            }
        }

        [ProtoMember(3, Name = "pkrewards", DataFormat = DataFormat.Default)]
        public List<PkRewards> pkrewards
        {
            get
            {
                return this._pkrewards;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "remainder_day", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint remainder_day
        {
            get
            {
                return this._remainder_day;
            }
            set
            {
                this._remainder_day = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "rank_level", DataFormat = DataFormat.TwosComplement)]
        public uint rank_level
        {
            get
            {
                return this._rank_level;
            }
            set
            {
                this._rank_level = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "rewards_received", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool rewards_received
        {
            get
            {
                return this._rewards_received;
            }
            set
            {
                this._rewards_received = value;
            }
        }

        private uint _battle_number;

        private uint _success_number;

        private readonly List<PkRewards> _pkrewards = new List<PkRewards>();

        private uint _remainder_day;

        private uint _rank_level;

        private bool _rewards_received;

        private IExtension extensionObject;
    }
}
