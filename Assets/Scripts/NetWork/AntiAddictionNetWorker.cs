using System;
using Framework.Managers;
using msg;
using Net;

public class AntiAddictionNetWorker : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_NotifyAntiAddict_SC>(CommandID.MSG_NotifyAntiAddict_SC, new ProtoMsgCallback<MSG_NotifyAntiAddict_SC>(this.OnRetNotifyAntiAddict));
        base.RegisterMsg();
    }

    private void OnRetNotifyAntiAddict(MSG_NotifyAntiAddict_SC data)
    {
        ManagerCenter.Instance.GetManager<AntiAddictionManager>().NotifyAntiAddictCb(data.isAntiAddcit, (int)data.onlinelasttime, data.isLoginPush);
    }
}
