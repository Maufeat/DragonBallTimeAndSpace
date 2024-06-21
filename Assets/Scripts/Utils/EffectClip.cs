using System;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class EffectClip : ScriptableObject
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

    [ProtoMember(8)]
    public ProtoVector3 P_Position
    {
        get
        {
            return ProtoVector3.Get(this.Position);
        }
        set
        {
            this.Position = value.Set();
        }
    }

    [ProtoMember(9)]
    public ProtoVector3 P_Rotation
    {
        get
        {
            return ProtoVector3.Get(this.Rotation);
        }
        set
        {
            this.Rotation = value.Set();
        }
    }

    [ProtoMember(10)]
    public ProtoVector3 P_Scale
    {
        get
        {
            return ProtoVector3.Get(this.Scale);
        }
        set
        {
            this.Scale = value.Set();
        }
    }

    [ProtoMember(127)]
    public EffectClip[] ProtoList;

    [ProtoMember(1)]
    public string ClipName;

    [ProtoMember(2)]
    public string EffectName;

    [ProtoMember(3)]
    public string BindPointName;

    [ProtoMember(4)]
    public uint Duration;

    [ProtoMember(5)]
    public uint StartDalay;

    [ProtoMember(6)]
    public bool IsBind;

    [ProtoMember(7)]
    public bool IsInfinite;

    public Vector3 Position;

    public Vector3 Rotation;

    public Vector3 Scale;

    [ProtoMember(11)]
    public bool IsCameraEffect;

    [ProtoMember(12)]
    public bool IsPointPosition;
}
