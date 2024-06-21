using System;
using Framework.Managers;
using msg;
using Net;
using UI.Exchange;

public class ExchangeGemNetWork : NetWorkBase
{
    public override void Initialize()
    {
        this.controller = ControllerManager.Instance.GetController<ExchangeGemController>();
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetQueryBalance_SC>(2522, new ProtoMsgCallback<MSG_RetQueryBalance_SC>(this.OnQueryBalace));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetRecharge_SC>(2523, new ProtoMsgCallback<MSG_RetRecharge_SC>(this.OnRecharge));
    }

    public override void UnRegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.DeRegisterMsg(2522);
        LSingleton<NetWorkModule>.Instance.DeRegisterMsg(2523);
    }

    public void ReqRecharge(uint point)
    {
        base.SendMsg<MSG_ReqRecharge_CS>(CommandID.MSG_ReqRecharge_CS, new MSG_ReqRecharge_CS
        {
            point = point,
            type = 0U
        }, false);
    }

    public void ReqQueryBalance()
    {
        MSG_ReqQueryBalance_CS t = new MSG_ReqQueryBalance_CS();
        base.SendMsg<MSG_ReqQueryBalance_CS>(CommandID.MSG_ReqQueryBalance_CS, t, false);
    }

    private void OnQueryBalace(MSG_RetQueryBalance_SC data)
    {
        this.controller.UpdatePoint(data.balance);
        this.controller.UpdateRate(data.point2tone);
    }

    private void OnRecharge(MSG_RetRecharge_SC data)
    {
        this.controller.UpdatePoint(data.balance);
    }

    private ExchangeGemController controller;
}
