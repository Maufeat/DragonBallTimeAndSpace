using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class FriendPrivateChatModel
{
    [ProtoMember(1)]
    public List<MyChatData> chatDataList { get; set; }
}
