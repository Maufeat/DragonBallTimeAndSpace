using System;
using ProtoBuf;

[ProtoContract]
[Serializable]
public class ModelBody
{
    [ProtoMember(1)]
    public string name;

    [ProtoMember(2)]
    public string[] rendererobjects = new string[0];
}
