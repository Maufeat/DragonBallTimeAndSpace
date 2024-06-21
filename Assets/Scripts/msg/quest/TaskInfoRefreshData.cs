using System;

public struct TaskInfoRefreshData
{
    public void Clear()
    {
        this.questid = 0U;
        this.state = 0U;
    }

    public uint questid;

    public uint state;
}
