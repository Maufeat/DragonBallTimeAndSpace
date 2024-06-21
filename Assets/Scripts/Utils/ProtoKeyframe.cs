using System;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public struct ProtoKeyframe
{
    public Keyframe Set()
    {
        return new Keyframe(this.time, this.value, this.inTangent, this.outTangent)
        {
            tangentMode = this.tangentMode
        };
    }

    public static ProtoKeyframe Get(Keyframe kf)
    {
        return new ProtoKeyframe
        {
            inTangent = kf.inTangent,
            outTangent = kf.outTangent,
            tangentMode = kf.tangentMode,
            time = kf.time,
            value = kf.value
        };
    }

    [ProtoMember(1)]
    public float inTangent;

    [ProtoMember(2)]
    public float outTangent;

    [ProtoMember(3)]
    public int tangentMode;

    [ProtoMember(4)]
    public float time;

    [ProtoMember(5)]
    public float value;
}
