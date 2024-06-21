using System;
using Framework.Managers;
using massive;
using Net;

public class PlayerVisitNetWork : NetWorkBase
{
    private VisitPlayerController visitPlayerController
    {
        get
        {
            return ControllerManager.Instance.GetController<VisitPlayerController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_HoldonUser_SC>(2253, new ProtoMsgCallback<MSG_Ret_HoldonUser_SC>(this.Ret_HoldonUser_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_HoldonUser_Interrupt_SC>(2254, new ProtoMsgCallback<MSG_Ret_HoldonUser_Interrupt_SC>(this.Ret_HoldonUser_Interrupt_SC));
    }

    public void Req_HoldonUser_CS(ulong thisid, bool BeginorEnd)
    {
        base.SendMsg<MSG_Req_HoldonUser_CS>(CommandID.MSG_Req_HoldonUser_CS, new MSG_Req_HoldonUser_CS
        {
            thisid = thisid,
            type = ((!BeginorEnd) ? 2U : 1U)
        }, false);
    }

    public void Ret_HoldonUser_SC(MSG_Ret_HoldonUser_SC data)
    {
        if (data.type == 1U)
        {
            if (data.retcode == 1U)
            {
                return;
            }
            if (data.retcode == 0U)
            {
                this.visitPlayerController.OnVisitBegin(data);
            }
        }
        else
        {
            this.visitPlayerController.OnVisitComplete(data.retcode);
        }
    }

    public void Ret_HoldonUser_Interrupt_SC(MSG_Ret_HoldonUser_Interrupt_SC data)
    {
        this.visitPlayerController.BreakVisitn(data.thisid);
    }
}
