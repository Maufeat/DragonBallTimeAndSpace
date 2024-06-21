using System;
using Framework.Managers;
using Net;
using quest;

public class UIEffectNetWork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_notifyQuestStateEffect_SC>(2534, new ProtoMsgCallback<MSG_notifyQuestStateEffect_SC>(this.MSG_notifyQuestStateEffect_SC));
    }

    public void MSG_notifyQuestStateEffect_SC(MSG_notifyQuestStateEffect_SC msg)
    {
        UIEffectController controller = ControllerManager.Instance.GetController<UIEffectController>();
        if (controller != null)
        {
            controller.ShowTaskEffect(msg.questid, msg.state);
        }
    }
}
