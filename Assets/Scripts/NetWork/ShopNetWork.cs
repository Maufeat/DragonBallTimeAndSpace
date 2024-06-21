using System;
using System.Collections.Generic;
using Framework.Managers;
using market;
using msg;
using Net;
using trade;

public class ShopNetWork : NetWorkBase
{
    private ShopController controller
    {
        get
        {
            if (this._controller == null)
            {
                this._controller = ControllerManager.Instance.GetController<ShopController>();
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
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetMarketItemList_SC>(CommandID.MSG_RetMarketItemList_SC, new ProtoMsgCallback<MSG_RetMarketItemList_SC>(this.RetMarketItemList_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetBuyMarketItem_SC>(CommandID.MSG_RetBuyMarketItem_SC, new ProtoMsgCallback<MSG_RetBuyMarketItem_SC>(this.RetBuyMarketItem_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetSellingStaff_SC>(CommandID.MSG_RetSellingStaff_SC, new ProtoMsgCallback<MSG_RetSellingStaff_SC>(this.RetSellingStaff_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetRecommandPrice_SC>(CommandID.MSG_RetRecommandPrice_SC, new ProtoMsgCallback<MSG_RetRecommandPrice_SC>(this.RetRecommandPrice_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetSellStaff_SC>(CommandID.MSG_RetSellStaff_SC, new ProtoMsgCallback<MSG_RetSellStaff_SC>(this.RetSellStaff_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetStopSellStaff_SC>(CommandID.MSG_RetStopSellStaff_SC, new ProtoMsgCallback<MSG_RetStopSellStaff_SC>(this.RetStopSellStaff_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetSubSellingList_SC>(CommandID.MSG_RetSubSellingList_SC, new ProtoMsgCallback<MSG_RetSubSellingList_SC>(this.RetSubSellingList_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetBuyItem_SC>(CommandID.MSG_RetBuyItem_SC, new ProtoMsgCallback<MSG_RetBuyItem_SC>(this.RetBuyItem_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetTradeItemHistory_SC>(CommandID.MSG_RetTradeItemHistory_SC, new ProtoMsgCallback<MSG_RetTradeItemHistory_SC>(this.RetTradeItemHistory_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetSublistPage_SC>(CommandID.MSG_RetSublistPage_SC, new ProtoMsgCallback<MSG_RetSublistPage_SC>(this.RetSublistPage_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetUserTradeHistory_SC>(CommandID.MSG_RetUserTradeHistory_SC, new ProtoMsgCallback<MSG_RetUserTradeHistory_SC>(this.RetUserTradeHistory_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_UserSelledItemList_CSC>(CommandID.MSG_UserSelledItemList_CSC, new ProtoMsgCallback<MSG_UserSelledItemList_CSC>(this.UserSelledItemList_CSC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetBuySelledItem_SC>(CommandID.MSG_RetBuySelledItem_SC, new ProtoMsgCallback<MSG_RetBuySelledItem_SC>(this.RetBuySelledItem_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetSellItem_SC>(CommandID.MSG_RetSellItem_SC, new ProtoMsgCallback<MSG_RetSellItem_SC>(this.RetSellItem_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetGetNewestStaff_SC>(CommandID.MSG_ReqGetNewestStaff_CS, new ProtoMsgCallback<MSG_RetGetNewestStaff_SC>(this.RetGetNewestStaff_SC));
    }

    public void OnReqMarketItemList(List<uint> lstShopId)
    {
        MSG_ReqMarketItemList_CS msg_ReqMarketItemList_CS = new MSG_ReqMarketItemList_CS();
        for (int i = 0; i < lstShopId.Count; i++)
        {
            msg_ReqMarketItemList_CS.marketid.Add(lstShopId[i]);
        }
        base.SendMsg<MSG_ReqMarketItemList_CS>(CommandID.MSG_ReqMarketItemList_CS, msg_ReqMarketItemList_CS, false);
    }

    public void OnReqBuyMarketItem(uint shopid, uint id, uint itemid, uint num)
    {
        base.SendMsg<MSG_ReqBuyMarketItem_CS>(CommandID.MSG_ReqBuyMarketItem_CS, new MSG_ReqBuyMarketItem_CS
        {
            marketid = shopid,
            id = id,
            itemid = itemid,
            itemnum = num
        }, false);
    }

    public void MSG_ReqSellingStaff_CS()
    {
        MSG_ReqSellingStaff_CS t = new MSG_ReqSellingStaff_CS();
        base.SendMsg<MSG_ReqSellingStaff_CS>(CommandID.MSG_ReqSellingStaff_CS, t, false);
    }

    public void MSG_ReqRecommandPrice_CS(SELLTYPE itemType, uint baseId)
    {
        base.SendMsg<MSG_ReqRecommandPrice_CS>(CommandID.MSG_ReqRecommandPrice_CS, new MSG_ReqRecommandPrice_CS
        {
            itemtype = itemType,
            baseid = baseId
        }, false);
    }

    public void MSG_ReqSellStaff_CS(SELLTYPE itemType, string thisId, uint baseId, uint price, uint num)
    {
        base.SendMsg<MSG_ReqSellStaff_CS>(CommandID.MSG_ReqSellStaff_CS, new MSG_ReqSellStaff_CS
        {
            itemtype = itemType,
            thisid = thisId,
            baseid = baseId,
            price = price,
            num = num
        }, false);
    }

    public void MSG_ReqStopSellStaff_CS(string thisId)
    {
        base.SendMsg<MSG_ReqStopSellStaff_CS>(CommandID.MSG_ReqStopSellStaff_CS, new MSG_ReqStopSellStaff_CS
        {
            thisid = thisId
        }, false);
    }

    public void MSG_ReqSubSellingList_CS(SELLTYPE itemType, List<uint> idList, uint levelStar, bool checkShow)
    {
        MSG_ReqSubSellingList_CS msg_ReqSubSellingList_CS = new MSG_ReqSubSellingList_CS();
        msg_ReqSubSellingList_CS.itemtype = itemType;
        for (int i = 0; i < idList.Count; i++)
        {
            msg_ReqSubSellingList_CS.idlist.Add(idList[i]);
        }
        msg_ReqSubSellingList_CS.levelstar = levelStar;
        msg_ReqSubSellingList_CS.checkshow = checkShow;
        base.SendMsg<MSG_ReqSubSellingList_CS>(CommandID.MSG_ReqSubSellingList_CS, msg_ReqSubSellingList_CS, false);
    }

    public void MSG_ReqBuyItem_CS(SELLTYPE itemType, uint baseId, uint levelStar, uint num, string thisId)
    {
        base.SendMsg<MSG_ReqBuyItem_CS>(CommandID.MSG_ReqBuyItem_CS, new MSG_ReqBuyItem_CS
        {
            itemtype = itemType,
            baseid = baseId,
            levelstar = levelStar,
            num = num,
            thisid = thisId
        }, false);
    }

    public void MSG_ReqTradeItemHistory_CS(SELLTYPE itemType, uint baseId)
    {
        base.SendMsg<MSG_ReqTradeItemHistory_CS>(CommandID.MSG_ReqTradeItemHistory_CS, new MSG_ReqTradeItemHistory_CS
        {
            itemtype = itemType,
            baseid = baseId
        }, false);
    }

    public void MSG_ReqSublistPage_CS(uint page)
    {
        base.SendMsg<MSG_ReqSublistPage_CS>(CommandID.MSG_ReqSublistPage_CS, new MSG_ReqSublistPage_CS
        {
            page = page
        }, false);
    }

    public void MSG_ReqUserTradeHistory_CS()
    {
        MSG_ReqUserTradeHistory_CS t = new MSG_ReqUserTradeHistory_CS();
        base.SendMsg<MSG_ReqUserTradeHistory_CS>(CommandID.MSG_ReqUserTradeHistory_CS, t, false);
    }

    public void MSG_UserSelledItemList_CSC()
    {
        MSG_UserSelledItemList_CSC t = new MSG_UserSelledItemList_CSC();
        base.SendMsg<MSG_UserSelledItemList_CSC>(CommandID.MSG_UserSelledItemList_CSC, t, false);
    }

    public void MSG_ReqBuySelledItem_CS(uint i)
    {
        base.SendMsg<MSG_ReqBuySelledItem_CS>(CommandID.MSG_ReqBuySelledItem_CS, new MSG_ReqBuySelledItem_CS
        {
            index = i
        }, false);
    }

    public void MSG_ReqSellItem_CS(string thisId)
    {
        base.SendMsg<MSG_ReqSellItem_CS>(CommandID.MSG_ReqSellItem_CS, new MSG_ReqSellItem_CS
        {
            thisid = thisId
        }, false);
    }

    public void MSG_ReqGetNewestStaff_CS()
    {
        MSG_ReqGetNewestStaff_CS t = new MSG_ReqGetNewestStaff_CS();
        base.SendMsg<MSG_ReqGetNewestStaff_CS>(CommandID.MSG_ReqGetNewestStaff_CS, t, false);
    }

    private void RetMarketItemList_SC(MSG_RetMarketItemList_SC msg)
    {
        this.controller.OnRetMarketItemList(msg);
    }

    private void RetBuyMarketItem_SC(MSG_RetBuyMarketItem_SC msg)
    {
        this.controller.OnRetBuyMarketItem(msg.errcode);
    }

    private void RetSellingStaff_SC(MSG_RetSellingStaff_SC msg)
    {
        this.controller.MSG_RetSellingStaff_SC(msg);
    }

    private void RetRecommandPrice_SC(MSG_RetRecommandPrice_SC msg)
    {
        this.controller.MSG_RetRecommandPrice_SC(msg);
    }

    private void RetSellStaff_SC(MSG_RetSellStaff_SC msg)
    {
        this.controller.MSG_RetSellStaff_SC(msg);
    }

    private void RetStopSellStaff_SC(MSG_RetStopSellStaff_SC msg)
    {
        this.controller.MSG_RetStopSellStaff_SC(msg);
    }

    private void RetSubSellingList_SC(MSG_RetSubSellingList_SC msg)
    {
        this.controller.MSG_RetSubSellingList_SC(msg);
    }

    private void RetBuyItem_SC(MSG_RetBuyItem_SC msg)
    {
        this.controller.MSG_RetBuyItem_SC(msg);
    }

    private void RetTradeItemHistory_SC(MSG_RetTradeItemHistory_SC msg)
    {
        this.controller.MSG_RetTradeItemHistory_SC(msg);
    }

    private void RetSublistPage_SC(MSG_RetSublistPage_SC msg)
    {
        this.controller.MSG_RetSublistPage_SC(msg);
    }

    private void RetUserTradeHistory_SC(MSG_RetUserTradeHistory_SC msg)
    {
        this.controller.MSG_RetUserTradeHistory_SC(msg);
    }

    private void UserSelledItemList_CSC(MSG_UserSelledItemList_CSC msg)
    {
        this.controller.MSG_UserSelledItemList_CSC(msg);
    }

    private void RetBuySelledItem_SC(MSG_RetBuySelledItem_SC msg)
    {
        if (msg.retcode == 5U)
        {
            TipsWindow.ShowWindow(TipsType.BUYBACK_TIMES_OUT, null);
            this.MSG_UserSelledItemList_CSC();
        }
    }

    private void RetSellItem_SC(MSG_RetSellItem_SC msg)
    {
    }

    private void RetGetNewestStaff_SC(MSG_RetGetNewestStaff_SC msg)
    {
        this.controller.MSG_RetGetNewestStaff_SC(msg);
    }

    private ShopController _controller;
}
