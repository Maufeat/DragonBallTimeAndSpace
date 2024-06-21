using System;
using Framework.Managers;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class NumberInputController : ControllerBase
{
    public UI_NumberInput numberInput
    {
        get
        {
            return UIManager.GetUIObject<UI_NumberInput>();
        }
    }

    public override string ControllerName
    {
        get
        {
            return "shop_controller";
        }
    }

    public void OpenNumberInput(InputField inputField)
    {
        this.MaxNum = 999;
        this._number = 0;
        if (inputField == null)
        {
            return;
        }
        if (this._inputField != null && this._inputField == inputField)
        {
            return;
        }
        this._inputField = inputField;
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_NumberInput>("UI_NumberInput", new Action(this.OnLoadUIComplete), UIManager.ParentType.CommonUI, false);
    }

    public void OpenNumberInputMaxToNum(InputField inputField, int num = 0)
    {
        this.MaxNum = int.MaxValue;
        if (num > 0 && num < this.MaxNum)
        {
            this.MaxNum = num;
        }
        this._number = 0;
        if (inputField == null)
        {
            return;
        }
        if (this._inputField != null && this._inputField == inputField)
        {
            return;
        }
        this._inputField = inputField;
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_NumberInput>("UI_NumberInput", new Action(this.OnLoadUIComplete), UIManager.ParentType.CommonUI, false);
    }

    public void CloseInput()
    {
        if (this.Callback != null)
        {
            this.Callback();
        }
        this._inputField = null;
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_NumberInput");
    }

    private void OnLoadUIComplete()
    {
    }

    public void InputNumber(int num)
    {
        num = Mathf.Max(0, num);
        if (this._number == 0)
        {
            if (num == 0)
            {
                this._inputField.text = this._number.ToString();
            }
            this._number = num;
        }
        else
        {
            this._number = Mathf.FloorToInt((float)(this._number * 10 + num));
            this._number = Mathf.Min(this.MaxNum, this._number);
        }
        if (this._inputField != null)
        {
            this._inputField.text = this._number.ToString();
        }
        Scheduler.Instance.AddFrame(2U, false, delegate
        {
            if (this._inputField != null)
            {
                this.InitOrginalNumber(this._inputField.text);
            }
        });
    }

    public void DeleteNumber()
    {
        if (this._number > 0)
        {
            this._number = Mathf.FloorToInt((float)this._number * 0.1f);
        }
        if (this._inputField != null)
        {
            this._inputField.text = this._number.ToString();
        }
    }

    private void InitOrginalNumber(string text)
    {
        int num = text.ToInt();
        num = Mathf.Max(0, num);
        this._number = num;
    }

    public void UpdatePosition()
    {
        if (this._inputField == null)
        {
            return;
        }
        if (!this.numberInput)
        {
            return;
        }
        this.numberInput.uiPanelRoot.transform.SetAsLastSibling();
        Transform panel_NumberInput_ = this.numberInput.Panel_NumberInput_0;
        Transform transform = this._inputField.transform;
        if (panel_NumberInput_ != null && transform != null)
        {
            GlobalRegister.UpdateInputNumPos(transform, panel_NumberInput_);
        }
    }

    public NumberInputController.CloseCallBack Callback;

    public int MaxNum = 999;

    private int _number;

    private InputField _inputField;

    public delegate void CloseCallBack();
}
