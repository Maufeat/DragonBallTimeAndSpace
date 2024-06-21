using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "TeamUser")]
    [Serializable]
    public class TeamUser : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "uid", DataFormat = DataFormat.TwosComplement)]
        public ulong uid
        {
            get
            {
                return this._uid;
            }
            set
            {
                this._uid = value;
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
        [ProtoMember(3, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "x", DataFormat = DataFormat.TwosComplement)]
        public uint x
        {
            get
            {
                return this._x;
            }
            set
            {
                this._x = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "y", DataFormat = DataFormat.TwosComplement)]
        public uint y
        {
            get
            {
                return this._y;
            }
            set
            {
                this._y = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "online", DataFormat = DataFormat.Default)]
        [DefaultValue(false)]
        public bool online
        {
            get
            {
                return this._online;
            }
            set
            {
                this._online = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "hairstyle", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(8, IsRequired = false, Name = "haircolor", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(9, IsRequired = false, Name = "headstyle", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [DefaultValue(0L)]
        [ProtoMember(10, IsRequired = false, Name = "bodystyle", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(11, IsRequired = false, Name = "antenna", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(12, IsRequired = false, Name = "coat", DataFormat = DataFormat.TwosComplement)]
        public uint coat
        {
            get
            {
                return this._coat;
            }
            set
            {
                this._coat = value;
            }
        }

        [ProtoMember(13, IsRequired = false, Name = "avatarid", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(14, IsRequired = false, Name = "maxhp", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(15, IsRequired = false, Name = "hp", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(16, IsRequired = false, Name = "heroid", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(17, IsRequired = false, Name = "exp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint exp
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

        private ulong _uid;

        private string _name = string.Empty;

        private uint _level;

        private uint _x;

        private uint _y;

        private bool _online;

        private uint _hairstyle;

        private uint _haircolor;

        private uint _headstyle;

        private uint _bodystyle;

        private uint _antenna;

        private uint _coat;

        private uint _avatarid;

        private uint _maxhp;

        private uint _hp;

        private uint _heroid;

        private uint _exp;

        private IExtension extensionObject;
    }
}
