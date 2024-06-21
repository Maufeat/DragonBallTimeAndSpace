using System;
using Framework.Managers;
using hero;
using Net;

public class GeneNetWork : NetWorkBase
{
    private GeneController gc
    {
        get
        {
            return ControllerManager.Instance.GetController<GeneController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_DnaBagInfo_CSC>(CommandID.MSG_DnaBagInfo_CSC, new ProtoMsgCallback<MSG_DnaBagInfo_CSC>(this.OnRetMSG_DnaBagInfo_CSC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_DnaPageInfo_CSC>(CommandID.MSG_DnaPageInfo_CSC, new ProtoMsgCallback<MSG_DnaPageInfo_CSC>(this.OnRetMSG_DnaPageInfo_CSC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_DnaPageInfo_CSC>(CommandID.MSG_ReqPutOnDna_CS, new ProtoMsgCallback<MSG_DnaPageInfo_CSC>(this.OnRetMSG_DnaPageInfo_CSC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_DnaPageInfo_CSC>(CommandID.MSG_ReqPutOffDna_CS, new ProtoMsgCallback<MSG_DnaPageInfo_CSC>(this.OnRetMSG_DnaPageInfo_CSC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ReqChangeCurDnaPage_CS>(CommandID.MSG_ReqChangeCurDnaPage_CS, new ProtoMsgCallback<MSG_ReqChangeCurDnaPage_CS>(this.OnRetMSG_ReqChangeCurDnaPage_CS));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ReqCombineDnaInBag_CS>(CommandID.MSG_ReqCombineDnaInBag_CS, new ProtoMsgCallback<MSG_ReqCombineDnaInBag_CS>(this.OnRtMSG_ReqCombineDnaInBag_CS));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ReqCombineDna_CS>(CommandID.MSG_ReqCombineDna_CS, new ProtoMsgCallback<MSG_ReqCombineDna_CS>(this.OnRtMSG_MSG_ReqCombineDna_CS));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_AllDnaPageInfo_CSC>(CommandID.MSG_AllDnaPageInfo_CSC, new ProtoMsgCallback<MSG_AllDnaPageInfo_CSC>(this.OnRetMSG_AllDnaPageInfo_CSC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ResponseSlotOpened_SC>(CommandID.MSG_ResponseSlotOpened_SC, new ProtoMsgCallback<MSG_ResponseSlotOpened_SC>(this.OnMSG_ResponseSlotOpened_SC));
    }

    public void ReqGeneBagInfo()
    {
        MSG_DnaBagInfo_CSC t = new MSG_DnaBagInfo_CSC();
        base.SendMsg<MSG_DnaBagInfo_CSC>(CommandID.MSG_DnaBagInfo_CSC, t, false);
    }

    public void OnMSG_ResponseSlotOpened_SC(MSG_ResponseSlotOpened_SC msg)
    {
        this.gc.OnMSG_ResponseSlotOpened_SC(msg);
    }

    public void ReqGeneDnaPageInfo(DNAPage page)
    {
        base.SendMsg<MSG_DnaPageInfo_CSC>(CommandID.MSG_DnaPageInfo_CSC, new MSG_DnaPageInfo_CSC
        {
            page = page
        }, false);
    }

    public void ReqMSG_ReqPutOnDna_CS(DNAPage page, uint pos, uint id, uint level, DNASlotType dst)
    {
        base.SendMsg<MSG_ReqPutOnDna_CS>(CommandID.MSG_ReqPutOnDna_CS, new MSG_ReqPutOnDna_CS
        {
            page = page,
            pos = pos,
            id = id,
            level = level,
            type = dst
        }, false);
    }

    public void ReqMSG_MSG_ReqPutOffDna_CS(uint pos, DNASlotType dst)
    {
        DNAPage curDnaPageType = this.gc.GetCurDnaPageType();
        base.SendMsg<MSG_ReqPutOffDna_CS>(CommandID.MSG_ReqPutOffDna_CS, new MSG_ReqPutOffDna_CS
        {
            page = curDnaPageType,
            pos = pos,
            type = dst
        }, false);
    }

    public void ReqChangeCurDnaPage(DNAPage page)
    {
        base.SendMsg<MSG_ReqChangeCurDnaPage_CS>(CommandID.MSG_ReqChangeCurDnaPage_CS, new MSG_ReqChangeCurDnaPage_CS
        {
            page = page
        }, false);
    }

    public void ReqFuseGeneInPage(DNAPage page, uint pos, DNASlotType dst, uint num)
    {
        base.SendMsg<MSG_ReqCombineDna_CS>(CommandID.MSG_ReqCombineDna_CS, new MSG_ReqCombineDna_CS
        {
            page = page,
            pos = pos,
            type = dst,
            num = num
        }, false);
    }

    public void ReqFuseGeneInBag(uint id, uint level, uint num)
    {
        base.SendMsg<MSG_ReqCombineDnaInBag_CS>(CommandID.MSG_ReqCombineDnaInBag_CS, new MSG_ReqCombineDnaInBag_CS
        {
            id = id,
            level = level,
            num = num
        }, false);
    }

    public void ReqMSG_AllDnaPageInfo_CSC()
    {
        MSG_AllDnaPageInfo_CSC t = new MSG_AllDnaPageInfo_CSC();
        base.SendMsg<MSG_AllDnaPageInfo_CSC>(CommandID.MSG_AllDnaPageInfo_CSC, t, false);
    }

    public void ReqMSG_ReqBuySlot_SC(DNAPage page, DNASlotType type)
    {
        base.SendMsg<MSG_ReqBuySlot_SC>(CommandID.MSG_ReqBuySlot_SC, new MSG_ReqBuySlot_SC
        {
            page = page,
            type = type
        }, false);
    }

    public void OnRetMSG_ReqChangeCurDnaPage_CS(MSG_ReqChangeCurDnaPage_CS msg)
    {
        this.gc.RetMSG_ReqChangeCurDnaPage_CS(msg);
        this.ReqGeneDnaPageInfo(msg.page);
    }

    public void OnRetMSG_DnaBagInfo_CSC(MSG_DnaBagInfo_CSC msg)
    {
        this.gc.OnGetAllDnaItemData(msg);
    }

    public void OnRetMSG_DnaPageInfo_CSC(MSG_DnaPageInfo_CSC msg)
    {
        this.gc.OnGetDnaPageData(msg);
        this.gc.SetActionState(false);
    }

    public void OnRtMSG_ReqCombineDnaInBag_CS(MSG_ReqCombineDnaInBag_CS msg)
    {
        this.gc.OnRtMSG_ReqCombineDnaInBag_CS(msg);
    }

    private void OnRtMSG_MSG_ReqCombineDna_CS(MSG_ReqCombineDna_CS MsgData)
    {
        this.gc.OnRtMSG_MSG_ReqCombineDna_CS(MsgData);
    }

    public void OnRetMSG_AllDnaPageInfo_CSC(MSG_AllDnaPageInfo_CSC msg)
    {
        this.gc.OnGetAllDnaPageInfo(msg);
    }
}
