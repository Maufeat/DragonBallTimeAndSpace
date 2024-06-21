using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "CharacterMapData")]
    [Serializable]
    public class CharacterMapData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(2, IsRequired = false, Name = "pos", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public FloatMovePos pos
        {
            get
            {
                return this._pos;
            }
            set
            {
                this._pos = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "dir", DataFormat = DataFormat.TwosComplement)]
        public uint dir
        {
            get
            {
                return this._dir;
            }
            set
            {
                this._dir = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "movespeed", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint movespeed
        {
            get
            {
                return this._movespeed;
            }
            set
            {
                this._movespeed = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "hp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint hp
        {
            get
            {
                return this._hp;
            }
            set
            {
                this._hp = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "maxhp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint maxhp
        {
            get
            {
                return this._maxhp;
            }
            set
            {
                this._maxhp = value;
            }
        }

        [ProtoMember(7, Name = "states", DataFormat = DataFormat.Default)]
        public List<StateItem> states
        {
            get
            {
                return this._states;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(8, IsRequired = false, Name = "teamid", DataFormat = DataFormat.TwosComplement)]
        public uint teamid
        {
            get
            {
                return this._teamid;
            }
            set
            {
                this._teamid = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(9, IsRequired = false, Name = "guildid", DataFormat = DataFormat.TwosComplement)]
        public ulong guildid
        {
            get
            {
                return this._guildid;
            }
            set
            {
                this._guildid = value;
            }
        }

        [ProtoMember(10, IsRequired = false, Name = "country", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint country
        {
            get
            {
                return this._country;
            }
            set
            {
                this._country = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(11, IsRequired = false, Name = "guildname", DataFormat = DataFormat.Default)]
        public string guildname
        {
            get
            {
                return this._guildname;
            }
            set
            {
                this._guildname = value;
            }
        }

        private uint _level;

        private FloatMovePos _pos;

        private uint _dir;

        private uint _movespeed;

        private uint _hp;

        private uint _maxhp;

        private readonly List<StateItem> _states = new List<StateItem>();

        private uint _teamid;

        private ulong _guildid;

        private uint _country;

        private string _guildname = string.Empty;

        private IExtension extensionObject;
    }
}
