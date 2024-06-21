using System;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
[Serializable]
public class CurveSamlpe
{
    [ProtoMember(1)]
    public ProtoVector3 P_pos
    {
        get
        {
            return ProtoVector3.Get(this.pos);
        }
        set
        {
            this.pos = value.Set();
        }
    }

    public Vector3 pos;

    [ProtoMember(2)]
    public float Distance;
}
