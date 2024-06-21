using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Managers;
using guild;
using LuaInterface;
using market;
using Models;
using msg;
using Obj;
using trade;
using UnityEngine;

public class ShopController : ControllerBase
{
    public UI_NPCshop shop
    {
        get
        {
            return UIManager.GetUIObject<UI_NPCshop>();
        }
    }

    public UI_Business business
    {
        get
        {
            return UIManager.GetUIObject<UI_Business>();
        }
    }

    public override string ControllerName
    {
        get
        {
            return "shop_controller";
        }
    }

    public override void Awake()
    {
        this.mNetWork = new ShopNetWork();
        this.mNetWork.Initialize();
        this.InitConfig();
        this.Init();
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.OnOpenBusiness));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.OnOpenWantedShop));
    }

    private void Init()
    {
        this.Id = LuaConfigManager.GetXmlConfigTable("trade").GetField_Uint("trade_money_id");
        this.SellCostId = LuaConfigManager.GetXmlConfigTable("trade").GetField_Uint("sell_cost_id");
        this.TradeMoneyId = LuaConfigManager.GetXmlConfigTable("trade").GetField_Uint("trade_money_id");
        this.MoneyIcon = LuaConfigManager.GetConfigTable("objects", (ulong)this.Id).GetField_String("icon");
        Scheduler.Instance.AddTimer(2f, true, new Scheduler.OnScheduler(this.Update));
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("trade").GetCacheField_Table("buyitem");
        IEnumerator enumerator = cacheField_Table.Values.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
        {
            object obj = enumerator.Current;
            LuaTable item = obj as LuaTable;
            this.BuyItem.Add(item);
            this.NumBuyItem++;
        }
        this.BuyItem.Sort(delegate (LuaTable x, LuaTable y)
        {
            int num = x.GetField_String("id").ToInt();
            int num2 = y.GetField_String("id").ToInt();
            if (num > num2)
            {
                return 1;
            }
            if (num == num2)
            {
                return 0;
            }
            return -1;
        });
    }

    public override void OnDestroy()
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.Update));
    }

    private void Update()
    {
        if (this.business != null && this.business.txtCapital.text != GlobalRegister.GetCurrencyByID(this.TradeMoneyId).ToString())
        {
            this.business.ShowMoney();
        }
    }

    private void InitConfig()
    {
        this.LstDefaultShop.Clear();
        this.ConfigShopInfo.Clear();
        string[] array = LuaConfigManager.GetXmlConfigTable("market").GetCacheField_Table("DefaultShopID").GetField_String("value").Split(new char[]
        {
            ','
        });
        for (int i = 0; i < array.Length; i++)
        {
            this.LstDefaultShop.Add((uint)array[i].ToInt());
        }
        this._revLstDefaultShop = this.LstDefaultShop;
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("market").GetCacheField_Table("shop_info");
        IEnumerator enumerator = cacheField_Table.Values.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
        {
            object obj = enumerator.Current;
            LuaTable item = obj as LuaTable;
            this.ConfigShopInfo.Add(item);
        }
        this.ConfigShopInfo.Sort(delegate (LuaTable x, LuaTable y)
        {
            int num = x.GetField_String("id").ToInt();
            int num2 = y.GetField_String("id").ToInt();
            if (num > num2)
            {
                return 1;
            }
            if (num == num2)
            {
                return 0;
            }
            return -1;
        });
    }

    private void OnLoadUIComplete()
    {
        this.mNetWork.OnReqMarketItemList(this._lstReqID);
    }

    public void CloseUI()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_NPCshop");
    }

    public void OnShopClose()
    {
        this.TargetShopID = 0U;
        this.TargetItemID = 0U;
        this.TargetItemIDByBagGet = 0U;
    }

    public void OnOpenWantedShop(List<VarType> varParams)
    {
        if (varParams.Count < 2)
        {
            return;
        }
        uint num = varParams[0];
        uint num2 = varParams[1];
        if (!(bool)LuaScriptMgr.Instance.CallLuaFunction("UnLockCtrl.BUnlock", new object[]
        {
            Util.GetLuaTable("UnLockCtrl"),
            4
        })[0])
        {
            TipsWindow.ShowWindow(TipsType.FUNCTION_UNLOCK, null);
            return;
        }
        this._lstReqID.Clear();
        uint num3 = num;
        if (num3 > 0U)
        {
            this.TargetShopID = num3;
            this._lstReqID.Add(num3);
        }
        string pathwayID = GlobalRegister.GetPathwayID();
        uint num4 = num2;
        if (pathwayID != null)
        {
            string[] array = pathwayID.Split(new char[]
            {
                '-'
            });
            if (array[0] != null)
            {
                num4 = (uint)array[0].ToInt();
            }
            if (array[1] != null)
            {
                this.WangtObjectNum = array[1].ToInt();
            }
        }
        if (num4 > 0U)
        {
            this.TargetItemID = num4;
        }
        if (num == 7U)
        {
            GuildControllerNew controller = ControllerManager.Instance.GetController<GuildControllerNew>();
            if (controller.GetMyGuildId() == 0UL)
            {
                TipsWindow.ShowWindow(TipsType.HAVE_NO_GUILD_CAN_NOT_OPEN_GUILD_SHOP, null);
                return;
            }
        }
        this.OpenShopUI(new Action(this.OnLoadUIComplete));
    }

    private void OnOpenWantedShopByBag(uint id1, uint id2)
    {
        this.TargetItemIDByBagGet = id2;
        this._lstReqID = this.LstDefaultShop;
        this.TargetShopID = id1;
        this.OnLoadUIComplete();
    }

    private void OnOpenWantedShopTotal(uint id1, uint id2, uint id3)
    {
        this._lstReqID.Clear();
        if (id1 > 0U)
        {
            this.TargetShopID = id1;
            this._lstReqID.Add(id1);
        }
        if (id2 > 0U)
        {
            this._lstReqID.Add(id2);
        }
        if (id3 > 0U)
        {
            this._lstReqID.Add(id3);
        }
        this.OpenShopUI(new Action(this.OnLoadUIComplete));
    }

    public void OnOpenShop(uint id)
    {
        this._lstReqID = this.LstDefaultShop;
        this.TargetShopID = id;
        this.OpenShopUI(new Action(this.OnLoadUIComplete));
    }

    private void OpenShopUI(Action callback)
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_NPCshop>("UI_NPCshop", delegate ()
        {
            if (this.shop != null)
            {
                callback();
                LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel("UI_Bag");
                this.shop.RegOpenUIByNpc("UI_Bag");
                if (luaUIPanel == null || luaUIPanel.uiRoot.transform.localPosition == new Vector3(10000f, 10000f, 0f))
                {
                    LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.EnterPackage", new object[]
                    {
                        Util.GetLuaTable("BagCtrl")
                    });
                }
                else
                {
                    luaUIPanel.uiRoot.transform.SetAsLastSibling();
                }
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public void OnOpenBusiness(List<VarType> varParams)
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Business>("UI_Business", null, UIManager.ParentType.CommonUI, false);
    }

    public void OnCloseBusiness()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_Business");
    }

    public void OnReqMarketItemList(List<uint> lstShopId)
    {
        this.mNetWork.OnReqMarketItemList(lstShopId);
    }

    public void OnRetMarketItemList(MSG_RetMarketItemList_SC info)
    {
        if (info == null)
        {
            FFDebug.LogWarning(this, "shopCtrl:OnRetMarketItemList msg is null ");
        }
        if (this.TargetItemIDByBagGet != 0U)
        {
            bool flag = false;
            for (int i = 0; i < info.marketdetail.Count; i++)
            {
                if (info.marketdetail[i].id == this.TargetShopID)
                {
                    for (int j = 0; j < info.marketdetail[i].itemlist.Count; j++)
                    {
                        if (this.TargetItemIDByBagGet == info.marketdetail[i].itemlist[j].itemid)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }
            if (flag)
            {
                this.TargetItemID = this.TargetItemIDByBagGet;
                this.OpenShopUI(delegate
                {
                    this.shop.InitShopItems(info);
                });
            }
            else
            {
                TipsWindow.ShowWindow(TipsType.SHOP_TIMES_OUT, null);
            }
            this.TargetItemIDByBagGet = 0U;
            return;
        }
        this.shop.InitShopItems(info);
    }

    public void OnReqBuyMarketItem(uint shopid, uint id, uint itemid, uint num)
    {
        this.mNetWork.OnReqBuyMarketItem(shopid, id, itemid, num);
    }

    public void OnRetBuyMarketItem(uint result)
    {
        this.shop.OnRetBuyMarketItem(result);
    }

    public void OpenTradeByNpcdlg()
    {
        if (!(bool)LuaScriptMgr.Instance.CallLuaFunction("UnLockCtrl.BUnlock", new object[]
        {
            Util.GetLuaTable("UnLockCtrl"),
            4
        })[0])
        {
            TipsWindow.ShowWindow(TipsType.FUNCTION_UNLOCK, null);
            return;
        }
        this.OpenShopUI(null);
    }

    public void OpenTradeSaleByBag(string thisId)
    {
        this.OpenShopUI(delegate
        {
            this.ThisIdforSale = thisId;
        });
    }

    public void OpenTradeBuyByBag(string thisId)
    {
        this.OpenShopUI(delegate
        {
            this.ThisIdforSale = thisId;
        });
    }

    public void MSG_ReqSellingStaff_CS()
    {
        this.mNetWork.MSG_ReqSellingStaff_CS();
    }

    public void MSG_RetSellingStaff_SC(MSG_RetSellingStaff_SC callBack)
    {
        if (this.business != null)
        {
            this.business.ShowInSale(callBack);
        }
    }

    public void MSG_ReqRecommandPrice_CS(SELLTYPE itemtype, uint baseId)
    {
        this._reqItemtype = itemtype;
        this._reqBaseid = baseId;
        this.mNetWork.MSG_ReqRecommandPrice_CS(itemtype, baseId);
    }

    public void MSG_RetRecommandPrice_SC(MSG_RetRecommandPrice_SC callBack)
    {
        if (callBack == null)
        {
            return;
        }
        if (this.business != null && callBack.itemtype == this._reqItemtype && callBack.baseid == this._reqBaseid)
        {
            if (callBack.itemtype == SELLTYPE.HERO)
            {
                this.business.ShowPropPutAway(this.ReqProp, callBack.price);
            }
            else
            {
                this.business.ShowPropPutAway(this.ReqProp, callBack.price);
            }
        }
    }

    public void MSG_ReqSellStaff_CS(SELLTYPE itemType, string thisId, uint baseId, uint price, uint num)
    {
        LastPriceData tempLastPrice = new LastPriceData(this.GetTradeObjType(itemType, baseId), baseId, price, this.Ratio);
        this._tempLastPrice = tempLastPrice;
        this.mNetWork.MSG_ReqSellStaff_CS(itemType, thisId, baseId, price, num);
    }

    public void MSG_RetSellStaff_SC(MSG_RetSellStaff_SC callBack)
    {
        if (this.business == null)
        {
            return;
        }
        if (callBack.retcode == 1U)
        {
            this.SaveLastPrice();
            this.business.ClosePutAway();
            this.MSG_ReqSellingStaff_CS();
            Scheduler.Instance.AddTimer(0.2f, false, delegate
            {
                if (this.business)
                {
                }
            });
            TipsWindow.ShowWindow(TipsType.PUTAWAY_SUCCESS, null);
        }
        this.business.ShowMoney();
    }

    internal void OnSkillLvUp(guildSkill skillinfo)
    {
        this.OnLoadUIComplete();
    }

    public void MSG_ReqStopSellStaff_CS(string thisId)
    {
        this.mNetWork.MSG_ReqStopSellStaff_CS(thisId);
    }

    public void MSG_RetStopSellStaff_SC(MSG_RetStopSellStaff_SC callBack)
    {
        if (this.business == null)
        {
            return;
        }
        if (callBack.retcode == 1U)
        {
            TipsWindow.ShowWindow(TipsType.HAVE_SALE_OUT, null);
            this.MSG_ReqSellingStaff_CS();
        }
        this.business.ShowMoney();
    }

    public void MSG_ReqSubSellingList_CS(SELLTYPE itemType, List<uint> idList, uint levelStar, bool checkShow)
    {
        this.LastReqSubSell = new ReqSubSellingList(itemType, idList, levelStar, checkShow);
        this.mNetWork.MSG_ReqSubSellingList_CS(itemType, idList, levelStar, checkShow);
    }

    public void MSG_RetSubSellingList_SC(MSG_RetSubSellingList_SC callBack)
    {
        if (this.business == null)
        {
            return;
        }
        if (callBack.totalpage == 0U)
        {
            callBack.totalpage = 1U;
            if (this.LastReqSublistPage > callBack.totalpage)
            {
                TipsWindow.ShowWindow(TipsType.THE_LAST_PAGE, null);
            }
            this.LastReqSublistPage = callBack.totalpage;
            if (this.LastReqSubSell != null)
            {
                MSG_RetSublistPage_SC msg_RetSublistPage_SC = new MSG_RetSublistPage_SC();
                if (this.LastReqSubSell.CheckShow)
                {
                    this.business.ShowNoticeChangePage(this.LastReqSubSell.ItemType, this.LastReqSubSell.IdList, this.LastReqSubSell.LevelStar, callBack.totalpage);
                    this.business.ShowInNotice(msg_RetSublistPage_SC.item, false);
                }
                else
                {
                    this.business.ShowBuyChangePage(this.LastReqSubSell.ItemType, this.LastReqSubSell.IdList, this.LastReqSubSell.LevelStar, callBack.totalpage);
                    this.business.ShowInBuy(msg_RetSublistPage_SC.item);
                }
            }
        }
        else
        {
            if (this.LastReqSublistPage > callBack.totalpage)
            {
                TipsWindow.ShowWindow(TipsType.THE_LAST_PAGE, null);
                this.LastReqSublistPage = callBack.totalpage;
            }
            if (this.LastReqSubSell != null)
            {
                if (this.LastReqSubSell.CheckShow)
                {
                    this.business.ShowNoticeChangePage(this.LastReqSubSell.ItemType, this.LastReqSubSell.IdList, this.LastReqSubSell.LevelStar, callBack.totalpage);
                }
                else
                {
                    this.business.ShowBuyChangePage(this.LastReqSubSell.ItemType, this.LastReqSubSell.IdList, this.LastReqSubSell.LevelStar, callBack.totalpage);
                }
            }
            this.MSG_ReqSublistPage_CS(this.LastReqSublistPage);
        }
    }

    public void MSG_ReqBuyItem_CS(SELLTYPE itemType, uint baseId, uint levelStar, uint num, string thisId)
    {
        this.mNetWork.MSG_ReqBuyItem_CS(itemType, baseId, levelStar, num, thisId);
    }

    public void MSG_RetBuyItem_SC(MSG_RetBuyItem_SC callBack)
    {
        if (this.business == null)
        {
            return;
        }
        if (this.LastReqSubSell != null)
        {
            this.MSG_ReqSubSellingList_CS(this.LastReqSubSell.ItemType, this.LastReqSubSell.IdList, this.LastReqSubSell.LevelStar, false);
        }
        this.business.ShowMoney();
    }

    public void MSG_ReqTradeItemHistory_CS(SELLTYPE itemType, uint baseId)
    {
        this.mNetWork.MSG_ReqTradeItemHistory_CS(itemType, baseId);
    }

    public void MSG_RetTradeItemHistory_SC(MSG_RetTradeItemHistory_SC callBack)
    {
        if (this.business == null)
        {
            return;
        }
    }

    public void MSG_ReqSublistPage_CS(uint page)
    {
        this.mNetWork.MSG_ReqSublistPage_CS(page);
    }

    public void MSG_RetSublistPage_SC(MSG_RetSublistPage_SC callBack)
    {
        if (this.business == null)
        {
            return;
        }
        if (this.LastReqSubSell != null)
        {
            if (this.LastReqSubSell.CheckShow)
            {
                this.business.ShowInNotice(callBack.item, false);
            }
            else
            {
                this.business.ShowInBuy(callBack.item);
            }
        }
    }

    public void MSG_ReqUserTradeHistory_CS()
    {
        this.mNetWork.MSG_ReqUserTradeHistory_CS();
    }

    public void MSG_RetUserTradeHistory_SC(MSG_RetUserTradeHistory_SC callBack)
    {
        if (this.business == null)
        {
            return;
        }
        this.business.MSG_RetUserTradeHistory_SC(callBack);
    }

    public void MSG_UserSelledItemList_CSC(MSG_UserSelledItemList_CSC callBack)
    {
        if (this.shop == null)
        {
            return;
        }
        this.shop.InitShopBuybackItems(callBack);
    }

    private void SaveLastPrice()
    {
        if (this._tempLastPrice != null)
        {
            this.AllLastPrice.Add(this._tempLastPrice);
        }
        this._tempLastPrice = null;
    }

    private bool CheckIsGemOrNot(uint baseId)
    {
        return this.business.CheckIsGemOrNot(baseId);
    }

    private TradeObjType GetTradeObjType(SELLTYPE itemType, uint baseId)
    {
        return this.business.GetTradeObjType(itemType, baseId);
    }

    public void MSG_ReqSellItem_CS(BagDragDropButtonData data, bool fastSell)
    {
        if (LuaConfigManager.GetConfigTable("objects", (ulong)data.mId) == null)
        {
            return;
        }
        PropsBase propsBase = (PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetPropBaseByThisID", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            data.thisid
        })[0];
        if (propsBase == null)
        {
            return;
        }
        if (propsBase._obj.type == ObjectType.OBJTYPE_QUEST)
        {
            TipsWindow.ShowWindow(TipsType.CANT_SELL_QUEST, null);
            return;
        }
        this.mNetWork.MSG_ReqSellItem_CS(data.thisid);
    }

    public void MSG_ReqBuySelledItem_CS(uint index)
    {
        this.mNetWork.MSG_ReqBuySelledItem_CS(index);
    }

    public void MSG_ReqGetNewestStaff_CS()
    {
        this.mNetWork.MSG_ReqGetNewestStaff_CS();
    }

    public void MSG_RetGetNewestStaff_SC(MSG_RetGetNewestStaff_SC callBack)
    {
        if (this.business != null)
        {
            this.business.ShowInBuy(callBack.itemlist);
        }
    }

    public bool CheckDragInSell(GameObject target)
    {
        if (this.business == null)
        {
            return false;
        }
        Transform sellItemTrans = this.business.GetSellItemTrans();
        if (sellItemTrans == null)
        {
            return false;
        }
        foreach (Transform y in sellItemTrans.GetComponentsInChildren<Transform>())
        {
            if (target.transform == y)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInBusinessSellUI()
    {
        bool result = false;
        if (this.business != null && this.business.Panel_Sale_8.gameObject.activeSelf)
        {
            result = true;
        }
        return result;
    }

    public void DragInSell(DragDropButtonDataBase data)
    {
        if (data == null)
        {
            return;
        }
        PropsBase propsBase = (PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetPropBaseByThisID", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            data.thisid
        })[0];
        if (propsBase == null)
        {
            this.business.ClosePutAway();
            return;
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)data.mId);
        if (configTable == null)
        {
            return;
        }
        string[] array = this.business.txt_count1_5.text.Split(new char[]
        {
            '/'
        });
        if (array[0].ToInt() == array[1].ToInt())
        {
            TipsWindow.ShowWindow(TipsType.NO_REMAIN, null);
            return;
        }
        if (propsBase._obj.tradetime >= SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond())
        {
            TipsWindow.ShowWindow(TipsType.NOTRADE_NOT_TRADE, null);
            return;
        }
        if (propsBase._obj.bind > 0U)
        {
            TipsWindow.ShowWindow(TipsType.BIND_NOT_TRADE, null);
            return;
        }
        if (propsBase._obj.lock_end_time == 1U)
        {
            TipsWindow.ShowWindow(TipsType.LOCK_NOT_TRADE, null);
            return;
        }
        if (propsBase._obj.type == ObjectType.OBJTYPE_QUEST)
        {
            TipsWindow.ShowWindow(TipsType.CANT_SELL_QUEST, null);
            return;
        }
        if (configTable.GetCacheField_String("recommandprice") == string.Empty)
        {
            TipsWindow.ShowWindow(TipsType.CANT_SELL_QUEST, null);
            return;
        }
        this._dragItem = data;
        this.ThisIdforSale = data.thisid;
        this.business.OnTradeBtnClick(propsBase);
    }

    public ShopNetWork mNetWork;

    public int WangtObjectNum = -1;

    public uint TargetShopID;

    public uint TargetItemID;

    public uint TargetItemIDByBagGet;

    public uint Id;

    public uint SellCostId;

    public uint TradeMoneyId;

    public string MoneyIcon = string.Empty;

    private SELLTYPE _reqItemtype = SELLTYPE.OBJECT;

    private uint _reqBaseid;

    public PropsBase ReqProp;

    public float Ratio = 1f;

    public string ThisIdforSale = string.Empty;

    public string ThisIdforBuy = string.Empty;

    private LastPriceData _tempLastPrice;

    private List<uint> _lstReqID = new List<uint>();

    public List<uint> LstDefaultShop = new List<uint>();

    private List<uint> _revLstDefaultShop = new List<uint>();

    public List<LuaTable> ConfigShopInfo = new List<LuaTable>();

    public List<LastPriceData> AllLastPrice = new List<LastPriceData>();

    public int NumBuyItem;

    public List<LuaTable> BuyItem = new List<LuaTable>();

    public ReqSubSellingList LastReqSubSell;

    public uint LastReqSublistPage = 1U;

    public bool NeedListenDrag;

    public List<Transform> recordBuyList = new List<Transform>();

    public List<Transform> recordSellList = new List<Transform>();

    private DragDropButtonDataBase _dragItem;
}
