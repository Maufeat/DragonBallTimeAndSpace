using System;
using Framework.Managers;
using massive;
using Net;

public class ComicNetWork : NetWorkBase
{
    private ComicController comicController
    {
        get
        {
            return ControllerManager.Instance.GetController<ComicController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetStartAIRunning_SC>(2240, new ProtoMsgCallback<MSG_RetStartAIRunning_SC>(this.OnStartAIRunning));
    }

    public void ReqStartAIRunning()
    {
        base.SendMsg<MSG_ReqStartAIRunning_CS>(CommandID.MSG_ReqStartAIRunning_CS, new MSG_ReqStartAIRunning_CS(), false);
    }

    private void OnStartAIRunning(MSG_RetStartAIRunning_SC data)
    {
    }
}
