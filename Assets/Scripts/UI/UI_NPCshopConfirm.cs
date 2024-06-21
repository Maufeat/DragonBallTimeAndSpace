using System;
using LuaInterface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_NPCshopConfirm : MonoBehaviour
{
    public void InitObj()
    {
        this.imgIcon = base.transform.Find("Item/img_icon").GetComponent<Image>();
        this.imgTotalCost = base.transform.Find("Panel_price/Image").GetComponent<Image>();
        this.txtMaxNum = base.transform.Find("Item/txt_count").GetComponent<Text>();
        this.txtTotalCost = base.transform.Find("Panel_price/txt_price").GetComponent<Text>();
        this.btnOK = base.transform.Find("btn_ok").GetComponent<Button>();
        this.btnCancel = base.transform.Find("btn_cancel").GetComponent<Button>();
        this.btnAdd = base.transform.Find("Panel_input/btn_add").GetComponent<Button>();
        this.btnMinus = base.transform.Find("Panel_input/btn_minus").GetComponent<Button>();
        this.inputNumber = base.transform.Find("Panel_input/input_number").GetComponent<InputField>();
        this.InitEvent();
    }

    private void InitEvent()
    {
        this.btnOK.onClick.RemoveAllListeners();
        this.btnOK.onClick.AddListener(new UnityAction(this.OnOKButtonClick));
        this.btnCancel.onClick.RemoveAllListeners();
        this.btnCancel.onClick.AddListener(new UnityAction(this.OnCancelButtonClick));
        this.btnAdd.onClick.RemoveAllListeners();
        this.btnAdd.onClick.AddListener(new UnityAction(this.OnAddButtonClick));
        this.btnMinus.onClick.RemoveAllListeners();
        this.btnMinus.onClick.AddListener(new UnityAction(this.OnMinusButtonClick));
        this.inputNumber.onValueChanged.RemoveAllListeners();
        this.inputNumber.onValueChanged.AddListener(new UnityAction<string>(this.OnNumberValueChanged));
    }

    private void OnOKButtonClick()
    {
        this._uiNPCShop.OnReqBuyMarketItem(this);
    }

    public void OnCancelButtonClick()
    {
        base.gameObject.SetActive(false);
    }

    private void OnAddButtonClick()
    {
        if (this.UINPCShopItem.IsSellOut)
        {
            TipsWindow.ShowWindow(TipsType.SHOP_TIMES_OUT, null);
            this.InputNum = 1U;
            this.inputNumber.text = "1";
            this.UpdateCostText();
            return;
        }
        int num = this.inputNumber.text.ToInt();
        if (num >= 0)
        {
            this.OnNumberValueChanged((num + 1).ToString());
        }
    }

    private void OnMinusButtonClick()
    {
        if (this.UINPCShopItem.IsSellOut)
        {
            TipsWindow.ShowWindow(TipsType.SHOP_TIMES_OUT, null);
            this.InputNum = 1U;
            this.inputNumber.text = "1";
            this.UpdateCostText();
            return;
        }
        int num = this.inputNumber.text.ToInt();
        if (num >= 1)
        {
            this.OnNumberValueChanged((num - 1).ToString());
        }
    }

    private void OnNumberValueChanged(string value)
    {
        if (this.UINPCShopItem.IsSellOut)
        {
            TipsWindow.ShowWindow(TipsType.SHOP_TIMES_OUT, null);
            this.InputNum = 1U;
            this.inputNumber.text = "1";
            this.UpdateCostText();
            return;
        }
        uint num = (uint)value.ToInt();
        if (num <= 0U)
        {
            num = 1U;
        }
        if (this.UINPCShopItem.MaxNum == 0U && this.UINPCShopItem.OverlayNum > 0U)
        {
            if (num > this.UINPCShopItem.OverlayNum)
            {
                num = this.UINPCShopItem.OverlayNum;
                string[] args = new string[]
                {
                    num.ToString()
                };
                TipsWindow.ShowWindow(TipsType.SHOP_NUM_TOOMUCH, args);
            }
        }
        else if (this.UINPCShopItem.MaxNum > 0U && this.UINPCShopItem.OverlayNum == 0U)
        {
            if (num > this.UINPCShopItem.MaxNum)
            {
                num = this.UINPCShopItem.MaxNum;
                TipsWindow.ShowWindow(TipsType.SHOP_NUM_BEYONDLIMIT, null);
            }
        }
        else if (this.UINPCShopItem.MaxNum > 0U && this.UINPCShopItem.OverlayNum > 0U)
        {
            uint num2 = (uint)Mathf.Min(this.UINPCShopItem.RestNum, this.UINPCShopItem.OverlayNum);
            if (num > num2 && this.UINPCShopItem.OverlayNum > this.UINPCShopItem.RestNum)
            {
                num = this.UINPCShopItem.RestNum;
                TipsWindow.ShowWindow(TipsType.SHOP_NUM_BEYONDLIMIT, null);
            }
            else if (num > num2 && this.UINPCShopItem.RestNum >= this.UINPCShopItem.OverlayNum)
            {
                num = num2;
                string[] args2 = new string[]
                {
                    num.ToString()
                };
                TipsWindow.ShowWindow(TipsType.SHOP_NUM_TOOMUCH, args2);
            }
        }
        this.InputNum = num;
        this.inputNumber.text = num.ToString();
        this.UpdateCostText();
    }

    private void UpdateCostText()
    {
        this.TotalCost = this.InputNum * this.UINPCShopItem.CostPrice;
        this.txtTotalCost.text = GlobalRegister.GetCurrenyStr(this.TotalCost);
        this._uiNPCShop.UpdataHaveCost();
        if ((long)this._uiNPCShop.GetHaveCost() < (long)((ulong)(this.InputNum * this.UINPCShopItem.CostPrice)) || !this.UINPCShopItem.isGuildLvSatisfy)
        {
            this.btnOK.interactable = false;
            this.txtTotalCost.color = GlobalRegister.GetColorByName("itemlimit");
        }
        else
        {
            this.btnOK.interactable = true;
            this.txtTotalCost.color = GlobalRegister.GetColorByName("normalwhite");
        }
    }

    public void InitItem(UI_NPCshopItem uiNpcShopItem, UI_NPCshop uiNpcShop)
    {
        this._uiNPCShop = uiNpcShop;
        this.UINPCShopItem = uiNpcShopItem;
        this.InputNum = 1U;
        this.inputNumber.text = "1";
        base.gameObject.SetActive(true);
        if (uiNpcShopItem.IsLimit)
        {
            this.txtMaxNum.text = uiNpcShopItem.RestNum.ToString();
        }
        else
        {
            this.txtMaxNum.text = uiNpcShopItem.OverlayNum.ToString();
        }
        GlobalRegister.SetImage(0, uiNpcShopItem.IconName, this.imgIcon, true);
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)uiNpcShopItem.CostId);
        if (configTable != null)
        {
            GlobalRegister.SetImage(0, configTable.GetField_String("icon"), this.imgTotalCost, true);
        }
        this.UpdateCostText();
        GlobalRegister.AddItemTip(this.imgIcon.gameObject, this.imgIcon.gameObject, (int)uiNpcShopItem.ItemId);
    }

    private Image imgIcon;

    private Image imgTotalCost;

    private Text txtMaxNum;

    private Text txtTotalCost;

    private Button btnOK;

    private Button btnCancel;

    private Button btnAdd;

    private Button btnMinus;

    private InputField inputNumber;

    private UI_NPCshop _uiNPCShop;

    public UI_NPCshopItem UINPCShopItem;

    public uint InputNum;

    public uint TotalCost;
}
