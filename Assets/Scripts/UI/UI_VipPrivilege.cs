using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Obj;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_VipPrivilege : UIPanelBase
{
    private ItemTipController itc
    {
        get
        {
            if (this.itc_ == null)
            {
                this.itc_ = ControllerManager.Instance.GetController<ItemTipController>();
            }
            return this.itc_;
        }
        set
        {
            this.itc_ = value;
        }
    }

    private bool isVip
    {
        get
        {
            return this.remaintime > 0UL;
        }
    }

    private VipPrivilegeController vpc
    {
        get
        {
            return ControllerManager.Instance.GetController<VipPrivilegeController>();
        }
    }

    public override void OnDispose()
    {
        base.Dispose();
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.UpdateLeftTime));
        this.vpc.RegOnPlayerDataChangeAction(new Action(this.RefrashHaveStoneCount), false);
        this.itc = null;
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.stoneHave = GlobalRegister.GetCurrencyByID(2U);
        LuaTable configTable = LuaConfigManager.GetConfigTable("vipcard_config", 1UL);
        LuaTable configTable2 = LuaConfigManager.GetConfigTable("vipcard_config", 2UL);
        this.weekVipPrice = configTable.GetCacheField_Uint("gemcost");
        this.buyAwardWeek = configTable.GetCacheField_String("reward");
        this.monthVipPrice = configTable2.GetCacheField_Uint("gemcost");
        this.buyAwardMonth = configTable2.GetCacheField_String("reward");
        this.rootObject = root.gameObject;
        this.InitObject();
        this.InitEvent();
        this.InitUIData();
        Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.UpdateLeftTime));
        this.vpc.GetCurVipCardInfo();
        this.vpc.RegOnPlayerDataChangeAction(new Action(this.RefrashHaveStoneCount), true);
    }

    private void InitObject()
    {
        this.btnClose = this.rootObject.transform.Find("Offset/Panel_root/img_title/Button_close");
        this.btnChange = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Button_charge");
        this.textStoneHaveCount = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_diamon/Text_value");
        this.textWeekVipPrice = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_week/Panel_price/Text_price");
        this.textMonthVipPrice = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_month/Panel_price/Text_price");
        this.btnBuyWeekVip = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_week/Button_buy");
        this.btnBuyMonthVip = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_month/Button_buy");
        this.inputWeekBuyCount = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_week/InputField");
        this.inputMonthBuyCount = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_month/InputField");
        this.panelDailyAward = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_award");
        this.btnGetDailyAward = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Button_get_daily_award");
        this.textNotBuyLeft = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Text_des_not_buy");
        this.textNotBuyRight = this.rootObject.transform.Find("Offset/Panel_root/Panel_privilege/Text_des_not_buy");
        this.textTimeLeft = this.rootObject.transform.Find("Offset/Panel_root/Panel_privilege/Text_left_time/Text_value");
        this.nodeRafGet = this.rootObject.transform.Find("Offset/Panel_root/Panel_privilege/Image_obtain");
        this.textLeftRafCount = this.rootObject.transform.Find("Offset/Panel_root/Panel_privilege/Text_left_luck_count/Text_value");
        this.textLeftObjectCount = this.rootObject.transform.Find("Offset/Panel_root/Panel_privilege/Text_left_luck_object/Panel/Text_value");
        this.btnCountRaf = this.rootObject.transform.Find("Offset/Panel_root/Panel_privilege/Button_luck_draw_time");
        this.btnObjectRaf = this.rootObject.transform.Find("Offset/Panel_root/Panel_privilege/Button_luck_draw_object");
        this.nodRafAward = this.rootObject.transform.Find("Offset/Panel_root/Panel_privilege/Panel_perhaps_obtain");
        this.textLuckObject = this.rootObject.transform.Find("Offset/Panel_root/Panel_privilege/Text_left_luck_object/Text");
        this.btnAddCountWeek = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_week/InputField/btn_add");
        this.btnMinusCountWeek = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_week/InputField/btn_sub");
        this.btnAddCountMonth = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_month/InputField/btn_add");
        this.btnMinusCountMonth = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_month/InputField/btn_sub");
        this.btnChange.gameObject.SetActive(true);
    }

    private void InitEvent()
    {
        Button component = this.btnClose.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.Close));
        Button component2 = this.btnChange.GetComponent<Button>();
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(new UnityAction(this.Charge));
        Button component3 = this.btnBuyWeekVip.GetComponent<Button>();
        component3.onClick.RemoveAllListeners();
        component3.onClick.AddListener(delegate ()
        {
            this.BuyVip(1U, this.GetBuyCount(1U));
        });
        Button component4 = this.btnBuyMonthVip.GetComponent<Button>();
        component4.onClick.RemoveAllListeners();
        component4.onClick.AddListener(delegate ()
        {
            this.BuyVip(2U, this.GetBuyCount(2U));
        });
        InputField input_week = this.inputWeekBuyCount.GetComponent<InputField>();
        input_week.onValueChanged.RemoveAllListeners();
        input_week.onValueChanged.AddListener(delegate (string s)
        {
            this.OnInputCountChange(input_week, this.weekVipPrice);
        });
        InputField input_month = this.inputMonthBuyCount.GetComponent<InputField>();
        input_month.onValueChanged.RemoveAllListeners();
        input_month.onValueChanged.AddListener(delegate (string s)
        {
            this.OnInputCountChange(input_month, this.monthVipPrice);
        });
        Button component5 = this.btnGetDailyAward.GetComponent<Button>();
        component5.onClick.RemoveAllListeners();
        component5.onClick.AddListener(new UnityAction(this.GetDailyAward));
        Button component6 = this.btnCountRaf.GetComponent<Button>();
        component6.onClick.RemoveAllListeners();
        component6.onClick.AddListener(delegate ()
        {
            this.Raf(RaffUseType.RAFFUSETYPE_FREETIMES);
        });
        Button component7 = this.btnObjectRaf.GetComponent<Button>();
        component7.onClick.RemoveAllListeners();
        component7.onClick.AddListener(delegate ()
        {
            this.Raf(RaffUseType.RAFFUSETYPE_OBJECT);
        });
        Button component8 = this.btnAddCountWeek.GetComponent<Button>();
        component8.onClick.RemoveAllListeners();
        component8.onClick.AddListener(delegate ()
        {
            this.OnMountChange(input_week, true);
        });
        Button component9 = this.btnMinusCountWeek.GetComponent<Button>();
        component9.onClick.RemoveAllListeners();
        component9.onClick.AddListener(delegate ()
        {
            this.OnMountChange(input_week, false);
        });
        Button component10 = this.btnAddCountMonth.GetComponent<Button>();
        component10.onClick.RemoveAllListeners();
        component10.onClick.AddListener(delegate ()
        {
            this.OnMountChange(input_month, true);
        });
        Button component11 = this.btnMinusCountMonth.GetComponent<Button>();
        component11.onClick.RemoveAllListeners();
        component11.onClick.AddListener(delegate ()
        {
            this.OnMountChange(input_month, false);
        });
    }

    private void InitUIData()
    {
        this.RefrashHaveStoneCount();
        Text component = this.textWeekVipPrice.GetComponent<Text>();
        component.text = this.weekVipPrice + string.Empty;
        Text component2 = this.textMonthVipPrice.GetComponent<Text>();
        component2.text = this.monthVipPrice + string.Empty;
        string[] array = this.buyAwardWeek.Split(new char[]
        {
            '|'
        });
        string[] array2 = this.buyAwardMonth.Split(new char[]
        {
            '|'
        });
        uint itemIdWeek = uint.Parse(array[0].Split(new char[]
        {
            '-'
        })[0]);
        uint itemIdMonth = uint.Parse(array2[0].Split(new char[]
        {
            '-'
        })[0]);
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)itemIdWeek);
        LuaTable configTable2 = LuaConfigManager.GetConfigTable("objects", (ulong)itemIdMonth);
        Image imgBuyAwardWeek = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_week/Image_object").GetComponent<Image>();
        Image imgBuyAwardMonth = this.rootObject.transform.Find("Offset/Panel_root/Panel_charge/Image_month/Image_object").GetComponent<Image>();
        UITextureMgr.Instance.GetTexture(ImageType.ITEM, configTable.GetCacheField_String("icon"), delegate (UITextureAsset to)
        {
            if (imgBuyAwardWeek == null)
            {
                return;
            }
            if (to != null && to.textureObj != null)
            {
                Sprite sprite = Sprite.Create(to.textureObj, new Rect(0f, 0f, (float)to.textureObj.width, (float)to.textureObj.height), new Vector2(0.5f, 0.5f));
                imgBuyAwardWeek.sprite = sprite;
                imgBuyAwardWeek.material = null;
            }
        });
        UITextureMgr.Instance.GetTexture(ImageType.ITEM, configTable2.GetCacheField_String("icon"), delegate (UITextureAsset to)
        {
            if (imgBuyAwardMonth == null)
            {
                return;
            }
            if (to != null && to.textureObj != null)
            {
                Sprite sprite = Sprite.Create(to.textureObj, new Rect(0f, 0f, (float)to.textureObj.width, (float)to.textureObj.height), new Vector2(0.5f, 0.5f));
                imgBuyAwardMonth.sprite = sprite;
                imgBuyAwardMonth.material = null;
            }
        });
        HoverEventListener.Get(imgBuyAwardWeek.gameObject).onEnter = delegate (PointerEventData pd)
        {
            this.itc.OpenPanel(new t_Object
            {
                baseid = itemIdWeek
            }, imgBuyAwardWeek.gameObject);
        };
        HoverEventListener.Get(imgBuyAwardMonth.gameObject).onEnter = delegate (PointerEventData pd)
        {
            this.itc.OpenPanel(new t_Object
            {
                baseid = itemIdMonth
            }, imgBuyAwardMonth.gameObject);
        };
        this.InitRafShowData();
        this.RefrashRafObjectHaveCount();
        this.nodeRafGet.Find("Text_count").GetComponent<Text>().text = string.Empty;
    }

    private void InitRafShowData()
    {
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("newUserInit").GetCacheField_Table("Vipluckreward");
        if (cacheField_Table != null)
        {
            int num = 1;
            LuaTable cacheField_Table2 = cacheField_Table.GetCacheField_Table(num.ToString());
            Transform transform = this.nodRafAward;
            GameObject gameObject = transform.GetChild(0).gameObject;
            while (cacheField_Table2 != null)
            {
                GameObject itemObj = null;
                if (num - 1 < transform.childCount)
                {
                    itemObj = transform.GetChild(num - 1).gameObject;
                }
                else
                {
                    itemObj = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    itemObj.transform.SetParent(transform, false);
                }
                uint objID = uint.Parse(cacheField_Table2.GetField_String("obj"));
                uint num2 = uint.Parse(cacheField_Table2.GetField_String("num"));
                uint num3 = uint.Parse(cacheField_Table2.GetField_String("chance"));
                LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)objID);
                Image icon = itemObj.transform.Find("icon").GetComponent<Image>();
                HoverEventListener.Get(itemObj).onEnter = delegate (PointerEventData pd)
                {
                    this.itc.OpenPanel(new t_Object
                    {
                        baseid = objID,
                        level = 1U
                    }, itemObj);
                };
                UITextureMgr.Instance.GetTexture(ImageType.ITEM, configTable.GetCacheField_String("icon"), delegate (UITextureAsset to)
                {
                    if (icon == null)
                    {
                        return;
                    }
                    if (to != null && to.textureObj != null)
                    {
                        Sprite sprite = Sprite.Create(to.textureObj, new Rect(0f, 0f, (float)to.textureObj.width, (float)to.textureObj.height), new Vector2(0.5f, 0.5f));
                        icon.sprite = sprite;
                        icon.material = null;
                    }
                });
                GameObject qualityObj = itemObj.FindChild("quality");
                qualityObj.SetActive(false);
                string imgname = "quality" + configTable.GetField_Uint("quality");
                ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.RANK, imgname, delegate (UITextureAsset asset)
                {
                    if (qualityObj == null)
                    {
                        return;
                    }
                    if (asset == null)
                    {
                        return;
                    }
                    qualityObj.SetActive(true);
                    qualityObj.GetComponent<RawImage>().texture = asset.textureObj;
                });
                itemObj.transform.Find("Text_name").GetComponent<Text>().text = string.Empty;
                itemObj.transform.Find("Text_count").GetComponent<Text>().text = string.Empty + num2;
                itemObj.transform.Find("ratio/Text_value").GetComponent<Text>().text = num3 + "%";
                num++;
                cacheField_Table2 = cacheField_Table.GetCacheField_Table(num.ToString());
            }
        }
    }

    private void RefrashRafObjectHaveCount()
    {
        PropsBase propsBase = (PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetPropBaseByID", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            99999
        })[0];
        this.rafObjectCount = 0U;
        if (propsBase != null)
        {
            this.rafObjectCount = propsBase.Count;
        }
        Text component = this.textLeftObjectCount.GetComponent<Text>();
        component.text = string.Empty + this.rafObjectCount;
    }

    private void UpdateLeftTime()
    {
        if (this.textTimeLeft == null)
        {
            return;
        }
        if (this.remaintime > 0UL)
        {
            this.textTimeLeft.GetComponent<Text>().text = this.GetLeftVipTime();
            this.remaintime -= 1UL;
        }
        else
        {
            this.SetUIVipState();
        }
    }

    private string GetLeftVipTime()
    {
        string empty = string.Empty;
        ulong num = this.remaintime / (ulong)this.daySec;
        ulong num2 = (this.remaintime - num * (ulong)this.daySec) / (ulong)this.hourSec;
        ulong num3 = (this.remaintime - num * (ulong)this.daySec - num2 * (ulong)this.hourSec) / (ulong)this.minSce;
        ulong num4 = this.remaintime % 60UL;
        return string.Concat(new object[]
        {
            num.ToString("f0"),
            "天",
            num2,
            "小时",
            num3,
            "分钟",
            num4,
            "秒"
        });
    }

    private void Close()
    {
        UIManager.Instance.DeleteUI<UI_VipPrivilege>();
    }

    private void Charge()
    {
        string url = "http://pay.ztgame.com/p.php?t=qfillfrombank&gametype=52&fillsource=game&accid=" + UserInfoStorage.StorageInfo.Uid;
        Application.OpenURL(url);
    }

    private void BuyVip(uint cardID, uint count)
    {
        this.vpc.ReqBuyVipCard(cardID, count);
    }

    private uint GetBuyCount(uint cardID)
    {
        uint result = 0U;
        if (cardID == 1U)
        {
            InputField component = this.inputWeekBuyCount.GetComponent<InputField>();
            uint.TryParse(component.text, out result);
        }
        if (cardID == 2U)
        {
            InputField component2 = this.inputMonthBuyCount.GetComponent<InputField>();
            uint.TryParse(component2.text, out result);
        }
        return result;
    }

    private void OnInputCountChange(InputField input, uint price)
    {
        if (string.IsNullOrEmpty(input.text))
        {
            input.text = "1";
            return;
        }
        int num = int.Parse(input.text);
        if (num <= 0)
        {
            input.text = "1";
            return;
        }
        uint num2 = (uint)num;
        uint num3 = this.stoneHave / price;
        num3 = Math.Max(1U, num3);
        if (num2 > num3)
        {
            input.text = string.Empty + num3;
            return;
        }
    }

    private void OnMountChange(InputField input, bool addOrMinus)
    {
        int num = int.Parse(input.text);
        int num2 = num + ((!addOrMinus) ? -1 : 1);
        num2 = Mathf.Max(num2, 1);
        input.text = string.Empty + num2;
    }

    private void GetDailyAward()
    {
        this.vpc.ReqGetDailyAward();
    }

    private void Raf(RaffUseType type)
    {
        this.vpc.ReqRaf(type);
    }

    public void RefrashHaveStoneCount()
    {
        this.stoneHave = GlobalRegister.GetCurrencyByID(2U);
        Text component = this.textStoneHaveCount.GetComponent<Text>();
        component.text = this.stoneHave + string.Empty;
    }

    internal void OnGetVIPCardInfo_SC(VIPCardInfo vipcardinfo)
    {
        Scheduler.Instance.AddTimer(0.2f, false, delegate
        {
            this.RefrashHaveStoneCount();
            this.RefrashRafObjectHaveCount();
        });
        this.remaintime = vipcardinfo.remaintime;
        this.SetUIVipState();
        Button component = this.btnObjectRaf.GetComponent<Button>();
        component.interactable = (this.rafObjectCount > 0U);
        Text component2 = this.textLeftRafCount.GetComponent<Text>();
        Button component3 = this.btnCountRaf.GetComponent<Button>();
        component3.interactable = true;
        component2.text = string.Empty + this.rafObjectCount;
        Text component4 = this.textLuckObject.GetComponent<Text>();
        component4.text = component4.text.Replace("50", vipcardinfo.objraffcount.ToString());
        if (this.isVip)
        {
            component2.text = vipcardinfo.total_raffcount - vipcardinfo.raffcount + "/" + vipcardinfo.total_raffcount;
            if (vipcardinfo.raffcount >= vipcardinfo.total_raffcount)
            {
                component3.interactable = false;
            }
        }
        else
        {
            component3.interactable = false;
        }
        this.btnGetDailyAward.gameObject.SetActive(vipcardinfo.dayprize_state == 1U);
        this.InitDailyAward(vipcardinfo.arrprize);
    }

    private void InitDailyAward(List<PrizeBase> data)
    {
        if (data != null)
        {
            Transform transform = this.panelDailyAward.Find("Panel_award_items");
            GameObject gameObject = transform.GetChild(0).gameObject;
            for (int i = 0; i < data.Count; i++)
            {
                GameObject itemObj = null;
                if (i < transform.childCount)
                {
                    itemObj = transform.GetChild(i).gameObject;
                }
                else
                {
                    itemObj = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    itemObj.transform.SetParent(transform, false);
                }
                uint objID = data[i].id;
                uint quantity = data[i].quantity;
                LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)objID);
                Image icon = itemObj.transform.Find("icon").GetComponent<Image>();
                UITextureMgr.Instance.GetTexture(ImageType.ITEM, configTable.GetCacheField_String("icon"), delegate (UITextureAsset to)
                {
                    if (icon == null)
                    {
                        return;
                    }
                    if (to != null && to.textureObj != null)
                    {
                        Sprite sprite = Sprite.Create(to.textureObj, new Rect(0f, 0f, (float)to.textureObj.width, (float)to.textureObj.height), new Vector2(0.5f, 0.5f));
                        icon.sprite = sprite;
                        icon.material = null;
                    }
                });
                itemObj.FindChild("quality").SetActive(false);
                string imgname = "quality" + configTable.GetField_Uint("quality");
                ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.RANK, imgname, delegate (UITextureAsset asset)
                {
                    if (itemObj == null)
                    {
                        return;
                    }
                    if (asset == null)
                    {
                        return;
                    }
                    itemObj.FindChild("quality").SetActive(true);
                    itemObj.FindChild("quality").GetComponent<RawImage>().texture = asset.textureObj;
                });
                HoverEventListener.Get(itemObj).onEnter = delegate (PointerEventData pd)
                {
                    this.itc.OpenPanel(new t_Object
                    {
                        baseid = objID
                    }, itemObj);
                };
                itemObj.transform.Find("Text_name").GetComponent<Text>().text = string.Empty;
                itemObj.transform.Find("Text_count").GetComponent<Text>().text = string.Empty + quantity;
            }
        }
    }

    private void SetUIVipState()
    {
        this.textNotBuyLeft.gameObject.SetActive(!this.isVip);
        this.textNotBuyRight.gameObject.SetActive(false);
        this.textTimeLeft.transform.parent.gameObject.SetActive(this.isVip);
        if (!this.isVip)
        {
            Text component = this.textLeftRafCount.GetComponent<Text>();
            component.text = "0";
        }
    }

    internal void OnRaffVIPCardPrize_SC(uint id, uint quantity)
    {
        Scheduler.Instance.AddTimer(0.2f, false, delegate
        {
            this.RefrashRafObjectHaveCount();
        });
        this.nodeRafGet.Find("Text_count").GetComponent<Text>().text = "X" + quantity;
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)id);
        Image iconDefault = this.nodeRafGet.transform.Find("icon_default").GetComponent<Image>();
        UITextureMgr.Instance.GetTexture(ImageType.ITEM, configTable.GetCacheField_String("icon"), delegate (UITextureAsset to)
        {
            if (to != null && to.textureObj != null && iconDefault != null)
            {
                Sprite sprite = Sprite.Create(to.textureObj, new Rect(0f, 0f, (float)to.textureObj.width, (float)to.textureObj.height), new Vector2(0.5f, 0.5f));
                iconDefault.sprite = sprite;
                iconDefault.material = null;
            }
            HoverEventListener.Get(this.nodeRafGet.gameObject).onEnter = delegate (PointerEventData pd)
            {
                this.itc.OpenPanel(new t_Object
                {
                    baseid = id,
                    level = 1U
                }, this.nodeRafGet.gameObject);
            };
        });
    }

    private GameObject rootObject;

    private Transform btnClose;

    private Transform btnChange;

    private Transform textStoneHaveCount;

    private Transform textWeekVipPrice;

    private Transform textMonthVipPrice;

    private Transform btnBuyWeekVip;

    private Transform btnBuyMonthVip;

    private Transform inputWeekBuyCount;

    private Transform inputMonthBuyCount;

    private Transform panelDailyAward;

    private Transform btnGetDailyAward;

    private Transform textNotBuyLeft;

    private Transform textNotBuyRight;

    private Transform textTimeLeft;

    private Transform nodeRafGet;

    private Transform textLeftRafCount;

    private Transform textLeftObjectCount;

    private Transform btnCountRaf;

    private Transform btnObjectRaf;

    private Transform nodRafAward;

    private Transform textLuckObject;

    private Transform btnAddCountWeek;

    private Transform btnMinusCountWeek;

    private Transform btnAddCountMonth;

    private Transform btnMinusCountMonth;

    private uint stoneHave;

    private uint weekVipPrice;

    private uint monthVipPrice;

    private string buyAwardWeek;

    private string buyAwardMonth;

    private ulong remaintime;

    private uint rafObjectCount;

    private uint daySec = 86400U;

    private uint hourSec = 3600U;

    private uint minSce = 60U;

    private ItemTipController itc_;
}
