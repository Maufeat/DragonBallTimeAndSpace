using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "UserRankPkInfo")]
    [Serializable]
    public class UserRankPkInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint charid
        {
            get
            {
                return this._charid;
            }
            set
            {
                this._charid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "heroid", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(4, IsRequired = false, Name = "fight", DataFormat = DataFormat.TwosComplement)]
        public uint fight
        {
            get
            {
                return this._fight;
            }
            set
            {
                this._fight = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "rank_level", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [ProtoMember(6, IsRequired = false, Name = "rank_star", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint rank_star
        {
            get
            {
                return this._rank_star;
            }
            set
            {
                this._rank_star = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint rank
        {
            get
            {
                return this._rank;
            }
            set
            {
                this._rank = value;
            }
        }

        [ProtoMember(8, IsRequired = false, Name = "hide_score", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint hide_score
        {
            get
            {
                return this._hide_score;
            }
            set
            {
                this._hide_score = value;
            }
        }

        [ProtoMember(9, IsRequired = false, Name = "all_battles", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint all_battles
        {
            get
            {
                return this._all_battles;
            }
            set
            {
                this._all_battles = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(10, IsRequired = false, Name = "success_battles", DataFormat = DataFormat.TwosComplement)]
        public uint success_battles
        {
            get
            {
                return this._success_battles;
            }
            set
            {
                this._success_battles = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(11, IsRequired = false, Name = "battle_score", DataFormat = DataFormat.TwosComplement)]
        public uint battle_score
        {
            get
            {
                return this._battle_score;
            }
            set
            {
                this._battle_score = value;
            }
        }

        [ProtoMember(12, IsRequired = false, Name = "seanson_battles", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint seanson_battles
        {
            get
            {
                return this._seanson_battles;
            }
            set
            {
                this._seanson_battles = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(13, IsRequired = false, Name = "best_rank_level", DataFormat = DataFormat.TwosComplement)]
        public uint best_rank_level
        {
            get
            {
                return this._best_rank_level;
            }
            set
            {
                this._best_rank_level = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(14, IsRequired = false, Name = "best_rank", DataFormat = DataFormat.TwosComplement)]
        public uint best_rank
        {
            get
            {
                return this._best_rank;
            }
            set
            {
                this._best_rank = value;
            }
        }

        private uint _charid;

        private string _name = string.Empty;

        private uint _heroid;

        private uint _fight;

        private uint _rank_level;

        private uint _rank_star;

        private uint _rank;

        private uint _hide_score;

        private uint _all_battles;

        private uint _success_battles;

        private uint _battle_score;

        private uint _seanson_battles;

        private uint _best_rank_level;

        private uint _best_rank;

        private IExtension extensionObject;
    }
}
