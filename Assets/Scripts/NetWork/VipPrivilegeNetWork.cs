using System;
using Framework.Managers;
using Net;
using Obj;

public class VipPrivilegeNetWork : NetWorkBase
{
    private VipPrivilegeController vpc
    {
        get
        {
            return ControllerManager.Instance.GetController<VipPrivilegeController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetGetVIPCardInfo_SC>(CommandID.MSG_RetGetVIPCardInfo_SC, new ProtoMsgCallback<MSG_RetGetVIPCardInfo_SC>(this.MSG_RetGetVIPCardInfo_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetBuyVIPCard_SC>(CommandID.MSG_RetBuyVIPCard_SC, new ProtoMsgCallback<MSG_RetBuyVIPCard_SC>(this.MSG_RetBuyVIPCard_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetAcepVIPCardPrize_SC>(CommandID.MSG_RetAcepVIPCardPrize_SC, new ProtoMsgCallback<MSG_RetAcepVIPCardPrize_SC>(this.MSG_RetAcepVIPCardPrize_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetRaffVIPCardPrize_SC>(CommandID.MSG_RetRaffVIPCardPrize_SC, new ProtoMsgCallback<MSG_RetRaffVIPCardPrize_SC>(this.MSG_RetRaffVIPCardPrize_SC));
    }

    public void ReqGetVIPCardInfo_CS()
    {
        base.SendMsg<MSG_ReqGetVIPCardInfo_CS>(CommandID.MSG_ReqGetVIPCardInfo_CS, new MSG_ReqGetVIPCardInfo_CS(), false);
    }

    private void MSG_RetGetVIPCardInfo_SC(MSG_RetGetVIPCardInfo_SC msg)
    {
        this.vpc.RetGetVIPCardInfo_SC(msg);
    }

    public void ReqBuyVIPCard_CS(uint cardID, uint count)
    {
        base.SendMsg<MSG_ReqBuyVIPCard_CS>(CommandID.MSG_ReqBuyVIPCard_CS, new MSG_ReqBuyVIPCard_CS
        {
            cardid = cardID,
            count = count
        }, false);
    }

    private void MSG_RetBuyVIPCard_SC(MSG_RetBuyVIPCard_SC msg)
    {
        this.vpc.RetBuyVIPCard_SC(msg);
    }

    public void ReqAcepVIPCardPrize_CS()
    {
        base.SendMsg<MSG_ReqAcepVIPCardPrize_CS>(CommandID.MSG_ReqAcepVIPCardPrize_CS, new MSG_ReqAcepVIPCardPrize_CS(), false);
    }

    private void MSG_RetAcepVIPCardPrize_SC(MSG_RetAcepVIPCardPrize_SC msg)
    {
        this.vpc.RetAcepVIPCardPrize_SC(msg);
    }

    public void ReqRaffVIPCardPrize_CS(RaffUseType usetype)
    {
        base.SendMsg<MSG_ReqRaffVIPCardPrize_CS>(CommandID.MSG_ReqRaffVIPCardPrize_CS, new MSG_ReqRaffVIPCardPrize_CS
        {
            usetype = usetype
        }, false);
    }

    private void MSG_RetRaffVIPCardPrize_SC(MSG_RetRaffVIPCardPrize_SC msg)
    {
        this.vpc.RetRaffVIPCardPrize_SC(msg);
    }
}
