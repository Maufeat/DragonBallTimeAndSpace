using System;

public class cs_BaseData
{
    public cs_BaseData()
    {
    }

    public cs_BaseData(CharactorType type, ulong id, float posx, float posy, uint dir)
    {
        this._charactorType = type;
        this._id = id;
        this._pos = default(cs_FloatMovePos);
        this._pos.fx = posx;
        this._pos.fy = posy;
        this._dir = dir;
    }

    public CharactorType charactorType
    {
        get
        {
            return this._charactorType;
        }
        set
        {
            this._charactorType = value;
        }
    }

    public ulong id
    {
        get
        {
            return this._id;
        }
        set
        {
            this._id = value;
        }
    }

    public cs_FloatMovePos pos
    {
        get
        {
            return this._pos;
        }
        set
        {
            this._pos = value;
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

    private CharactorType _charactorType;

    private ulong _id;

    private cs_FloatMovePos _pos;

    private uint _dir;
}
