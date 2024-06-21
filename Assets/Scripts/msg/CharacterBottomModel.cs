using System;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class CharacterBottomModel
{
    [ProtoMember(1)]
    public List<int> idList;
}
