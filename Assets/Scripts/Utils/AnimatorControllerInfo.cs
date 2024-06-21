using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class AnimatorControllerInfo : ScriptableObject
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

    public void FillMap()
    {
        if (this.cliplist == null)
        {
            return;
        }
        this.ClipMap = new Dictionary<string, AnimatorControllerInfo.Clip>();
        for (int i = 0; i < this.cliplist.Length; i++)
        {
            AnimatorControllerInfo.Clip clip = this.cliplist[i];
            this.ClipMap[clip.Name] = clip;
        }
    }

    [ProtoMember(127)]
    [NonSerialized]
    public AnimatorControllerInfo[] ProtoList;

    [ProtoMember(1)]
    public AnimatorControllerInfo.Clip[] cliplist = new AnimatorControllerInfo.Clip[0];

    [NonSerialized]
    public Dictionary<string, AnimatorControllerInfo.Clip> ClipMap;

    [ProtoContract]
    [Serializable]
    public class Clip
    {
        [ProtoMember(1)]
        public string Name;

        [ProtoMember(2)]
        public float Length;
    }
}
