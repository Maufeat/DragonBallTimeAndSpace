using System;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class WeaponArtConfig : ScriptableObject
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

    [ProtoMember(4)]
    public ProtoVector3 P_NormalPosition
    {
        get
        {
            return ProtoVector3.Get(this.NormalPosition);
        }
        set
        {
            this.NormalPosition = value.Set();
        }
    }

    [ProtoMember(5)]
    public ProtoVector3 P_NormalRotation
    {
        get
        {
            return ProtoVector3.Get(this.NormalRotation);
        }
        set
        {
            this.NormalRotation = value.Set();
        }
    }

    [ProtoMember(6)]
    public ProtoVector3 P_NormalScale
    {
        get
        {
            return ProtoVector3.Get(this.NormalScale);
        }
        set
        {
            this.NormalScale = value.Set();
        }
    }

    [ProtoMember(8)]
    public ProtoVector3 P_FightPosition
    {
        get
        {
            return ProtoVector3.Get(this.FightPosition);
        }
        set
        {
            this.FightPosition = value.Set();
        }
    }

    [ProtoMember(9)]
    public ProtoVector3 P_FightlRotation
    {
        get
        {
            return ProtoVector3.Get(this.FightlRotation);
        }
        set
        {
            this.FightlRotation = value.Set();
        }
    }

    [ProtoMember(10)]
    public ProtoVector3 P_FightScale
    {
        get
        {
            return ProtoVector3.Get(this.FightScale);
        }
        set
        {
            this.FightScale = value.Set();
        }
    }

    [ProtoMember(127)]
    [NonSerialized]
    public WeaponArtConfig[] ProtoList;

    [ProtoMember(1)]
    public string WeaponName = string.Empty;

    [ProtoMember(2)]
    public string WeaponModel = string.Empty;

    [ProtoMember(3)]
    public string NormalBindPointName;

    public Vector3 NormalPosition;

    public Vector3 NormalRotation;

    public Vector3 NormalScale;

    [ProtoMember(7)]
    public string FightBindPointName;

    public Vector3 FightPosition;

    public Vector3 FightlRotation;

    public Vector3 FightScale;

    [ProtoMember(11)]
    public string[] WeaponEffects = new string[0];
}
