using System;
using guild;

public class TimeUpCompare : GuildCompare
{
    public override int Compare(guildMember x, guildMember y)
    {
        return x.lastonlinetime.CompareTo(y.lastonlinetime);
    }
}
