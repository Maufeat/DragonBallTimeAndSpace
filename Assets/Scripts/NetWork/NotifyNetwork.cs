using System;
using Framework.Managers;
using Net;
using quest;

public class NotifyNetwork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_NotifyClientOptional_SC>(CommandID.MSG_NotifyClientOptional_SC, new ProtoMsgCallback<MSG_NotifyClientOptional_SC>(this.OnMSG_NotifyClientOptional_SC));
    }

    private void OnMSG_NotifyClientOptional_SC(MSG_NotifyClientOptional_SC msg)
    {
        NotifyController nc = ControllerManager.Instance.GetController<NotifyController>();
        if (nc != null)
        {
            Scheduler.Instance.AddTimer(0.5f, false, delegate
            {
                nc.OnNotify(msg.type, msg.setting);
                FFDebug.LogWarning(this, msg.type + ":" + msg.setting);
            });
        }
    }
}
