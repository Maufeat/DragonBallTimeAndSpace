using ProtoBuf;

[ProtoContract]
public class NaveMeshDataPos
{
    [ProtoMember(1)]
    public float x;

    [ProtoMember(2)]
    public float y;

    [ProtoMember(3)]
    public float z;
}
