using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MapNpcData")]
    [Serializable]
    public class MapNpcData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "baseid", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0f)]
        [ProtoMember(2, IsRequired = false, Name = "tempid", DataFormat = DataFormat.TwosComplement)]
        public ulong tempid
        {
            get
            {
                return this._tempid;
            }
            set
            {
                this._tempid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(3, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
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

        [ProtoMember(4, IsRequired = false, Name = "hp", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(5, IsRequired = false, Name = "maxhp", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(6, IsRequired = false, Name = "pos", DataFormat = DataFormat.Default)]
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
        [ProtoMember(8, IsRequired = false, Name = "dir", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(9, IsRequired = false, Name = "movespeed", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(10, IsRequired = false, Name = "attspeed", DataFormat = DataFormat.TwosComplement)]
        public uint attspeed
        {
            get
            {
                return this._attspeed;
            }
            set
            {
                this._attspeed = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(11, IsRequired = false, Name = "master", DataFormat = DataFormat.Default)]
        public MasterData master
        {
            get
            {
                return this._master;
            }
            set
            {
                this._master = value;
            }
        }

        [ProtoMember(12, IsRequired = false, Name = "visit", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint visit
        {
            get
            {
                return this._visit;
            }
            set
            {
                this._visit = value;
            }
        }

        [ProtoMember(13, Name = "states", DataFormat = DataFormat.Default)]
        public List<StateItem> states
        {
            get
            {
                return this._states;
            }
        }

        [ProtoMember(14, IsRequired = false, Name = "showdata", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public CharacterMapShow showdata
        {
            get
            {
                return this._showdata;
            }
            set
            {
                this._showdata = value;
            }
        }

        [ProtoMember(15, IsRequired = false, Name = "titlename", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string titlename
        {
            get
            {
                return this._titlename;
            }
            set
            {
                this._titlename = value;
            }
        }

        [ProtoMember(16, IsRequired = false, Name = "birth", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool birth
        {
            get
            {
                return this._birth;
            }
            set
            {
                this._birth = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(17, IsRequired = false, Name = "hatredlist", DataFormat = DataFormat.Default)]
        public NPC_HatredList hatredlist
        {
            get
            {
                return this._hatredlist;
            }
            set
            {
                this._hatredlist = value;
            }
        }

        private uint _baseid;

        private ulong _tempid;

        private string _name = string.Empty;

        private uint _hp;

        private uint _maxhp;

        private FloatMovePos _pos;

        private uint _dir;

        private uint _movespeed;

        private uint _attspeed;

        private MasterData _master;

        private uint _visit;

        private readonly List<StateItem> _states = new List<StateItem>();

        private CharacterMapShow _showdata;

        private string _titlename = string.Empty;

        private bool _birth;

        private NPC_HatredList _hatredlist;

        private IExtension extensionObject;
    }
}
