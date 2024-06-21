namespace Net
{
    public enum BlockingState
    {
        Sendable,
        Waiting,
        TimeOut,
        Exception,
        Close,
    }
}
