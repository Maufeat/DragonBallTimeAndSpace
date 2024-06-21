using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class AudioConfig : ScriptableObject
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

    public List<WwiseAudioClip> ClipList
    {
        get
        {
            if (this._clipList == null)
            {
                if (this.Clips == null)
                {
                    this.Clips = new WwiseAudioClip[0];
                }
                this._clipList = new List<WwiseAudioClip>(this.Clips);
            }
            return this._clipList;
        }
    }

    public void AddAudioClip(WwiseAudioClip clip)
    {
        this.ClipList.Add(clip);
        this.Clips = this.ClipList.ToArray();
    }

    public void RemoveAudioClip(WwiseAudioClip clip)
    {
        if (this.ClipList.Contains(clip))
        {
            this.ClipList.Remove(clip);
            this.Clips = this.ClipList.ToArray();
        }
    }

    [ProtoMember(127)]
    [NonSerialized]
    public AudioConfig[] ProtoList;

    [ProtoMember(1)]
    public AudioConfig.BindType BidType;

    [ProtoMember(2)]
    public string BindName;

    [ProtoMember(3)]
    public WwiseAudioClip[] Clips = new WwiseAudioClip[0];

    [NonSerialized]
    private List<WwiseAudioClip> _clipList;

    public enum BindType
    {
        Action,
        ActionHit,
        Effect,
        EffectGroup,
        FlyObj
    }
}
