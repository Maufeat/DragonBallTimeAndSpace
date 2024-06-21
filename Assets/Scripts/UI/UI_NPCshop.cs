using System;
using System.Collections.Generic;
using Framework.Managers;
using guild;
using LuaInterface;
using market;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_NPCshop : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.controller = ControllerManager.Instance.GetController<ShopController>();
        this.controllerGuild = ControllerManager.Instance.GetController<GuildControllerNew>();
        this.InitObj(root);
        this.InitEvent();
        base.RegOpenUIByNpc(string.Empty);
    }

    public override void OnDispose()
    {
        base.OnDispose();
        this.controller.OnShopClose();
    }

    private void InitObj(Transform root)
    {
        this.panelRoot = root.Find("Offset_Example/Panel_shop");
        this.btnClose = this.panelRoot.Find("Panel_title/btn_close").GetComponent<Button>();
        this.panelBuy = this.panelRoot.Find("Panel_buy");
        this.panelBuyBack = this.panelRoot.Find("Panel_buyback");
        this.panelBuyConfirm = this.panelRoot.Find("Panel");
        this.panelBuyConfirm.gameObject.SetActive(false);
        this.togBuy = this.panelRoot.Find("Panel_tab/ToggleGroup/Buy").GetComponent<Toggle>();
        this.togBuyBack = this.panelRoot.Find("Panel_tab/ToggleGroup/Buyback").GetComponent<Toggle>();
        this.btnBuySell = this.panelRoot.Find("Panel_buy/btn_sell").GetComponent<Button>();
        this.btnBuyFix = this.panelRoot.Find("Panel_buy/btn_repair").GetComponent<Button>();
        this.btnBuyFixAll = this.panelRoot.Find("Panel_buy/btn_all").GetComponent<Button>();
        this.transShopItemParent = this.panelRoot.Find("Panel_buy/Scroll View/Viewport/Content");
        this.transItemSource = this.panelRoot.Find("Panel_buy/Scroll View/Viewport/Content/Panel_item");
        this.transShopItemBuybackParent = this.panelRoot.Find("Panel_buyback/Scroll View/Viewport/Content");
        this.transItemBuybackSource = this.panelRoot.Find("Panel_buyback/Scroll View/Viewport/Content/Panel_item");
        this.txtHave = this.panelRoot.Find("Panel_buy/img_exchangeitem/text").GetComponent<Text>();
        this.imgHave = this.panelRoot.Find("Panel_buy/img_exchangeitem").GetComponent<Image>();
        this._shopConfirm = this.panelBuyConfirm.gameObject.AddComponent<UI_NPCshopConfirm>();
        this._shopConfirm.InitObj();
        this.transItemSource.gameObject.SetActive(false);
        this.transItemBuybackSource.gameObject.SetActive(false);
        this.haveTip = this.txtHave.GetComponent<TextTip>();
        if (null == this.haveTip)
        {
            this.haveTip = this.txtHave.gameObject.AddComponent<TextTip>();
        }
        this.haveTip.SetText(this.GetHaveCost().ToString());
    }

    private void InitEvent()
    {
        this.btnClose.onClick.RemoveAllListeners();
        this.btnClose.onClick.AddListener(new UnityAction(this.OnCloseButtonClick));
        this.btnBuySell.onClick.RemoveAllListeners();
        this.btnBuySell.onClick.AddListener(new UnityAction(this.OnBuySellButtonClick));
        this.btnBuyFix.onClick.RemoveAllListeners();
        this.btnBuyFix.onClick.AddListener(new UnityAction(this.OnBuyFixButtonClick));
        this.btnBuyFixAll.onClick.RemoveAllListeners();
        this.btnBuyFixAll.onClick.AddListener(new UnityAction(this.OnBuyFixAllButtonClick));
        this.togBuy.onValueChanged.RemoveAllListeners();
        this.togBuy.onValueChanged.AddListener(delegate (bool isSelect)
        {
            this.SwitchInnerType(isSelect, UI_NPCshop.ShopToggleType.Buy);
        });
        this.togBuyBack.onValueChanged.RemoveAllListeners();
        this.togBuyBack.onValueChanged.AddListener(delegate (bool isSelect)
        {
            this.SwitchInnerType(isSelect, UI_NPCshop.ShopToggleType.BuyBack);
        });
    }

    private void OnCloseButtonClick()
    {
        this.controller.CloseUI();
    }

    private void OnBuySellButtonClick()
    {
        GlobalRegister.OpenMouseState(11);
    }

    private void OnBuyFixButtonClick()
    {
        MouseStateControoler.Instan.SetMoseState(MoseState.m_itemrepair);
    }

    private void OnBuyFixAllButtonClick()
    {
        UIBagManager.Instance.TryReqCharacterRepair();
    }

    public void AddClickForItem(Button btn, UI_NPCshopItem item)
    {
        if (item == null)
        {
            FFDebug.LogWarning(this, "UI_NPCshop:AddClickForItem item is null");
        }
        btn.onClick.AddListener(delegate ()
        {
            this._shopConfirm.InitItem(item, this);
        });
    }

    private void SwitchInnerType(bool isSelete, UI_NPCshop.ShopToggleType type)
    {
        if (!isSelete)
        {
            return;
        }
        this.panelBuy.gameObject.SetActive(false);
        this.panelBuyBack.gameObject.SetActive(false);
        if (type != UI_NPCshop.ShopToggleType.Buy)
        {
            if (type == UI_NPCshop.ShopToggleType.BuyBack)
            {
                this.controller.mNetWork.MSG_UserSelledItemList_CSC();
                this.panelBuyBack.gameObject.SetActive(true);
            }
        }
        else
        {
            this.panelBuy.gameObject.SetActive(true);
        }
    }

    private void ClearContentInfo()
    {
    }

    public void OnReqBuyMarketItem(UI_NPCshopConfirm info)
    {
        if (info.UINPCShopItem.ItemId <= 0U || info.InputNum <= 0U)
        {
            return;
        }
        if (this._isReqBuyItem)
        {
            return;
        }
        if (info.UINPCShopItem.RestNum <= 0U && info.UINPCShopItem.MaxNum > 0U)
        {
            TipsWindow.ShowWindow(TipsType.SHOP_TIMES_OUT, null);
            return;
        }
        if ((ulong)info.TotalCost > (ulong)((long)this.GetHaveCost()))
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)info.UINPCShopItem.CostId);
            if (configTable != null)
            {
                string[] args = new string[]
                {
                    info.UINPCShopItem.CostName
                };
                TipsWindow.ShowWindow(TipsType.SHOP_NUM_TOOMUCH, args);
            }
            else
            {
                FFDebug.LogWarning(this, "cant find cost item id = " + info.UINPCShopItem.CostId);
            }
            return;
        }
        this._isReqBuyItem = true;
        this._reqShopID = info.UINPCShopItem.ShopId;
        this._reqID = info.UINPCShopItem.Id;
        this._reqItemID = info.UINPCShopItem.ItemId;
        this._reqItemNum = info.InputNum;
        this._reqShopType = info.UINPCShopItem.ShopType;
        this._costName = info.UINPCShopItem.CostName;
        this.controller.OnReqBuyMarketItem(info.UINPCShopItem.ShopId, this._reqID, this._reqItemID, this._reqItemNum);
    }

    public void OnReqBuySelledItem_CS(UI_NPCshopItemBuyback info)
    {
        if ((ulong)info.TotalCost > (ulong)((long)this.GetObjNumById(info.BuybackId)))
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)info.BuybackId);
            if (configTable != null)
            {
                string[] args = new string[]
                {
                    info.BuybackName
                };
                TipsWindow.ShowWindow(TipsType.SHOP_NUM_TOOMUCH, args);
            }
            else
            {
                FFDebug.LogWarning(this, "cant find cost item id = " + info.BuybackId);
            }
            return;
        }
        this.controller.MSG_ReqBuySelledItem_CS(info.Index);
    }

    public void OnRetBuyMarketItem(uint resultCode)
    {
        if (resultCode == 5U)
        {
            this.controller.mNetWork.OnReqMarketItemList(this.controller.LstDefaultShop);
        }
        for (int i = 0; i < this._serverInfo.Count; i++)
        {
            if (this._serverInfo[i].id == this._reqShopID)
            {
                for (int j = 0; j < this._serverInfo[i].itemlist.Count; j++)
                {
                    if (this._serverInfo[i].itemlist[j].id == this._reqID && this._serverInfo[i].itemlist[j].itemid == this._reqItemID)
                    {
                        this._serverInfo[i].itemlist[j].curnum += this._reqItemNum;
                        this._serverInfo[i].itemlist[j].curnum = (uint)Mathf.Clamp(this._serverInfo[i].itemlist[j].curnum, 0f, this._serverInfo[i].itemlist[j].maxnum);
                    }
                }
                break;
            }
        }
        if (resultCode == 0U)
        {
            Scheduler.Instance.AddFrame(2U, false, new Scheduler.OnScheduler(this.UpdataHaveCost));
            this.UpdateLstItem();
            this._shopConfirm.OnCancelButtonClick();
        }
        else if (resultCode != 1U)
        {
            if (resultCode == 2U)
            {
                string[] args = new string[]
                {
                    this._costName.ToString()
                };
                TipsWindow.ShowWindow(TipsType.ITEM_NOT_ENOUGH, args);
            }
            else if (resultCode == 3U)
            {
                TipsWindow.ShowWindow(TipsType.SHOP_TIMES_OUT, null);
            }
            else if (resultCode == 4U)
            {
                TipsWindow.ShowWindow(TipsType.PACKAGE_FULL, null);
            }
            else if (resultCode == 6U)
            {
                TipsWindow.ShowWindow(TipsType.GUILD_SKILL_LV_NOT_ENOUGH, null);
            }
        }
        this._isReqBuyItem = false;
        this._reqID = 0U;
        this._reqShopID = 0U;
        this._reqItemID = 0U;
        this._reqItemNum = 0U;
    }

    private void UpdateLstItem()
    {
        MarketType reqShopType = this._reqShopType;
        for (int i = 0; i < this._lstShopItem.Count; i++)
        {
            if (this._lstShopItem[i].Id == this._reqID && this._lstShopItem[i].ItemId == this._reqItemID)
            {
                this._lstShopItem[i].UpdateDataNum(this._reqItemNum);
            }
        }
    }

    public void InitShopItems(MSG_RetMarketItemList_SC scMsg)
    {
        this.ClearAllItemObj();
        this.ClearContentInfo();
        this._serverInfo = scMsg.marketdetail;
        this._refreshInfo.Clear();
        for (int i = 0; i < scMsg.marketdetail.Count; i++)
        {
            MarketType type = scMsg.marketdetail[i].type;
            if (type == MarketType.MarketType_1)
            {
                this.togBuy.gameObject.SetActive(true);
            }
            this._refreshInfo.Add(scMsg.marketdetail[i]);
            for (int j = 0; j < scMsg.marketdetail[i].itemlist.Count; j++)
            {
                if (scMsg.marketdetail[i].type != MarketType.MarketType_4)
                {
                    this.CreateItem(scMsg.marketdetail[i].id, scMsg.marketdetail[i].type, scMsg.marketdetail[i].itemlist[j], scMsg.guildskilllv);
                }
            }
            if (this.controller.TargetShopID != 0U && scMsg.marketdetail[i].id == this.controller.TargetShopID)
            {
                type = scMsg.marketdetail[i].type;
                if (type == MarketType.MarketType_1)
                {
                    this.togBuy.isOn = true;
                }
                this.SwitchInnerType(true, UI_NPCshop.ShopToggleType.Buy);
            }
            else if (this.controller.TargetShopID == 0U)
            {
                this.togBuy.isOn = true;
                this.SwitchInnerType(true, UI_NPCshop.ShopToggleType.Buy);
            }
        }
        this.controller.WangtObjectNum = -1;
        this.controller.TargetShopID = 0U;
        this.controller.TargetItemID = 0U;
    }

    private void CreateItem(uint marketId, MarketType markettype, Item serverData, uint guildSkilLv = 0U)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)serverData.itemid);
        if (configTable == null)
        {
            FFDebug.LogWarning(this, "cannt find config id = " + serverData.itemid);
        }
        else
        {
            Transform parentTrans = this.transShopItemParent;
            GameObject gameObject = this.transItemSource.gameObject;
            parentTrans = this.transShopItemParent;
            UI_NPCshopItem ui_NPCshopItem = this.InstantiateItem(gameObject, parentTrans);
            ui_NPCshopItem.UpdateItemByServerData(configTable, serverData, marketId, markettype);
            ui_NPCshopItem.gameObject.SetActive(true);
            if (markettype == MarketType.MarketType_7)
            {
                guildInfo guildBaseInfo = this.controllerGuild.GetGuildBaseInfo();
                List<guildSkill> guildSkill = this.controllerGuild.GetGuildSkill();
                ui_NPCshopItem.TryUpdateGuildByGuildSkill(guildSkill, serverData, this.controllerGuild, guildSkilLv);
            }
            this._lstShopItem.Add(ui_NPCshopItem);
        }
    }

    private void ClearAllItemObj()
    {
        this.ClearItemObj(this._lstShopItem);
        this._lstShopItem.Clear();
    }

    private void ClearItemObj(List<UI_NPCshopItem> itemList)
    {
        if (itemList == null)
        {
            return;
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            UnityEngine.Object.Destroy(itemList[i].gameObject);
        }
    }

    private void ClearAllItemObjBuyBack()
    {
        if (this._lstShopItemBuyback == null)
        {
            return;
        }
        for (int i = 0; i < this._lstShopItemBuyback.Count; i++)
        {
            UnityEngine.Object.Destroy(this._lstShopItemBuyback[i].gameObject);
        }
        this._lstShopItemBuyback.Clear();
    }

    private UI_NPCshopItem InstantiateItem(GameObject sourceObj, Transform parentTrans)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(sourceObj);
        gameObject.transform.SetParent(parentTrans);
        gameObject.transform.localScale = Vector3.one;
        gameObject.name = "ShopItem";
        gameObject.SetActive(true);
        UI_NPCshopItem ui_NPCshopItem = gameObject.AddComponent<UI_NPCshopItem>();
        ui_NPCshopItem.InitItem(this);
        return ui_NPCshopItem;
    }

    private UI_NPCshopItemBuyback InstantiateItemBuyback(GameObject sourceObj, Transform parentTrans)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(sourceObj);
        gameObject.transform.SetParent(parentTrans);
        gameObject.transform.localScale = Vector3.one;
        gameObject.name = "ShopItemBuyback";
        gameObject.SetActive(true);
        UI_NPCshopItemBuyback ui_NPCshopItemBuyback = gameObject.AddComponent<UI_NPCshopItemBuyback>();
        ui_NPCshopItemBuyback.InitItem(this);
        return ui_NPCshopItemBuyback;
    }

    public void InitHave(uint costId, string iconName)
    {
        this._costId = costId;
        int num = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            this._costId
        })[0].ToString().ToInt();
        this.txtHave.text = GlobalRegister.GetCurrenyStr((uint)num);
        if (this.haveTip != null)
        {
            this.haveTip.SetText(this.GetHaveCost().ToString());
        }
        GlobalRegister.SetImage(0, iconName, this.imgHave, true);
    }

    public int GetHaveCost()
    {
        return LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            this._costId
        })[0].ToString().ToInt();
    }

    public int GetObjNumById(uint itemId)
    {
        return LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            itemId
        })[0].ToString().ToInt();
    }

    public void UpdataHaveCost()
    {
        int num = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            this._costId
        })[0].ToString().ToInt();
        this.txtHave.text = GlobalRegister.GetCurrenyStr((uint)num);
        if (this.haveTip != null)
        {
            this.haveTip.SetText(this.GetHaveCost().ToString());
        }
    }

    public void InitShopBuybackItems(MSG_UserSelledItemList_CSC callBack)
    {
        this.ClearAllItemObjBuyBack();
        for (int i = 0; i < callBack.objs.Count; i++)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)callBack.objs[i].item.baseid);
            if (configTable == null)
            {
                FFDebug.LogWarning(this, "cannt find config id = " + callBack.objs[i].item.baseid);
            }
            else
            {
                Transform parentTrans = this.transShopItemBuybackParent;
                GameObject gameObject = this.transItemBuybackSource.gameObject;
                UI_NPCshopItemBuyback ui_NPCshopItemBuyback = this.InstantiateItemBuyback(gameObject, parentTrans);
                ui_NPCshopItemBuyback.UpdateItemByServerData(configTable, callBack.objs[i], (uint)i);
                ui_NPCshopItemBuyback.gameObject.SetActive(true);
                this._lstShopItemBuyback.Add(ui_NPCshopItemBuyback);
            }
        }
    }

    public void SetTextQualityColor(Text curTxt, int quality)
    {
        switch (quality)
        {
            case 1:
                {
                    string modelColor = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item1");
                    curTxt.color = CommonTools.HexToColor(modelColor);
                    break;
                }
            case 2:
                {
                    string modelColor2 = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item2");
                    curTxt.color = CommonTools.HexToColor(modelColor2);
                    break;
                }
            case 3:
                {
                    string modelColor3 = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item3");
                    curTxt.color = CommonTools.HexToColor(modelColor3);
                    break;
                }
            case 4:
                {
                    string modelColor4 = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item4");
                    curTxt.color = CommonTools.HexToColor(modelColor4);
                    break;
                }
        }
    }

    private Transform ui_root;

    private ShopController controller;

    private GuildControllerNew controllerGuild;

    private Transform panelRoot;

    private Transform panelBuy;

    private Transform panelBuyBack;

    private Transform panelBuyConfirm;

    private Toggle togBuy;

    private Toggle togBuyBack;

    private Button btnClose;

    private Button btnBuySell;

    private Button btnBuyFix;

    private Button btnBuyFixAll;

    private Transform transShopItemParent;

    private Transform transItemSource;

    private Transform transShopItemBuybackParent;

    private Transform transItemBuybackSource;

    private Text txtHave;

    private Image imgHave;

    private UI_NPCshopConfirm _shopConfirm;

    private List<OneMarket> _serverInfo = new List<OneMarket>();

    private List<OneMarket> _refreshInfo = new List<OneMarket>();

    private List<UI_NPCshopItem> _lstShopItem = new List<UI_NPCshopItem>();

    private List<UI_NPCshopItemBuyback> _lstShopItemBuyback = new List<UI_NPCshopItemBuyback>();

    private bool _isReqBuyItem;

    private uint _reqShopID;

    private uint _reqID;

    private uint _reqItemID;

    private uint _reqItemNum;

    private string _costName;

    private uint _costId;

    private MarketType _reqShopType;

    private TextTip haveTip;

    private enum ShopToggleType
    {
        Buy = 1,
        BuyBack
    }
}
