using System;
using msg;

public class cs_CharacterFightData
{
    public cs_CharacterFightData()
    {
    }

    public cs_CharacterFightData(CharacterFightData data)
    {
        this._curfightvalue = data.curfightvalue;
    }

    public uint curfightvalue
    {
        get
        {
            return this._curfightvalue;
        }
        set
        {
            this._curfightvalue = value;
        }
    }

    private uint _curfightvalue;
}
