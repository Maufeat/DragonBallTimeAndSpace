using System;
using System.Collections.Generic;
using msg;

public class cs_MapNpcData
{
    public cs_MapNpcData()
    {
    }

    public cs_MapNpcData(MapNpcData data)
    {
        this._baseid = data.baseid;
        this._tempid = data.tempid;
        this._name = data.name;
        this._hp = data.hp;
        this._maxhp = data.maxhp;
        this.x = data.pos.fx;
        this.y = data.pos.fy;
        this._dir = data.dir;
        this._movespeed = data.movespeed;
        this._attspeed = data.attspeed;
        if (data.master != null && data.master.idtype != null && data.master.idtype.id != 0UL)
        {
            this.MasterData = new NpcMasterData(data.master);
        }
        this._visit = data.visit;
        this._lstState = data.states;
        this._showdata = new cs_CharacterMapShow(data.showdata);
        this._titlename = data.titlename;
        this._bBirth = data.birth;
        if (!string.IsNullOrEmpty(data.titlename))
        {
            this._owner = data.titlename;
        }
    }

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

    public string introduce
    {
        get
        {
            return this._introduce;
        }
        set
        {
            this._introduce = value;
        }
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

    public List<uint> state
    {
        get
        {
            return this._state;
        }
    }

    public List<StateItem> lstState
    {
        get
        {
            return this._lstState;
        }
    }

    public cs_CharacterMapShow showdata
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

    public uint appearanceid
    {
        get
        {
            if (this.showdata != null)
            {
                if (this.showdata.coat != 0U)
                {
                    return this.showdata.coat;
                }
                if (this.showdata.heroid != 0U)
                {
                    return this.showdata.heroid;
                }
            }
            return this.baseid;
        }
    }

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

    public bool bBirth
    {
        get
        {
            return this._bBirth;
        }
        set
        {
            this._bBirth = value;
        }
    }

    public uint ApparentBodySize
    {
        get
        {
            return this._bodySize;
        }
        set
        {
            this._bodySize = value;
        }
    }

    public float FHitPercent
    {
        get
        {
            return this._fHitPercent;
        }
        set
        {
            this._fHitPercent = value;
        }
    }

    public string Owner
    {
        get
        {
            return this._owner;
        }
        set
        {
            this._owner = value;
        }
    }

    public NpcType NpcType
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

    public NpcMasterData MasterData;

    private uint _baseid;

    private ulong _tempid;

    private string _name = string.Empty;

    private string _introduce = string.Empty;

    private uint _hp;

    private uint _maxhp;

    public float x;

    public float y;

    private uint _dir;

    private uint _movespeed;

    private uint _attspeed;

    private uint _visit;

    private readonly List<uint> _state = new List<uint>();

    private readonly List<StateItem> _lstState = new List<StateItem>();

    private cs_CharacterMapShow _showdata;

    private string _titlename = string.Empty;

    private bool _bBirth;

    private uint _bodySize;

    private float _fHitPercent;

    private string _owner = string.Empty;

    private NpcType _type;
}
