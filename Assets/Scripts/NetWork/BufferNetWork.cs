using System;
using Framework.Managers;
using msg;
using Net;

public class BufferNetWork : NetWorkBase
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void RegisterMsg()
    {
        base.RegisterMsg();
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_SetState_SC>(2284, new ProtoMsgCallback<MSG_Ret_SetState_SC>(this.HandleSetBufferState));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_ClearState_SC>(2285, new ProtoMsgCallback<MSG_Ret_ClearState_SC>(this.HandleClearBufferState));
    }

    private void HandleSetBufferState(MSG_Ret_SetState_SC data)
    {
        ManagerCenter.Instance.GetManager<BufferStateManager>().HandleSetBufferState(data);
    }

    private void HandleClearBufferState(MSG_Ret_ClearState_SC data)
    {
        ManagerCenter.Instance.GetManager<BufferStateManager>().HandleClearBufferState(data);
    }
}
