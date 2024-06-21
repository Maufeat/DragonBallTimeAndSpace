using System;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class NavMeshData
{
    [ProtoMember(1)]
    public List<NaveMeshDataPos> pos;

    [ProtoMember(2)]
    public List<NaveMeshDataPolys> polys;
}
