using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class ProtoAnimationCurve
{
    public AnimationCurve Set()
    {
        this.KeyframetmpList.Clear();
        if (this.keys != null)
        {
            for (int i = 0; i < this.keys.Length; i++)
            {
                this.KeyframetmpList.Add(this.keys[i].Set());
            }
        }
        return new AnimationCurve(this.KeyframetmpList.ToArray())
        {
            postWrapMode = this.postWrapMode,
            preWrapMode = this.preWrapMode
        };
    }

    public static ProtoAnimationCurve Get(AnimationCurve Ac)
    {
        List<ProtoKeyframe> list = new List<ProtoKeyframe>();
        ProtoAnimationCurve protoAnimationCurve = new ProtoAnimationCurve();
        if (Ac != null)
        {
            if (Ac.keys != null)
            {
                for (int i = 0; i < Ac.keys.Length; i++)
                {
                    list.Add(ProtoKeyframe.Get(Ac.keys[i]));
                }
            }
            protoAnimationCurve.keys = list.ToArray();
            protoAnimationCurve.postWrapMode = Ac.postWrapMode;
            protoAnimationCurve.preWrapMode = Ac.preWrapMode;
        }
        return protoAnimationCurve;
    }

    [ProtoMember(1)]
    public ProtoKeyframe[] keys;

    [ProtoMember(2)]
    public WrapMode postWrapMode;

    [ProtoMember(3)]
    public WrapMode preWrapMode;

    private List<Keyframe> KeyframetmpList = new List<Keyframe>();
}
