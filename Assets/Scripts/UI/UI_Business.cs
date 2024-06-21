using System;
using System.Collections.Generic;
using Framework.Managers;
using hero;
using LuaInterface;
using msg;
using Obj;
using trade;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Business : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.controller = ControllerManager.Instance.GetController<ShopController>();
        this.InitObj(root);
        this.InitEvent();
        this.onvaluechanged_btn_buy_3(true);
        this.controller.MSG_ReqGetNewestStaff_CS();
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private void InitObj(Transform root)
    {
        this.panelRoot = root.Find("Panel_business");
        this.Panel_Record_42 = this.panelRoot.Find("Panel_record");
        this.btn_buy_3 = this.panelRoot.Find("Panel_toggle/Panel_tab/buy").GetComponent<Toggle>();
        this.Panel_Buy_2 = this.panelRoot.Find("Panel_buy");
        this.btn_sale_5 = this.panelRoot.Find("Panel_toggle/Panel_tab/sell").GetComponent<Toggle>();
        this.Panel_Sale_8 = this.panelRoot.Find("Panel_sell");
        this.btn_show_8 = this.panelRoot.Find("Panel_toggle/Panel_tab/show").GetComponent<Toggle>();
        this.Panel_Show_16 = this.panelRoot.Find("Panel_show");
        this.Panel_Show_53 = this.panelRoot.Find("Panel_show");
        this.txtCapital = this.panelRoot.Find("Panel_toggle/Panel_capital/Text").GetComponent<Text>();
        this.btnRecordClose = this.panelRoot.Find("Panel_record/Panel_top/btn_close").GetComponent<Button>();
        this.list_43 = this.panelRoot.Find("Panel_record/Scroll View");
        this.Item_44 = this.panelRoot.Find("Panel_record/Scroll View/Viewport/Content/Panel_info");
        this.ddRecord = this.panelRoot.Find("Panel_record/Panel_top/Dropdown").GetComponent<Dropdown>();
        this.btnClose = this.panelRoot.Find("Panel_top/btn_close").GetComponent<Button>();
        this.btnRecord = this.panelRoot.Find("Panel_toggle/btn_record").GetComponent<Button>();
        this.Item_44.gameObject.SetActive(false);
        this.InitBusinessBuy();
        this.InitBusinessNotice();
        this.InitBusinessSell();
        this.ShowMoney();
        this.capitalTip = this.txtCapital.GetComponent<TextTip>();
        if (null == this.capitalTip)
        {
            this.capitalTip = this.txtCapital.gameObject.AddComponent<TextTip>();
        }
        this.capitalTip.SetText(GlobalRegister.GetCurrenyStr(GlobalRegister.GetCurrencyByID(this.controller.TradeMoneyId)));
    }

    private void InitEvent()
    {
        this.btn_buy_3.onValueChanged.RemoveAllListeners();
        this.btn_buy_3.onValueChanged.AddListener(new UnityAction<bool>(this.onvaluechanged_btn_buy_3));
        this.btn_sale_5.onValueChanged.RemoveAllListeners();
        this.btn_sale_5.onValueChanged.AddListener(new UnityAction<bool>(this.onvaluechanged_btn_sale_5));
        this.btn_show_8.onValueChanged.RemoveAllListeners();
        this.btn_show_8.onValueChanged.AddListener(new UnityAction<bool>(this.onvaluechanged_btn_show_8));
        this.btnClose.onClick.RemoveAllListeners();
        this.btnClose.onClick.AddListener(new UnityAction(this.OnBtnCloseClick));
        this.btnRecord.onClick.RemoveAllListeners();
        this.btnRecord.onClick.AddListener(new UnityAction(this.OnBtnRecordClick));
        this.btnRecordClose.onClick.RemoveAllListeners();
        this.btnRecordClose.onClick.AddListener(delegate ()
        {
            this.Panel_Record_42.gameObject.SetActive(false);
        });
        this.ddRecord.onValueChanged.RemoveAllListeners();
        this.ddRecord.onValueChanged.AddListener(new UnityAction<int>(this.OnDdRecordValueChanged));
    }

    private void onvaluechanged_btn_buy_3(bool isSelect)
    {
        if (!isSelect)
        {
            return;
        }
        this.Panel_Buy_2.gameObject.SetActive(true);
        this.Panel_Sale_8.gameObject.SetActive(false);
        this.Panel_Show_16.gameObject.SetActive(false);
        this.ShowTradeBuy();
    }

    private void onvaluechanged_btn_sale_5(bool isSelect)
    {
        if (!isSelect)
        {
            return;
        }
        this.Panel_Buy_2.gameObject.SetActive(false);
        this.Panel_Sale_8.gameObject.SetActive(true);
        this.Panel_Show_16.gameObject.SetActive(false);
        this.ShowTradeSale();
    }

    private void onvaluechanged_btn_show_8(bool isSelect)
    {
        if (!isSelect)
        {
            return;
        }
        this.Panel_Buy_2.gameObject.SetActive(false);
        this.Panel_Sale_8.gameObject.SetActive(false);
        this.Panel_Show_16.gameObject.SetActive(true);
        this.ShowTradeNotice();
    }

    private void OnBtnCloseClick()
    {
        this.controller.OnCloseBusiness();
    }

    private void OnBtnRecordClick()
    {
        this.controller.MSG_ReqUserTradeHistory_CS();
    }

    private void OnDdRecordValueChanged(int index)
    {
        for (int i = 0; i < this.controller.recordSellList.Count; i++)
        {
            if (this.controller.recordSellList[i] != null)
            {
                this.controller.recordSellList[i].gameObject.SetActive(index == 0 || index == 2);
            }
        }
        for (int j = 0; j < this.controller.recordBuyList.Count; j++)
        {
            if (this.controller.recordBuyList[j] != null)
            {
                this.controller.recordBuyList[j].gameObject.SetActive(index == 0 || index == 1);
            }
        }
    }

    public void ShowMoney()
    {
        if (this.controller.Id == 0U)
        {
            return;
        }
        this.txtCapital.text = GlobalRegister.GetCurrenyStr(GlobalRegister.GetCurrencyByID(this.controller.TradeMoneyId));
        if (this.capitalTip != null)
        {
            this.capitalTip.SetText(GlobalRegister.GetCurrencyByID(this.controller.TradeMoneyId).ToString());
        }
    }

    public void MSG_RetUserTradeHistory_SC(MSG_RetUserTradeHistory_SC callBack)
    {
        if (callBack == null)
        {
            return;
        }
        callBack.one.Sort(delegate (UserTradeItem x, UserTradeItem y)
        {
            int tradetime = (int)x.item.tradetime;
            int tradetime2 = (int)y.item.tradetime;
            if (tradetime < tradetime2)
            {
                return 1;
            }
            if (tradetime == tradetime2)
            {
                return 0;
            }
            return -1;
        });
        this.Panel_Record_42.gameObject.SetActive(true);
        int count = callBack.one.Count;
        Transform item_ = this.Item_44;
        Transform parent = item_.parent;
        this.CopyToEnough(item_, count);
        this.controller.recordBuyList.Clear();
        this.controller.recordSellList.Clear();
        for (int i = 1; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child != null)
            {
                if (i <= count)
                {
                    TradeItemData tid = callBack.one[i - 1].item.data;
                    LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)tid.objdata.baseid);
                    if (configTable != null)
                    {
                        GlobalRegister.SetImage(0, configTable.GetField_String("icon"), child.Find("img_icon/Image").GetComponent<Image>(), true);
                        string imgname = "quality" + configTable.GetField_Uint("quality");
                        RawImage imgIconQuality = child.Find("img_icon/quality").GetComponent<RawImage>();
                        imgIconQuality.gameObject.SetActive(false);
                        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.RANK, imgname, delegate (UITextureAsset asset)
                        {
                            if (asset == null)
                            {
                                return;
                            }
                            if (imgIconQuality == null)
                            {
                                return;
                            }
                            imgIconQuality.gameObject.SetActive(true);
                            imgIconQuality.texture = asset.textureObj;
                        });
                    }
                    child.Find("price/Text").GetComponent<Text>().text = callBack.one[i - 1].item.price.ToString();
                    child.Find("txt_time").GetComponent<Text>().text = GlobalRegister.GetServerDateTimeByTimeStampWithAllTime((int)callBack.one[i - 1].item.tradetime);
                    if (tid.itemtype == SELLTYPE.OBJECT)
                    {
                        child.Find("txt_name").GetComponent<Text>().text = tid.objdata.name;
                    }
                    else if (tid.itemtype == SELLTYPE.HERO)
                    {
                        uint baseid = tid.herodata.baseid;
                        LuaTable configTable2 = LuaConfigManager.GetConfigTable("npc_data", (ulong)baseid);
                        if (configTable2 != null)
                        {
                            child.Find("txt_name").GetComponent<Text>().text = configTable2.GetField_String("name");
                        }
                    }
                    if (callBack.one[i - 1].op == TradeOP.SELL)
                    {
                        this.controller.recordSellList.Add(child);
                    }
                    else if (callBack.one[i - 1].op == TradeOP.BUY)
                    {
                        this.controller.recordBuyList.Add(child);
                    }
                    int currServerTimeBySecond = (int)SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond();
                    if ((ulong)callBack.one[i - 1].judgeduetime > (ulong)((long)currServerTimeBySecond))
                    {
                    }
                    child.gameObject.SetActive(true);
                    UIEventListener.Get(child.Find("img_icon").gameObject).onEnter = delegate (PointerEventData ed)
                    {
                        ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(tid.objdata, child.Find("img_icon").gameObject);
                    };
                    UIEventListener.Get(child.Find("img_icon").gameObject).onExit = delegate (PointerEventData ed)
                    {
                        ControllerManager.Instance.GetController<ItemTipController>().ClosePanel();
                    };
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    public void CopyToEnough(Transform trans, int num)
    {
        if (trans == null)
        {
            return;
        }
        int num2 = trans.parent.childCount - 1;
        if (num2 < num)
        {
            for (int i = 0; i < num - num2; i++)
            {
                this.InstantiateObj(trans.gameObject);
            }
        }
    }

    public void CommonCopyToEnough(Transform trans, int other, int num)
    {
        if (trans == null)
        {
            return;
        }
        int num2 = trans.parent.childCount - 1 - other;
        if (num2 < num)
        {
            for (int i = 0; i < num - num2; i++)
            {
                this.InstantiateObj(trans.gameObject);
            }
        }
    }

    public void InstantiateObj(GameObject go)
    {
        if (go == null)
        {
            return;
        }
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(go);
        gameObject.transform.SetParent(go.transform.parent);
        gameObject.transform.localScale = go.transform.localScale;
        gameObject.transform.localRotation = go.transform.localRotation;
    }

    public string SecondToTime(int second)
    {
        second = ((second >= 0) ? second : 0);
        return GlobalRegister.GetTimeInDays((uint)second);
    }

    public string IntToString(uint num, uint max)
    {
        return (num >= max) ? max.ToString("D2") : num.ToString("D2");
    }

    public void ShowSprite(ImageType imageType, uint id, Image image)
    {
        if (image == null)
        {
            return;
        }
        if (imageType == ImageType.ITEM)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)id);
            if (configTable == null)
            {
                return;
            }
            GlobalRegister.SetImage(0, configTable.GetField_String("icon"), image, true);
        }
        else if (imageType == ImageType.ROLES)
        {
            LuaTable configTable2 = LuaConfigManager.GetConfigTable("npc_data", (ulong)id);
            if (configTable2 == null)
            {
                return;
            }
            GlobalRegister.SetImage(3, configTable2.GetField_String("icon"), image, true);
        }
    }

    public bool CheckIsGemOrNot(uint baseId)
    {
        bool result = false;
        if (baseId == 0U)
        {
            return result;
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)baseId);
        if (configTable != null && configTable.GetField_Int("treasure") == 1)
        {
            result = true;
        }
        return result;
    }

    public TradeObjType GetTradeObjType(SELLTYPE itemType, uint baseId)
    {
        TradeObjType result = TradeObjType.None;
        if (itemType == SELLTYPE.HERO)
        {
            result = TradeObjType.Hero;
        }
        else if (itemType == SELLTYPE.OBJECT)
        {
            bool flag = this.CheckIsGemOrNot(baseId);
            result = ((!flag) ? TradeObjType.Item : TradeObjType.Gem);
        }
        return result;
    }

    private void ClearItem(Transform parent, int startIndex = 0)
    {
        for (int i = startIndex; i < parent.childCount; i++)
        {
            parent.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void TryShowItemTip(t_Object data, GameObject go)
    {
        ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(data, go);
    }

    public void TryShowItemTip(LuaTable data, GameObject go)
    {
        ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(data, go);
    }

    public void CloseItemTip()
    {
        ControllerManager.Instance.GetController<ItemTipController>().ClosePanel();
    }

    private int SortByAllPriceUp(TradeItem x, TradeItem y)
    {
        uint price = x.price;
        uint price2 = y.price;
        if (price > price2)
        {
            return 1;
        }
        if (price == price2)
        {
            return 0;
        }
        return -1;
    }

    private int SortByAllPriceDown(TradeItem x, TradeItem y)
    {
        uint price = x.price;
        uint price2 = y.price;
        if (price < price2)
        {
            return 1;
        }
        if (price == price2)
        {
            return 0;
        }
        return -1;
    }

    private int SortByNumUp(TradeItem x, TradeItem y)
    {
        uint num = 1U;
        uint num2 = 1U;
        if (x.data.itemtype == SELLTYPE.OBJECT)
        {
            num = x.data.objdata.num;
            num2 = y.data.objdata.num;
        }
        if (num > num2)
        {
            return 1;
        }
        if (num == num2)
        {
            return 0;
        }
        return -1;
    }

    private int SortByNumDown(TradeItem x, TradeItem y)
    {
        uint num = 1U;
        uint num2 = 1U;
        if (x.data.itemtype == SELLTYPE.OBJECT)
        {
            num = x.data.objdata.num;
            num2 = y.data.objdata.num;
        }
        if (num < num2)
        {
            return 1;
        }
        if (num == num2)
        {
            return 0;
        }
        return -1;
    }

    private int SortByPriceUp(TradeItem x, TradeItem y)
    {
        float num = x.price;
        float num2 = y.price;
        if (x.data.itemtype == SELLTYPE.OBJECT)
        {
            num = x.price / (x.data.objdata.num * 1f);
            num2 = y.price / (y.data.objdata.num * 1f);
        }
        if (num > num2)
        {
            return 1;
        }
        if (num == num2)
        {
            return 0;
        }
        return -1;
    }

    private int SortByPriceDown(TradeItem x, TradeItem y)
    {
        float num = x.price;
        float num2 = y.price;
        if (x.data.itemtype == SELLTYPE.OBJECT)
        {
            num = x.price / (x.data.objdata.num * 1f);
            num2 = y.price / (y.data.objdata.num * 1f);
        }
        if (num < num2)
        {
            return 1;
        }
        if (num == num2)
        {
            return 0;
        }
        return -1;
    }

    private int SortByTimeUp(TradeItem x, TradeItem y)
    {
        uint selltime = x.selltime;
        uint selltime2 = y.selltime;
        if (selltime > selltime2)
        {
            return 1;
        }
        if (selltime == selltime2)
        {
            return 0;
        }
        return -1;
    }

    private int SortByTimeDown(TradeItem x, TradeItem y)
    {
        uint selltime = x.selltime;
        uint selltime2 = y.selltime;
        if (selltime < selltime2)
        {
            return 1;
        }
        if (selltime == selltime2)
        {
            return 0;
        }
        return -1;
    }

    public void InitBusinessBuy()
    {
        this.btn_tab_4 = this.panelRoot.Find("Panel_buy/Scroll_filter/Viewport/Content/panel_lv1");
        this.list_4 = this.panelRoot.Find("Panel_buy/Panel_list/Scroll View");
        this.object_28 = this.panelRoot.Find("Panel_buy/Panel_list/Scroll View/Viewport/Content/Panel_info");
        this.filterContent = this.panelRoot.Find("Panel_buy/Scroll_filter/Viewport/Content");
        this.btn_minus_24 = this.panelRoot.Find("Panel_buy/Panel_list/bottom/Panel_page/btn_previous").GetComponent<Button>();
        this.btn_add_26 = this.panelRoot.Find("Panel_buy/Panel_list/bottom/Panel_page/btn_next").GetComponent<Button>();
        this.txt_allpage_53 = this.panelRoot.Find("Panel_buy/Panel_list/bottom/Panel_page/Text").GetComponent<Text>();
        this.btnBuyNumSortUp = this.panelRoot.Find("Panel_buy/Panel_list/title/txt2/btn_up").GetComponent<Button>();
        this.btnBuyNumSortDown = this.panelRoot.Find("Panel_buy/Panel_list/title/txt2/btn_down").GetComponent<Button>();
        this.btnBuyPriceSortUp = this.panelRoot.Find("Panel_buy/Panel_list/title/txt3/btn_up").GetComponent<Button>();
        this.btnBuyPriceSortDown = this.panelRoot.Find("Panel_buy/Panel_list/title/txt3/btn_down").GetComponent<Button>();
        this.btnBuyAllPriceSortUp = this.panelRoot.Find("Panel_buy/Panel_list/title/txt4/btn_up").GetComponent<Button>();
        this.btnBuyAllPriceSortDown = this.panelRoot.Find("Panel_buy/Panel_list/title/txt4/btn_up").GetComponent<Button>();
        this.btnBuyTimeSortUp = this.panelRoot.Find("Panel_buy/Panel_list/title/txt4/btn_up").GetComponent<Button>();
        this.btnBuyTimeSortDown = this.panelRoot.Find("Panel_buy/Panel_list/title/txt4/btn_up").GetComponent<Button>();
        this.btnBuyFresh = this.panelRoot.Find("Panel_buy/Panel_list/bottom/btn_fresh").GetComponent<Button>();
        this.btnBuyFresh.gameObject.SetActive(false);
        this.object_28.gameObject.SetActive(false);
        this.InitBusinessBuyEvent();
    }

    public void InitBusinessBuyEvent()
    {
        this.btnBuyNumSortUp.onClick.RemoveAllListeners();
        this.btnBuyNumSortUp.onClick.AddListener(delegate ()
        {
            this.SortBuyItemList(UI_Business.SortType.NumUp);
        });
        this.btnBuyNumSortDown.onClick.RemoveAllListeners();
        this.btnBuyNumSortDown.onClick.AddListener(delegate ()
        {
            this.SortBuyItemList(UI_Business.SortType.NumDown);
        });
        this.btnBuyPriceSortUp.onClick.RemoveAllListeners();
        this.btnBuyPriceSortUp.onClick.AddListener(delegate ()
        {
            this.SortBuyItemList(UI_Business.SortType.PriceUp);
        });
        this.btnBuyPriceSortDown.onClick.RemoveAllListeners();
        this.btnBuyPriceSortDown.onClick.AddListener(delegate ()
        {
            this.SortBuyItemList(UI_Business.SortType.PriceDown);
        });
        this.btnBuyAllPriceSortUp.onClick.RemoveAllListeners();
        this.btnBuyAllPriceSortUp.onClick.AddListener(delegate ()
        {
            this.SortBuyItemList(UI_Business.SortType.AllPriceUp);
        });
        this.btnBuyAllPriceSortDown.onClick.RemoveAllListeners();
        this.btnBuyAllPriceSortDown.onClick.AddListener(delegate ()
        {
            this.SortBuyItemList(UI_Business.SortType.AllPriceDown);
        });
        this.btnBuyTimeSortUp.onClick.RemoveAllListeners();
        this.btnBuyTimeSortUp.onClick.AddListener(delegate ()
        {
            this.SortBuyItemList(UI_Business.SortType.TimeUp);
        });
        this.btnBuyTimeSortDown.onClick.RemoveAllListeners();
        this.btnBuyTimeSortDown.onClick.AddListener(delegate ()
        {
            this.SortBuyItemList(UI_Business.SortType.TimeDown);
        });
        this.btnBuyFresh.onClick.RemoveAllListeners();
        this.btnBuyFresh.onClick.AddListener(delegate ()
        {
            if (this.controller.LastReqSubSell != null)
            {
                this.controller.MSG_ReqSubSellingList_CS(this.controller.LastReqSubSell.ItemType, this.controller.LastReqSubSell.IdList, this.controller.LastReqSubSell.LevelStar, false);
            }
        });
    }

    private void SortBuyItemList(UI_Business.SortType type)
    {
        if (this.buyItemList == null || this.buyItemList.Count < 1)
        {
            return;
        }
        switch (type)
        {
            case UI_Business.SortType.NumUp:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByNumUp));
                break;
            case UI_Business.SortType.NumDown:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByNumDown));
                break;
            case UI_Business.SortType.PriceUp:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByPriceUp));
                break;
            case UI_Business.SortType.PriceDown:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByPriceDown));
                break;
            case UI_Business.SortType.AllPriceUp:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByAllPriceUp));
                break;
            case UI_Business.SortType.AllPriceDown:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByAllPriceDown));
                break;
            case UI_Business.SortType.TimeUp:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByTimeUp));
                break;
            case UI_Business.SortType.TimeDown:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByTimeDown));
                break;
        }
        this.ShowAllInBuy(this.object_28, this.buyItemList);
    }

    public void ShowTradeBuy()
    {
        if (this._buyFirstShow)
        {
            return;
        }
        this._buyFirstShow = true;
        this.ClearBuyItem();
        this.ShowBuyItem();
    }

    public void ClearBuyItem()
    {
        Transform transform = this.btn_tab_4.transform;
        this.ClearItem(transform.parent, 0);
    }

    public void ShowBuyItem()
    {
        Transform transform = this.btn_tab_4.transform;
        this._buyFirstItemList.Clear();
        int numBuyItem = this.controller.NumBuyItem;
        this.CopyToEnough(transform, numBuyItem);
        for (int i = 1; i < transform.parent.childCount; i++)
        {
            if (i <= numBuyItem)
            {
                this._buyFirstItemList.Add(transform.parent.GetChild(i));
                this.ShowBuyItemObject(transform.parent.GetChild(i), i - 1);
            }
            else
            {
                transform.parent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void ShowBuyItemObject(Transform trans, int index)
    {
        LuaTable tab = this.controller.BuyItem[index];
        if (trans == null || tab == null)
        {
            return;
        }
        trans.Find("Panel_lv2").gameObject.SetActive(false);
        trans.Find("btn_lv1/Text").GetComponent<Text>().text = tab.GetField_String("name");
        Button component = trans.Find("btn_lv1").GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            if (trans.Find("Panel_lv2").gameObject.activeSelf)
            {
                this.ClearItem(trans, 1);
            }
            else
            {
                for (int i = 0; i < this._buyFirstItemList.Count; i++)
                {
                    if (trans.parent != this._buyFirstItemList[i])
                    {
                        this.ClearItem(this._buyFirstItemList[i], 1);
                    }
                }
                this.ShowBuySubItem(tab, trans.Find("Panel_lv2"));
                this.ShowFirstMenu(tab, trans.Find("Panel_lv2"));
            }
        });
        trans.gameObject.SetActive(true);
    }

    private void ShowFirstMenu(LuaTable tab, Transform secondItem)
    {
        this.btnBuyFresh.gameObject.SetActive(true);
        List<uint> list = new List<uint>();
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList(tab, "subitem");
        int count = tab.GetField_Table("subitem").Count;
        for (int i = 1; i < secondItem.parent.childCount; i++)
        {
            if (i <= count)
            {
                LuaTable ltb = configTableList[i - 1];
                List<LuaTable> configTableList2 = LuaConfigManager.GetConfigTableList(ltb, "minitem");
                for (int j = 0; j < configTableList2.Count; j++)
                {
                    string[] array = configTableList2[j].GetField_String("objID").Split(new char[]
                    {
                        ','
                    });
                    for (int k = 0; k < array.Length; k++)
                    {
                        list.Add((uint)array[k].ToInt());
                    }
                }
            }
        }
        this.controller.MSG_ReqSubSellingList_CS(SELLTYPE.OBJECT, list, 1U, false);
    }

    public void ShowBuySubItem(LuaTable tab, Transform secondItem)
    {
        if (tab == null)
        {
            return;
        }
        int count = tab.GetField_Table("subitem").Count;
        this.CopyToEnough(secondItem, count);
        for (int i = 1; i < secondItem.parent.childCount; i++)
        {
            if (i <= count)
            {
                this.ShowBuySubItemObject(secondItem.parent.GetChild(i), tab, i - 1);
            }
            else
            {
                secondItem.parent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void ShowBuySubItemObject(Transform trans, LuaTable tab, int subIndex)
    {
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList(tab, "subitem");
        LuaTable subTab = configTableList[subIndex];
        if (subTab == null)
        {
            return;
        }
        trans.Find("Panel_lv3").gameObject.SetActive(false);
        trans.Find("btn_lv2/Text").GetComponent<Text>().text = subTab.GetField_String("name");
        trans.Find("btn_lv2").GetComponent<Button>().onClick.RemoveAllListeners();
        trans.Find("btn_lv2").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (trans.Find("Panel_lv3").gameObject.activeSelf)
            {
                this.ClearItem(trans, 1);
            }
            else
            {
                this.filterContent.transform.localPosition = Vector3.zero;
                uint level = GlobalRegister.GetCharacterMapData(ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer).mapdata.level;
                List<LuaTable> configTableList2 = LuaConfigManager.GetConfigTableList(subTab, "minitem");
                List<uint> list = new List<uint>();
                SELLTYPE itemType = SELLTYPE.OBJECT;
                string[] array = new string[]
                {
                    "0"
                };
                for (int i = 0; i < configTableList2.Count; i++)
                {
                    string[] array2 = configTableList2[i].GetField_String("level").Split(new char[]
                    {
                        '-'
                    });
                    int num = array2[0].ToInt();
                    int num2 = (array2.Length <= 1) ? num : array2[1].ToInt();
                    if ((ulong)level >= (ulong)((long)num) && (ulong)level <= (ulong)((long)num2))
                    {
                        if (tab.GetField_String("AtlasName") == "HeroItems")
                        {
                            itemType = SELLTYPE.HERO;
                        }
                        string[] array3 = configTableList2[i].GetField_String("objID").Split(new char[]
                        {
                            ','
                        });
                        for (int j = 0; j < array3.Length; j++)
                        {
                            list.Add((uint)array3[j].ToInt());
                        }
                        array = configTableList2[i].GetField_String("filtrate").Split(new char[]
                        {
                            '-'
                        });
                        this.controller.LastReqSublistPage = 1U;
                        this.ChangeInBuy(itemType, subTab.GetField_Table("minitem"), (uint)i, trans.Find("Panel_lv3"));
                    }
                }
                if (list.Count > 0)
                {
                    this.controller.MSG_ReqSubSellingList_CS(itemType, list, (uint)array[1].ToInt(), false);
                }
            }
        });
        trans.gameObject.SetActive(true);
    }

    public void ChangeInBuy(SELLTYPE itemType, LuaTable minitem, uint curLevel, Transform thirdItem)
    {
        int count = minitem.Count;
        thirdItem.gameObject.SetActive(count >= 1);
        this.ShowBuyPanelFilter(itemType, count, minitem, curLevel, thirdItem.Find("btn_lv3"));
    }

    public void ShowBuyPanelFilter(SELLTYPE itemType, int num, LuaTable minitem, uint curLevel, Transform thirdBtn)
    {
        thirdBtn.gameObject.SetActive(false);
        this.CopyToEnough(thirdBtn, num);
        for (int i = 1; i < thirdBtn.parent.childCount; i++)
        {
            if (i <= num)
            {
                this.ShowBuyPanelFilterObject(itemType, thirdBtn.parent.GetChild(i), i - 1, minitem, curLevel);
            }
            else
            {
                thirdBtn.parent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void ShowBuyPanelFilterObject(SELLTYPE itemType, Transform trans, int index, LuaTable minitem, uint curLevel)
    {
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList(minitem, string.Empty);
        LuaTable luaTable = configTableList[index];
        if (luaTable == null)
        {
            return;
        }
        trans.Find("Text").GetComponent<Text>().text = luaTable.GetCacheField_String("name");
        string[] array = luaTable.GetField_String("objID").Split(new char[]
        {
            ','
        });
        List<uint> idList = new List<uint>();
        for (int i = 0; i < array.Length; i++)
        {
            idList.Add((uint)array[i].ToInt());
        }
        string[] filtRate = luaTable.GetField_String("filtrate").Split(new char[]
        {
            '-'
        });
        trans.GetComponent<Button>().onClick.RemoveAllListeners();
        trans.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (this._curSeleteThirdItem != null)
            {
                this._curSeleteThirdItem.Find("img_s").gameObject.SetActive(false);
            }
            trans.Find("img_s").gameObject.SetActive(true);
            this._curSeleteThirdItem = trans;
            if (this.controller.LastReqSubSell != null)
            {
                this.controller.MSG_ReqSubSellingList_CS(this.controller.LastReqSubSell.ItemType, idList, (uint)filtRate[1].ToInt(), false);
            }
        });
        UIEventListener.Get(trans.gameObject).onEnter = delegate (PointerEventData point)
        {
            trans.Find("img_h").gameObject.SetActive(true);
        };
        UIEventListener.Get(trans.gameObject).onExit = delegate (PointerEventData point)
        {
            trans.Find("img_h").gameObject.SetActive(false);
        };
        trans.gameObject.SetActive(true);
    }

    public void ShowBuyChangePage(SELLTYPE itemType, List<uint> idList, uint levelStar, uint totalPage)
    {
        if (idList.Count < 1)
        {
            return;
        }
        uint num = idList[0];
        this.ShowBuyNotGemChangePage(itemType, idList, levelStar, totalPage);
    }

    public void ShowBuyNotGemChangePage(SELLTYPE itemType, List<uint> idList, uint levelStar, uint totalPage)
    {
        if (this.controller.LastReqSublistPage < 1U)
        {
            this.controller.LastReqSublistPage = 1U;
        }
        if (totalPage < 1U)
        {
            totalPage = 1U;
        }
        if (this.controller.LastReqSublistPage > totalPage)
        {
            this.controller.LastReqSublistPage = totalPage;
        }
        this.txt_allpage_53.text = this.controller.LastReqSublistPage + "/" + totalPage;
        this.btn_add_26.onClick.RemoveAllListeners();
        this.btn_add_26.onClick.AddListener(delegate ()
        {
            TipsWindow.ShowWindow(TipsType.CAN_NOT_DISPLAY_ANY_MORE, null);
            this.controller.MSG_ReqSubSellingList_CS(itemType, idList, levelStar, false);
        });
    }

    public void ShowInBuy(List<TradeItem> list)
    {
        this.list_4.gameObject.SetActive(true);
        if (this.controller.LastReqSubSell != null)
        {
            SELLTYPE itemType = this.controller.LastReqSubSell.ItemType;
            List<uint> idList = this.controller.LastReqSubSell.IdList;
        }
        if (list == null)
        {
            return;
        }
        this.buyItemList = list;
        this.ShowAllInBuy(this.object_28, this.buyItemList);
    }

    public void ShowAllInBuy(Transform trans, List<TradeItem> itemList)
    {
        if (itemList == null)
        {
            return;
        }
        int count = itemList.Count;
        this.CopyToEnough(trans, count);
        for (int i = 1; i < trans.parent.childCount; i++)
        {
            if (i <= count)
            {
                this.ShowInBuyObject(trans.parent.GetChild(i), itemList[i - 1]);
            }
            else
            {
                trans.parent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void ShowInBuyObject(Transform trans, TradeItem item)
    {
        Image component = trans.Find("img_icon/Image").GetComponent<Image>();
        RawImage imgIconQuality = trans.Find("img_icon/quality").GetComponent<RawImage>();
        Image component2 = trans.Find("img_icon/img_true").GetComponent<Image>();
        component.gameObject.SetActive(true);
        imgIconQuality.gameObject.SetActive(false);
        Text component3 = trans.Find("txt_name").GetComponent<Text>();
        Text component4 = trans.Find("txt_num").GetComponent<Text>();
        Text component5 = trans.Find("txt_time").GetComponent<Text>();
        Text component6 = trans.Find("price/Text").GetComponent<Text>();
        Image component7 = trans.Find("price/Image").GetComponent<Image>();
        Text component8 = trans.Find("price_total/Text").GetComponent<Text>();
        Image component9 = trans.Find("price_total/Image").GetComponent<Image>();
        uint num = 1U;
        if (item.data.itemtype == SELLTYPE.OBJECT)
        {
            t_Object objdata = item.data.objdata;
            LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)objdata.baseid);
            if (configTable == null && objdata.baseid > 0U)
            {
                FFDebug.LogWarning(this, string.Format("Does not exist item with id: " + objdata.baseid, new object[0]));
                return;
            }
            GlobalRegister.SetImage(0, configTable.GetField_String("icon"), component, true);
            component2.gameObject.SetActive(this.CheckIsGemOrNot(objdata.baseid));
            string imgname = "quality" + configTable.GetField_Uint("quality");
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.RANK, imgname, delegate (UITextureAsset asset)
            {
                if (asset == null)
                {
                    return;
                }
                if (imgIconQuality == null)
                {
                    return;
                }
                imgIconQuality.gameObject.SetActive(true);
                imgIconQuality.texture = asset.textureObj;
            });
            component3.text = objdata.name;
            component4.text = objdata.num.ToString();
            component6.text = item.price.ToString();
            num = objdata.num;
        }
        else if (item.data.itemtype == SELLTYPE.HERO)
        {
            Hero herodata = item.data.herodata;
            LuaTable configTable2 = LuaConfigManager.GetConfigTable("npc_data", (ulong)herodata.baseid);
            if (configTable2 == null && herodata.baseid > 0U)
            {
                FFDebug.LogWarning(this, string.Format("Does not exist item with id: " + herodata.baseid, new object[0]));
                return;
            }
            GlobalRegister.SetImage(0, configTable2.GetField_String("icon"), component, true);
            component3.text = configTable2.GetField_String("name");
            component4.text = "1";
            component6.text = item.price.ToString();
        }
        int currServerTimeBySecond = (int)SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond();
        int num2 = currServerTimeBySecond - (int)item.selltime;
        int field_Int = LuaConfigManager.GetXmlConfigTable("trade").GetField_Int("sell_duration");
        if (num2 <= 0)
        {
            num2 = 1;
        }
        component5.text = this.SecondToTime(field_Int - num2);
        component8.text = (item.price * num).ToString();
        GlobalRegister.SetImage(0, this.controller.MoneyIcon, component9, true);
        GlobalRegister.SetImage(0, this.controller.MoneyIcon, component7, true);
        Button btnBuy = trans.Find("btn_buy").GetComponent<Button>();
        btnBuy.onClick.RemoveAllListeners();
        btnBuy.onClick.AddListener(delegate ()
        {
            if (item.data.itemtype == SELLTYPE.OBJECT)
            {
                this.controller.MSG_ReqBuyItem_CS(item.data.itemtype, item.data.objdata.baseid, item.data.objdata.level, item.data.objdata.num, item.thisid);
            }
            else if (item.data.itemtype == SELLTYPE.HERO)
            {
                this.controller.MSG_ReqBuyItem_CS(item.data.itemtype, item.data.herodata.baseid, item.data.herodata.level, 1U, item.thisid);
            }
        });
        trans.GetComponent<Button>().onClick.RemoveAllListeners();
        trans.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (this._curBuyItem != null)
            {
                this._curBuyItem.Find("btn_buy").gameObject.SetActive(false);
            }
            this._curBuyItem = trans;
            btnBuy.gameObject.SetActive(true);
        });
        trans.gameObject.SetActive(true);
        UIEventListener.Get(trans.Find("img_icon").gameObject).onEnter = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(item.data.objdata, trans.Find("img_icon").gameObject);
        };
        UIEventListener.Get(trans.Find("img_icon").gameObject).onExit = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().ClosePanel();
        };
    }

    public void InitBusinessNotice()
    {
        this.btn_tab_46 = this.panelRoot.Find("Panel_show/Scroll_filter/Viewport/Content/panel_lv1");
        this.list_55 = this.panelRoot.Find("Panel_show/Panel_list/Scroll View");
        this.object_57 = this.panelRoot.Find("Panel_show/Panel_list/Scroll View/Viewport/Content/Panel_info");
        this.btn_minus_61 = this.panelRoot.Find("Panel_show/Panel_list/bottom/Panel_page/btn_previous").GetComponent<Button>();
        this.btn_add_63 = this.panelRoot.Find("Panel_show/Panel_list/bottom/Panel_page/btn_next").GetComponent<Button>();
        this.txt_allpage_54 = this.panelRoot.Find("Panel_show/Panel_list/bottom/Panel_page/Text").GetComponent<Text>();
        this.btnNoticeNumSortUp = this.panelRoot.Find("Panel_show/Panel_list/title/txt2/btn_up").GetComponent<Button>();
        this.btnNoticeNumSortDown = this.panelRoot.Find("Panel_show/Panel_list/title/txt2/btn_down").GetComponent<Button>();
        this.btnNoticePriceSortUp = this.panelRoot.Find("Panel_show/Panel_list/title/txt3/btn_up").GetComponent<Button>();
        this.btnNoticePriceSortDown = this.panelRoot.Find("Panel_show/Panel_list/title/txt3/btn_down").GetComponent<Button>();
        this.btnNoticeAllPriceSortUp = this.panelRoot.Find("Panel_show/Panel_list/title/txt4/btn_up").GetComponent<Button>();
        this.btnNoticeAllPriceSortDown = this.panelRoot.Find("Panel_show/Panel_list/title/txt4/btn_up").GetComponent<Button>();
        this.btnNoticeTimeSortUp = this.panelRoot.Find("Panel_show/Panel_list/title/txt4/btn_up").GetComponent<Button>();
        this.btnNoticeTimeSortDown = this.panelRoot.Find("Panel_show/Panel_list/title/txt4/btn_up").GetComponent<Button>();
        this.btnNoticeFresh = this.panelRoot.Find("Panel_show/Panel_list/bottom/btn_fresh").GetComponent<Button>();
        this.object_57.gameObject.SetActive(false);
        this.InitBusinessNoticeEvent();
    }

    public void InitBusinessNoticeEvent()
    {
        this.btnNoticeNumSortUp.onClick.RemoveAllListeners();
        this.btnNoticeNumSortUp.onClick.AddListener(delegate ()
        {
            this.SortNoticeItemList(UI_Business.SortType.NumUp);
        });
        this.btnNoticeNumSortDown.onClick.RemoveAllListeners();
        this.btnNoticeNumSortDown.onClick.AddListener(delegate ()
        {
            this.SortNoticeItemList(UI_Business.SortType.NumDown);
        });
        this.btnNoticePriceSortUp.onClick.RemoveAllListeners();
        this.btnNoticePriceSortUp.onClick.AddListener(delegate ()
        {
            this.SortNoticeItemList(UI_Business.SortType.PriceUp);
        });
        this.btnNoticePriceSortDown.onClick.RemoveAllListeners();
        this.btnNoticePriceSortDown.onClick.AddListener(delegate ()
        {
            this.SortNoticeItemList(UI_Business.SortType.PriceDown);
        });
        this.btnNoticeAllPriceSortUp.onClick.RemoveAllListeners();
        this.btnNoticeAllPriceSortUp.onClick.AddListener(delegate ()
        {
            this.SortNoticeItemList(UI_Business.SortType.AllPriceUp);
        });
        this.btnNoticeAllPriceSortDown.onClick.RemoveAllListeners();
        this.btnNoticeAllPriceSortDown.onClick.AddListener(delegate ()
        {
            this.SortNoticeItemList(UI_Business.SortType.AllPriceDown);
        });
        this.btnNoticeTimeSortUp.onClick.RemoveAllListeners();
        this.btnNoticeTimeSortUp.onClick.AddListener(delegate ()
        {
            this.SortNoticeItemList(UI_Business.SortType.TimeUp);
        });
        this.btnNoticeTimeSortDown.onClick.RemoveAllListeners();
        this.btnNoticeTimeSortDown.onClick.AddListener(delegate ()
        {
            this.SortNoticeItemList(UI_Business.SortType.TimeDown);
        });
        this.btnNoticeFresh.onClick.RemoveAllListeners();
        this.btnNoticeFresh.onClick.AddListener(delegate ()
        {
            if (this.controller.LastReqSubSell != null)
            {
                this.controller.MSG_ReqSubSellingList_CS(this.controller.LastReqSubSell.ItemType, this.controller.LastReqSubSell.IdList, this.controller.LastReqSubSell.LevelStar, true);
            }
        });
    }

    private void SortNoticeItemList(UI_Business.SortType type)
    {
        if (this.buyItemList == null || this.buyItemList.Count < 1)
        {
            return;
        }
        switch (type)
        {
            case UI_Business.SortType.NumUp:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByNumUp));
                break;
            case UI_Business.SortType.NumDown:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByNumDown));
                break;
            case UI_Business.SortType.PriceUp:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByPriceUp));
                break;
            case UI_Business.SortType.PriceDown:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByPriceDown));
                break;
            case UI_Business.SortType.AllPriceUp:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByAllPriceUp));
                break;
            case UI_Business.SortType.AllPriceDown:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByAllPriceDown));
                break;
            case UI_Business.SortType.TimeUp:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByTimeUp));
                break;
            case UI_Business.SortType.TimeDown:
                this.buyItemList.Sort(new Comparison<TradeItem>(this.SortByTimeDown));
                break;
        }
        this.ShowAllInNotice(this.object_57, this.noticeItemList);
    }

    public void ShowTradeNotice()
    {
        if (this._noticeFirstShow)
        {
            return;
        }
        this._noticeFirstShow = true;
        this.ClearNoticeItem();
        this.ShowNoticeItem();
    }

    public void ClearNoticeItem()
    {
        Transform transform = this.btn_tab_46.transform;
        this.ClearItem(transform.parent, 0);
    }

    public void ShowNoticeItem()
    {
        Transform transform = this.btn_tab_46.transform;
        this._noticeFirstItemList.Clear();
        int numBuyItem = this.controller.NumBuyItem;
        this.CopyToEnough(transform, numBuyItem);
        for (int i = 1; i < transform.parent.childCount; i++)
        {
            if (i <= numBuyItem)
            {
                this._noticeFirstItemList.Add(transform.parent.GetChild(i));
                this.ShowNoticeItemObject(transform.parent.GetChild(i), i - 1);
            }
            else
            {
                transform.parent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void ShowNoticeItemObject(Transform trans, int index)
    {
        LuaTable tab = this.controller.BuyItem[index];
        if (trans == null || tab == null)
        {
            return;
        }
        if (tab.GetField_Int("isGem") == 0)
        {
            trans.gameObject.SetActive(false);
            return;
        }
        trans.Find("Panel_lv2").gameObject.SetActive(false);
        trans.Find("btn_lv1/Text").GetComponent<Text>().text = tab.GetField_String("name");
        Button component = trans.Find("btn_lv1").GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            if (trans.Find("Panel_lv2").gameObject.activeSelf)
            {
                this.ClearItem(trans, 1);
            }
            else
            {
                this.ShowNoticeSubItem(tab, trans.Find("Panel_lv2"));
            }
        });
        trans.gameObject.SetActive(true);
    }

    public void ShowNoticeSubItem(LuaTable tab, Transform secondItem)
    {
        if (tab == null)
        {
            return;
        }
        int count = tab.GetField_Table("subitem").Count;
        this.CopyToEnough(secondItem, count);
        for (int i = 1; i < secondItem.parent.childCount; i++)
        {
            if (i <= count)
            {
                this.ShowNoticeSubItemObject(secondItem.parent.GetChild(i), tab, i - 1);
            }
            else
            {
                secondItem.parent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void ShowNoticeSubItemObject(Transform trans, LuaTable tab, int subIndex)
    {
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList(tab, "subitem");
        LuaTable subTab = configTableList[subIndex];
        if (subTab == null)
        {
            return;
        }
        if (tab.GetField_Int("isGem") == 0)
        {
            trans.gameObject.SetActive(false);
            return;
        }
        trans.Find("Panel_lv3").gameObject.SetActive(false);
        trans.Find("btn_lv2/Text").GetComponent<Text>().text = subTab.GetField_String("name");
        trans.Find("btn_lv2").GetComponent<Button>().onClick.RemoveAllListeners();
        trans.Find("btn_lv2").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (trans.Find("Panel_lv3").gameObject.activeSelf)
            {
                this.ClearItem(trans, 1);
            }
            else
            {
                for (int i = 0; i < this._noticeFirstItemList.Count; i++)
                {
                    if (trans.parent != this._noticeFirstItemList[i])
                    {
                        this.ClearItem(this._noticeFirstItemList[i], 1);
                    }
                }
                uint level = GlobalRegister.GetCharacterMapData(ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer).mapdata.level;
                List<LuaTable> configTableList2 = LuaConfigManager.GetConfigTableList(subTab, "minitem");
                for (int j = 0; j < configTableList2.Count; j++)
                {
                    string[] array = configTableList2[j].GetField_String("level").Split(new char[]
                    {
                        '-'
                    });
                    int num = array[0].ToInt();
                    int num2 = (array.Length <= 1) ? num : array[1].ToInt();
                    if ((ulong)level >= (ulong)((long)num) && (ulong)level <= (ulong)((long)num2))
                    {
                        SELLTYPE itemType = SELLTYPE.OBJECT;
                        if (tab.GetField_String("AtlasName") == "HeroItems")
                        {
                            itemType = SELLTYPE.HERO;
                        }
                        string[] array2 = configTableList2[j].GetField_String("objID").Split(new char[]
                        {
                            ','
                        });
                        List<uint> list = new List<uint>();
                        for (int k = 0; k < array2.Length; k++)
                        {
                            list.Add((uint)array2[k].ToInt());
                        }
                        string[] array3 = configTableList2[j].GetField_String("filtrate").Split(new char[]
                        {
                            '-'
                        });
                        this.controller.LastReqSublistPage = 1U;
                        if (j == 0)
                        {
                            this.controller.MSG_ReqSubSellingList_CS(itemType, list, (uint)array3[1].ToInt(), true);
                        }
                        this.ChangeInNotice(itemType, subTab.GetField_Table("minitem"), (uint)j, trans.Find("Panel_lv3"));
                    }
                }
            }
        });
        trans.gameObject.SetActive(true);
    }

    public void ChangeInNotice(SELLTYPE itemType, LuaTable minitem, uint curLevel, Transform thirdItem)
    {
        int count = minitem.Count;
        thirdItem.gameObject.SetActive(count > 1);
        this.ShowNoticePanelFilter(itemType, count, minitem, curLevel, thirdItem.Find("btn_lv3"));
    }

    public void ShowNoticePanelFilter(SELLTYPE itemType, int num, LuaTable minitem, uint curLevel, Transform thirdBtn)
    {
        thirdBtn.gameObject.SetActive(false);
        this.CopyToEnough(thirdBtn, num);
        for (int i = 1; i < thirdBtn.parent.childCount; i++)
        {
            if (i <= num)
            {
                this.ShowNoticePanelFilterObject(itemType, thirdBtn.parent.GetChild(i), i - 1, minitem, curLevel);
            }
            else
            {
                thirdBtn.parent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void ShowNoticePanelFilterObject(SELLTYPE itemType, Transform trans, int index, LuaTable minitem, uint curLevel)
    {
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList(minitem, string.Empty);
        LuaTable luaTable = configTableList[index];
        if (luaTable == null)
        {
            return;
        }
        trans.Find("Text").GetComponent<Text>().text = luaTable.GetCacheField_String("name");
        string[] array = luaTable.GetField_String("objID").Split(new char[]
        {
            ','
        });
        List<uint> idList = new List<uint>();
        for (int i = 0; i < array.Length; i++)
        {
            idList.Add((uint)array[i].ToInt());
        }
        string[] filtRate = luaTable.GetField_String("filtrate").Split(new char[]
        {
            '-'
        });
        trans.GetComponent<Button>().onClick.RemoveAllListeners();
        trans.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (this.controller.LastReqSubSell != null)
            {
                this.controller.MSG_ReqSubSellingList_CS(this.controller.LastReqSubSell.ItemType, idList, (uint)filtRate[1].ToInt(), true);
            }
        });
        trans.gameObject.SetActive(true);
    }

    public void ShowNoticeChangePage(SELLTYPE itemType, List<uint> idList, uint levelStar, uint totalPage)
    {
        if (idList.Count < 1)
        {
            return;
        }
        uint num = idList[0];
        this.ShowNoticeNotGemChangePage(itemType, idList, levelStar, totalPage);
    }

    public void ShowNoticeNotGemChangePage(SELLTYPE itemType, List<uint> idList, uint levelStar, uint totalPage)
    {
        if (this.controller.LastReqSublistPage < 1U)
        {
            this.controller.LastReqSublistPage = 1U;
        }
        if (totalPage < 1U)
        {
            totalPage = 1U;
        }
        if (this.controller.LastReqSublistPage > totalPage)
        {
            this.controller.LastReqSublistPage = totalPage;
        }
        this.txt_allpage_54.text = "/" + totalPage;
        this.btn_add_63.onClick.RemoveAllListeners();
        this.btn_add_63.onClick.AddListener(delegate ()
        {
            TipsWindow.ShowWindow(TipsType.CAN_NOT_DISPLAY_ANY_MORE, null);
            this.controller.MSG_ReqSubSellingList_CS(itemType, idList, levelStar, true);
        });
    }

    public void ShowInNotice(List<TradeItem> list, bool isLove = false)
    {
        this.list_55.gameObject.SetActive(true);
        if (this.controller.LastReqSubSell != null)
        {
            SELLTYPE itemType = this.controller.LastReqSubSell.ItemType;
            List<uint> idList = this.controller.LastReqSubSell.IdList;
        }
        if (list == null)
        {
            return;
        }
        this.noticeItemList = list;
        this.ShowAllInNotice(this.object_57, this.noticeItemList);
    }

    private void SetListDragNotice()
    {
        this.controller.NeedListenDrag = true;
        float oriLocalPos = this.list_55.GetComponent<ScrollRect>().content.localPosition.y;
        float max = this.list_55.Find("Viewport/Content/Panel_info").GetComponent<LayoutElement>().preferredHeight;
        this.list_55.GetComponent<ScrollRect>().onValueChanged.RemoveAllListeners();
        this.list_55.GetComponent<ScrollRect>().onValueChanged.AddListener(delegate (Vector2 b)
        {
            if (this.controller.NeedListenDrag)
            {
                float y = this.list_55.GetComponent<ScrollRect>().content.localPosition.y;
                if (b.y == 0f && y - oriLocalPos > max)
                {
                    this.controller.NeedListenDrag = false;
                    this.controller.LastReqSublistPage += 1U;
                    if (this.controller.LastReqSubSell != null)
                    {
                        this.controller.LastReqSublistPage -= 1U;
                        TipsWindow.ShowWindow(TipsType.CAN_NOT_DISPLAY_ANY_MORE, null);
                        this.controller.MSG_ReqSubSellingList_CS(this.controller.LastReqSubSell.ItemType, this.controller.LastReqSubSell.IdList, this.controller.LastReqSubSell.LevelStar, true);
                    }
                }
                else if (b.y == 1f && y - oriLocalPos < -1f * max)
                {
                    this.controller.NeedListenDrag = false;
                    if (this.controller.LastReqSublistPage > 1U)
                    {
                        this.controller.LastReqSublistPage -= 1U;
                    }
                    else if (this.controller.LastReqSubSell != null)
                    {
                        TipsWindow.ShowWindow(TipsType.CAN_NOT_DISPLAY_ANY_MORE, null);
                    }
                    if (this.controller.LastReqSubSell != null)
                    {
                        this.controller.MSG_ReqSubSellingList_CS(this.controller.LastReqSubSell.ItemType, this.controller.LastReqSubSell.IdList, this.controller.LastReqSubSell.LevelStar, true);
                    }
                }
            }
        });
    }

    public void ShowAllInNotice(Transform trans, List<TradeItem> itemList)
    {
        if (itemList == null)
        {
            return;
        }
        int count = itemList.Count;
        this.CopyToEnough(trans, count);
        for (int i = 1; i < trans.parent.childCount; i++)
        {
            if (i <= count)
            {
                this.ShowInNoticeObject(trans.parent.GetChild(i), itemList[i - 1]);
            }
            else
            {
                trans.parent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void ShowInNoticeObject(Transform trans, TradeItem item)
    {
        Image imgIcon = trans.Find("img_icon/Image").GetComponent<Image>();
        UIEventListener.Get(imgIcon.gameObject).onEnter = delegate (PointerEventData ed)
        {
            this.TryShowItemTip(item.data.objdata, imgIcon.gameObject);
        };
        UIEventListener.Get(imgIcon.gameObject).onExit = delegate (PointerEventData ed)
        {
            this.CloseItemTip();
        };
        Image component = trans.Find("img_icon/img_true").GetComponent<Image>();
        imgIcon.gameObject.SetActive(true);
        Text component2 = trans.Find("txt_name").GetComponent<Text>();
        Text component3 = trans.Find("txt_num").GetComponent<Text>();
        Text component4 = trans.Find("txt_time").GetComponent<Text>();
        Text component5 = trans.Find("price/Text").GetComponent<Text>();
        Image component6 = trans.Find("price/Image").GetComponent<Image>();
        Text component7 = trans.Find("price_total/Text").GetComponent<Text>();
        Image component8 = trans.Find("price_total/Image").GetComponent<Image>();
        uint num = 1U;
        if (item.data.itemtype == SELLTYPE.OBJECT)
        {
            t_Object objdata = item.data.objdata;
            LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)objdata.baseid);
            if (configTable == null && objdata.baseid > 0U)
            {
                FFDebug.LogWarning(this, string.Format("Does not exist item with id: " + objdata.baseid, new object[0]));
                return;
            }
            GlobalRegister.SetImage(0, configTable.GetField_String("icon"), imgIcon, true);
            component.gameObject.SetActive(this.CheckIsGemOrNot(objdata.baseid));
            component2.text = objdata.name;
            component3.text = objdata.num.ToString();
            component5.text = item.price.ToString();
            num = objdata.num;
        }
        else if (item.data.itemtype == SELLTYPE.HERO)
        {
            Hero herodata = item.data.herodata;
            LuaTable configTable2 = LuaConfigManager.GetConfigTable("npc_data", (ulong)herodata.baseid);
            if (configTable2 == null && herodata.baseid > 0U)
            {
                FFDebug.LogWarning(this, string.Format("Does not exist item with id: " + herodata.baseid, new object[0]));
                return;
            }
            GlobalRegister.SetImage(0, configTable2.GetField_String("icon"), imgIcon, true);
            component2.text = configTable2.GetField_String("name");
            component3.text = "1";
            component5.text = item.price.ToString();
        }
        int currServerTimeBySecond = (int)SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond();
        int num2 = currServerTimeBySecond - (int)item.selltime;
        int field_Int = LuaConfigManager.GetXmlConfigTable("trade").GetField_Int("show_duration");
        if (num2 <= 0)
        {
            num2 = 1;
        }
        component4.text = this.SecondToTime(field_Int - num2);
        component7.text = (item.price * num).ToString();
        GlobalRegister.SetImage(0, this.controller.MoneyIcon, component8, true);
        GlobalRegister.SetImage(0, this.controller.MoneyIcon, component6, true);
        trans.gameObject.SetActive(true);
    }

    public void InitBusinessSell()
    {
        this.list_0 = this.panelRoot.Find("Panel_sell/Panel_list/Scroll View").GetComponent<ScrollRect>();
        this.Rect_9 = this.panelRoot.Find("Panel_sell/Panel_list/Scroll View/Viewport/Content");
        this.object_10 = this.panelRoot.Find("Panel_sell/Panel_list/Scroll View/Viewport/Content/Panel_info");
        this.img_goldicon_1 = this.panelRoot.Find("Panel_sell/Panel_list/Scroll View/Viewport/Content/Panel_info/price/Image").GetComponent<Image>();
        this.txt_goldvalue_3 = this.panelRoot.Find("Panel_sell/Panel_list/Scroll View/Viewport/Content/Panel_info/price/Text").GetComponent<Text>();
        this.txt_time_4 = this.panelRoot.Find("Panel_sell/Panel_list/Scroll View/Viewport/Content/Panel_info/txt_time").GetComponent<Text>();
        this.txt_count1_5 = this.panelRoot.Find("Panel_sell/Panel_list/bottom/text/value").GetComponent<Text>();
        this.btn_record_4 = this.panelRoot.Find("Panel_toggle/btn_record").GetComponent<Button>();
        this.Panel_SaleItem_17 = this.panelRoot.Find("Panel_sell/Panel_on/Info");
        this.imgSellItem = this.panelRoot.Find("Panel_sell/Panel_on/Info/img_icon/Image").GetComponent<Image>();
        this.imgSellItemQuality = this.panelRoot.Find("Panel_sell/Panel_on/Info/img_icon/quality").GetComponent<RawImage>();
        this.imgSellItemGem = this.panelRoot.Find("Panel_sell/Panel_on/Info/img_icon/img_true").GetComponent<Image>();
        this.txtSellPrice = this.panelRoot.Find("Panel_sell/Panel_on/Info/insert_price/Panel/txt_value").GetComponent<Text>();
        this.imgSellPrice = this.panelRoot.Find("Panel_sell/Panel_on/Info/insert_price/Panel/Image").GetComponent<Image>();
        this.btn_inputnumber_1_0 = this.panelRoot.Find("Panel_sell/Panel_on/Info/insert_price/btn_inputnumberprice").GetComponent<Button>();
        this.input_number2_1 = this.panelRoot.Find("Panel_sell/Panel_on/Info/insert_num/InputField").GetComponent<InputField>();
        this.btn_inputnumber_2_1 = this.panelRoot.Find("Panel_sell/Panel_on/Info/insert_num/btn_inputnumbernum").GetComponent<Button>();
        this.img_icon3_6 = this.panelRoot.Find("Panel_sell/Panel_on/Info/txt_selltotal/Panel/Image").GetComponent<Image>();
        this.txt_value3_9 = this.panelRoot.Find("Panel_sell/Panel_on/Info/txt_selltotal/Panel/txt_value").GetComponent<Text>();
        this.img_icon4_7 = this.panelRoot.Find("Panel_sell/Panel_on/Info/txt_revenue/Panel/Image").GetComponent<Image>();
        this.txt_value4_10 = this.panelRoot.Find("Panel_sell/Panel_on/Info/txt_revenue/Panel/txt_value").GetComponent<Text>();
        this.btn_sale_10 = this.panelRoot.Find("Panel_sell/Panel_on/Info/Button").GetComponent<Button>();
        this.txtSellMinPrice = this.panelRoot.Find("Panel_sell/Panel_on/Info/Panel_section/txt_floor/Panel/txt_value").GetComponent<Text>();
        this.txtSellMaxPrice = this.panelRoot.Find("Panel_sell/Panel_on/Info/Panel_section/txt_ceiling/Panel/txt_value").GetComponent<Text>();
        this.imgSellMinPrice = this.panelRoot.Find("Panel_sell/Panel_on/Info/Panel_section/txt_floor/Panel/Image").GetComponent<Image>();
        this.imgSellMaxPrice = this.panelRoot.Find("Panel_sell/Panel_on/Info/Panel_section/txt_ceiling/Panel/Image").GetComponent<Image>();
        this.txtSellShowTime = this.panelRoot.Find("Panel_sell/Panel_on/Info/Panel_section/txt_showtime/txt_value").GetComponent<Text>();
        this.txtSellSellTime = this.panelRoot.Find("Panel_sell/Panel_on/Info/Panel_section/txt_selltime/txt_value").GetComponent<Text>();
        this.btnSellAddPrice = this.panelRoot.Find("Panel_sell/Panel_on/Info/insert_price/btn_add").GetComponent<Button>();
        this.btnSellSubPrice = this.panelRoot.Find("Panel_sell/Panel_on/Info/insert_price/btn_sub").GetComponent<Button>();
        this.btnSellNumSortUp = this.panelRoot.Find("Panel_sell/Panel_list/title/txt2/btn_up").GetComponent<Button>();
        this.btnSellNumSortDown = this.panelRoot.Find("Panel_sell/Panel_list/title/txt2/btn_down").GetComponent<Button>();
        this.btnSellPriceSortUp = this.panelRoot.Find("Panel_sell/Panel_list/title/txt3/btn_up").GetComponent<Button>();
        this.btnSellPriceSortDown = this.panelRoot.Find("Panel_sell/Panel_list/title/txt3/btn_down").GetComponent<Button>();
        this.btnSellAllPriceSortUp = this.panelRoot.Find("Panel_sell/Panel_list/title/txt4/btn_up").GetComponent<Button>();
        this.btnSellAllPriceSortDown = this.panelRoot.Find("Panel_sell/Panel_list/title/txt4/btn_down").GetComponent<Button>();
        this.btnSellTimeSortUp = this.panelRoot.Find("Panel_sell/Panel_list/title/txt5/btn_up").GetComponent<Button>();
        this.btnSellTimeSortDown = this.panelRoot.Find("Panel_sell/Panel_list/title/txt5/btn_down").GetComponent<Button>();
        this.btn_inputnumber_2_1.gameObject.SetActive(false);
        this.imgSellItem.gameObject.SetActive(true);
        this.imgSellItemQuality.gameObject.SetActive(false);
        this.object_10.gameObject.SetActive(false);
        this.InitBusinessSellEvent();
        this.ClearSellItem();
    }

    public void InitBusinessSellEvent()
    {
        this.btnSellNumSortUp.onClick.RemoveAllListeners();
        this.btnSellNumSortUp.onClick.AddListener(delegate ()
        {
            this.SortSellItemList(UI_Business.SortType.NumUp);
        });
        this.btnSellNumSortDown.onClick.RemoveAllListeners();
        this.btnSellNumSortDown.onClick.AddListener(delegate ()
        {
            this.SortSellItemList(UI_Business.SortType.NumDown);
        });
        this.btnSellPriceSortUp.onClick.RemoveAllListeners();
        this.btnSellPriceSortUp.onClick.AddListener(delegate ()
        {
            this.SortSellItemList(UI_Business.SortType.PriceUp);
        });
        this.btnSellPriceSortDown.onClick.RemoveAllListeners();
        this.btnSellPriceSortDown.onClick.AddListener(delegate ()
        {
            this.SortSellItemList(UI_Business.SortType.PriceDown);
        });
        this.btnSellAllPriceSortUp.onClick.RemoveAllListeners();
        this.btnSellAllPriceSortUp.onClick.AddListener(delegate ()
        {
            this.SortSellItemList(UI_Business.SortType.AllPriceUp);
        });
        this.btnSellAllPriceSortDown.onClick.RemoveAllListeners();
        this.btnSellAllPriceSortDown.onClick.AddListener(delegate ()
        {
            this.SortSellItemList(UI_Business.SortType.AllPriceDown);
        });
        this.btnSellTimeSortUp.onClick.RemoveAllListeners();
        this.btnSellTimeSortUp.onClick.AddListener(delegate ()
        {
            this.SortSellItemList(UI_Business.SortType.TimeUp);
        });
        this.btnSellTimeSortDown.onClick.RemoveAllListeners();
        this.btnSellTimeSortDown.onClick.AddListener(delegate ()
        {
            this.SortSellItemList(UI_Business.SortType.TimeDown);
        });
    }

    private void SortSellItemList(UI_Business.SortType type)
    {
        if (this.sellItemList == null || this.sellItemList.Count < 1)
        {
            return;
        }
        switch (type)
        {
            case UI_Business.SortType.NumUp:
                this.sellItemList.Sort(new Comparison<TradeItem>(this.SortByNumUp));
                break;
            case UI_Business.SortType.NumDown:
                this.sellItemList.Sort(new Comparison<TradeItem>(this.SortByNumDown));
                break;
            case UI_Business.SortType.PriceUp:
                this.sellItemList.Sort(new Comparison<TradeItem>(this.SortByPriceUp));
                break;
            case UI_Business.SortType.PriceDown:
                this.sellItemList.Sort(new Comparison<TradeItem>(this.SortByPriceDown));
                break;
            case UI_Business.SortType.AllPriceUp:
                this.sellItemList.Sort(new Comparison<TradeItem>(this.SortByAllPriceUp));
                break;
            case UI_Business.SortType.AllPriceDown:
                this.sellItemList.Sort(new Comparison<TradeItem>(this.SortByAllPriceDown));
                break;
            case UI_Business.SortType.TimeUp:
                this.sellItemList.Sort(new Comparison<TradeItem>(this.SortByTimeUp));
                break;
            case UI_Business.SortType.TimeDown:
                this.sellItemList.Sort(new Comparison<TradeItem>(this.SortByTimeDown));
                break;
        }
        this.ShowAllInSale(this.object_10, this.sellItemList);
    }

    public void ShowTradeSale()
    {
        this.controller.MSG_ReqSellingStaff_CS();
        LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel("UI_Bag");
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

    public void ShowInSale(MSG_RetSellingStaff_SC callBack)
    {
        if (callBack == null)
        {
            return;
        }
        this.sellItemList = callBack.itemlist;
        this.ShowInSaleNum(callBack.itemlist.Count);
        this.ShowAllInSale(this.object_10, callBack.itemlist);
    }

    public void ShowInSaleNum(int num)
    {
        string field_String = LuaConfigManager.GetXmlConfigTable("trade").GetField_String("user_selling_place");
        if (field_String == null)
        {
            return;
        }
        this.txt_count1_5.text = num + "/" + field_String;
    }

    public void ShowAllInSale(Transform trans, List<TradeItem> tradeItemList)
    {
        if (trans == null || tradeItemList == null)
        {
            return;
        }
        Transform parent = trans.parent;
        if (parent == null)
        {
            return;
        }
        int count = tradeItemList.Count;
        this.CopyToEnough(trans, count);
        for (int i = 1; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child != null)
            {
                if (i <= count)
                {
                    this.ShowInSaleObject(child, tradeItemList[i - 1]);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ShowInSaleObject(Transform trans, TradeItem item)
    {
        Image component = trans.Find("img_icon/Image").GetComponent<Image>();
        RawImage imgIconQuality = trans.Find("img_icon/quality").GetComponent<RawImage>();
        component.gameObject.SetActive(true);
        imgIconQuality.gameObject.SetActive(false);
        Image component2 = trans.Find("img_icon/img_true").GetComponent<Image>();
        Text component3 = trans.Find("txt_name").GetComponent<Text>();
        Text component4 = trans.Find("txt_num").GetComponent<Text>();
        Text component5 = trans.Find("txt_time").GetComponent<Text>();
        Text component6 = trans.Find("price/Text").GetComponent<Text>();
        Image component7 = trans.Find("price/Image").GetComponent<Image>();
        Text component8 = trans.Find("price_total/Text").GetComponent<Text>();
        Image component9 = trans.Find("price_total/Image").GetComponent<Image>();
        Button btnOff = trans.Find("btn_off").GetComponent<Button>();
        uint num = 1U;
        if (item.data.itemtype == SELLTYPE.OBJECT)
        {
            t_Object objdata = item.data.objdata;
            LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)objdata.baseid);
            if (configTable == null && objdata.baseid > 0U)
            {
                FFDebug.LogWarning(this, string.Format("Does not exist item with id: " + objdata.baseid, new object[0]));
                return;
            }
            GlobalRegister.SetImage(0, configTable.GetField_String("icon"), component, true);
            component2.gameObject.SetActive(this.CheckIsGemOrNot(objdata.baseid));
            string imgname = "quality" + configTable.GetField_Uint("quality");
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.RANK, imgname, delegate (UITextureAsset asset)
            {
                if (asset == null)
                {
                    return;
                }
                if (imgIconQuality == null)
                {
                    return;
                }
                imgIconQuality.gameObject.SetActive(true);
                imgIconQuality.texture = asset.textureObj;
            });
            component3.text = objdata.name;
            component4.text = objdata.num.ToString();
            component6.text = item.price.ToString();
            num = objdata.num;
        }
        else if (item.data.itemtype == SELLTYPE.HERO)
        {
            Hero herodata = item.data.herodata;
            LuaTable configTable2 = LuaConfigManager.GetConfigTable("npc_data", (ulong)herodata.baseid);
            if (configTable2 == null && herodata.baseid > 0U)
            {
                FFDebug.LogWarning(this, string.Format("Does not exist item with id: " + herodata.baseid, new object[0]));
                return;
            }
            GlobalRegister.SetImage(0, configTable2.GetField_String("icon"), component, true);
            component3.text = configTable2.GetField_String("name");
            component4.text = "1";
            component6.text = item.price.ToString();
        }
        int currServerTimeBySecond = (int)SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond();
        int num2 = currServerTimeBySecond - (int)item.selltime;
        int maxTime = LuaConfigManager.GetXmlConfigTable("trade").GetField_Int("sell_duration");
        if (num2 <= 0)
        {
            num2 = 1;
        }
        component5.text = ((!item.isshow) ? this.SecondToTime(maxTime - num2) : "正在公示");
        component8.text = (item.price * num).ToString();
        GlobalRegister.SetImage(0, this.controller.MoneyIcon, component9, true);
        GlobalRegister.SetImage(0, this.controller.MoneyIcon, component7, true);
        btnOff.onClick.RemoveAllListeners();
        btnOff.onClick.AddListener(delegate ()
        {
            int num3 = maxTime - (int)(SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond() - item.selltime);
            if (num3 > 0)
            {
                this.controller.MSG_ReqStopSellStaff_CS(item.thisid);
            }
            else
            {
                TipsWindow.ShowWindow(TipsType.HAVE_SALE_OUT, null);
            }
            if (item.data.itemtype == SELLTYPE.OBJECT)
            {
                this.controller.MSG_ReqTradeItemHistory_CS(item.data.itemtype, item.data.objdata.baseid);
            }
            else if (item.data.itemtype == SELLTYPE.HERO)
            {
                this.controller.MSG_ReqTradeItemHistory_CS(item.data.itemtype, item.data.herodata.baseid);
            }
        });
        trans.GetComponent<Button>().onClick.RemoveAllListeners();
        trans.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (this._curSellItem != null)
            {
                this._curSellItem.Find("btn_off").gameObject.SetActive(false);
            }
            this._curSellItem = trans;
            btnOff.gameObject.SetActive(true);
        });
        trans.gameObject.SetActive(true);
        UIEventListener.Get(trans.Find("img_icon").gameObject).onEnter = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(item.data.objdata, trans.Find("img_icon").gameObject);
        };
        UIEventListener.Get(trans.Find("img_icon").gameObject).onExit = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().ClosePanel();
        };
    }

    public void ShowPropPutAway(PropsBase prop, uint price)
    {
        if (this.controller.ReqProp == null)
        {
            return;
        }
        GlobalRegister.SetImage(0, this.controller.ReqProp.config.GetField_String("icon"), this.imgSellItem, true);
        UIEventListener.Get(this.imgSellItem.gameObject).onClick = delegate (PointerEventData ed)
        {
            if (ed.button == PointerEventData.InputButton.Right)
            {
                this.ClearSellItem();
            }
        };
        UIEventListener.Get(this.imgSellItem.gameObject).onEnter = delegate (PointerEventData ed)
        {
            object[] array = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetItemData", new object[]
            {
                this.controller.ReqProp._obj.thisid
            });
            if (array.Length > 0)
            {
                LuaTable ltb = array[0] as LuaTable;
                ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(ltb, this.imgSellItem.gameObject);
            }
        };
        UIEventListener.Get(this.imgSellItem.gameObject).onExit = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().ClosePanel();
        };
        this.imgSellItemGem.gameObject.SetActive(this.CheckIsGemOrNot(this.controller.ReqProp._obj.baseid));
        string imgname = "quality" + this.controller.ReqProp.config.GetField_Uint("quality");
        this.imgSellItemQuality.gameObject.SetActive(false);
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.RANK, imgname, delegate (UITextureAsset asset)
        {
            if (asset == null)
            {
                return;
            }
            if (this.imgSellItemQuality == null)
            {
                return;
            }
            this.imgSellItemQuality.gameObject.SetActive(true);
            this.imgSellItemQuality.texture = asset.textureObj;
        });
        this.controller.Ratio = 0f;
        this.txtSellPrice.text = price.ToString();
        GlobalRegister.SetImage(0, this.controller.MoneyIcon, this.imgSellPrice, true);
        for (int i = 0; i < this.controller.AllLastPrice.Count; i++)
        {
            if (this.controller.AllLastPrice[i].TradeType == TradeObjType.Item && this.controller.AllLastPrice[i].Baseid == this.controller.ReqProp._obj.baseid)
            {
                this.controller.Ratio = this.controller.AllLastPrice[i].Ratio;
                this.txtSellPrice.text = Mathf.Floor((float)price * (1f + this.controller.AllLastPrice[i].Ratio / 100f)).ToString();
            }
        }
        this.input_number2_1.onValueChanged.RemoveAllListeners();
        this.input_number2_1.onValueChanged.AddListener(delegate (string str)
        {
            this.ChangeNum(this.controller.ReqProp._obj.baseid, this.controller.ReqProp._obj.num);
            this.SetSumAndTipMoney();
        });
        this.input_number2_1.text = this.controller.ReqProp._obj.num.ToString();
        this.btnSellAddPrice.onClick.RemoveAllListeners();
        this.btnSellAddPrice.onClick.AddListener(delegate ()
        {
            this.AddPrice((int)price);
            this.SetSumAndTipMoney();
        });
        this.btnSellSubPrice.onClick.RemoveAllListeners();
        this.btnSellSubPrice.onClick.AddListener(delegate ()
        {
            this.SubPrice((int)price);
            this.SetSumAndTipMoney();
        });
        this.ShowSprite(ImageType.ITEM, this.controller.Id, this.img_icon3_6);
        this.ShowSprite(ImageType.ITEM, this.controller.SellCostId, this.img_icon4_7);
        this.SetSumAndTipMoney();
        this.btn_sale_10.gameObject.SetActive(true);
        this.btn_sale_10.onClick.RemoveAllListeners();
        this.btn_sale_10.onClick.AddListener(delegate ()
        {
            string[] array = this.txt_count1_5.text.Split(new char[]
            {
                '/'
            });
            int field_Int3 = LuaConfigManager.GetConfigTable("objects", (ulong)this.controller.ReqProp._obj.baseid).GetField_Int("maxnum");
            int num3 = (int)this.controller.ReqProp._obj.num;
            int num4 = Mathf.Min(field_Int3, num3);
            int num5 = this.input_number2_1.text.ToInt();
            uint sellCostId = this.controller.SellCostId;
            string field_String = LuaConfigManager.GetConfigTable("objects", (ulong)sellCostId).GetField_String("name");
            if (array[0].ToInt() == array[1].ToInt())
            {
                TipsWindow.ShowWindow(TipsType.NO_REMAIN, null);
            }
            else if (num5 > num4)
            {
                TipsWindow.ShowWindow(TipsType.ONCE_CANNOT_SO_MANY, null);
            }
            else if ((ulong)GlobalRegister.GetCurrencyByID(sellCostId) < (ulong)((long)this.txt_value4_10.text.ToInt()))
            {
                TipsWindow.ShowWindow(TipsType.MONEY_ISNOT_ENOUGH, new string[]
                {
                    field_String
                });
            }
            else
            {
                this.controller.MSG_ReqSellStaff_CS(SELLTYPE.OBJECT, this.controller.ReqProp._obj.thisid, this.controller.ReqProp._obj.baseid, (uint)this.txtSellPrice.text.ToInt(), (uint)this.input_number2_1.text.ToInt());
            }
        });
        float num = (float)LuaConfigManager.GetXmlConfigTable("trade").GetField_Int("recommandprice_sub_ratio") / 10000f;
        this._minSellPrice = price * (1f - num);
        float num2 = (float)LuaConfigManager.GetXmlConfigTable("trade").GetField_Int("recommandprice_add_ratio") / 10000f;
        this._maxSellPrice = price * (1f + num2);
        this.txtSellMinPrice.text = Mathf.Max(Mathf.Floor(this._minSellPrice), 1f).ToString();
        this.txtSellMaxPrice.text = Mathf.Floor(this._maxSellPrice).ToString();
        this.ShowSprite(ImageType.ITEM, this.controller.Id, this.imgSellMinPrice);
        this.ShowSprite(ImageType.ITEM, this.controller.Id, this.imgSellMaxPrice);
        int field_Int = LuaConfigManager.GetXmlConfigTable("trade").GetField_Int("sell_duration");
        this.txtSellSellTime.text = this.SecondToTime(field_Int);
        int field_Int2 = LuaConfigManager.GetXmlConfigTable("trade").GetField_Int("show_duration");
        this.txtSellShowTime.text = this.SecondToTime(field_Int2);
        this.txtSellShowTime.transform.parent.gameObject.SetActive(this.CheckIsGemOrNot(this.controller.ReqProp._obj.baseid));
        this.Panel_SaleItem_17.gameObject.SetActive(true);
    }

    public void SetSumAndTipMoney()
    {
        float num = Mathf.Max(Mathf.Floor((float)this.txtSellPrice.text.ToInt()), 1f);
        int num2 = this.input_number2_1.text.ToInt();
        this.txt_value3_9.text = (num * (float)num2).ToString();
        float num3 = Mathf.Floor(num * (float)LuaConfigManager.GetXmlConfigTable("trade").GetField_Int("sell_fee_ratio") / 10000f) * (float)num2;
        float field_Float = LuaConfigManager.GetXmlConfigTable("trade").GetField_Table("sell_fee_range").GetField_Float("min");
        float field_Float2 = LuaConfigManager.GetXmlConfigTable("trade").GetField_Table("sell_fee_range").GetField_Float("max");
        if (num3 < field_Float)
        {
            this.txt_value4_10.text = Mathf.Floor(field_Float).ToString();
        }
        else if (num3 >= field_Float && num3 <= field_Float2)
        {
            this.txt_value4_10.text = Mathf.Floor(num3).ToString();
        }
        else
        {
            this.txt_value4_10.text = Mathf.Floor(field_Float2).ToString();
        }
        uint sellCostId = this.controller.SellCostId;
        if ((ulong)GlobalRegister.GetCurrencyByID(sellCostId) < (ulong)((long)this.txt_value4_10.text.ToInt()))
        {
            this.txt_value4_10.color = GlobalRegister.GetColorByName("itemlimit");
            this.btn_sale_10.interactable = false;
        }
        else
        {
            this.txt_value4_10.color = GlobalRegister.GetColorByName("normalwhite");
            this.btn_sale_10.interactable = true;
        }
    }

    public void ChangeNum(uint id, uint num)
    {
        int num2 = 1;
        int a = (!this.CheckIsGemOrNot(id)) ? LuaConfigManager.GetConfigTable("objects", (ulong)id).GetField_Int("maxnum") : 1;
        int num3 = Mathf.Min(a, (int)num);
        int num4 = this.input_number2_1.text.ToInt();
        if (num4 > num3)
        {
            this.input_number2_1.text = num3.ToString();
        }
        else if (num4 < num2)
        {
            this.input_number2_1.text = num2.ToString();
        }
    }

    public void AddPrice(int recommendPrice)
    {
        int num = this.txtSellPrice.text.ToInt();
        float num2 = (float)LuaConfigManager.GetXmlConfigTable("trade").GetField_Int("recommandprice_add_ratio") / 10000f;
        float f = (float)recommendPrice * (1f + num2);
        float num3 = (float)LuaConfigManager.GetXmlConfigTable("trade").GetField_Int("once_add_ratio") / 10000f;
        float num4 = (float)num + (float)recommendPrice * num3;
        if (this.controller.Ratio / 100f < num2)
        {
            this.controller.Ratio = this.controller.Ratio + Mathf.Floor(num3 * 100f);
            this.txtSellPrice.text = Mathf.Floor((float)recommendPrice * (1f + this.controller.Ratio / 100f)).ToString();
        }
        else
        {
            this.txtSellPrice.text = Mathf.Floor(f).ToString();
            TipsWindow.ShowWindow(TipsType.PRICETOUP, null);
            this.controller.Ratio = Mathf.Floor(num2 * 100f);
        }
        this.txtSellPrice.text = Mathf.Max(Mathf.Floor((float)this.txtSellPrice.text.ToInt()), 1f).ToString();
    }

    public void SubPrice(int recommendPrice)
    {
        int num = this.txtSellPrice.text.ToInt();
        float num2 = (float)LuaConfigManager.GetXmlConfigTable("trade").GetField_Int("recommandprice_sub_ratio") / 10000f;
        float f = (float)recommendPrice * (1f - num2);
        float num3 = (float)LuaConfigManager.GetXmlConfigTable("trade").GetField_Int("once_sub_ratio") / 10000f;
        float num4 = (float)num - (float)recommendPrice * num3;
        if (this.controller.Ratio / 100f > -num2)
        {
            this.controller.Ratio = this.controller.Ratio - Mathf.Floor(num3 * 100f);
            this.txtSellPrice.text = Mathf.Floor((float)recommendPrice * (1f + this.controller.Ratio / 100f)).ToString();
        }
        else
        {
            this.txtSellPrice.text = Mathf.Floor(f).ToString();
            TipsWindow.ShowWindow(TipsType.PRICETOLOW, null);
            this.controller.Ratio = -Mathf.Floor(num2 * 100f);
        }
        this.txtSellPrice.text = Mathf.Max(Mathf.Floor((float)this.txtSellPrice.text.ToInt()), 1f).ToString();
    }

    public void OnTradeBtnClick(PropsBase prop)
    {
        if (this.CheckIsGemOrNot(prop._obj.baseid))
        {
            this.controller.ReqProp = prop;
            this.controller.MSG_ReqRecommandPrice_CS(SELLTYPE.OBJECT, prop._obj.baseid);
            this.controller.MSG_ReqTradeItemHistory_CS(SELLTYPE.OBJECT, prop._obj.baseid);
        }
        else
        {
            this.controller.ReqProp = prop;
            this.controller.MSG_ReqRecommandPrice_CS(SELLTYPE.OBJECT, prop._obj.baseid);
            this.controller.MSG_ReqTradeItemHistory_CS(SELLTYPE.OBJECT, prop._obj.baseid);
        }
    }

    public void ClosePutAway()
    {
        this.ClearSellItem();
    }

    public void ClearSellItem()
    {
        this.imgSellItem.gameObject.SetActive(false);
        this.imgSellItemGem.gameObject.SetActive(false);
        this.btn_sale_10.gameObject.SetActive(false);
        this.txtSellPrice.text = "0";
        this.input_number2_1.text = "0";
        this.txtSellMinPrice.text = "0";
        this.txtSellMaxPrice.text = "0";
        this.txtSellSellTime.text = "0";
        this.txtSellShowTime.text = "0";
        this.txt_value3_9.text = "0";
        this.txt_value4_10.text = "0";
    }

    public Transform GetSellItemTrans()
    {
        return this.Panel_SaleItem_17.parent;
    }

    private Transform ui_root;

    private ShopController controller;

    private Transform panelRoot;

    private Transform Panel_Record_42;

    public Toggle btn_buy_3;

    private Transform Panel_Buy_2;

    public Toggle btn_sale_5;

    public Transform Panel_Sale_8;

    public Toggle btn_show_8;

    private Transform Panel_Show_16;

    private Transform Panel_Show_53;

    private Button btnClose;

    public Text txtCapital;

    private Button btnRecordClose;

    private Transform list_43;

    private Transform Item_44;

    private Button btnRecord;

    private Dropdown ddRecord;

    private ToggleGroup tog_group_1;

    private ToggleGroup tog_group_2;

    private TextTip capitalTip;

    private Transform btn_tab_4;

    private Transform list_4;

    private Transform object_28;

    private Transform filterContent;

    private Button btn_minus_24;

    private Button btn_add_26;

    private Text txt_allpage_53;

    private Button btnBuyNumSortUp;

    private Button btnBuyNumSortDown;

    private Button btnBuyPriceSortUp;

    private Button btnBuyPriceSortDown;

    private Button btnBuyAllPriceSortUp;

    private Button btnBuyAllPriceSortDown;

    private Button btnBuyTimeSortUp;

    private Button btnBuyTimeSortDown;

    private Button btnBuyFresh;

    private Transform _curBuyItem;

    private List<TradeItem> buyItemList = new List<TradeItem>();

    private List<Transform> _buyFirstItemList = new List<Transform>();

    private bool _buyFirstShow;

    private Transform _curSeleteThirdItem;

    private Transform btn_tab_46;

    private Transform list_55;

    private Transform object_57;

    private Button btn_minus_61;

    private Button btn_add_63;

    private Text txt_allpage_54;

    private Button btnNoticeNumSortUp;

    private Button btnNoticeNumSortDown;

    private Button btnNoticePriceSortUp;

    private Button btnNoticePriceSortDown;

    private Button btnNoticeAllPriceSortUp;

    private Button btnNoticeAllPriceSortDown;

    private Button btnNoticeTimeSortUp;

    private Button btnNoticeTimeSortDown;

    private Button btnNoticeFresh;

    private List<TradeItem> noticeItemList = new List<TradeItem>();

    private List<Transform> _noticeFirstItemList = new List<Transform>();

    private bool _noticeFirstShow;

    private ScrollRect list_0;

    private Transform Rect_9;

    private Transform object_10;

    private Image img_goldicon_1;

    private Text txt_goldvalue_3;

    private Text txt_time_4;

    public Text txt_count1_5;

    private Button btn_record_4;

    private Transform Panel_SaleItem_17;

    private Image imgSellItem;

    private RawImage imgSellItemQuality;

    private Image imgSellItemGem;

    private Text txtSellPrice;

    private Image imgSellPrice;

    private Button btn_inputnumber_1_0;

    private InputField input_number2_1;

    private Button btn_inputnumber_2_1;

    private Image img_icon3_6;

    private Text txt_value3_9;

    private Image img_icon4_7;

    private Text txt_value4_10;

    private Button btn_sale_10;

    private Text txtSellMinPrice;

    private Text txtSellMaxPrice;

    private Image imgSellMinPrice;

    private Image imgSellMaxPrice;

    private Text txtSellShowTime;

    private Text txtSellSellTime;

    private Button btnSellAddPrice;

    private Button btnSellSubPrice;

    private Button btnSellNumSortUp;

    private Button btnSellNumSortDown;

    private Button btnSellPriceSortUp;

    private Button btnSellPriceSortDown;

    private Button btnSellAllPriceSortUp;

    private Button btnSellAllPriceSortDown;

    private Button btnSellTimeSortUp;

    private Button btnSellTimeSortDown;

    private Transform _curSellItem;

    private List<TradeItem> sellItemList = new List<TradeItem>();

    private float _minSellPrice;

    private float _maxSellPrice;

    private enum SortType
    {
        NumUp = 1,
        NumDown,
        PriceUp,
        PriceDown,
        AllPriceUp,
        AllPriceDown,
        TimeUp,
        TimeDown
    }
}
