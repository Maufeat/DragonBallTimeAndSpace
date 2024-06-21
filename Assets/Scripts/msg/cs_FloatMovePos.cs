using System;
using msg;

public struct cs_FloatMovePos
{
    public cs_FloatMovePos(FloatMovePos data)
    {
        this.fx = data.fx;
        this.fy = data.fy;
    }

    public cs_FloatMovePos(float x, float y)
    {
        this.fx = x;
        this.fy = y;
    }

    public float fx { get; set; }

    public float fy { get; set; }
}
