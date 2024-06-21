using System;
using msg;

public class cs_MoveData
{
    public cs_MoveData()
    {
    }

    public cs_MoveData(MoveData data)
    {
        this.pos = new cs_FloatMovePos(data.pos);
        this.dir = data.dir;
    }

    public cs_FloatMovePos pos;

    public uint dir;

    public int step;
}
