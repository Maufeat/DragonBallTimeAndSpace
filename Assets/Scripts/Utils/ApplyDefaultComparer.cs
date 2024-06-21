using System;
using guild;

public class ApplyDefaultComparer : GuildCompare
{
    public override int Compare(guildMember x, guildMember y)
    {
        int num = (!x.isonline) ? 0 : 1;
        int num2 = (!y.isonline) ? 0 : 1;
        if (num != num2)
        {
            return num2.CompareTo(num);
        }
        return y.applytime.CompareTo(x.applytime);
    }
}
