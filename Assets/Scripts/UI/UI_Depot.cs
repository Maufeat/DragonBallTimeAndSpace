using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Obj;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Depot : UIPanelBase
{
    public override void AfterInit()
    {
        base.AfterInit();
    }

    public override void OnInit(Transform root)
    {
        this.mController = ControllerManager.Instance.GetController<DepotController>();
        this.itemObjList = new List<GameObject>();
        Transform transform = root.Find("Offset_Example/Panel_depot");
        Transform transform2 = transform.Find("Panel_item/ItemList");
        for (int i = 0; i < transform2.childCount; i++)
        {
            Transform child = transform2.GetChild(i);
            DragDropButton dragDropButton = child.gameObject.AddComponent<DragDropButton>();
            Transform transform3 = child.Find("Item");
            RawImage component = child.Find("Item/quality").GetComponent<RawImage>();
            component.raycastTarget = false;
            dragDropButton.Initilize(UIRootType.Depot, new Vector2(0f, (float)i), "Item/img_icon", null);
            this.itemObjList.Add(child.gameObject);
        }
        Transform transform4 = transform.Find("Head/CloseButton");
        UIEventListener.Get(transform4.gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_close_onclick);
        this.txtGolden = transform.Find("FunctionList/Panel_coin/trademoney/img_bg/txt_value").GetComponent<Text>();
        this.txtDiamond = transform.Find("FunctionList/Panel_coin/deposit /img_bg/txt_value").GetComponent<Text>();
        Button component2 = transform.Find("ToggleGroup/Panel_btn/btn_lock").GetComponent<Button>();
        Button component3 = transform.Find("ToggleGroup/Panel_btn/btn_split").GetComponent<Button>();
        this.btnExtend = transform.Find("FunctionList/btn_extend").GetComponent<Button>();
        this.btnExtend.onClick.AddListener(new UnityAction(this.btnadddepot_onclick));
        component2.onClick.AddListener(new UnityAction(this.btnlock_onclick));
        component3.onClick.AddListener(new UnityAction(this.btnsplit_onclick));
        this.transSplitPanel = root.Find("Offset_Example/Panel_Split");
        Button component4 = this.transSplitPanel.Find("Panel_input/btn_minus").GetComponent<Button>();
        Button component5 = this.transSplitPanel.Find("Panel_input/btn_add").GetComponent<Button>();
        Button component6 = this.transSplitPanel.Find("btn_ok").GetComponent<Button>();
        Button component7 = this.transSplitPanel.Find("btn_cancel").GetComponent<Button>();
        this.inputSplitNum = this.transSplitPanel.Find("Panel_input/input_number").GetComponent<InputField>();
        component4.onClick.AddListener(new UnityAction(this.btn_splitnummin_onclick));
        component5.onClick.AddListener(new UnityAction(this.btn_splitnumadd_onclick));
        component6.onClick.AddListener(new UnityAction(this.btn_splitsure_onclick));
        component7.onClick.AddListener(new UnityAction(this.btn_splitcancel_onclick));
        this.inputSplitNum.onValueChanged.AddListener(new UnityAction<string>(this.input_splitnum_onvaluechanged));
        this.inputSplitNum.text = "1";
        this.transBankPanel = root.Find("Offset_Example/Panel_bank");
        Button component8 = this.transBankPanel.Find("btn_ok").GetComponent<Button>();
        Button component9 = this.transBankPanel.Find("btn_cancel").GetComponent<Button>();
        this.inputfieldSaveMoney = this.transBankPanel.Find("save/InputField").GetComponent<InputField>();
        this.inputfieldTakeMoney = this.transBankPanel.Find("take/InputField").GetComponent<InputField>();
        component8.onClick.AddListener(new UnityAction(this.btn_moneychangesure_onclick));
        component9.onClick.AddListener(new UnityAction(this.btn_moneychangecancel_onclick));
        this.transExtendPanel = root.Find("Offset_Example/Panel_Extend");
        Button component10 = this.transExtendPanel.Find("btn_ok").GetComponent<Button>();
        Button component11 = this.transExtendPanel.Find("btn_cancel").GetComponent<Button>();
        component10.onClick.AddListener(new UnityAction(this.btnaddepotsure_onclick));
        component11.onClick.AddListener(new UnityAction(this.btnaddepotcancel_onclick));
        Button component12 = transform.Find("FunctionList/Panel_coin/trademoney/img_bg/btn_shop").GetComponent<Button>();
        Button component13 = transform.Find("FunctionList/Panel_coin/deposit /img_bg/btn_shop").GetComponent<Button>();
        component12.onClick.AddListener(new UnityAction(this.btn_goldenchange_onclick));
        component13.onClick.AddListener(new UnityAction(this.btn_diamondchange_onclick));
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("bag").GetCacheField_Table("extend");
        IEnumerator enumerator = cacheField_Table.Values.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
        {
            object obj = enumerator.Current;
            LuaTable luaTable = obj as LuaTable;
            uint field_Uint = luaTable.GetField_Uint("ID");
            uint field_Uint2 = luaTable.GetField_Uint("value");
            this.cacheCostDic.Add(field_Uint, field_Uint2);
        }
        Button component14 = transform.Find("FunctionList/btn_sort").GetComponent<Button>();
        component14.onClick.AddListener(new UnityAction(this.btn_sort_onclick));
        this.SetupPanel();
        base.RegOpenUIByNpc("UI_Depot");
        base.OnInit(root);
    }

    private void btn_sort_onclick()
    {
        this.mController.TidyPack(PackType.PACK_TYPE_WAREHOUSE1);
    }

    private void btnadddepot_onclick()
    {
        uint num = this.cacheCostDic[this.mController.GetUnlockCount() + 1U];
        Text component = this.transExtendPanel.Find("info/txt_number").GetComponent<Text>();
        component.text = num.ToString();
        this.transExtendPanel.gameObject.SetActive(true);
    }

    private void btnaddepotsure_onclick()
    {
        this.transExtendPanel.gameObject.SetActive(false);
        uint num = this.cacheCostDic[this.mController.GetUnlockCount() + 1U];
        if (GlobalRegister.GetCurrencyByID(2U) < num)
        {
            TipsWindow.ShowWindow("砖石不足，需要" + num);
            return;
        }
        this.mController.ExtendPackage();
    }

    private void btnaddepotcancel_onclick()
    {
        this.transExtendPanel.gameObject.SetActive(false);
    }

    public void ExtendPackCb(MSG_PackUnlock_SC mdata)
    {
        this.SetupUnlockPanel();
    }

    private void SetupUnlockPanel()
    {
        uint unlockCount = this.mController.GetUnlockCount();
        int num = this.BASE_NUM;
        while ((long)num < (long)((ulong)(unlockCount * this.ROW_NUM) + (ulong)((long)this.BASE_NUM)))
        {
            this.itemObjList[num].SetActive(true);
            num++;
        }
        this.btnExtend.interactable = (unlockCount < this.MAX_UNLOCK_NUM);
    }

    private void btnlock_onclick()
    {
        MouseStateControoler.Instan.SetMoseState(MoseState.m_safelock);
    }

    private void btnsplit_onclick()
    {
        MouseStateControoler.Instan.SetMoseState(MoseState.m_itemsplit);
    }

    private void btn_close_onclick(PointerEventData eventData)
    {
        this.mController.ClosePanel();
    }

    public void SetupPanel()
    {
        MSG_PackData_SC depotData = this.mController.GetDepotData();
        if (depotData == null)
        {
            return;
        }
        this.btnExtend.interactable = (depotData.unlockcount < this.MAX_UNLOCK_NUM);
        List<t_Object> objs = depotData.objects.objs;
        Dictionary<uint, t_Object> dictionary = new Dictionary<uint, t_Object>();
        for (int i = 0; i < objs.Count; i++)
        {
            t_Object t_Object = objs[i];
            if (t_Object.grid_y == 65535U)
            {
                if (t_Object.baseid == 3U)
                {
                    this.txtGolden.text = t_Object.num.ToString();
                }
                else if (t_Object.baseid == 2U)
                {
                    this.txtDiamond.text = t_Object.num.ToString();
                }
            }
            else
            {
                dictionary.Add(t_Object.grid_y, t_Object);
            }
        }
        uint num = 0U;
        while ((ulong)num < (ulong)((long)this.itemObjList.Count))
        {
            if (dictionary.ContainsKey(num))
            {
                this.SetupItem(dictionary[num]);
            }
            else
            {
                this.SetupItemDefault(this.itemObjList[(int)num]);
            }
            num += 1U;
        }
        this.SetupUnlockPanel();
    }

    private void SetupItemDefault(GameObject obj)
    {
        Image component = obj.transform.Find("Item/img_icon").GetComponent<Image>();
        component.color = new Color(0f, 0f, 0f, 0f);
        Image component2 = obj.transform.Find("Item/img_lock").GetComponent<Image>();
        component2.gameObject.SetActive(false);
        Text component3 = obj.transform.Find("Item/txt_count").GetComponent<Text>();
        component3.text = string.Empty;
        obj.FindChild("Item/quality").SetActive(false);
        obj.GetComponent<DragDropButton>().mData = null;
    }

    private void SetupItem(t_Object objData)
    {
        uint indexX = objData.grid_x;
        uint indexY = objData.grid_y;
        GameObject obj = this.itemObjList[(int)objData.grid_y];
        Image imgicon = obj.transform.Find("Item/img_icon").GetComponent<Image>();
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)objData.baseid);
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, configTable.GetField_String("icon"), delegate (UITextureAsset asset)
        {
            if (asset == null)
            {
                FFDebug.LogWarning("CommonItem", "  req  texture   is  null ");
                return;
            }
            if (imgicon == null)
            {
                return;
            }
            Texture2D textureObj = asset.textureObj;
            Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
            imgicon.overrideSprite = sprite;
            imgicon.sprite = sprite;
            imgicon.color = Color.white;
            imgicon.gameObject.SetActive(true);
        });
        GameObject qualityObj = obj.FindChild("Item/quality");
        qualityObj.SetActive(false);
        string imgname = "quality" + configTable.GetField_Uint("quality");
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.RANK, imgname, delegate (UITextureAsset asset)
        {
            if (asset == null)
            {
                return;
            }
            if (qualityObj == null)
            {
                return;
            }
            qualityObj.SetActive(true);
            qualityObj.GetComponent<RawImage>().texture = asset.textureObj;
        });
        Image component = obj.transform.Find("Item/img_lock").GetComponent<Image>();
        component.gameObject.SetActive(objData.lock_end_time == 1U);
        Text component2 = obj.transform.Find("Item/txt_count").GetComponent<Text>();
        component2.text = objData.num.ToString();
        obj.GetComponent<DragDropButton>().mData = new BagDragDropButtonData(objData.baseid, objData.thisid, PackType.PACK_TYPE_WAREHOUSE1, objData.lock_end_time, obj.transform.GetChild(0), null);
        Scheduler.OnScheduler TryShowItemTip = delegate ()
        {
            MSG_PackData_SC depotData = this.mController.GetDepotData();
            if (depotData == null)
            {
                return;
            }
            List<t_Object> objs = depotData.objects.objs;
            for (int i = 0; i < objs.Count; i++)
            {
                t_Object t_Object = objs[i];
                if (indexX == t_Object.grid_x && indexY == t_Object.grid_y)
                {
                    ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(t_Object, obj);
                }
            }
        };
        UIEventListener.Get(obj).onEnter = delegate (PointerEventData eventData)
        {
            if (MouseStateControoler.Instan.IsContinuedMouseState())
            {
                Scheduler.Instance.AddTimer(0.5f, false, TryShowItemTip);
            }
            else
            {
                TryShowItemTip();
            }
        };
        UIEventListener.Get(obj).onDestroy = delegate (PointerEventData eventData)
        {
            Scheduler.Instance.RemoveTimer(TryShowItemTip);
        };
        UIEventListener.Get(obj).onExit = delegate (PointerEventData eventData)
        {
            Scheduler.Instance.RemoveTimer(TryShowItemTip);
        };
    }

    private void btn_splitnummin_onclick()
    {
        uint num = uint.Parse(this.inputSplitNum.text);
        num -= 1U;
        if (num < 1U)
        {
            num = 1U;
        }
        this.inputSplitNum.text = num.ToString();
    }

    private void btn_splitnumadd_onclick()
    {
        uint num = uint.Parse(this.inputSplitNum.text);
        num += 1U;
        if (num > this.split_max_num)
        {
            num = this.split_max_num;
        }
        this.inputSplitNum.text = num.ToString();
    }

    private void input_splitnum_onvaluechanged(string num)
    {
        if (uint.Parse(num) > this.split_max_num)
        {
            this.inputSplitNum.text = this.split_max_num.ToString();
        }
        else if (uint.Parse(num) < 1U)
        {
            this.inputSplitNum.text = "1";
        }
    }

    public void SetupSplitPanel(uint baseid, uint count)
    {
        this.split_max_num = count;
        Text component = this.transSplitPanel.Find("Panel_title/txt_Name").GetComponent<Text>();
        RawImage imgicon = this.transSplitPanel.Find("Item/img_icon").GetComponent<RawImage>();
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)baseid);
        component.text = configTable.GetField_String("name");
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, configTable.GetField_String("icon"), delegate (UITextureAsset asset)
        {
            if (asset == null)
            {
                FFDebug.LogWarning("CommonItem", "  req  texture   is  null ");
                return;
            }
            if (imgicon == null)
            {
                return;
            }
            Texture2D textureObj = asset.textureObj;
            imgicon.texture = textureObj;
            imgicon.color = Color.white;
            imgicon.gameObject.SetActive(true);
        });
        this.transSplitPanel.gameObject.SetActive(true);
    }

    private void btn_splitsure_onclick()
    {
        this.mController.ReqSplitObject(uint.Parse(this.inputSplitNum.text));
        this.transSplitPanel.gameObject.SetActive(false);
    }

    private void btn_splitcancel_onclick()
    {
        this.transSplitPanel.gameObject.SetActive(false);
    }

    private void btn_goldenchange_onclick()
    {
        this.btn_moneychange_onclick(3U);
    }

    private void btn_diamondchange_onclick()
    {
        this.btn_moneychange_onclick(2U);
    }

    private void btn_moneychange_onclick(uint baseid)
    {
        this.mCurSaveTakeBaseid = baseid;
        Image imgDepotMoneyIcon = this.transBankPanel.Find("img_depot/Panel_coin/img_icon").GetComponent<Image>();
        Text component = this.transBankPanel.Find("img_depot/Panel_coin/Text").GetComponent<Text>();
        Image imgBagMoneyIcon = this.transBankPanel.Find("img_bag/Panel_coin/img_icon").GetComponent<Image>();
        Text component2 = this.transBankPanel.Find("img_bag/Panel_coin/Text").GetComponent<Text>();
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)baseid);
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, configTable.GetField_String("icon"), delegate (UITextureAsset asset)
        {
            Texture2D textureObj = asset.textureObj;
            Sprite sprite = null;
            if (imgDepotMoneyIcon != null)
            {
                if (sprite == null)
                {
                    sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                }
                imgDepotMoneyIcon.overrideSprite = sprite;
                imgDepotMoneyIcon.sprite = sprite;
                imgDepotMoneyIcon.color = Color.white;
                imgDepotMoneyIcon.material = null;
                imgDepotMoneyIcon.gameObject.SetActive(true);
            }
            if (imgBagMoneyIcon != null)
            {
                if (sprite == null)
                {
                    sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                }
                imgBagMoneyIcon.overrideSprite = sprite;
                imgBagMoneyIcon.sprite = sprite;
                imgBagMoneyIcon.color = Color.white;
                imgBagMoneyIcon.material = null;
                imgBagMoneyIcon.gameObject.SetActive(true);
            }
        });
        component.text = this.mController.GetItemCount(baseid).ToString();
        component2.text = GlobalRegister.GetCurrencyByID(baseid).ToString();
        this.transBankPanel.gameObject.SetActive(true);
    }

    private void btn_moneychangesure_onclick()
    {
        if (this.inputfieldSaveMoney.text != string.Empty)
        {
            this.mController.TransMoney(PackType.PACK_TYPE_MAIN, PackType.PACK_TYPE_WAREHOUSE1, this.mCurSaveTakeBaseid, uint.Parse(this.inputfieldSaveMoney.text));
        }
        if (this.inputfieldTakeMoney.text != string.Empty)
        {
            this.mController.TransMoney(PackType.PACK_TYPE_WAREHOUSE1, PackType.PACK_TYPE_MAIN, this.mCurSaveTakeBaseid, uint.Parse(this.inputfieldTakeMoney.text));
        }
        this.transBankPanel.gameObject.SetActive(false);
    }

    private void btn_moneychangecancel_onclick()
    {
        this.transBankPanel.gameObject.SetActive(false);
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    public const uint UNVALID_POSNUM = 65535U;

    private List<GameObject> itemObjList;

    private Text txtGolden;

    private Text txtDiamond;

    private Transform transSplitPanel;

    private InputField inputSplitNum;

    private Transform transBankPanel;

    private InputField inputfieldSaveMoney;

    private InputField inputfieldTakeMoney;

    private Button btnExtend;

    private Transform transExtendPanel;

    private int BASE_NUM = 30;

    private uint ROW_NUM = 10U;

    private uint MAX_UNLOCK_NUM = 7U;

    private Dictionary<uint, uint> cacheCostDic = new Dictionary<uint, uint>();

    private DepotController mController;

    private uint split_max_num = 1U;

    private uint mCurSaveTakeBaseid;
}
