using System;
using System.ComponentModel;
using Obj;
using ProtoBuf;

namespace hero
{
    [ProtoContract(Name = "Hero")]
    [Serializable]
    public class Hero : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string thisid
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "baseid", DataFormat = DataFormat.TwosComplement)]
        public uint baseid
        {
            get
            {
                return this._baseid;
            }
            set
            {
                this._baseid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "star", DataFormat = DataFormat.TwosComplement)]
        public uint star
        {
            get
            {
                return this._star;
            }
            set
            {
                this._star = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "bind", DataFormat = DataFormat.TwosComplement)]
        public uint bind
        {
            get
            {
                return this._bind;
            }
            set
            {
                this._bind = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "lock", DataFormat = DataFormat.TwosComplement)]
        public uint @lock
        {
            get
            {
                return this._lock;
            }
            set
            {
                this._lock = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "runepageid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint runepageid
        {
            get
            {
                return this._runepageid;
            }
            set
            {
                this._runepageid = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "score", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(8, IsRequired = false, Name = "train", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public HeroTrain train
        {
            get
            {
                return this._train;
            }
            set
            {
                this._train = value;
            }
        }

        [ProtoMember(9, IsRequired = false, Name = "skill", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public HeroSkill skill
        {
            get
            {
                return this._skill;
            }
            set
            {
                this._skill = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(10, IsRequired = false, Name = "avatar", DataFormat = DataFormat.Default)]
        public HeroAvatar avatar
        {
            get
            {
                return this._avatar;
            }
            set
            {
                this._avatar = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(11, IsRequired = false, Name = "evolution", DataFormat = DataFormat.Default)]
        public HeroEvolution evolution
        {
            get
            {
                return this._evolution;
            }
            set
            {
                this._evolution = value;
            }
        }

        [ProtoMember(12, IsRequired = false, Name = "show", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public HeroShow show
        {
            get
            {
                return this._show;
            }
            set
            {
                this._show = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(13, IsRequired = false, Name = "cardpack", DataFormat = DataFormat.Default)]
        public CardPackInfo cardpack
        {
            get
            {
                return this._cardpack;
            }
            set
            {
                this._cardpack = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(14, IsRequired = false, Name = "tradetime", DataFormat = DataFormat.TwosComplement)]
        public uint tradetime
        {
            get
            {
                return this._tradetime;
            }
            set
            {
                this._tradetime = value;
            }
        }

        [ProtoMember(15, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint level
        {
            get
            {
                return this._level;
            }
            set
            {
                this._level = value;
            }
        }

        [ProtoMember(16, IsRequired = false, Name = "exp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong exp
        {
            get
            {
                return this._exp;
            }
            set
            {
                this._exp = value;
            }
        }

        [DefaultValue(false)]
        [ProtoMember(17, IsRequired = false, Name = "self_created", DataFormat = DataFormat.Default)]
        public bool self_created
        {
            get
            {
                return this._self_created;
            }
            set
            {
                this._self_created = value;
            }
        }

        private string _thisid = string.Empty;

        private uint _baseid;

        private uint _star;

        private uint _bind;

        private uint _lock;

        private uint _runepageid;

        private uint _score;

        private HeroTrain _train;

        private HeroSkill _skill;

        private HeroAvatar _avatar;

        private HeroEvolution _evolution;

        private HeroShow _show;

        private CardPackInfo _cardpack;

        private uint _tradetime;

        private uint _level;

        private ulong _exp;

        private bool _self_created;

        private IExtension extensionObject;
    }
}
