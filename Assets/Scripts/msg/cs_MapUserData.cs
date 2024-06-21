using System;
using msg;

public class cs_MapUserData
{
    public cs_MapUserData()
    {
    }

    public cs_MapUserData(MapUserData data)
    {
        this._charid = data.charid;
        this._name = data.name;
        this._mapshow = new cs_CharacterMapShow(data.mapshow);
        this._bakmapshow = new cs_CharacterMapShow(data.bakhero);
        this._mapdata = new cs_CharacterMapData(data.mapdata);
    }

    public ulong charid
    {
        get
        {
            return this._charid;
        }
        set
        {
            this._charid = value;
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

    public cs_CharacterMapShow mapshow
    {
        get
        {
            return this._mapshow;
        }
        set
        {
            this._mapshow = value;
        }
    }

    public cs_CharacterMapShow bakmapshow
    {
        get
        {
            return this._bakmapshow;
        }
        set
        {
            this._bakmapshow = value;
        }
    }

    public cs_CharacterMapData mapdata
    {
        get
        {
            return this._mapdata;
        }
        set
        {
            this._mapdata = value;
        }
    }

    public uint appearanceid
    {
        get
        {
            if (this.mapshow.coat != 0U)
            {
                return this.mapshow.coat;
            }
            return this.mapshow.heroid;
        }
    }

    public bool isSameExceptSpeetAndPosition(cs_MapUserData mapData)
    {
        return this._charid == mapData.charid && !(this._name != mapData._name) && this._mapshow.face == mapData._mapshow.face && this._mapshow.weapon == mapData._mapshow.weapon && this._mapshow.coat == mapData._mapshow.coat && this._mapshow.occupation == mapData._mapshow.occupation && this._mapshow.heroid == mapData._mapshow.heroid && this._mapshow.haircolor == mapData._mapshow.haircolor && this._mapshow.hairstyle == mapData._mapshow.hairstyle && this._mapshow.facestyle == mapData._mapshow.facestyle && this._mapshow.antenna == mapData._mapshow.antenna && this._mapdata.level == mapData._mapdata.level && this._mapdata.hp == mapData._mapdata.hp && this._mapdata.maxhp == mapData._mapdata.maxhp && this._mapdata.teamid == mapData._mapdata.teamid && this._mapdata.guildid == mapData._mapdata.guildid && this._mapdata.country == mapData._mapdata.country && !(this._mapdata.guildname != mapData._mapdata.guildname) && !(this._mapdata.titlename != mapData._mapdata.titlename);
    }

    private ulong _charid;

    private string _name = string.Empty;

    private string _guildname = string.Empty;

    private cs_CharacterMapShow _mapshow;

    private cs_CharacterMapShow _bakmapshow;

    private cs_CharacterMapData _mapdata;
}
