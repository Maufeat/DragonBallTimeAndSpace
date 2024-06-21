using System;
using ProtoBuf;

[ProtoContract]
[Serializable]
public class FFActionClipEffects
{
    [ProtoMember(1)]
    public uint group;

    [ProtoMember(2)]
    public string[] effects = new string[0];
}
