using System;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public struct ProtoVector3
{
    public Vector3 Set()
    {
        return new Vector3(this.x, this.y, this.z);
    }

    public static ProtoVector3 Get(Vector3 v)
    {
        return new ProtoVector3
        {
            x = v.x,
            y = v.y,
            z = v.z
        };
    }

    public override string ToString()
    {
        return this.Set().ToString();
    }

    [ProtoMember(1)]
    public float x;

    [ProtoMember(2)]
    public float y;

    [ProtoMember(3)]
    public float z;
}
