namespace Net
{
    public delegate void StructMsgCallback<T>(T MsgData) where T : StructCmd;
}
