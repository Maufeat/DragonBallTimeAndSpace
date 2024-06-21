using System;
using System.Collections.Generic;
using Framework.Managers;
using guild;
using LuaInterface;
using market;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_NPCshopItem : MonoBehaviour
{
    private void InitObj()
    {
        this.itc = ControllerManager.Instance.GetController<ItemTipController>();
        this.txtName = base.transform.Find("txt_name").GetComponent<Text>();
        this.imgItemIcon = base.transform.Find("Image/img_icon").GetComponent<Image>();
        this.qualityIcon = base.transform.Find("Image/quality").GetComponent<RawImage>();
        this.imgItemBg = base.transform.Find("Image").GetComponent<Image>();
        this.transParentTxtNum = base.transform.Find("txt_limited");
        this.txtNum = base.transform.Find("txt_limited/text").GetComponent<Text>();
        this.imgCost = base.transform.Find("img_chip").GetComponent<Image>();
        this.txtCost = base.transform.Find("img_chip/Text").GetComponent<Text>();
        this.objSellout = base.transform.Find("text").gameObject;
        this.btnItem = base.transform.Find("btn_buy").GetComponent<Button>();
        this.imgGuildSkill = base.transform.Find("img_familylv").GetComponent<Image>();
        this.txtGuildSkill = base.transform.Find("img_familylv/Text").GetComponent<Text>();
    }

    public void InitItem(UI_NPCshop uiNpcShop)
    {
        this.InitObj();
        this._isSelect = false;
        this._uiNPCShop = uiNpcShop;
        this.txtNum.text = string.Empty;
        this.objSellout.SetActive(false);
        this.SetSelect(false);
        this._uiNPCShop.AddClickForItem(this.btnItem, this);
    }

    public void UpdateItemByServerData(LuaTable config, Item serverData, uint shopId, MarketType shopType)
    {
        this.Id = serverData.id;
        this.ItemId = config.GetField_Uint("id");
        this.CostId = serverData.costid;
        this.Discount = serverData.discount;
        this.CurNum = serverData.curnum;
        this.MaxNum = serverData.maxnum;
        this.OverlayNum = config.GetField_Uint("maxnum");
        this.ShopId = shopId;
        this.ShopType = shopType;
        this.IconName = config.GetField_String("icon");
        this.IsSellOut = false;
        this.RestNum = serverData.maxnum - serverData.curnum;
        this._itemCofig = LuaConfigManager.GetConfigTable("objects", (ulong)this.Id);
        this.txtName.text = config.GetField_String("name");
        this.IsLimit = (this.MaxNum > 0U);
        this.txtNum.text = this.CurNum + "/" + this.MaxNum;
        this._uiNPCShop.SetTextQualityColor(this.txtName, config.GetField_Int("quality"));
        float num = Mathf.Clamp(serverData.discount / 100f, 0f, 1f);
        float num2 = Mathf.Ceil(serverData.costnum * num);
        this.CostPrice = (uint)num2;
        this.txtCost.text = GlobalRegister.GetCurrenyStr((uint)num2);
        this.GetSpCallBack(this.IconName, 1);
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)this.CostId);
        if (configTable == null)
        {
            FFDebug.LogWarning(this, "cannt find config id = " + this.CostId);
        }
        else
        {
            this.GetSpCallBack(configTable.GetField_String("icon"), 2);
            this.CostName = configTable.GetField_String("name");
        }
        if (serverData.curnum >= serverData.maxnum && serverData.maxnum > 0U)
        {
            this.IsSellOut = true;
            this.objSellout.SetActive(true);
            this.transParentTxtNum.gameObject.SetActive(false);
            this.btnItem.gameObject.SetActive(false);
        }
        else
        {
            this.IsSellOut = false;
            this.objSellout.SetActive(false);
            this.transParentTxtNum.gameObject.SetActive(true);
            this.btnItem.gameObject.SetActive(true);
        }
        if (!this.IsLimit)
        {
            this.transParentTxtNum.gameObject.SetActive(false);
        }
        this.qualityIcon.gameObject.SetActive(false);
        string imgname = "quality" + config.GetField_Uint("quality");
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.RANK, imgname, delegate (UITextureAsset asset)
        {
            if (asset == null)
            {
                return;
            }
            if (this.qualityIcon == null)
            {
                return;
            }
            this.qualityIcon.gameObject.SetActive(true);
            this.qualityIcon.texture = asset.textureObj;
        });
        if (num < 1f)
        {
        }
        this._uiNPCShop.InitHave(this.CostId, configTable.GetField_String("icon"));
        GlobalRegister.AddItemTip(this.imgItemIcon.gameObject, this.imgItemIcon.gameObject, (int)this.ItemId);
        HoverEventListener.Get(this.imgCost.gameObject).onEnter = delegate (PointerEventData pd)
        {
            if (this.itc != null)
            {
                this.itc.OpenPanel(serverData.costid, this.imgCost.gameObject);
            }
        };
        this.imgGuildSkill.gameObject.SetActive(false);
    }

    public void UpdateDataNum(uint sellNum)
    {
        if (sellNum <= 0U)
        {
            return;
        }
        uint num = this.CurNum + sellNum;
        uint maxNum = this.MaxNum;
        if (num >= maxNum && maxNum > 0U)
        {
            this.RestNum = 0U;
            this.CurNum = maxNum;
            this.IsSellOut = true;
            this.objSellout.SetActive(true);
            this.transParentTxtNum.gameObject.SetActive(false);
            this.btnItem.gameObject.SetActive(false);
        }
        else
        {
            this.RestNum = maxNum - num;
            this.CurNum = num;
            this.IsSellOut = false;
            this.objSellout.SetActive(false);
            this.transParentTxtNum.gameObject.SetActive(true);
            this.btnItem.gameObject.SetActive(true);
        }
        this.txtNum.text = this.CurNum + "/" + this.MaxNum;
        if (!this.IsLimit)
        {
            this.transParentTxtNum.gameObject.SetActive(false);
        }
    }

    public void SetSelect(bool status)
    {
        this._isSelect = status;
    }

    public void GetSpCallBack(string name, int sptype)
    {
        if (sptype == 1)
        {
            GlobalRegister.SetImage(0, name, this.imgItemIcon, true);
        }
        else if (sptype == 2)
        {
            GlobalRegister.SetImage(0, name, this.imgCost, true);
        }
    }

    internal void TryUpdateGuildByGuildSkill(List<guildSkill> guildSkillSlt, Item serverData, GuildControllerNew gcn, uint guildSkillLv)
    {
        this.imgGuildSkill.gameObject.SetActive(serverData.skilllv > 0U);
        LuaTable guildSkillConfigByID = gcn.GetGuildSkillConfigByID(serverData.skillid);
        this.txtGuildSkill.color = ((guildSkillLv < serverData.skilllv) ? Color.red : Color.white);
        this.txtGuildSkill.text = string.Empty;
        this.isGuildLvSatisfy = (guildSkillLv >= serverData.skilllv);
        if (guildSkillConfigByID != null)
        {
            this.txtGuildSkill.text = serverData.skilllv + string.Empty;
            string cacheField_String = guildSkillConfigByID.GetCacheField_String("skillicon");
            GlobalRegister.SetImage(0, cacheField_String, this.imgGuildSkill, true);
            HoverEventListener.Get(this.imgGuildSkill.gameObject).onEnter = delegate (PointerEventData pd)
            {
                if (this.itc != null)
                {
                    this.itc.OpenGuildSkillPanel(serverData.skillid, this.imgGuildSkill.gameObject);
                }
            };
        }
    }

    private Text txtName;

    private Image imgItemIcon;

    private RawImage qualityIcon;

    private Image imgItemBg;

    private Transform transParentTxtNum;

    private Text txtNum;

    private Image imgCost;

    private Text txtCost;

    private Image imgGuildSkill;

    private Text txtGuildSkill;

    private GameObject objSellout;

    private Button btnItem;

    private UI_NPCshop _uiNPCShop;

    private LuaTable _itemCofig;

    private bool _isSelect;

    public uint Id;

    public uint ItemId;

    public uint CostId;

    public uint CostPrice;

    public uint Discount;

    public uint MaxNum;

    public uint CurNum;

    public uint OverlayNum;

    public uint ShopId;

    public uint RestNum;

    public string IconName;

    public string CostName;

    public MarketType ShopType;

    public bool IsSellOut;

    public bool IsLimit;

    private ItemTipController itc;

    public bool isGuildLvSatisfy = true;
}
