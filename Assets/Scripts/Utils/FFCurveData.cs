using System;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
[Serializable]
public class FFCurveData
{
    [ProtoMember(1)]
    public ProtoAnimationCurve Pcurve
    {
        get
        {
            return ProtoAnimationCurve.Get(this.curve);
        }
        set
        {
            this.curve = value.Set();
        }
    }

    public void SetKey(Keyframe kf)
    {
        int num = -1;
        for (int i = 0; i < this.curve.keys.Length; i++)
        {
            Keyframe keyframe = this.curve.keys[i];
            if (object.Equals(keyframe.time, kf.time))
            {
                num = i;
                break;
            }
        }
        if (num != -1)
        {
            this.curve.MoveKey(num, kf);
        }
        else
        {
            this.curve.AddKey(kf);
        }
    }

    public AnimationCurve curve;

    [ProtoMember(2)]
    public string propertyName;
}
