using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "CharacterBaseData")]
    [Serializable]
    public class CharacterBaseData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(2, IsRequired = false, Name = "exp", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "money", DataFormat = DataFormat.TwosComplement)]
        public uint money
        {
            get
            {
                return this._money;
            }
            set
            {
                this._money = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "welpoint", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint welpoint
        {
            get
            {
                return this._welpoint;
            }
            set
            {
                this._welpoint = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "tilizhi", DataFormat = DataFormat.TwosComplement)]
        public uint tilizhi
        {
            get
            {
                return this._tilizhi;
            }
            set
            {
                this._tilizhi = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        public uint type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "famelevel", DataFormat = DataFormat.TwosComplement)]
        public uint famelevel
        {
            get
            {
                return this._famelevel;
            }
            set
            {
                this._famelevel = value;
            }
        }

        [ProtoMember(8, IsRequired = false, Name = "position", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(9, IsRequired = false, Name = "viplevel", DataFormat = DataFormat.TwosComplement)]
        public uint viplevel
        {
            get
            {
                return this._viplevel;
            }
            set
            {
                this._viplevel = value;
            }
        }

        [ProtoMember(10, IsRequired = false, Name = "port", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint port
        {
            get
            {
                return this._port;
            }
            set
            {
                this._port = value;
            }
        }

        [ProtoMember(11, IsRequired = false, Name = "laststage", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint laststage
        {
            get
            {
                return this._laststage;
            }
            set
            {
                this._laststage = value;
            }
        }

        [ProtoMember(12, IsRequired = false, Name = "nextexp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong nextexp
        {
            get
            {
                return this._nextexp;
            }
            set
            {
                this._nextexp = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(13, IsRequired = false, Name = "pkmode", DataFormat = DataFormat.TwosComplement)]
        public uint pkmode
        {
            get
            {
                return this._pkmode;
            }
            set
            {
                this._pkmode = value;
            }
        }

        [ProtoMember(14, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(15, IsRequired = false, Name = "stone", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint stone
        {
            get
            {
                return this._stone;
            }
            set
            {
                this._stone = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(16, IsRequired = false, Name = "edupoint", DataFormat = DataFormat.TwosComplement)]
        public uint edupoint
        {
            get
            {
                return this._edupoint;
            }
            set
            {
                this._edupoint = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(17, IsRequired = false, Name = "cooppoint", DataFormat = DataFormat.TwosComplement)]
        public uint cooppoint
        {
            get
            {
                return this._cooppoint;
            }
            set
            {
                this._cooppoint = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(18, IsRequired = false, Name = "bluecrystal", DataFormat = DataFormat.TwosComplement)]
        public uint bluecrystal
        {
            get
            {
                return this._bluecrystal;
            }
            set
            {
                this._bluecrystal = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(19, IsRequired = false, Name = "bluecrystalincnum", DataFormat = DataFormat.TwosComplement)]
        public uint bluecrystalincnum
        {
            get
            {
                return this._bluecrystalincnum;
            }
            set
            {
                this._bluecrystalincnum = value;
            }
        }

        [ProtoMember(20, IsRequired = false, Name = "purplecrystal", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint purplecrystal
        {
            get
            {
                return this._purplecrystal;
            }
            set
            {
                this._purplecrystal = value;
            }
        }

        [ProtoMember(21, IsRequired = false, Name = "purplecrystalincnum", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint purplecrystalincnum
        {
            get
            {
                return this._purplecrystalincnum;
            }
            set
            {
                this._purplecrystalincnum = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(22, IsRequired = false, Name = "vigourpoint", DataFormat = DataFormat.TwosComplement)]
        public uint vigourpoint
        {
            get
            {
                return this._vigourpoint;
            }
            set
            {
                this._vigourpoint = value;
            }
        }

        [ProtoMember(23, IsRequired = false, Name = "doublepoint", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint doublepoint
        {
            get
            {
                return this._doublepoint;
            }
            set
            {
                this._doublepoint = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(24, IsRequired = false, Name = "familyatt", DataFormat = DataFormat.TwosComplement)]
        public uint familyatt
        {
            get
            {
                return this._familyatt;
            }
            set
            {
                this._familyatt = value;
            }
        }

        [ProtoMember(25, IsRequired = false, Name = "herothisid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string herothisid
        {
            get
            {
                return this._herothisid;
            }
            set
            {
                this._herothisid = value;
            }
        }

        private ulong _exp;

        private uint _money;

        private uint _welpoint;

        private uint _tilizhi;

        private uint _type;

        private uint _famelevel;

        private uint _position;

        private uint _viplevel;

        private uint _port;

        private uint _laststage;

        private ulong _nextexp;

        private uint _pkmode;

        private uint _level;

        private uint _stone;

        private uint _edupoint;

        private uint _cooppoint;

        private uint _bluecrystal;

        private uint _bluecrystalincnum;

        private uint _purplecrystal;

        private uint _purplecrystalincnum;

        private uint _vigourpoint;

        private uint _doublepoint;

        private uint _familyatt;

        private string _herothisid = string.Empty;

        private IExtension extensionObject;
    }
}
