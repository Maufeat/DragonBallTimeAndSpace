using System;
using System.Collections.Generic;
using Subtitle;

public class SubtitleContentList : IComparer<SubtitleContent>
{
    public int Compare(SubtitleContent a, SubtitleContent b)
    {
        if (a.showTime < b.showTime)
        {
            return -1;
        }
        return 1;
    }
}
