using System;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
[Serializable]
public class BindPoint
{
    [ProtoMember(3)]
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

    [ProtoMember(4)]
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

    [ProtoMember(5)]
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

    public void SavePos()
    {
        if (this.Tran == null)
        {
            return;
        }
        this.Path = this.GetPath();
        this.Position = this.Tran.localPosition;
        this.Rotation = this.Tran.localEulerAngles;
        this.Scale = this.Tran.localScale;
    }

    public void ReadPos()
    {
        if (this.Tran == null)
        {
            return;
        }
        this.Tran.localPosition = this.Position;
        this.Tran.localEulerAngles = this.Rotation;
        this.Tran.localScale = this.Scale;
    }

    private string GetPath()
    {
        if (this.Tran == null)
        {
            return string.Empty;
        }
        Transform transform = this.Tran;
        string text = string.Empty;
        while (transform.GetComponent<Animator>() == null)
        {
            if (transform == this.Tran)
            {
                text = string.Empty;
            }
            else
            {
                text = transform.name + "/" + text;
            }
            transform = transform.parent;
            if (transform == null)
            {
                return string.Empty;
            }
        }
        if (text.EndsWith("/"))
        {
            text.Substring(0, text.Length - 1);
        }
        return text;
    }

    [ProtoMember(1)]
    public string Name = string.Empty;

    [ProtoMember(2)]
    public string Path = string.Empty;

    public Vector3 Position;

    public Vector3 Rotation;

    public Vector3 Scale;

    [ProtoMember(6)]
    public string FollowTarget = string.Empty;

    [ProtoMember(7)]
    public float FollowPosY;

    [ProtoMember(8)]
    public float FollowPosX;

    [ProtoMember(9)]
    public float FollowPosZ;

    [NonSerialized]
    public Transform Tran;
}
