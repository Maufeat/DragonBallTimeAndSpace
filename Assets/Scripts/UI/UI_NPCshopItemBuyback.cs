using System;
using Framework.Managers;
using LuaInterface;
using market;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_NPCshopItemBuyback : MonoBehaviour
{
    private void InitObj()
    {
        this.txtName = base.transform.Find("txt_name").GetComponent<Text>();
        this.imgItemIcon = base.transform.Find("Image/img_icon").GetComponent<Image>();
        this.imgItemBg = base.transform.Find("Image").GetComponent<Image>();
        this.rawImgQuality = base.transform.Find("Image/quality").GetComponent<RawImage>();
        this.txtNum = base.transform.Find("Image/Text").GetComponent<Text>();
        this.imgCost = base.transform.Find("img_chip").GetComponent<Image>();
        this.txtCost = base.transform.Find("img_chip/Text").GetComponent<Text>();
        this.txtTime = base.transform.Find("txt_time").GetComponent<Text>();
        this.btnItem = base.transform.Find("btn_buyback").GetComponent<Button>();
        this.btnItem.onClick.RemoveAllListeners();
        this.btnItem.onClick.AddListener(new UnityAction(this.OnBuybackButtonClick));
        this.btnItem.gameObject.SetActive(true);
    }

    public void InitItem(UI_NPCshop uiNpcShop)
    {
        this.InitObj();
        this._uiNPCShop = uiNpcShop;
    }

    public void UpdateItemByServerData(LuaTable config, SelledItem info, uint index)
    {
        this.Index = index;
        this.Id = info.item.baseid;
        this.BuybackNum = info.item.num;
        this.TotalCost = info.item.num * config.GetField_Uint("sell_price");
        this.BuybackId = 3U;
        this.BuybackTime = info.selltime;
        this.txtName.text = config.GetField_String("name");
        this.txtNum.text = this.BuybackNum + string.Empty;
        this.txtCost.text = this.TotalCost + string.Empty;
        this.SetLeftTime(this.BuybackTime);
        this._uiNPCShop.SetTextQualityColor(this.txtName, config.GetField_Int("quality"));
        GlobalRegister.SetImage(0, config.GetField_String("icon"), this.imgItemIcon, true);
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)this.BuybackId);
        if (configTable == null)
        {
            FFDebug.LogWarning(this, "cannt find config id = " + this.BuybackId);
        }
        else
        {
            GlobalRegister.SetImage(0, configTable.GetField_String("icon"), this.imgCost, true);
            this.BuybackName = configTable.GetField_String("name");
        }
        this.rawImgQuality.gameObject.SetActive(false);
        string imgname = "quality" + config.GetField_Uint("quality");
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.RANK, imgname, delegate (UITextureAsset asset)
        {
            if (asset == null)
            {
                return;
            }
            if (this.rawImgQuality == null)
            {
                return;
            }
            this.rawImgQuality.gameObject.SetActive(true);
            this.rawImgQuality.texture = asset.textureObj;
        });
        UIEventListener.Get(this.imgItemIcon.gameObject).onEnter = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(info.item, this.imgItemIcon.gameObject);
        };
        UIEventListener.Get(this.imgItemIcon.gameObject).onExit = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().ClosePanel();
        };
    }

    private void OnBuybackButtonClick()
    {
        this._uiNPCShop.OnReqBuySelledItem_CS(this);
    }

    private void SetLeftTime(uint sellTime)
    {
        uint currServerTimeBySecond = SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond();
        uint num = currServerTimeBySecond - sellTime;
        uint field_Uint = LuaConfigManager.GetXmlConfigTable("newUserInit").GetField_Uint("marketbuyback");
        if (num <= 0U)
        {
            num = 1U;
        }
        this.txtTime.text = GlobalRegister.GetTimeInDays(field_Uint - num);
    }

    private Text txtName;

    private Image imgItemIcon;

    private Image imgItemBg;

    private RawImage rawImgQuality;

    private Text txtNum;

    private Image imgCost;

    private Text txtCost;

    private Text txtTime;

    private Button btnItem;

    private UI_NPCshop _uiNPCShop;

    public uint Index;

    public uint Id;

    public uint ItemId;

    public uint TotalCost;

    public uint BuybackId;

    public string BuybackName;

    public uint BuybackNum;

    public uint BuybackTime;
}
