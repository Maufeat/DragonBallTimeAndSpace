using System;
using ProtoBuf;

[ProtoContract]
public class NaveMeshDataPolys
{
    [ProtoMember(1)]
    public int[] poly;
}
