using System;
using Framework.Managers;
using msg;
using Net;

public class QueueNetWorker : NetWorkBase
{
    private QueueController queueController
    {
        get
        {
            return ControllerManager.Instance.GetController<QueueController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_QueueInfo_SC>(CommandID.MSG_Ret_QueueInfo_SC, new ProtoMsgCallback<MSG_Ret_QueueInfo_SC>(this.RetQueue));
    }

    public override void Uninitialize()
    {
        base.Uninitialize();
    }

    public void ReqQueue()
    {
        ControllerManager.Instance.GetLoginController().Login();
    }

    public void RetQueue(MSG_Ret_QueueInfo_SC data)
    {
        this.queueController.RetQueueData((int)data.queue_user_num, (int)data.queue_wait_time);
    }
}
