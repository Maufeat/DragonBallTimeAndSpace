using System;
using Framework.Managers;
using Net;
using quiz;

public class SevenDaysNetWork : NetWorkBase
{
    private SevenDaysController controller
    {
        get
        {
            return ControllerManager.Instance.GetController<SevenDaysController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_Day7ActivityInfo_SC>(CommandID.MSG_Ret_Day7ActivityInfo_SC, new ProtoMsgCallback<MSG_Ret_Day7ActivityInfo_SC>(this.Ret_Day7ActivityInfo_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_SeekActivityInfo_SC>(CommandID.MSG_Ret_SeekActivityInfo_SC, new ProtoMsgCallback<MSG_Ret_SeekActivityInfo_SC>(this.Ret_SeekActivityInfo_SC));
    }

    public override void UnRegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.DeRegisterMsg(2569);
        LSingleton<NetWorkModule>.Instance.DeRegisterMsg(2571);
    }

    public void Req_RecvDay7ActivityPrize_CS()
    {
        MSG_Req_RecvDay7ActivityPrize_CS t = new MSG_Req_RecvDay7ActivityPrize_CS();
        base.SendMsg<MSG_Req_RecvDay7ActivityPrize_CS>(CommandID.MSG_Req_RecvDay7ActivityPrize_CS, t, false);
    }

    public void Req_RecvSeekActivityPrize_CS(uint id)
    {
        base.SendMsg<MSG_Req_RecvSeekActivityPrize_CS>(CommandID.MSG_Req_RecvSeekActivityPrize_CS, new MSG_Req_RecvSeekActivityPrize_CS
        {
            id = id
        }, false);
    }

    private void Ret_Day7ActivityInfo_SC(MSG_Ret_Day7ActivityInfo_SC msg)
    {
        this.controller.Ret_Day7ActivityInfo_SC(msg);
    }

    private void Ret_SeekActivityInfo_SC(MSG_Ret_SeekActivityInfo_SC msg)
    {
        this.controller.Ret_SeekActivityInfo_SC(msg);
    }
}
