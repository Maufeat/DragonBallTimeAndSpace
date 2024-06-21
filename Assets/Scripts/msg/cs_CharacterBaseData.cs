using System;
using msg;

public class cs_CharacterBaseData
{
    public cs_CharacterBaseData()
    {
    }

    public cs_CharacterBaseData(CharacterBaseData data)
    {
        this._exp = data.exp;
        this._welfarePoint = data.welpoint;
        this._money = data.money;
        this._stone = data.stone;
        this._tilizhi = data.tilizhi;
        this._type = data.type;
        this._famelevel = data.famelevel;
        this._position = data.position;
        this._viplevel = data.viplevel;
        this._port = data.port;
        this._laststage = data.laststage;
        this._nextexp = data.nextexp;
        this._pkmode = data.pkmode;
        this._edupoint = data.edupoint;
        this._cooppoint = data.cooppoint;
        this._bluecrystal = data.bluecrystal;
        this._purplecrystal = data.purplecrystal;
        this._vigourpoint = data.vigourpoint;
        this._doublepoint = data.doublepoint;
        this._bluecrystalincnum = data.bluecrystalincnum;
        this._purplecrystalincnum = data.purplecrystalincnum;
        this._familyatt = data.familyatt;
        this._herothisid = data.herothisid;
    }

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

    public uint WelPoint
    {
        get
        {
            return this._welfarePoint;
        }
        set
        {
            this._welfarePoint = value;
        }
    }

    public uint Money
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

    public uint Stone
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

    public uint EduPoint
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

    private uint _welfarePoint;

    private uint _money;

    private uint _stone;

    private uint _tilizhi;

    private uint _type;

    private uint _famelevel;

    private uint _position;

    private uint _viplevel;

    private uint _port;

    private uint _laststage;

    private ulong _nextexp;

    private uint _pkmode;

    private uint _edupoint;

    private uint _cooppoint;

    private uint _bluecrystal;

    private uint _bluecrystalincnum;

    private uint _purplecrystal;

    private uint _purplecrystalincnum;

    private uint _vigourpoint;

    private uint _doublepoint;

    private uint _familyatt;

    private string _herothisid;
}
