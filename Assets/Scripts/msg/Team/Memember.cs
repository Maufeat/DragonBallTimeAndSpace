using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "Memember")]
    [Serializable]
    public class Memember : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "mark", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint mark
        {
            get
            {
                return this._mark;
            }
            set
            {
                this._mark = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
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
        [ProtoMember(3, IsRequired = false, Name = "occupation", DataFormat = DataFormat.TwosComplement)]
        public uint occupation
        {
            get
            {
                return this._occupation;
            }
            set
            {
                this._occupation = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(5, Name = "skill", DataFormat = DataFormat.TwosComplement)]
        public List<uint> skill
        {
            get
            {
                return this._skill;
            }
        }

        [DefaultValue("")]
        [ProtoMember(6, IsRequired = false, Name = "mememberid", DataFormat = DataFormat.Default)]
        public string mememberid
        {
            get
            {
                return this._mememberid;
            }
            set
            {
                this._mememberid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "hp", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(8, IsRequired = false, Name = "maxhp", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(9, IsRequired = false, Name = "occupationlevel", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint occupationlevel
        {
            get
            {
                return this._occupationlevel;
            }
            set
            {
                this._occupationlevel = value;
            }
        }

        [ProtoMember(10, IsRequired = false, Name = "heroid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [ProtoMember(11, IsRequired = false, Name = "fight", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [ProtoMember(12, IsRequired = false, Name = "sceneid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string sceneid
        {
            get
            {
                return this._sceneid;
            }
            set
            {
                this._sceneid = value;
            }
        }

        [ProtoMember(13, IsRequired = false, Name = "state", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(MemState.NORMAL)]
        public MemState state
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(14, IsRequired = false, Name = "privilege", DataFormat = DataFormat.TwosComplement)]
        public uint privilege
        {
            get
            {
                return this._privilege;
            }
            set
            {
                this._privilege = value;
            }
        }

        [ProtoMember(15, IsRequired = false, Name = "hairstyle", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint hairstyle
        {
            get
            {
                return this._hairstyle;
            }
            set
            {
                this._hairstyle = value;
            }
        }

        [ProtoMember(16, IsRequired = false, Name = "haircolor", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint haircolor
        {
            get
            {
                return this._haircolor;
            }
            set
            {
                this._haircolor = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(17, IsRequired = false, Name = "headstyle", DataFormat = DataFormat.TwosComplement)]
        public uint headstyle
        {
            get
            {
                return this._headstyle;
            }
            set
            {
                this._headstyle = value;
            }
        }

        [ProtoMember(18, IsRequired = false, Name = "bodystyle", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint bodystyle
        {
            get
            {
                return this._bodystyle;
            }
            set
            {
                this._bodystyle = value;
            }
        }

        [ProtoMember(19, IsRequired = false, Name = "antenna", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint antenna
        {
            get
            {
                return this._antenna;
            }
            set
            {
                this._antenna = value;
            }
        }

        [ProtoMember(20, IsRequired = false, Name = "avatarid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint avatarid
        {
            get
            {
                return this._avatarid;
            }
            set
            {
                this._avatarid = value;
            }
        }

        private uint _mark;

        private string _name = string.Empty;

        private uint _occupation;

        private uint _level;

        private readonly List<uint> _skill = new List<uint>();

        private string _mememberid = string.Empty;

        private uint _hp;

        private uint _maxhp;

        private uint _occupationlevel;

        private uint _heroid;

        private uint _fight;

        private string _sceneid = string.Empty;

        private MemState _state;

        private uint _privilege;

        private uint _hairstyle;

        private uint _haircolor;

        private uint _headstyle;

        private uint _bodystyle;

        private uint _antenna;

        private uint _avatarid;

        private IExtension extensionObject;
    }
}
