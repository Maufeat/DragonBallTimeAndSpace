using System;
using msg;

public class cs_CharacterMapShow
{
    public cs_CharacterMapShow()
    {
    }

    public cs_CharacterMapShow(CharacterMapShow data)
    {
        this._face = data.face;
        this._weapon = data.weapon;
        this._coat = data.coat;
        this._occupation = data.occupation;
        this._heroid = data.heroid;
        this._haircolor = data.haircolor;
        this._hairstyle = data.hairstyle;
        this._facestyle = data.facestyle;
        this._antenna = data.antenna;
        this._bodystyle = data.bodystyle;
        this._avatarid = data.avatarId;
    }

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

    public uint facestyle
    {
        get
        {
            return this._facestyle;
        }
        set
        {
            this._facestyle = value;
        }
    }

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

    public uint face
    {
        get
        {
            return this._face;
        }
        set
        {
            this._face = value;
        }
    }

    public uint weapon
    {
        get
        {
            return this._weapon;
        }
        set
        {
            this._weapon = value;
        }
    }

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

    public uint occupation
    {
        get
        {
            return this._occupation;
        }
    }

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

    private uint _bodystyle;

    private uint _haircolor;

    private uint _hairstyle;

    private uint _facestyle;

    private uint _antenna;

    private uint _heroid;

    private uint _face;

    private uint _weapon;

    private uint _coat;

    private uint _occupation;

    private uint _avatarid;
}
