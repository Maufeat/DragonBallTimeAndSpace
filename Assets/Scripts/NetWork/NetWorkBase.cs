
using Net;
using ProtoBuf;

public class NetWorkBase
{
    public virtual void Initialize()
    {
        this.RegisterMsg();
    }

    public virtual void RegisterMsg()
    {
    }

    public virtual void UnRegisterMsg()
    {
    }

    public void SendMsg<T>(CommandID id, T t, bool istoself = false) where T : IExtensible
    {
        LSingleton<NetWorkModule>.Instance.Send<T>(id, t, istoself);
    }

    public void SendMsg(StructCmd cmd, bool istoself = false)
    {
        LSingleton<NetWorkModule>.Instance.Send(cmd, istoself);
    }

    public void SendMsg(ushort id, byte[] data)
    {
        LSingleton<NetWorkModule>.Instance.Send(id, data);
    }

    public virtual void Uninitialize()
    {
        this.UnRegisterMsg();
    }
}
