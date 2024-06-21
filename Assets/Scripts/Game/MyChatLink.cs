using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class MyChatLink
{
    [ProtoMember(1)]
    public string linktype { get; set; }

    [ProtoMember(2)]
    public List<string> data_args { get; set; }
}
