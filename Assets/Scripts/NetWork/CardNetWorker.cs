using System;
using Framework.Managers;
using Net;
using Obj;

public class CardNetWorker : NetWorkBase
{
    private CardController cardController
    {
        get
        {
            return ControllerManager.Instance.GetController<CardController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetCardPackInfo_SC>(CommandID.MSG_RetCardPackInfo_SC, new ProtoMsgCallback<MSG_RetCardPackInfo_SC>(this.OnRetCardPackInfo));
    }

    public void ReqCardPackInfo()
    {
        base.SendMsg<MSG_ReqCardPackInfo_CS>(CommandID.MSG_ReqCardPackInfo_CS, new MSG_ReqCardPackInfo_CS(), false);
    }

    private void OnRetCardPackInfo(MSG_RetCardPackInfo_SC data)
    {
        CardPackInfo data2 = data.data;
        this.cardController.SetupCardPackInfo(data2);
    }

    public void ReqPutOnCard(PackType packtype, string thisid, uint grid_y)
    {
        base.SendMsg<MSG_ReqPutOnCard_CS>(CommandID.MSG_ReqPutOnCard_CS, new MSG_ReqPutOnCard_CS
        {
            thisid = thisid,
            grid_x = 0U,
            grid_y = grid_y,
            packtype = packtype
        }, false);
    }

    public void ReqPutOffCard(string thisid, int grid_x, int grid_y)
    {
        base.SendMsg<MSG_ReqPutOffCard_CS>(CommandID.MSG_ReqPutOffCard_CS, new MSG_ReqPutOffCard_CS
        {
            thisid = thisid,
            grid_x = (uint)grid_x,
            grid_y = (uint)grid_y
        }, false);
    }

    public void ReqSwapCard(string src_thisid, string dst_thisid)
    {
        base.SendMsg<MSG_ReqSwapCard_CS>(CommandID.MSG_ReqSwapCard_CS, new MSG_ReqSwapCard_CS
        {
            src_thisid = src_thisid,
            dst_thisid = dst_thisid
        }, false);
    }

    public override void Uninitialize()
    {
        base.Uninitialize();
    }
}
