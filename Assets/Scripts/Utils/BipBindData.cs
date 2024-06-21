using System;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class BipBindData : ScriptableObject
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
    [NonSerialized]
    public BipBindData[] ProtoList;

    [ProtoMember(1)]
    public string ModelName;

    [ProtoMember(2)]
    public BindPoint[] BindPointList = new BindPoint[0];
}
