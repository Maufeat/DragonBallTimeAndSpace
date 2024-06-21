public struct FirstCopyInfo
{
    public void Clear()
    {
        this.id = 0U;
        this.isEmpty = true;
        this.state = 0U;
    }

    public uint id;

    public bool isEmpty;

    public uint state;
}
