using System;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class EffectGroup : ScriptableObject
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

    [ProtoMember(127)]
    public EffectGroup[] ProtoList;

    [ProtoMember(1)]
    public string GroupName;

    [ProtoMember(2)]
    public string[] Groups = new string[0];

    [ProtoMember(3)]
    public string[] MaterialEffects = new string[0];
}
