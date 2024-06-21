using System;
using Framework.Managers;
using Net;
using quest;

public class QTENetWork : NetWorkBase
{
    private QTEController controller
    {
        get
        {
            if (this._controller == null)
            {
                this._controller = ControllerManager.Instance.GetController<QTEController>();
            }
            return this._controller;
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_PlayBellQTE_SC>(CommandID.MSG_PlayBellQTE_SC, new ProtoMsgCallback<MSG_PlayBellQTE_SC>(this.OnGet_MSG_PlayBellQTE_SC));
    }

    private void OnGet_MSG_PlayBellQTE_SC(MSG_PlayBellQTE_SC msg)
    {
        this.controller.OnGetMSG_PlayBellQTE_SC(msg);
    }

    public void ReqMSG_PlayBellQTEResult_CS(uint qteLv, uint result)
    {
        base.SendMsg<MSG_PlayBellQTEResult_CS>(CommandID.MSG_PlayBellQTEResult_CS, new MSG_PlayBellQTEResult_CS
        {
            qtelevel = qteLv,
            result = result
        }, false);
    }

    private QTEController _controller;
}
