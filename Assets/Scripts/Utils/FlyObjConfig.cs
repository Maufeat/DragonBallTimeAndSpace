using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class FlyObjConfig : ScriptableObject
{
    [ProtoMember(126)]
    public string UObjname
    {
        get
        {
            return base.name;
        }
        set
        {
            base.name = value;
        }
    }

    [ProtoMember(9)]
    public ProtoVector3 P_TargetRelativePos
    {
        get
        {
            return ProtoVector3.Get(this.TargetRelativePos);
        }
        set
        {
            this.TargetRelativePos = value.Set();
        }
    }

    [ProtoMember(10)]
    public ProtoVector3 P_LaunchRelativePos
    {
        get
        {
            return ProtoVector3.Get(this.LaunchRelativePos);
        }
        set
        {
            this.LaunchRelativePos = value.Set();
        }
    }

    [ProtoMember(12)]
    public ProtoVector3[] P_CurveControlPos
    {
        get
        {
            if (this.CurveControlPos != null)
            {
                List<ProtoVector3> list = new List<ProtoVector3>();
                for (int i = 0; i < this.CurveControlPos.Length; i++)
                {
                    list.Add(ProtoVector3.Get(this.CurveControlPos[i]));
                }
                return list.ToArray();
            }
            return new ProtoVector3[0];
        }
        set
        {
            if (value != null)
            {
                List<Vector3> list = new List<Vector3>();
                for (int i = 0; i < value.Length; i++)
                {
                    list.Add(value[i].Set());
                }
                this.CurveControlPos = list.ToArray();
            }
            else
            {
                this.CurveControlPos = new Vector3[0];
            }
        }
    }

    [ProtoMember(127)]
    public FlyObjConfig[] ProtoList;

    [ProtoMember(1)]
    public string FlyobjNmae;

    [ProtoMember(2)]
    public FlyObjConfig.FlyTrackType mFlyTrackType;

    [ProtoMember(3)]
    public FlyObjConfig.TargetType mTargetType;

    [ProtoMember(4)]
    public FlyObjConfig.LaunchType mLaunchType;

    [ProtoMember(5)]
    public uint TotalLength;

    [ProtoMember(6)]
    public uint FlyLength;

    [ProtoMember(7)]
    public uint LaunchDalayF;

    [ProtoMember(8)]
    public string LaunchBindPoint;

    public Vector3 TargetRelativePos;

    public Vector3 LaunchRelativePos;

    [ProtoMember(11)]
    public string[] EffectList = new string[0];

    public Vector3[] CurveControlPos;

    [ProtoMember(13)]
    public CurveSamlpe[] CurveSamlpeList = new CurveSamlpe[0];

    public enum TargetType
    {
        TargetEntity,
        Position
    }

    public enum LaunchType
    {
        ByHit,
        ByStart
    }

    public enum FlyTrackType
    {
        Straight,
        Curve
    }
}
