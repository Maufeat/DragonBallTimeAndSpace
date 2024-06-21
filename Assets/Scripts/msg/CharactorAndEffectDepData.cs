using ProtoBuf;
using System.Collections.Generic;

[ProtoContract]
public class CharactorAndEffectDepData
{
    [ProtoMember(1)]
    public List<string> lstDepData;
}
