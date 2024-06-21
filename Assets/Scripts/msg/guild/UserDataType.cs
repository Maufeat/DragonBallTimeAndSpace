using System;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "UserDataType")]
    public enum UserDataType
    {
        [ProtoEnum(Name = "GUILD_DAILY_COUNTRIBUTE", Value = 1)]
        GUILD_DAILY_COUNTRIBUTE = 1
    }
}
