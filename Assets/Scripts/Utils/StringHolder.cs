using System;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class StringHolder : ScriptableObject
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
    public StringHolder[] ProtoList;

    [ProtoMember(1)]
    public string[] content = new string[0];
}
