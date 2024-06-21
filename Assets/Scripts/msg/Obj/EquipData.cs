using System;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "EquipData")]
    [Serializable]
    public class EquipData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "weaponatt", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint weaponatt
        {
            get
            {
                return this._weaponatt;
            }
            set
            {
                this._weaponatt = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "pdam", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint pdam
        {
            get
            {
                return this._pdam;
            }
            set
            {
                this._pdam = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "mdam", DataFormat = DataFormat.TwosComplement)]
        public uint mdam
        {
            get
            {
                return this._mdam;
            }
            set
            {
                this._mdam = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "pdef", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint pdef
        {
            get
            {
                return this._pdef;
            }
            set
            {
                this._pdef = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "mdef", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint mdef
        {
            get
            {
                return this._mdef;
            }
            set
            {
                this._mdef = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "maxhp", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "str", DataFormat = DataFormat.TwosComplement)]
        public uint str
        {
            get
            {
                return this._str;
            }
            set
            {
                this._str = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(8, IsRequired = false, Name = "dex", DataFormat = DataFormat.TwosComplement)]
        public uint dex
        {
            get
            {
                return this._dex;
            }
            set
            {
                this._dex = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(9, IsRequired = false, Name = "intel", DataFormat = DataFormat.TwosComplement)]
        public uint intel
        {
            get
            {
                return this._intel;
            }
            set
            {
                this._intel = value;
            }
        }

        [ProtoMember(10, IsRequired = false, Name = "phy", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint phy
        {
            get
            {
                return this._phy;
            }
            set
            {
                this._phy = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(11, IsRequired = false, Name = "bang", DataFormat = DataFormat.TwosComplement)]
        public uint bang
        {
            get
            {
                return this._bang;
            }
            set
            {
                this._bang = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(12, IsRequired = false, Name = "toughness", DataFormat = DataFormat.TwosComplement)]
        public uint toughness
        {
            get
            {
                return this._toughness;
            }
            set
            {
                this._toughness = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(13, IsRequired = false, Name = "block", DataFormat = DataFormat.TwosComplement)]
        public uint block
        {
            get
            {
                return this._block;
            }
            set
            {
                this._block = value;
            }
        }

        [ProtoMember(14, IsRequired = false, Name = "penetrate", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint penetrate
        {
            get
            {
                return this._penetrate;
            }
            set
            {
                this._penetrate = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(15, IsRequired = false, Name = "accurate", DataFormat = DataFormat.TwosComplement)]
        public uint accurate
        {
            get
            {
                return this._accurate;
            }
            set
            {
                this._accurate = value;
            }
        }

        [ProtoMember(16, IsRequired = false, Name = "hold", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint hold
        {
            get
            {
                return this._hold;
            }
            set
            {
                this._hold = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(17, IsRequired = false, Name = "deflect", DataFormat = DataFormat.TwosComplement)]
        public uint deflect
        {
            get
            {
                return this._deflect;
            }
            set
            {
                this._deflect = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(18, IsRequired = false, Name = "bangextradamage", DataFormat = DataFormat.TwosComplement)]
        public uint bangextradamage
        {
            get
            {
                return this._bangextradamage;
            }
            set
            {
                this._bangextradamage = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(19, IsRequired = false, Name = "toughnessreducedamage", DataFormat = DataFormat.TwosComplement)]
        public uint toughnessreducedamage
        {
            get
            {
                return this._toughnessreducedamage;
            }
            set
            {
                this._toughnessreducedamage = value;
            }
        }

        [ProtoMember(20, IsRequired = false, Name = "blockreducedamage", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint blockreducedamage
        {
            get
            {
                return this._blockreducedamage;
            }
            set
            {
                this._blockreducedamage = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(21, IsRequired = false, Name = "penetrateextradamage", DataFormat = DataFormat.TwosComplement)]
        public uint penetrateextradamage
        {
            get
            {
                return this._penetrateextradamage;
            }
            set
            {
                this._penetrateextradamage = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(22, IsRequired = false, Name = "accurateextradamage", DataFormat = DataFormat.TwosComplement)]
        public uint accurateextradamage
        {
            get
            {
                return this._accurateextradamage;
            }
            set
            {
                this._accurateextradamage = value;
            }
        }

        [ProtoMember(23, IsRequired = false, Name = "holdreducedamage", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint holdreducedamage
        {
            get
            {
                return this._holdreducedamage;
            }
            set
            {
                this._holdreducedamage = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(24, IsRequired = false, Name = "deflectreducedamage", DataFormat = DataFormat.TwosComplement)]
        public uint deflectreducedamage
        {
            get
            {
                return this._deflectreducedamage;
            }
            set
            {
                this._deflectreducedamage = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(25, IsRequired = false, Name = "maxmp", DataFormat = DataFormat.TwosComplement)]
        public uint maxmp
        {
            get
            {
                return this._maxmp;
            }
            set
            {
                this._maxmp = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(26, IsRequired = false, Name = "miss", DataFormat = DataFormat.TwosComplement)]
        public uint miss
        {
            get
            {
                return this._miss;
            }
            set
            {
                this._miss = value;
            }
        }

        [ProtoMember(27, IsRequired = false, Name = "hit", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint hit
        {
            get
            {
                return this._hit;
            }
            set
            {
                this._hit = value;
            }
        }

        [ProtoMember(28, IsRequired = false, Name = "firemastery", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint firemastery
        {
            get
            {
                return this._firemastery;
            }
            set
            {
                this._firemastery = value;
            }
        }

        [ProtoMember(29, IsRequired = false, Name = "icemastery", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint icemastery
        {
            get
            {
                return this._icemastery;
            }
            set
            {
                this._icemastery = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(30, IsRequired = false, Name = "lightningmastery", DataFormat = DataFormat.TwosComplement)]
        public uint lightningmastery
        {
            get
            {
                return this._lightningmastery;
            }
            set
            {
                this._lightningmastery = value;
            }
        }

        [ProtoMember(31, IsRequired = false, Name = "brightmastery", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint brightmastery
        {
            get
            {
                return this._brightmastery;
            }
            set
            {
                this._brightmastery = value;
            }
        }

        [ProtoMember(32, IsRequired = false, Name = "darkmastery", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint darkmastery
        {
            get
            {
                return this._darkmastery;
            }
            set
            {
                this._darkmastery = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(33, IsRequired = false, Name = "fireresist", DataFormat = DataFormat.TwosComplement)]
        public uint fireresist
        {
            get
            {
                return this._fireresist;
            }
            set
            {
                this._fireresist = value;
            }
        }

        [ProtoMember(34, IsRequired = false, Name = "iceresist", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint iceresist
        {
            get
            {
                return this._iceresist;
            }
            set
            {
                this._iceresist = value;
            }
        }

        [ProtoMember(35, IsRequired = false, Name = "lightningresist", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint lightningresist
        {
            get
            {
                return this._lightningresist;
            }
            set
            {
                this._lightningresist = value;
            }
        }

        [ProtoMember(36, IsRequired = false, Name = "brightresist", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint brightresist
        {
            get
            {
                return this._brightresist;
            }
            set
            {
                this._brightresist = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(37, IsRequired = false, Name = "darkresist", DataFormat = DataFormat.TwosComplement)]
        public uint darkresist
        {
            get
            {
                return this._darkresist;
            }
            set
            {
                this._darkresist = value;
            }
        }

        [ProtoMember(38, IsRequired = false, Name = "firepen", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint firepen
        {
            get
            {
                return this._firepen;
            }
            set
            {
                this._firepen = value;
            }
        }

        [ProtoMember(39, IsRequired = false, Name = "icepen", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint icepen
        {
            get
            {
                return this._icepen;
            }
            set
            {
                this._icepen = value;
            }
        }

        [ProtoMember(40, IsRequired = false, Name = "lightningpen", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint lightningpen
        {
            get
            {
                return this._lightningpen;
            }
            set
            {
                this._lightningpen = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(41, IsRequired = false, Name = "brightpen", DataFormat = DataFormat.TwosComplement)]
        public uint brightpen
        {
            get
            {
                return this._brightpen;
            }
            set
            {
                this._brightpen = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(42, IsRequired = false, Name = "darkpen", DataFormat = DataFormat.TwosComplement)]
        public uint darkpen
        {
            get
            {
                return this._darkpen;
            }
            set
            {
                this._darkpen = value;
            }
        }

        [ProtoMember(43, IsRequired = false, Name = "blowint", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint blowint
        {
            get
            {
                return this._blowint;
            }
            set
            {
                this._blowint = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(44, IsRequired = false, Name = "knockint", DataFormat = DataFormat.TwosComplement)]
        public uint knockint
        {
            get
            {
                return this._knockint;
            }
            set
            {
                this._knockint = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(45, IsRequired = false, Name = "floatint", DataFormat = DataFormat.TwosComplement)]
        public uint floatint
        {
            get
            {
                return this._floatint;
            }
            set
            {
                this._floatint = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(46, IsRequired = false, Name = "superhitint", DataFormat = DataFormat.TwosComplement)]
        public uint superhitint
        {
            get
            {
                return this._superhitint;
            }
            set
            {
                this._superhitint = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(47, IsRequired = false, Name = "blowresist", DataFormat = DataFormat.TwosComplement)]
        public uint blowresist
        {
            get
            {
                return this._blowresist;
            }
            set
            {
                this._blowresist = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(48, IsRequired = false, Name = "knockresist", DataFormat = DataFormat.TwosComplement)]
        public uint knockresist
        {
            get
            {
                return this._knockresist;
            }
            set
            {
                this._knockresist = value;
            }
        }

        [ProtoMember(49, IsRequired = false, Name = "floatresist", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint floatresist
        {
            get
            {
                return this._floatresist;
            }
            set
            {
                this._floatresist = value;
            }
        }

        [ProtoMember(50, IsRequired = false, Name = "superhitresist", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint superhitresist
        {
            get
            {
                return this._superhitresist;
            }
            set
            {
                this._superhitresist = value;
            }
        }

        [ProtoMember(51, IsRequired = false, Name = "stiffaddtime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint stiffaddtime
        {
            get
            {
                return this._stiffaddtime;
            }
            set
            {
                this._stiffaddtime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(52, IsRequired = false, Name = "stiffdectime", DataFormat = DataFormat.TwosComplement)]
        public uint stiffdectime
        {
            get
            {
                return this._stiffdectime;
            }
            set
            {
                this._stiffdectime = value;
            }
        }

        [ProtoMember(53, IsRequired = false, Name = "blowdectime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint blowdectime
        {
            get
            {
                return this._blowdectime;
            }
            set
            {
                this._blowdectime = value;
            }
        }

        [ProtoMember(54, IsRequired = false, Name = "knockdectime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint knockdectime
        {
            get
            {
                return this._knockdectime;
            }
            set
            {
                this._knockdectime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(55, IsRequired = false, Name = "floatdectime", DataFormat = DataFormat.TwosComplement)]
        public uint floatdectime
        {
            get
            {
                return this._floatdectime;
            }
            set
            {
                this._floatdectime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(56, IsRequired = false, Name = "superhitdectime", DataFormat = DataFormat.TwosComplement)]
        public uint superhitdectime
        {
            get
            {
                return this._superhitdectime;
            }
            set
            {
                this._superhitdectime = value;
            }
        }

        private uint _weaponatt;

        private uint _pdam;

        private uint _mdam;

        private uint _pdef;

        private uint _mdef;

        private uint _maxhp;

        private uint _str;

        private uint _dex;

        private uint _intel;

        private uint _phy;

        private uint _bang;

        private uint _toughness;

        private uint _block;

        private uint _penetrate;

        private uint _accurate;

        private uint _hold;

        private uint _deflect;

        private uint _bangextradamage;

        private uint _toughnessreducedamage;

        private uint _blockreducedamage;

        private uint _penetrateextradamage;

        private uint _accurateextradamage;

        private uint _holdreducedamage;

        private uint _deflectreducedamage;

        private uint _maxmp;

        private uint _miss;

        private uint _hit;

        private uint _firemastery;

        private uint _icemastery;

        private uint _lightningmastery;

        private uint _brightmastery;

        private uint _darkmastery;

        private uint _fireresist;

        private uint _iceresist;

        private uint _lightningresist;

        private uint _brightresist;

        private uint _darkresist;

        private uint _firepen;

        private uint _icepen;

        private uint _lightningpen;

        private uint _brightpen;

        private uint _darkpen;

        private uint _blowint;

        private uint _knockint;

        private uint _floatint;

        private uint _superhitint;

        private uint _blowresist;

        private uint _knockresist;

        private uint _floatresist;

        private uint _superhitresist;

        private uint _stiffaddtime;

        private uint _stiffdectime;

        private uint _blowdectime;

        private uint _knockdectime;

        private uint _floatdectime;

        private uint _superhitdectime;

        private IExtension extensionObject;
    }
}
