using System;
using msg;

public class cs_AttributeData
{
    public cs_AttributeData()
    {
    }

    public cs_AttributeData(AttributeData data)
    {
        this._hp = data.hp;
        this._maxhp = data.maxhp;
        this._mp = data.mp;
        this._maxmp = data.maxmp;
        this._str = data.str;
        this._dex = data.dex;
        this._intel = data.intel;
        this._phy = data.phy;
        this._matt = data.matt;
        this._patt = data.patt;
        this._mdef = data.mdef;
        this._pdef = data.pdef;
        this._bang = data.bang;
        this._bangextradamage = data.bangextradamage;
        this._toughness = data.toughness;
        this._toughnessreducedamage = data.toughnessreducedamage;
        this._penetrate = data.penetrate;
        this._hit = data.hit;
        this._penetrateextradamage = data.penetrateextradamage;
        this._block = data.block;
        this._blockreducedamage = data.blockreducedamage;
        this._accurate = data.accurate;
        this._accurateextradamage = data.accurateextradamage;
        this._hold = data.hold;
        this._holdreducedamage = data.holdreducedamage;
        this._deflect = data.deflect;
        this._deflectreducedamage = data.deflectreducedamage;
        this._dodge = data.dodge;
        this._weaponatt = data.weaponatt;
        this._firemastery = data.firemastery;
        this._icemastery = data.icemastery;
        this._lightningmastery = data.lightningmastery;
        this._brightmastery = data.brightmastery;
        this._darkmastery = data.darkmastery;
        this._fireresist = data.fireresist;
        this._iceresist = data.iceresist;
        this._lightningresist = data.lightningresist;
        this._brightresist = data.brightresist;
        this._darkresist = data.darkresist;
        this._firepen = data.firepen;
        this._icepen = data.icepen;
        this._lightningpen = data.lightningpen;
        this._brightpen = data.brightpen;
        this._darkpen = data.darkpen;
        this._blowint = data.blowint;
        this._knockint = data.knockint;
        this._floatint = data.floatint;
        this._superhitint = data.superhitint;
        this._blowresist = data.blowresist;
        this._knockresist = data.knockresist;
        this._floatresist = data.floatresist;
        this._superhitresist = data.superhitresist;
        this._blowdectime = data.blowdectime;
        this._knockdectime = data.knockdectime;
        this._floatdectime = data.floatdectime;
        this._superhitdectime = data.superhitdectime;
        this._stiffaddtime = data.stiffaddtime;
        this._stiffdectime = data.stiffdectime;
    }

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

    public uint matt
    {
        get
        {
            return this._matt;
        }
        set
        {
            this._matt = value;
        }
    }

    public uint patt
    {
        get
        {
            return this._patt;
        }
        set
        {
            this._patt = value;
        }
    }

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

    public uint penerate
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

    public uint penerateextradamage
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

    public uint dodge
    {
        get
        {
            return this._dodge;
        }
        set
        {
            this._dodge = value;
        }
    }

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

    public uint mp
    {
        get
        {
            return this._mp;
        }
        set
        {
            this._mp = value;
        }
    }

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

    private uint _hp;

    private uint _maxhp;

    private uint _str;

    private uint _dex;

    private uint _intel;

    private uint _phy;

    private uint _matt;

    private uint _patt;

    private uint _mdef;

    private uint _pdef;

    private uint _bang;

    private uint _bangextradamage;

    private uint _toughness;

    private uint _toughnessreducedamage;

    private uint _penetrate;

    private uint _hit;

    private uint _penetrateextradamage;

    private uint _block;

    private uint _blockreducedamage;

    private uint _accurate;

    private uint _accurateextradamage;

    private uint _hold;

    private uint _holdreducedamage;

    private uint _deflect;

    private uint _deflectreducedamage;

    private uint _dodge;

    private uint _weaponatt;

    private uint _mp;

    private uint _maxmp;

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

    private uint _blowdectime;

    private uint _knockdectime;

    private uint _floatdectime;

    private uint _superhitdectime;

    private uint _stiffaddtime;

    private uint _stiffdectime;
}
