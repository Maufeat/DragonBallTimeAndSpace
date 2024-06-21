using System;
using System.Collections.Generic;
using msg;

public class cs_CharacterMapData
{
    public cs_CharacterMapData()
    {
    }

    public cs_CharacterMapData(CharacterMapData data)
    {
        this._level = data.level;
        this.pos = new cs_FloatMovePos(data.pos);
        this._dir = data.dir;
        this._movespeed = data.movespeed;
        this._hp = data.hp;
        this._maxhp = data.maxhp;
        this._teamid = data.teamid;
        this._guildid = data.guildid;
        this._country = data.country;
        this._guildname = data.guildname;
        this._tittlename = string.Empty;
        for (int i = 0; i < data.states.Count; i++)
        {
            this.lstState.Add(data.states[i]);
        }
    }

    public cs_CharacterMapData(NpcMasterData data)
    {
        this._teamid = data.teamid;
        this._guildid = data.guildid;
        this._country = data.country;
    }

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

    public cs_FloatMovePos pos { get; set; }

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

    public string titlename
    {
        get
        {
            return this._tittlename;
        }
        set
        {
            this._tittlename = value;
        }
    }

    private uint _level;

    private uint _dir;

    private uint _movespeed;

    private uint _hp;

    private uint _maxhp;

    private readonly List<uint> _state = new List<uint>();

    private readonly List<StateItem> _lstState = new List<StateItem>();

    private uint _teamid;

    private ulong _guildid;

    private uint _country;

    private string _guildname = string.Empty;

    private string _tittlename = string.Empty;
}
