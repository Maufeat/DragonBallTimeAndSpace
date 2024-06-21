using System;

public class NextTaskInfo
{
    public override string ToString()
    {
        return string.Format("NextTaskInfo: npcID = {0}; nextSwitchID = {1}; strNextTarget = {2}; strIcon = {3}; strInfo= {4};nextOffset = {5}; buffAnim = {6};revertAnim = {7}", new object[]
        {
            this.npcID,
            this.nextSwitchID,
            this.strNextTarget,
            this.strIcon,
            this.strInfo,
            this.nextOffet,
            this.buffAnim,
            this.revertAnim
        });
    }

    public int npcID = -1;

    public int nextSwitchID = -1;

    public string strNextTarget = string.Empty;

    public string strIcon = string.Empty;

    public string strInfo = string.Empty;

    public int nextOffet = -1;

    public int buffAnim = -1;

    public int revertAnim = -1;
}
